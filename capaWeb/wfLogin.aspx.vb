Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization

Partial Class wfLogin
    Inherits System.Web.UI.Page

    ' La Site Key se usa desde el markup para cargar el script de reCAPTCHA.
    Protected ReadOnly Property RecaptchaSiteKey As String
        Get
            Return ValorConfig("Recaptcha.SiteKey")
        End Get
    End Property

    Private Function ValorConfig(clave As String, Optional porDefecto As String = "") As String
        Dim valor As String = ConfigurationManager.AppSettings(clave)
        If valor Is Nothing Then Return porDefecto
        Return valor
    End Function

    Dim objUsuario As New Usuario()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("salir") = "1" Then
                lblMensaje.CssClass = "mensaje-info"
                lblMensaje.Text = "Sesión finalizada correctamente."
            ElseIf Request.QueryString("cambio") = "1" Then
                lblMensaje.CssClass = "mensaje-ok"
                lblMensaje.Text = "Contraseña modificada correctamente. Ya puede iniciar sesión."
            ElseIf Session("idUsuario") IsNot Nothing Then
                Response.Redirect("~/wfInicio.aspx")
            End If
        End If
    End Sub

    Private Function ValidarRecaptcha(ByRef mensajeError As String) As Boolean
        mensajeError = ""

        If Not String.Equals(ValorConfig("Recaptcha.Enabled", "true"), "true", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        Dim token As String = ""
        If hfRecaptchaToken IsNot Nothing Then
            token = If(hfRecaptchaToken.Value, "").Trim()
            hfRecaptchaToken.Value = ""
        End If

        If token.Length = 0 Then
            mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
            Return False
        End If

        Dim secreto As String = ValorConfig("Recaptcha.SecretKey")
        If secreto.Length = 0 Then
            mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
            Return False
        End If

        Try
            Dim url As String = ValorConfig("Recaptcha.VerifyUrl", "https://www.google.com/recaptcha/api/siteverify")
            Dim cuerpo As String = "secret=" & Uri.EscapeDataString(secreto) &
                                   "&response=" & Uri.EscapeDataString(token) &
                                   "&remoteip=" & Uri.EscapeDataString(If(Request.UserHostAddress, ""))

            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol Or SecurityProtocolType.Tls12

            Dim solicitud As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
            solicitud.Method = "POST"
            solicitud.ContentType = "application/x-www-form-urlencoded"
            solicitud.Timeout = 5000
            solicitud.ReadWriteTimeout = 5000

            Using flujo As Stream = solicitud.GetRequestStream()
                Dim datosBytes As Byte() = Encoding.UTF8.GetBytes(cuerpo)
                flujo.Write(datosBytes, 0, datosBytes.Length)
            End Using

            Dim contenido As String = ""
            Using respuesta As HttpWebResponse = DirectCast(solicitud.GetResponse(), HttpWebResponse)
                Using lector As New StreamReader(respuesta.GetResponseStream(), Encoding.UTF8)
                    contenido = lector.ReadToEnd()
                End Using
            End Using

            Dim datos As Dictionary(Of String, Object) =
                New JavaScriptSerializer().Deserialize(Of Dictionary(Of String, Object))(contenido)

            Dim exito As Boolean = False
            If datos.ContainsKey("success") AndAlso datos("success") IsNot Nothing Then
                Boolean.TryParse(datos("success").ToString(), exito)
            End If
            If Not exito Then
                mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
                Return False
            End If

            Dim accionEsperada As String = ValorConfig("Recaptcha.Action", "login")
            Dim accion As String = ""
            If datos.ContainsKey("action") AndAlso datos("action") IsNot Nothing Then accion = datos("action").ToString()
            If Not String.Equals(accion, accionEsperada, StringComparison.OrdinalIgnoreCase) Then
                mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
                Return False
            End If

            Dim puntajeMinimo As Decimal = 0.5D
            Decimal.TryParse(ValorConfig("Recaptcha.MinScore", "0.5"), NumberStyles.Any,
                             CultureInfo.InvariantCulture, puntajeMinimo)
            Dim puntaje As Decimal = 0D
            If datos.ContainsKey("score") AndAlso datos("score") IsNot Nothing Then
                Decimal.TryParse(datos("score").ToString(), NumberStyles.Any,
                                 CultureInfo.InvariantCulture, puntaje)
            End If
            If puntaje < puntajeMinimo Then
                mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
                Return False
            End If

            Dim hostnames As String() = ValorConfig("Recaptcha.Hostnames").Split(","c)
            Dim hostname As String = ""
            If datos.ContainsKey("hostname") AndAlso datos("hostname") IsNot Nothing Then hostname = datos("hostname").ToString()
            Dim hostnameValido As Boolean = False
            For Each permitido As String In hostnames
                If String.Equals(permitido.Trim(), hostname, StringComparison.OrdinalIgnoreCase) Then
                    hostnameValido = True
                    Exit For
                End If
            Next
            If Not hostnameValido Then
                mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
                Return False
            End If

            Return True
        Catch ex As Exception
            ' No se expone el mensaje de error ni el secreto al usuario.
            System.Diagnostics.Trace.TraceWarning("reCAPTCHA: error al verificar (" & ex.GetType().Name & ")")
            mensajeError = "No se pudo validar la solicitud. Intente nuevamente."
            Return False
        End Try
    End Function

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs)
        Try
            lblMensaje.CssClass = "mensaje-error"

            Dim mensajeCaptcha As String = ""
            If Not ValidarRecaptcha(mensajeCaptcha) Then
                lblMensaje.Text = mensajeCaptcha
                Return
            End If

            Dim usuario As String = txtUsuario.Text.Trim()

            If usuario.Length = 0 Then
                lblMensaje.Text = "Ingrese nombre de usuario."
                txtUsuario.Focus()
                Return
            End If

            If txtContrasena.Text.Trim().Length = 0 Then
                lblMensaje.Text = "Ingrese la contraseña."
                txtContrasena.Focus()
                Return
            End If

            Dim intentos As Integer = 3
            If Session("intentosLogin") IsNot Nothing Then
                intentos = Convert.ToInt32(Session("intentosLogin"))
            End If
            If intentos <= 0 Then
                lblMensaje.Text = "Demasiados intentos fallidos. Contacte al administrador del sistema."
                Return
            End If

            If Not objUsuario.validarNombreUsuario(usuario) Then
                lblMensaje.Text = "Usuario no encontrado."
                txtUsuario.Focus()
                Return
            End If

            If Not objUsuario.usuarioActivo(usuario) Then
                lblMensaje.Text = "Usuario dado de baja. Comuníquese con el administrador."
                txtUsuario.Focus()
                Return
            End If

            If objUsuario.iniciarSesion(usuario, txtContrasena.Text) Then
                Dim idUsuario As Integer = objUsuario.obtenerIDUsuario(usuario)
                Session("idUsuario") = idUsuario
                Session("usuario") = usuario
                Session("intentosLogin") = 3
                Session("usuarioCambio") = Nothing
                Session("permisos") = Nothing

                Try
                    Dim dtUsuario As DataTable = objUsuario.obtenerUsuario(idUsuario)
                    If dtUsuario.Rows.Count > 0 Then
                        Session("rol") = dtUsuario.Rows(0)("rol").ToString()
                    Else
                        Session("rol") = ""
                    End If
                Catch ex As Exception
                    Session("rol") = ""
                End Try

                Try
                    Dim dtPreferencia As DataTable = objUsuario.obtenerPreferencia(idUsuario)
                    If dtPreferencia.Rows.Count > 0 Then
                        Session("tema") = dtPreferencia.Rows(0)("tema").ToString()
                    Else
                        Session("tema") = "OSCURO"
                    End If
                Catch ex As Exception
                    Session("tema") = "OSCURO"
                End Try

                Response.Redirect("~/wfInicio.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
                Return
            Else
                intentos -= 1
                Session("intentosLogin") = intentos
                lblMensaje.Text = "Contraseña incorrecta. Intentos restantes: " & intentos.ToString()
                txtContrasena.Focus()
            End If
        Catch ex As Exception
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = ex.Message
        End Try
    End Sub

    Protected Sub chbCambiar_CheckedChanged(sender As Object, e As EventArgs)
        Try
            lblMensaje.CssClass = "mensaje-error"

            If chbCambiar.Checked Then
                Dim usuario As String = txtUsuario.Text.Trim()
                If usuario.Length = 0 Then
                    lblMensaje.Text = "Ingrese nombre de usuario."
                    chbCambiar.Checked = False
                    pnlCambio.Visible = False
                    txtUsuario.Focus()
                    Return
                End If

                If Not objUsuario.validarNombreUsuario(usuario) Then
                    lblMensaje.Text = "Usuario no encontrado."
                    chbCambiar.Checked = False
                    pnlCambio.Visible = False
                    txtUsuario.Focus()
                    Return
                End If

                Dim pregunta As String = objUsuario.obtenerPregunta(usuario)
                If String.IsNullOrEmpty(pregunta) Then
                    lblMensaje.Text = "El usuario no tiene una pregunta de seguridad configurada."
                    chbCambiar.Checked = False
                    pnlCambio.Visible = False
                    Return
                End If

                Session("usuarioCambio") = usuario
                txtPregunta.Text = pregunta
                txtUsuario.Enabled = False
                pnlCambio.Visible = True
                txtRespuesta.Text = ""
                txtRespuesta.Focus()
            Else
                pnlCambio.Visible = False
                txtUsuario.Enabled = True
                txtPregunta.Text = ""
                txtRespuesta.Text = ""
                Session("usuarioCambio") = Nothing
                txtUsuario.Focus()
            End If
        Catch ex As Exception
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnCambiar_Click(sender As Object, e As EventArgs)
        Try
            lblMensaje.CssClass = "mensaje-error"
            Dim usuario As String = txtUsuario.Text.Trim()
            If Session("usuarioCambio") IsNot Nothing Then
                usuario = Session("usuarioCambio").ToString()
            End If

            If usuario.Length = 0 Then
                lblMensaje.Text = "Ingrese nombre de usuario."
                Return
            End If

            If txtRespuesta.Text.Trim().Length = 0 Then
                lblMensaje.Text = "Ingrese la respuesta de seguridad."
                txtRespuesta.Focus()
                Return
            End If

            If objUsuario.validarRespuesta(usuario, txtRespuesta.Text.Trim()) Then
                Session("usuarioCambio") = usuario
                Response.Redirect("~/wfCambiarContrasena.aspx", False)
                Context.ApplicationInstance.CompleteRequest()
                Return
            Else
                lblMensaje.Text = "Respuesta incorrecta. Intente nuevamente."
                txtRespuesta.Text = ""
                txtRespuesta.Focus()
            End If
        Catch ex As Exception
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = ex.Message
        End Try
    End Sub

End Class

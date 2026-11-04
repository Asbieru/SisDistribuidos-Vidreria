Imports System.Data
Imports capaNegocio

Partial Class wfLogin
    Inherits System.Web.UI.Page

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

    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs)
        Try
            lblMensaje.CssClass = "mensaje-error"
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

    Protected Sub btnSalir_Click(sender As Object, e As EventArgs)
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/wfLogin.aspx?salir=1")
    End Sub
End Class

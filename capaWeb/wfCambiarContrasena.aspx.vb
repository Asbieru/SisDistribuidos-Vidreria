Imports capaNegocio

Partial Class wfCambiarContrasena
    Inherits System.Web.UI.Page

    Dim objUsuario As New Usuario()

    ' Usuario cuya contraseña se va a modificar. Viene de la sesión cuando el
    ' usuario ya inició sesión, o de la validación de la pregunta de seguridad
    ' realizada desde wfLogin.
    Private ReadOnly Property UsuarioObjetivo As String
        Get
            ' Si ya hay una sesión iniciada se cambia la clave del usuario actual.
            ' Si no, se usa el usuario validado con la pregunta de seguridad.
            If Session("idUsuario") IsNot Nothing AndAlso Session("usuario") IsNot Nothing Then
                Return Session("usuario").ToString()
            End If
            If Session("usuarioCambio") IsNot Nothing Then
                Return Session("usuarioCambio").ToString()
            End If
            Return String.Empty
        End Get
    End Property

    Private ReadOnly Property SesionIniciada As Boolean
        Get
            Return Session("idUsuario") IsNot Nothing
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim usuario As String = UsuarioObjetivo
            If usuario.Length = 0 Then
                Response.Redirect("~/wfLogin.aspx")
                Return
            End If

            txtUsuario.Text = usuario
            txtContrasena.Focus()
        End If
    End Sub

    Protected Sub chkMostrar_CheckedChanged(sender As Object, e As EventArgs)
        Dim modo As TextBoxMode = If(chkMostrar.Checked, TextBoxMode.SingleLine, TextBoxMode.Password)
        txtContrasena.TextMode = modo
        txtConfirmar.TextMode = modo
        txtContrasena.Focus()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Try
            lblMensaje.CssClass = "mensaje-error"

            Dim usuario As String = UsuarioObjetivo
            If usuario.Length = 0 Then
                Response.Redirect("~/wfLogin.aspx")
                Return
            End If

            If txtContrasena.Text.Length = 0 OrElse txtConfirmar.Text.Length = 0 Then
                lblMensaje.Text = "Ingrese y confirme la nueva contraseña."
                txtContrasena.Focus()
                Return
            End If

            If Not txtContrasena.Text.Equals(txtConfirmar.Text) Then
                lblMensaje.Text = "Las contraseñas no coinciden."
                txtConfirmar.Text = ""
                txtConfirmar.Focus()
                Return
            End If

            objUsuario.cambiarContraseña(usuario, txtContrasena.Text)

            Session("usuarioCambio") = Nothing
            Session("intentosLogin") = 3

            If SesionIniciada Then
                Response.Redirect("~/wfInicio.aspx?cambio=1", False)
            Else
                Response.Redirect("~/wfLogin.aspx?cambio=1", False)
            End If
            Context.ApplicationInstance.CompleteRequest()
            Return
        Catch ex As Exception
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
        Session("usuarioCambio") = Nothing

        If SesionIniciada Then
            Response.Redirect("~/wfInicio.aspx")
        Else
            Response.Redirect("~/wfLogin.aspx")
        End If
    End Sub
End Class

Partial Class wfInicio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUsuarioInicio.Text = Server.HtmlEncode(If(Session("usuario") Is Nothing, "", Session("usuario").ToString()))
            lblRolInicio.Text = Server.HtmlEncode(If(Session("rol") Is Nothing, "", Session("rol").ToString()))
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm")

            If Request.QueryString("cambio") = "1" Then
                pnlAviso.Visible = True
                lblAviso.Text = "Contraseña modificada correctamente."
            End If
        End If
    End Sub
End Class

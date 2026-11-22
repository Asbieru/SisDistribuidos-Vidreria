Partial Class wfInicio
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUsuarioInicio.Text = Server.HtmlEncode(If(Session("usuario") Is Nothing, "", Session("usuario").ToString()))
            lblRolInicio.Text = Server.HtmlEncode(If(Session("rol") Is Nothing, "", Session("rol").ToString()))
            Try
                Dim zonaPeru As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time")
                lblFecha.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaPeru).ToString("dd/MM/yyyy HH:mm")
            Catch
                lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            End Try

            If Request.QueryString("cambio") = "1" Then
                pnlAviso.Visible = True
                lblAviso.Text = "Contraseña modificada correctamente."
            End If
        End If
    End Sub
End Class

Partial Class wfModulo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim modulo As String = Request.QueryString("modulo")
        If String.IsNullOrEmpty(modulo) Then
            modulo = "Módulo"
        End If

        ' Label no codifica el texto, por eso se codifica antes de mostrarlo.
        Dim moduloSeguro As String = Server.HtmlEncode(modulo.Trim())

        Page.Title = moduloSeguro & " - SISCORP"
        lblModulo.Text = moduloSeguro
        lblDetalle.Text = moduloSeguro
        lblUsuarioModulo.Text = Server.HtmlEncode(If(Session("usuario") Is Nothing, "", Session("usuario").ToString()))
    End Sub
End Class

Partial Class wfDefault
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.Redirect("~/wfLogin.aspx", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class

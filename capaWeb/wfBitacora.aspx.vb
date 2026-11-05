Imports System.Data
Imports capaNegocio

' REPORTE: registros de la tabla bitacora_login (solo consulta, solo ADMINISTRADOR).
Partial Class wfBitacora
    Inherits System.Web.UI.Page

    Private ReadOnly objBit As New Bitacora()
    Private Const TODAS As String = "(Todas)"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Dim rol As String = If(Session("rol") Is Nothing, "", Session("rol").ToString())
        If Not rol.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase) Then
            pnlBitacora.Visible = False
            lblMensaje.Text = "Solo el administrador puede ver la bitácora."
            Return
        End If

        If Not IsPostBack Then
            ddlAccion.Items.Add(TODAS)
            Try
                For Each accion As String In objBit.listarAcciones()
                    ddlAccion.Items.Add(accion)
                Next
            Catch ex As Exception
                lblMensaje.Text = ex.Message
            End Try
            CargarLista()
        End If
    End Sub

    ' Carga todos los registros o solo los de la acción elegida
    Private Sub CargarLista()
        lblMensaje.Text = ""
        Try
            Dim dt As DataTable
            If ddlAccion.SelectedIndex <= 0 Then
                dt = objBit.listarBitacora()
            Else
                dt = objBit.listarPorAccion(ddlAccion.SelectedValue)
            End If
            grvBitacora.DataSource = dt
            grvBitacora.DataBind()
            lblResumen.Text = "Registros: " & dt.Rows.Count.ToString()
        Catch ex As Exception
            grvBitacora.DataSource = Nothing
            grvBitacora.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs)
        CargarLista()
    End Sub

    Protected Sub btnRefrescar_Click(sender As Object, e As EventArgs)
        CargarLista()
    End Sub
End Class

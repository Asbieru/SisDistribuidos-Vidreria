Imports System.Data

Partial Class wfReportePendientes
    Inherits System.Web.UI.Page

    Private ReadOnly objReporte As New Reporte()

    ' Cliente cuyo detalle se está mostrando; se conserva al paginar u ordenar.
    Private Property ClienteDetalleId As Integer
        Get
            If ViewState("ClienteDetalleId") Is Nothing Then Return 0
            Return CInt(ViewState("ClienteDetalleId"))
        End Get
        Set(value As Integer)
            ViewState("ClienteDetalleId") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse
           Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not ReporteUtil.PuedeVerReportes(idUsuario) Then
            pnlReporte.Visible = False
            pnlDetalle.Visible = False
            lblMensaje.Text = "No tiene permiso para consultar reportes."
            Return
        End If

        If Not IsPostBack Then CargarClientes()
    End Sub

    Private Sub CargarClientes(Optional restablecerPagina As Boolean = True)
        lblMensaje.Text = ""
        pnlDetalle.Visible = False
        If restablecerPagina Then grvClientes.PageIndex = 0

        Try
            Dim datos As DataTable = objReporte.ClientesConPendientes()
            Dim saldo As Decimal = 0D
            For Each fila As DataRow In datos.Rows
                If Not IsDBNull(fila("saldo_faltante")) Then saldo += CDec(fila("saldo_faltante"))
            Next

            grvClientes.DataSource = OrdenarClientes(datos)
            grvClientes.DataBind()
            lblResumen.Text = "Clientes: " & datos.Rows.Count.ToString() &
                " | Saldo faltante: S/ " & saldo.ToString("N2")
        Catch ex As Exception
            grvClientes.DataSource = Nothing
            grvClientes.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo generar el reporte de pendientes."
        End Try
    End Sub

    Private Function ExpresionOrdenClientes() As String
        Select Case ddlOrden.SelectedValue
            Case "FECHA_ASC"
                Return "fecha_mas_antigua ASC, cliente ASC"
            Case "CLIENTE_ASC"
                Return "cliente ASC, fecha_mas_antigua DESC"
            Case "CLIENTE_DESC"
                Return "cliente DESC, fecha_mas_antigua DESC"
            Case Else
                Return "fecha_mas_antigua DESC, cliente ASC"
        End Select
    End Function

    Private Function OrdenarClientes(datos As DataTable) As DataView
        Dim vista As DataView = datos.DefaultView
        vista.Sort = ExpresionOrdenClientes()
        Return vista
    End Function

    Private Function ExpresionOrdenDetalle() As String
        If ddlOrdenDetalle.SelectedValue = "FECHA_ASC" Then Return "fecha ASC, id_pedido ASC"
        Return "fecha DESC, id_pedido DESC"
    End Function

    Private Function OrdenarDetalle(datos As DataTable) As DataView
        Dim vista As DataView = datos.DefaultView
        vista.Sort = ExpresionOrdenDetalle()
        Return vista
    End Function

    Private Sub CargarDetalle(idCliente As Integer, Optional restablecerPagina As Boolean = True)
        lblMensaje.Text = ""
        If restablecerPagina Then grvPedidos.PageIndex = 0

        Try
            Dim detalle As DataTable = objReporte.DetallePendientesPorCliente(idCliente)
            grvPedidos.DataSource = OrdenarDetalle(detalle)
            grvPedidos.DataBind()
            lblDetalle.Text = "Pedidos pendientes de " & ObtenerNombreCliente(idCliente)
            pnlDetalle.Visible = True
        Catch ex As Exception
            grvPedidos.DataSource = Nothing
            grvPedidos.DataBind()
            pnlDetalle.Visible = False
            lblMensaje.Text = "No se pudo consultar el detalle del cliente."
        End Try
    End Sub

    Protected Sub ddlOrden_SelectedIndexChanged(sender As Object, e As EventArgs)
        CargarClientes()
    End Sub

    Protected Sub grvClientes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvClientes.PageIndex = e.NewPageIndex
        CargarClientes(False)
    End Sub

    Protected Sub grvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName <> "VerDetalle" Then Return

        Dim idCliente As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), idCliente) OrElse idCliente <= 0 Then Return

        ClienteDetalleId = idCliente
        CargarDetalle(idCliente)
    End Sub

    Protected Sub ddlOrdenDetalle_SelectedIndexChanged(sender As Object, e As EventArgs)
        If ClienteDetalleId > 0 AndAlso pnlDetalle.Visible Then CargarDetalle(ClienteDetalleId)
    End Sub

    Protected Sub grvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvPedidos.PageIndex = e.NewPageIndex
        If ClienteDetalleId > 0 Then CargarDetalle(ClienteDetalleId, False)
    End Sub

    Private Function ObtenerNombreCliente(idCliente As Integer) As String
        Try
            For Each fila As DataRow In objReporte.ClientesConPendientes().Rows
                If CInt(fila("id_cliente")) = idCliente Then
                    Return fila("cliente").ToString()
                End If
            Next
        Catch
        End Try
        Return "Cliente " & idCliente.ToString()
    End Function

    Protected Sub btnCerrarDetalle_Click(sender As Object, e As EventArgs)
        pnlDetalle.Visible = False
        ClienteDetalleId = 0
    End Sub

End Class

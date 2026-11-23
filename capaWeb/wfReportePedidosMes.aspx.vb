Imports System.Data
Imports System.Web.UI.WebControls

Partial Class wfReportePedidosMes
    Inherits System.Web.UI.Page

    Private ReadOnly objReporte As New Reporte()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse
           Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not ReporteUtil.PuedeVerReportes(idUsuario) Then
            pnlReporte.Visible = False
            lblMensaje.Text = "No tiene permiso para consultar reportes."
            Return
        End If

        If Not IsPostBack Then
            CargarFiltros()
            GenerarReporte()
        End If
    End Sub

    Private Sub CargarFiltros()
        Dim nombresMeses As String() =
            {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
             "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}

        ddlMes.Items.Clear()
        For mes As Integer = 1 To 12
            ddlMes.Items.Add(New ListItem(nombresMeses(mes - 1), mes.ToString()))
        Next

        Dim hoy As Date = ReporteUtil.HoyPeru()
        ddlAnio.Items.Clear()
        For anio As Integer = hoy.Year - 5 To hoy.Year + 1
            ddlAnio.Items.Add(New ListItem(anio.ToString(), anio.ToString()))
        Next
        ddlMes.SelectedValue = hoy.Month.ToString()
        ddlAnio.SelectedValue = hoy.Year.ToString()
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub ddlOrden_SelectedIndexChanged(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub grvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvPedidos.PageIndex = e.NewPageIndex
        GenerarReporte(False)
    End Sub

    Private Function ExpresionOrden() As String
        Select Case ddlOrden.SelectedValue
            Case "FECHA_ASC"
                Return "fecha ASC, id_pedido ASC"
            Case "CLIENTE_ASC"
                Return "cliente ASC, fecha DESC"
            Case "CLIENTE_DESC"
                Return "cliente DESC, fecha DESC"
            Case Else
                Return "fecha DESC, id_pedido DESC"
        End Select
    End Function

    Private Function Ordenar(datos As DataTable) As DataView
        Dim vista As DataView = datos.DefaultView
        vista.Sort = ExpresionOrden()
        Return vista
    End Function

    Private Sub GenerarReporte(Optional restablecerPagina As Boolean = True)
        lblMensaje.Text = ""
        If restablecerPagina Then grvPedidos.PageIndex = 0

        Dim anio As Integer
        Dim mes As Integer
        If Not Integer.TryParse(ddlAnio.SelectedValue, anio) OrElse
           Not Integer.TryParse(ddlMes.SelectedValue, mes) Then
            lblMensaje.Text = "Seleccione un mes y un año válidos."
            Return
        End If

        Try
            Dim datos As DataTable = objReporte.PedidosDelMes(anio, mes, ddlEstado.SelectedValue)
            Dim importe As Decimal = 0D
            Dim cobrado As Decimal = 0D
            Dim deuda As Decimal = 0D
            Dim excedentes As Decimal = 0D

            For Each fila As DataRow In datos.Rows
                If Not IsDBNull(fila("total")) Then importe += CDec(fila("total"))
                If Not IsDBNull(fila("pagado")) Then cobrado += CDec(fila("pagado"))
                If IsDBNull(fila("saldo")) Then Continue For
                Dim saldo As Decimal = CDec(fila("saldo"))
                If saldo > 0D Then deuda += saldo
                If saldo < 0D Then excedentes -= saldo
            Next

            grvPedidos.DataSource = Ordenar(datos)
            grvPedidos.DataBind()
            lblResumen.Text = "Pedidos: " & datos.Rows.Count.ToString() &
                " | Importe: S/ " & importe.ToString("N2") &
                " | Cobrado: S/ " & cobrado.ToString("N2") &
                " | Deuda pendiente: S/ " & deuda.ToString("N2") &
                " | Excedentes: S/ " & excedentes.ToString("N2")
        Catch ex As Exception
            grvPedidos.DataSource = Nothing
            grvPedidos.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo generar el reporte de pedidos."
        End Try
    End Sub

End Class

Imports System.Data
Imports System.Globalization

Partial Class wfReporteIngresosMes
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
            lblMensaje.CssClass = "mensaje-error"
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

    Private Sub GenerarReporte()
        lblMensaje.Text = ""
        lblMensaje.CssClass = "mensaje-error"

        Dim anio As Integer
        Dim mes As Integer
        If Not Integer.TryParse(ddlAnio.SelectedValue, anio) OrElse
           Not Integer.TryParse(ddlMes.SelectedValue, mes) Then
            lblMensaje.Text = "Seleccione un mes y un año válidos."
            Return
        End If

        Try
            Dim datos As DataTable = objReporte.IngresosDelMes(anio, mes)

            Dim cantidadCobros As Integer = 0
            Dim pedidosCobrados As Integer = 0
            Dim entradas As Decimal = 0D
            Dim cantidadCompras As Integer = 0
            Dim compras As Decimal = 0D
            Dim cantidadCaja As Integer = 0
            Dim salidasCaja As Decimal = 0D
            Dim saldo As Decimal = 0D

            If datos IsNot Nothing AndAlso datos.Rows.Count > 0 Then
                Dim fila As DataRow = datos.Rows(0)
                cantidadCobros = EnteroDe(fila, "cantidad_cobros")
                pedidosCobrados = EnteroDe(fila, "pedidos_cobrados")
                entradas = DecimalDe(fila, "total_entradas")
                cantidadCompras = EnteroDe(fila, "cantidad_compras")
                compras = DecimalDe(fila, "total_compras")
                cantidadCaja = EnteroDe(fila, "cantidad_salidas_caja")
                salidasCaja = DecimalDe(fila, "total_salidas_caja")
                saldo = DecimalDe(fila, "saldo_neto")
            End If

            Dim salidasTotales As Decimal = compras + salidasCaja
            Dim flujo As Decimal = 0D
            If entradas <> 0D Then flujo = saldo * 100D / entradas

            lblPeriodo.Text = Server.HtmlEncode(ddlMes.SelectedItem.Text & " " & anio.ToString())

            lblEntradas.Text = "S/ " & entradas.ToString("N2")
            lblSalidas.Text = "S/ " & salidasTotales.ToString("N2")
            lblSaldo.Text = "S/ " & saldo.ToString("N2")
            lblFlujo.Text = If(entradas = 0D, "—", flujo.ToString("N2") & "%")

            lblDetalleEntradas.Text = cantidadCobros.ToString() &
                If(cantidadCobros = 1, " cobro · ", " cobros · ") &
                pedidosCobrados.ToString() &
                If(pedidosCobrados = 1, " pedido", " pedidos")

            lblDetalleSalidas.Text = cantidadCompras.ToString() &
                If(cantidadCompras = 1, " compra · ", " compras · ") &
                cantidadCaja.ToString() &
                If(cantidadCaja = 1, " movimiento de caja", " movimientos de caja")

            lblGraficoEntradas.Text = "S/ " & entradas.ToString("N2")
            lblGraficoSalidas.Text = "S/ " & salidasTotales.ToString("N2")

            lblTablaCobros.Text = cantidadCobros.ToString()
            lblTablaPedidos.Text = pedidosCobrados.ToString()
            lblTablaEntradas.Text = "S/ " & entradas.ToString("N2")
            lblTablaCantidadCompras.Text = cantidadCompras.ToString()
            lblTablaCompras.Text = "S/ " & compras.ToString("N2")
            lblTablaCaja.Text = "S/ " & salidasCaja.ToString("N2")
            lblTablaSalidas.Text = "S/ " & salidasTotales.ToString("N2")
            lblTablaSaldo.Text = "S/ " & saldo.ToString("N2")
            lblTablaFlujo.Text = If(entradas = 0D, "—", flujo.ToString("N2") & "%")

            AplicarColor(lblSaldo, lblTablaSaldo, saldo)
            AplicarColor(lblFlujo, lblTablaFlujo, If(entradas = 0D, 0D, flujo))

            Dim maximo As Decimal = Math.Max(entradas, salidasTotales)
            Dim porcentajeEntradas As Decimal = 0D
            Dim porcentajeSalidas As Decimal = 0D
            If maximo > 0D Then
                porcentajeEntradas = entradas * 100D / maximo
                porcentajeSalidas = salidasTotales * 100D / maximo
            End If

            barraVentas.Style("width") =
                porcentajeEntradas.ToString("0.##", CultureInfo.InvariantCulture) & "%"
            barraCompras.Style("width") =
                porcentajeSalidas.ToString("0.##", CultureInfo.InvariantCulture) & "%"

            grvMetodos.DataSource = objReporte.CobrosPorMetodo(anio, mes)
            grvMetodos.DataBind()

            If entradas = 0D AndAlso salidasTotales = 0D Then
                lblMensaje.CssClass = "mensaje-info"
                lblMensaje.Text = "No hay entradas ni salidas registradas en el mes seleccionado."
            End If
        Catch ex As Exception
            LimpiarResumen()
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = "No se pudo generar el reporte de ingresos y egresos."
        End Try
    End Sub

    Private Sub AplicarColor(valor As Label, valorTabla As Label, monto As Decimal)
        Dim clase As String = ""
        If monto > 0D Then
            clase = "monto-utilidad-positiva"
        ElseIf monto < 0D Then
            clase = "monto-utilidad-negativa"
        End If
        ' Conserva el tamaño de la tarjeta y solo agrega el color del saldo.
        valor.CssClass = ("ingresos-tarjeta-valor " & clase).Trim()
        valorTabla.CssClass = clase
    End Sub

    Private Function EnteroDe(fila As DataRow, columna As String) As Integer
        If fila Is Nothing OrElse IsDBNull(fila(columna)) Then Return 0
        Return Convert.ToInt32(fila(columna))
    End Function

    Private Function DecimalDe(fila As DataRow, columna As String) As Decimal
        If fila Is Nothing OrElse IsDBNull(fila(columna)) Then Return 0D
        Return Convert.ToDecimal(fila(columna))
    End Function

    Private Sub LimpiarResumen()
        lblPeriodo.Text = ""
        lblEntradas.Text = "S/ 0.00"
        lblSalidas.Text = "S/ 0.00"
        lblSaldo.Text = "S/ 0.00"
        lblFlujo.Text = "—"
        lblDetalleEntradas.Text = "0 cobros · 0 pedidos"
        lblDetalleSalidas.Text = "0 compras · 0 movimientos de caja"
        lblGraficoEntradas.Text = "S/ 0.00"
        lblGraficoSalidas.Text = "S/ 0.00"
        lblTablaCobros.Text = "0"
        lblTablaPedidos.Text = "0"
        lblTablaEntradas.Text = "S/ 0.00"
        lblTablaCantidadCompras.Text = "0"
        lblTablaCompras.Text = "S/ 0.00"
        lblTablaCaja.Text = "S/ 0.00"
        lblTablaSalidas.Text = "S/ 0.00"
        lblTablaSaldo.Text = "S/ 0.00"
        lblTablaFlujo.Text = "—"

        lblSaldo.CssClass = "ingresos-tarjeta-valor"
        lblTablaSaldo.CssClass = ""
        lblFlujo.CssClass = "ingresos-tarjeta-valor"
        lblTablaFlujo.CssClass = ""

        barraVentas.Style("width") = "0%"
        barraCompras.Style("width") = "0%"

        grvMetodos.DataSource = Nothing
        grvMetodos.DataBind()
    End Sub

End Class

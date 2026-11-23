Imports System.Data
Imports System.Globalization
Imports System.Web.UI.WebControls

Partial Class wfReporteFlujoInventario
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
        ddlCategoria.Items.Clear()
        ddlCategoria.Items.Add(New ListItem("Todas", ""))
        For Each categoria As String In Inventario.Categorias
            ddlCategoria.Items.Add(New ListItem(categoria, categoria))
        Next

        txtProducto.Attributes("list") = "dlProductos"
        txtProducto.Attributes("autocomplete") = "off"
        txtProducto.Attributes("placeholder") = "Escriba para buscar un producto"

        ddlProducto.Items.Clear()
        ddlProducto.Items.Add(New ListItem("Todos", "0"))
        For Each fila As DataRow In New Inventario().ListarTodasLasVariantes().Rows
            ddlProducto.Items.Add(New ListItem(
                fila("producto").ToString() & " | " & fila("unidad_costo").ToString(),
                fila("id_variante").ToString()))
        Next

        Dim hoy As Date = ReporteUtil.HoyPeru()
        txtHasta.Text = hoy.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).AddMonths(-11).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub ddlOrden_SelectedIndexChanged(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub grvFlujo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvFlujo.PageIndex = e.NewPageIndex
        GenerarReporte(False)
    End Sub

    ' El cuadro de texto es el que ve el usuario; el combo oculto conserva el ID.
    Private Sub SincronizarProducto()
        If ddlProducto.Items.Count = 0 Then Return

        Dim texto As String = If(txtProducto.Text, "").Trim()
        If texto.Length = 0 Then
            ddlProducto.SelectedIndex = 0
            Return
        End If

        For Each item As ListItem In ddlProducto.Items
            If String.Equals(item.Text, texto, StringComparison.OrdinalIgnoreCase) Then
                ddlProducto.SelectedValue = item.Value
                Return
            End If
        Next

        For Each item As ListItem In ddlProducto.Items
            If item.Text.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 Then
                ddlProducto.SelectedValue = item.Value
                Return
            End If
        Next

        ddlProducto.SelectedIndex = 0
    End Sub

    Private Function ExpresionOrden() As String
        Select Case ddlOrden.SelectedValue
            Case "FECHA_ASC"
                Return "periodo ASC, producto ASC"
            Case "PRODUCTO_ASC"
                Return "producto ASC, periodo DESC"
            Case "PRODUCTO_DESC"
                Return "producto DESC, periodo DESC"
            Case Else
                Return "periodo DESC, producto ASC"
        End Select
    End Function

    Private Function Ordenar(datos As DataTable) As DataView
        Dim vista As DataView = datos.DefaultView
        vista.Sort = ExpresionOrden()
        Return vista
    End Function

    Private Sub GenerarReporte(Optional restablecerPagina As Boolean = True)
        lblMensaje.Text = ""
        SincronizarProducto()
        If restablecerPagina Then grvFlujo.PageIndex = 0

        Dim desde As Date
        Dim hasta As Date
        If Not Date.TryParseExact(txtDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, desde) OrElse
           Not Date.TryParseExact(txtHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, hasta) Then
            lblMensaje.Text = "Ingrese ambas fechas con el formato AAAA-MM-DD."
            Return
        End If
        If desde > hasta Then
            lblMensaje.Text = "La fecha Desde no puede ser posterior a Hasta."
            Return
        End If

        Dim idVariante As Integer? = Nothing
        Dim valor As Integer
        If Integer.TryParse(ddlProducto.SelectedValue, valor) AndAlso valor > 0 Then idVariante = valor
        Dim categoria As String = ddlCategoria.SelectedValue
        If String.IsNullOrEmpty(categoria) Then categoria = Nothing

        Try
            Dim datos As DataTable = objReporte.FlujoInventario(desde, hasta, categoria, idVariante)
            Dim entradas As Decimal = 0D
            Dim salidas As Decimal = 0D
            Dim ajustes As Decimal = 0D
            Dim neto As Decimal = 0D
            Dim movimientos As Integer = 0

            For Each fila As DataRow In datos.Rows
                If Not IsDBNull(fila("entradas")) Then entradas += CDec(fila("entradas"))
                If Not IsDBNull(fila("salidas")) Then salidas += CDec(fila("salidas"))
                If Not IsDBNull(fila("ajustes")) Then ajustes += CDec(fila("ajustes"))
                If Not IsDBNull(fila("neto")) Then neto += CDec(fila("neto"))
                If Not IsDBNull(fila("movimientos")) Then movimientos += CInt(fila("movimientos"))
            Next

            grvFlujo.DataSource = Ordenar(datos)
            grvFlujo.DataBind()
            lblResumen.Text = "Períodos: " & datos.Rows.Count.ToString() &
                " | Movimientos: " & movimientos.ToString() &
                " | Entradas: " & entradas.ToString("N2") &
                " | Salidas: " & salidas.ToString("N2") &
                " | Ajustes: " & ajustes.ToString("N2") &
                " | Neto: " & neto.ToString("N2")
        Catch ex As Exception
            grvFlujo.DataSource = Nothing
            grvFlujo.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo generar el reporte de flujo de inventario."
        End Try
    End Sub

End Class

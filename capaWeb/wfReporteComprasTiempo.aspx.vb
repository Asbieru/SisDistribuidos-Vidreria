Imports System.Data
Imports System.Globalization

Partial Class wfReporteComprasTiempo
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
            Dim hoy As Date = ReporteUtil.HoyPeru()
            txtHasta.Text = hoy.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            txtDesde.Text = New Date(hoy.Year, hoy.Month, 1).AddMonths(-11).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            GenerarReporte()
        End If
    End Sub

    Protected Sub btnGenerar_Click(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub ddlOrden_SelectedIndexChanged(sender As Object, e As EventArgs)
        GenerarReporte()
    End Sub

    Protected Sub grvCompras_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvCompras.PageIndex = e.NewPageIndex
        GenerarReporte(False)
    End Sub

    Private Function ExpresionOrden() As String
        Select Case ddlOrden.SelectedValue
            Case "FECHA_ASC"
                Return "periodo ASC, proveedor ASC"
            Case "PROVEEDOR_ASC"
                Return "proveedor ASC, periodo DESC"
            Case "PROVEEDOR_DESC"
                Return "proveedor DESC, periodo DESC"
            Case Else
                Return "periodo DESC, proveedor ASC"
        End Select
    End Function

    Private Function Ordenar(datos As DataTable) As DataView
        Dim vista As DataView = datos.DefaultView
        vista.Sort = ExpresionOrden()
        Return vista
    End Function

    Private Sub GenerarReporte(Optional restablecerPagina As Boolean = True)
        lblMensaje.Text = ""
        If restablecerPagina Then grvCompras.PageIndex = 0

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

        Try
            Dim datos As DataTable = objReporte.ComprasPorTiempo(desde, hasta)
            Dim cantidad As Integer = 0
            Dim total As Decimal = 0D

            For Each fila As DataRow In datos.Rows
                If Not IsDBNull(fila("cantidad_compras")) Then cantidad += CInt(fila("cantidad_compras"))
                If Not IsDBNull(fila("total_compras")) Then total += CDec(fila("total_compras"))
            Next

            grvCompras.DataSource = Ordenar(datos)
            grvCompras.DataBind()
            lblResumen.Text = "Períodos/proveedores: " & datos.Rows.Count.ToString() &
                " | Compras: " & cantidad.ToString() &
                " | Total: S/ " & total.ToString("N3")
        Catch ex As Exception
            grvCompras.DataSource = Nothing
            grvCompras.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo generar el reporte de compras."
        End Try
    End Sub

End Class

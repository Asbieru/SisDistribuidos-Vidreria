Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfListadoCompras
    Inherits System.Web.UI.Page

    Private ReadOnly objCompra As New Compra()
    Private autorizado As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Try
            Dim permisos = New Permiso().codigosPermisoUsuario(idUsuario)
            autorizado = permisos IsNot Nothing AndAlso permisos.Contains("COMPRAS")
        Catch ex As Exception
            lblMensaje.Text = "No se pudieron verificar los permisos."
        End Try

        If Not autorizado Then
            pnlListado.Visible = False
            pnlDetalle.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar compras."
            Return
        End If

        If Not IsPostBack Then
            txtDesde.Text = New Date(Today.Year, Today.Month, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            txtHasta.Text = Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            CargarListado("", Nothing, Nothing)
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        lblMensaje.Text = ""
        Dim desde As Date
        Dim hasta As Date
        If txtDesde.Text = "" AndAlso txtHasta.Text = "" Then
            CargarListado(txtProveedor.Text.Trim(), Nothing, Nothing)
        ElseIf Not Date.TryParseExact(txtDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, desde) OrElse
               Not Date.TryParseExact(txtHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, hasta) Then
            lblMensaje.Text = "Ingrese ambas fechas o deje las dos en blanco para buscar solo por proveedor."
        ElseIf desde > hasta Then
            lblMensaje.Text = "La fecha Desde no puede ser posterior a Hasta."
        Else
            CargarListado(txtProveedor.Text.Trim(), desde, hasta)
        End If
    End Sub

    Protected Sub btnVerTodas_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        txtProveedor.Text = ""
        txtDesde.Text = ""
        txtHasta.Text = ""
        CargarListado("", Nothing, Nothing)
    End Sub

    Private Sub CargarListado(proveedor As String, desde As Date?, hasta As Date?)
        lblMensaje.Text = ""
        pnlDetalle.Visible = False
        Try
            Dim datos As DataTable = If(desde.HasValue AndAlso hasta.HasValue,
                objCompra.ListarPorFecha(desde.Value, hasta.Value), objCompra.Listar())
            Dim resultados As DataTable = datos.Clone()
            Dim total As Decimal = 0D
            For Each fila As DataRow In datos.Rows
                If fila("proveedor").ToString().IndexOf(proveedor, StringComparison.CurrentCultureIgnoreCase) < 0 Then Continue For
                resultados.ImportRow(fila)
                If Not IsDBNull(fila("total")) Then total += CDec(fila("total"))
            Next
            grvCompras.DataSource = resultados
            grvCompras.DataBind()
            lblResumen.Text = "Compras: " & resultados.Rows.Count.ToString() & " | Total: S/ " & total.ToString("N3")
        Catch ex As Exception
            grvCompras.DataSource = Nothing
            grvCompras.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo cargar el listado de compras."
        End Try
    End Sub

    Protected Sub grvCompras_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not autorizado OrElse e.CommandName <> "VerDetalle" Then Return
        Dim id As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), id) OrElse id <= 0 Then Return

        lblMensaje.Text = ""
        pnlDetalle.Visible = False
        Try
            Dim cabecera As DataTable = objCompra.ObtenerCompra(id)
            If cabecera.Rows.Count = 0 Then
                lblMensaje.Text = "La compra seleccionada ya no existe."
                Return
            End If

            Dim detalle As DataTable = objCompra.ObtenerDetalleParaEditar(id)
            detalle.Columns.Add("medida", GetType(String))
            For Each fila As DataRow In detalle.Rows
                If Not IsDBNull(fila("ancho_cm")) AndAlso Not IsDBNull(fila("alto_cm")) Then
                    fila("medida") = CDec(fila("ancho_cm")).ToString("0.###") & " x " &
                        CDec(fila("alto_cm")).ToString("0.###") & " cm"
                ElseIf Not IsDBNull(fila("largo_cm")) Then
                    fila("medida") = CDec(fila("largo_cm")).ToString("0.###") & " cm"
                End If
            Next
            grvDetalle.DataSource = detalle
            grvDetalle.DataBind()
            lblDetalle.Text = "Compra " & id.ToString() & " - " & Server.HtmlEncode(cabecera.Rows(0)("proveedor").ToString())
            pnlDetalle.Visible = True
        Catch ex As Exception
            lblMensaje.Text = "No se pudo consultar el detalle de la compra."
        End Try
    End Sub

    Protected Sub btnCerrarDetalle_Click(sender As Object, e As EventArgs)
        pnlDetalle.Visible = False
    End Sub
End Class

Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfListadoVentas
    Inherits System.Web.UI.Page

    Private ReadOnly objVenta As New Venta()
    Private autorizado As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Try
            Dim permisos = New Permiso().codigosPermisoUsuario(idUsuario)
            autorizado = permisos IsNot Nothing AndAlso permisos.Contains("PEDIDOS") AndAlso permisos.Contains("PAGOS")
        Catch ex As Exception
            lblMensaje.Text = "No se pudieron verificar los permisos."
        End Try

        If Not autorizado Then
            pnlListado.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar ventas."
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
            CargarListado(txtCliente.Text.Trim(), Nothing, Nothing)
        ElseIf Not Date.TryParseExact(txtDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, desde) OrElse
               Not Date.TryParseExact(txtHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, hasta) Then
            lblMensaje.Text = "Ingrese ambas fechas o deje las dos en blanco para buscar solo por cliente."
        ElseIf desde > hasta Then
            lblMensaje.Text = "La fecha Desde no puede ser posterior a Hasta."
        Else
            CargarListado(txtCliente.Text.Trim(), desde, hasta)
        End If
    End Sub

    Protected Sub btnVerTodas_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        txtCliente.Text = ""
        ddlEstado.SelectedValue = "TODOS"
        txtDesde.Text = ""
        txtHasta.Text = ""
        CargarListado("", Nothing, Nothing)
    End Sub

    Private Sub CargarListado(cliente As String, desde As Date?, hasta As Date?)
        lblMensaje.Text = ""
        Try
            Dim datos As DataTable = objVenta.Listar(desde, hasta, cliente, ddlEstado.SelectedValue)
            Dim importe As Decimal = 0D
            Dim cobrado As Decimal = 0D
            Dim deuda As Decimal = 0D
            Dim excedentes As Decimal = 0D
            For Each fila As DataRow In datos.Rows
                importe += CDec(fila("total"))
                cobrado += CDec(fila("pagado"))
                Dim saldo As Decimal = CDec(fila("saldo"))
                If saldo > 0D Then deuda += saldo
                If saldo < 0D Then excedentes -= saldo
            Next
            grvVentas.DataSource = datos
            grvVentas.DataBind()
            lblResultados.Text = "Ventas encontradas: " & datos.Rows.Count.ToString()
            lblResumen.Text = "Importe: S/ " & importe.ToString("N2") &
                " | Cobrado: S/ " & cobrado.ToString("N2") &
                " | Deuda pendiente: S/ " & deuda.ToString("N2") &
                " | Excedentes: S/ " & excedentes.ToString("N2")
        Catch ex As Exception
            grvVentas.DataSource = Nothing
            grvVentas.DataBind()
            lblResultados.Text = ""
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo cargar el listado de ventas."
        End Try
    End Sub
End Class

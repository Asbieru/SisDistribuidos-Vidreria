Imports System.Data
Imports System.Globalization
Imports capaNegocio

' Kardex: movimientos del inventario (vw_inv_movimientos) por fechas y producto.
Partial Class wfVerKardex
    Inherits System.Web.UI.Page

    Private ReadOnly objInv As New Inventario()
    Private autorizado As Boolean
    Private Const TODOS As String = "(Todos)"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Try
            Dim permisos = New Permiso().codigosPermisoUsuario(idUsuario)
            autorizado = permisos IsNot Nothing AndAlso permisos.Contains("INVENTARIO")
        Catch ex As Exception
            lblMensaje.Text = "No se pudieron verificar los permisos."
        End Try

        If Not autorizado Then
            pnlKardex.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar el inventario."
            Return
        End If

        If Not IsPostBack Then
            Try
                ddlVariante.Items.Add(New ListItem(TODOS, ""))
                For Each fila As DataRow In objInv.ListarTodasLasVariantes().Rows
                    ddlVariante.Items.Add(New ListItem(fila("producto").ToString(), fila("id_variante").ToString()))
                Next
            Catch ex As Exception
                lblMensaje.Text = "No se pudieron cargar los productos."
            End Try
            txtDesde.Text = Today.AddDays(-30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            txtHasta.Text = Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            CargarKardex()
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        CargarKardex()
    End Sub

    Private Sub CargarKardex()
        Dim desde As Date
        Dim hasta As Date
        If Not Date.TryParseExact(txtDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, desde) OrElse
           Not Date.TryParseExact(txtHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, hasta) Then
            lblMensaje.Text = "Ingrese las fechas Desde y Hasta."
            Return
        End If
        If desde > hasta Then
            lblMensaje.Text = "La fecha Desde no puede ser posterior a Hasta."
            Return
        End If

        lblMensaje.Text = ""
        Try
            Dim idVariante As Integer? = Nothing
            If ddlVariante.SelectedValue <> "" Then idVariante = CInt(ddlVariante.SelectedValue)
            ' Si "Hasta" es hoy no se pone límite superior (somee puede guardar con otra zona horaria)
            Dim limite As Date? = If(hasta >= Today, CType(Nothing, Date?), hasta)
            Dim dt As DataTable = objInv.ListarMovimientos(desde, limite, idVariante)
            grvKardex.DataSource = dt
            grvKardex.DataBind()
            lblResumen.Text = "Movimientos: " & dt.Rows.Count.ToString()
        Catch ex As Exception
            grvKardex.DataSource = Nothing
            grvKardex.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo cargar el kardex."
        End Try
    End Sub
End Class

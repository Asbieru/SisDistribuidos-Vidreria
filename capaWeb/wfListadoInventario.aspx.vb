Imports System.Data
Imports capaNegocio

' Existencias por variante (vw_inv_resumen) y mantenimiento del stock mínimo.
Partial Class wfListadoInventario
    Inherits System.Web.UI.Page

    Private ReadOnly objInv As New Inventario()
    Private autorizado As Boolean
    Private Const TODAS As String = "(Todas)"

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
            pnlListado.Visible = False
            pnlMinimo.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar el inventario."
            Return
        End If

        If Not IsPostBack Then
            ddlCategoria.Items.Add(TODAS)
            For Each categoria As String In Inventario.Categorias
                ddlCategoria.Items.Add(categoria)
            Next
            CargarListado()
        End If
    End Sub

    Private Sub CargarListado()
        lblMensaje.CssClass = "mensaje-error"
        lblMensaje.Text = ""
        Try
            Dim categoria As String = If(ddlCategoria.SelectedIndex <= 0, Nothing, ddlCategoria.SelectedValue)
            Dim datos As DataTable = objInv.ListarResumen(categoria, chkBajoMinimo.Checked)
            ' Filtro por nombre de producto (lo escrito en Buscar)
            Dim texto As String = txtBuscar.Text.Trim()
            Dim dt As DataTable = datos.Clone()
            For Each fila As DataRow In datos.Rows
                If fila("producto").ToString().IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0 Then dt.ImportRow(fila)
            Next

            Dim total As Integer = dt.Rows.Count
            Dim totalPaginas As Integer = Math.Max(1, CInt(Math.Ceiling(total / grvInventario.PageSize)))
            If grvInventario.PageIndex >= totalPaginas Then grvInventario.PageIndex = totalPaginas - 1
            grvInventario.DataSource = dt
            grvInventario.DataBind()

            Dim cantidad As String = "Productos: 0"
            If total > 0 Then
                Dim desde As Integer = grvInventario.PageIndex * grvInventario.PageSize + 1
                Dim hasta As Integer = Math.Min(desde + grvInventario.PageSize - 1, total)
                cantidad = "Mostrando " & desde & " - " & hasta & " de " & total &
                           " producto(s). Página " & (grvInventario.PageIndex + 1) & " de " & totalPaginas & "."
            End If
            lblResumen.Text = cantidad & " | En o bajo el stock mínimo: " & objInv.ContarBajoMinimo().ToString()
        Catch ex As Exception
            grvInventario.DataSource = Nothing
            grvInventario.DataBind()
            lblResumen.Text = ""
            lblMensaje.Text = "No se pudo cargar el inventario."
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        pnlMinimo.Visible = False
        grvInventario.PageIndex = 0   ' una búsqueda nueva empieza en la página 1
        CargarListado()
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        txtBuscar.Text = ""
        ddlCategoria.SelectedIndex = 0
        chkBajoMinimo.Checked = False
        pnlMinimo.Visible = False
        grvInventario.PageIndex = 0
        CargarListado()
    End Sub

    Protected Sub grvInventario_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        If Not autorizado Then Return
        grvInventario.PageIndex = e.NewPageIndex
        pnlMinimo.Visible = False
        CargarListado()   ' mantiene los filtros que estaban elegidos
    End Sub

    ' "Stock mínimo" de una fila: abre el panel para cambiarlo
    Protected Sub grvInventario_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not autorizado OrElse e.CommandName <> "EditarMinimo" Then Return
        Dim fila As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), fila) OrElse fila < 0 OrElse fila >= grvInventario.DataKeys.Count Then Return

        Dim claves As DataKey = grvInventario.DataKeys(fila)
        ViewState("idVariante") = CInt(claves("id_variante"))
        lblProducto.Text = Server.HtmlEncode(claves("producto").ToString())
        txtMinimo.Text = claves("stock_minimo").ToString()
        pnlMinimo.Visible = True
        txtMinimo.Focus()
    End Sub

    Protected Sub btnGuardarMinimo_Click(sender As Object, e As EventArgs)
        If Not autorizado OrElse ViewState("idVariante") Is Nothing Then Return
        Dim minimo As Integer
        If Not Integer.TryParse(txtMinimo.Text.Trim(), minimo) OrElse minimo < 0 Then
            lblMensaje.Text = "Ingrese un stock mínimo válido (entero, 0 o más)."
            Return
        End If
        Try
            objInv.ActualizarStockMinimo(CInt(ViewState("idVariante")), minimo)
            pnlMinimo.Visible = False
            ViewState("idVariante") = Nothing
            CargarListado()
            lblMensaje.CssClass = "mensaje-ok"
            lblMensaje.Text = "Stock mínimo actualizado."
        Catch ex As Exception
            lblMensaje.CssClass = "mensaje-error"
            lblMensaje.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnCancelarMinimo_Click(sender As Object, e As EventArgs)
        pnlMinimo.Visible = False
        ViewState("idVariante") = Nothing
    End Sub
End Class

Imports System.Data
Imports capaNegocio

' Stock de productos por caja o unidad y registro de entradas, salidas y ajustes
' (todo pasa por sp_inv_mover_cajas, que valida el stock y lo deja en el kardex).
Partial Class wfMovimientoCajas
    Inherits System.Web.UI.Page

    Private ReadOnly objCaja As New InventarioCaja()
    Private autorizado As Boolean
    Private idUsuario As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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
            pnlMovimiento.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar el inventario."
            Return
        End If

        If Not IsPostBack Then CargarListado()
    End Sub

    Private Sub CargarListado()
        Try
            grvCajas.DataSource = objCaja.Listar()
            grvCajas.DataBind()
        Catch ex As Exception
            grvCajas.DataSource = Nothing
            grvCajas.DataBind()
            MostrarError("No se pudo cargar el stock por cajas.")
        End Try
    End Sub

    ' "Registrar movimiento" de una fila: abre el panel
    Protected Sub grvCajas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not autorizado OrElse e.CommandName <> "Mover" Then Return
        Dim fila As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), fila) OrElse fila < 0 OrElse fila >= grvCajas.DataKeys.Count Then Return

        lblMensaje.Text = ""
        Dim claves As DataKey = grvCajas.DataKeys(fila)
        ViewState("idVariante") = CInt(claves("id_variante"))
        ViewState("unidad") = claves("unidad_costo").ToString()
        lblProducto.Text = Server.HtmlEncode(claves("producto").ToString()) & " - Stock actual: " & claves("stock_cajas").ToString() &
                           " " & InventarioCaja.NombreUnidad(claves("unidad_costo").ToString())
        txtCantidad.Text = ""
        txtObservacion.Text = ""
        ddlTipo.SelectedIndex = 0
        pnlMovimiento.Visible = True
        txtCantidad.Focus()
    End Sub

    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        If Not autorizado OrElse ViewState("idVariante") Is Nothing Then Return
        Dim cantidad As Integer
        Dim esAjuste As Boolean = (ddlTipo.SelectedValue = "AJUSTE")
        ' En un ajuste el stock real puede ser 0; en entradas y salidas debe ser mayor a 0
        If Not Integer.TryParse(txtCantidad.Text.Trim(), cantidad) OrElse cantidad < 0 OrElse (cantidad = 0 AndAlso Not esAjuste) Then
            MostrarError("Ingrese una cantidad entera válida.")
            Return
        End If

        Dim idVariante As Integer = CInt(ViewState("idVariante"))
        Try
            Dim stock As Integer
            Select Case ddlTipo.SelectedValue
                Case "COMPRA" : stock = objCaja.SumarStock(idVariante, cantidad, "COMPRA", idUsuario, Nothing, txtObservacion.Text)
                Case "VENTA" : stock = objCaja.RestarStock(idVariante, cantidad, "VENTA", idUsuario, Nothing, txtObservacion.Text)
                Case "DEFECTO" : stock = objCaja.RestarStock(idVariante, cantidad, "DEFECTO", idUsuario, Nothing, txtObservacion.Text)
                Case Else : stock = objCaja.AjustarStock(idVariante, cantidad, idUsuario, txtObservacion.Text)
            End Select
            pnlMovimiento.Visible = False
            CargarListado()
            MostrarOk("Movimiento registrado. Stock resultante: " & stock.ToString() & " " &
                      InventarioCaja.NombreUnidad(Convert.ToString(ViewState("unidad"))) & ".")
        Catch ex As Exception
            MostrarError(ex.Message)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
        pnlMovimiento.Visible = False
        ViewState("idVariante") = Nothing
    End Sub

    Private Sub MostrarError(texto As String)
        lblMensaje.CssClass = "mensaje-error"
        lblMensaje.Text = texto
    End Sub

    Private Sub MostrarOk(texto As String)
        lblMensaje.CssClass = "mensaje-ok"
        lblMensaje.Text = texto
    End Sub
End Class

Imports System.Data
Imports System.Globalization

Partial Class wfFichaCliente
    Inherits System.Web.UI.Page

    Private ReadOnly objCliente As New Cliente()
    Private idCliente As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        pnlFicha.Visible = False
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not Integer.TryParse(Request.QueryString("id"), idCliente) OrElse idCliente <= 0 Then
            lblMensaje.Text = "Seleccione un cliente desde el listado."
            Return
        End If

        Try
            If Not String.Equals(Convert.ToString(Session("rol")), "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase) Then
                Dim permisos = New Permiso().codigosPermisoUsuario(Convert.ToInt32(Session("idUsuario")))
                If permisos Is Nothing OrElse Not permisos.Contains("CLIENTES") Then
                    lblMensaje.Text = "No tiene permiso para consultar la ficha comercial."
                    Return
                End If
            End If

            Dim cliente As DataTable = objCliente.Obtener(idCliente)
            If cliente.Rows.Count = 0 Then
                lblMensaje.Text = "El cliente ya no existe. Vuelva al listado."
                Return
            End If

            Dim filaCliente As DataRow = cliente.Rows(0)
            litCliente.Text = filaCliente("nombre").ToString() &
                " | " & If(filaCliente("tipo_persona").ToString() = "", "Por clasificar", filaCliente("tipo_persona").ToString()) &
                " | " & If(filaCliente("tipo_documento").ToString() = "", "Documento", filaCliente("tipo_documento").ToString()) &
                " " & filaCliente("documento").ToString()
            pnlFicha.Visible = True
            If Not IsPostBack Then CargarPedidos()
        Catch
            lblMensaje.Text = "No se pudo abrir la ficha. Vuelva al listado e inténtelo nuevamente."
        End Try
    End Sub

    Private Sub CargarPedidos()
        If Not pnlFicha.Visible Then Return
        lblMensaje.Text = ""
        litResumen.Text = ""
        litPagos.Text = "Seleccione Ver pagos en un pedido."
        grvPagos.Visible = False
        grvPedidos.DataSource = Nothing
        grvPedidos.DataBind()

        Try
            Dim desde As Date? = LeerFecha(txtDesde.Text)
            Dim hasta As Date? = LeerFecha(txtHasta.Text)
            If desde.HasValue AndAlso hasta.HasValue AndAlso desde.Value > hasta.Value Then
                lblMensaje.Text = "La fecha Desde no puede ser posterior a Hasta."
                Return
            End If

            Dim pedidos As DataTable = objCliente.Historial(idCliente, ddlEstado.SelectedValue, desde, hasta)
            Dim totalPaginas As Integer = Math.Max(1, CInt(Math.Ceiling(pedidos.Rows.Count / grvPedidos.PageSize)))
            If grvPedidos.PageIndex >= totalPaginas Then grvPedidos.PageIndex = totalPaginas - 1
            grvPedidos.DataSource = pedidos
            grvPedidos.DataBind()

            Dim total As Decimal = 0D
            Dim pagado As Decimal = 0D
            Dim saldo As Decimal = 0D
            For Each fila As DataRow In pedidos.Rows
                If fila("estado").ToString() = "CANCELADO" Then Continue For
                total += CDec(fila("total"))
                pagado += CDec(fila("pagado"))
                saldo += CDec(fila("saldo"))
            Next
            Dim cantidad As String = pedidos.Rows.Count.ToString() & " pedido(s)."
            If pedidos.Rows.Count > 0 Then
                Dim primero As Integer = grvPedidos.PageIndex * grvPedidos.PageSize + 1
                Dim ultimo As Integer = Math.Min(primero + grvPedidos.PageSize - 1, pedidos.Rows.Count)
                cantidad = "Mostrando " & primero & " - " & ultimo & " de " & pedidos.Rows.Count &
                           " pedido(s). Página " & (grvPedidos.PageIndex + 1) & " de " & totalPaginas & "."
            End If
            ' El resumen suma todos los pedidos del filtro, no solo los de la página visible.
            litResumen.Text = String.Format("{0} Resumen sin cancelados: total S/ {1:N2} | pagado S/ {2:N2} | saldo S/ {3:N2}.",
                                            cantidad, total, pagado, saldo)
        Catch ex As FormatException
            lblMensaje.Text = "Ingrese fechas válidas."
        Catch
            lblMensaje.Text = "No se pudieron cargar los pedidos. Vuelva a intentar."
        End Try
    End Sub

    Private Shared Function LeerFecha(texto As String) As Date?
        If String.IsNullOrWhiteSpace(texto) Then Return Nothing
        Return Date.ParseExact(texto, "yyyy-MM-dd", CultureInfo.InvariantCulture)
    End Function

    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs)
        grvPedidos.PageIndex = 0   ' un filtro nuevo empieza en la página 1
        CargarPedidos()
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        If Not pnlFicha.Visible Then Return
        ddlEstado.SelectedIndex = 0
        txtDesde.Text = ""
        txtHasta.Text = ""
        grvPedidos.PageIndex = 0
        CargarPedidos()
    End Sub

    Protected Sub grvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        If Not pnlFicha.Visible Then Return
        grvPedidos.PageIndex = e.NewPageIndex
        CargarPedidos()   ' mantiene los filtros que estaban escritos
    End Sub

    Protected Sub grvPedidos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not pnlFicha.Visible OrElse e.CommandName <> "VerPagos" Then Return
        Dim idPedido As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), idPedido) OrElse idPedido <= 0 Then Return

        lblMensaje.Text = ""
        grvPagos.Visible = False
        litPagos.Text = ""
        Try
            grvPagos.DataSource = objCliente.PagosDePedido(idCliente, idPedido)
            grvPagos.DataBind()
            grvPagos.Visible = True
            litPagos.Text = "Pedido " & idPedido.ToString() &
                ". Se muestra una fila por método; el total de un pago puede repetirse."
        Catch
            lblMensaje.Text = "No se pudieron cargar los pagos. Vuelva a seleccionar el pedido."
        End Try
    End Sub
End Class

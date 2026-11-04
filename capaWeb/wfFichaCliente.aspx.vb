Imports System.Data
Imports System.Globalization
Imports capaNegocio

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

            litCliente.Text = cliente.Rows(0)("nombre").ToString()
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
            litResumen.Text = String.Format("{0} pedido(s). Resumen sin cancelados: total S/ {1:N2} | pagado S/ {2:N2} | saldo S/ {3:N2}.",
                                            pedidos.Rows.Count, total, pagado, saldo)
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
        CargarPedidos()
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        If Not pnlFicha.Visible Then Return
        ddlEstado.SelectedIndex = 0
        txtDesde.Text = ""
        txtHasta.Text = ""
        CargarPedidos()
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

Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfRegistrarPago
    Inherits System.Web.UI.Page

    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objPago As New Pago()
    Private ReadOnly objMetodo As New MetodoPago()

    ' Pedido que se esta cobrando (se conserva entre postbacks)
    Private Property IdPedidoSeleccionado As Integer
        Get
            If ViewState("idPedido") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idPedido"))
        End Get
        Set(value As Integer)
            ViewState("idPedido") = value
        End Set
    End Property

    ' Detalle de pago en edicion (0 = ninguno)
    Private Property IdDetalleEditando As Integer
        Get
            If ViewState("idDetalle") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idDetalle"))
        End Get
        Set(value As Integer)
            ViewState("idDetalle") = value
        End Set
    End Property

    Private Property MontoEditando As Decimal
        Get
            If ViewState("montoEdicion") Is Nothing Then Return 0D
            Return Convert.ToDecimal(ViewState("montoEdicion"))
        End Get
        Set(value As Decimal)
            ViewState("montoEdicion") = value
        End Set
    End Property

    ' Total del pedido seleccionado
    Private Property TotalPedido As Decimal
        Get
            If ViewState("totalPedido") Is Nothing Then Return 0D
            Return Convert.ToDecimal(ViewState("totalPedido"))
        End Get
        Set(value As Decimal)
            ViewState("totalPedido") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarMetodos()
            CargarPedidos("")

            ' Permite abrir la pagina con ?idPedido=123 desde otros modulos.
            Dim idInicial As Integer
            If Integer.TryParse(Request.QueryString("idPedido"), idInicial) AndAlso idInicial > 0 Then
                SeleccionarPedido(idInicial)
            Else
                MostrarBusqueda(True)
            End If
        Else
            ' Restaura el panel de proceso cuando ya habia un pedido elegido.
            If IdPedidoSeleccionado > 0 Then
                pnlBusqueda.Visible = False
                pnlProceso.Visible = True
                ActualizarVueltoVisible()
            Else
                pnlBusqueda.Visible = True
                pnlProceso.Visible = False
            End If
        End If
    End Sub

    ' ------------------------- Seleccion de pedido -------------------------

    Private Sub MostrarBusqueda(mostrar As Boolean)
        pnlBusqueda.Visible = mostrar
        pnlProceso.Visible = Not mostrar
    End Sub

    Private Sub CargarPedidos(filtro As String)
        Try
            Dim dt As DataTable = objPedido.ListarDetallado()
            If dt Is Nothing Then
                dt = New DataTable()
            End If

            If Not String.IsNullOrEmpty(filtro) Then
                Dim texto As String = filtro.Trim().ToUpperInvariant()
                Dim filtrado As DataTable = dt.Clone()
                For Each fila As DataRow In dt.Rows
                    Dim coincide As Boolean =
                        fila("id_pedido").ToString().Contains(texto) OrElse
                        fila("cliente").ToString().ToUpperInvariant().Contains(texto)
                    If coincide Then
                        filtrado.ImportRow(fila)
                    End If
                Next
                dt = filtrado
            End If

            grvPedidos.DataSource = dt
            grvPedidos.DataBind()
            lblMensajeBusqueda.CssClass = ""
            lblMensajeBusqueda.Text = If(dt.Rows.Count = 0, "No se encontraron pedidos.", "")
        Catch ex As Exception
            lblMensajeBusqueda.CssClass = "mensaje-error"
            lblMensajeBusqueda.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnBuscarPedido_Click(sender As Object, e As EventArgs)
        CargarPedidos(txtBuscarPedido.Text)
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscarPedido.Text = ""
        CargarPedidos("")
    End Sub

    Protected Sub grvPedidos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "SeleccionarPedido" Then
            Dim id As Integer
            If Integer.TryParse(e.CommandArgument.ToString(), id) AndAlso id > 0 Then
                SeleccionarPedido(id)
            End If
        End If
    End Sub

    Private Sub SeleccionarPedido(id As Integer)
        If id <= 0 Then Return

        IdPedidoSeleccionado = id
        TotalPedido = 0D
        IdDetalleEditando = 0
        MontoEditando = 0D

        lblPedidoId.Text = id.ToString()

        Try
            Dim dt As DataTable = objPedido.Obtener(id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim fila As DataRow = dt.Rows(0)
                TotalPedido = ADecimal(fila("total"))
                lblPedidoInfo.Text = "Cliente: " & Server.HtmlEncode(fila("cliente").ToString()) &
                                     " &nbsp;|&nbsp; Total: S/. " & FormatoNumero(TotalPedido) &
                                     " &nbsp;|&nbsp; Estado: " & Server.HtmlEncode(fila("estado").ToString())
            Else
                lblPedidoInfo.Text = "Pedido #" & id.ToString()
            End If
        Catch ex As Exception
            lblPedidoInfo.Text = "Pedido #" & id.ToString()
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try

        MostrarBusqueda(False)
        CargarMetodos()
        CargarDetalles()
    End Sub

    Protected Sub btnCambiarPedido_Click(sender As Object, e As EventArgs)
        IdPedidoSeleccionado = 0
        TotalPedido = 0D
        IdDetalleEditando = 0
        MontoEditando = 0D
        lblMensaje.Text = ""
        MostrarBusqueda(True)
        CargarPedidos("")
    End Sub

    ' ------------------------- Metodos y estado -------------------------

    Private Sub CargarMetodos()
        Try
            Dim dt As DataTable = objMetodo.AsegurarPorDefecto()
            ddlMetodoPago.DataSource = dt
            ddlMetodoPago.DataTextField = "nombre"
            ddlMetodoPago.DataValueField = "id_metodo_pago"
            ddlMetodoPago.DataBind()

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("No hay métodos de pago registrados.", "mensaje-error")
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub CargarDetalles()
        If IdPedidoSeleccionado <= 0 Then Return

        ' Igual que el formulario de escritorio: primero sale del modo edicion
        ' y despues refresca la grilla y el estado (que puede reactivar el vuelto).
        LimpiarEntrada()

        Try
            Dim dt As DataTable = objPago.ListarDetallesPorPedido(IdPedidoSeleccionado)
            grvPagos.DataSource = dt
            grvPagos.DataBind()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try

        SincronizarEstadoPedido()
        ActualizarEstado()
    End Sub

    ' Refleja en pedidos.estado la suma de pagos (nunca toca CANCELADO)
    Private Sub SincronizarEstadoPedido()
        If IdPedidoSeleccionado <= 0 Then Return
        Try
            objPedido.ActualizarEstadoPago(IdPedidoSeleccionado)
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' Pendiente si no hay pagos, Pagado si la suma coincide, Parcial en otro caso.
    Private Sub ActualizarEstado()
        If IdPedidoSeleccionado <= 0 Then Return

        Dim pagado As Decimal = 0D
        Try
            pagado = objPago.TotalPagado(IdPedidoSeleccionado)
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try

        lblPagado.Text = "S/. " & FormatoNumero(pagado)

        If pagado <= 0D Then
            lblEstado.Text = "Pendiente"
            lblEstado.CssClass = "estado-pendiente"
        ElseIf TotalPedido > 0D AndAlso pagado = TotalPedido Then
            lblEstado.Text = "Pagado"
            lblEstado.CssClass = "estado-pagado"
        Else
            lblEstado.Text = "Parcial"
            lblEstado.CssClass = "estado-parcial"
        End If

        ' Si lo pagado supera el total queda un vuelto por cuadrar: se fuerza
        ' EFECTIVO para poder registrarlo como monto negativo.
        Dim hayVueltoPendiente As Boolean = TotalPedido > 0D AndAlso pagado > TotalPedido
        If hayVueltoPendiente Then
            SeleccionarMetodoPorNombre("EFECTIVO")
        End If

        ActualizarVueltoVisible()
        chkVuelto.Checked = hayVueltoPendiente AndAlso EsEfectivo()
    End Sub

    Protected Sub ddlMetodoPago_SelectedIndexChanged(sender As Object, e As EventArgs)
        ActualizarVueltoVisible()
    End Sub

    Private Sub ActualizarVueltoVisible()
        Dim efectivo As Boolean = EsEfectivo()
        chkVuelto.Visible = efectivo
        If Not efectivo Then chkVuelto.Checked = False
    End Sub

    Private Function EsEfectivo() As Boolean
        Return ddlMetodoPago.SelectedItem IsNot Nothing AndAlso
               ddlMetodoPago.SelectedItem.Text.Trim().Equals("EFECTIVO", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub SeleccionarMetodoPorNombre(nombre As String)
        For Each item As ListItem In ddlMetodoPago.Items
            If item.Text.Trim().Equals(nombre, StringComparison.OrdinalIgnoreCase) Then
                ddlMetodoPago.SelectedValue = item.Value
                Return
            End If
        Next
    End Sub

    Private Function IdMetodoSeleccionado() As Integer
        Dim id As Integer
        If Integer.TryParse(ddlMetodoPago.SelectedValue, id) Then Return id
        Return 0
    End Function

    ' ------------------------- Guardar / editar / eliminar -------------------------

    Protected Sub btnGuardarPago_Click(sender As Object, e As EventArgs)
        If IdPedidoSeleccionado <= 0 Then
            MostrarMensaje("Seleccione un pedido.", "mensaje-error")
            Return
        End If

        Dim monto As Decimal
        If Not TryParseMonto(txtCantidad.Text, monto) OrElse monto <= 0D Then
            MostrarMensaje("Ingrese una cantidad válida mayor a cero.", "mensaje-error")
            Return
        End If

        Dim idMetodo As Integer = IdMetodoSeleccionado()
        If idMetodo <= 0 Then
            MostrarMensaje("Seleccione un método de pago.", "mensaje-error")
            Return
        End If

        ' chkVuelto activo => se guarda como negativo (devolucion)
        Dim esVuelto As Boolean = chkVuelto.Visible AndAlso chkVuelto.Checked

        Dim pagado As Decimal = 0D
        Try
            pagado = objPago.TotalPagado(IdPedidoSeleccionado)
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
            Return
        End Try

        ' Lo ya pagado, sin contar el detalle que se esta editando
        If IdDetalleEditando > 0 Then pagado -= MontoEditando

        If esVuelto Then
            If pagado <= 0D Then
                MostrarMensaje("El vuelto no puede ser el primer pago registrado.", "mensaje-error")
                Return
            End If

            Dim ultimo As Decimal = 0D
            Try
                ultimo = Math.Abs(objPago.UltimoMonto(IdPedidoSeleccionado))
            Catch ex As Exception
                MostrarMensaje(ex.Message, "mensaje-error")
                Return
            End Try

            If monto > ultimo Then
                MostrarMensaje("El vuelto no puede superar el último pago (S/. " & FormatoNumero(ultimo) & ").", "mensaje-error")
                Return
            End If

            If pagado - monto < 0D Then
                MostrarMensaje("El vuelto supera lo pagado del pedido.", "mensaje-error")
                Return
            End If
        Else
            ' Se permite superar el total solo con pago en efectivo.
            If pagado + monto > TotalPedido AndAlso Not EsEfectivo() Then
                MostrarMensaje("La cantidad supera el total del pedido. Disponible: S/. " &
                               FormatoNumero(TotalPedido - pagado), "mensaje-error")
                Return
            End If
        End If

        Dim montoGuardar As Decimal = If(esVuelto, -monto, monto)

        Try
            If IdDetalleEditando > 0 Then
                objPago.ActualizarDetalle(IdDetalleEditando, idMetodo, montoGuardar)
            Else
                objPago.RegistrarDetalle(IdPedidoSeleccionado, idMetodo, montoGuardar)
            End If

            CargarDetalles()
            MostrarMensaje("Pago registrado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub grvPagos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim idDetalle As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), idDetalle) OrElse idDetalle <= 0 Then Return

        If e.CommandName = "EditarPago" Then
            CargarDetalleParaEdicion(idDetalle)
        ElseIf e.CommandName = "EliminarPago" Then
            EliminarDetalle(idDetalle)
        End If
    End Sub

    Private Sub CargarDetalleParaEdicion(idDetalle As Integer)
        Try
            Dim dt As DataTable = objPago.ListarDetallesPorPedido(IdPedidoSeleccionado)
            For Each fila As DataRow In dt.Rows
                If Convert.ToInt32(fila("id_detalle_pago")) = idDetalle Then
                    IdDetalleEditando = idDetalle
                    MontoEditando = ADecimal(fila("monto"))

                    Dim idMetodo As Integer = Convert.ToInt32(fila("id_metodo_pago"))
                    ddlMetodoPago.SelectedValue = idMetodo.ToString()

                    ActualizarVueltoVisible()
                    chkVuelto.Checked = MontoEditando < 0D AndAlso chkVuelto.Visible
                    txtCantidad.Text = FormatoNumero(Math.Abs(MontoEditando))

                    btnGuardarPago.Text = "Actualizar pago"
                    btnCancelarEdicion.Visible = True
                    MostrarMensaje("Editando el pago seleccionado.", "mensaje-info")
                    Return
                End If
            Next
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub EliminarDetalle(idDetalle As Integer)
        Try
            objPago.EliminarDetalle(idDetalle)
            CargarDetalles()
            MostrarMensaje("Pago eliminado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs)
        LimpiarEntrada()
        MostrarMensaje("", "")
    End Sub

    ' Vuelve al modo "nuevo pago": limpia la edicion y los campos.
    Private Sub LimpiarEntrada()
        IdDetalleEditando = 0
        MontoEditando = 0D
        txtCantidad.Text = ""
        chkVuelto.Checked = False
        btnCancelarEdicion.Visible = False
        btnGuardarPago.Text = "Guardar pago"
        If ddlMetodoPago.Items.Count > 0 Then ddlMetodoPago.SelectedIndex = 0
        ActualizarVueltoVisible()
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Function TryParseMonto(texto As String, ByRef monto As Decimal) As Boolean
        texto = If(texto, "").Trim()
        If texto.Length = 0 Then Return False

        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, monto) Then
            Return True
        End If
        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, monto) Then
            Return True
        End If
        ' Acepta montos escritos como 12,50 aunque la cultura use punto decimal.
        Dim normalizado As String = texto.Replace(",", ".")
        Return Decimal.TryParse(normalizado, NumberStyles.Number, CultureInfo.InvariantCulture, monto)
    End Function

    Private Function ADecimal(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return Convert.ToDecimal(valor)
    End Function

    Private Function FormatoNumero(valor As Decimal) As String
        Return valor.ToString("0.00", CultureInfo.InvariantCulture)
    End Function
End Class

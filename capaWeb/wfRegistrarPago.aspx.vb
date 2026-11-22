Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization

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

    ' Total pagado leido en ActualizarEstado. Se usa para decidir si el
    ' formulario de nuevo pago debe quedar bloqueado.
    Private Property PagadoActual As Decimal
        Get
            If ViewState("pagadoActual") Is Nothing Then Return 0D
            Return Convert.ToDecimal(ViewState("pagadoActual"))
        End Get
        Set(value As Decimal)
            ViewState("pagadoActual") = value
        End Set
    End Property

    ' Resultado ya filtrado/ordenado de la grilla; se conserva para paginar sin
    ' volver a consultar la base de datos en cada cambio de pagina.
    Private Property PedidosFiltrados As DataTable
        Get
            Return TryCast(ViewState("pedidosFiltrados"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("pedidosFiltrados") = value
        End Set
    End Property

    ' Detalles que no se pueden tocar: el vuelto y el pago que lo origino.
    ' El vuelto se graba como detalle negativo justo despues de su pago, por eso
    ' se puede reconocer sin necesitar una relacion explicita en la BD.
    Private Property DetallesBloqueados As List(Of Integer)
        Get
            Dim lista As List(Of Integer) = TryCast(ViewState("detallesBloqueados"), List(Of Integer))
            If lista Is Nothing Then lista = New List(Of Integer)()
            Return lista
        End Get
        Set(value As List(Of Integer))
            ViewState("detallesBloqueados") = value
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
                ActualizarVistaVuelto()
                AplicarRestriccionPago()
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
        CargarPedidos(filtro, ddlEstado.SelectedValue)
    End Sub

    Private Sub CargarPedidos(filtro As String, estado As String)
        Try
            Dim dt As DataTable = objPedido.ListarDetallado()
            If dt Is Nothing Then dt = New DataTable()

            ' Filtro por número de pedido o cliente
            If Not String.IsNullOrEmpty(filtro) Then
                Dim texto As String = filtro.Trim().ToUpperInvariant()
                Dim filtrado As DataTable = dt.Clone()
                For Each fila As DataRow In dt.Rows
                    Dim coincide As Boolean =
                        fila("id_pedido").ToString().Contains(texto) OrElse
                        fila("cliente").ToString().ToUpperInvariant().Contains(texto)
                    If coincide Then filtrado.ImportRow(fila)
                Next
                dt = filtrado
            End If

            ' Filtro por estado
            If Not String.IsNullOrEmpty(estado) AndAlso dt.Columns.Contains("estado") Then
                Dim porEstado As DataTable = dt.Clone()
                For Each fila As DataRow In dt.Rows
                    If fila("estado").ToString().Equals(estado, StringComparison.OrdinalIgnoreCase) Then
                        porEstado.ImportRow(fila)
                    End If
                Next
                dt = porEstado
            End If

            ' Más reciente primero, con resolución hasta segundos. Si dos pedidos
            ' comparten fecha (incluso el mismo segundo), desempata el id.
            Dim vista As New DataView(dt)
            If dt.Columns.Contains("fecha") AndAlso dt.Columns("fecha").DataType Is GetType(DateTime) Then
                vista.Sort = "fecha DESC, id_pedido DESC"
            Else
                vista.Sort = "id_pedido DESC"
            End If
            dt = vista.ToTable()

            PedidosFiltrados = dt
            grvPedidos.PageIndex = 0
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
        CargarPedidos(txtBuscarPedido.Text, ddlEstado.SelectedValue)
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscarPedido.Text = ""
        ddlEstado.SelectedIndex = 0
        CargarPedidos("", "")
    End Sub

    Protected Sub ddlEstado_SelectedIndexChanged(sender As Object, e As EventArgs)
        CargarPedidos(txtBuscarPedido.Text, ddlEstado.SelectedValue)
    End Sub

    Protected Sub grvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvPedidos.PageIndex = e.NewPageIndex
        Dim dt As DataTable = PedidosFiltrados
        If dt Is Nothing Then dt = New DataTable()
        grvPedidos.DataSource = dt
        grvPedidos.DataBind()
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
        PagadoActual = 0D
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
        PagadoActual = 0D
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
            Dim pagoAnterior As String = ddlMetodoPago.SelectedValue
            Dim vueltoAnterior As String = ddlMetodoVuelto.SelectedValue

            ddlMetodoPago.DataSource = dt
            ddlMetodoPago.DataTextField = "nombre"
            ddlMetodoPago.DataValueField = "id_metodo_pago"
            ddlMetodoPago.DataBind()

            ddlMetodoVuelto.DataSource = dt
            ddlMetodoVuelto.DataTextField = "nombre"
            ddlMetodoVuelto.DataValueField = "id_metodo_pago"
            ddlMetodoVuelto.DataBind()

            If pagoAnterior <> "" Then ddlMetodoPago.SelectedValue = pagoAnterior
            If vueltoAnterior <> "" Then
                ddlMetodoVuelto.SelectedValue = vueltoAnterior
            Else
                SeleccionarMetodoPorNombre(ddlMetodoVuelto, "EFECTIVO")
            End If

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
            DetallesBloqueados = CalcularDetallesBloqueados(dt)
            grvPagos.DataSource = ConstruirTablaPagos(dt)
            grvPagos.DataBind()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try

        SincronizarEstadoPedido()
        ActualizarEstado()
        AplicarRestriccionPago()
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

        PagadoActual = pagado
        lblPagado.Text = "S/. " & FormatoNumero(pagado)

        If pagado <= 0D Then
            lblEstado.Text = "Pendiente"
            lblEstado.CssClass = "estado-pendiente"
        ElseIf TotalPedido > 0D AndAlso pagado >= TotalPedido Then
            lblEstado.Text = "Pagado"
            lblEstado.CssClass = "estado-pagado"
        Else
            lblEstado.Text = "Parcial"
            lblEstado.CssClass = "estado-parcial"
        End If

        ActualizarVistaVuelto()
    End Sub

    ' El pedido ya cubrio su total. Se usa para impedir nuevos ingresos de dinero,
    ' sin quitar las acciones de edicion/eliminacion de los pagos existentes.
    Private Function PedidoCompletamentePagado() As Boolean
        Return TotalPedido > 0D AndAlso PagadoActual >= TotalPedido
    End Function

    ' Con el pedido pagado solo se habilita el formulario si se esta editando
    ' un detalle existente; nunca para registrar un pago nuevo.
    Private Sub AplicarRestriccionPago()
        Dim bloquearNuevoPago As Boolean = IdPedidoSeleccionado > 0 AndAlso
                                           PedidoCompletamentePagado() AndAlso
                                           IdDetalleEditando = 0

        lblAvisoPagado.Visible = bloquearNuevoPago
        txtCantidad.Enabled = Not bloquearNuevoPago
        ddlMetodoPago.Enabled = Not bloquearNuevoPago
        btnGuardarPago.Enabled = Not bloquearNuevoPago

        If bloquearNuevoPago Then
            txtCantidad.Text = ""
            lblVuelto.Text = ""
            filaMetodoVuelto.Style("display") = "none"
            btnGuardarPago.Text = "Guardar pago"
            btnCancelarEdicion.Visible = False
        End If
    End Sub

    Protected Sub ddlMetodoPago_SelectedIndexChanged(sender As Object, e As EventArgs)
        ActualizarVistaVuelto()
    End Sub

    Protected Sub txtCantidad_TextChanged(sender As Object, e As EventArgs)
        ActualizarVistaVuelto()
    End Sub

    ' Saldo que falta cubrir. Excluye el detalle que se esta editando para no
    ' contarlo dos veces al calcular el vuelto.
    Private Function SaldoPendiente() As Decimal
        If IdPedidoSeleccionado <= 0 Then Return 0D

        Dim pagado As Decimal = 0D
        Try
            pagado = objPago.TotalPagado(IdPedidoSeleccionado)
        Catch
        End Try
        If IdDetalleEditando > 0 Then pagado -= MontoEditando

        Dim saldo As Decimal = TotalPedido - pagado
        If saldo < 0D Then saldo = 0D
        Return saldo
    End Function

    Private Function VueltoDeEntrada() As Decimal
        Dim monto As Decimal
        If Not TryParseMonto(txtCantidad.Text, monto) OrElse monto <= 0D Then Return 0D

        Dim saldo As Decimal = SaldoPendiente()
        If IdDetalleEditando > 0 OrElse monto <= saldo Then Return 0D
        Return monto - saldo
    End Function

    ' El vuelto ya no se escribe: se calcula al superar el saldo y el usuario
    ' solo elige con que metodo se registra la devolucion.
    Private Sub ActualizarVistaVuelto()
        Dim vuelto As Decimal = VueltoDeEntrada()
        Dim esDetalleVuelto As Boolean = IdDetalleEditando > 0 AndAlso MontoEditando < 0D

        lblVuelto.Text = If(vuelto > 0D,
                            "Vuelto: S/. " & FormatoNumero(vuelto),
                            If(esDetalleVuelto, "Detalle de vuelto", ""))
        filaMetodoVuelto.Style("display") = If(vuelto > 0D OrElse esDetalleVuelto, "", "none")

        If vuelto <= 0D AndAlso Not esDetalleVuelto AndAlso ddlMetodoVuelto.Items.Count > 0 Then
            ddlMetodoVuelto.SelectedIndex = 0
            SeleccionarMetodoPorNombre(ddlMetodoVuelto, "EFECTIVO")
        End If
    End Sub

    Private Sub SeleccionarMetodoPorNombre(combo As DropDownList, nombre As String)
        For Each item As ListItem In combo.Items
            If item.Text.Trim().Equals(nombre, StringComparison.OrdinalIgnoreCase) Then
                combo.SelectedValue = item.Value
                Return
            End If
        Next
    End Sub

    Private Function IdMetodoSeleccionado() As Integer
        Dim id As Integer
        If Integer.TryParse(ddlMetodoPago.SelectedValue, id) Then Return id
        Return 0
    End Function

    Private Function IdMetodoVueltoSeleccionado() As Integer
        Dim id As Integer
        If Integer.TryParse(ddlMetodoVuelto.SelectedValue, id) Then Return id
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

        ' Un pedido pagado no admite mas dinero. La edicion de un detalle
        ' existente sigue permitida, pero no puede aumentar el total pagado.
        Dim saldo As Decimal = SaldoPendiente()
        If IdDetalleEditando = 0 Then
            If saldo <= 0D Then
                PagadoActual = TotalPedido - saldo
                MostrarMensaje("El pedido ya está pagado en su totalidad. No se pueden registrar nuevos pagos; " &
                               "puede editar o eliminar los pagos registrados.", "mensaje-error")
                AplicarRestriccionPago()
                Return
            End If
        ElseIf monto > saldo + 0.01D Then
            MostrarMensaje("El monto supera el saldo del pago que se está editando. " &
                           "No puede ingresar dinero adicional.", "mensaje-error")
            Return
        End If

        Dim vuelto As Decimal = VueltoDeEntrada()
        If vuelto > 0D AndAlso IdMetodoVueltoSeleccionado() <= 0 Then
            MostrarMensaje("Seleccione el método con el que se registrará el vuelto.", "mensaje-error")
            Return
        End If

        Try
            If IdDetalleEditando > 0 Then
                ' Al editar se conserva el signo original del detalle.
                Dim esDetalleVuelto As Boolean = MontoEditando < 0D
                Dim idMetodoDestino As Integer = If(esDetalleVuelto, IdMetodoVueltoSeleccionado(), idMetodo)
                Dim montoGuardar As Decimal = If(esDetalleVuelto, -monto, monto)
                objPago.ActualizarDetalle(IdDetalleEditando, idMetodoDestino, montoGuardar)
                CargarDetalles()
                MostrarMensaje("Pago actualizado correctamente.", "mensaje-ok")
            Else
                objPago.RegistrarDetalle(IdPedidoSeleccionado, idMetodo, monto)

                If vuelto > 0D Then
                    Dim idMetodoVuelto As Integer = IdMetodoVueltoSeleccionado()
                    objPago.RegistrarDetalle(IdPedidoSeleccionado, idMetodoVuelto, -vuelto)
                    CargarDetalles()
                    Dim nombreVuelto As String = NombreMetodo(ddlMetodoVuelto, idMetodoVuelto)
                    MostrarMensaje("Pago registrado. Vuelto de S/. " & FormatoNumero(vuelto) &
                                   " registrado con " & nombreVuelto & " / vuelto.", "mensaje-ok")
                Else
                    CargarDetalles()
                    MostrarMensaje("Pago registrado correctamente.", "mensaje-ok")
                End If
            End If
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
                    If MontoEditando < 0D Then
                        ddlMetodoVuelto.SelectedValue = idMetodo.ToString()
                    Else
                        ddlMetodoPago.SelectedValue = idMetodo.ToString()
                    End If

                    txtCantidad.Text = FormatoNumero(Math.Abs(MontoEditando))
                    ActualizarVistaVuelto()
                    AplicarRestriccionPago()

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

    ' Un vuelto siempre se escribe justo despues de su pago, dentro del mismo
    ' guardado. Recorriendo el listado en su orden original (ascendente) se
    ' reconoce la pareja: el detalle negativo y el positivo inmediato anterior.
    Private Function CalcularDetallesBloqueados(dt As DataTable) As List(Of Integer)
        Dim bloqueados As New List(Of Integer)()
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return bloqueados

        Dim anteriorPositivo As Integer = 0
        Dim vista As New DataView(dt)
        vista.Sort = "id_detalle_pago ASC"
        For Each fila As DataRowView In vista
            Dim id As Integer = Convert.ToInt32(fila("id_detalle_pago"))
            Dim monto As Decimal = ADecimal(fila("monto"))
            If monto < 0D Then
                If Not bloqueados.Contains(id) Then bloqueados.Add(id)
                If anteriorPositivo > 0 AndAlso Not bloqueados.Contains(anteriorPositivo) Then
                    bloqueados.Add(anteriorPositivo)
                End If
                anteriorPositivo = 0
            Else
                anteriorPositivo = id
            End If
        Next
        Return bloqueados
    End Function

    Private Sub EliminarDetalle(idDetalle As Integer)
        Try
            ' Se vuelve a consultar para no depender solo del ViewState: un
            ' postback manipulado tampoco debe poder romper la pareja.
            Dim actual As DataTable = objPago.ListarDetallesPorPedido(IdPedidoSeleccionado)
            If CalcularDetallesBloqueados(actual).Contains(idDetalle) Then
                MostrarMensaje("Este pago forma parte de una operación con vuelto y no se puede eliminar. " &
                               "El pago y su vuelto se registran juntos.", "mensaje-error")
                Return
            End If

            objPago.EliminarDetalle(idDetalle)
            CargarDetalles()
            MostrarMensaje("Pago eliminado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs)
        LimpiarEntrada()
        AplicarRestriccionPago()
        MostrarMensaje("", "")
    End Sub

    ' Vuelve al modo "nuevo pago": limpia la edicion y los campos.
    Private Sub LimpiarEntrada()
        IdDetalleEditando = 0
        MontoEditando = 0D
        txtCantidad.Text = ""
        lblVuelto.Text = ""
        btnCancelarEdicion.Visible = False
        btnGuardarPago.Text = "Guardar pago"
        If ddlMetodoPago.Items.Count > 0 Then ddlMetodoPago.SelectedIndex = 0
        ActualizarVistaVuelto()
    End Sub

    ' Prepara la grilla: los negativos llevan "/ vuelto" en el metodo y su
    ' importe se muestra en valor absoluto (el signo se indica con el color).
    Private Function ConstruirTablaPagos(dt As DataTable) As DataTable
        Dim salida As New DataTable()
        salida.Columns.Add("id_detalle_pago", GetType(Integer))
        salida.Columns.Add("fecha", GetType(DateTime))
        salida.Columns.Add("metodo", GetType(String))
        salida.Columns.Add("monto", GetType(Decimal))
        salida.Columns.Add("monto_mostrar", GetType(String))

        If dt Is Nothing Then Return salida
        For Each fila As DataRow In dt.Rows
            Dim monto As Decimal = ADecimal(fila("monto"))
            Dim metodo As String = If(fila("metodo") Is Nothing OrElse IsDBNull(fila("metodo")),
                                      "", fila("metodo").ToString())
            Dim nueva As DataRow = salida.NewRow()
            nueva("id_detalle_pago") = Convert.ToInt32(fila("id_detalle_pago"))
            nueva("fecha") = If(fila("fecha") Is DBNull.Value,
                                DateTime.MinValue,
                                Convert.ToDateTime(fila("fecha")))
            nueva("metodo") = If(monto < 0D, metodo & " / vuelto", metodo)
            nueva("monto") = monto
            nueva("monto_mostrar") = FormatoNumero(Math.Abs(monto))
            salida.Rows.Add(nueva)
        Next

        ' El listado de negocio viene ascendente; aqui se ordena por fecha con
        ' resolución de segundos y se desempata por id (más actual primero).
        Dim vista As New DataView(salida)
        vista.Sort = "fecha DESC, id_detalle_pago DESC"
        Return vista.ToTable()
    End Function

    Protected Sub grvPagos_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return

        Dim fila As DataRowView = TryCast(e.Row.DataItem, DataRowView)
        If fila Is Nothing Then Return

        Dim esVuelto As Boolean = ADecimal(fila("monto")) < 0D
        If esVuelto Then e.Row.CssClass = (e.Row.CssClass & " fila-negativa").Trim()

        Dim id As Integer = Convert.ToInt32(fila("id_detalle_pago"))
        Dim etiqueta As Label = TryCast(e.Row.FindControl("lblBloqueado"), Label)
        If DetallesBloqueados.Contains(id) Then
            ' Editar el monto de cualquiera de las dos filas dejaría el vuelto
            ' descuadrado, así que se bloquean las dos acciones.
            Dim editar As LinkButton = TryCast(e.Row.FindControl("lnkEditarPago"), LinkButton)
            Dim eliminar As LinkButton = TryCast(e.Row.FindControl("lnkEliminarPago"), LinkButton)
            If editar IsNot Nothing Then editar.Visible = False
            If eliminar IsNot Nothing Then eliminar.Visible = False
            If etiqueta IsNot Nothing Then
                etiqueta.Text = If(esVuelto, "Vuelto", "Con vuelto")
                etiqueta.Visible = True
            End If
        ElseIf etiqueta IsNot Nothing Then
            etiqueta.Visible = False
        End If
    End Sub

    Private Function NombreMetodo(combo As DropDownList, idMetodo As Integer) As String
        For Each item As ListItem In combo.Items
            If item.Value = idMetodo.ToString() Then Return item.Text.Trim()
        Next
        Return "el método seleccionado"
    End Function

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

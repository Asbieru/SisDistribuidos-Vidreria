Imports capaNegocio

Public Class frmRegistrarPago
    Implements IFormularioTema

    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idUsuario As Integer = 0
    ' Si llega con un pedido ya elegido (por ejemplo desde frmPedidoUnitario)
    ' no se pide seleccionarlo: se muestra directo.
    Public Property idPedido As Integer = 0

    ' Id del pedido seleccionado en este formulario
    Private idPedidoSeleccionado As Integer = 0
    Private totalPedidoActual As Decimal = 0D

    ' Detalle de pago en edición (0 = ninguno)
    Private idDetalleEditando As Integer = 0
    Private montoEditando As Decimal = 0D

    Dim objPedido As New Pedido
    Dim objPago As New Pago
    Dim objMetodo As New MetodoPago

    Private Sub frmRegistrarPago_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        ResolverUsuario()

        Dim p As Paleta = Temas.Obtener(tema)
        Panel1.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
        lblSubTitulo.ForeColor = p.Texto
        msj.ForeColor = ColorAlerta(tema)

        Text = "Registrar Pago"
        lblTitulo.Text = "Registrar Pago"

        ' Desde el menú no hay pedido: se ofrece buscarlo y el proceso queda oculto
        lblSubTitulo.Visible = False
        btnSelectPedido.Visible = True
        pnlProceso.Visible = False
        lblEstado.Visible = False

        lblPagado.Text = "S/. 0.00"
        chbVuelto.Visible = False
        txtCantidad.Width = 191
        Me.Height = 139

        If idPedido > 0 Then SeleccionarPedido(idPedido)
        ActualizarBotones()
    End Sub

    ' Cuando el formulario ya se mostró (handle creado) se asegura la selección inicial
    Private Sub frmRegistrarPago_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Height = If(pnlProceso.Visible, 495, 139)
        ActualizarVueltoVisible()
        SeleccionarPrimeraFila()
        ActualizarBotones()
    End Sub

    ' Si no le pasaron el usuario, lo toma del formulario padre (MenuInicio.txtID)
    Private Sub ResolverUsuario()
        If idUsuario > 0 Then Return
        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If padre IsNot Nothing Then Integer.TryParse(padre.txtID.Text, idUsuario)
    End Sub

    ' Abre el listado en modo selección (igual que btnBuscarCliente en frmRegistrarPedido)
    Private Sub btnSelectPedido_Click(sender As Object, e As EventArgs) Handles btnSelectPedido.Click
        Using formulario As New frmListadoPedido With {.tema = tema, .idUsuario = idUsuario, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                SeleccionarPedido(formulario.idPedidoSeleccionado)
            End If
        End Using
    End Sub

    ' Carga el pedido elegido y habilita el panel de pagos
    Private Sub SeleccionarPedido(id As Integer)
        If id <= 0 Then Return
        idPedidoSeleccionado = id
        totalPedidoActual = 0D

        Try
            Dim dt As DataTable = objPedido.Obtener(id)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim fila As DataRow = dt.Rows(0)
                totalPedidoActual = ADecimal(fila("total"))
                lblSubTitulo.Text = "Pedido #" & id.ToString() & " - " & fila("cliente").ToString() &
                                    " - Total: S/." & FormatoNumero(totalPedidoActual)
            Else
                lblSubTitulo.Text = "Pedido #" & id.ToString()
            End If
        Catch ex As Exception
            msj.Text = ex.Message
            lblSubTitulo.Text = "Pedido #" & id.ToString()
        End Try

        lblSubTitulo.Visible = True
        ' Ya hay pedido: no hace falta volver a elegirlo
        If idPedido > 0 Then btnSelectPedido.Visible = False

        pnlProceso.Visible = True
        lblEstado.Visible = True
        Me.Height = 495
        CargarMetodos()
        cargarDetalles()
    End Sub

    ' Llena el combo con los métodos de pago registrados en la BD
    Private Sub CargarMetodos()
        Try
            Dim dt As DataTable = objMetodo.Listar()
            cmbMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList
            cmbMetodoPago.DisplayMember = "nombre"
            cmbMetodoPago.ValueMember = "id_metodo_pago"
            cmbMetodoPago.DataSource = dt
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                msj.Text = "No hay métodos de pago registrados."
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    ' Carga en la grilla los pagos (detalle_pago) del pedido
    Private Sub cargarDetalles()
        If idPedidoSeleccionado <= 0 Then Return
        SalirDeEdicion()
        Try
            Dim dt As DataTable = objPago.ListarDetallesPorPedido(idPedidoSeleccionado)
            tblPagos.DataSource = dt
            Encabezados(tblPagos, "fecha|Fecha", "metodo|Método", "monto|Monto")
            If tblPagos.Columns.Contains("fecha") Then
                tblPagos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            End If
            If tblPagos.Columns.Contains("monto") Then
                tblPagos.Columns("monto").DefaultCellStyle.Format = "0.00"
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
        SincronizarEstadoPedido()
        ActualizarEstado()
        SeleccionarPrimeraFila()
        ActualizarBotones()
    End Sub

    ' Deja seleccionada la primera fila para que Modificar/Eliminar queden activos
    Private Sub SeleccionarPrimeraFila()
        If tblPagos.Rows.Count = 0 Then Return
        If tblPagos.CurrentRow IsNot Nothing Then Return
        Dim primera As DataGridViewColumn = tblPagos.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
        If primera Is Nothing Then Return
        tblPagos.ClearSelection()
        tblPagos.CurrentCell = tblPagos.Rows(0).Cells(primera.Index)
        tblPagos.Rows(0).Selected = True
    End Sub

    ' Pendiente si no hay pagos, Parcial si no coincide con el total, Pagado si coincide exacto
    Private Sub ActualizarEstado()
        Dim pagado As Decimal = objPago.TotalPagado(idPedidoSeleccionado)
        lblPagado.Text = "S/. " & FormatoNumero(pagado)

        ' Pendiente si no hay pagos, Pagado solo si la suma coincide con el total.
        ' Si se excedio y aun falta el vuelto, queda Parcial (no Pagado).
        If pagado <= 0D Then
            lblEstado.Text = "Pendiente"
            lblEstado.ForeColor = Color.OrangeRed
        ElseIf totalPedidoActual > 0D AndAlso pagado = totalPedidoActual Then
            lblEstado.Text = "Pagado"
            lblEstado.ForeColor = Color.ForestGreen
        Else
            lblEstado.Text = "Parcial"
            lblEstado.ForeColor = Color.DarkOrange
        End If

        ' Si lo pagado supera el total queda un vuelto por cuadrar: chbVuelto se
        ' mantiene activo (en efectivo) hasta que la suma coincida con el total.
        Dim hayVueltoPendiente As Boolean = totalPedidoActual > 0D AndAlso pagado > totalPedidoActual
        If hayVueltoPendiente Then SeleccionarMetodoPorNombre("EFECTIVO")
        ActualizarVueltoVisible()
        chbVuelto.Checked = hayVueltoPendiente AndAlso EsEfectivo()
    End Sub

    ' chbVuelto solo tiene sentido con pagos en efectivo
    Private Sub cmbMetodoPago_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMetodoPago.SelectedIndexChanged
        ActualizarVueltoVisible()
    End Sub

    ' Muestra el checkbox solo en efectivo y ajusta el ancho de la cantidad
    Private Sub ActualizarVueltoVisible()
        Dim efectivo As Boolean = EsEfectivo()
        chbVuelto.Visible = efectivo
        txtCantidad.Width = If(efectivo, 148, 191)
        If Not efectivo Then chbVuelto.Checked = False
    End Sub

    Private Function EsEfectivo() As Boolean
        Return cmbMetodoPago IsNot Nothing AndAlso
               cmbMetodoPago.Text.Trim().Equals("EFECTIVO", StringComparison.OrdinalIgnoreCase)
    End Function

    ' Selecciona en el combo el método cuyo nombre coincide (p. ej. "EFECTIVO")
    Private Sub SeleccionarMetodoPorNombre(nombre As String)
        For i As Integer = 0 To cmbMetodoPago.Items.Count - 1
            Dim fila As DataRowView = TryCast(cmbMetodoPago.Items(i), DataRowView)
            If fila IsNot Nothing AndAlso fila("nombre").ToString().Equals(nombre, StringComparison.OrdinalIgnoreCase) Then
                cmbMetodoPago.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    ' Refleja en pedidos.estado la suma de pagos (nunca toca CANCELADO)
    Private Sub SincronizarEstadoPedido()
        If idPedidoSeleccionado <= 0 Then Return
        Try
            objPedido.ActualizarEstadoPago(idPedidoSeleccionado)
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If idPedidoSeleccionado <= 0 Then
            msj.Text = "Seleccione un pedido."
            Return
        End If

        Dim monto As Decimal
        If Not Decimal.TryParse(txtCantidad.Text.Trim(), monto) OrElse monto <= 0D Then
            msj.Text = "Ingrese una cantidad válida mayor a cero."
            Return
        End If

        Dim idMetodo As Integer = IdMetodoSeleccionado()
        If idMetodo <= 0 Then
            msj.Text = "Seleccione un método de pago."
            Return
        End If

        ' chbVuelto activo => se guarda como negativo (devolución)
        Dim esVuelto As Boolean = chbVuelto.Visible AndAlso chbVuelto.Checked

        ' Lo ya pagado, sin contar el detalle que se está editando
        Dim pagado As Decimal = objPago.TotalPagado(idPedidoSeleccionado)
        If idDetalleEditando > 0 Then pagado -= montoEditando

        If esVuelto Then
            ' El vuelto necesita un pago previo
            If pagado <= 0D Then
                msj.Text = "El vuelto no puede ser el primer pago registrado."
                Return
            End If
            ' No puede ser mayor al último registro de pago
            Dim ultimo As Decimal = Math.Abs(objPago.UltimoMonto(idPedidoSeleccionado))
            If monto > ultimo Then
                msj.Text = "El vuelto no puede superar el último pago (S/." & FormatoNumero(ultimo) & ")."
                Return
            End If
            ' No dejar lo pagado en negativo
            If pagado - monto < 0D Then
                msj.Text = "El vuelto supera lo pagado del pedido."
                Return
            End If
        Else
            ' Se permite superar el total solo con pago en efectivo (el vuelto lo devuelve)
            If pagado + monto > totalPedidoActual AndAlso Not EsEfectivo() Then
                msj.Text = "La cantidad supera el total del pedido. Disponible: S/." &
                           FormatoNumero(totalPedidoActual - pagado)
                Return
            End If
        End If

        Dim montoGuardar As Decimal = If(esVuelto, -monto, monto)

        Try
            If idDetalleEditando > 0 Then
                objPago.ActualizarDetalle(idDetalleEditando, idMetodo, montoGuardar)
            Else
                objPago.RegistrarDetalle(idPedidoSeleccionado, idMetodo, montoGuardar)
            End If
            cargarDetalles()
            msj.Text = ""
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        If tblPagos.CurrentRow Is Nothing Then Return
        Dim fila As DataGridViewRow = tblPagos.CurrentRow
        idDetalleEditando = CInt(fila.Cells("id_detalle_pago").Value)
        montoEditando = CDec(fila.Cells("monto").Value)
        SeleccionarMetodo(CInt(fila.Cells("id_metodo_pago").Value))
        ActualizarVueltoVisible()
        chbVuelto.Checked = montoEditando < 0D AndAlso chbVuelto.Visible
        txtCantidad.Text = FormatoNumero(Math.Abs(montoEditando))
        msj.Text = ""
        ActualizarBotones()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        SalirDeEdicion()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If tblPagos.CurrentRow Is Nothing Then Return
        Dim fila As DataGridViewRow = tblPagos.CurrentRow
        Dim idDetalle As Integer = CInt(fila.Cells("id_detalle_pago").Value)
        If Not Confirmar("¿Eliminar este pago del pedido?") Then Return
        Try
            objPago.EliminarDetalle(idDetalle)
            cargarDetalles()
            msj.Text = ""
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub tblPagos_SelectionChanged(sender As Object, e As EventArgs) Handles tblPagos.SelectionChanged
        ActualizarBotones()
    End Sub

    ' Al terminar de enlazar los datos, deja la primera fila lista y refresca los botones
    Private Sub tblPagos_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles tblPagos.DataBindingComplete
        SeleccionarPrimeraFila()
        ActualizarBotones()
    End Sub

    ' Vuelve al modo "nuevo pago": limpia la edición y los campos
    Private Sub SalirDeEdicion()
        idDetalleEditando = 0
        montoEditando = 0D
        txtCantidad.Clear()
        msj.Text = ""
        If cmbMetodoPago.Items.Count > 0 Then cmbMetodoPago.SelectedIndex = 0
        ActualizarBotones()
    End Sub

    ' Cancelar solo se activa al entrar en edición; Eliminar/Modificar se apagan mientras se edita.
    ' No se bloquea por estado: se puede modificar o eliminar aun si el pedido está Pagado.
    Private Sub ActualizarBotones()
        Dim hayFila As Boolean = tblPagos.CurrentRow IsNot Nothing
        btnCancelar.Enabled = idDetalleEditando > 0
        btnModificar.Enabled = hayFila AndAlso idDetalleEditando = 0
        btnEliminar.Enabled = hayFila AndAlso idDetalleEditando = 0
    End Sub

    Private Function IdMetodoSeleccionado() As Integer
        If cmbMetodoPago.SelectedValue Is Nothing OrElse TypeOf cmbMetodoPago.SelectedValue Is DataRowView Then Return 0
        Return CInt(cmbMetodoPago.SelectedValue)
    End Function

    Private Sub SeleccionarMetodo(idMetodo As Integer)
        For i As Integer = 0 To cmbMetodoPago.Items.Count - 1
            Dim fila As DataRowView = TryCast(cmbMetodoPago.Items(i), DataRowView)
            If fila IsNot Nothing AndAlso CInt(fila("id_metodo_pago")) = idMetodo Then
                cmbMetodoPago.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    Private Function ADecimal(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return Convert.ToDecimal(valor)
    End Function

    Private Function FormatoNumero(valor As Decimal) As String
        Return valor.ToString("0.00", Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Sub tblPagos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles tblPagos.CellContentClick

    End Sub
End Class

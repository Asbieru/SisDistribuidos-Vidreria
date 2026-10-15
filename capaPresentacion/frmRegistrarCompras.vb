Imports capaNegocio

Public Class frmRegistrarCompras
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    ' Variante que llega preseleccionada desde frmPedidoUnitario (boton "Comprar Ahora")
    Public Property idVarianteInicial As Integer? = Nothing
    Private objVariante As New ProductoVariante
    Private objCompra As New Compra
    Private variantes As DataTable
    Private idVarianteSeleccionada As Integer = 0
    Private detalle As DataTable
    Private idCompraActual As Integer = 0
    Private detalleEnEdicion As DataRow = Nothing
    Private Sub frmRegistrarCompras_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        btnGuardar.Enabled = False

        Try
            cboMotivo.DropDownStyle = ComboBoxStyle.DropDownList
            cboMotivo.Items.Clear()
            cboMotivo.Items.AddRange(
                New String() {"REPOSICION", "REEMPLAZO_DEFECTO"})


            nudCantidad.DecimalPlaces = 0
            nudCantidad.Minimum = 1D
            nudCantidad.Maximum = 1000000D

            nudCostoUnitario.DecimalPlaces = 2
            nudCostoUnitario.Minimum = 0
            nudCostoUnitario.Maximum = 1000000D

            nudPedidoOrigen.DecimalPlaces = 0
            nudPedidoOrigen.Minimum = 1
            nudPedidoOrigen.Maximum = Integer.MaxValue

            txtProveedor.MaxLength = 150

            detalle = New DataTable()
            detalle.Columns.Add("id_variante", GetType(Integer))
            detalle.Columns.Add("producto", GetType(String))
            detalle.Columns.Add("cantidad", GetType(Decimal))
            detalle.Columns.Add("costo_unitario", GetType(Decimal))
            detalle.Columns.Add("subtotal", GetType(Decimal))

            dgvDetalle.ReadOnly = True
            dgvDetalle.AllowUserToAddRows = False
            dgvDetalle.AllowUserToDeleteRows = False
            dgvDetalle.MultiSelect = False
            dgvDetalle.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect
            dgvDetalle.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill

            dgvDetalle.DataSource = detalle

            dgvDetalle.Columns("id_variante").Visible = False
            dgvDetalle.Columns("producto").HeaderText = "Producto / variante"
            dgvDetalle.Columns("cantidad").HeaderText = "Cantidad"
            dgvDetalle.Columns("costo_unitario").HeaderText = "Costo unitario"
            dgvDetalle.Columns("subtotal").HeaderText = "Subtotal"
            dgvDetalle.Columns("producto").FillWeight = 200

            For Each nombre As String In
                New String() {"costo_unitario", "subtotal"}

                dgvDetalle.Columns(nombre).DefaultCellStyle.Format = "N2"
            Next

            dgvDetalle.Columns("cantidad").DefaultCellStyle.Format = "N0"

            cargarVariantes()
            limpiar()
            btnGuardar.Enabled = True

            If idVarianteInicial.HasValue AndAlso idVarianteInicial.Value > 0 Then
                If seleccionarVariante(idVarianteInicial.Value) Then nudCantidad.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cargarVariantes()
        variantes = objVariante.Listar()
        variantes.Columns.Add("descripcion", GetType(String))

        For Each fila As DataRow In variantes.Rows
            Dim descripcion As String =
                fila("nombre").ToString() &
                " | Variante " & fila("id_variante").ToString()

            If Not IsDBNull(fila("espesor_cm")) Then
                descripcion &= " | Espesor: " &
                    fila("espesor_cm").ToString() & " cm"
            End If

            If Not IsDBNull(fila("medida_cm")) Then
                descripcion &= " | Medida: " &
                    fila("medida_cm").ToString() & " cm"
            End If

            descripcion &= " | " & fila("unidad_costo").ToString()
            fila("descripcion") = descripcion
        Next


    End Sub

    Private Function seleccionarVariante(id As Integer) As Boolean
        If variantes Is Nothing Then Return False
        Dim filas As DataRow() = variantes.Select("id_variante = " & id.ToString())
        If filas.Length = 0 Then Return False
        Dim fila As DataRow = filas(0)
        Dim costo As Decimal = If(IsDBNull(fila("precio_unitario")), 0D, CDec(fila("precio_unitario")))
        nudCostoUnitario.Maximum = Math.Max(nudCostoUnitario.Maximum, costo)
        nudCostoUnitario.Value = Math.Max(nudCostoUnitario.Minimum, costo)
        idVarianteSeleccionada = id
        txtProductoSeleccionado.Text = fila("descripcion").ToString()
        Return True
    End Function

    Private Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        Try
            cargarVariantes()
            Using selector As New Form With {
                .Text = "Seleccionar producto para compra",
                .Size = New Size(950, 500),
                .MinimumSize = New Size(700, 350),
                .StartPosition = FormStartPosition.CenterParent,
                .MinimizeBox = False, .MaximizeBox = False}

                Dim tabla As New DataGridView With {
                    .Dock = DockStyle.Fill, .ReadOnly = True,
                    .AllowUserToAddRows = False, .AllowUserToDeleteRows = False,
                    .MultiSelect = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    .AutoGenerateColumns = False, .DataSource = variantes}
                Dim campos As String() = {"nombre", "categoria", "tipo", "color", "espesor_cm", "medida_cm", "unidad_costo", "precio_unitario"}
                Dim titulos As String() = {"Producto", "Categoría", "Tipo", "Color", "Espesor (cm)", "Medida (cm)", "Unidad", "Costo unitario"}
                For i As Integer = 0 To campos.Length - 1
                    tabla.Columns.Add(New DataGridViewTextBoxColumn With {
                        .Name = campos(i), .DataPropertyName = campos(i), .HeaderText = titulos(i)})
                Next
                tabla.Columns("precio_unitario").DefaultCellStyle.Format = "N2"
                tabla.Columns("nombre").FillWeight = 180

                Dim acciones As New FlowLayoutPanel With {
                    .Dock = DockStyle.Bottom, .Height = 45, .FlowDirection = FlowDirection.RightToLeft,
                    .Padding = New Padding(5)}
                Dim cancelar As New Button With {.Text = "CANCELAR", .AutoSize = True, .DialogResult = DialogResult.Cancel}
                Dim agregar As New Button With {.Text = "AGREGAR", .AutoSize = True}
                AddHandler agregar.Click,
                    Sub()
                        If tabla.CurrentRow Is Nothing Then
                            MessageBox.Show(selector, "Seleccione un producto.")
                            Return
                        End If
                        Dim fila As DataRowView = TryCast(tabla.CurrentRow.DataBoundItem, DataRowView)
                        If fila IsNot Nothing AndAlso seleccionarVariante(CInt(fila("id_variante"))) Then
                            selector.DialogResult = DialogResult.OK
                        End If
                    End Sub
                acciones.Controls.Add(cancelar)
                acciones.Controls.Add(agregar)
                selector.Controls.Add(tabla)
                selector.Controls.Add(acciones)
                selector.CancelButton = cancelar
                AplicarTema(selector, tema)
                If selector.ShowDialog(Me) = DialogResult.OK Then nudCantidad.Focus()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub limpiar()
        restablecerEntradaProducto()
        btnGuardar.Text = "GUARDAR"
        idCompraActual = 0
        txtProveedor.Clear()
        dtpFecha.Value = Today
        cboMotivo.SelectedIndex = 0
        nudPedidoOrigen.Value = 1
        nudPedidoOrigen.Enabled = False
        idVarianteSeleccionada = 0
        txtProductoSeleccionado.Clear()
        nudCantidad.Value = 1
        nudCostoUnitario.Value = 0

        If detalle IsNot Nothing Then detalle.Clear()

        lblModo.Text = "NUEVA COMPRA"
        actualizarTotal()
        txtProveedor.Focus()
    End Sub

    Private Sub cboMotivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMotivo.SelectedIndexChanged
        nudPedidoOrigen.Enabled =
            (cboMotivo.Text = "REEMPLAZO_DEFECTO")
    End Sub

    Private Sub btnAgregarDetalle_Click(sender As Object, e As EventArgs) Handles btnAgregarDetalle.Click
        If detalle Is Nothing Then Return

        If idVarianteSeleccionada = 0 Then
            MessageBox.Show("Seleccione un producto.")
            Return
        End If

        If nudCantidad.Value < 1D OrElse nudCantidad.Value <> Decimal.Truncate(nudCantidad.Value) Then
            MessageBox.Show("La cantidad debe ser un número entero desde 1.")
            Return
        End If

        If nudCostoUnitario.Value <= 0 Then
            MessageBox.Show("La cantidad y el costo deben ser mayores a cero.")
            Return
        End If

        Dim id As Integer = idVarianteSeleccionada
        Dim totalOtros As Decimal = 0

        For Each fila As DataRow In detalle.Rows
            If Object.ReferenceEquals(fila, detalleEnEdicion) Then Continue For

            If CInt(fila("id_variante")) = id Then
                MessageBox.Show("Esta variante ya está en la tabla.")
                Return
            End If

            totalOtros += CDec(fila("subtotal"))
        Next

        Dim subtotal As Decimal = Decimal.Round(
        nudCantidad.Value * nudCostoUnitario.Value,
        2, MidpointRounding.AwayFromZero)

        If totalOtros + subtotal > 99999999.99D Then
            MessageBox.Show("El total supera el límite de la base de datos.")
            Return
        End If

        If detalleEnEdicion Is Nothing Then
            detalle.Rows.Add(
            id, txtProductoSeleccionado.Text, nudCantidad.Value,
            nudCostoUnitario.Value, subtotal)
        Else
            detalleEnEdicion("id_variante") = id
            detalleEnEdicion("producto") = txtProductoSeleccionado.Text
            detalleEnEdicion("cantidad") = nudCantidad.Value
            detalleEnEdicion("costo_unitario") = nudCostoUnitario.Value
            detalleEnEdicion("subtotal") = subtotal
        End If

        actualizarTotal()
        restablecerEntradaProducto()
    End Sub

    Private Sub btnQuitarDetalle_Click(sender As Object, e As EventArgs) Handles btnQuitarDetalle.Click
        If detalle Is Nothing OrElse dgvDetalle.CurrentRow Is Nothing Then Return

        Dim seleccion As DataRowView =
        TryCast(dgvDetalle.CurrentRow.DataBoundItem, DataRowView)

        If seleccion Is Nothing Then Return

        If Object.ReferenceEquals(seleccion.Row, detalleEnEdicion) Then
            restablecerEntradaProducto()
        End If

        detalle.Rows.Remove(seleccion.Row)
        actualizarTotal()
    End Sub

    Private Sub actualizarTotal()
        Dim total As Decimal = 0

        If detalle IsNot Nothing Then
            For Each fila As DataRow In detalle.Rows
                total += CDec(fila("subtotal"))
            Next
        End If

        lblTotal.Text = "TOTAL: S/ " & total.ToString("N2")
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If detalle Is Nothing Then Return

        If detalleEnEdicion IsNot Nothing Then
            MessageBox.Show(
            "Pulse ACTUALIZAR PRODUCTO para aplicar la cantidad y el costo " &
            "antes de guardar la compra.",
            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnGuardar.Enabled = False

        Try
            Dim pedido As Integer? = Nothing

            If cboMotivo.Text = "REEMPLAZO_DEFECTO" Then
                pedido = CInt(nudPedidoOrigen.Value)
            End If

            Dim modificando As Boolean = (idCompraActual > 0)

            Dim id As Integer = objCompra.GuardarCompra(
            txtProveedor.Text,
            dtpFecha.Value.Date,
            cboMotivo.Text,
            pedido,
            detalle,
            idCompraActual)

            limpiar()

            For Each formulario As Form In Application.OpenForms
                If TypeOf formulario Is frmListadoCompras Then
                    DirectCast(formulario, frmListadoCompras).cargarLista()
                End If
            Next

            Dim mensaje As String =
            If(modificando,
               "Compra modificada correctamente.",
               "Compra registrada correctamente.")

            MessageBox.Show(
            mensaje & " ID: " & id.ToString(),
            "Mensaje", MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show(
            ex.Message, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        If detalle Is Nothing Then Return

        If MessageBox.Show(
            "¿Limpiar los datos de la compra?", "Confirmar",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        limpiar()
    End Sub

    Private Sub btnVerLista_Click(sender As Object, e As EventArgs) Handles btnVerLista.Click
        Dim listado As frmListadoCompras = Nothing

        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is frmListadoCompras Then
                listado = DirectCast(formulario, frmListadoCompras)
                Exit For
            End If
        Next

        If listado Is Nothing Then
            listado = New frmListadoCompras With {.tema = tema}
            listado.Show()
        Else
            listado.cargarLista()
            listado.WindowState = FormWindowState.Normal
            listado.BringToFront()
        End If
    End Sub

    Public Sub cargarCompra(id As Integer)
        If detalle Is Nothing Then
            MessageBox.Show("El formulario no terminó de cargar.")
            Return
        End If

        Try
            Dim cabecera As DataTable = objCompra.ObtenerCompra(id)

            If cabecera.Rows.Count = 0 Then
                MessageBox.Show("La compra seleccionada ya no existe.",
                                "Aviso", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
                Return
            End If

            Dim productos As DataTable =
                objCompra.ObtenerDetalleParaEditar(id)

            Dim fila As DataRow = cabecera.Rows(0)

            txtProveedor.Text = fila("proveedor").ToString()
            dtpFecha.Value = CDate(fila("fecha"))
            cboMotivo.SelectedItem = fila("motivo").ToString()

            nudPedidoOrigen.Value = 1

            If Not IsDBNull(fila("id_pedido_origen")) Then
                nudPedidoOrigen.Value = CDec(fila("id_pedido_origen"))
            End If

            detalle.Clear()

            For Each producto As DataRow In productos.Rows
                detalle.Rows.Add(
                    CInt(producto("id_variante")),
                    producto("producto").ToString(),
                    CDec(producto("cantidad")),
                    CDec(producto("costo_unitario")),
                    CDec(producto("subtotal")))
            Next

            idCompraActual = id
            restablecerEntradaProducto()
            btnGuardar.Text = "GUARDAR CAMBIOS"
            lblModo.Text = "MODIFICAR COMPRA - ID " & id.ToString()
            idVarianteSeleccionada = 0
            txtProductoSeleccionado.Clear()
            nudCantidad.Value = 1
            nudCostoUnitario.Value = 0
            actualizarTotal()
            txtProveedor.Focus()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub restablecerEntradaProducto()
        detalleEnEdicion = Nothing
        idVarianteSeleccionada = 0
        txtProductoSeleccionado.Clear()
        nudCantidad.Value = 1
        nudCostoUnitario.Value = 0
        btnAgregarDetalle.Text = "AGREGAR"
    End Sub

    Private Sub dgvDetalle_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDetalle.CellDoubleClick
        If e.RowIndex < 0 Then Return

        Dim seleccion As DataRowView = TryCast(
            dgvDetalle.Rows(e.RowIndex).DataBoundItem, DataRowView)

        If seleccion Is Nothing Then Return

        Dim fila As DataRow = seleccion.Row
        Dim cantidad As Decimal = CDec(fila("cantidad"))
        Dim costo As Decimal = CDec(fila("costo_unitario"))

        ' Permitir cargar valores existentes que superen el máximo inicial.
        nudCantidad.Maximum = Math.Max(nudCantidad.Maximum, cantidad)
        nudCostoUnitario.Maximum = Math.Max(nudCostoUnitario.Maximum, costo)

        If Not seleccionarVariante(CInt(fila("id_variante"))) Then
            MessageBox.Show("No se encontró la variante de este producto.")
            Return
        End If

        detalleEnEdicion = fila
        nudCantidad.Value = cantidad
        nudCostoUnitario.Value = costo
        btnAgregarDetalle.Text = "ACTUALIZAR PRODUCTO"
        nudCantidad.Focus()
    End Sub
End Class

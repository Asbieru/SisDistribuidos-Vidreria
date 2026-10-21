Imports capaNegocio

Public Class frmRegistrarCompras
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    ' Variante que llega preseleccionada desde frmPedidoUnitario (boton "Comprar Ahora")
    Public Property idVarianteInicial As Integer? = Nothing
    ' Usuario conectado, para dejar rastro en movimientos_inventario
    Public Property idUsuario As Integer? = Nothing
    Private objVariante As New ProductoVariante
    Private objCompra As New Compra
    Private variantes As DataTable
    Private idVarianteSeleccionada As Integer = 0
    Private unidadSeleccionada As String = ""
    Private detalle As DataTable
    Private idCompraActual As Integer = 0
    Private detalleEnEdicion As DataRow = Nothing
    Private estadoGuardado As String = Nothing
    Private idProveedorSeleccionado As Integer = 0

    Private Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click
        Using formulario As New frmListadoProveedores With {.tema = tema, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                idProveedorSeleccionado = formulario.idProveedorSeleccionado
                txtProveedor.Text = formulario.nombreProveedorSeleccionado
            End If
        End Using
    End Sub

    Private Function estadoCompra() As String
        Dim estado As New System.Text.StringBuilder()
        For Each valor As String In {idProveedorSeleccionado.ToString(), txtProveedor.Text, dtpFecha.Value.Date.Ticks.ToString(),
            cboMotivo.Text, If(nudPedidoOrigen.Enabled, nudPedidoOrigen.Value.ToString(), ""),
            If(detalle Is Nothing, "", detalle.Rows.Count.ToString())}
            estado.Append(valor.Length).Append(":"c).Append(valor)
        Next
        If detalle IsNot Nothing Then
            For Each fila As DataRow In detalle.Rows
                For Each valor As Object In fila.ItemArray
                    Dim texto As String = Convert.ToString(valor, Globalization.CultureInfo.InvariantCulture)
                    estado.Append(texto.Length).Append(":"c).Append(texto)
                Next
            Next
        End If
        Return estado.ToString()
    End Function

    Private Sub frmRegistrarCompras_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If estadoGuardado Is Nothing Then Return
        Dim entradaPendiente As Boolean = idVarianteSeleccionada > 0 OrElse
            detalleEnEdicion IsNot Nothing OrElse nudCantidad.Value <> 1D OrElse nudCostoUnitario.Value <> 0D
        If Not entradaPendiente AndAlso estadoCompra() = estadoGuardado Then Return
        If MessageBox.Show(Me, "Hay datos sin guardar. ¿Desea cerrar y descartarlos?",
            "Cerrar compra", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then e.Cancel = True
    End Sub
    Private Sub frmRegistrarCompras_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        btnGuardar.Enabled = False

        Try
            cboMotivo.DropDownStyle = ComboBoxStyle.DropDownList
            cboMotivo.Items.Clear()
            cboMotivo.Items.AddRange(
                New String() {"REPOSICION", "REEMPLAZO_DEFECTO"})


            ' Cantidad = conteo entero (piezas idénticas o cajas); la medida
            ' derivada (m2 / pie2 / cm) se calcula sola y viaja en otra columna
            nudCantidad.DecimalPlaces = 0
            nudCantidad.Minimum = 1D
            nudCantidad.Maximum = 1000000D

            nudCostoUnitario.DecimalPlaces = 3
            nudCostoUnitario.Minimum = 0
            nudCostoUnitario.Maximum = 1000000D

            numAncho.DecimalPlaces = 3
            numAncho.Minimum = 0D
            numAncho.Maximum = 1000000D

            numAlto.DecimalPlaces = 3
            numAlto.Minimum = 0D
            numAlto.Maximum = 1000000D

            nudPedidoOrigen.DecimalPlaces = 0
            nudPedidoOrigen.Minimum = 1
            nudPedidoOrigen.Maximum = Integer.MaxValue

            txtProveedor.MaxLength = 150

            detalle = New DataTable()
            detalle.Columns.Add("id_variante", GetType(Integer))
            detalle.Columns.Add("producto", GetType(String))
            detalle.Columns.Add("cantidad", GetType(Decimal))
            detalle.Columns.Add("piezas", GetType(Integer))
            detalle.Columns.Add("costo_unitario", GetType(Decimal))
            detalle.Columns.Add("subtotal", GetType(Decimal))
            detalle.Columns.Add("unidad", GetType(String))
            detalle.Columns.Add("ancho_cm", GetType(Decimal))
            detalle.Columns.Add("alto_cm", GetType(Decimal))
            detalle.Columns.Add("largo_cm", GetType(Decimal))
            detalle.Columns.Add("medida", GetType(String))

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
            dgvDetalle.Columns("unidad").HeaderText = "Unidad"
            dgvDetalle.Columns("ancho_cm").Visible = False
            dgvDetalle.Columns("alto_cm").Visible = False
            dgvDetalle.Columns("largo_cm").Visible = False
            dgvDetalle.Columns("producto").HeaderText = "Producto / variante"
            dgvDetalle.Columns("medida").HeaderText = "Medida"
            dgvDetalle.Columns("piezas").HeaderText = "Conteo"
            dgvDetalle.Columns("piezas").ToolTipText = "Número entero de piezas, cajas o unidades compradas."
            dgvDetalle.Columns("cantidad").ToolTipText = "Área, largo o cantidad facturada, según la unidad de costo."
            dgvDetalle.Columns("cantidad").HeaderText = "Cantidad facturada"
            dgvDetalle.Columns("costo_unitario").HeaderText = "Costo unitario"
            dgvDetalle.Columns("subtotal").HeaderText = "Subtotal"
            dgvDetalle.Columns("producto").FillWeight = 170
            dgvDetalle.Columns("medida").FillWeight = 100
            dgvDetalle.Columns("piezas").FillWeight = 45
            dgvDetalle.Columns("producto").MinimumWidth = 210
            dgvDetalle.Columns("cantidad").MinimumWidth = 115
            dgvDetalle.Columns("piezas").MinimumWidth = 70
            dgvDetalle.Columns("costo_unitario").MinimumWidth = 105
            dgvDetalle.Columns("subtotal").MinimumWidth = 100
            dgvDetalle.Columns("unidad").MinimumWidth = 70
            dgvDetalle.Columns("medida").MinimumWidth = 110

            For Each nombre As String In
                New String() {"costo_unitario", "subtotal", "cantidad"}

                dgvDetalle.Columns(nombre).DefaultCellStyle.Format = "N3"
            Next

            dgvDetalle.Columns("piezas").DefaultCellStyle.Format = "N0"

            cargarVariantes()
            limpiar()
            btnGuardar.Enabled = True

            If idVarianteInicial.HasValue AndAlso idVarianteInicial.Value > 0 Then
                If seleccionarVariante(idVarianteInicial.Value) Then EnfocarMedida()
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
        AplicarModoMedida(If(IsDBNull(fila("unidad_costo")), "", fila("unidad_costo").ToString()))
        Return True
    End Function

    ' Muestra los controles que corresponden a la unidad de la variante elegida
    Private Sub AplicarModoMedida(unidad As String)
        unidadSeleccionada = If(unidad, "").Trim().ToUpper()
        Dim esVidrio As Boolean = unidadSeleccionada = "CM2" OrElse unidadSeleccionada = "PIE2"
        Dim esAluminio As Boolean = unidadSeleccionada = "CM_LINEAL"

        numAncho.Visible = esVidrio
        lblAncho.Visible = esVidrio
        numAlto.Visible = esVidrio OrElse esAluminio
        lblAlto.Visible = esVidrio OrElse esAluminio
        lblAlto.Text = If(esAluminio, "Largo:", "Alto:")

        ' La cantidad siempre se ve: para piezas cuenta cuantas identicas
        ' se compran; para cajas/unidades es el numero de cajas
        lblCantidad.Text = If(esVidrio OrElse esAluminio, "Piezas:", "Cantidad:")
        nudCantidad.Visible = True
        lblCantidad.Visible = True
        Select Case unidadSeleccionada
            Case "CM2" : lblCosto.Text = "Costo / cm²:"
            Case "PIE2" : lblCosto.Text = "Costo / pie²:"
            Case "CM_LINEAL" : lblCosto.Text = "Costo / cm:"
            Case "CAJA" : lblCosto.Text = "Costo / caja:"
            Case "UNIDAD" : lblCosto.Text = "Costo / unidad:"
            Case Else : lblCosto.Text = "Costo unitario:"
        End Select
    End Sub

    ' Enfoca el primer control de entrada segun el modo activo
    Private Sub EnfocarMedida()
        If numAncho.Visible Then
            numAncho.Focus()
        ElseIf numAlto.Visible Then
            numAlto.Focus()
        Else
            nudCantidad.Focus()
        End If
    End Sub

    Private Function UnidadDeVariante(id As Integer) As String
        If variantes Is Nothing Then Return ""
        Dim filas As DataRow() = variantes.Select("id_variante = " & id.ToString())
        If filas.Length = 0 OrElse IsDBNull(filas(0)("unidad_costo")) Then Return ""
        Return filas(0)("unidad_costo").ToString()
    End Function

    Private Shared Function ValorNud(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return CDec(valor)
    End Function

    ' Carga una variante y su medida desde el pedido (boton "Comprar Ahora").
    ' Reemplaza la entrada pendiente sin tocar el detalle ya agregado.
    Public Sub CargarDesdePedido(idVariante As Integer, ancho As Decimal?,
                                 alto As Decimal?, largo As Decimal?,
                                 cajas As Integer?)
        If detalle Is Nothing Then Return
        If Not seleccionarVariante(idVariante) Then Return

        detalleEnEdicion = Nothing
        btnAgregarDetalle.Text = "AGREGAR"
        btnCancelarEdicion.Visible = False

        If unidadSeleccionada = "CM2" OrElse unidadSeleccionada = "PIE2" Then
            numAncho.Value = LimitarNud(numAncho, ancho)
            numAlto.Value = LimitarNud(numAlto, alto)
            nudCantidad.Value = 1D
        ElseIf unidadSeleccionada = "CM_LINEAL" Then
            numAlto.Value = LimitarNud(numAlto, largo)
            nudCantidad.Value = 1D
        ElseIf cajas.HasValue Then
            nudCantidad.Value = Math.Min(nudCantidad.Maximum, Math.Max(nudCantidad.Minimum, cajas.Value))
        End If

        EnfocarMedida()
    End Sub

    Private Shared Function LimitarNud(nud As NumericUpDown, valor As Decimal?) As Decimal
        If Not valor.HasValue OrElse valor.Value <= 0D Then
            Return Math.Max(nud.Minimum, 1D)
        End If
        Return Math.Min(nud.Maximum, Math.Max(nud.Minimum, valor.Value))
    End Function

    Private Sub btnBuscarProducto_Click(sender As Object, e As EventArgs) Handles btnBuscarProducto.Click
        Try
            cargarVariantes()
            Using selector As New Form With {
                .Text = "Seleccionar producto para compra",
                .Font = New Font("Microsoft Sans Serif", 9.75F),
                .Size = New Size(950, 500),
                .MinimumSize = New Size(700, 350),
                .StartPosition = FormStartPosition.CenterParent,
                .MinimizeBox = False, .MaximizeBox = False}

                Using vista As New DataView(variantes)
                    Dim busqueda As New Panel With {.Dock = DockStyle.Top, .Height = 50, .Width = selector.ClientSize.Width}
                    Dim etiqueta As New Label With {.Text = "Buscar producto:", .AutoSize = True, .Location = New Point(10, 13)}
                    Dim buscar As New TextBox With {.Location = New Point(140, 9), .Width = selector.ClientSize.Width - 164,
                    .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right}
                    busqueda.Controls.Add(etiqueta)
                    busqueda.Controls.Add(buscar)
                    AddHandler buscar.TextChanged,
                    Sub()
                        Dim texto As String = buscar.Text.Trim().Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
                        vista.RowFilter = If(texto = "", "", "descripcion LIKE '%" & texto &
                            "%' OR categoria LIKE '%" & texto & "%' OR tipo LIKE '%" & texto &
                            "%' OR color LIKE '%" & texto & "%'")
                    End Sub

                    Dim tabla As New DataGridView With {
                    .Dock = DockStyle.Fill, .ReadOnly = True,
                    .AllowUserToAddRows = False, .AllowUserToDeleteRows = False,
                    .MultiSelect = False, .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    .AutoGenerateColumns = False, .DataSource = vista}
                    Dim campos As String() = {"nombre", "categoria", "tipo", "color", "espesor_cm", "medida_cm", "unidad_costo", "precio_unitario"}
                    Dim titulos As String() = {"Producto", "Categoría", "Tipo", "Color", "Espesor (cm)", "Medida (cm)", "Unidad", "Costo unitario"}
                    For i As Integer = 0 To campos.Length - 1
                        tabla.Columns.Add(New DataGridViewTextBoxColumn With {
                        .Name = campos(i), .DataPropertyName = campos(i), .HeaderText = titulos(i)})
                    Next
                    tabla.Columns("precio_unitario").DefaultCellStyle.Format = "N3"
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
                    selector.Controls.Add(busqueda)
                    selector.CancelButton = cancelar
                    selector.ActiveControl = buscar
                    AplicarTema(selector, tema)
                    If selector.ShowDialog(Me) = DialogResult.OK Then EnfocarMedida()
                End Using
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
        idProveedorSeleccionado = 0
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
        estadoGuardado = estadoCompra()
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

        If nudCostoUnitario.Value <= 0 Then
            MessageBox.Show("El costo debe ser mayor a cero.")
            Return
        End If

        Dim esVidrio As Boolean = unidadSeleccionada = "CM2" OrElse unidadSeleccionada = "PIE2"
        Dim esAluminio As Boolean = unidadSeleccionada = "CM_LINEAL"
        Dim esPieza As Boolean = esVidrio OrElse esAluminio

        ' Para piezas: cantidad = cuantas identicas se compran (entero >= 1).
        ' La medida unitaria (ancho x alto, o largo) describe a CADA pieza.
        If nudCantidad.Value < 1D OrElse nudCantidad.Value <> Decimal.Truncate(nudCantidad.Value) Then
            MessageBox.Show("La cantidad debe ser un número entero desde 1.")
            Return
        End If
        Dim piezas As Integer = CInt(nudCantidad.Value)

        ' Medidas segun el tipo: vidrio (ancho x alto), aluminio (largo), accesorios (cajas)
        Dim cantidad As Decimal
        Dim ancho As Decimal? = Nothing
        Dim alto As Decimal? = Nothing
        Dim largo As Decimal? = Nothing

        If esVidrio Then
            If numAncho.Value <= 0D OrElse numAlto.Value <= 0D Then
                MessageBox.Show("Ingrese el ancho y el alto del vidrio (mayores a cero).")
                Return
            End If
            ancho = numAncho.Value
            alto = numAlto.Value

        ElseIf esAluminio Then
            If numAlto.Value <= 0D Then
                MessageBox.Show("Ingrese el largo del aluminio (mayor a cero).")
                Return
            End If
            largo = numAlto.Value

        End If

        Dim id As Integer = idVarianteSeleccionada
        Dim totalOtros As Decimal = 0

        For Each fila As DataRow In detalle.Rows
            If Object.ReferenceEquals(fila, detalleEnEdicion) Then Continue For

            ' Cada pieza es una fila con su propia medida; solo se evita repetir accesorios
            If Not esPieza AndAlso CInt(fila("id_variante")) = id Then
                MessageBox.Show("Esta variante ya está en la tabla.")
                Return
            End If

            totalOtros += CDec(fila("subtotal"))
        Next

        Dim importes As Compra.ImportesDetalle
        Try
            importes = Compra.CalcularImportes(unidadSeleccionada, piezas, nudCostoUnitario.Value, ancho, alto, largo)
        Catch ex As ArgumentException
            MessageBox.Show(Me, ex.Message, "Compra", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try
        cantidad = importes.Cantidad
        Dim subtotal As Decimal = importes.Subtotal

        If totalOtros + subtotal > 99999999.999D Then
            MessageBox.Show("El total supera el límite de la base de datos.")
            Return
        End If

        Dim medida As String = ""
        If esPieza Then medida = TextoMedida(ancho, alto, largo)

        If detalleEnEdicion Is Nothing Then
            detalle.Rows.Add(
            id, txtProductoSeleccionado.Text, cantidad,
            piezas, nudCostoUnitario.Value, subtotal,
            unidadSeleccionada, ADBNull(ancho), ADBNull(alto), ADBNull(largo), medida)
        Else
            detalleEnEdicion("id_variante") = id
            detalleEnEdicion("producto") = txtProductoSeleccionado.Text
            detalleEnEdicion("cantidad") = cantidad
            detalleEnEdicion("piezas") = piezas
            detalleEnEdicion("costo_unitario") = nudCostoUnitario.Value
            detalleEnEdicion("subtotal") = subtotal
            detalleEnEdicion("unidad") = unidadSeleccionada
            detalleEnEdicion("ancho_cm") = ADBNull(ancho)
            detalleEnEdicion("alto_cm") = ADBNull(alto)
            detalleEnEdicion("largo_cm") = ADBNull(largo)
            detalleEnEdicion("medida") = medida
        End If

        actualizarTotal()
        restablecerEntradaProducto()
    End Sub

    ' Empaqueta una medida opcional para una columna del DataTable (Nothing -> DBNull)
    Private Shared Function ADBNull(valor As Decimal?) As Object
        Return If(valor.HasValue, CType(valor.Value, Object), DBNull.Value)
    End Function

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

        lblTotal.Text = "TOTAL: S/ " & total.ToString("N3")
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If detalle Is Nothing Then Return
        If idProveedorSeleccionado <= 0 Then
            MessageBox.Show(Me, "Seleccione un proveedor registrado con BUSCAR.", "Proveedor", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnBuscarProveedor.Focus()
            Return
        End If

        If detalleEnEdicion IsNot Nothing Then
            MessageBox.Show(
            "Pulse ACTUALIZAR PRODUCTO para aplicar la cantidad y el costo " &
            "antes de guardar la compra.",
            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If idVarianteSeleccionada > 0 Then
            MessageBox.Show(Me, "Hay un producto seleccionado pendiente. Pulse AGREGAR antes de guardar la compra.",
                "Producto pendiente", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
            idCompraActual,
            idUsuario,
            idProveedorSeleccionado)

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
            idProveedorSeleccionado = If(IsDBNull(fila("id_proveedor")), 0, CInt(fila("id_proveedor")))
            dtpFecha.Value = CDate(fila("fecha"))
            cboMotivo.SelectedItem = fila("motivo").ToString()

            nudPedidoOrigen.Value = 1

            If Not IsDBNull(fila("id_pedido_origen")) Then
                nudPedidoOrigen.Value = CDec(fila("id_pedido_origen"))
            End If

            detalle.Clear()

            For Each producto As DataRow In productos.Rows
                Dim idVar As Integer = CInt(producto("id_variante"))
                Dim unidad As String = UnidadDeVariante(idVar)
                Dim esPieza As Boolean = unidad = "CM2" OrElse unidad = "PIE2" OrElse unidad = "CM_LINEAL"
                Dim piezas As Integer = 1
                If productos.Columns.Contains("piezas") AndAlso Not IsDBNull(producto("piezas")) Then
                    piezas = Math.Max(1, CInt(producto("piezas")))
                End If
                If Not esPieza Then piezas = CInt(producto("cantidad"))
                Dim ancho As Object = If(productos.Columns.Contains("ancho_cm"), producto("ancho_cm"), DBNull.Value)
                Dim alto As Object = If(productos.Columns.Contains("alto_cm"), producto("alto_cm"), DBNull.Value)
                Dim largo As Object = If(productos.Columns.Contains("largo_cm"), producto("largo_cm"), DBNull.Value)
                Dim medida As String = ""
                If esPieza Then medida = TextoMedida(ancho, alto, largo)

                detalle.Rows.Add(
                    idVar,
                    producto("producto").ToString(),
                    CDec(producto("cantidad")),
                    piezas,
                    CDec(producto("costo_unitario")),
                    CDec(producto("subtotal")),
                    unidad,
                    ancho, alto, largo,
                    medida)
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
            estadoGuardado = estadoCompra()

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
        numAncho.Value = 1D
        numAlto.Value = 1D
        AplicarModoMedida("")
        btnAgregarDetalle.Text = "AGREGAR"
        btnCancelarEdicion.Visible = False
    End Sub

    Private Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs) Handles btnCancelarEdicion.Click
        restablecerEntradaProducto()
    End Sub

    Private Sub dgvDetalle_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDetalle.CellDoubleClick
        If e.RowIndex < 0 Then Return

        Dim seleccion As DataRowView = TryCast(
            dgvDetalle.Rows(e.RowIndex).DataBoundItem, DataRowView)

        If seleccion Is Nothing Then Return

        Dim fila As DataRow = seleccion.Row
        Dim cantidad As Decimal = CDec(fila("cantidad"))
        Dim costo As Decimal = CDec(fila("costo_unitario"))
        Dim piezasFila As Integer = 1
        If fila.Table.Columns.Contains("piezas") AndAlso Not IsDBNull(fila("piezas")) Then
            piezasFila = Math.Max(1, CInt(fila("piezas")))
        End If

        ' Permitir cargar valores existentes que superen el máximo inicial.
        nudCantidad.Maximum = Math.Max(nudCantidad.Maximum, piezasFila)
        nudCostoUnitario.Maximum = Math.Max(nudCostoUnitario.Maximum, costo)

        If Not seleccionarVariante(CInt(fila("id_variante"))) Then
            MessageBox.Show("No se encontró la variante de este producto.")
            Return
        End If

        Dim unidad As String = If(IsDBNull(fila("unidad")), "", fila("unidad").ToString())
        If unidad = "CM2" OrElse unidad = "PIE2" Then
            numAncho.Value = ValorNud(fila("ancho_cm"))
            numAlto.Value = ValorNud(fila("alto_cm"))
            nudCantidad.Value = piezasFila
        ElseIf unidad = "CM_LINEAL" Then
            numAlto.Value = ValorNud(fila("largo_cm"))
            nudCantidad.Value = piezasFila
        Else
            nudCantidad.Value = cantidad
        End If

        detalleEnEdicion = fila
        nudCostoUnitario.Value = costo
        btnAgregarDetalle.Text = "ACTUALIZAR PRODUCTO"
        btnCancelarEdicion.Visible = True
        EnfocarMedida()
    End Sub
End Class

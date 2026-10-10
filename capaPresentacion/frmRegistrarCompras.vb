Imports capaNegocio
Imports System.Data
Imports System.Windows.Forms

Public Class frmRegistrarCompras
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Private objVariante As New ProductoVariante
    Private objCompra As New Compra
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

            cboVariante.DropDownStyle = ComboBoxStyle.DropDownList
            cboVariante.DropDownWidth = 550

            nudCantidad.DecimalPlaces = 2
            nudCantidad.Minimum = 0.01D
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
                New String() {"cantidad", "costo_unitario", "subtotal"}

                dgvDetalle.Columns(nombre).DefaultCellStyle.Format = "N2"
            Next

            cargarVariantes()
            limpiar()
            btnGuardar.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cargarVariantes()
        Dim variantes As DataTable = objVariante.Listar()
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

        cboVariante.DisplayMember = "descripcion"
        cboVariante.ValueMember = "id_variante"
        cboVariante.DataSource = variantes
        cboVariante.SelectedIndex = -1
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
        cboVariante.SelectedIndex = -1
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

        If cboVariante.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione un producto.")
            Return
        End If

        If nudCantidad.Value <= 0 OrElse nudCostoUnitario.Value <= 0 Then
            MessageBox.Show("La cantidad y el costo deben ser mayores a cero.")
            Return
        End If

        Dim id As Integer = CInt(cboVariante.SelectedValue)
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
            id, cboVariante.Text, nudCantidad.Value,
            nudCostoUnitario.Value, subtotal)
        Else
            detalleEnEdicion("id_variante") = id
            detalleEnEdicion("producto") = cboVariante.Text
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
            cboVariante.SelectedIndex = -1
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
        cboVariante.SelectedIndex = -1
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

        cboVariante.SelectedValue = CInt(fila("id_variante"))

        If cboVariante.SelectedIndex = -1 Then
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
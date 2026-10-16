<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarCompras
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pnlRegistroCompra = New System.Windows.Forms.Panel()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnVerLista = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnQuitarDetalle = New System.Windows.Forms.Button()
        Me.dgvDetalle = New System.Windows.Forms.DataGridView()
        Me.btnAgregarDetalle = New System.Windows.Forms.Button()
        Me.nudCostoUnitario = New System.Windows.Forms.NumericUpDown()
        Me.nudCantidad = New System.Windows.Forms.NumericUpDown()
        Me.txtProductoSeleccionado = New System.Windows.Forms.TextBox()
        Me.btnBuscarProducto = New System.Windows.Forms.Button()
        Me.btnCancelarEdicion = New System.Windows.Forms.Button()
        Me.lblCosto = New System.Windows.Forms.Label()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.lblProducto = New System.Windows.Forms.Label()
        Me.nudPedidoOrigen = New System.Windows.Forms.NumericUpDown()
        Me.cboMotivo = New System.Windows.Forms.ComboBox()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.txtProveedor = New System.Windows.Forms.TextBox()
        Me.lblPedidoOrigen = New System.Windows.Forms.Label()
        Me.lblMotivo = New System.Windows.Forms.Label()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.lblProveedor = New System.Windows.Forms.Label()
        Me.lblModo = New System.Windows.Forms.Label()
        Me.pnlRegistroCompra.SuspendLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCostoUnitario, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPedidoOrigen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlRegistroCompra
        '
        Me.pnlRegistroCompra.BackColor = System.Drawing.Color.White
        Me.pnlRegistroCompra.Controls.Add(Me.lblTotal)
        Me.pnlRegistroCompra.Controls.Add(Me.btnVerLista)
        Me.pnlRegistroCompra.Controls.Add(Me.btnGuardar)
        Me.pnlRegistroCompra.Controls.Add(Me.btnCancelar)
        Me.pnlRegistroCompra.Controls.Add(Me.btnQuitarDetalle)
        Me.pnlRegistroCompra.Controls.Add(Me.dgvDetalle)
        Me.pnlRegistroCompra.Controls.Add(Me.btnAgregarDetalle)
        Me.pnlRegistroCompra.Controls.Add(Me.nudCostoUnitario)
        Me.pnlRegistroCompra.Controls.Add(Me.nudCantidad)
        Me.pnlRegistroCompra.Controls.Add(Me.txtProductoSeleccionado)
        Me.pnlRegistroCompra.Controls.Add(Me.btnBuscarProducto)
        Me.pnlRegistroCompra.Controls.Add(Me.btnCancelarEdicion)
        Me.pnlRegistroCompra.Controls.Add(Me.lblCosto)
        Me.pnlRegistroCompra.Controls.Add(Me.lblCantidad)
        Me.pnlRegistroCompra.Controls.Add(Me.lblProducto)
        Me.pnlRegistroCompra.Controls.Add(Me.nudPedidoOrigen)
        Me.pnlRegistroCompra.Controls.Add(Me.cboMotivo)
        Me.pnlRegistroCompra.Controls.Add(Me.dtpFecha)
        Me.pnlRegistroCompra.Controls.Add(Me.txtProveedor)
        Me.pnlRegistroCompra.Controls.Add(Me.lblPedidoOrigen)
        Me.pnlRegistroCompra.Controls.Add(Me.lblMotivo)
        Me.pnlRegistroCompra.Controls.Add(Me.lblFecha)
        Me.pnlRegistroCompra.Controls.Add(Me.lblProveedor)
        Me.pnlRegistroCompra.Controls.Add(Me.lblModo)
        Me.pnlRegistroCompra.Location = New System.Drawing.Point(0, 0)
        Me.pnlRegistroCompra.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRegistroCompra.Name = "pnlRegistroCompra"
        Me.pnlRegistroCompra.Size = New System.Drawing.Size(900, 620)
        Me.pnlRegistroCompra.TabIndex = 0
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTotal.Location = New System.Drawing.Point(580, 513)
        Me.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(292, 28)
        Me.lblTotal.TabIndex = 21
        Me.lblTotal.Text = "TOTAL: S/ 0.00"
        '
        'btnVerLista
        '
        Me.btnVerLista.AutoSize = True
        Me.btnVerLista.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerLista.Location = New System.Drawing.Point(288, 566)
        Me.btnVerLista.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerLista.Name = "btnVerLista"
        Me.btnVerLista.Size = New System.Drawing.Size(140, 34)
        Me.btnVerLista.TabIndex = 18
        Me.btnVerLista.Text = "VER LISTADO"
        Me.btnVerLista.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.AutoSize = True
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(24, 566)
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 20
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.AutoSize = True
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(156, 566)
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 19
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnQuitarDetalle
        '
        Me.btnQuitarDetalle.AutoSize = True
        Me.btnQuitarDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnQuitarDetalle.Location = New System.Drawing.Point(24, 508)
        Me.btnQuitarDetalle.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnQuitarDetalle.Name = "btnQuitarDetalle"
        Me.btnQuitarDetalle.Size = New System.Drawing.Size(180, 34)
        Me.btnQuitarDetalle.TabIndex = 17
        Me.btnQuitarDetalle.Text = "QUITAR PRODUCTO"
        Me.btnQuitarDetalle.UseVisualStyleBackColor = True
        '
        'dgvDetalle
        '
        Me.dgvDetalle.AllowUserToAddRows = False
        Me.dgvDetalle.AllowUserToDeleteRows = False
        Me.dgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetalle.Location = New System.Drawing.Point(24, 292)
        Me.dgvDetalle.MultiSelect = False
        Me.dgvDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dgvDetalle.Name = "dgvDetalle"
        Me.dgvDetalle.ReadOnly = True
        Me.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDetalle.Size = New System.Drawing.Size(848, 202)
        Me.dgvDetalle.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.dgvDetalle.TabIndex = 16
        '
        'btnAgregarDetalle
        '
        Me.btnAgregarDetalle.AutoSize = True
        Me.btnAgregarDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnAgregarDetalle.Location = New System.Drawing.Point(24, 242)
        Me.btnAgregarDetalle.Name = "btnAgregarDetalle"
        Me.btnAgregarDetalle.Size = New System.Drawing.Size(120, 34)
        Me.btnAgregarDetalle.TabIndex = 15
        Me.btnAgregarDetalle.Text = "AGREGAR"
        Me.btnAgregarDetalle.UseVisualStyleBackColor = True
        '
        'nudCostoUnitario
        '
        Me.nudCostoUnitario.DecimalPlaces = 2
        Me.nudCostoUnitario.Location = New System.Drawing.Point(590, 195)
        Me.nudCostoUnitario.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudCostoUnitario.Name = "nudCostoUnitario"
        Me.nudCostoUnitario.Size = New System.Drawing.Size(150, 24)
        Me.nudCostoUnitario.TabIndex = 14
        Me.nudCostoUnitario.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudCantidad
        '
        Me.nudCantidad.Location = New System.Drawing.Point(590, 155)
        Me.nudCantidad.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudCantidad.Name = "nudCantidad"
        Me.nudCantidad.Size = New System.Drawing.Size(150, 24)
        Me.nudCantidad.TabIndex = 13
        Me.nudCantidad.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'txtProductoSeleccionado
        '
        Me.txtProductoSeleccionado.ReadOnly = True
        Me.txtProductoSeleccionado.Enabled = False
        Me.txtProductoSeleccionado.TabStop = False

        Me.txtProductoSeleccionado.Location = New System.Drawing.Point(590, 75)
        Me.txtProductoSeleccionado.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtProductoSeleccionado.Name = "txtProductoSeleccionado"
        Me.txtProductoSeleccionado.Size = New System.Drawing.Size(282, 24)
        Me.txtProductoSeleccionado.TabIndex = 12
        Me.btnBuscarProducto.Location = New System.Drawing.Point(590, 109)
        Me.btnBuscarProducto.Size = New System.Drawing.Size(180, 34)
        Me.btnBuscarProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnBuscarProducto.Name = "btnBuscarProducto"
        Me.btnBuscarProducto.Text = "BUSCAR PRODUCTO"
        Me.btnBuscarProducto.TabIndex = 22
        Me.btnBuscarProducto.UseVisualStyleBackColor = True
        Me.btnCancelarEdicion.Location = New System.Drawing.Point(156, 242)
        Me.btnCancelarEdicion.Size = New System.Drawing.Size(180, 34)
        Me.btnCancelarEdicion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelarEdicion.Name = "btnCancelarEdicion"
        Me.btnCancelarEdicion.Text = "CANCELAR EDICIÓN"
        Me.btnCancelarEdicion.TabIndex = 23
        Me.btnCancelarEdicion.Visible = False
        Me.btnCancelarEdicion.UseVisualStyleBackColor = True
        '
        'lblCosto
        '
        Me.lblCosto.AutoSize = True
        Me.lblCosto.Location = New System.Drawing.Point(452, 199)
        Me.lblCosto.Name = "lblCosto"
        Me.lblCosto.Size = New System.Drawing.Size(130, 22)
        Me.lblCosto.TabIndex = 11
        Me.lblCosto.Text = "Costo unitario:"
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(452, 159)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(130, 22)
        Me.lblCantidad.TabIndex = 10
        Me.lblCantidad.Text = "Cantidad:"
        '
        'lblProducto
        '
        Me.lblProducto.AutoSize = True
        Me.lblProducto.Location = New System.Drawing.Point(452, 79)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(130, 22)
        Me.lblProducto.TabIndex = 9
        Me.lblProducto.Text = "Producto / variante:"
        '
        'nudPedidoOrigen
        '
        Me.nudPedidoOrigen.Location = New System.Drawing.Point(162, 195)
        Me.nudPedidoOrigen.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.nudPedidoOrigen.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPedidoOrigen.Name = "nudPedidoOrigen"
        Me.nudPedidoOrigen.Size = New System.Drawing.Size(260, 24)
        Me.nudPedidoOrigen.TabIndex = 8
        Me.nudPedidoOrigen.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cboMotivo
        '
        Me.cboMotivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMotivo.FormattingEnabled = True
        Me.cboMotivo.Location = New System.Drawing.Point(162, 155)
        Me.cboMotivo.Name = "cboMotivo"
        Me.cboMotivo.Size = New System.Drawing.Size(260, 24)
        Me.cboMotivo.TabIndex = 7
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(162, 115)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(260, 24)
        Me.dtpFecha.TabIndex = 6
        '
        'txtProveedor
        '
        Me.txtProveedor.Location = New System.Drawing.Point(162, 75)
        Me.txtProveedor.MaxLength = 150
        Me.txtProveedor.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(260, 24)
        Me.txtProveedor.TabIndex = 5
        '
        'lblPedidoOrigen
        '
        Me.lblPedidoOrigen.AutoSize = True
        Me.lblPedidoOrigen.Location = New System.Drawing.Point(24, 199)
        Me.lblPedidoOrigen.Name = "lblPedidoOrigen"
        Me.lblPedidoOrigen.Size = New System.Drawing.Size(130, 22)
        Me.lblPedidoOrigen.TabIndex = 4
        Me.lblPedidoOrigen.Text = "Pedido de origen:"
        '
        'lblMotivo
        '
        Me.lblMotivo.AutoSize = True
        Me.lblMotivo.Location = New System.Drawing.Point(24, 159)
        Me.lblMotivo.Name = "lblMotivo"
        Me.lblMotivo.Size = New System.Drawing.Size(130, 22)
        Me.lblMotivo.TabIndex = 3
        Me.lblMotivo.Text = "Motivo:"
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(24, 119)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(130, 22)
        Me.lblFecha.TabIndex = 2
        Me.lblFecha.Text = "Fecha:"
        '
        'lblProveedor
        '
        Me.lblProveedor.AutoSize = True
        Me.lblProveedor.Location = New System.Drawing.Point(24, 79)
        Me.lblProveedor.Name = "lblProveedor"
        Me.lblProveedor.Size = New System.Drawing.Size(130, 22)
        Me.lblProveedor.TabIndex = 1
        Me.lblProveedor.Text = "Proveedor:"
        '
        'lblModo
        '
        Me.lblModo.AutoSize = True
        Me.lblModo.Enabled = True
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblModo.Location = New System.Drawing.Point(24, 20)
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Size = New System.Drawing.Size(500, 32)
        Me.lblModo.TabIndex = 0
        Me.lblModo.Text = "NUEVA COMPRA"
        '
        'frmRegistrarCompras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 620)
        Me.Controls.Add(Me.pnlRegistroCompra)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(916, 570)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Name = "frmRegistrarCompras"
        Me.Text = "Registro de compras"
        Me.pnlRegistroCompra.ResumeLayout(False)
        Me.pnlRegistroCompra.PerformLayout()
        CType(Me.dgvDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCostoUnitario, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPedidoOrigen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlRegistroCompra As Panel
    Friend WithEvents lblMotivo As Label
    Friend WithEvents lblFecha As Label
    Friend WithEvents lblProveedor As Label
    Friend WithEvents lblModo As Label
    Friend WithEvents lblPedidoOrigen As Label
    Friend WithEvents nudPedidoOrigen As NumericUpDown
    Friend WithEvents cboMotivo As ComboBox
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents txtProveedor As TextBox
    Friend WithEvents btnAgregarDetalle As Button
    Friend WithEvents nudCostoUnitario As NumericUpDown
    Friend WithEvents nudCantidad As NumericUpDown
    Friend WithEvents txtProductoSeleccionado As TextBox
    Friend WithEvents btnBuscarProducto As Button
    Friend WithEvents btnCancelarEdicion As Button
    Friend WithEvents lblCosto As Label
    Friend WithEvents lblCantidad As Label
    Friend WithEvents lblProducto As Label
    Friend WithEvents dgvDetalle As DataGridView
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnVerLista As Button
    Friend WithEvents btnQuitarDetalle As Button
    Friend WithEvents lblTotal As Label
End Class

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
        Me.cboVariante = New System.Windows.Forms.ComboBox()
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
        Me.pnlRegistroCompra.Controls.Add(Me.cboVariante)
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
        Me.pnlRegistroCompra.Location = New System.Drawing.Point(12, 18)
        Me.pnlRegistroCompra.Name = "pnlRegistroCompra"
        Me.pnlRegistroCompra.Size = New System.Drawing.Size(646, 601)
        Me.pnlRegistroCompra.TabIndex = 0
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(481, 506)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(131, 20)
        Me.lblTotal.TabIndex = 21
        Me.lblTotal.Text = "TOTAL: S/ 0.00"
        '
        'btnVerLista
        '
        Me.btnVerLista.AutoSize = True
        Me.btnVerLista.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVerLista.Location = New System.Drawing.Point(322, 543)
        Me.btnVerLista.Name = "btnVerLista"
        Me.btnVerLista.Size = New System.Drawing.Size(136, 31)
        Me.btnVerLista.TabIndex = 18
        Me.btnVerLista.Text = "VER LISTADO"
        Me.btnVerLista.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.AutoSize = True
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardar.Location = New System.Drawing.Point(38, 543)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(137, 31)
        Me.btnGuardar.TabIndex = 20
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.AutoSize = True
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelar.Location = New System.Drawing.Point(180, 543)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(136, 31)
        Me.btnCancelar.TabIndex = 19
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnQuitarDetalle
        '
        Me.btnQuitarDetalle.AutoSize = True
        Me.btnQuitarDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnQuitarDetalle.Location = New System.Drawing.Point(38, 506)
        Me.btnQuitarDetalle.Name = "btnQuitarDetalle"
        Me.btnQuitarDetalle.Size = New System.Drawing.Size(137, 31)
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
        Me.dgvDetalle.Location = New System.Drawing.Point(38, 338)
        Me.dgvDetalle.MultiSelect = False
        Me.dgvDetalle.Name = "dgvDetalle"
        Me.dgvDetalle.ReadOnly = True
        Me.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDetalle.Size = New System.Drawing.Size(574, 150)
        Me.dgvDetalle.TabIndex = 16
        '
        'btnAgregarDetalle
        '
        Me.btnAgregarDetalle.AutoSize = True
        Me.btnAgregarDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAgregarDetalle.Location = New System.Drawing.Point(236, 290)
        Me.btnAgregarDetalle.Name = "btnAgregarDetalle"
        Me.btnAgregarDetalle.Size = New System.Drawing.Size(111, 31)
        Me.btnAgregarDetalle.TabIndex = 15
        Me.btnAgregarDetalle.Text = "AGREGAR"
        Me.btnAgregarDetalle.UseVisualStyleBackColor = True
        '
        'nudCostoUnitario
        '
        Me.nudCostoUnitario.DecimalPlaces = 2
        Me.nudCostoUnitario.Location = New System.Drawing.Point(427, 216)
        Me.nudCostoUnitario.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudCostoUnitario.Name = "nudCostoUnitario"
        Me.nudCostoUnitario.Size = New System.Drawing.Size(102, 20)
        Me.nudCostoUnitario.TabIndex = 14
        Me.nudCostoUnitario.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudCantidad
        '
        Me.nudCantidad.Location = New System.Drawing.Point(427, 170)
        Me.nudCantidad.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudCantidad.Name = "nudCantidad"
        Me.nudCantidad.Size = New System.Drawing.Size(102, 20)
        Me.nudCantidad.TabIndex = 13
        Me.nudCantidad.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cboVariante
        '
        Me.cboVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVariante.FormattingEnabled = True
        Me.cboVariante.Location = New System.Drawing.Point(427, 113)
        Me.cboVariante.Name = "cboVariante"
        Me.cboVariante.Size = New System.Drawing.Size(185, 21)
        Me.cboVariante.TabIndex = 12
        '
        'lblCosto
        '
        Me.lblCosto.AutoSize = True
        Me.lblCosto.Location = New System.Drawing.Point(319, 223)
        Me.lblCosto.Name = "lblCosto"
        Me.lblCosto.Size = New System.Drawing.Size(74, 13)
        Me.lblCosto.TabIndex = 11
        Me.lblCosto.Text = "Costo unitario:"
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(319, 170)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(52, 13)
        Me.lblCantidad.TabIndex = 10
        Me.lblCantidad.Text = "Cantidad:"
        '
        'lblProducto
        '
        Me.lblProducto.AutoSize = True
        Me.lblProducto.Location = New System.Drawing.Point(319, 116)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(102, 13)
        Me.lblProducto.TabIndex = 9
        Me.lblProducto.Text = "Producto / variante:"
        '
        'nudPedidoOrigen
        '
        Me.nudPedidoOrigen.Location = New System.Drawing.Point(146, 235)
        Me.nudPedidoOrigen.Maximum = New Decimal(New Integer() {2147483647, 0, 0, 0})
        Me.nudPedidoOrigen.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPedidoOrigen.Name = "nudPedidoOrigen"
        Me.nudPedidoOrigen.Size = New System.Drawing.Size(102, 20)
        Me.nudPedidoOrigen.TabIndex = 8
        Me.nudPedidoOrigen.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cboMotivo
        '
        Me.cboMotivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMotivo.FormattingEnabled = True
        Me.cboMotivo.Location = New System.Drawing.Point(146, 187)
        Me.cboMotivo.Name = "cboMotivo"
        Me.cboMotivo.Size = New System.Drawing.Size(102, 21)
        Me.cboMotivo.TabIndex = 7
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(146, 148)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(102, 20)
        Me.dtpFecha.TabIndex = 6
        '
        'txtProveedor
        '
        Me.txtProveedor.Location = New System.Drawing.Point(146, 99)
        Me.txtProveedor.MaxLength = 150
        Me.txtProveedor.Name = "txtProveedor"
        Me.txtProveedor.Size = New System.Drawing.Size(102, 20)
        Me.txtProveedor.TabIndex = 5
        '
        'lblPedidoOrigen
        '
        Me.lblPedidoOrigen.AutoSize = True
        Me.lblPedidoOrigen.Location = New System.Drawing.Point(50, 242)
        Me.lblPedidoOrigen.Name = "lblPedidoOrigen"
        Me.lblPedidoOrigen.Size = New System.Drawing.Size(90, 13)
        Me.lblPedidoOrigen.TabIndex = 4
        Me.lblPedidoOrigen.Text = "Pedido de origen:"
        '
        'lblMotivo
        '
        Me.lblMotivo.AutoSize = True
        Me.lblMotivo.Location = New System.Drawing.Point(98, 195)
        Me.lblMotivo.Name = "lblMotivo"
        Me.lblMotivo.Size = New System.Drawing.Size(42, 13)
        Me.lblMotivo.TabIndex = 3
        Me.lblMotivo.Text = "Motivo:"
        '
        'lblFecha
        '
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(81, 148)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(40, 13)
        Me.lblFecha.TabIndex = 2
        Me.lblFecha.Text = "Fecha:"
        '
        'lblProveedor
        '
        Me.lblProveedor.AutoSize = True
        Me.lblProveedor.Location = New System.Drawing.Point(81, 106)
        Me.lblProveedor.Name = "lblProveedor"
        Me.lblProveedor.Size = New System.Drawing.Size(59, 13)
        Me.lblProveedor.TabIndex = 1
        Me.lblProveedor.Text = "Proveedor:"
        '
        'lblModo
        '
        Me.lblModo.AutoSize = True
        Me.lblModo.Enabled = False
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModo.Location = New System.Drawing.Point(33, 24)
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Size = New System.Drawing.Size(194, 25)
        Me.lblModo.TabIndex = 0
        Me.lblModo.Text = "NUEVA COMPRA"
        '
        'frmRegistrarCompras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 631)
        Me.Controls.Add(Me.pnlRegistroCompra)
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
    Friend WithEvents cboVariante As ComboBox
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

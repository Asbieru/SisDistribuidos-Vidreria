<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListadoVentas
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
        Me.pnlVentas = New System.Windows.Forms.Panel()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.lblCliente = New System.Windows.Forms.Label()
        Me.lblEstadoPago = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.btnVerTodas = New System.Windows.Forms.Button()
        Me.txtBuscarCliente = New System.Windows.Forms.TextBox()
        Me.cboEstadoPago = New System.Windows.Forms.ComboBox()
        Me.dgvVentas = New System.Windows.Forms.DataGridView()
        Me.lblTotalPedidos = New System.Windows.Forms.Label()
        Me.lblTotalCobrado = New System.Windows.Forms.Label()
        Me.lblTotalSaldo = New System.Windows.Forms.Label()
        Me.btnVerDetalle = New System.Windows.Forms.Button()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.lblResultados = New System.Windows.Forms.Label()
        Me.lblTotalExcedentes = New System.Windows.Forms.Label()
        Me.pnlVentas.SuspendLayout()
        CType(Me.dgvVentas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlVentas
        '
        Me.pnlVentas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlVentas.BackColor = System.Drawing.Color.White
        Me.pnlVentas.Controls.Add(Me.lblResultados)
        Me.pnlVentas.Controls.Add(Me.lblTotalExcedentes)
        Me.pnlVentas.Controls.Add(Me.btnCerrar)
        Me.pnlVentas.Controls.Add(Me.btnActualizar)
        Me.pnlVentas.Controls.Add(Me.btnVerDetalle)
        Me.pnlVentas.Controls.Add(Me.lblTotalSaldo)
        Me.pnlVentas.Controls.Add(Me.lblTotalCobrado)
        Me.pnlVentas.Controls.Add(Me.lblTotalPedidos)
        Me.pnlVentas.Controls.Add(Me.dgvVentas)
        Me.pnlVentas.Controls.Add(Me.cboEstadoPago)
        Me.pnlVentas.Controls.Add(Me.txtBuscarCliente)
        Me.pnlVentas.Controls.Add(Me.btnVerTodas)
        Me.pnlVentas.Controls.Add(Me.btnFiltrar)
        Me.pnlVentas.Controls.Add(Me.dtpHasta)
        Me.pnlVentas.Controls.Add(Me.dtpDesde)
        Me.pnlVentas.Controls.Add(Me.Label5)
        Me.pnlVentas.Controls.Add(Me.lblEstadoPago)
        Me.pnlVentas.Controls.Add(Me.lblCliente)
        Me.pnlVentas.Controls.Add(Me.lblHasta)
        Me.pnlVentas.Controls.Add(Me.lblDesde)
        Me.pnlVentas.Location = New System.Drawing.Point(12, 12)
        Me.pnlVentas.Name = "pnlVentas"
        Me.pnlVentas.Size = New System.Drawing.Size(776, 496)
        Me.pnlVentas.TabIndex = 0
        '
        'lblDesde
        '
        Me.lblDesde.Location = New System.Drawing.Point(83, 62)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(52, 13)
        Me.lblDesde.TabIndex = 0
        Me.lblDesde.Text = "Desde:"
        '
        'lblHasta
        '
        Me.lblHasta.Location = New System.Drawing.Point(266, 62)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(39, 13)
        Me.lblHasta.TabIndex = 1
        Me.lblHasta.Text = "Hasta:"
        '
        'lblCliente
        '
        Me.lblCliente.Location = New System.Drawing.Point(120, 127)
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(50, 12)
        Me.lblCliente.TabIndex = 2
        Me.lblCliente.Text = "Cliente:"
        '
        'lblEstadoPago
        '
        Me.lblEstadoPago.Location = New System.Drawing.Point(393, 122)
        Me.lblEstadoPago.Name = "lblEstadoPago"
        Me.lblEstadoPago.Size = New System.Drawing.Size(89, 13)
        Me.lblEstadoPago.TabIndex = 3
        Me.lblEstadoPago.Text = "Estado de pago:"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(393, 231)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 13)
        Me.Label5.TabIndex = 4
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(127, 56)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(81, 20)
        Me.dtpDesde.TabIndex = 5
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(309, 56)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(81, 20)
        Me.dtpHasta.TabIndex = 6
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFiltrar.Location = New System.Drawing.Point(459, 52)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(75, 23)
        Me.btnFiltrar.TabIndex = 7
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnVerTodas
        '
        Me.btnVerTodas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVerTodas.Location = New System.Drawing.Point(559, 52)
        Me.btnVerTodas.Name = "btnVerTodas"
        Me.btnVerTodas.Size = New System.Drawing.Size(91, 23)
        Me.btnVerTodas.TabIndex = 8
        Me.btnVerTodas.Text = "VER TODAS"
        Me.btnVerTodas.UseVisualStyleBackColor = True
        '
        'txtBuscarCliente
        '
        Me.txtBuscarCliente.Location = New System.Drawing.Point(176, 119)
        Me.txtBuscarCliente.Name = "txtBuscarCliente"
        Me.txtBuscarCliente.Size = New System.Drawing.Size(161, 20)
        Me.txtBuscarCliente.TabIndex = 9
        '
        'cboEstadoPago
        '
        Me.cboEstadoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstadoPago.FormattingEnabled = True
        Me.cboEstadoPago.Location = New System.Drawing.Point(488, 118)
        Me.cboEstadoPago.Name = "cboEstadoPago"
        Me.cboEstadoPago.Size = New System.Drawing.Size(121, 21)
        Me.cboEstadoPago.TabIndex = 10
        '
        'dgvVentas
        '
        Me.dgvVentas.AllowUserToAddRows = False
        Me.dgvVentas.AllowUserToDeleteRows = False
        Me.dgvVentas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvVentas.Location = New System.Drawing.Point(49, 185)
        Me.dgvVentas.MultiSelect = False
        Me.dgvVentas.Name = "dgvVentas"
        Me.dgvVentas.ReadOnly = True
        Me.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvVentas.Size = New System.Drawing.Size(671, 150)
        Me.dgvVentas.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.dgvVentas.TabIndex = 11
        '
        'lblTotalPedidos
        '
        Me.lblTotalPedidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalPedidos.Location = New System.Drawing.Point(59, 353)
        Me.lblTotalPedidos.Name = "lblTotalPedidos"
        Me.lblTotalPedidos.Size = New System.Drawing.Size(187, 18)
        Me.lblTotalPedidos.TabIndex = 12
        Me.lblTotalPedidos.Text = "IMPORTE PEDIDOS: S/ 0.00"
        Me.lblTotalPedidos.AutoSize = True
        Me.lblTotalPedidos.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        '
        'lblTotalCobrado
        '
        Me.lblTotalCobrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalCobrado.Location = New System.Drawing.Point(59, 376)
        Me.lblTotalCobrado.Name = "lblTotalCobrado"
        Me.lblTotalCobrado.Size = New System.Drawing.Size(126, 18)
        Me.lblTotalCobrado.TabIndex = 13
        Me.lblTotalCobrado.Text = "COBRADO: S/ 0.00"
        Me.lblTotalCobrado.AutoSize = True
        Me.lblTotalCobrado.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        '
        'lblTotalSaldo
        '
        Me.lblTotalSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalSaldo.Location = New System.Drawing.Point(59, 399)
        Me.lblTotalSaldo.Name = "lblTotalSaldo"
        Me.lblTotalSaldo.Size = New System.Drawing.Size(114, 18)
        Me.lblTotalSaldo.TabIndex = 14
        Me.lblTotalSaldo.Text = "DEUDA PENDIENTE: S/ 0.00"
        Me.lblTotalSaldo.AutoSize = True
        Me.lblTotalSaldo.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.lblTotalExcedentes.Font = Me.lblTotalSaldo.Font
        Me.lblTotalExcedentes.Location = New System.Drawing.Point(59, 422)
        Me.lblTotalExcedentes.Name = "lblTotalExcedentes"
        Me.lblTotalExcedentes.AutoSize = True
        Me.lblTotalExcedentes.Text = "EXCEDENTES: S/ 0.00"
        Me.lblTotalExcedentes.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        '
        'btnVerDetalle
        '
        Me.btnVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVerDetalle.Location = New System.Drawing.Point(194, 454)
        Me.btnVerDetalle.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerDetalle.Name = "btnVerDetalle"
        Me.btnVerDetalle.Size = New System.Drawing.Size(102, 23)
        Me.btnVerDetalle.TabIndex = 15
        Me.btnVerDetalle.Text = "VER DETALLE"
        Me.btnVerDetalle.UseVisualStyleBackColor = True
        '
        'btnActualizar
        '
        Me.btnActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnActualizar.Location = New System.Drawing.Point(334, 454)
        Me.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(102, 23)
        Me.btnActualizar.TabIndex = 16
        Me.btnActualizar.Text = "ACTUALIZAR"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCerrar.Location = New System.Drawing.Point(472, 454)
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(102, 23)
        Me.btnCerrar.TabIndex = 17
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmListadoVentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.lblResultados.Location = New System.Drawing.Point(49, 158)
        Me.lblResultados.AutoSize = True
        Me.lblResultados.Name = "lblResultados"
        Me.lblResultados.Text = "Ventas encontradas: 0"
        Me.ClientSize = New System.Drawing.Size(800, 520)
        Me.MinimumSize = New System.Drawing.Size(816, 559)
        Me.Controls.Add(Me.pnlVentas)
        Me.Name = "frmListadoVentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Listado de ventas"
        Me.pnlVentas.ResumeLayout(False)
        Me.pnlVentas.PerformLayout()
        CType(Me.dgvVentas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlVentas As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents lblEstadoPago As Label
    Friend WithEvents lblCliente As Label
    Friend WithEvents lblHasta As Label
    Friend WithEvents lblDesde As Label
    Friend WithEvents cboEstadoPago As ComboBox
    Friend WithEvents txtBuscarCliente As TextBox
    Friend WithEvents btnVerTodas As Button
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents lblTotalPedidos As Label
    Friend WithEvents dgvVentas As DataGridView
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnActualizar As Button
    Friend WithEvents btnVerDetalle As Button
    Friend WithEvents lblTotalSaldo As Label
    Friend WithEvents lblTotalCobrado As Label
    Friend WithEvents lblResultados As Label
    Friend WithEvents lblTotalExcedentes As Label
End Class

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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.pnlFiltro2 = New System.Windows.Forms.Panel()
        Me.pnlFiltro3 = New System.Windows.Forms.Panel()
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
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
        Me.pnlVentas.Controls.Add(Me.Label5)
        Me.pnlVentas.Location = New System.Drawing.Point(12, 12)
        Me.pnlVentas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlVentas.Name = "pnlVentas"
        Me.pnlVentas.Size = New System.Drawing.Size(776, 496)
        Me.pnlVentas.TabIndex = 0
        '
        'lblDesde
        '
        Me.lblDesde.Location = New System.Drawing.Point(0, 0)
        Me.lblDesde.AutoSize = False
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(140, 20)
        Me.lblDesde.TabIndex = 0
        Me.lblDesde.Text = "Desde:"
        '
        'lblHasta
        '
        Me.lblHasta.Location = New System.Drawing.Point(0, 0)
        Me.lblHasta.AutoSize = False
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(140, 20)
        Me.lblHasta.TabIndex = 1
        Me.lblHasta.Text = "Hasta:"
        '
        'lblCliente
        '
        Me.lblCliente.Location = New System.Drawing.Point(0, 0)
        Me.lblCliente.AutoSize = False
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(300, 20)
        Me.lblCliente.TabIndex = 2
        Me.lblCliente.Text = "Cliente:"
        '
        'lblEstadoPago
        '
        Me.lblEstadoPago.Location = New System.Drawing.Point(0, 0)
        Me.lblEstadoPago.AutoSize = False
        Me.lblEstadoPago.Name = "lblEstadoPago"
        Me.lblEstadoPago.Size = New System.Drawing.Size(190, 20)
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
        Me.dtpDesde.Location = New System.Drawing.Point(0, 24)
        Me.dtpDesde.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(140, 24)
        Me.dtpDesde.TabIndex = 5
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(0, 24)
        Me.dtpHasta.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(140, 24)
        Me.dtpHasta.TabIndex = 6
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnFiltrar.Location = New System.Drawing.Point(459, 52)
        Me.btnFiltrar.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(110, 34)
        Me.btnFiltrar.TabIndex = 7
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnVerTodas
        '
        Me.btnVerTodas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodas.Location = New System.Drawing.Point(559, 52)
        Me.btnVerTodas.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnVerTodas.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerTodas.Name = "btnVerTodas"
        Me.btnVerTodas.Size = New System.Drawing.Size(120, 34)
        Me.btnVerTodas.TabIndex = 8
        Me.btnVerTodas.Text = "VER TODAS"
        Me.btnVerTodas.UseVisualStyleBackColor = True
        '
        'txtBuscarCliente
        '
        Me.txtBuscarCliente.Location = New System.Drawing.Point(0, 24)
        Me.txtBuscarCliente.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtBuscarCliente.Name = "txtBuscarCliente"
        Me.txtBuscarCliente.Size = New System.Drawing.Size(300, 24)
        Me.txtBuscarCliente.TabIndex = 9
        '
        'cboEstadoPago
        '
        Me.cboEstadoPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstadoPago.FormattingEnabled = True
        Me.cboEstadoPago.Location = New System.Drawing.Point(0, 24)
        Me.cboEstadoPago.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cboEstadoPago.Name = "cboEstadoPago"
        Me.cboEstadoPago.Size = New System.Drawing.Size(190, 24)
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
        Me.dgvVentas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvVentas.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvVentas.Name = "dgvVentas"
        Me.dgvVentas.ReadOnly = True
        Me.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvVentas.Size = New System.Drawing.Size(671, 150)
        Me.dgvVentas.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvVentas.TabIndex = 11
        '
        'lblTotalPedidos
        '
        Me.lblTotalPedidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTotalPedidos.Location = New System.Drawing.Point(59, 353)
        Me.lblTotalPedidos.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblTotalPedidos.Name = "lblTotalPedidos"
        Me.lblTotalPedidos.Size = New System.Drawing.Size(187, 18)
        Me.lblTotalPedidos.TabIndex = 12
        Me.lblTotalPedidos.Text = "IMPORTE PEDIDOS: S/ 0.00"
        Me.lblTotalPedidos.AutoSize = True
        Me.lblTotalPedidos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        '
        'lblTotalCobrado
        '
        Me.lblTotalCobrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTotalCobrado.Location = New System.Drawing.Point(59, 376)
        Me.lblTotalCobrado.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblTotalCobrado.Name = "lblTotalCobrado"
        Me.lblTotalCobrado.Size = New System.Drawing.Size(126, 18)
        Me.lblTotalCobrado.TabIndex = 13
        Me.lblTotalCobrado.Text = "COBRADO: S/ 0.00"
        Me.lblTotalCobrado.AutoSize = True
        Me.lblTotalCobrado.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        '
        'lblTotalSaldo
        '
        Me.lblTotalSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTotalSaldo.Location = New System.Drawing.Point(59, 399)
        Me.lblTotalSaldo.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblTotalSaldo.Name = "lblTotalSaldo"
        Me.lblTotalSaldo.Size = New System.Drawing.Size(114, 18)
        Me.lblTotalSaldo.TabIndex = 14
        Me.lblTotalSaldo.Text = "DEUDA PENDIENTE: S/ 0.00"
        Me.lblTotalSaldo.AutoSize = True
        Me.lblTotalSaldo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblTotalExcedentes.Font = Me.lblTotalSaldo.Font
        Me.lblTotalExcedentes.Location = New System.Drawing.Point(59, 422)
        Me.lblTotalExcedentes.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblTotalExcedentes.Name = "lblTotalExcedentes"
        Me.lblTotalExcedentes.AutoSize = True
        Me.lblTotalExcedentes.Text = "EXCEDENTES: S/ 0.00"
        Me.lblTotalExcedentes.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        '
        'btnVerDetalle
        '
        Me.btnVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerDetalle.Location = New System.Drawing.Point(194, 454)
        Me.btnVerDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerDetalle.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerDetalle.Name = "btnVerDetalle"
        Me.btnVerDetalle.Size = New System.Drawing.Size(120, 34)
        Me.btnVerDetalle.TabIndex = 15
        Me.btnVerDetalle.Text = "VER DETALLE"
        Me.btnVerDetalle.UseVisualStyleBackColor = True
        '
        'btnActualizar
        '
        Me.btnActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnActualizar.Location = New System.Drawing.Point(334, 454)
        Me.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnActualizar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(110, 34)
        Me.btnActualizar.TabIndex = 16
        Me.btnActualizar.Text = "ACTUALIZAR"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(472, 454)
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 17
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmListadoVentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.lblResultados.Location = New System.Drawing.Point(49, 158)
        Me.lblResultados.AutoSize = True
        Me.lblResultados.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblResultados.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblResultados.Name = "lblResultados"
        Me.lblResultados.Text = "Ventas encontradas: 0"
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.Controls.Add(Me.pnlVentas)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmListadoVentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Listado de ventas"
        Me.pnlVentas.ResumeLayout(False)
        Me.pnlVentas.PerformLayout()
        CType(Me.dgvVentas, System.ComponentModel.ISupportInitialize).EndInit()

        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Text = "VENTAS Y COBROS"
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.RowCount = 5
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.WrapContents = True
        Me.flpFiltros.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(300, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblCliente)
        Me.pnlFiltro0.Controls.Add(Me.txtBuscarCliente)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(190, 58)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Controls.Add(Me.lblEstadoPago)
        Me.pnlFiltro1.Controls.Add(Me.cboEstadoPago)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.pnlFiltro2.Name = "pnlFiltro2"
        Me.pnlFiltro2.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro2.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro2.Controls.Add(Me.lblDesde)
        Me.pnlFiltro2.Controls.Add(Me.dtpDesde)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro2)
        Me.pnlFiltro3.Name = "pnlFiltro3"
        Me.pnlFiltro3.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro3.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro3.Controls.Add(Me.lblHasta)
        Me.pnlFiltro3.Controls.Add(Me.dtpHasta)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro3)
        Me.flpFiltros.Controls.Add(Me.btnFiltrar)
        Me.flpFiltros.Controls.Add(Me.btnVerTodas)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.WrapContents = True
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Controls.Add(Me.lblResultados)
        Me.flpResumen.Controls.Add(Me.lblTotalPedidos)
        Me.flpResumen.Controls.Add(Me.lblTotalCobrado)
        Me.flpResumen.Controls.Add(Me.lblTotalSaldo)
        Me.flpResumen.Controls.Add(Me.lblTotalExcedentes)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.WrapContents = True
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Controls.Add(Me.btnVerDetalle)
        Me.flpAcciones.Controls.Add(Me.btnActualizar)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvVentas, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.pnlVentas.Controls.Add(Me.tlpContenido)
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
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro2 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro3 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

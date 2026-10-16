<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistorialClientes
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.pnlFiltro2 = New System.Windows.Forms.Panel()
        Me.pnlFechas = New System.Windows.Forms.Panel()
        Me.tlpResumen = New System.Windows.Forms.TableLayoutPanel()
        Me.splHistorial = New System.Windows.Forms.SplitContainer()
        Me.tlpPie = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblCliente = New System.Windows.Forms.Label()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.lblPagos = New System.Windows.Forms.Label()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.chkFechas = New System.Windows.Forms.CheckBox()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.lblFiltro = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.btnVerTodos = New System.Windows.Forms.Button()
        Me.dgvPedidos = New System.Windows.Forms.DataGridView()
        Me.dgvPagos = New System.Windows.Forms.DataGridView()
        Me.btnCambiarCliente = New System.Windows.Forms.Button()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvPedidos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        Me.ClientSize = New System.Drawing.Size(1000, 680)
        Me.Panel2.Size = New System.Drawing.Size(1050, 715)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Name = "Panel2"
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(24, 20)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Text = "FICHA COMERCIAL DEL CLIENTE"
        Me.lblCliente.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblCliente.AutoEllipsis = True
        Me.lblCliente.Location = New System.Drawing.Point(24, 60)
        Me.lblCliente.AutoSize = True
        Me.lblCliente.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCliente.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(1002, 25)
        Me.lblCliente.Text = "Cliente:"
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Location = New System.Drawing.Point(0, 0)
        Me.lblEstado.Size = New System.Drawing.Size(160, 20)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Text = "Estado:"
        Me.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstado.Location = New System.Drawing.Point(0, 24)
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(160, 24)
        Me.cmbEstado.TabIndex = 1
        Me.btnCambiarCliente.Location = New System.Drawing.Point(24, 94)
        Me.btnCambiarCliente.Margin = New System.Windows.Forms.Padding(0, 20, 10, 10)
        Me.btnCambiarCliente.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCambiarCliente.Name = "btnCambiarCliente"
        Me.btnCambiarCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCambiarCliente.Size = New System.Drawing.Size(150, 34)
        Me.btnCambiarCliente.TabIndex = 0
        Me.btnCambiarCliente.Text = "ELEGIR CLIENTE"
        Me.btnCambiarCliente.UseVisualStyleBackColor = True
        Me.btnActualizar.Location = New System.Drawing.Point(454, 94)
        Me.btnActualizar.Margin = New System.Windows.Forms.Padding(0, 20, 10, 10)
        Me.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnActualizar.Size = New System.Drawing.Size(120, 34)
        Me.btnActualizar.TabIndex = 2
        Me.btnActualizar.Text = "ACTUALIZAR"
        Me.btnActualizar.UseVisualStyleBackColor = True
        Me.chkFechas.AutoSize = True
        Me.chkFechas.Location = New System.Drawing.Point(24, 143)
        Me.chkFechas.Size = New System.Drawing.Size(155, 34)
        Me.chkFechas.Margin = New System.Windows.Forms.Padding(0, 20, 10, 10)
        Me.chkFechas.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.chkFechas.Name = "chkFechas"
        Me.chkFechas.Text = "Filtrar por fechas"
        Me.chkFechas.TabIndex = 3
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(0, 0)
        Me.lblDesde.Size = New System.Drawing.Size(140, 20)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Text = "Desde:"
        Me.dtpDesde.Enabled = False
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpDesde.Location = New System.Drawing.Point(0, 24)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(140, 24)
        Me.dtpDesde.TabIndex = 4
        Me.lblHasta.AutoSize = True
        Me.lblHasta.Location = New System.Drawing.Point(0, 0)
        Me.lblHasta.Size = New System.Drawing.Size(140, 20)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Text = "Hasta:"
        Me.dtpHasta.Enabled = False
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpHasta.Location = New System.Drawing.Point(0, 24)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(140, 24)
        Me.dtpHasta.TabIndex = 5
        Me.btnVerTodos.Location = New System.Drawing.Point(660, 136)
        Me.btnVerTodos.Margin = New System.Windows.Forms.Padding(0, 20, 10, 10)
        Me.btnVerTodos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerTodos.Name = "btnVerTodos"
        Me.btnVerTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodos.Size = New System.Drawing.Size(120, 34)
        Me.btnVerTodos.Text = "VER TODO"
        Me.btnVerTodos.TabIndex = 6
        Me.btnVerTodos.UseVisualStyleBackColor = True
        Me.lblFiltro.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblFiltro.Location = New System.Drawing.Point(24, 178)
        Me.lblFiltro.AutoSize = True
        Me.lblFiltro.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFiltro.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.lblFiltro.Name = "lblFiltro"
        Me.lblFiltro.Size = New System.Drawing.Size(1002, 22)
        Me.lblFiltro.Text = "Pedidos: Todos | Todas las fechas"
        Me.lblResumen.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblResumen.Location = New System.Drawing.Point(24, 208)
        Me.lblResumen.AutoSize = True
        Me.lblResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblResumen.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Size = New System.Drawing.Size(1002, 30)
        Me.lblResumen.Text = "Resumen del filtro:"
        Me.lblAyuda.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblAyuda.Location = New System.Drawing.Point(24, 243)
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(1002, 38)
        Me.lblAyuda.Text = "Los cancelados no generan saldo. Los pagos se muestran tal como están registrados; esta consulta no cobra ni procesa devoluciones."
        Me.dgvPedidos.AllowUserToAddRows = False
        Me.dgvPedidos.AllowUserToDeleteRows = False
        Me.dgvPedidos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dgvPedidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPedidos.Location = New System.Drawing.Point(24, 287)
        Me.dgvPedidos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPedidos.Name = "dgvPedidos"
        Me.dgvPedidos.Size = New System.Drawing.Size(1002, 203)
        Me.dgvPedidos.ReadOnly = True
        Me.dgvPedidos.MultiSelect = False
        Me.dgvPedidos.RowHeadersVisible = False
        Me.dgvPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPedidos.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvPedidos.TabIndex = 7
        Me.lblPagos.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblPagos.AutoEllipsis = True
        Me.lblPagos.Location = New System.Drawing.Point(24, 508)
        Me.lblPagos.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblPagos.Height = 30
        Me.lblPagos.AutoSize = False
        Me.lblPagos.Name = "lblPagos"
        Me.lblPagos.Size = New System.Drawing.Size(1002, 22)
        Me.lblPagos.Text = "PAGOS DEL PEDIDO"
        Me.dgvPagos.AllowUserToAddRows = False
        Me.dgvPagos.AllowUserToDeleteRows = False
        Me.dgvPagos.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dgvPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPagos.Location = New System.Drawing.Point(24, 535)
        Me.dgvPagos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPagos.Name = "dgvPagos"
        Me.dgvPagos.Size = New System.Drawing.Size(1002, 125)
        Me.dgvPagos.ReadOnly = True
        Me.dgvPagos.MultiSelect = False
        Me.dgvPagos.RowHeadersVisible = False
        Me.dgvPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPagos.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvPagos.TabIndex = 8
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblCantidad.AutoEllipsis = True
        Me.lblCantidad.Location = New System.Drawing.Point(24, 678)
        Me.lblCantidad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(850, 24)
        Me.lblCantidad.Text = "Pedidos encontrados: 0"
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        Me.btnCerrar.Location = New System.Drawing.Point(906, 670)
        Me.btnCerrar.Margin = System.Windows.Forms.Padding.Empty
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Size = New System.Drawing.Size(120, 34)
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.TabIndex = 9
        Me.btnCerrar.UseVisualStyleBackColor = True

        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 580)
        Me.Controls.Add(Me.Panel2)
        Me.CancelButton = Me.btnCerrar
        Me.Name = "frmHistorialClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ficha comercial del cliente"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvPedidos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).EndInit()

        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.Padding = System.Windows.Forms.Padding.Empty
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.RowCount = 6
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42.0!))
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.Margin = System.Windows.Forms.Padding.Empty
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(160, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 10, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblEstado)
        Me.pnlFiltro0.Controls.Add(Me.cmbEstado)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 10, 6)
        Me.pnlFiltro1.Controls.Add(Me.lblDesde)
        Me.pnlFiltro1.Controls.Add(Me.dtpDesde)
        Me.pnlFiltro2.Name = "pnlFiltro2"
        Me.pnlFiltro2.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro2.Margin = New System.Windows.Forms.Padding(0, 0, 10, 6)
        Me.pnlFiltro2.Controls.Add(Me.lblHasta)
        Me.pnlFiltro2.Controls.Add(Me.dtpHasta)
        Me.pnlFechas.Name = "pnlFechas"
        Me.pnlFechas.Size = New System.Drawing.Size(475, 58)
        Me.pnlFechas.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.chkFechas.Location = New System.Drawing.Point(0, 20)
        Me.pnlFiltro1.Location = New System.Drawing.Point(165, 0)
        Me.pnlFiltro2.Location = New System.Drawing.Point(320, 0)
        Me.pnlFechas.Controls.Add(Me.chkFechas)
        Me.pnlFechas.Controls.Add(Me.pnlFiltro1)
        Me.pnlFechas.Controls.Add(Me.pnlFiltro2)
        Me.flpFiltros.Controls.Add(Me.btnCambiarCliente)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.flpFiltros.Controls.Add(Me.btnActualizar)
        Me.flpFiltros.Controls.Add(Me.btnVerTodos)
        Me.flpFiltros.Controls.Add(Me.pnlFechas)
        Me.tlpResumen.Name = "tlpResumen"
        Me.tlpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpResumen.AutoSize = True
        Me.tlpResumen.ColumnCount = 1
        Me.tlpResumen.RowCount = 3
        Me.tlpResumen.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.tlpResumen.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResumen.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpResumen.Controls.Add(Me.lblFiltro, 0, 0)
        Me.tlpResumen.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpResumen.Controls.Add(Me.lblResumen, 0, 1)
        Me.tlpResumen.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpResumen.Controls.Add(Me.lblAyuda, 0, 2)
        Me.splHistorial.Name = "splHistorial"
        Me.splHistorial.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splHistorial.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.splHistorial.Size = New System.Drawing.Size(952, 300)
        Me.splHistorial.SplitterDistance = 160
        Me.splHistorial.Panel1MinSize = 80
        Me.splHistorial.Panel2MinSize = 80
        Me.splHistorial.SplitterWidth = 6
        Me.splHistorial.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.splHistorial.Panel1.Controls.Add(Me.dgvPedidos)
        Me.splHistorial.Panel2.Controls.Add(Me.dgvPagos)
        Me.splHistorial.Panel2.Controls.Add(Me.lblPagos)
        Me.tlpPie.Name = "tlpPie"
        Me.tlpPie.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpPie.ColumnCount = 2
        Me.tlpPie.RowCount = 1
        Me.tlpPie.Margin = System.Windows.Forms.Padding.Empty
        Me.tlpPie.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpPie.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.tlpPie.Controls.Add(Me.lblCantidad, 0, 0)
        Me.tlpPie.Controls.Add(Me.btnCerrar, 1, 0)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.lblCliente, 0, 1)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 2)
        Me.tlpContenido.Controls.Add(Me.tlpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.splHistorial, 0, 4)
        Me.tlpContenido.Controls.Add(Me.tlpPie, 0, 5)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents chkFechas As System.Windows.Forms.CheckBox
    Friend WithEvents lblDesde As System.Windows.Forms.Label
    Friend WithEvents lblHasta As System.Windows.Forms.Label
    Friend WithEvents lblFiltro As System.Windows.Forms.Label
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnVerTodos As System.Windows.Forms.Button
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblCliente As System.Windows.Forms.Label
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents lblResumen As System.Windows.Forms.Label
    Friend WithEvents lblAyuda As System.Windows.Forms.Label
    Friend WithEvents lblPagos As System.Windows.Forms.Label
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents cmbEstado As System.Windows.Forms.ComboBox
    Friend WithEvents dgvPedidos As System.Windows.Forms.DataGridView
    Friend WithEvents dgvPagos As System.Windows.Forms.DataGridView
    Friend WithEvents btnCambiarCliente As System.Windows.Forms.Button
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro2 As System.Windows.Forms.Panel
    Friend WithEvents pnlFechas As System.Windows.Forms.Panel
    Friend WithEvents tlpResumen As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents splHistorial As System.Windows.Forms.SplitContainer
    Friend WithEvents tlpPie As System.Windows.Forms.TableLayoutPanel
End Class

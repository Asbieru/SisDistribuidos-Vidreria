<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerKardex
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
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.pnlFiltro2 = New System.Windows.Forms.Panel()
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.lblVariante = New System.Windows.Forms.Label()
        Me.cmbVariante = New System.Windows.Forms.ComboBox()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.dgvMovimientos = New System.Windows.Forms.DataGridView()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(900, 535)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(21, 22)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(494, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "INVENTARIO - KARDEX (MOVIMIENTOS)"
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = False
        Me.lblDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblDesde.Location = New System.Drawing.Point(0, 0)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(140, 20)
        Me.lblDesde.TabIndex = 1
        Me.lblDesde.Text = "Desde"
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(0, 24)
        Me.dtpDesde.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(140, 24)
        Me.dtpDesde.TabIndex = 2
        '
        'lblHasta
        '
        Me.lblHasta.AutoSize = False
        Me.lblHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblHasta.Location = New System.Drawing.Point(0, 0)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(140, 20)
        Me.lblHasta.TabIndex = 3
        Me.lblHasta.Text = "Hasta"
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(0, 24)
        Me.dtpHasta.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(140, 24)
        Me.dtpHasta.TabIndex = 4
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = False
        Me.lblVariante.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblVariante.Location = New System.Drawing.Point(0, 0)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(330, 20)
        Me.lblVariante.TabIndex = 5
        Me.lblVariante.Text = "Producto"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(0, 24)
        Me.cmbVariante.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(330, 24)
        Me.cmbVariante.TabIndex = 6
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnFiltrar.Location = New System.Drawing.Point(715, 85)
        Me.btnFiltrar.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnFiltrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(110, 34)
        Me.btnFiltrar.TabIndex = 7
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'dgvMovimientos
        '
        Me.dgvMovimientos.AllowUserToAddRows = False
        Me.dgvMovimientos.AllowUserToDeleteRows = False
        Me.dgvMovimientos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvMovimientos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMovimientos.Location = New System.Drawing.Point(23, 125)
        Me.dgvMovimientos.MultiSelect = False
        Me.dgvMovimientos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMovimientos.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvMovimientos.Name = "dgvMovimientos"
        Me.dgvMovimientos.ReadOnly = True
        Me.dgvMovimientos.RowHeadersVisible = False
        Me.dgvMovimientos.RowHeadersWidth = 51
        Me.dgvMovimientos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMovimientos.Size = New System.Drawing.Size(854, 330)
        Me.dgvMovimientos.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvMovimientos.TabIndex = 8
        '
        'lblCantidad
        '
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCantidad.Location = New System.Drawing.Point(23, 464)
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(200, 17)
        Me.lblCantidad.TabIndex = 9
        Me.lblCantidad.Text = "Movimientos encontrados: 0"
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(795, 495)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 10
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmVerKardex
        '
        Me.AcceptButton = Me.btnFiltrar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmVerKardex"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventario - Kardex (historial de movimientos)"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvMovimientos, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.WrapContents = True
        Me.flpFiltros.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblDesde)
        Me.pnlFiltro0.Controls.Add(Me.dtpDesde)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Controls.Add(Me.lblHasta)
        Me.pnlFiltro1.Controls.Add(Me.dtpHasta)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.pnlFiltro2.Name = "pnlFiltro2"
        Me.pnlFiltro2.Size = New System.Drawing.Size(330, 58)
        Me.pnlFiltro2.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro2.Controls.Add(Me.lblVariante)
        Me.pnlFiltro2.Controls.Add(Me.cmbVariante)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro2)
        Me.flpFiltros.Controls.Add(Me.btnFiltrar)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.WrapContents = True
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Controls.Add(Me.lblCantidad)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.WrapContents = True
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvMovimientos, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblDesde As Label
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents lblHasta As Label
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents lblVariante As Label
    Friend WithEvents cmbVariante As ComboBox
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents dgvMovimientos As DataGridView
    Friend WithEvents lblCantidad As Label
    Friend WithEvents btnCerrar As Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro2 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

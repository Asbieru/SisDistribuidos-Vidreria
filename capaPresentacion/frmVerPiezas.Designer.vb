<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerPiezas
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
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblVariante = New System.Windows.Forms.Label()
        Me.cmbVariante = New System.Windows.Forms.ComboBox()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.dgvPiezas = New System.Windows.Forms.DataGridView()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.btnNuevaPieza = New System.Windows.Forms.Button()
        Me.btnEditarPieza = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblTitulo.Size = New System.Drawing.Size(506, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "INVENTARIO - PIEZAS DE VIDRIO Y ALUMINIO"
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = False
        Me.lblVariante.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblVariante.Location = New System.Drawing.Point(0, 0)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(350, 20)
        Me.lblVariante.TabIndex = 1
        Me.lblVariante.Text = "Producto"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(0, 24)
        Me.cmbVariante.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(350, 24)
        Me.cmbVariante.TabIndex = 2
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = False
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblEstado.Location = New System.Drawing.Point(0, 0)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(180, 20)
        Me.lblEstado.TabIndex = 3
        Me.lblEstado.Text = "Estado"
        '
        'cmbEstado
        '
        Me.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstado.FormattingEnabled = True
        Me.cmbEstado.Location = New System.Drawing.Point(0, 24)
        Me.cmbEstado.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(180, 24)
        Me.cmbEstado.TabIndex = 4
        '
        'btnBuscar
        '
        Me.btnBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnBuscar.Location = New System.Drawing.Point(620, 85)
        Me.btnBuscar.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnBuscar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(110, 34)
        Me.btnBuscar.TabIndex = 5
        Me.btnBuscar.Text = "BUSCAR"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'dgvPiezas
        '
        Me.dgvPiezas.AllowUserToAddRows = False
        Me.dgvPiezas.AllowUserToDeleteRows = False
        Me.dgvPiezas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPiezas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPiezas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPiezas.Location = New System.Drawing.Point(23, 125)
        Me.dgvPiezas.MultiSelect = False
        Me.dgvPiezas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPiezas.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvPiezas.Name = "dgvPiezas"
        Me.dgvPiezas.ReadOnly = True
        Me.dgvPiezas.RowHeadersVisible = False
        Me.dgvPiezas.RowHeadersWidth = 51
        Me.dgvPiezas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPiezas.Size = New System.Drawing.Size(854, 330)
        Me.dgvPiezas.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvPiezas.TabIndex = 6
        '
        'lblCantidad
        '
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCantidad.Location = New System.Drawing.Point(23, 464)
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(158, 17)
        Me.lblCantidad.TabIndex = 7
        Me.lblCantidad.Text = "Piezas encontradas: 0"
        '
        'btnNuevaPieza
        '
        Me.btnNuevaPieza.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnNuevaPieza.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnNuevaPieza.Location = New System.Drawing.Point(22, 495)
        Me.btnNuevaPieza.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnNuevaPieza.Name = "btnNuevaPieza"
        Me.btnNuevaPieza.Size = New System.Drawing.Size(120, 34)
        Me.btnNuevaPieza.TabIndex = 8
        Me.btnNuevaPieza.Text = "NUEVA PIEZA"
        Me.btnNuevaPieza.UseVisualStyleBackColor = True
        '
        'btnEditarPieza
        '
        Me.btnEditarPieza.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnEditarPieza.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEditarPieza.Location = New System.Drawing.Point(154, 495)
        Me.btnEditarPieza.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEditarPieza.Name = "btnEditarPieza"
        Me.btnEditarPieza.Size = New System.Drawing.Size(120, 34)
        Me.btnEditarPieza.TabIndex = 9
        Me.btnEditarPieza.Text = "EDITAR PIEZA"
        Me.btnEditarPieza.UseVisualStyleBackColor = True
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
        'frmVerPiezas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmVerPiezas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventario - Piezas de vidrio y aluminio"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.pnlFiltro0.Size = New System.Drawing.Size(350, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblVariante)
        Me.pnlFiltro0.Controls.Add(Me.cmbVariante)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(180, 58)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Controls.Add(Me.lblEstado)
        Me.pnlFiltro1.Controls.Add(Me.cmbEstado)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.flpFiltros.Controls.Add(Me.btnBuscar)
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
        Me.flpAcciones.Controls.Add(Me.btnNuevaPieza)
        Me.flpAcciones.Controls.Add(Me.btnEditarPieza)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvPiezas, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblVariante As Label
    Friend WithEvents cmbVariante As ComboBox
    Friend WithEvents lblEstado As Label
    Friend WithEvents cmbEstado As ComboBox
    Friend WithEvents btnBuscar As Button
    Friend WithEvents dgvPiezas As DataGridView
    Friend WithEvents lblCantidad As Label
    Friend WithEvents btnNuevaPieza As Button
    Friend WithEvents btnEditarPieza As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

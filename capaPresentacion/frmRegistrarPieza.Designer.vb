<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarPieza
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
        Me.tlpCampos = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblVariante = New System.Windows.Forms.Label()
        Me.cmbVariante = New System.Windows.Forms.ComboBox()
        Me.lblAncho = New System.Windows.Forms.Label()
        Me.nudAncho = New System.Windows.Forms.NumericUpDown()
        Me.lblAlto = New System.Windows.Forms.Label()
        Me.nudAlto = New System.Windows.Forms.NumericUpDown()
        Me.lblLargo = New System.Windows.Forms.Label()
        Me.nudLargo = New System.Windows.Forms.NumericUpDown()
        Me.lblObs = New System.Windows.Forms.Label()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.nudAncho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAlto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLargo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(640, 420)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(29, 25)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = System.Windows.Forms.Padding.Empty
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(156, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "NUEVA PIEZA"
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = False
        Me.lblVariante.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblVariante.Location = New System.Drawing.Point(31, 85)
        Me.lblVariante.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblVariante.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(75, 17)
        Me.lblVariante.TabIndex = 1
        Me.lblVariante.Text = "Producto *"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(171, 82)
        Me.cmbVariante.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbVariante.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(430, 24)
        Me.cmbVariante.TabIndex = 2
        '
        'lblAncho
        '
        Me.lblAncho.AutoSize = False
        Me.lblAncho.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblAncho.Location = New System.Drawing.Point(31, 125)
        Me.lblAncho.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAncho.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblAncho.Name = "lblAncho"
        Me.lblAncho.Size = New System.Drawing.Size(88, 17)
        Me.lblAncho.TabIndex = 3
        Me.lblAncho.Text = "Ancho (cm) *"
        '
        'nudAncho
        '
        Me.nudAncho.DecimalPlaces = 2
        Me.nudAncho.Location = New System.Drawing.Point(171, 123)
        Me.nudAncho.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudAncho.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.nudAncho.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.nudAncho.Name = "nudAncho"
        Me.nudAncho.Size = New System.Drawing.Size(225, 22)
        Me.nudAncho.TabIndex = 4
        '
        'lblAlto
        '
        Me.lblAlto.AutoSize = False
        Me.lblAlto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblAlto.Location = New System.Drawing.Point(31, 165)
        Me.lblAlto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAlto.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblAlto.Name = "lblAlto"
        Me.lblAlto.Size = New System.Drawing.Size(74, 17)
        Me.lblAlto.TabIndex = 5
        Me.lblAlto.Text = "Alto (cm) *"
        '
        'nudAlto
        '
        Me.nudAlto.DecimalPlaces = 2
        Me.nudAlto.Location = New System.Drawing.Point(171, 163)
        Me.nudAlto.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudAlto.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.nudAlto.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.nudAlto.Name = "nudAlto"
        Me.nudAlto.Size = New System.Drawing.Size(225, 22)
        Me.nudAlto.TabIndex = 6
        '
        'lblLargo
        '
        Me.lblLargo.AutoSize = False
        Me.lblLargo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblLargo.Location = New System.Drawing.Point(31, 205)
        Me.lblLargo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblLargo.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblLargo.Name = "lblLargo"
        Me.lblLargo.Size = New System.Drawing.Size(86, 17)
        Me.lblLargo.TabIndex = 7
        Me.lblLargo.Text = "Largo (cm) *"
        '
        'nudLargo
        '
        Me.nudLargo.DecimalPlaces = 2
        Me.nudLargo.Location = New System.Drawing.Point(171, 203)
        Me.nudLargo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudLargo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.nudLargo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.nudLargo.Name = "nudLargo"
        Me.nudLargo.Size = New System.Drawing.Size(225, 22)
        Me.nudLargo.TabIndex = 8
        '
        'lblObs
        '
        Me.lblObs.AutoSize = False
        Me.lblObs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblObs.Location = New System.Drawing.Point(31, 245)
        Me.lblObs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblObs.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblObs.Name = "lblObs"
        Me.lblObs.Size = New System.Drawing.Size(86, 17)
        Me.lblObs.TabIndex = 9
        Me.lblObs.Text = "Observación"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(171, 242)
        Me.txtObs.MaxLength = 255
        Me.txtObs.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtObs.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(430, 22)
        Me.txtObs.TabIndex = 10
        '
        'lblAyuda
        '
        Me.lblAyuda.Location = New System.Drawing.Point(31, 285)
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 8, 0, 12)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(570, 54)
        Me.lblAyuda.TabIndex = 11
        Me.lblAyuda.Text = "* Obligatorio según el material: el vidrio usa ancho y alto; el aluminio, largo." &
    " La pieza se registra como inventario INICIAL (material que ya estaba en el alma" &
    "cén). Las piezas compradas se registran desde el módulo de Compras."
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(351, 365)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 12
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(481, 365)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmRegistrarPieza
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(700, 420)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmRegistrarPieza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar pieza"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.nudAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLargo, System.ComponentModel.ISupportInitialize).EndInit()

        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.RowCount = 4
        Me.tlpContenido.Margin = System.Windows.Forms.Padding.Empty
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpCampos.Name = "tlpCampos"
        Me.tlpCampos.Dock = System.Windows.Forms.DockStyle.Top
        Me.tlpCampos.AutoSize = True
        Me.tlpCampos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpCampos.ColumnCount = 2
        Me.tlpCampos.RowCount = 5
        Me.tlpCampos.Margin = System.Windows.Forms.Padding.Empty
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblVariante, 0, 0)
        Me.tlpCampos.Controls.Add(Me.cmbVariante, 1, 0)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblAncho, 0, 1)
        Me.tlpCampos.Controls.Add(Me.nudAncho, 1, 1)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblAlto, 0, 2)
        Me.tlpCampos.Controls.Add(Me.nudAlto, 1, 2)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblLargo, 0, 3)
        Me.tlpCampos.Controls.Add(Me.nudLargo, 1, 3)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblObs, 0, 4)
        Me.tlpCampos.Controls.Add(Me.txtObs, 1, 4)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpAcciones.Margin = System.Windows.Forms.Padding.Empty
        Me.flpAcciones.Controls.Add(Me.btnCancelar)
        Me.flpAcciones.Controls.Add(Me.btnGuardar)
        Me.tlpContenido.Controls.Add(Me.lblAyuda, 0, 2)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.tlpCampos, 0, 1)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 3)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblVariante As Label
    Friend WithEvents cmbVariante As ComboBox
    Friend WithEvents lblAncho As Label
    Friend WithEvents nudAncho As NumericUpDown
    Friend WithEvents lblAlto As Label
    Friend WithEvents nudAlto As NumericUpDown
    Friend WithEvents lblLargo As Label
    Friend WithEvents nudLargo As NumericUpDown
    Friend WithEvents lblObs As Label
    Friend WithEvents txtObs As TextBox
    Friend WithEvents lblAyuda As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tlpCampos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

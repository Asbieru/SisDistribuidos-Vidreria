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
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblVariante)
        Me.Panel2.Controls.Add(Me.cmbVariante)
        Me.Panel2.Controls.Add(Me.lblAncho)
        Me.Panel2.Controls.Add(Me.nudAncho)
        Me.Panel2.Controls.Add(Me.lblAlto)
        Me.Panel2.Controls.Add(Me.nudAlto)
        Me.Panel2.Controls.Add(Me.lblLargo)
        Me.Panel2.Controls.Add(Me.nudLargo)
        Me.Panel2.Controls.Add(Me.lblObs)
        Me.Panel2.Controls.Add(Me.txtObs)
        Me.Panel2.Controls.Add(Me.lblAyuda)
        Me.Panel2.Controls.Add(Me.btnGuardar)
        Me.Panel2.Controls.Add(Me.btnCancelar)
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
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(156, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "NUEVA PIEZA"
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = True
        Me.lblVariante.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblVariante.Location = New System.Drawing.Point(31, 85)
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
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(430, 24)
        Me.cmbVariante.TabIndex = 2
        '
        'lblAncho
        '
        Me.lblAncho.AutoSize = True
        Me.lblAncho.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblAncho.Location = New System.Drawing.Point(31, 125)
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
        Me.nudAncho.Name = "nudAncho"
        Me.nudAncho.Size = New System.Drawing.Size(130, 22)
        Me.nudAncho.TabIndex = 4
        '
        'lblAlto
        '
        Me.lblAlto.AutoSize = True
        Me.lblAlto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblAlto.Location = New System.Drawing.Point(31, 165)
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
        Me.nudAlto.Name = "nudAlto"
        Me.nudAlto.Size = New System.Drawing.Size(130, 22)
        Me.nudAlto.TabIndex = 6
        '
        'lblLargo
        '
        Me.lblLargo.AutoSize = True
        Me.lblLargo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblLargo.Location = New System.Drawing.Point(31, 205)
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
        Me.nudLargo.Name = "nudLargo"
        Me.nudLargo.Size = New System.Drawing.Size(130, 22)
        Me.nudLargo.TabIndex = 8
        '
        'lblObs
        '
        Me.lblObs.AutoSize = True
        Me.lblObs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblObs.Location = New System.Drawing.Point(31, 245)
        Me.lblObs.Name = "lblObs"
        Me.lblObs.Size = New System.Drawing.Size(86, 17)
        Me.lblObs.TabIndex = 9
        Me.lblObs.Text = "Observación"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(171, 242)
        Me.txtObs.MaxLength = 255
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(430, 22)
        Me.txtObs.TabIndex = 10
        '
        'lblAyuda
        '
        Me.lblAyuda.Location = New System.Drawing.Point(31, 285)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(570, 54)
        Me.lblAyuda.TabIndex = 11
        Me.lblAyuda.Text = "* Obligatorio según el material: el vidrio usa ancho y alto; el aluminio, largo." &
    " La pieza se registra como inventario INICIAL (material que ya estaba en el alma" &
    "cén). Las piezas compradas se registran desde el módulo de Compras."
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(351, 365)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 12
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(481, 365)
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
        Me.ClientSize = New System.Drawing.Size(640, 420)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRegistrarPieza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar pieza"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.nudAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLargo, System.ComponentModel.ISupportInitialize).EndInit()
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
End Class

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
        Me.lblNota = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        CType(Me.nudAncho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudAlto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLargo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = True
        Me.lblVariante.Location = New System.Drawing.Point(12, 15)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(53, 13)
        Me.lblVariante.TabIndex = 0
        Me.lblVariante.Text = "Producto:"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(90, 12)
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(355, 21)
        Me.cmbVariante.TabIndex = 1
        '
        'lblAncho
        '
        Me.lblAncho.AutoSize = True
        Me.lblAncho.Location = New System.Drawing.Point(12, 50)
        Me.lblAncho.Name = "lblAncho"
        Me.lblAncho.Size = New System.Drawing.Size(65, 13)
        Me.lblAncho.TabIndex = 2
        Me.lblAncho.Text = "Ancho (cm):"
        '
        'nudAncho
        '
        Me.nudAncho.DecimalPlaces = 2
        Me.nudAncho.Location = New System.Drawing.Point(90, 47)
        Me.nudAncho.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudAncho.Name = "nudAncho"
        Me.nudAncho.Size = New System.Drawing.Size(100, 20)
        Me.nudAncho.TabIndex = 3
        '
        'lblAlto
        '
        Me.lblAlto.AutoSize = True
        Me.lblAlto.Location = New System.Drawing.Point(12, 80)
        Me.lblAlto.Name = "lblAlto"
        Me.lblAlto.Size = New System.Drawing.Size(53, 13)
        Me.lblAlto.TabIndex = 4
        Me.lblAlto.Text = "Alto (cm):"
        '
        'nudAlto
        '
        Me.nudAlto.DecimalPlaces = 2
        Me.nudAlto.Location = New System.Drawing.Point(90, 77)
        Me.nudAlto.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudAlto.Name = "nudAlto"
        Me.nudAlto.Size = New System.Drawing.Size(100, 20)
        Me.nudAlto.TabIndex = 5
        '
        'lblLargo
        '
        Me.lblLargo.AutoSize = True
        Me.lblLargo.Location = New System.Drawing.Point(12, 110)
        Me.lblLargo.Name = "lblLargo"
        Me.lblLargo.Size = New System.Drawing.Size(62, 13)
        Me.lblLargo.TabIndex = 6
        Me.lblLargo.Text = "Largo (cm):"
        '
        'nudLargo
        '
        Me.nudLargo.DecimalPlaces = 2
        Me.nudLargo.Location = New System.Drawing.Point(90, 107)
        Me.nudLargo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudLargo.Name = "nudLargo"
        Me.nudLargo.Size = New System.Drawing.Size(100, 20)
        Me.nudLargo.TabIndex = 7
        '
        'lblObs
        '
        Me.lblObs.AutoSize = True
        Me.lblObs.Location = New System.Drawing.Point(12, 140)
        Me.lblObs.Name = "lblObs"
        Me.lblObs.Size = New System.Drawing.Size(70, 13)
        Me.lblObs.TabIndex = 8
        Me.lblObs.Text = "Observación:"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(90, 137)
        Me.txtObs.MaxLength = 255
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(355, 20)
        Me.txtObs.TabIndex = 9
        '
        'lblNota
        '
        Me.lblNota.ForeColor = System.Drawing.Color.Gray
        Me.lblNota.Location = New System.Drawing.Point(12, 170)
        Me.lblNota.Name = "lblNota"
        Me.lblNota.Size = New System.Drawing.Size(433, 30)
        Me.lblNota.TabIndex = 10
        Me.lblNota.Text = "La pieza se registra como inventario INICIAL (material que ya estaba en el almacén). Las piezas compradas se registran desde el módulo de Compras."
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(235, 212)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 30)
        Me.btnGuardar.TabIndex = 11
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(345, 212)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 30)
        Me.btnCancelar.TabIndex = 12
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmRegistrarPieza
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(460, 256)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.lblNota)
        Me.Controls.Add(Me.txtObs)
        Me.Controls.Add(Me.lblObs)
        Me.Controls.Add(Me.nudLargo)
        Me.Controls.Add(Me.lblLargo)
        Me.Controls.Add(Me.nudAlto)
        Me.Controls.Add(Me.lblAlto)
        Me.Controls.Add(Me.nudAncho)
        Me.Controls.Add(Me.lblAncho)
        Me.Controls.Add(Me.cmbVariante)
        Me.Controls.Add(Me.lblVariante)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRegistrarPieza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar pieza"
        CType(Me.nudAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLargo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

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
    Friend WithEvents lblNota As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class

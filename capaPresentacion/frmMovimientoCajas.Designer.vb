<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovimientoCajas
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
        Me.lblProducto = New System.Windows.Forms.Label()
        Me.lblStock = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.nudCantidad = New System.Windows.Forms.NumericUpDown()
        Me.lblObs = New System.Windows.Forms.Label()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblProducto
        '
        Me.lblProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblProducto.Location = New System.Drawing.Point(12, 12)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(436, 20)
        Me.lblProducto.TabIndex = 0
        Me.lblProducto.Text = "Producto"
        '
        'lblStock
        '
        Me.lblStock.AutoSize = True
        Me.lblStock.Location = New System.Drawing.Point(12, 38)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(70, 13)
        Me.lblStock.TabIndex = 1
        Me.lblStock.Text = "Stock actual:"
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(12, 72)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(66, 13)
        Me.lblTipo.TabIndex = 2
        Me.lblTipo.Text = "Movimiento:"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(130, 69)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(200, 21)
        Me.cmbTipo.TabIndex = 3
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(12, 104)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(98, 13)
        Me.lblCantidad.TabIndex = 4
        Me.lblCantidad.Text = "Cantidad de cajas:"
        '
        'nudCantidad
        '
        Me.nudCantidad.Location = New System.Drawing.Point(130, 101)
        Me.nudCantidad.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCantidad.Name = "nudCantidad"
        Me.nudCantidad.Size = New System.Drawing.Size(100, 20)
        Me.nudCantidad.TabIndex = 5
        '
        'lblObs
        '
        Me.lblObs.AutoSize = True
        Me.lblObs.Location = New System.Drawing.Point(12, 136)
        Me.lblObs.Name = "lblObs"
        Me.lblObs.Size = New System.Drawing.Size(70, 13)
        Me.lblObs.TabIndex = 6
        Me.lblObs.Text = "Observación:"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(130, 133)
        Me.txtObs.MaxLength = 255
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(318, 20)
        Me.txtObs.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(238, 172)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 30)
        Me.btnGuardar.TabIndex = 8
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(348, 172)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 30)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmMovimientoCajas
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(460, 216)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.txtObs)
        Me.Controls.Add(Me.lblObs)
        Me.Controls.Add(Me.nudCantidad)
        Me.Controls.Add(Me.lblCantidad)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.lblTipo)
        Me.Controls.Add(Me.lblStock)
        Me.Controls.Add(Me.lblProducto)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMovimientoCajas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar movimiento de cajas"
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblProducto As Label
    Friend WithEvents lblStock As Label
    Friend WithEvents lblTipo As Label
    Friend WithEvents cmbTipo As ComboBox
    Friend WithEvents lblCantidad As Label
    Friend WithEvents nudCantidad As NumericUpDown
    Friend WithEvents lblObs As Label
    Friend WithEvents txtObs As TextBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class

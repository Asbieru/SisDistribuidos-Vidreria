<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockMinimo
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
        Me.lblExistencias = New System.Windows.Forms.Label()
        Me.lblMinimo = New System.Windows.Forms.Label()
        Me.nudMinimo = New System.Windows.Forms.NumericUpDown()
        Me.lblNota = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        CType(Me.nudMinimo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblProducto
        '
        Me.lblProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblProducto.Location = New System.Drawing.Point(12, 12)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(396, 34)
        Me.lblProducto.TabIndex = 0
        Me.lblProducto.Text = "Producto"
        '
        'lblExistencias
        '
        Me.lblExistencias.AutoSize = True
        Me.lblExistencias.Location = New System.Drawing.Point(12, 52)
        Me.lblExistencias.Name = "lblExistencias"
        Me.lblExistencias.Size = New System.Drawing.Size(110, 13)
        Me.lblExistencias.TabIndex = 1
        Me.lblExistencias.Text = "Existencias actuales:"
        '
        'lblMinimo
        '
        Me.lblMinimo.AutoSize = True
        Me.lblMinimo.Location = New System.Drawing.Point(12, 84)
        Me.lblMinimo.Name = "lblMinimo"
        Me.lblMinimo.Size = New System.Drawing.Size(77, 13)
        Me.lblMinimo.TabIndex = 2
        Me.lblMinimo.Text = "Stock mínimo:"
        '
        'nudMinimo
        '
        Me.nudMinimo.Location = New System.Drawing.Point(110, 81)
        Me.nudMinimo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudMinimo.Name = "nudMinimo"
        Me.nudMinimo.Size = New System.Drawing.Size(90, 20)
        Me.nudMinimo.TabIndex = 3
        '
        'lblNota
        '
        Me.lblNota.ForeColor = System.Drawing.Color.Gray
        Me.lblNota.Location = New System.Drawing.Point(12, 108)
        Me.lblNota.Name = "lblNota"
        Me.lblNota.Size = New System.Drawing.Size(396, 28)
        Me.lblNota.TabIndex = 4
        Me.lblNota.Text = "Vidrio y aluminio: cantidad de piezas disponibles. Tornillos y tarugos: cantidad de cajas. Con 0 no se muestra alerta."
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(198, 142)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 30)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(308, 142)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(100, 30)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmStockMinimo
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(420, 184)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.lblNota)
        Me.Controls.Add(Me.nudMinimo)
        Me.Controls.Add(Me.lblMinimo)
        Me.Controls.Add(Me.lblExistencias)
        Me.Controls.Add(Me.lblProducto)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStockMinimo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editar stock mínimo"
        CType(Me.nudMinimo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblProducto As Label
    Friend WithEvents lblExistencias As Label
    Friend WithEvents lblMinimo As Label
    Friend WithEvents nudMinimo As NumericUpDown
    Friend WithEvents lblNota As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerCajas
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
        Me.dgvCajas = New System.Windows.Forms.DataGridView()
        Me.btnMovimiento = New System.Windows.Forms.Button()
        Me.lblAyuda = New System.Windows.Forms.Label()
        CType(Me.dgvCajas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvCajas
        '
        Me.dgvCajas.AllowUserToAddRows = False
        Me.dgvCajas.AllowUserToDeleteRows = False
        Me.dgvCajas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvCajas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCajas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCajas.Location = New System.Drawing.Point(12, 12)
        Me.dgvCajas.MultiSelect = False
        Me.dgvCajas.Name = "dgvCajas"
        Me.dgvCajas.ReadOnly = True
        Me.dgvCajas.RowHeadersVisible = False
        Me.dgvCajas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCajas.Size = New System.Drawing.Size(756, 400)
        Me.dgvCajas.TabIndex = 0
        '
        'btnMovimiento
        '
        Me.btnMovimiento.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMovimiento.Location = New System.Drawing.Point(12, 422)
        Me.btnMovimiento.Name = "btnMovimiento"
        Me.btnMovimiento.Size = New System.Drawing.Size(200, 32)
        Me.btnMovimiento.TabIndex = 1
        Me.btnMovimiento.Text = "Registrar movimiento"
        Me.btnMovimiento.UseVisualStyleBackColor = True
        '
        'lblAyuda
        '
        Me.lblAyuda.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(225, 432)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(330, 13)
        Me.lblAyuda.TabIndex = 2
        Me.lblAyuda.Text = "Doble clic en un producto para registrar una entrada, salida o ajuste."
        '
        'frmVerCajas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(780, 466)
        Me.Controls.Add(Me.lblAyuda)
        Me.Controls.Add(Me.btnMovimiento)
        Me.Controls.Add(Me.dgvCajas)
        Me.MinimumSize = New System.Drawing.Size(600, 300)
        Me.Name = "frmVerCajas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventario - Tornillos y tarugos (cajas)"
        CType(Me.dgvCajas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvCajas As DataGridView
    Friend WithEvents btnMovimiento As Button
    Friend WithEvents lblAyuda As Label
End Class

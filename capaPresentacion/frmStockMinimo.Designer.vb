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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblProducto = New System.Windows.Forms.Label()
        Me.txtProducto = New System.Windows.Forms.TextBox()
        Me.lblExistencias = New System.Windows.Forms.Label()
        Me.txtExistencias = New System.Windows.Forms.TextBox()
        Me.lblMinimo = New System.Windows.Forms.Label()
        Me.nudMinimo = New System.Windows.Forms.NumericUpDown()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.nudMinimo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblProducto)
        Me.Panel2.Controls.Add(Me.txtProducto)
        Me.Panel2.Controls.Add(Me.lblExistencias)
        Me.Panel2.Controls.Add(Me.txtExistencias)
        Me.Panel2.Controls.Add(Me.lblMinimo)
        Me.Panel2.Controls.Add(Me.nudMinimo)
        Me.Panel2.Controls.Add(Me.lblAyuda)
        Me.Panel2.Controls.Add(Me.btnGuardar)
        Me.Panel2.Controls.Add(Me.btnCancelar)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(580, 300)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(29, 25)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(178, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "STOCK MÍNIMO"
        '
        'lblProducto
        '
        Me.lblProducto.AutoSize = True
        Me.lblProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblProducto.Location = New System.Drawing.Point(31, 85)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(65, 17)
        Me.lblProducto.TabIndex = 1
        Me.lblProducto.Text = "Producto"
        '
        'txtProducto
        '
        Me.txtProducto.Location = New System.Drawing.Point(171, 82)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.ReadOnly = True
        Me.txtProducto.Size = New System.Drawing.Size(380, 22)
        Me.txtProducto.TabIndex = 2
        Me.txtProducto.TabStop = False
        '
        'lblExistencias
        '
        Me.lblExistencias.AutoSize = True
        Me.lblExistencias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblExistencias.Location = New System.Drawing.Point(31, 125)
        Me.lblExistencias.Name = "lblExistencias"
        Me.lblExistencias.Size = New System.Drawing.Size(80, 17)
        Me.lblExistencias.TabIndex = 3
        Me.lblExistencias.Text = "Existencias"
        '
        'txtExistencias
        '
        Me.txtExistencias.Location = New System.Drawing.Point(171, 122)
        Me.txtExistencias.Name = "txtExistencias"
        Me.txtExistencias.ReadOnly = True
        Me.txtExistencias.Size = New System.Drawing.Size(130, 22)
        Me.txtExistencias.TabIndex = 4
        Me.txtExistencias.TabStop = False
        '
        'lblMinimo
        '
        Me.lblMinimo.AutoSize = True
        Me.lblMinimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblMinimo.Location = New System.Drawing.Point(31, 165)
        Me.lblMinimo.Name = "lblMinimo"
        Me.lblMinimo.Size = New System.Drawing.Size(105, 17)
        Me.lblMinimo.TabIndex = 5
        Me.lblMinimo.Text = "Stock mínimo *"
        '
        'nudMinimo
        '
        Me.nudMinimo.Location = New System.Drawing.Point(171, 163)
        Me.nudMinimo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudMinimo.Name = "nudMinimo"
        Me.nudMinimo.Size = New System.Drawing.Size(130, 22)
        Me.nudMinimo.TabIndex = 6
        '
        'lblAyuda
        '
        Me.lblAyuda.Location = New System.Drawing.Point(31, 202)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(520, 36)
        Me.lblAyuda.TabIndex = 7
        Me.lblAyuda.Text = "Vidrio y aluminio: cantidad de piezas disponibles. Tornillos, tarugos y accesorio" &
    "s: cantidad de cajas o unidades. Con 0 no se muestra alerta."
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(301, 250)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 8
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(431, 250)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmStockMinimo
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(580, 300)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmStockMinimo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editar stock mínimo"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.nudMinimo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblProducto As Label
    Friend WithEvents txtProducto As TextBox
    Friend WithEvents lblExistencias As Label
    Friend WithEvents txtExistencias As TextBox
    Friend WithEvents lblMinimo As Label
    Friend WithEvents nudMinimo As NumericUpDown
    Friend WithEvents lblAyuda As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class

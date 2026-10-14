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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblProducto = New System.Windows.Forms.Label()
        Me.txtProducto = New System.Windows.Forms.TextBox()
        Me.lblStock = New System.Windows.Forms.Label()
        Me.txtStock = New System.Windows.Forms.TextBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.nudCantidad = New System.Windows.Forms.NumericUpDown()
        Me.lblObs = New System.Windows.Forms.Label()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblProducto)
        Me.Panel2.Controls.Add(Me.txtProducto)
        Me.Panel2.Controls.Add(Me.lblStock)
        Me.Panel2.Controls.Add(Me.txtStock)
        Me.Panel2.Controls.Add(Me.lblTipo)
        Me.Panel2.Controls.Add(Me.cmbTipo)
        Me.Panel2.Controls.Add(Me.lblCantidad)
        Me.Panel2.Controls.Add(Me.nudCantidad)
        Me.Panel2.Controls.Add(Me.lblObs)
        Me.Panel2.Controls.Add(Me.txtObs)
        Me.Panel2.Controls.Add(Me.lblAyuda)
        Me.Panel2.Controls.Add(Me.btnGuardar)
        Me.Panel2.Controls.Add(Me.btnCancelar)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(600, 380)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(29, 25)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(271, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "MOVIMIENTO DE STOCK"
        '
        'lblProducto
        '
        Me.lblProducto.AutoSize = True
        Me.lblProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtProducto.Size = New System.Drawing.Size(400, 22)
        Me.txtProducto.TabIndex = 2
        Me.txtProducto.TabStop = False
        '
        'lblStock
        '
        Me.lblStock.AutoSize = True
        Me.lblStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStock.Location = New System.Drawing.Point(31, 125)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(86, 17)
        Me.lblStock.TabIndex = 3
        Me.lblStock.Text = "Stock actual"
        '
        'txtStock
        '
        Me.txtStock.Location = New System.Drawing.Point(171, 122)
        Me.txtStock.Name = "txtStock"
        Me.txtStock.ReadOnly = True
        Me.txtStock.Size = New System.Drawing.Size(180, 22)
        Me.txtStock.TabIndex = 4
        Me.txtStock.TabStop = False
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipo.Location = New System.Drawing.Point(31, 165)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(91, 17)
        Me.lblTipo.TabIndex = 5
        Me.lblTipo.Text = "Movimiento *"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(171, 162)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(250, 24)
        Me.cmbTipo.TabIndex = 6
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad.Location = New System.Drawing.Point(31, 205)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(73, 17)
        Me.lblCantidad.TabIndex = 7
        Me.lblCantidad.Text = "Cantidad *"
        '
        'nudCantidad
        '
        Me.nudCantidad.Location = New System.Drawing.Point(171, 203)
        Me.nudCantidad.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCantidad.Name = "nudCantidad"
        Me.nudCantidad.Size = New System.Drawing.Size(130, 22)
        Me.nudCantidad.TabIndex = 8
        '
        'lblObs
        '
        Me.lblObs.AutoSize = True
        Me.lblObs.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.txtObs.Size = New System.Drawing.Size(400, 22)
        Me.txtObs.TabIndex = 10
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(168, 280)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(289, 16)
        Me.lblAyuda.TabIndex = 11
        Me.lblAyuda.Text = "* Obligatorio. La observación es opcional."
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(321, 325)
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
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(451, 325)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmMovimientoCajas
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(600, 380)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMovimientoCajas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar movimiento de stock"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblProducto As Label
    Friend WithEvents txtProducto As TextBox
    Friend WithEvents lblStock As Label
    Friend WithEvents txtStock As TextBox
    Friend WithEvents lblTipo As Label
    Friend WithEvents cmbTipo As ComboBox
    Friend WithEvents lblCantidad As Label
    Friend WithEvents nudCantidad As NumericUpDown
    Friend WithEvents lblObs As Label
    Friend WithEvents txtObs As TextBox
    Friend WithEvents lblAyuda As Label
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class

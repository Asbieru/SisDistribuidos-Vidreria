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
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpCampos = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
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
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = System.Windows.Forms.Padding.Empty
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(271, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "MOVIMIENTO DE STOCK"
        '
        'lblProducto
        '
        Me.lblProducto.AutoSize = False
        Me.lblProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblProducto.Location = New System.Drawing.Point(31, 85)
        Me.lblProducto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblProducto.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblProducto.Name = "lblProducto"
        Me.lblProducto.Size = New System.Drawing.Size(65, 17)
        Me.lblProducto.TabIndex = 1
        Me.lblProducto.Text = "Producto"
        '
        'txtProducto
        '
        Me.txtProducto.Location = New System.Drawing.Point(171, 82)
        Me.txtProducto.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtProducto.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.ReadOnly = True
        Me.txtProducto.Size = New System.Drawing.Size(400, 22)
        Me.txtProducto.TabIndex = 2
        Me.txtProducto.TabStop = False
        '
        'lblStock
        '
        Me.lblStock.AutoSize = False
        Me.lblStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblStock.Location = New System.Drawing.Point(31, 125)
        Me.lblStock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStock.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(86, 17)
        Me.lblStock.TabIndex = 3
        Me.lblStock.Text = "Stock actual"
        '
        'txtStock
        '
        Me.txtStock.Location = New System.Drawing.Point(171, 122)
        Me.txtStock.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtStock.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtStock.Name = "txtStock"
        Me.txtStock.ReadOnly = True
        Me.txtStock.Size = New System.Drawing.Size(180, 22)
        Me.txtStock.TabIndex = 4
        Me.txtStock.TabStop = False
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = False
        Me.lblTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTipo.Location = New System.Drawing.Point(31, 165)
        Me.lblTipo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
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
        Me.cmbTipo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(250, 24)
        Me.cmbTipo.TabIndex = 6
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = False
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCantidad.Location = New System.Drawing.Point(31, 205)
        Me.lblCantidad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(73, 17)
        Me.lblCantidad.TabIndex = 7
        Me.lblCantidad.Text = "Cantidad *"
        '
        'nudCantidad
        '
        Me.nudCantidad.Location = New System.Drawing.Point(171, 203)
        Me.nudCantidad.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCantidad.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.nudCantidad.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.nudCantidad.Name = "nudCantidad"
        Me.nudCantidad.Size = New System.Drawing.Size(225, 22)
        Me.nudCantidad.TabIndex = 8
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
        Me.txtObs.Size = New System.Drawing.Size(400, 22)
        Me.txtObs.TabIndex = 10
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(168, 280)
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 8, 0, 12)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(289, 16)
        Me.lblAyuda.TabIndex = 11
        Me.lblAyuda.Text = "* Obligatorio. La observación es opcional."
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(321, 325)
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
        Me.btnCancelar.Location = New System.Drawing.Point(451, 325)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
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
        Me.ClientSize = New System.Drawing.Size(700, 400)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmMovimientoCajas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar movimiento de stock"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.nudCantidad, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.tlpCampos.Controls.Add(Me.lblProducto, 0, 0)
        Me.tlpCampos.Controls.Add(Me.txtProducto, 1, 0)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblStock, 0, 1)
        Me.tlpCampos.Controls.Add(Me.txtStock, 1, 1)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblTipo, 0, 2)
        Me.tlpCampos.Controls.Add(Me.cmbTipo, 1, 2)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblCantidad, 0, 3)
        Me.tlpCampos.Controls.Add(Me.nudCantidad, 1, 3)
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
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tlpCampos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

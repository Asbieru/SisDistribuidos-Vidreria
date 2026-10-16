<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarProductos
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
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.cboCategoria = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblModo = New System.Windows.Forms.Label()
        Me.lblId = New System.Windows.Forms.Label()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.lblTelefono = New System.Windows.Forms.Label()
        Me.lblDireccion = New System.Windows.Forms.Label()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.txtTipo = New System.Windows.Forms.TextBox()
        Me.txtColor = New System.Windows.Forms.TextBox()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(733, 581)
        Me.Panel2.TabIndex = 0
        '
        'chkActivo
        '
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.chkActivo.Location = New System.Drawing.Point(174, 415)
        Me.chkActivo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.chkActivo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(77, 24)
        Me.chkActivo.TabIndex = 33
        Me.chkActivo.Text = "Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(174, 267)
        Me.txtNombre.MaxLength = 20
        Me.txtNombre.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(480, 22)
        Me.txtNombre.TabIndex = 32
        '
        'cboCategoria
        '
        Me.cboCategoria.FormattingEnabled = True
        Me.cboCategoria.Location = New System.Drawing.Point(171, 138)
        Me.cboCategoria.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cboCategoria.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cboCategoria.Name = "cboCategoria"
        Me.cboCategoria.Size = New System.Drawing.Size(225, 24)
        Me.cboCategoria.TabIndex = 31
        '
        'Label1
        '
        Me.Label1.AutoSize = False
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(31, 314)
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 20)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Descripción"
        '
        'lblModo
        '
        Me.lblModo.AutoSize = True
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblModo.Location = New System.Drawing.Point(29, 33)
        Me.lblModo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblModo.Margin = System.Windows.Forms.Padding.Empty
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Size = New System.Drawing.Size(289, 31)
        Me.lblModo.TabIndex = 15
        Me.lblModo.Text = "NUEVO PRODUCTO"
        '
        'lblId
        '
        Me.lblId.AutoSize = False
        Me.lblId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblId.Location = New System.Drawing.Point(31, 95)
        Me.lblId.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblId.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblId.Name = "lblId"
        Me.lblId.Size = New System.Drawing.Size(26, 20)
        Me.lblId.TabIndex = 16
        Me.lblId.Text = "ID"
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = False
        Me.lblNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblNombre.Location = New System.Drawing.Point(31, 138)
        Me.lblNombre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(92, 20)
        Me.lblNombre.TabIndex = 18
        Me.lblNombre.Text = "Categoría *"
        '
        'lblDocumento
        '
        Me.lblDocumento.AutoSize = False
        Me.lblDocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblDocumento.Location = New System.Drawing.Point(31, 182)
        Me.lblDocumento.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumento.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Size = New System.Drawing.Size(47, 20)
        Me.lblDocumento.TabIndex = 20
        Me.lblDocumento.Text = "Tipo*"
        '
        'lblTelefono
        '
        Me.lblTelefono.AutoSize = False
        Me.lblTelefono.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblTelefono.Location = New System.Drawing.Point(31, 226)
        Me.lblTelefono.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTelefono.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Size = New System.Drawing.Size(49, 20)
        Me.lblTelefono.TabIndex = 22
        Me.lblTelefono.Text = "Color"
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = False
        Me.lblDireccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblDireccion.Location = New System.Drawing.Point(31, 267)
        Me.lblDireccion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDireccion.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(68, 20)
        Me.lblDireccion.TabIndex = 24
        Me.lblDireccion.Text = "Nombre"
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(168, 461)
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 8, 0, 12)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(289, 16)
        Me.lblAyuda.TabIndex = 26
        Me.lblAyuda.Text = "* Obligatorio. Los demás datos son opcionales."
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(171, 91)
        Me.txtId.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.txtId.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(130, 22)
        Me.txtId.TabIndex = 17
        Me.txtId.TabStop = False
        '
        'txtTipo
        '
        Me.txtTipo.Location = New System.Drawing.Point(171, 180)
        Me.txtTipo.MaxLength = 20
        Me.txtTipo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtTipo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtTipo.Name = "txtTipo"
        Me.txtTipo.Size = New System.Drawing.Size(225, 22)
        Me.txtTipo.TabIndex = 21
        '
        'txtColor
        '
        Me.txtColor.Location = New System.Drawing.Point(171, 223)
        Me.txtColor.MaxLength = 20
        Me.txtColor.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtColor.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtColor.Name = "txtColor"
        Me.txtColor.Size = New System.Drawing.Size(225, 22)
        Me.txtColor.TabIndex = 23
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtDescripcion.Location = New System.Drawing.Point(174, 318)
        Me.txtDescripcion.MaxLength = 255
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtDescripcion.Height = 62
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescripcion.Size = New System.Drawing.Size(480, 60)
        Me.txtDescripcion.TabIndex = 25
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(404, 503)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 27
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(534, 503)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 28
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmRegistrarProductos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 500)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "frmRegistrarProductos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar producto"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()

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
        Me.tlpCampos.RowCount = 7
        Me.tlpCampos.Margin = System.Windows.Forms.Padding.Empty
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblId, 0, 0)
        Me.tlpCampos.Controls.Add(Me.txtId, 1, 0)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblNombre, 0, 1)
        Me.tlpCampos.Controls.Add(Me.cboCategoria, 1, 1)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblDocumento, 0, 2)
        Me.tlpCampos.Controls.Add(Me.txtTipo, 1, 2)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblTelefono, 0, 3)
        Me.tlpCampos.Controls.Add(Me.txtColor, 1, 3)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblDireccion, 0, 4)
        Me.tlpCampos.Controls.Add(Me.txtNombre, 1, 4)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.tlpCampos.Controls.Add(Me.Label1, 0, 5)
        Me.tlpCampos.Controls.Add(Me.txtDescripcion, 1, 5)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.chkActivo, 1, 6)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpAcciones.Margin = System.Windows.Forms.Padding.Empty
        Me.flpAcciones.Controls.Add(Me.btnCancelar)
        Me.flpAcciones.Controls.Add(Me.btnGuardar)
        Me.tlpContenido.Controls.Add(Me.lblAyuda, 0, 2)
        Me.tlpContenido.Controls.Add(Me.lblModo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.tlpCampos, 0, 1)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 3)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblModo As Label
    Friend WithEvents lblId As Label
    Friend WithEvents lblNombre As Label
    Friend WithEvents lblDocumento As Label
    Friend WithEvents lblTelefono As Label
    Friend WithEvents lblDireccion As Label
    Friend WithEvents lblAyuda As Label
    Friend WithEvents txtId As TextBox
    Friend WithEvents txtTipo As TextBox
    Friend WithEvents txtColor As TextBox
    Friend WithEvents txtDescripcion As TextBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents cboCategoria As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tlpCampos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

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
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.lblModo = New System.Windows.Forms.Label()
        Me.tlpCampos = New System.Windows.Forms.TableLayoutPanel()
        Me.lblId = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.cboCategoria = New System.Windows.Forms.ComboBox()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.txtTipo = New System.Windows.Forms.TextBox()
        Me.lblTelefono = New System.Windows.Forms.Label()
        Me.txtColor = New System.Windows.Forms.TextBox()
        Me.lblDireccion = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.tlpContenido.SuspendLayout()
        Me.tlpCampos.SuspendLayout()
        Me.flpAcciones.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpContenido
        '
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.Controls.Add(Me.lblAyuda, 0, 2)
        Me.tlpContenido.Controls.Add(Me.lblModo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.tlpCampos, 0, 1)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 3)
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.Location = New System.Drawing.Point(0, 0)
        Me.tlpContenido.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpContenido.RowCount = 4
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.Size = New System.Drawing.Size(700, 500)
        Me.tlpContenido.TabIndex = 0
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Location = New System.Drawing.Point(24, 410)
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 8, 0, 12)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(652, 20)
        Me.lblAyuda.TabIndex = 26
        Me.lblAyuda.Text = "* Obligatorio. Los demás datos son opcionales."
        '
        'lblModo
        '
        Me.lblModo.AutoSize = True
        Me.lblModo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblModo.Location = New System.Drawing.Point(24, 20)
        Me.lblModo.Margin = New System.Windows.Forms.Padding(0)
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Size = New System.Drawing.Size(652, 48)
        Me.lblModo.TabIndex = 15
        Me.lblModo.Text = "NUEVO PRODUCTO"
        '
        'tlpCampos
        '
        Me.tlpCampos.AutoSize = True
        Me.tlpCampos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpCampos.ColumnCount = 2
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpCampos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpCampos.Controls.Add(Me.lblId, 0, 0)
        Me.tlpCampos.Controls.Add(Me.txtId, 1, 0)
        Me.tlpCampos.Controls.Add(Me.lblNombre, 0, 1)
        Me.tlpCampos.Controls.Add(Me.cboCategoria, 1, 1)
        Me.tlpCampos.Controls.Add(Me.lblDocumento, 0, 2)
        Me.tlpCampos.Controls.Add(Me.txtTipo, 1, 2)
        Me.tlpCampos.Controls.Add(Me.lblTelefono, 0, 3)
        Me.tlpCampos.Controls.Add(Me.txtColor, 1, 3)
        Me.tlpCampos.Controls.Add(Me.lblDireccion, 0, 4)
        Me.tlpCampos.Controls.Add(Me.txtNombre, 1, 4)
        Me.tlpCampos.Controls.Add(Me.Label1, 0, 5)
        Me.tlpCampos.Controls.Add(Me.txtDescripcion, 1, 5)
        Me.tlpCampos.Controls.Add(Me.chkActivo, 1, 6)
        Me.tlpCampos.Dock = System.Windows.Forms.DockStyle.Top
        Me.tlpCampos.Location = New System.Drawing.Point(24, 68)
        Me.tlpCampos.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpCampos.Name = "tlpCampos"
        Me.tlpCampos.RowCount = 7
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Size = New System.Drawing.Size(652, 302)
        Me.tlpCampos.TabIndex = 27
        '
        'lblId
        '
        Me.lblId.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblId.Location = New System.Drawing.Point(0, 7)
        Me.lblId.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblId.Name = "lblId"
        Me.lblId.Size = New System.Drawing.Size(172, 31)
        Me.lblId.TabIndex = 16
        Me.lblId.Text = "ID"
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(180, 4)
        Me.txtId.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtId.MinimumSize = New System.Drawing.Size(130, 4)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(130, 26)
        Me.txtId.TabIndex = 17
        Me.txtId.TabStop = False
        '
        'lblNombre
        '
        Me.lblNombre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblNombre.Location = New System.Drawing.Point(0, 45)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(172, 31)
        Me.lblNombre.TabIndex = 18
        Me.lblNombre.Text = "Categoría *"
        '
        'cboCategoria
        '
        Me.cboCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCategoria.FormattingEnabled = True
        Me.cboCategoria.Location = New System.Drawing.Point(180, 42)
        Me.cboCategoria.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cboCategoria.Name = "cboCategoria"
        Me.cboCategoria.Size = New System.Drawing.Size(464, 28)
        Me.cboCategoria.TabIndex = 31
        '
        'lblDocumento
        '
        Me.lblDocumento.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblDocumento.Location = New System.Drawing.Point(0, 83)
        Me.lblDocumento.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Size = New System.Drawing.Size(172, 31)
        Me.lblDocumento.TabIndex = 20
        Me.lblDocumento.Text = "Tipo*"
        '
        'txtTipo
        '
        Me.txtTipo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTipo.Location = New System.Drawing.Point(180, 80)
        Me.txtTipo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtTipo.MaxLength = 20
        Me.txtTipo.Name = "txtTipo"
        Me.txtTipo.Size = New System.Drawing.Size(464, 26)
        Me.txtTipo.TabIndex = 21
        '
        'lblTelefono
        '
        Me.lblTelefono.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTelefono.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblTelefono.Location = New System.Drawing.Point(0, 121)
        Me.lblTelefono.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Size = New System.Drawing.Size(172, 31)
        Me.lblTelefono.TabIndex = 22
        Me.lblTelefono.Text = "Color"
        '
        'txtColor
        '
        Me.txtColor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtColor.Location = New System.Drawing.Point(180, 118)
        Me.txtColor.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtColor.MaxLength = 20
        Me.txtColor.Name = "txtColor"
        Me.txtColor.Size = New System.Drawing.Size(464, 26)
        Me.txtColor.TabIndex = 23
        '
        'lblDireccion
        '
        Me.lblDireccion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDireccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblDireccion.Location = New System.Drawing.Point(0, 159)
        Me.lblDireccion.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(172, 31)
        Me.lblDireccion.TabIndex = 24
        Me.lblDireccion.Text = "Nombre"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(180, 156)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtNombre.MaxLength = 20
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(464, 26)
        Me.txtNombre.TabIndex = 32
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(0, 197)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(172, 67)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Descripción"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcion.Location = New System.Drawing.Point(180, 194)
        Me.txtDescripcion.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtDescripcion.MaxLength = 255
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescripcion.Size = New System.Drawing.Size(464, 60)
        Me.txtDescripcion.TabIndex = 25
        '
        'chkActivo
        '
        Me.chkActivo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkActivo.Location = New System.Drawing.Point(180, 268)
        Me.chkActivo.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(464, 24)
        Me.chkActivo.TabIndex = 33
        Me.chkActivo.Text = "Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'flpAcciones
        '
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.Controls.Add(Me.btnCancelar)
        Me.flpAcciones.Controls.Add(Me.btnGuardar)
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpAcciones.Location = New System.Drawing.Point(24, 442)
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Size = New System.Drawing.Size(652, 42)
        Me.flpAcciones.TabIndex = 28
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(532, 8)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 28
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(404, 8)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 27
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(700, 500)
        Me.Panel2.TabIndex = 0
        '
        'frmRegistrarProductos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 500)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "frmRegistrarProductos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar producto"
        Me.tlpContenido.ResumeLayout(False)
        Me.tlpContenido.PerformLayout()
        Me.tlpCampos.ResumeLayout(False)
        Me.tlpCampos.PerformLayout()
        Me.flpAcciones.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
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

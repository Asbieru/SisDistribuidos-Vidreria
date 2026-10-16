<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarClientes
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpCampos = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.components = New System.ComponentModel.Container()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTipoPersona = New System.Windows.Forms.Label()
        Me.lblTipoDocumento = New System.Windows.Forms.Label()
        Me.cmbTipoPersona = New System.Windows.Forms.ComboBox()
        Me.cmbTipoDocumento = New System.Windows.Forms.ComboBox()
        Me.errDatos = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblModo = New System.Windows.Forms.Label()
        Me.lblId = New System.Windows.Forms.Label()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.lblTelefono = New System.Windows.Forms.Label()
        Me.lblDireccion = New System.Windows.Forms.Label()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.txtTelefono = New System.Windows.Forms.TextBox()
        Me.txtDireccion = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnRecargar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.errDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()

        Me.ClientSize = New System.Drawing.Size(760, 540)
        Me.Panel2.Size = New System.Drawing.Size(760, 540)

        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Name = "Panel2"
        Me.Panel2.TabIndex = 0

        Me.lblTipoPersona.AutoSize = False
        Me.lblTipoPersona.Location = New System.Drawing.Point(30, 127)
        Me.lblTipoPersona.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTipoPersona.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblTipoPersona.Name = "lblTipoPersona"
        Me.lblTipoPersona.Text = "Tipo de persona *"
        Me.cmbTipoPersona.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoPersona.Location = New System.Drawing.Point(200, 123)
        Me.cmbTipoPersona.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbTipoPersona.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cmbTipoPersona.Name = "cmbTipoPersona"
        Me.cmbTipoPersona.Size = New System.Drawing.Size(260, 24)
        Me.cmbTipoPersona.TabIndex = 3
        Me.lblTipoDocumento.AutoSize = False
        Me.lblTipoDocumento.Location = New System.Drawing.Point(30, 213)
        Me.lblTipoDocumento.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTipoDocumento.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblTipoDocumento.Name = "lblTipoDocumento"
        Me.lblTipoDocumento.Text = "Tipo de documento *"
        Me.cmbTipoDocumento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDocumento.Location = New System.Drawing.Point(200, 209)
        Me.cmbTipoDocumento.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbTipoDocumento.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.cmbTipoDocumento.Name = "cmbTipoDocumento"
        Me.cmbTipoDocumento.Size = New System.Drawing.Size(260, 24)
        Me.cmbTipoDocumento.TabIndex = 5
        Me.errDatos.ContainerControl = Me
        Me.errDatos.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink

        Me.lblModo.AutoSize = True
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblModo.Location = New System.Drawing.Point(28, 22)
        Me.lblModo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblModo.Margin = System.Windows.Forms.Padding.Empty
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Text = "NUEVO CLIENTE"
        Me.lblModo.TabIndex = 0

        Me.lblId.AutoSize = False
        Me.lblId.Location = New System.Drawing.Point(30, 84)
        Me.lblId.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblId.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblId.Name = "lblId"
        Me.lblId.Text = "ID"
        Me.lblId.TabIndex = 1
        Me.txtId.Location = New System.Drawing.Point(200, 80)
        Me.txtId.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.txtId.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtId.MinimumSize = New System.Drawing.Size(130, 0)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(130, 22)
        Me.txtId.TabIndex = 2
        Me.txtId.TabStop = False

        Me.lblNombre.AutoSize = False
        Me.lblNombre.Location = New System.Drawing.Point(30, 170)
        Me.lblNombre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Text = "Nombre *"
        Me.lblNombre.TabIndex = 3
        Me.txtNombre.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtNombre.Location = New System.Drawing.Point(200, 166)
        Me.txtNombre.MaxLength = 150
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(525, 22)
        Me.txtNombre.TabIndex = 4

        Me.lblDocumento.AutoSize = False
        Me.lblDocumento.Location = New System.Drawing.Point(30, 256)
        Me.lblDocumento.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDocumento.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Text = "Número de documento"
        Me.lblDocumento.TabIndex = 5
        Me.txtDocumento.Location = New System.Drawing.Point(200, 252)
        Me.txtDocumento.MaxLength = 20
        Me.txtDocumento.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.txtDocumento.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtDocumento.MinimumSize = New System.Drawing.Size(225, 0)
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(225, 22)
        Me.txtDocumento.TabIndex = 6

        Me.lblTelefono.AutoSize = False
        Me.lblTelefono.Location = New System.Drawing.Point(30, 299)
        Me.lblTelefono.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTelefono.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Text = "Teléfono"
        Me.lblTelefono.TabIndex = 7
        Me.txtTelefono.Location = New System.Drawing.Point(200, 295)
        Me.txtTelefono.MaxLength = 20
        Me.txtTelefono.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.txtTelefono.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtTelefono.MinimumSize = New System.Drawing.Size(225, 0)
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(225, 22)
        Me.txtTelefono.TabIndex = 8

        Me.lblDireccion.AutoSize = False
        Me.lblDireccion.Location = New System.Drawing.Point(30, 342)
        Me.lblDireccion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDireccion.Margin = New System.Windows.Forms.Padding(0, 7, 8, 0)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Text = "Dirección"
        Me.lblDireccion.TabIndex = 9
        Me.txtDireccion.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtDireccion.Location = New System.Drawing.Point(200, 338)
        Me.txtDireccion.MaxLength = 255
        Me.txtDireccion.Multiline = True
        Me.txtDireccion.Margin = New System.Windows.Forms.Padding(0, 4, 8, 8)
        Me.txtDireccion.Height = 62
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDireccion.Size = New System.Drawing.Size(525, 60)
        Me.txtDireccion.TabIndex = 10

        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(200, 409)
        Me.lblAyuda.Size = New System.Drawing.Size(525, 55)
        Me.lblAyuda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblAyuda.Margin = New System.Windows.Forms.Padding(0, 8, 0, 12)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Text = "* Obligatorio. Los demás datos son opcionales."
        Me.lblAyuda.TabIndex = 11

        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(475, 484)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 12
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(605, 484)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True

        Me.btnRecargar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnRecargar.Location = New System.Drawing.Point(345, 484)
        Me.btnRecargar.Margin = New System.Windows.Forms.Padding(8, 8, 0, 0)
        Me.btnRecargar.Name = "btnRecargar"
        Me.btnRecargar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnRecargar.Size = New System.Drawing.Size(120, 34)
        Me.btnRecargar.TabIndex = 14
        Me.btnRecargar.Text = "RECARGAR"
        Me.btnRecargar.Visible = False
        Me.btnRecargar.UseVisualStyleBackColor = True

        Me.AcceptButton = Me.btnGuardar
        Me.CancelButton = Me.btnCancelar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 540)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRegistrarClientes"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar cliente"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.errDatos, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.tlpCampos.Controls.Add(Me.lblTipoPersona, 0, 1)
        Me.tlpCampos.Controls.Add(Me.cmbTipoPersona, 1, 1)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblNombre, 0, 2)
        Me.tlpCampos.Controls.Add(Me.txtNombre, 1, 2)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblTipoDocumento, 0, 3)
        Me.tlpCampos.Controls.Add(Me.cmbTipoDocumento, 1, 3)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblDocumento, 0, 4)
        Me.tlpCampos.Controls.Add(Me.txtDocumento, 1, 4)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38.0!))
        Me.tlpCampos.Controls.Add(Me.lblTelefono, 0, 5)
        Me.tlpCampos.Controls.Add(Me.txtTelefono, 1, 5)
        Me.tlpCampos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.tlpCampos.Controls.Add(Me.lblDireccion, 0, 6)
        Me.tlpCampos.Controls.Add(Me.txtDireccion, 1, 6)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpAcciones.Margin = System.Windows.Forms.Padding.Empty
        Me.flpAcciones.Controls.Add(Me.btnCancelar)
        Me.flpAcciones.Controls.Add(Me.btnGuardar)
        Me.flpAcciones.Controls.Add(Me.btnRecargar)
        Me.tlpContenido.Controls.Add(Me.lblAyuda, 0, 2)
        Me.tlpContenido.Controls.Add(Me.lblModo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.tlpCampos, 0, 1)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 3)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblTipoPersona As System.Windows.Forms.Label
    Friend WithEvents lblTipoDocumento As System.Windows.Forms.Label
    Friend WithEvents cmbTipoPersona As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoDocumento As System.Windows.Forms.ComboBox
    Friend WithEvents errDatos As System.Windows.Forms.ErrorProvider
    Friend WithEvents lblModo As System.Windows.Forms.Label
    Friend WithEvents lblId As System.Windows.Forms.Label
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents lblDocumento As System.Windows.Forms.Label
    Friend WithEvents lblTelefono As System.Windows.Forms.Label
    Friend WithEvents lblDireccion As System.Windows.Forms.Label
    Friend WithEvents lblAyuda As System.Windows.Forms.Label
    Friend WithEvents txtId As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtDocumento As System.Windows.Forms.TextBox
    Friend WithEvents txtTelefono As System.Windows.Forms.TextBox
    Friend WithEvents txtDireccion As System.Windows.Forms.TextBox
    Friend WithEvents btnGuardar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
    Friend WithEvents btnRecargar As System.Windows.Forms.Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tlpCampos As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

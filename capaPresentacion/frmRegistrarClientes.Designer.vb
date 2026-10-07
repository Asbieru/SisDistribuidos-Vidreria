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
        Me.Panel2 = New System.Windows.Forms.Panel()
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
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()

        Me.ClientSize = New System.Drawing.Size(680, 430)
        Me.Panel2.Size = New System.Drawing.Size(680, 430)

        Me.Panel2.Controls.Add(Me.lblModo)
        Me.Panel2.Controls.Add(Me.lblId)
        Me.Panel2.Controls.Add(Me.lblNombre)
        Me.Panel2.Controls.Add(Me.lblDocumento)
        Me.Panel2.Controls.Add(Me.lblTelefono)
        Me.Panel2.Controls.Add(Me.lblDireccion)
        Me.Panel2.Controls.Add(Me.lblAyuda)
        Me.Panel2.Controls.Add(Me.txtId)
        Me.Panel2.Controls.Add(Me.txtNombre)
        Me.Panel2.Controls.Add(Me.txtDocumento)
        Me.Panel2.Controls.Add(Me.txtTelefono)
        Me.Panel2.Controls.Add(Me.txtDireccion)
        Me.Panel2.Controls.Add(Me.btnGuardar)
        Me.Panel2.Controls.Add(Me.btnCancelar)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Name = "Panel2"
        Me.Panel2.TabIndex = 0

        Me.lblModo.AutoSize = True
        Me.lblModo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblModo.Location = New System.Drawing.Point(28, 22)
        Me.lblModo.Name = "lblModo"
        Me.lblModo.Text = "NUEVO CLIENTE"
        Me.lblModo.TabIndex = 0

        Me.lblId.AutoSize = True
        Me.lblId.Location = New System.Drawing.Point(30, 84)
        Me.lblId.Name = "lblId"
        Me.lblId.Text = "ID"
        Me.lblId.TabIndex = 1
        Me.txtId.Location = New System.Drawing.Point(170, 80)
        Me.txtId.Name = "txtId"
        Me.txtId.ReadOnly = True
        Me.txtId.Size = New System.Drawing.Size(130, 22)
        Me.txtId.TabIndex = 2
        Me.txtId.TabStop = False

        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(30, 127)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Text = "Nombre *"
        Me.lblNombre.TabIndex = 3
        Me.txtNombre.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtNombre.Location = New System.Drawing.Point(170, 123)
        Me.txtNombre.MaxLength = 150
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(480, 22)
        Me.txtNombre.TabIndex = 4

        Me.lblDocumento.AutoSize = True
        Me.lblDocumento.Location = New System.Drawing.Point(30, 170)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Text = "Documento"
        Me.lblDocumento.TabIndex = 5
        Me.txtDocumento.Location = New System.Drawing.Point(170, 166)
        Me.txtDocumento.MaxLength = 20
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(225, 22)
        Me.txtDocumento.TabIndex = 6

        Me.lblTelefono.AutoSize = True
        Me.lblTelefono.Location = New System.Drawing.Point(30, 213)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Text = "Teléfono"
        Me.lblTelefono.TabIndex = 7
        Me.txtTelefono.Location = New System.Drawing.Point(170, 209)
        Me.txtTelefono.MaxLength = 20
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(225, 22)
        Me.txtTelefono.TabIndex = 8

        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.Location = New System.Drawing.Point(30, 256)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Text = "Dirección"
        Me.lblDireccion.TabIndex = 9
        Me.txtDireccion.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtDireccion.Location = New System.Drawing.Point(170, 252)
        Me.txtDireccion.MaxLength = 255
        Me.txtDireccion.Multiline = True
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDireccion.Size = New System.Drawing.Size(480, 60)
        Me.txtDireccion.TabIndex = 10

        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(170, 324)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Text = "* Obligatorio. Los demás datos son opcionales."
        Me.lblAyuda.TabIndex = 11

        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(400, 372)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 12
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(530, 372)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 13
        Me.btnCancelar.Text = "CANCELAR"
        Me.btnCancelar.UseVisualStyleBackColor = True

        Me.AcceptButton = Me.btnGuardar
        Me.CancelButton = Me.btnCancelar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(680, 430)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRegistrarClientes"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar cliente"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents Panel2 As System.Windows.Forms.Panel
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
End Class

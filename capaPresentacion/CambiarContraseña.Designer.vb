<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CambiarContraseña
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CambiarContraseña))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.USUARIO = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkMostrar = New System.Windows.Forms.CheckBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.txtContraseña = New System.Windows.Forms.TextBox()
        Me.txtConfirmar = New System.Windows.Forms.TextBox()
        Me.pnlDatos = New System.Windows.Forms.Panel()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDatos.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(152, 64)
        Me.Label1.Size = New System.Drawing.Size(320, 34)
        Me.Label1.Name = "Label1"
        '
        'lbl1
        '
        resources.ApplyResources(Me.lbl1, "lbl1")
        Me.lbl1.ForeColor = System.Drawing.Color.White
        Me.lbl1.Location = New System.Drawing.Point(152, 26)
        Me.lbl1.Size = New System.Drawing.Size(320, 34)
        Me.lbl1.Name = "lbl1"
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Location = New System.Drawing.Point(24, 20)
        Me.PictureBox1.Size = New System.Drawing.Size(88, 88)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'USUARIO
        '
        resources.ApplyResources(Me.USUARIO, "USUARIO")
        Me.USUARIO.BackColor = System.Drawing.Color.Transparent
        Me.USUARIO.ForeColor = System.Drawing.Color.White
        Me.USUARIO.Location = New System.Drawing.Point(16, 20)
        Me.USUARIO.Size = New System.Drawing.Size(215, 24)
        Me.USUARIO.Name = "USUARIO"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(16, 66)
        Me.Label2.Size = New System.Drawing.Size(215, 24)
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(16, 112)
        Me.Label3.Size = New System.Drawing.Size(215, 24)
        Me.Label3.Name = "Label3"
        '
        'chkMostrar
        '
        resources.ApplyResources(Me.chkMostrar, "chkMostrar")
        Me.chkMostrar.ForeColor = System.Drawing.Color.White
        Me.chkMostrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.chkMostrar.Location = New System.Drawing.Point(244, 154)
        Me.chkMostrar.Size = New System.Drawing.Size(250, 24)
        Me.chkMostrar.Name = "chkMostrar"
        Me.chkMostrar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        resources.ApplyResources(Me.btnGuardar, "btnGuardar")
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(244, 220)
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        resources.ApplyResources(Me.btnCancelar, "btnCancelar")
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(376, 220)
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'txtUsuario
        '
        resources.ApplyResources(Me.txtUsuario, "txtUsuario")
        Me.txtUsuario.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtUsuario.Location = New System.Drawing.Point(244, 18)
        Me.txtUsuario.Size = New System.Drawing.Size(250, 24)
        Me.txtUsuario.Name = "txtUsuario"
        '
        'txtContraseña
        '
        resources.ApplyResources(Me.txtContraseña, "txtContraseña")
        Me.txtContraseña.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtContraseña.Location = New System.Drawing.Point(244, 64)
        Me.txtContraseña.Size = New System.Drawing.Size(250, 24)
        Me.txtContraseña.Name = "txtContraseña"
        '
        'txtConfirmar
        '
        resources.ApplyResources(Me.txtConfirmar, "txtConfirmar")
        Me.txtConfirmar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtConfirmar.Location = New System.Drawing.Point(244, 110)
        Me.txtConfirmar.Size = New System.Drawing.Size(250, 24)
        Me.txtConfirmar.Name = "txtConfirmar"
        '
        'pnlDatos
        '
        Me.pnlDatos.Controls.Add(Me.txtConfirmar)
        Me.pnlDatos.Controls.Add(Me.txtContraseña)
        Me.pnlDatos.Controls.Add(Me.txtUsuario)
        Me.pnlDatos.Controls.Add(Me.btnCancelar)
        Me.pnlDatos.Controls.Add(Me.btnGuardar)
        Me.pnlDatos.Controls.Add(Me.chkMostrar)
        Me.pnlDatos.Controls.Add(Me.Label3)
        Me.pnlDatos.Controls.Add(Me.Label2)
        Me.pnlDatos.Controls.Add(Me.USUARIO)
        resources.ApplyResources(Me.pnlDatos, "pnlDatos")
        Me.pnlDatos.Location = New System.Drawing.Point(24, 132)
        Me.pnlDatos.Size = New System.Drawing.Size(512, 280)
        Me.pnlDatos.Name = "pnlDatos"
        '
        'CambiarContraseña
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me.pnlDatos)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.ClientSize = New System.Drawing.Size(560, 432)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.AcceptButton = Me.btnGuardar
        Me.CancelButton = Me.btnCancelar
        Me.Name = "CambiarContraseña"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDatos.ResumeLayout(False)
        Me.pnlDatos.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents lbl1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents USUARIO As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents chkMostrar As CheckBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents txtUsuario As TextBox
    Friend WithEvents txtContraseña As TextBox
    Friend WithEvents txtConfirmar As TextBox
    Friend WithEvents pnlDatos As Panel
End Class

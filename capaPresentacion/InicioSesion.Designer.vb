<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InicioSesion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InicioSesion))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.pnlLogin = New System.Windows.Forms.Panel()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnIngresar = New System.Windows.Forms.Button()
        Me.txtContrasena = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.msj = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chbMostrar = New System.Windows.Forms.CheckBox()
        Me.pnlCambio = New System.Windows.Forms.Panel()
        Me.msj2 = New System.Windows.Forms.Label()
        Me.btnCambio = New System.Windows.Forms.Button()
        Me.txtRespuesta = New System.Windows.Forms.TextBox()
        Me.txtPregunta = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLogin.SuspendLayout()
        Me.pnlCambio.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox1.InitialImage = CType(resources.GetObject("PictureBox1.InitialImage"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(32, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(88, 88)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'pnlLogin
        '
        Me.pnlLogin.BackColor = System.Drawing.Color.DarkGray
        Me.pnlLogin.Controls.Add(Me.btnSalir)
        Me.pnlLogin.Controls.Add(Me.btnIngresar)
        Me.pnlLogin.Controls.Add(Me.txtContrasena)
        Me.pnlLogin.Controls.Add(Me.Label3)
        Me.pnlLogin.Controls.Add(Me.txtUsuario)
        Me.pnlLogin.Controls.Add(Me.Label2)
        Me.pnlLogin.Location = New System.Drawing.Point(24, 136)
        Me.pnlLogin.Name = "pnlLogin"
        Me.pnlLogin.Size = New System.Drawing.Size(372, 190)
        Me.pnlLogin.TabIndex = 0
        '
        'btnSalir
        '
        Me.btnSalir.BackColor = System.Drawing.Color.White
        Me.btnSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnSalir.ForeColor = System.Drawing.Color.Black
        Me.btnSalir.Location = New System.Drawing.Point(232, 146)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(120, 34)
        Me.btnSalir.TabIndex = 5
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'btnIngresar
        '
        Me.btnIngresar.BackColor = System.Drawing.Color.White
        Me.btnIngresar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnIngresar.ForeColor = System.Drawing.Color.Black
        Me.btnIngresar.Location = New System.Drawing.Point(100, 146)
        Me.btnIngresar.Name = "btnIngresar"
        Me.btnIngresar.Size = New System.Drawing.Size(120, 34)
        Me.btnIngresar.TabIndex = 3
        Me.btnIngresar.Text = "Ingresar"
        Me.btnIngresar.UseVisualStyleBackColor = False
        '
        'txtContrasena
        '
        Me.txtContrasena.Location = New System.Drawing.Point(16, 108)
        Me.txtContrasena.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtContrasena.Name = "txtContrasena"
        Me.txtContrasena.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtContrasena.Size = New System.Drawing.Size(332, 24)
        Me.txtContrasena.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(16, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(332, 22)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Contraseña"
        '
        'txtUsuario
        '
        Me.txtUsuario.Location = New System.Drawing.Point(16, 42)
        Me.txtUsuario.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(332, 24)
        Me.txtUsuario.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(16, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(332, 22)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Usuario"
        '
        'msj
        '
        Me.msj.BackColor = System.Drawing.Color.Transparent
        Me.msj.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msj.ForeColor = System.Drawing.Color.Red
        Me.msj.Location = New System.Drawing.Point(24, 338)
        Me.msj.AutoSize = False
        Me.msj.Name = "msj"
        Me.msj.Size = New System.Drawing.Size(372, 60)
        Me.msj.TabIndex = 0
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.ForeColor = System.Drawing.Color.White
        Me.lbl1.Location = New System.Drawing.Point(152, 30)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(230, 34)
        Me.lbl1.TabIndex = 0
        Me.lbl1.Text = "INICIO DE"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(152, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(230, 34)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SESIÓN"
        '
        'chbMostrar
        '
        Me.chbMostrar.AutoSize = True
        Me.chbMostrar.ForeColor = System.Drawing.Color.White
        Me.chbMostrar.Location = New System.Drawing.Point(24, 404)
        Me.chbMostrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.chbMostrar.Name = "chbMostrar"
        Me.chbMostrar.Size = New System.Drawing.Size(320, 24)
        Me.chbMostrar.TabIndex = 4
        Me.chbMostrar.Text = "Cambiar Contraseña"
        Me.chbMostrar.UseVisualStyleBackColor = True
        '
        'pnlCambio
        '
        Me.pnlCambio.BackColor = System.Drawing.Color.Transparent
        Me.pnlCambio.Controls.Add(Me.msj2)
        Me.pnlCambio.Controls.Add(Me.btnCambio)
        Me.pnlCambio.Controls.Add(Me.txtRespuesta)
        Me.pnlCambio.Controls.Add(Me.txtPregunta)
        Me.pnlCambio.Controls.Add(Me.Label4)
        Me.pnlCambio.Location = New System.Drawing.Point(24, 440)
        Me.pnlCambio.Name = "pnlCambio"
        Me.pnlCambio.Size = New System.Drawing.Size(372, 190)
        Me.pnlCambio.TabIndex = 0
        Me.pnlCambio.Visible = False
        '
        'msj2
        '
        Me.msj2.BackColor = System.Drawing.Color.Transparent
        Me.msj2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.msj2.ForeColor = System.Drawing.Color.Red
        Me.msj2.Location = New System.Drawing.Point(16, 156)
        Me.msj2.AutoSize = False
        Me.msj2.Name = "msj2"
        Me.msj2.Size = New System.Drawing.Size(332, 34)
        Me.msj2.TabIndex = 0
        '
        'btnCambio
        '
        Me.btnCambio.BackColor = System.Drawing.Color.White
        Me.btnCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCambio.ForeColor = System.Drawing.Color.Black
        Me.btnCambio.Location = New System.Drawing.Point(16, 114)
        Me.btnCambio.Name = "btnCambio"
        Me.btnCambio.Size = New System.Drawing.Size(210, 34)
        Me.btnCambio.TabIndex = 7
        Me.btnCambio.Text = "Cambiar Contraseña"
        Me.btnCambio.UseVisualStyleBackColor = False
        '
        'txtRespuesta
        '
        Me.txtRespuesta.Location = New System.Drawing.Point(16, 78)
        Me.txtRespuesta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtRespuesta.Name = "txtRespuesta"
        Me.txtRespuesta.Size = New System.Drawing.Size(332, 24)
        Me.txtRespuesta.TabIndex = 6
        '
        'txtPregunta
        '
        Me.txtPregunta.Location = New System.Drawing.Point(16, 16)
        Me.txtPregunta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.txtPregunta.Name = "txtPregunta"
        Me.txtPregunta.Size = New System.Drawing.Size(332, 24)
        Me.txtPregunta.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(16, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(332, 22)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Respuesta"
        '
        'InicioSesion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(420, 450)
        Me.Controls.Add(Me.msj)
        Me.Controls.Add(Me.pnlCambio)
        Me.Controls.Add(Me.chbMostrar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.pnlLogin)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.AcceptButton = Me.btnIngresar
        Me.CancelButton = Me.btnSalir
        Me.Name = "InicioSesion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Iniciar sesión"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLogin.ResumeLayout(False)
        Me.pnlLogin.PerformLayout()
        Me.pnlCambio.ResumeLayout(False)
        Me.pnlCambio.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents pnlLogin As Panel
    Friend WithEvents lbl1 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtContrasena As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtUsuario As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnIngresar As Button
    Friend WithEvents chbMostrar As CheckBox
    Friend WithEvents msj As Label
    Friend WithEvents pnlCambio As Panel
    Friend WithEvents msj2 As Label
    Friend WithEvents btnCambio As Button
    Friend WithEvents txtRespuesta As TextBox
    Friend WithEvents txtPregunta As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents btnSalir As Button
End Class

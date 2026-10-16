<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCotizarVentana
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCotizarVentana))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtEspesor = New System.Windows.Forms.ComboBox()
        Me.cmbTipoVidrio = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbPedido = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbTipoAluminio = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbColorVidrio = New System.Windows.Forms.ComboBox()
        Me.cmbColorAluminio = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtAlto = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtAncho = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnLlevarPedido = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtPreciomaterial = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtExtra = New System.Windows.Forms.TextBox()
        Me.txtManoObra = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnDuplicar = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.txtEspesor)
        Me.Panel1.Controls.Add(Me.cmbTipoVidrio)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.cmbPedido)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.cmbTipoAluminio)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.cmbColorVidrio)
        Me.Panel1.Controls.Add(Me.cmbColorAluminio)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(431, 201)
        Me.Panel1.TabIndex = 0
        '
        'txtEspesor
        '
        Me.txtEspesor.FormattingEnabled = True
        Me.txtEspesor.Location = New System.Drawing.Point(258, 169)
        Me.txtEspesor.Name = "txtEspesor"
        Me.txtEspesor.Size = New System.Drawing.Size(112, 21)
        Me.txtEspesor.TabIndex = 8
        '
        'cmbTipoVidrio
        '
        Me.cmbTipoVidrio.FormattingEnabled = True
        Me.cmbTipoVidrio.Location = New System.Drawing.Point(170, 117)
        Me.cmbTipoVidrio.Name = "cmbTipoVidrio"
        Me.cmbTipoVidrio.Size = New System.Drawing.Size(241, 21)
        Me.cmbTipoVidrio.TabIndex = 7
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label17.Location = New System.Drawing.Point(10, 119)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(92, 16)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "Tipo de Vidrio"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label7.Location = New System.Drawing.Point(376, 170)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "m.m."
        '
        'cmbPedido
        '
        Me.cmbPedido.FormattingEnabled = True
        Me.cmbPedido.Location = New System.Drawing.Point(170, 40)
        Me.cmbPedido.Name = "cmbPedido"
        Me.cmbPedido.Size = New System.Drawing.Size(241, 21)
        Me.cmbPedido.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label4.Location = New System.Drawing.Point(7, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Tipo de Pedido"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label6.Location = New System.Drawing.Point(23, 170)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(115, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Espesor de Vidrio"
        '
        'cmbTipoAluminio
        '
        Me.cmbTipoAluminio.FormattingEnabled = True
        Me.cmbTipoAluminio.Location = New System.Drawing.Point(170, 67)
        Me.cmbTipoAluminio.Name = "cmbTipoAluminio"
        Me.cmbTipoAluminio.Size = New System.Drawing.Size(241, 21)
        Me.cmbTipoAluminio.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label5.Location = New System.Drawing.Point(10, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Tipo de Aluminio"
        '
        'cmbColorVidrio
        '
        Me.cmbColorVidrio.FormattingEnabled = True
        Me.cmbColorVidrio.Location = New System.Drawing.Point(211, 142)
        Me.cmbColorVidrio.Name = "cmbColorVidrio"
        Me.cmbColorVidrio.Size = New System.Drawing.Size(200, 21)
        Me.cmbColorVidrio.TabIndex = 4
        '
        'cmbColorAluminio
        '
        Me.cmbColorAluminio.FormattingEnabled = True
        Me.cmbColorAluminio.Items.AddRange(New Object() {"ESTANDAR", "NEGRO", "MADERA"})
        Me.cmbColorAluminio.Location = New System.Drawing.Point(211, 92)
        Me.cmbColorAluminio.Name = "cmbColorAluminio"
        Me.cmbColorAluminio.Size = New System.Drawing.Size(200, 21)
        Me.cmbColorAluminio.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label3.Location = New System.Drawing.Point(27, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Color de Vidrio"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(26, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Color de Aluminio"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(2, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(170, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Caracteristicas"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.txtAlto)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.txtAncho)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Location = New System.Drawing.Point(12, 219)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(431, 108)
        Me.Panel2.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label11.Location = New System.Drawing.Point(376, 65)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(28, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "cm."
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label12.Location = New System.Drawing.Point(7, 68)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Alto de Pedido"
        '
        'txtAlto
        '
        Me.txtAlto.Location = New System.Drawing.Point(170, 64)
        Me.txtAlto.Name = "txtAlto"
        Me.txtAlto.Size = New System.Drawing.Size(200, 20)
        Me.txtAlto.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label10.Location = New System.Drawing.Point(376, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(28, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "cm."
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label9.Location = New System.Drawing.Point(7, 42)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(111, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Ancho de Pedido"
        '
        'txtAncho
        '
        Me.txtAncho.Location = New System.Drawing.Point(170, 38)
        Me.txtAncho.Name = "txtAncho"
        Me.txtAncho.Size = New System.Drawing.Size(200, 20)
        Me.txtAncho.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label8.Location = New System.Drawing.Point(3, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(101, 25)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Medidas"
        '
        'btnLlevarPedido
        '
        Me.btnLlevarPedido.Location = New System.Drawing.Point(335, 463)
        Me.btnLlevarPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnLlevarPedido.Name = "btnLlevarPedido"
        Me.btnLlevarPedido.Size = New System.Drawing.Size(108, 23)
        Me.btnLlevarPedido.TabIndex = 10
        Me.btnLlevarPedido.Text = "Llevar a pedido"
        Me.btnLlevarPedido.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label13.Location = New System.Drawing.Point(19, 377)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(182, 16)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Precio de los materiales (S/.):"
        '
        'txtPreciomaterial
        '
        Me.txtPreciomaterial.Enabled = False
        Me.txtPreciomaterial.Location = New System.Drawing.Point(223, 375)
        Me.txtPreciomaterial.Name = "txtPreciomaterial"
        Me.txtPreciomaterial.Size = New System.Drawing.Size(200, 20)
        Me.txtPreciomaterial.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label14.Location = New System.Drawing.Point(127, 352)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(74, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Extras (S/.):"
        '
        'txtExtra
        '
        Me.txtExtra.Location = New System.Drawing.Point(223, 349)
        Me.txtExtra.Name = "txtExtra"
        Me.txtExtra.Size = New System.Drawing.Size(200, 20)
        Me.txtExtra.TabIndex = 8
        '
        'txtManoObra
        '
        Me.txtManoObra.Location = New System.Drawing.Point(223, 401)
        Me.txtManoObra.Name = "txtManoObra"
        Me.txtManoObra.Size = New System.Drawing.Size(200, 20)
        Me.txtManoObra.TabIndex = 9
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label15.Location = New System.Drawing.Point(80, 403)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(121, 16)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Mano de obra (S/.):"
        '
        'txtTotal
        '
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(223, 427)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(200, 20)
        Me.txtTotal.TabIndex = 0
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label16.Location = New System.Drawing.Point(133, 427)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(68, 16)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Total (S/.):"
        '
        'btnDuplicar
        '
        Me.btnDuplicar.Location = New System.Drawing.Point(12, 463)
        Me.btnDuplicar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnDuplicar.Name = "btnDuplicar"
        Me.btnDuplicar.Size = New System.Drawing.Size(108, 23)
        Me.btnDuplicar.TabIndex = 12
        Me.btnDuplicar.Text = "Duplicar"
        Me.btnDuplicar.UseVisualStyleBackColor = True
        '
        'frmCotizarVentana
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(455, 494)
        Me.Controls.Add(Me.btnDuplicar)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtManoObra)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.txtExtra)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.txtPreciomaterial)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.btnLlevarPedido)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Name = "frmCotizarVentana"
        Me.Text = "Cotizar ventana"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbTipoVidrio As ComboBox
    Friend WithEvents cmbColorVidrio As ComboBox
    Friend WithEvents cmbColorAluminio As ComboBox
    Friend WithEvents cmbPedido As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbTipoAluminio As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnLlevarPedido As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents txtAncho As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtAlto As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtPreciomaterial As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtExtra As TextBox
    Friend WithEvents txtManoObra As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents btnDuplicar As Button
    Friend WithEvents Label17 As Label
    Friend WithEvents txtEspesor As ComboBox
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRegistrarPedido
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnBuscarCliente = New System.Windows.Forms.Button()
        Me.txtNombreCliente = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbPedido = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnAgregarPedido = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tabsPedidos = New System.Windows.Forms.TabControl()
        Me.tabModeloVidrio = New System.Windows.Forms.TabPage()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtManoObra = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtExtra = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtPreciomaterial = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.lbl6 = New System.Windows.Forms.Label()
        Me.lbl3 = New System.Windows.Forms.Label()
        Me.lbl5 = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.chbAlto = New System.Windows.Forms.CheckBox()
        Me.chbAncho = New System.Windows.Forms.CheckBox()
        Me.lbl7 = New System.Windows.Forms.Label()
        Me.txtAlto2 = New System.Windows.Forms.TextBox()
        Me.lbl4 = New System.Windows.Forms.Label()
        Me.txtAncho2 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtAlto = New System.Windows.Forms.TextBox()
        Me.lbl2 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtAncho = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtNombreVidrio = New System.Windows.Forms.TextBox()
        Me.btnSelectVidrio = New System.Windows.Forms.Button()
        Me.txtEspesor = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tabModeloVentana = New System.Windows.Forms.TabPage()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.CheckBox3 = New System.Windows.Forms.CheckBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.txtCenVentana = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtIzqVentana = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtAbaVentana = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.txtDerVentana = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.txtArrVentana = New System.Windows.Forms.TextBox()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.cmbColorAlumnio = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.btnDuplicar = New System.Windows.Forms.Button()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnGuardarPedido = New System.Windows.Forms.Button()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabsPedidos.SuspendLayout()
        Me.tabModeloVidrio.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.tabModeloVentana.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnBuscarCliente)
        Me.Panel1.Controls.Add(Me.txtNombreCliente)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(364, 99)
        Me.Panel1.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Nombre"
        '
        'btnBuscarCliente
        '
        Me.btnBuscarCliente.Location = New System.Drawing.Point(82, 66)
        Me.btnBuscarCliente.Name = "btnBuscarCliente"
        Me.btnBuscarCliente.Size = New System.Drawing.Size(195, 23)
        Me.btnBuscarCliente.TabIndex = 1
        Me.btnBuscarCliente.Text = "Buscar Cliente"
        Me.btnBuscarCliente.UseVisualStyleBackColor = True
        '
        'txtNombreCliente
        '
        Me.txtNombreCliente.Enabled = False
        Me.txtNombreCliente.Location = New System.Drawing.Point(95, 33)
        Me.txtNombreCliente.Name = "txtNombreCliente"
        Me.txtNombreCliente.Size = New System.Drawing.Size(256, 20)
        Me.txtNombreCliente.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Cliente"
        '
        'cmbPedido
        '
        Me.cmbPedido.FormattingEnabled = True
        Me.cmbPedido.Items.AddRange(New Object() {"Vidrio", "Ventana"})
        Me.cmbPedido.Location = New System.Drawing.Point(116, 35)
        Me.cmbPedido.Name = "cmbPedido"
        Me.cmbPedido.Size = New System.Drawing.Size(280, 21)
        Me.cmbPedido.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 16)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Tipo de Pedido"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnAgregarPedido)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.cmbPedido)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(388, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(405, 99)
        Me.Panel2.TabIndex = 0
        '
        'btnAgregarPedido
        '
        Me.btnAgregarPedido.Location = New System.Drawing.Point(284, 66)
        Me.btnAgregarPedido.Name = "btnAgregarPedido"
        Me.btnAgregarPedido.Size = New System.Drawing.Size(112, 23)
        Me.btnAgregarPedido.TabIndex = 4
        Me.btnAgregarPedido.Text = "Agregar"
        Me.btnAgregarPedido.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 25)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Pedido"
        '
        'tabsPedidos
        '
        Me.tabsPedidos.Controls.Add(Me.tabModeloVidrio)
        Me.tabsPedidos.Controls.Add(Me.tabModeloVentana)
        Me.tabsPedidos.Location = New System.Drawing.Point(12, 117)
        Me.tabsPedidos.Name = "tabsPedidos"
        Me.tabsPedidos.SelectedIndex = 0
        Me.tabsPedidos.Size = New System.Drawing.Size(781, 375)
        Me.tabsPedidos.TabIndex = 0
        '
        'tabModeloVidrio
        '
        Me.tabModeloVidrio.Controls.Add(Me.txtTotal)
        Me.tabModeloVidrio.Controls.Add(Me.Label10)
        Me.tabModeloVidrio.Controls.Add(Me.txtManoObra)
        Me.tabModeloVidrio.Controls.Add(Me.Label17)
        Me.tabModeloVidrio.Controls.Add(Me.txtExtra)
        Me.tabModeloVidrio.Controls.Add(Me.Label18)
        Me.tabModeloVidrio.Controls.Add(Me.txtPreciomaterial)
        Me.tabModeloVidrio.Controls.Add(Me.Label19)
        Me.tabModeloVidrio.Controls.Add(Me.Panel5)
        Me.tabModeloVidrio.Controls.Add(Me.Panel4)
        Me.tabModeloVidrio.Location = New System.Drawing.Point(4, 22)
        Me.tabModeloVidrio.Name = "tabModeloVidrio"
        Me.tabModeloVidrio.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModeloVidrio.Size = New System.Drawing.Size(773, 349)
        Me.tabModeloVidrio.TabIndex = 0
        Me.tabModeloVidrio.Text = "Modelo Vidrio"
        Me.tabModeloVidrio.UseVisualStyleBackColor = True
        '
        'txtTotal
        '
        Me.txtTotal.Enabled = False
        Me.txtTotal.Location = New System.Drawing.Point(562, 302)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(200, 20)
        Me.txtTotal.TabIndex = 10
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(488, 303)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(68, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Total (S/.):"
        '
        'txtManoObra
        '
        Me.txtManoObra.Location = New System.Drawing.Point(562, 276)
        Me.txtManoObra.Name = "txtManoObra"
        Me.txtManoObra.Size = New System.Drawing.Size(200, 20)
        Me.txtManoObra.TabIndex = 17
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(435, 279)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(121, 16)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Mano de obra (S/.):"
        '
        'txtExtra
        '
        Me.txtExtra.Location = New System.Drawing.Point(201, 274)
        Me.txtExtra.Name = "txtExtra"
        Me.txtExtra.Size = New System.Drawing.Size(200, 20)
        Me.txtExtra.TabIndex = 16
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(121, 276)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(74, 16)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Extras (S/.):"
        '
        'txtPreciomaterial
        '
        Me.txtPreciomaterial.Enabled = False
        Me.txtPreciomaterial.Location = New System.Drawing.Point(201, 302)
        Me.txtPreciomaterial.Name = "txtPreciomaterial"
        Me.txtPreciomaterial.Size = New System.Drawing.Size(200, 20)
        Me.txtPreciomaterial.TabIndex = 14
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(13, 303)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(182, 16)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Precio de los materiales (S/.):"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.Controls.Add(Me.lbl6)
        Me.Panel5.Controls.Add(Me.lbl3)
        Me.Panel5.Controls.Add(Me.lbl5)
        Me.Panel5.Controls.Add(Me.lbl1)
        Me.Panel5.Controls.Add(Me.chbAlto)
        Me.Panel5.Controls.Add(Me.chbAncho)
        Me.Panel5.Controls.Add(Me.lbl7)
        Me.Panel5.Controls.Add(Me.txtAlto2)
        Me.Panel5.Controls.Add(Me.lbl4)
        Me.Panel5.Controls.Add(Me.txtAncho2)
        Me.Panel5.Controls.Add(Me.Label11)
        Me.Panel5.Controls.Add(Me.Label12)
        Me.Panel5.Controls.Add(Me.txtAlto)
        Me.Panel5.Controls.Add(Me.lbl2)
        Me.Panel5.Controls.Add(Me.Label13)
        Me.Panel5.Controls.Add(Me.txtAncho)
        Me.Panel5.Controls.Add(Me.Label14)
        Me.Panel5.Location = New System.Drawing.Point(6, 112)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(756, 138)
        Me.Panel5.TabIndex = 0
        '
        'lbl6
        '
        Me.lbl6.AutoSize = True
        Me.lbl6.BackColor = System.Drawing.Color.Transparent
        Me.lbl6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl6.Location = New System.Drawing.Point(439, 91)
        Me.lbl6.Name = "lbl6"
        Me.lbl6.Size = New System.Drawing.Size(62, 16)
        Me.lbl6.TabIndex = 0
        Me.lbl6.Text = "Izquierda"
        '
        'lbl3
        '
        Me.lbl3.AutoSize = True
        Me.lbl3.BackColor = System.Drawing.Color.Transparent
        Me.lbl3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3.Location = New System.Drawing.Point(455, 44)
        Me.lbl3.Name = "lbl3"
        Me.lbl3.Size = New System.Drawing.Size(43, 16)
        Me.lbl3.TabIndex = 0
        Me.lbl3.Text = "Abajo"
        '
        'lbl5
        '
        Me.lbl5.AutoSize = True
        Me.lbl5.BackColor = System.Drawing.Color.Transparent
        Me.lbl5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl5.Location = New System.Drawing.Point(131, 91)
        Me.lbl5.Name = "lbl5"
        Me.lbl5.Size = New System.Drawing.Size(59, 16)
        Me.lbl5.TabIndex = 0
        Me.lbl5.Text = "Derecha"
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.BackColor = System.Drawing.Color.Transparent
        Me.lbl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.Location = New System.Drawing.Point(147, 45)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(43, 16)
        Me.lbl1.TabIndex = 0
        Me.lbl1.Text = "Arriba"
        '
        'chbAlto
        '
        Me.chbAlto.AutoSize = True
        Me.chbAlto.Location = New System.Drawing.Point(10, 107)
        Me.chbAlto.Name = "chbAlto"
        Me.chbAlto.Size = New System.Drawing.Size(74, 17)
        Me.chbAlto.TabIndex = 0
        Me.chbAlto.Text = "2 medidas"
        Me.chbAlto.UseVisualStyleBackColor = True
        '
        'chbAncho
        '
        Me.chbAncho.AutoSize = True
        Me.chbAncho.Location = New System.Drawing.Point(10, 61)
        Me.chbAncho.Name = "chbAncho"
        Me.chbAncho.Size = New System.Drawing.Size(74, 17)
        Me.chbAncho.TabIndex = 0
        Me.chbAncho.Text = "2 medidas"
        Me.chbAncho.UseVisualStyleBackColor = True
        '
        'lbl7
        '
        Me.lbl7.AutoSize = True
        Me.lbl7.BackColor = System.Drawing.Color.Transparent
        Me.lbl7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl7.Location = New System.Drawing.Point(710, 90)
        Me.lbl7.Name = "lbl7"
        Me.lbl7.Size = New System.Drawing.Size(28, 16)
        Me.lbl7.TabIndex = 0
        Me.lbl7.Text = "cm."
        '
        'txtAlto2
        '
        Me.txtAlto2.Location = New System.Drawing.Point(504, 89)
        Me.txtAlto2.Name = "txtAlto2"
        Me.txtAlto2.Size = New System.Drawing.Size(200, 20)
        Me.txtAlto2.TabIndex = 11
        '
        'lbl4
        '
        Me.lbl4.AutoSize = True
        Me.lbl4.BackColor = System.Drawing.Color.Transparent
        Me.lbl4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl4.Location = New System.Drawing.Point(710, 43)
        Me.lbl4.Name = "lbl4"
        Me.lbl4.Size = New System.Drawing.Size(28, 16)
        Me.lbl4.TabIndex = 0
        Me.lbl4.Text = "cm."
        '
        'txtAncho2
        '
        Me.txtAncho2.Location = New System.Drawing.Point(504, 43)
        Me.txtAncho2.Name = "txtAncho2"
        Me.txtAncho2.Size = New System.Drawing.Size(200, 20)
        Me.txtAncho2.TabIndex = 10
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(401, 91)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(28, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "cm."
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(7, 88)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(96, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Alto de Pedido"
        '
        'txtAlto
        '
        Me.txtAlto.Location = New System.Drawing.Point(195, 90)
        Me.txtAlto.Name = "txtAlto"
        Me.txtAlto.Size = New System.Drawing.Size(200, 20)
        Me.txtAlto.TabIndex = 7
        '
        'lbl2
        '
        Me.lbl2.AutoSize = True
        Me.lbl2.BackColor = System.Drawing.Color.Transparent
        Me.lbl2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl2.Location = New System.Drawing.Point(401, 44)
        Me.lbl2.Name = "lbl2"
        Me.lbl2.Size = New System.Drawing.Size(28, 16)
        Me.lbl2.TabIndex = 0
        Me.lbl2.Text = "cm."
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(7, 42)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(111, 16)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Ancho de Pedido"
        '
        'txtAncho
        '
        Me.txtAncho.Location = New System.Drawing.Point(195, 44)
        Me.txtAncho.Name = "txtAncho"
        Me.txtAncho.Size = New System.Drawing.Size(200, 20)
        Me.txtAncho.TabIndex = 6
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(3, 4)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(101, 25)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Medidas"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.Controls.Add(Me.txtNombreVidrio)
        Me.Panel4.Controls.Add(Me.btnSelectVidrio)
        Me.Panel4.Controls.Add(Me.txtEspesor)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Location = New System.Drawing.Point(6, 6)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(756, 100)
        Me.Panel4.TabIndex = 0
        '
        'txtNombreVidrio
        '
        Me.txtNombreVidrio.Enabled = False
        Me.txtNombreVidrio.Location = New System.Drawing.Point(134, 39)
        Me.txtNombreVidrio.Name = "txtNombreVidrio"
        Me.txtNombreVidrio.Size = New System.Drawing.Size(308, 20)
        Me.txtNombreVidrio.TabIndex = 2
        '
        'btnSelectVidrio
        '
        Me.btnSelectVidrio.Location = New System.Drawing.Point(458, 36)
        Me.btnSelectVidrio.Name = "btnSelectVidrio"
        Me.btnSelectVidrio.Size = New System.Drawing.Size(160, 25)
        Me.btnSelectVidrio.TabIndex = 2
        Me.btnSelectVidrio.Text = "Buscar Vidrio"
        Me.btnSelectVidrio.UseVisualStyleBackColor = True
        '
        'txtEspesor
        '
        Me.txtEspesor.FormattingEnabled = True
        Me.txtEspesor.Location = New System.Drawing.Point(134, 65)
        Me.txtEspesor.Name = "txtEspesor"
        Me.txtEspesor.Size = New System.Drawing.Size(100, 21)
        Me.txtEspesor.TabIndex = 8
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(240, 68)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "m.m."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 69)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Espesor de Vidrio"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(4, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(170, 25)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Caracteristicas"
        '
        'tabModeloVentana
        '
        Me.tabModeloVentana.Controls.Add(Me.TextBox6)
        Me.tabModeloVentana.Controls.Add(Me.Panel6)
        Me.tabModeloVentana.Controls.Add(Me.Panel3)
        Me.tabModeloVentana.Controls.Add(Me.Label32)
        Me.tabModeloVentana.Controls.Add(Me.TextBox7)
        Me.tabModeloVentana.Controls.Add(Me.Label34)
        Me.tabModeloVentana.Controls.Add(Me.Label35)
        Me.tabModeloVentana.Controls.Add(Me.TextBox8)
        Me.tabModeloVentana.Controls.Add(Me.TextBox9)
        Me.tabModeloVentana.Controls.Add(Me.Label33)
        Me.tabModeloVentana.Location = New System.Drawing.Point(4, 22)
        Me.tabModeloVentana.Name = "tabModeloVentana"
        Me.tabModeloVentana.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModeloVentana.Size = New System.Drawing.Size(773, 349)
        Me.tabModeloVentana.TabIndex = 1
        Me.tabModeloVentana.Text = "Modelo Ventana"
        Me.tabModeloVentana.UseVisualStyleBackColor = True
        '
        'TextBox6
        '
        Me.TextBox6.Enabled = False
        Me.TextBox6.Location = New System.Drawing.Point(562, 319)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(200, 20)
        Me.TextBox6.TabIndex = 22
        Me.TextBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.White
        Me.Panel6.Controls.Add(Me.Label27)
        Me.Panel6.Controls.Add(Me.TextBox2)
        Me.Panel6.Controls.Add(Me.CheckBox4)
        Me.Panel6.Controls.Add(Me.CheckBox3)
        Me.Panel6.Controls.Add(Me.Label36)
        Me.Panel6.Controls.Add(Me.txtCenVentana)
        Me.Panel6.Controls.Add(Me.Label21)
        Me.Panel6.Controls.Add(Me.Label22)
        Me.Panel6.Controls.Add(Me.Label23)
        Me.Panel6.Controls.Add(Me.Label24)
        Me.Panel6.Controls.Add(Me.Label25)
        Me.Panel6.Controls.Add(Me.txtIzqVentana)
        Me.Panel6.Controls.Add(Me.Label26)
        Me.Panel6.Controls.Add(Me.txtAbaVentana)
        Me.Panel6.Controls.Add(Me.Label28)
        Me.Panel6.Controls.Add(Me.txtDerVentana)
        Me.Panel6.Controls.Add(Me.Label30)
        Me.Panel6.Controls.Add(Me.txtArrVentana)
        Me.Panel6.Controls.Add(Me.Label31)
        Me.Panel6.Location = New System.Drawing.Point(6, 112)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(756, 165)
        Me.Panel6.TabIndex = 12
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(328, 46)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(46, 16)
        Me.Label27.TabIndex = 17
        Me.Label27.Text = "Centro"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(376, 45)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(126, 20)
        Me.TextBox2.TabIndex = 16
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Location = New System.Drawing.Point(13, 138)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(74, 17)
        Me.CheckBox4.TabIndex = 15
        Me.CheckBox4.Text = "3 medidas"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(13, 73)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(74, 17)
        Me.CheckBox3.TabIndex = 14
        Me.CheckBox3.Text = "3 medidas"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.BackColor = System.Drawing.Color.Transparent
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(327, 109)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(46, 16)
        Me.Label36.TabIndex = 0
        Me.Label36.Text = "Centro"
        '
        'txtCenVentana
        '
        Me.txtCenVentana.Location = New System.Drawing.Point(376, 107)
        Me.txtCenVentana.Name = "txtCenVentana"
        Me.txtCenVentana.Size = New System.Drawing.Size(126, 20)
        Me.txtCenVentana.TabIndex = 13
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.BackColor = System.Drawing.Color.Transparent
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(511, 105)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(62, 16)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Izquierda"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.BackColor = System.Drawing.Color.Transparent
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(529, 45)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(43, 16)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Abajo"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.BackColor = System.Drawing.Color.Transparent
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(122, 109)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(59, 16)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Derecha"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.BackColor = System.Drawing.Color.Transparent
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(136, 46)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(43, 16)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Arriba"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.BackColor = System.Drawing.Color.Transparent
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(710, 104)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(28, 16)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "cm."
        '
        'txtIzqVentana
        '
        Me.txtIzqVentana.Location = New System.Drawing.Point(578, 104)
        Me.txtIzqVentana.Name = "txtIzqVentana"
        Me.txtIzqVentana.Size = New System.Drawing.Size(126, 20)
        Me.txtIzqVentana.TabIndex = 11
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(710, 43)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(28, 16)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "cm."
        '
        'txtAbaVentana
        '
        Me.txtAbaVentana.Location = New System.Drawing.Point(578, 43)
        Me.txtAbaVentana.Name = "txtAbaVentana"
        Me.txtAbaVentana.Size = New System.Drawing.Size(126, 20)
        Me.txtAbaVentana.TabIndex = 10
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(7, 110)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(96, 16)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = "Alto de Pedido"
        '
        'txtDerVentana
        '
        Me.txtDerVentana.Location = New System.Drawing.Point(188, 108)
        Me.txtDerVentana.Name = "txtDerVentana"
        Me.txtDerVentana.Size = New System.Drawing.Size(127, 20)
        Me.txtDerVentana.TabIndex = 7
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.BackColor = System.Drawing.Color.Transparent
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(7, 42)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(111, 16)
        Me.Label30.TabIndex = 0
        Me.Label30.Text = "Ancho de Pedido"
        '
        'txtArrVentana
        '
        Me.txtArrVentana.Location = New System.Drawing.Point(188, 44)
        Me.txtArrVentana.Name = "txtArrVentana"
        Me.txtArrVentana.Size = New System.Drawing.Size(127, 20)
        Me.txtArrVentana.TabIndex = 6
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.BackColor = System.Drawing.Color.Transparent
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(3, 4)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(101, 25)
        Me.Label31.TabIndex = 0
        Me.Label31.Text = "Medidas"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.cmbColorAlumnio)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Location = New System.Drawing.Point(6, 6)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(756, 100)
        Me.Panel3.TabIndex = 0
        '
        'cmbColorAlumnio
        '
        Me.cmbColorAlumnio.FormattingEnabled = True
        Me.cmbColorAlumnio.Items.AddRange(New Object() {"Estandar", "Negro", "Madera"})
        Me.cmbColorAlumnio.Location = New System.Drawing.Point(128, 35)
        Me.cmbColorAlumnio.Name = "cmbColorAlumnio"
        Me.cmbColorAlumnio.Size = New System.Drawing.Size(226, 21)
        Me.cmbColorAlumnio.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 36)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Color de Aluminio"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(170, 25)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Caracteristicas"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.Color.Transparent
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(488, 324)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(68, 16)
        Me.Label32.TabIndex = 18
        Me.Label32.Text = "Total (S/.):"
        '
        'TextBox7
        '
        Me.TextBox7.Location = New System.Drawing.Point(562, 293)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.Size = New System.Drawing.Size(200, 20)
        Me.TextBox7.TabIndex = 25
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.BackColor = System.Drawing.Color.Transparent
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(114, 297)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(74, 16)
        Me.Label34.TabIndex = 20
        Me.Label34.Text = "Extras (S/.):"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.BackColor = System.Drawing.Color.Transparent
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(11, 324)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(182, 16)
        Me.Label35.TabIndex = 21
        Me.Label35.Text = "Precio de los materiales (S/.):"
        '
        'TextBox8
        '
        Me.TextBox8.Location = New System.Drawing.Point(201, 292)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.Size = New System.Drawing.Size(200, 20)
        Me.TextBox8.TabIndex = 24
        '
        'TextBox9
        '
        Me.TextBox9.Enabled = False
        Me.TextBox9.Location = New System.Drawing.Point(201, 319)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.Size = New System.Drawing.Size(200, 20)
        Me.TextBox9.TabIndex = 23
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.BackColor = System.Drawing.Color.Transparent
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(435, 296)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(121, 16)
        Me.Label33.TabIndex = 19
        Me.Label33.Text = "Mano de obra (S/.):"
        '
        'btnDuplicar
        '
        Me.btnDuplicar.Location = New System.Drawing.Point(16, 498)
        Me.btnDuplicar.Name = "btnDuplicar"
        Me.btnDuplicar.Size = New System.Drawing.Size(75, 23)
        Me.btnDuplicar.TabIndex = 20
        Me.btnDuplicar.Text = "Duplicar"
        Me.btnDuplicar.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(129, 498)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(75, 23)
        Me.btnLimpiar.TabIndex = 19
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Location = New System.Drawing.Point(210, 498)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(75, 23)
        Me.btnEliminar.TabIndex = 18
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnGuardarPedido
        '
        Me.btnGuardarPedido.Location = New System.Drawing.Point(629, 499)
        Me.btnGuardarPedido.Name = "btnGuardarPedido"
        Me.btnGuardarPedido.Size = New System.Drawing.Size(158, 23)
        Me.btnGuardarPedido.TabIndex = 0
        Me.btnGuardarPedido.Text = "Guarda todo el Pedido"
        Me.btnGuardarPedido.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.BackColor = System.Drawing.Color.Transparent
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(343, 501)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(68, 16)
        Me.Label20.TabIndex = 18
        Me.Label20.Text = "Total (S/.):"
        '
        'TextBox1
        '
        Me.TextBox1.Enabled = False
        Me.TextBox1.Location = New System.Drawing.Point(417, 500)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(200, 20)
        Me.TextBox1.TabIndex = 18
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmRegistrarPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 534)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.btnEliminar)
        Me.Controls.Add(Me.btnLimpiar)
        Me.Controls.Add(Me.btnDuplicar)
        Me.Controls.Add(Me.btnGuardarPedido)
        Me.Controls.Add(Me.tabsPedidos)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmRegistrarPedido"
        Me.Text = "frmRegistrarPedido"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.tabsPedidos.ResumeLayout(False)
        Me.tabModeloVidrio.ResumeLayout(False)
        Me.tabModeloVidrio.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.tabModeloVentana.ResumeLayout(False)
        Me.tabModeloVentana.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents cmbPedido As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNombreCliente As TextBox
    Friend WithEvents btnBuscarCliente As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnAgregarPedido As Button
    Friend WithEvents tabsPedidos As TabControl
    Friend WithEvents tabModeloVidrio As TabPage
    Friend WithEvents tabModeloVentana As TabPage
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbColorAlumnio As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtAlto As TextBox
    Friend WithEvents lbl2 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtAncho As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents btnGuardarPedido As Button
    Friend WithEvents chbAncho As CheckBox
    Friend WithEvents lbl7 As Label
    Friend WithEvents txtAlto2 As TextBox
    Friend WithEvents lbl4 As Label
    Friend WithEvents txtAncho2 As TextBox
    Friend WithEvents chbAlto As CheckBox
    Friend WithEvents lbl6 As Label
    Friend WithEvents lbl3 As Label
    Friend WithEvents lbl5 As Label
    Friend WithEvents lbl1 As Label
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtManoObra As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtExtra As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtPreciomaterial As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents btnDuplicar As Button
    Friend WithEvents btnLimpiar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents Label20 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents Label24 As Label
    Friend WithEvents Label25 As Label
    Friend WithEvents txtIzqVentana As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtAbaVentana As TextBox
    Friend WithEvents Label28 As Label
    Friend WithEvents txtDerVentana As TextBox
    Friend WithEvents Label30 As Label
    Friend WithEvents txtArrVentana As TextBox
    Friend WithEvents Label31 As Label
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Label32 As Label
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents Label33 As Label
    Friend WithEvents Label35 As Label
    Friend WithEvents TextBox9 As TextBox
    Friend WithEvents Label34 As Label
    Friend WithEvents TextBox8 As TextBox
    Friend WithEvents CheckBox4 As CheckBox
    Friend WithEvents CheckBox3 As CheckBox
    Friend WithEvents Label36 As Label
    Friend WithEvents txtCenVentana As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents txtEspesor As ComboBox
    Friend WithEvents txtNombreVidrio As TextBox
    Friend WithEvents btnSelectVidrio As Button
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarPedido
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
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cmbColorVIdrio = New System.Windows.Forms.ComboBox()
        Me.cmbTipoVidrio = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtEspesor = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tabModeloVentana = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.cmbColorAlumnio = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnGuardarPedido = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabsPedidos.SuspendLayout()
        Me.tabModeloVidrio.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.tabModeloVentana.SuspendLayout()
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
        Me.Panel2.Location = New System.Drawing.Point(382, 12)
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
        Me.Panel5.Size = New System.Drawing.Size(756, 147)
        Me.Panel5.TabIndex = 0
        '
        'lbl6
        '
        Me.lbl6.AutoSize = True
        Me.lbl6.BackColor = System.Drawing.Color.Transparent
        Me.lbl6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl6.Location = New System.Drawing.Point(455, 91)
        Me.lbl6.Name = "lbl6"
        Me.lbl6.Size = New System.Drawing.Size(43, 16)
        Me.lbl6.TabIndex = 16
        Me.lbl6.Text = "Abajo"
        '
        'lbl3
        '
        Me.lbl3.AutoSize = True
        Me.lbl3.BackColor = System.Drawing.Color.Transparent
        Me.lbl3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3.Location = New System.Drawing.Point(439, 45)
        Me.lbl3.Name = "lbl3"
        Me.lbl3.Size = New System.Drawing.Size(62, 16)
        Me.lbl3.TabIndex = 15
        Me.lbl3.Text = "Izquierda"
        '
        'lbl5
        '
        Me.lbl5.AutoSize = True
        Me.lbl5.BackColor = System.Drawing.Color.Transparent
        Me.lbl5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl5.Location = New System.Drawing.Point(147, 91)
        Me.lbl5.Name = "lbl5"
        Me.lbl5.Size = New System.Drawing.Size(43, 16)
        Me.lbl5.TabIndex = 14
        Me.lbl5.Text = "Arriba"
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.BackColor = System.Drawing.Color.Transparent
        Me.lbl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.Location = New System.Drawing.Point(131, 44)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(59, 16)
        Me.lbl1.TabIndex = 13
        Me.lbl1.Text = "Derecha"
        '
        'chbAlto
        '
        Me.chbAlto.AutoSize = True
        Me.chbAlto.Location = New System.Drawing.Point(10, 107)
        Me.chbAlto.Name = "chbAlto"
        Me.chbAlto.Size = New System.Drawing.Size(74, 17)
        Me.chbAlto.TabIndex = 12
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
        Me.lbl7.TabIndex = 8
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
        Me.lbl4.TabIndex = 9
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
        Me.Panel4.Controls.Add(Me.Label16)
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.cmbColorVIdrio)
        Me.Panel4.Controls.Add(Me.cmbTipoVidrio)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.txtEspesor)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Location = New System.Drawing.Point(6, 6)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(756, 100)
        Me.Panel4.TabIndex = 0
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(344, 63)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(96, 16)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Color de Vidrio"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(344, 36)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(92, 16)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Tipo de Vidrio"
        '
        'cmbColorVIdrio
        '
        Me.cmbColorVIdrio.FormattingEnabled = True
        Me.cmbColorVIdrio.Items.AddRange(New Object() {"Incoloro", "Azul", "Azul Ezpejado", "Catedral", "Espejo Solido"})
        Me.cmbColorVIdrio.Location = New System.Drawing.Point(451, 61)
        Me.cmbColorVIdrio.Name = "cmbColorVIdrio"
        Me.cmbColorVIdrio.Size = New System.Drawing.Size(280, 21)
        Me.cmbColorVIdrio.TabIndex = 5
        '
        'cmbTipoVidrio
        '
        Me.cmbTipoVidrio.FormattingEnabled = True
        Me.cmbTipoVidrio.Items.AddRange(New Object() {"Cuadrado", "Circulo"})
        Me.cmbTipoVidrio.Location = New System.Drawing.Point(451, 34)
        Me.cmbTipoVidrio.Name = "cmbTipoVidrio"
        Me.cmbTipoVidrio.Size = New System.Drawing.Size(280, 21)
        Me.cmbTipoVidrio.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(237, 35)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(35, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "m.m."
        '
        'txtEspesor
        '
        Me.txtEspesor.Location = New System.Drawing.Point(131, 34)
        Me.txtEspesor.Name = "txtEspesor"
        Me.txtEspesor.Size = New System.Drawing.Size(100, 20)
        Me.txtEspesor.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(10, 36)
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
        Me.tabModeloVentana.Controls.Add(Me.Panel3)
        Me.tabModeloVentana.Location = New System.Drawing.Point(4, 22)
        Me.tabModeloVentana.Name = "tabModeloVentana"
        Me.tabModeloVentana.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModeloVentana.Size = New System.Drawing.Size(773, 349)
        Me.tabModeloVentana.TabIndex = 1
        Me.tabModeloVentana.Text = "Modelo Ventana"
        Me.tabModeloVentana.UseVisualStyleBackColor = True
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
        'btnGuardarPedido
        '
        Me.btnGuardarPedido.Location = New System.Drawing.Point(629, 499)
        Me.btnGuardarPedido.Name = "btnGuardarPedido"
        Me.btnGuardarPedido.Size = New System.Drawing.Size(158, 23)
        Me.btnGuardarPedido.TabIndex = 0
        Me.btnGuardarPedido.Text = "Guarda todo el Pedido"
        Me.btnGuardarPedido.UseVisualStyleBackColor = True
        '
        'frmRegistrarPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 534)
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
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.tabModeloVentana.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

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
    Friend WithEvents txtEspesor As TextBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents txtAlto As TextBox
    Friend WithEvents lbl2 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtAncho As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents cmbTipoVidrio As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents cmbColorVIdrio As ComboBox
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
End Class

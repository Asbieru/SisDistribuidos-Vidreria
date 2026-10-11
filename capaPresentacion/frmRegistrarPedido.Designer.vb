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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tabsPedidos = New System.Windows.Forms.TabControl()
        Me.tabModeloVidrio = New System.Windows.Forms.TabPage()
        Me.tabModuloVentana = New System.Windows.Forms.TabPage()
        Me.btnAgregarPedido = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabsPedidos.SuspendLayout()
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
        Me.cmbPedido.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
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
        Me.tabsPedidos.Controls.Add(Me.tabModuloVentana)
        Me.tabsPedidos.Location = New System.Drawing.Point(12, 117)
        Me.tabsPedidos.Name = "tabsPedidos"
        Me.tabsPedidos.SelectedIndex = 0
        Me.tabsPedidos.Size = New System.Drawing.Size(781, 375)
        Me.tabsPedidos.TabIndex = 0
        '
        'tabModeloVidrio
        '
        Me.tabModeloVidrio.Location = New System.Drawing.Point(4, 22)
        Me.tabModeloVidrio.Name = "tabModeloVidrio"
        Me.tabModeloVidrio.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModeloVidrio.Size = New System.Drawing.Size(773, 349)
        Me.tabModeloVidrio.TabIndex = 0
        Me.tabModeloVidrio.Text = "Modelo Vidrio"
        Me.tabModeloVidrio.UseVisualStyleBackColor = True
        '
        'tabModuloVentana
        '
        Me.tabModuloVentana.Location = New System.Drawing.Point(4, 22)
        Me.tabModuloVentana.Name = "tabModuloVentana"
        Me.tabModuloVentana.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModuloVentana.Size = New System.Drawing.Size(773, 285)
        Me.tabModuloVentana.TabIndex = 1
        Me.tabModuloVentana.Text = "Modulo Ventana"
        Me.tabModuloVentana.UseVisualStyleBackColor = True
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
        'frmRegistrarPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 504)
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
    Friend WithEvents tabModuloVentana As TabPage
End Class

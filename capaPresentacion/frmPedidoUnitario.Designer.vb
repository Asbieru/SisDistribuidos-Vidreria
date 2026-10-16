<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPedidoUnitario
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
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()

        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblSubTitulo = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.tabModelo = New System.Windows.Forms.TabPage()
        Me.btnCancelarItems = New System.Windows.Forms.Button()
        Me.btnItemRoto = New System.Windows.Forms.Button()
        Me.tblItems = New System.Windows.Forms.DataGridView()
        Me.txtPrecio = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTipoPedido = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabItems = New System.Windows.Forms.TabControl()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.btnPagar = New System.Windows.Forms.Button()
        Me.btnTerminar = New System.Windows.Forms.Button()
        Me.btnTerminarUnitario = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.tabModelo.SuspendLayout()
        CType(Me.tblItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabItems.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = False
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(12, 8)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTitulo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(850, 30)
        Me.lblTitulo.TabIndex = 3
        Me.lblTitulo.Text = "Pedido"
        '
        'lblSubTitulo
        '
        Me.lblSubTitulo.AutoSize = False
        Me.lblSubTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblSubTitulo.Location = New System.Drawing.Point(12, 42)
        Me.lblSubTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSubTitulo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblSubTitulo.Name = "lblSubTitulo"
        Me.lblSubTitulo.Size = New System.Drawing.Size(850, 24)
        Me.lblSubTitulo.TabIndex = 0
        Me.lblSubTitulo.Text = "subtitulo"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblEstado)
        Me.Panel1.Controls.Add(Me.lblSubTitulo)
        Me.Panel1.Controls.Add(Me.lblTitulo)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Height = 100
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(705, 77)
        Me.Panel1.TabIndex = 4
        '
        'lblEstado
        '
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblEstado.Location = New System.Drawing.Point(12, 70)
        Me.lblEstado.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEstado.AutoSize = False
        Me.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(850, 22)
        Me.lblEstado.TabIndex = 0
        Me.lblEstado.Text = "estado"
        Me.lblEstado.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'tabModelo
        '
        Me.tabModelo.Controls.Add(Me.btnTerminarUnitario)
        Me.tabModelo.Controls.Add(Me.btnCancelarItems)
        Me.tabModelo.Controls.Add(Me.btnItemRoto)
        Me.tabModelo.Controls.Add(Me.tblItems)
        Me.tabModelo.Controls.Add(Me.txtPrecio)
        Me.tabModelo.Controls.Add(Me.Label2)
        Me.tabModelo.Controls.Add(Me.txtTipoPedido)
        Me.tabModelo.Controls.Add(Me.Label1)
        Me.tabModelo.Location = New System.Drawing.Point(4, 22)
        Me.tabModelo.Name = "tabModelo"
        Me.tabModelo.Padding = New System.Windows.Forms.Padding(3)
        Me.tabModelo.Size = New System.Drawing.Size(697, 310)
        Me.tabModelo.TabIndex = 0
        Me.tabModelo.Text = "Modelo"
        Me.tabModelo.UseVisualStyleBackColor = True
        '
        'btnCancelarItems
        '
        Me.btnCancelarItems.Location = New System.Drawing.Point(550, 274)
        Me.btnCancelarItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelarItems.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCancelarItems.Name = "btnCancelarItems"
        Me.btnCancelarItems.Size = New System.Drawing.Size(145, 34)
        Me.btnCancelarItems.TabIndex = 7
        Me.btnCancelarItems.Text = "Cancelar Pedido"
        Me.btnCancelarItems.UseVisualStyleBackColor = True
        '
        'btnItemRoto
        '
        Me.btnItemRoto.Location = New System.Drawing.Point(420, 274)
        Me.btnItemRoto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnItemRoto.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnItemRoto.Name = "btnItemRoto"
        Me.btnItemRoto.Size = New System.Drawing.Size(120, 34)
        Me.btnItemRoto.TabIndex = 7
        Me.btnItemRoto.Text = "Item Roto"
        Me.btnItemRoto.UseVisualStyleBackColor = True
        '
        'tblItems
        '
        Me.tblItems.AllowUserToAddRows = False
        Me.tblItems.AllowUserToDeleteRows = False
        Me.tblItems.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.tblItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.tblItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.tblItems.Location = New System.Drawing.Point(16, 63)
        Me.tblItems.Margin = New System.Windows.Forms.Padding(2)
        Me.tblItems.MultiSelect = False
        Me.tblItems.Height = 196
        Me.tblItems.Name = "tblItems"
        Me.tblItems.ReadOnly = True
        Me.tblItems.RowHeadersVisible = False
        Me.tblItems.RowHeadersWidth = 51
        Me.tblItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tblItems.Size = New System.Drawing.Size(662, 206)
        Me.tblItems.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.tblItems.TabIndex = 4
        '
        'txtPrecio
        '
        Me.txtPrecio.Location = New System.Drawing.Point(571, 19)
        Me.txtPrecio.Name = "txtPrecio"
        Me.txtPrecio.Size = New System.Drawing.Size(107, 20)
        Me.txtPrecio.TabIndex = 3
        Me.txtPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(489, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Costo Total"
        '
        'txtTipoPedido
        '
        Me.txtTipoPedido.Location = New System.Drawing.Point(123, 17)
        Me.txtTipoPedido.Name = "txtTipoPedido"
        Me.txtTipoPedido.Size = New System.Drawing.Size(319, 20)
        Me.txtTipoPedido.TabIndex = 1
        Me.txtTipoPedido.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(13, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tipo de pedido"
        '
        'tabItems
        '
        Me.tabItems.Controls.Add(Me.tabModelo)
        Me.tabItems.Location = New System.Drawing.Point(12, 95)
        Me.tabItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabItems.Padding = New System.Drawing.Point(12, 6)
        Me.tabItems.ShowToolTips = True
        Me.tabItems.Name = "tabItems"
        Me.tabItems.SelectedIndex = 0
        Me.tabItems.Size = New System.Drawing.Size(705, 336)
        Me.tabItems.TabIndex = 5
        '
        'btnCerrar
        '
        Me.btnCerrar.Location = New System.Drawing.Point(619, 437)
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(120, 34)
        Me.btnCerrar.TabIndex = 6
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'btnPagar
        '
        Me.btnPagar.Location = New System.Drawing.Point(139, 437)
        Me.btnPagar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnPagar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnPagar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnPagar.Name = "btnPagar"
        Me.btnPagar.Size = New System.Drawing.Size(120, 34)
        Me.btnPagar.TabIndex = 6
        Me.btnPagar.Text = "Pagar Todo"
        Me.btnPagar.UseVisualStyleBackColor = True
        '
        'btnTerminar
        '
        Me.btnTerminar.Location = New System.Drawing.Point(12, 437)
        Me.btnTerminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnTerminar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnTerminar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnTerminar.Name = "btnTerminar"
        Me.btnTerminar.Size = New System.Drawing.Size(150, 34)
        Me.btnTerminar.TabIndex = 6
        Me.btnTerminar.Text = "Terminar Pedido"
        Me.btnTerminar.UseVisualStyleBackColor = True
        '
        'btnTerminarUnitario
        '
        Me.btnTerminarUnitario.Location = New System.Drawing.Point(16, 274)
        Me.btnTerminarUnitario.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnTerminarUnitario.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.btnTerminarUnitario.Name = "btnTerminarUnitario"
        Me.btnTerminarUnitario.Size = New System.Drawing.Size(180, 34)
        Me.btnTerminarUnitario.TabIndex = 7
        Me.btnTerminarUnitario.Text = "Registrar Culminación"
        Me.btnTerminarUnitario.UseVisualStyleBackColor = True
        '
        'frmPedidoUnitario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 600)
        Me.Controls.Add(Me.tabItems)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(816, 500)
        Me.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Name = "frmPedidoUnitario"
        Me.Text = "Pedido"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabModelo.ResumeLayout(False)
        Me.tabModelo.PerformLayout()
        CType(Me.tblItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabItems.ResumeLayout(False)

        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.Padding = New System.Windows.Forms.Padding(0, 8, 0, 0)
        Me.flpAcciones.Controls.Add(Me.btnTerminar)
        Me.flpAcciones.Controls.Add(Me.btnPagar)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.flpAcciones)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblSubTitulo As Label
    Friend WithEvents lblEstado As Label
    Friend WithEvents tabModelo As TabPage
    Friend WithEvents txtPrecio As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTipoPedido As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents tabItems As TabControl
    Friend WithEvents Panel1 As Panel
    Friend WithEvents tblItems As DataGridView
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnPagar As Button
    Friend WithEvents btnTerminar As Button
    Friend WithEvents btnItemRoto As Button
    Friend WithEvents btnCancelarItems As Button
    Friend WithEvents btnTerminarUnitario As Button
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

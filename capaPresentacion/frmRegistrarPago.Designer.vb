<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistrarPago
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
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.btnSelectPedido = New System.Windows.Forms.Button()
        Me.lblSubTitulo = New System.Windows.Forms.Label()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.pnlProceso = New System.Windows.Forms.Panel()
        Me.msj = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMetodoPago = New System.Windows.Forms.ComboBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.tblPagos = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chbVuelto = New System.Windows.Forms.CheckBox()
        Me.lblPagado = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.pnlProceso.SuspendLayout()
        CType(Me.tblPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblPagado)
        Me.Panel1.Controls.Add(Me.lblEstado)
        Me.Panel1.Controls.Add(Me.btnSelectPedido)
        Me.Panel1.Controls.Add(Me.lblSubTitulo)
        Me.Panel1.Controls.Add(Me.lblTitulo)
        Me.Panel1.Location = New System.Drawing.Point(24, 20)
        Me.Panel1.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(802, 100)
        Me.Panel1.TabIndex = 5
        '
        'lblEstado
        '
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblEstado.Location = New System.Drawing.Point(620, 46)
        Me.lblEstado.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEstado.AutoSize = False
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(170, 24)
        Me.lblEstado.TabIndex = 5
        Me.lblEstado.Text = "estado"
        Me.lblEstado.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnSelectPedido
        '
        Me.btnSelectPedido.Location = New System.Drawing.Point(620, 4)
        Me.btnSelectPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnSelectPedido.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        Me.btnSelectPedido.Name = "btnSelectPedido"
        Me.btnSelectPedido.Size = New System.Drawing.Size(170, 34)
        Me.btnSelectPedido.TabIndex = 4
        Me.btnSelectPedido.Text = "Buscar Pedido"
        Me.btnSelectPedido.UseVisualStyleBackColor = True
        '
        'lblSubTitulo
        '
        Me.lblSubTitulo.AutoSize = False
        Me.lblSubTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblSubTitulo.Location = New System.Drawing.Point(12, 46)
        Me.lblSubTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSubTitulo.AutoEllipsis = True
        Me.lblSubTitulo.Name = "lblSubTitulo"
        Me.lblSubTitulo.Size = New System.Drawing.Size(420, 24)
        Me.lblSubTitulo.TabIndex = 0
        Me.lblSubTitulo.Text = "subtitulo"
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = False
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(12, 8)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTitulo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(590, 32)
        Me.lblTitulo.TabIndex = 3
        Me.lblTitulo.Text = "Pedido"
        '
        'pnlProceso
        '
        Me.pnlProceso.Controls.Add(Me.chbVuelto)
        Me.pnlProceso.Controls.Add(Me.msj)
        Me.pnlProceso.Controls.Add(Me.txtCantidad)
        Me.pnlProceso.Controls.Add(Me.Label2)
        Me.pnlProceso.Controls.Add(Me.cmbMetodoPago)
        Me.pnlProceso.Controls.Add(Me.btnGuardar)
        Me.pnlProceso.Controls.Add(Me.btnCancelar)
        Me.pnlProceso.Controls.Add(Me.btnModificar)
        Me.pnlProceso.Controls.Add(Me.btnEliminar)
        Me.pnlProceso.Controls.Add(Me.tblPagos)
        Me.pnlProceso.Controls.Add(Me.Label1)
        Me.pnlProceso.Location = New System.Drawing.Point(24, 136)
        Me.pnlProceso.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.pnlProceso.Name = "pnlProceso"
        Me.pnlProceso.Size = New System.Drawing.Size(802, 374)
        Me.pnlProceso.TabIndex = 6
        '
        'msj
        '
        Me.msj.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.msj.ForeColor = System.Drawing.Color.Red
        Me.msj.Location = New System.Drawing.Point(16, 310)
        Me.msj.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.msj.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        Me.msj.AutoSize = False
        Me.msj.Name = "msj"
        Me.msj.Size = New System.Drawing.Size(250, 54)
        Me.msj.TabIndex = 0
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(492, 21)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(191, 20)
        Me.txtCantidad.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(389, 20)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(98, 18)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Cantidad (S/.)"
        '
        'cmbMetodoPago
        '
        Me.cmbMetodoPago.FormattingEnabled = True
        Me.cmbMetodoPago.Location = New System.Drawing.Point(139, 20)
        Me.cmbMetodoPago.Name = "cmbMetodoPago"
        Me.cmbMetodoPago.Size = New System.Drawing.Size(218, 21)
        Me.cmbMetodoPago.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(274, 322)
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 6
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(402, 322)
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.Location = New System.Drawing.Point(530, 322)
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(120, 34)
        Me.btnModificar.TabIndex = 6
        Me.btnModificar.Text = "Modificar"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Location = New System.Drawing.Point(658, 322)
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(120, 34)
        Me.btnEliminar.TabIndex = 6
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'tblPagos
        '
        Me.tblPagos.AllowUserToAddRows = False
        Me.tblPagos.AllowUserToDeleteRows = False
        Me.tblPagos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.tblPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.tblPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.tblPagos.Location = New System.Drawing.Point(16, 64)
        Me.tblPagos.Margin = New System.Windows.Forms.Padding(2)
        Me.tblPagos.MultiSelect = False
        Me.tblPagos.Name = "tblPagos"
        Me.tblPagos.ReadOnly = True
        Me.tblPagos.RowHeadersVisible = False
        Me.tblPagos.RowHeadersWidth = 51
        Me.tblPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tblPagos.Size = New System.Drawing.Size(770, 236)
        Me.tblPagos.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.tblPagos.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(16, 20)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Metodo de Pago"
        '
        'chbVuelto
        '
        Me.chbVuelto.AutoSize = True
        Me.chbVuelto.Location = New System.Drawing.Point(710, 20)
        Me.chbVuelto.Name = "chbVuelto"
        Me.chbVuelto.Size = New System.Drawing.Size(80, 24)
        Me.chbVuelto.TabIndex = 10
        Me.chbVuelto.Text = "Vuelto"
        Me.chbVuelto.UseVisualStyleBackColor = True
        '
        'lblPagado
        '
        Me.lblPagado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblPagado.Location = New System.Drawing.Point(450, 46)
        Me.lblPagado.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPagado.Name = "lblPagado"
        Me.lblPagado.Size = New System.Drawing.Size(150, 24)
        Me.lblPagado.TabIndex = 6
        Me.lblPagado.Text = "estado"
        Me.lblPagado.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmRegistrarPago
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 530)
        Me.Controls.Add(Me.pnlProceso)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(750, 0)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Name = "frmRegistrarPago"
        Me.Text = "Registrar pago"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlProceso.ResumeLayout(False)
        Me.pnlProceso.PerformLayout()
        CType(Me.tblPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblSubTitulo As Label
    Friend WithEvents lblTitulo As Label
    Friend WithEvents btnSelectPedido As Button
    Friend WithEvents pnlProceso As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents tblPagos As DataGridView
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnModificar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents txtCantidad As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbMetodoPago As ComboBox
    Friend WithEvents lblEstado As Label
    Friend WithEvents msj As Label
    Friend WithEvents chbVuelto As CheckBox
    Friend WithEvents lblPagado As Label
End Class

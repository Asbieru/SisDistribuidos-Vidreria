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
        Me.tlpPago = New System.Windows.Forms.TableLayoutPanel()
        Me.flpDatosPago = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlMetodo = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbMetodoPago = New System.Windows.Forms.ComboBox()
        Me.pnlImporte = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.chbVuelto = New System.Windows.Forms.CheckBox()
        Me.tblPagos = New System.Windows.Forms.DataGridView()
        Me.flpAccionesPago = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.msj = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblPagado = New System.Windows.Forms.Label()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.btnSelectPedido = New System.Windows.Forms.Button()
        Me.lblSubTitulo = New System.Windows.Forms.Label()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.pnlProceso = New System.Windows.Forms.Panel()
        Me.tlpPago.SuspendLayout()
        Me.flpDatosPago.SuspendLayout()
        Me.pnlMetodo.SuspendLayout()
        Me.pnlImporte.SuspendLayout()
        CType(Me.tblPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flpAccionesPago.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlProceso.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpPago
        '
        Me.tlpPago.ColumnCount = 1
        Me.tlpPago.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpPago.Controls.Add(Me.flpDatosPago, 0, 0)
        Me.tlpPago.Controls.Add(Me.tblPagos, 0, 1)
        Me.tlpPago.Controls.Add(Me.flpAccionesPago, 0, 2)
        Me.tlpPago.Controls.Add(Me.msj, 0, 3)
        Me.tlpPago.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpPago.Location = New System.Drawing.Point(0, 0)
        Me.tlpPago.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpPago.Name = "tlpPago"
        Me.tlpPago.Padding = New System.Windows.Forms.Padding(12)
        Me.tlpPago.RowCount = 4
        Me.tlpPago.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpPago.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpPago.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpPago.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpPago.Size = New System.Drawing.Size(802, 366)
        Me.tlpPago.TabIndex = 0
        '
        'flpDatosPago
        '
        Me.flpDatosPago.AutoSize = True
        Me.flpDatosPago.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpDatosPago.Controls.Add(Me.pnlMetodo)
        Me.flpDatosPago.Controls.Add(Me.pnlImporte)
        Me.flpDatosPago.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpDatosPago.Location = New System.Drawing.Point(12, 12)
        Me.flpDatosPago.Margin = New System.Windows.Forms.Padding(0)
        Me.flpDatosPago.Name = "flpDatosPago"
        Me.flpDatosPago.Size = New System.Drawing.Size(778, 70)
        Me.flpDatosPago.TabIndex = 0
        '
        'pnlMetodo
        '
        Me.pnlMetodo.Controls.Add(Me.Label1)
        Me.pnlMetodo.Controls.Add(Me.cmbMetodoPago)
        Me.pnlMetodo.Location = New System.Drawing.Point(0, 0)
        Me.pnlMetodo.Margin = New System.Windows.Forms.Padding(0, 0, 16, 8)
        Me.pnlMetodo.Name = "pnlMetodo"
        Me.pnlMetodo.Size = New System.Drawing.Size(391, 62)
        Me.pnlMetodo.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(9, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Metodo de Pago"
        '
        'cmbMetodoPago
        '
        Me.cmbMetodoPago.FormattingEnabled = True
        Me.cmbMetodoPago.Location = New System.Drawing.Point(12, 28)
        Me.cmbMetodoPago.Name = "cmbMetodoPago"
        Me.cmbMetodoPago.Size = New System.Drawing.Size(364, 24)
        Me.cmbMetodoPago.TabIndex = 2
        '
        'pnlImporte
        '
        Me.pnlImporte.Controls.Add(Me.Label2)
        Me.pnlImporte.Controls.Add(Me.txtCantidad)
        Me.pnlImporte.Controls.Add(Me.chbVuelto)
        Me.pnlImporte.Location = New System.Drawing.Point(407, 0)
        Me.pnlImporte.Margin = New System.Windows.Forms.Padding(0, 0, 16, 8)
        Me.pnlImporte.Name = "pnlImporte"
        Me.pnlImporte.Size = New System.Drawing.Size(351, 62)
        Me.pnlImporte.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(6, 6)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Cantidad (S/.)"
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(16, 28)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(247, 22)
        Me.txtCantidad.TabIndex = 3
        '
        'chbVuelto
        '
        Me.chbVuelto.AutoSize = True
        Me.chbVuelto.Location = New System.Drawing.Point(269, 30)
        Me.chbVuelto.Name = "chbVuelto"
        Me.chbVuelto.Size = New System.Drawing.Size(64, 20)
        Me.chbVuelto.TabIndex = 4
        Me.chbVuelto.Text = "Vuelto"
        Me.chbVuelto.UseVisualStyleBackColor = True
        '
        'tblPagos
        '
        Me.tblPagos.AllowUserToAddRows = False
        Me.tblPagos.AllowUserToDeleteRows = False
        Me.tblPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.tblPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.tblPagos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblPagos.Location = New System.Drawing.Point(12, 82)
        Me.tblPagos.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.tblPagos.MultiSelect = False
        Me.tblPagos.Name = "tblPagos"
        Me.tblPagos.ReadOnly = True
        Me.tblPagos.RowHeadersVisible = False
        Me.tblPagos.RowHeadersWidth = 51
        Me.tblPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tblPagos.Size = New System.Drawing.Size(778, 202)
        Me.tblPagos.TabIndex = 0
        '
        'flpAccionesPago
        '
        Me.flpAccionesPago.AutoSize = True
        Me.flpAccionesPago.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAccionesPago.Controls.Add(Me.btnGuardar)
        Me.flpAccionesPago.Controls.Add(Me.btnCancelar)
        Me.flpAccionesPago.Controls.Add(Me.btnModificar)
        Me.flpAccionesPago.Controls.Add(Me.btnEliminar)
        Me.flpAccionesPago.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAccionesPago.Location = New System.Drawing.Point(12, 296)
        Me.flpAccionesPago.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAccionesPago.Name = "flpAccionesPago"
        Me.flpAccionesPago.Size = New System.Drawing.Size(778, 42)
        Me.flpAccionesPago.TabIndex = 6
        '
        'btnGuardar
        '
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(0, 0)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(120, 34)
        Me.btnGuardar.TabIndex = 5
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(128, 0)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(120, 34)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Location = New System.Drawing.Point(256, 0)
        Me.btnModificar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(120, 34)
        Me.btnModificar.TabIndex = 7
        Me.btnModificar.Text = "Modificar"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Location = New System.Drawing.Point(384, 0)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(120, 34)
        Me.btnEliminar.TabIndex = 8
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'msj
        '
        Me.msj.AutoSize = True
        Me.msj.Dock = System.Windows.Forms.DockStyle.Fill
        Me.msj.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.msj.ForeColor = System.Drawing.Color.Red
        Me.msj.Location = New System.Drawing.Point(12, 338)
        Me.msj.Margin = New System.Windows.Forms.Padding(0)
        Me.msj.Name = "msj"
        Me.msj.Size = New System.Drawing.Size(778, 16)
        Me.msj.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.lblPagado)
        Me.Panel1.Controls.Add(Me.lblEstado)
        Me.Panel1.Controls.Add(Me.btnSelectPedido)
        Me.Panel1.Controls.Add(Me.lblSubTitulo)
        Me.Panel1.Controls.Add(Me.lblTitulo)
        Me.Panel1.Location = New System.Drawing.Point(24, 20)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(802, 112)
        Me.Panel1.TabIndex = 5
        '
        'lblPagado
        '
        Me.lblPagado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblPagado.Location = New System.Drawing.Point(12, 80)
        Me.lblPagado.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPagado.Name = "lblPagado"
        Me.lblPagado.Size = New System.Drawing.Size(300, 24)
        Me.lblPagado.TabIndex = 0
        Me.lblPagado.Text = "estado"
        Me.lblPagado.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblEstado
        '
        Me.lblEstado.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblEstado.Location = New System.Drawing.Point(420, 80)
        Me.lblEstado.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(360, 24)
        Me.lblEstado.TabIndex = 0
        Me.lblEstado.Text = "estado"
        Me.lblEstado.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnSelectPedido
        '
        Me.btnSelectPedido.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSelectPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnSelectPedido.Location = New System.Drawing.Point(620, 9)
        Me.btnSelectPedido.Name = "btnSelectPedido"
        Me.btnSelectPedido.Size = New System.Drawing.Size(170, 34)
        Me.btnSelectPedido.TabIndex = 1
        Me.btnSelectPedido.Text = "Buscar Pedido"
        Me.btnSelectPedido.UseVisualStyleBackColor = True
        '
        'lblSubTitulo
        '
        Me.lblSubTitulo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSubTitulo.AutoEllipsis = True
        Me.lblSubTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblSubTitulo.Location = New System.Drawing.Point(12, 46)
        Me.lblSubTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblSubTitulo.Name = "lblSubTitulo"
        Me.lblSubTitulo.Size = New System.Drawing.Size(770, 24)
        Me.lblSubTitulo.TabIndex = 0
        Me.lblSubTitulo.Text = "subtitulo"
        '
        'lblTitulo
        '
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(12, 8)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(520, 32)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Pedido"
        '
        'pnlProceso
        '
        Me.pnlProceso.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlProceso.Controls.Add(Me.tlpPago)
        Me.pnlProceso.Location = New System.Drawing.Point(24, 144)
        Me.pnlProceso.Name = "pnlProceso"
        Me.pnlProceso.Size = New System.Drawing.Size(802, 366)
        Me.pnlProceso.TabIndex = 6
        '
        'frmRegistrarPago
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 530)
        Me.Controls.Add(Me.pnlProceso)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MinimumSize = New System.Drawing.Size(750, 39)
        Me.Name = "frmRegistrarPago"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Registrar pago"
        Me.tlpPago.ResumeLayout(False)
        Me.tlpPago.PerformLayout()
        Me.flpDatosPago.ResumeLayout(False)
        Me.pnlMetodo.ResumeLayout(False)
        Me.pnlMetodo.PerformLayout()
        Me.pnlImporte.ResumeLayout(False)
        Me.pnlImporte.PerformLayout()
        CType(Me.tblPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flpAccionesPago.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.pnlProceso.ResumeLayout(False)
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
    Friend WithEvents tlpPago As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpDatosPago As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlMetodo As System.Windows.Forms.Panel
    Friend WithEvents pnlImporte As System.Windows.Forms.Panel
    Friend WithEvents flpAccionesPago As System.Windows.Forms.FlowLayoutPanel
End Class

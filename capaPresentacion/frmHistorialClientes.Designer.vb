<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHistorialClientes
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblCliente = New System.Windows.Forms.Label()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.lblPagos = New System.Windows.Forms.Label()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.dgvPedidos = New System.Windows.Forms.DataGridView()
        Me.dgvPagos = New System.Windows.Forms.DataGridView()
        Me.btnCambiarCliente = New System.Windows.Forms.Button()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvPedidos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        Me.ClientSize = New System.Drawing.Size(1050, 650)
        Me.Panel2.Size = New System.Drawing.Size(1050, 650)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblCliente)
        Me.Panel2.Controls.Add(Me.lblEstado)
        Me.Panel2.Controls.Add(Me.lblResumen)
        Me.Panel2.Controls.Add(Me.lblAyuda)
        Me.Panel2.Controls.Add(Me.lblPagos)
        Me.Panel2.Controls.Add(Me.lblCantidad)
        Me.Panel2.Controls.Add(Me.cmbEstado)
        Me.Panel2.Controls.Add(Me.dgvPedidos)
        Me.Panel2.Controls.Add(Me.dgvPagos)
        Me.Panel2.Controls.Add(Me.btnCambiarCliente)
        Me.Panel2.Controls.Add(Me.btnActualizar)
        Me.Panel2.Controls.Add(Me.btnCerrar)

        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(24, 20)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Text = "FICHA COMERCIAL DEL CLIENTE"
        Me.lblCliente.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblCliente.AutoEllipsis = True
        Me.lblCliente.Location = New System.Drawing.Point(24, 60)
        Me.lblCliente.Name = "lblCliente"
        Me.lblCliente.Size = New System.Drawing.Size(1002, 25)
        Me.lblCliente.Text = "Cliente:"
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Location = New System.Drawing.Point(200, 101)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Text = "Estado:"
        Me.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstado.Location = New System.Drawing.Point(260, 98)
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(170, 24)
        Me.cmbEstado.TabIndex = 1
        Me.btnCambiarCliente.Location = New System.Drawing.Point(24, 94)
        Me.btnCambiarCliente.Name = "btnCambiarCliente"
        Me.btnCambiarCliente.Size = New System.Drawing.Size(150, 34)
        Me.btnCambiarCliente.TabIndex = 0
        Me.btnCambiarCliente.Text = "ELEGIR CLIENTE"
        Me.btnCambiarCliente.UseVisualStyleBackColor = True
        Me.btnActualizar.Location = New System.Drawing.Point(454, 94)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(130, 34)
        Me.btnActualizar.TabIndex = 2
        Me.btnActualizar.Text = "ACTUALIZAR"
        Me.btnActualizar.UseVisualStyleBackColor = True
        Me.lblResumen.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblResumen.Location = New System.Drawing.Point(24, 143)
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Size = New System.Drawing.Size(1002, 30)
        Me.lblResumen.Text = "Resumen del filtro:"
        Me.lblAyuda.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblAyuda.Location = New System.Drawing.Point(24, 178)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(1002, 38)
        Me.lblAyuda.Text = "Los cancelados no generan saldo. Los pagos se muestran tal como están registrados; esta consulta no cobra ni procesa devoluciones."
        Me.dgvPedidos.AllowUserToAddRows = False
        Me.dgvPedidos.AllowUserToDeleteRows = False
        Me.dgvPedidos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dgvPedidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPedidos.Location = New System.Drawing.Point(24, 222)
        Me.dgvPedidos.Name = "dgvPedidos"
        Me.dgvPedidos.Size = New System.Drawing.Size(1002, 203)
        Me.dgvPedidos.ReadOnly = True
        Me.dgvPedidos.MultiSelect = False
        Me.dgvPedidos.RowHeadersVisible = False
        Me.dgvPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPedidos.TabIndex = 3
        Me.lblPagos.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblPagos.AutoEllipsis = True
        Me.lblPagos.Location = New System.Drawing.Point(24, 443)
        Me.lblPagos.Name = "lblPagos"
        Me.lblPagos.Size = New System.Drawing.Size(1002, 22)
        Me.lblPagos.Text = "PAGOS DEL PEDIDO"
        Me.dgvPagos.AllowUserToAddRows = False
        Me.dgvPagos.AllowUserToDeleteRows = False
        Me.dgvPagos.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.dgvPagos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPagos.Location = New System.Drawing.Point(24, 470)
        Me.dgvPagos.Name = "dgvPagos"
        Me.dgvPagos.Size = New System.Drawing.Size(1002, 125)
        Me.dgvPagos.ReadOnly = True
        Me.dgvPagos.MultiSelect = False
        Me.dgvPagos.RowHeadersVisible = False
        Me.dgvPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPagos.TabIndex = 4
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.lblCantidad.AutoEllipsis = True
        Me.lblCantidad.Location = New System.Drawing.Point(24, 613)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(850, 24)
        Me.lblCantidad.Text = "Pedidos encontrados: 0"
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
        Me.btnCerrar.Location = New System.Drawing.Point(906, 605)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(120, 34)
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.TabIndex = 5
        Me.btnCerrar.UseVisualStyleBackColor = True

        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MinimumSize = New System.Drawing.Size(1066, 689)
        Me.Controls.Add(Me.Panel2)
        Me.CancelButton = Me.btnCerrar
        Me.Name = "frmHistorialClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ficha comercial del cliente"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvPedidos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvPagos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents lblCliente As System.Windows.Forms.Label
    Friend WithEvents lblEstado As System.Windows.Forms.Label
    Friend WithEvents lblResumen As System.Windows.Forms.Label
    Friend WithEvents lblAyuda As System.Windows.Forms.Label
    Friend WithEvents lblPagos As System.Windows.Forms.Label
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents cmbEstado As System.Windows.Forms.ComboBox
    Friend WithEvents dgvPedidos As System.Windows.Forms.DataGridView
    Friend WithEvents dgvPagos As System.Windows.Forms.DataGridView
    Friend WithEvents btnCambiarCliente As System.Windows.Forms.Button
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
End Class

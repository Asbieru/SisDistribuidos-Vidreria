<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListadoCompras
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.lblBuscarProveedor = New System.Windows.Forms.Label()
        Me.txtBuscarProveedor = New System.Windows.Forms.TextBox()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.pnlFiltro2 = New System.Windows.Forms.Panel()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.btnVerTodas = New System.Windows.Forms.Button()
        Me.dgvCompras = New System.Windows.Forms.DataGridView()
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNuevaCompra = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnVerDetalle = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.pnlCompras = New System.Windows.Forms.Panel()
        Me.tlpContenido.SuspendLayout()
        Me.flpFiltros.SuspendLayout()
        Me.pnlFiltro0.SuspendLayout()
        Me.pnlFiltro1.SuspendLayout()
        Me.pnlFiltro2.SuspendLayout()
        CType(Me.dgvCompras, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flpResumen.SuspendLayout()
        Me.flpAcciones.SuspendLayout()
        Me.pnlCompras.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(24, 20)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(912, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "LISTADO DE COMPRAS"
        '
        'tlpContenido
        '
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvCompras, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.Location = New System.Drawing.Point(0, 0)
        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpContenido.RowCount = 5
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpContenido.Size = New System.Drawing.Size(960, 560)
        Me.tlpContenido.TabIndex = 0
        '
        'flpFiltros
        '
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro2)
        Me.flpFiltros.Controls.Add(Me.btnFiltrar)
        Me.flpFiltros.Controls.Add(Me.btnVerTodas)
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.Location = New System.Drawing.Point(24, 57)
        Me.flpFiltros.Margin = New System.Windows.Forms.Padding(0)
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Size = New System.Drawing.Size(912, 64)
        Me.flpFiltros.TabIndex = 1
        '
        'pnlFiltro0
        '
        Me.pnlFiltro0.Controls.Add(Me.lblBuscarProveedor)
        Me.pnlFiltro0.Controls.Add(Me.txtBuscarProveedor)
        Me.pnlFiltro0.Location = New System.Drawing.Point(0, 0)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(310, 58)
        Me.pnlFiltro0.TabIndex = 0
        '
        'lblBuscarProveedor
        '
        Me.lblBuscarProveedor.Location = New System.Drawing.Point(4, 4)
        Me.lblBuscarProveedor.Name = "lblBuscarProveedor"
        Me.lblBuscarProveedor.Size = New System.Drawing.Size(301, 20)
        Me.lblBuscarProveedor.TabIndex = 0
        Me.lblBuscarProveedor.Text = "Buscar proveedor:"
        '
        'txtBuscarProveedor
        '
        Me.txtBuscarProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscarProveedor.Location = New System.Drawing.Point(7, 24)
        Me.txtBuscarProveedor.Name = "txtBuscarProveedor"
        Me.txtBuscarProveedor.Size = New System.Drawing.Size(296, 22)
        Me.txtBuscarProveedor.TabIndex = 1
        '
        'pnlFiltro1
        '
        Me.pnlFiltro1.Controls.Add(Me.lblDesde)
        Me.pnlFiltro1.Controls.Add(Me.dtpDesde)
        Me.pnlFiltro1.Location = New System.Drawing.Point(322, 0)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro1.TabIndex = 1
        '
        'lblDesde
        '
        Me.lblDesde.Location = New System.Drawing.Point(2, 3)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(135, 20)
        Me.lblDesde.TabIndex = 1
        Me.lblDesde.Text = "Desde:"
        '
        'dtpDesde
        '
        Me.dtpDesde.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(6, 24)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(127, 22)
        Me.dtpDesde.TabIndex = 3
        '
        'pnlFiltro2
        '
        Me.pnlFiltro2.Controls.Add(Me.lblHasta)
        Me.pnlFiltro2.Controls.Add(Me.dtpHasta)
        Me.pnlFiltro2.Location = New System.Drawing.Point(474, 0)
        Me.pnlFiltro2.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro2.Name = "pnlFiltro2"
        Me.pnlFiltro2.Size = New System.Drawing.Size(140, 58)
        Me.pnlFiltro2.TabIndex = 2
        '
        'lblHasta
        '
        Me.lblHasta.Location = New System.Drawing.Point(4, 3)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(133, 20)
        Me.lblHasta.TabIndex = 2
        Me.lblHasta.Text = "Hasta:"
        '
        'dtpHasta
        '
        Me.dtpHasta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(9, 24)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(123, 22)
        Me.dtpHasta.TabIndex = 4
        '
        'btnFiltrar
        '
        Me.btnFiltrar.AutoSize = True
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnFiltrar.Location = New System.Drawing.Point(626, 18)
        Me.btnFiltrar.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(110, 34)
        Me.btnFiltrar.TabIndex = 5
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnVerTodas
        '
        Me.btnVerTodas.AutoSize = True
        Me.btnVerTodas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodas.Location = New System.Drawing.Point(744, 18)
        Me.btnVerTodas.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnVerTodas.Name = "btnVerTodas"
        Me.btnVerTodas.Size = New System.Drawing.Size(120, 34)
        Me.btnVerTodas.TabIndex = 6
        Me.btnVerTodas.Text = "VER TODAS"
        Me.btnVerTodas.UseVisualStyleBackColor = True
        '
        'dgvCompras
        '
        Me.dgvCompras.AllowUserToAddRows = False
        Me.dgvCompras.AllowUserToDeleteRows = False
        Me.dgvCompras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCompras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCompras.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCompras.Location = New System.Drawing.Point(24, 121)
        Me.dgvCompras.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvCompras.MultiSelect = False
        Me.dgvCompras.Name = "dgvCompras"
        Me.dgvCompras.ReadOnly = True
        Me.dgvCompras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCompras.Size = New System.Drawing.Size(912, 329)
        Me.dgvCompras.TabIndex = 0
        '
        'flpResumen
        '
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.Controls.Add(Me.lblResumen)
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.Location = New System.Drawing.Point(24, 470)
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Size = New System.Drawing.Size(912, 24)
        Me.flpResumen.TabIndex = 2
        '
        'lblResumen
        '
        Me.lblResumen.AutoSize = True
        Me.lblResumen.Location = New System.Drawing.Point(0, 0)
        Me.lblResumen.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Size = New System.Drawing.Size(164, 16)
        Me.lblResumen.TabIndex = 0
        Me.lblResumen.Text = "Compras: 0    Total: S/ 0.00"
        '
        'flpAcciones
        '
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.Controls.Add(Me.btnNuevaCompra)
        Me.flpAcciones.Controls.Add(Me.btnModificar)
        Me.flpAcciones.Controls.Add(Me.btnVerDetalle)
        Me.flpAcciones.Controls.Add(Me.btnEliminar)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.Location = New System.Drawing.Point(24, 502)
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Size = New System.Drawing.Size(912, 42)
        Me.flpAcciones.TabIndex = 3
        '
        'btnNuevaCompra
        '
        Me.btnNuevaCompra.AutoSize = True
        Me.btnNuevaCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnNuevaCompra.Location = New System.Drawing.Point(0, 0)
        Me.btnNuevaCompra.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnNuevaCompra.Name = "btnNuevaCompra"
        Me.btnNuevaCompra.Size = New System.Drawing.Size(124, 34)
        Me.btnNuevaCompra.TabIndex = 7
        Me.btnNuevaCompra.Text = "NUEVA COMPRA"
        Me.btnNuevaCompra.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.AutoSize = True
        Me.btnModificar.Enabled = False
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Location = New System.Drawing.Point(132, 0)
        Me.btnModificar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(110, 34)
        Me.btnModificar.TabIndex = 8
        Me.btnModificar.Text = "MODIFICAR"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnVerDetalle
        '
        Me.btnVerDetalle.Enabled = False
        Me.btnVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerDetalle.Location = New System.Drawing.Point(250, 0)
        Me.btnVerDetalle.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerDetalle.Name = "btnVerDetalle"
        Me.btnVerDetalle.Size = New System.Drawing.Size(120, 34)
        Me.btnVerDetalle.TabIndex = 9
        Me.btnVerDetalle.Text = "VER DETALLE"
        Me.btnVerDetalle.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.AutoSize = True
        Me.btnEliminar.Enabled = False
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Location = New System.Drawing.Point(378, 0)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(110, 34)
        Me.btnEliminar.TabIndex = 10
        Me.btnEliminar.Text = "ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.AutoSize = True
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(512, 0)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 9
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'pnlCompras
        '
        Me.pnlCompras.BackColor = System.Drawing.Color.White
        Me.pnlCompras.Controls.Add(Me.tlpContenido)
        Me.pnlCompras.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCompras.Location = New System.Drawing.Point(0, 0)
        Me.pnlCompras.Name = "pnlCompras"
        Me.pnlCompras.Size = New System.Drawing.Size(960, 560)
        Me.pnlCompras.TabIndex = 0
        '
        'frmListadoCompras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.pnlCompras)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.Name = "frmListadoCompras"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Listado de compras"
        Me.tlpContenido.ResumeLayout(False)
        Me.tlpContenido.PerformLayout()
        Me.flpFiltros.ResumeLayout(False)
        Me.flpFiltros.PerformLayout()
        Me.pnlFiltro0.ResumeLayout(False)
        Me.pnlFiltro0.PerformLayout()
        Me.pnlFiltro1.ResumeLayout(False)
        Me.pnlFiltro2.ResumeLayout(False)
        CType(Me.dgvCompras, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flpResumen.ResumeLayout(False)
        Me.flpResumen.PerformLayout()
        Me.flpAcciones.ResumeLayout(False)
        Me.flpAcciones.PerformLayout()
        Me.pnlCompras.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlCompras As Panel
    Friend WithEvents dgvCompras As DataGridView
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents lblHasta As Label
    Friend WithEvents lblDesde As Label
    Friend WithEvents btnVerTodas As Button
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnModificar As Button
    Friend WithEvents btnNuevaCompra As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents txtBuscarProveedor As TextBox
    Friend WithEvents lblBuscarProveedor As Label
    Friend WithEvents lblResumen As Label
    Friend WithEvents btnVerDetalle As Button
    Friend WithEvents lblTitulo As System.Windows.Forms.Label
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro2 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

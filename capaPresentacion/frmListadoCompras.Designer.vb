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
        Me.pnlCompras = New System.Windows.Forms.Panel()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnNuevaCompra = New System.Windows.Forms.Button()
        Me.btnVerTodas = New System.Windows.Forms.Button()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblHasta = New System.Windows.Forms.Label()
        Me.lblDesde = New System.Windows.Forms.Label()
        Me.dgvCompras = New System.Windows.Forms.DataGridView()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.txtBuscarProveedor = New System.Windows.Forms.TextBox()
        Me.lblBuscarProveedor = New System.Windows.Forms.Label()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.btnVerDetalle = New System.Windows.Forms.Button()
        Me.pnlCompras.SuspendLayout()
        CType(Me.dgvCompras, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlCompras
        '
        Me.pnlCompras.BackColor = System.Drawing.Color.White
        Me.pnlCompras.Controls.Add(Me.txtBuscarProveedor)
        Me.pnlCompras.Controls.Add(Me.lblBuscarProveedor)
        Me.pnlCompras.Controls.Add(Me.lblResumen)
        Me.pnlCompras.Controls.Add(Me.btnVerDetalle)
        Me.pnlCompras.Controls.Add(Me.btnEliminar)
        Me.pnlCompras.Controls.Add(Me.btnCerrar)
        Me.pnlCompras.Controls.Add(Me.btnModificar)
        Me.pnlCompras.Controls.Add(Me.btnNuevaCompra)
        Me.pnlCompras.Controls.Add(Me.btnVerTodas)
        Me.pnlCompras.Controls.Add(Me.btnFiltrar)
        Me.pnlCompras.Controls.Add(Me.dtpHasta)
        Me.pnlCompras.Controls.Add(Me.dtpDesde)
        Me.pnlCompras.Controls.Add(Me.lblHasta)
        Me.pnlCompras.Controls.Add(Me.lblDesde)
        Me.pnlCompras.Controls.Add(Me.dgvCompras)
        Me.pnlCompras.Location = New System.Drawing.Point(22, 12)
        Me.pnlCompras.Name = "pnlCompras"
        Me.pnlCompras.Size = New System.Drawing.Size(755, 426)
        Me.pnlCompras.TabIndex = 0
        '
        'btnCerrar
        '
        Me.btnCerrar.AutoSize = True
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(598, 364)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(88, 23)
        Me.btnCerrar.TabIndex = 9
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.AutoSize = True
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Location = New System.Drawing.Point(268, 364)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(88, 23)
        Me.btnModificar.TabIndex = 8
        Me.btnModificar.Text = "MODIFICAR"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnNuevaCompra
        '
        Me.btnNuevaCompra.AutoSize = True
        Me.btnNuevaCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnNuevaCompra.Location = New System.Drawing.Point(62, 364)
        Me.btnNuevaCompra.Name = "btnNuevaCompra"
        Me.btnNuevaCompra.Size = New System.Drawing.Size(115, 23)
        Me.btnNuevaCompra.TabIndex = 7
        Me.btnNuevaCompra.Text = "NUEVA COMPRA"
        Me.btnNuevaCompra.UseVisualStyleBackColor = True
        '
        'btnVerTodas
        '
        Me.btnVerTodas.AutoSize = True
        Me.btnVerTodas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodas.Location = New System.Drawing.Point(477, 60)
        Me.btnVerTodas.Name = "btnVerTodas"
        Me.btnVerTodas.Size = New System.Drawing.Size(88, 23)
        Me.btnVerTodas.TabIndex = 6
        Me.btnVerTodas.Text = "VER TODAS"
        Me.btnVerTodas.UseVisualStyleBackColor = True
        '
        'btnFiltrar
        '
        Me.btnFiltrar.AutoSize = True
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnFiltrar.Location = New System.Drawing.Point(361, 60)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(75, 23)
        Me.btnFiltrar.TabIndex = 5
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(232, 80)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(96, 20)
        Me.dtpHasta.TabIndex = 4
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(232, 41)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(96, 20)
        Me.dtpDesde.TabIndex = 3
        '
        'lblHasta
        '
        Me.lblHasta.AutoSize = True
        Me.lblHasta.Location = New System.Drawing.Point(165, 80)
        Me.lblHasta.Name = "lblHasta"
        Me.lblHasta.Size = New System.Drawing.Size(38, 13)
        Me.lblHasta.TabIndex = 2
        Me.lblHasta.Text = "Hasta:"
        '
        'lblDesde
        '
        Me.lblDesde.AutoSize = True
        Me.lblDesde.Location = New System.Drawing.Point(165, 41)
        Me.lblDesde.Name = "lblDesde"
        Me.lblDesde.Size = New System.Drawing.Size(41, 13)
        Me.lblDesde.TabIndex = 1
        Me.lblDesde.Text = "Desde:"
        '
        'dgvCompras
        '
        Me.dgvCompras.AllowUserToAddRows = False
        Me.dgvCompras.AllowUserToDeleteRows = False
        Me.dgvCompras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvCompras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCompras.Location = New System.Drawing.Point(62, 119)
        Me.dgvCompras.MultiSelect = False
        Me.dgvCompras.Name = "dgvCompras"
        Me.dgvCompras.ReadOnly = True
        Me.dgvCompras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCompras.Size = New System.Drawing.Size(624, 213)
        Me.dgvCompras.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvCompras.TabIndex = 0
        '
        'btnEliminar
        '
        Me.btnEliminar.AutoSize = True
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Location = New System.Drawing.Point(486, 364)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(88, 23)
        Me.btnEliminar.TabIndex = 10
        Me.btnEliminar.Text = "ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'Controles de búsqueda, resumen y consulta
        '
        Me.lblBuscarProveedor.Location = New System.Drawing.Point(62, 12)
        Me.lblBuscarProveedor.AutoSize = True
        Me.lblBuscarProveedor.Name = "lblBuscarProveedor"
        Me.lblBuscarProveedor.Text = "Buscar proveedor:"
        Me.txtBuscarProveedor.Location = New System.Drawing.Point(180, 9)
        Me.txtBuscarProveedor.Size = New System.Drawing.Size(506, 20)
        Me.txtBuscarProveedor.Name = "txtBuscarProveedor"
        Me.txtBuscarProveedor.TabIndex = 1
        Me.lblResumen.Location = New System.Drawing.Point(62, 338)
        Me.lblResumen.AutoSize = True
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Text = "Compras: 0    Total: S/ 0.00"
        Me.btnVerDetalle.Location = New System.Drawing.Point(368, 364)
        Me.btnVerDetalle.Size = New System.Drawing.Size(106, 23)
        Me.btnVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerDetalle.Name = "btnVerDetalle"
        Me.btnVerDetalle.Text = "VER DETALLE"
        Me.btnVerDetalle.TabIndex = 9
        Me.btnVerDetalle.UseVisualStyleBackColor = True
        Me.btnModificar.Enabled = False
        Me.btnEliminar.Enabled = False
        Me.btnVerDetalle.Enabled = False
        '
        'frmListadoCompras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pnlCompras)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Name = "frmListadoCompras"
        Me.Text = "Listado de compras"
        Me.pnlCompras.ResumeLayout(False)
        Me.pnlCompras.PerformLayout()
        CType(Me.dgvCompras, System.ComponentModel.ISupportInitialize).EndInit()
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
End Class

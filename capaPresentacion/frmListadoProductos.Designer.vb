<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListadoProductos
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
        Me.tlpContenido = New System.Windows.Forms.TableLayoutPanel()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboCategoria = New System.Windows.Forms.ComboBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.btnVerTodos = New System.Windows.Forms.Button()
        Me.dgvProductos = New System.Windows.Forms.DataGridView()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.btnDarBaja = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvProductos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(21, 22)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(295, 25)
        Me.lblTitulo.TabIndex = 1
        Me.lblTitulo.Text = "LISTADO DE PRODUCTOS"
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(682, 466)
        Me.Panel2.TabIndex = 2
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnSeleccionar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnSeleccionar.Location = New System.Drawing.Point(464, 428)
        Me.btnSeleccionar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(120, 34)
        Me.btnSeleccionar.TabIndex = 27
        Me.btnSeleccionar.Text = "SELECCIONAR"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        Me.btnSeleccionar.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = False
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 20)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Categoría"
        '
        'cboCategoria
        '
        Me.cboCategoria.FormattingEnabled = True
        Me.cboCategoria.Location = New System.Drawing.Point(0, 24)
        Me.cboCategoria.Margin = New System.Windows.Forms.Padding(2)
        Me.cboCategoria.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cboCategoria.Name = "cboCategoria"
        Me.cboCategoria.Size = New System.Drawing.Size(200, 24)
        Me.cboCategoria.TabIndex = 25
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = False
        Me.lblBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblBuscar.Location = New System.Drawing.Point(0, 0)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(330, 20)
        Me.lblBuscar.TabIndex = 13
        Me.lblBuscar.Text = "Nombre, tipo o color"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtBuscar.Location = New System.Drawing.Point(0, 24)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(2)
        Me.txtBuscar.MaxLength = 150
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(330, 24)
        Me.txtBuscar.TabIndex = 14
        '
        'btnVerTodos
        '
        Me.btnVerTodos.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodos.Location = New System.Drawing.Point(548, 78)
        Me.btnVerTodos.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnVerTodos.Name = "btnVerTodos"
        Me.btnVerTodos.Size = New System.Drawing.Size(120, 34)
        Me.btnVerTodos.TabIndex = 16
        Me.btnVerTodos.Text = "VER TODOS"
        Me.btnVerTodos.UseVisualStyleBackColor = True
        '
        'dgvProductos
        '
        Me.dgvProductos.AllowUserToAddRows = False
        Me.dgvProductos.AllowUserToDeleteRows = False
        Me.dgvProductos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProductos.Location = New System.Drawing.Point(23, 125)
        Me.dgvProductos.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvProductos.MultiSelect = False
        Me.dgvProductos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvProductos.Name = "dgvProductos"
        Me.dgvProductos.ReadOnly = True
        Me.dgvProductos.RowHeadersVisible = False
        Me.dgvProductos.RowHeadersWidth = 51
        Me.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProductos.Size = New System.Drawing.Size(634, 255)
        Me.dgvProductos.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvProductos.TabIndex = 17
        '
        'lblCantidad
        '
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCantidad.Location = New System.Drawing.Point(23, 391)
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(171, 17)
        Me.lblCantidad.TabIndex = 18
        Me.lblCantidad.Text = "Productos encontrados: 0"
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnNuevo.Location = New System.Drawing.Point(22, 428)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(110, 34)
        Me.btnNuevo.TabIndex = 19
        Me.btnNuevo.Text = "NUEVO"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Location = New System.Drawing.Point(131, 428)
        Me.btnModificar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(110, 34)
        Me.btnModificar.TabIndex = 20
        Me.btnModificar.Text = "MODIFICAR"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Location = New System.Drawing.Point(240, 428)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(110, 34)
        Me.btnEliminar.TabIndex = 21
        Me.btnEliminar.Text = "ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(575, 428)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 23
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'btnDarBaja
        '
        Me.btnDarBaja.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnDarBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnDarBaja.Location = New System.Drawing.Point(349, 428)
        Me.btnDarBaja.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnDarBaja.Name = "btnDarBaja"
        Me.btnDarBaja.Size = New System.Drawing.Size(120, 34)
        Me.btnDarBaja.TabIndex = 24
        Me.btnDarBaja.Text = "DAR DE BAJA"
        Me.btnDarBaja.UseVisualStyleBackColor = True
        '
        'frmListadoProductos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.Panel2)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmListadoProductos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Listado de productos"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvProductos, System.ComponentModel.ISupportInitialize).EndInit()

        Me.tlpContenido.Name = "tlpContenido"
        Me.tlpContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpContenido.Padding = New System.Windows.Forms.Padding(24, 20, 24, 16)
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.RowCount = 5
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.tlpContenido.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.WrapContents = True
        Me.flpFiltros.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(330, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblBuscar)
        Me.pnlFiltro0.Controls.Add(Me.txtBuscar)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(200, 58)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Controls.Add(Me.Label1)
        Me.pnlFiltro1.Controls.Add(Me.cboCategoria)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.flpFiltros.Controls.Add(Me.btnVerTodos)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.WrapContents = True
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Controls.Add(Me.lblCantidad)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.WrapContents = True
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Controls.Add(Me.btnNuevo)
        Me.flpAcciones.Controls.Add(Me.btnModificar)
        Me.flpAcciones.Controls.Add(Me.btnEliminar)
        Me.flpAcciones.Controls.Add(Me.btnDarBaja)
        Me.flpAcciones.Controls.Add(Me.btnSeleccionar)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvProductos, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblBuscar As Label
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents btnVerTodos As Button
    Friend WithEvents dgvProductos As DataGridView
    Friend WithEvents lblCantidad As Label
    Friend WithEvents btnNuevo As Button
    Friend WithEvents btnModificar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnDarBaja As Button
    Friend WithEvents cboCategoria As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

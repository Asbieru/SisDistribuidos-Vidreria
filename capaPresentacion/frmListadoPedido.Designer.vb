<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListadoPedido
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.flpFiltros = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlFiltro0 = New System.Windows.Forms.Panel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.pnlFiltro1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboCategoria = New System.Windows.Forms.ComboBox()
        Me.btnVerTodos = New System.Windows.Forms.Button()
        Me.tblPedidos = New System.Windows.Forms.DataGridView()
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.msj = New System.Windows.Forms.Label()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnVerPedido = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.tlpContenido.SuspendLayout()
        Me.flpFiltros.SuspendLayout()
        Me.pnlFiltro0.SuspendLayout()
        Me.pnlFiltro1.SuspendLayout()
        CType(Me.tblPedidos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flpResumen.SuspendLayout()
        Me.flpAcciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpContenido
        '
        Me.tlpContenido.ColumnCount = 1
        Me.tlpContenido.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.tblPedidos, 0, 2)
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
        Me.lblTitulo.Text = "LISTADO DE PEDIDOS"
        '
        'flpFiltros
        '
        Me.flpFiltros.AutoSize = True
        Me.flpFiltros.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro1)
        Me.flpFiltros.Controls.Add(Me.btnVerTodos)
        Me.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFiltros.Location = New System.Drawing.Point(24, 57)
        Me.flpFiltros.Margin = New System.Windows.Forms.Padding(0)
        Me.flpFiltros.Name = "flpFiltros"
        Me.flpFiltros.Size = New System.Drawing.Size(912, 64)
        Me.flpFiltros.TabIndex = 0
        '
        'pnlFiltro0
        '
        Me.pnlFiltro0.Controls.Add(Me.lblBuscar)
        Me.pnlFiltro0.Controls.Add(Me.txtBuscar)
        Me.pnlFiltro0.Location = New System.Drawing.Point(0, 0)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Name = "pnlFiltro0"
        Me.pnlFiltro0.Size = New System.Drawing.Size(417, 58)
        Me.pnlFiltro0.TabIndex = 0
        '
        'lblBuscar
        '
        Me.lblBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblBuscar.Location = New System.Drawing.Point(6, 3)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(314, 20)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Busqueda"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(9, 24)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(2)
        Me.txtBuscar.MaxLength = 150
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(397, 22)
        Me.txtBuscar.TabIndex = 1
        '
        'pnlFiltro1
        '
        Me.pnlFiltro1.Controls.Add(Me.Label1)
        Me.pnlFiltro1.Controls.Add(Me.cboCategoria)
        Me.pnlFiltro1.Location = New System.Drawing.Point(429, 0)
        Me.pnlFiltro1.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro1.Name = "pnlFiltro1"
        Me.pnlFiltro1.Size = New System.Drawing.Size(315, 58)
        Me.pnlFiltro1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(9, 2)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(200, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Categoría"
        '
        'cboCategoria
        '
        Me.cboCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCategoria.FormattingEnabled = True
        Me.cboCategoria.Items.AddRange(New Object() {"Cliente", "Usuario", "Fecha de Registro"})
        Me.cboCategoria.Location = New System.Drawing.Point(12, 24)
        Me.cboCategoria.Margin = New System.Windows.Forms.Padding(2)
        Me.cboCategoria.Name = "cboCategoria"
        Me.cboCategoria.Size = New System.Drawing.Size(290, 24)
        Me.cboCategoria.TabIndex = 2
        '
        'btnVerTodos
        '
        Me.btnVerTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerTodos.Location = New System.Drawing.Point(756, 18)
        Me.btnVerTodos.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnVerTodos.Name = "btnVerTodos"
        Me.btnVerTodos.Size = New System.Drawing.Size(140, 34)
        Me.btnVerTodos.TabIndex = 3
        Me.btnVerTodos.Text = "Mostrar Todos"
        Me.btnVerTodos.UseVisualStyleBackColor = True
        '
        'tblPedidos
        '
        Me.tblPedidos.AllowUserToAddRows = False
        Me.tblPedidos.AllowUserToDeleteRows = False
        Me.tblPedidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.tblPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.tblPedidos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblPedidos.Location = New System.Drawing.Point(24, 121)
        Me.tblPedidos.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.tblPedidos.MultiSelect = False
        Me.tblPedidos.Name = "tblPedidos"
        Me.tblPedidos.ReadOnly = True
        Me.tblPedidos.RowHeadersVisible = False
        Me.tblPedidos.RowHeadersWidth = 51
        Me.tblPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tblPedidos.Size = New System.Drawing.Size(912, 329)
        Me.tblPedidos.TabIndex = 0
        '
        'flpResumen
        '
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.Controls.Add(Me.msj)
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.Location = New System.Drawing.Point(24, 470)
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Size = New System.Drawing.Size(912, 24)
        Me.flpResumen.TabIndex = 0
        '
        'msj
        '
        Me.msj.AutoSize = True
        Me.msj.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.msj.ForeColor = System.Drawing.Color.Red
        Me.msj.Location = New System.Drawing.Point(0, 0)
        Me.msj.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.msj.Name = "msj"
        Me.msj.Size = New System.Drawing.Size(0, 16)
        Me.msj.TabIndex = 35
        '
        'flpAcciones
        '
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.Controls.Add(Me.btnNuevo)
        Me.flpAcciones.Controls.Add(Me.btnVerPedido)
        Me.flpAcciones.Controls.Add(Me.btnCancelar)
        Me.flpAcciones.Controls.Add(Me.btnSeleccionar)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.Location = New System.Drawing.Point(24, 502)
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Size = New System.Drawing.Size(912, 42)
        Me.flpAcciones.TabIndex = 0
        '
        'btnNuevo
        '
        Me.btnNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnNuevo.Location = New System.Drawing.Point(0, 0)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(110, 34)
        Me.btnNuevo.TabIndex = 4
        Me.btnNuevo.Text = "Nuevo +"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnVerPedido
        '
        Me.btnVerPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerPedido.Location = New System.Drawing.Point(118, 0)
        Me.btnVerPedido.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerPedido.Name = "btnVerPedido"
        Me.btnVerPedido.Size = New System.Drawing.Size(110, 34)
        Me.btnVerPedido.TabIndex = 5
        Me.btnVerPedido.Text = "Ver Pedido"
        Me.btnVerPedido.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancelar.Location = New System.Drawing.Point(236, 0)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(110, 34)
        Me.btnCancelar.TabIndex = 6
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnSeleccionar.Location = New System.Drawing.Point(354, 0)
        Me.btnSeleccionar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(120, 34)
        Me.btnSeleccionar.TabIndex = 7
        Me.btnSeleccionar.Text = "Seleccionar"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        Me.btnSeleccionar.Visible = False
        '
        'btnCerrar
        '
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(498, 0)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 8
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmListadoPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.tlpContenido)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.Name = "frmListadoPedido"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Listado de pedidos"
        Me.tlpContenido.ResumeLayout(False)
        Me.tlpContenido.PerformLayout()
        Me.flpFiltros.ResumeLayout(False)
        Me.pnlFiltro0.ResumeLayout(False)
        Me.pnlFiltro0.PerformLayout()
        Me.pnlFiltro1.ResumeLayout(False)
        CType(Me.tblPedidos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flpResumen.ResumeLayout(False)
        Me.flpResumen.PerformLayout()
        Me.flpAcciones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tblPedidos As DataGridView
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents lblBuscar As Label
    Friend WithEvents lblTitulo As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cboCategoria As ComboBox
    Friend WithEvents btnVerTodos As Button
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents btnNuevo As Button
    Friend WithEvents btnVerPedido As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents msj As Label
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents pnlFiltro1 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

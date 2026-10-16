<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerInventario
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
        Me.flpResumen = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblCategoria = New System.Windows.Forms.Label()
        Me.cmbCategoria = New System.Windows.Forms.ComboBox()
        Me.chkBajoMinimo = New System.Windows.Forms.CheckBox()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.lblAlerta = New System.Windows.Forms.Label()
        Me.dgvResumen = New System.Windows.Forms.DataGridView()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.btnEditarMinimo = New System.Windows.Forms.Button()
        Me.btnVerPiezas = New System.Windows.Forms.Button()
        Me.btnVerCajas = New System.Windows.Forms.Button()
        Me.btnVerKardex = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvResumen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(900, 535)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(21, 22)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(461, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "INVENTARIO - RESUMEN DE EXISTENCIAS"
        '
        'lblCategoria
        '
        Me.lblCategoria.AutoSize = False
        Me.lblCategoria.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCategoria.Location = New System.Drawing.Point(0, 0)
        Me.lblCategoria.Name = "lblCategoria"
        Me.lblCategoria.Size = New System.Drawing.Size(190, 20)
        Me.lblCategoria.TabIndex = 1
        Me.lblCategoria.Text = "Categoría"
        '
        'cmbCategoria
        '
        Me.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoria.FormattingEnabled = True
        Me.cmbCategoria.Location = New System.Drawing.Point(0, 24)
        Me.cmbCategoria.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.cmbCategoria.Name = "cmbCategoria"
        Me.cmbCategoria.Size = New System.Drawing.Size(190, 24)
        Me.cmbCategoria.TabIndex = 2
        '
        'chkBajoMinimo
        '
        Me.chkBajoMinimo.AutoSize = True
        Me.chkBajoMinimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.chkBajoMinimo.Location = New System.Drawing.Point(222, 89)
        Me.chkBajoMinimo.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.chkBajoMinimo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.chkBajoMinimo.Name = "chkBajoMinimo"
        Me.chkBajoMinimo.Size = New System.Drawing.Size(220, 34)
        Me.chkBajoMinimo.TabIndex = 3
        Me.chkBajoMinimo.Text = "Solo bajo el stock mínimo"
        Me.chkBajoMinimo.UseVisualStyleBackColor = True
        '
        'btnActualizar
        '
        Me.btnActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnActualizar.Location = New System.Drawing.Point(432, 85)
        Me.btnActualizar.Margin = New System.Windows.Forms.Padding(0, 18, 8, 12)
        Me.btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(120, 34)
        Me.btnActualizar.TabIndex = 4
        Me.btnActualizar.Text = "ACTUALIZAR"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'lblAlerta
        '
        Me.lblAlerta.AutoSize = True
        Me.lblAlerta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblAlerta.ForeColor = System.Drawing.Color.DarkRed
        Me.lblAlerta.Location = New System.Drawing.Point(545, 92)
        Me.lblAlerta.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblAlerta.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblAlerta.Name = "lblAlerta"
        Me.lblAlerta.Size = New System.Drawing.Size(0, 13)
        Me.lblAlerta.TabIndex = 5
        '
        'dgvResumen
        '
        Me.dgvResumen.AllowUserToAddRows = False
        Me.dgvResumen.AllowUserToDeleteRows = False
        Me.dgvResumen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvResumen.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvResumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResumen.Location = New System.Drawing.Point(23, 125)
        Me.dgvResumen.MultiSelect = False
        Me.dgvResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResumen.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvResumen.Name = "dgvResumen"
        Me.dgvResumen.ReadOnly = True
        Me.dgvResumen.RowHeadersVisible = False
        Me.dgvResumen.RowHeadersWidth = 51
        Me.dgvResumen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResumen.Size = New System.Drawing.Size(854, 330)
        Me.dgvResumen.ScrollBars = System.Windows.Forms.ScrollBars.Both

        Me.dgvResumen.TabIndex = 6
        '
        'lblCantidad
        '
        Me.lblCantidad.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblCantidad.Location = New System.Drawing.Point(23, 464)
        Me.lblCantidad.Margin = New System.Windows.Forms.Padding(0, 0, 20, 8)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(178, 17)
        Me.lblCantidad.TabIndex = 7
        Me.lblCantidad.Text = "Productos encontrados: 0"
        '
        'btnEditarMinimo
        '
        Me.btnEditarMinimo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnEditarMinimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEditarMinimo.Location = New System.Drawing.Point(22, 495)
        Me.btnEditarMinimo.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEditarMinimo.Name = "btnEditarMinimo"
        Me.btnEditarMinimo.Size = New System.Drawing.Size(120, 34)
        Me.btnEditarMinimo.TabIndex = 8
        Me.btnEditarMinimo.Text = "STOCK MÍNIMO"
        Me.btnEditarMinimo.UseVisualStyleBackColor = True
        '
        'btnVerPiezas
        '
        Me.btnVerPiezas.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerPiezas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerPiezas.Location = New System.Drawing.Point(164, 495)
        Me.btnVerPiezas.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerPiezas.Name = "btnVerPiezas"
        Me.btnVerPiezas.Size = New System.Drawing.Size(110, 34)
        Me.btnVerPiezas.TabIndex = 9
        Me.btnVerPiezas.Text = "VER PIEZAS"
        Me.btnVerPiezas.UseVisualStyleBackColor = True
        '
        'btnVerCajas
        '
        Me.btnVerCajas.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerCajas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerCajas.Location = New System.Drawing.Point(306, 495)
        Me.btnVerCajas.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerCajas.Name = "btnVerCajas"
        Me.btnVerCajas.Size = New System.Drawing.Size(160, 34)
        Me.btnVerCajas.TabIndex = 10
        Me.btnVerCajas.Text = "VER CAJAS Y ACCESORIOS"
        Me.btnVerCajas.UseVisualStyleBackColor = True
        '
        'btnVerKardex
        '
        Me.btnVerKardex.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnVerKardex.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnVerKardex.Location = New System.Drawing.Point(518, 495)
        Me.btnVerKardex.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnVerKardex.Name = "btnVerKardex"
        Me.btnVerKardex.Size = New System.Drawing.Size(110, 34)
        Me.btnVerKardex.TabIndex = 11
        Me.btnVerKardex.Text = "VER KARDEX"
        Me.btnVerKardex.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(795, 495)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 12
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmVerInventario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmVerInventario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventario - Resumen de existencias"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvResumen, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.pnlFiltro0.Size = New System.Drawing.Size(190, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.lblCategoria)
        Me.pnlFiltro0.Controls.Add(Me.cmbCategoria)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.flpFiltros.Controls.Add(Me.chkBajoMinimo)
        Me.flpFiltros.Controls.Add(Me.btnActualizar)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.WrapContents = True
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.flpResumen.Controls.Add(Me.lblCantidad)
        Me.flpResumen.Controls.Add(Me.lblAlerta)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.WrapContents = True
        Me.flpAcciones.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAcciones.Controls.Add(Me.btnEditarMinimo)
        Me.flpAcciones.Controls.Add(Me.btnVerPiezas)
        Me.flpAcciones.Controls.Add(Me.btnVerCajas)
        Me.flpAcciones.Controls.Add(Me.btnVerKardex)
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvResumen, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblCategoria As Label
    Friend WithEvents cmbCategoria As ComboBox
    Friend WithEvents chkBajoMinimo As CheckBox
    Friend WithEvents btnActualizar As Button
    Friend WithEvents lblAlerta As Label
    Friend WithEvents dgvResumen As DataGridView
    Friend WithEvents lblCantidad As Label
    Friend WithEvents btnEditarMinimo As Button
    Friend WithEvents btnVerPiezas As Button
    Friend WithEvents btnVerCajas As Button
    Friend WithEvents btnVerKardex As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

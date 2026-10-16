<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListadoUsuarios
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
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.dgvUsuarios = New System.Windows.Forms.DataGridView()
        Me.btnDarBaja = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnModificar = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Location = New System.Drawing.Point(27, 27)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(784, 563)
        Me.Panel2.TabIndex = 2
        '
        'btnNuevo
        '
        Me.btnNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnNuevo.Location = New System.Drawing.Point(30, 497)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnNuevo.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(110, 34)
        Me.btnNuevo.TabIndex = 24
        Me.btnNuevo.Text = "NUEVO"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(617, 497)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 8)
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 23
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'dgvUsuarios
        '
        Me.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUsuarios.Location = New System.Drawing.Point(33, 128)
        Me.dgvUsuarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUsuarios.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.dgvUsuarios.Name = "dgvUsuarios"
        Me.dgvUsuarios.RowHeadersWidth = 51
        Me.dgvUsuarios.RowTemplate.Height = 24
        Me.dgvUsuarios.Size = New System.Drawing.Size(724, 334)
        Me.dgvUsuarios.TabIndex = 22
        '
        'btnDarBaja
        '
        Me.btnDarBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnDarBaja.Location = New System.Drawing.Point(458, 497)
        Me.btnDarBaja.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnDarBaja.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnDarBaja.Name = "btnDarBaja"
        Me.btnDarBaja.Size = New System.Drawing.Size(120, 34)
        Me.btnDarBaja.TabIndex = 21
        Me.btnDarBaja.Text = "DAR DE BAJA"
        Me.btnDarBaja.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnEliminar.Location = New System.Drawing.Point(315, 497)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnEliminar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(110, 34)
        Me.btnEliminar.TabIndex = 20
        Me.btnEliminar.Text = "ELIMINAR"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnModificar
        '
        Me.btnModificar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnModificar.Location = New System.Drawing.Point(173, 497)
        Me.btnModificar.Margin = New System.Windows.Forms.Padding(0, 0, 8, 8)
        Me.btnModificar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        Me.btnModificar.Name = "btnModificar"
        Me.btnModificar.Size = New System.Drawing.Size(110, 34)
        Me.btnModificar.TabIndex = 19
        Me.btnModificar.Text = "MODIFICAR"
        Me.btnModificar.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = False
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label9.Location = New System.Drawing.Point(0, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(450, 20)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(0, 24)
        Me.txtBuscar.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(450, 24)
        Me.txtBuscar.TabIndex = 6
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(27, 18)
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 0, 12)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(345, 31)
        Me.lblTitulo.TabIndex = 25
        Me.lblTitulo.Text = "LISTADO DE USUARIOS"
        '
        'frmListadoUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 560)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(800, 480)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.Name = "frmListadoUsuarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Listado de usuarios"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvUsuarios, System.ComponentModel.ISupportInitialize).EndInit()

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
        Me.pnlFiltro0.Size = New System.Drawing.Size(450, 58)
        Me.pnlFiltro0.Margin = New System.Windows.Forms.Padding(0, 0, 12, 6)
        Me.pnlFiltro0.Controls.Add(Me.Label9)
        Me.pnlFiltro0.Controls.Add(Me.txtBuscar)
        Me.flpFiltros.Controls.Add(Me.pnlFiltro0)
        Me.flpResumen.Name = "flpResumen"
        Me.flpResumen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpResumen.AutoSize = True
        Me.flpResumen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpResumen.WrapContents = True
        Me.flpResumen.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
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
        Me.flpAcciones.Controls.Add(Me.btnCerrar)
        Me.tlpContenido.Controls.Add(Me.lblTitulo, 0, 0)
        Me.tlpContenido.Controls.Add(Me.flpFiltros, 0, 1)
        Me.tlpContenido.Controls.Add(Me.dgvUsuarios, 0, 2)
        Me.tlpContenido.Controls.Add(Me.flpResumen, 0, 3)
        Me.tlpContenido.Controls.Add(Me.flpAcciones, 0, 4)
        Me.Panel2.Controls.Add(Me.tlpContenido)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents btnDarBaja As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnModificar As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents dgvUsuarios As DataGridView
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnNuevo As Button
    Friend WithEvents lblTitulo As Label
    Friend WithEvents tlpContenido As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents flpFiltros As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlFiltro0 As System.Windows.Forms.Panel
    Friend WithEvents flpResumen As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpAcciones As System.Windows.Forms.FlowLayoutPanel
End Class

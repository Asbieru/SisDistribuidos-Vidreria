<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVerPiezas
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblVariante = New System.Windows.Forms.Label()
        Me.cmbVariante = New System.Windows.Forms.ComboBox()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.dgvPiezas = New System.Windows.Forms.DataGridView()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.btnNuevaPieza = New System.Windows.Forms.Button()
        Me.btnEditarPieza = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblVariante)
        Me.Panel2.Controls.Add(Me.cmbVariante)
        Me.Panel2.Controls.Add(Me.lblEstado)
        Me.Panel2.Controls.Add(Me.cmbEstado)
        Me.Panel2.Controls.Add(Me.btnBuscar)
        Me.Panel2.Controls.Add(Me.dgvPiezas)
        Me.Panel2.Controls.Add(Me.lblCantidad)
        Me.Panel2.Controls.Add(Me.btnNuevaPieza)
        Me.Panel2.Controls.Add(Me.btnEditarPieza)
        Me.Panel2.Controls.Add(Me.btnCerrar)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(900, 535)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.Location = New System.Drawing.Point(21, 22)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(506, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "INVENTARIO - PIEZAS DE VIDRIO Y ALUMINIO"
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = True
        Me.lblVariante.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVariante.Location = New System.Drawing.Point(23, 68)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(65, 17)
        Me.lblVariante.TabIndex = 1
        Me.lblVariante.Text = "Producto"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(23, 88)
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(400, 24)
        Me.cmbVariante.TabIndex = 2
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEstado.Location = New System.Drawing.Point(440, 68)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(52, 17)
        Me.lblEstado.TabIndex = 3
        Me.lblEstado.Text = "Estado"
        '
        'cmbEstado
        '
        Me.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstado.FormattingEnabled = True
        Me.cmbEstado.Location = New System.Drawing.Point(440, 88)
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(160, 24)
        Me.cmbEstado.TabIndex = 4
        '
        'btnBuscar
        '
        Me.btnBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscar.Location = New System.Drawing.Point(620, 85)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(97, 28)
        Me.btnBuscar.TabIndex = 5
        Me.btnBuscar.Text = "BUSCAR"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'dgvPiezas
        '
        Me.dgvPiezas.AllowUserToAddRows = False
        Me.dgvPiezas.AllowUserToDeleteRows = False
        Me.dgvPiezas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPiezas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPiezas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPiezas.Location = New System.Drawing.Point(23, 125)
        Me.dgvPiezas.MultiSelect = False
        Me.dgvPiezas.Name = "dgvPiezas"
        Me.dgvPiezas.ReadOnly = True
        Me.dgvPiezas.RowHeadersVisible = False
        Me.dgvPiezas.RowHeadersWidth = 51
        Me.dgvPiezas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPiezas.Size = New System.Drawing.Size(854, 330)
        Me.dgvPiezas.TabIndex = 6
        '
        'lblCantidad
        '
        Me.lblCantidad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad.Location = New System.Drawing.Point(23, 464)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(158, 17)
        Me.lblCantidad.TabIndex = 7
        Me.lblCantidad.Text = "Piezas encontradas: 0"
        '
        'btnNuevaPieza
        '
        Me.btnNuevaPieza.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaPieza.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNuevaPieza.Location = New System.Drawing.Point(22, 495)
        Me.btnNuevaPieza.Name = "btnNuevaPieza"
        Me.btnNuevaPieza.Size = New System.Drawing.Size(120, 28)
        Me.btnNuevaPieza.TabIndex = 8
        Me.btnNuevaPieza.Text = "NUEVA PIEZA"
        Me.btnNuevaPieza.UseVisualStyleBackColor = True
        '
        'btnEditarPieza
        '
        Me.btnEditarPieza.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditarPieza.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEditarPieza.Location = New System.Drawing.Point(154, 495)
        Me.btnEditarPieza.Name = "btnEditarPieza"
        Me.btnEditarPieza.Size = New System.Drawing.Size(120, 28)
        Me.btnEditarPieza.TabIndex = 9
        Me.btnEditarPieza.Text = "EDITAR PIEZA"
        Me.btnEditarPieza.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCerrar.Location = New System.Drawing.Point(795, 495)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(82, 28)
        Me.btnCerrar.TabIndex = 10
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmVerPiezas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(900, 535)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MinimumSize = New System.Drawing.Size(916, 574)
        Me.Name = "frmVerPiezas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Inventario - Piezas de vidrio y aluminio"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblVariante As Label
    Friend WithEvents cmbVariante As ComboBox
    Friend WithEvents lblEstado As Label
    Friend WithEvents cmbEstado As ComboBox
    Friend WithEvents btnBuscar As Button
    Friend WithEvents dgvPiezas As DataGridView
    Friend WithEvents lblCantidad As Label
    Friend WithEvents btnNuevaPieza As Button
    Friend WithEvents btnEditarPieza As Button
    Friend WithEvents btnCerrar As Button
End Class

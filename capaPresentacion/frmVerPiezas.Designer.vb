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
        Me.lblVariante = New System.Windows.Forms.Label()
        Me.cmbVariante = New System.Windows.Forms.ComboBox()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.dgvPiezas = New System.Windows.Forms.DataGridView()
        Me.btnNuevaPieza = New System.Windows.Forms.Button()
        Me.btnEditarPieza = New System.Windows.Forms.Button()
        Me.lblAyuda = New System.Windows.Forms.Label()
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblVariante
        '
        Me.lblVariante.AutoSize = True
        Me.lblVariante.Location = New System.Drawing.Point(12, 15)
        Me.lblVariante.Name = "lblVariante"
        Me.lblVariante.Size = New System.Drawing.Size(53, 13)
        Me.lblVariante.TabIndex = 0
        Me.lblVariante.Text = "Producto:"
        '
        'cmbVariante
        '
        Me.cmbVariante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVariante.FormattingEnabled = True
        Me.cmbVariante.Location = New System.Drawing.Point(80, 12)
        Me.cmbVariante.Name = "cmbVariante"
        Me.cmbVariante.Size = New System.Drawing.Size(380, 21)
        Me.cmbVariante.TabIndex = 1
        '
        'lblEstado
        '
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Location = New System.Drawing.Point(475, 15)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(43, 13)
        Me.lblEstado.TabIndex = 2
        Me.lblEstado.Text = "Estado:"
        '
        'cmbEstado
        '
        Me.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstado.FormattingEnabled = True
        Me.cmbEstado.Location = New System.Drawing.Point(525, 12)
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(130, 21)
        Me.cmbEstado.TabIndex = 3
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(670, 10)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(100, 25)
        Me.btnBuscar.TabIndex = 4
        Me.btnBuscar.Text = "Buscar"
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
        Me.dgvPiezas.Location = New System.Drawing.Point(12, 45)
        Me.dgvPiezas.MultiSelect = False
        Me.dgvPiezas.Name = "dgvPiezas"
        Me.dgvPiezas.ReadOnly = True
        Me.dgvPiezas.RowHeadersVisible = False
        Me.dgvPiezas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPiezas.Size = New System.Drawing.Size(956, 440)
        Me.dgvPiezas.TabIndex = 5
        '
        'btnNuevaPieza
        '
        Me.btnNuevaPieza.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaPieza.Location = New System.Drawing.Point(12, 495)
        Me.btnNuevaPieza.Name = "btnNuevaPieza"
        Me.btnNuevaPieza.Size = New System.Drawing.Size(160, 32)
        Me.btnNuevaPieza.TabIndex = 6
        Me.btnNuevaPieza.Text = "Nueva pieza"
        Me.btnNuevaPieza.UseVisualStyleBackColor = True
        '
        'btnEditarPieza
        '
        Me.btnEditarPieza.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditarPieza.Location = New System.Drawing.Point(182, 495)
        Me.btnEditarPieza.Name = "btnEditarPieza"
        Me.btnEditarPieza.Size = New System.Drawing.Size(200, 32)
        Me.btnEditarPieza.TabIndex = 7
        Me.btnEditarPieza.Text = "Editar pieza seleccionada"
        Me.btnEditarPieza.UseVisualStyleBackColor = True
        '
        'lblAyuda
        '
        Me.lblAyuda.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.Location = New System.Drawing.Point(395, 505)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(330, 13)
        Me.lblAyuda.TabIndex = 8
        Me.lblAyuda.Text = "Doble clic en una pieza para editarla (cortar o cambiar su estado)."
        '
        'frmVerPiezas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(980, 540)
        Me.Controls.Add(Me.lblAyuda)
        Me.Controls.Add(Me.btnEditarPieza)
        Me.Controls.Add(Me.btnNuevaPieza)
        Me.Controls.Add(Me.dgvPiezas)
        Me.Controls.Add(Me.btnBuscar)
        Me.Controls.Add(Me.cmbEstado)
        Me.Controls.Add(Me.lblEstado)
        Me.Controls.Add(Me.cmbVariante)
        Me.Controls.Add(Me.lblVariante)
        Me.MinimumSize = New System.Drawing.Size(800, 400)
        Me.Name = "frmVerPiezas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventario - Piezas de vidrio y aluminio"
        CType(Me.dgvPiezas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblVariante As Label
    Friend WithEvents cmbVariante As ComboBox
    Friend WithEvents lblEstado As Label
    Friend WithEvents cmbEstado As ComboBox
    Friend WithEvents btnBuscar As Button
    Friend WithEvents dgvPiezas As DataGridView
    Friend WithEvents btnNuevaPieza As Button
    Friend WithEvents btnEditarPieza As Button
    Friend WithEvents lblAyuda As Label
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBitacora
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
        Me.dgvBitacora = New System.Windows.Forms.DataGridView()
        Me.pnlBotones = New System.Windows.Forms.Panel()
        Me.lblAccion = New System.Windows.Forms.Label()
        Me.cmbAccion = New System.Windows.Forms.ComboBox()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.btnRefrescar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        CType(Me.dgvBitacora, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvBitacora
        '
        Me.dgvBitacora.AllowUserToAddRows = False
        Me.dgvBitacora.AllowUserToDeleteRows = False
        Me.dgvBitacora.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBitacora.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvBitacora.Location = New System.Drawing.Point(0, 0)
        Me.dgvBitacora.MultiSelect = False
        Me.dgvBitacora.Name = "dgvBitacora"
        Me.dgvBitacora.ReadOnly = True
        Me.dgvBitacora.RowHeadersVisible = False
        Me.dgvBitacora.RowHeadersWidth = 51
        Me.dgvBitacora.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBitacora.Size = New System.Drawing.Size(800, 400)
        Me.dgvBitacora.TabIndex = 0
        '
        'pnlBotones
        '
        Me.pnlBotones.Controls.Add(Me.lblAccion)
        Me.pnlBotones.Controls.Add(Me.cmbAccion)
        Me.pnlBotones.Controls.Add(Me.btnFiltrar)
        Me.pnlBotones.Controls.Add(Me.btnRefrescar)
        Me.pnlBotones.Controls.Add(Me.btnCerrar)
        Me.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBotones.Location = New System.Drawing.Point(0, 400)
        Me.pnlBotones.Name = "pnlBotones"
        Me.pnlBotones.Size = New System.Drawing.Size(800, 54)
        Me.pnlBotones.TabIndex = 1
        '
        'lblAccion
        '
        Me.lblAccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAccion.AutoSize = True
        Me.lblAccion.Location = New System.Drawing.Point(12, 19)
        Me.lblAccion.Name = "lblAccion"
        Me.lblAccion.Size = New System.Drawing.Size(51, 16)
        Me.lblAccion.TabIndex = 2
        Me.lblAccion.Text = "Acción:"
        '
        'cmbAccion
        '
        Me.cmbAccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbAccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAccion.FormattingEnabled = True
        Me.cmbAccion.Location = New System.Drawing.Point(72, 15)
        Me.cmbAccion.Name = "cmbAccion"
        Me.cmbAccion.Size = New System.Drawing.Size(180, 24)
        Me.cmbAccion.TabIndex = 3
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnFiltrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnFiltrar.Location = New System.Drawing.Point(262, 10)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(110, 34)
        Me.btnFiltrar.TabIndex = 4
        Me.btnFiltrar.Text = "FILTRAR"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnRefrescar
        '
        Me.btnRefrescar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefrescar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnRefrescar.Location = New System.Drawing.Point(558, 10)
        Me.btnRefrescar.Name = "btnRefrescar"
        Me.btnRefrescar.Size = New System.Drawing.Size(110, 34)
        Me.btnRefrescar.TabIndex = 0
        Me.btnRefrescar.Text = "REFRESCAR"
        Me.btnRefrescar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(678, 10)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(110, 34)
        Me.btnCerrar.TabIndex = 1
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmBitacora
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(800, 454)
        Me.Controls.Add(Me.dgvBitacora)
        Me.Controls.Add(Me.pnlBotones)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.MinimumSize = New System.Drawing.Size(600, 360)
        Me.Name = "frmBitacora"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Bitácora"
        CType(Me.dgvBitacora, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBotones.ResumeLayout(False)
        Me.pnlBotones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvBitacora As DataGridView
    Friend WithEvents pnlBotones As Panel
    Friend WithEvents lblAccion As Label
    Friend WithEvents cmbAccion As ComboBox
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents btnRefrescar As Button
    Friend WithEvents btnCerrar As Button
End Class

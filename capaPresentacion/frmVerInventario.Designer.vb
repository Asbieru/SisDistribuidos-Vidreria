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
        Me.lblCategoria = New System.Windows.Forms.Label()
        Me.cmbCategoria = New System.Windows.Forms.ComboBox()
        Me.chkBajoMinimo = New System.Windows.Forms.CheckBox()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.lblAlerta = New System.Windows.Forms.Label()
        Me.dgvResumen = New System.Windows.Forms.DataGridView()
        Me.btnEditarMinimo = New System.Windows.Forms.Button()
        Me.btnVerPiezas = New System.Windows.Forms.Button()
        Me.btnVerCajas = New System.Windows.Forms.Button()
        Me.btnVerKardex = New System.Windows.Forms.Button()
        CType(Me.dgvResumen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCategoria
        '
        Me.lblCategoria.AutoSize = True
        Me.lblCategoria.Location = New System.Drawing.Point(12, 15)
        Me.lblCategoria.Name = "lblCategoria"
        Me.lblCategoria.Size = New System.Drawing.Size(57, 13)
        Me.lblCategoria.TabIndex = 0
        Me.lblCategoria.Text = "Categoría:"
        '
        'cmbCategoria
        '
        Me.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoria.FormattingEnabled = True
        Me.cmbCategoria.Location = New System.Drawing.Point(80, 12)
        Me.cmbCategoria.Name = "cmbCategoria"
        Me.cmbCategoria.Size = New System.Drawing.Size(150, 21)
        Me.cmbCategoria.TabIndex = 1
        '
        'chkBajoMinimo
        '
        Me.chkBajoMinimo.AutoSize = True
        Me.chkBajoMinimo.Location = New System.Drawing.Point(245, 14)
        Me.chkBajoMinimo.Name = "chkBajoMinimo"
        Me.chkBajoMinimo.Size = New System.Drawing.Size(155, 17)
        Me.chkBajoMinimo.TabIndex = 2
        Me.chkBajoMinimo.Text = "Solo bajo el stock mínimo"
        Me.chkBajoMinimo.UseVisualStyleBackColor = True
        '
        'btnActualizar
        '
        Me.btnActualizar.Location = New System.Drawing.Point(415, 10)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(100, 25)
        Me.btnActualizar.TabIndex = 3
        Me.btnActualizar.Text = "Actualizar"
        Me.btnActualizar.UseVisualStyleBackColor = True
        '
        'lblAlerta
        '
        Me.lblAlerta.AutoSize = True
        Me.lblAlerta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblAlerta.ForeColor = System.Drawing.Color.DarkRed
        Me.lblAlerta.Location = New System.Drawing.Point(530, 16)
        Me.lblAlerta.Name = "lblAlerta"
        Me.lblAlerta.Size = New System.Drawing.Size(0, 13)
        Me.lblAlerta.TabIndex = 4
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
        Me.dgvResumen.Location = New System.Drawing.Point(12, 45)
        Me.dgvResumen.MultiSelect = False
        Me.dgvResumen.Name = "dgvResumen"
        Me.dgvResumen.ReadOnly = True
        Me.dgvResumen.RowHeadersVisible = False
        Me.dgvResumen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResumen.Size = New System.Drawing.Size(936, 420)
        Me.dgvResumen.TabIndex = 5
        '
        'btnEditarMinimo
        '
        Me.btnEditarMinimo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEditarMinimo.Location = New System.Drawing.Point(12, 475)
        Me.btnEditarMinimo.Name = "btnEditarMinimo"
        Me.btnEditarMinimo.Size = New System.Drawing.Size(170, 32)
        Me.btnEditarMinimo.TabIndex = 6
        Me.btnEditarMinimo.Text = "Editar stock mínimo"
        Me.btnEditarMinimo.UseVisualStyleBackColor = True
        '
        'btnVerPiezas
        '
        Me.btnVerPiezas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerPiezas.Location = New System.Drawing.Point(418, 475)
        Me.btnVerPiezas.Name = "btnVerPiezas"
        Me.btnVerPiezas.Size = New System.Drawing.Size(180, 32)
        Me.btnVerPiezas.TabIndex = 7
        Me.btnVerPiezas.Text = "Ver piezas (vidrio y aluminio)"
        Me.btnVerPiezas.UseVisualStyleBackColor = True
        '
        'btnVerCajas
        '
        Me.btnVerCajas.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerCajas.Location = New System.Drawing.Point(608, 475)
        Me.btnVerCajas.Name = "btnVerCajas"
        Me.btnVerCajas.Size = New System.Drawing.Size(180, 32)
        Me.btnVerCajas.TabIndex = 8
        Me.btnVerCajas.Text = "Ver cajas (tornillos y tarugos)"
        Me.btnVerCajas.UseVisualStyleBackColor = True
        '
        'btnVerKardex
        '
        Me.btnVerKardex.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerKardex.Location = New System.Drawing.Point(798, 475)
        Me.btnVerKardex.Name = "btnVerKardex"
        Me.btnVerKardex.Size = New System.Drawing.Size(150, 32)
        Me.btnVerKardex.TabIndex = 9
        Me.btnVerKardex.Text = "Ver kardex"
        Me.btnVerKardex.UseVisualStyleBackColor = True
        '
        'frmVerInventario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(960, 520)
        Me.Controls.Add(Me.btnVerKardex)
        Me.Controls.Add(Me.btnVerCajas)
        Me.Controls.Add(Me.btnVerPiezas)
        Me.Controls.Add(Me.btnEditarMinimo)
        Me.Controls.Add(Me.dgvResumen)
        Me.Controls.Add(Me.lblAlerta)
        Me.Controls.Add(Me.btnActualizar)
        Me.Controls.Add(Me.chkBajoMinimo)
        Me.Controls.Add(Me.cmbCategoria)
        Me.Controls.Add(Me.lblCategoria)
        Me.MinimumSize = New System.Drawing.Size(976, 400)
        Me.Name = "frmVerInventario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Inventario - Resumen de existencias"
        CType(Me.dgvResumen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblCategoria As Label
    Friend WithEvents cmbCategoria As ComboBox
    Friend WithEvents chkBajoMinimo As CheckBox
    Friend WithEvents btnActualizar As Button
    Friend WithEvents lblAlerta As Label
    Friend WithEvents dgvResumen As DataGridView
    Friend WithEvents btnEditarMinimo As Button
    Friend WithEvents btnVerPiezas As Button
    Friend WithEvents btnVerCajas As Button
    Friend WithEvents btnVerKardex As Button
End Class

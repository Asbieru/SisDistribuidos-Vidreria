<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditarPieza
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
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpCortar = New System.Windows.Forms.GroupBox()
        Me.lblNotaCorte = New System.Windows.Forms.Label()
        Me.btnCortar = New System.Windows.Forms.Button()
        Me.nudMinUtil = New System.Windows.Forms.NumericUpDown()
        Me.lblMinUtil = New System.Windows.Forms.Label()
        Me.nudCorteLargo = New System.Windows.Forms.NumericUpDown()
        Me.lblCorteLargo = New System.Windows.Forms.Label()
        Me.nudCorteAlto = New System.Windows.Forms.NumericUpDown()
        Me.lblCorteAlto = New System.Windows.Forms.Label()
        Me.nudCorteAncho = New System.Windows.Forms.NumericUpDown()
        Me.lblCorteAncho = New System.Windows.Forms.Label()
        Me.grpEstado = New System.Windows.Forms.GroupBox()
        Me.lblNotaEstado = New System.Windows.Forms.Label()
        Me.btnDefectuosa = New System.Windows.Forms.Button()
        Me.btnVendida = New System.Windows.Forms.Button()
        Me.btnLiberar = New System.Windows.Forms.Button()
        Me.btnReservar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.grpCortar.SuspendLayout()
        CType(Me.nudMinUtil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorteLargo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorteAlto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorteAncho, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEstado.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblInfo.Location = New System.Drawing.Point(12, 12)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(496, 50)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.Text = "Pieza"
        '
        'grpCortar
        '
        Me.grpCortar.Controls.Add(Me.lblNotaCorte)
        Me.grpCortar.Controls.Add(Me.btnCortar)
        Me.grpCortar.Controls.Add(Me.nudMinUtil)
        Me.grpCortar.Controls.Add(Me.lblMinUtil)
        Me.grpCortar.Controls.Add(Me.nudCorteLargo)
        Me.grpCortar.Controls.Add(Me.lblCorteLargo)
        Me.grpCortar.Controls.Add(Me.nudCorteAlto)
        Me.grpCortar.Controls.Add(Me.lblCorteAlto)
        Me.grpCortar.Controls.Add(Me.nudCorteAncho)
        Me.grpCortar.Controls.Add(Me.lblCorteAncho)
        Me.grpCortar.Location = New System.Drawing.Point(12, 70)
        Me.grpCortar.Name = "grpCortar"
        Me.grpCortar.Size = New System.Drawing.Size(496, 150)
        Me.grpCortar.TabIndex = 1
        Me.grpCortar.TabStop = False
        Me.grpCortar.Text = "Cortar la pieza"
        '
        'lblCorteAncho
        '
        Me.lblCorteAncho.AutoSize = True
        Me.lblCorteAncho.Location = New System.Drawing.Point(12, 28)
        Me.lblCorteAncho.Name = "lblCorteAncho"
        Me.lblCorteAncho.Size = New System.Drawing.Size(65, 13)
        Me.lblCorteAncho.TabIndex = 0
        Me.lblCorteAncho.Text = "Ancho (cm):"
        '
        'nudCorteAncho
        '
        Me.nudCorteAncho.DecimalPlaces = 2
        Me.nudCorteAncho.Location = New System.Drawing.Point(140, 25)
        Me.nudCorteAncho.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteAncho.Name = "nudCorteAncho"
        Me.nudCorteAncho.Size = New System.Drawing.Size(100, 20)
        Me.nudCorteAncho.TabIndex = 1
        '
        'lblCorteAlto
        '
        Me.lblCorteAlto.AutoSize = True
        Me.lblCorteAlto.Location = New System.Drawing.Point(12, 58)
        Me.lblCorteAlto.Name = "lblCorteAlto"
        Me.lblCorteAlto.Size = New System.Drawing.Size(53, 13)
        Me.lblCorteAlto.TabIndex = 2
        Me.lblCorteAlto.Text = "Alto (cm):"
        '
        'nudCorteAlto
        '
        Me.nudCorteAlto.DecimalPlaces = 2
        Me.nudCorteAlto.Location = New System.Drawing.Point(140, 55)
        Me.nudCorteAlto.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteAlto.Name = "nudCorteAlto"
        Me.nudCorteAlto.Size = New System.Drawing.Size(100, 20)
        Me.nudCorteAlto.TabIndex = 3
        '
        'lblCorteLargo
        '
        Me.lblCorteLargo.AutoSize = True
        Me.lblCorteLargo.Location = New System.Drawing.Point(12, 88)
        Me.lblCorteLargo.Name = "lblCorteLargo"
        Me.lblCorteLargo.Size = New System.Drawing.Size(62, 13)
        Me.lblCorteLargo.TabIndex = 4
        Me.lblCorteLargo.Text = "Largo (cm):"
        '
        'nudCorteLargo
        '
        Me.nudCorteLargo.DecimalPlaces = 2
        Me.nudCorteLargo.Location = New System.Drawing.Point(140, 85)
        Me.nudCorteLargo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteLargo.Name = "nudCorteLargo"
        Me.nudCorteLargo.Size = New System.Drawing.Size(100, 20)
        Me.nudCorteLargo.TabIndex = 5
        '
        'lblMinUtil
        '
        Me.lblMinUtil.AutoSize = True
        Me.lblMinUtil.Location = New System.Drawing.Point(12, 118)
        Me.lblMinUtil.Name = "lblMinUtil"
        Me.lblMinUtil.Size = New System.Drawing.Size(110, 13)
        Me.lblMinUtil.TabIndex = 6
        Me.lblMinUtil.Text = "Retazo mínimo (cm):"
        '
        'nudMinUtil
        '
        Me.nudMinUtil.DecimalPlaces = 2
        Me.nudMinUtil.Location = New System.Drawing.Point(140, 115)
        Me.nudMinUtil.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudMinUtil.Name = "nudMinUtil"
        Me.nudMinUtil.Size = New System.Drawing.Size(100, 20)
        Me.nudMinUtil.TabIndex = 7
        Me.nudMinUtil.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblNotaCorte
        '
        Me.lblNotaCorte.Location = New System.Drawing.Point(260, 25)
        Me.lblNotaCorte.Name = "lblNotaCorte"
        Me.lblNotaCorte.Size = New System.Drawing.Size(225, 70)
        Me.lblNotaCorte.TabIndex = 8
        Me.lblNotaCorte.Text = "La pieza quedará CONSUMIDA y los sobrantes útiles se registrarán solos como retazos. Los sobrantes menores al retazo mínimo se consideran desperdicio."
        '
        'btnCortar
        '
        Me.btnCortar.Location = New System.Drawing.Point(335, 105)
        Me.btnCortar.Name = "btnCortar"
        Me.btnCortar.Size = New System.Drawing.Size(150, 32)
        Me.btnCortar.TabIndex = 9
        Me.btnCortar.Text = "Cortar"
        Me.btnCortar.UseVisualStyleBackColor = True
        '
        'grpEstado
        '
        Me.grpEstado.Controls.Add(Me.lblNotaEstado)
        Me.grpEstado.Controls.Add(Me.btnDefectuosa)
        Me.grpEstado.Controls.Add(Me.btnVendida)
        Me.grpEstado.Controls.Add(Me.btnLiberar)
        Me.grpEstado.Controls.Add(Me.btnReservar)
        Me.grpEstado.Location = New System.Drawing.Point(12, 230)
        Me.grpEstado.Name = "grpEstado"
        Me.grpEstado.Size = New System.Drawing.Size(496, 112)
        Me.grpEstado.TabIndex = 2
        Me.grpEstado.TabStop = False
        Me.grpEstado.Text = "Cambiar estado"
        '
        'btnReservar
        '
        Me.btnReservar.Location = New System.Drawing.Point(12, 28)
        Me.btnReservar.Name = "btnReservar"
        Me.btnReservar.Size = New System.Drawing.Size(150, 32)
        Me.btnReservar.TabIndex = 0
        Me.btnReservar.Text = "Reservar"
        Me.btnReservar.UseVisualStyleBackColor = True
        '
        'btnLiberar
        '
        Me.btnLiberar.Location = New System.Drawing.Point(172, 28)
        Me.btnLiberar.Name = "btnLiberar"
        Me.btnLiberar.Size = New System.Drawing.Size(150, 32)
        Me.btnLiberar.TabIndex = 1
        Me.btnLiberar.Text = "Liberar reserva"
        Me.btnLiberar.UseVisualStyleBackColor = True
        '
        'btnVendida
        '
        Me.btnVendida.Location = New System.Drawing.Point(12, 68)
        Me.btnVendida.Name = "btnVendida"
        Me.btnVendida.Size = New System.Drawing.Size(150, 32)
        Me.btnVendida.TabIndex = 2
        Me.btnVendida.Text = "Vendida entera"
        Me.btnVendida.UseVisualStyleBackColor = True
        '
        'btnDefectuosa
        '
        Me.btnDefectuosa.Location = New System.Drawing.Point(172, 68)
        Me.btnDefectuosa.Name = "btnDefectuosa"
        Me.btnDefectuosa.Size = New System.Drawing.Size(150, 32)
        Me.btnDefectuosa.TabIndex = 3
        Me.btnDefectuosa.Text = "Marcar defectuosa"
        Me.btnDefectuosa.UseVisualStyleBackColor = True
        '
        'lblNotaEstado
        '
        Me.lblNotaEstado.Location = New System.Drawing.Point(335, 28)
        Me.lblNotaEstado.Name = "lblNotaEstado"
        Me.lblNotaEstado.Size = New System.Drawing.Size(150, 72)
        Me.lblNotaEstado.TabIndex = 4
        Me.lblNotaEstado.Text = "Vendida y defectuosa sacan la pieza del inventario y no se pueden revertir."
        '
        'btnCerrar
        '
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Location = New System.Drawing.Point(408, 352)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(100, 30)
        Me.btnCerrar.TabIndex = 3
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmEditarPieza
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(520, 394)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.grpEstado)
        Me.Controls.Add(Me.grpCortar)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditarPieza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editar pieza"
        Me.grpCortar.ResumeLayout(False)
        Me.grpCortar.PerformLayout()
        CType(Me.nudMinUtil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorteLargo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorteAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorteAncho, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEstado.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblInfo As Label
    Friend WithEvents grpCortar As GroupBox
    Friend WithEvents lblNotaCorte As Label
    Friend WithEvents btnCortar As Button
    Friend WithEvents nudMinUtil As NumericUpDown
    Friend WithEvents lblMinUtil As Label
    Friend WithEvents nudCorteLargo As NumericUpDown
    Friend WithEvents lblCorteLargo As Label
    Friend WithEvents nudCorteAlto As NumericUpDown
    Friend WithEvents lblCorteAlto As Label
    Friend WithEvents nudCorteAncho As NumericUpDown
    Friend WithEvents lblCorteAncho As Label
    Friend WithEvents grpEstado As GroupBox
    Friend WithEvents lblNotaEstado As Label
    Friend WithEvents btnDefectuosa As Button
    Friend WithEvents btnVendida As Button
    Friend WithEvents btnLiberar As Button
    Friend WithEvents btnReservar As Button
    Friend WithEvents btnCerrar As Button
End Class

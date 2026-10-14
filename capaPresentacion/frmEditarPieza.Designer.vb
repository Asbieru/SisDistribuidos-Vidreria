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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpCortar = New System.Windows.Forms.GroupBox()
        Me.lblCorteAncho = New System.Windows.Forms.Label()
        Me.nudCorteAncho = New System.Windows.Forms.NumericUpDown()
        Me.lblCorteAlto = New System.Windows.Forms.Label()
        Me.nudCorteAlto = New System.Windows.Forms.NumericUpDown()
        Me.lblCorteLargo = New System.Windows.Forms.Label()
        Me.nudCorteLargo = New System.Windows.Forms.NumericUpDown()
        Me.lblMinUtil = New System.Windows.Forms.Label()
        Me.nudMinUtil = New System.Windows.Forms.NumericUpDown()
        Me.lblNotaCorte = New System.Windows.Forms.Label()
        Me.btnCortar = New System.Windows.Forms.Button()
        Me.grpEstado = New System.Windows.Forms.GroupBox()
        Me.btnReservar = New System.Windows.Forms.Button()
        Me.btnLiberar = New System.Windows.Forms.Button()
        Me.btnVendida = New System.Windows.Forms.Button()
        Me.btnDefectuosa = New System.Windows.Forms.Button()
        Me.lblNotaEstado = New System.Windows.Forms.Label()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.grpCortar.SuspendLayout()
        CType(Me.nudCorteAncho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorteAlto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCorteLargo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMinUtil, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEstado.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Controls.Add(Me.lblInfo)
        Me.Panel2.Controls.Add(Me.grpCortar)
        Me.Panel2.Controls.Add(Me.grpEstado)
        Me.Panel2.Controls.Add(Me.btnCerrar)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(640, 495)
        Me.Panel2.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(29, 25)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(158, 25)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "EDITAR PIEZA"
        '
        'lblInfo
        '
        Me.lblInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.Location = New System.Drawing.Point(31, 68)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(578, 42)
        Me.lblInfo.TabIndex = 1
        Me.lblInfo.Text = "Pieza"
        '
        'grpCortar
        '
        Me.grpCortar.Controls.Add(Me.lblCorteAncho)
        Me.grpCortar.Controls.Add(Me.nudCorteAncho)
        Me.grpCortar.Controls.Add(Me.lblCorteAlto)
        Me.grpCortar.Controls.Add(Me.nudCorteAlto)
        Me.grpCortar.Controls.Add(Me.lblCorteLargo)
        Me.grpCortar.Controls.Add(Me.nudCorteLargo)
        Me.grpCortar.Controls.Add(Me.lblMinUtil)
        Me.grpCortar.Controls.Add(Me.nudMinUtil)
        Me.grpCortar.Controls.Add(Me.lblNotaCorte)
        Me.grpCortar.Controls.Add(Me.btnCortar)
        Me.grpCortar.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCortar.Location = New System.Drawing.Point(31, 118)
        Me.grpCortar.Name = "grpCortar"
        Me.grpCortar.Size = New System.Drawing.Size(578, 172)
        Me.grpCortar.TabIndex = 2
        Me.grpCortar.TabStop = False
        Me.grpCortar.Text = "Cortar la pieza"
        '
        'lblCorteAncho
        '
        Me.lblCorteAncho.AutoSize = True
        Me.lblCorteAncho.Location = New System.Drawing.Point(15, 32)
        Me.lblCorteAncho.Name = "lblCorteAncho"
        Me.lblCorteAncho.Size = New System.Drawing.Size(76, 17)
        Me.lblCorteAncho.TabIndex = 0
        Me.lblCorteAncho.Text = "Ancho (cm)"
        '
        'nudCorteAncho
        '
        Me.nudCorteAncho.DecimalPlaces = 2
        Me.nudCorteAncho.Location = New System.Drawing.Point(175, 30)
        Me.nudCorteAncho.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteAncho.Name = "nudCorteAncho"
        Me.nudCorteAncho.Size = New System.Drawing.Size(110, 23)
        Me.nudCorteAncho.TabIndex = 1
        '
        'lblCorteAlto
        '
        Me.lblCorteAlto.AutoSize = True
        Me.lblCorteAlto.Location = New System.Drawing.Point(15, 66)
        Me.lblCorteAlto.Name = "lblCorteAlto"
        Me.lblCorteAlto.Size = New System.Drawing.Size(62, 17)
        Me.lblCorteAlto.TabIndex = 2
        Me.lblCorteAlto.Text = "Alto (cm)"
        '
        'nudCorteAlto
        '
        Me.nudCorteAlto.DecimalPlaces = 2
        Me.nudCorteAlto.Location = New System.Drawing.Point(175, 64)
        Me.nudCorteAlto.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteAlto.Name = "nudCorteAlto"
        Me.nudCorteAlto.Size = New System.Drawing.Size(110, 23)
        Me.nudCorteAlto.TabIndex = 3
        '
        'lblCorteLargo
        '
        Me.lblCorteLargo.AutoSize = True
        Me.lblCorteLargo.Location = New System.Drawing.Point(15, 100)
        Me.lblCorteLargo.Name = "lblCorteLargo"
        Me.lblCorteLargo.Size = New System.Drawing.Size(74, 17)
        Me.lblCorteLargo.TabIndex = 4
        Me.lblCorteLargo.Text = "Largo (cm)"
        '
        'nudCorteLargo
        '
        Me.nudCorteLargo.DecimalPlaces = 2
        Me.nudCorteLargo.Location = New System.Drawing.Point(175, 98)
        Me.nudCorteLargo.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCorteLargo.Name = "nudCorteLargo"
        Me.nudCorteLargo.Size = New System.Drawing.Size(110, 23)
        Me.nudCorteLargo.TabIndex = 5
        '
        'lblMinUtil
        '
        Me.lblMinUtil.AutoSize = True
        Me.lblMinUtil.Location = New System.Drawing.Point(15, 134)
        Me.lblMinUtil.Name = "lblMinUtil"
        Me.lblMinUtil.Size = New System.Drawing.Size(134, 17)
        Me.lblMinUtil.TabIndex = 6
        Me.lblMinUtil.Text = "Retazo mínimo (cm)"
        '
        'nudMinUtil
        '
        Me.nudMinUtil.DecimalPlaces = 2
        Me.nudMinUtil.Location = New System.Drawing.Point(175, 132)
        Me.nudMinUtil.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudMinUtil.Name = "nudMinUtil"
        Me.nudMinUtil.Size = New System.Drawing.Size(110, 23)
        Me.nudMinUtil.TabIndex = 7
        Me.nudMinUtil.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblNotaCorte
        '
        Me.lblNotaCorte.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblNotaCorte.Location = New System.Drawing.Point(305, 30)
        Me.lblNotaCorte.Name = "lblNotaCorte"
        Me.lblNotaCorte.Size = New System.Drawing.Size(258, 82)
        Me.lblNotaCorte.TabIndex = 8
        Me.lblNotaCorte.Text = "La pieza quedará CONSUMIDA y los sobrantes útiles se registrarán solos como retaz" &
    "os. Los sobrantes menores al retazo mínimo se consideran desperdicio."
        '
        'btnCortar
        '
        Me.btnCortar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCortar.Location = New System.Drawing.Point(443, 124)
        Me.btnCortar.Name = "btnCortar"
        Me.btnCortar.Size = New System.Drawing.Size(120, 34)
        Me.btnCortar.TabIndex = 9
        Me.btnCortar.Text = "CORTAR"
        Me.btnCortar.UseVisualStyleBackColor = True
        '
        'grpEstado
        '
        Me.grpEstado.Controls.Add(Me.btnReservar)
        Me.grpEstado.Controls.Add(Me.btnLiberar)
        Me.grpEstado.Controls.Add(Me.btnVendida)
        Me.grpEstado.Controls.Add(Me.btnDefectuosa)
        Me.grpEstado.Controls.Add(Me.lblNotaEstado)
        Me.grpEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpEstado.Location = New System.Drawing.Point(31, 300)
        Me.grpEstado.Name = "grpEstado"
        Me.grpEstado.Size = New System.Drawing.Size(578, 125)
        Me.grpEstado.TabIndex = 3
        Me.grpEstado.TabStop = False
        Me.grpEstado.Text = "Cambiar estado"
        '
        'btnReservar
        '
        Me.btnReservar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnReservar.Location = New System.Drawing.Point(15, 32)
        Me.btnReservar.Name = "btnReservar"
        Me.btnReservar.Size = New System.Drawing.Size(140, 34)
        Me.btnReservar.TabIndex = 0
        Me.btnReservar.Text = "RESERVAR"
        Me.btnReservar.UseVisualStyleBackColor = True
        '
        'btnLiberar
        '
        Me.btnLiberar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnLiberar.Location = New System.Drawing.Point(165, 32)
        Me.btnLiberar.Name = "btnLiberar"
        Me.btnLiberar.Size = New System.Drawing.Size(140, 34)
        Me.btnLiberar.TabIndex = 1
        Me.btnLiberar.Text = "LIBERAR RESERVA"
        Me.btnLiberar.UseVisualStyleBackColor = True
        '
        'btnVendida
        '
        Me.btnVendida.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVendida.Location = New System.Drawing.Point(15, 76)
        Me.btnVendida.Name = "btnVendida"
        Me.btnVendida.Size = New System.Drawing.Size(140, 34)
        Me.btnVendida.TabIndex = 2
        Me.btnVendida.Text = "VENDIDA ENTERA"
        Me.btnVendida.UseVisualStyleBackColor = True
        '
        'btnDefectuosa
        '
        Me.btnDefectuosa.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnDefectuosa.Location = New System.Drawing.Point(165, 76)
        Me.btnDefectuosa.Name = "btnDefectuosa"
        Me.btnDefectuosa.Size = New System.Drawing.Size(140, 34)
        Me.btnDefectuosa.TabIndex = 3
        Me.btnDefectuosa.Text = "DEFECTUOSA"
        Me.btnDefectuosa.UseVisualStyleBackColor = True
        '
        'lblNotaEstado
        '
        Me.lblNotaEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblNotaEstado.Location = New System.Drawing.Point(320, 32)
        Me.lblNotaEstado.Name = "lblNotaEstado"
        Me.lblNotaEstado.Size = New System.Drawing.Size(243, 78)
        Me.lblNotaEstado.TabIndex = 4
        Me.lblNotaEstado.Text = "Vendida y defectuosa sacan la pieza del inventario y no se pueden revertir."
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(489, 442)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(120, 34)
        Me.btnCerrar.TabIndex = 4
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmEditarPieza
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(640, 495)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditarPieza"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Editar pieza"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.grpCortar.ResumeLayout(False)
        Me.grpCortar.PerformLayout()
        CType(Me.nudCorteAncho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorteAlto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCorteLargo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMinUtil, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEstado.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents grpCortar As GroupBox
    Friend WithEvents lblCorteAncho As Label
    Friend WithEvents nudCorteAncho As NumericUpDown
    Friend WithEvents lblCorteAlto As Label
    Friend WithEvents nudCorteAlto As NumericUpDown
    Friend WithEvents lblCorteLargo As Label
    Friend WithEvents nudCorteLargo As NumericUpDown
    Friend WithEvents lblMinUtil As Label
    Friend WithEvents nudMinUtil As NumericUpDown
    Friend WithEvents lblNotaCorte As Label
    Friend WithEvents btnCortar As Button
    Friend WithEvents grpEstado As GroupBox
    Friend WithEvents btnReservar As Button
    Friend WithEvents btnLiberar As Button
    Friend WithEvents btnVendida As Button
    Friend WithEvents btnDefectuosa As Button
    Friend WithEvents lblNotaEstado As Label
    Friend WithEvents btnCerrar As Button
End Class

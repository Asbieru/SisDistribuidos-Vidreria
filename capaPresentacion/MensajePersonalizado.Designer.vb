<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MensajePersonalizado
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.chbNMostrar = New System.Windows.Forms.CheckBox()
        Me.lblContenido = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.BackColor = System.Drawing.Color.Transparent
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.Location = New System.Drawing.Point(13, 13)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(600, 25)
        Me.lblTitulo.TabIndex = 0
        '
        'btnCancelar
        '
        Me.btnCancelar.BackColor = System.Drawing.Color.Gainsboro
        Me.btnCancelar.Location = New System.Drawing.Point(555, 111)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 3
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = False
        '
        'btnAceptar
        '
        Me.btnAceptar.BackColor = System.Drawing.Color.Gainsboro
        Me.btnAceptar.Location = New System.Drawing.Point(474, 111)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 23)
        Me.btnAceptar.TabIndex = 2
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = False
        '
        'chbNMostrar
        '
        Me.chbNMostrar.AutoSize = True
        Me.chbNMostrar.BackColor = System.Drawing.Color.Transparent
        Me.chbNMostrar.ForeColor = System.Drawing.Color.DarkBlue
        Me.chbNMostrar.Location = New System.Drawing.Point(18, 96)
        Me.chbNMostrar.Name = "chbNMostrar"
        Me.chbNMostrar.Size = New System.Drawing.Size(184, 17)
        Me.chbNMostrar.TabIndex = 1
        Me.chbNMostrar.Text = "No volver a mostrar este Mensaje"
        Me.chbNMostrar.UseVisualStyleBackColor = False
        '
        'lblContenido
        '
        Me.lblContenido.BackColor = System.Drawing.Color.Transparent
        Me.lblContenido.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContenido.Location = New System.Drawing.Point(20, 51)
        Me.lblContenido.Name = "lblContenido"
        Me.lblContenido.Size = New System.Drawing.Size(600, 18)
        Me.lblContenido.TabIndex = 0
        '
        'MensajePersonalizado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(642, 143)
        Me.Controls.Add(Me.lblContenido)
        Me.Controls.Add(Me.chbNMostrar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.lblTitulo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MensajePersonalizado"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MensajePersonalizado"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnAceptar As Button
    Friend WithEvents chbNMostrar As CheckBox
    Friend WithEvents lblContenido As Label
End Class

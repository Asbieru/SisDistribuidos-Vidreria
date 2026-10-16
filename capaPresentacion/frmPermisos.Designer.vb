<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPermisos
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
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.btnQuitar = New System.Windows.Forms.Button()
        Me.btnAgregar = New System.Windows.Forms.Button()
        Me.lsvAsignados = New System.Windows.Forms.ListView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lsvDisponibles = New System.Windows.Forms.ListView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblRol = New System.Windows.Forms.Label()
        Me.cboUsuarios = New System.Windows.Forms.ComboBox()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.btnPermisosRol = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnCerrar)
        Me.Panel2.Controls.Add(Me.btnGuardar)
        Me.Panel2.Controls.Add(Me.btnPermisosRol)
        Me.Panel2.Controls.Add(Me.lblCantidad)
        Me.Panel2.Controls.Add(Me.lblDescripcion)
        Me.Panel2.Controls.Add(Me.btnQuitar)
        Me.Panel2.Controls.Add(Me.btnAgregar)
        Me.Panel2.Controls.Add(Me.lsvAsignados)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.lsvDisponibles)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.lblRol)
        Me.Panel2.Controls.Add(Me.cboUsuarios)
        Me.Panel2.Controls.Add(Me.lbl1)
        Me.Panel2.Controls.Add(Me.lblTitulo)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(987, 594)
        Me.Panel2.TabIndex = 0
        '
        'lblCantidad
        '
        Me.lblCantidad.AutoSize = True
        Me.lblCantidad.Location = New System.Drawing.Point(46, 491)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(0, 16)
        Me.lblCantidad.TabIndex = 28
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(46, 453)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(0, 16)
        Me.lblDescripcion.TabIndex = 27
        '
        'btnQuitar
        '
        Me.btnQuitar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnQuitar.Location = New System.Drawing.Point(424, 360)
        Me.btnQuitar.Name = "btnQuitar"
        Me.btnQuitar.Size = New System.Drawing.Size(136, 30)
        Me.btnQuitar.TabIndex = 26
        Me.btnQuitar.Text = "QUITAR"
        Me.btnQuitar.UseVisualStyleBackColor = True
        '
        'btnAgregar
        '
        Me.btnAgregar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnAgregar.Location = New System.Drawing.Point(424, 291)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(136, 30)
        Me.btnAgregar.TabIndex = 25
        Me.btnAgregar.Text = "AGREGAR"
        Me.btnAgregar.UseVisualStyleBackColor = True
        '
        'lsvAsignados
        '
        Me.lsvAsignados.HideSelection = False
        Me.lsvAsignados.Location = New System.Drawing.Point(598, 257)
        Me.lsvAsignados.Name = "lsvAsignados"
        Me.lsvAsignados.Size = New System.Drawing.Size(347, 170)
        Me.lsvAsignados.TabIndex = 20
        Me.lsvAsignados.UseCompatibleStateImageBehavior = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(597, 219)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 20)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Permisos asignados:"
        '
        'lsvDisponibles
        '
        Me.lsvDisponibles.HideSelection = False
        Me.lsvDisponibles.Location = New System.Drawing.Point(41, 257)
        Me.lsvDisponibles.Name = "lsvDisponibles"
        Me.lsvDisponibles.Size = New System.Drawing.Size(347, 170)
        Me.lsvDisponibles.TabIndex = 18
        Me.lsvDisponibles.UseCompatibleStateImageBehavior = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(37, 219)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 20)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Permisos disponibles:"
        '
        'lblRol
        '
        Me.lblRol.AutoSize = True
        Me.lblRol.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lblRol.Location = New System.Drawing.Point(37, 161)
        Me.lblRol.Name = "lblRol"
        Me.lblRol.Size = New System.Drawing.Size(0, 20)
        Me.lblRol.TabIndex = 16
        '
        'cboUsuarios
        '
        Me.cboUsuarios.FormattingEnabled = True
        Me.cboUsuarios.Location = New System.Drawing.Point(138, 115)
        Me.cboUsuarios.Name = "cboUsuarios"
        Me.cboUsuarios.Size = New System.Drawing.Size(356, 24)
        Me.cboUsuarios.TabIndex = 15
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.lbl1.Location = New System.Drawing.Point(37, 119)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(72, 20)
        Me.lbl1.TabIndex = 14
        Me.lbl1.Text = "Usuario:"
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(35, 48)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(353, 31)
        Me.lblTitulo.TabIndex = 2
        Me.lblTitulo.Text = "PERMISOS DE USUARIO"
        '
        'btnPermisosRol
        '
        Me.btnPermisosRol.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnPermisosRol.Location = New System.Drawing.Point(41, 537)
        Me.btnPermisosRol.Name = "btnPermisosRol"
        Me.btnPermisosRol.Size = New System.Drawing.Size(223, 30)
        Me.btnPermisosRol.TabIndex = 29
        Me.btnPermisosRol.Text = "PERMISOS DEL ROL"
        Me.btnPermisosRol.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.Location = New System.Drawing.Point(601, 537)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(136, 30)
        Me.btnGuardar.TabIndex = 30
        Me.btnGuardar.Text = "GUARDAR"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9!, System.Drawing.FontStyle.Bold)
        Me.btnCerrar.Location = New System.Drawing.Point(809, 537)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(136, 30)
        Me.btnCerrar.TabIndex = 31
        Me.btnCerrar.Text = "CERRAR"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmPermisos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(987, 594)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular)
        Me.Name = "frmPermisos"
        Me.Text = "Gestión de permisos"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblRol As Label
    Friend WithEvents cboUsuarios As ComboBox
    Friend WithEvents lbl1 As Label
    Friend WithEvents lsvAsignados As ListView
    Friend WithEvents Label2 As Label
    Friend WithEvents lsvDisponibles As ListView
    Friend WithEvents btnQuitar As Button
    Friend WithEvents btnAgregar As Button
    Friend WithEvents lblCantidad As Label
    Friend WithEvents lblDescripcion As Label
    Friend WithEvents btnCerrar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnPermisosRol As Button
End Class

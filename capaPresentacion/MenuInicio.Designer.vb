<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MenuInicio
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MenuInicio))
        Me.menu = New System.Windows.Forms.MenuStrip()
        Me.mnProcesos = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnRCompra = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnRPedido = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnRPago = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnReportes = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnVCompra = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnVVenta = New System.Windows.Forms.ToolStripMenuItem()
        Me.smnVInventario = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnConfiguracion = New System.Windows.Forms.ToolStripMenuItem()
        Me.TemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClaroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OscuroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnCerrarS = New System.Windows.Forms.ToolStripMenuItem()
        Me.stripDatos = New System.Windows.Forms.StatusStrip()
        Me.lblID = New System.Windows.Forms.ToolStripStatusLabel()
        Me.txtID = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.txtUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.toolBarra = New System.Windows.Forms.ToolStrip()
        Me.toolCotizar = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolInventario = New System.Windows.Forms.ToolStripButton()
        Me.menu.SuspendLayout()
        Me.stripDatos.SuspendLayout()
        Me.toolBarra.SuspendLayout()
        Me.SuspendLayout()
        '
        'menu
        '
        Me.menu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnProcesos, Me.mnReportes, Me.mnConfiguracion, Me.mnCerrarS})
        Me.menu.Location = New System.Drawing.Point(0, 0)
        Me.menu.Name = "menu"
        Me.menu.Size = New System.Drawing.Size(800, 24)
        Me.menu.TabIndex = 0
        Me.menu.Text = "menu"
        '
        'mnProcesos
        '
        Me.mnProcesos.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.smnRCompra, Me.smnRPedido, Me.smnRPago})
        Me.mnProcesos.Name = "mnProcesos"
        Me.mnProcesos.Size = New System.Drawing.Size(66, 20)
        Me.mnProcesos.Text = "Procesos"
        '
        'smnRCompra
        '
        Me.smnRCompra.Name = "smnRCompra"
        Me.smnRCompra.Size = New System.Drawing.Size(166, 22)
        Me.smnRCompra.Text = "Registrar Compra"
        '
        'smnRPedido
        '
        Me.smnRPedido.Name = "smnRPedido"
        Me.smnRPedido.Size = New System.Drawing.Size(166, 22)
        Me.smnRPedido.Text = "Regitrar Pedido"
        '
        'smnRPago
        '
        Me.smnRPago.Name = "smnRPago"
        Me.smnRPago.Size = New System.Drawing.Size(166, 22)
        Me.smnRPago.Text = "Registro Pago"
        '
        'mnReportes
        '
        Me.mnReportes.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.smnVCompra, Me.smnVVenta, Me.smnVInventario})
        Me.mnReportes.Name = "mnReportes"
        Me.mnReportes.Size = New System.Drawing.Size(65, 20)
        Me.mnReportes.Text = "Reportes"
        '
        'smnVCompra
        '
        Me.smnVCompra.Name = "smnVCompra"
        Me.smnVCompra.Size = New System.Drawing.Size(180, 22)
        Me.smnVCompra.Text = "Ver Compras"
        '
        'smnVVenta
        '
        Me.smnVVenta.Name = "smnVVenta"
        Me.smnVVenta.Size = New System.Drawing.Size(180, 22)
        Me.smnVVenta.Text = "Ver Ventas"
        '
        'smnVInventario
        '
        Me.smnVInventario.Name = "smnVInventario"
        Me.smnVInventario.Size = New System.Drawing.Size(180, 22)
        Me.smnVInventario.Text = "Ver Inventario"
        '
        'mnConfiguracion
        '
        Me.mnConfiguracion.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TemaToolStripMenuItem})
        Me.mnConfiguracion.Name = "mnConfiguracion"
        Me.mnConfiguracion.Size = New System.Drawing.Size(95, 20)
        Me.mnConfiguracion.Text = "Configuración"
        '
        'TemaToolStripMenuItem
        '
        Me.TemaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClaroToolStripMenuItem, Me.OscuroToolStripMenuItem})
        Me.TemaToolStripMenuItem.Name = "TemaToolStripMenuItem"
        Me.TemaToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.TemaToolStripMenuItem.Text = "Tema"
        '
        'ClaroToolStripMenuItem
        '
        Me.ClaroToolStripMenuItem.Name = "ClaroToolStripMenuItem"
        Me.ClaroToolStripMenuItem.Size = New System.Drawing.Size(112, 22)
        Me.ClaroToolStripMenuItem.Text = "Claro"
        '
        'OscuroToolStripMenuItem
        '
        Me.OscuroToolStripMenuItem.Name = "OscuroToolStripMenuItem"
        Me.OscuroToolStripMenuItem.Size = New System.Drawing.Size(112, 22)
        Me.OscuroToolStripMenuItem.Text = "Oscuro"
        '
        'mnCerrarS
        '
        Me.mnCerrarS.Name = "mnCerrarS"
        Me.mnCerrarS.Size = New System.Drawing.Size(87, 20)
        Me.mnCerrarS.Text = "Cerrar sesión"
        '
        'stripDatos
        '
        Me.stripDatos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblID, Me.txtID, Me.lblUsuario, Me.txtUsuario})
        Me.stripDatos.Location = New System.Drawing.Point(0, 364)
        Me.stripDatos.Name = "stripDatos"
        Me.stripDatos.Size = New System.Drawing.Size(800, 22)
        Me.stripDatos.TabIndex = 2
        Me.stripDatos.Text = "StatusStrip1"
        '
        'lblID
        '
        Me.lblID.Name = "lblID"
        Me.lblID.Size = New System.Drawing.Size(21, 17)
        Me.lblID.Text = "ID:"
        '
        'txtID
        '
        Me.txtID.AutoSize = False
        Me.txtID.BackColor = System.Drawing.Color.Gainsboro
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(50, 17)
        '
        'lblUsuario
        '
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(109, 17)
        Me.lblUsuario.Text = "Usuario conectado:"
        '
        'txtUsuario
        '
        Me.txtUsuario.AutoSize = False
        Me.txtUsuario.BackColor = System.Drawing.Color.Gainsboro
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(150, 17)
        '
        'toolBarra
        '
        Me.toolBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.toolCotizar, Me.ToolStripSeparator1, Me.toolInventario})
        Me.toolBarra.Location = New System.Drawing.Point(0, 24)
        Me.toolBarra.Name = "toolBarra"
        Me.toolBarra.Size = New System.Drawing.Size(800, 25)
        Me.toolBarra.TabIndex = 4
        Me.toolBarra.Text = "ToolStrip1"
        '
        'toolCotizar
        '
        Me.toolCotizar.BackColor = System.Drawing.Color.White
        Me.toolCotizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolCotizar.Image = CType(resources.GetObject("toolCotizar.Image"), System.Drawing.Image)
        Me.toolCotizar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolCotizar.Name = "toolCotizar"
        Me.toolCotizar.Size = New System.Drawing.Size(23, 22)
        Me.toolCotizar.Tag = "CotizarVentana"
        Me.toolCotizar.Text = "Cotizar Ventana"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'toolInventario
        '
        Me.toolInventario.BackColor = System.Drawing.Color.White
        Me.toolInventario.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.toolInventario.Image = CType(resources.GetObject("toolInventario.Image"), System.Drawing.Image)
        Me.toolInventario.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.toolInventario.Name = "toolInventario"
        Me.toolInventario.Size = New System.Drawing.Size(23, 22)
        Me.toolInventario.Tag = "Inventario"
        Me.toolInventario.Text = "Inventario"
        '
        'MenuInicio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(800, 386)
        Me.Controls.Add(Me.toolBarra)
        Me.Controls.Add(Me.stripDatos)
        Me.Controls.Add(Me.menu)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.menu
        Me.Name = "MenuInicio"
        Me.Text = "Página de Incio"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.menu.ResumeLayout(False)
        Me.menu.PerformLayout()
        Me.stripDatos.ResumeLayout(False)
        Me.stripDatos.PerformLayout()
        Me.toolBarra.ResumeLayout(False)
        Me.toolBarra.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents menu As MenuStrip
    Friend WithEvents mnProcesos As ToolStripMenuItem
    Friend WithEvents mnReportes As ToolStripMenuItem
    Friend WithEvents mnConfiguracion As ToolStripMenuItem
    Friend WithEvents mnCerrarS As ToolStripMenuItem
    Friend WithEvents smnRCompra As ToolStripMenuItem
    Friend WithEvents smnRPedido As ToolStripMenuItem
    Friend WithEvents stripDatos As StatusStrip
    Friend WithEvents smnVCompra As ToolStripMenuItem
    Friend WithEvents smnVVenta As ToolStripMenuItem
    Friend WithEvents smnVInventario As ToolStripMenuItem
    Friend WithEvents lblUsuario As ToolStripStatusLabel
    Friend WithEvents txtUsuario As ToolStripStatusLabel
    Friend WithEvents lblID As ToolStripStatusLabel
    Friend WithEvents txtID As ToolStripStatusLabel
    Friend WithEvents TemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClaroToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OscuroToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents toolBarra As ToolStrip
    Friend WithEvents toolInventario As ToolStripButton
    Friend WithEvents toolCotizar As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents smnRPago As ToolStripMenuItem
End Class

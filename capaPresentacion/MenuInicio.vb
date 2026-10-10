Imports System.IO
Imports capaNegocio

Public Class MenuInicio
    Dim tema As String = "CLARO"
    Dim objUsu As New Usuario

    Private Sub temaColor()
        Dim p As Paleta = Temas.Obtener(tema)
        ' Menú y submenús (recursivo)
        menu.BackColor = p.Fondo
        AplicarTemaMenu(menu.Items, p)
        ' ToolStrip, incluyendo cambio de íconos
        toolBarra.BackColor = p.Fondo
        AplicarTemaToolStrip(toolBarra.Items, p)
        ' Status strip
        stripDatos.BackColor = p.Fondo
        AplicarStatusStrip(stripDatos.Items, p)
        ' Imagen de fondo según el tema
        Dim sufijo As String = If(tema.Equals("OSCURO"), "Oscuro", "Claro")
        Dim rutaFondo As String = Path.Combine(Application.StartupPath, "Imagenes", "Fondos", "fondoInicio" & sufijo & ".jpg")
        If File.Exists(rutaFondo) Then
            Dim fondoViejo As Image = Me.BackgroundImage
            Me.BackgroundImage = Image.FromFile(rutaFondo)
            Me.BackgroundImageLayout = ImageLayout.Stretch
            If fondoViejo IsNot Nothing Then fondoViejo.Dispose()  ' libera el fondo anterior
        End If
    End Sub

    Private Sub AplicarTemaMenu(items As ToolStripItemCollection, p As Paleta)
        For Each item As ToolStripItem In items
            item.ForeColor = p.Texto
            item.BackColor = p.Fondo
            Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
            If menuItem IsNot Nothing AndAlso menuItem.HasDropDownItems Then
                AplicarTemaMenu(menuItem.DropDownItems, p)  ' entra al submenú
            End If
        Next
    End Sub

    Private Sub AplicarStatusStrip(items As ToolStripItemCollection, p As Paleta)
        For Each item As ToolStripItem In items
            item.ForeColor = p.Texto
            item.BackColor = p.Fondo
        Next
    End Sub

    Private Sub AplicarTemaToolStrip(items As ToolStripItemCollection, p As Paleta)
        Dim carpeta As String = If(tema.Equals("OSCURO"), "Oscuro", "Claro")
        For Each item As ToolStripItem In items
            item.ForeColor = p.Texto
            item.BackColor = p.Fondo

            Dim nombreIcono As String = If(item.Tag IsNot Nothing, item.Tag.ToString(), item.Name)
            If Not String.IsNullOrEmpty(nombreIcono) Then
                Dim ruta As String = Path.Combine(Application.StartupPath, "Imagenes", "Iconos", carpeta, nombreIcono & ".png")
                If File.Exists(ruta) Then
                    Dim imgVieja As Image = item.Image
                    item.Image = Image.FromFile(ruta)
                    If imgVieja IsNot Nothing Then imgVieja.Dispose()  ' libera la imagen anterior de memoria
                End If
            End If
        Next
    End Sub

    Private Sub mnCerrarS_Click(sender As Object, e As EventArgs) Handles mnCerrarS.Click
        Dim codigo As String = "CERRARSESION"
        Try
            Dim idMs As Integer = objUsu.obtenerIDMensaje(codigo)
            If objUsu.mensajeYaDescartado(Integer.Parse(txtID.Text), idMs) Then
                Dim objIncio As New InicioSesion
                objIncio.Show()
                Dispose()
            Else
                Dim objMsj As New MensajePersonalizado With {.idUsuario = Integer.Parse(txtID.Text), .tema = tema, .idMensaje = idMs}
                objMsj.llenarDatos()
                objMsj.ShowDialog()
                If objMsj.respuesta Then
                    Dim objIncio As New InicioSesion
                    objIncio.Show()
                    Dispose()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error mensaje: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MenuInicio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            txtID.Text = objUsu.obtenerIDUsuario(txtUsuario.Text)
            Dim dt As DataTable = objUsu.obtenerPreferencia(Integer.Parse(txtID.Text))
            If dt.Rows.Count > 0 Then
                tema = dt.Rows(0).Item(0).ToString
                temaColor()
            End If
        Catch ex As Exception
            MessageBox.Show("Error al iniciar Menu: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GuardarTema(color As String)
        Try
            objUsu.guardarPreferencia(Integer.Parse(txtID.Text), color, "Arial")
        Catch ex As Exception
            MessageBox.Show("Error al guardar tema", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ClaroToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClaroToolStripMenuItem.Click
        CambiarTema("CLARO")
    End Sub

    Private Sub OscuroToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OscuroToolStripMenuItem.Click
        CambiarTema("OSCURO")
    End Sub

    ' Cambia el tema del menú y lo propaga a todas las ventanas hijas abiertas.
    ' Se recorren Application.OpenForms para cubrir también las ventanas que
    ' no son MDI hijas directas (abiertas desde otros formularios).
    Private Sub CambiarTema(nuevoTema As String)
        tema = nuevoTema
        GuardarTema(tema)
        temaColor()
        For Each f As Form In Application.OpenForms
            Dim conTema As IFormularioTema = TryCast(f, IFormularioTema)
            If conTema IsNot Nothing Then
                conTema.tema = nuevoTema
                AplicarTema(f, nuevoTema)
                f.Refresh()
            End If
        Next
    End Sub

    Private Sub toolCotizar_Click(sender As Object, e As EventArgs) Handles toolCotizar.Click
        Dim objMsj As New frmCotizarVentana With {.tema = tema}
        objMsj.MdiParent = Me
        objMsj.Show()
    End Sub

    Private Sub toolInventario_Click(sender As Object, e As EventArgs) Handles toolInventario.Click, smnVInventario.Click
        Dim objInv As New frmVerInventario With {.idUsuario = Integer.Parse(txtID.Text), .tema = tema}
        objInv.MdiParent = Me
        objInv.Show()
    End Sub

    Private Sub smnUsuarios_Click(sender As Object, e As EventArgs) Handles smnUsuarios.Click
        Dim frm As New frmRegistrarUsuarios With {.tema = tema}
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub smnRCompra_Click(sender As Object, e As EventArgs) Handles smnRCompra.Click
        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is frmRegistrarCompras Then
                formulario.WindowState = FormWindowState.Normal
                formulario.BringToFront()
                Return
            End If
        Next

        Dim registro As New frmRegistrarCompras With {.tema = tema}
        registro.MdiParent = Me
        registro.Show()
    End Sub

    Private Sub smnVCompra_Click(sender As Object, e As EventArgs) Handles smnVCompra.Click
        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is frmListadoCompras Then
                Dim listado As frmListadoCompras =
                    DirectCast(formulario, frmListadoCompras)

                listado.cargarLista()
                listado.WindowState = FormWindowState.Normal
                listado.BringToFront()
                Return
            End If
        Next

        Dim nuevoListado As New frmListadoCompras With {.tema = tema}
        nuevoListado.MdiParent = Me
        nuevoListado.Show()
    End Sub

    Private Sub smnClientes_Click(sender As Object, e As EventArgs) Handles smnClientes.Click
        Try
            For Each formulario As Form In Application.OpenForms
                If TypeOf formulario Is frmListadoClientes Then
                    Dim listado As frmListadoClientes = DirectCast(formulario, frmListadoClientes)
                    If Not listado.modoSeleccion Then
                        listado.WindowState = FormWindowState.Normal
                        listado.cargarLista()
                        listado.BringToFront()
                        Return
                    End If
                End If
            Next
            Dim nuevoListado As New frmListadoClientes With {.tema = tema}
            nuevoListado.MdiParent = Me
            nuevoListado.Show()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CerrarTodasLasVentanasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CerrarTodasLasVentanasToolStripMenuItem.Click
        For Each f As Form In Me.MdiChildren
            f.Close()
        Next
    End Sub
End Class

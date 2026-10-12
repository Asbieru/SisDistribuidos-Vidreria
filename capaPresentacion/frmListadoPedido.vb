Imports capaNegocio
Imports System.Data

Public Class frmListadoPedido
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    ' False: mantenimiento. True: ShowDialog devuelve el pedido elegido.
    Public Property modoSeleccion As Boolean = False
    Public Property idUsuario As Integer = 0
    Public Property idPedidoSeleccionado As Integer = 0

    Dim objPedido As New Pedido
    Dim dtPedidos As DataTable

    Private Sub frmListadoPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        lblTitulo.ForeColor = Temas.Obtener(tema).Acento
        ResolverUsuario()

        btnSeleccionar.Visible = modoSeleccion
        btnNuevo.Visible = Not modoSeleccion
        btnModificar.Visible = Not modoSeleccion
        btnCancelar.Visible = Not modoSeleccion
        btnModificar.Enabled = False
        lblTitulo.Text = If(modoSeleccion, "SELECCIONAR PEDIDO", "LISTADO DE PEDIDOS")
        Text = If(modoSeleccion, "Seleccionar pedido", "Listado de pedidos")
        idPedidoSeleccionado = 0

        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList
        cboCategoria.SelectedIndex = cboCategoria.Items.IndexOf("Usuario")

        cargarLista()
    End Sub

    ' Si no le pasaron el usuario, lo toma del formulario padre (MenuInicio.txtID)
    Private Sub ResolverUsuario()
        If idUsuario > 0 Then Return
        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If padre IsNot Nothing Then Integer.TryParse(padre.txtID.Text, idUsuario)
    End Sub

    ' idReseleccionar: deja seleccionado el pedido recién modificado
    Public Sub cargarLista(Optional idReseleccionar As Integer = 0)
        Try
            dtPedidos = objPedido.ListarDetallado()
            tblPedidos.DataSource = dtPedidos

            Encabezados(tblPedidos, "id_pedido|ID", "cliente|Cliente", "usuario|Usuario", "fecha|Fecha", "estado|Estado")
            tblPedidos.Columns("id_pedido").FillWeight = 30
            tblPedidos.Columns("cliente").FillWeight = 200
            tblPedidos.Columns("usuario").FillWeight = 150
            tblPedidos.Columns("fecha").FillWeight = 120
            tblPedidos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            tblPedidos.Columns("estado").FillWeight = 90

            filtrar()

            If idReseleccionar > 0 Then
                For Each fila As DataGridViewRow In tblPedidos.Rows
                    If CInt(fila.Cells("id_pedido").Value) = idReseleccionar Then
                        tblPedidos.CurrentCell = fila.Cells("id_pedido")
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        Finally
            actualizarBotones()
        End Try
    End Sub

    ' Filtra por el texto buscado en la columna de la categoría elegida, sin volver a consultar la base
    Private Sub filtrar()
        If dtPedidos Is Nothing Then Return
        Dim texto As String = txtBuscar.Text.Trim().Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
        Dim filtro As String = ""
        If texto <> "" Then
            Dim campo As String
            Select Case cboCategoria.Text
                Case "Cliente"
                    campo = "cliente"
                Case "Fecha de Registro"
                    campo = "fecha_registro"
                Case Else
                    campo = "usuario"
            End Select
            filtro = campo & " LIKE '%" & texto & "%'"
        End If
        dtPedidos.DefaultView.RowFilter = filtro
        Dim encontrados As Integer = dtPedidos.DefaultView.Count
        msj.Text = If(encontrados = 0, "No se encontraron pedidos.",
                      "Pedidos encontrados: " & encontrados.ToString())
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        filtrar()
    End Sub

    Private Sub cboCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategoria.SelectedIndexChanged
        filtrar()
    End Sub

    Private Sub btnVerTodos_Click(sender As Object, e As EventArgs) Handles btnVerTodos.Click
        txtBuscar.Clear()
        cargarLista()
    End Sub

    ' Devuelve el id de la fila seleccionada, o 0 si no hay ninguna
    Private Function idSeleccionado() As Integer
        If tblPedidos.CurrentRow Is Nothing OrElse Not tblPedidos.Columns.Contains("id_pedido") Then Return 0
        Return CInt(tblPedidos.CurrentRow.Cells("id_pedido").Value)
    End Function

    Private Function estadoSeleccionado() As String
        If tblPedidos.CurrentRow Is Nothing OrElse Not tblPedidos.Columns.Contains("estado") Then Return ""
        Return tblPedidos.CurrentRow.Cells("estado").Value.ToString()
    End Function

    Private Sub actualizarBotones()
        Dim haySeleccion As Boolean = idSeleccionado() > 0
        btnCancelar.Enabled = haySeleccion
        btnSeleccionar.Enabled = haySeleccion
    End Sub

    Private Sub tblPedidos_SelectionChanged(sender As Object, e As EventArgs) Handles tblPedidos.SelectionChanged
        actualizarBotones()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Using frm As New frmRegistrarPedido With {.tema = tema, .idUsuario = idUsuario}
            frm.ShowDialog(Me)
            cargarLista()
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        If estadoSeleccionado() = "CANCELADO" Then
            UtilFormularios.MostrarInfo("El pedido seleccionado ya está cancelado.")
            Return
        End If
        Try
            Dim objUsu As New Usuario
            Dim objMsj As New MensajePersonalizado With {.idUsuario = idUsuario, .tema = tema,
                                                         .idMensaje = objUsu.obtenerIDMensaje("CANCELARPEDIDO")}
            objMsj.llenarDatos()
            objMsj.ShowDialog()
            If Not objMsj.respuesta Then Return

            objPedido.CambiarEstado(id, "CANCELADO")
            cargarLista(id)
            UtilFormularios.MostrarInfo("Pedido cancelado correctamente.")
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        If modoSeleccion Then
            DialogResult = DialogResult.Cancel
        Else
            Close()
        End If
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        If Not modoSeleccion Then Return
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        idPedidoSeleccionado = id
        DialogResult = DialogResult.OK
    End Sub
End Class

Imports capaNegocio
Imports System.Data

Public Class frmListadoClientes
    Public Property tema As String = "CLARO"
    ' False: mantenimiento. True: ShowDialog devuelve el cliente elegido.
    Public Property modoSeleccion As Boolean = False
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""
    Private ReadOnly objCliente As New Cliente
    Private dtClientes As DataTable
    Private cargando As Boolean = True

    Private Sub frmListadoClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Panel2.BackColor = Temas.Obtener(tema).Fondo
        lblTitulo.ForeColor = Temas.Obtener(tema).Acento
        btnSeleccionar.Visible = modoSeleccion
        btnEliminar.Visible = Not modoSeleccion
        lblTitulo.Text = If(modoSeleccion, "SELECCIONAR CLIENTE", "LISTADO DE CLIENTES")
        Text = If(modoSeleccion, "Seleccionar cliente", "Mantenimiento de clientes")
        idClienteSeleccionado = 0
        nombreClienteSeleccionado = ""
        cargando = False
        cargarLista()
    End Sub

    Public Sub cargarLista(Optional idReseleccionar As Integer = 0)
        Try
            dtClientes = If(String.IsNullOrWhiteSpace(txtBuscar.Text),
                            objCliente.Listar(), objCliente.Buscar(txtBuscar.Text))
            dgvClientes.DataSource = dtClientes
            Encabezados(dgvClientes, "nombre|Nombre", "documento|Documento",
                        "telefono|Teléfono", "direccion|Dirección")
            dgvClientes.Columns("nombre").FillWeight = 180
            dgvClientes.Columns("direccion").FillWeight = 220
            lblCantidad.Text = If(dtClientes.Rows.Count = 0, "No se encontraron clientes.",
                                 "Clientes encontrados: " & dtClientes.Rows.Count.ToString())
            For Each fila As DataGridViewRow In dgvClientes.Rows
                If CInt(fila.Cells("id_cliente").Value) = idReseleccionar Then
                    dgvClientes.CurrentCell = fila.Cells("nombre")
                    Exit For
                End If
            Next
        Catch ex As Exception
            dtClientes = Nothing
            dgvClientes.DataSource = Nothing
            lblCantidad.Text = "No se pudo cargar el listado. Presione Buscar para reintentar."
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            actualizarBotones()
        End Try
    End Sub

    Private Function idSeleccionado() As Integer
        If dgvClientes.CurrentRow Is Nothing OrElse
           Not dgvClientes.Columns.Contains("id_cliente") Then Return 0
        Return CInt(dgvClientes.CurrentRow.Cells("id_cliente").Value)
    End Function

    Private Sub actualizarBotones()
        Dim haySeleccion As Boolean = idSeleccionado() > 0
        btnModificar.Enabled = haySeleccion
        btnEliminar.Enabled = haySeleccion
        btnSeleccionar.Enabled = haySeleccion
    End Sub

    Private Sub dgvClientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvClientes.SelectionChanged
        If Not cargando Then actualizarBotones()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        cargarLista()
    End Sub

    Private Sub txtBuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuscar.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            cargarLista()
        End If
    End Sub

    Private Sub btnVerTodos_Click(sender As Object, e As EventArgs) Handles btnVerTodos.Click
        txtBuscar.Clear()
        cargarLista()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        abrirRegistro(0)
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim id As Integer = idSeleccionado()
        If id > 0 Then abrirRegistro(id)
    End Sub

    Private Sub abrirRegistro(id As Integer)
        Using formulario As New frmRegistrarClientes With {.idCliente = id, .tema = tema}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                ' La búsqueda anterior puede ocultar al cliente recién guardado.
                txtBuscar.Clear()
                cargarLista(formulario.idClienteGuardado)
            End If
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Try
            If objCliente.TienePedidos(id) Then
                MessageBox.Show("El cliente tiene pedidos y no se puede eliminar. Su historial debe conservarse.",
                    "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim nombre As String = dgvClientes.CurrentRow.Cells("nombre").Value.ToString()
            If MessageBox.Show("¿Eliminar al cliente '" & nombre & "'? Esta acción no se puede deshacer.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            objCliente.Eliminar(id)
            cargarLista()
            MessageBox.Show("Cliente eliminado correctamente.", "Clientes",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        If Not modoSeleccion Then Return
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Try
            ' Revalidar: otro trabajador pudo eliminar el cliente tras cargar la grilla.
            Dim dt As DataTable = objCliente.Obtener(id)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("El cliente ya no existe. Seleccione otro.", "Clientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cargarLista()
                Return
            End If
            idClienteSeleccionado = id
            nombreClienteSeleccionado = dt.Rows(0)("nombre").ToString()
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvClientes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvClientes.CellDoubleClick
        If e.RowIndex < 0 Then Return
        If modoSeleccion Then
            btnSeleccionar.PerformClick()
        Else
            btnModificar.PerformClick()
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        If modoSeleccion Then
            DialogResult = DialogResult.Cancel
        Else
            Close()
        End If
    End Sub
End Class

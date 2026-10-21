Imports capaNegocio

Public Class frmListadoProveedores
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    ' False: mantenimiento. True: ShowDialog devuelve el proveedor elegido.
    Public Property modoSeleccion As Boolean = False
    Public Property idProveedorSeleccionado As Integer = 0
    Public Property nombreProveedorSeleccionado As String = ""
    Private ReadOnly objProveedor As New Proveedor
    Private dtProveedores As DataTable
    Private cargando As Boolean = True

    Private Sub frmListadoProveedores_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Titulo
        btnSeleccionar.Visible = modoSeleccion
        btnEliminar.Visible = Not modoSeleccion
        lblTitulo.Text = If(modoSeleccion, "SELECCIONAR PROVEEDOR", "LISTADO DE PROVEEDORES")
        Text = If(modoSeleccion, "Seleccionar proveedor", "Mantenimiento de proveedores")
        idProveedorSeleccionado = 0
        nombreProveedorSeleccionado = ""
        cargando = False
        cargarLista()
    End Sub

    Public Sub cargarLista(Optional idReseleccionar As Integer = 0)
        Try
            dtProveedores = If(String.IsNullOrWhiteSpace(txtBuscar.Text),
                            objProveedor.Listar(), objProveedor.Buscar(txtBuscar.Text))
            dgvProveedores.DataSource = dtProveedores
            Encabezados(dgvProveedores, "nombre|Nombre / Razón social", "tipo_persona|Persona",
                        "tipo_documento|Tipo documento", "documento|Número",
                        "telefono|Teléfono", "direccion|Dirección")
            dgvProveedores.Columns("nombre").FillWeight = 180
            dgvProveedores.Columns("tipo_persona").FillWeight = 140
            dgvProveedores.Columns("tipo_persona").MinimumWidth = 125
            dgvProveedores.Columns("tipo_documento").FillWeight = 140
            dgvProveedores.Columns("tipo_documento").MinimumWidth = 125
            dgvProveedores.Columns("direccion").FillWeight = 160
            lblCantidad.Text = If(dtProveedores.Rows.Count = 0, "No se encontraron proveedores.",
                                 "Proveedores encontrados: " & dtProveedores.Rows.Count.ToString())
            For Each fila As DataGridViewRow In dgvProveedores.Rows
                If CInt(fila.Cells("id_proveedor").Value) = idReseleccionar Then
                    dgvProveedores.CurrentCell = fila.Cells("nombre")
                    Exit For
                End If
            Next
        Catch ex As Exception
            dtProveedores = Nothing
            dgvProveedores.DataSource = Nothing
            lblCantidad.Text = "No se pudo cargar el listado. Presione Buscar para reintentar."
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            actualizarBotones()
        End Try
    End Sub

    Private Function idSeleccionado() As Integer
        If dgvProveedores.CurrentRow Is Nothing OrElse
           Not dgvProveedores.Columns.Contains("id_proveedor") Then Return 0
        Return CInt(dgvProveedores.CurrentRow.Cells("id_proveedor").Value)
    End Function

    Private Sub actualizarBotones()
        Dim haySeleccion As Boolean = idSeleccionado() > 0
        btnModificar.Enabled = haySeleccion
        btnEliminar.Enabled = haySeleccion
        btnSeleccionar.Enabled = haySeleccion

    End Sub

    Private Sub dgvProveedores_SelectionChanged(sender As Object, e As EventArgs) Handles dgvProveedores.SelectionChanged
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
        Using formulario As New frmRegistrarProveedores With {.idProveedor = id, .tema = tema}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                ' La búsqueda anterior puede ocultar al proveedor recién guardado.
                txtBuscar.Clear()
                cargarLista(formulario.idProveedorGuardado)
            End If
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Try
            If objProveedor.TieneCompras(id) Then
                MessageBox.Show("El proveedor tiene compras y no se puede eliminar. Su historial debe conservarse.",
                    "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim nombre As String = dgvProveedores.CurrentRow.Cells("nombre").Value.ToString()
            If MessageBox.Show("¿Eliminar al proveedor '" & nombre & "'? Esta acción no se puede deshacer.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            objProveedor.Eliminar(id)
            cargarLista()
            MessageBox.Show("Proveedor eliminado correctamente.", "Proveedores",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        If Not modoSeleccion Then Return
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Try
            ' Revalidar: otro trabajador pudo eliminar el proveedor tras cargar la grilla.
            Dim dt As DataTable = objProveedor.Obtener(id)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("El proveedor ya no existe. Seleccione otro.", "Proveedores",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cargarLista()
                Return
            End If
            idProveedorSeleccionado = id
            nombreProveedorSeleccionado = dt.Rows(0)("nombre").ToString()
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvProveedores_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProveedores.CellDoubleClick
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

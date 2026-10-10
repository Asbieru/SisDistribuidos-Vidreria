Imports capaNegocio

Public Class frmListadoProductos
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objProd As New Producto
    Dim dtProductos As DataTable

    Private Sub frmListadoProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento

        dgvProductos.ReadOnly = True
        dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProductos.MultiSelect = False
        dgvProductos.AllowUserToAddRows = False
        dgvProductos.AllowUserToDeleteRows = False
        dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList
        cboCategoria.Items.Clear()
        cboCategoria.Items.Add("TODAS")
        cboCategoria.Items.AddRange(Producto.Categorias)
        cboCategoria.SelectedIndex = 0

        cargarLista()
    End Sub

    ' idReseleccionar: deja seleccionado el producto recién guardado
    Public Sub cargarLista(Optional idReseleccionar As Integer = 0)
        Try
            dtProductos = objProd.ListarTodos()
            dgvProductos.DataSource = dtProductos

            Encabezados(dgvProductos, "id_producto|ID", "categoria|Categoría", "tipo|Tipo", "color|Color",
                        "nombre|Nombre", "descripcion|Descripción", "variantes|Variantes", "estado|Estado")
            dgvProductos.Columns("id_producto").FillWeight = 40
            dgvProductos.Columns("variantes").FillWeight = 60
            dgvProductos.Columns("nombre").FillWeight = 160
            dgvProductos.Columns("descripcion").FillWeight = 180

            filtrar()

            If idReseleccionar > 0 Then
                For Each fila As DataGridViewRow In dgvProductos.Rows
                    If CInt(fila.Cells("id_producto").Value) = idReseleccionar Then
                        dgvProductos.CurrentCell = fila.Cells("nombre")
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            actualizarBotones()
        End Try
    End Sub

    ' Filtra por texto (nombre, tipo o color) y por categoría, sin volver a consultar la base
    Private Sub filtrar()
        If dtProductos Is Nothing Then Return
        Dim condiciones As New List(Of String)
        Dim texto As String = txtBuscar.Text.Trim.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
        If texto <> "" Then
            condiciones.Add("(nombre LIKE '%" & texto & "%' OR tipo LIKE '%" & texto & "%' OR color LIKE '%" & texto & "%')")
        End If
        If cboCategoria.SelectedIndex > 0 Then
            condiciones.Add("categoria = '" & cboCategoria.Text & "'")
        End If
        dtProductos.DefaultView.RowFilter = String.Join(" AND ", condiciones)
        lblCantidad.Text = If(dtProductos.DefaultView.Count = 0, "No se encontraron productos.",
                              "Productos encontrados: " & dtProductos.DefaultView.Count.ToString)
        actualizarBotones()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        filtrar()
    End Sub

    Private Sub cboCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategoria.SelectedIndexChanged
        filtrar()
    End Sub

    Private Sub btnVerTodos_Click(sender As Object, e As EventArgs) Handles btnVerTodos.Click
        txtBuscar.Clear()
        cboCategoria.SelectedIndex = 0
        cargarLista()
    End Sub

    ' Devuelve el id de la fila seleccionada, o 0 si no hay ninguna
    Private Function idSeleccionado() As Integer
        If dgvProductos.CurrentRow Is Nothing OrElse Not dgvProductos.Columns.Contains("id_producto") Then Return 0
        Return CInt(dgvProductos.CurrentRow.Cells("id_producto").Value)
    End Function

    Private Function productoActivo() As Boolean
        If dgvProductos.CurrentRow Is Nothing OrElse Not dgvProductos.Columns.Contains("activo") Then Return True
        Return CBool(dgvProductos.CurrentRow.Cells("activo").Value)
    End Function

    Private Sub actualizarBotones()
        Dim haySeleccion As Boolean = idSeleccionado() > 0
        btnModificar.Enabled = haySeleccion
        btnEliminar.Enabled = haySeleccion
        btnDarBaja.Enabled = haySeleccion
        btnDarBaja.Text = If(haySeleccion AndAlso Not productoActivo(), "REACTIVAR", "DAR DE BAJA")
    End Sub

    Private Sub dgvProductos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvProductos.SelectionChanged
        actualizarBotones()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        abrirRegistro(0)
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim id As Integer = idSeleccionado()
        If id > 0 Then abrirRegistro(id)
    End Sub

    ' Doble clic en una fila hace lo mismo que Modificar
    Private Sub dgvProductos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos.CellDoubleClick
        If e.RowIndex >= 0 Then btnModificar.PerformClick()
    End Sub

    ' ShowDialog abre el registro como ventana modal: mientras esté abierto
    ' no se puede usar el listado; al cerrarlo, el código sigue en la línea siguiente
    Private Sub abrirRegistro(id As Integer)
        Using frm As New frmRegistrarProductos With {.idProducto = id, .tema = tema}
            If frm.ShowDialog(Me) = DialogResult.OK Then
                ' Los filtros podrían ocultar el producto recién guardado
                txtBuscar.Clear()
                cboCategoria.SelectedIndex = 0
                cargarLista(frm.idProductoGuardado)
            End If
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Dim nombre As String = dgvProductos.CurrentRow.Cells("nombre").Value.ToString
        Try
            If objProd.TieneVariantes(id) Then
                MessageBox.Show("El producto '" & nombre & "' tiene variantes registradas (medidas y precios), " &
                                "y puede estar en compras o inventario, así que no se puede eliminar." & vbCrLf &
                                "Use el botón Dar de baja.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If MessageBox.Show("¿Seguro que desea eliminar el producto '" & nombre & "'?" & vbCrLf & "Esta acción no se puede deshacer.",
                               "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
            objProd.Eliminar(id)
            cargarLista()
            MessageBox.Show("Producto eliminado correctamente.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Si el producto está ACTIVO lo da de baja; si está INACTIVO lo reactiva
    Private Sub btnDarBaja_Click(sender As Object, e As EventArgs) Handles btnDarBaja.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Dim nombre As String = dgvProductos.CurrentRow.Cells("nombre").Value.ToString
        Dim reactivar As Boolean = Not productoActivo()
        Dim pregunta As String = If(reactivar,
            "¿Reactivar el producto '" & nombre & "'?",
            "¿Dar de baja el producto '" & nombre & "'?" & vbCrLf &
            "Ya no aparecerá al registrar compras ni piezas de inventario, pero se conserva su historial.")
        If MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Try
            objProd.CambiarEstado(id, reactivar)
            cargarLista(id)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Los productos dados de baja se ven en gris
    Private Sub dgvProductos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvProductos.CellFormatting
        If e.RowIndex < 0 OrElse Not dgvProductos.Columns.Contains("activo") Then Return
        Dim valor As Object = dgvProductos.Rows(e.RowIndex).Cells("activo").Value
        If valor IsNot Nothing AndAlso Not IsDBNull(valor) AndAlso Not CBool(valor) Then
            e.CellStyle.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class
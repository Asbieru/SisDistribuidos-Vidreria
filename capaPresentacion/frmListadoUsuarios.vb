Imports capaNegocio
Public Class frmListadoUsuarios
    Dim objUsu As New Usuario
    Dim dtUsuarios As DataTable

    Private Sub frmListadoUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dgvUsuarios.ReadOnly = True
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvUsuarios.MultiSelect = False
        dgvUsuarios.AllowUserToAddRows = False
        dgvUsuarios.AllowUserToDeleteRows = False
        dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        cargarLista()
    End Sub

    ' Pública para que frmRegistrarUsuarios pueda refrescarla después de guardar
    Public Sub cargarLista()
        Try
            dtUsuarios = objUsu.listarUsuarios()
            dgvUsuarios.DataSource = dtUsuarios

            ' Datos sensibles que no se muestran en la tabla (igual se cargan al modificar)
            dgvUsuarios.Columns("contrasena").Visible = False
            dgvUsuarios.Columns("respuesta").Visible = False

            dgvUsuarios.Columns("id_usuario").HeaderText = "ID"
            dgvUsuarios.Columns("nombre").HeaderText = "Usuario"
            dgvUsuarios.Columns("correo").HeaderText = "Correo"
            dgvUsuarios.Columns("pregunta_seguridad").HeaderText = "Pregunta de seguridad"
            dgvUsuarios.Columns("rol").HeaderText = "Rol"
            dgvUsuarios.Columns("estado").HeaderText = "Estado"
            dgvUsuarios.Columns("fecha_creacion").HeaderText = "Fecha de registro"

            filtrar()
            actualizarBotonBaja()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub filtrar()
        If dtUsuarios Is Nothing Then Return
        Dim texto As String = txtBuscar.Text.Trim.Replace("'", "''")
        If texto = "" Then
            dtUsuarios.DefaultView.RowFilter = ""
        Else
            dtUsuarios.DefaultView.RowFilter = "nombre LIKE '%" & texto & "%' OR correo LIKE '%" & texto & "%'"
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        filtrar()
    End Sub

    ' Devuelve el id de la fila seleccionada, o 0 si no hay ninguna
    Private Function idSeleccionado() As Integer
        If dgvUsuarios.CurrentRow Is Nothing Then
            MessageBox.Show("Primero seleccione un usuario de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return 0
        End If
        Return CInt(dgvUsuarios.CurrentRow.Cells("id_usuario").Value)
    End Function

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return

        ' Si el formulario de registro ya está abierto se reutiliza; si está cerrado, se abre
        Dim frm As frmRegistrarUsuarios = Application.OpenForms.OfType(Of frmRegistrarUsuarios)().FirstOrDefault()
        If frm Is Nothing Then
            frm = New frmRegistrarUsuarios
            frm.Show()
        Else
            If frm.WindowState = FormWindowState.Minimized Then frm.WindowState = FormWindowState.Normal
            frm.BringToFront()
        End If
        frm.cargarUsuario(id)
    End Sub

    ' Doble clic en una fila hace lo mismo que Modificar
    Private Sub dgvUsuarios_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvUsuarios.CellDoubleClick
        If e.RowIndex >= 0 Then btnModificar.PerformClick()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Dim nombre As String = dgvUsuarios.CurrentRow.Cells("nombre").Value.ToString
        Try
            If objUsu.tienePedidos(id) Then
                MessageBox.Show("El usuario '" & nombre & "' tiene pedidos registrados y no se puede eliminar." & vbCrLf &
                                "Use el botón Dar de baja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If MessageBox.Show("¿Seguro que desea eliminar al usuario '" & nombre & "'?" & vbCrLf & "Esta acción no se puede deshacer.",
                               "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Return
            End If
            objUsu.eliminarUsuario(id)
            MessageBox.Show("Usuario eliminado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
            cargarLista()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Si el usuario está ACTIVO lo da de baja; si está INACTIVO lo reactiva
    Private Sub btnDarBaja_Click(sender As Object, e As EventArgs) Handles btnDarBaja.Click
        Dim id As Integer = idSeleccionado()
        If id = 0 Then Return
        Dim nombre As String = dgvUsuarios.CurrentRow.Cells("nombre").Value.ToString
        Dim reactivar As Boolean = dgvUsuarios.CurrentRow.Cells("estado").Value.ToString = "INACTIVO"
        Dim pregunta As String = If(reactivar,
                                    "¿Reactivar al usuario '" & nombre & "'?",
                                    "¿Dar de baja al usuario '" & nombre & "'? Ya no podrá iniciar sesión.")
        If MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If
        Try
            objUsu.cambiarEstadoUsuario(id, reactivar)
            cargarLista()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvUsuarios_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUsuarios.SelectionChanged
        actualizarBotonBaja()
    End Sub

    ' Cambia el texto del botón según el estado de la fila seleccionada
    Private Sub actualizarBotonBaja()
        If dgvUsuarios.CurrentRow Is Nothing OrElse Not dgvUsuarios.Columns.Contains("estado") Then Return
        If dgvUsuarios.CurrentRow.Cells("estado").Value.ToString = "INACTIVO" Then
            btnDarBaja.Text = "Reactivar"
        Else
            btnDarBaja.Text = "Dar de baja"
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class
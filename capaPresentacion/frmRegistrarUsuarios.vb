Imports capaNegocio
Public Class frmRegistrarUsuarios
    Dim objUsu As New Usuario
    Dim idUsuario As Integer = 0   ' 0 = usuario nuevo, mayor a 0 = se está modificando ese usuario

    Private Sub frmRegistrarUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboRol.DropDownStyle = ComboBoxStyle.DropDownList
        cboRol.Items.Clear()
        cboRol.Items.AddRange(New String() {"VENDEDOR", "ADMINISTRADOR"})

        txtContrasena.UseSystemPasswordChar = True

        ' El ID lo genera la base de datos: solo se muestra, no se escribe
        txtId.ReadOnly = True
        txtId.TabStop = False

        limpiar()
    End Sub

    ' Lo llama frmListadoUsuarios cuando se presiona Modificar
    Public Sub cargarUsuario(id As Integer)
        Try
            Dim dt As DataTable = objUsu.obtenerUsuario(id)
            If dt.Rows.Count = 0 Then
                MessageBox.Show("No se encontró el usuario seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim fila As DataRow = dt.Rows(0)
            idUsuario = id
            txtId.Text = id.ToString
            txtUsuario.Text = fila("nombre").ToString
            txtCorreo.Text = fila("correo").ToString
            txtContrasena.Text = fila("contrasena").ToString
            txtPregunta.Text = fila("pregunta_seguridad").ToString
            txtRespuesta.Text = fila("respuesta").ToString
            cboRol.SelectedItem = fila("rol").ToString
            chkActivo.Checked = CBool(fila("activo"))

            lblModo.Text = "Modificando usuario (ID " & id & ")"
            txtUsuario.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub limpiar()
        idUsuario = 0
        txtId.Text = "(automático)"
        txtUsuario.Clear()
        txtCorreo.Clear()
        txtContrasena.Clear()
        txtPregunta.Clear()
        txtRespuesta.Clear()
        cboRol.SelectedIndex = 0
        chkActivo.Checked = True
        lblModo.Text = "Nuevo usuario"
        txtUsuario.Focus()
    End Sub

    Private Function validar() As Boolean
        If txtUsuario.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese el nombre de usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsuario.Focus()
            Return False
        End If
        If txtCorreo.Text.Trim.Length = 0 OrElse Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            MessageBox.Show("Ingrese un correo válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCorreo.Focus()
            Return False
        End If
        If txtContrasena.Text.Trim.Length < 4 Then
            MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContrasena.Focus()
            Return False
        End If
        If txtPregunta.Text.Trim.Length = 0 Then
            MessageBox.Show("Escriba una pregunta de seguridad.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPregunta.Focus()
            Return False
        End If
        If txtRespuesta.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese la respuesta de seguridad.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRespuesta.Focus()
            Return False
        End If
        If cboRol.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione un rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboRol.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not validar() Then Return
        Try
            If objUsu.existeNombre(txtUsuario.Text.Trim, idUsuario) Then
                MessageBox.Show("Ya existe otro usuario con ese nombre.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUsuario.Focus()
                Return
            End If
            If objUsu.existeCorreo(txtCorreo.Text.Trim, idUsuario) Then
                MessageBox.Show("Ya existe otro usuario con ese correo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCorreo.Focus()
                Return
            End If

            If idUsuario = 0 Then
                objUsu.registrarUsuario(txtUsuario.Text.Trim, txtCorreo.Text.Trim, txtContrasena.Text.Trim,
                                        txtPregunta.Text.Trim, txtRespuesta.Text.Trim, cboRol.Text, chkActivo.Checked)
                MessageBox.Show("Usuario registrado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                objUsu.modificarUsuario(idUsuario, txtUsuario.Text.Trim, txtCorreo.Text.Trim, txtContrasena.Text.Trim,
                                        txtPregunta.Text.Trim, txtRespuesta.Text.Trim, cboRol.Text, chkActivo.Checked)
                MessageBox.Show("Usuario modificado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            limpiar()
            actualizarListado()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
    End Sub

    Private Sub btnVerLista_Click(sender As Object, e As EventArgs) Handles btnVerLista.Click
        ' Si el listado ya está abierto se trae al frente; si no, se abre uno nuevo
        Dim frm As frmListadoUsuarios = Application.OpenForms.OfType(Of frmListadoUsuarios)().FirstOrDefault()
        If frm Is Nothing Then
            frm = New frmListadoUsuarios
            frm.Show()
        Else
            If frm.WindowState = FormWindowState.Minimized Then frm.WindowState = FormWindowState.Normal
            frm.cargarLista()
            frm.BringToFront()
        End If
    End Sub

    ' Si el listado está abierto, se refresca para que muestre el cambio
    Private Sub actualizarListado()
        Dim frm As frmListadoUsuarios = Application.OpenForms.OfType(Of frmListadoUsuarios)().FirstOrDefault()
        If frm IsNot Nothing Then frm.cargarLista()
    End Sub
End Class
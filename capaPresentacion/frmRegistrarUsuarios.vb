Imports capaNegocio

Public Class frmRegistrarUsuarios
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    ' Lo envía frmListadoUsuarios: 0 = usuario nuevo, mayor a 0 = modificar ese usuario
    Public Property idUsuario As Integer = 0
    ' Lo lee frmListadoUsuarios al volver, para dejar seleccionado al usuario guardado
    Public Property nombreGuardado As String = ""

    Dim objUsu As New Usuario
    Dim objPer As New Permiso
    Dim rolOriginal As String = ""   ' para saber si al modificar le cambiaron el rol

    Private Sub frmRegistrarUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblModo.ForeColor = p.Acento

        cboRol.DropDownStyle = ComboBoxStyle.DropDownList
        cboRol.Items.Clear()
        cboRol.Items.AddRange(New String() {"VENDEDOR", "ADMINISTRADOR"})

        txtContrasena.UseSystemPasswordChar = True

        ' El ID lo genera la base de datos: solo se muestra, no se escribe
        txtId.ReadOnly = True
        txtId.TabStop = False

        nombreGuardado = ""
        Try
            If idUsuario = 0 Then
                lblModo.Text = "NUEVO USUARIO"
                txtId.Text = "(automático)"
                cboRol.SelectedIndex = 0
                ' Un usuario nuevo siempre se crea activo: el check no se muestra
                chkActivo.Checked = True
                chkActivo.Visible = False
            Else
                chkActivo.Visible = True
                cargarUsuario()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        End Try
    End Sub

    Private Sub cargarUsuario()
        Dim dt As DataTable = objUsu.obtenerUsuario(idUsuario)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("El usuario ya no existe. Actualice el listado.", "Usuarios",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Return
        End If

        Dim fila As DataRow = dt.Rows(0)
        lblModo.Text = "MODIFICAR USUARIO"
        txtId.Text = idUsuario.ToString
        txtUsuario.Text = fila("nombre").ToString
        txtCorreo.Text = fila("correo").ToString
        txtContrasena.Text = fila("contrasena").ToString
        txtPregunta.Text = fila("pregunta_seguridad").ToString
        txtRespuesta.Text = fila("respuesta").ToString
        cboRol.SelectedItem = fila("rol").ToString
        rolOriginal = fila("rol").ToString
        chkActivo.Checked = CBool(fila("activo"))   ' marcado = ACTIVO, sin marcar = INACTIVO
    End Sub

    Private Sub frmRegistrarUsuarios_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtUsuario.Focus()
        txtUsuario.SelectAll()
    End Sub

    Private Function validar() As Boolean
        If txtUsuario.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese el nombre de usuario.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUsuario.Focus()
            Return False
        End If
        If txtCorreo.Text.Trim.Length = 0 OrElse Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            MessageBox.Show("Ingrese un correo válido.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCorreo.Focus()
            Return False
        End If
        If txtContrasena.Text.Trim.Length < 4 Then
            MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtContrasena.Focus()
            Return False
        End If
        If txtPregunta.Text.Trim.Length = 0 Then
            MessageBox.Show("Escriba una pregunta de seguridad.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPregunta.Focus()
            Return False
        End If
        If txtRespuesta.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese la respuesta de seguridad.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRespuesta.Focus()
            Return False
        End If
        If cboRol.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione un rol.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboRol.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not validar() Then Return
        Try
            If objUsu.existeNombre(txtUsuario.Text.Trim, idUsuario) Then
                MessageBox.Show("Ya existe otro usuario con ese nombre.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtUsuario.Focus()
                Return
            End If
            If objUsu.existeCorreo(txtCorreo.Text.Trim, idUsuario) Then
                MessageBox.Show("Ya existe otro usuario con ese correo.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCorreo.Focus()
                Return
            End If

            ' No se puede dejar el sistema sin administradores activos
            If idUsuario > 0 AndAlso (cboRol.Text <> "ADMINISTRADOR" OrElse Not chkActivo.Checked) _
               AndAlso objUsu.esUltimoAdministrador(idUsuario) Then
                MessageBox.Show("Este es el único administrador activo del sistema." & vbCrLf &
                                "No se le puede quitar el rol ADMINISTRADOR ni dejarlo inactivo.", "Usuarios",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Si al modificar le cambiaron el rol, se pregunta si se le dan los permisos del nuevo rol
            Dim reasignarPermisos As Boolean = False
            If idUsuario > 0 AndAlso cboRol.Text <> rolOriginal Then
                reasignarPermisos = MessageBox.Show(
                    "Cambió el rol de " & rolOriginal & " a " & cboRol.Text & "." & vbCrLf &
                    "¿Desea reemplazar sus permisos por los del rol " & cboRol.Text & "?",
                    "Usuarios", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
            End If

            If idUsuario = 0 Then
                ' Todo usuario nuevo se crea activo
                objUsu.registrarUsuario(txtUsuario.Text.Trim, txtCorreo.Text.Trim, txtContrasena.Text.Trim,
                                        txtPregunta.Text.Trim, txtRespuesta.Text.Trim, cboRol.Text, True)
                ' El usuario nuevo recibe los permisos por defecto de su rol
                Dim idNuevo As Integer = objUsu.obtenerIDUsuario(txtUsuario.Text.Trim)
                objPer.asignarPermisosRol(idNuevo, cboRol.Text)
                MessageBox.Show("Usuario registrado correctamente con los permisos del rol " & cboRol.Text & ".",
                                "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' Se guarda el estado que tenga el check: marcado = ACTIVO, sin marcar = INACTIVO
                objUsu.modificarUsuario(idUsuario, txtUsuario.Text.Trim, txtCorreo.Text.Trim, txtContrasena.Text.Trim,
                                        txtPregunta.Text.Trim, txtRespuesta.Text.Trim, cboRol.Text, chkActivo.Checked)
                If reasignarPermisos Then objPer.asignarPermisosRol(idUsuario, cboRol.Text)
                MessageBox.Show("Usuario modificado correctamente.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            nombreGuardado = txtUsuario.Text.Trim
            DialogResult = DialogResult.OK   ' cierra el formulario y avisa al listado que hubo cambios
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel   ' cierra sin guardar
    End Sub
End Class
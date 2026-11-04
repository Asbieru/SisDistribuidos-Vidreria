Imports System.Data
Imports capaNegocio

Partial Class wfRegistrarUsuarios
    Inherits System.Web.UI.Page

    Private ReadOnly objUsuario As New Usuario()
    Private ReadOnly objPermiso As New Permiso()

    ' Usuario que se esta editando (0 = nuevo)
    Private Property IdUsuarioActual As Integer
        Get
            If ViewState("idUsuario") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idUsuario"))
        End Get
        Set(value As Integer)
            ViewState("idUsuario") = value
        End Set
    End Property

    ' Rol que tenia el usuario al cargarlo (para saber si cambio)
    Private Property RolOriginal As String
        Get
            Return If(ViewState("rolOriginal"), "").ToString()
        End Get
        Set(value As String)
            ViewState("rolOriginal") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarListado("")

            Dim id As Integer
            If Integer.TryParse(Request.QueryString("id"), id) AndAlso id > 0 Then
                CargarUsuario(id)
            Else
                PrepararNuevo()
            End If
        End If
    End Sub

    ' ------------------------- Formulario -------------------------

    Private Sub PrepararNuevo()
        IdUsuarioActual = 0
        RolOriginal = ""

        lblModo.Text = "NUEVO USUARIO"
        txtId.Text = "(automático)"
        txtUsuario.Text = ""
        txtCorreo.Text = ""
        txtContrasena.Text = ""
        txtPregunta.Text = ""
        txtRespuesta.Text = ""
        ddlRol.SelectedValue = "VENDEDOR"

        filaActivo.Visible = False
        filaPermisos.Visible = False
        chkActivo.Checked = True
        chkReasignarPermisos.Checked = False
        MostrarMensaje("", "")
    End Sub

    Private Sub CargarUsuario(id As Integer)
        If id <= 0 Then
            PrepararNuevo()
            Return
        End If

        Try
            Dim dt As DataTable = objUsuario.obtenerUsuario(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                PrepararNuevo()
                MostrarMensaje("El usuario ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If

            Dim fila As DataRow = dt.Rows(0)
            IdUsuarioActual = id
            RolOriginal = Texto(fila("rol"))

            lblModo.Text = "MODIFICAR USUARIO"
            txtId.Text = id.ToString()
            txtUsuario.Text = Texto(fila("nombre"))
            txtCorreo.Text = Texto(fila("correo"))
            txtContrasena.Text = Texto(fila("contrasena"))
            txtPregunta.Text = Texto(fila("pregunta_seguridad"))
            txtRespuesta.Text = Texto(fila("respuesta"))
            ddlRol.SelectedValue = If(RolOriginal = "ADMINISTRADOR", "ADMINISTRADOR", "VENDEDOR")

            filaActivo.Visible = True
            filaPermisos.Visible = True
            chkActivo.Checked = Convert.ToBoolean(fila("activo"))
            chkReasignarPermisos.Checked = False
            MostrarMensaje("", "")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        PrepararNuevo()
        txtUsuario.Focus()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        btnGuardar.Enabled = False
        Try
            If Not ValidarFormulario() Then Return

            Dim nombre As String = txtUsuario.Text.Trim()
            Dim correo As String = txtCorreo.Text.Trim()
            Dim rol As String = ddlRol.SelectedValue

            If objUsuario.existeNombre(nombre, IdUsuarioActual) Then
                MostrarMensaje("Ya existe otro usuario con ese nombre.", "mensaje-error")
                Return
            End If
            If objUsuario.existeCorreo(correo, IdUsuarioActual) Then
                MostrarMensaje("Ya existe otro usuario con ese correo.", "mensaje-error")
                Return
            End If

            ' No se puede dejar el sistema sin administradores activos.
            If IdUsuarioActual > 0 AndAlso
               (rol <> "ADMINISTRADOR" OrElse Not chkActivo.Checked) AndAlso
               objUsuario.esUltimoAdministrador(IdUsuarioActual) Then
                MostrarMensaje("Este es el único administrador activo del sistema. " &
                               "No se le puede quitar el rol ADMINISTRADOR ni dejarlo inactivo.", "mensaje-error")
                Return
            End If

            If IdUsuarioActual = 0 Then
                objUsuario.registrarUsuario(nombre, correo, txtContrasena.Text.Trim(),
                                            txtPregunta.Text.Trim(), txtRespuesta.Text.Trim(),
                                            rol, True)
                Dim idNuevo As Integer = objUsuario.obtenerIDUsuario(nombre)
                objPermiso.asignarPermisosRol(idNuevo, rol)

                CargarUsuario(idNuevo)
                CargarListado(txtBuscar.Text)
                MostrarMensaje("Usuario registrado correctamente con los permisos del rol " & rol & ".", "mensaje-ok")
            Else
                objUsuario.modificarUsuario(IdUsuarioActual, nombre, correo, txtContrasena.Text.Trim(),
                                            txtPregunta.Text.Trim(), txtRespuesta.Text.Trim(),
                                            rol, chkActivo.Checked)

                ' Si cambio el rol y se marco la casilla, se reemplazan sus permisos.
                If chkReasignarPermisos.Checked AndAlso rol <> RolOriginal Then
                    objPermiso.asignarPermisosRol(IdUsuarioActual, rol)
                End If

                CargarUsuario(IdUsuarioActual)
                CargarListado(txtBuscar.Text)
                MostrarMensaje("Usuario modificado correctamente.", "mensaje-ok")
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Function ValidarFormulario() As Boolean
        If txtUsuario.Text.Trim().Length = 0 Then
            MostrarMensaje("Ingrese el nombre de usuario.", "mensaje-error")
            Return False
        End If
        If txtCorreo.Text.Trim().Length = 0 OrElse
           Not txtCorreo.Text.Contains("@") OrElse Not txtCorreo.Text.Contains(".") Then
            MostrarMensaje("Ingrese un correo válido.", "mensaje-error")
            Return False
        End If
        If txtContrasena.Text.Trim().Length < 4 Then
            MostrarMensaje("La contraseña debe tener al menos 4 caracteres.", "mensaje-error")
            Return False
        End If
        If txtPregunta.Text.Trim().Length = 0 Then
            MostrarMensaje("Escriba una pregunta de seguridad.", "mensaje-error")
            Return False
        End If
        If txtRespuesta.Text.Trim().Length = 0 Then
            MostrarMensaje("Ingrese la respuesta de seguridad.", "mensaje-error")
            Return False
        End If
        If String.IsNullOrEmpty(ddlRol.SelectedValue) Then
            MostrarMensaje("Seleccione un rol.", "mensaje-error")
            Return False
        End If
        Return True
    End Function

    ' ------------------------- Listado -------------------------

    Private Sub CargarListado(filtro As String)
        Try
            Dim dt As DataTable = objUsuario.listarUsuarios()
            If dt Is Nothing Then dt = New DataTable()

            If Not String.IsNullOrWhiteSpace(filtro) Then
                Dim texto As String = filtro.Trim().ToUpperInvariant()
                Dim filtrado As DataTable = dt.Clone()
                For Each fila As DataRow In dt.Rows
                    Dim coincide As Boolean =
                        fila("id_usuario").ToString().Contains(texto) OrElse
                        fila("nombre").ToString().ToUpperInvariant().Contains(texto) OrElse
                        fila("correo").ToString().ToUpperInvariant().Contains(texto) OrElse
                        fila("rol").ToString().ToUpperInvariant().Contains(texto)
                    If coincide Then filtrado.ImportRow(fila)
                Next
                dt = filtrado
            End If

            grvUsuarios.DataSource = dt
            grvUsuarios.DataBind()

            If dt.Rows.Count = 0 Then
                lblMensajeLista.CssClass = "mensaje-info"
                lblMensajeLista.Text = "No se encontraron usuarios."
            Else
                lblMensajeLista.CssClass = ""
                lblMensajeLista.Text = dt.Rows.Count.ToString() & " usuario(s) encontrado(s)."
            End If
        Catch ex As Exception
            lblMensajeLista.CssClass = "mensaje-error"
            lblMensajeLista.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        CargarListado(txtBuscar.Text)
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscar.Text = ""
        CargarListado("")
    End Sub

    Protected Sub grvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim id As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), id) OrElse id <= 0 Then Return

        Select Case e.CommandName
            Case "EditarUsuario"
                CargarUsuario(id)
            Case "CambiarEstadoUsuario"
                CambiarEstadoUsuario(id)
            Case "EliminarUsuario"
                EliminarUsuario(id)
        End Select
    End Sub

    Private Sub CambiarEstadoUsuario(id As Integer)
        Try
            Dim dt As DataTable = objUsuario.obtenerUsuario(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("El usuario ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If

            Dim activo As Boolean = Convert.ToBoolean(dt.Rows(0)("activo"))
            If activo AndAlso objUsuario.esUltimoAdministrador(id) Then
                MostrarMensaje("Este es el único administrador activo del sistema. No se le puede dar de baja.", "mensaje-error")
                Return
            End If

            objUsuario.cambiarEstadoUsuario(id, Not activo)

            If IdUsuarioActual = id Then
                CargarUsuario(id)
            End If

            CargarListado(txtBuscar.Text)
            MostrarMensaje(If(activo, "Usuario inactivado.", "Usuario activado."), "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub EliminarUsuario(id As Integer)
        Try
            If objUsuario.esUltimoAdministrador(id) Then
                MostrarMensaje("Este es el único administrador activo del sistema. No se puede eliminar.", "mensaje-error")
                Return
            End If

            objUsuario.eliminarUsuario(id)

            If IdUsuarioActual = id Then
                PrepararNuevo()
            End If

            CargarListado(txtBuscar.Text)
            MostrarMensaje("Usuario eliminado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Function Texto(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        Return valor.ToString()
    End Function
End Class

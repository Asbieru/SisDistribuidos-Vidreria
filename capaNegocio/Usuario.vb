Imports capaDatos
Public Class Usuario
    Dim objMan As New clsMantenimiento
    Dim strCon As String

    Public Function iniciarSesion(usu As String, con As String) As Boolean
        Dim dt As New DataTable
        strCon = "select * from usuarios where nombre='" & usu & "' and contrasena='" & con & "'"
        Try
            dt = objMan.listarComando(strCon)
            Return dt.Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al iniciar sesión!")
        End Try
    End Function

    Public Function validarNombreUsuario(usu As String) As Boolean
        Dim dt As New DataTable
        strCon = "select * from usuarios where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(strCon)
            Return dt.Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar nombre de usuario!")
        End Try
    End Function

    Public Function obtenerPregunta(usu As String) As String
        Dim dt As New DataTable
        strCon = "select pregunta_seguridad from usuarios where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(strCon)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item(0).ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception("Error al obtener pregunta secreta!")
        End Try
    End Function

    Public Function validarRespuesta(usu As String, res As String) As Boolean
        Dim dt As New DataTable
        strCon = "select respuesta from usuarios where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(strCon)
            Return dt.Rows.Count > 0 AndAlso dt.Rows(0).Item(0).ToString.Equals(res)
        Catch ex As Exception
            Throw New Exception("Error al validar respuesta!")
        End Try
    End Function

    Public Sub cambiarContraseña(usu As String, con As String)
        strCon = "update usuarios set contrasena='" & con & "' where nombre='" & usu & "'"
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al modificar contraseña!")
        End Try
    End Sub

    Public Function obtenerIDUsuario(usu As String) As Integer
        Dim dt As New DataTable
        strCon = "select id_usuario from usuarios where nombre='" & usu & "'"
        dt = objMan.listarComando(strCon)
        Return Integer.Parse(dt.Rows(0).Item(0))
    End Function

    ' Preferencias
    Public Function obtenerPreferencia(idUsuario As Integer) As DataTable
        Return objMan.listarComando("select tema, fuente from usuario_preferencias where id_usuario=" & idUsuario)
    End Function

    Public Sub guardarPreferencia(idUsuario As Integer, tema As String, fuente As String)
        Dim dt As DataTable = obtenerPreferencia(idUsuario)
        If dt.Rows.Count > 0 Then
            objMan.ejecutarComando("update usuario_preferencias set tema='" & Esc(tema) & "', fuente='" & Esc(fuente) & "' where id_usuario=" & idUsuario)
        Else
            objMan.ejecutarComando("insert into usuario_preferencias (id_usuario, tema, fuente) values (" & idUsuario & ",'" & Esc(tema) & "','" & Esc(fuente) & "')")
        End If
    End Sub

    ' Mensaje destacados
    Public Function mensajeYaDescartado(idUsuario As Integer, idMensaje As Integer) As Boolean
        Dim dt As DataTable = objMan.listarComando("select * from usuario_mensajes_descartados where id_usuario=" & idUsuario & " and id_mensaje=" & idMensaje)
        Return dt.Rows.Count > 0
    End Function

    Public Function obtenerMensaje(idMensaje As Integer) As DataTable
        Return objMan.listarComando("select titulo, texto from tipos_mensaje where id_mensaje=" & idMensaje)
    End Function

    Public Function obtenerIDMensaje(cod As String) As Integer
        Dim dt As New DataTable
        strCon = "select id_mensaje from tipos_mensaje where codigo='" & cod & "'"
        dt = objMan.listarComando(strCon)
        Return Integer.Parse(dt.Rows(0).Item(0))
    End Function

    Public Sub descartarMensaje(idUsuario As Integer, idMensaje As Integer)
        If Not mensajeYaDescartado(idUsuario, idMensaje) Then
            objMan.ejecutarComando("insert into usuario_mensajes_descartados (id_usuario, id_mensaje) values (" & idUsuario & "," & idMensaje & ")")
        End If
    End Sub


    ' =========================================================
    ' Mantenimiento de usuarios (frmRegistrarUsuarios / frmListadoUsuarios)
    ' =========================================================

    Public Function listarUsuarios() As DataTable
        strCon = "select id_usuario, nombre, correo, contrasena, pregunta_seguridad, respuesta, rol, " &
                 "case when activo = 1 then 'ACTIVO' else 'INACTIVO' end as estado, fecha_creacion " &
                 "from usuarios order by nombre"
        Try
            Return objMan.listarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al listar usuarios!")
        End Try
    End Function

    Public Function obtenerUsuario(idUsuario As Integer) As DataTable
        strCon = "select id_usuario, nombre, correo, contrasena, pregunta_seguridad, respuesta, rol, activo " &
                 "from usuarios where id_usuario=" & idUsuario
        Try
            Return objMan.listarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al obtener datos del usuario!")
        End Try
    End Function

    ' idExcluir: al modificar se pasa el id del propio usuario para que no se compare consigo mismo
    Public Function existeNombre(nombre As String, idExcluir As Integer) As Boolean
        strCon = "select id_usuario from usuarios where nombre='" & Esc(nombre) & "' and id_usuario<>" & idExcluir
        Try
            Return objMan.listarComando(strCon).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar nombre de usuario!")
        End Try
    End Function

    Public Function existeCorreo(correo As String, idExcluir As Integer) As Boolean
        strCon = "select id_usuario from usuarios where correo='" & Esc(correo) & "' and id_usuario<>" & idExcluir
        Try
            Return objMan.listarComando(strCon).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar correo!")
        End Try
    End Function

    Public Sub registrarUsuario(nombre As String, correo As String, contrasena As String,
                                pregunta As String, respuesta As String, rol As String, activo As Boolean)
        strCon = "insert into usuarios (nombre, correo, contrasena, pregunta_seguridad, respuesta, rol, activo) values ('" &
                 Esc(nombre) & "','" & Esc(correo) & "','" & Esc(contrasena) & "','" & Esc(pregunta) & "','" &
                 Esc(respuesta) & "','" & Esc(rol) & "'," & If(activo, "1", "0") & ")"
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al registrar usuario!")
        End Try
    End Sub

    Public Sub modificarUsuario(idUsuario As Integer, nombre As String, correo As String, contrasena As String,
                                pregunta As String, respuesta As String, rol As String, activo As Boolean)
        strCon = "update usuarios set nombre='" & Esc(nombre) & "', correo='" & Esc(correo) &
                 "', contrasena='" & Esc(contrasena) & "', pregunta_seguridad='" & Esc(pregunta) &
                 "', respuesta='" & Esc(respuesta) & "', rol='" & Esc(rol) &
                 "', activo=" & If(activo, "1", "0") & " where id_usuario=" & idUsuario
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al modificar usuario!")
        End Try
    End Sub

    ' Un usuario con pedidos no se puede eliminar (llave foránea), solo dar de baja
    Public Function tienePedidos(idUsuario As Integer) As Boolean
        strCon = "select top 1 id_pedido from pedidos where id_usuario=" & idUsuario
        Try
            Return objMan.listarComando(strCon).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al verificar pedidos del usuario!")
        End Try
    End Function

    Public Sub eliminarUsuario(idUsuario As Integer)
        strCon = "delete from usuarios where id_usuario=" & idUsuario
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al eliminar usuario!")
        End Try
    End Sub

    ' activo = False -> dar de baja ; activo = True -> reactivar
    Public Sub cambiarEstadoUsuario(idUsuario As Integer, activo As Boolean)
        strCon = "update usuarios set activo=" & If(activo, "1", "0") & " where id_usuario=" & idUsuario
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al cambiar estado del usuario!")
        End Try
    End Sub

    ' Para el login: un usuario dado de baja no debe poder entrar
    Public Function usuarioActivo(usu As String) As Boolean
        strCon = "select id_usuario from usuarios where nombre='" & Esc(usu) & "' and activo=1"
        Try
            Return objMan.listarComando(strCon).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar estado del usuario!")
        End Try
    End Function
End Class

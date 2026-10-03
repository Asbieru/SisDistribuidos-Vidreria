Imports capaDatos
Public Class Usuario
    Dim objMan As New clsMantenimiento
    Dim strCon As String

    Public Function iniciarSesion(usu As String, con As String) As Boolean
        Dim dt As New DataTable
        strCon = "select * from usuarios where nombre='" & usu & "' and contrasena_hash='" & con & "'"
        Try
            dt = objMan.listarComando(strCon)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error al iniciar sesión!")
        End Try
    End Function

    Public Function validarNombreUsuario(usu As String) As Boolean
        Dim dt As New DataTable
        strCon = "select * from usuarios where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(strCon)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
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
        strCon = "select respuesta_hash from usuarios where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(strCon)
            If dt.Rows.Count > 0 AndAlso dt.Rows(0).Item(0).ToString.Equals(res) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw New Exception("Error al validar respuesta!")
        End Try
    End Function

    Public Sub cambiarContraseña(usu As String, con As String)
        strCon = "update usuarios set contrasena_hash='" & con & "' where nombre='" & usu & "'"
        Try
            objMan.ejecutarComando(strCon)
        Catch ex As Exception
            Throw New Exception("Error al modificar contraseña!")
        End Try
    End Sub

    ' Preferencias
    Public Function Obtener(idUsuario As Integer) As DataTable
        Return objMan.listarComando("select * from usuario_preferencias where id_usuario=" & idUsuario)
    End Function

    Public Sub Guardar(idUsuario As Integer, tema As String, fuente As String)
        Dim dt As DataTable = Obtener(idUsuario)
        If dt.Rows.Count > 0 Then
            objMan.ejecutarComando("update usuario_preferencias set tema='" & Esc(tema) & "', fuente='" & Esc(fuente) & "' where id_usuario=" & idUsuario)
        Else
            objMan.ejecutarComando("insert into usuario_preferencias (id_usuario, tema, fuente) values (" & idUsuario & ",'" & Esc(tema) & "','" & Esc(fuente) & "')")
        End If
    End Sub

    ' Mensaje destacados
    Public Function YaDescartado(idUsuario As Integer, idMensaje As Integer) As Boolean
        Dim dt As DataTable = objMan.listarComando("select * from usuario_mensajes_descartados where id_usuario=" & idUsuario & " and id_mensaje=" & idMensaje)
        Return dt.Rows.Count > 0
    End Function

    Public Sub Descartar(idUsuario As Integer, idMensaje As Integer)
        If Not YaDescartado(idUsuario, idMensaje) Then
            objMan.ejecutarComando("insert into usuario_mensajes_descartados (id_usuario, id_mensaje) values (" & idUsuario & "," & idMensaje & ")")
        End If
    End Sub
End Class

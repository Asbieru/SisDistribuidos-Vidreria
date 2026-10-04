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
        Dim dt As DataTable = Obtener(idUsuario)
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
End Class

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
End Class

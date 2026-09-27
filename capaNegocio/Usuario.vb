Imports capaDatos
Public Class Usuario
    Dim objConect As New clsMantenimiento
    Dim sql As String

    Public Function iniciarSesion(usu As String, con As String) As Boolean
        Dim dt As New DataTable
        sql = "select * from USUARIO where nombre='" & usu & "' and contraseña='" & con & "'"
        Try
            dt = objMan.listarComando(sql)
            Return dt.Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al inciar sesión!")
        End Try
    End Function

    Public Function validarNombreUsuario(usu As String) As Boolean
        Dim dt As New DataTable
        sql = "select * from USUARIO where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(sql)
            Return dt.Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar nombre de usuario!")
        End Try
    End Function

    Public Function obtenerPregunta(usu As String) As String
        Dim dt As New DataTable
        sql = "select pregunta from usuario where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(sql)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0).Item(0)
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw New Exception("Error al obtener pregunta secreta!")
        End Try
    End Function

    Public Function validarRespuesta(usu As String, res As String) As Boolean
        Dim dt As New DataTable
        sql = "select respuesta from usuario where nombre='" & usu & "'"
        Try
            dt = objMan.listarComando(sql)
            Return dt.Rows(0).Item(0).Equals(res)
        Catch ex As Exception
            Throw New Exception("Error al validar respuesta!")
        End Try
    End Function

    Public Sub cambiarContraseña(usu As String, con As String)
        sql = "update usuario set contraseña='" & con & "' where nombre='" & usu & "'"
        Try
            objMan.ejecutarComando(sql)
        Catch ex As Exception
            Throw New Exception("Error al modificar contraseña!")
        End Try
    End Sub
End Class

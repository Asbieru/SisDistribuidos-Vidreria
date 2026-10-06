Imports System.Data
Imports System.Data.SqlClient

Public Class clsMantenimiento
    Dim objConecta As New clsConectaBD
    Dim cm As New SqlCommand

    'Insert, update y delete
    Sub ejecutarComando(strConsulta As String)
        Try
            'objConecta.abrirconexionTrans()
            objConecta.abrirconexion()
            cm.Connection = objConecta.miConexion
            cm.CommandText = strConsulta
            'If transaccion = True Then
            '    cm.Transaction = tsql
            'End If
            cm.ExecuteNonQuery()
            'objConecta.cerrarconexionTrans()
            objConecta.cerrarconexion()
        Catch ex As Exception
            'objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la operación...")
        End Try
    End Sub

    'select
    Public Function listarComando(strConsulta As String) As DataTable
        Dim dt As New DataTable
        Dim da As SqlDataAdapter
        Try
            'objConecta.abrirconexionTrans()
            objConecta.abrirconexion()
            da = New SqlDataAdapter(strConsulta, objConecta.miConexion)
            'If transaccion = True Then
            '    da.SelectCommand.Transaction = tsql
            'End If
            da.Fill(dt)
            'objConecta.cerrarconexionTrans()
            objConecta.cerrarconexion()
            Return dt
        Catch ex As Exception
            objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la consulta...")
        End Try
        Return Nothing
    End Function

    Public Function listarComando(Optional strConsulta As String = "", Optional strConsulta1 As String = "") As DataSet
        Dim ds As New DataSet
        Dim da As SqlDataAdapter
        Try
            objConecta.abrirconexionTrans()
            da = New SqlDataAdapter(strConsulta, objConecta.miConexion)
            'If transaccion = True Then
            '    da.SelectCommand.Transaction = tsql
            'End If
            da.Fill(ds, "Tabla1")
            objConecta.cerrarconexionTrans()
            Return ds
        Catch ex As Exception
            objConecta.cancelarconexionTrans()
            Throw New Exception("Error al realizar la consulta...")
        End Try
        Return Nothing
    End Function

    '----------------------------------------------------------------
    ' Métodos con PARÁMETROS (SqlParameter): los valores viajan aparte del
    ' texto SQL, así que no hay inyección SQL ni problemas con comas/puntos
    ' decimales ni comillas. Parámetros: nombre sin "@" -> valor (Nothing = NULL)
    '----------------------------------------------------------------

    'select con parámetros. Ej: consultarConParametros("select * from x where id=@id", New Dictionary(Of String, Object) From {{"id", 5}})
    Public Function consultarConParametros(strConsulta As String, Optional parametros As Dictionary(Of String, Object) = Nothing) As DataTable
        Return ejecutarConsulta(strConsulta, CommandType.Text, parametros)
    End Function

    'Ejecuta un procedimiento almacenado que devuelve filas (ej: el id generado, los retazos)
    Public Function listarProcedimiento(nombreProcedimiento As String, Optional parametros As Dictionary(Of String, Object) = Nothing) As DataTable
        Return ejecutarConsulta(nombreProcedimiento, CommandType.StoredProcedure, parametros)
    End Function

    'Ejecuta un procedimiento almacenado que no devuelve filas
    Public Sub ejecutarProcedimiento(nombreProcedimiento As String, Optional parametros As Dictionary(Of String, Object) = Nothing)
        ejecutarConsulta(nombreProcedimiento, CommandType.StoredProcedure, parametros)
    End Sub

    'insert, update y delete con parámetros
    Public Sub ejecutarConParametros(strConsulta As String, Optional parametros As Dictionary(Of String, Object) = Nothing)
        ejecutarConsulta(strConsulta, CommandType.Text, parametros)
    End Sub

    Private Function ejecutarConsulta(texto As String, tipo As CommandType, parametros As Dictionary(Of String, Object)) As DataTable
        Dim dt As New DataTable
        Try
            objConecta.abrirconexion()
            Using cmd As New SqlCommand(texto, objConecta.miConexion)
                cmd.CommandType = tipo
                If parametros IsNot Nothing Then
                    For Each par In parametros
                        cmd.Parameters.AddWithValue("@" & par.Key, If(par.Value, DBNull.Value))
                    Next
                End If
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
            Return dt
        Catch ex As SqlException When ex.Number >= 50000
            'Error de negocio lanzado con THROW desde la BD (ej: "Stock insuficiente..."): se muestra tal cual
            Throw New Exception(ex.Message)
        Catch ex As Exception
            Throw New Exception("Error al realizar la operación en la BD: " & ex.Message)
        Finally
            objConecta.cerrarconexion()
        End Try
    End Function
End Class

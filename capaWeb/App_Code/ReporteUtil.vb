Imports System.Collections.Generic
Imports System.Data
Imports System.Web

' Utilidades compartidas por las paginas de reportes.
Public NotInheritable Class ReporteUtil

    Private Sub New()
    End Sub

    ' Fecha local de Peru (UTC-5), igual que la muestra wfInicio.
    Public Shared Function HoyPeru() As Date
        Try
            Dim zonaPeru As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time")
            Return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaPeru)
        Catch
            Return DateTime.Now
        End Try
    End Function

    ' Verifica REPORTES en los permisos directos; si el usuario no tiene permisos
    ' propios, usa los de su rol, igual que plantilla.master.vb.
    Public Shared Function PuedeVerReportes(idUsuario As Integer) As Boolean
        Try
            Dim lista As List(Of String) = New Permiso().codigosPermisoUsuario(idUsuario)
            If lista Is Nothing Then lista = New List(Of String)()

            If lista.Count = 0 AndAlso HttpContext.Current IsNot Nothing AndAlso
               HttpContext.Current.Session IsNot Nothing Then
                Dim rol As String = Convert.ToString(HttpContext.Current.Session("rol"))
                If Not String.IsNullOrWhiteSpace(rol) Then
                    For Each fila As DataRow In New Permiso().listarPermisosRol(rol).Rows
                        lista.Add(fila("codigo").ToString())
                    Next
                End If
            End If

            Return lista.Contains("REPORTES")
        Catch
            Return False
        End Try
    End Function

End Class

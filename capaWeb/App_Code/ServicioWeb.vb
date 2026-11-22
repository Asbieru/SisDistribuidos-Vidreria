Imports System.Data
Imports System.Collections.Generic
Imports System.Web.Script.Serialization

' Punto unico de instanciacion del proxy generado por la referencia de servicio
' "libreriaVidrieria". Ningun formulario debe crear libreriaVidrieria.WSLibreriasSoapClient.
Public NotInheritable Class ServicioWeb

    Private Sub New()
    End Sub

    Public Shared Function Ejecutar(clase As String, metodo As String, parametros As Object()) As String
        Dim cliente As New libreriaVidrieria.WSLibreriasSoapClient("WSLibreriasSoap")
        Try
            Dim jsonParametros As String = "[]"
            If parametros IsNot Nothing AndAlso parametros.Length > 0 Then
                jsonParametros = New JavaScriptSerializer().Serialize(parametros)
            End If
            Return cliente.Ejecutar(clase, metodo, jsonParametros)
        Catch ex As System.ServiceModel.FaultException
            Throw New Exception(ex.Message)
        Catch ex As System.ServiceModel.CommunicationException
            Throw New Exception("No se pudo conectar con el servicio web. " & ex.Message)
        Catch ex As System.TimeoutException
            Throw New Exception("El servicio web no respondio a tiempo. " & ex.Message)
        Finally
            Try
                If cliente IsNot Nothing AndAlso cliente.State <> System.ServiceModel.CommunicationState.Closed Then
                    cliente.Close()
                End If
            Catch
                Try
                    cliente.Abort()
                Catch
                End Try
            End Try
        End Try
    End Function

    Public Shared Function Valor(Of T)(json As String) As T
        If String.IsNullOrEmpty(json) OrElse json = "null" Then Return Nothing
        Return New JavaScriptSerializer().Deserialize(Of T)(json)
    End Function

    Public Shared Function Tabla(json As String) As DataTable
        Dim dt As New DataTable()
        If String.IsNullOrEmpty(json) OrElse json = "[]" OrElse json = "null" Then Return dt

        Dim filas As Object() = TryCast(New JavaScriptSerializer().DeserializeObject(json), Object())
        If filas Is Nothing Then Return dt

        For Each fila As Object In filas
            Dim d As Dictionary(Of String, Object) = TryCast(fila, Dictionary(Of String, Object))
            If d Is Nothing Then Continue For
            For Each kv As KeyValuePair(Of String, Object) In d
                If Not dt.Columns.Contains(kv.Key) Then
                    dt.Columns.Add(kv.Key, TipoColumna(kv.Key, kv.Value))
                End If
            Next
        Next

        For Each fila As Object In filas
            Dim d As Dictionary(Of String, Object) = TryCast(fila, Dictionary(Of String, Object))
            If d Is Nothing Then Continue For
            Dim r As DataRow = dt.NewRow()
            For Each kv As KeyValuePair(Of String, Object) In d
                If Not dt.Columns.Contains(kv.Key) Then Continue For
                If kv.Value Is Nothing Then
                    r(kv.Key) = DBNull.Value
                ElseIf dt.Columns(kv.Key).DataType Is GetType(Byte()) Then
                    r(kv.Key) = ABytes(kv.Value)
                Else
                    r(kv.Key) = Convert.ChangeType(kv.Value, dt.Columns(kv.Key).DataType,
                                                   Globalization.CultureInfo.InvariantCulture)
                End If
            Next
            dt.Rows.Add(r)
        Next
        Return dt
    End Function

    ' JavaScriptSerializer serializa Byte() como arreglo de numeros
    ' ([0,0,0,0,0,0,19,255]), no como base64. Acepta ambos formatos.
    Public Shared Function ABytes(valor As Object) As Byte()
        If valor Is Nothing Then Return Nothing
        If TypeOf valor Is Byte() Then Return DirectCast(valor, Byte())
        If TypeOf valor Is String Then Return Convert.FromBase64String(DirectCast(valor, String))

        Dim arreglo As Object() = TryCast(valor, Object())
        If arreglo IsNot Nothing Then
            Dim bytes(arreglo.Length - 1) As Byte
            For i As Integer = 0 To arreglo.Length - 1
                bytes(i) = Convert.ToByte(arreglo(i), Globalization.CultureInfo.InvariantCulture)
            Next
            Return bytes
        End If

        Throw New Exception("No se pudo convertir el valor a Byte().")
    End Function

    Public Shared Function FilasComoListas(dt As DataTable) As List(Of Dictionary(Of String, Object))
        Dim lista As New List(Of Dictionary(Of String, Object))
        If dt Is Nothing Then Return lista
        For Each fila As DataRow In dt.Rows
            Dim d As New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                If fila(col) Is DBNull.Value Then
                    d(col.ColumnName) = Nothing
                Else
                    d(col.ColumnName) = fila(col)
                End If
            Next
            lista.Add(d)
        Next
        Return lista
    End Function

    Private Shared Function TipoColumna(nombre As String, valor As Object) As Type
        If nombre.IndexOf("version", StringComparison.OrdinalIgnoreCase) >= 0 Then Return GetType(Byte())
        If valor Is Nothing Then Return GetType(String)
        Dim t As Type = valor.GetType()
        Select Case t.FullName
            Case "System.Int32", "System.Int64", "System.Int16", "System.Byte", "System.Single",
                 "System.Double", "System.Decimal"
                Return GetType(Decimal)
            Case Else
                Return t
        End Select
    End Function

End Class

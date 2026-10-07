Option Strict On
Imports capaDatos
Imports System.Data
Imports System.Collections.Generic

Public Class Cliente
    Private ReadOnly objMan As New clsMantenimiento

    ' Se conservan las firmas originales para las llamadas del equipo.
    Public Function Listar() As DataTable
        Return objMan.listarProcedimiento("dbo.sp_cli_listar")
    End Function

    Public Function Buscar(texto As String) As DataTable
        Dim filtro As String = Normalizar(texto)
        ValidarLongitud(filtro, 150, "La búsqueda")
        Return objMan.listarProcedimiento("dbo.sp_cli_listar",
            New Dictionary(Of String, Object) From {{"texto", filtro}})
    End Function

    Public Function Obtener(idCliente As Integer) As DataTable
        ValidarID(idCliente)
        Return objMan.listarProcedimiento("dbo.sp_cli_obtener",
            New Dictionary(Of String, Object) From {{"id_cliente", idCliente}})
    End Function

    Public Function Insertar(nombre As String, documento As String,
                             telefono As String, direccion As String) As Integer
        Return Guardar(0, nombre, documento, telefono, direccion)
    End Function

    Public Sub Actualizar(idCliente As Integer, nombre As String,
                          documento As String, telefono As String, direccion As String)
        ValidarID(idCliente)
        Guardar(idCliente, nombre, documento, telefono, direccion)
    End Sub

    Public Function TienePedidos(idCliente As Integer) As Boolean
        ValidarID(idCliente)
        Dim dt As DataTable = objMan.listarProcedimiento("dbo.sp_cli_tiene_pedidos",
            New Dictionary(Of String, Object) From {{"id_cliente", idCliente}})
        Return CBool(dt.Rows(0)("tiene_pedidos"))
    End Function

    Public Sub Eliminar(idCliente As Integer)
        ValidarID(idCliente)
        ' La BD vuelve a comprobar pedidos dentro de la transacción.
        objMan.ejecutarProcedimiento("dbo.sp_cli_eliminar",
            New Dictionary(Of String, Object) From {{"id_cliente", idCliente}})
    End Sub

    Private Function Guardar(idCliente As Integer, nombre As String,
                             documento As String, telefono As String,
                             direccion As String) As Integer
        nombre = Normalizar(nombre)
        documento = Normalizar(documento)
        telefono = Normalizar(telefono)
        direccion = Normalizar(direccion)
        If nombre Is Nothing Then Throw New Exception("Ingrese el nombre del cliente.")
        ValidarLongitud(nombre, 150, "El nombre")
        ValidarLongitud(documento, 20, "El documento")
        ValidarLongitud(telefono, 20, "El teléfono")
        ValidarLongitud(direccion, 255, "La dirección")
        Dim dt As DataTable = objMan.listarProcedimiento("dbo.sp_cli_guardar",
            New Dictionary(Of String, Object) From {
                {"id_cliente", idCliente}, {"nombre", nombre},
                {"documento", documento}, {"telefono", telefono},
                {"direccion", direccion}})
        Return CInt(dt.Rows(0)("id_cliente"))
    End Function

    Private Shared Function Normalizar(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return Nothing
        Return texto.Trim()
    End Function

    Private Shared Sub ValidarLongitud(texto As String, limite As Integer, campo As String)
        If texto IsNot Nothing AndAlso texto.Length > limite Then
            Throw New Exception(campo & " admite hasta " & limite.ToString() & " caracteres.")
        End If
    End Sub

    Private Shared Sub ValidarID(idCliente As Integer)
        If idCliente <= 0 Then Throw New Exception("Seleccione un cliente válido.")
    End Sub
End Class

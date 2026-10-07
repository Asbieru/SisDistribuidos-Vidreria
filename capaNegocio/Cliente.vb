Option Strict On
Imports capaDatos
Imports System.Data
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Linq

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
        ' Compatibilidad con llamadas antiguas. Los formularios deben pasar
        ' la version que leyeron al abrirse mediante la sobrecarga siguiente.
        Dim dt As DataTable = Obtener(idCliente)
        If dt.Rows.Count = 0 Then Throw New Exception("El cliente ya no existe. Actualice el listado.")
        Actualizar(idCliente, nombre, documento, telefono, direccion,
                   DirectCast(dt.Rows(0)("version_cliente"), Byte()))
    End Sub

    Public Sub Actualizar(idCliente As Integer, nombre As String,
                          documento As String, telefono As String, direccion As String,
                          versionOriginal As Byte())
        ValidarID(idCliente)
        If versionOriginal Is Nothing OrElse versionOriginal.Length <> 8 Then
            Throw New Exception("Recargue el cliente antes de guardar sus cambios.")
        End If
        Guardar(idCliente, nombre, documento, telefono, direccion, versionOriginal)
    End Sub

    Public Function DocumentoCoincidente(documento As String,
                                         Optional idExcluir As Integer = 0) As DataTable
        documento = Normalizar(documento)
        ValidarLongitud(documento, 20, "El documento")
        If idExcluir < 0 Then Throw New Exception("El ID del cliente no es válido.")
        Return objMan.listarProcedimiento("dbo.sp_cli_documento_coincidente",
            New Dictionary(Of String, Object) From {
                {"documento", documento}, {"id_excluir", idExcluir}})
    End Function

    Public Function Historial(idCliente As Integer, Optional estado As String = Nothing) As DataTable
        ValidarID(idCliente)
        estado = Normalizar(estado)
        If estado IsNot Nothing Then
            estado = estado.ToUpperInvariant()
            If Not {"PENDIENTE", "PARCIAL", "PAGADO", "CANCELADO"}.Contains(estado) Then
                Throw New Exception("Seleccione un estado válido.")
            End If
        End If
        Return objMan.listarProcedimiento("dbo.sp_cli_historial",
            New Dictionary(Of String, Object) From {{"id_cliente", idCliente}, {"estado", estado}})
    End Function

    Public Function PagosDePedido(idCliente As Integer, idPedido As Integer) As DataTable
        ValidarID(idCliente)
        If idPedido <= 0 Then Throw New Exception("Seleccione un pedido válido.")
        Return objMan.listarProcedimiento("dbo.sp_cli_pagos_pedido",
            New Dictionary(Of String, Object) From {{"id_cliente", idCliente}, {"id_pedido", idPedido}})
    End Function

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
                             direccion As String, Optional versionOriginal As Byte() = Nothing) As Integer
        nombre = Normalizar(nombre)
        documento = Normalizar(documento)
        telefono = Normalizar(telefono)
        direccion = Normalizar(direccion)
        If nombre Is Nothing Then Throw New Exception("Ingrese el nombre del cliente.")
        ValidarLongitud(nombre, 150, "El nombre")
        ValidarLongitud(documento, 20, "El documento")
        ValidarLongitud(telefono, 20, "El teléfono")
        ValidarLongitud(direccion, 255, "La dirección")
        If telefono IsNot Nothing AndAlso
           (Not Regex.IsMatch(telefono, "^[0-9+() \-]+$") OrElse Not Regex.IsMatch(telefono, "[0-9]")) Then
            Throw New Exception("El teléfono debe contener números; puede incluir espacios, +, paréntesis y guiones.")
        End If
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_cliente", idCliente}, {"nombre", nombre},
            {"documento", documento}, {"telefono", telefono}, {"direccion", direccion}}
        ' En altas se omite el parametro binario para usar su NULL SQL por defecto.
        ' La utilidad compartida infiere un tipo textual cuando recibe Nothing.
        If versionOriginal IsNot Nothing Then parametros.Add("version_original", versionOriginal)
        Dim dt As DataTable = objMan.listarProcedimiento("dbo.sp_cli_guardar", parametros)
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

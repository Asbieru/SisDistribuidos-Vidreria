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
                             telefono As String, direccion As String,
                             Optional tipoPersona As String = Nothing,
                             Optional tipoDocumento As String = Nothing) As Integer
        Return Guardar(0, nombre, documento, telefono, direccion, Nothing, tipoPersona, tipoDocumento)
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
                          versionOriginal As Byte(), Optional tipoPersona As String = Nothing,
                          Optional tipoDocumento As String = Nothing)
        ValidarID(idCliente)
        If versionOriginal Is Nothing OrElse versionOriginal.Length <> 8 Then
            Throw New Exception("Recargue el cliente antes de guardar sus cambios.")
        End If
        If tipoPersona Is Nothing AndAlso tipoDocumento Is Nothing Then
            Dim actual As DataTable = Obtener(idCliente)
            If actual.Rows.Count = 0 Then Throw New Exception("El cliente ya no existe. Actualice el listado.")
            tipoPersona = Normalizar(actual.Rows(0)("tipo_persona").ToString())
            tipoDocumento = Normalizar(actual.Rows(0)("tipo_documento").ToString())
        End If
        Guardar(idCliente, nombre, documento, telefono, direccion, versionOriginal, tipoPersona, tipoDocumento)
    End Sub

    Public Function DocumentoCoincidente(documento As String,
                                         Optional idExcluir As Integer = 0,
                                         Optional tipoDocumento As String = Nothing) As DataTable
        documento = Normalizar(documento)
        ValidarLongitud(documento, 20, "El documento")
        If idExcluir < 0 Then Throw New Exception("El ID del cliente no es válido.")
        Return objMan.listarProcedimiento("dbo.sp_cli_documento_coincidente",
            New Dictionary(Of String, Object) From {
                {"documento", documento}, {"id_excluir", idExcluir}, {"tipo_documento", tipoDocumento}})
    End Function

    Public Function Historial(idCliente As Integer, Optional estado As String = Nothing,
                              Optional desde As Date? = Nothing, Optional hasta As Date? = Nothing) As DataTable
        ValidarID(idCliente)
        estado = Normalizar(estado)
        If estado IsNot Nothing Then
            estado = estado.ToUpperInvariant()
            If Not {"PENDIENTE", "PARCIAL", "PAGADO", "CANCELADO"}.Contains(estado) Then
                Throw New Exception("Seleccione un estado válido.")
            End If
        End If
        If desde.HasValue AndAlso hasta.HasValue AndAlso desde.Value.Date > hasta.Value.Date Then
            Throw New ArgumentException("La fecha Desde no puede ser posterior a Hasta.", "desde")
        End If
        Dim parametros As New Dictionary(Of String, Object) From {{"id_cliente", idCliente}, {"estado", estado}}
        If desde.HasValue Then parametros.Add("desde", desde.Value.Date)
        If hasta.HasValue Then parametros.Add("hasta", hasta.Value.Date)
        Return objMan.listarProcedimiento("dbo.sp_cli_historial", parametros)
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
                             direccion As String, Optional versionOriginal As Byte() = Nothing,
                             Optional tipoPersona As String = Nothing, Optional tipoDocumento As String = Nothing) As Integer
        nombre = Normalizar(nombre)
        documento = Normalizar(documento)
        telefono = Normalizar(telefono)
        direccion = Normalizar(direccion)
        tipoPersona = If(Normalizar(tipoPersona), "").ToUpperInvariant()
        tipoDocumento = If(Normalizar(tipoDocumento), "").ToUpperInvariant()
        ValidarDatos(nombre, documento, telefono, direccion, tipoPersona, tipoDocumento)
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_cliente", idCliente}, {"nombre", nombre},
            {"documento", documento}, {"telefono", telefono}, {"direccion", direccion},
            {"tipo_persona", Normalizar(tipoPersona)}, {"tipo_documento", Normalizar(tipoDocumento)}}
        ' En altas se omite el parametro binario para usar su NULL SQL por defecto.
        ' La utilidad compartida infiere un tipo textual cuando recibe Nothing.
        If versionOriginal IsNot Nothing Then parametros.Add("version_original", versionOriginal)
        Dim dt As DataTable = objMan.listarProcedimiento("dbo.sp_cli_guardar", parametros)
        Return CInt(dt.Rows(0)("id_cliente"))
    End Function

    Public Sub ValidarDatos(nombre As String, documento As String, telefono As String,
                            direccion As String, tipoPersona As String, tipoDocumento As String)
        nombre = Normalizar(nombre)
        documento = Normalizar(documento)
        telefono = Normalizar(telefono)
        tipoPersona = If(Normalizar(tipoPersona), "").ToUpperInvariant()
        tipoDocumento = If(Normalizar(tipoDocumento), "").ToUpperInvariant()
        If nombre Is Nothing Then Throw New ArgumentException("Ingrese el nombre o razón social del cliente.", "nombre")
        If nombre.Length > 150 Then Throw New ArgumentException("El nombre admite hasta 150 caracteres.", "nombre")
        If documento IsNot Nothing AndAlso documento.Length > 20 Then Throw New ArgumentException("El documento admite hasta 20 caracteres.", "documento")
        If telefono IsNot Nothing AndAlso telefono.Length > 20 Then Throw New ArgumentException("El teléfono admite hasta 20 caracteres.", "telefono")
        If direccion IsNot Nothing AndAlso direccion.Trim().Length > 255 Then Throw New ArgumentException("La dirección admite hasta 255 caracteres.", "direccion")

        ' Sin tipos: compatibilidad con registros y llamadas anteriores.
        If tipoPersona <> "" OrElse tipoDocumento <> "" Then
            If tipoPersona <> "NATURAL" AndAlso tipoPersona <> "JURIDICA" Then
                Throw New ArgumentException("Seleccione el tipo de persona.", "tipoPersona")
            End If
            If Not {"DNI", "RUC", "SIN DOCUMENTO"}.Contains(tipoDocumento) Then
                Throw New ArgumentException("Seleccione el tipo de documento.", "tipoDocumento")
            End If
            If tipoPersona = "JURIDICA" AndAlso tipoDocumento <> "RUC" Then
                Throw New ArgumentException("La persona jurídica requiere RUC.", "tipoDocumento")
            End If
            Select Case tipoDocumento
                Case "DNI"
                    If documento Is Nothing OrElse Not Regex.IsMatch(documento, "\A[0-9]{8}\z") Then
                        Throw New ArgumentException("El DNI debe contener exactamente 8 dígitos.", "documento")
                    End If
                Case "RUC"
                    If documento Is Nothing OrElse Not Regex.IsMatch(documento, "\A[0-9]{11}\z") Then
                        Throw New ArgumentException("El RUC debe contener exactamente 11 dígitos.", "documento")
                    End If
                    If (tipoPersona = "JURIDICA" AndAlso Not documento.StartsWith("20")) OrElse
                       (tipoPersona = "NATURAL" AndAlso Not documento.StartsWith("10") AndAlso Not documento.StartsWith("15")) Then
                        Throw New ArgumentException("El RUC debe empezar por 20 para persona jurídica, o por 10 o 15 para natural.", "documento")
                    End If
                Case "SIN DOCUMENTO"
                    If documento IsNot Nothing Then Throw New ArgumentException("Deje el número vacío si selecciona Sin documento.", "documento")
            End Select
        End If
        If telefono IsNot Nothing AndAlso
           (Not Regex.IsMatch(telefono, "\A\+?[0-9() \-]+\z") OrElse
            Regex.Matches(telefono, "[0-9]").Count < 7 OrElse Regex.Matches(telefono, "[0-9]").Count > 15) Then
            Throw New ArgumentException("El teléfono debe contener entre 7 y 15 dígitos. El + solo se permite al inicio.", "telefono")
        End If
        If telefono IsNot Nothing AndAlso
           (Regex.Matches(telefono, "\(").Count <> Regex.Matches(telefono, "\)").Count OrElse
            Regex.Matches(telefono, "\(").Count > 1 OrElse
            (telefono.Contains("(") AndAlso telefono.IndexOf("(") > telefono.IndexOf(")"))) Then
            Throw New ArgumentException("Revise los paréntesis del teléfono; se permite un par alrededor del código de área.", "telefono")
        End If
    End Sub

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

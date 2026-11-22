Imports System.ComponentModel
Imports System.Data
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports System.Reflection
Imports capaNegocio

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la linea siguiente.
' <System.Web.Script.Services.ScriptService()> _

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WSLibrerias
    Inherits System.Web.Services.WebService

    Private ReadOnly objUsuario As New Usuario()
    Private ReadOnly objCliente As New Cliente()
    Private ReadOnly objProveedor As New Proveedor()
    Private ReadOnly objProducto As New Producto()
    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objPago As New Pago()
    Private ReadOnly objMetodoPago As New MetodoPago()
    Private ReadOnly objInventario As New Inventario()
    Private ReadOnly objVariante As New ProductoVariante()
    Private ReadOnly objPermiso As New Permiso()

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hola a todos"
    End Function

    ' ============================================================
    ' SESION
    ' ============================================================

    <WebMethod()> _
    Public Function IniciarSesion(usu As String, con As String) As String
        Dim ok As Boolean = False
        Dim idUsuario As Integer = 0
        Try
            ok = objUsuario.IniciarSesion(usu, con)
            If ok Then
                idUsuario = objUsuario.ObtenerIDUsuario(usu)
            End If
        Catch ex As Exception
            ok = False
        End Try
        Dim datos As New Dictionary(Of String, Object)
        datos.Add("ok", ok)
        datos.Add("idUsuario", idUsuario)
        datos.Add("usuario", If(usu, String.Empty))
        Return JsonObjeto(datos)
    End Function

    <WebMethod()> _
    Public Function CambiarContrasena(usu As String, con As String) As Boolean
        Try
            objUsuario.cambiarContraseña(usu, con)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function ObtenerUsuario(idUsuario As Integer) As String
        Return JsonDataTable(objUsuario.ObtenerUsuario(idUsuario))
    End Function

    ' ============================================================
    ' CLIENTES
    ' ============================================================

    <WebMethod()> _
    Public Function ListarClientes(texto As String) As String
        Return JsonDataTable(objCliente.Buscar(texto))
    End Function

    <WebMethod()> _
    Public Function ObtenerCliente(idCliente As Integer) As String
        Return JsonDataTable(objCliente.Obtener(idCliente))
    End Function

    <WebMethod()> _
    Public Function InsertarCliente(nombre As String, documento As String, telefono As String, direccion As String, tipoPersona As String, tipoDocumento As String) As Integer
        Return objCliente.Insertar(nombre, documento, telefono, direccion, tipoPersona, tipoDocumento)
    End Function

    <WebMethod()> _
    Public Function ActualizarCliente(idCliente As Integer, nombre As String, documento As String, telefono As String, direccion As String, versionOriginal As Byte(), tipoPersona As String, tipoDocumento As String) As Boolean
        Try
            objCliente.Actualizar(idCliente, nombre, documento, telefono, direccion, versionOriginal, tipoPersona, tipoDocumento)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function EliminarCliente(idCliente As Integer) As Boolean
        Try
            objCliente.Eliminar(idCliente)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function TienePedidos(idCliente As Integer) As Boolean
        Return objCliente.TienePedidos(idCliente)
    End Function

    ' ============================================================
    ' PROVEEDORES
    ' ============================================================

    <WebMethod()> _
    Public Function ListarProveedores(texto As String) As String
        Return JsonDataTable(objProveedor.Buscar(texto))
    End Function

    <WebMethod()> _
    Public Function ObtenerProveedor(idProveedor As Integer) As String
        Return JsonDataTable(objProveedor.Obtener(idProveedor))
    End Function

    <WebMethod()> _
    Public Function InsertarProveedor(nombre As String, documento As String, telefono As String, direccion As String, tipoPersona As String, tipoDocumento As String) As Integer
        Return objProveedor.Insertar(nombre, documento, telefono, direccion, tipoPersona, tipoDocumento)
    End Function

    <WebMethod()> _
    Public Function ActualizarProveedor(idProveedor As Integer, nombre As String, documento As String, telefono As String, direccion As String, versionOriginal As Byte(), tipoPersona As String, tipoDocumento As String) As Boolean
        Try
            objProveedor.Actualizar(idProveedor, nombre, documento, telefono, direccion, versionOriginal, tipoPersona, tipoDocumento)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function EliminarProveedor(idProveedor As Integer) As Boolean
        Try
            objProveedor.Eliminar(idProveedor)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function TieneCompras(idProveedor As Integer) As Boolean
        Return objProveedor.TieneCompras(idProveedor)
    End Function

    ' ============================================================
    ' PRODUCTOS
    ' ============================================================

    <WebMethod()> _
    Public Function ListarProductos(categoria As String) As String
        Return JsonDataTable(objProducto.ListarPorCategoria(categoria))
    End Function

    <WebMethod()> _
    Public Function ObtenerProducto(idProducto As Integer) As String
        Return JsonDataTable(objProducto.Obtener(idProducto))
    End Function

    <WebMethod()> _
    Public Function InsertarProducto(categoria As String, tipo As String, color As String, nombre As String, descripcion As String, imagenUrl As String) As Integer
        Return objProducto.Insertar(categoria, tipo, color, nombre, descripcion, imagenUrl)
    End Function

    <WebMethod()> _
    Public Function ActualizarProducto(idProducto As Integer, categoria As String, tipo As String, color As String, nombre As String, descripcion As String, imagenUrl As String) As Boolean
        Try
            objProducto.Actualizar(idProducto, categoria, tipo, color, nombre, descripcion, imagenUrl)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function InactivarProducto(idProducto As Integer) As Boolean
        Try
            objProducto.Inactivar(idProducto)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function TieneVariantes(idProducto As Integer) As Boolean
        Return objProducto.TieneVariantes(idProducto)
    End Function

    ' ============================================================
    ' USUARIOS Y PERMISOS
    ' ============================================================

    <WebMethod()> _
    Public Function ListarUsuarios() As String
        Return JsonDataTable(objUsuario.listarUsuarios())
    End Function

    <WebMethod()> _
    Public Function InsertarUsuario(nombre As String, correo As String, contrasena As String, pregunta As String, respuesta As String, rol As String, activo As Boolean) As Boolean
        Try
            objUsuario.registrarUsuario(nombre, correo, contrasena, pregunta, respuesta, rol, activo)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function ActualizarUsuario(idUsuario As Integer, nombre As String, correo As String, contrasena As String, pregunta As String, respuesta As String, rol As String, activo As Boolean) As Boolean
        Try
            objUsuario.modificarUsuario(idUsuario, nombre, correo, contrasena, pregunta, respuesta, rol, activo)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function CambiarEstadoUsuario(idUsuario As Integer, activo As Boolean) As Boolean
        Try
            objUsuario.cambiarEstadoUsuario(idUsuario, activo)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function EsUltimoAdministrador(idUsuario As Integer) As Boolean
        Return objUsuario.esUltimoAdministrador(idUsuario)
    End Function

    <WebMethod()> _
    Public Function ListarPermisosRol(rol As String) As String
        Return JsonDataTable(objPermiso.listarPermisosRol(rol))
    End Function

    ' ============================================================
    ' PEDIDOS Y PAGOS
    ' ============================================================

    <WebMethod()> _
    Public Function ObtenerPedido(idPedido As Integer) As String
        Return JsonDataTable(objPedido.Obtener(idPedido))
    End Function

    <WebMethod()> _
    Public Function ListarPedidos() As String
        Return JsonDataTable(objPedido.Listar())
    End Function

    <WebMethod()> _
    Public Function ActualizarEstadoPago(idPedido As Integer) As Boolean
        Try
            objPedido.ActualizarEstadoPago(idPedido)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function ListarPagos(idPedido As Integer) As String
        Return JsonDataTable(objPago.ListarPorPedido(idPedido))
    End Function

    <WebMethod()> _
    Public Function RegistrarPago(idPedido As Integer, idMetodoPago As Integer, monto As Decimal) As Boolean
        Try
            objPago.RegistrarDetalle(idPedido, idMetodoPago, monto)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function ActualizarPago(idDetallePago As Integer, idMetodoPago As Integer, monto As Decimal) As Boolean
        Try
            objPago.ActualizarDetalle(idDetallePago, idMetodoPago, monto)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function EliminarPago(idDetallePago As Integer) As Boolean
        Try
            objPago.EliminarDetalle(idDetallePago)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <WebMethod()> _
    Public Function TotalPagado(idPedido As Integer) As Decimal
        Return objPago.TotalPagado(idPedido)
    End Function

    <WebMethod()> _
    Public Function UltimoMonto(idPedido As Integer) As Decimal
        Return objPago.UltimoMonto(idPedido)
    End Function

    <WebMethod()> _
    Public Function ListarMetodosPago() As String
        Return JsonDataTable(objMetodoPago.Listar())
    End Function

    ' ============================================================
    ' INVENTARIO
    ' ============================================================

    <WebMethod()> _
    Public Function ListarVariantes(porPiezas As Boolean) As String
        Return JsonDataTable(objInventario.ListarVariantes(porPiezas))
    End Function

    <WebMethod()> _
    Public Function ListarResumenInventario(categoria As String) As String
        Return JsonDataTable(objInventario.ListarResumen(categoria))
    End Function

    ' ============================================================
    ' FIN
    ' ============================================================

    Private Function JsonDataTable(dt As DataTable) As String
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return "[]"
        Dim lista As New List(Of Dictionary(Of String, Object))
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim fila As DataRow = dt.Rows(i)
            Dim item As New Dictionary(Of String, Object)
            For c As Integer = 0 To dt.Columns.Count - 1
                Dim nombre As String = dt.Columns(c).ColumnName
                Dim valor As Object = Nothing
                If Not IsDBNull(fila(nombre)) Then
                    valor = fila(nombre)
                End If
                item(nombre) = valor
            Next
            lista.Add(item)
        Next
        Return New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lista)
    End Function

    Private Function JsonObjeto(datos As Dictionary(Of String, Object)) As String
        Return New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(datos)
    End Function


    ' ============================================================
    ' INVOCACION GENERICA USADA POR capaWeb (sin DLLs en Bin)
    ' ============================================================

    Private Shared ReadOnly MetodosPermitidos As New Dictionary(Of String, String) From {
        {"Usuario", "iniciarSesion,validarNombreUsuario,obtenerPregunta,validarRespuesta,cambiarContraseña,obtenerIDUsuario,obtenerPreferencia,guardarPreferencia,listarUsuarios,obtenerUsuario,existeNombre,existeCorreo,registrarUsuario,modificarUsuario,eliminarUsuario,cambiarEstadoUsuario,usuarioActivo,esUltimoAdministrador"},
        {"Cliente", "Listar,Buscar,Obtener,Insertar,Actualizar,DocumentoCoincidente,Historial,PagosDePedido,TienePedidos,Eliminar,ValidarDatos"},
        {"Proveedor", "Listar,Buscar,Obtener,Insertar,Actualizar,DocumentoCoincidente,TieneCompras,Eliminar,ValidarDatos"},
        {"Producto", "Listar,ListarPorCategoria,ListarTodos,Obtener,ObtenerIdPorTipoColor,ExisteProducto,Insertar,Actualizar,Eliminar,CambiarEstado,Inactivar,TieneVariantes,TarifaSugerida"},
        {"Pedido", "Obtener,Listar,ListarDetallado,CambiarEstado,ActualizarEstadoPago,ListarItems,ListarDetalles,ListarItemsDetallado,ListarDetallesDeItem,ResolverDetalle,MarcarItemRoto,CancelarItem,Cancelar,Terminar,TerminarItem,RecalcularEstados,TarifaSugerida,Registrar"},
        {"Pago", "ListarPorPedido,ListarPorPago,ListarDetallesPorPedido,RegistrarDetalle,TotalPagado,UltimoMonto,ActualizarDetalle,EliminarDetalle,Insertar,InsertarDetallePago"},
        {"Compra", "Listar,ListarPorFecha,ObtenerCompra,ObtenerDetalleParaEditar,GuardarCompra,EliminarCompra,CalcularImportes"},
        {"Inventario", "ListarResumen,ContarBajoMinimo,ListarVariantes,ListarTodasLasVariantes,ActualizarStockMinimo,ListarMovimientos,EsVidrio"},
        {"InventarioPieza", "Listar,Obtener,BuscarDisponibles,InsertarInicial,MarcarEstado,Cortar,MarcarVendida"},
        {"MetodoPago", "Listar,AsegurarPorDefecto"},
        {"Permiso", "listarTiposPermiso,listarPermisosUsuario,listarPermisosRol,codigosPermisoUsuario,guardarPermisos,asignarPermisosRol,contarOtrosConPermiso"},
        {"ProductoVariante", "Listar,ListarColoresAluminio,ListarTiposAluminio,ListarColoresVidrio,ListarEspesoresVidrio,ListarEspesoresPorProducto,ObtenerPrecioUnitario,ObtenerPrecioUnitarioPorProducto,ObtenerVariantePorProducto,ResolverVariantePorProducto"},
        {"Venta", "Listar"},
        {"InventarioCaja", "Listar,NombreUnidad,ObtenerStock,SumarStock,RestarStock,AjustarStock"},
        {"ModeloVentanaWS", "ListarActivos,CargarPorId,Calcular"}
    }

    <WebMethod()> _
    Public Function Ejecutar(clase As String, metodo As String, parametrosJson As String) As String
        Try
            If String.IsNullOrEmpty(clase) OrElse String.IsNullOrEmpty(metodo) Then
                Throw New Exception("Faltan los datos de la operacion.")
            End If
            If Not MetodosPermitidos.ContainsKey(clase) Then
                Throw New Exception("La clase " & clase & " no esta disponible.")
            End If
            Dim permitidos As String() = MetodosPermitidos(clase).Split(","c)
            Dim autorizado As Boolean = False
            For Each p As String In permitidos
                If String.Equals(p.Trim(), metodo, StringComparison.OrdinalIgnoreCase) Then
                    autorizado = True
                    Exit For
                End If
            Next
            If Not autorizado Then
                Throw New Exception("El metodo " & metodo & " no esta disponible en " & clase & ".")
            End If

            Dim tipo As Type = Type.GetType("capaNegocio." & clase & ", capaNegocio")
            If tipo Is Nothing Then tipo = Type.GetType("capaWSLibrerias." & clase & ", capaWSLibrerias")
            If tipo Is Nothing Then Throw New Exception("No se pudo cargar la clase " & clase)
            Dim instancia As Object = Activator.CreateInstance(tipo)

            Dim argsJson As Object()
            If String.IsNullOrEmpty(parametrosJson) OrElse parametrosJson = "[]" Then
                argsJson = New Object() {}
            Else
                argsJson = TryCast(New JavaScriptSerializer().DeserializeObject(parametrosJson), Object())
                If argsJson Is Nothing Then Throw New Exception("Los parametros no son validos.")
            End If

            Dim elegido As MethodInfo = Nothing
            For Each m As MethodInfo In tipo.GetMethods(BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.Static)
                If Not String.Equals(m.Name, metodo, StringComparison.OrdinalIgnoreCase) Then Continue For
                Dim ps As ParameterInfo() = m.GetParameters()
                If ps.Length <> argsJson.Length Then Continue For
                elegido = m
                Exit For
            Next
            If elegido Is Nothing Then
                Throw New Exception("No se encontro la firma de " & metodo & ".")
            End If

            Dim parametros As ParameterInfo() = elegido.GetParameters()
            Dim valores(parametros.Length - 1) As Object
            For i As Integer = 0 To parametros.Length - 1
                valores(i) = ConvertirArgumento(argsJson(i), parametros(i).ParameterType)
            Next

            Dim resultado As Object = elegido.Invoke(If(elegido.IsStatic, Nothing, instancia), valores)
            Return Serializar(resultado)

        Catch ex As TargetInvocationException
            Dim inner As Exception = If(ex.InnerException, ex)
            Throw New SoapException(inner.Message, SoapException.ServerFaultCode)
        Catch ex As SoapException
            Throw
        Catch ex As Exception
            Throw New SoapException(ex.Message, SoapException.ServerFaultCode)
        End Try
    End Function

    Private Function ConvertirArgumento(valor As Object, tipoDestino As Type) As Object
        Dim subTipo As Type = Nullable.GetUnderlyingType(tipoDestino)
        Dim esNullable As Boolean = subTipo IsNot Nothing
        If esNullable Then tipoDestino = subTipo

        If valor Is Nothing Then
            If esNullable Then Return Nothing
            If tipoDestino.IsValueType Then Return Activator.CreateInstance(tipoDestino)
            Return Nothing
        End If
        If tipoDestino Is GetType(Byte()) Then
            Return ABytes(valor)
        End If
        If tipoDestino Is GetType(DataTable) Then
            If TypeOf valor Is String Then Return TablaDesdeJson(DirectCast(valor, String))
            Return valor
        End If
        If tipoDestino.IsGenericType AndAlso tipoDestino.GetGenericTypeDefinition() Is GetType(List(Of )) Then
            Dim js As New JavaScriptSerializer()
            If TypeOf valor Is String Then
                Return js.Deserialize(DirectCast(valor, String), tipoDestino)
            End If
            Return js.Deserialize(js.Serialize(valor), tipoDestino)
        End If
        If TypeOf valor Is Dictionary(Of String, Object) Then
            Return New JavaScriptSerializer().Deserialize(New JavaScriptSerializer().Serialize(valor), tipoDestino)
        End If
        If tipoDestino.IsEnum Then Return [Enum].Parse(tipoDestino, valor.ToString(), True)
        If TypeOf valor Is String AndAlso tipoDestino Is GetType(Date) Then
            Return Convert.ToDateTime(DirectCast(valor, String), Globalization.CultureInfo.InvariantCulture)
        End If
        Return Convert.ChangeType(valor, tipoDestino, Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Function Serializar(resultado As Object) As String
        If resultado Is Nothing Then Return "null"
        If TypeOf resultado Is DataTable Then Return JsonDataTable(DirectCast(resultado, DataTable))
        If TypeOf resultado Is String Then Return New JavaScriptSerializer().Serialize(DirectCast(resultado, String))

        Dim t As Type = resultado.GetType()
        If t.IsGenericType AndAlso t.GetGenericTypeDefinition().FullName.StartsWith("System.ValueTuple") Then
            Dim datos As New Dictionary(Of String, Object)
            Dim n As Integer = 1
            For Each f As FieldInfo In t.GetFields()
                datos.Add("Item" & n, f.GetValue(resultado))
                n += 1
            Next
            Return New JavaScriptSerializer().Serialize(datos)
        End If
        If t Is GetType(Boolean) OrElse t Is GetType(Integer) OrElse t Is GetType(Decimal) OrElse
           t Is GetType(Long) OrElse t Is GetType(Double) Then
            Return New JavaScriptSerializer().Serialize(resultado)
        End If
        Return New JavaScriptSerializer().Serialize(resultado)
    End Function

    Private Function TablaDesdeJson(json As String) As DataTable
        Dim dt As New DataTable()
        If String.IsNullOrEmpty(json) OrElse json = "[]" OrElse json = "null" Then Return dt
        Dim filas As Object() = TryCast(New JavaScriptSerializer().DeserializeObject(json), Object())
        If filas Is Nothing Then Return dt

        For Each fila As Object In filas
            Dim d As Dictionary(Of String, Object) = TryCast(fila, Dictionary(Of String, Object))
            If d Is Nothing Then Continue For
            For Each kv As KeyValuePair(Of String, Object) In d
                If Not dt.Columns.Contains(kv.Key) Then dt.Columns.Add(kv.Key, TipoColumna(kv.Key, kv.Value))
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
                    r(kv.Key) = Convert.ChangeType(kv.Value, dt.Columns(kv.Key).DataType, Globalization.CultureInfo.InvariantCulture)
                End If
            Next
            dt.Rows.Add(r)
        Next
        Return dt
    End Function

    ' JavaScriptSerializer convierte Byte() en un arreglo de numeros
    ' ([0,0,0,0,0,0,19,255]); acepta tambien base64 y Byte().
    Private Function ABytes(valor As Object) As Byte()
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

    Private Function TipoColumna(nombre As String, valor As Object) As Type
        If nombre.IndexOf("version", StringComparison.OrdinalIgnoreCase) >= 0 Then Return GetType(Byte())
        If valor Is Nothing Then Return GetType(String)
        Dim t As Type = valor.GetType()
        Select Case t.FullName
            Case "System.Int32", "System.Int64", "System.Int16", "System.Byte", "System.Single", "System.Double", "System.Decimal"
                Return GetType(Decimal)
            Case Else
                Return t
        End Select
    End Function

End Class

' ============================================================
' MODELOS DE VENTANA: la evaluacion sigue en el servicio.
' Se entregan DTOs con propiedades publicas para que JSON
' pueda reconstruirlos en capaWeb sin referencias a capaNegocio.
' ============================================================
Public Class ModeloVentanaWS

    Public Class VariableDTO
        Public Property Nombre As String
        Public Property Formula As String
    End Class

    Public Class ComponenteDTO
        Public Property Categoria As String
        Public Property Tipo As String
        Public Property RolColor As String
        Public Property TipoMedida As String
        Public Property ExprLargo As String
        Public Property ExprCantidad As String
        Public Property ExprAncho As String
        Public Property ExprAlto As String
        Public Property Condicion As String
    End Class

    Public Class ValidacionDTO
        Public Property Condicion As String
        Public Property Mensaje As String
    End Class

    Public Class ModeloDTO
        Public Property IdModelo As Integer
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Orden As Integer
        Public Property Variables As New List(Of VariableDTO)
        Public Property Componentes As New List(Of ComponenteDTO)
        Public Property Validaciones As New List(Of ValidacionDTO)
    End Class

    Public Class LineaDTO
        Public Property Categoria As String
        Public Property Tipo As String
        Public Property RolColor As String
        Public Property TipoMedida As String
        Public Property LargoCm As Decimal?
        Public Property Cantidad As Integer?
        Public Property AnchoCm As Decimal?
        Public Property AltoCm As Decimal?
    End Class

    Public Class ResultadoDTO
        Public Property Lineas As New List(Of LineaDTO)
        Public Property Mensajes As New List(Of String)
    End Class

    Public Shared Function ListarActivos() As List(Of ModeloDTO)
        Dim lista As New List(Of ModeloDTO)
        For Each m As capaNegocio.ModeloVentana In capaNegocio.ModeloVentana.ListarActivos()
            lista.Add(DesdeModelo(m))
        Next
        Return lista
    End Function

    Public Shared Function CargarPorId(idModelo As Integer) As ModeloDTO
        Return DesdeModelo(capaNegocio.ModeloVentana.CargarPorId(idModelo))
    End Function

    Public Shared Function Calcular(idModelo As Integer, entradas As Dictionary(Of String, Decimal)) As ResultadoDTO
        Dim modelo As capaNegocio.ModeloVentana = capaNegocio.ModeloVentana.CargarPorId(idModelo)
        If modelo Is Nothing Then Throw New Exception("No existe el modelo de ventana solicitado.")
        Dim calculado As capaNegocio.ModeloVentana.ResultadoModelo = modelo.Calcular(entradas)
        Dim resultado As New ResultadoDTO
        For Each msj As String In calculado.Mensajes
            resultado.Mensajes.Add(msj)
        Next
        For Each linea As capaNegocio.ModeloVentana.LineaVentana In calculado.Lineas
            resultado.Lineas.Add(New LineaDTO With {
                .Categoria = linea.Categoria,
                .Tipo = linea.Tipo,
                .RolColor = linea.RolColor,
                .TipoMedida = linea.TipoMedida,
                .LargoCm = linea.LargoCm,
                .Cantidad = linea.Cantidad,
                .AnchoCm = linea.AnchoCm,
                .AltoCm = linea.AltoCm})
        Next
        Return resultado
    End Function

    Private Shared Function DesdeModelo(m As capaNegocio.ModeloVentana) As ModeloDTO
        If m Is Nothing Then Return Nothing
        Dim dto As New ModeloDTO With {
            .IdModelo = m.IdModelo,
            .Nombre = m.Nombre,
            .Descripcion = m.Descripcion,
            .Orden = m.Orden}
        For Each v As capaNegocio.ModeloVentana.VariableModelo In m.Variables
            dto.Variables.Add(New VariableDTO With {.Nombre = v.Nombre, .Formula = v.Formula})
        Next
        For Each c As capaNegocio.ModeloVentana.ComponenteVentana In m.Componentes
            dto.Componentes.Add(New ComponenteDTO With {
                .Categoria = c.Categoria,
                .Tipo = c.Tipo,
                .RolColor = c.RolColor,
                .TipoMedida = c.TipoMedida,
                .ExprLargo = c.ExprLargo,
                .ExprCantidad = c.ExprCantidad,
                .ExprAncho = c.ExprAncho,
                .ExprAlto = c.ExprAlto,
                .Condicion = c.Condicion})
        Next
        For Each v As capaNegocio.ModeloVentana.ValidacionModelo In m.Validaciones
            dto.Validaciones.Add(New ValidacionDTO With {.Condicion = v.Condicion, .Mensaje = v.Mensaje})
        Next
        Return dto
    End Function

End Class
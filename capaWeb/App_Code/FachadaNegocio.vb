Imports System.Data
Imports System.Collections.Generic

' Esta fachada conserva los nombres y firmas de capaNegocio para que los
' formularios no cambien. Cada llamada se resuelve contra capaWSLibrerias.

Public Class ItemPedidoDTO
    Public Property Tipo As String
    Public Property Etiqueta As String
    Public Property PrecioMateriales As Decimal?
    Public Property CostoExtra As Decimal
    Public Property ManoObra As Decimal
    Public Property Total As Decimal
    Public Property IdModelo As Integer?
    Public Property Detalles As New List(Of DetallePedidoDTO)
End Class

Public Class DetallePedidoDTO
    Public Property IdVariante As Integer
    Public Property Accion As String
    Public Property IdPieza As Integer?
    Public Property AnchoCm As Decimal?
    Public Property AltoCm As Decimal?
    Public Property LargoCm As Decimal?
    Public Property CantidadCajas As Integer?
    Public Property Subtotal As Decimal
End Class

Public Class Usuario

    Public Function iniciarSesion(usu As String, con As String) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "iniciarSesion", New Object() {usu, con}))
    End Function

    Public Function validarNombreUsuario(usu As String) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "validarNombreUsuario", New Object() {usu}))
    End Function

    Public Function obtenerPregunta(usu As String) As String
        Return ServicioWeb.Valor(Of String)(ServicioWeb.Ejecutar("Usuario", "obtenerPregunta", New Object() {usu}))
    End Function

    Public Function validarRespuesta(usu As String, res As String) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "validarRespuesta", New Object() {usu, res}))
    End Function

    Public Sub cambiarContraseña(usu As String, con As String)
        ServicioWeb.Ejecutar("Usuario", "cambiarContraseña", New Object() {usu, con})
    End Sub

    Public Function obtenerIDUsuario(usu As String) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Usuario", "obtenerIDUsuario", New Object() {usu}))
    End Function

    Public Function obtenerPreferencia(idUsuario As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Usuario", "obtenerPreferencia", New Object() {idUsuario}))
    End Function

    Public Sub guardarPreferencia(idUsuario As Integer, tema As String, fuente As String)
        ServicioWeb.Ejecutar("Usuario", "guardarPreferencia", New Object() {idUsuario, tema, fuente})
    End Sub

    Public Function listarUsuarios() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Usuario", "listarUsuarios", Nothing))
    End Function

    Public Function obtenerUsuario(idUsuario As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Usuario", "obtenerUsuario", New Object() {idUsuario}))
    End Function

    Public Function existeNombre(nombre As String, idExcluir As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "existeNombre", New Object() {nombre, idExcluir}))
    End Function

    Public Function existeCorreo(correo As String, idExcluir As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "existeCorreo", New Object() {correo, idExcluir}))
    End Function

    Public Sub registrarUsuario(nombre As String, correo As String, contrasena As String,
                                pregunta As String, respuesta As String, rol As String, activo As Boolean)
        ServicioWeb.Ejecutar("Usuario", "registrarUsuario",
                             New Object() {nombre, correo, contrasena, pregunta, respuesta, rol, activo})
    End Sub

    Public Sub modificarUsuario(idUsuario As Integer, nombre As String, correo As String, contrasena As String,
                                pregunta As String, respuesta As String, rol As String, activo As Boolean)
        ServicioWeb.Ejecutar("Usuario", "modificarUsuario",
                             New Object() {idUsuario, nombre, correo, contrasena, pregunta, respuesta, rol, activo})
    End Sub

    Public Sub eliminarUsuario(idUsuario As Integer)
        ServicioWeb.Ejecutar("Usuario", "eliminarUsuario", New Object() {idUsuario})
    End Sub

    Public Sub cambiarEstadoUsuario(idUsuario As Integer, activo As Boolean)
        ServicioWeb.Ejecutar("Usuario", "cambiarEstadoUsuario", New Object() {idUsuario, activo})
    End Sub

    Public Function usuarioActivo(usu As String) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "usuarioActivo", New Object() {usu}))
    End Function

    Public Function esUltimoAdministrador(idUsuario As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Usuario", "esUltimoAdministrador", New Object() {idUsuario}))
    End Function

End Class

Public Class Cliente

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "Listar", Nothing))
    End Function

    Public Function Buscar(texto As String) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "Buscar", New Object() {texto}))
    End Function

    Public Function Obtener(idCliente As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "Obtener", New Object() {idCliente}))
    End Function

    Public Function Insertar(nombre As String, documento As String, telefono As String, direccion As String,
                             Optional tipoPersona As String = Nothing, Optional tipoDocumento As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Cliente", "Insertar",
            New Object() {nombre, documento, telefono, direccion, tipoPersona, tipoDocumento}))
    End Function

    Public Sub Actualizar(idCliente As Integer, nombre As String, documento As String, telefono As String, direccion As String)
        Dim dt As DataTable = Obtener(idCliente)
        If dt.Rows.Count = 0 Then Throw New Exception("El cliente ya no existe. Actualice el listado.")
        Actualizar(idCliente, nombre, documento, telefono, direccion,
                   DirectCast(dt.Rows(0)("version_cliente"), Byte()))
    End Sub

    Public Sub Actualizar(idCliente As Integer, nombre As String, documento As String, telefono As String,
                          direccion As String, versionOriginal As Byte(),
                          Optional tipoPersona As String = Nothing, Optional tipoDocumento As String = Nothing)
        ServicioWeb.Ejecutar("Cliente", "Actualizar",
            New Object() {idCliente, nombre, documento, telefono, direccion, versionOriginal, tipoPersona, tipoDocumento})
    End Sub

    Public Function DocumentoCoincidente(documento As String, Optional idExcluir As Integer = 0,
                                         Optional tipoDocumento As String = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "DocumentoCoincidente",
            New Object() {documento, idExcluir, tipoDocumento}))
    End Function

    Public Function Historial(idCliente As Integer, Optional estado As String = Nothing,
                              Optional desde As Date? = Nothing, Optional hasta As Date? = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "Historial",
            New Object() {idCliente, estado, desde, hasta}))
    End Function

    Public Function PagosDePedido(idCliente As Integer, idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Cliente", "PagosDePedido", New Object() {idCliente, idPedido}))
    End Function

    Public Function TienePedidos(idCliente As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Cliente", "TienePedidos", New Object() {idCliente}))
    End Function

    Public Sub Eliminar(idCliente As Integer)
        ServicioWeb.Ejecutar("Cliente", "Eliminar", New Object() {idCliente})
    End Sub

    Public Sub ValidarDatos(nombre As String, documento As String, telefono As String, direccion As String,
                            tipoPersona As String, tipoDocumento As String)
        ServicioWeb.Ejecutar("Cliente", "ValidarDatos",
            New Object() {nombre, documento, telefono, direccion, tipoPersona, tipoDocumento})
    End Sub

End Class

Public Class Proveedor

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Proveedor", "Listar", Nothing))
    End Function

    Public Function Buscar(texto As String) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Proveedor", "Buscar", New Object() {texto}))
    End Function

    Public Function Obtener(idProveedor As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Proveedor", "Obtener", New Object() {idProveedor}))
    End Function

    Public Function Insertar(nombre As String, documento As String, telefono As String, direccion As String,
                             Optional tipoPersona As String = Nothing, Optional tipoDocumento As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Proveedor", "Insertar",
            New Object() {nombre, documento, telefono, direccion, tipoPersona, tipoDocumento}))
    End Function

    Public Sub Actualizar(idProveedor As Integer, nombre As String, documento As String, telefono As String, direccion As String)
        Dim dt As DataTable = Obtener(idProveedor)
        If dt.Rows.Count = 0 Then Throw New Exception("El proveedor ya no existe. Actualice el listado.")
        Actualizar(idProveedor, nombre, documento, telefono, direccion,
                   DirectCast(dt.Rows(0)("version_proveedor"), Byte()))
    End Sub

    Public Sub Actualizar(idProveedor As Integer, nombre As String, documento As String, telefono As String,
                          direccion As String, versionOriginal As Byte(),
                          Optional tipoPersona As String = Nothing, Optional tipoDocumento As String = Nothing)
        ServicioWeb.Ejecutar("Proveedor", "Actualizar",
            New Object() {idProveedor, nombre, documento, telefono, direccion, versionOriginal, tipoPersona, tipoDocumento})
    End Sub

    Public Function DocumentoCoincidente(documento As String, Optional idExcluir As Integer = 0,
                                         Optional tipoDocumento As String = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Proveedor", "DocumentoCoincidente",
            New Object() {documento, idExcluir, tipoDocumento}))
    End Function

    Public Function TieneCompras(idProveedor As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Proveedor", "TieneCompras", New Object() {idProveedor}))
    End Function

    Public Sub Eliminar(idProveedor As Integer)
        ServicioWeb.Ejecutar("Proveedor", "Eliminar", New Object() {idProveedor})
    End Sub

    Public Sub ValidarDatos(nombre As String, documento As String, telefono As String, direccion As String,
                            tipoPersona As String, tipoDocumento As String)
        ServicioWeb.Ejecutar("Proveedor", "ValidarDatos",
            New Object() {nombre, documento, telefono, direccion, tipoPersona, tipoDocumento})
    End Sub

End Class

Public Class Producto

    Public Shared ReadOnly Categorias As String() = {"VIDRIO", "ALUMINIO", "ACCESORIO", "TORNILLO", "TARUGO"}

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Producto", "Listar", Nothing))
    End Function

    Public Function ListarTodos() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Producto", "ListarTodos", Nothing))
    End Function

    Public Function ListarPorCategoria(categoria As String) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Producto", "ListarPorCategoria", New Object() {categoria}))
    End Function

    Public Function Obtener(idProducto As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Producto", "Obtener", New Object() {idProducto}))
    End Function

    Public Function ObtenerIdPorTipoColor(categoria As String, tipo As String, color As String) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Producto", "ObtenerIdPorTipoColor",
            New Object() {categoria, tipo, color}))
    End Function

    Public Function ExisteProducto(categoria As String, tipo As String, color As String, idExcluir As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Producto", "ExisteProducto",
            New Object() {categoria, tipo, color, idExcluir}))
    End Function

    Public Function Insertar(categoria As String, tipo As String, color As String, nombre As String,
                             descripcion As String, Optional imagenUrl As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Producto", "Insertar",
            New Object() {categoria, tipo, color, nombre, descripcion, imagenUrl}))
    End Function

    Public Sub Actualizar(idProducto As Integer, categoria As String, tipo As String, color As String,
                          nombre As String, descripcion As String, Optional imagenUrl As String = Nothing)
        ServicioWeb.Ejecutar("Producto", "Actualizar",
            New Object() {idProducto, categoria, tipo, color, nombre, descripcion, imagenUrl})
    End Sub

    Public Sub Inactivar(idProducto As Integer)
        ServicioWeb.Ejecutar("Producto", "Inactivar", New Object() {idProducto})
    End Sub

    Public Sub Eliminar(idProducto As Integer)
        ServicioWeb.Ejecutar("Producto", "Eliminar", New Object() {idProducto})
    End Sub

    Public Sub CambiarEstado(idProducto As Integer, activo As Boolean)
        ServicioWeb.Ejecutar("Producto", "CambiarEstado", New Object() {idProducto, activo})
    End Sub

    Public Function TieneVariantes(idProducto As Integer) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Producto", "TieneVariantes", New Object() {idProducto}))
    End Function

End Class

Public Class Pedido

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "Listar", Nothing))
    End Function

    Public Function ListarDetallado() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "ListarDetallado", Nothing))
    End Function

    Public Function Obtener(idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "Obtener", New Object() {idPedido}))
    End Function

    Public Sub CambiarEstado(idPedido As Integer, estado As String)
        ServicioWeb.Ejecutar("Pedido", "CambiarEstado", New Object() {idPedido, estado})
    End Sub

    Public Sub ActualizarEstadoPago(idPedido As Integer)
        ServicioWeb.Ejecutar("Pedido", "ActualizarEstadoPago", New Object() {idPedido})
    End Sub

    Public Function ListarItems(idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "ListarItems", New Object() {idPedido}))
    End Function

    Public Function ListarDetalles(idItem As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "ListarDetalles", New Object() {idItem}))
    End Function

    Public Function ListarItemsDetallado(idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "ListarItemsDetallado", New Object() {idPedido}))
    End Function

    Public Function ListarDetallesDeItem(idItem As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "ListarDetallesDeItem", New Object() {idItem}))
    End Function

    Public Sub ResolverDetalle(idDetalle As Integer)
        ServicioWeb.Ejecutar("Pedido", "ResolverDetalle", New Object() {idDetalle})
    End Sub

    Public Function MarcarItemRoto(idDetalle As Integer, Optional idUsuario As Integer? = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pedido", "MarcarItemRoto", New Object() {idDetalle, idUsuario}))
    End Function

    Public Sub CancelarItem(idItem As Integer, Optional idUsuario As Integer? = Nothing)
        ServicioWeb.Ejecutar("Pedido", "CancelarItem", New Object() {idItem, idUsuario})
    End Sub

    Public Sub Cancelar(idPedido As Integer, Optional idUsuario As Integer? = Nothing)
        ServicioWeb.Ejecutar("Pedido", "Cancelar", New Object() {idPedido, idUsuario})
    End Sub

    Public Sub Terminar(idPedido As Integer, Optional idUsuario As Integer? = Nothing)
        ServicioWeb.Ejecutar("Pedido", "Terminar", New Object() {idPedido, idUsuario})
    End Sub

    Public Sub TerminarItem(idItem As Integer, Optional idUsuario As Integer? = Nothing)
        ServicioWeb.Ejecutar("Pedido", "TerminarItem", New Object() {idItem, idUsuario})
    End Sub

    Public Sub RecalcularEstados(idPedido As Integer)
        ServicioWeb.Ejecutar("Pedido", "RecalcularEstados", New Object() {idPedido})
    End Sub

    Public Function TarifaSugerida(anchoCm As Decimal, altoCm As Decimal) As (aluminio As Decimal, silicona As Decimal)
        Dim json As String = ServicioWeb.Ejecutar("Pedido", "TarifaSugerida", New Object() {anchoCm, altoCm})
        Dim d As Dictionary(Of String, Object) = ServicioWeb.Valor(Of Dictionary(Of String, Object))(json)
        Return (Convert.ToDecimal(d("Item1"), Globalization.CultureInfo.InvariantCulture),
                Convert.ToDecimal(d("Item2"), Globalization.CultureInfo.InvariantCulture))
    End Function

    Public Function Registrar(idCliente As Integer, idUsuario As Integer, items As List(Of ItemPedidoDTO)) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Pedido", "Registrar",
            New Object() {idCliente, idUsuario, items}))
    End Function

End Class

Public Class Pago

    Public Function ListarPorPedido(idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pago", "ListarPorPedido", New Object() {idPedido}))
    End Function

    Public Function ListarDetallesPorPedido(idPedido As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Pago", "ListarDetallesPorPedido", New Object() {idPedido}))
    End Function

    Public Function RegistrarDetalle(idPedido As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Pago", "RegistrarDetalle",
            New Object() {idPedido, idMetodoPago, monto}))
    End Function

    Public Sub ActualizarDetalle(idDetallePago As Integer, idMetodoPago As Integer, monto As Decimal)
        ServicioWeb.Ejecutar("Pago", "ActualizarDetalle", New Object() {idDetallePago, idMetodoPago, monto})
    End Sub

    Public Sub EliminarDetalle(idDetallePago As Integer)
        ServicioWeb.Ejecutar("Pago", "EliminarDetalle", New Object() {idDetallePago})
    End Sub

    Public Function TotalPagado(idPedido As Integer) As Decimal
        Return ServicioWeb.Valor(Of Decimal)(ServicioWeb.Ejecutar("Pago", "TotalPagado", New Object() {idPedido}))
    End Function

    Public Function UltimoMonto(idPedido As Integer) As Decimal
        Return ServicioWeb.Valor(Of Decimal)(ServicioWeb.Ejecutar("Pago", "UltimoMonto", New Object() {idPedido}))
    End Function

End Class

Public Class MetodoPago

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("MetodoPago", "Listar", Nothing))
    End Function

    Public Function AsegurarPorDefecto() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("MetodoPago", "AsegurarPorDefecto", Nothing))
    End Function

End Class

Public Class Compra

    Public Class ImportesDetalle
        Public Property Cantidad As Decimal
        Public Property Subtotal As Decimal
    End Class

    Public Shared Function CalcularImportes(unidad As String, piezas As Decimal, costo As Decimal,
                                            Optional ancho As Decimal? = Nothing,
                                            Optional alto As Decimal? = Nothing,
                                            Optional largo As Decimal? = Nothing) As ImportesDetalle
        Return ServicioWeb.Valor(Of ImportesDetalle)(ServicioWeb.Ejecutar("Compra", "CalcularImportes",
            New Object() {unidad, piezas, costo, ancho, alto, largo}))
    End Function

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Compra", "Listar", Nothing))
    End Function

    Public Function ListarPorFecha(desde As Date, hasta As Date) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Compra", "ListarPorFecha", New Object() {desde, hasta}))
    End Function

    Public Function ObtenerCompra(idCompra As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Compra", "ObtenerCompra", New Object() {idCompra}))
    End Function

    Public Function ObtenerDetalleParaEditar(idCompra As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Compra", "ObtenerDetalleParaEditar", New Object() {idCompra}))
    End Function

    Public Function GuardarCompra(proveedor As String, fecha As Date, motivo As String,
                                  idPedidoOrigen As Integer?, detalle As DataTable,
                                  Optional idCompra As Integer = 0,
                                  Optional idUsuario As Integer? = Nothing,
                                  Optional idProveedor As Integer? = Nothing) As Integer
        Dim detalleJson As String = "[]"
        If detalle IsNot Nothing Then
            detalleJson = New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(
                ServicioWeb.FilasComoListas(detalle))
        End If
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Compra", "GuardarCompra",
            New Object() {proveedor, fecha, motivo, idPedidoOrigen, detalleJson, idCompra, idUsuario, idProveedor}))
    End Function

    Public Sub EliminarCompra(idCompra As Integer)
        ServicioWeb.Ejecutar("Compra", "EliminarCompra", New Object() {idCompra})
    End Sub

End Class

Public Class Inventario

    Public Shared ReadOnly Categorias As String() = {"VIDRIO", "ALUMINIO", "ACCESORIO", "TORNILLO", "TARUGO"}
    Public Shared ReadOnly EstadosPieza As String() = {"DISPONIBLE", "RESERVADA", "VENDIDA", "CONSUMIDA", "DEFECTUOSA"}
    Public Shared ReadOnly UnidadesPorPieza As String() = {"CM2", "PIE2", "CM_LINEAL"}
    Public Shared ReadOnly UnidadesPorCantidad As String() = {"CAJA", "UNIDAD"}

    Public Function ListarResumen(Optional categoria As String = Nothing,
                                  Optional soloBajoMinimo As Boolean = False) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Inventario", "ListarResumen",
            New Object() {categoria, soloBajoMinimo}))
    End Function

    Public Function ContarBajoMinimo() As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Inventario", "ContarBajoMinimo", Nothing))
    End Function

    Public Shared Function EsVidrio(unidadCosto As String) As Boolean
        Return ServicioWeb.Valor(Of Boolean)(ServicioWeb.Ejecutar("Inventario", "EsVidrio", New Object() {unidadCosto}))
    End Function

    Public Function ListarVariantes(porPiezas As Boolean) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Inventario", "ListarVariantes", New Object() {porPiezas}))
    End Function

    Public Function ListarTodasLasVariantes() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Inventario", "ListarTodasLasVariantes", Nothing))
    End Function

    Public Sub ActualizarStockMinimo(idVariante As Integer, stockMinimo As Integer)
        ServicioWeb.Ejecutar("Inventario", "ActualizarStockMinimo", New Object() {idVariante, stockMinimo})
    End Sub

    Public Function ListarMovimientos(desde As Date, hasta As Date?,
                                      Optional idVariante As Integer? = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Inventario", "ListarMovimientos",
            New Object() {desde, hasta, idVariante}))
    End Function

End Class

Public Class InventarioPieza

    Public Function Listar(Optional idVariante As Integer? = Nothing,
                           Optional estado As String = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("InventarioPieza", "Listar", New Object() {idVariante, estado}))
    End Function

    Public Function BuscarDisponibles(idVariante As Integer, Optional anchoMin As Decimal? = Nothing,
                                      Optional altoMin As Decimal? = Nothing,
                                      Optional largoMin As Decimal? = Nothing) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("InventarioPieza", "BuscarDisponibles",
            New Object() {idVariante, anchoMin, altoMin, largoMin}))
    End Function

    Public Function InsertarInicial(idVariante As Integer, anchoCm As Decimal?, altoCm As Decimal?,
                                    largoCm As Decimal?, Optional idUsuario As Integer? = Nothing,
                                    Optional observacion As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("InventarioPieza", "InsertarInicial",
            New Object() {idVariante, anchoCm, altoCm, largoCm, idUsuario, observacion}))
    End Function

    Public Sub MarcarEstado(idPieza As Integer, estado As String,
                            Optional idUsuario As Integer? = Nothing,
                            Optional idPedido As Integer? = Nothing,
                            Optional observacion As String = Nothing)
        ServicioWeb.Ejecutar("InventarioPieza", "MarcarEstado",
            New Object() {idPieza, estado, idUsuario, idPedido, observacion})
    End Sub

    Public Function Cortar(idPieza As Integer, anchoCorte As Decimal?, altoCorte As Decimal?,
                           largoCorte As Decimal?, Optional idUsuario As Integer? = Nothing,
                           Optional idPedido As Integer? = Nothing,
                           Optional minimoUtilCm As Decimal = 10D) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("InventarioPieza", "Cortar",
            New Object() {idPieza, anchoCorte, altoCorte, largoCorte, idUsuario, idPedido, minimoUtilCm}))
    End Function

    Public Sub MarcarVendida(idPieza As Integer, Optional idUsuario As Integer? = Nothing,
                             Optional idPedido As Integer? = Nothing)
        ServicioWeb.Ejecutar("InventarioPieza", "MarcarVendida", New Object() {idPieza, idUsuario, idPedido})
    End Sub

End Class

Public Class Permiso

    Public Function listarTiposPermiso() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Permiso", "listarTiposPermiso", Nothing))
    End Function

    Public Function listarPermisosUsuario(idUsuario As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Permiso", "listarPermisosUsuario", New Object() {idUsuario}))
    End Function

    Public Function listarPermisosRol(rol As String) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Permiso", "listarPermisosRol", New Object() {rol}))
    End Function

    Public Function codigosPermisoUsuario(idUsuario As Integer) As List(Of String)
        Return ServicioWeb.Valor(Of List(Of String))(ServicioWeb.Ejecutar("Permiso", "codigosPermisoUsuario",
            New Object() {idUsuario}))
    End Function

    Public Sub guardarPermisos(idUsuario As Integer, idsTipo As List(Of Integer))
        ServicioWeb.Ejecutar("Permiso", "guardarPermisos", New Object() {idUsuario, idsTipo})
    End Sub

    Public Sub asignarPermisosRol(idUsuario As Integer, rol As String)
        ServicioWeb.Ejecutar("Permiso", "asignarPermisosRol", New Object() {idUsuario, rol})
    End Sub

    Public Function contarOtrosConPermiso(codigo As String, idExcluir As Integer) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("Permiso", "contarOtrosConPermiso",
            New Object() {codigo, idExcluir}))
    End Function

End Class

Public Class ProductoVariante

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("ProductoVariante", "Listar", Nothing))
    End Function

    Public Function ListarColoresAluminio() As List(Of String)
        Return ServicioWeb.Valor(Of List(Of String))(ServicioWeb.Ejecutar("ProductoVariante", "ListarColoresAluminio", Nothing))
    End Function

    Public Function ListarEspesoresPorProducto(idProducto As Integer) As List(Of Decimal)
        Return ServicioWeb.Valor(Of List(Of Decimal))(ServicioWeb.Ejecutar("ProductoVariante", "ListarEspesoresPorProducto",
            New Object() {idProducto}))
    End Function

    Public Function ObtenerPrecioUnitario(idProducto As Integer, espesorCm As Decimal) As Decimal
        Return ServicioWeb.Valor(Of Decimal)(ServicioWeb.Ejecutar("ProductoVariante", "ObtenerPrecioUnitario",
            New Object() {idProducto, espesorCm}))
    End Function

    Public Function ObtenerPrecioUnitarioPorProducto(idProducto As Integer) As Decimal
        Return ServicioWeb.Valor(Of Decimal)(ServicioWeb.Ejecutar("ProductoVariante", "ObtenerPrecioUnitarioPorProducto",
            New Object() {idProducto}))
    End Function

    Public Function ObtenerVariantePorProducto(idProducto As Integer) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("ProductoVariante", "ObtenerVariantePorProducto",
            New Object() {idProducto}))
    End Function

    Public Function ResolverVariantePorProducto(idProducto As Integer, espesorCm As Decimal) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("ProductoVariante", "ResolverVariantePorProducto",
            New Object() {idProducto, espesorCm}))
    End Function

End Class

Public Class Venta

    Public Function Listar(Optional desde As Date? = Nothing, Optional hasta As Date? = Nothing,
                           Optional cliente As String = "", Optional estadoPago As String = "TODOS") As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Venta", "Listar", New Object() {desde, hasta, cliente, estadoPago}))
    End Function

End Class

Public Class InventarioCaja

    Public Shared Function NombreUnidad(unidadCosto As String) As String
        Return ServicioWeb.Valor(Of String)(ServicioWeb.Ejecutar("InventarioCaja", "NombreUnidad", New Object() {unidadCosto}))
    End Function

    Public Function Listar() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("InventarioCaja", "Listar", Nothing))
    End Function

    Public Function SumarStock(idVariante As Integer, cantidad As Integer,
                               Optional motivo As String = "COMPRA", Optional idUsuario As Integer? = Nothing,
                               Optional idCompraDetalle As Integer? = Nothing,
                               Optional observacion As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("InventarioCaja", "SumarStock",
            New Object() {idVariante, cantidad, motivo, idUsuario, idCompraDetalle, observacion}))
    End Function

    Public Function RestarStock(idVariante As Integer, cantidad As Integer,
                                Optional motivo As String = "VENTA", Optional idUsuario As Integer? = Nothing,
                                Optional idPedido As Integer? = Nothing,
                                Optional observacion As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("InventarioCaja", "RestarStock",
            New Object() {idVariante, cantidad, motivo, idUsuario, idPedido, observacion}))
    End Function

    Public Function AjustarStock(idVariante As Integer, stockReal As Integer,
                                 Optional idUsuario As Integer? = Nothing,
                                 Optional observacion As String = Nothing) As Integer
        Return ServicioWeb.Valor(Of Integer)(ServicioWeb.Ejecutar("InventarioCaja", "AjustarStock",
            New Object() {idVariante, stockReal, idUsuario, observacion}))
    End Function

End Class

' Fachada del modulo de reportes. Las paginas usan esta clase, nunca capaNegocio.
Public Class Reporte

    Public Function FlujoInventario(desde As Date, hasta As Date,
                                    categoria As String, idVariante As Integer?) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Reporte", "FlujoInventario",
            New Object() {desde, hasta, categoria, idVariante}))
    End Function

    Public Function PedidosDelMes(anio As Integer, mes As Integer, estadoPago As String) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Reporte", "PedidosDelMes",
            New Object() {anio, mes, estadoPago}))
    End Function

    Public Function ComprasPorTiempo(desde As Date, hasta As Date) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Reporte", "ComprasPorTiempo",
            New Object() {desde, hasta}))
    End Function

    Public Function ClientesConPendientes() As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Reporte", "ClientesConPendientes", Nothing))
    End Function

    Public Function DetallePendientesPorCliente(idCliente As Integer) As DataTable
        Return ServicioWeb.Tabla(ServicioWeb.Ejecutar("Reporte", "DetallePendientesPorCliente",
            New Object() {idCliente}))
    End Function

End Class

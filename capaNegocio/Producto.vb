Imports capaDatos
Imports System.Collections.Generic

Public Class Producto
    Dim objMan As New clsMantenimiento

    ' Categorías válidas (las usan inventario, compras y los datos de prueba)
    Public Shared ReadOnly Categorias As String() = {"VIDRIO", "ALUMINIO", "TORNILLO", "TARUGO"}


    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from productos where activo=1 order by categoria, tipo")
    End Function

    Public Function ListarPorCategoria(categoria As String) As DataTable
        Return objMan.consultarConParametros(
            "select * from productos where categoria=@categoria and activo=1 order by tipo",
            New Dictionary(Of String, Object) From {{"categoria", categoria}})
    End Function

    Public Function Insertar(categoria As String, tipo As String, color As String, nombre As String,
                             descripcion As String, Optional imagenUrl As String = Nothing) As Integer
        Dim sql As String = "insert into productos (categoria, tipo, color, nombre, descripcion, imagen_url, activo) " &
                            "values (@categoria, @tipo, @color, @nombre, @descripcion, @imagen_url, 1); " &
                            "select cast(scope_identity() as int) as id"
        Dim dt As DataTable = objMan.consultarConParametros(sql, Parametros(categoria, tipo, color, nombre, descripcion, imagenUrl))
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idProducto As Integer, categoria As String, tipo As String, color As String, nombre As String,
                          descripcion As String, Optional imagenUrl As String = Nothing)
        Dim sql As String = "update productos set categoria=@categoria, tipo=@tipo, color=@color, nombre=@nombre, " &
                            "descripcion=@descripcion, imagen_url=@imagen_url where id_producto=@id_producto"
        Dim par As Dictionary(Of String, Object) = Parametros(categoria, tipo, color, nombre, descripcion, imagenUrl)
        par.Add("id_producto", idProducto)
        objMan.ejecutarConParametros(sql, par)
    End Sub

    ' Baja lógica: no se borra, se marca inactivo (puede estar en compras o ventas pasadas)
    Public Sub Inactivar(idProducto As Integer)
        CambiarEstado(idProducto, False)
    End Sub

    ' =========================================================
    ' Mantenimiento de productos (frmListadoProductos / frmRegistrarProductos)
    ' =========================================================

    ' Todos los productos (activos e inactivos), con cuántas variantes tiene cada uno
    Public Function ListarTodos() As DataTable
        Dim sql As String =
            "select p.id_producto, p.categoria, p.tipo, p.color, p.nombre, p.descripcion, p.activo, " &
            "case when p.activo = 1 then 'ACTIVO' else 'INACTIVO' end as estado, " &
            "(select count(*) from producto_variante pv where pv.id_producto = p.id_producto) as variantes " &
            "from productos p order by p.categoria, p.nombre"
        Try
            Return objMan.listarComando(sql)
        Catch ex As Exception
            Throw New Exception("Error al listar productos!")
        End Try
    End Function

    Public Function Obtener(idProducto As Integer) As DataTable
        Try
            Return objMan.consultarConParametros(
                "select id_producto, categoria, tipo, color, nombre, descripcion, imagen_url, activo " &
                "from productos where id_producto=@id_producto",
                New Dictionary(Of String, Object) From {{"id_producto", idProducto}})
        Catch ex As Exception
            Throw New Exception("Error al obtener datos del producto!")
        End Try
    End Function

    ' Evita registrar dos veces el mismo producto (misma categoría, tipo y color)
    ' idExcluir: al modificar se pasa el id del propio producto para que no se compare consigo mismo
    Public Function ExisteProducto(categoria As String, tipo As String, color As String, idExcluir As Integer) As Boolean
        Dim sql As String =
            "select top 1 id_producto from productos " &
            "where categoria=@categoria and tipo=@tipo and isnull(color,'')=isnull(@color,'') and id_producto<>@id_excluir"
        Try
            Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {
                {"categoria", categoria}, {"tipo", Limpiar(tipo)}, {"color", Limpiar(color)}, {"id_excluir", idExcluir}}).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al validar el producto!")
        End Try
    End Function

    ' Un producto con variantes (medidas y precios) no se puede eliminar: la base lo impide
    Public Function TieneVariantes(idProducto As Integer) As Boolean
        Try
            Return objMan.consultarConParametros(
                "select top 1 id_variante from producto_variante where id_producto=@id_producto",
                New Dictionary(Of String, Object) From {{"id_producto", idProducto}}).Rows.Count > 0
        Catch ex As Exception
            Throw New Exception("Error al verificar variantes del producto!")
        End Try
    End Function

    Public Sub Eliminar(idProducto As Integer)
        Try
            objMan.ejecutarConParametros("delete from productos where id_producto=@id_producto",
                New Dictionary(Of String, Object) From {{"id_producto", idProducto}})
        Catch ex As Exception
            Throw New Exception("Error al eliminar producto!")
        End Try
    End Sub

    ' activo = False -> dar de baja ; activo = True -> reactivar
    Public Sub CambiarEstado(idProducto As Integer, activo As Boolean)
        Try
            objMan.ejecutarConParametros("update productos set activo=@activo where id_producto=@id_producto",
                New Dictionary(Of String, Object) From {{"activo", activo}, {"id_producto", idProducto}})
        Catch ex As Exception
            Throw New Exception("Error al cambiar estado del producto!")
        End Try
    End Sub

    ' =========================================================
    ' Ayudantes internos
    ' =========================================================

    ' Texto vacío se guarda como NULL en la base, no como ''
    Private Shared Function Limpiar(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return Nothing
        Return texto.Trim()
    End Function

    Private Shared Function Parametros(categoria As String, tipo As String, color As String, nombre As String,
                                       descripcion As String, imagenUrl As String) As Dictionary(Of String, Object)
        Return New Dictionary(Of String, Object) From {
            {"categoria", Limpiar(categoria)}, {"tipo", Limpiar(tipo)}, {"color", Limpiar(color)},
            {"nombre", Limpiar(nombre)}, {"descripcion", Limpiar(descripcion)}, {"imagen_url", Limpiar(imagenUrl)}}
    End Function
End Class
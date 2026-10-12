Imports capaDatos

Public Class ProductoVariante
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Dim sql As String = "select pv.id_variante, p.nombre, p.categoria, p.tipo, p.color, c.espesor_cm, c.medida_cm, pv.unidad_costo, pv.precio_unitario " &
                             "from producto_variante pv " &
                             "inner join productos p on p.id_producto = pv.id_producto " &
                             "inner join caracteristicas c on c.id_caracteristica = pv.id_caracteristica"
        Return objMan.listarComando(sql)
    End Function

    Public Function Insertar(idProducto As Integer, idCaracteristica As Integer, unidadCosto As String, precioUnitario As Decimal) As Integer
        Dim sql As String = "insert into producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario) values (" &
            idProducto & "," & idCaracteristica & ",'" & Esc(unidadCosto) & "'," & Num(precioUnitario) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idVariante As Integer, unidadCosto As String, precioUnitario As Decimal)
        Dim sql As String = "update producto_variante set unidad_costo='" & Esc(unidadCosto) & "', precio_unitario=" & Num(precioUnitario) & " where id_variante=" & idVariante
        objMan.ejecutarComando(sql)
    End Sub

    Public Function ListarColoresAluminio() As List(Of String)
        Dim objMan As New clsMantenimiento
        Dim dt = objMan.listarProcedimiento("sp_ped_combo_colores_aluminio", Nothing)
        Dim lista As New List(Of String)
        For Each fila As DataRow In dt.Rows
            lista.Add(fila("color").ToString())
        Next
        Return lista
    End Function

    Public Function ListarTiposAluminio(color As String) As List(Of String)
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"color", color}}
        Dim dt = objMan.listarProcedimiento("sp_ped_combo_tipos_aluminio", parametros)
        Dim lista As New List(Of String)
        For Each fila As DataRow In dt.Rows
            lista.Add(fila("tipo").ToString())
        Next
        Return lista
    End Function

    Public Function ListarColoresVidrio() As List(Of String)
        Dim objMan As New clsMantenimiento
        Dim dt = objMan.listarProcedimiento("sp_ped_combo_colores_vidrio", Nothing)
        Dim lista As New List(Of String)
        For Each fila As DataRow In dt.Rows
            lista.Add(fila("color_vidrio").ToString())
        Next
        Return lista
    End Function

    Public Function ListarEspesoresVidrio(tipo As String, color As String) As List(Of Decimal)
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"tipo", tipo}, {"color", color}}
        Dim dt = objMan.listarProcedimiento("sp_ped_combo_espesores_vidrio", parametros)
        Dim lista As New List(Of Decimal)
        For Each fila As DataRow In dt.Rows
            lista.Add(CDec(fila("espesor_cm")))
        Next
        Return lista
    End Function

    Public Function ResolverVarianteVidrio(tipo As String, color As String, espesorCm As Decimal) As (idVariante As Integer, precioUnitario As Decimal)
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"tipo", tipo}, {"color", color}, {"espesor_cm", espesorCm}}
        Dim dt = objMan.listarProcedimiento("sp_ped_resolver_variante_vidrio", parametros)
        If dt.Rows.Count = 0 Then Throw New Exception("No existe una variante de vidrio con ese tipo, color y espesor.")
        Return (CInt(dt.Rows(0)("id_variante")), CDec(dt.Rows(0)("precio_unitario")))
    End Function

    Public Function ResolverVarianteAluminio(color As String, tipo As String) As (idVariante As Integer, precioUnitario As Decimal)
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"color", color}, {"tipo", tipo}}
        Dim dt = objMan.listarProcedimiento("sp_ped_resolver_variante_aluminio", parametros)
        If dt.Rows.Count = 0 Then Throw New Exception("No existe una variante de aluminio con ese color y tipo.")
        Return (CInt(dt.Rows(0)("id_variante")), CDec(dt.Rows(0)("precio_unitario")))
    End Function

    ' Espesores disponibles de un producto concreto (para el combo en cascada del pedido)
    Public Function ListarEspesoresPorProducto(idProducto As Integer) As List(Of Decimal)
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"id_producto", idProducto}}
        Dim dt = objMan.listarProcedimiento("sp_ped_espesores_por_producto", parametros)
        Dim lista As New List(Of Decimal)
        For Each fila As DataRow In dt.Rows
            If Not IsDBNull(fila("espesor_cm")) Then lista.Add(CDec(fila("espesor_cm")))
        Next
        Return lista
    End Function

    ' Resuelve la variante (id) de un producto segun el espesor elegido
    Public Function ResolverVariantePorProducto(idProducto As Integer, espesorCm As Decimal) As Integer
        Dim objMan As New clsMantenimiento
        Dim parametros As New Dictionary(Of String, Object) From {{"id_producto", idProducto}, {"espesor_cm", espesorCm}}
        Dim dt = objMan.listarProcedimiento("sp_ped_resolver_variante_producto", parametros)
        If dt.Rows.Count = 0 Then Throw New Exception("No existe una variante de vidrio para ese producto y espesor.")
        Return CInt(dt.Rows(0)("id_variante"))
    End Function

End Class

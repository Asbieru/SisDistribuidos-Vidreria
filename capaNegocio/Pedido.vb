Imports System.Data
Imports System.Data.SqlClient
Imports capaDatos

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

Public Class Pedido

    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from pedidos order by fecha desc")
    End Function

    Public Function ListarPorCliente(idCliente As Integer) As DataTable
        Return objMan.listarComando("select * from pedidos where id_cliente=" & idCliente & " order by fecha")
    End Function

    ' Listado para la grilla: trae el nombre del cliente y del vendedor.
    ' fecha_registro queda como texto (dd/MM/yyyy) para poder buscar por fecha.
    Public Function ListarDetallado() As DataTable
        Return objMan.listarComando(
            "select p.id_pedido, c.nombre as cliente, u.nombre as usuario, p.fecha, p.estado, " &
            "convert(varchar(10), p.fecha, 103) as fecha_registro " &
            "from pedidos p " &
            "inner join clientes c on c.id_cliente = p.id_cliente " &
            "inner join usuarios u on u.id_usuario = p.id_usuario " &
            "order by p.fecha desc, p.id_pedido desc")
    End Function

    ' Cabecera de un pedido con los nombres de cliente y vendedor
    Public Function Obtener(idPedido As Integer) As DataTable
        Return objMan.listarComando(
            "select p.id_pedido, c.nombre as cliente, u.nombre as usuario, p.fecha, p.estado, p.total, p.fecha_termino " &
            "from pedidos p " &
            "inner join clientes c on c.id_cliente = p.id_cliente " &
            "inner join usuarios u on u.id_usuario = p.id_usuario " &
            "where p.id_pedido=" & idPedido)
    End Function

    ' estado: PENDIENTE, PAGADO, PARCIAL o CANCELADO
    Public Sub CambiarEstado(idPedido As Integer, estado As String)
        objMan.ejecutarComando("update pedidos set estado='" & Esc(estado) & "' where id_pedido=" & idPedido)
    End Sub

    ' Sincroniza pedidos.estado con la suma de pagos:
    ' sin pagos -> PENDIENTE, parcial (o con vuelto pendiente) -> PARCIAL,
    ' cubierto exacto -> PAGADO. Nunca toca un pedido CANCELADO.
    Public Sub ActualizarEstadoPago(idPedido As Integer)
        objMan.ejecutarComando(
            "update p set estado = case " &
            "  when p.estado = 'CANCELADO' then 'CANCELADO' " &
            "  when isnull(pg.pagado, 0) <= 0 then 'PENDIENTE' " &
            "  when p.total > 0 and isnull(pg.pagado, 0) = p.total then 'PAGADO' " &
            "  else 'PARCIAL' end " &
            "from pedidos p " &
            "outer apply (select sum(monto_total) as pagado from pagos where id_pedido = p.id_pedido) pg " &
            "where p.id_pedido = " & idPedido)
    End Sub

    Public Function ListarItems(idPedido As Integer) As DataTable
        Return objMan.listarComando("select * from pedido_item where id_pedido=" & idPedido)
    End Function

    Public Function ListarDetalles(idItem As Integer) As DataTable
        Return objMan.listarComando("select * from pedido_detalle where id_item=" & idItem & " and estado='ACTIVO'")
    End Function

    ' Cabecera de cada item del pedido (una pestaña en frmPedidoUnitario)
    Public Function ListarItemsDetallado(idPedido As Integer) As DataTable
        Return objMan.listarComando(
            "select pi.id_item, pi.tipo, pi.descripcion, pi.estado, pi.total, " &
            "medidas.ancho_cm, medidas.alto_cm " &
            "from pedido_item pi " &
            "outer apply ( " &
            "  select top 1 pd.ancho_cm, pd.alto_cm " &
            "  from pedido_detalle pd " &
            "  where pd.id_item = pi.id_item and pd.estado = 'ACTIVO' and pd.ancho_cm is not null " &
            "  order by pd.id_detalle " &
            ") medidas " &
            "where pi.id_pedido = " & idPedido & " and pi.estado <> 'CANCELADO' " &
            "order by pi.id_item")
    End Function

    ' Lineas de material de un item, con el nombre del producto y la pieza asociada
    Public Function ListarDetallesDeItem(idItem As Integer) As DataTable
        Return objMan.listarComando(
            "select pd.id_detalle, v.producto as material, v.unidad_costo, " &
            "pd.ancho_cm, pd.alto_cm, pd.largo_cm, pd.cantidad_cajas, " &
            "pd.accion, pd.subtotal, pd.id_variante, pd.id_compra_detalle, " &
            "ip.id_pieza as pieza_usada, ip.ancho_cm as pieza_ancho, " &
            "ip.alto_cm as pieza_alto, ip.largo_cm as pieza_largo, ip.origen as pieza_origen " &
            "from pedido_detalle pd " &
            "inner join vw_inv_variantes v on v.id_variante = pd.id_variante " &
            "left join inventario_piezas ip on ip.id_pieza = pd.id_pieza " &
            "where pd.id_item = " & idItem & " and pd.estado = 'ACTIVO' " &
            "order by pd.id_detalle")
    End Function

    ' Marca una linea de material como resuelta (p.ej. despues de confirmar el corte)
    Public Sub ResolverDetalle(idDetalle As Integer)
        objMan.ejecutarComando("update pedido_detalle set accion='USA_EXISTENTE', id_pieza=NULL where id_detalle=" & idDetalle)
    End Sub

    ' Marca la linea (detalle) como rota: la pieza usada pasa a DEFECTUOSA, la linea
    ' queda REEMPLAZADA y se crea una nueva linea con el mismo material y medidas.
    ' Devuelve la linea nueva (id_detalle, accion, id_pieza).
    Public Function MarcarItemRoto(idDetalle As Integer, Optional idUsuario As Integer? = Nothing) As DataTable
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_detalle", idDetalle},
            {"id_usuario", If(idUsuario.HasValue, CObj(idUsuario.Value), Nothing)}}
        Return objMan.listarProcedimiento("sp_ped_item_roto", parametros)
    End Function

    ' Cancela un item completo del pedido y libera sus piezas reservadas
    Public Sub CancelarItem(idItem As Integer, Optional idUsuario As Integer? = Nothing)
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_item", idItem},
            {"id_usuario", If(idUsuario.HasValue, CObj(idUsuario.Value), Nothing)}}
        objMan.ejecutarProcedimiento("sp_ped_cancelar_item", parametros)
    End Sub

    ' Cancela TODOS los items activos del pedido (libera piezas y recalcula el total)
    Public Sub Cancelar(idPedido As Integer, Optional idUsuario As Integer? = Nothing)
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_pedido", idPedido},
            {"id_usuario", If(idUsuario.HasValue, CObj(idUsuario.Value), Nothing)}}
        objMan.ejecutarProcedimiento("sp_ped_cancelar_pedido", parametros)
    End Sub

    ' Marca el pedido como terminado (entregado/instalado), sin importar el pago
    Public Sub Terminar(idPedido As Integer, Optional idUsuario As Integer? = Nothing)
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_pedido", idPedido},
            {"id_usuario", If(idUsuario.HasValue, CObj(idUsuario.Value), Nothing)}}
        objMan.ejecutarProcedimiento("sp_ped_terminar", parametros)
    End Sub

    ' Termina un solo item (solo si esta EN_PROCESO, con todos sus materiales a mano)
    Public Sub TerminarItem(idItem As Integer, Optional idUsuario As Integer? = Nothing)
        Dim parametros As New Dictionary(Of String, Object) From {
            {"id_item", idItem},
            {"id_usuario", If(idUsuario.HasValue, CObj(idUsuario.Value), Nothing)}}
        objMan.ejecutarProcedimiento("sp_ped_terminar_item", parametros)
    End Sub

    ' Promueve a EN_PROCESO los items cuyos detalles ya estan todos a mano
    Public Sub RecalcularEstados(idPedido As Integer)
        Dim parametros As New Dictionary(Of String, Object) From {{"id_pedido", idPedido}}
        objMan.ejecutarProcedimiento("sp_ped_recalcular_estados", parametros)
    End Sub

    Public Function TarifaSugerida(anchoCm As Decimal, altoCm As Decimal) As (aluminio As Decimal, silicona As Decimal)
        Dim perimetro As Decimal = 2 * (anchoCm + altoCm)
        Dim parametros As New Dictionary(Of String, Object) From {{"perimetro_cm", perimetro}}
        Dim dt = objMan.listarProcedimiento("sp_ped_tarifa_sugerida", parametros)
        If dt.Rows.Count = 0 Then Return (0D, 0D)
        Return (CDec(dt.Rows(0)("costo_aluminio")), CDec(dt.Rows(0)("costo_silicona")))
    End Function

    ' Registra el pedido completo (todos los items con sus detalles) en una sola transaccion
    Public Function Registrar(idCliente As Integer, idUsuario As Integer, items As List(Of ItemPedidoDTO)) As Integer
        Dim tblItems As New DataTable
        tblItems.Columns.Add("id_temp", GetType(Integer))
        tblItems.Columns.Add("tipo", GetType(String))
        tblItems.Columns.Add("etiqueta", GetType(String))
        tblItems.Columns.Add("ancho_cm", GetType(Decimal))
        tblItems.Columns.Add("alto_cm", GetType(Decimal))
        tblItems.Columns.Add("precio_materiales", GetType(Decimal))
        tblItems.Columns.Add("costo_extra", GetType(Decimal))
        tblItems.Columns.Add("mano_obra", GetType(Decimal))
        tblItems.Columns.Add("total", GetType(Decimal))
        tblItems.Columns.Add("id_modelo", GetType(Integer))

        Dim tblDetalles As New DataTable
        tblDetalles.Columns.Add("id_temp", GetType(Integer))
        tblDetalles.Columns.Add("id_item_temp", GetType(Integer))
        tblDetalles.Columns.Add("id_variante", GetType(Integer))
        tblDetalles.Columns.Add("accion", GetType(String))
        tblDetalles.Columns.Add("id_pieza", GetType(Integer))
        tblDetalles.Columns.Add("ancho_cm", GetType(Decimal))
        tblDetalles.Columns.Add("alto_cm", GetType(Decimal))
        tblDetalles.Columns.Add("largo_cm", GetType(Decimal))
        tblDetalles.Columns.Add("cantidad_cajas", GetType(Integer))
        tblDetalles.Columns.Add("subtotal", GetType(Decimal))

        Dim tblMedidas As New DataTable
        tblMedidas.Columns.Add("id_detalle_temp", GetType(Integer))
        tblMedidas.Columns.Add("etiqueta", GetType(String))
        tblMedidas.Columns.Add("valor_cm", GetType(Decimal))

        Dim idItemTemp As Integer = 0
        Dim idDetalleTemp As Integer = 0

        For Each it In items
            idItemTemp += 1
            tblItems.Rows.Add(idItemTemp, it.Tipo, DBNullIfEmpty(it.Etiqueta),
                               DBNull.Value, DBNull.Value,
                               NumOrDbNull(it.PrecioMateriales), it.CostoExtra, it.ManoObra, it.Total,
                               If(it.IdModelo.HasValue, CType(it.IdModelo.Value, Object), DBNull.Value))

            For Each det In it.Detalles
                idDetalleTemp += 1
                tblDetalles.Rows.Add(idDetalleTemp, idItemTemp, det.IdVariante, det.Accion,
                                      If(det.IdPieza.HasValue, CType(det.IdPieza.Value, Object), DBNull.Value),
                                      NumOrDbNull(det.AnchoCm), NumOrDbNull(det.AltoCm), NumOrDbNull(det.LargoCm),
                                      If(det.CantidadCajas.HasValue, CType(det.CantidadCajas.Value, Object), DBNull.Value),
                                      det.Subtotal)
            Next
        Next

        Dim pItems As New SqlParameter("@Items", SqlDbType.Structured) With {.TypeName = "dbo.TVP_PedidoItem", .Value = tblItems}
        Dim pDetalles As New SqlParameter("@Detalles", SqlDbType.Structured) With {.TypeName = "dbo.TVP_PedidoDetalle", .Value = tblDetalles}
        Dim pMedidas As New SqlParameter("@Medidas", SqlDbType.Structured) With {.TypeName = "dbo.TVP_PedidoMedida", .Value = tblMedidas}
        Dim pCliente As New SqlParameter("@id_cliente", idCliente)
        Dim pUsuario As New SqlParameter("@id_usuario", idUsuario)
        Dim pIdPedido As New SqlParameter("@id_pedido", SqlDbType.Int) With {.Direction = ParameterDirection.Output}

        objMan.EjecutarProcedimientoConParametros("sp_ped_registrar",
            New List(Of SqlParameter) From {pCliente, pUsuario, pItems, pDetalles, pMedidas, pIdPedido})

        Return CInt(pIdPedido.Value)
    End Function

    Private Function NumOrDbNull(valor As Decimal?) As Object
        Return If(valor.HasValue, CType(valor.Value, Object), DBNull.Value)
    End Function

    Private Function DBNullIfEmpty(valor As String) As Object
        Return If(String.IsNullOrWhiteSpace(valor), CType(DBNull.Value, Object), valor)
    End Function

End Class

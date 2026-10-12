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

    ' estado: PENDIENTE, PAGADO, PARCIAL o CANCELADO
    Public Sub CambiarEstado(idPedido As Integer, estado As String)
        objMan.ejecutarComando("update pedidos set estado='" & Esc(estado) & "' where id_pedido=" & idPedido)
    End Sub

    Public Function ListarItems(idPedido As Integer) As DataTable
        Return objMan.listarComando("select * from pedido_item where id_pedido=" & idPedido)
    End Function

    Public Function ListarDetalles(idItem As Integer) As DataTable
        Return objMan.listarComando("select * from pedido_detalle where id_item=" & idItem & " and estado='ACTIVO'")
    End Function

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
                               NumOrDbNull(it.PrecioMateriales), it.CostoExtra, it.ManoObra, it.Total)

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

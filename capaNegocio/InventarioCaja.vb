Imports capaDatos

' TORNILLOS y TARUGOS: se manejan por cantidad de cajas.
' Todo movimiento pasa por sp_inv_mover_cajas, que valida que el stock no quede
' negativo y lo registra en el kardex (movimientos_inventario).
Public Class InventarioCaja
    Dim objMan As New clsMantenimiento

    ' Variantes vendidas por caja con su stock y stock mínimo
    Public Function Listar() As DataTable
        Return objMan.consultarConParametros(
            "select id_variante, categoria, producto, stock_cajas, stock_minimo, bajo_minimo, precio_unitario " &
            "from vw_inv_resumen where unidad_costo = 'CAJA' order by categoria, producto")
    End Function

    Public Function ObtenerStock(idVariante As Integer) As Integer
        Dim dt As DataTable = objMan.consultarConParametros("select stock_cajas from inventario_cajas where id_variante = @id",
            New Dictionary(Of String, Object) From {{"id", idVariante}})
        If dt.Rows.Count > 0 Then Return CInt(dt.Rows(0)(0))
        Return 0
    End Function

    ' Stock con el que arranca la variante (carga inicial)
    Public Sub Inicializar(idVariante As Integer, stockInicial As Integer, Optional idUsuario As Integer? = Nothing)
        Mover(idVariante, "AJUSTE", stockInicial, "INICIAL", idUsuario, Nothing, Nothing, "Stock inicial")
    End Sub

    ' motivo: COMPRA, INICIAL o AJUSTE. Devuelve el stock resultante
    Public Function SumarStock(idVariante As Integer, cantidad As Integer, Optional motivo As String = "COMPRA",
                               Optional idUsuario As Integer? = Nothing, Optional idCompraDetalle As Integer? = Nothing,
                               Optional observacion As String = Nothing) As Integer
        Return Mover(idVariante, "ENTRADA", cantidad, motivo, idUsuario, Nothing, idCompraDetalle, observacion)
    End Function

    ' motivo: VENTA, DEFECTO o AJUSTE. Falla si no hay stock suficiente. Devuelve el stock resultante
    Public Function RestarStock(idVariante As Integer, cantidad As Integer, Optional motivo As String = "VENTA",
                                Optional idUsuario As Integer? = Nothing, Optional idPedido As Integer? = Nothing,
                                Optional observacion As String = Nothing) As Integer
        Return Mover(idVariante, "SALIDA", cantidad, motivo, idUsuario, idPedido, Nothing, observacion)
    End Function

    ' Conteo físico: el stock pasa a ser exactamente stockReal (la diferencia queda en el kardex)
    Public Function AjustarStock(idVariante As Integer, stockReal As Integer, Optional idUsuario As Integer? = Nothing,
                                 Optional observacion As String = Nothing) As Integer
        Return Mover(idVariante, "AJUSTE", stockReal, "AJUSTE", idUsuario, Nothing, Nothing, observacion)
    End Function

    Private Function Mover(idVariante As Integer, tipo As String, cantidad As Integer, motivo As String, idUsuario As Integer?,
                           idPedido As Integer?, idCompraDetalle As Integer?, observacion As String) As Integer
        Dim dt As DataTable = objMan.listarProcedimiento("sp_inv_mover_cajas", New Dictionary(Of String, Object) From {
            {"id_variante", idVariante}, {"tipo", tipo}, {"cantidad", cantidad}, {"motivo", motivo},
            {"id_usuario", idUsuario}, {"id_pedido", idPedido}, {"id_compra_detalle", idCompraDetalle},
            {"observacion", If(String.IsNullOrWhiteSpace(observacion), Nothing, observacion.Trim())}})
        Return CInt(dt.Rows(0)("stock_resultante"))
    End Function
End Class

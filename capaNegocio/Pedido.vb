Imports System.Data

Public Class Pedido
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from pedidos order by fecha desc")
    End Function

    Public Function ListarPorCliente(idCliente As Integer) As DataTable
        Return objMan.listarComando("select * from pedidos where id_cliente=" & idCliente & " order by fecha")
    End Function

    Public Function Insertar(idCliente As Integer, idUsuario As Integer) As Integer
        Dim sql As String = "insert into pedidos (id_cliente, id_usuario, estado, total) values (" &
            idCliente & "," & idUsuario & ",'PENDIENTE',0); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub ActualizarTotal(idPedido As Integer, total As Decimal)
        objMan.ejecutarComando("update pedidos set total=" & Num(total) & " where id_pedido=" & idPedido)
    End Sub

    ' estado: PENDIENTE, PAGADO, PARCIAL o CANCELADO
    Public Sub CambiarEstado(idPedido As Integer, estado As String)
        objMan.ejecutarComando("update pedidos set estado='" & Esc(estado) & "' where id_pedido=" & idPedido)
    End Sub

    ' Detalle de Pedido
    Public Function ListarPorPedido(idPedido As Integer) As DataTable
        Return objMan.listarComando("select * from pedido_detalle where id_pedido=" & idPedido)
    End Function

    ' Línea de vidrio o aluminio: referencia la pieza específica usada
    Public Function InsertarPieza(idPedido As Integer, idVariante As Integer, idPieza As Integer,
                                   anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?, precioUnitarioAplicado As Decimal) As Integer
        Dim subtotal As Decimal = CalcularSubtotal(anchoCm, altoCm, largoCm, precioUnitarioAplicado)
        Dim sql As String = "insert into pedido_detalle (id_pedido, id_variante, id_pieza, ancho_cm, alto_cm, largo_cm, precio_unitario_aplicado, subtotal) values (" &
            idPedido & "," & idVariante & "," & idPieza & "," & Num(anchoCm) & "," & Num(altoCm) & "," & Num(largoCm) & "," &
            Num(precioUnitarioAplicado) & "," & Num(subtotal) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Línea de tornillo/tarugo: se vende por caja, no referencia una pieza
    Public Function InsertarCajas(idPedido As Integer, idVariante As Integer, cantidadCajas As Integer, precioUnitarioAplicado As Decimal) As Integer
        Dim subtotal As Decimal = cantidadCajas * precioUnitarioAplicado
        Dim sql As String = "insert into pedido_detalle (id_pedido, id_variante, cantidad_cajas, precio_unitario_aplicado, subtotal) values (" &
            idPedido & "," & idVariante & "," & cantidadCajas & "," & Num(precioUnitarioAplicado) & "," & Num(subtotal) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Private Function CalcularSubtotal(anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?, precioUnitario As Decimal) As Decimal
        If anchoCm.HasValue AndAlso altoCm.HasValue Then
            Return anchoCm.Value * altoCm.Value * precioUnitario   ' vidrio: cm2
        ElseIf largoCm.HasValue Then
            Return largoCm.Value * precioUnitario                  ' aluminio: cm lineal
        End If
        Return 0
    End Function
End Class

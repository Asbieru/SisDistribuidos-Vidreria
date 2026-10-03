Imports System.Data

Public Class Compra
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from compras order by fecha desc")
    End Function

    Public Function ListarPorFecha(desde As Date, hasta As Date) As DataTable
        Dim sql As String = "select * from compras where fecha between '" & desde.ToString("yyyy-MM-dd") &
            "' and '" & hasta.ToString("yyyy-MM-dd") & "' order by fecha"
        Return objMan.listarComando(sql)
    End Function

    ' motivo: 'REPOSICION' o 'REEMPLAZO_DEFECTO'. idPedidoOrigen solo aplica al segundo caso
    Public Function Insertar(proveedor As String, fecha As Date, total As Decimal, motivo As String, idPedidoOrigen As Integer?) As Integer
        Dim sql As String = "insert into compras (proveedor, fecha, total, motivo, id_pedido_origen) values ('" &
            Esc(proveedor) & "','" & fecha.ToString("yyyy-MM-dd") & "'," & Num(total) & ",'" & Esc(motivo) & "'," & NumOrNull(idPedidoOrigen) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Detalle de Compra
    Public Function ListarPorCompra(idCompra As Integer) As DataTable
        Return objMan.listarComando("select * from compra_detalle where id_compra=" & idCompra)
    End Function

    Public Function Insertar(idCompra As Integer, idVariante As Integer, cantidad As Decimal, costoUnitario As Decimal) As Integer
        Dim subtotal As Decimal = cantidad * costoUnitario
        Dim sql As String = "insert into compra_detalle (id_compra, id_variante, cantidad, costo_unitario, subtotal) values (" &
            idCompra & "," & idVariante & "," & Num(cantidad) & "," & Num(costoUnitario) & "," & Num(subtotal) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function
End Class

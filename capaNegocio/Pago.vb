Imports capaDatos
Imports System.Linq
Imports System.Collections.Generic

Public Class Pago
    Dim objMan As New clsMantenimiento

    Public Function ListarPorPedido(idPedido As Integer) As DataTable
        Return objMan.listarComando("select * from pagos where id_pedido=" & idPedido)
    End Function

    Public Function Insertar(idPedido As Integer, montoTotal As Decimal) As Integer
        Dim sql As String = "insert into pagos (id_pedido, monto_total) values (" & idPedido & "," & Num(montoTotal) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Registra el pago y lo reparte entre uno o más métodos.
    ' metodosYMontos: clave = id_metodo_pago, valor = monto pagado con ese método.
    Public Function RegistrarConReparto(idPedido As Integer, metodosYMontos As Dictionary(Of Integer, Decimal)) As Integer
        Dim montoTotal As Decimal = metodosYMontos.Values.Sum()
        Dim idPago As Integer = Insertar(idPedido, montoTotal)
        For Each par In metodosYMontos
            InsertarDetallePago(idPago, par.Key, par.Value)
        Next
        Return idPago
    End Function

    ' Detalle de Pago
    Public Function ListarPorPago(idPago As Integer) As DataTable
        Dim sql As String = "select dp.id_detalle_pago, mp.nombre as metodo, dp.monto " &
                             "from detalle_pago dp inner join metodos_pago mp on mp.id_metodo_pago = dp.id_metodo_pago " &
                             "where dp.id_pago=" & idPago
        Return objMan.listarComando(sql)
    End Function

    Public Function InsertarDetallePago(idPago As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        Dim sql As String = "insert into detalle_pago (id_pago, id_metodo_pago, monto) values (" &
            idPago & "," & idMetodoPago & "," & Num(monto) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Un pago simple: una cabecera con un unico metodo y monto
    Public Function RegistrarDetalle(idPedido As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        Dim idPago As Integer = Insertar(idPedido, monto)
        InsertarDetallePago(idPago, idMetodoPago, monto)
        Return idPago
    End Function

    ' Detalles de pago de un pedido, con la fecha de su cabecera
    Public Function ListarDetallesPorPedido(idPedido As Integer) As DataTable
        Return objMan.listarComando(
            "select dp.id_detalle_pago, dp.id_pago, dp.id_metodo_pago, mp.nombre as metodo, dp.monto, p.fecha " &
            "from detalle_pago dp " &
            "inner join pagos p on p.id_pago = dp.id_pago " &
            "inner join metodos_pago mp on mp.id_metodo_pago = dp.id_metodo_pago " &
            "where p.id_pedido = " & idPedido & " " &
            "order by p.fecha, dp.id_detalle_pago")
    End Function

    ' Total pagado de un pedido (suma de las cabeceras)
    Public Function TotalPagado(idPedido As Integer) As Decimal
        Dim dt As DataTable = objMan.listarComando(
            "select coalesce(sum(monto_total),0) as pagado from pagos where id_pedido=" & idPedido)
        If dt Is Nothing OrElse dt.Rows.Count = 0 OrElse IsDBNull(dt.Rows(0)("pagado")) Then Return 0D
        Return CDec(dt.Rows(0)("pagado"))
    End Function

    ' Monto del ultimo detalle de pago registrado en el pedido (el vuelto no puede superarlo)
    Public Function UltimoMonto(idPedido As Integer) As Decimal
        Dim dt As DataTable = objMan.listarComando(
            "select top 1 dp.monto from detalle_pago dp " &
            "inner join pagos p on p.id_pago = dp.id_pago " &
            "where p.id_pedido = " & idPedido & " " &
            "order by dp.id_detalle_pago desc")
        If dt Is Nothing OrElse dt.Rows.Count = 0 OrElse IsDBNull(dt.Rows(0)("monto")) Then Return 0D
        Return CDec(dt.Rows(0)("monto"))
    End Function

    ' Cambia el metodo y el monto de un detalle; sincroniza el total de su cabecera
    Public Sub ActualizarDetalle(idDetallePago As Integer, idMetodoPago As Integer, monto As Decimal)
        objMan.ejecutarComando("update detalle_pago set id_metodo_pago=" & idMetodoPago &
                               ", monto=" & Num(monto) &
                               " where id_detalle_pago=" & idDetallePago)
        objMan.ejecutarComando(
            "update pagos set monto_total = (select coalesce(sum(monto),0) from detalle_pago where id_pago = pagos.id_pago) " &
            "where id_pago = (select id_pago from detalle_pago where id_detalle_pago=" & idDetallePago & ")")
    End Sub

    ' Elimina un detalle; si su cabecera queda vacia, se elimina tambien
    Public Sub EliminarDetalle(idDetallePago As Integer)
        Dim dt As DataTable = objMan.listarComando("select id_pago from detalle_pago where id_detalle_pago=" & idDetallePago)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return
        Dim idPago As Integer = CInt(dt.Rows(0)("id_pago"))
        objMan.ejecutarComando("delete from detalle_pago where id_detalle_pago=" & idDetallePago)
        objMan.ejecutarComando("delete from pagos where id_pago=" & idPago &
                               " and not exists (select 1 from detalle_pago where id_pago=" & idPago & ")")
    End Sub
End Class

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
End Class

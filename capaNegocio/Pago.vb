Imports capaDatos
Imports System.Linq
Imports System.Collections.Generic

' Movimientos de caja de un pedido.
' Cada fila de dbo.pagos es un movimiento: PAGO (ingreso), VUELTO (cambio)
' o DEVOLUCION (reembolso). Los vueltos y devoluciones se guardan con
' monto_total negativo para que la suma de la columna sea el dinero neto.
Public Class Pago
    Dim objMan As New clsMantenimiento

    ' ------------------------- Consultas -------------------------

    Public Function ListarPorPedido(idPedido As Integer) As DataTable
        Return objMan.listarComando(
            "select * from pagos where id_pedido=" & idPedido & " order by fecha, id_pago")
    End Function

    Public Function ListarPorPago(idPago As Integer) As DataTable
        Dim sql As String = "select dp.id_detalle_pago, mp.nombre as metodo, dp.monto " &
                             "from detalle_pago dp inner join metodos_pago mp on mp.id_metodo_pago = dp.id_metodo_pago " &
                             "where dp.id_pago=" & idPago
        Return objMan.listarComando(sql)
    End Function

    Public Function ListarDetallesPorPedido(idPedido As Integer) As DataTable
        Return objMan.listarComando(
            "select dp.id_detalle_pago, dp.id_pago, dp.id_metodo_pago, mp.nombre as metodo, " &
            "dp.monto, p.fecha, p.tipo, p.id_pago_padre " &
            "from detalle_pago dp " &
            "inner join pagos p on p.id_pago = dp.id_pago " &
            "inner join metodos_pago mp on mp.id_metodo_pago = dp.id_metodo_pago " &
            "where p.id_pedido = " & idPedido & " " &
            "order by p.fecha desc, dp.id_detalle_pago desc")
    End Function

    ' ------------------------- Totales -------------------------

    ' Dinero neto cobrado: pagos menos vueltos y devoluciones.
    Public Function TotalPagado(idPedido As Integer) As Decimal
        Return DecimalDe("select coalesce(sum(monto_total),0) as valor from pagos " &
                         "where id_pedido=" & idPedido)
    End Function

    Public Function TotalDevuelto(idPedido As Integer) As Decimal
        Return DecimalDe("select coalesce(sum(abs(monto_total)),0) as valor from pagos " &
                         "where id_pedido=" & idPedido & " and tipo = N'DEVOLUCION'")
    End Function

    Public Function TotalVuelto(idPedido As Integer) As Decimal
        Return DecimalDe("select coalesce(sum(abs(monto_total)),0) as valor from pagos " &
                         "where id_pedido=" & idPedido & " and tipo = N'VUELTO'")
    End Function

    Public Function UltimoMonto(idPedido As Integer) As Decimal
        Return DecimalDe("select top 1 dp.monto as valor from detalle_pago dp " &
                         "inner join pagos p on p.id_pago = dp.id_pago " &
                         "where p.id_pedido = " & idPedido & " and p.tipo = N'PAGO' " &
                         "order by dp.id_detalle_pago desc")
    End Function

    ' ------------------------- Registro -------------------------

    Public Function Insertar(idPedido As Integer, montoTotal As Decimal) As Integer
        ValidarPedidoNoCancelado(idPedido)
        Return InsertarMovimiento(idPedido, montoTotal, "PAGO", Nothing)
    End Function

    ' Registra el pago y lo reparte entre uno o más métodos.
    ' metodosYMontos: clave = id_metodo_pago, valor = monto cobrado con ese método.
    Public Function RegistrarConReparto(idPedido As Integer, metodosYMontos As Dictionary(Of Integer, Decimal)) As Integer
        ValidarPedidoNoCancelado(idPedido)
        If metodosYMontos Is Nothing OrElse metodosYMontos.Count = 0 Then
            Throw New Exception("Registre al menos un método de pago.")
        End If

        Dim montoTotal As Decimal = metodosYMontos.Values.Sum()
        Dim idPago As Integer = InsertarMovimiento(idPedido, montoTotal, "PAGO", Nothing)
        For Each par In metodosYMontos
            InsertarDetallePago(idPago, par.Key, par.Value)
        Next
        Return idPago
    End Function

    ' Un pago simple: una cabecera con un unico metodo y monto.
    Public Function RegistrarDetalle(idPedido As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        ValidarPedidoNoCancelado(idPedido)
        If monto = 0D Then Throw New Exception("El monto no puede ser cero.")

        Dim tipo As String = If(monto < 0D, "VUELTO", "PAGO")
        Dim idPago As Integer = InsertarMovimiento(idPedido, monto, tipo, Nothing)
        InsertarDetallePago(idPago, idMetodoPago, monto)
        Return idPago
    End Function

    Public Function InsertarDetallePago(idPago As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        ' El metodo es publico: un pedido cancelado solo admite movimientos de devolucion.
        Dim dtTipo As DataTable = objMan.listarComando(
            "select p.tipo, pe.estado from pagos p " &
            "inner join pedidos pe on pe.id_pedido = p.id_pedido " &
            "where p.id_pago = " & idPago)
        If dtTipo IsNot Nothing AndAlso dtTipo.Rows.Count > 0 Then
            Dim tipo As String = Convert.ToString(dtTipo.Rows(0)("tipo"))
            Dim estado As String = Convert.ToString(dtTipo.Rows(0)("estado"))
            If String.Equals(estado, "CANCELADO", StringComparison.OrdinalIgnoreCase) AndAlso
               tipo <> "DEVOLUCION" Then
                Throw New Exception("El pedido está cancelado: no se puede ingresar dinero. " &
                                    "Solo se permiten devoluciones.")
            End If
        End If

        Dim sql As String = "insert into detalle_pago (id_pago, id_metodo_pago, monto) values (" &
            idPago & "," & idMetodoPago & "," & Num(monto) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Registra un pago con su vuelto. El vuelto se calcula en el servidor:
    ' no se confía en el monto que envía la web. Devuelve el vuelto registrado.
    Public Function RegistrarPagoConVuelto(idPedido As Integer, idMetodoPago As Integer,
                                           monto As Decimal, idMetodoVuelto As Integer) As Decimal
        ValidarPedidoNoCancelado(idPedido)
        If monto <= 0D Then Throw New Exception("Ingrese una cantidad válida mayor a cero.")
        If idMetodoPago <= 0 Then Throw New Exception("Seleccione un método de pago.")

        Dim totalDelPedido As Decimal = TotalPedido(idPedido)
        Dim saldo As Decimal = totalDelPedido - TotalPagado(idPedido)
        If saldo < 0D Then saldo = 0D

        If totalDelPedido > 0D AndAlso saldo <= 0.005D Then
            Throw New Exception("El pedido ya está pagado en su totalidad. " &
                        "No se pueden registrar nuevos pagos; puede editar o eliminar los pagos existentes.")
        End If

        Dim vuelto As Decimal = 0D
        If totalDelPedido > 0D AndAlso monto > saldo Then vuelto = monto - saldo

        If vuelto > 0D AndAlso idMetodoVuelto <= 0 Then
            Throw New Exception("Seleccione el método con el que se registrará el vuelto.")
        End If

        Dim idPago As Integer = InsertarMovimiento(idPedido, monto, "PAGO", Nothing)
        InsertarDetallePago(idPago, idMetodoPago, monto)

        If vuelto > 0D Then
            Dim idVuelto As Integer = InsertarMovimiento(idPedido, -vuelto, "VUELTO", idPago)
            InsertarDetallePago(idVuelto, idMetodoVuelto, -vuelto)
        End If

        Return vuelto
    End Function

    ' Edita un pago existente y ajusta o elimina su vuelto.
    ' Devuelve el vuelto que quedó registrado.
    Public Function ActualizarPagoConVuelto(idDetallePago As Integer, idMetodoPago As Integer,
                                            monto As Decimal, idMetodoVuelto As Integer) As Decimal
        If monto <= 0D Then Throw New Exception("Ingrese una cantidad válida mayor a cero.")
        If idMetodoPago <= 0 Then Throw New Exception("Seleccione un método de pago.")

        Dim movimiento As DataRow = MovimientoDeDetalle(idDetallePago)
        If movimiento Is Nothing Then Throw New Exception("El pago ya no existe.")
        If Convert.ToString(movimiento("tipo")) <> "PAGO" Then
            Throw New Exception("El vuelto y las devoluciones se ajustan automáticamente desde su pago. " &
                                "No se editan por separado.")
        End If

        Dim idPedido As Integer = CInt(movimiento("id_pedido"))
        ValidarPedidoNoCancelado(idPedido)

        Dim idPago As Integer = CInt(movimiento("id_pago"))
        Dim montoAnterior As Decimal = CDec(movimiento("monto_total"))

        Dim hijo As DataRow = VueltoDePago(idPago)
        Dim montoVueltoAnterior As Decimal = If(hijo Is Nothing, 0D, CDec(hijo("monto_total")))

        ' Lo que cubre este pedido sin contar este pago ni su vuelto.
        Dim pagadoSinPareja As Decimal = TotalPagado(idPedido) - montoAnterior - montoVueltoAnterior
        Dim saldo As Decimal = TotalPedido(idPedido) - pagadoSinPareja
        If saldo < 0D Then saldo = 0D

        Dim netoNuevo As Decimal = If(monto > saldo, saldo, monto)
        Dim vueltoNuevo As Decimal = monto - netoNuevo

        Dim metodoVuelto As Integer = idMetodoVuelto
        If hijo IsNot Nothing AndAlso metodoVuelto <= 0 Then
            Dim dtMetodo As DataTable = objMan.listarComando(
                "select top 1 id_metodo_pago from detalle_pago where id_pago=" & CInt(hijo("id_pago")))
            If dtMetodo IsNot Nothing AndAlso dtMetodo.Rows.Count > 0 Then
                metodoVuelto = CInt(dtMetodo.Rows(0)("id_metodo_pago"))
            End If
        End If
        If vueltoNuevo > 0D AndAlso metodoVuelto <= 0 Then
            Throw New Exception("Seleccione el método con el que se registrará el vuelto.")
        End If

        ' El pago guarda el monto bruto recibido; el vuelto queda como egreso aparte.
        objMan.ejecutarComando("update detalle_pago set id_metodo_pago=" & idMetodoPago &
                               ", monto=" & Num(monto) &
                               " where id_detalle_pago=" & idDetallePago)
        SincronizarCabecera(idPago)

        If vueltoNuevo > 0D Then
            If hijo Is Nothing Then
                Dim idVuelto As Integer = InsertarMovimiento(idPedido, -vueltoNuevo, "VUELTO", idPago)
                InsertarDetallePago(idVuelto, metodoVuelto, -vueltoNuevo)
            Else
                Dim idVuelto As Integer = CInt(hijo("id_pago"))
                objMan.ejecutarComando("update detalle_pago set id_metodo_pago=" & metodoVuelto &
                                       ", monto=" & Num(-vueltoNuevo) &
                                       " where id_pago=" & idVuelto)
                SincronizarCabecera(idVuelto)
            End If
        ElseIf hijo IsNot Nothing Then
            EliminarPagoInterno(CInt(hijo("id_pago")))
        End If

        Return vueltoNuevo
    End Function

    ' Elimina el pago junto con su vuelto, si lo tiene.
    ' Una devolución se puede eliminar por separado para corregir errores.
    Public Sub EliminarPagoConVuelto(idDetallePago As Integer)
        Dim movimiento As DataRow = MovimientoDeDetalle(idDetallePago)
        If movimiento Is Nothing Then Throw New Exception("El pago ya no existe.")

        Dim tipo As String = Convert.ToString(movimiento("tipo"))
        If tipo = "VUELTO" Then
            Throw New Exception("El vuelto se elimina automáticamente desde su pago.")
        End If

        Dim idPedido As Integer = CInt(movimiento("id_pedido"))
        If tipo = "PAGO" Then ValidarPedidoNoCancelado(idPedido)

        Dim idPago As Integer = CInt(movimiento("id_pago"))
        If tipo = "PAGO" Then
            Dim dtDev As DataTable = objMan.listarComando(
                "select count(*) as n from pagos where tipo = N'DEVOLUCION' and id_pago_padre = " & idPago)
            If dtDev IsNot Nothing AndAlso dtDev.Rows.Count > 0 AndAlso CInt(dtDev.Rows(0)("n")) > 0 Then
                Throw New Exception("Este pago tiene devoluciones registradas. " &
                                    "Elimine primero las devoluciones y vuelva a intentarlo.")
            End If

            Dim hijo As DataRow = VueltoDePago(idPago)
            If hijo IsNot Nothing Then EliminarPagoInterno(CInt(hijo("id_pago")))
        End If
        EliminarPagoInterno(idPago)
    End Sub

    ' Devuelve al cliente dinero ya cobrado. Queda como egreso (monto negativo).
    Public Function RegistrarDevolucion(idPedido As Integer, idMetodoPago As Integer, monto As Decimal) As Integer
        If monto <= 0D Then Throw New Exception("Ingrese una cantidad válida mayor a cero.")
        If idMetodoPago <= 0 Then Throw New Exception("Seleccione el método de la devolución.")

        Dim pagado As Decimal = TotalPagado(idPedido)
        If pagado <= 0.005D Then
            Throw New Exception("El pedido no tiene dinero cobrado para devolver.")
        End If
        If monto > pagado + 0.01D Then
            Throw New Exception("La devolución no puede superar el dinero cobrado (S/. " &
                                pagado.ToString("0.00", Globalization.CultureInfo.InvariantCulture) & ").")
        End If

        Dim idPadre As Integer? = Nothing
        Dim dtPadre As DataTable = objMan.listarComando(
            "select max(id_pago) as id from pagos where id_pedido=" & idPedido & " and tipo = N'PAGO'")
        If dtPadre IsNot Nothing AndAlso dtPadre.Rows.Count > 0 AndAlso Not IsDBNull(dtPadre.Rows(0)("id")) Then
            idPadre = CInt(dtPadre.Rows(0)("id"))
        End If

        Dim idDevolucion As Integer = InsertarMovimiento(idPedido, -monto, "DEVOLUCION", idPadre)
        InsertarDetallePago(idDevolucion, idMetodoPago, -monto)
        Return idDevolucion
    End Function

    ' ------------------------- Compatibilidad -------------------------

    ' Cambia el metodo y el monto de un detalle; sincroniza el total de su cabecera.
    Public Sub ActualizarDetalle(idDetallePago As Integer, idMetodoPago As Integer, monto As Decimal)
        Dim movimiento As DataRow = MovimientoDeDetalle(idDetallePago)
        If movimiento IsNot Nothing Then ValidarPedidoNoCancelado(CInt(movimiento("id_pedido")))

        objMan.ejecutarComando("update detalle_pago set id_metodo_pago=" & idMetodoPago &
                               ", monto=" & Num(monto) &
                               " where id_detalle_pago=" & idDetallePago)
        Dim dt As DataTable = objMan.listarComando(
            "select id_pago from detalle_pago where id_detalle_pago=" & idDetallePago)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            SincronizarCabecera(CInt(dt.Rows(0)("id_pago")))
        End If
    End Sub

    ' Elimina un detalle; si su cabecera queda vacia, se elimina tambien.
    Public Sub EliminarDetalle(idDetallePago As Integer)
        Dim movimiento As DataRow = MovimientoDeDetalle(idDetallePago)
        If movimiento IsNot Nothing Then ValidarPedidoNoCancelado(CInt(movimiento("id_pedido")))

        Dim dt As DataTable = objMan.listarComando("select id_pago from detalle_pago where id_detalle_pago=" & idDetallePago)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return
        Dim idPago As Integer = CInt(dt.Rows(0)("id_pago"))
        objMan.ejecutarComando("delete from detalle_pago where id_detalle_pago=" & idDetallePago)
        objMan.ejecutarComando("delete from pagos where id_pago=" & idPago &
                               " and not exists (select 1 from detalle_pago where id_pago=" & idPago & ")")
    End Sub

    ' ------------------------- Internos -------------------------

    Private Function InsertarMovimiento(idPedido As Integer, monto As Decimal,
                                        tipo As String, idPagoPadre As Integer?) As Integer
        Dim sql As String = "insert into pagos (id_pedido, monto_total, tipo, id_pago_padre) values (" &
            idPedido & "," & Num(monto) & ",N'" & tipo & "'," & NumOrNull(idPagoPadre) &
            "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' La cabecera siempre refleja la suma de sus detalles.
    Private Sub SincronizarCabecera(idPago As Integer)
        objMan.ejecutarComando(
            "update pagos set monto_total = (select coalesce(sum(monto),0) from detalle_pago " &
            "where id_pago = " & idPago & ") where id_pago = " & idPago)
    End Sub

    Private Sub EliminarPagoInterno(idPago As Integer)
        objMan.ejecutarComando("delete from detalle_pago where id_pago=" & idPago)
        objMan.ejecutarComando("delete from pagos where id_pago=" & idPago)
    End Sub

    Private Function MovimientoDeDetalle(idDetallePago As Integer) As DataRow
        Dim dt As DataTable = objMan.listarComando(
            "select dp.id_detalle_pago, dp.id_pago, p.id_pedido, p.tipo, p.monto_total " &
            "from detalle_pago dp inner join pagos p on p.id_pago = dp.id_pago " &
            "where dp.id_detalle_pago=" & idDetallePago)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Private Function VueltoDePago(idPago As Integer) As DataRow
        Dim dt As DataTable = objMan.listarComando(
            "select id_pago, monto_total from pagos " &
            "where tipo = N'VUELTO' and id_pago_padre=" & idPago)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return Nothing
        Return dt.Rows(0)
    End Function

    Private Function DecimalDe(consulta As String) As Decimal
        Dim dt As DataTable = objMan.listarComando(consulta)
        If dt Is Nothing OrElse dt.Rows.Count = 0 OrElse IsDBNull(dt.Rows(0)("valor")) Then Return 0D
        Return CDec(dt.Rows(0)("valor"))
    End Function

    Private Function EstadoPedido(idPedido As Integer) As String
        Dim dt As DataTable = objMan.listarComando("select estado from pedidos where id_pedido=" & idPedido)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Throw New Exception("El pedido no existe.")
        Return Convert.ToString(dt.Rows(0)("estado"))
    End Function

    Private Function TotalPedido(idPedido As Integer) As Decimal
        Dim dt As DataTable = objMan.listarComando("select total from pedidos where id_pedido=" & idPedido)
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then Throw New Exception("El pedido no existe.")
        If IsDBNull(dt.Rows(0)("total")) Then Return 0D
        Return CDec(dt.Rows(0)("total"))
    End Function

    Private Sub ValidarPedidoNoCancelado(idPedido As Integer)
        If String.Equals(EstadoPedido(idPedido), "CANCELADO", StringComparison.OrdinalIgnoreCase) Then
            Throw New Exception("El pedido está cancelado: no se puede ingresar dinero.")
        End If
    End Sub
End Class

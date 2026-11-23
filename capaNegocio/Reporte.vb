Option Strict On
Imports System.Data
Imports System.Collections.Generic
Imports System.Text
Imports capaDatos

' Consultas readonly usadas por el modulo de reportes de capaWeb.
' Todas las firmas son fijas porque el servicio WSLibrerias las resuelve
' por reflexion desde Ejecutar (sin sobrecargas ni parametros Optional).
Public Class Reporte
    Private ReadOnly objMan As New clsMantenimiento()

    ' Flujo de inventario resumido por mes y producto.
    ' Until: la web envia la fecha "hasta" normal y aqui se transforma en dia siguiente.
    Public Function FlujoInventario(desde As Date, hasta As Date,
                                    categoria As String, idVariante As Integer?) As DataTable
        ValidarRango(desde, hasta)

        Dim sql As String =
            "SELECT CONVERT(char(7), m.fecha, 120) AS periodo, " &
            "v.categoria, v.id_variante, v.producto, v.unidad_costo, " &
            "SUM(CASE WHEN m.tipo = 'ENTRADA' THEN m.cantidad ELSE 0 END) AS entradas, " &
            "SUM(CASE WHEN m.tipo = 'SALIDA' THEN m.cantidad ELSE 0 END) AS salidas, " &
            "SUM(CASE WHEN m.tipo = 'AJUSTE' THEN m.cantidad ELSE 0 END) AS ajustes, " &
            "SUM(CASE WHEN m.tipo = 'ESTADO' THEN 1 ELSE 0 END) AS cambios_estado, " &
            "SUM(CASE WHEN m.tipo = 'ENTRADA' THEN m.cantidad ELSE 0 END) " &
            "  - SUM(CASE WHEN m.tipo = 'SALIDA' THEN m.cantidad ELSE 0 END) " &
            "  + SUM(CASE WHEN m.tipo = 'AJUSTE' THEN m.cantidad ELSE 0 END) AS neto, " &
            "COUNT(*) AS movimientos " &
            "FROM dbo.movimientos_inventario m " &
            "INNER JOIN dbo.vw_inv_variantes v ON v.id_variante = m.id_variante " &
            "WHERE m.fecha >= @desde AND m.fecha < @hasta " &
            "AND (@categoria IS NULL OR v.categoria = @categoria) " &
            "AND (@id_variante IS NULL OR v.id_variante = @id_variante) " &
            "GROUP BY CONVERT(char(7), m.fecha, 120), v.categoria, v.id_variante, v.producto, v.unidad_costo " &
            "ORDER BY periodo DESC, v.producto"

        Dim parametros As New Dictionary(Of String, Object) From {
            {"desde", desde.Date},
            {"hasta", hasta.Date.AddDays(1)},
            {"categoria", Normalizar(categoria)},
            {"id_variante", If(idVariante.HasValue, CObj(idVariante.Value), Nothing)}
        }
        Return objMan.consultarConParametros(sql, parametros)
    End Function

    ' Pedidos registrados en un mes, con su situacion de cobro.
    Public Function PedidosDelMes(anio As Integer, mes As Integer, estadoPago As String) As DataTable
        If anio < 1900 OrElse anio > 2100 Then
            Throw New ArgumentException("El año debe estar entre 1900 y 2100.", "anio")
        End If
        If mes < 1 OrElse mes > 12 Then
            Throw New ArgumentException("El mes debe estar entre 1 y 12.", "mes")
        End If

        Dim estado As String = If(String.IsNullOrWhiteSpace(estadoPago), "TODOS", estadoPago.Trim().ToUpperInvariant())
        If Array.IndexOf(New String() {"TODOS", "PENDIENTE", "PARCIAL", "PAGADO", "EXCEDENTE"}, estado) < 0 Then
            Throw New ArgumentException("Seleccione un estado de pago válido.", "estadoPago")
        End If

        Dim sql As New StringBuilder()
        sql.AppendLine(
            ";WITH cobros AS (" &
            "SELECT pg.id_pedido, SUM(dp.monto) AS pagado " &
            "FROM dbo.pagos pg INNER JOIN dbo.detalle_pago dp ON dp.id_pago = pg.id_pago " &
            "GROUP BY pg.id_pedido), ")
        sql.AppendLine(
            "ventas AS (" &
            "SELECT p.id_pedido, p.fecha, c.nombre AS cliente, u.nombre AS vendedor, p.estado, " &
            "CASE WHEN p.fecha_termino IS NULL THEN 'SIN TERMINAR' ELSE 'TERMINADO' END AS estado_trabajo, " &
            "p.total, ISNULL(cb.pagado, 0) AS pagado, p.total - ISNULL(cb.pagado, 0) AS saldo, " &
            "CASE WHEN ISNULL(cb.pagado, 0) > p.total THEN 'EXCEDENTE' " &
            "     WHEN ISNULL(cb.pagado, 0) = p.total THEN 'PAGADO' " &
            "     WHEN ISNULL(cb.pagado, 0) <= 0 THEN 'PENDIENTE' " &
            "     ELSE 'PARCIAL' END AS estado_pago " &
            "FROM dbo.pedidos p " &
            "INNER JOIN dbo.clientes c ON c.id_cliente = p.id_cliente " &
            "INNER JOIN dbo.usuarios u ON u.id_usuario = p.id_usuario " &
            "LEFT JOIN cobros cb ON cb.id_pedido = p.id_pedido " &
            "WHERE p.estado <> 'CANCELADO' " &
            "AND p.fecha >= DATEFROMPARTS(@anio, @mes, 1) " &
            "AND p.fecha < DATEADD(MONTH, 1, DATEFROMPARTS(@anio, @mes, 1))) ")
        sql.AppendLine("SELECT * FROM ventas WHERE 1 = 1")

        Dim parametros As New Dictionary(Of String, Object) From {
            {"anio", anio},
            {"mes", mes}
        }
        If estado <> "TODOS" Then
            sql.AppendLine("AND estado_pago = @estado_pago")
            parametros.Add("estado_pago", estado)
        End If
        sql.AppendLine("ORDER BY fecha DESC, id_pedido DESC")

        Return objMan.consultarConParametros(sql.ToString(), parametros)
    End Function

    ' Evolucion mensual de compras dentro de un rango.
    Public Function ComprasPorTiempo(desde As Date, hasta As Date) As DataTable
        ValidarRango(desde, hasta)
        If desde.Date.AddMonths(60) < hasta.Date Then
            Throw New ArgumentException("El rango del reporte no puede superar los 60 meses.", "hasta")
        End If

        Dim sql As String =
            "SELECT CONVERT(char(7), fecha, 120) AS periodo, proveedor, " &
            "COUNT(*) AS cantidad_compras, " &
            "SUM(total) AS total_compras, " &
            "SUM(total) / COUNT(*) AS promedio_compra " &
            "FROM dbo.compras " &
            "WHERE fecha >= @desde AND fecha < @hasta " &
            "GROUP BY CONVERT(char(7), fecha, 120), proveedor " &
            "ORDER BY periodo DESC, proveedor"

        Dim parametros As New Dictionary(Of String, Object) From {
            {"desde", desde.Date},
            {"hasta", hasta.Date.AddDays(1)}
        }
        Return objMan.consultarConParametros(sql, parametros)
    End Function

    ' Clientes con pedidos no cancelados cuyo total supera lo pagado.
    Public Function ClientesConPendientes() As DataTable
        Dim sql As String =
            ";WITH cobros AS (" &
            "SELECT pg.id_pedido, SUM(dp.monto) AS pagado " &
            "FROM dbo.pagos pg INNER JOIN dbo.detalle_pago dp ON dp.id_pago = pg.id_pago " &
            "GROUP BY pg.id_pedido), " &
            "pendientes AS (" &
            "SELECT p.id_pedido, p.id_cliente, p.fecha, p.total, " &
            "ISNULL(cb.pagado, 0) AS pagado, p.total - ISNULL(cb.pagado, 0) AS saldo " &
            "FROM dbo.pedidos p LEFT JOIN cobros cb ON cb.id_pedido = p.id_pedido " &
            "WHERE p.estado <> 'CANCELADO') " &
            "SELECT c.id_cliente, c.nombre AS cliente, ISNULL(c.documento, '') AS documento, " &
            "ISNULL(c.telefono, '') AS telefono, COUNT(*) AS cantidad_pedidos, " &
            "SUM(p.total) AS total, SUM(p.pagado) AS pagado, " &
            "SUM(p.saldo) AS saldo_faltante, MIN(p.fecha) AS fecha_mas_antigua " &
            "FROM pendientes p " &
            "INNER JOIN dbo.clientes c ON c.id_cliente = p.id_cliente " &
            "WHERE p.saldo > 0 " &
            "GROUP BY c.id_cliente, c.nombre, c.documento, c.telefono " &
            "ORDER BY saldo_faltante DESC, c.nombre"

        Return objMan.consultarConParametros(sql)
    End Function

    ' Detalle de los pedidos con saldo de un cliente.
    Public Function DetallePendientesPorCliente(idCliente As Integer) As DataTable
        If idCliente <= 0 Then
            Throw New ArgumentException("Seleccione un cliente válido.", "idCliente")
        End If

        Dim sql As String =
            ";WITH cobros AS (" &
            "SELECT pg.id_pedido, SUM(dp.monto) AS pagado " &
            "FROM dbo.pagos pg INNER JOIN dbo.detalle_pago dp ON dp.id_pago = pg.id_pago " &
            "GROUP BY pg.id_pedido), " &
            "pendientes AS (" &
            "SELECT p.id_pedido, p.id_cliente, p.total, " &
            "ISNULL(cb.pagado, 0) AS pagado, p.total - ISNULL(cb.pagado, 0) AS saldo " &
            "FROM dbo.pedidos p LEFT JOIN cobros cb ON cb.id_pedido = p.id_pedido " &
            "WHERE p.estado <> 'CANCELADO') " &
            "SELECT pe.id_pedido, pe.fecha, pe.estado, " &
            "CASE WHEN pe.fecha_termino IS NULL THEN 'SIN TERMINAR' ELSE 'TERMINADO' END AS estado_trabajo, " &
            "p.total, p.pagado, p.saldo, " &
            "CASE WHEN p.pagado > p.total THEN 'EXCEDENTE' " &
            "     WHEN p.pagado = p.total THEN 'PAGADO' " &
            "     WHEN p.pagado <= 0 THEN 'PENDIENTE' " &
            "     ELSE 'PARCIAL' END AS estado_pago " &
            "FROM pendientes p " &
            "INNER JOIN dbo.pedidos pe ON pe.id_pedido = p.id_pedido " &
            "WHERE p.id_cliente = @id_cliente AND p.saldo > 0 " &
            "ORDER BY pe.fecha, pe.id_pedido"

        Return objMan.consultarConParametros(
            sql, New Dictionary(Of String, Object) From {{"id_cliente", idCliente}})
    End Function

    Private Shared Sub ValidarRango(desde As Date, hasta As Date)
        If desde.Date > hasta.Date Then
            Throw New ArgumentException("La fecha Desde no puede ser posterior a Hasta.", "desde")
        End If
    End Sub

    Private Shared Function Normalizar(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return Nothing
        Return texto.Trim()
    End Function

End Class

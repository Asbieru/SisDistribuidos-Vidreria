Imports capaDatos
Imports System.Data
Imports System.Collections.Generic
Imports System.Text

Public Class Venta
    Private objMan As New clsMantenimiento

    Public Function Listar(Optional desde As Date? = Nothing,
                           Optional hasta As Date? = Nothing,
                           Optional cliente As String = "",
                           Optional estadoPago As String = "TODOS") As DataTable

        If desde.HasValue AndAlso hasta.HasValue Then
            If desde.Value.Date > hasta.Value.Date Then
                Throw New Exception(
                    "La fecha Desde no puede ser posterior a Hasta.")
            End If
        End If

        Dim sql As New StringBuilder()

        ' Sumar los detalles de pago antes de unirlos con los pedidos.
        sql.AppendLine(
            ";WITH cobros AS (" &
            "SELECT pg.id_pedido, SUM(dp.monto) AS pagado " &
            "FROM pagos pg " &
            "INNER JOIN detalle_pago dp ON dp.id_pago=pg.id_pago " &
            "GROUP BY pg.id_pedido),")

        sql.AppendLine(
            "ventas AS (" &
            "SELECT p.id_pedido, c.nombre AS cliente, " &
            "u.nombre AS vendedor, p.fecha, " &
            "CASE WHEN p.fecha_termino IS NULL " &
            "THEN 'SIN TERMINAR' ELSE 'TERMINADO' END AS estado_trabajo, " &
            "p.total, COALESCE(cb.pagado,0) AS pagado, " &
            "p.total-COALESCE(cb.pagado,0) AS saldo, " &
            "CASE WHEN COALESCE(cb.pagado,0)>p.total THEN 'EXCEDENTE' " &
            "WHEN COALESCE(cb.pagado,0)=p.total THEN 'PAGADO' " &
            "WHEN COALESCE(cb.pagado,0)<=0 THEN 'PENDIENTE' " &
            "ELSE 'PARCIAL' END AS estado_pago " &
            "FROM pedidos p " &
            "INNER JOIN clientes c ON c.id_cliente=p.id_cliente " &
            "INNER JOIN usuarios u ON u.id_usuario=p.id_usuario " &
            "LEFT JOIN cobros cb ON cb.id_pedido=p.id_pedido " &
            "WHERE p.estado<>'CANCELADO')")

        sql.AppendLine("SELECT * FROM ventas WHERE 1=1")

        Dim parametros As New Dictionary(Of String, Object)

        If desde.HasValue Then
            sql.AppendLine("AND fecha>=@desde")
            parametros.Add("desde", desde.Value.Date)
        End If

        If hasta.HasValue Then
            sql.AppendLine("AND fecha<@hasta")
            parametros.Add("hasta", hasta.Value.Date.AddDays(1))
        End If

        If Not String.IsNullOrWhiteSpace(cliente) Then
            sql.AppendLine("AND CHARINDEX(@cliente, cliente)>0")
            parametros.Add("cliente", cliente.Trim())
        End If

        If estadoPago <> "TODOS" Then
            sql.AppendLine("AND estado_pago=@estado")
            parametros.Add("estado", estadoPago)
        End If

        sql.AppendLine("ORDER BY fecha DESC, id_pedido DESC")

        Return objMan.consultarConParametros(
            sql.ToString(), parametros)
    End Function
End Class
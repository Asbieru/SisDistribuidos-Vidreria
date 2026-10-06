Imports capaDatos

' Consultas generales del módulo de inventario: resumen de existencias,
' stock mínimo y kardex (historial de movimientos).
Public Class Inventario
    Dim objMan As New clsMantenimiento

    Public Shared ReadOnly Categorias As String() = {"VIDRIO", "ALUMINIO", "TORNILLO", "TARUGO"}
    Public Shared ReadOnly EstadosPieza As String() = {"DISPONIBLE", "RESERVADA", "VENDIDA", "CONSUMIDA", "DEFECTUOSA"}

    ' Existencias por variante. categoria Nothing = todas
    Public Function ListarResumen(Optional categoria As String = Nothing, Optional soloBajoMinimo As Boolean = False) As DataTable
        Dim sql As String = "select id_variante, categoria, producto, unidad_costo, existencias, stock_minimo, bajo_minimo, " &
                            "piezas_disponibles, piezas_reservadas, area_disponible_cm2, largo_disponible_cm, stock_cajas, precio_unitario " &
                            "from vw_inv_resumen " &
                            "where (@categoria is null or categoria = @categoria) and (@solo_bajo = 0 or bajo_minimo = 1) " &
                            "order by bajo_minimo desc, categoria, producto"
        Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {
            {"categoria", If(String.IsNullOrEmpty(categoria), Nothing, categoria)}, {"solo_bajo", soloBajoMinimo}})
    End Function

    ' Cuántas variantes están en o por debajo de su stock mínimo (para mostrar una alerta)
    Public Function ContarBajoMinimo() As Integer
        Dim dt As DataTable = objMan.consultarConParametros("select count(*) from vw_inv_resumen where bajo_minimo = 1")
        Return CInt(dt.Rows(0)(0))
    End Function

    ' Variantes para llenar combos. porPiezas = True: vidrio y aluminio; False: tornillos y tarugos
    Public Function ListarVariantes(porPiezas As Boolean) As DataTable
        Dim sql As String = "select id_variante, producto, unidad_costo from vw_inv_variantes " &
                            "where (@piezas = 1 and unidad_costo in ('CM2','CM_LINEAL')) or (@piezas = 0 and unidad_costo = 'CAJA') " &
                            "order by categoria, producto"
        Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {{"piezas", porPiezas}})
    End Function

    Public Function ListarTodasLasVariantes() As DataTable
        Return objMan.consultarConParametros("select id_variante, producto, unidad_costo from vw_inv_variantes order by categoria, producto")
    End Function

    Public Sub ActualizarStockMinimo(idVariante As Integer, stockMinimo As Integer)
        If stockMinimo < 0 Then Throw New Exception("El stock mínimo no puede ser negativo.")
        objMan.ejecutarConParametros("update producto_variante set stock_minimo = @minimo where id_variante = @id",
            New Dictionary(Of String, Object) From {{"minimo", stockMinimo}, {"id", idVariante}})
    End Sub

    ' Kardex entre dos fechas (ambas incluidas). idVariante Nothing = todas
    Public Function ListarMovimientos(desde As Date, hasta As Date, Optional idVariante As Integer? = Nothing) As DataTable
        Dim sql As String = "select id_movimiento, fecha, categoria, producto, id_pieza, tipo, motivo, cantidad, stock_resultante, " &
                            "usuario, id_pedido, observacion " &
                            "from vw_inv_movimientos " &
                            "where fecha >= @desde and fecha < @hasta and (@id_variante is null or id_variante = @id_variante) " &
                            "order by fecha desc, id_movimiento desc"
        Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {
            {"desde", desde.Date}, {"hasta", hasta.Date.AddDays(1)}, {"id_variante", idVariante}})
    End Function
End Class

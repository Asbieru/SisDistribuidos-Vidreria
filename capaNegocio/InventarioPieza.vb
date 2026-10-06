Imports capaDatos

' Piezas de VIDRIO (ancho x alto) y ALUMINIO (largo).
' Cada pieza física es una fila; todo cambio pasa por un procedimiento almacenado
' que además lo registra en el kardex (movimientos_inventario).
Public Class InventarioPieza
    Dim objMan As New clsMantenimiento

    ' Listado para pantalla. idVariante / estado en Nothing = todos
    Public Function Listar(Optional idVariante As Integer? = Nothing, Optional estado As String = Nothing) As DataTable
        Dim sql As String = "select id_pieza, producto, ancho_cm, alto_cm, largo_cm, area_cm2, estado, origen, id_pieza_origen, fecha_ingreso " &
                            "from vw_inv_piezas " &
                            "where (@id_variante is null or id_variante = @id_variante) and (@estado is null or estado = @estado) " &
                            "order by case estado when 'DISPONIBLE' then 0 when 'RESERVADA' then 1 else 2 end, id_pieza desc"
        Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {
            {"id_variante", idVariante}, {"estado", If(String.IsNullOrEmpty(estado), Nothing, estado)}})
    End Function

    ' Datos de una sola pieza (0 filas si no existe)
    Public Function Obtener(idPieza As Integer) As DataTable
        Return objMan.consultarConParametros("select * from vw_inv_piezas where id_pieza = @id",
            New Dictionary(Of String, Object) From {{"id", idPieza}})
    End Function

    ' Piezas DISPONIBLE que encajan en la medida pedida (vidrio: ancho+alto, aluminio: largo).
    ' Ordenadas de la más chica a la más grande: así se usan primero los retazos.
    Public Function BuscarDisponibles(idVariante As Integer, Optional anchoMin As Decimal? = Nothing,
                                       Optional altoMin As Decimal? = Nothing, Optional largoMin As Decimal? = Nothing) As DataTable
        Dim sql As String = "select * from vw_inv_piezas where id_variante = @id_variante and estado = 'DISPONIBLE' " &
                            "and (@ancho is null or @alto is null or (ancho_cm >= @ancho and alto_cm >= @alto) or (ancho_cm >= @alto and alto_cm >= @ancho)) " &
                            "and (@largo is null or largo_cm >= @largo) " &
                            "order by area_cm2, largo_cm"
        Return objMan.consultarConParametros(sql, New Dictionary(Of String, Object) From {
            {"id_variante", idVariante}, {"ancho", anchoMin}, {"alto", altoMin}, {"largo", largoMin}})
    End Function

    ' ---------------- Entradas ----------------

    ' Pieza que llega por una compra (la usa el módulo de compras)
    Public Function InsertarDesdeCompra(idVariante As Integer, idCompraDetalle As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?,
                                        Optional idUsuario As Integer? = Nothing) As Integer
        Return Ingresar(idVariante, anchoCm, altoCm, largoCm, "COMPRA", idCompraDetalle, Nothing, idUsuario, Nothing)
    End Function

    ' Sobrante de un corte registrado a mano. Normalmente se usa Cortar(), que calcula los retazos solo.
    Public Function InsertarRetazo(idVariante As Integer, idPiezaOrigen As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?,
                                   Optional idUsuario As Integer? = Nothing) As Integer
        Return Ingresar(idVariante, anchoCm, altoCm, largoCm, "RETAZO", Nothing, idPiezaOrigen, idUsuario, Nothing)
    End Function

    ' Pieza que ya estaba en el almacén antes de usar el sistema (carga inicial / conteo físico)
    Public Function InsertarInicial(idVariante As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?,
                                    Optional idUsuario As Integer? = Nothing, Optional observacion As String = Nothing) As Integer
        Return Ingresar(idVariante, anchoCm, altoCm, largoCm, "INICIAL", Nothing, Nothing, idUsuario, observacion)
    End Function

    Private Function Ingresar(idVariante As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?, origen As String,
                              idCompraDetalle As Integer?, idPiezaOrigen As Integer?, idUsuario As Integer?, observacion As String) As Integer
        Dim dt As DataTable = objMan.listarProcedimiento("sp_inv_ingresar_pieza", New Dictionary(Of String, Object) From {
            {"id_variante", idVariante}, {"ancho_cm", anchoCm}, {"alto_cm", altoCm}, {"largo_cm", largoCm},
            {"origen", origen}, {"id_compra_detalle", idCompraDetalle}, {"id_pieza_origen", idPiezaOrigen},
            {"id_usuario", idUsuario}, {"observacion", Vacio(observacion)}})
        Return CInt(dt.Rows(0)("id_pieza"))
    End Function

    ' ---------------- Cambios de estado ----------------

    ' estado: DISPONIBLE, RESERVADA, VENDIDA, CONSUMIDA o DEFECTUOSA
    Public Sub MarcarEstado(idPieza As Integer, estado As String, Optional idUsuario As Integer? = Nothing,
                            Optional idPedido As Integer? = Nothing, Optional observacion As String = Nothing)
        objMan.ejecutarProcedimiento("sp_inv_cambiar_estado_pieza", New Dictionary(Of String, Object) From {
            {"id_pieza", idPieza}, {"estado", estado}, {"id_usuario", idUsuario},
            {"id_pedido", idPedido}, {"observacion", Vacio(observacion)}})
    End Sub

    ' Separa la pieza para un pedido: deja de aparecer como disponible
    Public Sub Reservar(idPieza As Integer, Optional idUsuario As Integer? = Nothing, Optional idPedido As Integer? = Nothing)
        MarcarEstado(idPieza, "RESERVADA", idUsuario, idPedido)
    End Sub

    ' Devuelve una pieza reservada al stock disponible (ej: pedido cancelado)
    Public Sub Liberar(idPieza As Integer, Optional idUsuario As Integer? = Nothing, Optional idPedido As Integer? = Nothing)
        MarcarEstado(idPieza, "DISPONIBLE", idUsuario, idPedido)
    End Sub

    ' Pieza vendida entera, sin cortar
    Public Sub MarcarVendida(idPieza As Integer, Optional idUsuario As Integer? = Nothing, Optional idPedido As Integer? = Nothing)
        MarcarEstado(idPieza, "VENDIDA", idUsuario, idPedido)
    End Sub

    ' Pieza que llegó rota/defectuosa: no se reutiliza ni genera retazo, solo se marca
    Public Sub MarcarDefectuosa(idPieza As Integer, Optional idUsuario As Integer? = Nothing, Optional observacion As String = Nothing)
        MarcarEstado(idPieza, "DEFECTUOSA", idUsuario, Nothing, observacion)
    End Sub

    ' ---------------- Corte ----------------

    ' Corta la medida pedida de una pieza: la pieza queda CONSUMIDA y los sobrantes útiles
    ' (de al menos minimoUtilCm por lado) se registran solos como retazos DISPONIBLES.
    ' Vidrio: enviar ancho y alto. Aluminio: enviar largo.
    ' Devuelve los retazos generados (id_pieza, ancho_cm, alto_cm, largo_cm); puede venir vacío.
    Public Function Cortar(idPieza As Integer, anchoCorte As Decimal?, altoCorte As Decimal?, largoCorte As Decimal?,
                           Optional idUsuario As Integer? = Nothing, Optional idPedido As Integer? = Nothing,
                           Optional minimoUtilCm As Decimal = 10D) As DataTable
        Return objMan.listarProcedimiento("sp_inv_cortar_pieza", New Dictionary(Of String, Object) From {
            {"id_pieza", idPieza}, {"ancho_corte", anchoCorte}, {"alto_corte", altoCorte}, {"largo_corte", largoCorte},
            {"minimo_util_cm", minimoUtilCm}, {"id_usuario", idUsuario}, {"id_pedido", idPedido}})
    End Function

    ' Versión anterior (se mantiene por compatibilidad): el sobrante se indica a mano.
    ' Devuelve el id del retazo generado, o Nothing si no quedó sobrante.
    Public Function CortarYGenerarRetazo(idPieza As Integer, idVariante As Integer,
                                          anchoSobrante As Decimal?, altoSobrante As Decimal?, largoSobrante As Decimal?,
                                          Optional idUsuario As Integer? = Nothing) As Integer?
        MarcarEstado(idPieza, "CONSUMIDA", idUsuario)
        If anchoSobrante.HasValue OrElse altoSobrante.HasValue OrElse largoSobrante.HasValue Then
            Return InsertarRetazo(idVariante, idPieza, anchoSobrante, altoSobrante, largoSobrante, idUsuario)
        End If
        Return Nothing
    End Function

    Private Function Vacio(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return Nothing
        Return texto.Trim()
    End Function
End Class

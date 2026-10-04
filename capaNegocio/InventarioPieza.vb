Imports capaDatos

Public Class InventarioPieza
    Dim objMan As New clsMantenimiento

    ' Piezas DISPONIBLE que encajan en la medida pedida (vidrio: ancho+alto, aluminio: largo)
    Public Function BuscarDisponibles(idVariante As Integer, Optional anchoMin As Decimal? = Nothing,
                                       Optional altoMin As Decimal? = Nothing, Optional largoMin As Decimal? = Nothing) As DataTable
        Dim sql As String = "select * from inventario_piezas where id_variante=" & idVariante & " and estado='DISPONIBLE'"
        If anchoMin.HasValue Then sql &= " and ancho_cm >= " & Num(anchoMin)
        If altoMin.HasValue Then sql &= " and alto_cm >= " & Num(altoMin)
        If largoMin.HasValue Then sql &= " and largo_cm >= " & Num(largoMin)
        sql &= " order by ancho_cm, alto_cm, largo_cm"
        Return objMan.listarComando(sql)
    End Function

    Public Function InsertarDesdeCompra(idVariante As Integer, idCompraDetalle As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?) As Integer
        Return InsertarInterno(idVariante, Nothing, idCompraDetalle, anchoCm, altoCm, largoCm, "COMPRA")
    End Function

    Public Function InsertarRetazo(idVariante As Integer, idPiezaOrigen As Integer, anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?) As Integer
        Return InsertarInterno(idVariante, idPiezaOrigen, Nothing, anchoCm, altoCm, largoCm, "RETAZO")
    End Function

    Private Function InsertarInterno(idVariante As Integer, idPiezaOrigen As Integer?, idCompraDetalle As Integer?,
                                       anchoCm As Decimal?, altoCm As Decimal?, largoCm As Decimal?, origen As String) As Integer
        Dim sql As String = "insert into inventario_piezas (id_variante, id_pieza_origen, id_compra_detalle, ancho_cm, alto_cm, largo_cm, estado, origen) values (" &
            idVariante & "," & NumOrNull(idPiezaOrigen) & "," & NumOrNull(idCompraDetalle) & "," &
            Num(anchoCm) & "," & Num(altoCm) & "," & Num(largoCm) & ",'DISPONIBLE','" & Esc(origen) & "'); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    ' estado: DISPONIBLE, RESERVADA, VENDIDA, CONSUMIDA o DEFECTUOSA
    Public Sub MarcarEstado(idPieza As Integer, estado As String)
        objMan.ejecutarComando("update inventario_piezas set estado='" & Esc(estado) & "' where id_pieza=" & idPieza)
    End Sub

    ' Marca la pieza usada como CONSUMIDA y, si sobró material, genera el retazo como pieza nueva.
    ' Devuelve el id del retazo generado, o Nothing si no quedó sobrante.
    Public Function CortarYGenerarRetazo(idPieza As Integer, idVariante As Integer,
                                          anchoSobrante As Decimal?, altoSobrante As Decimal?, largoSobrante As Decimal?) As Integer?
        MarcarEstado(idPieza, "CONSUMIDA")
        If anchoSobrante.HasValue OrElse altoSobrante.HasValue OrElse largoSobrante.HasValue Then
            Return InsertarRetazo(idVariante, idPieza, anchoSobrante, altoSobrante, largoSobrante)
        End If
        Return Nothing
    End Function

    ' Pieza que llegó rota/defectuosa: no se reutiliza ni genera retazo, solo se marca
    Public Sub MarcarDefectuosa(idPieza As Integer)
        MarcarEstado(idPieza, "DEFECTUOSA")
    End Sub
End Class

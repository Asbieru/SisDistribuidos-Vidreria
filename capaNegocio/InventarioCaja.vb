Imports System.Data

Public Class InventarioCaja
    Dim objMan As New clsMantenimiento

    Public Function ObtenerStock(idVariante As Integer) As Integer
        Dim dt As DataTable = objMan.listarComando("select stock_cajas from inventario_cajas where id_variante=" & idVariante)
        If dt.Rows.Count > 0 Then Return CInt(dt.Rows(0)(0))
        Return 0
    End Function

    Public Sub Inicializar(idVariante As Integer, stockInicial As Integer)
        objMan.ejecutarComando("insert into inventario_cajas (id_variante, stock_cajas) values (" & idVariante & "," & stockInicial & ")")
    End Sub

    Public Sub SumarStock(idVariante As Integer, cantidad As Integer)
        objMan.ejecutarComando("update inventario_cajas set stock_cajas = stock_cajas + " & cantidad & " where id_variante=" & idVariante)
    End Sub

    Public Sub RestarStock(idVariante As Integer, cantidad As Integer)
        objMan.ejecutarComando("update inventario_cajas set stock_cajas = stock_cajas - " & cantidad & " where id_variante=" & idVariante)
    End Sub
End Class

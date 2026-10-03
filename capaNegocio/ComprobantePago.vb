Imports System.Data

Public Class ComprobantePago
    Dim objMan As New clsMantenimiento

    Public Function ObtenerPorPago(idPago As Integer) As DataTable
        Return objMan.listarComando("select * from comprobante_pago where id_pago=" & idPago)
    End Function

    ' tipoComprobante: 'BOLETA' o 'FACTURA'
    Public Function Insertar(idPago As Integer, tipoComprobante As String, serie As String, numero As String) As Integer
        Dim sql As String = "insert into comprobante_pago (id_pago, tipo_comprobante, serie, numero) values (" &
            idPago & ",'" & Esc(tipoComprobante) & "','" & Esc(serie) & "','" & Esc(numero) & "'); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

End Class

Imports System.Data

Public Class MetodoPago
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from metodos_pago order by nombre")
    End Function

    Public Function Insertar(nombre As String) As Integer
        Dim sql As String = "insert into metodos_pago (nombre) values ('" & Esc(nombre) & "'); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function
End Class

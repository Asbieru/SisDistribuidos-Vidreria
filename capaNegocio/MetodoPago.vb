Imports capaDatos

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

    ' Metodos registrados; si no hay ninguno, crea los basicos (YAPE, EFECTIVO, TARJETA, PLIN)
    Public Function AsegurarPorDefecto() As DataTable
        Dim dt As DataTable = Listar()
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            For Each nombre As String In New String() {"YAPE", "EFECTIVO", "TARJETA", "PLIN"}
                Insertar(nombre)
            Next
            dt = Listar()
        End If
        Return dt
    End Function
End Class

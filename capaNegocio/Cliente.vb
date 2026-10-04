Imports capaDatos

Public Class Cliente
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from clientes order by nombre")
    End Function

    Public Function Buscar(texto As String) As DataTable
        Dim sql As String = "select * from clientes where nombre like '%" & Esc(texto) & "%' or documento like '%" & Esc(texto) & "%'"
        Return objMan.listarComando(sql)
    End Function

    Public Function Insertar(nombre As String, documento As String, telefono As String, direccion As String) As Integer
        Dim sql As String = "insert into clientes (nombre, documento, telefono, direccion) values ('" &
            Esc(nombre) & "','" & Esc(documento) & "','" & Esc(telefono) & "','" & Esc(direccion) & "'); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idCliente As Integer, nombre As String, documento As String, telefono As String, direccion As String)
        Dim sql As String = "update clientes set nombre='" & Esc(nombre) & "', documento='" & Esc(documento) &
            "', telefono='" & Esc(telefono) & "', direccion='" & Esc(direccion) & "' where id_cliente=" & idCliente
        objMan.ejecutarComando(sql)
    End Sub

    Public Sub Eliminar(idCliente As Integer)
        objMan.ejecutarComando("delete from clientes where id_cliente=" & idCliente)
    End Sub
End Class

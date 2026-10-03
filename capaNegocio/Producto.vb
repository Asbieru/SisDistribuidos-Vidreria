Imports System.Data

Public Class Producto
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from productos where activo=1 order by categoria, tipo")
    End Function

    Public Function ListarPorCategoria(categoria As String) As DataTable
        Return objMan.listarComando("select * from productos where categoria='" & Esc(categoria) & "' and activo=1")
    End Function

    Public Function Insertar(categoria As String, tipo As String, color As String, nombre As String, descripcion As String, imagenUrl As String) As Integer
        Dim sql As String = "insert into productos (categoria, tipo, color, nombre, descripcion, imagen_url, activo) values ('" &
            Esc(categoria) & "','" & Esc(tipo) & "','" & Esc(color) & "','" & Esc(nombre) & "','" & Esc(descripcion) & "','" & Esc(imagenUrl) & "',1); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idProducto As Integer, categoria As String, tipo As String, color As String, nombre As String, descripcion As String, imagenUrl As String)
        Dim sql As String = "update productos set categoria='" & Esc(categoria) & "', tipo='" & Esc(tipo) &
            "', color='" & Esc(color) & "', nombre='" & Esc(nombre) & "', descripcion='" & Esc(descripcion) &
            "', imagen_url='" & Esc(imagenUrl) & "' where id_producto=" & idProducto
        objMan.ejecutarComando(sql)
    End Sub

    ' Baja lógica: no se borra, se marca inactivo (puede estar referenciado en ventas pasadas)
    Public Sub Inactivar(idProducto As Integer)
        objMan.ejecutarComando("update productos set activo=0 where id_producto=" & idProducto)
    End Sub
End Class

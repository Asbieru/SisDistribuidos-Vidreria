Imports System.Data

Public Class ProductoVariante
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Dim sql As String = "select pv.id_variante, p.nombre, p.categoria, p.tipo, p.color, c.espesor_cm, c.medida_cm, pv.unidad_costo, pv.precio_unitario " &
                             "from producto_variante pv " &
                             "inner join productos p on p.id_producto = pv.id_producto " &
                             "inner join caracteristicas c on c.id_caracteristica = pv.id_caracteristica"
        Return objMan.listarComando(sql)
    End Function

    Public Function Insertar(idProducto As Integer, idCaracteristica As Integer, unidadCosto As String, precioUnitario As Decimal) As Integer
        Dim sql As String = "insert into producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario) values (" &
            idProducto & "," & idCaracteristica & ",'" & Esc(unidadCosto) & "'," & Num(precioUnitario) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idVariante As Integer, unidadCosto As String, precioUnitario As Decimal)
        Dim sql As String = "update producto_variante set unidad_costo='" & Esc(unidadCosto) & "', precio_unitario=" & Num(precioUnitario) & " where id_variante=" & idVariante
        objMan.ejecutarComando(sql)
    End Sub
End Class

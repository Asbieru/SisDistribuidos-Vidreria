Imports System.Data

Public Class Caracteristica
    Dim objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando("select * from caracteristicas")
    End Function

    Public Function Insertar(espesorCm As Decimal?, medidaCm As Decimal?) As Integer
        Dim sql As String = "insert into caracteristicas (espesor_cm, medida_cm) values (" & Num(espesorCm) & "," & Num(medidaCm) & "); select scope_identity() as id"
        Dim dt As DataTable = objMan.listarComando(sql)
        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Sub Actualizar(idCaracteristica As Integer, espesorCm As Decimal?, medidaCm As Decimal?)
        Dim sql As String = "update caracteristicas set espesor_cm=" & Num(espesorCm) & ", medida_cm=" & Num(medidaCm) & " where id_caracteristica=" & idCaracteristica
        objMan.ejecutarComando(sql)
    End Sub
End Class

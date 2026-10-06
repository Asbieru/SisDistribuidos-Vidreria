Imports capaNegocio

' EDITAR: cambia el stock mínimo de un producto.
' Lo abre frmVerInventario con los datos de la fila seleccionada.
Public Class frmStockMinimo
    Public idVariante As Integer
    Public producto As String = ""
    Public existencias As Integer
    Public stockMinimo As Integer
    Public tema As String = "CLARO"

    Dim objInv As New Inventario

    Private Sub frmStockMinimo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblProducto.Text = producto
        lblExistencias.Text = "Existencias actuales: " & existencias
        nudMinimo.Value = Math.Min(nudMinimo.Maximum, stockMinimo)
        AplicarTema(Me, tema)
        lblNota.ForeColor = Color.Gray
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            objInv.ActualizarStockMinimo(idVariante, CInt(nudMinimo.Value))
            DialogResult = DialogResult.OK   ' cierra el form y avisa que hubo cambios
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub
End Class

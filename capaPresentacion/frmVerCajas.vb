Imports capaNegocio

' VER: stock de tornillos y tarugos (solo consulta).
' "Registrar movimiento" abre frmMovimientoCajas.
Public Class frmVerCajas
    Implements IFormularioTema
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objCaja As New InventarioCaja

    Private Sub frmVerCajas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        lblAyuda.ForeColor = Color.Gray
        Cargar()
    End Sub

    Private Sub Cargar()
        Try
            dgvCajas.DataSource = objCaja.Listar()
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    Private Sub dgvCajas_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvCajas.DataBindingComplete
        Encabezados(dgvCajas, "categoria|Categoría", "producto|Producto", "stock_cajas|Stock (cajas)", "stock_minimo|Mínimo", "precio_unitario|Precio por caja")
        dgvCajas.Columns("producto").FillWeight = 250
        dgvCajas.Columns("precio_unitario").DefaultCellStyle.Format = "N2"
    End Sub

    Private Sub dgvCajas_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvCajas.CellFormatting
        PintarBajoMinimo(dgvCajas, e, tema)
    End Sub

    ' ---------------- Abre el form para REGISTRAR un movimiento ----------------
    Private Sub btnMovimiento_Click(sender As Object, e As EventArgs) Handles btnMovimiento.Click
        RegistrarMovimiento()
    End Sub

    Private Sub dgvCajas_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCajas.CellDoubleClick
        If e.RowIndex >= 0 Then RegistrarMovimiento()
    End Sub

    Private Sub RegistrarMovimiento()
        Dim fila As DataGridViewRow = dgvCajas.CurrentRow
        If fila Is Nothing Then
            MostrarAviso("Seleccione un producto de la lista.")
            Return
        End If
        Dim indice As Integer = fila.Index
        Using frm As New frmMovimientoCajas With {
                .idVariante = CInt(fila.Cells("id_variante").Value),
                .producto = fila.Cells("producto").Value.ToString(),
                .stockActual = CInt(fila.Cells("stock_cajas").Value),
                .idUsuario = idUsuario,
                .tema = tema}
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Cargar()
                Reseleccionar(dgvCajas, indice)
            End If
        End Using
    End Sub
End Class

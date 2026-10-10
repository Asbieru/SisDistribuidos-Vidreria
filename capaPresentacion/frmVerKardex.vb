Imports capaNegocio

' VER: kardex, el historial de entradas, salidas y cambios del inventario (solo consulta).
Public Class frmVerKardex
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objInv As New Inventario

    Private Sub frmVerKardex_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        dtpDesde.Value = Date.Today.AddDays(-30)
        dtpHasta.Value = Date.Today
        Try
            CargarComboVariantes(cmbVariante, objInv.ListarTodasLasVariantes(), True)
        Catch ex As Exception
            MostrarError(ex)
        End Try
        Cargar()
    End Sub

    Private Sub Cargar()
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            MostrarAviso("La fecha 'Desde' no puede ser mayor que 'Hasta'.")
            Return
        End If
        Try
            dgvMovimientos.DataSource = objInv.ListarMovimientos(dtpDesde.Value, dtpHasta.Value, VarianteElegida(cmbVariante))
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Cargar()
    End Sub

    Private Sub dgvMovimientos_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvMovimientos.DataBindingComplete
        Encabezados(dgvMovimientos, "fecha|Fecha", "producto|Producto", "id_pieza|N° pieza", "tipo|Tipo", "motivo|Motivo", "cantidad|Cantidad",
                    "stock_resultante|Stock final", "usuario|Usuario", "id_pedido|Pedido", "observacion|Observación")
        dgvMovimientos.Columns("producto").FillWeight = 200
        dgvMovimientos.Columns("observacion").FillWeight = 200
        dgvMovimientos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
        dgvMovimientos.Columns("fecha").FillWeight = 140
    End Sub

    ' Entradas en verde y salidas en rojo
    Private Sub dgvMovimientos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvMovimientos.CellFormatting
        If e.RowIndex < 0 OrElse dgvMovimientos.Columns(e.ColumnIndex).Name <> "tipo" Then Return
        Dim oscuro As Boolean = tema.Equals("OSCURO")
        Select Case e.Value?.ToString()
            Case "ENTRADA" : e.CellStyle.ForeColor = If(oscuro, Color.LightGreen, Color.DarkGreen)
            Case "SALIDA" : e.CellStyle.ForeColor = If(oscuro, Color.LightCoral, Color.DarkRed)
        End Select
    End Sub
End Class

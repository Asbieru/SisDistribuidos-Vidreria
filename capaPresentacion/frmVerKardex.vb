Imports capaNegocio

' VER: kardex, el historial de entradas, salidas y cambios del inventario (solo consulta).
Public Class frmVerKardex
    Implements IFormularioTema, IListaInventario
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objInv As New Inventario
    Dim ultimaCarga As Date = Date.MinValue
    Dim ultimaCargaOk As Boolean = False

    Private Sub frmVerKardex_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Titulo

        dtpDesde.Value = Date.Today.AddDays(-30)
        dtpHasta.Value = Date.Today
        Try
            CargarComboVariantes(cmbVariante, objInv.ListarTodasLasVariantes(), True)
        Catch ex As Exception
            MostrarError(ex)
        End Try
        cargarLista()
    End Sub

    Public Sub cargarLista() Implements IListaInventario.cargarLista
        ultimaCargaOk = False
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            MostrarAviso("La fecha 'Desde' no puede ser mayor que 'Hasta'.")
            Return
        End If
        Try
            ' Si "Hasta" es hoy (o después) no se pone límite superior: así aparecen también los
            ' movimientos que un servidor con otra zona horaria guardó con fecha "de mañana".
            Dim hasta As Date? = If(dtpHasta.Value.Date >= Date.Today, CType(Nothing, Date?), dtpHasta.Value)
            dgvMovimientos.DataSource = objInv.ListarMovimientos(dtpDesde.Value, hasta, VarianteElegida(cmbVariante))
            lblCantidad.Text = If(dgvMovimientos.Rows.Count = 0, "No se encontraron movimientos.",
                                  "Movimientos encontrados: " & dgvMovimientos.Rows.Count.ToString())
            ultimaCargaOk = True
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ultimaCarga = Date.Now
    End Sub

    ' Al volver a esta ventana (después de registrar algo en otra) se recarga sola.
    ' Si la última carga falló no se reintenta, para no repetir el mensaje de error en bucle.
    Private Sub frmVerKardex_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If ultimaCargaOk AndAlso (Date.Now - ultimaCarga).TotalSeconds > 2 Then cargarLista()
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        cargarLista()
    End Sub

    Private Sub dgvMovimientos_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvMovimientos.DataBindingComplete
        Encabezados(dgvMovimientos, "fecha|Fecha", "producto|Producto", "id_pieza|N° pieza", "tipo|Tipo", "motivo|Motivo", "cantidad|Cantidad",
                    "stock_resultante|Stock final", "usuario|Usuario", "id_pedido|Pedido", "observacion|Observación")
        dgvMovimientos.Columns("producto").FillWeight = 200
        dgvMovimientos.Columns("observacion").FillWeight = 200
        dgvMovimientos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
        dgvMovimientos.Columns("fecha").FillWeight = 140
        ' Ancho minimo: si el total de columnas no cabe, aparece el scroll horizontal
        AnchoMinimoColumnas(dgvMovimientos, 110)
        dgvMovimientos.Columns("producto").MinimumWidth = 200
        dgvMovimientos.Columns("observacion").MinimumWidth = 200
    End Sub

    ' Entradas en verde y salidas en rojo
    Private Sub dgvMovimientos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvMovimientos.CellFormatting
        If e.RowIndex < 0 OrElse dgvMovimientos.Columns(e.ColumnIndex).Name <> "tipo" Then Return
        Dim oscuro As Boolean = Temas.EsOscuro(tema)
        Select Case e.Value?.ToString()
            Case "ENTRADA" : e.CellStyle.ForeColor = If(oscuro, Color.LightGreen, Color.DarkGreen)
            Case "SALIDA" : e.CellStyle.ForeColor = If(oscuro, Color.LightCoral, Color.DarkRed)
        End Select
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

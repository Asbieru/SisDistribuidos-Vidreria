Imports capaNegocio

' VER: stock de tornillos y tarugos (por caja) y accesorios (por unidad). Solo consulta.
' "Registrar movimiento" abre frmMovimientoCajas.
Public Class frmVerCajas
    Implements IFormularioTema, IListaInventario
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objCaja As New InventarioCaja
    Dim dtCajas As DataTable
    Dim ultimaCarga As Date = Date.MinValue
    Dim ultimaCargaOk As Boolean = False

    Private Sub frmVerCajas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento

        cmbCategoria.Items.Add(TODOS)
        cmbCategoria.Items.AddRange(New String() {"TORNILLO", "TARUGO", "ACCESORIO"})
        cmbCategoria.SelectedIndex = 0
        cargarLista()
    End Sub

    Public Sub cargarLista() Implements IListaInventario.cargarLista
        ultimaCargaOk = False
        Try
            dtCajas = objCaja.Listar()
            dgvCajas.DataSource = dtCajas
            filtrar()
            ultimaCargaOk = True
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ultimaCarga = Date.Now
    End Sub

    ' Al volver a esta ventana se recarga sola (si la última carga falló, no se reintenta en bucle)
    Private Sub frmVerCajas_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If ultimaCargaOk AndAlso (Date.Now - ultimaCarga).TotalSeconds > 2 Then cargarLista()
    End Sub

    ' Filtra por categoría sin volver a consultar la base
    Private Sub filtrar()
        If dtCajas Is Nothing Then Return
        dtCajas.DefaultView.RowFilter = If(cmbCategoria.SelectedIndex <= 0, "",
                                           "categoria = '" & cmbCategoria.SelectedItem.ToString() & "'")
        lblCantidad.Text = If(dtCajas.DefaultView.Count = 0, "No se encontraron productos.",
                              "Productos encontrados: " & dtCajas.DefaultView.Count.ToString())
        btnMovimiento.Enabled = dgvCajas.CurrentRow IsNot Nothing
    End Sub

    Private Sub cmbCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged
        filtrar()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        cargarLista()
    End Sub

    Private Sub dgvCajas_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvCajas.DataBindingComplete
        Encabezados(dgvCajas, "categoria|Categoría", "producto|Producto", "stock_cajas|Stock", "unidad_costo|Unidad",
                    "stock_minimo|Mínimo", "precio_unitario|Precio unit.")
        dgvCajas.Columns("producto").FillWeight = 250
        dgvCajas.Columns("precio_unitario").DefaultCellStyle.Format = "N2"
    End Sub

    Private Sub dgvCajas_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvCajas.CellFormatting
        PintarBajoMinimo(dgvCajas, e, tema)
    End Sub

    Private Sub dgvCajas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCajas.SelectionChanged
        btnMovimiento.Enabled = dgvCajas.CurrentRow IsNot Nothing
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
                .unidadCosto = fila.Cells("unidad_costo").Value.ToString(),
                .stockActual = CInt(fila.Cells("stock_cajas").Value),
                .idUsuario = idUsuario,
                .tema = tema}
            If frm.ShowDialog(Me) = DialogResult.OK Then
                cargarLista()
                Reseleccionar(dgvCajas, indice)
            End If
        End Using
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

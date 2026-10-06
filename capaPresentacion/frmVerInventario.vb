Imports capaNegocio

' VER: resumen de existencias de todos los productos (solo consulta).
' Desde aquí se abren los demás formularios del inventario.
Public Class frmVerInventario
    ' Los envía MenuInicio al abrir el formulario
    Public idUsuario As Integer? = Nothing
    Public tema As String = "CLARO"

    Dim objInv As New Inventario
    Dim cargando As Boolean = True

    Private Sub frmVerInventario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbCategoria.Items.Add(TODOS)
        cmbCategoria.Items.AddRange(Inventario.Categorias)
        cmbCategoria.SelectedIndex = 0
        AplicarTema(Me, tema)
        lblAlerta.ForeColor = ColorAlerta(tema)
        cargando = False
        Cargar()
    End Sub

    Private Sub Cargar()
        Try
            Dim categoria As String = If(cmbCategoria.SelectedIndex <= 0, Nothing, cmbCategoria.SelectedItem.ToString())
            dgvResumen.DataSource = objInv.ListarResumen(categoria, chkBajoMinimo.Checked)

            Dim cantidad As Integer = objInv.ContarBajoMinimo()
            lblAlerta.Text = If(cantidad = 0, "", "¡Atención! " & cantidad & " producto(s) en o por debajo del stock mínimo")
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    Private Sub Filtro_Changed(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged, chkBajoMinimo.CheckedChanged, btnActualizar.Click
        If Not cargando Then Cargar()
    End Sub

    Private Sub dgvResumen_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvResumen.DataBindingComplete
        Encabezados(dgvResumen, "categoria|Categoría", "producto|Producto", "unidad_costo|Unidad", "existencias|Existencias",
                    "stock_minimo|Mínimo", "piezas_reservadas|Reservadas", "area_disponible_cm2|Área disp. (cm²)",
                    "largo_disponible_cm|Largo disp. (cm)", "precio_unitario|Precio unit.")
        dgvResumen.Columns("producto").FillWeight = 250
        dgvResumen.Columns("area_disponible_cm2").DefaultCellStyle.Format = "N0"
        dgvResumen.Columns("largo_disponible_cm").DefaultCellStyle.Format = "N2"
        dgvResumen.Columns("precio_unitario").DefaultCellStyle.Format = "N2"
    End Sub

    Private Sub dgvResumen_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvResumen.CellFormatting
        PintarBajoMinimo(dgvResumen, e, tema)
    End Sub

    ' ---------------- Abre el form de EDICIÓN del stock mínimo ----------------
    Private Sub btnEditarMinimo_Click(sender As Object, e As EventArgs) Handles btnEditarMinimo.Click
        EditarMinimo()
    End Sub

    Private Sub dgvResumen_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResumen.CellDoubleClick
        If e.RowIndex >= 0 Then EditarMinimo()
    End Sub

    Private Sub EditarMinimo()
        Dim fila As DataGridViewRow = dgvResumen.CurrentRow
        If fila Is Nothing Then
            MostrarAviso("Seleccione un producto de la lista.")
            Return
        End If
        Dim indice As Integer = fila.Index
        Using frm As New frmStockMinimo With {
                .idVariante = CInt(fila.Cells("id_variante").Value),
                .producto = fila.Cells("producto").Value.ToString(),
                .existencias = CInt(fila.Cells("existencias").Value),
                .stockMinimo = CInt(fila.Cells("stock_minimo").Value),
                .tema = tema}
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Cargar()
                Reseleccionar(dgvResumen, indice)
            End If
        End Using
    End Sub

    ' ---------------- Abre los otros formularios de consulta ----------------
    Private Sub btnVerPiezas_Click(sender As Object, e As EventArgs) Handles btnVerPiezas.Click
        Dim frm As New frmVerPiezas With {.idUsuario = idUsuario, .tema = tema}
        frm.Show()
    End Sub

    Private Sub btnVerCajas_Click(sender As Object, e As EventArgs) Handles btnVerCajas.Click
        Dim frm As New frmVerCajas With {.idUsuario = idUsuario, .tema = tema}
        frm.Show()
    End Sub

    Private Sub btnVerKardex_Click(sender As Object, e As EventArgs) Handles btnVerKardex.Click
        Dim frm As New frmVerKardex With {.tema = tema}
        frm.Show()
    End Sub
End Class

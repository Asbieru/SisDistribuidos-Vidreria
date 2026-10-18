Imports capaNegocio

' VER: resumen de existencias de todos los productos (solo consulta).
' Desde aquí se abren los demás formularios del inventario.
Public Class frmVerInventario
    Implements IFormularioTema, IListaInventario
    ' Los envía MenuInicio al abrir el formulario
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objInv As New Inventario
    Dim cargando As Boolean = True
    Dim ultimaCarga As Date = Date.MinValue
    Dim ultimaCargaOk As Boolean = False

    Private Sub frmVerInventario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Titulo
        lblAlerta.ForeColor = ColorAlerta(tema)

        cmbCategoria.Items.Add(TODOS)
        cmbCategoria.Items.AddRange(Inventario.Categorias)
        cmbCategoria.SelectedIndex = 0
        cargando = False
        cargarLista()
    End Sub

    Public Sub cargarLista() Implements IListaInventario.cargarLista
        ultimaCargaOk = False
        Try
            Dim categoria As String = If(cmbCategoria.SelectedIndex <= 0, Nothing, cmbCategoria.SelectedItem.ToString())
            dgvResumen.DataSource = objInv.ListarResumen(categoria, chkBajoMinimo.Checked)
            lblCantidad.Text = If(dgvResumen.Rows.Count = 0, "No se encontraron productos.",
                                  "Productos encontrados: " & dgvResumen.Rows.Count.ToString())

            Dim cantidad As Integer = objInv.ContarBajoMinimo()
            lblAlerta.Text = If(cantidad = 0, "", "¡Atención! " & cantidad & " producto(s) en o por debajo del stock mínimo")
            ultimaCargaOk = True
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ultimaCarga = Date.Now
        btnEditarMinimo.Enabled = dgvResumen.CurrentRow IsNot Nothing
    End Sub

    ' Al volver a esta ventana se recarga sola (si la última carga falló, no se reintenta en bucle)
    Private Sub frmVerInventario_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If ultimaCargaOk AndAlso (Date.Now - ultimaCarga).TotalSeconds > 2 Then cargarLista()
    End Sub

    Private Sub Filtro_Changed(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged, chkBajoMinimo.CheckedChanged, btnActualizar.Click
        If Not cargando Then cargarLista()
    End Sub

    Private Sub dgvResumen_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvResumen.DataBindingComplete
        Encabezados(dgvResumen, "categoria|Categoría", "producto|Producto", "unidad_costo|Unidad", "existencias|Existencias",
                    "stock_minimo|Mínimo", "piezas_reservadas|Reservadas", "area_disponible_cm2|Área disp. (cm²)",
                    "largo_disponible_cm|Largo disp. (cm)", "precio_unitario|Precio unit.")
        dgvResumen.Columns("producto").FillWeight = 250
        dgvResumen.Columns("categoria").FillWeight = 120
        dgvResumen.Columns("unidad_costo").FillWeight = 120
        dgvResumen.Columns("area_disponible_cm2").DefaultCellStyle.Format = "N0"
        dgvResumen.Columns("largo_disponible_cm").DefaultCellStyle.Format = "N2"
        dgvResumen.Columns("precio_unitario").DefaultCellStyle.Format = "N2"
        ' Ancho minimo: si el total de columnas no cabe, aparece el scroll horizontal
        AnchoMinimoColumnas(dgvResumen, 110)
        dgvResumen.Columns("producto").MinimumWidth = 220
    End Sub

    Private Sub dgvResumen_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvResumen.CellFormatting
        PintarBajoMinimo(dgvResumen, e, tema)
    End Sub

    Private Sub dgvResumen_SelectionChanged(sender As Object, e As EventArgs) Handles dgvResumen.SelectionChanged
        btnEditarMinimo.Enabled = dgvResumen.CurrentRow IsNot Nothing
    End Sub

    ' ---------------- Abre el form de EDICIÓN del stock mínimo ----------------
    Private Sub btnEditarMinimo_Click(sender As Object, e As EventArgs) Handles btnEditarMinimo.Click
        EditarMinimo()
    End Sub

    Private Sub dgvResumen_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResumen.CellDoubleClick
        If e.RowIndex >= 0 Then EditarMinimo()
    End Sub

    ' ShowDialog abre la edición como ventana modal: al cerrarla, el código sigue en la línea siguiente
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
                cargarLista()
                Reseleccionar(dgvResumen, indice)
            End If
        End Using
    End Sub

    ' ---------------- Abre los otros formularios de consulta ----------------
    ' Igual que MenuInicio: si la ventana ya está abierta la trae al frente;
    ' si no, la abre dentro del menú principal (MDI).
    Private Sub btnVerPiezas_Click(sender As Object, e As EventArgs) Handles btnVerPiezas.Click
        AbrirVentana(Of frmVerPiezas)(Sub(f) f.idUsuario = idUsuario)
    End Sub

    Private Sub btnVerCajas_Click(sender As Object, e As EventArgs) Handles btnVerCajas.Click
        AbrirVentana(Of frmVerCajas)(Sub(f) f.idUsuario = idUsuario)
    End Sub

    Private Sub btnVerKardex_Click(sender As Object, e As EventArgs) Handles btnVerKardex.Click
        AbrirVentana(Of frmVerKardex)(Nothing)
    End Sub

    Private Sub AbrirVentana(Of T As {Form, IFormularioTema, New})(configurar As Action(Of T))
        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is T Then
                formulario.WindowState = FormWindowState.Normal
                Dim lista As IListaInventario = TryCast(formulario, IListaInventario)
                If lista IsNot Nothing Then lista.cargarLista()   ' que no muestre datos antiguos
                formulario.BringToFront()
                Return
            End If
        Next
        Dim frm As New T
        frm.tema = tema
        If configurar IsNot Nothing Then configurar(frm)
        frm.MdiParent = MdiParent
        frm.Show()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

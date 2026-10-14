Imports capaNegocio

' VER: listado de piezas de vidrio y aluminio (solo consulta).
' "Nueva pieza" abre frmRegistrarPieza; "Editar pieza" abre frmEditarPieza.
Public Class frmVerPiezas
    Implements IFormularioTema, IListaInventario
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objInv As New Inventario
    Dim objPieza As New InventarioPieza
    Dim cargando As Boolean = True
    Dim ultimaCarga As Date = Date.MinValue
    Dim ultimaCargaOk As Boolean = False

    Private Sub frmVerPiezas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
        Try
            cmbEstado.Items.Add(TODOS)
            cmbEstado.Items.AddRange(Inventario.EstadosPieza)
            cmbEstado.SelectedItem = "DISPONIBLE"
            CargarComboVariantes(cmbVariante, objInv.ListarVariantes(True), True)
        Catch ex As Exception
            MostrarError(ex)
        End Try
        cargando = False
        cargarLista()
    End Sub

    Public Sub cargarLista() Implements IListaInventario.cargarLista
        ultimaCargaOk = False
        Try
            Dim estado As String = If(cmbEstado.SelectedIndex <= 0, Nothing, cmbEstado.SelectedItem.ToString())
            dgvPiezas.DataSource = objPieza.Listar(VarianteElegida(cmbVariante), estado)
            lblCantidad.Text = If(dgvPiezas.Rows.Count = 0, "No se encontraron piezas.",
                                  "Piezas encontradas: " & dgvPiezas.Rows.Count.ToString())
            ultimaCargaOk = True
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ultimaCarga = Date.Now
        ActualizarBotones()
    End Sub

    ' Al volver a esta ventana se recarga sola (si la última carga falló, no se reintenta en bucle)
    Private Sub frmVerPiezas_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If ultimaCargaOk AndAlso (Date.Now - ultimaCarga).TotalSeconds > 2 Then cargarLista()
    End Sub

    Private Sub Filtro_Changed(sender As Object, e As EventArgs) Handles cmbVariante.SelectedIndexChanged, cmbEstado.SelectedIndexChanged, btnBuscar.Click
        If Not cargando Then cargarLista()
    End Sub

    Private Sub dgvPiezas_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvPiezas.DataBindingComplete
        Encabezados(dgvPiezas, "id_pieza|N° pieza", "producto|Producto", "ancho_cm|Ancho (cm)", "alto_cm|Alto (cm)", "largo_cm|Largo (cm)",
                    "area_cm2|Área (cm²)", "estado|Estado", "origen|Origen", "id_pieza_origen|Retazo de", "fecha_ingreso|Ingreso")
        dgvPiezas.Columns("producto").FillWeight = 250
        dgvPiezas.Columns("area_cm2").DefaultCellStyle.Format = "N0"
        dgvPiezas.Columns("fecha_ingreso").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
        dgvPiezas.Columns("fecha_ingreso").FillWeight = 140
    End Sub

    Private Sub dgvPiezas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPiezas.SelectionChanged
        ActualizarBotones()
    End Sub

    ' Solo se editan piezas que siguen en el almacén (DISPONIBLE o RESERVADA)
    Private Sub ActualizarBotones()
        Dim fila As DataGridViewRow = dgvPiezas.CurrentRow
        Dim estado As String = If(fila Is Nothing, "", fila.Cells("estado").Value.ToString())
        btnEditarPieza.Enabled = (estado = "DISPONIBLE" OrElse estado = "RESERVADA")
    End Sub

    ' ---------------- Abre el form para INGRESAR ----------------
    Private Sub btnNuevaPieza_Click(sender As Object, e As EventArgs) Handles btnNuevaPieza.Click
        Using frm As New frmRegistrarPieza With {.idUsuario = idUsuario, .tema = tema, .idVarianteInicial = VarianteElegida(cmbVariante)}
            If frm.ShowDialog(Me) = DialogResult.OK Then cargarLista()
        End Using
    End Sub

    ' ---------------- Abre el form para EDITAR ----------------
    Private Sub btnEditarPieza_Click(sender As Object, e As EventArgs) Handles btnEditarPieza.Click
        EditarPieza()
    End Sub

    Private Sub dgvPiezas_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPiezas.CellDoubleClick
        If e.RowIndex >= 0 AndAlso btnEditarPieza.Enabled Then EditarPieza()
    End Sub

    Private Sub EditarPieza()
        Dim fila As DataGridViewRow = dgvPiezas.CurrentRow
        If fila Is Nothing Then Return
        Using frm As New frmEditarPieza With {.idPieza = CInt(fila.Cells("id_pieza").Value), .idUsuario = idUsuario, .tema = tema}
            If frm.ShowDialog(Me) = DialogResult.OK Then cargarLista()
        End Using
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

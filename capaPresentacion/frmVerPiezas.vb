Imports capaNegocio

' VER: listado de piezas de vidrio y aluminio (solo consulta).
' "Nueva pieza" abre frmRegistrarPieza; "Editar" abre frmEditarPieza.
Public Class frmVerPiezas
    Public idUsuario As Integer? = Nothing
    Public tema As String = "CLARO"

    Dim objInv As New Inventario
    Dim objPieza As New InventarioPieza
    Dim cargando As Boolean = True

    Private Sub frmVerPiezas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cmbEstado.Items.Add(TODOS)
            cmbEstado.Items.AddRange(Inventario.EstadosPieza)
            cmbEstado.SelectedItem = "DISPONIBLE"
            CargarComboVariantes(cmbVariante, objInv.ListarVariantes(True), True)
        Catch ex As Exception
            MostrarError(ex)
        End Try
        AplicarTema(Me, tema)
        lblAyuda.ForeColor = Color.Gray
        cargando = False
        Cargar()
    End Sub

    Private Sub Cargar()
        Try
            Dim estado As String = If(cmbEstado.SelectedIndex <= 0, Nothing, cmbEstado.SelectedItem.ToString())
            dgvPiezas.DataSource = objPieza.Listar(VarianteElegida(cmbVariante), estado)
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ActualizarBotones()
    End Sub

    Private Sub Filtro_Changed(sender As Object, e As EventArgs) Handles cmbVariante.SelectedIndexChanged, cmbEstado.SelectedIndexChanged, btnBuscar.Click
        If Not cargando Then Cargar()
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
            If frm.ShowDialog(Me) = DialogResult.OK Then Cargar()
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
            If frm.ShowDialog(Me) = DialogResult.OK Then Cargar()
        End Using
    End Sub
End Class

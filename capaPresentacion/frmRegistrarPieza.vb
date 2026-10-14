Imports capaNegocio

' INGRESAR: registra una pieza nueva de vidrio o aluminio (inventario inicial).
' Lo abre frmVerPiezas.
Public Class frmRegistrarPieza
    Implements IFormularioTema
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public idVarianteInicial As Integer? = Nothing   ' producto que venía filtrado en frmVerPiezas

    Dim objInv As New Inventario
    Dim objPieza As New InventarioPieza

    Private Sub frmRegistrarPieza_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
        Try
            CargarComboVariantes(cmbVariante, objInv.ListarVariantes(True), False)
            If idVarianteInicial.HasValue Then cmbVariante.SelectedValue = idVarianteInicial.Value
        Catch ex As Exception
            MostrarError(ex)
        End Try
        ActualizarCampos()
    End Sub

    Private Sub cmbVariante_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbVariante.SelectedIndexChanged
        ActualizarCampos()
    End Sub

    ' Vidrio (por cm² o por pie²) pide ancho y alto; aluminio (por cm lineal) pide largo
    Private Sub ActualizarCampos()
        Dim unidad As String = UnidadElegida(cmbVariante)
        nudAncho.Enabled = Inventario.EsVidrio(unidad)
        nudAlto.Enabled = Inventario.EsVidrio(unidad)
        nudLargo.Enabled = (unidad = "CM_LINEAL")
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim idVariante As Integer? = VarianteElegida(cmbVariante)
        If Not idVariante.HasValue Then
            MostrarAviso("No hay productos de vidrio o aluminio registrados, o no eligió ninguno.")
            Return
        End If
        Try
            Dim id As Integer = objPieza.InsertarInicial(idVariante.Value, Medida(nudAncho), Medida(nudAlto), Medida(nudLargo), idUsuario, txtObs.Text)
            MostrarInfo("Pieza #" & id & " registrada en el inventario.")
            DialogResult = DialogResult.OK   ' cierra el form y avisa que hubo cambios
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub
End Class

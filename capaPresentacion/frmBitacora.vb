Imports capaNegocio

' REPORTE: muestra todos los registros de la bitácora (solo consulta).
Public Class frmBitacora
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objBit As New Bitacora

    Private Sub frmBitacora_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        cmbAccion.Items.Add(TODOS)
        cmbAccion.Items.AddRange(Bitacora.Acciones)
        cmbAccion.SelectedIndex = 0
        cargarLista()
    End Sub

    ' Carga todos los registros o solo los de la acción elegida en la lista
    Public Sub cargarLista()
        Try
            If cmbAccion.SelectedIndex <= 0 Then
                dgvBitacora.DataSource = objBit.listarBitacora()
            Else
                dgvBitacora.DataSource = objBit.listarPorAccion(cmbAccion.SelectedItem.ToString())
            End If
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    Private Sub dgvBitacora_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvBitacora.DataBindingComplete
        Encabezados(dgvBitacora, "idRegistro|N°", "fechaReg|Fecha", "horaReg|Hora", "usuario|Usuario",
                    "rol|Rol", "accion|Acción", "numIntentos|Intentos fallidos")
        dgvBitacora.Columns("fechaReg").DefaultCellStyle.Format = "dd/MM/yyyy"
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        cargarLista()
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        cargarLista()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

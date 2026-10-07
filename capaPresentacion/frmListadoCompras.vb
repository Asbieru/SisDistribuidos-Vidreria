Imports capaNegocio

Public Class frmListadoCompras
    Private objCompra As New Compra

    Private Sub frmListadoCompras_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpDesde.Value = New Date(Today.Year, Today.Month, 1)
        dtpHasta.Value = Today
        cargarLista()
    End Sub

    Public Sub cargarLista()
        Try
            mostrarCompras(objCompra.Listar())
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mostrarCompras(datos As DataTable)
        dgvCompras.DataSource = datos

        dgvCompras.Columns("id_compra").HeaderText = "ID"
        dgvCompras.Columns("proveedor").HeaderText = "Proveedor"
        dgvCompras.Columns("fecha").HeaderText = "Fecha"
        dgvCompras.Columns("total").HeaderText = "Total"
        dgvCompras.Columns("motivo").HeaderText = "Motivo"
        dgvCompras.Columns("id_pedido_origen").HeaderText = "Pedido de origen"

        dgvCompras.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvCompras.Columns("total").DefaultCellStyle.Format = "N2"
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            MessageBox.Show("La fecha Desde no puede ser posterior a Hasta.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            mostrarCompras(objCompra.ListarPorFecha(
                           dtpDesde.Value.Date, dtpHasta.Value.Date))
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnVerTodas_Click(sender As Object, e As EventArgs) Handles btnVerTodas.Click
        cargarLista()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub btnNuevaCompra_Click(sender As Object, e As EventArgs) Handles btnNuevaCompra.Click
        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is frmRegistrarCompras Then
                formulario.WindowState = FormWindowState.Normal
                formulario.BringToFront()
                Return
            End If
        Next

        Dim registro As New frmRegistrarCompras()
        registro.Show()
    End Sub

    Private Sub btnModificar_Click(sender As Object, e As EventArgs) Handles btnModificar.Click
        If dgvCompras.CurrentRow Is Nothing Then
            MessageBox.Show(
                "Seleccione una compra de la lista.",
                "Aviso", MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Dim id As Integer =
            CInt(dgvCompras.CurrentRow.Cells("id_compra").Value)

        Dim registro As frmRegistrarCompras = Nothing

        For Each formulario As Form In Application.OpenForms
            If TypeOf formulario Is frmRegistrarCompras Then
                registro = DirectCast(formulario, frmRegistrarCompras)
                Exit For
            End If
        Next

        If registro IsNot Nothing Then
            If MessageBox.Show(
                "Se cargarán los datos de la compra seleccionada. " &
                "Los cambios sin guardar del formulario se reemplazarán. ¿Continuar?",
                "Confirmar", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Else
            registro = New frmRegistrarCompras()
            registro.Show()
        End If

        registro.WindowState = FormWindowState.Normal
        registro.BringToFront()
        registro.cargarCompra(id)
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvCompras.CurrentRow Is Nothing Then
            MessageBox.Show(
                "Seleccione una compra de la lista.",
                "Aviso", MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Dim id As Integer =
            CInt(dgvCompras.CurrentRow.Cells("id_compra").Value)

        Dim proveedor As String =
            dgvCompras.CurrentRow.Cells("proveedor").Value.ToString()

        Dim respuesta As DialogResult = MessageBox.Show(
            "¿Eliminar la compra ID " & id.ToString() &
            " del proveedor " & proveedor & "?" &
            vbCrLf & "Se eliminarán también sus detalles." &
            vbCrLf & "Esta acción no se puede deshacer.",
            "Confirmar eliminación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2)

        If respuesta <> DialogResult.Yes Then Return

        Try
            objCompra.EliminarCompra(id)
        Catch ex As Exception
            MessageBox.Show(
                ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        MessageBox.Show(
            "Compra eliminada correctamente.",
            "Mensaje", MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        cargarLista()
    End Sub
End Class
Imports capaNegocio

Public Class frmListadoCompras
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Private objCompra As New Compra
    Private compras As DataTable
    Private filtrarFechas As Boolean = False
    Private desdeAplicado As Date
    Private hastaAplicado As Date

    Private Sub frmListadoCompras_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        dtpDesde.Value = New Date(Today.Year, Today.Month, 1)
        dtpHasta.Value = Today
        cargarLista()
    End Sub

    Public Sub cargarLista()
        Try
            Dim datos As DataTable = If(filtrarFechas,
                objCompra.ListarPorFecha(desdeAplicado, hastaAplicado), objCompra.Listar())
            mostrarCompras(datos)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub mostrarCompras(datos As DataTable)
        compras = datos
        dgvCompras.DataSource = compras.DefaultView

        dgvCompras.Columns("id_compra").HeaderText = "ID"
        dgvCompras.Columns("proveedor").HeaderText = "Proveedor"
        dgvCompras.Columns("fecha").HeaderText = "Fecha"
        dgvCompras.Columns("total").HeaderText = "Total"
        dgvCompras.Columns("motivo").HeaderText = "Motivo"
        dgvCompras.Columns("id_pedido_origen").HeaderText = "Pedido de origen"

        dgvCompras.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvCompras.Columns("total").DefaultCellStyle.Format = "N2"
        filtrarProveedor()
    End Sub

    Private Sub filtrarProveedor()
        If compras Is Nothing Then Return
        Dim texto As String = txtBuscarProveedor.Text.Trim().Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
        compras.DefaultView.RowFilter = If(texto = "", "", "proveedor LIKE '%" & texto & "%'")
        Dim total As Decimal = 0D
        For Each fila As DataRowView In compras.DefaultView
            If Not IsDBNull(fila("total")) Then total += CDec(fila("total"))
        Next
        lblResumen.Text = "Compras: " & compras.DefaultView.Count.ToString() &
            "    Total: S/ " & total.ToString("N2")
        actualizarBotones()
    End Sub

    Private Sub txtBuscarProveedor_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarProveedor.TextChanged
        filtrarProveedor()
    End Sub

    Private Sub actualizarBotones()
        Dim seleccion As Boolean = dgvCompras.CurrentRow IsNot Nothing AndAlso
            TypeOf dgvCompras.CurrentRow.DataBoundItem Is DataRowView
        btnModificar.Enabled = seleccion
        btnEliminar.Enabled = seleccion
        btnVerDetalle.Enabled = seleccion
    End Sub

    Private Sub dgvCompras_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCompras.SelectionChanged
        actualizarBotones()
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            MessageBox.Show("La fecha Desde no puede ser posterior a Hasta.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim desde As Date = dtpDesde.Value.Date
            Dim hasta As Date = dtpHasta.Value.Date
            Dim datos As DataTable = objCompra.ListarPorFecha(desde, hasta)
            desdeAplicado = desde
            hastaAplicado = hasta
            filtrarFechas = True
            mostrarCompras(datos)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnVerTodas_Click(sender As Object, e As EventArgs) Handles btnVerTodas.Click
        filtrarFechas = False
        txtBuscarProveedor.Clear()
        cargarLista()
    End Sub

    Private Sub dgvCompras_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvCompras.CellDoubleClick
        If e.RowIndex >= 0 Then btnVerDetalle.PerformClick()
    End Sub

    Private Sub btnVerDetalle_Click(sender As Object, e As EventArgs) Handles btnVerDetalle.Click
        If dgvCompras.CurrentRow Is Nothing Then Return
        Dim id As Integer = CInt(dgvCompras.CurrentRow.Cells("id_compra").Value)
        Try
            Dim cabecera As DataTable = objCompra.ObtenerCompra(id)
            If cabecera.Rows.Count = 0 Then
                MessageBox.Show(Me, "La compra seleccionada ya no existe.", "Detalle de compra",
                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                cargarLista()
                Return
            End If
            Dim datos As DataTable = objCompra.ObtenerDetalleParaEditar(id)
            Using ventana As New Form With {.Text = "Detalle de compra " & id.ToString(),
                .Size = New Size(850, 450), .MinimumSize = New Size(650, 350),
                .StartPosition = FormStartPosition.CenterParent, .MinimizeBox = False}
                Dim encabezado As New Label With {.Dock = DockStyle.Top, .Height = 60,
                    .Padding = New Padding(10), .Text = "Proveedor: " & cabecera.Rows(0)("proveedor").ToString() &
                    vbCrLf & "Fecha: " & CDate(cabecera.Rows(0)("fecha")).ToString("dd/MM/yyyy") &
                    "    Total: S/ " & CDec(cabecera.Rows(0)("total")).ToString("N2")}
                Dim tabla As New DataGridView With {.Dock = DockStyle.Fill, .ReadOnly = True,
                    .AllowUserToAddRows = False, .AllowUserToDeleteRows = False,
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    .MultiSelect = False, .DataSource = datos}
                ventana.Controls.Add(tabla)
                tabla.Columns("id_variante").Visible = False
                tabla.Columns("producto").HeaderText = "Producto / variante"
                tabla.Columns("producto").FillWeight = 250
                tabla.Columns("cantidad").HeaderText = "Cantidad"
                tabla.Columns("cantidad").DefaultCellStyle.Format = "0.##"
                tabla.Columns("costo_unitario").HeaderText = "Costo unitario"
                tabla.Columns("subtotal").HeaderText = "Subtotal"
                tabla.Columns("costo_unitario").DefaultCellStyle.Format = "N2"
                tabla.Columns("subtotal").DefaultCellStyle.Format = "N2"
                Dim cerrar As New Button With {.Text = "CERRAR", .Dock = DockStyle.Bottom,
                    .Height = 35, .DialogResult = DialogResult.Cancel}
                ventana.Controls.Add(encabezado)
                ventana.Controls.Add(cerrar)
                ventana.CancelButton = cerrar
                AplicarTema(ventana, tema)
                ventana.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show(Me, ex.Message, "Detalle de compra", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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

        Dim registro As New frmRegistrarCompras With {.tema = tema}
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
            registro = New frmRegistrarCompras With {.tema = tema}
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

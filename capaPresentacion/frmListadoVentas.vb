Imports capaNegocio
Imports System.Data
Imports System.Windows.Forms

Public Class frmListadoVentas
    Implements IFormularioTema

    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idUsuario As Integer = 0

    Private objVenta As New Venta()
    Private usarFechas As Boolean = False

    Private Sub frmListadoVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)

        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If idUsuario = 0 AndAlso padre IsNot Nothing Then
            Dim id As Integer
            If Integer.TryParse(padre.txtID.Text, id) Then
                idUsuario = id
            End If
        End If

        dtpDesde.Value = New Date(Today.Year, Today.Month, 1)
        dtpHasta.Value = Today

        cboEstadoPago.DropDownStyle = ComboBoxStyle.DropDownList
        cboEstadoPago.Items.Clear()
        cboEstadoPago.Items.AddRange(
            New String() {
                "TODOS", "PENDIENTE", "PARCIAL", "PAGADO", "EXCEDENTE"
            })
        cboEstadoPago.SelectedIndex = 0

        dgvVentas.ReadOnly = True
        dgvVentas.AllowUserToAddRows = False
        dgvVentas.AllowUserToDeleteRows = False
        dgvVentas.MultiSelect = False
        dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        cargarLista()
    End Sub

    Public Sub cargarLista()
        Try
            Dim desde As Date? = Nothing
            Dim hasta As Date? = Nothing

            If usarFechas Then
                desde = dtpDesde.Value.Date
                hasta = dtpHasta.Value.Date
            End If

            Dim estado As String = "TODOS"
            If cboEstadoPago.SelectedItem IsNot Nothing Then
                estado = cboEstadoPago.SelectedItem.ToString()
            End If

            Dim datos As DataTable = objVenta.Listar(
                desde, hasta, txtBuscarCliente.Text, estado)

            dgvVentas.DataSource = datos

            Encabezados(
                dgvVentas,
                "id_pedido|Pedido",
                "cliente|Cliente",
                "vendedor|Vendedor",
                "fecha|Fecha del pedido",
                "estado_trabajo|Trabajo",
                "total|Importe S/",
                "pagado|Cobrado S/",
                "saldo|Saldo S/",
                "estado_pago|Pago")

            dgvVentas.Columns("id_pedido").FillWeight = 45
            dgvVentas.Columns("cliente").FillWeight = 160
            dgvVentas.Columns("vendedor").FillWeight = 100
            dgvVentas.Columns("fecha").FillWeight = 100
            dgvVentas.Columns("estado_trabajo").FillWeight = 100
            dgvVentas.Columns("estado_pago").FillWeight = 90

            dgvVentas.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy"

            For Each nombre As String In
                New String() {"total", "pagado", "saldo"}

                dgvVentas.Columns(nombre).DefaultCellStyle.Format = "N2"
                dgvVentas.Columns(nombre).DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment.MiddleRight
                dgvVentas.Columns(nombre).FillWeight = 90
            Next

            Dim importe As Decimal = 0
            Dim cobrado As Decimal = 0
            Dim saldo As Decimal = 0

            For Each fila As DataRow In datos.Rows
                importe += CDec(fila("total"))
                cobrado += CDec(fila("pagado"))
                saldo += CDec(fila("saldo"))
            Next

            lblTotalPedidos.Text = "IMPORTE PEDIDOS: S/ " & importe.ToString("N2")
            lblTotalCobrado.Text = "COBRADO: S/ " & cobrado.ToString("N2")
            lblTotalSaldo.Text = "SALDO: S/ " & saldo.ToString("N2")

            Text = "Listado de ventas - " &
                If(usarFechas,
                   "Fechas: " & dtpDesde.Value.ToString("dd/MM/yyyy") &
                   " al " & dtpHasta.Value.ToString("dd/MM/yyyy"),
                   "Todas las fechas")

            actualizarBotonDetalle()

        Catch ex As Exception
            MessageBox.Show(
                ex.Message, "Ventas",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            MessageBox.Show(
                "La fecha Desde no puede ser posterior a Hasta.",
                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        usarFechas = True
        cargarLista()
    End Sub

    Private Sub btnVerTodas_Click(sender As Object, e As EventArgs) Handles btnVerTodas.Click
        usarFechas = False
        txtBuscarCliente.Clear()
        cboEstadoPago.SelectedIndex = 0
        cargarLista()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        cargarLista()
    End Sub

    Private Sub actualizarBotonDetalle()
        btnVerDetalle.Enabled = (dgvVentas.CurrentRow IsNot Nothing)
    End Sub

    Private Sub dgvVentas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvVentas.SelectionChanged
        actualizarBotonDetalle()
    End Sub

    Private Sub btnVerDetalle_Click(sender As Object, e As EventArgs) Handles btnVerDetalle.Click
        abrirDetalle()
    End Sub

    Private Sub dgvVentas_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvVentas.CellDoubleClick
        If e.RowIndex >= 0 Then abrirDetalle()
    End Sub

    Private Sub abrirDetalle()
        If dgvVentas.CurrentRow Is Nothing Then
            MessageBox.Show("Seleccione una venta de la tabla.")
            Return
        End If

        Dim id As Integer =
            CInt(dgvVentas.CurrentRow.Cells("id_pedido").Value)

        Try
            ' Reutilizar el detalle si ya está abierto.
            For Each formulario As Form In Application.OpenForms
                Dim existente As frmPedidoUnitario =
                    TryCast(formulario, frmPedidoUnitario)

                If existente IsNot Nothing AndAlso existente.idPedido = id Then
                    existente.WindowState = FormWindowState.Normal
                    existente.BringToFront()
                    Return
                End If
            Next

            Dim detalle As New frmPedidoUnitario With {
                .idPedido = id,
                .tema = tema,
                .idUsuario = idUsuario
            }

            If Me.MdiParent IsNot Nothing Then
                detalle.MdiParent = Me.MdiParent
            End If

            AddHandler detalle.FormClosed, AddressOf detalleCerrado
            detalle.Show()

        Catch ex As Exception
            MessageBox.Show(
                ex.Message, "Ventas",
                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub detalleCerrado(sender As Object, e As FormClosedEventArgs)
        If Not Me.IsDisposed AndAlso Not Me.Disposing Then
            cargarLista()
        End If
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class
Imports capaNegocio
Imports System.Data

Public Class frmHistorialClientes
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idCliente As Integer
    Private ReadOnly objCliente As New Cliente
    Private cargando As Boolean = True

    Private Sub frmHistorialClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
        cmbEstado.Items.AddRange(New Object() {TODOS, "PENDIENTE", "PARCIAL", "PAGADO", "CANCELADO"})
        cmbEstado.SelectedIndex = 0
        cargando = False
        cargarHistorial()
    End Sub

    Public Sub cargarHistorial()
        cargando = True
        Try
            Dim cliente As DataTable = objCliente.Obtener(idCliente)
            If cliente.Rows.Count = 0 Then Throw New Exception("El cliente ya no existe. Seleccione otro cliente.")
            lblCliente.Text = "Cliente: " & cliente.Rows(0)("nombre").ToString() &
                              "  |  Documento: " & cliente.Rows(0)("documento").ToString()
            Dim estado As String = If(cmbEstado.SelectedIndex <= 0, Nothing, cmbEstado.SelectedItem.ToString())
            Dim pedidos As DataTable = objCliente.Historial(idCliente, estado)
            dgvPedidos.DataSource = pedidos
            Encabezados(dgvPedidos, "id_pedido|Pedido", "fecha|Fecha", "estado|Estado",
                        "vendedor|Vendedor", "total|Total S/.", "pagado|Pagado S/.",
                        "saldo|Saldo S/.", "excedente|Excedente S/.")
            dgvPedidos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvPedidos.Columns("vendedor").FillWeight = 160
            For Each columna As String In {"total", "pagado", "saldo", "excedente"}
                dgvPedidos.Columns(columna).DefaultCellStyle.Format = "N2"
                dgvPedidos.Columns(columna).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next
            If dgvPedidos.Rows.Count > 0 Then
                dgvPedidos.CurrentCell = dgvPedidos.Rows(0).Cells("id_pedido")
            End If
            Dim total As Decimal = 0D
            Dim pagado As Decimal = 0D
            Dim saldo As Decimal = 0D
            For Each fila As DataRow In pedidos.Rows
                If fila("estado").ToString() <> "CANCELADO" Then
                    total += CDec(fila("total"))
                    pagado += CDec(fila("pagado"))
                    saldo += CDec(fila("saldo"))
                End If
            Next
            lblResumen.Text = "Resumen del filtro (sin cancelados):  Total S/. " & total.ToString("N2") &
                              "   |   Pagado S/. " & pagado.ToString("N2") & "   |   Saldo S/. " & saldo.ToString("N2")
            lblCantidad.Text = If(pedidos.Rows.Count = 0, "Este cliente no tiene pedidos en el filtro seleccionado.",
                                  "Pedidos encontrados: " & pedidos.Rows.Count.ToString())
        Catch ex As Exception
            dgvPedidos.DataSource = Nothing
            dgvPagos.DataSource = Nothing
            lblCliente.Text = "No se pudo cargar el cliente."
            lblResumen.Text = "Resumen no disponible."
            lblCantidad.Text = "Presione Actualizar o seleccione otro cliente."
            MessageBox.Show(ex.Message, "Historial de clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            cargando = False
        End Try
        cargarPagos()
    End Sub

    Private Sub cargarPagos()
        dgvPagos.DataSource = Nothing
        lblPagos.Text = "PAGOS DEL PEDIDO: seleccione una fila del listado superior."
        If dgvPedidos.CurrentRow Is Nothing OrElse Not dgvPedidos.Columns.Contains("id_pedido") Then Return
        Try
            Dim idPedido As Integer = CInt(dgvPedidos.CurrentRow.Cells("id_pedido").Value)
            Dim pagos As DataTable = objCliente.PagosDePedido(idCliente, idPedido)
            dgvPagos.DataSource = pagos
            Encabezados(dgvPagos, "id_pago|Pago", "fecha|Fecha", "total_pago|Total pago S/.",
                        "metodo|Método", "monto_metodo|Importe método S/.", "comprobante|Comprobante")
            dgvPagos.Columns("fecha").DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            dgvPagos.Columns("total_pago").DefaultCellStyle.Format = "N2"
            dgvPagos.Columns("monto_metodo").DefaultCellStyle.Format = "N2"
            dgvPagos.Columns("comprobante").FillWeight = 180
            lblPagos.Text = "PAGOS DEL PEDIDO " & idPedido.ToString() &
                            If(pagos.Rows.Count = 0, ": sin pagos registrados.", " (una fila por método; el total del pago puede repetirse).")
        Catch ex As Exception
            lblPagos.Text = "No se pudieron cargar los pagos del pedido."
            MessageBox.Show(ex.Message, "Pagos del cliente", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvPedidos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPedidos.SelectionChanged
        If Not cargando Then cargarPagos()
    End Sub

    Private Sub cmbEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEstado.SelectedIndexChanged
        If Not cargando Then cargarHistorial()
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        cargarHistorial()
    End Sub

    Private Sub btnCambiarCliente_Click(sender As Object, e As EventArgs) Handles btnCambiarCliente.Click
        Using formulario As New frmListadoClientes With {.tema = tema, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                idCliente = formulario.idClienteSeleccionado
                cargando = True
                cmbEstado.SelectedIndex = 0
                cargando = False
                cargarHistorial()
            End If
        End Using
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class

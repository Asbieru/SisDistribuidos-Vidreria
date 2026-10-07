Imports capaNegocio

Public Class frmCotizarVentana
    Public Property tema As String = "CLARO"
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""

    Private Sub frmCotizarVentana_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        PanelCliente.BackColor = Temas.Obtener(tema).Fondo
        Panel1.BackColor = Temas.Obtener(tema).Fondo
        Panel2.BackColor = Temas.Obtener(tema).Fondo
        actualizarCliente()
    End Sub

    Private Sub actualizarCliente()
        txtCliente.Text = nombreClienteSeleccionado
        btnHistorialCliente.Enabled = idClienteSeleccionado > 0
    End Sub

    ' Este enlace entrega el cliente al futuro registro del pedido.
    ' El calculo de cotizacion y el registro comercial conservan su alcance original.
    Private Sub btnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Using formulario As New frmListadoClientes With {.tema = tema, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                idClienteSeleccionado = formulario.idClienteSeleccionado
                nombreClienteSeleccionado = formulario.nombreClienteSeleccionado
                actualizarCliente()
            End If
        End Using
    End Sub

    Private Sub btnHistorialCliente_Click(sender As Object, e As EventArgs) Handles btnHistorialCliente.Click
        If idClienteSeleccionado <= 0 Then Return
        Using formulario As New frmHistorialClientes With {.tema = tema, .idCliente = idClienteSeleccionado}
            formulario.ShowDialog(Me)
        End Using
    End Sub
End Class

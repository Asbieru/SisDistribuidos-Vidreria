Imports capaNegocio

Public Class frmRegistrarPedido
    Implements IFormularioTema
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Private Sub actualizarCliente()
        txtNombreCliente.Text = nombreClienteSeleccionado
    End Sub

    Private Sub frmRegistrarPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
    End Sub

    Private Sub btnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Using formulario As New frmListadoClientes With {.tema = tema, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                idClienteSeleccionado = formulario.idClienteSeleccionado
                nombreClienteSeleccionado = formulario.nombreClienteSeleccionado
                actualizarCliente()
            End If
        End Using
    End Sub

    Private Sub btnAgregarPedido_Click(sender As Object, e As EventArgs) Handles btnAgregarPedido.Click

    End Sub
End Class
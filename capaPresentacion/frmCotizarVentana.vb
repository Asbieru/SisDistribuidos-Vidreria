Imports capaNegocio

Public Class frmCotizarVentana
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Private Sub precioMaterial()

    End Sub

    Private Sub frmCotizarVentana_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
    End Sub
End Class

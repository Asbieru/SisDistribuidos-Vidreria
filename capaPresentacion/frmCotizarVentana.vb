Imports capaNegocio

Public Class frmCotizarVentana
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Private Sub precioMaterial()

    End Sub

    Private Sub frmCotizarVentana_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Dim objCotV As New frmCotizarVentana With {
            .tema = tema,
            .txtAlto = Me.txtAlto,
            .txtAncho = Me.txtAncho,
            .txtEspesor = Me.txtEspesor,
            .txtManoObra = Me.txtManoObra,
            .txtExtra = Me.txtExtra}
        objCotV.MdiParent = Me.MdiParent
        objCotV.Show()
    End Sub

    Private Sub btnLlevarPedido_Click(sender As Object, e As EventArgs) Handles btnLlevarPedido.Click
        Dim objFrm As New frmRegistrarPedido With {.tema = tema}
        objFrm.MdiParent = Me.MdiParent
        objFrm.Show()
    End Sub
End Class

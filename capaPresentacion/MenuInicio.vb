Public Class MenuInicio
    Private Sub mnCerrarS_Click(sender As Object, e As EventArgs) Handles mnCerrarS.Click
        Dim res As Integer
        res = MessageBox.Show("¿Desea Cerrar Sesión?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If res = 7 Then
            Return
        End If
        Dim objIncio As New InicioSesion
        objIncio.Show()
        Dispose()
    End Sub
End Class
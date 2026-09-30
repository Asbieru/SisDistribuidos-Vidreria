Imports capaNegocio
Public Class CambiarContraseña
    Dim objUsu As New Usuario

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        Try
            If txtContraseña.TextLength > 0 And txtConfirmar.TextLength > 0 Then
                If txtContraseña.Text.Equals(txtConfirmar.Text) Then
                    objUsu.cambiarContraseña(txtUsuario.Text, txtContraseña.Text)
                    MessageBox.Show("Contraseña modificada correctamente!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Close()
                Else
                    MessageBox.Show("Las contraseñas no coinciden!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtConfirmar.Clear()
                    txtConfirmar.Focus()
                End If
            Else
                MessageBox.Show("Ingrese y confirme la nueva contraseña!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtContraseña.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub chkMostrar_CheckedChanged(sender As Object, e As EventArgs) Handles chkMostrar.CheckedChanged
        If chkMostrar.Checked Then
            txtContraseña.PasswordChar = ChrW(0)
            txtConfirmar.PasswordChar = ChrW(0)
        Else
            txtContraseña.PasswordChar = "*"c
            txtConfirmar.PasswordChar = "*"c
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class
Imports capaNegocio
Public Class InicioSesion
    Dim objUsu As New Usuario
    Dim intentos As Integer = 3

    Private Sub chbMostrar_CheckedChanged(sender As Object, e As EventArgs) Handles chbMostrar.CheckedChanged
        pnlCambio.Visible = chbMostrar.Checked
        Try
            If chbMostrar.Checked Then
                If txtUsuario.Text.Trim.Length = 0 Then
                    msj.Text = "Ingrese nombre de usario!"
                    txtUsuario.Focus()
                    Return
                End If
                If Not objUsu.validarNombreUsuario(txtUsuario.Text) Then
                    msj.Text = "Usuario no encontrado!"
                    txtUsuario.Focus()
                    Return
                End If
                txtPregunta.Text = objUsu.obtenerPregunta(txtUsuario.Text)
            Else
                txtPregunta.Clear()
                txtRespuesta.Clear()
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Try
            If txtUsuario.Text.Trim.Length = 0 Then
                msj.Text = "Ingrese nombre de usario!"
                txtUsuario.Focus()
                Return
            End If
            If txtContrasena.Text.Trim.Length = 0 Then
                msj.Text = "Ingrese nombre de contraseña!"
                txtContrasena.Focus()
                Return
            End If
            If Not objUsu.validarNombreUsuario(txtUsuario.Text) Then
                msj.Text = "Usuario no encontrado!"
                txtUsuario.Focus()
                Return
            End If
            If objUsu.iniciarSesion(txtUsuario.Text, txtContrasena.Text) Then
                MessageBox.Show("Bienvenido al Sistema!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                msj.Text = "Contraseña incorrecta!"
                intentos -= 1
                If intentos = 0 Then
                    txtContrasena.Focus()
                    txtContrasena.Clear()
                    msj.Text = "Contraseña incorrecta, intentos restantes: " & intentos
                End If
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnCambio_Click(sender As Object, e As EventArgs) Handles btnCambio.Click
        Dim objCambio As New CambiarContraseña
        Try
            If txtUsuario.Text.Trim.Length = 0 Then
                msj2.Text = "Ingrese nombre de usario!"
                txtUsuario.Focus()
                Return
            End If
            If objUsu.validarRespuesta(txtUsuario.Text, txtRespuesta.Text) Then
                MessageBox.Show("Bienvenido al Sistema!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                objCambio.txtUsuario.Text = Me.txtUsuario.Text
                objCambio.txtContraseña.Focus()
                objCambio.ShowDialog()
            Else
                msj2.Text = "Respuesta incorrecta, intente nuevamente!"
                txtRespuesta.Clear()
                txtRespuesta.Focus()
            End If
        Catch ex As Exception
            msj2.Text = ex.Message
        End Try
    End Sub
End Class
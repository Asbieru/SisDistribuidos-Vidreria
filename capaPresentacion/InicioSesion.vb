Imports capaNegocio
Public Class InicioSesion
    Dim objUsu As New Usuario
    Dim objBit As New Bitacora
    Dim intentos As Integer = 3

    ' Registra en la bitácora la acción del usuario escrito en pantalla.
    ' bitacora_login exige id_usuario, así que solo se registra si el usuario existe.
    ' Nunca interrumpe el inicio de sesión aunque la bitácora falle.
    Private Sub RegistrarBitacora(accion As String, Optional numIntentos As Integer? = Nothing)
        Try
            Dim nombre As String = txtUsuario.Text.Trim()
            If nombre.Length = 0 OrElse Not objUsu.validarNombreUsuario(nombre) Then Return
            objBit.registrarAccionSeguro(objUsu.obtenerIDUsuario(nombre), accion, numIntentos)
        Catch ex As Exception
            ' La bitácora no debe cortar el inicio de sesión
        End Try
    End Sub

    Private Sub chbMostrar_CheckedChanged(sender As Object, e As EventArgs) Handles chbMostrar.CheckedChanged
        pnlCambio.Visible = chbMostrar.Checked
        Try
            If chbMostrar.Checked Then
                If txtUsuario.Text.Trim.Length = 0 Then
                    msj.Text = "¡Ingrese nombre de usuario!"
                ElseIf Not objUsu.validarNombreUsuario(txtUsuario.Text) Then
                    msj.Text = "¡Usuario no encontrado!"
                Else
                    AjustarTamanoAcceso()
                    txtUsuario.Enabled = False
                    txtPregunta.Text = objUsu.obtenerPregunta(txtUsuario.Text)
                    Return
                End If
                txtUsuario.Focus()
                chbMostrar.Checked = False
                pnlCambio.Visible = False
                AjustarTamanoAcceso()
            Else
                AjustarTamanoAcceso()
                txtUsuario.Enabled = True
                txtPregunta.Clear()
                txtRespuesta.Clear()
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
                RegistrarBitacora(Bitacora.RECUPERA_CLAVE)
                MessageBox.Show("Bienvenido al Sistema!", "MENSAJE", MessageBoxButtons.OK, MessageBoxIcon.Information)
                objCambio.txtUsuario.Text = Me.txtUsuario.Text
                objCambio.txtContraseña.Focus()
                objCambio.ShowDialog()
            Else
                RegistrarBitacora(Bitacora.RESP_ERRADA)
                msj2.Text = "Respuesta incorrecta, intente nuevamente!"
                txtRespuesta.Clear()
                txtRespuesta.Focus()
            End If
        Catch ex As Exception
            msj2.Text = ex.Message
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
            If Not objUsu.usuarioActivo(txtUsuario.Text) Then
                RegistrarBitacora(Bitacora.USUARIO_BAJA)
                msj.Text = "Usuario dado de baja, comuníquese con el administrador!"
                txtUsuario.Focus()
                Return
            End If
            If objUsu.iniciarSesion(txtUsuario.Text, txtContrasena.Text) Then
                RegistrarBitacora(Bitacora.INICIO_SESION, 3 - intentos)
                Dim objMn As New MenuInicio
                objMn.txtUsuario.Text = txtUsuario.Text
                objMn.Show()
                Me.Close()
            Else
                msj.Text = "Contraseña incorrecta!"
                intentos -= 1
                RegistrarBitacora(Bitacora.CLAVE_ERRADA, 3 - Math.Max(intentos, 0))
                If intentos = 0 Then
                    RegistrarBitacora(Bitacora.SIN_INTENTOS, 3)
                    txtContrasena.Focus()
                    txtContrasena.Clear()
                    msj.Text = "Contraseña incorrecta, intentos restantes: " & intentos
                End If
            End If
        Catch ex As Exception
            msj.Text = ex.Message
        End Try
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub

    Private Sub InicioSesion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, "OSCURO")
        AjustarTamanoAcceso()
    End Sub

    Private Sub AjustarTamanoAcceso()
        ClientSize = New Size(ClientSize.Width,
            If(pnlCambio.Visible, pnlCambio.Bottom, chbMostrar.Bottom) + CInt(24 * DeviceDpi / 96.0))
        AjustarVentana(Me, True)
    End Sub
End Class

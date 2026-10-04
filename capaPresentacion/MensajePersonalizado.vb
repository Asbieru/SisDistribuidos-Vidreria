Imports capaNegocio
Public Class MensajePersonalizado
    Dim respuesta As Boolean
    Dim tema As String = "CLARO"
    Dim idUsuario As Integer
    Dim idMensaje As Integer
    Dim objUsuario As New Usuario

    Private Sub temaColor()
        If tema.Equals("CLARO") Then
            BackColor = Color.White
            lblTitulo.ForeColor = Color.Black
            lblContenido.ForeColor = Color.Black
            btnAceptar.ForeColor = Color.Black
            btnAceptar.BackColor = Color.Gainsboro
            btnCancelar.ForeColor = Color.Black
            btnCancelar.BackColor = Color.Gainsboro
            chbNMostrar.ForeColor = Color.DarkBlue
        ElseIf tema.Equals("OSCURO") Then
            BackColor = Color.White
            lblTitulo.ForeColor = Color.White
            lblContenido.ForeColor = Color.White
            btnAceptar.ForeColor = Color.White
            btnAceptar.BackColor = Color.Gray
            btnCancelar.ForeColor = Color.White
            btnCancelar.BackColor = Color.Gray
            chbNMostrar.ForeColor = Color.LightBlue
        End If
    End Sub

    Private Sub modoError()
        temaColor()
        If tema.Equals("CLARO") Then
            lblTitulo.ForeColor = Color.DarkRed
        ElseIf tema.Equals("OSCURO") Then
            lblTitulo.ForeColor = Color.LightCoral
        End If
        btnAceptar.Visible = False
        btnCancelar.Text = "Cerrar"
    End Sub

    Private Sub modoInformacion()
        temaColor()
        btnAceptar.Visible = False
        btnCancelar.Text = "Cerrar"
    End Sub

    Private Sub llenarDatos(cod As String)
        Dim dt As New DataTable
        Try
            idMensaje = objUsuario.obtenerIDMensaje(cod)
            dt = objUsuario.obtenerMensaje(idMensaje)
            lblTitulo.Text = dt.Rows(0).Item(0).ToString
            lblContenido.Text = dt.Rows(0).Item(1).ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        respuesta = True
        Dispose()
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        respuesta = False
        Dispose()
    End Sub
End Class
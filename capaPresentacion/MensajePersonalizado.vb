Imports capaNegocio
Public Class MensajePersonalizado
    Implements IFormularioTema
    Public respuesta As Boolean
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public idUsuario As Integer = 0
    Public idMensaje As Integer = 0
    Dim objUsuario As New Usuario

    Private Sub MensajePersonalizado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
    End Sub

    Public Sub temaColor()
        Dim p As Paleta = Temas.Obtener(tema)
        BackColor = p.Fondo
        For Each lbl As Label In New Label() {lblTitulo, lblContenido}
            lbl.ForeColor = p.Texto
        Next
        For Each btn As Button In New Button() {btnAceptar, btnCancelar}
            btn.ForeColor = p.BotonTexto
            btn.BackColor = p.BotonFondo
        Next
        chbNMostrar.ForeColor = p.Acento
    End Sub

    Public Sub modoError()
        temaColor()
        If tema.Equals("CLARO") Then
            lblTitulo.ForeColor = Color.DarkRed
        ElseIf tema.Equals("OSCURO") Then
            lblTitulo.ForeColor = Color.LightCoral
        End If
        btnAceptar.Visible = False
        btnCancelar.Text = "Cerrar"
    End Sub

    Public Sub modoInformacion()
        temaColor()
        btnAceptar.Visible = False
        btnCancelar.Text = "Cerrar"
    End Sub

    Public Sub llenarDatos()
        Dim dt As New DataTable
        Try
            dt = objUsuario.obtenerMensaje(idMensaje)
            lblTitulo.Text = dt.Rows(0).Item(0).ToString
            lblContenido.Text = dt.Rows(0).Item(1).ToString
        Catch ex As Exception
            MessageBox.Show("Error al encontrar Datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub descarteMensaje()
        If chbNMostrar.Checked Then
            Try
                objUsuario.descartarMensaje(idUsuario, idMensaje)
            Catch ex As Exception
                MessageBox.Show("Error al descartar mensaje", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        respuesta = True
        descarteMensaje()
        Dispose()
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        respuesta = False
        descarteMensaje()
        Dispose()
    End Sub
End Class
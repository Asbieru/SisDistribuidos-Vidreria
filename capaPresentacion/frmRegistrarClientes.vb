Imports capaNegocio
Imports System.Data

Public Class frmRegistrarClientes
    Public Property tema As String = "CLARO"
    Public Property idCliente As Integer = 0
    Public Property idClienteGuardado As Integer = 0
    Private ReadOnly objCliente As New Cliente
    Private versionOriginal As Byte()

    Private Sub frmRegistrarClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Panel2.BackColor = Temas.Obtener(tema).Fondo
        lblModo.ForeColor = Temas.Obtener(tema).Acento
        idClienteGuardado = 0
        Try
            If idCliente = 0 Then
                lblModo.Text = "NUEVO CLIENTE"
                txtId.Text = "(automático)"
            Else
                cargarCliente()
            End If
            btnRecargar.Visible = idCliente > 0
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        End Try
    End Sub

    Private Sub cargarCliente()
        Dim dt As DataTable = objCliente.Obtener(idCliente)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("El cliente ya no existe. Actualice el listado.",
                "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Return
        End If
        Dim fila As DataRow = dt.Rows(0)
        lblModo.Text = "MODIFICAR CLIENTE"
        txtId.Text = idCliente.ToString()
        txtNombre.Text = fila("nombre").ToString()
        txtDocumento.Text = fila("documento").ToString()
        txtTelefono.Text = fila("telefono").ToString()
        txtDireccion.Text = fila("direccion").ToString()
        versionOriginal = DirectCast(fila("version_cliente"), Byte())
    End Sub

    Private Sub frmRegistrarClientes_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtNombre.Focus()
        txtNombre.SelectAll()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("Ingrese el nombre del cliente.", "Clientes",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombre.Focus()
            Return
        End If
        btnGuardar.Enabled = False
        Try
            Dim coincidentes As DataTable = objCliente.DocumentoCoincidente(txtDocumento.Text, idCliente)
            If coincidentes.Rows.Count > 0 Then
                If MessageBox.Show("El documento ya está registrado en " & coincidentes.Rows.Count.ToString() &
                    " cliente(s), por ejemplo: " & coincidentes.Rows(0)("nombre").ToString() &
                    "." & vbCrLf & "Revise el documento. ¿Desea guardar de todas formas?",
                    "Documento repetido", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
            End If
            If idCliente = 0 Then
                idClienteGuardado = objCliente.Insertar(txtNombre.Text, txtDocumento.Text,
                                                       txtTelefono.Text, txtDireccion.Text)
            Else
                objCliente.Actualizar(idCliente, txtNombre.Text, txtDocumento.Text,
                                      txtTelefono.Text, txtDireccion.Text, versionOriginal)
                idClienteGuardado = idCliente
            End If
            MessageBox.Show("Cliente guardado correctamente.", "Clientes",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub btnRecargar_Click(sender As Object, e As EventArgs) Handles btnRecargar.Click
        If MessageBox.Show("Se reemplazarán los datos de la pantalla por los últimos datos guardados. ¿Continuar?",
            "Recargar cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Try
            cargarCliente()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class

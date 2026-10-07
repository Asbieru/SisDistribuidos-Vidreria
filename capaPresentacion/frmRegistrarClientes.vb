Imports capaNegocio
Imports System.Data

Public Class frmRegistrarClientes
    Public Property tema As String = "CLARO"
    Public Property idCliente As Integer = 0
    Public Property idClienteGuardado As Integer = 0
    Private ReadOnly objCliente As New Cliente

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
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        End Try
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
            If idCliente = 0 Then
                idClienteGuardado = objCliente.Insertar(txtNombre.Text, txtDocumento.Text,
                                                       txtTelefono.Text, txtDireccion.Text)
            Else
                objCliente.Actualizar(idCliente, txtNombre.Text, txtDocumento.Text,
                                      txtTelefono.Text, txtDireccion.Text)
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

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class

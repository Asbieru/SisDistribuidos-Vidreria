Imports capaNegocio
Imports System.Data

Public Class frmRegistrarClientes
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idCliente As Integer = 0
    Public Property idClienteGuardado As Integer = 0
    Private ReadOnly objCliente As New Cliente
    Private versionOriginal As Byte()
    Private cargando As Boolean = True
    Private datosIniciales As String()

    Private Sub frmRegistrarClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblModo.ForeColor = p.Acento
        idClienteGuardado = 0
        Try
            cmbTipoPersona.Items.AddRange(New Object() {"Natural", "Jurídica"})
            cmbTipoDocumento.Items.AddRange(New Object() {"DNI", "RUC", "Sin documento"})
            If idCliente = 0 Then
                lblModo.Text = "NUEVO CLIENTE"
                txtId.Text = "(automático)"
                cmbTipoPersona.SelectedIndex = 0
                cmbTipoDocumento.SelectedIndex = 0
            Else
                cargarCliente()
            End If
            btnRecargar.Visible = idCliente > 0
            cargando = False
            actualizarIdentificacion()
            datosIniciales = datosActuales()
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
        cargando = True
        cmbTipoPersona.SelectedIndex = If(fila("tipo_persona").ToString() = "NATURAL", 0,
            If(fila("tipo_persona").ToString() = "JURIDICA", 1, -1))
        cmbTipoDocumento.SelectedIndex = -1
        cmbTipoDocumento.SelectedItem = If(fila("tipo_documento").ToString() = "SIN DOCUMENTO",
            "Sin documento", fila("tipo_documento").ToString())
        cargando = False
        actualizarIdentificacion()
        datosIniciales = datosActuales()
    End Sub

    Private Function tipoPersona() As String
        If cmbTipoPersona.SelectedIndex < 0 Then Return Nothing
        Return If(cmbTipoPersona.SelectedIndex = 0, "NATURAL", "JURIDICA")
    End Function

    Private Function tipoDocumento() As String
        If cmbTipoDocumento.SelectedIndex < 0 Then Return Nothing
        Return cmbTipoDocumento.SelectedItem.ToString().ToUpperInvariant()
    End Function

    Private Sub actualizarIdentificacion()
        If cargando Then Return
        lblNombre.Text = If(tipoPersona() = "JURIDICA", "Razón social *", "Nombres y apellidos *")
        cmbTipoDocumento.Enabled = tipoPersona() <> "JURIDICA"
        If tipoPersona() = "JURIDICA" Then cmbTipoDocumento.SelectedItem = "RUC"
        lblAyuda.Text = "* Obligatorio. Teléfono y dirección opcionales."
        If tipoPersona() Is Nothing Then
            lblAyuda.Text &= vbCrLf & "Registro anterior: seleccione persona y documento antes de guardar."
        Else
            Select Case tipoDocumento()
                Case "DNI" : lblAyuda.Text &= vbCrLf & "DNI: 8 dígitos. Se conservan los ceros iniciales."
                Case "RUC" : lblAyuda.Text &= vbCrLf & "RUC: 11 dígitos. La validación comprueba el formato."
                Case "SIN DOCUMENTO" : lblAyuda.Text &= vbCrLf & "Sin documento: deje el número vacío."
            End Select
        End If
    End Sub

    Private Sub cmbIdentificacion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoPersona.SelectedIndexChanged, cmbTipoDocumento.SelectedIndexChanged
        actualizarIdentificacion()
    End Sub

    Private Function datosActuales() As String()
        Return {txtNombre.Text, txtDocumento.Text, txtTelefono.Text, txtDireccion.Text,
            cmbTipoPersona.SelectedIndex.ToString(), cmbTipoDocumento.SelectedIndex.ToString()}
    End Function

    Private Function hayCambios() As Boolean
        If datosIniciales Is Nothing Then Return False
        Dim actuales As String() = datosActuales()
        For i As Integer = 0 To actuales.Length - 1
            If actuales(i) <> datosIniciales(i) Then Return True
        Next
        Return False
    End Function

    Private Sub frmRegistrarClientes_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtNombre.Focus()
        txtNombre.SelectAll()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        btnGuardar.Enabled = False
        errDatos.Clear()
        Try
            If tipoPersona() Is Nothing Then Throw New ArgumentException("Seleccione el tipo de persona.", "tipoPersona")
            objCliente.ValidarDatos(txtNombre.Text, txtDocumento.Text, txtTelefono.Text,
                txtDireccion.Text, tipoPersona(), tipoDocumento())
            Dim coincidentes As DataTable = objCliente.DocumentoCoincidente(txtDocumento.Text, idCliente, tipoDocumento())
            If coincidentes.Rows.Count > 0 Then
                Throw New ArgumentException("El documento ya está registrado a nombre de " &
                    coincidentes.Rows(0)("nombre").ToString() & ". Busque el cliente existente.", "documento")
            End If
            If idCliente = 0 Then
                idClienteGuardado = objCliente.Insertar(txtNombre.Text, txtDocumento.Text,
                                                       txtTelefono.Text, txtDireccion.Text, tipoPersona(), tipoDocumento())
            Else
                objCliente.Actualizar(idCliente, txtNombre.Text, txtDocumento.Text,
                                      txtTelefono.Text, txtDireccion.Text, versionOriginal, tipoPersona(), tipoDocumento())
                idClienteGuardado = idCliente
            End If
            MessageBox.Show("Cliente guardado correctamente.", "Clientes",
                MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
        Catch ex As ArgumentException
            Dim control As Control = txtNombre
            Select Case ex.ParamName
                Case "documento" : control = txtDocumento
                Case "telefono" : control = txtTelefono
                Case "direccion" : control = txtDireccion
                Case "tipoPersona" : control = cmbTipoPersona
                Case "tipoDocumento" : control = cmbTipoDocumento
            End Select
            Dim mensaje As String = ex.Message.Split(New String() {Environment.NewLine}, StringSplitOptions.None)(0)
            errDatos.SetError(control, mensaje)
            MessageBox.Show(mensaje, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            control.Focus()
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
            errDatos.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Close()
    End Sub

    Private Sub frmRegistrarClientes_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If DialogResult = DialogResult.OK OrElse Not hayCambios() Then Return
        If MessageBox.Show("Hay cambios sin guardar. ¿Desea descartarlos?", "Clientes",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            e.Cancel = True
            DialogResult = DialogResult.None
        End If
    End Sub
End Class

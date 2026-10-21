Imports capaNegocio

Public Class frmRegistrarProveedores
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idProveedor As Integer = 0
    Public Property idProveedorGuardado As Integer = 0
    Private ReadOnly objProveedor As New Proveedor
    Private versionOriginal As Byte()
    Private cargando As Boolean = True
    Private datosIniciales As String()

    Private Sub frmRegistrarProveedores_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblModo.ForeColor = p.Titulo
        idProveedorGuardado = 0
        Try
            cmbTipoPersona.Items.AddRange(New Object() {"Natural", "Jurídica"})
            cmbTipoDocumento.Items.AddRange(New Object() {"DNI", "RUC", "Sin documento"})
            If idProveedor = 0 Then
                lblModo.Text = "NUEVO PROVEEDOR"
                txtId.Text = "(automático)"
                cmbTipoPersona.SelectedIndex = 0
                cmbTipoDocumento.SelectedIndex = 0
            Else
                cargarProveedor()
            End If
            btnRecargar.Visible = idProveedor > 0
            cargando = False
            actualizarIdentificacion()
            datosIniciales = datosActuales()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        End Try
    End Sub

    Private Sub cargarProveedor()
        Dim dt As DataTable = objProveedor.Obtener(idProveedor)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("El proveedor ya no existe. Actualice el listado.",
                "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Return
        End If
        Dim fila As DataRow = dt.Rows(0)
        lblModo.Text = "MODIFICAR PROVEEDOR"
        txtId.Text = idProveedor.ToString()
        txtNombre.Text = fila("nombre").ToString()
        txtDocumento.Text = fila("documento").ToString()
        txtTelefono.Text = fila("telefono").ToString()
        txtDireccion.Text = fila("direccion").ToString()
        versionOriginal = DirectCast(fila("version_proveedor"), Byte())
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

    Private Sub frmRegistrarProveedores_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        txtNombre.Focus()
        txtNombre.SelectAll()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        btnGuardar.Enabled = False
        errDatos.Clear()
        Try
            If tipoPersona() Is Nothing Then Throw New ArgumentException("Seleccione el tipo de persona.", "tipoPersona")
            objProveedor.ValidarDatos(txtNombre.Text, txtDocumento.Text, txtTelefono.Text,
                txtDireccion.Text, tipoPersona(), tipoDocumento())
            Dim coincidentes As DataTable = objProveedor.DocumentoCoincidente(txtDocumento.Text, idProveedor, tipoDocumento())
            If coincidentes.Rows.Count > 0 Then
                Throw New ArgumentException("El documento ya está registrado a nombre de " &
                    coincidentes.Rows(0)("nombre").ToString() & ". Busque el proveedor existente.", "documento")
            End If
            If idProveedor = 0 Then
                idProveedorGuardado = objProveedor.Insertar(txtNombre.Text, txtDocumento.Text,
                                                       txtTelefono.Text, txtDireccion.Text, tipoPersona(), tipoDocumento())
            Else
                objProveedor.Actualizar(idProveedor, txtNombre.Text, txtDocumento.Text,
                                      txtTelefono.Text, txtDireccion.Text, versionOriginal, tipoPersona(), tipoDocumento())
                idProveedorGuardado = idProveedor
            End If
            MessageBox.Show("Proveedor guardado correctamente.", "Proveedores",
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
            MessageBox.Show(mensaje, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            control.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub btnRecargar_Click(sender As Object, e As EventArgs) Handles btnRecargar.Click
        If MessageBox.Show("Se reemplazarán los datos de la pantalla por los últimos datos guardados. ¿Continuar?",
            "Recargar proveedor", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        Try
            cargarProveedor()
            errDatos.Clear()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Proveedores", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Close()
    End Sub

    Private Sub frmRegistrarProveedores_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If DialogResult = DialogResult.OK OrElse Not hayCambios() Then Return
        If MessageBox.Show("Hay cambios sin guardar. ¿Desea descartarlos?", "Proveedores",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            e.Cancel = True
            DialogResult = DialogResult.None
        End If
    End Sub
End Class

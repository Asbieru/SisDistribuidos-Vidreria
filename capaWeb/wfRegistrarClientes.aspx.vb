Imports System.Data
Imports capaNegocio

Partial Class wfRegistrarClientes
    Inherits System.Web.UI.Page

    Private ReadOnly objCliente As New Cliente()

    ' Cliente que se esta editando (0 = nuevo)
    Private Property IdClienteActual As Integer
        Get
            If ViewState("idCliente") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idCliente"))
        End Get
        Set(value As Integer)
            ViewState("idCliente") = value
        End Set
    End Property

    ' Version leida de la BD para el control de concurrencia optimista
    Private Property VersionOriginal As Byte()
        Get
            Return TryCast(ViewState("versionCliente"), Byte())
        End Get
        Set(value As Byte())
            ViewState("versionCliente") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarListado("")

            Dim id As Integer
            If Integer.TryParse(Request.QueryString("id"), id) AndAlso id > 0 Then
                CargarCliente(id)
            Else
                PrepararNuevo()
            End If
        End If

        ActualizarIdentificacion()
    End Sub

    ' ------------------------- Formulario -------------------------

    Private Sub PrepararNuevo()
        IdClienteActual = 0
        VersionOriginal = Nothing

        lblModo.Text = "NUEVO CLIENTE"
        txtId.Text = "(automático)"
        txtNombre.Text = ""
        txtDocumento.Text = ""
        txtTelefono.Text = ""
        txtDireccion.Text = ""

        ddlTipoPersona.SelectedValue = "NATURAL"
        ddlTipoDocumento.SelectedValue = "DNI"
        btnRecargar.Visible = False
        MostrarMensaje("", "")
        ActualizarIdentificacion()
    End Sub

    Private Sub CargarCliente(id As Integer)
        If id <= 0 Then
            PrepararNuevo()
            Return
        End If

        Try
            Dim dt As DataTable = objCliente.Obtener(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                PrepararNuevo()
                MostrarMensaje("El cliente ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If

            Dim fila As DataRow = dt.Rows(0)
            IdClienteActual = id
            VersionOriginal = DirectCast(fila("version_cliente"), Byte())

            lblModo.Text = "MODIFICAR CLIENTE"
            txtId.Text = id.ToString()
            txtNombre.Text = Texto(fila("nombre"))
            txtDocumento.Text = Texto(fila("documento"))
            txtTelefono.Text = Texto(fila("telefono"))
            txtDireccion.Text = Texto(fila("direccion"))

            SeleccionarTipoPersona(Texto(fila("tipo_persona")))
            SeleccionarTipoDocumento(Texto(fila("tipo_documento")))

            btnRecargar.Visible = True
            MostrarMensaje("", "")
            ActualizarIdentificacion()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub SeleccionarTipoPersona(valor As String)
        valor = If(valor, "").Trim().ToUpperInvariant()
        If valor = "NATURAL" OrElse valor = "JURIDICA" Then
            ddlTipoPersona.SelectedValue = valor
        Else
            ddlTipoPersona.SelectedIndex = -1
        End If
    End Sub

    Private Sub SeleccionarTipoDocumento(valor As String)
        valor = If(valor, "").Trim().ToUpperInvariant()
        If valor = "DNI" OrElse valor = "RUC" OrElse valor = "SIN DOCUMENTO" Then
            ddlTipoDocumento.SelectedValue = valor
        Else
            ddlTipoDocumento.SelectedIndex = -1
        End If
    End Sub

    Protected Sub ddlIdentificacion_SelectedIndexChanged(sender As Object, e As EventArgs)
        ActualizarIdentificacion()
    End Sub

    ' Ajusta la etiqueta del nombre, el documento y el texto de ayuda segun el tipo.
    Private Sub ActualizarIdentificacion()
        Dim esJuridica As Boolean = ddlTipoPersona.SelectedValue = "JURIDICA"

        lblNombreCampo.Text = If(esJuridica, "Razón social * :", "Nombres y apellidos * :")

        If esJuridica Then
            ddlTipoDocumento.SelectedValue = "RUC"
        End If
        ddlTipoDocumento.Enabled = Not esJuridica

        lblAyuda.Text = "* Obligatorio. Teléfono y dirección opcionales."
        Select Case ddlTipoDocumento.SelectedValue
            Case "DNI"
                lblAyuda.Text &= " DNI: 8 dígitos. Se conservan los ceros iniciales."
            Case "RUC"
                lblAyuda.Text &= " RUC: 11 dígitos. La validación comprueba el formato."
            Case "SIN DOCUMENTO"
                lblAyuda.Text &= " Sin documento: deje el número vacío."
        End Select
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        PrepararNuevo()
        txtNombre.Focus()
    End Sub

    Protected Sub btnRecargar_Click(sender As Object, e As EventArgs)
        If IdClienteActual > 0 Then
            CargarCliente(IdClienteActual)
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        btnGuardar.Enabled = False
        Try
            Dim tipoPersona As String = ddlTipoPersona.SelectedValue
            Dim tipoDocumento As String = ddlTipoDocumento.SelectedValue

            If String.IsNullOrEmpty(tipoPersona) Then
                Throw New ArgumentException("Seleccione el tipo de persona.", "tipoPersona")
            End If
            If String.IsNullOrEmpty(tipoDocumento) Then
                Throw New ArgumentException("Seleccione el tipo de documento.", "tipoDocumento")
            End If

            objCliente.ValidarDatos(txtNombre.Text, txtDocumento.Text, txtTelefono.Text,
                                    txtDireccion.Text, tipoPersona, tipoDocumento)

            Dim coincidentes As DataTable = objCliente.DocumentoCoincidente(
                txtDocumento.Text, IdClienteActual, tipoDocumento)
            If coincidentes IsNot Nothing AndAlso coincidentes.Rows.Count > 0 Then
                Throw New ArgumentException("El documento ya está registrado a nombre de " &
                    coincidentes.Rows(0)("nombre").ToString() & ". Busque el cliente existente.", "documento")
            End If

            Dim idGuardado As Integer
            If IdClienteActual = 0 Then
                idGuardado = objCliente.Insertar(txtNombre.Text, txtDocumento.Text,
                                                 txtTelefono.Text, txtDireccion.Text,
                                                 tipoPersona, tipoDocumento)
            Else
                objCliente.Actualizar(IdClienteActual, txtNombre.Text, txtDocumento.Text,
                                      txtTelefono.Text, txtDireccion.Text, VersionOriginal,
                                      tipoPersona, tipoDocumento)
                idGuardado = IdClienteActual
            End If

            CargarCliente(idGuardado)
            CargarListado(txtBuscar.Text)
            MostrarMensaje("Cliente guardado correctamente.", "mensaje-ok")
        Catch ex As ArgumentException
            MostrarMensaje(ex.Message, "mensaje-error")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    ' ------------------------- Listado -------------------------

    Private Sub CargarListado(filtro As String)
        Try
            Dim dt As DataTable
            If String.IsNullOrWhiteSpace(filtro) Then
                dt = objCliente.Listar()
            Else
                dt = objCliente.Buscar(filtro)
            End If

            grvClientes.DataSource = dt
            grvClientes.DataBind()

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                lblMensajeLista.CssClass = "mensaje-info"
                lblMensajeLista.Text = "No se encontraron clientes."
            Else
                lblMensajeLista.CssClass = ""
                lblMensajeLista.Text = dt.Rows.Count.ToString() & " cliente(s) encontrado(s)."
            End If
        Catch ex As Exception
            lblMensajeLista.CssClass = "mensaje-error"
            lblMensajeLista.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        CargarListado(txtBuscar.Text)
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscar.Text = ""
        CargarListado("")
    End Sub

    Protected Sub grvClientes_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim id As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), id) OrElse id <= 0 Then Return

        If e.CommandName = "EditarCliente" Then
            CargarCliente(id)
        ElseIf e.CommandName = "EliminarCliente" Then
            EliminarCliente(id)
        End If
    End Sub

    Private Sub EliminarCliente(id As Integer)
        Try
            objCliente.Eliminar(id)

            If IdClienteActual = id Then
                PrepararNuevo()
            End If

            CargarListado(txtBuscar.Text)
            MostrarMensaje("Cliente eliminado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Function Texto(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        Return valor.ToString()
    End Function
End Class

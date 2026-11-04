Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfRegistrarPedido
    Inherits System.Web.UI.Page

    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objCliente As New Cliente()
    Private ReadOnly objInventario As New Inventario()

    ' Fila del detalle en edicion (-1 = ninguna)
    Private Property IndiceEditando As Integer
        Get
            If ViewState("indiceEdicion") Is Nothing Then Return -1
            Return Convert.ToInt32(ViewState("indiceEdicion"))
        End Get
        Set(value As Integer)
            ViewState("indiceEdicion") = value
        End Set
    End Property

    ' Unidad de costo de la variante seleccionada
    Private Property UnidadSeleccionada As String
        Get
            Return If(ViewState("unidadSeleccionada"), "").ToString()
        End Get
        Set(value As String)
            ViewState("unidadSeleccionada") = value
        End Set
    End Property

    ' Items del pedido (se conservan entre postbacks)
    Private Property Items As DataTable
        Get
            Dim dt As DataTable = TryCast(ViewState("itemsPedido"), DataTable)
            If dt Is Nothing Then
                dt = CrearTablaItems()
                ViewState("itemsPedido") = dt
            End If
            Return dt
        End Get
        Set(value As DataTable)
            ViewState("itemsPedido") = value
        End Set
    End Property

    ' Variantes de vidrio y aluminio (se cargan una vez)
    Private Property Variantes As DataTable
        Get
            Return TryCast(ViewState("variantesPedido"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("variantesPedido") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarClientes("")
            CargarVariantes()
            EnlazarVariantes()

            Dim nombre As String = If(Session("usuario") Is Nothing, "", Session("usuario").ToString())
            lblUsuario.Text = Server.HtmlEncode(nombre)

            PrepararNuevoPedido()
        End If

        ActualizarCamposVariante()
    End Sub

    ' ------------------------- Cliente -------------------------

    Private Sub CargarClientes(filtro As String)
        Try
            Dim dt As DataTable
            If String.IsNullOrWhiteSpace(filtro) Then
                dt = objCliente.Listar()
            Else
                dt = objCliente.Buscar(filtro)
            End If
            If dt Is Nothing Then dt = New DataTable()

            ddlCliente.DataSource = dt
            ddlCliente.DataTextField = "nombre"
            ddlCliente.DataValueField = "id_cliente"
            ddlCliente.DataBind()
            ddlCliente.Items.Insert(0, New ListItem("(Seleccione un cliente)", ""))

            If dt.Rows.Count = 0 Then
                MostrarMensaje("No se encontraron clientes. Registre uno en el módulo de clientes.", "mensaje-error")
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnBuscarCliente_Click(sender As Object, e As EventArgs)
        CargarClientes(txtBuscarCliente.Text)
    End Sub

    Protected Sub btnVerTodosClientes_Click(sender As Object, e As EventArgs)
        txtBuscarCliente.Text = ""
        CargarClientes("")
    End Sub

    ' ------------------------- Variantes -------------------------

    Private Sub CargarVariantes()
        Try
            Dim dt As DataTable = objInventario.ListarVariantes(True)
            If dt Is Nothing Then dt = New DataTable()
            If Not dt.Columns.Contains("descripcion") Then
                dt.Columns.Add("descripcion", GetType(String))
            End If

            For Each fila As DataRow In dt.Rows
                fila("descripcion") = fila("producto").ToString() & " | " & fila("unidad_costo").ToString()
            Next

            Variantes = dt
        Catch ex As Exception
            Dim vacia As New DataTable()
            vacia.Columns.Add("id_variante", GetType(Integer))
            vacia.Columns.Add("producto", GetType(String))
            vacia.Columns.Add("unidad_costo", GetType(String))
            vacia.Columns.Add("descripcion", GetType(String))
            Variantes = vacia
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' Filtra el combo segun el tipo de item elegido.
    Private Sub EnlazarVariantes()
        Dim dt As DataTable = Variantes
        If dt Is Nothing Then dt = New DataTable()

        Dim filtrado As DataTable = dt.Clone()
        Dim tipo As String = ddlTipoItem.SelectedValue
        For Each fila As DataRow In dt.Rows
            Dim unidad As String = If(IsDBNull(fila("unidad_costo")), "", fila("unidad_costo").ToString())
            Dim coincide As Boolean =
                (tipo = "ALUMINIO" AndAlso unidad = "CM_LINEAL") OrElse
                (tipo <> "ALUMINIO" AndAlso (unidad = "CM2" OrElse unidad = "PIE2"))
            If coincide Then filtrado.ImportRow(fila)
        Next

        ddlVariante.DataSource = filtrado
        ddlVariante.DataTextField = "descripcion"
        ddlVariante.DataValueField = "id_variante"
        ddlVariante.DataBind()
        ddlVariante.Items.Insert(0, New ListItem("(Seleccione un producto)", ""))

        UnidadSeleccionada = ""
        ActualizarCamposVariante()
    End Sub

    Protected Sub ddlTipoItem_SelectedIndexChanged(sender As Object, e As EventArgs)
        EnlazarVariantes()
        MostrarMensajeItem("", "")
    End Sub

    Protected Sub ddlVariante_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim id As Integer
        If Integer.TryParse(ddlVariante.SelectedValue, id) AndAlso id > 0 Then
            UnidadSeleccionada = UnidadDeVariante(id)
        Else
            UnidadSeleccionada = ""
        End If
        ActualizarCamposVariante()
    End Sub

    Private Function UnidadDeVariante(idVariante As Integer) As String
        Dim dt As DataTable = Variantes
        If dt Is Nothing Then Return ""
        For Each fila As DataRow In dt.Rows
            If Convert.ToInt32(fila("id_variante")) = idVariante Then
                Return If(IsDBNull(fila("unidad_costo")), "", fila("unidad_costo").ToString().Trim().ToUpperInvariant())
            End If
        Next
        Return ""
    End Function

    Private Function DescripcionDeVariante(idVariante As Integer) As String
        Dim dt As DataTable = Variantes
        If dt Is Nothing Then Return ""
        For Each fila As DataRow In dt.Rows
            If Convert.ToInt32(fila("id_variante")) = idVariante Then
                Return If(IsDBNull(fila("descripcion")), fila("producto").ToString(), fila("descripcion").ToString())
            End If
        Next
        Return ""
    End Function

    ' Muestra solo las medidas que corresponden al tipo elegido.
    Private Sub ActualizarCamposVariante()
        Dim tipo As String = ddlTipoItem.SelectedValue
        Dim esVidrio As Boolean = tipo <> "ALUMINIO"

        filaAncho.Visible = esVidrio
        filaAlto.Visible = esVidrio
        filaLargo.Visible = Not esVidrio

        Dim unidad As String = UnidadSeleccionada
        lblUnidad.Text = If(unidad.Length = 0, "(seleccione un producto)", unidad)
    End Sub

    ' ------------------------- Items -------------------------

    Protected Sub btnAgregarItem_Click(sender As Object, e As EventArgs)
        MostrarMensajeItem("", "")

        Dim idVariante As Integer
        If Not Integer.TryParse(ddlVariante.SelectedValue, idVariante) OrElse idVariante <= 0 Then
            MostrarMensajeItem("Seleccione un producto o variante.", "mensaje-error")
            Return
        End If

        Dim tipo As String = ddlTipoItem.SelectedValue
        Dim unidad As String = UnidadDeVariante(idVariante)
        If unidad.Length = 0 Then
            MostrarMensajeItem("La variante seleccionada no tiene una unidad válida.", "mensaje-error")
            Return
        End If

        Dim esVidrio As Boolean = tipo <> "ALUMINIO"
        If esVidrio AndAlso unidad <> "CM2" AndAlso unidad <> "PIE2" Then
            MostrarMensajeItem("Seleccione una variante de vidrio para el tipo Vidrio suelto.", "mensaje-error")
            Return
        End If
        If Not esVidrio AndAlso unidad <> "CM_LINEAL" Then
            MostrarMensajeItem("Seleccione una variante de aluminio para el tipo Aluminio.", "mensaje-error")
            Return
        End If

        Dim ancho As Decimal? = Nothing
        Dim alto As Decimal? = Nothing
        Dim largo As Decimal? = Nothing

        If esVidrio Then
            If Not TryParseDecimal(txtAncho.Text, ancho) OrElse ancho.GetValueOrDefault() <= 0D Then
                MostrarMensajeItem("Ingrese un ancho válido mayor a cero.", "mensaje-error")
                Return
            End If
            If Not TryParseDecimal(txtAlto.Text, alto) OrElse alto.GetValueOrDefault() <= 0D Then
                MostrarMensajeItem("Ingrese un alto válido mayor a cero.", "mensaje-error")
                Return
            End If
        Else
            If Not TryParseDecimal(txtLargo.Text, largo) OrElse largo.GetValueOrDefault() <= 0D Then
                MostrarMensajeItem("Ingrese un largo válido mayor a cero.", "mensaje-error")
                Return
            End If
        End If

        Dim materiales As Decimal
        If Not TryParseDecimal(txtPrecioMateriales.Text, materiales) OrElse materiales <= 0D Then
            MostrarMensajeItem("Ingrese el precio de los materiales (mayor a cero).", "mensaje-error")
            Return
        End If

        Dim extras As Decimal
        If Not TryParseDecimal(txtExtra.Text, extras) OrElse extras < 0D Then
            MostrarMensajeItem("Los extras deben ser un número desde cero.", "mensaje-error")
            Return
        End If

        Dim manoObra As Decimal
        If Not TryParseDecimal(txtManoObra.Text, manoObra) OrElse manoObra < 0D Then
            MostrarMensajeItem("La mano de obra debe ser un número desde cero.", "mensaje-error")
            Return
        End If

        Dim total As Decimal = materiales + extras + manoObra
        If total <= 0D Then
            MostrarMensajeItem("El total del ítem debe ser mayor a cero.", "mensaje-error")
            Return
        End If

        Dim dt As DataTable = Items
        Dim descripcion As String = DescripcionDeVariante(idVariante)
        Dim medida As String = TextoMedida(ancho, alto, largo)
        Dim etiqueta As String = txtEtiqueta.Text.Trim()
        If etiqueta.Length = 0 Then
            etiqueta = tipo & " " & (dt.Rows.Count + 1).ToString()
        End If

        If IndiceEditando >= 0 AndAlso IndiceEditando < dt.Rows.Count Then
            Dim fila As DataRow = dt.Rows(IndiceEditando)
            fila("id_variante") = idVariante
            fila("producto") = descripcion
            fila("tipo") = tipo
            fila("etiqueta") = etiqueta
            fila("unidad") = unidad
            fila("ancho_cm") = ADBNull(ancho)
            fila("alto_cm") = ADBNull(alto)
            fila("largo_cm") = ADBNull(largo)
            fila("precio_materiales") = materiales
            fila("costo_extra") = extras
            fila("mano_obra") = manoObra
            fila("total") = total
            fila("medida") = medida
        Else
            dt.Rows.Add(idVariante, descripcion, tipo, etiqueta, unidad,
                        ADBNull(ancho), ADBNull(alto), ADBNull(largo),
                        materiales, extras, manoObra, total, medida)
        End If

        Items = dt
        EnlazarItems()
        LimpiarEntradaItem()
        MostrarMensajeItem("Ítem agregado al pedido.", "mensaje-ok")
    End Sub

    Protected Sub btnCancelarItemEdicion_Click(sender As Object, e As EventArgs)
        LimpiarEntradaItem()
        MostrarMensajeItem("", "")
    End Sub

    Protected Sub grvItems_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim indice As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), indice) Then Return

        If e.CommandName = "EditarItem" Then
            CargarItemParaEdicion(indice)
        ElseIf e.CommandName = "QuitarItem" Then
            QuitarItem(indice)
        End If
    End Sub

    Private Sub CargarItemParaEdicion(indice As Integer)
        Dim dt As DataTable = Items
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return

        Dim fila As DataRow = dt.Rows(indice)
        Dim tipo As String = If(IsDBNull(fila("tipo")), "VIDRIO_SUELTO", fila("tipo").ToString())
        For Each item As ListItem In ddlTipoItem.Items
            If item.Value = tipo Then
                ddlTipoItem.SelectedValue = item.Value
                Exit For
            End If
        Next

        EnlazarVariantes()

        Dim idVariante As Integer = Convert.ToInt32(fila("id_variante"))
        For Each item As ListItem In ddlVariante.Items
            If item.Value = idVariante.ToString() Then
                ddlVariante.SelectedValue = item.Value
                Exit For
            End If
        Next

        UnidadSeleccionada = If(IsDBNull(fila("unidad")), "", fila("unidad").ToString())
        IndiceEditando = indice

        txtEtiqueta.Text = ValorTexto(fila("etiqueta"))
        txtAncho.Text = ValorTexto(fila("ancho_cm"))
        txtAlto.Text = ValorTexto(fila("alto_cm"))
        txtLargo.Text = ValorTexto(fila("largo_cm"))
        txtPrecioMateriales.Text = Convert.ToDecimal(fila("precio_materiales")).ToString("0.00", CultureInfo.InvariantCulture)
        txtExtra.Text = Convert.ToDecimal(fila("costo_extra")).ToString("0.00", CultureInfo.InvariantCulture)
        txtManoObra.Text = Convert.ToDecimal(fila("mano_obra")).ToString("0.00", CultureInfo.InvariantCulture)

        btnAgregarItem.Text = "Actualizar ítem"
        btnCancelarItemEdicion.Visible = True
        ActualizarCamposVariante()
        MostrarMensajeItem("Editando el ítem seleccionado.", "mensaje-info")
    End Sub

    Private Sub QuitarItem(indice As Integer)
        Dim dt As DataTable = Items
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return

        If indice = IndiceEditando Then
            LimpiarEntradaItem()
        End If

        dt.Rows.RemoveAt(indice)
        Items = dt
        EnlazarItems()
        MostrarMensajeItem("Ítem quitado del pedido.", "mensaje-ok")
    End Sub

    Private Sub LimpiarEntradaItem()
        IndiceEditando = -1
        UnidadSeleccionada = ""
        If ddlVariante.Items.Count > 0 Then ddlVariante.SelectedIndex = 0
        txtEtiqueta.Text = ""
        txtAncho.Text = ""
        txtAlto.Text = ""
        txtLargo.Text = ""
        txtPrecioMateriales.Text = ""
        txtExtra.Text = ""
        txtManoObra.Text = ""
        btnAgregarItem.Text = "Agregar ítem"
        btnCancelarItemEdicion.Visible = False
        ActualizarCamposVariante()
    End Sub

    ' ------------------------- Guardar pedido -------------------------

    Protected Sub btnGuardarPedido_Click(sender As Object, e As EventArgs)
        Dim idCliente As Integer
        If Not Integer.TryParse(ddlCliente.SelectedValue, idCliente) OrElse idCliente <= 0 Then
            MostrarMensaje("Seleccione un cliente antes de guardar el pedido.", "mensaje-error")
            Return
        End If

        Dim idUsuario As Integer
        If Session("idUsuario") Is Nothing OrElse
           Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            MostrarMensaje("No se pudo determinar el usuario conectado. Vuelva a iniciar sesión.", "mensaje-error")
            Return
        End If

        Dim dt As DataTable = Items
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            MostrarMensaje("Agregue al menos un ítem antes de guardar el pedido.", "mensaje-error")
            Return
        End If

        If IndiceEditando >= 0 Then
            MostrarMensaje("Pulse 'Actualizar ítem' para aplicar los cambios antes de guardar.", "mensaje-error")
            Return
        End If

        btnGuardarPedido.Enabled = False
        Try
            Dim lista As New List(Of ItemPedidoDTO)
            For Each fila As DataRow In dt.Rows
                Dim tipo As String = If(IsDBNull(fila("tipo")), "VIDRIO_SUELTO", fila("tipo").ToString())
                Dim ancho As Decimal? = ADecimalNulo(fila("ancho_cm"))
                Dim alto As Decimal? = ADecimalNulo(fila("alto_cm"))
                Dim largo As Decimal? = ADecimalNulo(fila("largo_cm"))

                Dim detalle As New DetallePedidoDTO With {
                    .IdVariante = Convert.ToInt32(fila("id_variante")),
                    .Accion = "COMPRA_NUEVA",
                    .IdPieza = Nothing,
                    .AnchoCm = ancho,
                    .AltoCm = alto,
                    .LargoCm = largo,
                    .CantidadCajas = Nothing,
                    .Subtotal = Convert.ToDecimal(fila("precio_materiales"))
                }

                Dim item As New ItemPedidoDTO With {
                    .Tipo = tipo,
                    .Etiqueta = ValorTexto(fila("etiqueta")),
                    .PrecioMateriales = Convert.ToDecimal(fila("precio_materiales")),
                    .CostoExtra = Convert.ToDecimal(fila("costo_extra")),
                    .ManoObra = Convert.ToDecimal(fila("mano_obra")),
                    .Total = Convert.ToDecimal(fila("total")),
                    .Detalles = New List(Of DetallePedidoDTO) From {detalle}
                }
                lista.Add(item)
            Next

            Dim idPedido As Integer = objPedido.Registrar(idCliente, idUsuario, lista)

            PrepararNuevoPedido()
            MostrarMensaje("Pedido N° " & idPedido.ToString() & " registrado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        Finally
            btnGuardarPedido.Enabled = True
        End Try
    End Sub

    Protected Sub btnLimpiarPedido_Click(sender As Object, e As EventArgs)
        PrepararNuevoPedido()
        MostrarMensaje("Datos del pedido limpiados.", "mensaje-info")
    End Sub

    Private Sub PrepararNuevoPedido()
        Items = CrearTablaItems()
        IndiceEditando = -1
        UnidadSeleccionada = ""
        ddlCliente.SelectedIndex = 0
        ddlTipoItem.SelectedValue = "VIDRIO_SUELTO"
        EnlazarVariantes()
        LimpiarEntradaItem()
        EnlazarItems()
        MostrarMensajeItem("", "")
    End Sub

    ' ------------------------- Grilla y total -------------------------

    Private Sub EnlazarItems()
        grvItems.DataSource = Items
        grvItems.DataBind()
        ActualizarTotal()
    End Sub

    Private Sub ActualizarTotal()
        Dim total As Decimal = 0D
        Dim dt As DataTable = Items
        If dt IsNot Nothing Then
            For Each fila As DataRow In dt.Rows
                total += Convert.ToDecimal(fila("total"))
            Next
        End If
        lblTotalPedido.Text = "TOTAL: S/ " & total.ToString("N2", CultureInfo.InvariantCulture)
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Function CrearTablaItems() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("id_variante", GetType(Integer))
        dt.Columns.Add("producto", GetType(String))
        dt.Columns.Add("tipo", GetType(String))
        dt.Columns.Add("etiqueta", GetType(String))
        dt.Columns.Add("unidad", GetType(String))
        dt.Columns.Add("ancho_cm", GetType(Object))
        dt.Columns.Add("alto_cm", GetType(Object))
        dt.Columns.Add("largo_cm", GetType(Object))
        dt.Columns.Add("precio_materiales", GetType(Decimal))
        dt.Columns.Add("costo_extra", GetType(Decimal))
        dt.Columns.Add("mano_obra", GetType(Decimal))
        dt.Columns.Add("total", GetType(Decimal))
        dt.Columns.Add("medida", GetType(String))
        Return dt
    End Function

    Private Shared Function ADBNull(valor As Decimal?) As Object
        Return If(valor.HasValue, CType(valor.Value, Object), DBNull.Value)
    End Function

    Private Shared Function ADecimalNulo(valor As Object) As Decimal?
        If valor Is Nothing OrElse IsDBNull(valor) Then Return Nothing
        Return Convert.ToDecimal(valor)
    End Function

    Private Shared Function TextoMedida(ancho As Decimal?, alto As Decimal?, largo As Decimal?) As String
        If largo.HasValue Then
            Return "Largo: " & largo.Value.ToString("0.##", CultureInfo.InvariantCulture) & " cm"
        End If
        If ancho.HasValue AndAlso alto.HasValue Then
            Return ancho.Value.ToString("0.##", CultureInfo.InvariantCulture) & " x " &
                   alto.Value.ToString("0.##", CultureInfo.InvariantCulture) & " cm"
        End If
        Return ""
    End Function

    Private Function ValorTexto(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        If TypeOf valor Is Decimal Then
            Return Convert.ToDecimal(valor).ToString("0.##", CultureInfo.InvariantCulture)
        End If
        Return valor.ToString()
    End Function

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Sub MostrarMensajeItem(texto As String, clase As String)
        lblMensajeItem.CssClass = clase
        lblMensajeItem.Text = texto
    End Sub

    Private Function TryParseDecimal(texto As String, ByRef valor As Decimal) As Boolean
        texto = If(texto, "").Trim()
        If texto.Length = 0 Then Return False

        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, valor) Then Return True
        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, valor) Then Return True
        Return Decimal.TryParse(texto.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, valor)
    End Function

    Private Function TryParseDecimal(texto As String, ByRef valor As Decimal?) As Boolean
        Dim numero As Decimal
        If TryParseDecimal(texto, numero) Then
            valor = numero
            Return True
        End If
        valor = Nothing
        Return False
    End Function
End Class

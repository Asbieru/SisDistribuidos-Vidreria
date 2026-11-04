Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfRegistrarCompras
    Inherits System.Web.UI.Page

    Private ReadOnly objCompra As New Compra()
    Private ReadOnly objVariante As New ProductoVariante()
    Private ReadOnly objProveedor As New Proveedor()

    ' Compra que se esta editando (0 = nueva)
    Private Property IdCompraActual As Integer
        Get
            If ViewState("idCompra") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idCompra"))
        End Get
        Set(value As Integer)
            ViewState("idCompra") = value
        End Set
    End Property

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

    ' Detalle de la compra (se conserva entre postbacks)
    Private Property Detalle As DataTable
        Get
            Dim dt As DataTable = TryCast(ViewState("detalleCompra"), DataTable)
            If dt Is Nothing Then
                dt = CrearTablaDetalle()
                ViewState("detalleCompra") = dt
            End If
            Return dt
        End Get
        Set(value As DataTable)
            ViewState("detalleCompra") = value
        End Set
    End Property

    ' Variantes disponibles (se cargan una vez por compra)
    Private Property Variantes As DataTable
        Get
            Return TryCast(ViewState("variantesCompra"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("variantesCompra") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarProveedores()
            CargarVariantes()
            txtFecha.Text = Date.Today.ToString("yyyy-MM-dd")
            PrepararNuevaCompra()
            CargarListado()
        End If

        ActualizarCamposVariante()
    End Sub

    ' ------------------------- Carga de combos -------------------------

    Private Sub CargarProveedores()
        Try
            Dim dt As DataTable = objProveedor.Listar()
            ddlProveedor.DataSource = dt
            ddlProveedor.DataTextField = "nombre"
            ddlProveedor.DataValueField = "id_proveedor"
            ddlProveedor.DataBind()
            ddlProveedor.Items.Insert(0, New ListItem("(Seleccione un proveedor)", ""))

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("No hay proveedores registrados. Registre uno antes de comprar.", "mensaje-error")
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub CargarVariantes()
        Try
            Dim dt As DataTable = objVariante.Listar()
            If dt Is Nothing Then dt = New DataTable()
            If Not dt.Columns.Contains("descripcion") Then
                dt.Columns.Add("descripcion", GetType(String))
            End If

            For Each fila As DataRow In dt.Rows
                Dim descripcion As String =
                    fila("nombre").ToString() & " | Variante " & fila("id_variante").ToString()
                If Not IsDBNull(fila("espesor_cm")) Then
                    descripcion &= " | Espesor: " & fila("espesor_cm").ToString() & " cm"
                End If
                If Not IsDBNull(fila("medida_cm")) Then
                    descripcion &= " | Medida: " & fila("medida_cm").ToString() & " cm"
                End If
                descripcion &= " | " & fila("unidad_costo").ToString()
                fila("descripcion") = descripcion
            Next

            Variantes = dt

            ddlVariante.DataSource = dt
            ddlVariante.DataTextField = "descripcion"
            ddlVariante.DataValueField = "id_variante"
            ddlVariante.DataBind()
            ddlVariante.Items.Insert(0, New ListItem("(Seleccione un producto)", ""))
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Entrada de productos -------------------------

    Protected Sub ddlVariante_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim id As Integer
        If Integer.TryParse(ddlVariante.SelectedValue, id) AndAlso id > 0 Then
            UnidadSeleccionada = UnidadDeVariante(id)
            CargarCostoSugerido(id)
        Else
            UnidadSeleccionada = ""
        End If
        ActualizarCamposVariante()
    End Sub

    Private Sub CargarCostoSugerido(idVariante As Integer)
        Dim costo As Decimal = CostoDeVariante(idVariante)
        If costo > 0D AndAlso String.IsNullOrWhiteSpace(txtCosto.Text) Then
            txtCosto.Text = costo.ToString("0.###", CultureInfo.InvariantCulture)
        End If
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

    Private Function CostoDeVariante(idVariante As Integer) As Decimal
        Dim dt As DataTable = Variantes
        If dt Is Nothing Then Return 0D
        For Each fila As DataRow In dt.Rows
            If Convert.ToInt32(fila("id_variante")) = idVariante Then
                If IsDBNull(fila("precio_unitario")) Then Return 0D
                Return Convert.ToDecimal(fila("precio_unitario"))
            End If
        Next
        Return 0D
    End Function

    ' Muestra los campos que corresponden a la unidad de la variante elegida.
    Private Sub ActualizarCamposVariante()
        Dim unidad As String = UnidadSeleccionada
        Dim esVidrio As Boolean = unidad = "CM2" OrElse unidad = "PIE2"
        Dim esAluminio As Boolean = unidad = "CM_LINEAL"
        Dim esPieza As Boolean = esVidrio OrElse esAluminio

        txtAncho.Enabled = esVidrio
        txtAlto.Enabled = esPieza
        lblAlto.Text = If(esAluminio, "Largo (cm) :", "Alto (cm) :")
        lblCantidad.Text = If(esPieza, "Piezas * :", "Cantidad * :")

        Select Case unidad
            Case "CM2"
                lblCosto.Text = "Costo / cm² * :"
            Case "PIE2"
                lblCosto.Text = "Costo / pie² * :"
            Case "CM_LINEAL"
                lblCosto.Text = "Costo / cm * :"
            Case "CAJA"
                lblCosto.Text = "Costo / caja * :"
            Case "UNIDAD"
                lblCosto.Text = "Costo / unidad * :"
            Case Else
                lblCosto.Text = "Costo unitario * :"
        End Select

        lblUnidad.Text = If(unidad.Length = 0, "(seleccione un producto)", unidad)
    End Sub

    Protected Sub btnAgregarDetalle_Click(sender As Object, e As EventArgs)
        lblMensajeDetalle.CssClass = ""
        lblMensajeDetalle.Text = ""

        Dim idVariante As Integer
        If Not Integer.TryParse(ddlVariante.SelectedValue, idVariante) OrElse idVariante <= 0 Then
            MostrarMensajeDetalle("Seleccione un producto.", "mensaje-error")
            Return
        End If

        Dim unidad As String = UnidadDeVariante(idVariante)
        If unidad.Length = 0 Then
            MostrarMensajeDetalle("La variante seleccionada no tiene una unidad válida.", "mensaje-error")
            Return
        End If

        Dim costo As Decimal
        If Not TryParseDecimal(txtCosto.Text, costo) OrElse costo <= 0D Then
            MostrarMensajeDetalle("El costo debe ser mayor a cero.", "mensaje-error")
            Return
        End If

        Dim piezas As Integer
        If Not Integer.TryParse(txtPiezas.Text.Trim(), piezas) OrElse piezas < 1 Then
            MostrarMensajeDetalle("La cantidad debe ser un número entero desde 1.", "mensaje-error")
            Return
        End If

        Dim esVidrio As Boolean = unidad = "CM2" OrElse unidad = "PIE2"
        Dim esAluminio As Boolean = unidad = "CM_LINEAL"
        Dim esPieza As Boolean = esVidrio OrElse esAluminio

        Dim ancho As Decimal? = Nothing
        Dim alto As Decimal? = Nothing
        Dim largo As Decimal? = Nothing

        If esVidrio Then
            If Not TryParseDecimal(txtAncho.Text, ancho) OrElse ancho.GetValueOrDefault() <= 0D Then
                MostrarMensajeDetalle("Ingrese el ancho del vidrio (mayor a cero).", "mensaje-error")
                Return
            End If
            If Not TryParseDecimal(txtAlto.Text, alto) OrElse alto.GetValueOrDefault() <= 0D Then
                MostrarMensajeDetalle("Ingrese el alto del vidrio (mayor a cero).", "mensaje-error")
                Return
            End If
        ElseIf esAluminio Then
            If Not TryParseDecimal(txtAlto.Text, largo) OrElse largo.GetValueOrDefault() <= 0D Then
                MostrarMensajeDetalle("Ingrese el largo del aluminio (mayor a cero).", "mensaje-error")
                Return
            End If
        End If

        Dim importes As Compra.ImportesDetalle
        Try
            importes = Compra.CalcularImportes(unidad, piezas, costo, ancho, alto, largo)
        Catch ex As ArgumentException
            MostrarMensajeDetalle(ex.Message, "mensaje-error")
            Return
        End Try

        Dim dt As DataTable = Detalle

        ' Los accesorios (caja/unidad) no pueden repetirse; las piezas sí.
        If Not esPieza Then
            For i As Integer = 0 To dt.Rows.Count - 1
                If i = IndiceEditando Then Continue For
                If Convert.ToInt32(dt.Rows(i)("id_variante")) = idVariante Then
                    MostrarMensajeDetalle("Esta variante ya está en la tabla.", "mensaje-error")
                    Return
                End If
            Next
        End If

        Dim totalOtros As Decimal = 0D
        For i As Integer = 0 To dt.Rows.Count - 1
            If i = IndiceEditando Then Continue For
            totalOtros += Convert.ToDecimal(dt.Rows(i)("subtotal"))
        Next

        If totalOtros + importes.Subtotal > 99999999.999D Then
            MostrarMensajeDetalle("El total supera el límite de la base de datos.", "mensaje-error")
            Return
        End If

        Dim medida As String = ""
        If esPieza Then medida = TextoMedida(ancho, alto, largo)
        Dim descripcion As String = ddlVariante.SelectedItem.Text

        If IndiceEditando >= 0 AndAlso IndiceEditando < dt.Rows.Count Then
            Dim fila As DataRow = dt.Rows(IndiceEditando)
            fila("id_variante") = idVariante
            fila("producto") = descripcion
            fila("cantidad") = importes.Cantidad
            fila("piezas") = piezas
            fila("costo_unitario") = costo
            fila("subtotal") = importes.Subtotal
            fila("unidad") = unidad
            fila("ancho_cm") = ADBNull(ancho)
            fila("alto_cm") = ADBNull(alto)
            fila("largo_cm") = ADBNull(largo)
            fila("medida") = medida
        Else
            dt.Rows.Add(idVariante, descripcion, importes.Cantidad, piezas, costo,
                        importes.Subtotal, unidad, ADBNull(ancho), ADBNull(alto),
                        ADBNull(largo), medida)
        End If

        Detalle = dt
        EnlazarDetalle()
        LimpiarEntradaProducto()
        MostrarMensajeDetalle("Producto agregado al detalle.", "mensaje-ok")
    End Sub

    Protected Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs)
        LimpiarEntradaProducto()
        MostrarMensajeDetalle("", "")
    End Sub

    Protected Sub grvDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim indice As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), indice) Then Return

        If e.CommandName = "EditarDetalle" Then
            CargarDetalleParaEdicion(indice)
        ElseIf e.CommandName = "QuitarDetalle" Then
            QuitarDetalle(indice)
        End If
    End Sub

    Private Sub CargarDetalleParaEdicion(indice As Integer)
        Dim dt As DataTable = Detalle
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return

        Dim fila As DataRow = dt.Rows(indice)
        Dim idVariante As Integer = Convert.ToInt32(fila("id_variante"))

        For Each item As ListItem In ddlVariante.Items
            If item.Value = idVariante.ToString() Then
                ddlVariante.SelectedValue = item.Value
                Exit For
            End If
        Next

        UnidadSeleccionada = If(IsDBNull(fila("unidad")), "", fila("unidad").ToString())
        IndiceEditando = indice

        txtPiezas.Text = fila("piezas").ToString()
        txtCosto.Text = Convert.ToDecimal(fila("costo_unitario")).ToString("0.###", CultureInfo.InvariantCulture)

        Dim unidad As String = UnidadSeleccionada
        If unidad = "CM2" OrElse unidad = "PIE2" Then
            txtAncho.Text = ValorTexto(fila("ancho_cm"))
            txtAlto.Text = ValorTexto(fila("alto_cm"))
        ElseIf unidad = "CM_LINEAL" Then
            txtAncho.Text = ""
            txtAlto.Text = ValorTexto(fila("largo_cm"))
        Else
            txtAncho.Text = ""
            txtAlto.Text = ""
        End If

        btnAgregarDetalle.Text = "Actualizar producto"
        btnCancelarEdicion.Visible = True
        ActualizarCamposVariante()
        MostrarMensajeDetalle("Editando el producto seleccionado.", "mensaje-info")
    End Sub

    Private Sub QuitarDetalle(indice As Integer)
        Dim dt As DataTable = Detalle
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return

        If indice = IndiceEditando Then
            LimpiarEntradaProducto()
        End If

        dt.Rows.RemoveAt(indice)
        Detalle = dt
        EnlazarDetalle()
        MostrarMensajeDetalle("Producto quitado del detalle.", "mensaje-ok")
    End Sub

    ' Vuelve al modo "nuevo producto".
    Private Sub LimpiarEntradaProducto()
        IndiceEditando = -1
        UnidadSeleccionada = ""
        ddlVariante.SelectedIndex = 0
        txtPiezas.Text = "1"
        txtAncho.Text = ""
        txtAlto.Text = ""
        txtCosto.Text = ""
        btnAgregarDetalle.Text = "Agregar producto"
        btnCancelarEdicion.Visible = False
        ActualizarCamposVariante()
    End Sub

    ' ------------------------- Guardar compra -------------------------

    Protected Sub ddlMotivo_SelectedIndexChanged(sender As Object, e As EventArgs)
        txtPedidoOrigen.Enabled = ddlMotivo.SelectedValue = "REEMPLAZO_DEFECTO"
        If Not txtPedidoOrigen.Enabled Then txtPedidoOrigen.Text = ""
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Dim idProveedor As Integer
        If Not Integer.TryParse(ddlProveedor.SelectedValue, idProveedor) OrElse idProveedor <= 0 Then
            MostrarMensaje("Seleccione un proveedor registrado.", "mensaje-error")
            Return
        End If

        If Detalle Is Nothing OrElse Detalle.Rows.Count = 0 Then
            MostrarMensaje("Agregue al menos un producto.", "mensaje-error")
            Return
        End If

        If IndiceEditando >= 0 Then
            MostrarMensaje("Pulse 'Actualizar producto' para aplicar la cantidad y el costo antes de guardar.", "mensaje-error")
            Return
        End If

        Dim fecha As Date
        If Not TryParseFecha(txtFecha.Text, fecha) Then
            MostrarMensaje("Ingrese una fecha válida.", "mensaje-error")
            Return
        End If

        Dim motivo As String = ddlMotivo.SelectedValue
        If motivo <> "REPOSICION" AndAlso motivo <> "REEMPLAZO_DEFECTO" Then
            MostrarMensaje("Seleccione un motivo válido.", "mensaje-error")
            Return
        End If

        Dim pedido As Integer? = Nothing
        If motivo = "REEMPLAZO_DEFECTO" Then
            Dim idPedido As Integer
            If Not Integer.TryParse(txtPedidoOrigen.Text.Trim(), idPedido) OrElse idPedido <= 0 Then
                MostrarMensaje("Indique el pedido de origen.", "mensaje-error")
                Return
            End If
            pedido = idPedido
        End If

        btnGuardar.Enabled = False
        Try
            Dim idUsuario As Integer? = Nothing
            Dim idSesion As Integer
            If Session("idUsuario") IsNot Nothing AndAlso Integer.TryParse(Session("idUsuario").ToString(), idSesion) Then
                idUsuario = idSesion
            End If

            Dim modificando As Boolean = IdCompraActual > 0
            Dim id As Integer = objCompra.GuardarCompra(
                ddlProveedor.SelectedItem.Text,
                fecha,
                motivo,
                pedido,
                Detalle,
                IdCompraActual,
                idUsuario,
                idProveedor)

            PrepararNuevaCompra()
            CargarListado()

            Dim mensaje As String = If(modificando,
                "Compra modificada correctamente. ID: ",
                "Compra registrada correctamente. ID: ")
            MostrarMensaje(mensaje & id.ToString(), "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs)
        PrepararNuevaCompra()
        MostrarMensaje("Datos de la compra limpiados.", "mensaje-info")
    End Sub

    Private Sub PrepararNuevaCompra()
        IdCompraActual = 0
        IndiceEditando = -1
        Detalle = CrearTablaDetalle()

        lblModo.Text = "NUEVA COMPRA"
        btnGuardar.Text = "Guardar compra"
        ddlProveedor.SelectedIndex = 0
        txtFecha.Text = Date.Today.ToString("yyyy-MM-dd")
        ddlMotivo.SelectedValue = "REPOSICION"
        txtPedidoOrigen.Text = ""
        txtPedidoOrigen.Enabled = False

        LimpiarEntradaProducto()
        EnlazarDetalle()
        MostrarMensaje("", "")
        MostrarMensajeDetalle("", "")
    End Sub

    ' ------------------------- Editar compra existente -------------------------

    Protected Sub grvCompras_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim id As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), id) OrElse id <= 0 Then Return

        If e.CommandName = "EditarCompra" Then
            CargarCompra(id)
        ElseIf e.CommandName = "EliminarCompra" Then
            EliminarCompra(id)
        End If
    End Sub

    Private Sub CargarCompra(id As Integer)
        Try
            Dim cabecera As DataTable = objCompra.ObtenerCompra(id)
            If cabecera Is Nothing OrElse cabecera.Rows.Count = 0 Then
                MostrarMensaje("La compra seleccionada ya no existe.", "mensaje-error")
                Return
            End If

            Dim fila As DataRow = cabecera.Rows(0)
            SeleccionarProveedor(fila)

            If Not IsDBNull(fila("fecha")) Then
                txtFecha.Text = Convert.ToDateTime(fila("fecha")).ToString("yyyy-MM-dd")
            End If

            Dim motivo As String = If(IsDBNull(fila("motivo")), "REPOSICION", fila("motivo").ToString())
            If motivo = "REEMPLAZO_DEFECTO" Then
                ddlMotivo.SelectedValue = "REEMPLAZO_DEFECTO"
            Else
                ddlMotivo.SelectedValue = "REPOSICION"
            End If

            If Not IsDBNull(fila("id_pedido_origen")) Then
                txtPedidoOrigen.Text = fila("id_pedido_origen").ToString()
            Else
                txtPedidoOrigen.Text = ""
            End If
            txtPedidoOrigen.Enabled = ddlMotivo.SelectedValue = "REEMPLAZO_DEFECTO"

            Dim productos As DataTable = objCompra.ObtenerDetalleParaEditar(id)
            Dim dt As DataTable = CrearTablaDetalle()
            If productos IsNot Nothing Then
                For Each producto As DataRow In productos.Rows
                    Dim ancho As Object = If(productos.Columns.Contains("ancho_cm"), producto("ancho_cm"), DBNull.Value)
                    Dim alto As Object = If(productos.Columns.Contains("alto_cm"), producto("alto_cm"), DBNull.Value)
                    Dim largo As Object = If(productos.Columns.Contains("largo_cm"), producto("largo_cm"), DBNull.Value)
                    Dim unidad As String = If(IsDBNull(producto("unidad")), "", producto("unidad").ToString())
                    Dim medida As String = ""
                    If unidad = "CM2" OrElse unidad = "PIE2" OrElse unidad = "CM_LINEAL" Then
                        medida = TextoMedida(ADecimalNulo(ancho), ADecimalNulo(alto), ADecimalNulo(largo))
                    End If

                    dt.Rows.Add(
                        Convert.ToInt32(producto("id_variante")),
                        producto("producto").ToString(),
                        Convert.ToDecimal(producto("cantidad")),
                        Convert.ToInt32(If(IsDBNull(producto("piezas")), 1, producto("piezas"))),
                        Convert.ToDecimal(producto("costo_unitario")),
                        Convert.ToDecimal(producto("subtotal")),
                        unidad,
                        ancho, alto, largo, medida)
                Next
            End If

            Detalle = dt
            IdCompraActual = id
            IndiceEditando = -1
            lblModo.Text = "MODIFICAR COMPRA - ID " & id.ToString()
            btnGuardar.Text = "Guardar cambios"
            LimpiarEntradaProducto()
            EnlazarDetalle()
            MostrarMensaje("Compra cargada para modificar.", "mensaje-info")
            MostrarMensajeDetalle("", "")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub SeleccionarProveedor(fila As DataRow)
        Dim idProveedor As Integer = 0
        If Not IsDBNull(fila("id_proveedor")) Then
            idProveedor = Convert.ToInt32(fila("id_proveedor"))
        End If

        If idProveedor > 0 Then
            For Each item As ListItem In ddlProveedor.Items
                If item.Value = idProveedor.ToString() Then
                    ddlProveedor.SelectedValue = item.Value
                    Return
                End If
            Next
        End If

        ' Si el proveedor ya no esta en el combo, se agrega temporalmente.
        Dim nombre As String = If(IsDBNull(fila("proveedor")), "", fila("proveedor").ToString())
        If nombre.Length > 0 Then
            Dim nuevo As New ListItem(nombre & " (histórico)", If(idProveedor > 0, idProveedor.ToString(), ""))
            ddlProveedor.Items.Add(nuevo)
            ddlProveedor.SelectedValue = nuevo.Value
        Else
            ddlProveedor.SelectedIndex = 0
        End If
    End Sub

    Private Sub EliminarCompra(id As Integer)
        Try
            objCompra.EliminarCompra(id)

            If IdCompraActual = id Then
                PrepararNuevaCompra()
            End If

            CargarListado()
            MostrarMensaje("Compra eliminada correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Listado y detalle -------------------------

    Protected Sub btnRecargar_Click(sender As Object, e As EventArgs)
        CargarListado()
    End Sub

    Private Sub CargarListado()
        Try
            Dim dt As DataTable = objCompra.Listar()
            If dt Is Nothing Then dt = New DataTable()

            ' Solo las 50 compras mas recientes.
            If dt.Rows.Count > 50 Then
                Dim recorte As DataTable = dt.Clone()
                For i As Integer = 0 To 49
                    recorte.ImportRow(dt.Rows(i))
                Next
                dt = recorte
            End If

            grvCompras.DataSource = dt
            grvCompras.DataBind()
        Catch ex As Exception
            lblMensajeLista.CssClass = "mensaje-error"
            lblMensajeLista.Text = ex.Message
        End Try
    End Sub

    Private Sub EnlazarDetalle()
        grvDetalle.DataSource = Detalle
        grvDetalle.DataBind()
        ActualizarTotal()
    End Sub

    Private Sub ActualizarTotal()
        Dim total As Decimal = 0D
        Dim dt As DataTable = Detalle
        If dt IsNot Nothing Then
            For Each fila As DataRow In dt.Rows
                total += Convert.ToDecimal(fila("subtotal"))
            Next
        End If
        lblTotal.Text = "TOTAL: S/ " & total.ToString("N3", CultureInfo.InvariantCulture)
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Function CrearTablaDetalle() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("id_variante", GetType(Integer))
        dt.Columns.Add("producto", GetType(String))
        dt.Columns.Add("cantidad", GetType(Decimal))
        dt.Columns.Add("piezas", GetType(Integer))
        dt.Columns.Add("costo_unitario", GetType(Decimal))
        dt.Columns.Add("subtotal", GetType(Decimal))
        dt.Columns.Add("unidad", GetType(String))
        dt.Columns.Add("ancho_cm", GetType(Object))
        dt.Columns.Add("alto_cm", GetType(Object))
        dt.Columns.Add("largo_cm", GetType(Object))
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
            Return "Largo: " & largo.Value.ToString("0.###", CultureInfo.InvariantCulture) & " cm"
        End If
        If ancho.HasValue AndAlso alto.HasValue Then
            Return ancho.Value.ToString("0.###", CultureInfo.InvariantCulture) & " x " &
                   alto.Value.ToString("0.###", CultureInfo.InvariantCulture) & " cm"
        End If
        Return ""
    End Function

    Private Function ValorTexto(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        Return Convert.ToDecimal(valor).ToString("0.###", CultureInfo.InvariantCulture)
    End Function

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Sub MostrarMensajeDetalle(texto As String, clase As String)
        lblMensajeDetalle.CssClass = clase
        lblMensajeDetalle.Text = texto
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

    Private Function TryParseFecha(texto As String, ByRef fecha As Date) As Boolean
        texto = If(texto, "").Trim()
        If texto.Length = 0 Then Return False

        If Date.TryParseExact(texto, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                              DateTimeStyles.None, fecha) Then
            Return True
        End If
        Return Date.TryParse(texto, fecha)
    End Function
End Class

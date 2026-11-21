Imports System.Data
Imports System.Globalization
Imports capaNegocio   ' capaNegocio.dll desde la carpeta Bin del sitio

' Equivalente web de frmPedidoUnitario (capaPresentacion): cabecera del pedido,
' una pestaña por item, materiales de cada item y las mismas acciones.
Partial Class wfPedidoUnitario
    Inherits System.Web.UI.Page

    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objPieza As New InventarioPieza()
    Private ReadOnly objPago As New Pago()

    Private Property IdPedido As Integer
        Get
            If ViewState("idPedido") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idPedido"))
        End Get
        Set(value As Integer)
            ViewState("idPedido") = value
        End Set
    End Property

    Protected Property IdItemSeleccionado As Integer
        Get
            If ViewState("idItem") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idItem"))
        End Get
        Set(value As Integer)
            ViewState("idItem") = value
        End Set
    End Property

    ' Items del pedido (ya con la descripción que se muestra en cada pestaña)
    Private Property ItemsDelPedido As DataTable
        Get
            Return TryCast(ViewState("itemsPedido"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("itemsPedido") = value
        End Set
    End Property

    ' Materiales del item seleccionado
    Private Property Detalles As DataTable
        Get
            Return TryCast(ViewState("detallesItem"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("detallesItem") = value
        End Set
    End Property

    Private Property EstadoActual As String
        Get
            Return If(ViewState("estadoPedido"), "").ToString()
        End Get
        Set(value As String)
            ViewState("estadoPedido") = value
        End Set
    End Property

    Private Property TotalActual As Decimal
        Get
            If ViewState("totalPedido") Is Nothing Then Return 0D
            Return Convert.ToDecimal(ViewState("totalPedido"))
        End Get
        Set(value As Decimal)
            ViewState("totalPedido") = value
        End Set
    End Property

    ' Una ventana puede tener muchos items: se muestran de 10 en 10 como en el
    ' listado y en la grilla de materiales.
    Private Const TamanoPaginaItems As Integer = 10

    Private Property PaginaItems As Integer
        Get
            If ViewState("paginaItems") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("paginaItems"))
        End Get
        Set(value As Integer)
            ViewState("paginaItems") = value
        End Set
    End Property

    Private ReadOnly Property IdUsuarioConectado As Integer
        Get
            Dim id As Integer = 0
            If Session("idUsuario") IsNot Nothing Then Integer.TryParse(Session("idUsuario").ToString(), id)
            Return id
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not IsPostBack Then
            Dim id As Integer = 0
            If Not Integer.TryParse(Request.QueryString("idPedido"), id) OrElse id <= 0 Then
                MostrarMensaje("No se indicó un pedido. Vuelva al listado.", "mensaje-error")
                Return
            End If

            IdPedido = id
            ' Igual que frmPedidoUnitario.cargarItems: promueve a EN_PROCESO los
            ' items cuyos materiales ya están todos a mano.
            Try
                objPedido.RecalcularEstados(IdPedido)
            Catch ex As Exception
                MostrarMensaje(ex.Message, "mensaje-error")
            End Try

            If Not CargarPedido() Then Return
            CargarItems()
        Else
            If IdPedido <= 0 Then Return

            ' El DataSource no se conserva entre postbacks: se reedita desde ViewState.
            If ItemsDelPedido IsNot Nothing Then BindItems()
            If IdItemSeleccionado > 0 AndAlso Detalles IsNot Nothing Then
                grvDetalles.DataSource = Detalles
                grvDetalles.DataBind()
            End If
        End If
    End Sub

    ' ------------------------- Cabecera -------------------------

    Private Function CargarPedido() As Boolean
        Try
            Dim dt As DataTable = objPedido.Obtener(IdPedido)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                pnlPedido.Visible = False
                MostrarMensaje("El pedido ya no existe. Vuelva al listado.", "mensaje-error")
                Return False
            End If

            Dim fila As DataRow = dt.Rows(0)
            EstadoActual = TextoDe(fila("estado"))
            If EstadoActual = "CANCELADO" Then
                pnlPedido.Visible = False
                MostrarMensaje("Este pedido está cancelado y no puede abrirse.", "mensaje-info")
                Return False
            End If

            Dim cliente As String = TextoDe(fila("cliente"))
            Dim usuario As String = TextoDe(fila("usuario"))
            Dim fecha As DateTime = Convert.ToDateTime(fila("fecha"))
            TotalActual = Convert.ToDecimal(fila("total"))

            lblTitulo.Text = "Pedido #" & IdPedido.ToString() & " - " & Server.HtmlEncode(cliente)
            lblSubTitulo.Text = fecha.ToString("dd/MM/yyyy HH:mm") &
                                " - Vendedor: " & Server.HtmlEncode(usuario) &
                                " - Total: S/." & FormatoNumero(TotalActual)

            Dim textoEstado As String = EstadoActual
            If dt.Columns.Contains("fecha_termino") AndAlso Not IsDBNull(fila("fecha_termino")) Then
                textoEstado &= " - Terminado: " & Convert.ToDateTime(fila("fecha_termino")).ToString("dd/MM/yyyy HH:mm")
            End If
            lblEstado.Text = Server.HtmlEncode(textoEstado)
            lblEstado.CssClass = ClaseEstado(EstadoActual)

            lnkPagar.NavigateUrl = "~/wfRegistrarPago.aspx?idPedido=" & IdPedido.ToString()
            pnlPedido.Visible = True
            Return True
        Catch ex As Exception
            pnlPedido.Visible = False
            MostrarMensaje(ex.Message, "mensaje-error")
            Return False
        End Try
    End Function

    ' ------------------------- Pestañas de items -------------------------

    Private Sub CargarItems()
        Try
            Dim origen As DataTable = objPedido.ListarItemsDetallado(IdPedido)
            Dim tabla As DataTable = If(origen, New DataTable())
            If Not tabla.Columns.Contains("descripcion_mostrar") Then
                tabla.Columns.Add("descripcion_mostrar", GetType(String))
            End If

            For Each fila As DataRow In tabla.Rows
                Dim det As DataTable = objPedido.ListarDetallesDeItem(Convert.ToInt32(fila("id_item")))
                Dim descripcion As String = TextoDe(fila("descripcion"))
                If descripcion.Trim() = "" Then descripcion = TextoDe(fila("tipo"))
                If det IsNot Nothing AndAlso det.Rows.Count > 0 Then
                    descripcion &= MedidaDetalle(det.Rows(0))
                End If
                fila("descripcion_mostrar") = Server.HtmlEncode(descripcion)
            Next

            ItemsDelPedido = tabla

            ' Si el item que veniamos viendo ya no existe, se toma el primero.
            Dim indiceSeleccion As Integer = -1
            If IdItemSeleccionado > 0 Then
                For i As Integer = 0 To tabla.Rows.Count - 1
                    If Convert.ToInt32(tabla.Rows(i)("id_item")) = IdItemSeleccionado Then
                        indiceSeleccion = i
                        Exit For
                    End If
                Next
            End If

            If indiceSeleccion >= 0 Then
                PaginaItems = indiceSeleccion \ TamanoPaginaItems
            ElseIf tabla.Rows.Count > 0 Then
                IdItemSeleccionado = Convert.ToInt32(tabla.Rows(0)("id_item"))
                PaginaItems = 0
            Else
                IdItemSeleccionado = 0
            End If

            BindItems()
            ActualizarBotonTerminar()

            If IdItemSeleccionado > 0 Then
                SeleccionarItem(IdItemSeleccionado)
            Else
                pnlItem.Visible = False
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' Enlaza solo la pagina actual de items y deja los botones/etiqueta listos.
    Private Sub BindItems()
        Dim total As Integer = If(ItemsDelPedido Is Nothing, 0, ItemsDelPedido.Rows.Count)
        Dim totalPaginas As Integer = Math.Max(1, Math.Ceiling(total / CDbl(TamanoPaginaItems)))
        If PaginaItems < 0 Then PaginaItems = 0
        If PaginaItems >= totalPaginas Then PaginaItems = totalPaginas - 1

        Dim pagina As DataTable = Nothing
        If ItemsDelPedido IsNot Nothing Then
            pagina = ItemsDelPedido.Clone()
            Dim desde As Integer = PaginaItems * TamanoPaginaItems
            Dim hasta As Integer = Math.Min(desde + TamanoPaginaItems, total) - 1
            For i As Integer = desde To hasta
                pagina.ImportRow(ItemsDelPedido.Rows(i))
            Next
        End If

        rptItems.DataSource = pagina
        rptItems.DataBind()

        Dim primero As Integer = If(total = 0, 0, PaginaItems * TamanoPaginaItems + 1)
        Dim ultimo As Integer = Math.Min((PaginaItems + 1) * TamanoPaginaItems, total)
        lblItemsPagina.Text = "Página " & (PaginaItems + 1).ToString() & " de " & totalPaginas.ToString() &
                              " (" & primero.ToString() & "-" & ultimo.ToString() & " de " & total.ToString() & ")"
        lnkItemsAnterior.Enabled = PaginaItems > 0
        lnkItemsSiguiente.Enabled = PaginaItems < totalPaginas - 1
    End Sub

    Protected Sub lnkItemsAnterior_Click(sender As Object, e As EventArgs)
        If PaginaItems > 0 Then
            PaginaItems -= 1
            BindItems()
            SeleccionarPrimeroDePagina()
        End If
    End Sub

    Protected Sub lnkItemsSiguiente_Click(sender As Object, e As EventArgs)
        Dim total As Integer = If(ItemsDelPedido Is Nothing, 0, ItemsDelPedido.Rows.Count)
        If PaginaItems < Math.Max(1, Math.Ceiling(total / CDbl(TamanoPaginaItems))) - 1 Then
            PaginaItems += 1
            BindItems()
            SeleccionarPrimeroDePagina()
        End If
    End Sub

    ' Al cambiar de pagina el item abierto puede quedar fuera: se muestra el
    ' primero de la pagina nueva para que el panel nunca quede desfasado.
    Private Sub SeleccionarPrimeroDePagina()
        If ItemsDelPedido Is Nothing Then Return
        Dim desde As Integer = PaginaItems * TamanoPaginaItems
        Dim hasta As Integer = Math.Min(desde + TamanoPaginaItems, ItemsDelPedido.Rows.Count) - 1
        For i As Integer = desde To hasta
            Dim idItem As Integer = Convert.ToInt32(ItemsDelPedido.Rows(i)("id_item"))
            If idItem = IdItemSeleccionado Then Return
        Next
        If hasta >= desde Then SeleccionarItem(Convert.ToInt32(ItemsDelPedido.Rows(desde)("id_item")))
    End Sub

    Protected Sub rptItems_ItemCommand(sender As Object, e As RepeaterCommandEventArgs)
        If e.CommandName <> "SeleccionarItem" Then Return
        Dim idItem As Integer = 0
        If Not Integer.TryParse(e.CommandArgument.ToString(), idItem) OrElse idItem <= 0 Then Return
        SeleccionarItem(idItem)
    End Sub

    Private Sub SeleccionarItem(idItem As Integer)
        IdItemSeleccionado = idItem

        If ItemsDelPedido IsNot Nothing Then
            For Each fila As DataRow In ItemsDelPedido.Rows
                If Convert.ToInt32(fila("id_item")) = idItem Then
                    ' Ya viene codificada desde CargarItems: no volver a codificar.
                    lblItemTitulo.Text = TextoDe(fila("descripcion_mostrar"))
                    lblItemPrecio.Text = "S/. " & FormatoNumero(ADecimal(fila("total")))
                    Exit For
                End If
            Next
        End If

        Detalles = ConstruirDetalles(objPedido.ListarDetallesDeItem(idItem))
        grvDetalles.PageIndex = 0
        grvDetalles.DataSource = Detalles
        grvDetalles.DataBind()
        pnlItem.Visible = True

        ' Actualiza el resaltado de la pestaña activa dentro de la pagina actual.
        BindItems()
    End Sub

    ' ------------------------- Materiales del item -------------------------

    Private Function ConstruirDetalles(dt As DataTable) As DataTable
        Dim salida As New DataTable()
        salida.Columns.Add("id_detalle", GetType(Integer))
        salida.Columns.Add("id_variante", GetType(Integer))
        salida.Columns.Add("id_pieza", GetType(Integer))
        salida.Columns.Add("accion", GetType(String))
        salida.Columns.Add("ancho", GetType(Decimal))
        salida.Columns.Add("alto", GetType(Decimal))
        salida.Columns.Add("largo", GetType(Decimal))
        salida.Columns.Add("cajas", GetType(Integer))
        salida.Columns.Add("id_compra_detalle", GetType(Integer))
        salida.Columns.Add("nombre", GetType(String))
        salida.Columns.Add("pieza", GetType(String))
        salida.Columns.Add("estado", GetType(String))

        If dt Is Nothing Then Return salida

        For Each d As DataRow In dt.Rows
            Dim accion As String = TextoDe(d("accion"))
            Dim comprado As Boolean = Entero(d("id_compra_detalle")) > 0
            Dim nueva As DataRow = salida.NewRow()

            nueva("id_detalle") = Entero(d("id_detalle"))
            nueva("id_variante") = Entero(d("id_variante"))
            nueva("id_pieza") = Entero(d("pieza_usada"))
            nueva("accion") = accion
            nueva("ancho") = DecimalDe(d("ancho_cm"))
            nueva("alto") = DecimalDe(d("alto_cm"))
            nueva("largo") = DecimalDe(d("largo_cm"))
            nueva("cajas") = Entero(d("cantidad_cajas"))
            nueva("id_compra_detalle") = Entero(d("id_compra_detalle"))
            nueva("nombre") = Server.HtmlEncode(TextoDe(d("material")) & MedidaDetalle(d))
            nueva("pieza") = Server.HtmlEncode(PiezaTexto(d, accion))
            nueva("estado") = EstadoVisual(accion, comprado)
            salida.Rows.Add(nueva)
        Next
        Return salida
    End Function

    Protected Sub grvDetalles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType <> DataControlRowType.DataRow Then Return

        Dim fila As DataRowView = TryCast(e.Row.DataItem, DataRowView)
        If fila Is Nothing Then Return

        Dim boton As LinkButton = TryCast(e.Row.FindControl("lnkAccion"), LinkButton)
        Dim usarStock As LinkButton = TryCast(e.Row.FindControl("lnkUsarStock"), LinkButton)
        Dim comprar As HyperLink = TryCast(e.Row.FindControl("lnkComprar"), HyperLink)
        Dim roto As LinkButton = TryCast(e.Row.FindControl("lnkRoto"), LinkButton)
        Dim estado As Label = TryCast(e.Row.FindControl("lblEstadoDetalle"), Label)

        Dim accion As String = TextoDe(fila("accion"))
        Dim comprado As Boolean = Convert.ToInt32(fila("id_compra_detalle")) > 0
        Dim sinPieza As Boolean = Convert.ToInt32(fila("id_pieza")) <= 0
        Dim idDetalle As String = fila("id_detalle").ToString()

        If boton IsNot Nothing Then
            If accion = "CORTA_PIEZA" Then
                boton.Text = "Confirmar corte"
                boton.CommandName = "ConfirmarCorte"
                boton.CommandArgument = idDetalle
                boton.Visible = True
            Else
                boton.Visible = False
            End If
        End If

        ' Comprar Ahora es un HyperLink con target="_blank": el pedido queda
        ' abierto en la pestaña original mientras se registra la compra.
        If comprar IsNot Nothing Then
            If accion = "COMPRA_NUEVA" AndAlso Not comprado Then
                Dim url As String = UrlComprar(fila.Row)
                comprar.Visible = url <> ""
                comprar.NavigateUrl = url
            Else
                comprar.Visible = False
            End If
        End If

        ' Una línea pendiente de compra puede haberse resuelto después con
        ' inventario (por ejemplo una pieza 50x50 para un pedido de 23x25).
        ' En ese caso se ofrece cortarla desde el stock existente.
        If usarStock IsNot Nothing Then
            If accion = "COMPRA_NUEVA" AndAlso sinPieza Then
                usarStock.Text = "Usar stock"
                usarStock.CommandName = "UsarStock"
                usarStock.CommandArgument = idDetalle
                usarStock.Visible = True
            Else
                usarStock.Visible = False
            End If
        End If

        ' Item roto es una accion por fila: solo tiene sentido sobre el material
        ' de esa fila y solo si ya tiene una pieza en inventario.
        If roto IsNot Nothing Then
            If sinPieza Then
                roto.Visible = False
            Else
                roto.CommandName = "ItemRoto"
                roto.CommandArgument = idDetalle
                roto.Visible = True
            End If
        End If

        If estado IsNot Nothing Then estado.CssClass = ClaseEstadoDetalle(accion, comprado, sinPieza)
    End Sub

    Protected Sub grvDetalles_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvDetalles.PageIndex = e.NewPageIndex
        If Detalles IsNot Nothing Then
            grvDetalles.DataSource = Detalles
            grvDetalles.DataBind()
        End If
    End Sub

    Protected Sub grvDetalles_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim idDetalle As Integer = 0
        If Not Integer.TryParse(e.CommandArgument.ToString(), idDetalle) OrElse idDetalle <= 0 Then Return
        Dim fila As DataRow = BuscarDetalle(idDetalle)
        If fila Is Nothing Then Return

        Select Case e.CommandName
            Case "ConfirmarCorte"
                ConfirmarCorte(fila)
            Case "UsarStock"
                UsarStock(fila)
            Case "ItemRoto"
                MarcarRoto(fila)
        End Select
    End Sub

    Private Function BuscarDetalle(idDetalle As Integer) As DataRow
        If Detalles Is Nothing Then Return Nothing
        For Each fila As DataRow In Detalles.Rows
            If Convert.ToInt32(fila("id_detalle")) = idDetalle Then Return fila
        Next
        Return Nothing
    End Function

    Private Sub ConfirmarCorte(fila As DataRow)
        Dim idPieza As Integer = Convert.ToInt32(fila("id_pieza"))
        If idPieza <= 0 Then
            MostrarMensaje("El material no tiene una pieza asignada para cortar.", "mensaje-error")
            Return
        End If

        Try
            Dim retazos As DataTable = objPieza.Cortar(idPieza,
                                                       MedidaONull(fila("ancho")),
                                                       MedidaONull(fila("alto")),
                                                       MedidaONull(fila("largo")),
                                                       IdUsuarioONull(), IdPedidoONull(), 10D)
            objPedido.ResolverDetalle(Convert.ToInt32(fila("id_detalle")))
            RecargarTodo()

            Dim texto As String = "Corte registrado."
            If retazos IsNot Nothing AndAlso retazos.Rows.Count > 0 Then
                texto &= " Retazos generados:"
                For Each r As DataRow In retazos.Rows
                    texto &= " Pieza #" & Server.HtmlEncode(TextoDe(r("id_pieza"))) & ": " &
                             Server.HtmlEncode(TextoMedida(r("ancho_cm"), r("alto_cm"), r("largo_cm"))) & ";"
                Next
                texto = texto.TrimEnd(";"c) & "."
            End If
            MostrarMensaje(texto, "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' El pedido pudo registrarse antes de que existiera stock, o la compra posterior
    ' no se enlazó porque la medida comprada era mayor (por ejemplo 50x50 para 23x25).
    ' Igual que en el escritorio se busca una pieza que sirva y se corta si hace falta.
    Private Sub UsarStock(fila As DataRow)
        Dim idDetalle As Integer = Convert.ToInt32(fila("id_detalle"))
        Dim idVariante As Integer = Convert.ToInt32(fila("id_variante"))
        If idVariante <= 0 Then
            MostrarMensaje("El material no tiene una variante de producto válida.", "mensaje-error")
            Return
        End If

        Dim ancho As Decimal? = MedidaONull(fila("ancho"))
        Dim alto As Decimal? = MedidaONull(fila("alto"))
        Dim largo As Decimal? = MedidaONull(fila("largo"))
        Dim cajas As Integer = Entero(fila("cajas"))

        Try
            Dim disponibles As DataTable = objPieza.BuscarDisponibles(idVariante, ancho, alto, largo)
            If disponibles Is Nothing OrElse disponibles.Rows.Count = 0 Then
                MostrarMensaje("No hay piezas disponibles para esta medida. Use Comprar Ahora.", "mensaje-info")
                Return
            End If

            Dim pieza As DataRow = disponibles.Rows(0)
            Dim idPieza As Integer = Convert.ToInt32(pieza("id_pieza"))
            Dim texto As String = ""

            If ancho.HasValue AndAlso (alto.HasValue OrElse largo.HasValue) AndAlso MedidaExacta(pieza, ancho, alto, largo) Then
                ' La pieza ya tiene la medida pedida: se entrega entera.
                objPieza.MarcarVendida(idPieza, IdUsuarioONull(), IdPedidoONull())
                texto = "Se usó la pieza #" & idPieza.ToString() & " del inventario."
            Else
                Dim retazos As DataTable = objPieza.Cortar(idPieza, ancho, alto, largo,
                                                           IdUsuarioONull(), IdPedidoONull(), 10D)
                texto = "Se cortó la pieza #" & idPieza.ToString() & "."
                If retazos IsNot Nothing AndAlso retazos.Rows.Count > 0 Then
                    texto &= " Retazos:"
                    For Each r As DataRow In retazos.Rows
                        texto &= " #" & Server.HtmlEncode(TextoDe(r("id_pieza"))) & ": " &
                                 Server.HtmlEncode(TextoMedida(r("ancho_cm"), r("alto_cm"), r("largo_cm"))) & ";"
                    Next
                    texto = texto.TrimEnd(";"c) & "."
                End If
            End If

            ' Materiales que se entregan por caja/unidad: consumir tantas piezas como cajas.
            If cajas > 1 AndAlso Not ancho.HasValue AndAlso Not largo.HasValue Then
                Dim usadas As Integer = 1
                For i As Integer = 1 To disponibles.Rows.Count - 1
                    If usadas >= cajas Then Exit For
                    objPieza.MarcarVendida(Convert.ToInt32(disponibles.Rows(i)("id_pieza")),
                                           IdUsuarioONull(), IdPedidoONull())
                    usadas += 1
                Next
                texto &= " Piezas usadas: " & usadas.ToString() & " de " & cajas.ToString() & "."
            End If

            objPedido.ResolverDetalle(idDetalle)
            RecargarTodo()
            MostrarMensaje(texto, "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Function MedidaExacta(pieza As DataRow, ancho As Decimal?, alto As Decimal?, largo As Decimal?) As Boolean
        Const tolerancia As Decimal = 0.01D
        Dim pa As Decimal = DecimalDe(pieza("ancho_cm"))
        Dim pb As Decimal = DecimalDe(pieza("alto_cm"))
        Dim pl As Decimal = DecimalDe(pieza("largo_cm"))

        If largo.HasValue AndAlso largo.Value > 0D Then
            Return Math.Abs(pl - largo.Value) <= tolerancia
        End If
        If ancho.HasValue AndAlso ancho.Value > 0D AndAlso alto.HasValue AndAlso alto.Value > 0D Then
            Return (Math.Abs(pa - ancho.Value) <= tolerancia AndAlso Math.Abs(pb - alto.Value) <= tolerancia) OrElse
                   (Math.Abs(pa - alto.Value) <= tolerancia AndAlso Math.Abs(pb - ancho.Value) <= tolerancia)
        End If
        Return False
    End Function

    ' URL de la compra con la medida ya precargada. Se usa desde un HyperLink
    ' con Target="_blank" para no perder el pedido en la pestaña actual.
    Private Function UrlComprar(fila As DataRow) As String
        Dim idVariante As Integer = Entero(fila("id_variante"))
        If idVariante <= 0 Then Return ""

        Dim url As String = "~/wfRegistrarCompras.aspx?idVariante=" & idVariante.ToString()
        Dim ancho As Decimal? = MedidaONull(fila("ancho"))
        Dim alto As Decimal? = MedidaONull(fila("alto"))
        Dim largo As Decimal? = MedidaONull(fila("largo"))
        Dim cajas As Integer? = EnteroONull(fila("cajas"))
        If ancho.HasValue Then url &= "&ancho=" & ancho.Value.ToString("0.###", CultureInfo.InvariantCulture)
        If alto.HasValue Then url &= "&alto=" & alto.Value.ToString("0.###", CultureInfo.InvariantCulture)
        If largo.HasValue Then url &= "&largo=" & largo.Value.ToString("0.###", CultureInfo.InvariantCulture)
        If cajas.HasValue Then url &= "&cajas=" & cajas.Value.ToString()
        Return ResolveUrl(url)
    End Function

    ' ------------------------- Acciones del item -------------------------

    ' La accion se ejecuta sobre la fila pulsada, no sobre la primera pieza
    ' del item como ocurria con el boton global.
    Private Sub MarcarRoto(fila As DataRow)
        Dim idDetalle As Integer = Entero(fila("id_detalle"))
        If Entero(fila("id_pieza")) <= 0 Then
            MostrarMensaje("Solo se puede marcar como roto un material que ya está en el inventario.", "mensaje-info")
            Return
        End If

        Try
            Dim resultado As DataTable = objPedido.MarcarItemRoto(idDetalle, IdUsuarioONull())
            Dim accion As String = ""
            If resultado IsNot Nothing AndAlso resultado.Rows.Count > 0 Then accion = TextoDe(resultado.Rows(0)("accion"))
            RecargarTodo()
            MostrarMensaje("El material se marcó como roto y se generó su reemplazo" &
                           If(accion = "", ".", " (" & Server.HtmlEncode(accion) & ")."), "mensaje-info")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnCancelarItem_Click(sender As Object, e As EventArgs)
        If IdItemSeleccionado <= 0 Then Return
        Try
            objPedido.CancelarItem(IdItemSeleccionado, IdUsuarioONull())
            RecargarTodo()

            If ItemsDelPedido Is Nothing OrElse ItemsDelPedido.Rows.Count = 0 Then
                pnlPedido.Visible = False
                MostrarMensaje("Se cancelaron todos los ítems: el pedido quedó CANCELADO.", "mensaje-info")
                Return
            End If
            MostrarMensaje("Ítem cancelado." & ExcedenteTexto(), "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnTerminarItem_Click(sender As Object, e As EventArgs)
        If IdItemSeleccionado <= 0 Then Return
        Try
            objPedido.TerminarItem(IdItemSeleccionado, IdUsuarioONull())
            RecargarTodo()
            MostrarMensaje("Ítem marcado como COMPLETADO.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Acciones del pedido -------------------------

    Protected Sub btnTerminarPedido_Click(sender As Object, e As EventArgs)
        If IdPedido <= 0 Then Return
        Try
            objPedido.Terminar(IdPedido, IdUsuarioONull())
            RecargarTodo()
            MostrarMensaje("Pedido marcado como terminado.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' El boton solo aparece cuando ningun item sigue PENDIENTE, es decir cuando
    ' todos sus materiales fueron comprados/registrados o estan en inventario.
    Private Sub ActualizarBotonTerminar()
        btnTerminarPedido.Visible = False
        btnTerminarPedido.ToolTip = "Todos los ítems deben estar comprados y registrados como suyos."
        If ItemsDelPedido Is Nothing OrElse ItemsDelPedido.Rows.Count = 0 Then Return
        If Not ItemsDelPedido.Columns.Contains("estado") Then Return

        For Each fila As DataRow In ItemsDelPedido.Rows
            If TextoDe(fila("estado")) = "PENDIENTE" Then Return
        Next
        btnTerminarPedido.Visible = True
    End Sub

    Private Sub RecargarTodo()
        Try
            objPedido.RecalcularEstados(IdPedido)
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
        If CargarPedido() Then CargarItems()
    End Sub

    Private Function ExcedenteTexto() As String
        If IdPedido <= 0 Then Return ""
        Dim pagado As Decimal = objPago.TotalPagado(IdPedido)
        If pagado <= TotalActual Then Return ""
        Return " El cliente ya pagó S/." & FormatoNumero(pagado) &
               " y el pedido quedó en S/." & FormatoNumero(TotalActual) &
               ". Excedente a favor del cliente: S/." & FormatoNumero(pagado - TotalActual) & "."
    End Function

    ' ------------------------- Utilidades -------------------------

    Private Function EstadoVisual(accion As String, comprado As Boolean) As String
        Select Case accion
            Case "USA_EXISTENTE"
                Return "En inventario"
            Case "CORTA_PIEZA"
                Return "Cortar retazo"
            Case Else
                Return If(comprado, "Comprado", "Comprar nuevo")
        End Select
    End Function

    Private Function PiezaTexto(d As DataRow, accion As String) As String
        If Not IsDBNull(d("pieza_usada")) AndAlso Convert.ToInt32(d("pieza_usada")) > 0 Then
            Dim medidas As String = MedidaPieza(d)
            If accion = "CORTA_PIEZA" Then
                Return "#" & d("pieza_usada").ToString() & " de " & medidas
            End If
            Return "#" & d("pieza_usada").ToString() & ", " & medidas
        End If
        If accion = "COMPRA_NUEVA" Then Return "No hay stock registrado"
        Return ""
    End Function

    Private Function MedidaPieza(d As DataRow) As String
        Dim largo As Decimal = DecimalDe(d("pieza_largo"))
        If largo > 0D Then Return FormatoMedida(largo) & " cm"
        Dim ancho As Decimal = DecimalDe(d("pieza_ancho"))
        Dim alto As Decimal = DecimalDe(d("pieza_alto"))
        If ancho > 0D AndAlso alto > 0D Then
            Return FormatoMedida(ancho) & " x " & FormatoMedida(alto) & " cm"
        End If
        Return ""
    End Function

    Private Function MedidaDetalle(d As DataRow) As String
        Dim largo As Decimal = DecimalDe(d("largo_cm"))
        If largo > 0D Then Return " · " & FormatoMedida(largo) & " cm"

        Dim ancho As Decimal = DecimalDe(d("ancho_cm"))
        Dim alto As Decimal = DecimalDe(d("alto_cm"))
        If ancho > 0D AndAlso alto > 0D Then
            Return " · " & FormatoMedida(ancho) & " x " & FormatoMedida(alto) & " cm"
        End If

        If Not IsDBNull(d("cantidad_cajas")) Then
            Dim unidad As String = "caja(s)"
            If d.Table.Columns.Contains("unidad_costo") AndAlso
               TextoDe(d("unidad_costo")).Equals("UNIDAD", StringComparison.OrdinalIgnoreCase) Then
                unidad = "unidad(es)"
            End If
            Return " · " & Convert.ToInt32(d("cantidad_cajas")).ToString() & " " & unidad
        End If
        Return ""
    End Function

    Private Function TextoMedida(ancho As Object, alto As Object, largo As Object) As String
        Dim l As Decimal = DecimalDe(largo)
        If l > 0D Then Return FormatoMedida(l) & " cm"
        Dim a As Decimal = DecimalDe(ancho)
        Dim b As Decimal = DecimalDe(alto)
        If a > 0D AndAlso b > 0D Then Return FormatoMedida(a) & " x " & FormatoMedida(b) & " cm"
        Return ""
    End Function

    Private Function FormatoMedida(valor As Decimal) As String
        Return valor.ToString("0.###", CultureInfo.InvariantCulture)
    End Function

    Private Function FormatoNumero(valor As Decimal) As String
        Return valor.ToString("0.00", CultureInfo.InvariantCulture)
    End Function

    Private Function TextoDe(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        Return valor.ToString()
    End Function

    Private Function DecimalDe(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Dim resultado As Decimal = 0D
        If Decimal.TryParse(valor.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, resultado) Then Return resultado
        Return 0D
    End Function

    Private Function Entero(valor As Object) As Integer
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0
        Dim resultado As Integer = 0
        If Integer.TryParse(valor.ToString(), resultado) Then Return resultado
        Return 0
    End Function

    Private Function ADecimal(valor As Object) As Decimal
        Return DecimalDe(valor)
    End Function

    Private Function MedidaONull(valor As Object) As Decimal?
        Dim resultado As Decimal = DecimalDe(valor)
        If resultado <= 0D Then Return Nothing
        Return resultado
    End Function

    Private Function EnteroONull(valor As Object) As Integer?
        Dim resultado As Integer = Entero(valor)
        If resultado <= 0 Then Return Nothing
        Return resultado
    End Function

    Private Function IdUsuarioONull() As Integer?
        Return If(IdUsuarioConectado > 0, CType(IdUsuarioConectado, Integer?), Nothing)
    End Function

    Private Function IdPedidoONull() As Integer?
        Return If(IdPedido > 0, CType(IdPedido, Integer?), Nothing)
    End Function

    Private Function ClaseEstado(estado As String) As String
        Select Case estado
            Case "PAGADO"
                Return "estado-pagado"
            Case "PARCIAL"
                Return "estado-parcial"
            Case "PENDIENTE"
                Return "estado-pendiente"
            Case "CANCELADO"
                Return "estado-cancelado"
            Case Else
                Return ""
        End Select
    End Function

    ' Misma paleta que wfListadoPedidos aplicada al Estado de cada material.
    Private Function ClaseEstadoDetalle(accion As String, comprado As Boolean, sinPieza As Boolean) As String
        If accion = "USA_EXISTENTE" Then Return "estado-pagado"
        If accion = "CORTA_PIEZA" Then Return "estado-parcial"
        If comprado AndAlso Not sinPieza Then Return "estado-pagado"
        If comprado Then Return "estado-parcial"
        Return "estado-pendiente"
    End Function

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub
End Class

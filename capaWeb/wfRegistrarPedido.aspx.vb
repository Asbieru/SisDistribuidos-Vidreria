Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.Text
Imports System.Web.UI.HtmlControls

Partial Class wfRegistrarPedido
    Inherits System.Web.UI.Page

    ' Todas las clases vienen de los DLL que ya estan en Bin (no se agrega
    ' ninguna referencia a capaPresentacion ni a capaDatos).
    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objCliente As New Cliente()
    Private ReadOnly objProducto As New Producto()
    Private ReadOnly objVariante As New ProductoVariante()
    Private ReadOnly objPieza As New InventarioPieza()

    Private Const TOLERANCIA_CM As Decimal = 0.01D
    Private Const VENTANA_ANCHO_MINIMO_CM As Decimal = 105D
    Private Const VENTANA_ALERO_CM As Decimal = 35D
    Private Const VENTANA_ALERO_ALTO_MINIMO_CM As Decimal = 90D

    ' ------------------------- Estado -------------------------

    Private Property IndiceEditando As Integer
        Get
            If ViewState("indiceEdicion") Is Nothing Then Return -1
            Return Convert.ToInt32(ViewState("indiceEdicion"))
        End Get
        Set(value As Integer)
            ViewState("indiceEdicion") = value
        End Set
    End Property

    Private Shadows Property Items As DataTable
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

    ' Modelos de ventana activos (nombre -> id_modelo).
    Private Property Modelos As DataTable
        Get
            Dim dt As DataTable = TryCast(ViewState("modelosPedido"), DataTable)
            If dt Is Nothing Then
                dt = New DataTable()
                dt.Columns.Add("id_modelo", GetType(Integer))
                dt.Columns.Add("nombre", GetType(String))
                ViewState("modelosPedido") = dt
            End If
            Return dt
        End Get
        Set(value As DataTable)
            ViewState("modelosPedido") = value
        End Set
    End Property

    Private Property Productos As DataTable
        Get
            Return TryCast(ViewState("productosPedido"), DataTable)
        End Get
        Set(value As DataTable)
            ViewState("productosPedido") = value
        End Set
    End Property

    Private Property Espesores As List(Of Decimal)
        Get
            Dim lista As List(Of Decimal) = TryCast(ViewState("espesoresPedido"), List(Of Decimal))
            If lista Is Nothing Then
                lista = New List(Of Decimal)()
                ViewState("espesoresPedido") = lista
            End If
            Return lista
        End Get
        Set(value As List(Of Decimal))
            ViewState("espesoresPedido") = value
        End Set
    End Property

    Private Property ColoresAluminio As List(Of String)
        Get
            Dim lista As List(Of String) = TryCast(ViewState("coloresPedido"), List(Of String))
            If lista Is Nothing Then
                lista = New List(Of String)()
                ViewState("coloresPedido") = lista
            End If
            Return lista
        End Get
        Set(value As List(Of String))
            ViewState("coloresPedido") = value
        End Set
    End Property

    Private Property ErrorModelos As String
        Get
            Return If(ViewState("errorModelos"), "").ToString()
        End Get
        Set(value As String)
            ViewState("errorModelos") = value
        End Set
    End Property

    ' ------------------------- Carga -------------------------

    ' Se enlaza en Init (antes de que ASP.NET procese el valor publicado) para
    ' que ddlCliente siempre tenga los items y nunca falle al restaurar la selección.
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        CargarClientes()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        PrepararCombos()

        If Not IsPostBack Then
            CargarModelos()
            EnlazarTiposDePedido()
            CargarColoresAluminio()

            PrepararNuevoPedido()
            If ErrorModelos <> "" Then
                MostrarMensaje("No se pudieron cargar los modelos de ventana: " & ErrorModelos, "mensaje-error")
            End If
            AplicarDesdeConsulta()
        Else
            SincronizarCombos()
        End If

        AplicarVisibilidadItem()
        CalcularVistaItem(False)
    End Sub

    ' Atributos que permiten escribir en los combos y usar el datalist nativo.
    Private Sub PrepararCombos()
        ConfigurarBusqueda(txtCliente, "dlClientes", "Escriba para buscar un cliente")
        ConfigurarBusqueda(txtProducto, "dlProductos", "Escriba para buscar un producto")
        ConfigurarBusqueda(txtEspesor, "dlEspesores", "Escriba para buscar un espesor")
        ConfigurarBusqueda(txtColorAluminio, "dlColores", "Escriba para buscar un color")
    End Sub

    Private Sub ConfigurarBusqueda(caja As TextBox, idLista As String, aviso As String)
        caja.Attributes("list") = idLista
        caja.Attributes("autocomplete") = "off"
        caja.Attributes("placeholder") = aviso
    End Sub

    ' El usuario puede escribir parte del texto: si no hay coincidencia exacta
    ' se toma la primera opcion que contenga lo escrito.
    Private Sub SincronizarCombos()
        SincronizarCombo(txtCliente, ddlCliente)
        SincronizarCombo(txtProducto, ddlProducto)
        SincronizarComboNumerico(txtEspesor, ddlEspesor)
        SincronizarCombo(txtColorAluminio, ddlColorAluminio)
    End Sub

    ' El espesor se escribe con punto o con coma (0.3 / 0,30).
    Private Sub SincronizarComboNumerico(caja As TextBox, combo As DropDownList)
        If combo.Items.Count = 0 Then Return
        Dim texto As String = If(caja.Text, "").Trim()
        If texto.Length = 0 Then
            combo.SelectedIndex = 0
            Return
        End If

        Dim valor As Decimal
        If TryParseDecimal(texto, valor) Then
            For i As Integer = 0 To combo.Items.Count - 1
                Dim valorItem As Decimal
                If TryParseDecimal(combo.Items(i).Value, valorItem) AndAlso valorItem = valor Then
                    combo.SelectedIndex = i
                    Return
                End If
            Next
        End If
        SincronizarCombo(caja, combo)
    End Sub

    Private Sub SincronizarCombo(caja As TextBox, combo As DropDownList)
        If combo.Items.Count = 0 Then Return
        Dim texto As String = If(caja.Text, "").Trim()
        If texto.Length = 0 Then
            combo.SelectedIndex = 0
            Return
        End If

        ' Se usa SelectedIndex en vez de SelectedValue: así nunca se asigna un
        ' valor que no esté en la lista (origen del error en ddlCliente).
        For i As Integer = 0 To combo.Items.Count - 1
            Dim valor As String = If(combo.Items(i).Value, "")
            If combo.Items(i).Text.Trim().Equals(texto, StringComparison.OrdinalIgnoreCase) OrElse
               (valor <> "" AndAlso valor.Trim().Equals(texto, StringComparison.OrdinalIgnoreCase)) Then
                combo.SelectedIndex = i
                Return
            End If
        Next

        For i As Integer = 0 To combo.Items.Count - 1
            If combo.Items(i).Value <> "" AndAlso
               combo.Items(i).Text.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 Then
                combo.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    ' El filtro de clientes lo hace el datalist en el navegador; aqui solo se
    ' carga el catalogo completo y nunca se sobreescribe lo que el usuario escribe.
    Private Sub CargarClientes()
        Try
            Dim dt As DataTable = objCliente.Listar()
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

    Private Sub CargarModelos()
        Dim dt As New DataTable()
        dt.Columns.Add("id_modelo", GetType(Integer))
        dt.Columns.Add("nombre", GetType(String))
        Try
            For Each modelo As ModeloVentana In ModeloVentana.ListarActivos()
                dt.Rows.Add(modelo.IdModelo, modelo.Nombre)
            Next
        Catch ex As Exception
            ErrorModelos = ex.Message
        End Try
        Modelos = dt
    End Sub

    ' ALUMINIO, VIDRIO y un modelo de ventana por cada fila activa.
    Private Sub EnlazarTiposDePedido()
        Dim actual As String = ddlTipoItem.SelectedValue
        ddlTipoItem.Items.Clear()
        ddlTipoItem.Items.Add(New ListItem("ALUMINIO", "ALUMINIO"))
        ddlTipoItem.Items.Add(New ListItem("VIDRIO", "VIDRIO"))
        For Each fila As DataRow In Modelos.Rows
            Dim nombre As String = fila("nombre").ToString()
            ddlTipoItem.Items.Add(New ListItem(nombre, nombre))
        Next

        If actual <> "" Then
            For Each item As ListItem In ddlTipoItem.Items
                If item.Value = actual Then
                    ddlTipoItem.SelectedValue = actual
                    Exit For
                End If
            Next
        End If
        If ddlTipoItem.SelectedIndex < 0 Then ddlTipoItem.SelectedIndex = 0
    End Sub

    Private Function TipoSeleccionado() As String
        Dim seleccion As String = ddlTipoItem.SelectedValue
        If seleccion.Equals("ALUMINIO", StringComparison.OrdinalIgnoreCase) Then Return "ALUMINIO"
        If seleccion.Equals("VIDRIO", StringComparison.OrdinalIgnoreCase) Then Return "VIDRIO"
        If IdModeloDeSeleccion(seleccion) > 0 Then Return "VENTANA"
        Return "VIDRIO"
    End Function

    Private Function IdModeloDeSeleccion(seleccion As String) As Integer
        For Each fila As DataRow In Modelos.Rows
            If fila("nombre").ToString().Equals(seleccion, StringComparison.OrdinalIgnoreCase) Then
                Return Convert.ToInt32(fila("id_modelo"))
            End If
        Next
        Return 0
    End Function

    Private Function CategoriaDeTipo(tipo As String) As String
        If tipo = "VENTANA" Then Return "VIDRIO"
        Return tipo
    End Function

    Private Sub CargarProductos(categoria As String, filtro As String)
        Dim dt As DataTable = Nothing
        Try
            dt = objProducto.ListarPorCategoria(categoria)
        Catch ex As Exception
            MostrarMensajeItem(ex.Message, "mensaje-error")
        End Try
        If dt Is Nothing Then dt = New DataTable()

        If Not dt.Columns.Contains("descripcion") Then dt.Columns.Add("descripcion", GetType(String))
        For Each fila As DataRow In dt.Rows
            Dim texto As String = ValorTexto(fila("nombre"))
            Dim tipo As String = ValorTexto(fila("tipo"))
            Dim color As String = If(dt.Columns.Contains("color"), ValorTexto(fila("color")), "")
            fila("descripcion") = texto & " | " & tipo & If(color = "", "", " " & color)
        Next

        Productos = dt
        EnlazarProductos(filtro)
    End Sub

    Private Sub EnlazarProductos(filtro As String)
        Dim dt As DataTable = Productos
        If dt Is Nothing Then dt = New DataTable()

        Dim anterior As String = ddlProducto.SelectedValue
        Dim filtrado As DataTable = dt.Clone()
        Dim texto As String = If(filtro, "").Trim()
        For Each fila As DataRow In dt.Rows
            If texto.Length = 0 OrElse
               fila("descripcion").ToString().IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 Then
                filtrado.ImportRow(fila)
            End If
        Next

        ddlProducto.DataSource = filtrado
        ddlProducto.DataTextField = "descripcion"
        ddlProducto.DataValueField = "id_producto"
        ddlProducto.DataBind()
        ddlProducto.Items.Insert(0, New ListItem("(Seleccione un producto)", ""))

        If anterior <> "" Then
            For Each item As ListItem In ddlProducto.Items
                If item.Value = anterior Then
                    ddlProducto.SelectedValue = anterior
                    Exit For
                End If
            Next
        End If
        ' Conserva siempre lo escrito por el usuario; el placeholder solo vive
        ' en el DropDownList oculto y no debe copiarse al cuadro de busqueda.
        If String.IsNullOrWhiteSpace(txtProducto.Text) AndAlso ddlProducto.SelectedIndex > 0 Then
            txtProducto.Text = ddlProducto.SelectedItem.Text
        End If
    End Sub

    Private Sub CargarEspesores(idProducto As Integer)
        Espesores = New List(Of Decimal)()
        ddlEspesor.Items.Clear()
        ddlEspesor.Items.Add(New ListItem("(Seleccione un espesor)", ""))
        txtEspesor.Text = ""

        If idProducto <= 0 Then Return
        Try
            Espesores = objVariante.ListarEspesoresPorProducto(idProducto)
        Catch ex As Exception
            MostrarMensajeItem(ex.Message, "mensaje-error")
        End Try

        For Each espesor As Decimal In Espesores
            Dim texto As String = espesor.ToString("0.##", CultureInfo.InvariantCulture)
            ddlEspesor.Items.Add(New ListItem(texto, espesor.ToString("0.####", CultureInfo.InvariantCulture)))
        Next
    End Sub

    Private Sub CargarColoresAluminio()
        Try
            ColoresAluminio = objVariante.ListarColoresAluminio()
        Catch ex As Exception
            ColoresAluminio = New List(Of String)()
        End Try

        ddlColorAluminio.Items.Clear()
        ddlColorAluminio.Items.Add(New ListItem("(Seleccione un color)", ""))
        For Each color As String In ColoresAluminio
            ddlColorAluminio.Items.Add(New ListItem(color, color))
        Next
        txtColorAluminio.Text = ""
    End Sub

    ' ------------------------- Eventos de los combos -------------------------

    Protected Sub ddlTipoItem_SelectedIndexChanged(sender As Object, e As EventArgs)
        txtProducto.Text = ""
        CargarProductos(CategoriaDeTipo(TipoSeleccionado()), "")
        CargarEspesores(0)
        txtExtra.Text = "0.00"
        MostrarMensajeItem("", "")
        AplicarVisibilidadItem()
        CalcularVistaItem(True)
    End Sub

    Protected Sub ddlProducto_SelectedIndexChanged(sender As Object, e As EventArgs)
        SincronizarCombo(txtProducto, ddlProducto)
        CargarEspesores(IdProductoSeleccionado())
        CalcularVistaItem(True)
    End Sub

    Protected Sub ddlEspesor_SelectedIndexChanged(sender As Object, e As EventArgs)
        SincronizarComboNumerico(txtEspesor, ddlEspesor)
        CalcularVistaItem(True)
    End Sub

    Protected Sub ddlColorAluminio_SelectedIndexChanged(sender As Object, e As EventArgs)
        SincronizarCombo(txtColorAluminio, ddlColorAluminio)
        CalcularVistaItem(True)
    End Sub

    Protected Sub Medida_CheckedChanged(sender As Object, e As EventArgs)
        AplicarVisibilidadItem()
        CalcularVistaItem(True)
    End Sub

    Protected Sub Medida_TextChanged(sender As Object, e As EventArgs)
        CalcularVistaItem(True)
    End Sub

    Protected Sub Importe_TextChanged(sender As Object, e As EventArgs)
        CalcularVistaItem(False)
    End Sub

    Private Function IdProductoSeleccionado() As Integer
        Dim id As Integer
        If Integer.TryParse(ddlProducto.SelectedValue, id) Then Return id
        Return 0
    End Function

    Private Function EspesorSeleccionado() As Decimal?
        Dim valor As Decimal
        If TryParseDecimal(ddlEspesor.SelectedValue, valor) AndAlso valor > 0D Then Return valor

        ' Si el usuario escribio el espesor pero aun no se disparo el postback
        ' del combo oculto, se toma directamente del cuadro de texto.
        If TryParseDecimal(txtEspesor.Text, valor) AndAlso valor > 0D Then Return valor
        Return Nothing
    End Function

    ' ------------------------- Calculos automaticos -------------------------

    Private Sub AplicarVisibilidadItem()
        Dim tipo As String = TipoSeleccionado()
        Dim esVidrio As Boolean = tipo = "VIDRIO"
        Dim esAluminio As Boolean = tipo = "ALUMINIO"
        Dim esVentana As Boolean = Not esVidrio AndAlso Not esAluminio
        Dim sinAlero As Boolean = ddlTipoItem.SelectedValue.IndexOf("sin alero", StringComparison.OrdinalIgnoreCase) >= 0

        EstablecerFila(filaAncho, esVidrio)
        EstablecerFila(filaAncho2, esVidrio AndAlso chbAncho2.Checked)
        EstablecerFila(filaAlto, esVidrio)
        EstablecerFila(filaAlto2, esVidrio AndAlso chbAlto2.Checked)
        EstablecerFila(filaLargo, esAluminio)
        EstablecerFila(filaEspesor, esVidrio OrElse esVentana)
        EstablecerFila(filaColor, esVentana)
        EstablecerFila(filaVentanaAncho, esVentana)
        EstablecerFila(filaAnchoArriba, esVentana)
        EstablecerFila(filaAnchoCentro, esVentana AndAlso chbTresAncho.Checked)
        EstablecerFila(filaAnchoAbajo, esVentana AndAlso chbTresAncho.Checked)
        EstablecerFila(filaVentanaAlto, esVentana)
        EstablecerFila(filaAltoDer, esVentana)
        EstablecerFila(filaAltoCentro, esVentana AndAlso chbTresAlto.Checked)
        EstablecerFila(filaAltoIzq, esVentana AndAlso chbTresAlto.Checked)
        EstablecerFila(filaAlero, esVentana AndAlso Not sinAlero)

        If esVentana AndAlso sinAlero Then txtAlero.Text = ""

        hfTipo.Value = If(esVidrio, "VIDRIO", If(esAluminio, "ALUMINIO", "VENTANA-" & ddlTipoItem.SelectedValue))
    End Sub

    Private Sub EstablecerFila(fila As HtmlTableRow, visible As Boolean)
        fila.Style("display") = If(visible, "", "none")
    End Sub

    ' Recalcula materiales, extras sugeridos y total como hace frmRegistrarPedido.
    Private Sub CalcularVistaItem(recalcularExtra As Boolean)
        Try
            Dim tipo As String = TipoSeleccionado()
            Dim idProducto As Integer = IdProductoSeleccionado()
            Dim materiales As Decimal = 0D
            Dim extra As Decimal = 0D
            Dim manoObra As Decimal = LeerDecimal(txtManoObra.Text)
            Dim unidad As String = ""

            If tipo = "VIDRIO" Then
                Dim ancho As Decimal = Mayor(LeerDecimal(txtAncho.Text),
                                             If(chbAncho2.Checked, LeerDecimal(txtAncho2.Text), 0D))
                Dim alto As Decimal = Mayor(LeerDecimal(txtAlto.Text),
                                            If(chbAlto2.Checked, LeerDecimal(txtAlto2.Text), 0D))
                Dim precio As Decimal = PrecioVidrio(idProducto, EspesorSeleccionado())
                unidad = "CM2"
                If ancho > 0D AndAlso alto > 0D AndAlso precio > 0D Then
                    materiales = Math.Round(alto * ancho / 900D * precio, 2)
                End If
                If ancho > 0D AndAlso alto > 0D Then
                    extra = objPedido.TarifaSugerida(ancho, alto).silicona
                End If
            ElseIf tipo = "ALUMINIO" Then
                Dim largo As Decimal = LeerDecimal(txtLargo.Text)
                unidad = "CM_LINEAL"
                If idProducto > 0 AndAlso largo > 0D Then
                    materiales = Math.Round(objVariante.ObtenerPrecioUnitarioPorProducto(idProducto) * largo, 2)
                End If
            Else
                Dim arriba As Decimal = LeerDecimal(txtArriba.Text)
                Dim centroAncho As Decimal = If(chbTresAncho.Checked, LeerDecimal(txtCentroAncho.Text), arriba)
                Dim abajo As Decimal = If(chbTresAncho.Checked, LeerDecimal(txtAbajo.Text), arriba)
                Dim derecha As Decimal = LeerDecimal(txtDerecha.Text)
                Dim centroAlto As Decimal = If(chbTresAlto.Checked, LeerDecimal(txtCentroAlto.Text), derecha)
                Dim izquierda As Decimal = If(chbTresAlto.Checked, LeerDecimal(txtIzquierda.Text), derecha)
                Dim anchoMayor As Decimal = Mayor(arriba, centroAncho, abajo)
                Dim altoMayor As Decimal = Mayor(izquierda, centroAlto, derecha)
                Dim precio As Decimal = PrecioVidrio(idProducto, EspesorSeleccionado())

                unidad = "VENTANA"
                If anchoMayor > 0D AndAlso altoMayor > 0D AndAlso precio > 0D Then
                    materiales = Math.Round(altoMayor * anchoMayor / 900D * precio, 2)
                End If
                If anchoMayor > 0D AndAlso altoMayor > 0D Then
                    Dim tarifa = objPedido.TarifaSugerida(anchoMayor, altoMayor)
                    extra = tarifa.aluminio + tarifa.silicona
                End If

                Dim color As String = ddlColorAluminio.SelectedValue
                If color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase) Then
                    extra *= 1.02D
                ElseIf color.Equals("MADERA", StringComparison.OrdinalIgnoreCase) Then
                    extra *= 1.05D
                End If

                If ddlTipoItem.SelectedValue.IndexOf("sin alero", StringComparison.OrdinalIgnoreCase) >= 0 Then
                    txtAlero.Text = ""
                ElseIf altoMayor >= VENTANA_ALERO_ALTO_MINIMO_CM AndAlso txtAlero.Text.Trim() = "" Then
                    txtAlero.Text = VENTANA_ALERO_CM.ToString("0.00", CultureInfo.InvariantCulture)
                End If
            End If

            txtPrecioMateriales.Text = materiales.ToString("0.00", CultureInfo.InvariantCulture)
            If recalcularExtra Then txtExtra.Text = extra.ToString("0.00", CultureInfo.InvariantCulture)

            Dim extras As Decimal = LeerDecimal(txtExtra.Text)
            txtTotalItem.Text = Redondear(materiales + extras + manoObra).ToString("0", CultureInfo.InvariantCulture)
            lblUnidad.Text = unidad
        Catch
            ' La vista puede estar a medio llenar mientras se cambia de tipo;
            ' los errores reales se muestran al intentar agregar el item.
        End Try
    End Sub

    Private Function PrecioVidrio(idProducto As Integer, espesor As Decimal?) As Decimal
        If idProducto <= 0 OrElse Not espesor.HasValue OrElse espesor.Value <= 0D Then Return 0D
        Try
            Return objVariante.ObtenerPrecioUnitario(idProducto, espesor.Value)
        Catch
            Return 0D
        End Try
    End Function

    Private Shared Function Mayor(ParamArray valores As Decimal()) As Decimal
        Dim maximo As Decimal = 0D
        For Each valor As Decimal In valores
            If valor > maximo Then maximo = valor
        Next
        Return maximo
    End Function

    ' El usuario puede marcar "Tres medidas" y dejar algunas vacias o en cero.
    ' Con una sola medida se baja a una medida; con dos se usa la mayor de las
    ' tres para las tres columnas, para que el modelo no reciba ceros.
    Private Shared Sub NormalizarTresMedidas(ByRef tres As Boolean, ByRef primera As Decimal?,
                                              ByRef segunda As Decimal?, ByRef tercera As Decimal?)
        Dim v1 As Decimal = primera.GetValueOrDefault()
        Dim v2 As Decimal = segunda.GetValueOrDefault()
        Dim v3 As Decimal = tercera.GetValueOrDefault()
        Dim cantidad As Integer = 0
        If v1 > 0D Then cantidad += 1
        If v2 > 0D Then cantidad += 1
        If v3 > 0D Then cantidad += 1

        If cantidad <= 1 Then
            Dim unica As Decimal = Mayor(v1, v2, v3)
            tres = False
            If unica > 0D Then
                primera = unica
            Else
                primera = Nothing
            End If
            segunda = primera
            tercera = primera
        ElseIf cantidad = 2 Then
            Dim maxima As Decimal = Mayor(v1, v2, v3)
            primera = maxima
            segunda = maxima
            tercera = maxima
        End If
    End Sub

    Private Shared Function Redondear(valor As Decimal) As Decimal
        Return Math.Round(valor, 0, MidpointRounding.AwayFromZero)
    End Function

    ' ------------------------- Items -------------------------

    Protected Sub btnAgregarItem_Click(sender As Object, e As EventArgs)
        MostrarMensajeItem("", "")
        SincronizarCombos()

        Dim tipo As String = TipoSeleccionado()
        Dim idProducto As Integer = IdProductoSeleccionado()
        If idProducto <= 0 Then
            MostrarMensajeItem("Seleccione un producto o variante.", "mensaje-error")
            Return
        End If

        Dim materiales As Decimal = LeerDecimal(txtPrecioMateriales.Text)
        Dim extras As Decimal = LeerDecimal(txtExtra.Text)
        Dim manoObra As Decimal = LeerDecimal(txtManoObra.Text)
        If extras < 0D OrElse manoObra < 0D Then
            MostrarMensajeItem("Extras y mano de obra deben ser números desde cero.", "mensaje-error")
            Return
        End If
        Dim total As Decimal = Redondear(materiales + extras + manoObra)
        If materiales <= 0D OrElse total <= 0D Then
            MostrarMensajeItem("No hay precios calculados. Revise el producto, las medidas y el espesor.", "mensaje-error")
            Return
        End If

        Dim medida As String = ""
        Dim detalle As String = ""
        Dim espesor As Decimal? = Nothing
        Dim color As String = ""
        Dim idModelo As Integer? = Nothing
        Dim ancho As Decimal? = Nothing
        Dim alto As Decimal? = Nothing
        Dim ancho2 As Decimal? = Nothing
        Dim alto2 As Decimal? = Nothing
        Dim largo As Decimal? = Nothing
        Dim arriba As Decimal? = Nothing
        Dim centroAncho As Decimal? = Nothing
        Dim abajo As Decimal? = Nothing
        Dim derecha As Decimal? = Nothing
        Dim centroAlto As Decimal? = Nothing
        Dim izquierda As Decimal? = Nothing
        Dim alero As Decimal? = Nothing
        Dim tresAncho As Boolean = False
        Dim tresAlto As Boolean = False

        Try
            If tipo = "VIDRIO" Then
                espesor = EspesorSeleccionado()
                If Not espesor.HasValue OrElse espesor.Value <= 0D Then
                    MostrarMensajeItem("Seleccione el espesor de vidrio.", "mensaje-error")
                    Return
                End If

                ancho = LeerDecimalNullable(txtAncho.Text)
                alto = LeerDecimalNullable(txtAlto.Text)
                tresAncho = chbAncho2.Checked
                tresAlto = chbAlto2.Checked
                If tresAncho Then ancho2 = LeerDecimalNullable(txtAncho2.Text)
                If tresAlto Then alto2 = LeerDecimalNullable(txtAlto2.Text)
                Dim anchoMayor As Decimal = Mayor(ancho.GetValueOrDefault(), ancho2.GetValueOrDefault())
                Dim altoMayor As Decimal = Mayor(alto.GetValueOrDefault(), alto2.GetValueOrDefault())
                If anchoMayor <= 0D OrElse altoMayor <= 0D Then
                    MostrarMensajeItem("Ingrese las medidas de pedido (ancho y alto).", "mensaje-error")
                    Return
                End If

                Dim idVariante As Integer = objVariante.ResolverVariantePorProducto(idProducto, espesor.Value)
                Dim accion As String = "COMPRA_NUEVA"
                Dim idPieza As Integer? = Nothing
                Dim piezas As DataTable = objPieza.BuscarDisponibles(idVariante, anchoMayor, altoMayor)
                If piezas IsNot Nothing AndAlso piezas.Rows.Count > 0 Then
                    Dim filaPieza As DataRow = piezas.Rows(0)
                    Dim pAncho As Decimal = ADecimal(filaPieza("ancho_cm"))
                    Dim pAlto As Decimal = ADecimal(filaPieza("alto_cm"))
                    idPieza = Convert.ToInt32(filaPieza("id_pieza"))
                    Dim calzaDirecto As Boolean = Math.Abs(pAncho - anchoMayor) <= TOLERANCIA_CM AndAlso
                                                  Math.Abs(pAlto - altoMayor) <= TOLERANCIA_CM
                    Dim calzaRotado As Boolean = Math.Abs(pAncho - altoMayor) <= TOLERANCIA_CM AndAlso
                                                 Math.Abs(pAlto - anchoMayor) <= TOLERANCIA_CM
                    accion = If(calzaDirecto OrElse calzaRotado, "USA_EXISTENTE", "CORTA_PIEZA")
                End If

                detalle = SerializarDetalle(New DetallePedidoDTO With {
                    .IdVariante = idVariante,
                    .Accion = accion,
                    .IdPieza = idPieza,
                    .AnchoCm = anchoMayor,
                    .AltoCm = altoMayor,
                    .Subtotal = materiales
                })

                medida = TextoMedida(anchoMayor, altoMayor, Nothing)
            ElseIf tipo = "ALUMINIO" Then
                largo = LeerDecimalNullable(txtLargo.Text)
                If Not largo.HasValue OrElse largo.Value <= 0D Then
                    MostrarMensajeItem("Ingrese el largo de aluminio.", "mensaje-error")
                    Return
                End If

                Dim idVariante As Integer = objVariante.ObtenerVariantePorProducto(idProducto)
                If idVariante <= 0 Then
                    MostrarMensajeItem("El aluminio no tiene una variante con precio.", "mensaje-error")
                    Return
                End If

                detalle = SerializarDetalle(New DetallePedidoDTO With {
                    .IdVariante = idVariante,
                    .Accion = "COMPRA_NUEVA",
                    .IdPieza = Nothing,
                    .AnchoCm = 0D,
                    .AltoCm = 0D,
                    .LargoCm = largo,
                    .Subtotal = materiales
                })

                medida = TextoMedida(Nothing, Nothing, largo)
            Else
                Dim modelo As ModeloVentana = Nothing
                Dim idModeloSeleccionado As Integer = IdModeloDeSeleccion(ddlTipoItem.SelectedValue)
                If idModeloSeleccionado > 0 Then modelo = ModeloVentana.CargarPorId(idModeloSeleccionado)
                If modelo Is Nothing Then
                    MostrarMensajeItem("La ventana no tiene un modelo asignado.", "mensaje-error")
                    Return
                End If

                espesor = EspesorSeleccionado()
                If Not espesor.HasValue OrElse espesor.Value <= 0D Then
                    MostrarMensajeItem("Seleccione el espesor de vidrio.", "mensaje-error")
                    Return
                End If
                color = ddlColorAluminio.SelectedValue
                If color = "" Then
                    MostrarMensajeItem("Seleccione el color de aluminio.", "mensaje-error")
                    Return
                End If

                arriba = LeerDecimalNullable(txtArriba.Text)
                tresAncho = chbTresAncho.Checked
                If tresAncho Then
                    centroAncho = LeerDecimalNullable(txtCentroAncho.Text)
                    abajo = LeerDecimalNullable(txtAbajo.Text)
                Else
                    centroAncho = arriba
                    abajo = arriba
                End If
                derecha = LeerDecimalNullable(txtDerecha.Text)
                tresAlto = chbTresAlto.Checked
                If tresAlto Then
                    centroAlto = LeerDecimalNullable(txtCentroAlto.Text)
                    izquierda = LeerDecimalNullable(txtIzquierda.Text)
                Else
                    centroAlto = derecha
                    izquierda = derecha
                End If

                ' Un usuario que marco "tres medidas" pero lleno una o dos no debe
                ' enviar ceros al modelo: se normaliza antes de calcular.
                NormalizarTresMedidas(tresAncho, arriba, centroAncho, abajo)
                NormalizarTresMedidas(tresAlto, izquierda, centroAlto, derecha)

                If ddlTipoItem.SelectedValue.IndexOf("sin alero", StringComparison.OrdinalIgnoreCase) < 0 Then
                    alero = LeerDecimalNullable(txtAlero.Text)
                End If

                Dim anchoMayor As Decimal = Mayor(arriba.GetValueOrDefault(), centroAncho.GetValueOrDefault(), abajo.GetValueOrDefault())
                Dim altoMayor As Decimal = Mayor(izquierda.GetValueOrDefault(), centroAlto.GetValueOrDefault(), derecha.GetValueOrDefault())
                Dim minAlto As Decimal = Menor(izquierda.GetValueOrDefault(), centroAlto.GetValueOrDefault(), derecha.GetValueOrDefault())
                If anchoMayor < VENTANA_ANCHO_MINIMO_CM Then
                    MostrarMensajeItem("El ancho debe ser de al menos " & VENTANA_ANCHO_MINIMO_CM.ToString("0") & " cm.", "mensaje-error")
                    Return
                End If
                If altoMayor <= 0D Then
                    MostrarMensajeItem("Ingrese las medidas de alto.", "mensaje-error")
                    Return
                End If

                Dim entradas As New Dictionary(Of String, Decimal) From {
                    {"ancho_abajo", abajo.GetValueOrDefault()},
                    {"ancho_arriba", arriba.GetValueOrDefault()},
                    {"ancho_centro", centroAncho.GetValueOrDefault()},
                    {"alto_izq", izquierda.GetValueOrDefault()},
                    {"alto_centro", centroAlto.GetValueOrDefault()},
                    {"alto_der", derecha.GetValueOrDefault()},
                    {"tres_ancho", If(tresAncho, 1D, 0D)},
                    {"tres_alto", If(tresAlto, 1D, 0D)},
                    {"max_ancho", anchoMayor},
                    {"max_alto", altoMayor},
                    {"min_alto", minAlto},
                    {"alero", alero.GetValueOrDefault()}}

                Dim resultado As ModeloVentana.ResultadoModelo
                Try
                    resultado = modelo.Calcular(entradas)
                Catch ex As Exception
                    MostrarMensajeItem("No se pudo evaluar el modelo '" & modelo.Nombre & "': " & ex.Message, "mensaje-error")
                    Return
                End Try
                If resultado.Mensajes.Count > 0 Then
                    MostrarMensajeItem(String.Join(" ", resultado.Mensajes), "mensaje-error")
                    Return
                End If

                Dim idVarianteVidrio As Integer = objVariante.ResolverVariantePorProducto(idProducto, espesor.Value)
                Dim precioVidrio As Decimal = objVariante.ObtenerPrecioUnitario(idProducto, espesor.Value)
                Dim Builder As New StringBuilder()
                For Each linea As ModeloVentana.LineaVentana In resultado.Lineas
                    Dim colorLinea As String = ColorDeRol(linea.RolColor, color)
                    Dim tipoMaterial As String = If(linea.Tipo, "")
                    Dim detalleLinea As DetallePedidoDTO = Nothing
                    If linea.TipoMedida = "CANTIDAD" Then
                        detalleLinea = DetalleMaterial(linea.Categoria, tipoMaterial, colorLinea,
                                                       Nothing, CInt(linea.Cantidad.GetValueOrDefault()))
                    ElseIf linea.TipoMedida = "VIDRIO" Then
                        detalleLinea = DetalleVidrio(idVarianteVidrio, precioVidrio,
                                                     linea.AnchoCm.GetValueOrDefault(),
                                                     linea.AltoCm.GetValueOrDefault())
                    Else
                        detalleLinea = DetalleMaterial(linea.Categoria, tipoMaterial, colorLinea,
                                                       linea.LargoCm.GetValueOrDefault(), Nothing)
                    End If
                    Builder.Append(SerializarDetalle(detalleLinea))
                Next
                detalle = Builder.ToString()

                medida = anchoMayor.ToString("0.##", CultureInfo.InvariantCulture) & " x " &
                         altoMayor.ToString("0.##", CultureInfo.InvariantCulture) & " cm"
                idModelo = idModeloSeleccionado
            End If
        Catch ex As Exception
            MostrarMensajeItem(ex.Message, "mensaje-error")
            Return
        End Try

        Dim dt As DataTable = Items
        Dim etiqueta As String = txtEtiqueta.Text.Trim()
        If etiqueta.Length = 0 Then
            etiqueta = ddlTipoItem.SelectedItem.Text & (dt.Rows.Count + 1).ToString()
        End If

        Dim fila As DataRow
        If IndiceEditando >= 0 AndAlso IndiceEditando < dt.Rows.Count Then
            fila = dt.Rows(IndiceEditando)
        Else
            fila = dt.NewRow()
        End If

        ' La BD solo acepta VIDRIO_SUELTO, VENTANA, ALUMINIO, MAMPARA o CAJA.
        ' El combo usa VIDRIO como etiqueta interna, pero al guardar se mapea
        ' al valor que exige el constraint ck_item_tipo.
        Dim tipoBD As String
        Select Case tipo
            Case "VIDRIO"
                tipoBD = "VIDRIO_SUELTO"
            Case Else
                tipoBD = tipo
        End Select

        fila("opcion") = ddlTipoItem.SelectedValue
        fila("tipo") = tipoBD
        fila("etiqueta") = etiqueta
        fila("producto") = If(ddlProducto.SelectedIndex >= 0, ddlProducto.SelectedItem.Text, "")
        fila("id_producto") = idProducto
        fila("espesor") = ADBNull(espesor)
        fila("color") = color
        fila("id_modelo") = If(idModelo.HasValue, CType(idModelo.Value, Object), DBNull.Value)
        fila("medida") = medida
        fila("ancho") = ADBNull(ancho)
        fila("alto") = ADBNull(alto)
        fila("ancho2") = ADBNull(ancho2)
        fila("alto2") = ADBNull(alto2)
        fila("largo") = ADBNull(largo)
        fila("arriba") = ADBNull(arriba)
        fila("centro_ancho") = ADBNull(centroAncho)
        fila("abajo") = ADBNull(abajo)
        fila("derecha") = ADBNull(derecha)
        fila("centro_alto") = ADBNull(centroAlto)
        fila("izquierda") = ADBNull(izquierda)
        fila("tres_ancho") = tresAncho
        fila("tres_alto") = tresAlto
        fila("alero") = ADBNull(alero)
        fila("precio_materiales") = materiales
        fila("costo_extra") = extras
        fila("mano_obra") = manoObra
        fila("total") = total
        fila("detalle") = detalle

        If IndiceEditando < 0 OrElse IndiceEditando >= dt.Rows.Count Then dt.Rows.Add(fila)

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
        ElseIf e.CommandName = "DuplicarItem" Then
            DuplicarItem(indice)
        ElseIf e.CommandName = "QuitarItem" Then
            QuitarItem(indice)
        End If
    End Sub

    Private Sub CargarItemParaEdicion(indice As Integer)
        Dim dt As DataTable = Items
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return
        Dim fila As DataRow = dt.Rows(indice)

        Dim opcion As String = ValorTexto(fila("opcion"))
        For Each item As ListItem In ddlTipoItem.Items
            If item.Value = opcion Then
                ddlTipoItem.SelectedValue = opcion
                Exit For
            End If
        Next

        ddlTipoItem_SelectedIndexChanged(Nothing, EventArgs.Empty)

        Dim idProducto As Integer = Convert.ToInt32(fila("id_producto"))
        For Each item As ListItem In ddlProducto.Items
            If item.Value = idProducto.ToString() Then
                ddlProducto.SelectedValue = item.Value
                Exit For
            End If
        Next
        txtProducto.Text = If(ddlProducto.SelectedIndex >= 0, ddlProducto.SelectedItem.Text, "")
        CargarEspesores(idProducto)

        Dim espesorItem As Decimal? = ADecimalNulo(fila("espesor"))
        If espesorItem.HasValue Then
            For Each item As ListItem In ddlEspesor.Items
                Dim valorItem As Decimal
                If TryParseDecimal(item.Value, valorItem) AndAlso valorItem = espesorItem.Value Then
                    ddlEspesor.SelectedValue = item.Value
                    Exit For
                End If
            Next
        End If
        txtEspesor.Text = If(ddlEspesor.SelectedIndex >= 0, ddlEspesor.SelectedItem.Text, "")

        Dim colorItem As String = ValorTexto(fila("color"))
        For Each item As ListItem In ddlColorAluminio.Items
            If item.Value = colorItem Then
                ddlColorAluminio.SelectedValue = item.Value
                Exit For
            End If
        Next
        txtColorAluminio.Text = If(ddlColorAluminio.SelectedIndex >= 0, ddlColorAluminio.SelectedItem.Text, "")

        chbAncho2.Checked = Convert.ToBoolean(fila("tres_ancho")) AndAlso TipoSeleccionado() <> "VENTANA"
        chbAlto2.Checked = Convert.ToBoolean(fila("tres_alto")) AndAlso TipoSeleccionado() <> "VENTANA"
        chbTresAncho.Checked = Convert.ToBoolean(fila("tres_ancho")) AndAlso TipoSeleccionado() = "VENTANA"
        chbTresAlto.Checked = Convert.ToBoolean(fila("tres_alto")) AndAlso TipoSeleccionado() = "VENTANA"

        txtEtiqueta.Text = ValorTexto(fila("etiqueta"))
        txtAncho.Text = ValorTexto(fila("ancho"))
        txtAncho2.Text = ValorTexto(fila("ancho2"))
        txtAlto.Text = ValorTexto(fila("alto"))
        txtAlto2.Text = ValorTexto(fila("alto2"))
        txtLargo.Text = ValorTexto(fila("largo"))
        txtArriba.Text = ValorTexto(fila("arriba"))
        txtCentroAncho.Text = ValorTexto(fila("centro_ancho"))
        txtAbajo.Text = ValorTexto(fila("abajo"))
        txtDerecha.Text = ValorTexto(fila("derecha"))
        txtCentroAlto.Text = ValorTexto(fila("centro_alto"))
        txtIzquierda.Text = ValorTexto(fila("izquierda"))
        txtAlero.Text = ValorTexto(fila("alero"))
        txtPrecioMateriales.Text = Convert.ToDecimal(fila("precio_materiales")).ToString("0.00", CultureInfo.InvariantCulture)
        txtExtra.Text = Convert.ToDecimal(fila("costo_extra")).ToString("0.00", CultureInfo.InvariantCulture)
        txtManoObra.Text = Convert.ToDecimal(fila("mano_obra")).ToString("0.00", CultureInfo.InvariantCulture)

        IndiceEditando = indice
        btnAgregarItem.Text = "Actualizar ítem"
        btnCancelarItemEdicion.Visible = True
        AplicarVisibilidadItem()
        CalcularVistaItem(False)
        MostrarMensajeItem("Editando el ítem seleccionado.", "mensaje-info")
    End Sub

    Private Sub DuplicarItem(indice As Integer)
        Dim dt As DataTable = Items
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return
        Dim copia As DataRow = dt.NewRow()
        For Each columna As DataColumn In dt.Columns
            copia(columna.ColumnName) = dt.Rows(indice)(columna.ColumnName)
        Next
        copia("etiqueta") = ValorTexto(dt.Rows(indice)("etiqueta")) & " (copia)"
        dt.Rows.Add(copia)
        Items = dt
        EnlazarItems()
        MostrarMensajeItem("Ítem duplicado.", "mensaje-ok")
    End Sub

    Private Sub QuitarItem(indice As Integer)
        Dim dt As DataTable = Items
        If indice < 0 OrElse indice >= dt.Rows.Count Then Return

        If indice = IndiceEditando Then LimpiarEntradaItem()
        dt.Rows.RemoveAt(indice)
        Items = dt
        EnlazarItems()
        MostrarMensajeItem("Ítem quitado del pedido.", "mensaje-ok")
    End Sub

    Private Sub LimpiarEntradaItem()
        IndiceEditando = -1
        txtEtiqueta.Text = ""
        txtAncho.Text = ""
        txtAncho2.Text = ""
        txtAlto.Text = ""
        txtAlto2.Text = ""
        txtLargo.Text = ""
        txtArriba.Text = ""
        txtCentroAncho.Text = ""
        txtAbajo.Text = ""
        txtDerecha.Text = ""
        txtCentroAlto.Text = ""
        txtIzquierda.Text = ""
        txtAlero.Text = ""
        chbAncho2.Checked = False
        chbAlto2.Checked = False
        chbTresAncho.Checked = False
        chbTresAlto.Checked = False
        txtExtra.Text = "0.00"
        txtManoObra.Text = "0.00"
        btnAgregarItem.Text = "Agregar ítem"
        btnCancelarItemEdicion.Visible = False
        AplicarVisibilidadItem()
        CalcularVistaItem(True)
    End Sub

    ' ------------------------- Guardar -------------------------

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
            Dim lista As New List(Of ItemPedidoDTO)()
            For Each fila As DataRow In dt.Rows
                Dim idModelo As Integer? = AEnteroNulo(fila("id_modelo"))
                Dim item As New ItemPedidoDTO With {
                    .Tipo = ValorTexto(fila("tipo")),
                    .Etiqueta = ValorTexto(fila("etiqueta")),
                    .PrecioMateriales = Convert.ToDecimal(fila("precio_materiales")),
                    .CostoExtra = Convert.ToDecimal(fila("costo_extra")),
                    .ManoObra = Convert.ToDecimal(fila("mano_obra")),
                    .Total = Convert.ToDecimal(fila("total")),
                    .IdModelo = idModelo,
                    .Detalles = DeserializarDetalle(ValorTexto(fila("detalle")))
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

        If ddlCliente.Items.Count > 0 Then ddlCliente.SelectedIndex = 0
        txtCliente.Text = ""

        EnlazarTiposDePedido()
        ddlTipoItem.SelectedValue = "VIDRIO"

        CargarProductos("VIDRIO", "")
        CargarEspesores(0)
        If ddlColorAluminio.Items.Count > 0 Then ddlColorAluminio.SelectedIndex = 0
        txtColorAluminio.Text = ""

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
        lblTotalPedido.Text = "TOTAL: S/ " & Redondear(total).ToString("0", CultureInfo.InvariantCulture)
    End Sub

    ' ------------------------- Detalles -------------------------

    Private Function DetalleVidrio(idVariante As Integer, precioUnitario As Decimal,
                                   ancho As Decimal, alto As Decimal) As DetallePedidoDTO
        Dim a As Decimal = Math.Round(ancho, 2)
        Dim h As Decimal = Math.Round(alto, 2)
        Return New DetallePedidoDTO With {
            .IdVariante = idVariante,
            .Accion = "COMPRA_NUEVA",
            .IdPieza = Nothing,
            .AnchoCm = a,
            .AltoCm = h,
            .Subtotal = Math.Round(precioUnitario * (a * h) / 900D, 2)
        }
    End Function

    ' Resuelve la variante por categoria + tipo + color del catalogo.
    Private Function DetalleMaterial(categoria As String, tipo As String, color As String,
                                     largo As Decimal?, cantidad As Integer?) As DetallePedidoDTO
        Dim idProducto As Integer = objProducto.ObtenerIdPorTipoColor(categoria, tipo, color)
        If idProducto <= 0 Then
            Throw New Exception("No existe el material " & categoria & " '" & tipo & "' (" & color & ") en el catálogo.")
        End If
        Dim idVariante As Integer = objVariante.ObtenerVariantePorProducto(idProducto)
        If idVariante <= 0 Then
            Throw New Exception("El material " & categoria & " '" & tipo & "' no tiene una variante con precio.")
        End If
        Dim precioUnitario As Decimal = objVariante.ObtenerPrecioUnitarioPorProducto(idProducto)

        Dim detalle As New DetallePedidoDTO With {
            .IdVariante = idVariante,
            .Accion = "COMPRA_NUEVA",
            .IdPieza = Nothing
        }
        If cantidad.HasValue Then
            detalle.CantidadCajas = cantidad.Value
            detalle.Subtotal = Math.Round(precioUnitario * cantidad.Value, 2)
        ElseIf largo.HasValue Then
            Dim l As Decimal = Math.Round(largo.Value, 2)
            detalle.LargoCm = l
            detalle.Subtotal = Math.Round(precioUnitario * l, 2)
        Else
            detalle.Subtotal = 0D
        End If
        Return detalle
    End Function

    Private Shared Function ColorDeRol(rol As String, colorVentana As String) As String
        Select Case If(rol, "").Trim().ToUpper()
            Case "VENTANA"
                Return colorVentana
            Case "SILICONA"
                Return ColorSilicona(colorVentana)
            Case "NINGUNO"
                Return ""
            Case Else
                Return ColorMaterial(colorVentana)
        End Select
    End Function

    Private Shared Function ColorMaterial(colorVentana As String) As String
        If colorVentana.Equals("MADERA", StringComparison.OrdinalIgnoreCase) Then Return "NEGRO"
        Return colorVentana
    End Function

    Private Shared Function ColorSilicona(colorVentana As String) As String
        If colorVentana.Equals("MADERA", StringComparison.OrdinalIgnoreCase) OrElse
           colorVentana.Equals("NEGRO", StringComparison.OrdinalIgnoreCase) Then Return "NEGRA"
        If colorVentana.Equals("BLANCO", StringComparison.OrdinalIgnoreCase) Then Return "BLANCA"
        Return "TRANSPARENTE"
    End Function

    ' Los DTO no son serializables: se guardan como texto en la grilla y se
    ' reconstruyen al guardar el pedido.
    Private Function SerializarDetalle(detalle As DetallePedidoDTO) As String
        Dim campos As String() = {
            detalle.IdVariante.ToString(CultureInfo.InvariantCulture),
            If(detalle.Accion, ""),
            If(detalle.IdPieza.HasValue, detalle.IdPieza.Value.ToString(CultureInfo.InvariantCulture), ""),
            Numero(detalle.AnchoCm), Numero(detalle.AltoCm), Numero(detalle.LargoCm),
            If(detalle.CantidadCajas.HasValue, detalle.CantidadCajas.Value.ToString(CultureInfo.InvariantCulture), ""),
            detalle.Subtotal.ToString("0.####", CultureInfo.InvariantCulture)
        }
        Return String.Join("|", campos) & ";"
    End Function

    Private Function DeserializarDetalle(texto As String) As List(Of DetallePedidoDTO)
        Dim lista As New List(Of DetallePedidoDTO)()
        For Each bloque As String In If(texto, "").Split(";"c)
            If bloque.Trim().Length = 0 Then Continue For
            Dim campos As String() = bloque.Split("|"c)
            If campos.Length < 8 Then Continue For

            Dim detalle As New DetallePedidoDTO With {
                .IdVariante = Convert.ToInt32(campos(0)),
                .Accion = campos(1),
                .IdPieza = EnteroNulo(campos(2)),
                .AnchoCm = DecimalNulo(campos(3)),
                .AltoCm = DecimalNulo(campos(4)),
                .LargoCm = DecimalNulo(campos(5)),
                .CantidadCajas = EnteroNulo(campos(6)),
                .Subtotal = Convert.ToDecimal(campos(7), CultureInfo.InvariantCulture)
            }
            lista.Add(detalle)
        Next
        Return lista
    End Function

    Private Shared Function Numero(valor As Decimal?) As String
        If Not valor.HasValue Then Return ""
        Return valor.Value.ToString("0.####", CultureInfo.InvariantCulture)
    End Function

    Private Shared Function EnteroNulo(texto As String) As Integer?
        Dim valor As Integer
        If Integer.TryParse(texto, valor) Then Return valor
        Return Nothing
    End Function

    Private Shared Function DecimalNulo(texto As String) As Decimal?
        Dim valor As Decimal
        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, valor) Then Return valor
        Return Nothing
    End Function

    ' ------------------------- Cotizacion (opcional) -------------------------

    ' Equivale a AgregarDesdeCotizacion: ?cot=1&tipo=Vidrio&ancho=...&alto=...
    Private Sub AplicarDesdeConsulta()
        If Request.QueryString("cot") <> "1" Then Return
        Try
            Dim seleccion As String = If(Request.QueryString("tipo"), "VIDRIO")
            If seleccion.Equals("Ventana", StringComparison.OrdinalIgnoreCase) AndAlso Modelos.Rows.Count > 0 Then
                seleccion = Modelos.Rows(0)("nombre").ToString()
            End If
            Dim existe As Boolean = False
            For Each item As ListItem In ddlTipoItem.Items
                If item.Value.Equals(seleccion, StringComparison.OrdinalIgnoreCase) Then
                    seleccion = item.Value
                    existe = True
                    Exit For
                End If
            Next
            If Not existe Then seleccion = "VIDRIO"
            ddlTipoItem.SelectedValue = seleccion
            ddlTipoItem_SelectedIndexChanged(Nothing, EventArgs.Empty)

            Dim tipo As String = TipoSeleccionado()
            Dim categoria As String = CategoriaDeTipo(tipo)
            Dim tipoProducto As String = If(Request.QueryString("tipoProducto"), "")
            Dim colorProducto As String = If(Request.QueryString("colorProducto"), "")
            If tipoProducto <> "" Then
                Dim idProducto As Integer = objProducto.ObtenerIdPorTipoColor(categoria, tipoProducto, colorProducto)
                If idProducto > 0 Then
                    For Each item As ListItem In ddlProducto.Items
                        If item.Value = idProducto.ToString() Then
                            ddlProducto.SelectedValue = item.Value
                            txtProducto.Text = item.Text
                            Exit For
                        End If
                    Next
                    CargarEspesores(idProducto)
                End If
            End If

            AsignarSiViene("espesor", txtEspesor)
            SincronizarComboNumerico(txtEspesor, ddlEspesor)
            AsignarSiViene("ancho", txtAncho)
            AsignarSiViene("alto", txtAlto)
            AsignarSiViene("largo", txtLargo)
            AsignarSiViene("arriba", txtArriba)
            AsignarSiViene("abajo", txtAbajo)
            AsignarSiViene("derecha", txtDerecha)
            AsignarSiViene("extra", txtExtra)
            AsignarSiViene("manoObra", txtManoObra)

            If tipo = "VENTANA" Then
                chbTresAncho.Checked = True
                chbTresAlto.Checked = True
                AsignarSiViene("centroAncho", txtCentroAncho)
                AsignarSiViene("centroAlto", txtCentroAlto)
                AsignarSiViene("izquierda", txtIzquierda)
                Dim color As String = If(Request.QueryString("colorAluminio"), "")
                For Each item As ListItem In ddlColorAluminio.Items
                    If item.Value.Equals(color, StringComparison.OrdinalIgnoreCase) Then
                        ddlColorAluminio.SelectedValue = item.Value
                        txtColorAluminio.Text = item.Text
                        Exit For
                    End If
                Next
            End If

            AplicarVisibilidadItem()
            CalcularVistaItem(True)
            btnAgregarItem_Click(Nothing, EventArgs.Empty)
        Catch ex As Exception
            MostrarMensajeItem("No se pudo precargar la cotización: " & ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub AsignarSiViene(clave As String, caja As TextBox)
        Dim valor As String = Request.QueryString(clave)
        If Not String.IsNullOrWhiteSpace(valor) Then caja.Text = valor
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Function CrearTablaItems() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("opcion", GetType(String))
        dt.Columns.Add("tipo", GetType(String))
        dt.Columns.Add("etiqueta", GetType(String))
        dt.Columns.Add("producto", GetType(String))
        dt.Columns.Add("id_producto", GetType(Integer))
        dt.Columns.Add("espesor", GetType(Object))
        dt.Columns.Add("color", GetType(String))
        dt.Columns.Add("id_modelo", GetType(Object))
        dt.Columns.Add("medida", GetType(String))
        dt.Columns.Add("ancho", GetType(Object))
        dt.Columns.Add("alto", GetType(Object))
        dt.Columns.Add("ancho2", GetType(Object))
        dt.Columns.Add("alto2", GetType(Object))
        dt.Columns.Add("largo", GetType(Object))
        dt.Columns.Add("arriba", GetType(Object))
        dt.Columns.Add("centro_ancho", GetType(Object))
        dt.Columns.Add("abajo", GetType(Object))
        dt.Columns.Add("derecha", GetType(Object))
        dt.Columns.Add("centro_alto", GetType(Object))
        dt.Columns.Add("izquierda", GetType(Object))
        dt.Columns.Add("tres_ancho", GetType(Boolean))
        dt.Columns.Add("tres_alto", GetType(Boolean))
        dt.Columns.Add("alero", GetType(Object))
        dt.Columns.Add("precio_materiales", GetType(Decimal))
        dt.Columns.Add("costo_extra", GetType(Decimal))
        dt.Columns.Add("mano_obra", GetType(Decimal))
        dt.Columns.Add("total", GetType(Decimal))
        dt.Columns.Add("detalle", GetType(String))
        Return dt
    End Function

    Private Shared Function ADBNull(valor As Decimal?) As Object
        Return If(valor.HasValue, CType(valor.Value, Object), DBNull.Value)
    End Function

    Private Shared Function ADecimalNulo(valor As Object) As Decimal?
        If valor Is Nothing OrElse IsDBNull(valor) Then Return Nothing
        Return Convert.ToDecimal(valor)
    End Function

    Private Shared Function AEnteroNulo(valor As Object) As Integer?
        If valor Is Nothing OrElse IsDBNull(valor) Then Return Nothing
        Return Convert.ToInt32(valor)
    End Function

    Private Shared Function ADecimal(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return Convert.ToDecimal(valor)
    End Function

    Private Shared Function Menor(ParamArray valores As Decimal()) As Decimal
        If valores.Length = 0 Then Return 0D
        Dim minimo As Decimal = valores(0)
        For Each valor As Decimal In valores
            If valor < minimo Then minimo = valor
        Next
        Return minimo
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

    Private Function LeerDecimal(texto As String) As Decimal
        Dim valor As Decimal
        If TryParseDecimal(texto, valor) Then Return valor
        Return 0D
    End Function

    Private Function LeerDecimalNullable(texto As String) As Decimal?
        Dim valor As Decimal
        If TryParseDecimal(texto, valor) Then Return valor
        Return Nothing
    End Function

End Class

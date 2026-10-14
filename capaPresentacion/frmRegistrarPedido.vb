Imports capaNegocio
Public Class frmRegistrarPedido
    Implements IFormularioTema
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""
    Public Property idUsuario As Integer = 0
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    ' Datos que llegan desde frmCotizarVentana para precrear una pestaña
    Public Property tieneCotizacion As Boolean = False
    Public Property cotSeleccion As String = ""
    Public Property cotAncho As Decimal = 0D
    Public Property cotAlto As Decimal = 0D
    Public Property cotEspesor As Decimal = 0D
    Public Property cotManoObra As Decimal = 0D
    Public Property cotExtra As Decimal = 0D
    Public Property cotTipoVidrio As String = ""
    Public Property cotColorVidrio As String = ""
    Public Property cotTipoAluminio As String = ""
    Public Property cotColorAluminio As String = ""
    Private contadores As New Dictionary(Of String, Integer)
    Private itemsPedido As New List(Of ItemPedidoDTO)

    Private Sub actualizarCliente()
        txtNombreCliente.Text = nombreClienteSeleccionado
    End Sub

    ' Si no le pasaron el usuario, lo toma del formulario padre (MenuInicio.txtID)
    Private Sub ResolverUsuario()
        If idUsuario > 0 Then Return
        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If padre IsNot Nothing Then
            Integer.TryParse(padre.txtID.Text, idUsuario)
        End If
    End Sub

    Private Sub frmRegistrarPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        ResolverUsuario()
        tabsPedidos.TabPages.Remove(tabModeloVidrio)
        tabsPedidos.TabPages.Remove(tabModeloVentana)
        tabsPedidos.TabPages.Remove(tabModeloAluminio)
        CargarCombos()
        AplicarVisibilidadMedidas(chbAncho)
        AplicarVisibilidadMedidas(chbAlto)
        AplicarVisibilidadMedidasVentana(CheckBox3)
        AplicarVisibilidadMedidasVentana(CheckBox4)
        If tieneCotizacion Then AgregarDesdeCotizacion()
    End Sub

    Private Sub CargarCombos()
        ' Ningun combo debe permitir escribir: solo seleccionar
        cmbPedido.DropDownStyle = ComboBoxStyle.DropDownList
        txtEspesor.DropDownStyle = ComboBoxStyle.DropDownList
        cmbEspesorVentana.DropDownStyle = ComboBoxStyle.DropDownList
        cmbColorAlumnioVentana.DropDownStyle = ComboBoxStyle.DropDownList

        txtEspesor.Items.Clear()
        txtEspesor.Enabled = False

        cmbEspesorVentana.Items.Clear()
        cmbEspesorVentana.Enabled = False

        LlenarCombo(cmbPedido, New List(Of String) From {"ALUMINIO", "VIDRIO", "Ventana"})
    End Sub

    Private Sub LlenarCombo(cmb As ComboBox, valores As List(Of String))
        cmb.Items.Clear()
        For Each valor As String In valores
            cmb.Items.Add(valor)
        Next
    End Sub

    Private Sub btnBuscarCliente_Click(sender As Object, e As EventArgs) Handles btnBuscarCliente.Click
        Using formulario As New frmListadoClientes With {.tema = tema, .modoSeleccion = True}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                idClienteSeleccionado = formulario.idClienteSeleccionado
                nombreClienteSeleccionado = formulario.nombreClienteSeleccionado
                actualizarCliente()
            End If
        End Using
    End Sub

    Private Sub btnAgregarPedido_Click(sender As Object, e As EventArgs) Handles btnAgregarPedido.Click
        If cmbPedido.SelectedIndex = -1 Then
            UtilFormularios.MostrarError("Seleccione un tipo de pedido.")
            Return
        End If

        CrearTab(cmbPedido.SelectedItem.ToString())
    End Sub

    ' Crea una pestaña nueva a partir del modelo (Ventana = objeto especial, Aluminio = solo largo, el resto = material)
    Private Sub CrearTab(seleccion As String)
        Dim esVentana As Boolean = seleccion.Equals("Ventana", StringComparison.OrdinalIgnoreCase)
        Dim esAluminio As Boolean = seleccion.Equals("ALUMINIO", StringComparison.OrdinalIgnoreCase)
        Dim modelo As TabPage
        If esVentana Then
            modelo = tabModeloVentana
        ElseIf esAluminio Then
            modelo = tabModeloAluminio
        Else
            modelo = tabModeloVidrio
        End If

        Dim numero As Integer = SiguienteNumero(seleccion)
        Dim nueva As TabPage = ClonarTabPage(modelo)
        nueva.Tag = seleccion
        nueva.Name = seleccion & numero.ToString()
        nueva.Text = seleccion & numero.ToString()
        tabsPedidos.TabPages.Add(nueva)
        tabsPedidos.SelectedTab = nueva
        RecalcularTab(nueva)
    End Sub

    Private Function SiguienteNumero(clave As String) As Integer
        If Not contadores.ContainsKey(clave) Then contadores(clave) = 0
        contadores(clave) += 1
        Return contadores(clave)
    End Function

    Private Function ClonarTabPage(origen As TabPage) As TabPage
        Dim copia As New TabPage()
        copia.Padding = origen.Padding
        copia.BackColor = origen.BackColor
        copia.UseVisualStyleBackColor = origen.UseVisualStyleBackColor

        For Each control As Control In origen.Controls
            copia.Controls.Add(ClonarControl(control))
        Next

        Return copia
    End Function

    Private Function ClonarControl(origen As Control) As Control
        Dim copia As Control = DirectCast(Activator.CreateInstance(origen.GetType()), Control)
        copia.Name = origen.Name
        copia.Text = origen.Text
        copia.Location = origen.Location
        copia.Size = origen.Size
        copia.Enabled = origen.Enabled
        copia.Visible = origen.Visible
        copia.Tag = origen.Tag
        copia.Dock = origen.Dock
        copia.Anchor = origen.Anchor
        copia.BackColor = origen.BackColor
        copia.ForeColor = origen.ForeColor
        copia.Font = origen.Font

        Dim comboOrigen As ComboBox = TryCast(origen, ComboBox)
        If comboOrigen IsNot Nothing Then
            Dim combo As ComboBox = DirectCast(copia, ComboBox)
            combo.DropDownStyle = comboOrigen.DropDownStyle
            For Each item As Object In comboOrigen.Items
                combo.Items.Add(item)
            Next
            combo.SelectedIndex = comboOrigen.SelectedIndex
            If EsCampoCalculoCombo(combo.Name) Then
                AddHandler combo.SelectedIndexChanged, AddressOf CampoCalculo_Changed
            End If
        End If

        Dim txtCopia As TextBox = TryCast(copia, TextBox)
        If txtCopia IsNot Nothing Then
            If EsCampoCalculo(txtCopia.Name) Then
                AddHandler txtCopia.TextChanged, AddressOf CampoCalculo_Changed
            ElseIf EsCampoTotal(txtCopia.Name) Then
                AddHandler txtCopia.TextChanged, AddressOf CampoTotal_Changed
            End If
            If EsCampoNumerico(txtCopia.Name) Then
                AddHandler txtCopia.Leave, AddressOf CampoNumerico_Leave
            End If
        End If

        Dim numOrigen As NumericUpDown = TryCast(origen, NumericUpDown)
        If numOrigen IsNot Nothing Then
            Dim num As NumericUpDown = DirectCast(copia, NumericUpDown)
            num.Minimum = numOrigen.Minimum
            num.Maximum = numOrigen.Maximum
            num.DecimalPlaces = numOrigen.DecimalPlaces
            num.Value = numOrigen.Value
        End If

        Dim chbOrigen As CheckBox = TryCast(origen, CheckBox)
        Dim chbCopia As CheckBox = TryCast(copia, CheckBox)
        If chbOrigen IsNot Nothing AndAlso chbCopia IsNot Nothing Then
            chbCopia.Checked = chbOrigen.Checked
            If chbCopia.Name = "chbAncho" OrElse chbCopia.Name = "chbAlto" OrElse
               chbCopia.Name = "CheckBox3" OrElse chbCopia.Name = "CheckBox4" Then
                AddHandler chbCopia.CheckedChanged, AddressOf chbMedida_CheckedChanged
            End If
        End If

        Dim btnCopia As Button = TryCast(copia, Button)
        If btnCopia IsNot Nothing Then
            If btnCopia.Name = "btnSelectVidrio" OrElse btnCopia.Name = "btnBuscarVidrioVentana" OrElse btnCopia.Name = "btnBuscarAluminio" Then
                AddHandler btnCopia.Click, AddressOf btnSelectVidrio_Click
            Else
                Select Case btnCopia.Text
                    Case "Duplicar"
                        AddHandler btnCopia.Click, AddressOf btnDuplicar_Click
                    Case "Limpiar"
                        AddHandler btnCopia.Click, AddressOf btnLimpiar_Click
                    Case "Eliminar"
                        AddHandler btnCopia.Click, AddressOf btnEliminar_Click
                End Select
            End If
        End If

        For Each hijo As Control In origen.Controls
            copia.Controls.Add(ClonarControl(hijo))
        Next

        Return copia
    End Function

    Private Sub btnGuardarPedido_Click(sender As Object, e As EventArgs) Handles btnGuardarPedido.Click
        If idClienteSeleccionado = 0 Then
            UtilFormularios.MostrarError("Seleccione un cliente antes de guardar el pedido.")
            Return
        End If

        ResolverUsuario()
        If idUsuario <= 0 Then
            UtilFormularios.MostrarError("No se pudo determinar el usuario conectado. Vuelva a abrir el formulario desde el menú.")
            Return
        End If

        If tabsPedidos.TabPages.Count = 0 Then
            UtilFormularios.MostrarError("Agregue al menos una pestaña antes de guardar el pedido.")
            Return
        End If

        Try
            itemsPedido = ConstruirItemsDesdeTabs()
            Dim idPedido As Integer = New Pedido().Registrar(idClienteSeleccionado, idUsuario, itemsPedido)
            UtilFormularios.MostrarInfo("Pedido N° " & idPedido.ToString() & " registrado correctamente.")

            itemsPedido = New List(Of ItemPedidoDTO)
            tabsPedidos.TabPages.Clear()
            RecalcularTotalTodo()
            idClienteSeleccionado = 0
            nombreClienteSeleccionado = ""
            actualizarCliente()
        Catch ex As Exception
            UtilFormularios.MostrarError(ex)
        End Try
    End Sub

    Private Const TOLERANCIA_CM As Decimal = 0.01D

    Private Function ConstruirItemsDesdeTabs() As List(Of ItemPedidoDTO)
        Dim lista As New List(Of ItemPedidoDTO)
        For Each tab As TabPage In tabsPedidos.TabPages
            lista.Add(ConstruirItemDesdeTab(tab))
        Next
        Return lista
    End Function

    Private Function ConstruirItemDesdeTab(tab As TabPage) As ItemPedidoDTO
        Dim seleccion As String = TryCast(tab.Tag, String)
        If String.IsNullOrEmpty(seleccion) Then seleccion = ""

        If seleccion.Equals("Ventana", StringComparison.OrdinalIgnoreCase) Then
            Return ConstruirItemVentana(tab)
        End If
        If EsTabAluminio(tab) Then
            Return ConstruirItemAluminio(tab)
        End If
        If Not seleccion.Equals("VIDRIO", StringComparison.OrdinalIgnoreCase) Then
            Throw New Exception("La pestaña '" & tab.Text & "' todavía no se puede guardar: por ahora solo se admiten ítems de vidrio, aluminio o ventana.")
        End If

        Dim txtNombre As TextBox = TryCast(BuscarControl(tab, "txtNombreVidrio"), TextBox)
        If txtNombre Is Nothing OrElse txtNombre.Tag Is Nothing OrElse CInt(txtNombre.Tag) <= 0 Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el vidrio con el botón Buscar Vidrio.")
        End If
        Dim idProducto As Integer = CInt(txtNombre.Tag)

        Dim espesor As Decimal = EspesorDeTab(tab)
        If espesor <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el espesor de vidrio.")
        End If

        Dim ancho As Decimal = DecimalDe(tab, "txtAncho", "el ancho")
        Dim alto As Decimal = DecimalDe(tab, "txtAlto", "el alto")
        If ancho <= 0D OrElse alto <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' ingrese las medidas de pedido (ancho y alto).")
        End If

        Dim precioMateriales As Decimal = DecimalDe(tab, "txtPreciomaterial", "el precio de los materiales")
        Dim costoExtra As Decimal = DecimalDe(tab, "txtExtra", "los extras")
        Dim manoObra As Decimal = DecimalDe(tab, "txtManoObra", "la mano de obra")
        Dim total As Decimal = DecimalDe(tab, "txtTotal", "el total")
        If total <= 0D Then total = precioMateriales + costoExtra + manoObra
        If total <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' no hay precios calculados.")
        End If
        total = RedondearEntero(total)

        Dim idVariante As Integer = New ProductoVariante().ResolverVariantePorProducto(idProducto, espesor)

        Dim accion As String = "COMPRA_NUEVA"
        Dim idPieza As Integer? = Nothing

        Dim piezas As DataTable = New InventarioPieza().BuscarDisponibles(idVariante, ancho, alto)
        If piezas.Rows.Count > 0 Then
            Dim fila As DataRow = piezas.Rows(0)
            Dim pAncho As Decimal = ADecimal(fila("ancho_cm"))
            Dim pAlto As Decimal = ADecimal(fila("alto_cm"))
            idPieza = Convert.ToInt32(fila("id_pieza"))
            Dim calzaDirecto As Boolean = Math.Abs(pAncho - ancho) <= TOLERANCIA_CM AndAlso Math.Abs(pAlto - alto) <= TOLERANCIA_CM
            Dim calzaRotado As Boolean = Math.Abs(pAncho - alto) <= TOLERANCIA_CM AndAlso Math.Abs(pAlto - ancho) <= TOLERANCIA_CM
            accion = If(calzaDirecto OrElse calzaRotado, "USA_EXISTENTE", "CORTA_PIEZA")
        End If

        Dim detalle As New DetallePedidoDTO With {
            .IdVariante = idVariante,
            .Accion = accion,
            .IdPieza = idPieza,
            .AnchoCm = ancho,
            .AltoCm = alto,
            .Subtotal = precioMateriales
        }

        Return New ItemPedidoDTO With {
            .Tipo = "VIDRIO_SUELTO",
            .Etiqueta = tab.Text,
            .PrecioMateriales = precioMateriales,
            .CostoExtra = costoExtra,
            .ManoObra = manoObra,
            .Total = total,
            .Detalles = New List(Of DetallePedidoDTO) From {detalle}
        }
    End Function

    ' Aluminio: se vende por largo (una sola medida) y siempre se registra como compra nueva
    Private Function ConstruirItemAluminio(tab As TabPage) As ItemPedidoDTO
        Dim txtNombre As TextBox = TryCast(BuscarControl(tab, "txtNombreAluminio"), TextBox)
        If txtNombre Is Nothing OrElse txtNombre.Tag Is Nothing OrElse CInt(txtNombre.Tag) <= 0 Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el aluminio con el botón Buscar Aluminio.")
        End If
        Dim idProducto As Integer = CInt(txtNombre.Tag)

        Dim largo As Decimal = DecimalDe(tab, "txtLargoAluminio", "el largo de aluminio")
        If largo <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' ingrese el largo de aluminio.")
        End If

        Dim precioMateriales As Decimal = DecimalDe(tab, "txtPrecioMaterialesAluminio", "el precio de los materiales")
        Dim costoExtra As Decimal = DecimalDe(tab, "txtExtraAluminio", "los extras")
        Dim manoObra As Decimal = DecimalDe(tab, "txtManoObraAluminio", "la mano de obra")
        Dim total As Decimal = DecimalDe(tab, "txtTotalAluminio", "el total")
        If total <= 0D Then total = precioMateriales + costoExtra + manoObra
        If total <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' no hay precios calculados.")
        End If
        total = RedondearEntero(total)

        Dim idVariante As Integer = New ProductoVariante().ObtenerVariantePorProducto(idProducto)
        If idVariante <= 0 Then
            Throw New Exception("En la pestaña '" & tab.Text & "' el aluminio no tiene una variante con precio.")
        End If

        Dim detalle As New DetallePedidoDTO With {
            .IdVariante = idVariante,
            .Accion = "COMPRA_NUEVA",
            .IdPieza = Nothing,
            .AnchoCm = 0D,
            .AltoCm = 0D,
            .LargoCm = largo,
            .Subtotal = precioMateriales
        }

        Return New ItemPedidoDTO With {
            .Tipo = "ALUMINIO",
            .Etiqueta = tab.Text,
            .PrecioMateriales = precioMateriales,
            .CostoExtra = costoExtra,
            .ManoObra = manoObra,
            .Total = total,
            .Detalles = New List(Of DetallePedidoDTO) From {detalle}
        }
    End Function

    ' ================== VENTANA (2 fijos + 1 corrediza) ==================
    ' El ancho debe tener 3 medidas (arriba/centro/abajo) o una sola (abajo).
    ' El alto debe tener 3 medidas (izquierda/centro/derecha) o una sola (izquierda).
    ' El alero (35 cm) se aplica automaticamente desde 90 cm de alto.
    Private Const VENTANA_ANCHO_MINIMO_CM As Decimal = 105D
    Private Const VENTANA_ALERO_CM As Decimal = 35D
    Private Const VENTANA_ALERO_ALTO_MINIMO_CM As Decimal = 90D
    Private Const VENTANA_ALERO_ANCHO_DOBLE_CM As Decimal = 120D

    Private Function ConstruirItemVentana(tab As TabPage) As ItemPedidoDTO
        Dim colorVentana As String = TextoCombo(tab, "cmbColorAlumnioVentana")
        If String.IsNullOrWhiteSpace(colorVentana) Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el color de aluminio.")
        End If
        Dim colorMat As String = ColorMaterial(colorVentana)

        Dim idProductoVidrio As Integer = IdProductoDe(tab, "txtNombreVidrioVentana")
        If idProductoVidrio <= 0 Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el vidrio con el botón Buscar Vidrio.")
        End If

        Dim espesor As Decimal = 0D
        Dim cmbEspesor As ComboBox = TryCast(BuscarControl(tab, "cmbEspesorVentana"), ComboBox)
        If cmbEspesor IsNot Nothing AndAlso cmbEspesor.SelectedItem IsNot Nothing Then
            espesor = Convert.ToDecimal(cmbEspesor.SelectedItem)
        End If
        If espesor <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' seleccione el espesor de vidrio.")
        End If

        Dim idVarianteVidrio As Integer = New ProductoVariante().ResolverVariantePorProducto(idProductoVidrio, espesor)
        Dim precioVidrio As Decimal = New ProductoVariante().ObtenerPrecioUnitario(idProductoVidrio, espesor)

        Dim tresAncho As Boolean = CheckMarcado(tab, "CheckBox3")
        Dim tresAlto As Boolean = CheckMarcado(tab, "CheckBox4")

        Dim anchoAbajo As Decimal = LeerDecimal(tab, "txtAbaVentana")
        Dim anchoArriba As Decimal = If(tresAncho, LeerDecimal(tab, "txtArrVentana"), anchoAbajo)
        Dim anchoCentro As Decimal = If(tresAncho, LeerDecimal(tab, "txtCenVentana1"), anchoAbajo)
        Dim altoIzq As Decimal = LeerDecimal(tab, "txtIzqVentana")
        Dim altoCentro As Decimal = If(tresAlto, LeerDecimal(tab, "txtCenVentana2"), altoIzq)
        Dim altoDer As Decimal = If(tresAlto, LeerDecimal(tab, "txtDerVentana"), altoIzq)

        Dim maxAncho As Decimal = Math.Max(anchoAbajo, Math.Max(anchoArriba, anchoCentro))
        Dim maxAlto As Decimal = Math.Max(altoIzq, Math.Max(altoCentro, altoDer))

        If maxAncho < VENTANA_ANCHO_MINIMO_CM Then
            Throw New Exception("La ventana '" & tab.Text & "' debe medir al menos 105 cm de ancho.")
        End If
        If maxAlto <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' ingrese las medidas de alto.")
        End If

        Dim tieneAlero As Boolean = maxAlto >= VENTANA_ALERO_ALTO_MINIMO_CM
        Dim medidaAlero As Decimal = If(tieneAlero, VENTANA_ALERO_CM, 0D)
        Dim anclaje As Decimal = maxAlto - 3D - medidaAlero

        Dim detalles As New List(Of DetallePedidoDTO)

        ' Retazos de aluminio
        detalles.Add(DetalleMaterial("ALUMINIO", "ANGULO GUIA", colorMat, anchoAbajo, Nothing))

        Dim largoTubo As Decimal = If(tresAncho, Math.Max(anchoArriba, anchoCentro), anchoAbajo)
        detalles.Add(DetalleMaterial("ALUMINIO", "TUBO MULTIPLE", colorMat, largoTubo, Nothing))
        detalles.Add(DetalleMaterial("ACCESORIO", "FELPA 10", colorMat, largoTubo, Nothing))
        detalles.Add(DetalleMaterial("ACCESORIO", "FELPA 20", colorMat, largoTubo, Nothing))

        If tieneAlero Then
            Dim anchoU13Alero As Decimal = If(tresAncho, anchoArriba, anchoAbajo)
            detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, anchoU13Alero, Nothing))
        End If

        ' Vidrios fijos (2)
        Dim altoFijos As Decimal = maxAlto - 3D - 1.5D - medidaAlero
        Dim anchoFijoArriba As Decimal = If(tresAncho, anchoCentro, anchoAbajo) / 3D
        Dim anchoFijoAbajo As Decimal = anchoAbajo / 3D

        detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, anchoFijoArriba, altoFijos))
        detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, anchoFijoAbajo, altoFijos))

        detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, anchoFijoArriba, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, anclaje, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, anchoFijoAbajo, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, anclaje, Nothing))

        ' Vidrio corredizo
        Dim maxFijoAncho As Decimal = Math.Max(anchoFijoArriba, anchoFijoAbajo)
        Dim corredizoAncho As Decimal = anchoAbajo - 2D * maxFijoAncho
        Dim corredizoAlto As Decimal = anclaje - 2.5D

        detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, corredizoAncho, corredizoAlto))

        detalles.Add(DetalleMaterial("ALUMINIO", "H8220", colorMat, corredizoAncho + 1D, Nothing))
        detalles.Add(DetalleMaterial("ACCESORIO", "RUEDA 8220", Nothing, Nothing, 2))
        detalles.Add(DetalleMaterial("ACCESORIO", "SEGURO PISTILLO", colorVentana, Nothing, 1))
        detalles.Add(DetalleMaterial("ALUMINIO", "PORTA FELPA", colorMat, anclaje, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "PORTA FELPA", colorMat, anclaje, Nothing))
        detalles.Add(DetalleMaterial("ACCESORIO", "FELPA 15", colorMat, anclaje, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "PORTA FELPA", colorMat, anclaje - 1D, Nothing))
        detalles.Add(DetalleMaterial("ALUMINIO", "PORTA FELPA", colorMat, anclaje - 1D, Nothing))
        detalles.Add(DetalleMaterial("ACCESORIO", "FELPA 20", colorMat, anclaje - 1D, Nothing))

        ' Alero: vidrios y U13
        If tieneAlero Then
            Dim dosVidrios As Boolean = anchoAbajo > VENTANA_ALERO_ANCHO_DOBLE_CM OrElse anchoArriba > VENTANA_ALERO_ANCHO_DOBLE_CM
            Dim anchoVidrioAlero As Decimal = If(dosVidrios, anchoArriba / 2D, anchoArriba)

            If dosVidrios Then
                Dim altoUno As Decimal = If(tresAlto, Math.Max(altoIzq, altoCentro), altoIzq) - 1D - anclaje
                Dim altoDos As Decimal = If(tresAlto, Math.Max(altoCentro, altoDer), altoIzq) - 1D - anclaje
                detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, anchoVidrioAlero, altoUno))
                detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, anchoVidrioAlero, altoDos))
                detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, maxAlto - 2D - anclaje, Nothing))
                detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, maxAlto - 2D - anclaje, Nothing))
                detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, maxAlto - 2D - anclaje, Nothing))
            Else
                Dim altoAlero As Decimal = If(tresAlto, maxAlto, altoIzq) - 1D - anclaje
                detalles.Add(DetalleVidrio(idVarianteVidrio, precioVidrio, anchoVidrioAlero, altoAlero))
                detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, maxAlto - 2D - anclaje, Nothing))
                detalles.Add(DetalleMaterial("ALUMINIO", "U13", colorMat, maxAlto - 2D - anclaje, Nothing))
            End If

            detalles.Add(DetalleMaterial("ACCESORIO", "SILICONA", ColorSilicona(colorVentana), Nothing, 2))
        End If

        Dim precioMateriales As Decimal = DecimalDe(tab, "txtPreciomaterialVentana", "el precio de los materiales")
        Dim costoExtra As Decimal = DecimalDe(tab, "txtExtraVentana", "los extras")
        Dim manoObra As Decimal = DecimalDe(tab, "txtManoObraVentana", "la mano de obra")
        Dim total As Decimal = DecimalDe(tab, "txtTotalVentana", "el total")
        If total <= 0D Then total = precioMateriales + costoExtra + manoObra
        If total <= 0D Then
            Throw New Exception("En la pestaña '" & tab.Text & "' no hay precios calculados.")
        End If
        total = RedondearEntero(total)

        Return New ItemPedidoDTO With {
            .Tipo = "VENTANA",
            .Etiqueta = tab.Text,
            .PrecioMateriales = precioMateriales,
            .CostoExtra = costoExtra,
            .ManoObra = manoObra,
            .Total = total,
            .Detalles = detalles
        }
    End Function

    ' Detalle de vidrio: se cobra por area (ancho * alto / 900) al precio de la variante
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

    ' Detalle de un material del catalogo: resuelve la variante por categoria + tipo + color.
    ' Si cantidad tiene valor se cobra por unidad; si no, por cm lineal (largo).
    Private Function DetalleMaterial(categoria As String, tipo As String, color As String,
                                     largo As Decimal?, cantidad As Integer?) As DetallePedidoDTO
        Dim idProducto As Integer = New Producto().ObtenerIdPorTipoColor(categoria, tipo, color)
        If idProducto <= 0 Then
            Throw New Exception("No existe el material " & categoria & " '" & tipo & "' (" & color & ") en el catálogo.")
        End If
        Dim idVariante As Integer = New ProductoVariante().ObtenerVariantePorProducto(idProducto)
        If idVariante <= 0 Then
            Throw New Exception("El material " & categoria & " '" & tipo & "' no tiene una variante con precio.")
        End If
        Dim precioUnitario As Decimal = New ProductoVariante().ObtenerPrecioUnitarioPorProducto(idProducto)

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

    ' El aluminio (y las felpas de 10/15/20 cm) se piden en negro cuando la ventana es color madera.
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

    Private Function CheckMarcado(tab As TabPage, nombre As String) As Boolean
        Dim chb As CheckBox = TryCast(BuscarControl(tab, nombre), CheckBox)
        Return chb IsNot Nothing AndAlso chb.Checked
    End Function

    Private Function BuscarControl(tab As TabPage, nombre As String) As Control
        Dim hallados As Control() = tab.Controls.Find(nombre, True)
        If hallados Is Nothing OrElse hallados.Length = 0 Then Return Nothing
        Return hallados(0)
    End Function

    Private Function TextoDe(tab As TabPage, nombre As String) As String
        Dim c As Control = BuscarControl(tab, nombre)
        If c Is Nothing Then Return ""
        Return c.Text.Trim()
    End Function

    Private Function DecimalDe(tab As TabPage, nombre As String, etiqueta As String) As Decimal
        Dim texto As String = TextoDe(tab, nombre)
        If texto = "" Then Return 0D
        Dim valor As Decimal
        If Not Decimal.TryParse(texto, valor) Then
            Throw New Exception("La pestaña '" & tab.Text & "' tiene un valor inválido en " & etiqueta & ".")
        End If
        Return valor
    End Function

    Private Function ADecimal(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return Convert.ToDecimal(valor)
    End Function

    Private Sub chbMedida_CheckedChanged(sender As Object, e As EventArgs) Handles chbAncho.CheckedChanged, chbAlto.CheckedChanged, CheckBox3.CheckedChanged, CheckBox4.CheckedChanged
        Dim chb As CheckBox = DirectCast(sender, CheckBox)
        If chb.Name = "CheckBox3" OrElse chb.Name = "CheckBox4" Then
            AplicarVisibilidadMedidasVentana(chb)
        Else
            AplicarVisibilidadMedidas(chb)
        End If
    End Sub

    Private Sub AplicarVisibilidadMedidas(chb As CheckBox)
        Dim panel As Control = chb.Parent
        If panel Is Nothing Then Return

        Dim nombres() As String
        If chb.Name = "chbAncho" Then
            nombres = New String() {"lbl1", "lbl3", "txtAncho2", "lbl4"}
        ElseIf chb.Name = "chbAlto" Then
            nombres = New String() {"lbl5", "lbl6", "txtAlto2", "lbl7"}
        Else
            Return
        End If

        For Each nombre As String In nombres
            If panel.Controls.ContainsKey(nombre) Then
                panel.Controls(nombre).Visible = chb.Checked
            End If
        Next
    End Sub

    ' Muestra/oculta la fila de medidas de la ventana (Ancho = arriba/centro/abajo; Alto = derecha/centro/izquierda)
    Private Sub AplicarVisibilidadMedidasVentana(chb As CheckBox)
        Dim panel As Control = chb.Parent
        If panel Is Nothing Then Return

        Dim nombres() As String
        If chb.Name = "CheckBox3" Then
            nombres = New String() {"Label24", "txtArrVentana", "Label27", "txtCenVentana1", "Label22"}
        ElseIf chb.Name = "CheckBox4" Then
            nombres = New String() {"Label23", "txtDerVentana", "Label36", "txtCenVentana2", "Label21"}
        Else
            Return
        End If

        For Each nombre As String In nombres
            If panel.Controls.ContainsKey(nombre) Then
                panel.Controls(nombre).Visible = chb.Checked
            End If
        Next
    End Sub

    ' Llena el combo de espesores segun el producto elegido (por su id)
    Private Sub CargarEspesoresEn(tab As TabPage, nombreCombo As String, idProducto As Integer)
        Dim cmbEspesor As ComboBox = TryCast(BuscarControl(tab, nombreCombo), ComboBox)
        If cmbEspesor Is Nothing Then Return

        cmbEspesor.Items.Clear()
        cmbEspesor.SelectedIndex = -1
        cmbEspesor.Enabled = False
        If idProducto <= 0 Then Return

        Try
            For Each espesor As Decimal In New ProductoVariante().ListarEspesoresPorProducto(idProducto)
                cmbEspesor.Items.Add(espesor)
            Next
        Catch
            Return
        End Try
        cmbEspesor.Enabled = cmbEspesor.Items.Count > 0
    End Sub

    Private Function TabDeControl(c As Control) As TabPage
        While c IsNot Nothing AndAlso Not (TypeOf c Is TabPage)
            c = c.Parent
        End While
        Return TryCast(c, TabPage)
    End Function

    Private Function EspesorDeTab(tab As TabPage) As Decimal
        Dim cmb As ComboBox = TryCast(BuscarControl(tab, "txtEspesor"), ComboBox)
        If cmb Is Nothing OrElse cmb.SelectedItem Is Nothing Then Return 0D
        Return Convert.ToDecimal(cmb.SelectedItem)
    End Function

    Private Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Dim actual As TabPage = tabsPedidos.SelectedTab
        If actual Is Nothing Then Return

        Dim seleccion As String = TryCast(actual.Tag, String)
        If String.IsNullOrEmpty(seleccion) Then Return

        Dim numero As Integer = SiguienteNumero(seleccion)
        Dim nueva As TabPage = ClonarTabPage(actual)
        nueva.Tag = seleccion
        nueva.Name = seleccion & numero.ToString()
        nueva.Text = seleccion & numero.ToString()
        tabsPedidos.TabPages.Add(nueva)
        tabsPedidos.SelectedTab = nueva
        RecalcularTab(nueva)
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Dim actual As TabPage = tabsPedidos.SelectedTab
        If actual Is Nothing Then Return
        LimpiarControles(actual)
        RecalcularTab(actual)
    End Sub

    Private Sub LimpiarControles(contenedor As Control)
        For Each c As Control In contenedor.Controls
            Dim txt As TextBox = TryCast(c, TextBox)
            If txt IsNot Nothing Then
                txt.Clear()
                txt.Tag = Nothing
            Else
                Dim cmb As ComboBox = TryCast(c, ComboBox)
                If cmb IsNot Nothing Then
                    If cmb.Name = "txtEspesor" Then
                        cmb.Items.Clear()
                        cmb.Enabled = False
                    End If
                    cmb.SelectedIndex = -1
                Else
                    Dim chb As CheckBox = TryCast(c, CheckBox)
                    If chb IsNot Nothing Then
                        chb.Checked = False
                        If chb.Name = "CheckBox3" OrElse chb.Name = "CheckBox4" Then
                            AplicarVisibilidadMedidasVentana(chb)
                        Else
                            AplicarVisibilidadMedidas(chb)
                        End If
                    ElseIf c.HasChildren Then
                        LimpiarControles(c)
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim actual As TabPage = tabsPedidos.SelectedTab
        If actual Is Nothing Then Return
        tabsPedidos.TabPages.Remove(actual)
        RecalcularTotalTodo()
    End Sub

    Private Sub btnSelectVidrio_Click(sender As Object, e As EventArgs) Handles btnSelectVidrio.Click, btnBuscarAluminio.Click
        Dim boton As Button = TryCast(sender, Button)
        Dim tab As TabPage = TabDeControl(boton)
        If tab Is Nothing Then Return

        Dim categoria As String = "VIDRIO"
        Dim nombreProducto As String = "txtNombreVidrio"
        Dim nombreEspesor As String = "txtEspesor"

        If boton IsNot Nothing Then
            Select Case boton.Name
                Case "btnBuscarVidrioVentana"
                    nombreProducto = "txtNombreVidrioVentana"
                    nombreEspesor = "cmbEspesorVentana"
                Case "btnBuscarAluminio"
                    categoria = "ALUMINIO"
                    nombreProducto = "txtNombreAluminio"
                    nombreEspesor = ""   ' el aluminio no usa espesor
            End Select
        End If

        Using formulario As New frmListadoProductos With {.tema = tema, .modoSeleccion = True, .categoriaFiltro = categoria}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                Dim txtNombre As TextBox = TryCast(BuscarControl(tab, nombreProducto), TextBox)
                If txtNombre IsNot Nothing Then
                    txtNombre.Text = formulario.nombreProductoSeleccionado
                    txtNombre.Tag = formulario.idProductoSeleccionado
                End If
                If nombreEspesor <> "" Then
                    CargarEspesoresEn(tab, nombreEspesor, formulario.idProductoSeleccionado)
                End If
                RecalcularTab(tab)
            End If
        End Using
    End Sub

    ' ======================= CALCULOS AUTOMATICOS =======================

    ' Campos que al cambiar disparan el recalculo completo (materiales, extras y total)
    Private Shared ReadOnly CamposCalculo As String() = {
        "txtAncho", "txtAncho2", "txtAlto", "txtAlto2",
        "txtArrVentana", "txtCenVentana1", "txtAbaVentana", "txtDerVentana",
        "txtCenVentana2", "txtIzqVentana", "txtLargoAluminio"}

    ' Campos editables que solo deben recalcular el total (no se sobreescriben)
    Private Shared ReadOnly CamposTotal As String() = {
        "txtExtra", "txtExtraVentana", "txtManoObra", "txtManoObraVentana",
        "txtExtraAluminio", "txtManoObraAluminio"}

    Private Shared ReadOnly CombosCalculo As String() = {
        "txtEspesor", "cmbEspesorVentana", "cmbColorAlumnioVentana"}

    ' Campos numericos que se muestran con dos decimales al salir del campo
    Private Shared ReadOnly CamposNumericos As String() = {
        "txtAncho", "txtAncho2", "txtAlto", "txtAlto2",
        "txtArrVentana", "txtCenVentana1", "txtAbaVentana", "txtDerVentana",
        "txtCenVentana2", "txtIzqVentana",
        "txtExtra", "txtManoObra", "txtExtraVentana", "txtManoObraVentana",
        "txtLargoAluminio", "txtExtraAluminio", "txtManoObraAluminio"}

    Private Shared Function EsCampoNumerico(nombre As String) As Boolean
        Return Array.IndexOf(CamposNumericos, nombre) >= 0
    End Function

    Private Shared Function EsCampoCalculo(nombre As String) As Boolean
        Return Array.IndexOf(CamposCalculo, nombre) >= 0
    End Function

    Private Shared Function EsCampoTotal(nombre As String) As Boolean
        Return Array.IndexOf(CamposTotal, nombre) >= 0
    End Function

    Private Shared Function EsCampoCalculoCombo(nombre As String) As Boolean
        Return Array.IndexOf(CombosCalculo, nombre) >= 0
    End Function

    Private Function EsTabVentana(tab As TabPage) As Boolean
        Return BuscarControl(tab, "txtTotalVentana") IsNot Nothing
    End Function

    Private Function EsTabAluminio(tab As TabPage) As Boolean
        Return BuscarControl(tab, "txtNombreAluminio") IsNot Nothing
    End Function

    Private Sub CampoCalculo_Changed(sender As Object, e As EventArgs)
        Dim c As Control = TryCast(sender, Control)
        If c Is Nothing Then Return
        RecalcularTab(TabDeControl(c))
    End Sub

    Private Sub CampoTotal_Changed(sender As Object, e As EventArgs)
        Dim c As Control = TryCast(sender, Control)
        If c Is Nothing Then Return
        RecalcularTotales(TabDeControl(c))
    End Sub

    ' Al salir de un campo numerico se normaliza a dos decimales
    Private Sub CampoNumerico_Leave(sender As Object, e As EventArgs)
        Dim txt As TextBox = TryCast(sender, TextBox)
        If txt Is Nothing Then Return
        Dim valor As Decimal
        If Decimal.TryParse(txt.Text.Trim(), valor) Then
            txt.Text = valor.ToString("0.00")
        End If
    End Sub

    ' Recalcula materiales, extras (sugeridos) y total de una pestaña
    Private Sub RecalcularTab(tab As TabPage)
        If tab Is Nothing Then Return
        Try
            If EsTabVentana(tab) Then
                AsignarDecimal(tab, "txtPreciomaterialVentana", MaterialVentana(tab))
                AsignarDecimal(tab, "txtExtraVentana", ExtraSugeridoVentana(tab))
            ElseIf EsTabAluminio(tab) Then
                AsignarDecimal(tab, "txtPrecioMaterialesAluminio", MaterialAluminio(tab))
            Else
                AsignarDecimal(tab, "txtPreciomaterial", MaterialVidrio(tab))
                AsignarDecimal(tab, "txtExtra", ExtraSugeridoVidrio(tab))
            End If
            RecalcularTotales(tab)
        Catch
        End Try
    End Sub

    ' Solo reevalua el total (cuando se editan extras o mano de obra)
    Private Sub RecalcularTotales(tab As TabPage)
        If tab Is Nothing Then Return
        Try
            If EsTabVentana(tab) Then
                Dim materialV As Decimal = MaterialVentana(tab)
                AsignarDecimal(tab, "txtPreciomaterialVentana", materialV)
                AsignarTotal(tab, "txtTotalVentana", LeerDecimal(tab, "txtExtraVentana") + LeerDecimal(tab, "txtManoObraVentana") + materialV)
            ElseIf EsTabAluminio(tab) Then
                Dim materialA As Decimal = MaterialAluminio(tab)
                AsignarDecimal(tab, "txtPrecioMaterialesAluminio", materialA)
                AsignarTotal(tab, "txtTotalAluminio", LeerDecimal(tab, "txtExtraAluminio") + LeerDecimal(tab, "txtManoObraAluminio") + materialA)
            Else
                Dim materialV As Decimal = MaterialVidrio(tab)
                AsignarDecimal(tab, "txtPreciomaterial", materialV)
                AsignarTotal(tab, "txtTotal", LeerDecimal(tab, "txtExtra") + LeerDecimal(tab, "txtManoObra") + materialV)
            End If
        Catch
        End Try
        RecalcularTotalTodo()
    End Sub

    Private Function MaterialVidrio(tab As TabPage) As Decimal
        Dim anchoMayor As Decimal = MayorDe(tab, "txtAncho", "txtAncho2")
        Dim altoMayor As Decimal = MayorDe(tab, "txtAlto", "txtAlto2")
        Dim precioUnitario As Decimal = PrecioMaterial(tab, "txtNombreVidrio", "txtEspesor")
        If anchoMayor > 0D AndAlso altoMayor > 0D AndAlso precioUnitario > 0D Then
            Return altoMayor * anchoMayor / 900D * precioUnitario
        End If
        Return 0D
    End Function

    ' ALUMINIO: se cobra por largo; material = precio_unitario de la variante * largo
    Private Function MaterialAluminio(tab As TabPage) As Decimal
        Dim idProducto As Integer = IdProductoDe(tab, "txtNombreAluminio")
        If idProducto <= 0 Then Return 0D
        Dim largo As Decimal = LeerDecimal(tab, "txtLargoAluminio")
        If largo <= 0D Then Return 0D
        Return New ProductoVariante().ObtenerPrecioUnitarioPorProducto(idProducto) * largo
    End Function

    ' Lee el id de producto guardado en el Tag de un TextBox dentro de la pestaña
    Private Function IdProductoDe(tab As TabPage, nombre As String) As Integer
        Dim txt As TextBox = TryCast(BuscarControl(tab, nombre), TextBox)
        If txt Is Nothing OrElse txt.Tag Is Nothing Then Return 0
        Dim id As Integer
        If Not Integer.TryParse(txt.Tag.ToString(), id) Then Return 0
        Return id
    End Function

    Private Function ExtraSugeridoVidrio(tab As TabPage) As Decimal
        Dim anchoMayor As Decimal = MayorDe(tab, "txtAncho", "txtAncho2")
        Dim altoMayor As Decimal = MayorDe(tab, "txtAlto", "txtAlto2")
        If anchoMayor > 0D AndAlso altoMayor > 0D Then
            Return New Pedido().TarifaSugerida(anchoMayor, altoMayor).silicona
        End If
        Return 0D
    End Function

    Private Function MaterialVentana(tab As TabPage) As Decimal
        Dim anchoMayor As Decimal = MayorDe(tab, "txtArrVentana", "txtCenVentana1", "txtAbaVentana")
        Dim altoMayor As Decimal = MayorDe(tab, "txtDerVentana", "txtCenVentana2", "txtIzqVentana")
        Dim precioUnitario As Decimal = PrecioMaterial(tab, "txtNombreVidrioVentana", "cmbEspesorVentana")
        If anchoMayor > 0D AndAlso altoMayor > 0D AndAlso precioUnitario > 0D Then
            Return altoMayor * anchoMayor / 900D * precioUnitario
        End If
        Return 0D
    End Function

    Private Function ExtraSugeridoVentana(tab As TabPage) As Decimal
        Dim anchoMayor As Decimal = MayorDe(tab, "txtArrVentana", "txtCenVentana1", "txtAbaVentana")
        Dim altoMayor As Decimal = MayorDe(tab, "txtDerVentana", "txtCenVentana2", "txtIzqVentana")
        Dim extra As Decimal = 0D
        If anchoMayor > 0D AndAlso altoMayor > 0D Then
            Dim tarifa = New Pedido().TarifaSugerida(anchoMayor, altoMayor)
            extra = tarifa.aluminio + tarifa.silicona
        End If

        Dim color As String = TextoCombo(tab, "cmbColorAlumnioVentana")
        If color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase) Then
            extra *= 1.02D
        ElseIf color.Equals("MADERA", StringComparison.OrdinalIgnoreCase) Then
            extra *= 1.05D
        End If
        Return extra
    End Function

    Private Sub RecalcularTotalTodo()
        Dim suma As Decimal = 0D
        For Each tab As TabPage In tabsPedidos.TabPages
            Dim campoTotal As String
            If EsTabVentana(tab) Then
                campoTotal = "txtTotalVentana"
            ElseIf EsTabAluminio(tab) Then
                campoTotal = "txtTotalAluminio"
            Else
                campoTotal = "txtTotal"
            End If
            suma += LeerDecimal(tab, campoTotal)
        Next
        txtTotalTodo.Text = RedondearEntero(suma).ToString("0")
    End Sub

    Private Function MayorDe(tab As TabPage, ParamArray nombres() As String) As Decimal
        Dim mayor As Decimal = 0D
        For Each nombre As String In nombres
            Dim valor As Decimal = LeerDecimal(tab, nombre)
            If valor > mayor Then mayor = valor
        Next
        Return mayor
    End Function

    Private Function LeerDecimal(tab As TabPage, nombre As String) As Decimal
        Dim texto As String = TextoDe(tab, nombre)
        Dim valor As Decimal
        If texto = "" OrElse Not Decimal.TryParse(texto, valor) Then Return 0D
        Return valor
    End Function

    Private Function TextoCombo(tab As TabPage, nombre As String) As String
        Dim cmb As ComboBox = TryCast(BuscarControl(tab, nombre), ComboBox)
        If cmb Is Nothing OrElse cmb.SelectedItem Is Nothing Then Return ""
        Return cmb.SelectedItem.ToString()
    End Function

    Private Sub AsignarDecimal(tab As TabPage, nombre As String, valor As Decimal)
        Dim txt As TextBox = TryCast(BuscarControl(tab, nombre), TextBox)
        If txt IsNot Nothing Then txt.Text = valor.ToString("0.00")
    End Sub

    ' Los totales se muestran como soles enteros (aproximados)
    Private Sub AsignarTotal(tab As TabPage, nombre As String, valor As Decimal)
        Dim txt As TextBox = TryCast(BuscarControl(tab, nombre), TextBox)
        If txt IsNot Nothing Then txt.Text = RedondearEntero(valor).ToString("0")
    End Sub

    Private Function PrecioMaterial(tab As TabPage, nombreProducto As String, nombreEspesor As String) As Decimal
        Dim txtNombre As TextBox = TryCast(BuscarControl(tab, nombreProducto), TextBox)
        If txtNombre Is Nothing OrElse txtNombre.Tag Is Nothing Then Return 0D

        Dim idProducto As Integer
        If Not Integer.TryParse(txtNombre.Tag.ToString(), idProducto) OrElse idProducto <= 0 Then Return 0D

        Dim cmb As ComboBox = TryCast(BuscarControl(tab, nombreEspesor), ComboBox)
        If cmb Is Nothing OrElse cmb.SelectedItem Is Nothing Then Return 0D

        Dim espesor As Decimal = Convert.ToDecimal(cmb.SelectedItem)
        Try
            Return New ProductoVariante().ObtenerPrecioUnitario(idProducto, espesor)
        Catch
            Return 0D
        End Try
    End Function

    ' ================== DATOS DESDE frmCotizarVentana ==================

    ' Crea una pestaña con el tipo de pedido y los datos traidos del cotizador
    Private Sub AgregarDesdeCotizacion()
        Dim seleccion As String = cotSeleccion
        If String.IsNullOrWhiteSpace(seleccion) Then Return

        CrearTab(seleccion)
        Dim tab As TabPage = tabsPedidos.SelectedTab
        If tab Is Nothing Then Return

        If EsTabVentana(tab) Then
            AsignarTexto(tab, "txtArrVentana", cotAncho)
            AsignarTexto(tab, "txtDerVentana", cotAlto)
            AsignarTexto(tab, "txtManoObraVentana", cotManoObra)
            MarcarCheck(tab, "CheckBox3")
            MarcarCheck(tab, "CheckBox4")
            PonerVidrioDesdeCotizacion(tab, "txtNombreVidrioVentana", "cmbEspesorVentana")
            AsignarTexto(tab, "txtExtraVentana", cotExtra)
        ElseIf EsTabAluminio(tab) Then
            AsignarTexto(tab, "txtLargoAluminio", cotAncho)
            AsignarTexto(tab, "txtManoObraAluminio", cotManoObra)
            PonerAluminioDesdeCotizacion(tab)
            AsignarTexto(tab, "txtExtraAluminio", cotExtra)
        Else
            AsignarTexto(tab, "txtAncho", cotAncho)
            AsignarTexto(tab, "txtAlto", cotAlto)
            AsignarTexto(tab, "txtManoObra", cotManoObra)
            PonerVidrioDesdeCotizacion(tab, "txtNombreVidrio", "txtEspesor")
            AsignarTexto(tab, "txtExtra", cotExtra)
        End If

        RecalcularTotales(tab)
    End Sub

    ' Coloca el producto de aluminio (color + tipo) traido del cotizador
    Private Sub PonerAluminioDesdeCotizacion(tab As TabPage)
        Dim idProducto As Integer = 0
        Try
            idProducto = New Producto().ObtenerIdPorTipoColor("ALUMINIO", cotTipoAluminio, cotColorAluminio)
        Catch
        End Try
        If idProducto <= 0 Then Return

        Dim txtNombre As TextBox = TryCast(BuscarControl(tab, "txtNombreAluminio"), TextBox)
        If txtNombre IsNot Nothing Then
            txtNombre.Text = (cotTipoAluminio & " " & cotColorAluminio).Trim()
            txtNombre.Tag = idProducto
        End If
    End Sub

    Private Sub PonerVidrioDesdeCotizacion(tab As TabPage, nombreProducto As String, nombreEspesor As String)
        Dim idProducto As Integer = 0
        Try
            idProducto = New Producto().ObtenerIdPorTipoColor("VIDRIO", cotTipoVidrio, cotColorVidrio)
        Catch
        End Try
        If idProducto <= 0 Then Return

        Dim txtNombre As TextBox = TryCast(BuscarControl(tab, nombreProducto), TextBox)
        If txtNombre IsNot Nothing Then
            txtNombre.Text = (cotTipoVidrio & " " & cotColorVidrio).Trim()
            txtNombre.Tag = idProducto
        End If
        CargarEspesoresEn(tab, nombreEspesor, idProducto)
        SeleccionarEspesor(tab, nombreEspesor, cotEspesor)
    End Sub

    Private Sub SeleccionarEspesor(tab As TabPage, nombreCombo As String, espesor As Decimal)
        Dim cmb As ComboBox = TryCast(BuscarControl(tab, nombreCombo), ComboBox)
        If cmb Is Nothing Then Return
        For i As Integer = 0 To cmb.Items.Count - 1
            If Convert.ToDecimal(cmb.Items(i)) = espesor Then
                cmb.SelectedIndex = i
                Return
            End If
        Next
    End Sub

    Private Sub AsignarTexto(tab As TabPage, nombre As String, valor As Decimal)
        Dim txt As TextBox = TryCast(BuscarControl(tab, nombre), TextBox)
        If txt IsNot Nothing Then txt.Text = valor.ToString("0.00")
    End Sub

    Private Sub MarcarCheck(tab As TabPage, nombre As String)
        Dim chb As CheckBox = TryCast(BuscarControl(tab, nombre), CheckBox)
        If chb IsNot Nothing AndAlso Not chb.Checked Then chb.Checked = True
    End Sub

End Class
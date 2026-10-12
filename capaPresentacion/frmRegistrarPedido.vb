Imports capaNegocio
Imports System.Data
Imports System.Linq
Public Class frmRegistrarPedido
    Implements IFormularioTema
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""
    Public Property idUsuario As Integer = 0
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Private contadores As New Dictionary(Of String, Integer)
    Private itemsPedido As New List(Of ItemPedidoDTO)

    Private Sub actualizarCliente()
        txtNombreCliente.Text = nombreClienteSeleccionado
    End Sub

    Private Sub frmRegistrarPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        tabsPedidos.TabPages.Remove(tabModeloVidrio)
        tabsPedidos.TabPages.Remove(tabModeloVentana)
        CargarCombos()
        AplicarVisibilidadMedidas(chbAncho)
        AplicarVisibilidadMedidas(chbAlto)
    End Sub

    Private Sub CargarCombos()
        Try
            LlenarCombo(cmbPedido, New Producto().ListarCategorias())
            cmbPedido.Items.Add("Ventana")
        Catch
            Return
        End Try

        txtEspesor.DropDownStyle = ComboBoxStyle.DropDownList
        txtEspesor.Items.Clear()
        txtEspesor.Enabled = False
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

    ' Crea una pestaña nueva a partir del modelo (Ventana = objeto especial, el resto = material)
    Private Sub CrearTab(seleccion As String)
        Dim esVentana As Boolean = seleccion.Equals("Ventana", StringComparison.OrdinalIgnoreCase)
        Dim modelo As TabPage = If(esVentana, tabModeloVentana, tabModeloVidrio)

        Dim numero As Integer = SiguienteNumero(seleccion)
        Dim nueva As TabPage = ClonarTabPage(modelo)
        nueva.Tag = seleccion
        nueva.Name = seleccion & numero.ToString()
        nueva.Text = seleccion & numero.ToString()
        tabsPedidos.TabPages.Add(nueva)
        tabsPedidos.SelectedTab = nueva
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
            If chbCopia.Name = "chbAncho" OrElse chbCopia.Name = "chbAlto" Then
                AddHandler chbCopia.CheckedChanged, AddressOf chbMedida_CheckedChanged
            End If
        End If

        Dim btnCopia As Button = TryCast(copia, Button)
        If btnCopia IsNot Nothing Then
            If btnCopia.Name = "btnSelectVidrio" Then
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
            Throw New Exception("La pestaña '" & tab.Text & "' (Ventana) todavía no se puede guardar.")
        End If
        If Not seleccion.Equals("VIDRIO", StringComparison.OrdinalIgnoreCase) Then
            Throw New Exception("La pestaña '" & tab.Text & "' todavía no se puede guardar: por ahora solo se admiten ítems de vidrio.")
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

    Private Sub chbMedida_CheckedChanged(sender As Object, e As EventArgs) Handles chbAncho.CheckedChanged, chbAlto.CheckedChanged
        AplicarVisibilidadMedidas(DirectCast(sender, CheckBox))
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

    ' Llena el combo de espesores segun el producto elegido (por su id)
    Private Sub CargarEspesores(tab As TabPage, idProducto As Integer)
        Dim cmbEspesor As ComboBox = TryCast(BuscarControl(tab, "txtEspesor"), ComboBox)
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
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        Dim actual As TabPage = tabsPedidos.SelectedTab
        If actual Is Nothing Then Return
        LimpiarControles(actual)
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
                        AplicarVisibilidadMedidas(chb)
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
    End Sub

    Private Sub btnSelectVidrio_Click(sender As Object, e As EventArgs) Handles btnSelectVidrio.Click
        Dim boton As Button = TryCast(sender, Button)
        Dim tab As TabPage = TabDeControl(boton)
        If tab Is Nothing Then Return

        Using formulario As New frmListadoProductos With {.tema = tema, .modoSeleccion = True, .categoriaFiltro = "VIDRIO"}
            If formulario.ShowDialog(Me) = DialogResult.OK Then
                Dim txtNombre As TextBox = TryCast(BuscarControl(tab, "txtNombreVidrio"), TextBox)
                If txtNombre IsNot Nothing Then
                    txtNombre.Text = formulario.nombreProductoSeleccionado
                    txtNombre.Tag = formulario.idProductoSeleccionado
                End If
                CargarEspesores(tab, formulario.idProductoSeleccionado)
            End If
        End Using
    End Sub
End Class
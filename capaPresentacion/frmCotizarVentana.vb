Imports capaNegocio

Public Class frmCotizarVentana
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idUsuario As Integer = 0

    ' Nombres de los modelos de ventana activos (se comportan como "Ventana")
    Private modelosVentana As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    Private Sub frmCotizarVentana_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        ResolverUsuario()
        PonerCombosSoloLectura()
        CargarCombos()
        AplicarVisibilidadPorPedido()
        Recalcular()
    End Sub

    ' Si no le pasaron el usuario, lo toma del formulario padre (MenuInicio.txtID)
    Private Sub ResolverUsuario()
        If idUsuario > 0 Then Return
        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If padre IsNot Nothing Then
            Integer.TryParse(padre.txtID.Text, idUsuario)
        End If
    End Sub

    ' Ningun combo debe permitir escribir: solo seleccionar
    Private Sub PonerCombosSoloLectura()
        For Each cmb As ComboBox In New ComboBox() {cmbPedido, cmbTipoVidrio, cmbColorVidrio, cmbTipoAluminio, cmbColorAluminio, txtEspesor}
            cmb.DropDownStyle = ComboBoxStyle.DropDownList
        Next
    End Sub

    Private Sub CargarCombos()
        Try
            cmbPedido.Items.Clear()
            cmbPedido.Items.Add("VIDRIO")
            cmbPedido.Items.Add("ALUMINIO")

            ' Un modelo de ventana por cada fila activa de modelos_ventana
            modelosVentana.Clear()
            Try
                For Each m As ModeloVentana In New ModeloVentana().ListarActivos()
                    modelosVentana.Add(m.Nombre)
                    cmbPedido.Items.Add(m.Nombre)
                Next
            Catch
            End Try

            cmbColorAluminio.Items.Clear()
            For Each c As String In New ProductoVariante().ListarColoresAluminio()
                cmbColorAluminio.Items.Add(c)
            Next

            cmbTipoVidrio.Items.Clear()
            For Each t As String In New Producto().ListarTiposPorCategoria("VIDRIO")
                cmbTipoVidrio.Items.Add(t)
            Next
        Catch
        End Try

        cmbColorVidrio.Enabled = False
        txtEspesor.Enabled = False
        cmbTipoAluminio.Enabled = False
    End Sub

    ' ================== MODO SEGUN EL TIPO DE PEDIDO ==================

    Private Function ModoPedido() As String
        If cmbPedido.SelectedItem Is Nothing Then Return ""
        Return cmbPedido.SelectedItem.ToString().Trim().ToUpper()
    End Function

    ' Ventana: oculta tipo de aluminio (queda solo el color).
    ' VIDRIO: oculta todo el aluminio. ALUMINIO: oculta el vidrio y el alto.
    Private Sub AplicarVisibilidadPorPedido()
        Dim sel As String = ModoPedido()
        Dim esVentana As Boolean = sel = "VENTANA" OrElse modelosVentana.Contains(sel)
        Dim esVidrio As Boolean = sel = "VIDRIO"
        Dim esAluminio As Boolean = sel = "ALUMINIO"

        ' --- Aluminio ---
        cmbColorAluminio.Visible = Not esVidrio
        cmbColorAluminio.Enabled = Not esVidrio
        Label2.Visible = Not esVidrio
        Label2.Enabled = Not esVidrio

        cmbTipoAluminio.Visible = Not (esVidrio OrElse esVentana)
        cmbTipoAluminio.Enabled = Not (esVidrio OrElse esVentana)
        Label5.Visible = Not (esVidrio OrElse esVentana)
        Label5.Enabled = Not (esVidrio OrElse esVentana)

        ' --- Vidrio ---
        For Each c As Control In New Control() {cmbTipoVidrio, cmbColorVidrio, txtEspesor, Label3, Label17, Label6, Label7}
            c.Visible = Not esAluminio
            c.Enabled = Not esAluminio
        Next

        ' --- Alto (no aplica a ALUMINIO, que se cobra por ancho) ---
        For Each c As Control In New Control() {txtAlto, Label12, Label11}
            c.Visible = Not esAluminio
            c.Enabled = Not esAluminio
        Next

        ' --- Modos con colecciones propias de color de aluminio ---
        If esVentana Then
            PoblarColorAluminioEstandar()
        ElseIf esAluminio Then
            PoblarAluminioDesdeProductos()
            txtExtra.Text = "0.00"
        End If
    End Sub

    ' Coleccion de colores de aluminio usada en las ventanas (igual que frmRegistrarPedido)
    Private Sub PoblarColorAluminioEstandar()
        cmbColorAluminio.Items.Clear()
        cmbColorAluminio.Items.Add("Estandar")
        cmbColorAluminio.Items.Add("Negro")
        cmbColorAluminio.Items.Add("Madera")
        cmbColorAluminio.SelectedIndex = -1
    End Sub

    ' Coloca todos los colores y tipos de aluminio tomados de la tabla productos
    Private Sub PoblarAluminioDesdeProductos()
        cmbColorAluminio.Items.Clear()
        cmbTipoAluminio.Items.Clear()
        cmbColorAluminio.SelectedIndex = -1
        cmbTipoAluminio.SelectedIndex = -1
        Try
            For Each c As String In New Producto().ListarColoresPorCategoria("ALUMINIO")
                cmbColorAluminio.Items.Add(c)
            Next
            For Each t As String In New Producto().ListarTiposPorCategoria("ALUMINIO")
                cmbTipoAluminio.Items.Add(t)
            Next
        Catch
        End Try
    End Sub

    Private Sub cmbPedido_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPedido.SelectedIndexChanged
        AplicarVisibilidadPorPedido()
        Recalcular()
    End Sub

    ' Cascada tipo de vidrio -> color de vidrio -> espesor
    Private Sub cmbTipoVidrio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoVidrio.SelectedIndexChanged
        cmbColorVidrio.Items.Clear()
        cmbColorVidrio.SelectedIndex = -1
        cmbColorVidrio.Enabled = False
        txtEspesor.Items.Clear()
        txtEspesor.SelectedIndex = -1
        txtEspesor.Enabled = False

        If cmbTipoVidrio.SelectedIndex >= 0 Then
            Try
                For Each c As String In New Producto().ListarColoresPorTipo("VIDRIO", cmbTipoVidrio.SelectedItem.ToString())
                    cmbColorVidrio.Items.Add(c)
                Next
            Catch
            End Try
            cmbColorVidrio.Enabled = cmbColorVidrio.Items.Count > 0
        End If
        Recalcular()
    End Sub

    Private Sub cmbColorVidrio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbColorVidrio.SelectedIndexChanged
        txtEspesor.Items.Clear()
        txtEspesor.SelectedIndex = -1
        txtEspesor.Enabled = False

        If cmbColorVidrio.SelectedIndex >= 0 AndAlso cmbTipoVidrio.SelectedIndex >= 0 Then
            Try
                For Each esp As Decimal In New ProductoVariante().ListarEspesoresVidrio(cmbTipoVidrio.SelectedItem.ToString(), cmbColorVidrio.SelectedItem.ToString())
                    txtEspesor.Items.Add(esp)
                Next
            Catch
            End Try
            txtEspesor.Enabled = txtEspesor.Items.Count > 0
        End If
        Recalcular()
    End Sub

    ' Cascada color de aluminio -> tipo de aluminio (solo en modo generico;
    ' Ventana oculta el tipo y ALUMINIO ya trae todos los tipos)
    Private Sub cmbColorAluminio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbColorAluminio.SelectedIndexChanged
        Dim sel As String = ModoPedido()
        If sel = "ALUMINIO" OrElse sel = "VENTANA" OrElse modelosVentana.Contains(sel) Then
            Recalcular()
            Return
        End If

        cmbTipoAluminio.Items.Clear()
        cmbTipoAluminio.SelectedIndex = -1
        cmbTipoAluminio.Enabled = False

        If cmbColorAluminio.SelectedIndex >= 0 Then
            Try
                For Each t As String In New ProductoVariante().ListarTiposAluminio(cmbColorAluminio.SelectedItem.ToString())
                    cmbTipoAluminio.Items.Add(t)
                Next
            Catch
            End Try
            cmbTipoAluminio.Enabled = cmbTipoAluminio.Items.Count > 0
        End If
        Recalcular()
    End Sub

    Private Sub cmbTipoAluminio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoAluminio.SelectedIndexChanged
        Recalcular()
    End Sub

    Private Sub CamposCalculo_Changed(sender As Object, e As EventArgs) Handles txtAncho.TextChanged, txtAlto.TextChanged, txtEspesor.SelectedIndexChanged
        Recalcular()
    End Sub

    Private Sub CamposTotal_Changed(sender As Object, e As EventArgs) Handles txtExtra.TextChanged, txtManoObra.TextChanged
        RecalcularTotales()
    End Sub

    ' Al salir de un campo numerico se normaliza a dos decimales
    Private Sub CampoNumerico_Leave(sender As Object, e As EventArgs) Handles txtAncho.Leave, txtAlto.Leave, txtManoObra.Leave, txtExtra.Leave
        Dim txt As TextBox = TryCast(sender, TextBox)
        If txt Is Nothing Then Return
        Dim valor As Decimal
        If Decimal.TryParse(txt.Text.Trim(), valor) Then
            txt.Text = valor.ToString("0.00")
        End If
    End Sub

    ' ======================= CALCULOS =======================

    Private Sub Recalcular()
        Try
            If ModoPedido() = "ALUMINIO" Then
                ' ALUMINIO: material = precio_unitario de la variante * ancho; sin extra sugerido
                txtPreciomaterial.Text = MaterialAluminio().ToString("0.00")
            Else
                Dim ancho As Decimal = LeerDecimal(txtAncho)
                Dim alto As Decimal = LeerDecimal(txtAlto)

                Dim precioUnitario As Decimal = PrecioVidrio()
                Dim material As Decimal = 0D
                If ancho > 0D AndAlso alto > 0D AndAlso precioUnitario > 0D Then
                    material = alto * ancho / 900D * precioUnitario
                End If
                txtPreciomaterial.Text = material.ToString("0.00")

                txtExtra.Text = ExtraSugerido(ancho, alto).ToString("0.00")
            End If
            RecalcularTotales()
        Catch
        End Try
    End Sub

    ' Material de aluminio: precio_unitario de producto_variante * ancho
    Private Function MaterialAluminio() As Decimal
        Dim color As String = TextoCombo(cmbColorAluminio)
        Dim tipo As String = TextoCombo(cmbTipoAluminio)
        If color = "" OrElse tipo = "" Then Return 0D
        Try
            Dim r = New ProductoVariante().ResolverVarianteAluminio(color, tipo)
            Return r.precioUnitario * LeerDecimal(txtAncho)
        Catch
            Return 0D
        End Try
    End Function

    ' El total se muestra como soles enteros (aproximado)
    Private Sub RecalcularTotales()
        Try
            Dim total As Decimal = LeerDecimal(txtExtra) + LeerDecimal(txtManoObra) + LeerDecimal(txtPreciomaterial)
            txtTotal.Text = RedondearEntero(total).ToString("0")
        Catch
        End Try
    End Sub

    Private Function PrecioVidrio() As Decimal
        If cmbTipoVidrio.SelectedIndex < 0 OrElse cmbColorVidrio.SelectedIndex < 0 OrElse txtEspesor.SelectedIndex < 0 Then Return 0D
        Try
            Dim r = New ProductoVariante().ResolverVarianteVidrio(
                cmbTipoVidrio.SelectedItem.ToString(), cmbColorVidrio.SelectedItem.ToString(), Convert.ToDecimal(txtEspesor.SelectedItem))
            Return r.precioUnitario
        Catch
            Return 0D
        End Try
    End Function

    Private Function ExtraSugerido(ancho As Decimal, alto As Decimal) As Decimal
        Dim extra As Decimal = 0D
        If ancho > 0D AndAlso alto > 0D Then
            Dim tarifa = New Pedido().TarifaSugerida(ancho, alto)
            extra = tarifa.aluminio + tarifa.silicona
        End If

        Dim color As String = TextoCombo(cmbColorAluminio)
        If color.Equals("NEGRO", StringComparison.OrdinalIgnoreCase) Then
            extra *= 1.02D
        ElseIf color.Equals("MADERA", StringComparison.OrdinalIgnoreCase) Then
            extra *= 1.05D
        End If
        Return extra
    End Function

    Private Function TextoCombo(cmb As ComboBox) As String
        If cmb Is Nothing OrElse cmb.SelectedItem Is Nothing Then Return ""
        Return cmb.SelectedItem.ToString()
    End Function

    Private Function LeerDecimal(txt As TextBox) As Decimal
        Dim valor As Decimal
        If txt Is Nothing OrElse txt.Text.Trim() = "" OrElse Not Decimal.TryParse(txt.Text.Trim(), valor) Then Return 0D
        Return valor
    End Function

    ' Abre el registro de pedidos con una pestaña ya cargada con los datos del cotizador
    Private Sub btnLlevarPedido_Click(sender As Object, e As EventArgs) Handles btnLlevarPedido.Click
        If cmbPedido.SelectedIndex < 0 Then
            UtilFormularios.MostrarError("Faltan datos: seleccione el tipo de pedido.")
            Return
        End If

        Dim esAluminio As Boolean = ModoPedido() = "ALUMINIO"

        If LeerDecimal(txtAncho) <= 0D Then
            UtilFormularios.MostrarError(If(esAluminio, "Faltan datos: ingrese el largo del aluminio.", "Faltan datos: ingrese el ancho y el alto del pedido."))
            Return
        End If

        Dim objFrm As New frmRegistrarPedido With {
            .tema = tema,
            .idUsuario = idUsuario,
            .tieneCotizacion = True,
            .cotSeleccion = cmbPedido.SelectedItem.ToString(),
            .cotAncho = LeerDecimal(txtAncho),
            .cotManoObra = LeerDecimal(txtManoObra),
            .cotExtra = LeerDecimal(txtExtra)}

        If esAluminio Then
            If cmbColorAluminio.SelectedIndex < 0 OrElse cmbTipoAluminio.SelectedIndex < 0 Then
                UtilFormularios.MostrarError("Faltan datos del aluminio: seleccione color y tipo.")
                Return
            End If
            objFrm.cotTipoAluminio = cmbTipoAluminio.SelectedItem.ToString()
            objFrm.cotColorAluminio = cmbColorAluminio.SelectedItem.ToString()
        Else
            If LeerDecimal(txtAlto) <= 0D Then
                UtilFormularios.MostrarError("Faltan datos: ingrese el ancho y el alto del pedido.")
                Return
            End If
            If cmbTipoVidrio.SelectedIndex < 0 OrElse cmbColorVidrio.SelectedIndex < 0 OrElse txtEspesor.SelectedIndex < 0 Then
                UtilFormularios.MostrarError("Faltan datos del vidrio: seleccione tipo, color y espesor.")
                Return
            End If
            objFrm.cotAlto = LeerDecimal(txtAlto)
            objFrm.cotEspesor = Convert.ToDecimal(txtEspesor.SelectedItem)
            objFrm.cotTipoVidrio = cmbTipoVidrio.SelectedItem.ToString()
            objFrm.cotColorVidrio = cmbColorVidrio.SelectedItem.ToString()
            ' En modo ventana se lleva el color de aluminio elegido (Estandar/Negro/Madera)
            objFrm.cotColorAluminio = TextoCombo(cmbColorAluminio)
        End If

        objFrm.MdiParent = Me.MdiParent
        objFrm.Show()
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Dim objCotV As New frmCotizarVentana With {.tema = tema, .idUsuario = idUsuario}
        objCotV.MdiParent = Me.MdiParent
        objCotV.Show()

        If cmbPedido.SelectedIndex >= 0 Then objCotV.cmbPedido.SelectedItem = cmbPedido.SelectedItem
        If cmbTipoVidrio.SelectedIndex >= 0 Then objCotV.cmbTipoVidrio.SelectedItem = cmbTipoVidrio.SelectedItem
        If cmbColorVidrio.SelectedIndex >= 0 Then objCotV.cmbColorVidrio.SelectedItem = cmbColorVidrio.SelectedItem
        If cmbColorAluminio.SelectedIndex >= 0 Then objCotV.cmbColorAluminio.SelectedItem = cmbColorAluminio.SelectedItem
        If cmbTipoAluminio.SelectedIndex >= 0 Then objCotV.cmbTipoAluminio.SelectedItem = cmbTipoAluminio.SelectedItem
        If txtEspesor.SelectedIndex >= 0 Then objCotV.txtEspesor.SelectedItem = txtEspesor.SelectedItem

        objCotV.txtAncho.Text = Me.txtAncho.Text
        objCotV.txtAlto.Text = Me.txtAlto.Text
        objCotV.txtManoObra.Text = Me.txtManoObra.Text
        objCotV.txtExtra.Text = Me.txtExtra.Text
    End Sub
End Class

Imports capaNegocio
Imports System.Data

Public Class frmPedidoUnitario
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Public Property idPedido As Integer = 0
    Public Property idUsuario As Integer = 0

    Dim objPedido As New Pedido
    Dim objPieza As New InventarioPieza

    ' Plantilla (tabModelo) copiada una sola vez para no depender del control original
    Private tmplLabel1 As Label
    Private tmplTipo As TextBox
    Private tmplLabel2 As Label
    Private tmplPrecio As TextBox
    Private tmplPadding As Padding
    Private tmplBackColor As Color
    Private tmplUseVisual As Boolean
    Private tmplGridLoc As Point
    Private tmplGridSize As Size
    Private tabsGenerados As New List(Of TabPage)

    Private Sub frmPedidoUnitario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CapturarPlantilla()
        tabItems.TabPages.Remove(tabModelo)

        AplicarTema(Me, tema)
        ResolverUsuario()

        Dim p As Paleta = Temas.Obtener(tema)
        Panel1.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
        lblSubTitulo.ForeColor = p.Texto
        lblEstado.ForeColor = p.Texto

        cargar()
        cargarItems()

        ' Los controles de cada pestaña se crean en tiempo de ejecución: se vuelven a tematizar
        AplicarTema(Me, tema)
        Panel1.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Acento
    End Sub

    Private Sub ResolverUsuario()
        If idUsuario > 0 Then Return
        Dim padre As MenuInicio = TryCast(Me.MdiParent, MenuInicio)
        If padre IsNot Nothing Then Integer.TryParse(padre.txtID.Text, idUsuario)
    End Sub

    ' ---------------- Cabecera ----------------
    Private Sub cargar()
        If idPedido <= 0 Then Return
        Try
            Dim dt As DataTable = objPedido.Obtener(idPedido)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return
            Dim fila As DataRow = dt.Rows(0)

            Dim cliente As String = fila("cliente").ToString()
            Dim usuario As String = fila("usuario").ToString()
            Dim fecha As DateTime = CDate(fila("fecha"))
            Dim total As Decimal = CDec(fila("total"))

            Text = "Pedido #" & idPedido.ToString()
            lblTitulo.Text = "Pedido #" & idPedido.ToString() & " - " & cliente
            lblSubTitulo.Text = fecha.ToString("dd/MM/yyyy HH:mm") & " - Vendedor: " & usuario &
                                " - Total: S/." & FormatoNumero(total)
            lblEstado.Text = fila("estado").ToString()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Pedido", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ---------------- Items ----------------
    Private Sub cargarItems()
        For Each t As TabPage In tabsGenerados
            tabItems.TabPages.Remove(t)
        Next
        tabsGenerados.Clear()

        If idPedido <= 0 Then Return
        Try
            Dim items As DataTable = objPedido.ListarItemsDetallado(idPedido)
            For Each fila As DataRow In items.Rows
                Dim idItem As Integer = CInt(fila("id_item"))
                Dim detalles As DataTable = objPedido.ListarDetallesDeItem(idItem)
                Dim tab As TabPage = CrearTab(fila, detalles)
                tabItems.TabPages.Add(tab)
                tabsGenerados.Add(tab)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Pedido", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function CrearTab(item As DataRow, detalles As DataTable) As TabPage
        Dim descripcion As String = item("descripcion").ToString()
        If descripcion.Trim() = "" Then descripcion = item("tipo").ToString()

        Dim tab As New TabPage()
        tab.Padding = tmplPadding
        tab.BackColor = tmplBackColor
        tab.UseVisualStyleBackColor = tmplUseVisual
        tab.Text = descripcion

        Dim lbl1 As Label = CopiarLabel(tmplLabel1)
        Dim txtTipo As TextBox = CopiarTexto(tmplTipo)
        Dim lbl2 As Label = CopiarLabel(tmplLabel2)
        Dim txtPrecio As TextBox = CopiarTexto(tmplPrecio)
        Dim grid As DataGridView = CrearGrid()

        txtTipo.ReadOnly = True
        txtPrecio.ReadOnly = True

        Dim medida As String = ""
        If detalles IsNot Nothing AndAlso detalles.Rows.Count > 0 Then medida = MedidaDetalle(detalles.Rows(0))
        txtTipo.Text = descripcion & medida
        txtPrecio.Text = FormatoNumero(ADecimal(item("total")))

        tab.Controls.Add(lbl1)
        tab.Controls.Add(txtTipo)
        tab.Controls.Add(lbl2)
        tab.Controls.Add(txtPrecio)
        tab.Controls.Add(grid)

        LlenarGrid(grid, detalles)
        Return tab
    End Function

    Private Function CrearGrid() As DataGridView
        Dim grid As New DataGridView()
        grid.Location = tmplGridLoc
        grid.Size = tmplGridSize
        grid.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        grid.ScrollBars = ScrollBars.Both
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        grid.ReadOnly = True
        grid.AllowUserToAddRows = False
        grid.AllowUserToDeleteRows = False
        grid.RowHeadersVisible = False
        grid.MultiSelect = False
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        grid.Columns.Add("nombre", "Nombre del Item")
        grid.Columns.Add("pieza", "Pieza exacta en stock")
        grid.Columns.Add("estado", "Estado")
        Dim btn As New DataGridViewButtonColumn()
        btn.Name = "btnAccion"
        btn.HeaderText = "Acción"
        btn.FlatStyle = FlatStyle.Flat
        grid.Columns.Add(btn)

        ' Ids y datos ocultos: evitan volver a consultar la base en cada acción
        For Each oculto As String In New String() {"id_detalle", "id_variante", "id_pieza", "accion",
                                                   "ancho", "alto", "largo", "cajas", "id_compra_detalle"}
            grid.Columns.Add(oculto, oculto)
            grid.Columns(oculto).Visible = False
        Next

        grid.Columns("nombre").Width = 260
        grid.Columns("pieza").Width = 150
        grid.Columns("estado").Width = 110
        grid.Columns("btnAccion").Width = 95

        AddHandler grid.CellContentClick, AddressOf gridItem_CellContentClick
        Return grid
    End Function

    Private Sub LlenarGrid(grid As DataGridView, detalles As DataTable)
        grid.Rows.Clear()
        If detalles Is Nothing Then Return
        For Each d As DataRow In detalles.Rows
            Dim accion As String = d("accion").ToString()
            Dim comprado As Boolean = Not IsDBNull(d("id_compra_detalle"))

            Dim idx As Integer = grid.Rows.Add()
            Dim fila As DataGridViewRow = grid.Rows(idx)

            fila.Cells("nombre").Value = d("material").ToString() & MedidaDetalle(d)
            fila.Cells("pieza").Value = PiezaTexto(d, accion)
            fila.Cells("estado").Value = EstadoVisual(accion, comprado)
            fila.Cells("btnAccion").Value = BotonTexto(accion, comprado)

            fila.Cells("id_detalle").Value = d("id_detalle")
            fila.Cells("id_variante").Value = d("id_variante")
            fila.Cells("id_pieza").Value = d("pieza_usada")
            fila.Cells("accion").Value = accion
            fila.Cells("ancho").Value = d("ancho_cm")
            fila.Cells("alto").Value = d("alto_cm")
            fila.Cells("largo").Value = d("largo_cm")
            fila.Cells("cajas").Value = d("cantidad_cajas")
            fila.Cells("id_compra_detalle").Value = d("id_compra_detalle")
        Next
    End Sub

    ' ---------------- Textos de apoyo ----------------
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

    Private Function BotonTexto(accion As String, comprado As Boolean) As String
        Select Case accion
            Case "CORTA_PIEZA"
                Return "Confirmar corte"
            Case "COMPRA_NUEVA"
                Return If(comprado, "", "Comprar Ahora")
            Case Else
                Return ""
        End Select
    End Function

    Private Function PiezaTexto(d As DataRow, accion As String) As String
        If Not IsDBNull(d("pieza_usada")) Then
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
        Dim ancho As Object = d("pieza_ancho")
        Dim alto As Object = d("pieza_alto")
        Dim largo As Object = d("pieza_largo")
        If Not IsDBNull(largo) AndAlso CDec(largo) > 0 Then Return Format(largo, "0.##") & " cm"
        If Not IsDBNull(ancho) AndAlso Not IsDBNull(alto) Then
            Return Format(ancho, "0.##") & " x " & Format(alto, "0.##") & " cm"
        End If
        Return ""
    End Function

    ' " · 120 x 120 cm" (vidrio), " · 480 cm" (aluminio) o " · N unidad(es)/caja(s)"
    Private Function MedidaDetalle(d As DataRow) As String
        If Not IsDBNull(d("largo_cm")) AndAlso CDec(d("largo_cm")) > 0 Then
            Return " · " & Format(d("largo_cm"), "0.##") & " cm"
        End If
        If Not IsDBNull(d("ancho_cm")) AndAlso Not IsDBNull(d("alto_cm")) AndAlso
           CDec(d("ancho_cm")) > 0 AndAlso CDec(d("alto_cm")) > 0 Then
            Return " · " & Format(d("ancho_cm"), "0.##") & " x " & Format(d("alto_cm"), "0.##") & " cm"
        End If
        If Not IsDBNull(d("cantidad_cajas")) Then
            Return " · " & d("cantidad_cajas").ToString() & " " & EtiquetaUnidad(d)
        End If
        Return ""
    End Function

    ' Los accesorios por unidad (p. ej. seguro pistillo, silicona) muestran "unidad(es)"
    Private Function EtiquetaUnidad(d As DataRow) As String
        If d.Table.Columns.Contains("unidad_costo") AndAlso Not IsDBNull(d("unidad_costo")) AndAlso
           d("unidad_costo").ToString().Equals("UNIDAD", StringComparison.OrdinalIgnoreCase) Then
            Return "unidad(es)"
        End If
        Return "caja(s)"
    End Function

    ' ---------------- Acciones de la grilla ----------------
    Private Sub gridItem_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim grid As DataGridView = DirectCast(sender, DataGridView)
        If grid.Columns(e.ColumnIndex).Name <> "btnAccion" Then Return

        Dim fila As DataGridViewRow = grid.Rows(e.RowIndex)
        Dim accion As String = CStr(fila.Cells("accion").Value)

        Select Case accion
            Case "CORTA_PIEZA"
                ConfirmarCorte(fila)
            Case "COMPRA_NUEVA"
                If IsDBNull(fila.Cells("id_compra_detalle").Value) Then ComprarMaterial(fila)
        End Select
    End Sub

    Private Sub ConfirmarCorte(fila As DataGridViewRow)
        Dim idPieza As Integer = CInt(fila.Cells("id_pieza").Value)
        If idPieza <= 0 Then Return
        If Not Confirmar("¿Confirmar el corte de la pieza #" & idPieza.ToString() & "?") Then Return
        Try
            Dim retazos As DataTable = objPieza.Cortar(idPieza,
                                                       MedidaONull(fila.Cells("ancho").Value),
                                                       MedidaONull(fila.Cells("alto").Value),
                                                       MedidaONull(fila.Cells("largo").Value),
                                                       IdUsuarioONull(), IdPedidoONull(), 10D)
            objPedido.ResolverDetalle(CInt(fila.Cells("id_detalle").Value))

            Dim texto As String = "Corte registrado."
            If retazos.Rows.Count = 0 Then
                texto &= vbCrLf & "No quedó ningún retazo útil."
            Else
                texto &= vbCrLf & "Retazos generados:"
                For Each r As DataRow In retazos.Rows
                    texto &= vbCrLf & "  - Pieza #" & r("id_pieza").ToString() & ": " &
                             TextoMedida(r("ancho_cm"), r("alto_cm"), r("largo_cm"))
                Next
            End If
            MostrarInfo(texto)
            cargarItems()
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    Private Sub ComprarMaterial(fila As DataGridViewRow)
        Dim idVariante As Integer = CInt(fila.Cells("id_variante").Value)
        If idVariante <= 0 Then Return
        Dim frm As New frmRegistrarCompras With {.tema = tema, .idVarianteInicial = idVariante}
        frm.MdiParent = Me.MdiParent
        frm.Show()
    End Sub

    ' ---------------- Utilidades ----------------
    Private Sub CapturarPlantilla()
        tmplLabel1 = CopiarLabel(TryCast(tabModelo.Controls("Label1"), Label))
        tmplTipo = CopiarTexto(TryCast(tabModelo.Controls("txtTipoPedido"), TextBox))
        tmplLabel2 = CopiarLabel(TryCast(tabModelo.Controls("Label2"), Label))
        tmplPrecio = CopiarTexto(TryCast(tabModelo.Controls("txtPrecio"), TextBox))

        tmplPadding = tabModelo.Padding
        tmplBackColor = tabModelo.BackColor
        tmplUseVisual = tabModelo.UseVisualStyleBackColor

        tmplGridLoc = tblItems.Location
        tmplGridSize = tblItems.Size
    End Sub

    Private Function CopiarLabel(origen As Label) As Label
        Dim copia As New Label()
        If origen Is Nothing Then Return copia
        copia.Text = origen.Text
        copia.Location = origen.Location
        copia.Size = origen.Size
        copia.AutoSize = origen.AutoSize
        copia.Font = origen.Font
        Return copia
    End Function

    Private Function CopiarTexto(origen As TextBox) As TextBox
        Dim copia As New TextBox()
        If origen Is Nothing Then Return copia
        copia.Location = origen.Location
        copia.Size = origen.Size
        copia.TextAlign = origen.TextAlign
        copia.Font = origen.Font
        Return copia
    End Function

    Private Function ADecimal(valor As Object) As Decimal
        If valor Is Nothing OrElse IsDBNull(valor) Then Return 0D
        Return Convert.ToDecimal(valor)
    End Function

    Private Function MedidaONull(valor As Object) As Decimal?
        If valor Is Nothing OrElse IsDBNull(valor) Then Return Nothing
        Dim v As Decimal
        If Not Decimal.TryParse(valor.ToString(), v) Then Return Nothing
        If v <= 0D Then Return Nothing
        Return v
    End Function

    Private Function IdUsuarioONull() As Integer?
        Return If(idUsuario > 0, CType(idUsuario, Integer?), Nothing)
    End Function

    Private Function IdPedidoONull() As Integer?
        Return If(idPedido > 0, CType(idPedido, Integer?), Nothing)
    End Function

    Private Function FormatoNumero(valor As Decimal) As String
        Return valor.ToString("0.00", Globalization.CultureInfo.InvariantCulture)
    End Function
End Class

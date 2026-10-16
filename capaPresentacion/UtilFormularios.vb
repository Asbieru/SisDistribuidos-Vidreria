' Ventanas de consulta del inventario que se pueden volver a cargar desde fuera
' (p. ej. al reabrirlas desde el resumen, para no mostrar datos antiguos)
Public Interface IListaInventario
    Sub cargarLista()
End Interface

' Funciones comunes que usan los formularios de inventario
Module UtilFormularios
    Public Const TODOS As String = "(Todos)"

    ' ---------------- Ventanas dentro del menu (MDI) ----------------

    Public Sub ComoVentanaHija(frm As Form)
        If frm Is Nothing Then Return
        frm.MinimizeBox = True
        frm.MaximizeBox = (frm.FormBorderStyle = FormBorderStyle.Sizable)
    End Sub

    ' Se ajusta una sola vez al abrir; al cambiar de ventana se conserva su posición.
    Public Sub PrepararVentana(frm As Form)
        RemoveHandler frm.Shown, AddressOf VentanaMostrada
        AddHandler frm.Shown, AddressOf VentanaMostrada
    End Sub

    Private Sub VentanaMostrada(sender As Object, e As EventArgs)
        AjustarVentana(DirectCast(sender, Form), True)
    End Sub

    Public Sub AjustarVentana(frm As Form, Optional centrar As Boolean = False)
        If frm Is Nothing OrElse frm.WindowState <> FormWindowState.Normal Then Return
        Dim area As Rectangle = Screen.FromControl(frm).WorkingArea
        If frm.MdiParent IsNot Nothing Then
            Dim escritorio As MdiClient = frm.MdiParent.Controls.OfType(Of MdiClient)().FirstOrDefault()
            If escritorio Is Nothing OrElse escritorio.ClientSize.Width < 100 Then Return
            area = escritorio.ClientRectangle
        End If
        area.Inflate(-8, -8)
        Dim ancho As Integer = Math.Min(frm.Width, area.Width)
        Dim alto As Integer = Math.Min(frm.Height, area.Height)
        If ancho < frm.Width OrElse alto < frm.Height Then
            If frm.FormBorderStyle <> FormBorderStyle.Sizable Then
                ' Los diálogos conservan el tamaño de sus campos y permiten desplazarse.
                frm.AutoScroll = True
                frm.AutoScrollMinSize = frm.ClientSize
                For Each panel As Panel In frm.Controls.OfType(Of Panel)()
                    If panel.Dock = DockStyle.Fill Then panel.MinimumSize = panel.Size
                Next
            End If
            frm.MinimumSize = New Size(Math.Min(frm.MinimumSize.Width, ancho), Math.Min(frm.MinimumSize.Height, alto))
            frm.Size = New Size(ancho, alto)
        End If
        If centrar Then
            frm.Location = New Point(area.Left + (area.Width - frm.Width) \ 2,
                                     area.Top + (area.Height - frm.Height) \ 2)
        Else
            frm.Location = New Point(Math.Max(area.Left, Math.Min(frm.Left, area.Right - frm.Width)),
                                     Math.Max(area.Top, Math.Min(frm.Top, area.Bottom - frm.Height)))
        End If
    End Sub

    ' Aproxima un monto a soles enteros (redondeo comercial: el .5 sube)
    Public Function RedondearEntero(valor As Decimal) As Decimal
        Return Math.Round(valor, 0, MidpointRounding.AwayFromZero)
    End Function

    ' ---------------- Tema claro / oscuro ----------------
    Public Sub AplicarTema(frm As Form, tema As String)
        Dim p As Paleta = Temas.Obtener(tema)
        frm.BackColor = p.Fondo
        AplicarTemaControles(frm.Controls, p)
        PrepararVentana(frm)
    End Sub

    ' Los botones con estilo del sistema ignoran el BackColor y dibujan un borde
    ' que no coincide con el: se pasan a plano y se pinta el borde del mismo color
    ' de fondo, quedando un borde que coincide con el color elegido.
    Public Sub AplicarEstiloBoton(btn As Button, p As Paleta)
        btn.UseVisualStyleBackColor = False
        btn.FlatStyle = FlatStyle.Flat
        btn.BackColor = p.BotonFondo
        btn.ForeColor = p.BotonTexto
        btn.FlatAppearance.BorderColor = p.BotonFondo
        btn.FlatAppearance.BorderSize = 1
        btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(p.BotonFondo, 0.25F)
        btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(p.BotonFondo, 0.1F)
    End Sub

    ' Los campos de texto traen un borde 3D que no coincide con el fondo: se deja
    ' un borde simple y se colorean con el tema (fondo y texto).
    Public Sub AplicarEstiloCampo(campo As Control, p As Paleta)
        campo.BackColor = p.Fondo
        campo.ForeColor = p.Texto
        If TypeOf campo Is TextBoxBase Then
            DirectCast(campo, TextBoxBase).BorderStyle = BorderStyle.FixedSingle
        ElseIf TypeOf campo Is NumericUpDown Then
            DirectCast(campo, NumericUpDown).BorderStyle = BorderStyle.FixedSingle
        End If
    End Sub

    Private Sub AplicarTemaControles(controles As Control.ControlCollection, p As Paleta)
        For Each c As Control In controles
            If TypeOf c Is Button Then
                AplicarEstiloBoton(DirectCast(c, Button), p)
            ElseIf TypeOf c Is ComboBox Then
                Dim cb As ComboBox = DirectCast(c, ComboBox)
                cb.FlatStyle = FlatStyle.Flat
                cb.BackColor = p.Fondo
                cb.ForeColor = p.Texto
            ElseIf TypeOf c Is TextBoxBase OrElse TypeOf c Is NumericUpDown Then
                AplicarEstiloCampo(c, p)
            ElseIf TypeOf c Is DataGridView Then
                Dim dgv As DataGridView = DirectCast(c, DataGridView)
                dgv.BackgroundColor = p.Fondo
                dgv.BorderStyle = BorderStyle.FixedSingle
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                dgv.GridColor = p.FondoPanel
                dgv.Font = New Font("Microsoft Sans Serif", 9.75F)
                dgv.DefaultCellStyle.Font = dgv.Font
                dgv.DefaultCellStyle.BackColor = p.Fondo
                dgv.DefaultCellStyle.ForeColor = p.Texto
                dgv.DefaultCellStyle.SelectionBackColor = p.BotonFondo
                dgv.DefaultCellStyle.SelectionForeColor = p.BotonTexto
                dgv.DefaultCellStyle.Padding = New Padding(4, 2, 4, 2)
                dgv.ColumnHeadersDefaultCellStyle.BackColor = p.BotonFondo
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = p.BotonTexto
                dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = p.BotonFondo
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = p.BotonTexto
                dgv.ColumnHeadersHeight = Math.Max(32, dgv.Font.Height + 14)
                dgv.RowTemplate.Height = Math.Max(28, dgv.Font.Height + 10)
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                dgv.ScrollBars = ScrollBars.Both
                dgv.EnableHeadersVisualStyles = False
            ElseIf TypeOf c Is Label Then
                c.ForeColor = If(c.Font.SizeInPoints >= 14, p.Acento, p.Texto)
            ElseIf TypeOf c Is CheckBox OrElse TypeOf c Is RadioButton OrElse TypeOf c Is GroupBox Then
                c.ForeColor = p.Texto
            ElseIf TypeOf c Is ListView Then
                c.BackColor = p.Fondo
                c.ForeColor = p.Texto
            ElseIf TypeOf c Is TabPage Then
                DirectCast(c, TabPage).UseVisualStyleBackColor = False
                c.BackColor = p.Fondo
                c.ForeColor = p.Texto
            ElseIf TypeOf c Is Panel Then
                c.BackColor = If(c.Dock = DockStyle.Fill, p.Fondo, p.FondoPanel)
            End If
            If c.HasChildren Then AplicarTemaControles(c.Controls, p)
        Next
    End Sub

    Public Function ColorAlerta(tema As String) As Color
        Return If(Temas.EsOscuro(tema), Color.LightCoral, Color.DarkRed)
    End Function

    ' Fondo de las filas que están en o por debajo del stock mínimo
    Public Function ColorBajoMinimo(tema As String) As Color
        Return If(Temas.EsOscuro(tema), Color.FromArgb(100, 30, 30), Color.MistyRose)
    End Function

    ' ---------------- Grillas ----------------

    ' Pone títulos a las columnas indicadas ("columna|Título") y oculta las demás
    Public Sub Encabezados(dgv As DataGridView, ParamArray pares() As String)
        For Each col As DataGridViewColumn In dgv.Columns
            col.Visible = False
        Next
        For i As Integer = 0 To pares.Length - 1
            Dim partes() As String = pares(i).Split("|"c)
            If dgv.Columns.Contains(partes(0)) Then
                With dgv.Columns(partes(0))
                    .HeaderText = partes(1)
                    .Visible = True
                    .DisplayIndex = i
                End With
            End If
        Next
    End Sub

    ' Pinta la fila si su columna "bajo_minimo" es verdadera
    Public Sub PintarBajoMinimo(dgv As DataGridView, e As DataGridViewCellFormattingEventArgs, tema As String)
        If e.RowIndex < 0 OrElse Not dgv.Columns.Contains("bajo_minimo") Then Return
        Dim valor As Object = dgv.Rows(e.RowIndex).Cells("bajo_minimo").Value
        If valor IsNot Nothing AndAlso Not IsDBNull(valor) AndAlso CBool(valor) Then
            e.CellStyle.BackColor = ColorBajoMinimo(tema)
        End If
    End Sub

    Public Sub Reseleccionar(dgv As DataGridView, indice As Integer)
        If indice >= 0 AndAlso indice < dgv.Rows.Count Then
            Dim primera As DataGridViewColumn = dgv.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
            If primera IsNot Nothing Then dgv.CurrentCell = dgv.Rows(indice).Cells(primera.Index)
        End If
    End Sub

    ' En modo Fill las columnas se reparten el ancho disponible y se comprimen
    ' hasta ser ilegibles (nunca aparece la barra horizontal). Fijando un ancho
    ' minimo por columna, si el total no cabe se muestra el scroll horizontal.
    Public Sub AnchoMinimoColumnas(dgv As DataGridView, minimo As Integer)
        For Each col As DataGridViewColumn In dgv.Columns
            If col.Visible Then col.MinimumWidth = Math.Max(minimo, col.MinimumWidth)
        Next
    End Sub

    ' ---------------- Combos de productos ----------------

    ' Llena un combo con variantes; opcionalmente agrega "(Todos)" con id 0 al inicio
    Public Sub CargarComboVariantes(cmb As ComboBox, dt As DataTable, incluirTodos As Boolean)
        If incluirTodos Then
            Dim fila As DataRow = dt.NewRow()
            fila("id_variante") = 0
            fila("producto") = TODOS
            fila("unidad_costo") = ""
            dt.Rows.InsertAt(fila, 0)
        End If
        cmb.DisplayMember = "producto"
        cmb.ValueMember = "id_variante"
        cmb.DataSource = dt
    End Sub

    ' Id de la variante elegida, o Nothing si es "(Todos)" o no hay selección
    Public Function VarianteElegida(cmb As ComboBox) As Integer?
        If cmb.SelectedValue Is Nothing OrElse TypeOf cmb.SelectedValue Is DataRowView Then Return Nothing
        Dim id As Integer = CInt(cmb.SelectedValue)
        If id = 0 Then Return Nothing
        Return id
    End Function

    ' CM2 (vidrio), CM_LINEAL (aluminio), CAJA, o "" si no hay selección
    Public Function UnidadElegida(cmb As ComboBox) As String
        Dim fila As DataRowView = TryCast(cmb.SelectedItem, DataRowView)
        If fila Is Nothing Then Return ""
        Return fila("unidad_costo").ToString()
    End Function

    ' ---------------- Medidas ----------------

    ' Valor del control, o Nothing si está deshabilitado o en cero
    Public Function Medida(nud As NumericUpDown) As Decimal?
        If Not nud.Enabled OrElse nud.Value <= 0 Then Return Nothing
        Return nud.Value
    End Function

    ' "180 x 120 cm" para vidrio, "600 cm" para aluminio
    Public Function TextoMedida(ancho As Object, alto As Object, largo As Object) As String
        If largo Is Nothing OrElse IsDBNull(largo) Then
            Return Format(ancho, "0.##") & " x " & Format(alto, "0.##") & " cm"
        End If
        Return Format(largo, "0.##") & " cm"
    End Function

    ' ---------------- Mensajes ----------------
    Public Sub MostrarError(texto As String)
        MessageBox.Show(texto, "Inventario", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub MostrarError(ex As Exception)
        MostrarError(ex.Message)
    End Sub

    Public Sub MostrarAviso(texto As String)
        MessageBox.Show(texto, "Inventario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Public Sub MostrarInfo(texto As String)
        MessageBox.Show(texto, "Inventario", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Function Confirmar(texto As String) As Boolean
        Return MessageBox.Show(texto, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
    End Function
End Module

Imports capaNegocio
Imports System.Data
Imports System.Linq
Public Class frmRegistrarPedido
    Implements IFormularioTema
    Public Property idClienteSeleccionado As Integer = 0
    Public Property nombreClienteSeleccionado As String = ""
    Public Property idUsuario As Integer = 0
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    Private contadores As New Dictionary(Of String, Integer) From {
        {"VENTANA", 0}, {"MAMPARA", 0}, {"VIDRIO_SUELTO", 0}, {"CAJA", 0}
    }
    Private itemsPedido As New List(Of ItemPedidoDTO)
    Private contadorVidrio As Integer = 0
    Private contadorVentana As Integer = 0

    Private Sub actualizarCliente()
        txtNombreCliente.Text = nombreClienteSeleccionado
    End Sub

    Private Sub frmRegistrarPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        tabsPedidos.TabPages.Remove(tabModeloVidrio)
        tabsPedidos.TabPages.Remove(tabModeloVentana)
        AplicarVisibilidadMedidas(chbAncho)
        AplicarVisibilidadMedidas(chbAlto)
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
            MessageBox.Show("Seleccione un tipo de pedido.")
            Return
        End If

        Dim tipo As String = cmbPedido.SelectedItem.ToString()

        If tipo = "Vidrio" Then
            contadorVidrio += 1
            Dim nueva As TabPage = ClonarTabPage(tabModeloVidrio)
            nueva.Name = tipo & contadorVidrio.ToString()
            nueva.Text = tipo & contadorVidrio.ToString()
            tabsPedidos.TabPages.Add(nueva)
            tabsPedidos.SelectedTab = nueva
        ElseIf tipo = "Ventana" Then
            contadorVentana += 1
            Dim nueva As TabPage = ClonarTabPage(tabModeloVentana)
            nueva.Name = tipo & contadorVentana.ToString()
            nueva.Text = tipo & contadorVentana.ToString()
            tabsPedidos.TabPages.Add(nueva)
            tabsPedidos.SelectedTab = nueva
        End If
    End Sub

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

        Dim chbOrigen As CheckBox = TryCast(copia, CheckBox)
        If chbOrigen IsNot Nothing AndAlso
            (chbOrigen.Name = "chbAncho" OrElse chbOrigen.Name = "chbAlto") Then
            AddHandler chbOrigen.CheckedChanged, AddressOf chbMedida_CheckedChanged
        End If

        For Each hijo As Control In origen.Controls
            copia.Controls.Add(ClonarControl(hijo))
        Next

        Return copia
    End Function

    Private Sub btnGuardarPedido_Click(sender As Object, e As EventArgs) Handles btnGuardarPedido.Click
        If idClienteSeleccionado = 0 Then
            UtilFormularios.MostrarAviso("Selecciona un cliente.") : Exit Sub
        End If
        If itemsPedido.Count = 0 Then
            UtilFormularios.MostrarAviso("Agrega al menos un item con 'Llevar a pedido'.") : Exit Sub
        End If

        Dim idPedido = New Pedido().Registrar(idClienteSeleccionado, idUsuario, itemsPedido)
        UtilFormularios.MostrarInfo($"Pedido #{idPedido} registrado.")
        itemsPedido.Clear()
        tabsPedidos.TabPages.Clear()
    End Sub

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
End Class
Imports System.Data
Imports System.Globalization
Imports capaNegocio

Partial Class wfRegistrarPieza
    Inherits System.Web.UI.Page

    Private ReadOnly objInventario As New Inventario()
    Private ReadOnly objPieza As New InventarioPieza()

    ' Unidad de costo de la variante seleccionada (CM2, PIE2 o CM_LINEAL)
    Private Property UnidadActual As String
        Get
            Return If(ViewState("unidad"), "").ToString()
        End Get
        Set(value As String)
            ViewState("unidad") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarVariantes()
            CargarListado()
        End If

        ActualizarCampos()
    End Sub

    ' ------------------------- Variantes y campos -------------------------

    Private Sub CargarVariantes()
        Try
            Dim dt As DataTable = objInventario.ListarVariantes(True)
            ddlVariante.DataSource = dt
            ddlVariante.DataTextField = "producto"
            ddlVariante.DataValueField = "id_variante"
            ddlVariante.DataBind()

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                UnidadActual = dt.Rows(0)("unidad_costo").ToString()
            Else
                UnidadActual = ""
                MostrarMensaje("No hay productos de vidrio o aluminio registrados.", "mensaje-error")
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub ddlVariante_SelectedIndexChanged(sender As Object, e As EventArgs)
        UnidadActual = UnidadDeVariante(ddlVariante.SelectedValue)
        ActualizarCampos()
    End Sub

    Private Function UnidadDeVariante(idVariante As String) As String
        Try
            Dim dt As DataTable = objInventario.ListarVariantes(True)
            For Each fila As DataRow In dt.Rows
                If fila("id_variante").ToString() = idVariante Then
                    Return fila("unidad_costo").ToString()
                End If
            Next
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
        Return ""
    End Function

    ' El vidrio usa ancho y alto; el aluminio usa largo.
    Private Sub ActualizarCampos()
        Dim esVidrio As Boolean = Inventario.EsVidrio(UnidadActual)
        Dim esLineal As Boolean = UnidadActual = "CM_LINEAL"

        txtAncho.Enabled = esVidrio
        txtAlto.Enabled = esVidrio
        txtLargo.Enabled = esLineal

        If esVidrio Then
            lblAyuda.Text = "Vidrio: ingrese ancho y alto en centímetros."
        ElseIf esLineal Then
            lblAyuda.Text = "Aluminio: ingrese el largo en centímetros lineales."
        Else
            lblAyuda.Text = "Seleccione un producto de vidrio o aluminio."
        End If
    End Sub

    ' ------------------------- Guardar -------------------------

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        Dim idVariante As Integer
        If Not Integer.TryParse(ddlVariante.SelectedValue, idVariante) OrElse idVariante <= 0 Then
            MostrarMensaje("No hay productos de vidrio o aluminio registrados, o no eligió ninguno.", "mensaje-error")
            Return
        End If

        Dim esVidrio As Boolean = Inventario.EsVidrio(UnidadActual)
        Dim ancho As Decimal? = Nothing
        Dim alto As Decimal? = Nothing
        Dim largo As Decimal? = Nothing

        If esVidrio Then
            If Not TryParseMedida(txtAncho.Text, ancho) OrElse ancho.GetValueOrDefault() <= 0D Then
                MostrarMensaje("Ingrese un ancho válido mayor a cero.", "mensaje-error")
                Return
            End If
            If Not TryParseMedida(txtAlto.Text, alto) OrElse alto.GetValueOrDefault() <= 0D Then
                MostrarMensaje("Ingrese un alto válido mayor a cero.", "mensaje-error")
                Return
            End If
        ElseIf UnidadActual = "CM_LINEAL" Then
            If Not TryParseMedida(txtLargo.Text, largo) OrElse largo.GetValueOrDefault() <= 0D Then
                MostrarMensaje("Ingrese un largo válido mayor a cero.", "mensaje-error")
                Return
            End If
        Else
            MostrarMensaje("Seleccione un producto de vidrio o aluminio.", "mensaje-error")
            Return
        End If

        Try
            Dim idUsuario As Integer? = Nothing
            Dim idSesion As Integer
            If Session("idUsuario") IsNot Nothing AndAlso Integer.TryParse(Session("idUsuario").ToString(), idSesion) Then
                idUsuario = idSesion
            End If

            Dim idPieza As Integer = objPieza.InsertarInicial(idVariante, ancho, alto, largo,
                                                              idUsuario, txtObservacion.Text)

            MostrarMensaje("Pieza #" & idPieza.ToString() & " registrada en el inventario.", "mensaje-ok")
            LimpiarCampos()
            CargarListado()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs)
        LimpiarCampos()
        MostrarMensaje("", "")
    End Sub

    Private Sub LimpiarCampos()
        txtAncho.Text = ""
        txtAlto.Text = ""
        txtLargo.Text = ""
        txtObservacion.Text = ""
    End Sub

    ' ------------------------- Listado -------------------------

    Protected Sub btnRecargar_Click(sender As Object, e As EventArgs)
        CargarListado()
    End Sub

    Private Sub CargarListado()
        Try
            Dim dt As DataTable = objPieza.Listar()
            If dt Is Nothing Then dt = New DataTable()

            ' Solo las 50 mas recientes para no cargar toda la tabla.
            If dt.Rows.Count > 50 Then
                Dim recorte As DataTable = dt.Clone()
                For i As Integer = 0 To 49
                    recorte.ImportRow(dt.Rows(i))
                Next
                dt = recorte
            End If

            grvPiezas.DataSource = dt
            grvPiezas.DataBind()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Function TryParseMedida(texto As String, ByRef medida As Decimal?) As Boolean
        texto = If(texto, "").Trim()
        If texto.Length = 0 Then Return False

        Dim valor As Decimal
        If Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, valor) OrElse
           Decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, valor) OrElse
           Decimal.TryParse(texto.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, valor) Then
            medida = valor
            Return True
        End If

        medida = Nothing
        Return False
    End Function
End Class

Imports System.Data
Imports System.Globalization
Imports capaNegocio

' Piezas de vidrio y aluminio (vw_inv_piezas): consulta, cambio de estado y corte.
Partial Class wfVerPiezas
    Inherits System.Web.UI.Page

    Private ReadOnly objInv As New Inventario()
    Private ReadOnly objPieza As New InventarioPieza()
    Private autorizado As Boolean
    Private idUsuario As Integer
    Private Const TODOS As String = "(Todos)"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing OrElse Not Integer.TryParse(Session("idUsuario").ToString(), idUsuario) OrElse idUsuario <= 0 Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Try
            Dim permisos = New Permiso().codigosPermisoUsuario(idUsuario)
            autorizado = permisos IsNot Nothing AndAlso permisos.Contains("INVENTARIO")
        Catch ex As Exception
            lblMensaje.Text = "No se pudieron verificar los permisos."
        End Try

        If Not autorizado Then
            pnlListado.Visible = False
            pnlEditar.Visible = False
            If lblMensaje.Text = "" Then lblMensaje.Text = "No tiene permiso para consultar el inventario."
            Return
        End If

        If Not IsPostBack Then
            Try
                ddlVariante.Items.Add(New ListItem(TODOS, ""))
                For Each fila As DataRow In objInv.ListarVariantes(True).Rows
                    ddlVariante.Items.Add(New ListItem(fila("producto").ToString(), fila("id_variante").ToString()))
                Next
            Catch ex As Exception
                lblMensaje.Text = "No se pudieron cargar los productos."
            End Try
            ddlEstado.Items.Add(TODOS)
            For Each estado As String In Inventario.EstadosPieza
                ddlEstado.Items.Add(estado)
            Next
            ' Al editar solo se ofrecen los cambios manuales (CONSUMIDA la pone el corte)
            For Each estado As String In New String() {"DISPONIBLE", "RESERVADA", "VENDIDA", "DEFECTUOSA"}
                ddlNuevoEstado.Items.Add(estado)
            Next
            CargarListado()
        End If
    End Sub

    Private Sub CargarListado()
        Try
            Dim idVariante As Integer? = Nothing
            If ddlVariante.SelectedValue <> "" Then idVariante = CInt(ddlVariante.SelectedValue)
            Dim estado As String = If(ddlEstado.SelectedIndex <= 0, Nothing, ddlEstado.SelectedValue)
            Dim dt As DataTable = objPieza.Listar(idVariante, estado)
            grvPiezas.DataSource = dt
            grvPiezas.DataBind()
            lblResumen.Text = "Piezas: " & dt.Rows.Count.ToString()
        Catch ex As Exception
            grvPiezas.DataSource = Nothing
            grvPiezas.DataBind()
            lblResumen.Text = ""
            MostrarError("No se pudieron cargar las piezas.")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        If Not autorizado Then Return
        lblMensaje.Text = ""
        pnlEditar.Visible = False
        CargarListado()
    End Sub

    ' "Editar" de una fila: abre el panel de cambio de estado / corte
    Protected Sub grvPiezas_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If Not autorizado OrElse e.CommandName <> "Editar" Then Return
        Dim fila As Integer
        If Not Integer.TryParse(Convert.ToString(e.CommandArgument), fila) OrElse fila < 0 OrElse fila >= grvPiezas.DataKeys.Count Then Return

        lblMensaje.Text = ""
        Dim claves As DataKey = grvPiezas.DataKeys(fila)
        Dim estado As String = claves("estado").ToString()
        ViewState("idPieza") = CInt(claves("id_pieza"))
        lblPieza.Text = "Pieza #" & claves("id_pieza").ToString() & " - " & Server.HtmlEncode(claves("producto").ToString()) & " - Estado: " & estado
        txtObservacion.Text = ""
        txtAncho.Text = ""
        txtAlto.Text = ""
        txtLargo.Text = ""
        ' Solo se puede cortar o cambiar una pieza que sigue en el almacén
        Dim activa As Boolean = (estado = "DISPONIBLE" OrElse estado = "RESERVADA")
        btnCambiarEstado.Enabled = activa
        btnCortar.Enabled = activa
        pnlEditar.Visible = True
        If Not activa Then MostrarError("La pieza está " & estado & ": ya no se puede cortar ni cambiar de estado.")
    End Sub

    Protected Sub btnCambiarEstado_Click(sender As Object, e As EventArgs)
        If Not autorizado OrElse ViewState("idPieza") Is Nothing Then Return
        Try
            objPieza.MarcarEstado(CInt(ViewState("idPieza")), ddlNuevoEstado.SelectedValue, idUsuario, Nothing, txtObservacion.Text)
            pnlEditar.Visible = False
            CargarListado()
            MostrarOk("Estado de la pieza actualizado a " & ddlNuevoEstado.SelectedValue & ".")
        Catch ex As Exception
            MostrarError(ex.Message)
        End Try
    End Sub

    Protected Sub btnCortar_Click(sender As Object, e As EventArgs)
        If Not autorizado OrElse ViewState("idPieza") Is Nothing Then Return
        Dim ancho As Decimal? = Medida(txtAncho.Text)
        Dim alto As Decimal? = Medida(txtAlto.Text)
        Dim largo As Decimal? = Medida(txtLargo.Text)
        If Not largo.HasValue AndAlso Not (ancho.HasValue AndAlso alto.HasValue) Then
            MostrarError("Ingrese ancho y alto (vidrio) o el largo (aluminio), mayores a 0.")
            Return
        End If
        Try
            Dim retazos As DataTable = objPieza.Cortar(CInt(ViewState("idPieza")), ancho, alto, largo, idUsuario)
            pnlEditar.Visible = False
            CargarListado()
            MostrarOk("Pieza cortada. Retazos generados: " & retazos.Rows.Count.ToString() & ".")
        Catch ex As Exception
            MostrarError(ex.Message)
        End Try
    End Sub

    Protected Sub btnCerrarEdicion_Click(sender As Object, e As EventArgs)
        pnlEditar.Visible = False
        ViewState("idPieza") = Nothing
    End Sub

    ' Medida en cm escrita por el usuario (acepta coma o punto). Vacío o 0 = Nothing
    Private Function Medida(texto As String) As Decimal?
        Dim valor As Decimal
        If Decimal.TryParse(texto.Trim().Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, valor) AndAlso valor > 0 Then Return valor
        Return Nothing
    End Function

    Private Sub MostrarError(texto As String)
        lblMensaje.CssClass = "mensaje-error"
        lblMensaje.Text = texto
    End Sub

    Private Sub MostrarOk(texto As String)
        lblMensaje.CssClass = "mensaje-ok"
        lblMensaje.Text = texto
    End Sub
End Class

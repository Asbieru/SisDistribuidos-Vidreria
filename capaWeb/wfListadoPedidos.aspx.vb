Imports System.Data
Imports System.Globalization

' LISTADO DE PEDIDOS: versión web de frmListadoPedido (capaPresentacion).
' Buscar por cliente / usuario / fecha, ver el detalle de un pedido y cancelarlo.
Partial Class wfListadoPedidos
    Inherits System.Web.UI.Page

    Private ReadOnly objPedido As New Pedido()
    Private ReadOnly objPago As New Pago()

    Private ReadOnly Property IdUsuarioConectado As Integer
        Get
            Dim id As Integer
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
            CargarLista()
        End If
    End Sub

    ' ------------------------- Listado -------------------------

    ' Trae los pedidos de la capa de negocio y aplica el filtro de búsqueda
    Private Sub CargarLista()
        Try
            Dim dt As DataTable = objPedido.ListarDetallado()
            If dt Is Nothing Then dt = New DataTable()

            Dim texto As String = txtBuscar.Text.Trim()
            If texto <> "" Then
                ' Se escapan los caracteres especiales del filtro (igual que en escritorio)
                texto = texto.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]")
                dt.DefaultView.RowFilter = ddlCategoria.SelectedValue & " LIKE '%" & texto & "%'"
            End If
            Dim vista As DataTable = dt.DefaultView.ToTable()

            ' Paginación: si la página actual ya no existe (por ejemplo tras una búsqueda), se ajusta
            Dim totalPaginas As Integer = Math.Max(1, CInt(Math.Ceiling(vista.Rows.Count / grvPedidos.PageSize)))
            If grvPedidos.PageIndex >= totalPaginas Then grvPedidos.PageIndex = totalPaginas - 1

            grvPedidos.DataSource = vista
            grvPedidos.DataBind()

            If vista.Rows.Count = 0 Then
                lblResumen.CssClass = "mensaje-info"
                lblResumen.Text = "No se encontraron pedidos."
            Else
                Dim desde As Integer = grvPedidos.PageIndex * grvPedidos.PageSize + 1
                Dim hasta As Integer = Math.Min(desde + grvPedidos.PageSize - 1, vista.Rows.Count)
                lblResumen.CssClass = ""
                lblResumen.Text = "Mostrando " & desde & " - " & hasta & " de " & vista.Rows.Count &
                                  " pedido(s). Página " & (grvPedidos.PageIndex + 1) & " de " & totalPaginas & "."
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        grvPedidos.PageIndex = 0
        lblMensaje.Text = ""
        CargarLista()
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscar.Text = ""
        grvPedidos.PageIndex = 0
        lblMensaje.Text = ""
        CargarLista()
    End Sub

    Protected Sub grvPedidos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        grvPedidos.PageIndex = e.NewPageIndex
        CargarLista()
    End Sub

    Protected Sub grvPedidos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim id As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), id) OrElse id <= 0 Then Return

        Select Case e.CommandName
            Case "VerPedido"
                VerPedido(id)
            Case "CancelarPedido"
                CancelarPedido(id)
        End Select
    End Sub

    ' Igual que en frmListadoPedido: "Ver Pedido" abre el detalle en una
    ' pagina propia (equivalente a frmPedidoUnitario), no dentro del listado.
    Private Sub VerPedido(id As Integer)
        Try
            Dim dt As DataTable = objPedido.Obtener(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("El pedido ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If
            If dt.Rows(0)("estado").ToString() = "CANCELADO" Then
                MostrarMensaje("El pedido seleccionado está cancelado y no puede abrirse.", "mensaje-info")
                Return
            End If

            Response.Redirect("~/wfPedidoUnitario.aspx?idPedido=" & id.ToString(), False)
            Context.ApplicationInstance.CompleteRequest()
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Cancelar pedido -------------------------

    Private Sub CancelarPedido(id As Integer)
        Try
            Dim dt As DataTable = objPedido.Obtener(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("El pedido ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If
            Dim estado As String = dt.Rows(0)("estado").ToString()
            If estado = "CANCELADO" Then
                MostrarMensaje("El pedido seleccionado ya está cancelado.", "mensaje-info")
                Return
            End If
            If estado = "PAGADO" Then
                MostrarMensaje("Un pedido pagado no puede cancelarse desde esta opción.", "mensaje-info")
                Return
            End If

            Dim idUsu As Integer? = If(IdUsuarioConectado > 0, CType(IdUsuarioConectado, Integer?), Nothing)
            objPedido.Cancelar(id, idUsu)

            Dim texto As String = "Pedido N° " & id & " cancelado correctamente."
            Dim pagado As Decimal = objPago.TotalPagado(id)
            If pagado > 0D Then
                texto &= " Dinero cobrado por devolver al cliente: S/. " &
                         pagado.ToString("0.00", CultureInfo.InvariantCulture) &
                         ". <a href=""wfRegistrarPago.aspx?idPedido=" & id.ToString() &
                         """>Ver los pagos del pedido</a>."
            End If

            CargarLista()
            MostrarMensaje(texto, "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Utilidades -------------------------

    ' Color del estado (las clases ya existen en sistema.css)
    Protected Function ClaseEstado(estado As Object) As String
        Select Case If(estado Is Nothing, "", estado.ToString())
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

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub
End Class
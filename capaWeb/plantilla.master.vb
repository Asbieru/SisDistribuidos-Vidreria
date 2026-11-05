Imports System.Collections.Generic
Imports capaNegocio

Partial Class plantilla
    Inherits System.Web.UI.MasterPage

    ' Tema activo de la sesion. Se guarda en la tabla usuario_preferencias
    ' igual que en la capaPresentacion.
    Protected ReadOnly Property TemaActual As String
        Get
            Dim tema As String = "OSCURO"
            If Session("tema") IsNot Nothing Then
                tema = Session("tema").ToString().ToUpperInvariant()
            End If
            Select Case tema
                Case "CLARO", "OSCURO", "AZULCLARO", "AZULOSCURO"
                    Return tema
                Case Else
                    Return "OSCURO"
            End Select
        End Get
    End Property

    Protected ReadOnly Property ClaseTema As String
        Get
            Return "tema-" & TemaActual.ToLowerInvariant()
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not IsPostBack Then
            lblId.Text = Session("idUsuario").ToString()
            lblUsuario.Text = Server.HtmlEncode(If(Session("usuario") Is Nothing, "", Session("usuario").ToString()))
            lblRol.Text = Server.HtmlEncode(If(Session("rol") Is Nothing, "", Session("rol").ToString()))
            MarcarOpcionActiva()
            AplicarPermisos()
            ' La bitácora solo la ve el administrador
            pnlReportes.Visible = lblRol.Text.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)
        End If
    End Sub

    Private Sub MarcarOpcionActiva()
        Dim pagina As String = IO.Path.GetFileName(Request.AppRelativeCurrentExecutionFilePath).ToLowerInvariant()
        If pagina = "wfinicio.aspx" Then
            lnkInicio.CssClass = "menu-opcion activo"
        End If
    End Sub

    ' Oculta las opciones que el usuario no tiene permitidas. Si la consulta
    ' falla o el usuario aun no tiene permisos asignados, se dejan visibles
    ' todas las opciones para no dejar la pantalla sin navegacion.
    Private Sub AplicarPermisos()
        Try
            Dim idUsuario As Integer = Convert.ToInt32(Session("idUsuario"))
            Dim permisos As List(Of String) = New Permiso().codigosPermisoUsuario(idUsuario)

            If permisos Is Nothing OrElse permisos.Count = 0 Then Return

            lnkRegistrarCompra.Visible = permisos.Contains("COMPRAS")
            lnkCompras.Visible = permisos.Contains("COMPRAS")
            lnkProveedores.Visible = permisos.Contains("COMPRAS")

            lnkRegistrarPedido.Visible = permisos.Contains("PEDIDOS")
            lnkPedidos.Visible = permisos.Contains("PEDIDOS")

            lnkRegistrarPago.Visible = permisos.Contains("PAGOS")

            lnkInventario.Visible = permisos.Contains("INVENTARIO")
            lnkRegistrarPieza.Visible = permisos.Contains("INVENTARIO")
            lnkUsuarios.Visible = permisos.Contains("USUARIOS")
            lnkPermisos.Visible = permisos.Contains("PERMISOS")
            lnkClientes.Visible = permisos.Contains("CLIENTES")
            lnkProductos.Visible = permisos.Contains("PRODUCTOS")

            lnkVentas.Visible = permisos.Contains("PEDIDOS") AndAlso permisos.Contains("PAGOS")

            pnlProcesos.Visible = lnkRegistrarCompra.Visible OrElse
                                  lnkRegistrarPedido.Visible OrElse
                                  lnkRegistrarPago.Visible OrElse
                                  lnkRegistrarPieza.Visible

            pnlListados.Visible = lnkCompras.Visible OrElse
                                  lnkPedidos.Visible OrElse
                                  lnkInventario.Visible OrElse
                                  lnkVentas.Visible

            pnlMantenimiento.Visible = lnkUsuarios.Visible OrElse
                                       lnkPermisos.Visible OrElse
                                       lnkClientes.Visible OrElse
                                       lnkProductos.Visible OrElse
                                       lnkProveedores.Visible
        Catch ex As Exception
            ' Se mantiene el menu completo como respaldo.
        End Try
    End Sub

    Protected Sub CambiarTema(sender As Object, e As CommandEventArgs)
        Dim nuevoTema As String = e.CommandArgument.ToString().ToUpperInvariant()
        If nuevoTema <> "CLARO" AndAlso nuevoTema <> "OSCURO" AndAlso
           nuevoTema <> "AZULCLARO" AndAlso nuevoTema <> "AZULOSCURO" Then
            Return
        End If

        Session("tema") = nuevoTema
        Try
            Dim idUsuario As Integer = Convert.ToInt32(Session("idUsuario"))
            Dim objUsu As New Usuario()
            objUsu.guardarPreferencia(idUsuario, nuevoTema, "Arial")
        Catch ex As Exception
            ' El tema se aplica igual en la sesion aunque no se pueda guardar.
        End Try

        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub CerrarSesion_Click(sender As Object, e As EventArgs)
        ' Deja constancia del cierre de sesión en la bitácora (nunca bloquea el cierre)
        Dim idUsuario As Integer
        If Session("idUsuario") IsNot Nothing AndAlso Integer.TryParse(Session("idUsuario").ToString(), idUsuario) Then
            Dim objBit As New Bitacora()
            objBit.registrarAccionSeguro(idUsuario, Bitacora.CIERRE_SESION)
        End If
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/wfLogin.aspx?salir=1", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class

Imports System.Collections.Generic

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

    ' ---------------- ROLES Y PERMISOS ----------------
    ' Antes de cargar cualquier página se revisa si el usuario conectado puede entrar.
    ' Así un VENDEDOR no puede abrir una página de ADMINISTRADOR escribiendo la dirección.
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        Dim pagina As String = IO.Path.GetFileName(Request.AppRelativeCurrentExecutionFilePath).ToLowerInvariant()
        If Not PuedeEntrar(pagina) Then
            Response.Redirect("~/wfInicio.aspx?denegado=1")
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        ' Al publicar este ajuste se descarta el estado anterior de esos dos grupos
        ' para que vuelvan a aparecer sin necesidad de cerrar sesión.
        If Convert.ToString(Session("versionEstadoMenu")) <> VersionEstadoMenu Then
            Session.Remove("menuAbierto_listados")
            Session.Remove("menuAbierto_mantenimiento")
            Session("versionEstadoMenu") = VersionEstadoMenu
        End If

        AplicarEstadoMenu()

        If Not IsPostBack Then
            lblId.Text = Session("idUsuario").ToString()
            lblUsuario.Text = Server.HtmlEncode(If(Session("usuario") Is Nothing, "", Session("usuario").ToString()))
            lblRol.Text = Server.HtmlEncode(RolActual)
            lblAcceso.Visible = (Request.QueryString("denegado") = "1")
            MarcarOpcionActiva()
            AplicarPermisos()
        End If
    End Sub

    Private Sub MarcarOpcionActiva()
        Dim pagina As String = IO.Path.GetFileName(Request.AppRelativeCurrentExecutionFilePath).ToLowerInvariant()
        If pagina = "wfinicio.aspx" Then
            lnkInicio.CssClass = "menu-opcion activo"
        End If
    End Sub

    Private ReadOnly Property RolActual As String
        Get
            Return If(Session("rol") Is Nothing, "", Session("rol").ToString())
        End Get
    End Property

    Private Function EsAdministrador() As Boolean
        Return RolActual.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)
    End Function

    ' Códigos de permiso del usuario conectado (COMPRAS, PEDIDOS, USUARIOS...).
    ' Si el usuario no tiene permisos propios se usan los de su rol (ADMINISTRADOR o VENDEDOR).
    ' Se guardan en la sesión para no consultar la base en cada página;
    ' si se cambian, se ven al volver a iniciar sesión (igual que en escritorio).
    Private Function PermisosUsuario() As List(Of String)
        Dim guardados As List(Of String) = TryCast(Session("permisos"), List(Of String))
        If guardados IsNot Nothing Then Return guardados

        Dim lista As New List(Of String)
        Try
            Dim objPer As New Permiso()
            lista = objPer.codigosPermisoUsuario(Convert.ToInt32(Session("idUsuario")))
            If lista.Count = 0 AndAlso RolActual <> "" Then
                For Each fila As System.Data.DataRow In objPer.listarPermisosRol(RolActual).Rows
                    lista.Add(fila("codigo").ToString())
                Next
            End If
            Session("permisos") = lista
        Catch ex As Exception
            ' Si falla la consulta no se guarda en sesión: se vuelve a intentar en la siguiente página.
        End Try
        Return lista
    End Function

    Private Function Tiene(codigo As String) As Boolean
        Return PermisosUsuario().Contains(codigo)
    End Function

    ' Qué permiso pide cada página. Las que no están en la lista (Inicio,
    ' Modificar Contraseña...) las puede abrir cualquier usuario conectado.
    Private Function PuedeEntrar(pagina As String) As Boolean
        Select Case pagina
            Case "wfregistrarcompras.aspx", "wflistadocompras.aspx", "wfregistrarproveedores.aspx"
                Return Tiene("COMPRAS")
            Case "wfregistrarpedido.aspx"
                Return Tiene("PEDIDOS")
            Case "wfregistrarpago.aspx"
                Return Tiene("PAGOS")
            Case "wflistadoventas.aspx"
                Return Tiene("PEDIDOS") AndAlso Tiene("PAGOS")
            Case "wflistadoinventario.aspx", "wfregistrarpieza.aspx", "wfverpiezas.aspx",
                 "wfmovimientocajas.aspx", "wfverkardex.aspx"
                Return Tiene("INVENTARIO")
            Case "wfregistrarusuarios.aspx"
                Return Tiene("USUARIOS")
            Case "wfpermisos.aspx"
                Return Tiene("PERMISOS")
            Case "wfregistrarclientes.aspx", "wffichacliente.aspx"
                Return Tiene("CLIENTES")
            Case "wfregistrarproductos.aspx"
                Return Tiene("PRODUCTOS")
            Case "wfreporteflujoinventario.aspx", "wfreportepedidosmes.aspx",
                 "wfreportecomprastiempo.aspx", "wfreportependientes.aspx"
                Return Tiene("REPORTES")
            Case Else
                Return True
        End Select
    End Function

    ' Arma el menú según el usuario que ingresó: el ADMINISTRADOR ve todas las opciones
    ' y el VENDEDOR solo las suyas. Es la misma lógica que aplicarPermisos() de MenuInicio.
    Private Sub AplicarPermisos()
        ' Mantenimiento
        lnkUsuarios.Visible = Tiene("USUARIOS")
        lnkPermisos.Visible = Tiene("PERMISOS")
        lnkClientes.Visible = Tiene("CLIENTES")
        lnkProductos.Visible = Tiene("PRODUCTOS")
        lnkProveedores.Visible = Tiene("COMPRAS")

        ' Inventario
        lnkInventario.Visible = Tiene("INVENTARIO")
        lnkRegistrarPieza.Visible = Tiene("INVENTARIO")

        ' Compras
        lnkRegistrarCompra.Visible = Tiene("COMPRAS")
        lnkCompras.Visible = Tiene("COMPRAS")

        ' Pedidos y pagos
        lnkRegistrarPedido.Visible = Tiene("PEDIDOS")
        lnkPedidos.Visible = Tiene("PEDIDOS")
        lnkRegistrarPago.Visible = Tiene("PAGOS")

        ' Ventas: requiere acceso a pedidos y pagos
        lnkVentas.Visible = Tiene("PEDIDOS") AndAlso Tiene("PAGOS")

        ' Reportes: permiso propio, actualmente solo para administradores
        lnkReporteFlujo.Visible = Tiene("REPORTES")
        lnkReportePedidosMes.Visible = Tiene("REPORTES")
        lnkReporteCompras.Visible = Tiene("REPORTES")
        lnkReportePendientes.Visible = Tiene("REPORTES")
        pnlReportes.Visible = Tiene("REPORTES")

        ' Solo se muestran los grupos que tienen alguna opción disponible
        pnlProcesos.Visible = lnkRegistrarCompra.Visible OrElse lnkRegistrarPedido.Visible OrElse
                              lnkRegistrarPago.Visible OrElse lnkRegistrarPieza.Visible

        pnlListados.Visible = lnkCompras.Visible OrElse lnkPedidos.Visible OrElse
                              lnkInventario.Visible OrElse lnkVentas.Visible

        pnlMantenimiento.Visible = lnkUsuarios.Visible OrElse lnkPermisos.Visible OrElse
                                   lnkClientes.Visible OrElse lnkProductos.Visible OrElse
                                   lnkProveedores.Visible
    End Sub

    ' ---------------- MENÚ COLAPSABLE ----------------
    ' Cada grupo recuerda si está abierto en la sesión, así el estado se mantiene
    ' al navegar entre páginas. Por defecto quedan abiertos Principal,
    ' Mostrar Listado y Mantenimiento; los demás se abren al hacer clic.
    Private Shared ReadOnly GruposMenu As String() =
        {"principal", "procesos", "listados", "reportes", "mantenimiento", "configuracion", "tema"}

    Private Const VersionEstadoMenu As String = "20261122b"

    Private Function GrupoMenuAbierto(clave As String) As Boolean
        Dim nombreSesion As String = "menuAbierto_" & clave
        If Session(nombreSesion) Is Nothing Then
            Dim abiertoPorDefecto As Boolean =
                (clave = "principal" OrElse clave = "listados" OrElse clave = "mantenimiento")
            Session(nombreSesion) = abiertoPorDefecto
        End If
        Return Convert.ToBoolean(Session(nombreSesion))
    End Function

    Private Sub AplicarEstadoMenu()
        pnlPrincipalItems.Visible = GrupoMenuAbierto("principal")
        pnlProcesosItems.Visible = GrupoMenuAbierto("procesos")
        pnlListadosItems.Visible = GrupoMenuAbierto("listados")
        pnlReportesItems.Visible = GrupoMenuAbierto("reportes")
        pnlMantenimientoItems.Visible = GrupoMenuAbierto("mantenimiento")
        pnlConfiguracionItems.Visible = GrupoMenuAbierto("configuracion")
        pnlTemaItems.Visible = GrupoMenuAbierto("tema")

        MarcarGrupo(lnkGrupoPrincipal, pnlPrincipalItems.Visible)
        MarcarGrupo(lnkGrupoProcesos, pnlProcesosItems.Visible)
        MarcarGrupo(lnkGrupoListados, pnlListadosItems.Visible)
        MarcarGrupo(lnkGrupoReportes, pnlReportesItems.Visible)
        MarcarGrupo(lnkGrupoMantenimiento, pnlMantenimientoItems.Visible)
        MarcarGrupo(lnkGrupoConfiguracion, pnlConfiguracionItems.Visible)
        MarcarGrupo(lnkGrupoTema, pnlTemaItems.Visible)
    End Sub

    Private Shared Sub MarcarGrupo(enlace As LinkButton, abierto As Boolean)
        enlace.CssClass = "menu-grupo-titulo menu-grupo-toggle " & If(abierto, "abierto", "cerrado")
    End Sub

    Protected Sub ToggleMenuGroup(sender As Object, e As CommandEventArgs)
        Dim clave As String = e.CommandArgument.ToString().ToLowerInvariant()
        If Array.IndexOf(GruposMenu, clave) < 0 Then Return

        Session("menuAbierto_" & clave) = Not GrupoMenuAbierto(clave)
        AplicarEstadoMenu()
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
        Session.Clear()
        Session.Abandon()
        Response.Redirect("~/wfLogin.aspx?salir=1", False)
        Context.ApplicationInstance.CompleteRequest()
    End Sub
End Class
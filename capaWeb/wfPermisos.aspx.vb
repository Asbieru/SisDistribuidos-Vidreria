Imports System.Data
Imports System.Collections.Generic

' MANTENIMIENTO: permisos de cada usuario. Es la versión web de frmPermisos (capaPresentacion).
' Solo entra quien tiene el permiso PERMISOS (lo controla plantilla.master).
Partial Class wfPermisos
    Inherits System.Web.UI.Page

    Private ReadOnly objPer As New Permiso()
    Private ReadOnly objUsu As New Usuario()

    ' Permisos que el usuario conectado NO se puede quitar a sí mismo (si no, se quedaría sin acceso)
    Private ReadOnly permisosProtegidos As String() = {"USUARIOS", "PERMISOS"}

    ' id_tipo de los permisos que están en la lista de asignados (se conservan entre postbacks)
    Private Property Asignados As List(Of Integer)
        Get
            If ViewState("asignados") Is Nothing Then ViewState("asignados") = New List(Of Integer)()
            Return CType(ViewState("asignados"), List(Of Integer))
        End Get
        Set(value As List(Of Integer))
            ViewState("asignados") = value
        End Set
    End Property

    Private Property RolSeleccionado As String
        Get
            Return If(ViewState("rol") Is Nothing, "", ViewState("rol").ToString())
        End Get
        Set(value As String)
            ViewState("rol") = value
        End Set
    End Property

    Private ReadOnly Property IdUsuarioConectado As Integer
        Get
            Return Convert.ToInt32(Session("idUsuario"))
        End Get
    End Property

    Private ReadOnly Property IdUsuarioSeleccionado As Integer
        Get
            Dim id As Integer
            Integer.TryParse(ddlUsuarios.SelectedValue, id)
            Return id
        End Get
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("idUsuario") Is Nothing Then
            Response.Redirect("~/wfLogin.aspx")
            Return
        End If

        If Not IsPostBack Then
            If Request.QueryString("ok") = "1" Then
                MostrarMensaje("Permisos guardados correctamente.", False)
            End If
            CargarUsuarios()
        End If
    End Sub

    ' Llena el combo: "juan - VENDEDOR (INACTIVO)"
    Private Sub CargarUsuarios()
        Try
            Dim dt As DataTable = objUsu.listarUsuarios()
            ddlUsuarios.Items.Clear()
            For Each fila As DataRow In dt.Rows
                Dim texto As String = fila("nombre").ToString() & " - " & fila("rol").ToString()
                If fila("estado").ToString() = "INACTIVO" Then texto &= " (INACTIVO)"
                ddlUsuarios.Items.Add(New ListItem(texto, fila("id_usuario").ToString()))
            Next
            If ddlUsuarios.Items.Count > 0 Then
                ddlUsuarios.SelectedIndex = 0
                MostrarPermisosUsuario()
            Else
                MostrarMensaje("No hay usuarios registrados.", True)
            End If
        Catch ex As Exception
            MostrarMensaje(ex.Message, True)
        End Try
    End Sub

    Protected Sub ddlUsuarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuarios.SelectedIndexChanged
        lblMensaje.Text = ""
        MostrarPermisosUsuario()
    End Sub

    ' Carga desde la base los permisos del usuario elegido en el combo
    Private Sub MostrarPermisosUsuario()
        Try
            Dim id As Integer = IdUsuarioSeleccionado
            Dim filas As DataRow() = objUsu.listarUsuarios().Select("id_usuario = " & id)
            If filas.Length = 0 Then Return

            RolSeleccionado = filas(0)("rol").ToString()
            lblRol.Text = "Rol: " & Server.HtmlEncode(RolSeleccionado) &
                          "   |   Estado: " & Server.HtmlEncode(filas(0)("estado").ToString())

            Dim lista As New List(Of Integer)()
            For Each p As DataRow In objPer.listarPermisosUsuario(id).Rows
                lista.Add(CInt(p("id_tipo")))
            Next
            Asignados = lista
            LlenarListas()
        Catch ex As Exception
            MostrarMensaje(ex.Message, True)
        End Try
    End Sub

    ' Reparte todos los tipos de permiso entre las dos listas según "Asignados"
    Private Sub LlenarListas()
        Dim dtTipos As DataTable = objPer.listarTiposPermiso()
        lstDisponibles.Items.Clear()
        lstAsignados.Items.Clear()
        For Each t As DataRow In dtTipos.Rows
            Dim item As New ListItem(t("nombre").ToString() & " (" & t("codigo").ToString() & ")", t("id_tipo").ToString())
            item.Attributes("title") = t("descripcion").ToString()   ' se ve al pasar el mouse
            If Asignados.Contains(CInt(t("id_tipo"))) Then
                lstAsignados.Items.Add(item)
            Else
                lstDisponibles.Items.Add(item)
            End If
        Next
        lblCantidad.Text = "Permisos asignados: " & Asignados.Count & " de " & dtTipos.Rows.Count
    End Sub

    ' Devuelve el código (USUARIOS, PERMISOS...) de un id_tipo
    Private Function CodigoDe(idTipo As Integer) As String
        For Each t As DataRow In objPer.listarTiposPermiso().Rows
            If CInt(t("id_tipo")) = idTipo Then Return t("codigo").ToString()
        Next
        Return ""
    End Function

    Private Function TieneCodigo(codigo As String) As Boolean
        For Each idTipo As Integer In Asignados
            If CodigoDe(idTipo) = codigo Then Return True
        Next
        Return False
    End Function

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim lista As List(Of Integer) = Asignados
        Dim hubo As Boolean = False
        For Each item As ListItem In lstDisponibles.Items
            If item.Selected Then
                lista.Add(CInt(item.Value))
                hubo = True
            End If
        Next
        If Not hubo Then
            MostrarMensaje("Seleccione uno o más permisos de la lista de disponibles.", True)
            Return
        End If
        Asignados = lista
        LlenarListas()
        MostrarMensaje("Permisos agregados. Presione GUARDAR para aplicar los cambios.", False)
    End Sub

    Protected Sub btnQuitar_Click(sender As Object, e As EventArgs) Handles btnQuitar.Click
        Dim lista As List(Of Integer) = Asignados
        Dim hubo As Boolean = False
        Dim avisos As String = ""
        For Each item As ListItem In lstAsignados.Items
            If item.Selected Then
                Dim idTipo As Integer = CInt(item.Value)
                If IdUsuarioSeleccionado = IdUsuarioConectado AndAlso
                   Array.IndexOf(permisosProtegidos, CodigoDe(idTipo)) >= 0 Then
                    avisos &= "No puede quitarse a usted mismo el permiso '" & item.Text & "'. "
                    Continue For
                End If
                lista.Remove(idTipo)
                hubo = True
            End If
        Next
        If Not hubo AndAlso avisos = "" Then
            MostrarMensaje("Seleccione uno o más permisos de la lista de asignados.", True)
            Return
        End If
        Asignados = lista
        LlenarListas()
        If avisos <> "" Then
            MostrarMensaje(avisos, True)
        Else
            MostrarMensaje("Permisos quitados. Presione GUARDAR para aplicar los cambios.", False)
        End If
    End Sub

    ' Reemplaza lo que hay en pantalla por los permisos por defecto del rol del usuario
    Protected Sub btnPermisosRol_Click(sender As Object, e As EventArgs) Handles btnPermisosRol.Click
        If IdUsuarioSeleccionado = 0 Then Return
        Try
            Dim lista As New List(Of Integer)()
            For Each p As DataRow In objPer.listarPermisosRol(RolSeleccionado).Rows
                lista.Add(CInt(p("id_tipo")))
            Next
            Asignados = lista
            LlenarListas()
            MostrarMensaje("Se cargaron los permisos del rol " & Server.HtmlEncode(RolSeleccionado) &
                           ". Presione GUARDAR para aplicarlos.", False)
        Catch ex As Exception
            MostrarMensaje(ex.Message, True)
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim id As Integer = IdUsuarioSeleccionado
        If id = 0 Then Return
        Try
            ' El usuario conectado no puede quedarse sin los permisos protegidos
            If id = IdUsuarioConectado Then
                For Each codigo As String In permisosProtegidos
                    If Not TieneCodigo(codigo) Then
                        MostrarMensaje("No puede quitarse a usted mismo el permiso " & codigo & ".", True)
                        Return
                    End If
                Next
            End If

            ' Siempre debe quedar al menos un usuario activo que pueda asignar permisos
            If Not TieneCodigo("PERMISOS") AndAlso objPer.contarOtrosConPermiso("PERMISOS", id) = 0 Then
                MostrarMensaje("Ningún otro usuario activo tiene el permiso PERMISOS. " &
                               "Si se lo quita a este usuario, nadie podrá volver a asignar permisos.", True)
                Return
            End If

            objPer.guardarPermisos(id, Asignados)
        Catch ex As Exception
            MostrarMensaje(ex.Message, True)
            Return
        End Try

        If id = IdUsuarioConectado Then
            ' Cambió sus propios permisos: se recarga la página para que el menú se actualice
            Session("permisos") = Nothing
            Response.Redirect("~/wfPermisos.aspx?ok=1")
            Return
        End If

        MostrarMensaje("Permisos guardados correctamente. Si el usuario tiene una sesión abierta, " &
                       "verá los cambios al volver a iniciar sesión.", False)
    End Sub

    Private Sub MostrarMensaje(texto As String, esError As Boolean)
        lblMensaje.CssClass = If(esError, "mensaje-error", "mensaje-ok")
        lblMensaje.Text = texto
    End Sub
End Class

Imports capaNegocio

Public Class frmPermisos
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema
    ' Lo envía MenuInicio: el usuario que tiene la sesión abierta
    Public Property idUsuarioConectado As Integer = 0

    ' Permisos que el usuario conectado NO se puede quitar a sí mismo (si no, se quedaría sin acceso)
    ReadOnly permisosProtegidos As String() = {"USUARIOS", "PERMISOS"}

    Dim objPer As New Permiso
    Dim objUsu As New Usuario
    Dim dtTipos As DataTable
    Dim dtUsuarios As DataTable
    Dim asignados As New HashSet(Of Integer)   ' id_tipo de los permisos asignados en pantalla
    Dim idUsuarioActual As Integer = 0
    Dim rolActual As String = ""
    Dim hayCambios As Boolean = False
    Dim cargando As Boolean = False

    Private Sub frmPermisos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Titulo

        configurarLista(lsvDisponibles)
        configurarLista(lsvAsignados)
        cboUsuarios.DropDownStyle = ComboBoxStyle.DropDownList

        Try
            dtTipos = objPer.listarTiposPermiso()
            cargarUsuarios()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Las columnas se crean por código, así no hay que configurarlas en el diseñador
    Private Sub configurarLista(lsv As ListView)
        lsv.View = View.Details
        lsv.FullRowSelect = True
        lsv.MultiSelect = True
        lsv.HideSelection = False
        lsv.Columns.Clear()
        lsv.Columns.Add("Permiso", 170)
        lsv.Columns.Add("Código", 100)
    End Sub

    Private Sub cargarUsuarios()
        cargando = True
        dtUsuarios = objUsu.listarUsuarios()
        ' Columna calculada para mostrar en el combo: "juan - VENDEDOR (INACTIVO)"
        dtUsuarios.Columns.Add("mostrar", GetType(String),
            "nombre + ' - ' + rol + IIF(estado = 'INACTIVO', ' (INACTIVO)', '')")
        cboUsuarios.DataSource = dtUsuarios
        cboUsuarios.DisplayMember = "mostrar"
        cboUsuarios.ValueMember = "id_usuario"
        cargando = False

        If dtUsuarios.Rows.Count > 0 Then
            cboUsuarios.SelectedIndex = 0
            mostrarPermisosUsuario()
        End If
    End Sub

    Private Sub cboUsuarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboUsuarios.SelectedIndexChanged
        If cargando OrElse cboUsuarios.SelectedValue Is Nothing OrElse TypeOf cboUsuarios.SelectedValue Is DataRowView Then Return
        If CInt(cboUsuarios.SelectedValue) = idUsuarioActual Then Return

        If hayCambios AndAlso MessageBox.Show("Hay cambios sin guardar en los permisos del usuario anterior." & vbCrLf &
                                              "¿Desea descartarlos?", "Permisos", MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question) = DialogResult.No Then
            ' Vuelve a seleccionar al usuario anterior sin perder los cambios
            cargando = True
            cboUsuarios.SelectedValue = idUsuarioActual
            cargando = False
            Return
        End If
        mostrarPermisosUsuario()
    End Sub

    ' Carga desde la base los permisos del usuario seleccionado en el combo
    Private Sub mostrarPermisosUsuario()
        Try
            Dim fila As DataRowView = CType(cboUsuarios.SelectedItem, DataRowView)
            idUsuarioActual = CInt(fila("id_usuario"))
            rolActual = fila("rol").ToString
            lblRol.Text = "Rol: " & rolActual & "   |   Estado: " & fila("estado").ToString

            asignados.Clear()
            For Each p As DataRow In objPer.listarPermisosUsuario(idUsuarioActual).Rows
                asignados.Add(CInt(p("id_tipo")))
            Next
            llenarListas()
            hayCambios = False
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Reparte todos los tipos de permiso entre las dos listas según "asignados"
    Private Sub llenarListas()
        lsvDisponibles.Items.Clear()
        lsvAsignados.Items.Clear()
        For Each t As DataRow In dtTipos.Rows
            Dim item As New ListViewItem(t("nombre").ToString)
            item.SubItems.Add(t("codigo").ToString)
            item.Tag = CInt(t("id_tipo"))
            item.ToolTipText = t("descripcion").ToString
            If asignados.Contains(CInt(t("id_tipo"))) Then
                lsvAsignados.Items.Add(item)
            Else
                lsvDisponibles.Items.Add(item)
            End If
        Next
        lblDescripcion.Text = ""
        lblCantidad.Text = "Permisos asignados: " & asignados.Count & " de " & dtTipos.Rows.Count
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If lsvDisponibles.SelectedItems.Count = 0 Then
            MessageBox.Show("Seleccione uno o más permisos de la lista de disponibles.", "Permisos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        For Each item As ListViewItem In lsvDisponibles.SelectedItems
            asignados.Add(CInt(item.Tag))
        Next
        hayCambios = True
        llenarListas()
    End Sub

    Private Sub btnQuitar_Click(sender As Object, e As EventArgs) Handles btnQuitar.Click
        If lsvAsignados.SelectedItems.Count = 0 Then
            MessageBox.Show("Seleccione uno o más permisos de la lista de asignados.", "Permisos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        For Each item As ListViewItem In lsvAsignados.SelectedItems
            Dim codigo As String = item.SubItems(1).Text
            If idUsuarioActual = idUsuarioConectado AndAlso permisosProtegidos.Contains(codigo) Then
                MessageBox.Show("No puede quitarse a usted mismo el permiso '" & item.Text & "'," & vbCrLf &
                                "porque se quedaría sin acceso para administrar usuarios y permisos.", "Permisos",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Continue For
            End If
            asignados.Remove(CInt(item.Tag))
            hayCambios = True
        Next
        llenarListas()
    End Sub

    ' Doble clic en una lista mueve el permiso a la otra
    Private Sub lsvDisponibles_DoubleClick(sender As Object, e As EventArgs) Handles lsvDisponibles.DoubleClick
        btnAgregar.PerformClick()
    End Sub

    Private Sub lsvAsignados_DoubleClick(sender As Object, e As EventArgs) Handles lsvAsignados.DoubleClick
        btnQuitar.PerformClick()
    End Sub

    ' Muestra para qué sirve el permiso seleccionado
    Private Sub lsv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lsvDisponibles.SelectedIndexChanged, lsvAsignados.SelectedIndexChanged
        Dim lsv As ListView = DirectCast(sender, ListView)
        lblDescripcion.Text = If(lsv.SelectedItems.Count = 1, lsv.SelectedItems(0).ToolTipText, "")
    End Sub

    ' Reemplaza lo que hay en pantalla por los permisos por defecto del rol del usuario
    Private Sub btnPermisosRol_Click(sender As Object, e As EventArgs) Handles btnPermisosRol.Click
        If idUsuarioActual = 0 Then Return
        If MessageBox.Show("¿Cargar los permisos por defecto del rol " & rolActual & "?" & vbCrLf &
                           "Se reemplazarán los de la lista (se guardan al presionar GUARDAR).", "Permisos",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Try
            asignados.Clear()
            For Each p As DataRow In objPer.listarPermisosRol(rolActual).Rows
                asignados.Add(CInt(p("id_tipo")))
            Next
            hayCambios = True
            llenarListas()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function tieneCodigo(codigo As String) As Boolean
        For Each t As DataRow In dtTipos.Rows
            If t("codigo").ToString = codigo Then Return asignados.Contains(CInt(t("id_tipo")))
        Next
        Return False
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If idUsuarioActual = 0 Then Return
        Try
            ' El usuario conectado no puede quedarse sin los permisos protegidos
            If idUsuarioActual = idUsuarioConectado Then
                For Each codigo As String In permisosProtegidos
                    If Not tieneCodigo(codigo) Then
                        MessageBox.Show("No puede quitarse a usted mismo el permiso " & codigo & ".", "Permisos",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                Next
            End If
            ' Siempre debe quedar al menos un usuario activo que pueda asignar permisos
            If Not tieneCodigo("PERMISOS") AndAlso objPer.contarOtrosConPermiso("PERMISOS", idUsuarioActual) = 0 Then
                MessageBox.Show("Ningún otro usuario activo tiene el permiso PERMISOS." & vbCrLf &
                                "Si se lo quita a este usuario, nadie podrá volver a asignar permisos.", "Permisos",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            objPer.guardarPermisos(idUsuarioActual, New List(Of Integer)(asignados))
            hayCambios = False
            MessageBox.Show("Permisos guardados correctamente." & vbCrLf &
                            "Si el usuario tiene una sesión abierta, verá los cambios al volver a iniciar sesión.",
                            "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Si se cambiaron los permisos propios, el menú se actualiza al instante
            If idUsuarioActual = idUsuarioConectado Then
                For Each f As Form In Application.OpenForms
                    If TypeOf f Is MenuInicio Then DirectCast(f, MenuInicio).aplicarPermisos()
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub frmPermisos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If hayCambios AndAlso MessageBox.Show("Hay cambios sin guardar. ¿Desea salir de todas formas?", "Permisos",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            e.Cancel = True
        End If
    End Sub
End Class
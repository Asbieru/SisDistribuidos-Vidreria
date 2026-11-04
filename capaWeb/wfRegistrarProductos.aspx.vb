Imports System.Data
Imports capaNegocio

Partial Class wfRegistrarProductos
    Inherits System.Web.UI.Page

    Private ReadOnly objProducto As New Producto()

    ' Producto que se esta editando (0 = nuevo)
    Private Property IdProductoActual As Integer
        Get
            If ViewState("idProducto") Is Nothing Then Return 0
            Return Convert.ToInt32(ViewState("idProducto"))
        End Get
        Set(value As Integer)
            ViewState("idProducto") = value
        End Set
    End Property

    ' Imagen que ya tuviera el producto; el formulario web no la modifica.
    Private Property ImagenActual As String
        Get
            Return If(ViewState("imagenUrl"), "").ToString()
        End Get
        Set(value As String)
            ViewState("imagenUrl") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarListado("")

            Dim id As Integer
            If Integer.TryParse(Request.QueryString("id"), id) AndAlso id > 0 Then
                CargarProducto(id)
            Else
                PrepararNuevo()
            End If
        End If
    End Sub

    ' ------------------------- Formulario -------------------------

    Private Sub PrepararNuevo()
        IdProductoActual = 0
        ImagenActual = ""

        lblModo.Text = "NUEVO PRODUCTO"
        txtId.Text = "(automático)"
        ddlCategoria.SelectedValue = "VIDRIO"
        txtTipo.Text = ""
        txtColor.Text = ""
        txtNombre.Text = ""
        txtDescripcion.Text = ""

        filaActivo.Visible = False
        chkActivo.Checked = True
        MostrarMensaje("", "")
    End Sub

    Private Sub CargarProducto(id As Integer)
        If id <= 0 Then
            PrepararNuevo()
            Return
        End If

        Try
            Dim dt As DataTable = objProducto.Obtener(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                PrepararNuevo()
                MostrarMensaje("El producto ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If

            Dim fila As DataRow = dt.Rows(0)
            IdProductoActual = id
            ImagenActual = Texto(fila("imagen_url"))

            lblModo.Text = "MODIFICAR PRODUCTO"
            txtId.Text = id.ToString()
            ddlCategoria.SelectedValue = Texto(fila("categoria"))
            txtTipo.Text = Texto(fila("tipo"))
            txtColor.Text = Texto(fila("color"))
            txtNombre.Text = Texto(fila("nombre"))
            txtDescripcion.Text = Texto(fila("descripcion"))

            filaActivo.Visible = True
            chkActivo.Checked = Convert.ToBoolean(fila("activo"))
            MostrarMensaje("", "")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs)
        PrepararNuevo()
        ddlCategoria.Focus()
    End Sub

    ' Sugiere el nombre a partir de la categoria, el tipo y el color.
    Protected Sub btnSugerirNombre_Click(sender As Object, e As EventArgs)
        If txtNombre.Text.Trim().Length = 0 AndAlso txtTipo.Text.Trim().Length > 0 Then
            Dim categoria As String = ddlCategoria.SelectedValue.ToLowerInvariant()
            If categoria.Length > 0 Then
                categoria = Char.ToUpper(categoria(0)) & categoria.Substring(1)
            End If
            txtNombre.Text = (categoria & " " & txtTipo.Text.Trim()).Trim()
            If txtColor.Text.Trim().Length > 0 Then
                txtNombre.Text &= " " & txtColor.Text.Trim()
            End If
        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        btnGuardar.Enabled = False
        Try
            If String.IsNullOrEmpty(ddlCategoria.SelectedValue) Then
                MostrarMensaje("Seleccione la categoría.", "mensaje-error")
                Return
            End If
            If txtTipo.Text.Trim().Length = 0 Then
                MostrarMensaje("Ingrese el tipo (por ejemplo: transparente, catedral, u13, autorroscante).", "mensaje-error")
                Return
            End If
            If txtNombre.Text.Trim().Length = 0 Then
                MostrarMensaje("Ingrese el nombre del producto.", "mensaje-error")
                Return
            End If

            If objProducto.ExisteProducto(ddlCategoria.SelectedValue, txtTipo.Text,
                                          txtColor.Text, IdProductoActual) Then
                MostrarMensaje("Ya existe otro producto con la misma categoría, tipo y color.", "mensaje-error")
                Return
            End If

            Dim idGuardado As Integer
            If IdProductoActual = 0 Then
                idGuardado = objProducto.Insertar(ddlCategoria.SelectedValue, txtTipo.Text,
                                                  txtColor.Text, txtNombre.Text, txtDescripcion.Text)
            Else
                objProducto.Actualizar(IdProductoActual, ddlCategoria.SelectedValue, txtTipo.Text,
                                       txtColor.Text, txtNombre.Text, txtDescripcion.Text, ImagenActual)
                objProducto.CambiarEstado(IdProductoActual, chkActivo.Checked)
                idGuardado = IdProductoActual
            End If

            CargarProducto(idGuardado)
            CargarListado(txtBuscar.Text)
            MostrarMensaje("Producto guardado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    ' ------------------------- Listado -------------------------

    Private Sub CargarListado(filtro As String)
        Try
            Dim dt As DataTable = objProducto.ListarTodos()
            If dt Is Nothing Then dt = New DataTable()

            If Not String.IsNullOrWhiteSpace(filtro) Then
                Dim texto As String = filtro.Trim().ToUpperInvariant()
                Dim filtrado As DataTable = dt.Clone()
                For Each fila As DataRow In dt.Rows
                    Dim coincide As Boolean =
                        fila("id_producto").ToString().Contains(texto) OrElse
                        fila("categoria").ToString().ToUpperInvariant().Contains(texto) OrElse
                        fila("tipo").ToString().ToUpperInvariant().Contains(texto) OrElse
                        fila("color").ToString().ToUpperInvariant().Contains(texto) OrElse
                        fila("nombre").ToString().ToUpperInvariant().Contains(texto)
                    If coincide Then filtrado.ImportRow(fila)
                Next
                dt = filtrado
            End If

            grvProductos.DataSource = dt
            grvProductos.DataBind()

            If dt.Rows.Count = 0 Then
                lblMensajeLista.CssClass = "mensaje-info"
                lblMensajeLista.Text = "No se encontraron productos."
            Else
                lblMensajeLista.CssClass = ""
                lblMensajeLista.Text = dt.Rows.Count.ToString() & " producto(s) encontrado(s)."
            End If
        Catch ex As Exception
            lblMensajeLista.CssClass = "mensaje-error"
            lblMensajeLista.Text = ex.Message
        End Try
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs)
        CargarListado(txtBuscar.Text)
    End Sub

    Protected Sub btnVerTodos_Click(sender As Object, e As EventArgs)
        txtBuscar.Text = ""
        CargarListado("")
    End Sub

    Protected Sub grvProductos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim id As Integer
        If Not Integer.TryParse(e.CommandArgument.ToString(), id) OrElse id <= 0 Then Return

        Select Case e.CommandName
            Case "EditarProducto"
                CargarProducto(id)
            Case "CambiarEstadoProducto"
                CambiarEstadoProducto(id)
            Case "EliminarProducto"
                EliminarProducto(id)
        End Select
    End Sub

    Private Sub CambiarEstadoProducto(id As Integer)
        Try
            Dim dt As DataTable = objProducto.Obtener(id)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                MostrarMensaje("El producto ya no existe. Actualice el listado.", "mensaje-error")
                Return
            End If

            Dim activo As Boolean = Convert.ToBoolean(dt.Rows(0)("activo"))
            objProducto.CambiarEstado(id, Not activo)

            If IdProductoActual = id Then
                CargarProducto(id)
            End If

            CargarListado(txtBuscar.Text)
            MostrarMensaje(If(activo, "Producto inactivado.", "Producto activado."), "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    Private Sub EliminarProducto(id As Integer)
        Try
            objProducto.Eliminar(id)

            If IdProductoActual = id Then
                PrepararNuevo()
            End If

            CargarListado(txtBuscar.Text)
            MostrarMensaje("Producto eliminado correctamente.", "mensaje-ok")
        Catch ex As Exception
            MostrarMensaje(ex.Message, "mensaje-error")
        End Try
    End Sub

    ' ------------------------- Utilidades -------------------------

    Private Sub MostrarMensaje(texto As String, clase As String)
        lblMensaje.CssClass = clase
        lblMensaje.Text = texto
    End Sub

    Private Function Texto(valor As Object) As String
        If valor Is Nothing OrElse IsDBNull(valor) Then Return ""
        Return valor.ToString()
    End Function
End Class

Imports capaNegocio

Public Class frmRegistrarProductos
    Implements IFormularioTema
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    ' Lo envía frmListadoProductos: 0 = producto nuevo, mayor a 0 = modificar ese producto
    Public Property idProducto As Integer = 0
    ' Lo lee frmListadoProductos al volver, para dejar seleccionado el producto guardado
    Public Property idProductoGuardado As Integer = 0

    Dim objProd As New Producto
    Dim imagenActual As String = Nothing   ' se conserva la imagen que ya tuviera el producto

    Private Sub frmRegistrarProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblModo.ForeColor = p.Acento

        cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList
        cboCategoria.Items.Clear()
        cboCategoria.Items.AddRange(Producto.Categorias)

        ' El ID lo genera la base de datos: solo se muestra, no se escribe
        txtId.ReadOnly = True
        txtId.TabStop = False

        ' Mismos límites que las columnas de la tabla productos
        txtTipo.MaxLength = 50
        txtColor.MaxLength = 30
        txtNombre.MaxLength = 150
        txtDescripcion.MaxLength = 255
        txtDescripcion.Multiline = True

        idProductoGuardado = 0
        Try
            If idProducto = 0 Then
                lblModo.Text = "NUEVO PRODUCTO"
                txtId.Text = "(automático)"
                cboCategoria.SelectedIndex = 0
                ' Un producto nuevo siempre se crea activo: el check no se muestra
                chkActivo.Checked = True
                chkActivo.Visible = False
            Else
                chkActivo.Visible = True
                cargarProducto()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DialogResult = DialogResult.Cancel
        End Try
    End Sub

    Private Sub cargarProducto()
        Dim dt As DataTable = objProd.Obtener(idProducto)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("El producto ya no existe. Actualice el listado.", "Productos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Return
        End If

        Dim fila As DataRow = dt.Rows(0)
        lblModo.Text = "MODIFICAR PRODUCTO"
        txtId.Text = idProducto.ToString
        cboCategoria.SelectedItem = fila("categoria").ToString
        txtTipo.Text = fila("tipo").ToString
        txtColor.Text = fila("color").ToString
        txtNombre.Text = fila("nombre").ToString
        txtDescripcion.Text = fila("descripcion").ToString
        imagenActual = If(IsDBNull(fila("imagen_url")), Nothing, fila("imagen_url").ToString)
        chkActivo.Checked = CBool(fila("activo"))   ' marcado = ACTIVO, sin marcar = INACTIVO
    End Sub

    Private Sub frmRegistrarProductos_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        cboCategoria.Focus()
    End Sub

    ' Si el nombre está vacío, se sugiere a partir de la categoría y el tipo (ej: "Vidrio transparente")
    Private Sub txtTipo_Leave(sender As Object, e As EventArgs) Handles txtTipo.Leave
        If txtNombre.Text.Trim = "" AndAlso txtTipo.Text.Trim <> "" AndAlso cboCategoria.SelectedIndex >= 0 Then
            Dim cat As String = cboCategoria.Text.ToLower
            cat = Char.ToUpper(cat(0)) & cat.Substring(1)
            txtNombre.Text = cat & " " & txtTipo.Text.Trim
            If txtColor.Text.Trim <> "" Then txtNombre.Text &= " " & txtColor.Text.Trim
        End If
    End Sub

    Private Function validar() As Boolean
        If cboCategoria.SelectedIndex = -1 Then
            MessageBox.Show("Seleccione la categoría.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboCategoria.Focus()
            Return False
        End If
        If txtTipo.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese el tipo (por ejemplo: transparente, catedral, u13, autorroscante).", "Productos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtTipo.Focus()
            Return False
        End If
        If txtNombre.Text.Trim.Length = 0 Then
            MessageBox.Show("Ingrese el nombre del producto.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombre.Focus()
            Return False
        End If
        Return True
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not validar() Then Return
        Try
            If objProd.ExisteProducto(cboCategoria.Text, txtTipo.Text, txtColor.Text, idProducto) Then
                MessageBox.Show("Ya existe otro producto con la misma categoría, tipo y color.", "Productos",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtTipo.Focus()
                Return
            End If

            If idProducto = 0 Then
                idProductoGuardado = objProd.Insertar(cboCategoria.Text, txtTipo.Text, txtColor.Text,
                                                      txtNombre.Text, txtDescripcion.Text)
                MessageBox.Show("Producto registrado correctamente.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                objProd.Actualizar(idProducto, cboCategoria.Text, txtTipo.Text, txtColor.Text,
                                   txtNombre.Text, txtDescripcion.Text, imagenActual)
                ' Se guarda el estado que tenga el check
                objProd.CambiarEstado(idProducto, chkActivo.Checked)
                idProductoGuardado = idProducto
                MessageBox.Show("Producto modificado correctamente.", "Productos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            DialogResult = DialogResult.OK   ' cierra el formulario y avisa al listado que hubo cambios
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Productos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel   ' cierra sin guardar
    End Sub
End Class
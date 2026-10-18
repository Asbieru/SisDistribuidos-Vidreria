Imports capaNegocio

' INGRESAR / EDITAR: registra una entrada, salida o ajuste de stock de
' tornillos y tarugos (por caja) o accesorios (por unidad).
' Lo abre frmVerCajas con el producto seleccionado.
Public Class frmMovimientoCajas
    Implements IFormularioTema
    Public idVariante As Integer
    Public producto As String = ""
    Public unidadCosto As String = "CAJA"   ' CAJA o UNIDAD
    Public stockActual As Integer
    Public idUsuario As Integer? = Nothing
    Public Property tema As String = "CLARO" Implements IFormularioTema.tema

    Dim objCaja As New InventarioCaja
    Dim nombreUnidad As String = "cajas"    ' "cajas" o "unidades", para los mensajes

    ' El orden de esta lista es el que usa btnGuardar_Click
    Const ENTRADA_COMPRA As Integer = 0
    Const SALIDA_VENTA As Integer = 1
    Const SALIDA_DEFECTO As Integer = 2
    Const AJUSTE_CONTEO As Integer = 3

    Private Sub frmMovimientoCajas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        Dim p As Paleta = Temas.Obtener(tema)
        Panel2.BackColor = p.Fondo
        lblTitulo.ForeColor = p.Titulo

        nombreUnidad = InventarioCaja.NombreUnidad(unidadCosto)
        txtProducto.Text = producto
        txtStock.Text = stockActual & " " & nombreUnidad
        cmbTipo.Items.AddRange(New String() {"Entrada (compra)", "Salida (venta)", "Salida por defecto", "Ajuste a conteo físico"})
        cmbTipo.SelectedIndex = ENTRADA_COMPRA
    End Sub

    ' En el ajuste se escribe cuánto hay realmente, no cuánto entra o sale
    Private Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged
        lblCantidad.Text = If(cmbTipo.SelectedIndex = AJUSTE_CONTEO, "Stock real *", "Cantidad *")
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cantidad As Integer = CInt(nudCantidad.Value)
        If cmbTipo.SelectedIndex <> AJUSTE_CONTEO AndAlso cantidad <= 0 Then
            MostrarAviso("Ingrese la cantidad de " & nombreUnidad & ".")
            Return
        End If
        If cmbTipo.SelectedIndex = AJUSTE_CONTEO AndAlso
           Not Confirmar("¿El conteo físico es de " & cantidad & " " & nombreUnidad & "? El stock quedará exactamente en ese valor.") Then Return
        Try
            Dim stock As Integer
            Select Case cmbTipo.SelectedIndex
                Case ENTRADA_COMPRA : stock = objCaja.SumarStock(idVariante, cantidad, "COMPRA", idUsuario, Nothing, txtObs.Text)
                Case SALIDA_VENTA : stock = objCaja.RestarStock(idVariante, cantidad, "VENTA", idUsuario, Nothing, txtObs.Text)
                Case SALIDA_DEFECTO : stock = objCaja.RestarStock(idVariante, cantidad, "DEFECTO", idUsuario, Nothing, txtObs.Text)
                Case Else : stock = objCaja.AjustarStock(idVariante, cantidad, idUsuario, txtObs.Text)
            End Select
            MostrarInfo("Movimiento registrado. Stock actual: " & stock & " " & nombreUnidad & ".")
            DialogResult = DialogResult.OK   ' cierra el form y avisa que hubo cambios
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub
End Class

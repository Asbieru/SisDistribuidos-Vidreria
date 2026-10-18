Imports System.Data
Imports capaDatos

' Modelo de ventana data-driven: se carga de las tablas
' modelos_ventana / modelo_ventana_variable / modelo_ventana_componente /
' modelo_ventana_validacion y se evalua con DataColumn.Expression.
Public Class ModeloVentana
    Public Property IdModelo As Integer
    Public Property Nombre As String
    Public Property Descripcion As String
    Public Property Orden As Integer

    Public ReadOnly Variables As New List(Of VariableModelo)
    Public ReadOnly Componentes As New List(Of ComponenteVentana)
    Public ReadOnly Validaciones As New List(Of ValidacionModelo)

    Public Class VariableModelo
        Public Property Nombre As String
        Public Property Formula As String
    End Class

    Public Class ComponenteVentana
        Public Property Categoria As String
        Public Property Tipo As String
        Public Property RolColor As String
        Public Property TipoMedida As String
        Public Property ExprLargo As String
        Public Property ExprCantidad As String
        Public Property ExprAncho As String
        Public Property ExprAlto As String
        Public Property Condicion As String
    End Class

    Public Class ValidacionModelo
        Public Property Condicion As String
        Public Property Mensaje As String
    End Class

    ' Una linea ya resuelta del modelo (material por largo/cantidad o vidrio por ancho+alto)
    Public Class LineaVentana
        Public Property Categoria As String
        Public Property Tipo As String
        Public Property RolColor As String
        Public Property TipoMedida As String
        Public Property LargoCm As Decimal?
        Public Property Cantidad As Integer?
        Public Property AnchoCm As Decimal?
        Public Property AltoCm As Decimal?
    End Class

    Public Class ResultadoModelo
        Public ReadOnly Lineas As New List(Of LineaVentana)
        Public ReadOnly Mensajes As New List(Of String)
    End Class

    ' Variables que siempre provee el formulario (ademas de las del modelo)
    Public Shared ReadOnly NombresEstandar As String() = {
        "ancho_abajo", "ancho_arriba", "ancho_centro",
        "alto_izq", "alto_centro", "alto_der",
        "tres_ancho", "tres_alto", "max_ancho", "max_alto", "min_alto"}

    ' ================== CARGA ==================

    Public Shared Function ListarActivos() As List(Of ModeloVentana)
        Return Cargar(0)
    End Function

    Public Shared Function CargarPorId(idModelo As Integer) As ModeloVentana
        Dim lista As List(Of ModeloVentana) = Cargar(idModelo)
        If lista.Count = 0 Then Return Nothing
        Return lista(0)
    End Function

    Private Shared Function Cargar(idModelo As Integer) As List(Of ModeloVentana)
        Dim objMan As New clsMantenimiento
        Dim filtro As String = If(idModelo > 0, " and id_modelo=" & idModelo, "")
        Dim modelos As New List(Of ModeloVentana)
        Dim porId As New Dictionary(Of Integer, ModeloVentana)

        For Each fila As DataRow In objMan.listarComando(
            "select id_modelo, nombre, descripcion, orden from modelos_ventana " &
            "where activo=1" & filtro & " order by orden, nombre").Rows
            Dim m As New ModeloVentana With {
                .IdModelo = CInt(fila("id_modelo")),
                .Nombre = fila("nombre").ToString(),
                .Descripcion = Texto(fila, "descripcion"),
                .Orden = CInt(fila("orden"))}
            modelos.Add(m)
            porId(m.IdModelo) = m
        Next

        If modelos.Count = 0 Then Return modelos

        For Each fila As DataRow In objMan.listarComando(
            "select id_modelo, nombre, formula from modelo_ventana_variable " &
            "where 1=1" & filtro & " order by id_modelo, orden").Rows
            Dim m As ModeloVentana = porId(CInt(fila("id_modelo")))
            m.Variables.Add(New VariableModelo With {
                .Nombre = fila("nombre").ToString(),
                .Formula = fila("formula").ToString()})
        Next

        For Each fila As DataRow In objMan.listarComando(
            "select id_modelo, categoria, tipo, rol_color, tipo_medida, " &
            "expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion " &
            "from modelo_ventana_componente where 1=1" & filtro & " order by id_modelo, orden").Rows
            Dim m As ModeloVentana = porId(CInt(fila("id_modelo")))
            m.Componentes.Add(New ComponenteVentana With {
                .Categoria = fila("categoria").ToString(),
                .Tipo = Texto(fila, "tipo"),
                .RolColor = Texto(fila, "rol_color"),
                .TipoMedida = fila("tipo_medida").ToString().Trim().ToUpper(),
                .ExprLargo = Texto(fila, "expr_largo"),
                .ExprCantidad = Texto(fila, "expr_cantidad"),
                .ExprAncho = Texto(fila, "expr_ancho"),
                .ExprAlto = Texto(fila, "expr_alto"),
                .Condicion = Texto(fila, "condicion")})
        Next

        For Each fila As DataRow In objMan.listarComando(
            "select id_modelo, condicion, mensaje from modelo_ventana_validacion " &
            "where 1=1" & filtro & " order by id_modelo, id_validacion").Rows
            Dim m As ModeloVentana = porId(CInt(fila("id_modelo")))
            m.Validaciones.Add(New ValidacionModelo With {
                .Condicion = fila("condicion").ToString(),
                .Mensaje = fila("mensaje").ToString()})
        Next

        Return modelos
    End Function

    ' ================== EVALUACION ==================

    ' Resuelve las lineas de materiales/vidrios y los mensajes de validacion
    Public Function Calcular(entradas As Dictionary(Of String, Decimal)) As ResultadoModelo
        Dim resultado As New ResultadoModelo
        Dim tabla As DataTable = ConstruirTabla(entradas)
        Dim fila As DataRow = tabla.Rows(0)

        For Each v As ValidacionModelo In Validaciones
            If EvaluarBooleano(tabla, fila, v.Condicion) Then resultado.Mensajes.Add(v.Mensaje)
        Next

        For Each c As ComponenteVentana In Componentes
            If Not String.IsNullOrWhiteSpace(c.Condicion) AndAlso
               Not EvaluarBooleano(tabla, fila, c.Condicion) Then
                Continue For
            End If

            Dim linea As New LineaVentana With {
                .Categoria = c.Categoria,
                .Tipo = c.Tipo,
                .RolColor = If(String.IsNullOrWhiteSpace(c.RolColor), "MATERIAL", c.RolColor),
                .TipoMedida = c.TipoMedida}

            If c.TipoMedida = "CANTIDAD" Then
                Dim cant As Decimal? = EvaluarNumero(tabla, fila, c.ExprCantidad)
                linea.Cantidad = If(cant.HasValue, CInt(Decimal.Round(cant.Value, 0)), CType(Nothing, Integer?))
            ElseIf c.TipoMedida = "VIDRIO" Then
                linea.AnchoCm = EvaluarNumero(tabla, fila, c.ExprAncho)
                linea.AltoCm = EvaluarNumero(tabla, fila, c.ExprAlto)
            Else
                linea.LargoCm = EvaluarNumero(tabla, fila, c.ExprLargo)
            End If

            resultado.Lineas.Add(linea)
        Next

        Return resultado
    End Function

    Private Function ConstruirTabla(entradas As Dictionary(Of String, Decimal)) As DataTable
        Dim tabla As New DataTable()

        For Each nombre As String In NombresEstandar
            tabla.Columns.Add(nombre, GetType(Double))
        Next

        Dim fila As DataRow = tabla.NewRow()
        For Each nombre As String In NombresEstandar
            If entradas.ContainsKey(nombre) Then fila(nombre) = Convert.ToDouble(entradas(nombre))
        Next
        tabla.Rows.Add(fila)

        ' Las variables del modelo se agregan en orden, para que una pueda usar a las anteriores
        For Each v As VariableModelo In Variables
            If String.IsNullOrWhiteSpace(v.Nombre) Then Continue For
            Dim col As New DataColumn(v.Nombre, GetType(Double)) With {.Expression = v.Formula}
            tabla.Columns.Add(col)
        Next

        Return tabla
    End Function

    Private Function EvaluarNumero(tabla As DataTable, fila As DataRow, expr As String) As Decimal?
        If String.IsNullOrWhiteSpace(expr) Then Return Nothing
        Dim col As New DataColumn("__tmp_num", GetType(Double)) With {.Expression = expr}
        Try
            tabla.Columns.Add(col)
            Dim valor As Object = fila("__tmp_num")
            If valor Is Nothing OrElse IsDBNull(valor) Then Return Nothing
            Return Convert.ToDecimal(valor)
        Finally
            If tabla.Columns.Contains("__tmp_num") Then tabla.Columns.Remove("__tmp_num")
        End Try
    End Function

    Private Function EvaluarBooleano(tabla As DataTable, fila As DataRow, expr As String) As Boolean
        If String.IsNullOrWhiteSpace(expr) Then Return False
        Dim col As New DataColumn("__cond", GetType(Boolean)) With {.Expression = expr}
        Try
            tabla.Columns.Add(col)
            Dim valor As Object = fila("__cond")
            If valor Is Nothing OrElse IsDBNull(valor) Then Return False
            Return Convert.ToBoolean(valor)
        Finally
            If tabla.Columns.Contains("__cond") Then tabla.Columns.Remove("__cond")
        End Try
    End Function

    Private Shared Function Texto(fila As DataRow, columna As String) As String
        If IsDBNull(fila(columna)) Then Return ""
        Return fila(columna).ToString().Trim()
    End Function
End Class

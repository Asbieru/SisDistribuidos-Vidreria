Imports System.Collections.Generic

' Representacion local del modelo de ventana. La carga y la evaluacion
' se ejecutan en capaWSLibrerias; aqui solo se invoca al proxy SOAP.
Public Class ModeloVentana

    Public Property IdModelo As Integer
    Public Property Nombre As String
    Public Property Descripcion As String
    Public Property Orden As Integer

    Public Property Variables As New List(Of VariableModelo)
    Public Property Componentes As New List(Of ComponenteVentana)
    Public Property Validaciones As New List(Of ValidacionModelo)

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
        Public Property Lineas As New List(Of LineaVentana)
        Public Property Mensajes As New List(Of String)
    End Class

    Public Shared ReadOnly NombresEstandar As String() = {
        "ancho_abajo", "ancho_arriba", "ancho_centro",
        "alto_izq", "alto_centro", "alto_der",
        "tres_ancho", "tres_alto", "max_ancho", "max_alto", "min_alto", "alero"}

    Public Shared Function ListarActivos() As List(Of ModeloVentana)
        Return ServicioWeb.Valor(Of List(Of ModeloVentana))(
            ServicioWeb.Ejecutar("ModeloVentanaWS", "ListarActivos", Nothing))
    End Function

    Public Shared Function CargarPorId(idModelo As Integer) As ModeloVentana
        Return ServicioWeb.Valor(Of ModeloVentana)(
            ServicioWeb.Ejecutar("ModeloVentanaWS", "CargarPorId", New Object() {idModelo}))
    End Function

    Public Function Calcular(entradas As Dictionary(Of String, Decimal)) As ResultadoModelo
        Return ServicioWeb.Valor(Of ResultadoModelo)(
            ServicioWeb.Ejecutar("ModeloVentanaWS", "Calcular",
                                 New Object() {IdModelo, entradas}))
    End Function

End Class

' Contrato para todos los formularios hijos de MenuInicio
' (listados, registros, inventario, reportes, etc.).
' El tema lo decide siempre el padre: los hijos solo lo reciben y lo aplican.
Public Interface IFormularioTema
    Property tema As String
End Interface

Module Temas
    Public Structure Paleta
        Public Fondo As Color
        Public Texto As Color
        Public FondoPanel As Color
        Public BotonTexto As Color
        Public BotonFondo As Color
        Public Acento As Color
    End Structure

    Public Function Obtener(tema As String) As Paleta
        Dim p As New Paleta
        Select Case tema
            Case "OSCURO"
                p.Fondo = Color.FromArgb(32, 32, 32)
                p.FondoPanel = Color.FromArgb(45, 45, 48)
                p.Texto = Color.White
                p.BotonTexto = Color.White
                p.BotonFondo = Color.FromArgb(70, 70, 76)
                p.Acento = Color.LightBlue
            Case "AZULCLARO"
                p.Fondo = Color.FromArgb(235, 244, 255)
                p.FondoPanel = Color.FromArgb(205, 225, 245)
                p.Texto = Color.FromArgb(12, 40, 80)
                p.BotonTexto = Color.White
                p.BotonFondo = Color.FromArgb(51, 122, 183)
                p.Acento = Color.FromArgb(0, 90, 158)
            Case "AZULOSCURO"
                p.Fondo = Color.FromArgb(16, 28, 52)
                p.FondoPanel = Color.FromArgb(26, 42, 74)
                p.Texto = Color.White
                p.BotonTexto = Color.White
                p.BotonFondo = Color.FromArgb(40, 70, 120)
                p.Acento = Color.FromArgb(120, 190, 255)
            Case Else
                p.Fondo = Color.White
                p.FondoPanel = Color.FromArgb(242, 244, 247)
                p.Texto = Color.Black
                p.BotonTexto = Color.Black
                p.BotonFondo = Color.Gainsboro
                p.Acento = Color.DarkBlue
        End Select
        Return p
    End Function

    ' True para los temas de fondo oscuro (oscuridad del texto/fondo).
    Public Function EsOscuro(tema As String) As Boolean
        Return tema = "OSCURO" OrElse tema = "AZULOSCURO"
    End Function

    ' Carpeta de imagenes/iconos del tema. Los temas azules pueden tener su
    ' propia carpeta; si no existe, en MenuInicio se cae a la equivalente.
    Public Function Carpeta(tema As String) As String
        Select Case tema
            Case "AZULCLARO" : Return "AzulClaro"
            Case "AZULOSCURO" : Return "AzulOscuro"
            Case "OSCURO" : Return "Oscuro"
            Case Else : Return "Claro"
        End Select
    End Function
End Module

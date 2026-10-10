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
        Public BotonTexto As Color
        Public BotonFondo As Color
        Public Acento As Color
    End Structure

    Public Function Obtener(tema As String) As Paleta
        Dim p As New Paleta
        Select Case tema
            Case "OSCURO"
                p.Fondo = Color.FromArgb(32, 32, 32)
                p.Texto = Color.White
                p.BotonTexto = Color.White
                p.BotonFondo = Color.Gray
                p.Acento = Color.LightBlue
            Case "CLARO"
                p.Fondo = Color.White
                p.Texto = Color.Black
                p.BotonTexto = Color.Black
                p.BotonFondo = Color.Gainsboro
                p.Acento = Color.DarkBlue
        End Select
        Return p
    End Function
End Module

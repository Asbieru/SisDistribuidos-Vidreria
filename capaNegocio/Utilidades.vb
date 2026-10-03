Module Utilidades
    ' Escapa comillas simples para reducir el riesgo al armar SQL por concatenación.
    ' No reemplaza a una consulta parametrizada, es solo una mitigación básica.
    Public Function Esc(s As String) As String
        If s Is Nothing Then Return ""
        Return s.Replace("'", "''")
    End Function

    ' Convierte un decimal a texto con punto como separador, sin importar el idioma
    ' configurado en Windows (en Perú el separador regional es la coma, y eso rompe el SQL)
    Public Function Num(valor As Decimal) As String
        Return valor.ToString(Globalization.CultureInfo.InvariantCulture)
    End Function

    Public Function Num(valor As Decimal?) As String
        If valor.HasValue Then Return Num(valor.Value)
        Return "NULL"
    End Function

    Public Function NumOrNull(valor As Integer?) As String
        If valor.HasValue Then Return valor.Value.ToString()
        Return "NULL"
    End Function
End Module

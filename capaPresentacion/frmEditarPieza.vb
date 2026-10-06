Imports capaNegocio

' EDITAR: corta una pieza o cambia su estado (reservar, liberar, vender, defectuosa).
' Lo abre frmVerPiezas con la pieza seleccionada.
Public Class frmEditarPieza
    Public idPieza As Integer
    Public idUsuario As Integer? = Nothing
    Public tema As String = "CLARO"

    Dim objPieza As New InventarioPieza

    Private Sub frmEditarPieza_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AplicarTema(Me, tema)
        lblNotaCorte.ForeColor = Color.Gray
        lblNotaEstado.ForeColor = Color.Gray
        CargarPieza()
    End Sub

    ' Muestra los datos de la pieza y habilita solo lo que corresponde
    Private Sub CargarPieza()
        Try
            Dim dt As DataTable = objPieza.Obtener(idPieza)
            If dt.Rows.Count = 0 Then
                lblInfo.Text = "La pieza #" & idPieza & " no existe."
                grpCortar.Enabled = False
                grpEstado.Enabled = False
                Return
            End If

            Dim fila As DataRow = dt.Rows(0)
            Dim esVidrio As Boolean = IsDBNull(fila("largo_cm"))
            Dim estado As String = fila("estado").ToString()
            Dim activa As Boolean = (estado = "DISPONIBLE" OrElse estado = "RESERVADA")

            lblInfo.Text = "Pieza #" & idPieza & " - " & fila("producto").ToString() & vbCrLf &
                           TextoMedida(fila("ancho_cm"), fila("alto_cm"), fila("largo_cm")) & "  -  Estado: " & estado

            grpCortar.Enabled = activa
            nudCorteAncho.Enabled = esVidrio
            nudCorteAlto.Enabled = esVidrio
            nudCorteLargo.Enabled = Not esVidrio

            grpEstado.Enabled = activa
            btnReservar.Enabled = (estado = "DISPONIBLE")
            btnLiberar.Enabled = (estado = "RESERVADA")
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    ' ---------------- Cortar ----------------
    Private Sub btnCortar_Click(sender As Object, e As EventArgs) Handles btnCortar.Click
        If Not Confirmar("¿Cortar la pieza #" & idPieza & "? La pieza original quedará CONSUMIDA.") Then Return
        Try
            Dim retazos As DataTable = objPieza.Cortar(idPieza, Medida(nudCorteAncho), Medida(nudCorteAlto), Medida(nudCorteLargo),
                                                       idUsuario, Nothing, nudMinUtil.Value)
            Dim msj As String = "Corte registrado."
            If retazos.Rows.Count = 0 Then
                msj &= vbCrLf & "No quedó ningún retazo útil."
            Else
                msj &= vbCrLf & "Retazos generados:"
                For Each r As DataRow In retazos.Rows
                    msj &= vbCrLf & "  - Pieza #" & r("id_pieza").ToString() & ": " & TextoMedida(r("ancho_cm"), r("alto_cm"), r("largo_cm"))
                Next
            End If
            MostrarInfo(msj)
            DialogResult = DialogResult.OK   ' cierra el form y avisa que hubo cambios
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub

    ' ---------------- Cambiar estado ----------------
    Private Sub btnReservar_Click(sender As Object, e As EventArgs) Handles btnReservar.Click
        CambiarEstado("RESERVADA", Nothing)
    End Sub

    Private Sub btnLiberar_Click(sender As Object, e As EventArgs) Handles btnLiberar.Click
        CambiarEstado("DISPONIBLE", Nothing)
    End Sub

    Private Sub btnVendida_Click(sender As Object, e As EventArgs) Handles btnVendida.Click
        CambiarEstado("VENDIDA", "¿Marcar la pieza como VENDIDA entera? No se podrá revertir.")
    End Sub

    Private Sub btnDefectuosa_Click(sender As Object, e As EventArgs) Handles btnDefectuosa.Click
        CambiarEstado("DEFECTUOSA", "¿Marcar la pieza como DEFECTUOSA? No se podrá revertir.")
    End Sub

    Private Sub CambiarEstado(estado As String, confirmacion As String)
        If confirmacion IsNot Nothing AndAlso Not Confirmar(confirmacion) Then Return
        Try
            objPieza.MarcarEstado(idPieza, estado, idUsuario)
            DialogResult = DialogResult.OK   ' cierra el form y avisa que hubo cambios
        Catch ex As Exception
            MostrarError(ex)
        End Try
    End Sub
End Class

Imports capaDatos


Public Class Bitacora
    Dim objMan As New clsMantenimiento
    Dim strCon As String

    ' ---------------- Acciones que registra el sistema (máximo 15 caracteres) ----------------
    Public Const INICIO_SESION As String = "INICIO_SESION"
    Public Const CIERRE_SESION As String = "CIERRE_SESION"
    Public Const USUARIO_BAJA As String = "USUARIO_BAJA"
    Public Const CLAVE_ERRADA As String = "CLAVE_ERRADA"
    Public Const SIN_INTENTOS As String = "SIN_INTENTOS"
    Public Const RESP_ERRADA As String = "RESP_ERRADA"
    Public Const RECUPERA_CLAVE As String = "RECUPERA_CLAVE"

    Public Shared ReadOnly Acciones As String() = {
        INICIO_SESION, CIERRE_SESION, USUARIO_BAJA, CLAVE_ERRADA, SIN_INTENTOS, RESP_ERRADA, RECUPERA_CLAVE}

    ' Acciones que son incidencias (algo salió mal): se resaltan en el reporte
    Public Shared Function esIncidencia(accion As String) As Boolean
        Return accion = USUARIO_BAJA OrElse accion = CLAVE_ERRADA OrElse
               accion = SIN_INTENTOS OrElse accion = RESP_ERRADA
    End Function

    ' ---------------- Registrar ----------------

    ' Guarda una acción del usuario. numIntentos es opcional (intentos fallidos de contraseña).
    ' idRegistro no es autoincremental: se calcula como el último + 1 dentro del mismo insert.
    Public Sub registrarAccion(idUsuario As Integer, accion As String, Optional numIntentos As Integer? = Nothing)
        strCon = "insert into bitacora_login (idRegistro, fechaReg, horaReg, id_usuario, accion, numIntentos) " &
                 "select isnull(max(idRegistro), 0) + 1, @fechaReg, @horaReg, @id_usuario, @accion, @numIntentos " &
                 "from bitacora_login with (updlock, holdlock)"
        ' Hora de Perú (UTC-5), sin depender de la zona horaria del servidor
        Dim ahora As Date = Date.UtcNow.AddHours(-5)
        Try
            objMan.ejecutarConParametros(strCon, New Dictionary(Of String, Object) From {
                {"fechaReg", ahora.Date},
                {"horaReg", ahora.ToString("HH:mm:ss")},
                {"id_usuario", idUsuario},
                {"accion", accion},
                {"numIntentos", numIntentos}})
        Catch ex As Exception
            Throw New Exception("Error al registrar en la bitácora!")
        End Try
    End Sub

    ' Igual que registrarAccion, pero si falla no lanza ningún error:
    ' la bitácora nunca debe impedir que el usuario trabaje (por ejemplo, iniciar sesión).
    Public Sub registrarAccionSeguro(idUsuario As Integer, accion As String, Optional numIntentos As Integer? = Nothing)
        Try
            registrarAccion(idUsuario, accion, numIntentos)
        Catch ex As Exception
            ' Se ignora a propósito: la operación principal continúa aunque no se pueda registrar
        End Try
    End Sub

    ' ---------------- Consultar ----------------

    ' Todos los registros, del más reciente al más antiguo
    Public Function listarBitacora() As DataTable
        strCon = consultaBase() & orden()
        Try
            Return objMan.consultarConParametros(strCon)
        Catch ex As Exception
            Throw New Exception("Error al listar la bitácora!")
        End Try
    End Function

    ' Registros filtrados. hasta = Nothing: sin límite superior.
    ' accion / usuario vacíos = todos.
    Public Function buscarBitacora(desde As Date, hasta As Date?, Optional accion As String = Nothing,
                                   Optional usuario As String = Nothing) As DataTable
        strCon = consultaBase() &
                 "where b.fechaReg >= @desde and (@hasta is null or b.fechaReg <= @hasta) " &
                 "and (@accion is null or b.accion = @accion) " &
                 "and (@usuario is null or u.nombre like '%' + @usuario + '%') " & orden()
        Try
            Return objMan.consultarConParametros(strCon, New Dictionary(Of String, Object) From {
                {"desde", desde.Date},
                {"hasta", If(hasta.HasValue, hasta.Value.Date, CType(Nothing, Date?))},
                {"accion", If(String.IsNullOrWhiteSpace(accion), Nothing, accion)},
                {"usuario", If(String.IsNullOrWhiteSpace(usuario), Nothing, usuario.Trim())}})
        Catch ex As Exception
            Throw New Exception("Error al buscar en la bitácora!")
        End Try
    End Function

    ' Acciones que ya existen en la tabla (incluye las que guardaron otros sistemas),
    ' para llenar el filtro del reporte
    Public Function listarAcciones() As List(Of String)
        strCon = "select distinct accion from bitacora_login order by accion"
        Dim lista As New List(Of String)
        Try
            For Each fila As DataRow In objMan.consultarConParametros(strCon).Rows
                lista.Add(fila("accion").ToString())
            Next
            Return lista
        Catch ex As Exception
            Throw New Exception("Error al listar las acciones de la bitácora!")
        End Try
    End Function

    ' Registros de una sola acción (por ejemplo, solo CLAVE_ERRADA)
    Public Function listarPorAccion(accion As String) As DataTable
        strCon = consultaBase() & "where b.accion = @accion " & orden()
        Try
            Return objMan.consultarConParametros(strCon, New Dictionary(Of String, Object) From {{"accion", accion}})
        Catch ex As Exception
            Throw New Exception("Error al filtrar la bitácora por acción!")
        End Try
    End Function

    ' Historial de un solo usuario
    Public Function listarPorUsuario(idUsuario As Integer) As DataTable
        strCon = consultaBase() & "where b.id_usuario = @id_usuario " & orden()
        Try
            Return objMan.consultarConParametros(strCon, New Dictionary(Of String, Object) From {{"id_usuario", idUsuario}})
        Catch ex As Exception
            Throw New Exception("Error al listar la bitácora del usuario!")
        End Try
    End Function

    ' Cuántas veces ocurrió cada acción en un rango de fechas (resumen del reporte)
    Public Function contarPorAccion(desde As Date, hasta As Date?) As DataTable
        strCon = "select accion, count(*) as cantidad from bitacora_login " &
                 "where fechaReg >= @desde and (@hasta is null or fechaReg <= @hasta) " &
                 "group by accion order by cantidad desc"
        Try
            Return objMan.consultarConParametros(strCon, New Dictionary(Of String, Object) From {
                {"desde", desde.Date},
                {"hasta", If(hasta.HasValue, hasta.Value.Date, CType(Nothing, Date?))}})
        Catch ex As Exception
            Throw New Exception("Error al contar las acciones de la bitácora!")
        End Try
    End Function

    ' ---------------- Auxiliares ----------------

    Private Function consultaBase() As String
        Return "select b.idRegistro, b.fechaReg, b.horaReg, u.nombre as usuario, u.rol, b.accion, b.numIntentos " &
               "from bitacora_login b left join usuarios u on u.id_usuario = b.id_usuario "
    End Function

    Private Function orden() As String
        Return "order by b.fechaReg desc, b.horaReg desc, b.idRegistro desc"
    End Function
End Class

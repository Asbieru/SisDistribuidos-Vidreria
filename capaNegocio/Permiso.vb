Imports capaDatos
Imports System.Collections.Generic

Public Class Permiso
    Dim objMan As New clsMantenimiento

    ' Lista de todos los permisos que existen en el sistema
    Public Function listarTiposPermiso() As DataTable
        Try
            Return objMan.listarComando("select id_tipo, codigo, nombre, descripcion from tipo_permiso order by id_tipo")
        Catch ex As Exception
            Throw New Exception("Error al listar tipos de permisos!")
        End Try
    End Function

    ' Permisos que tiene asignados un usuario
    Public Function listarPermisosUsuario(idUsuario As Integer) As DataTable
        Try
            Return objMan.consultarConParametros(
                "select t.id_tipo, t.codigo, t.nombre from permiso p " &
                "inner join tipo_permiso t on t.id_tipo = p.id_tipo " &
                "where p.id_usuario = @id_usuario order by t.id_tipo",
                New Dictionary(Of String, Object) From {{"id_usuario", idUsuario}})
        Catch ex As Exception
            Throw New Exception("Error al listar permisos del usuario!")
        End Try
    End Function

    ' Permisos por defecto de un rol (VENDEDOR / ADMINISTRADOR)
    Public Function listarPermisosRol(rol As String) As DataTable
        Try
            Return objMan.consultarConParametros(
                "select t.id_tipo, t.codigo, t.nombre from rol_permiso r " &
                "inner join tipo_permiso t on t.id_tipo = r.id_tipo " &
                "where r.rol = @rol order by t.id_tipo",
                New Dictionary(Of String, Object) From {{"rol", rol}})
        Catch ex As Exception
            Throw New Exception("Error al listar permisos del rol!")
        End Try
    End Function

    ' Solo los códigos (USUARIOS, CLIENTES...): lo usa el menú principal para mostrar u ocultar opciones
    Public Function codigosPermisoUsuario(idUsuario As Integer) As List(Of String)
        Dim lista As New List(Of String)
        For Each fila As DataRow In listarPermisosUsuario(idUsuario).Rows
            lista.Add(fila("codigo").ToString)
        Next
        Return lista
    End Function

    ' Reemplaza todos los permisos del usuario por los de la lista, en una sola transacción
    Public Sub guardarPermisos(idUsuario As Integer, idsTipo As List(Of Integer))
        Dim sql As String = "set xact_abort on; begin transaction; " &
                            "delete from permiso where id_usuario = @id_usuario; "
        If idsTipo.Count > 0 Then
            ' Los ids son números enteros, por eso se pueden unir con comas sin riesgo
            sql &= "insert into permiso (id_usuario, id_tipo) select @id_usuario, id_tipo from tipo_permiso " &
                   "where id_tipo in (" & String.Join(",", idsTipo) & "); "
        End If
        sql &= "commit transaction;"
        Try
            objMan.ejecutarConParametros(sql, New Dictionary(Of String, Object) From {{"id_usuario", idUsuario}})
        Catch ex As Exception
            Throw New Exception("Error al guardar permisos!")
        End Try
    End Sub

    ' Reemplaza los permisos del usuario por los que tiene por defecto su rol
    ' (se usa al registrar un usuario nuevo o al cambiarle el rol)
    Public Sub asignarPermisosRol(idUsuario As Integer, rol As String)
        Dim sql As String = "set xact_abort on; begin transaction; " &
                            "delete from permiso where id_usuario = @id_usuario; " &
                            "insert into permiso (id_usuario, id_tipo) " &
                            "select @id_usuario, id_tipo from rol_permiso where rol = @rol; " &
                            "commit transaction;"
        Try
            objMan.ejecutarConParametros(sql, New Dictionary(Of String, Object) From {{"id_usuario", idUsuario}, {"rol", rol}})
        Catch ex As Exception
            Throw New Exception("Error al asignar los permisos del rol!")
        End Try
    End Sub

    ' Cuántos OTROS usuarios activos tienen un permiso (para no dejar el sistema sin nadie que pueda administrarlo)
    Public Function contarOtrosConPermiso(codigo As String, idExcluir As Integer) As Integer
        Try
            Dim dt As DataTable = objMan.consultarConParametros(
                "select count(*) as n from permiso p " &
                "inner join tipo_permiso t on t.id_tipo = p.id_tipo " &
                "inner join usuarios u on u.id_usuario = p.id_usuario " &
                "where t.codigo = @codigo and u.activo = 1 and p.id_usuario <> @id_excluir",
                New Dictionary(Of String, Object) From {{"codigo", codigo}, {"id_excluir", idExcluir}})
            Return CInt(dt.Rows(0)("n"))
        Catch ex As Exception
            Throw New Exception("Error al validar permisos!")
        End Try
    End Function
End Class
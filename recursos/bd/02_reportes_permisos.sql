-- ============================================================
-- Permiso REPORTES para instalaciones existentes.
-- 00_vidrieria_completo.sql ya siembra REPORTES, pero un administrador
-- creado antes de esa siembra podria no tenerlo en la tabla permiso.
-- Este script es idempotente: se puede ejecutar varias veces.
-- ============================================================

IF OBJECT_ID(N'dbo.tipo_permiso', N'U') IS NULL
    OR OBJECT_ID(N'dbo.rol_permiso', N'U') IS NULL
    OR OBJECT_ID(N'dbo.usuarios', N'U') IS NULL
    OR OBJECT_ID(N'dbo.permiso', N'U') IS NULL
BEGIN
    RAISERROR(N'Faltan las tablas de usuarios y permisos. Ejecute primero 00_vidrieria_completo.sql.', 16, 1);
    RETURN;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tipo_permiso WHERE codigo = N'REPORTES')
BEGIN
    INSERT INTO dbo.tipo_permiso (codigo, nombre, descripcion)
    VALUES (N'REPORTES', N'Reportes', N'Ver los reportes del negocio');
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.rol_permiso r
    INNER JOIN dbo.tipo_permiso t ON t.id_tipo = r.id_tipo
    WHERE r.rol = N'ADMINISTRADOR' AND t.codigo = N'REPORTES'
)
BEGIN
    INSERT INTO dbo.rol_permiso (rol, id_tipo)
    SELECT N'ADMINISTRADOR', t.id_tipo
    FROM dbo.tipo_permiso t
    WHERE t.codigo = N'REPORTES';
END
GO

INSERT INTO dbo.permiso (id_usuario, id_tipo)
SELECT u.id_usuario, t.id_tipo
FROM dbo.usuarios u
INNER JOIN dbo.tipo_permiso t ON t.codigo = N'REPORTES'
WHERE u.rol = N'ADMINISTRADOR'
  AND u.activo = 1
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.permiso p
      WHERE p.id_usuario = u.id_usuario
        AND p.id_tipo = t.id_tipo
  );
GO

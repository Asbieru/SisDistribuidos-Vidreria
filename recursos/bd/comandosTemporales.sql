-- ============================================================
-- comandosTemporales.sql
-- Cambios incrementales que hay que aplicar sobre una base de
-- datos YA EXISTENTE (por ejemplo, la subida a somee.com).
--
-- El script completo y consolidado sigue estando en
--   00_vidrieria_completo.sql
-- (es idempotente: se puede volver a ejecutar entero sin errores).
-- Este archivo sirve solo para llevar los cambios puntuales a la BD
-- remota sin volver a ejecutar todo.
--
-- Ejecutar sobre la base de datos "vidrieria".
-- ============================================================

USE vidrieria;
GO

-- ------------------------------------------------------------
-- 1) Hora de Peru (UTC-5) para todas las fechas del sistema.
-- SYSDATETIME() devuelve la hora del SERVIDOR: en un hosting como
-- somee.com el servidor esta en otra zona horaria y las fechas
-- quedaban adelantadas (el kardex no mostraba los movimientos de
-- "hoy"). 'SA Pacific Standard Time' = Lima/Bogota, UTC-5, sin
-- horario de verano.
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.fn_hora_peru', N'FN') IS NULL
    EXEC(N'CREATE FUNCTION dbo.fn_hora_peru() RETURNS DATETIME2 AS
BEGIN
    RETURN CONVERT(DATETIME2, SYSDATETIMEOFFSET() AT TIME ZONE ''SA Pacific Standard Time'');
END');
GO

-- ------------------------------------------------------------
-- 2) Cambia el valor por defecto de las columnas de fecha de
-- SYSDATETIME() (hora del servidor) a dbo.fn_hora_peru()
-- (hora de Peru). Solo toca el valor por defecto de cada
-- columna; no modifica ningun dato ya guardado.
-- ------------------------------------------------------------
DECLARE @columnas TABLE (tabla SYSNAME, columna SYSNAME);
INSERT INTO @columnas VALUES
    ('usuarios', 'fecha_creacion'),
    ('usuario_mensajes_descartados', 'fecha_descarte'),
    ('pedidos', 'fecha'),
    ('inventario_piezas', 'fecha_ingreso'),
    ('movimientos_inventario', 'fecha'),
    ('pagos', 'fecha'),
    ('comprobante_pago', 'fecha_emision');

DECLARE @tabla SYSNAME, @columna SYSNAME, @df SYSNAME, @sql NVARCHAR(MAX);
DECLARE cur_fechas CURSOR LOCAL FAST_FORWARD FOR SELECT tabla, columna FROM @columnas;
OPEN cur_fechas;
FETCH NEXT FROM cur_fechas INTO @tabla, @columna;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @df = NULL;
    SELECT @df = dc.name
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID(N'dbo.' + @tabla) AND c.name = @columna
      AND dc.definition NOT LIKE N'%fn_hora_peru%';

    IF @df IS NOT NULL
    BEGIN
        SET @sql = N'ALTER TABLE dbo.' + QUOTENAME(@tabla) + N' DROP CONSTRAINT ' + QUOTENAME(@df) + N'; ' +
                   N'ALTER TABLE dbo.' + QUOTENAME(@tabla) + N' ADD DEFAULT dbo.fn_hora_peru() FOR ' + QUOTENAME(@columna) + N';';
        EXEC sp_executesql @sql;
    END
    FETCH NEXT FROM cur_fechas INTO @tabla, @columna;
END
CLOSE cur_fechas;
DEALLOCATE cur_fechas;
GO

-- ------------------------------------------------------------
-- 3) Procedimientos que usaban SYSDATETIME().
-- Se volvieron a crear con dbo.fn_hora_peru():
--   sp_ped_registrar, sp_ped_terminar_item, sp_ped_terminar.
-- Su definicion completa esta en 00_vidrieria_completo.sql.
-- Como ese script es idempotente, la forma mas simple de poner al
-- dia la BD remota es ejecutarlo completo.
-- ------------------------------------------------------------

-- ------------------------------------------------------------
-- 4) Reclasificacion de productos 2 y 3 como accesorios:
--    id 2 -> ACCESORIO / TARUGO   / PLASTICO
--    id 3 -> ACCESORIO / TORNILLO / AUTORROSCANTE
--    Solo ajusta categoria, tipo y color; no toca nombre ni variantes.
-- ------------------------------------------------------------
UPDATE dbo.productos SET categoria = 'ACCESORIO', tipo = 'TARUGO',   color = 'PLASTICO'      WHERE id_producto = 2;
UPDATE dbo.productos SET categoria = 'ACCESORIO', tipo = 'TORNILLO', color = 'AUTORROSCANTE' WHERE id_producto = 3;
GO

-- ------------------------------------------------------------
-- 5) Nuevos temas de la interfaz (azul claro / azul oscuro).
--    usuario_preferencias.tema tenia un CHECK que solo permitia
--    CLARO y OSCURO: hay que reemplazarlo para aceptar los nuevos
--    valores. (No hay tabla de temas, la preferencia se guarda por
--    usuario en usuario_preferencias.tema.)
-- ------------------------------------------------------------
DECLARE @ck SYSNAME, @sql NVARCHAR(MAX);
SELECT @ck = name
FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID(N'dbo.usuario_preferencias')
  AND definition LIKE N'%tema%';

IF @ck IS NOT NULL
BEGIN
    SET @sql = N'ALTER TABLE dbo.usuario_preferencias DROP CONSTRAINT ' + QUOTENAME(@ck) + N';';
    EXEC sp_executesql @sql;
END

ALTER TABLE dbo.usuario_preferencias ADD CONSTRAINT ck_pref_tema
    CHECK (tema IN ('CLARO', 'OSCURO', 'AZULCLARO', 'AZULOSCURO'));
GO

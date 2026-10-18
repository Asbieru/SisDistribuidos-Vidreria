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

ALTER TABLE dbo.usuario_preferencias ADD CONSTRAINT ck_tema_v2
    CHECK (tema IN ('CLARO', 'OSCURO', 'AZULCLARO', 'AZULOSCURO'));
GO

-- ------------------------------------------------------------
-- 6) Modelos de ventana (data-driven) + id_modelo en pedido_item.
--    Crea las tablas nuevas, agrega pedido_item.id_modelo, agrega los
--    productos NUEVOS al catalogo (J PORTAFELPA y TUBO MULTIPLE SIMPLE).
--
--    IMPORTANTE: el TVP_PedidoItem gano la columna id_modelo. Como SQL
--    Server no permite ALTER TYPE, hay que DROP TYPE + recrear el SP.
--    Este script dropea el tipo (y el SP que lo usa); para recrearlos
--    con la columna nueva ejecutar despues 00_vidrieria_completo.sql,
--    que es idempotente y ademas inserta los 5 modelos base si faltan.
-- ------------------------------------------------------------

IF OBJECT_ID(N'dbo.modelos_ventana', N'U') IS NULL
CREATE TABLE dbo.modelos_ventana (
    id_modelo   INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(255) NULL,
    orden       INT NOT NULL DEFAULT 0,
    activo      BIT NOT NULL DEFAULT 1,
    CONSTRAINT uq_mv_nombre UNIQUE (nombre)
);

IF OBJECT_ID(N'dbo.modelo_ventana_variable', N'U') IS NULL
CREATE TABLE dbo.modelo_ventana_variable (
    id_variable INT IDENTITY(1,1) PRIMARY KEY,
    id_modelo   INT NOT NULL,
    orden       INT NOT NULL DEFAULT 0,
    nombre      NVARCHAR(40) NOT NULL,
    formula     NVARCHAR(300) NOT NULL,
    CONSTRAINT fk_mv_var_modelo FOREIGN KEY (id_modelo)
        REFERENCES dbo.modelos_ventana(id_modelo) ON DELETE CASCADE,
    CONSTRAINT uq_mv_var UNIQUE (id_modelo, nombre)
);

IF OBJECT_ID(N'dbo.modelo_ventana_componente', N'U') IS NULL
CREATE TABLE dbo.modelo_ventana_componente (
    id_componente INT IDENTITY(1,1) PRIMARY KEY,
    id_modelo     INT NOT NULL,
    orden         INT NOT NULL DEFAULT 0,
    categoria     NVARCHAR(20) NOT NULL,
    tipo          NVARCHAR(50) NULL,
    rol_color     NVARCHAR(20) NOT NULL DEFAULT 'MATERIAL',
    tipo_medida   NVARCHAR(10) NOT NULL,
    expr_largo    NVARCHAR(300) NULL,
    expr_cantidad NVARCHAR(300) NULL,
    expr_ancho    NVARCHAR(300) NULL,
    expr_alto     NVARCHAR(300) NULL,
    condicion     NVARCHAR(300) NULL,
    CONSTRAINT fk_mv_comp_modelo FOREIGN KEY (id_modelo)
        REFERENCES dbo.modelos_ventana(id_modelo) ON DELETE CASCADE
);

IF OBJECT_ID(N'dbo.modelo_ventana_validacion', N'U') IS NULL
CREATE TABLE dbo.modelo_ventana_validacion (
    id_validacion INT IDENTITY(1,1) PRIMARY KEY,
    id_modelo     INT NOT NULL,
    condicion     NVARCHAR(300) NOT NULL,
    mensaje       NVARCHAR(255) NOT NULL,
    CONSTRAINT fk_mv_val_modelo FOREIGN KEY (id_modelo)
        REFERENCES dbo.modelos_ventana(id_modelo) ON DELETE CASCADE
);

IF COL_LENGTH('dbo.pedido_item','id_modelo') IS NULL
    ALTER TABLE dbo.pedido_item ADD id_modelo INT NULL;
GO

-- Productos nuevos (solo si faltan; precio = cm lineal)
DECLARE @nuevos TABLE (categoria NVARCHAR(20), tipo NVARCHAR(50), color NVARCHAR(30), nombre NVARCHAR(150), precio DECIMAL(10,2));
INSERT INTO @nuevos VALUES
    ('ALUMINIO','J PORTAFELPA','ESTANDAR','Aluminio J Portafelpa',0.03),
    ('ALUMINIO','J PORTAFELPA','NEGRO','Aluminio J Portafelpa',0.05),
    ('ALUMINIO','J PORTAFELPA','MADERA','Aluminio J Portafelpa',0.08),
    ('ALUMINIO','TUBO MULTIPLE SIMPLE','ESTANDAR','Aluminio Tubo Multiple Simple',0.04),
    ('ALUMINIO','TUBO MULTIPLE SIMPLE','NEGRO','Aluminio Tubo Multiple Simple',0.07),
    ('ALUMINIO','TUBO MULTIPLE SIMPLE','MADERA','Aluminio Tubo Multiple Simple',0.12);

DECLARE @categoria NVARCHAR(20), @tipo NVARCHAR(50), @color NVARCHAR(30),
        @nombre NVARCHAR(150), @precio DECIMAL(10,2), @idProd INT, @c_sinmedida INT;

SELECT TOP 1 @c_sinmedida = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm IS NULL AND medida_cm IS NULL ORDER BY id_caracteristica;
IF @c_sinmedida IS NULL
BEGIN
    INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (NULL, NULL);
    SET @c_sinmedida = SCOPE_IDENTITY();
END
DECLARE cur_nuevos CURSOR LOCAL FAST_FORWARD FOR SELECT categoria, tipo, color, nombre, precio FROM @nuevos;
OPEN cur_nuevos;
FETCH NEXT FROM cur_nuevos INTO @categoria, @tipo, @color, @nombre, @precio;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria=@categoria AND tipo=@tipo AND ISNULL(color,'')=ISNULL(@color,''))
    BEGIN
        INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES (@categoria, @tipo, @color, @nombre);
        SET @idProd = SCOPE_IDENTITY();
        INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
        VALUES (@idProd, @c_sinmedida, 'CM_LINEAL', @precio, 0);
    END
    FETCH NEXT FROM cur_nuevos INTO @categoria, @tipo, @color, @nombre, @precio;
END
CLOSE cur_nuevos;
DEALLOCATE cur_nuevos;
GO

-- El TVP cambio de forma: hay que dropear el SP y el tipo. Recrearlos con
-- 00_vidrieria_completo.sql (idempotente).
IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoItem' AND is_table_type = 1)
   AND NOT EXISTS (SELECT 1 FROM sys.columns c
                   INNER JOIN sys.types t ON t.user_type_id = c.user_type_id
                   WHERE t.name = 'TVP_PedidoItem' AND c.name = 'id_modelo')
BEGIN
    IF OBJECT_ID(N'dbo.sp_ped_registrar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ped_registrar;
    DROP TYPE dbo.TVP_PedidoItem;
END
GO

PRINT 'Modelos de ventana: tablas/productos/id_modelo listos. Ahora ejecute 00_vidrieria_completo.sql para recrear TVP_PedidoItem, sp_ped_registrar y cargar los modelos.';
GO

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

-- ------------------------------------------------------------
-- 7) El alero pasa a ser un dato del formulario (editable). Antes estaba
--    fijo en 35 dentro de las formulas del modelo. Se reemplaza 35 por la
--    variable "alero" en las variables de los modelos con alero.
-- ------------------------------------------------------------
UPDATE dbo.modelo_ventana_variable
SET formula = REPLACE(formula, '35', 'alero')
WHERE nombre IN ('anclaje', 'alto_fijo') AND formula LIKE '%35%';
GO

-- ------------------------------------------------------------
-- 8) Faltaba crear sp_ped_recalcular_estados en la BD remota (lo usa
--    frmPedidoUnitario al abrir un pedido). Su definicion completa esta
--    en 00_vidrieria_completo.sql.
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.sp_ped_recalcular_estados', N'P') IS NULL
    EXEC('CREATE PROCEDURE dbo.sp_ped_recalcular_estados AS BEGIN SET NOCOUNT ON; END');
GO

CREATE OR ALTER PROCEDURE dbo.sp_ped_recalcular_estados
    @id_pedido INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE pi
    SET estado = 'EN_PROCESO'
    FROM dbo.pedido_item pi
    WHERE pi.id_pedido = @id_pedido
      AND pi.estado = 'PENDIENTE'
      AND EXISTS (SELECT 1 FROM dbo.pedido_detalle pd
                  WHERE pd.id_item = pi.id_item AND pd.estado = 'ACTIVO')
      AND NOT EXISTS (SELECT 1 FROM dbo.pedido_detalle pd
                      WHERE pd.id_item = pi.id_item AND pd.estado = 'ACTIVO'
                        AND pd.id_pieza IS NULL AND pd.id_compra_detalle IS NULL);
END
GO

PRINT 'Alero editable + sp_ped_recalcular_estados listos.';
GO

-- ------------------------------------------------------------
-- 9) 3 decimales para costo de compra y medidas de pieza:
--    - producto_variante.precio_unitario (ej. felpa 0.009 soles)
--    - compra_detalle: cantidad/costo_unitario/subtotal + nuevas
--      columnas piezas/ancho_cm/alto_cm/largo_cm (medida de UNA pieza)
--    - compras.total
--    - inventario_piezas: medidas de las piezas
--    - sp_inv_ingresar_pieza y sp_inv_cortar_pieza con DECIMAL(9,3)
-- Idempotente: solo altera si la precision actual es menor.
-- ------------------------------------------------------------
IF EXISTS (SELECT 1 FROM sys.columns c
           INNER JOIN sys.tables t ON t.object_id = c.object_id
           WHERE t.name = 'producto_variante' AND c.name = 'precio_unitario'
             AND c.scale < 3)
    ALTER TABLE dbo.producto_variante ALTER COLUMN precio_unitario DECIMAL(10,3) NOT NULL;
GO
IF EXISTS (SELECT 1 FROM sys.columns c
           INNER JOIN sys.tables t ON t.object_id = c.object_id
           WHERE t.name = 'compras' AND c.name = 'total'
             AND c.scale < 3)
    ALTER TABLE dbo.compras ALTER COLUMN total DECIMAL(12,3) NOT NULL;
GO
IF EXISTS (SELECT 1 FROM sys.columns c
           INNER JOIN sys.tables t ON t.object_id = c.object_id
           WHERE t.name = 'compra_detalle' AND c.name IN ('cantidad','costo_unitario','subtotal')
             AND c.scale < 3)
BEGIN
    ALTER TABLE dbo.compra_detalle ALTER COLUMN cantidad DECIMAL(12,3) NOT NULL;
    ALTER TABLE dbo.compra_detalle ALTER COLUMN costo_unitario DECIMAL(12,3) NOT NULL;
    ALTER TABLE dbo.compra_detalle ALTER COLUMN subtotal DECIMAL(12,3) NOT NULL;
END
GO
IF COL_LENGTH('dbo.compra_detalle', 'piezas') IS NULL
    ALTER TABLE dbo.compra_detalle ADD piezas INT NOT NULL
        CONSTRAINT df_cdet_piezas DEFAULT 1;
GO
IF COL_LENGTH('dbo.compra_detalle', 'ancho_cm') IS NULL
    ALTER TABLE dbo.compra_detalle ADD ancho_cm DECIMAL(9,3) NULL;
GO
IF COL_LENGTH('dbo.compra_detalle', 'alto_cm') IS NULL
    ALTER TABLE dbo.compra_detalle ADD alto_cm DECIMAL(9,3) NULL;
GO
IF COL_LENGTH('dbo.compra_detalle', 'largo_cm') IS NULL
    ALTER TABLE dbo.compra_detalle ADD largo_cm DECIMAL(9,3) NULL;
GO
IF EXISTS (SELECT 1 FROM sys.columns c
           INNER JOIN sys.tables t ON t.object_id = c.object_id
           WHERE t.name = 'inventario_piezas' AND c.name IN ('ancho_cm','alto_cm','largo_cm')
             AND c.scale < 3)
BEGIN
    -- idx_piezas_medidas depende de ancho_cm: se quita y se vuelve a crear
    IF EXISTS (SELECT 1 FROM sys.indexes
               WHERE name = 'idx_piezas_medidas' AND object_id = OBJECT_ID(N'dbo.inventario_piezas'))
        DROP INDEX idx_piezas_medidas ON dbo.inventario_piezas;

    ALTER TABLE dbo.inventario_piezas ALTER COLUMN ancho_cm DECIMAL(9,3) NULL;
    ALTER TABLE dbo.inventario_piezas ALTER COLUMN alto_cm DECIMAL(9,3) NULL;
    ALTER TABLE dbo.inventario_piezas ALTER COLUMN largo_cm DECIMAL(9,3) NULL;

    CREATE INDEX idx_piezas_medidas ON dbo.inventario_piezas(ancho_cm, alto_cm);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_inv_ingresar_pieza
    @id_variante        INT,
    @ancho_cm           DECIMAL(9,3) = NULL,
    @alto_cm            DECIMAL(9,3) = NULL,
    @largo_cm           DECIMAL(9,3) = NULL,
    @origen             NVARCHAR(10),
    @id_compra_detalle  INT = NULL,
    @id_pieza_origen    INT = NULL,
    @id_usuario         INT = NULL,
    @observacion        NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;

    DECLARE @unidad NVARCHAR(10) = (SELECT unidad_costo FROM dbo.producto_variante WHERE id_variante = @id_variante);

    IF @unidad IS NULL
        THROW 50001, 'La variante de producto no existe.', 1;
    IF @unidad IN ('CAJA','UNIDAD')
        THROW 50002, 'Tornillos, tarugos y accesorios por unidad se manejan por cantidad, no por piezas.', 1;
    IF @unidad IN ('CM2','PIE2') AND (ISNULL(@ancho_cm, 0) <= 0 OR ISNULL(@alto_cm, 0) <= 0)
        THROW 50003, 'Para vidrio ingrese ancho y alto mayores a cero.', 1;
    IF @unidad = 'CM_LINEAL' AND ISNULL(@largo_cm, 0) <= 0
        THROW 50004, 'Para aluminio ingrese el largo mayor a cero.', 1;
    IF @origen = 'COMPRA' AND @id_compra_detalle IS NULL
        THROW 50005, 'Una pieza de origen COMPRA debe indicar el detalle de compra.', 1;
    IF @origen = 'RETAZO' AND @id_pieza_origen IS NULL
        THROW 50006, 'Un retazo debe indicar la pieza de la que salio.', 1;

    IF @unidad IN ('CM2','PIE2') SET @largo_cm = NULL;
    IF @unidad = 'CM_LINEAL' SELECT @ancho_cm = NULL, @alto_cm = NULL;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;

        INSERT INTO dbo.inventario_piezas (id_variante, id_pieza_origen, id_compra_detalle, ancho_cm, alto_cm, largo_cm, estado, origen)
        VALUES (@id_variante, @id_pieza_origen, @id_compra_detalle, @ancho_cm, @alto_cm, @largo_cm, 'DISPONIBLE', @origen);

        DECLARE @id_pieza INT = SCOPE_IDENTITY();

        INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_compra_detalle, observacion)
        VALUES (@id_variante, @id_pieza, 'ENTRADA', @origen, 1, @id_usuario, @id_compra_detalle, @observacion);

    IF @tran_externa = 0 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;
        THROW;
    END CATCH

    SELECT @id_pieza AS id_pieza;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_inv_cortar_pieza
    @id_pieza        INT,
    @ancho_corte     DECIMAL(9,3) = NULL,
    @alto_corte      DECIMAL(9,3) = NULL,
    @largo_corte     DECIMAL(9,3) = NULL,
    @minimo_util_cm  DECIMAL(9,3) = 10,
    @id_usuario      INT = NULL,
    @id_pedido       INT = NULL,
    @observacion     NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;

    DECLARE @retazos TABLE (n INT IDENTITY(1,1), id_pieza INT, ancho_cm DECIMAL(9,3), alto_cm DECIMAL(9,3), largo_cm DECIMAL(9,3));

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;

        DECLARE @estado NVARCHAR(15), @id_variante INT, @W DECIMAL(9,3), @H DECIMAL(9,3), @L DECIMAL(9,3);
        SELECT @estado = estado, @id_variante = id_variante, @W = ancho_cm, @H = alto_cm, @L = largo_cm
        FROM dbo.inventario_piezas WITH (UPDLOCK, ROWLOCK)
        WHERE id_pieza = @id_pieza;

        IF @estado IS NULL
            THROW 50020, 'La pieza no existe.', 1;
        IF @estado NOT IN ('DISPONIBLE','RESERVADA')
            THROW 50021, 'Solo se pueden cortar piezas DISPONIBLES o RESERVADAS.', 1;

        IF @L IS NULL  -- vidrio
        BEGIN
            IF ISNULL(@ancho_corte, 0) <= 0 OR ISNULL(@alto_corte, 0) <= 0
                THROW 50022, 'Ingrese ancho y alto del corte.', 1;

            IF NOT (@ancho_corte <= @W AND @alto_corte <= @H)
            BEGIN
                IF @alto_corte <= @W AND @ancho_corte <= @H
                BEGIN  -- entra girado
                    DECLARE @tmp DECIMAL(9,3) = @ancho_corte;
                    SET @ancho_corte = @alto_corte;
                    SET @alto_corte = @tmp;
                END
                ELSE
                    THROW 50023, 'El corte es mas grande que la pieza.', 1;
            END

            IF @W - @ancho_corte >= @minimo_util_cm AND @H >= @minimo_util_cm
                INSERT INTO @retazos (ancho_cm, alto_cm) VALUES (@W - @ancho_corte, @H);
            IF @H - @alto_corte >= @minimo_util_cm AND @ancho_corte >= @minimo_util_cm
                INSERT INTO @retazos (ancho_cm, alto_cm) VALUES (@ancho_corte, @H - @alto_corte);
        END
        ELSE           -- aluminio
        BEGIN
            IF ISNULL(@largo_corte, 0) <= 0
                THROW 50024, 'Ingrese el largo del corte.', 1;
            IF @largo_corte > @L
                THROW 50025, 'El corte es mas largo que la pieza.', 1;

            IF @L - @largo_corte >= @minimo_util_cm
                INSERT INTO @retazos (largo_cm) VALUES (@L - @largo_corte);
        END

        UPDATE dbo.inventario_piezas SET estado = 'CONSUMIDA' WHERE id_pieza = @id_pieza;

        INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES (@id_variante, @id_pieza, 'SALIDA', 'CORTE', 1, @id_usuario, @id_pedido,
                ISNULL(@observacion, 'Corte de ' +
                    CASE WHEN @L IS NULL
                         THEN CONVERT(NVARCHAR(12), @ancho_corte) + ' x ' + CONVERT(NVARCHAR(12), @alto_corte) + ' cm'
                         ELSE CONVERT(NVARCHAR(12), @largo_corte) + ' cm' END));

        DECLARE @n INT = 1, @total INT = (SELECT COUNT(*) FROM @retazos), @nuevo INT;
        WHILE @n <= @total
        BEGIN
            INSERT INTO dbo.inventario_piezas (id_variante, id_pieza_origen, ancho_cm, alto_cm, largo_cm, estado, origen)
            SELECT @id_variante, @id_pieza, ancho_cm, alto_cm, largo_cm, 'DISPONIBLE', 'RETAZO'
            FROM @retazos WHERE n = @n;
            SET @nuevo = SCOPE_IDENTITY();

            INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
            VALUES (@id_variante, @nuevo, 'ENTRADA', 'RETAZO', 1, @id_usuario, @id_pedido,
                    'Retazo de la pieza #' + CONVERT(NVARCHAR(12), @id_pieza));

            UPDATE @retazos SET id_pieza = @nuevo WHERE n = @n;
            SET @n += 1;
        END

    IF @tran_externa = 0 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;
        THROW;
    END CATCH

    SELECT id_pieza, ancho_cm, alto_cm, largo_cm FROM @retazos;
END
GO

PRINT '3 decimales: costo de compra, medidas de pieza y SPs de inventario listos.';
GO

-- ------------------------------------------------------------
-- 10) Enlace compra <-> pedido: las lineas COMPRA_NUEVA de un pedido
--     quedaban con "No hay stock registrado" aunque la compra ya existia,
--     porque el enlace no estaba implementado. Ahora GuardarCompra lo hace
--     solo; este bloque arregla los datos ya registrados (idempotente):
--     empareja cada linea COMPRA_NUEVA pendiente con la pieza DISPONIBLE
--     de origen COMPRA del mismo variante y medida, y recalcula el estado
--     de los pedidos afectados.
--     Nota: vuelva a ejecutar 00_vidrieria_completo.sql para que los SPs
--     de pedidos acepten la fecha enviada por la aplicacion (@fecha).
-- ------------------------------------------------------------
DECLARE @Enlazadas TABLE (id_pieza INT PRIMARY KEY, id_item INT);

;WITH pendientes AS (
    SELECT pd.id_detalle, pd.id_item, pd.id_variante, pd.ancho_cm, pd.alto_cm, pd.largo_cm,
           ROW_NUMBER() OVER (PARTITION BY pd.id_variante, pd.ancho_cm, pd.alto_cm, pd.largo_cm
                              ORDER BY pd.id_detalle) AS rn
    FROM dbo.pedido_detalle pd
    WHERE pd.estado = 'ACTIVO' AND pd.accion = 'COMPRA_NUEVA' AND pd.id_pieza IS NULL
      AND EXISTS (SELECT 1 FROM dbo.pedido_item pi
                  WHERE pi.id_item = pd.id_item AND pi.estado <> 'CANCELADO')
), nuevas AS (
    SELECT ip.id_pieza, ip.id_variante, ip.ancho_cm, ip.alto_cm, ip.largo_cm, ip.id_compra_detalle,
           ROW_NUMBER() OVER (PARTITION BY ip.id_variante, ip.ancho_cm, ip.alto_cm, ip.largo_cm
                              ORDER BY ip.id_pieza) AS rn
    FROM dbo.inventario_piezas ip
    WHERE ip.origen = 'COMPRA' AND ip.estado = 'DISPONIBLE'
)
UPDATE pd
SET pd.id_pieza = n.id_pieza, pd.accion = 'USA_EXISTENTE', pd.id_compra_detalle = n.id_compra_detalle
OUTPUT inserted.id_pieza, inserted.id_item INTO @Enlazadas
FROM dbo.pedido_detalle pd
INNER JOIN pendientes p ON p.id_detalle = pd.id_detalle
INNER JOIN nuevas n ON n.id_variante = p.id_variante AND n.rn = p.rn
 AND ((n.ancho_cm = p.ancho_cm AND n.alto_cm = p.alto_cm AND n.largo_cm IS NULL AND p.largo_cm IS NULL)
  OR  (n.largo_cm = p.largo_cm AND n.ancho_cm IS NULL AND p.ancho_cm IS NULL));

-- Las piezas enlazadas pasan a VENDIDA (quedan comprometidas con el pedido)
UPDATE ip SET ip.estado = 'VENDIDA'
FROM dbo.inventario_piezas ip
INNER JOIN @Enlazadas e ON e.id_pieza = ip.id_pieza;

INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
SELECT ip.id_variante, e.id_pieza, 'SALIDA', 'VENTA', 1, NULL, pi.id_pedido,
       'Enlace automatico compra-pedido'
FROM @Enlazadas e
INNER JOIN dbo.inventario_piezas ip ON ip.id_pieza = e.id_pieza
INNER JOIN dbo.pedido_item pi ON pi.id_item = e.id_item;

-- Recalcular el estado de los pedidos que recibieron material
DECLARE @pEd INT;
DECLARE curPed CURSOR LOCAL FAST_FORWARD FOR
    SELECT DISTINCT pi.id_pedido
    FROM @Enlazadas e
    INNER JOIN dbo.pedido_item pi ON pi.id_item = e.id_item;
OPEN curPed;
FETCH NEXT FROM curPed INTO @pEd;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXEC dbo.sp_ped_recalcular_estados @id_pedido = @pEd;
    FETCH NEXT FROM curPed INTO @pEd;
END
CLOSE curPed;
DEALLOCATE curPed;

PRINT 'Enlace compra-pedido listo: lineas COMPRA_NUEVA resueltas = ' +
      (SELECT CONVERT(NVARCHAR(10), COUNT(*)) FROM @Enlazadas) + '.';
GO

-- ------------------------------------------------------------
-- 11) Los SPs de pedidos ahora reciben la fecha desde la aplicacion
--     (@fecha; si viene NULL usan el servidor). Sin esta migracion,
--     guardar un pedido falla con "too many arguments specified" y
--     terminar un pedido tambien. Idempotente (CREATE OR ALTER).
--     Equivale a re-ejecutar 00_vidrieria_completo.sql.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_registrar
    @id_cliente INT,
    @id_usuario INT,
    @fecha      DATETIME2 = NULL,   -- la fecha la envia la aplicacion (si viene NULL usa el servidor)
    @Items      dbo.TVP_PedidoItem    READONLY,
    @Detalles   dbo.TVP_PedidoDetalle READONLY,
    @Medidas    dbo.TVP_PedidoMedida  READONLY,
    @id_pedido  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;

    IF NOT EXISTS (SELECT 1 FROM @Items)
        THROW 50040, 'El pedido debe tener al menos un item (ventana, mampara, vidrio suelto o caja).', 1;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_ped;

        INSERT INTO dbo.pedidos (id_cliente, id_usuario, fecha, estado, total)
        VALUES (@id_cliente, @id_usuario, ISNULL(@fecha, dbo.fn_hora_peru()), 'PENDIENTE', 0);
        SET @id_pedido = CONVERT(INT, SCOPE_IDENTITY());

        -- mapeo id_temp (item) -> id real generado
        DECLARE @MapItems TABLE (id_temp INT PRIMARY KEY, id_item INT);

        DECLARE @id_temp INT, @tipo NVARCHAR(20), @etiqueta NVARCHAR(50),
                @anchoI DECIMAL(10,2), @altoI DECIMAL(10,2),
                @precio_mat DECIMAL(10,2), @extra DECIMAL(10,2), @mano DECIMAL(10,2), @totalI DECIMAL(10,2),
                @id_modeloI INT;

        DECLARE cur_items CURSOR LOCAL FAST_FORWARD FOR
            SELECT id_temp, tipo, etiqueta, ancho_cm, alto_cm, precio_materiales, costo_extra, mano_obra, total, id_modelo
            FROM @Items;
        OPEN cur_items;
        FETCH NEXT FROM cur_items INTO @id_temp, @tipo, @etiqueta, @anchoI, @altoI, @precio_mat, @extra, @mano, @totalI, @id_modeloI;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO dbo.pedido_item (id_pedido, tipo, descripcion, estado,
                                          precio_materiales, costo_extra, mano_obra, total, id_modelo)
            VALUES (@id_pedido, @tipo, @etiqueta, 'PENDIENTE',
                    @precio_mat, @extra, @mano, @totalI, @id_modeloI);

            INSERT INTO @MapItems VALUES (@id_temp, CONVERT(INT, SCOPE_IDENTITY()));

            FETCH NEXT FROM cur_items INTO @id_temp, @tipo, @etiqueta, @anchoI, @altoI, @precio_mat, @extra, @mano, @totalI, @id_modeloI;
        END
        CLOSE cur_items; DEALLOCATE cur_items;

        -- detalles: resuelve inventario (usa existente / corta / compra nueva) e inserta
        DECLARE @MapDetalles TABLE (id_temp INT PRIMARY KEY, id_detalle INT);

        DECLARE @id_det_temp INT, @id_item_temp INT, @id_variante INT, @accion NVARCHAR(20),
                @id_pieza INT, @anchoD DECIMAL(10,2), @altoD DECIMAL(10,2), @largoD DECIMAL(10,2),
                @cajas INT, @subtotal DECIMAL(10,2),
                @id_item_real INT, @id_pieza_final INT, @precio_unit DECIMAL(10,2);

        DECLARE cur_det CURSOR LOCAL FAST_FORWARD FOR
            SELECT id_temp, id_item_temp, id_variante, accion, id_pieza, ancho_cm, alto_cm, largo_cm, cantidad_cajas, subtotal
            FROM @Detalles;
        OPEN cur_det;
        FETCH NEXT FROM cur_det INTO @id_det_temp, @id_item_temp, @id_variante, @accion, @id_pieza, @anchoD, @altoD, @largoD, @cajas, @subtotal;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @id_item_real = (SELECT id_item FROM @MapItems WHERE id_temp = @id_item_temp);
            SET @id_pieza_final = @id_pieza;
            SET @precio_unit = (SELECT precio_unitario FROM dbo.producto_variante WHERE id_variante = @id_variante);

            IF @accion = 'CORTA_PIEZA'
            BEGIN
                EXEC dbo.sp_inv_cortar_pieza
                     @id_pieza    = @id_pieza,
                     @ancho_corte = @anchoD,
                     @alto_corte  = @altoD,
                     @largo_corte = @largoD,
                     @id_usuario  = @id_usuario,
                     @id_pedido   = @id_pedido;
                SET @id_pieza_final = @id_pieza; -- la pieza original queda CONSUMIDA; esta linea registra lo que se uso de ella
            END
            ELSE IF @accion = 'USA_EXISTENTE'
            BEGIN
                EXEC dbo.sp_inv_cambiar_estado_pieza
                     @id_pieza   = @id_pieza,
                     @estado     = 'VENDIDA',
                     @id_usuario = @id_usuario,
                     @id_pedido  = @id_pedido;
            END
            -- COMPRA_NUEVA: @id_pieza_final queda NULL; se liga despues al registrar la compra

            INSERT INTO dbo.pedido_detalle (id_item, id_variante, accion, id_pieza, estado,
                                             cantidad_cajas, ancho_cm, alto_cm, largo_cm,
                                             precio_unitario_aplicado, subtotal)
            VALUES (@id_item_real, @id_variante, @accion, @id_pieza_final, 'ACTIVO',
                    @cajas, @anchoD, @altoD, @largoD,
                    @precio_unit, @subtotal);

            INSERT INTO @MapDetalles VALUES (@id_det_temp, CONVERT(INT, SCOPE_IDENTITY()));

            IF @accion = 'COMPRA_NUEVA'
                UPDATE dbo.pedido_item SET estado = 'PENDIENTE' WHERE id_item = @id_item_real;

            FETCH NEXT FROM cur_det INTO @id_det_temp, @id_item_temp, @id_variante, @accion, @id_pieza, @anchoD, @altoD, @largoD, @cajas, @subtotal;
        END
        CLOSE cur_det; DEALLOCATE cur_det;

        -- medidas crudas (para el proveedor)
        INSERT INTO dbo.pedido_detalle_medida (id_detalle, etiqueta, valor_cm)
        SELECT md.id_detalle, m.etiqueta, m.valor_cm
        FROM @Medidas m
        INNER JOIN @MapDetalles md ON md.id_temp = m.id_detalle_temp;

        UPDATE dbo.pedidos
        SET total = (SELECT ISNULL(SUM(total), 0) FROM dbo.pedido_item WHERE id_pedido = @id_pedido)
        WHERE id_pedido = @id_pedido;

    IF @tran_externa = 0 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_ped;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ped_terminar_item
    @id_item    INT,
    @id_usuario INT = NULL,
    @fecha      DATETIME2 = NULL   -- la fecha la envia la aplicacion
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @id_pedido INT, @estado NVARCHAR(15);
    SELECT @id_pedido = id_pedido, @estado = estado FROM dbo.pedido_item WHERE id_item = @id_item;

    IF @id_pedido IS NULL
        THROW 50080, 'El item indicado no existe.', 1;
    IF @estado = 'CANCELADO'
        THROW 50081, 'El item esta cancelado.', 1;
    IF @estado = 'COMPLETADO'
        THROW 50082, 'El item ya esta completado.', 1;

    EXEC dbo.sp_ped_recalcular_estados @id_pedido = @id_pedido;

    SELECT @estado = estado FROM dbo.pedido_item WHERE id_item = @id_item;
    IF @estado <> 'EN_PROCESO'
        THROW 50083, 'No se puede completar el item: aun faltan materiales en inventario.', 1;

    UPDATE dbo.pedido_item SET estado = 'COMPLETADO' WHERE id_item = @id_item;

    -- Si ya no queda ningun item pendiente o en proceso, el pedido queda terminado
    IF NOT EXISTS (SELECT 1 FROM dbo.pedido_item
                   WHERE id_pedido = @id_pedido AND estado IN ('PENDIENTE','EN_PROCESO'))
        UPDATE dbo.pedidos SET fecha_termino = ISNULL(@fecha, dbo.fn_hora_peru()) WHERE id_pedido = @id_pedido;
END
GO

-- Marca todo el pedido como terminado (entregado/instalado). Es independiente del pago.
CREATE OR ALTER PROCEDURE dbo.sp_ped_terminar
    @id_pedido  INT,
    @id_usuario INT = NULL,
    @fecha      DATETIME2 = NULL   -- la fecha la envia la aplicacion
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.pedidos WHERE id_pedido = @id_pedido)
        THROW 50070, 'El pedido indicado no existe.', 1;

    EXEC dbo.sp_ped_recalcular_estados @id_pedido = @id_pedido;

    IF EXISTS (SELECT 1 FROM dbo.pedido_item
               WHERE id_pedido = @id_pedido AND estado = 'PENDIENTE')
        THROW 50071, 'No se puede terminar el pedido: hay items cuyos materiales aun no estan en inventario.', 1;

    UPDATE dbo.pedido_item SET estado = 'COMPLETADO'
    WHERE id_pedido = @id_pedido AND estado = 'EN_PROCESO';

    UPDATE dbo.pedidos SET fecha_termino = ISNULL(@fecha, dbo.fn_hora_peru())
    WHERE id_pedido = @id_pedido;
END
GO

PRINT 'SPs de pedidos con @fecha (fecha enviada por la aplicacion) listos.';
GO

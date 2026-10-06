-- ============================================================
-- Módulo de INVENTARIO - cambios sobre el esquema base
-- Ejecutar DESPUÉS de esquema_vidrieria_sqlserver.sql
-- Motor: Microsoft SQL Server 2016 SP1 o superior (usa CREATE OR ALTER)
-- Se puede volver a ejecutar sin romper nada (cada cambio verifica si ya existe)
-- ============================================================

USE vidrieria;
GO

-- ------------------------------------------------------------
-- 1. Stock mínimo por variante
--    Vidrio/aluminio: cantidad mínima de piezas DISPONIBLES
--    Tornillo/tarugo: cantidad mínima de cajas
-- ------------------------------------------------------------
IF COL_LENGTH('producto_variante', 'stock_minimo') IS NULL
    ALTER TABLE producto_variante ADD stock_minimo INT NOT NULL
        CONSTRAINT df_variante_stock_minimo DEFAULT 0;
GO

IF OBJECT_ID('ck_variante_unidad_costo') IS NULL
    ALTER TABLE producto_variante ADD CONSTRAINT ck_variante_unidad_costo
        CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA'));
GO

-- ------------------------------------------------------------
-- 2. inventario_cajas: el stock nunca puede quedar negativo
-- ------------------------------------------------------------
IF OBJECT_ID('ck_cajas_stock_no_negativo') IS NULL
    ALTER TABLE inventario_cajas ADD CONSTRAINT ck_cajas_stock_no_negativo
        CHECK (stock_cajas >= 0);
GO

-- ------------------------------------------------------------
-- 3. inventario_piezas
--    a) Nuevo origen 'INICIAL': piezas que ya estaban en el almacén
--       cuando se empezó a usar el sistema (no vienen de una compra).
--       El CHECK original no tiene nombre, así que se busca y se reemplaza.
-- ------------------------------------------------------------
DECLARE @ck_origen SYSNAME;
SELECT @ck_origen = cc.name
FROM sys.check_constraints cc
INNER JOIN sys.columns c ON c.object_id = cc.parent_object_id AND c.column_id = cc.parent_column_id
WHERE cc.parent_object_id = OBJECT_ID('inventario_piezas') AND c.name = 'origen'
  AND cc.name <> 'ck_pieza_origen';

IF @ck_origen IS NOT NULL
    EXEC('ALTER TABLE inventario_piezas DROP CONSTRAINT ' + @ck_origen);
GO

IF OBJECT_ID('ck_pieza_origen') IS NULL
    ALTER TABLE inventario_piezas ADD CONSTRAINT ck_pieza_origen
        CHECK (origen IN ('COMPRA','RETAZO','INICIAL'));
GO

--    b) Medidas coherentes: vidrio = ancho y alto, aluminio = largo; siempre positivas
IF OBJECT_ID('ck_pieza_medidas') IS NULL
    ALTER TABLE inventario_piezas ADD CONSTRAINT ck_pieza_medidas CHECK (
        (ancho_cm > 0 AND alto_cm > 0 AND largo_cm IS NULL)
     OR (largo_cm > 0 AND ancho_cm IS NULL AND alto_cm IS NULL)
    );
GO

-- ------------------------------------------------------------
-- 4. Kardex: historial de TODO lo que entra, sale o cambia en el inventario
-- ------------------------------------------------------------
IF OBJECT_ID('movimientos_inventario') IS NULL
CREATE TABLE movimientos_inventario (
    id_movimiento      INT IDENTITY(1,1) PRIMARY KEY,
    fecha              DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    id_variante        INT NOT NULL,
    id_pieza           INT NULL,               -- vidrio/aluminio: pieza afectada
    tipo               NVARCHAR(10) NOT NULL   -- ENTRADA suma, SALIDA resta, AJUSTE corrige, ESTADO solo cambia estado
        CHECK (tipo IN ('ENTRADA','SALIDA','AJUSTE','ESTADO')),
    motivo             NVARCHAR(15) NOT NULL
        CHECK (motivo IN ('COMPRA','INICIAL','RETAZO','CORTE','VENTA','DEFECTO','RESERVA','LIBERACION','AJUSTE')),
    cantidad           DECIMAL(10,2) NOT NULL, -- cajas movidas, o 1 si es una pieza
    stock_resultante   INT NULL,               -- cajas: stock después del movimiento
    id_usuario         INT NULL,
    id_pedido          INT NULL,
    id_compra_detalle  INT NULL,
    observacion        NVARCHAR(255) NULL,
    CONSTRAINT fk_mov_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_mov_pieza FOREIGN KEY (id_pieza)
        REFERENCES inventario_piezas(id_pieza) ON DELETE NO ACTION,
    CONSTRAINT fk_mov_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE SET NULL,
    CONSTRAINT fk_mov_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedidos(id_pedido) ON DELETE SET NULL,
    CONSTRAINT fk_mov_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES compra_detalle(id_compra_detalle) ON DELETE SET NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_mov_fecha')
    CREATE INDEX idx_mov_fecha ON movimientos_inventario(fecha);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_mov_variante')
    CREATE INDEX idx_mov_variante ON movimientos_inventario(id_variante, fecha);
GO

-- ============================================================
-- VISTAS (las usa la capa de negocio para listar)
-- ============================================================

-- Variante con un nombre legible: "Vidrio catedral (4 mm)", "Aluminio u13 negro", etc.
CREATE OR ALTER VIEW vw_inv_variantes AS
SELECT pv.id_variante,
       p.id_producto,
       p.categoria,
       p.tipo,
       p.color,
       c.espesor_cm,
       c.medida_cm,
       p.nombre
         + ISNULL(' ' + NULLIF(p.color, ''), '')
         + ISNULL(' - espesor ' + CONVERT(NVARCHAR(10), CONVERT(DECIMAL(6,2), c.espesor_cm)) + ' cm', '')
         + ISNULL(' - medida ' + CONVERT(NVARCHAR(10), CONVERT(DECIMAL(6,2), c.medida_cm)) + ' cm', '') AS producto,
       pv.unidad_costo,
       pv.precio_unitario,
       pv.stock_minimo
FROM producto_variante pv
INNER JOIN productos p       ON p.id_producto = pv.id_producto
INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
WHERE p.activo = 1;
GO

-- Piezas de vidrio/aluminio con el nombre del producto
CREATE OR ALTER VIEW vw_inv_piezas AS
SELECT ip.id_pieza,
       ip.id_variante,
       v.categoria,
       v.producto,
       ip.ancho_cm,
       ip.alto_cm,
       ip.largo_cm,
       CONVERT(DECIMAL(12,2), ip.ancho_cm * ip.alto_cm) AS area_cm2,
       ip.estado,
       ip.origen,
       ip.id_pieza_origen,
       ip.id_compra_detalle,
       ip.fecha_ingreso
FROM inventario_piezas ip
INNER JOIN vw_inv_variantes v ON v.id_variante = ip.id_variante;
GO

-- Resumen por variante: cuánto hay y si está por debajo del mínimo
CREATE OR ALTER VIEW vw_inv_resumen AS
SELECT v.id_variante,
       v.categoria,
       v.producto,
       v.unidad_costo,
       v.precio_unitario,
       ISNULL(pz.piezas_disponibles, 0) AS piezas_disponibles,
       ISNULL(pz.piezas_reservadas, 0)  AS piezas_reservadas,
       ISNULL(pz.area_disponible_cm2, 0) AS area_disponible_cm2,
       ISNULL(pz.largo_disponible_cm, 0) AS largo_disponible_cm,
       ISNULL(ic.stock_cajas, 0)         AS stock_cajas,
       CASE WHEN v.unidad_costo = 'CAJA' THEN ISNULL(ic.stock_cajas, 0)
            ELSE ISNULL(pz.piezas_disponibles, 0) END AS existencias,
       v.stock_minimo,
       CONVERT(BIT, CASE WHEN (CASE WHEN v.unidad_costo = 'CAJA' THEN ISNULL(ic.stock_cajas, 0)
                                    ELSE ISNULL(pz.piezas_disponibles, 0) END) <= v.stock_minimo
                         AND v.stock_minimo > 0
                    THEN 1 ELSE 0 END) AS bajo_minimo
FROM vw_inv_variantes v
LEFT JOIN (
    SELECT id_variante,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN 1 ELSE 0 END) AS piezas_disponibles,
           SUM(CASE WHEN estado = 'RESERVADA'  THEN 1 ELSE 0 END) AS piezas_reservadas,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN ancho_cm * alto_cm ELSE 0 END) AS area_disponible_cm2,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN largo_cm ELSE 0 END) AS largo_disponible_cm
    FROM inventario_piezas
    GROUP BY id_variante
) pz ON pz.id_variante = v.id_variante
LEFT JOIN inventario_cajas ic ON ic.id_variante = v.id_variante;
GO

-- Kardex legible
CREATE OR ALTER VIEW vw_inv_movimientos AS
SELECT m.id_movimiento,
       m.fecha,
       m.id_variante,
       v.categoria,
       v.producto,
       m.id_pieza,
       m.tipo,
       m.motivo,
       m.cantidad,
       m.stock_resultante,
       u.nombre AS usuario,
       m.id_pedido,
       m.id_compra_detalle,
       m.observacion
FROM movimientos_inventario m
LEFT JOIN vw_inv_variantes v ON v.id_variante = m.id_variante
LEFT JOIN usuarios u         ON u.id_usuario = m.id_usuario;
GO

-- ============================================================
-- PROCEDIMIENTOS ALMACENADOS
-- Cada uno hace su trabajo dentro de una TRANSACCIÓN: o se guarda todo
-- (el cambio + su registro en el kardex) o no se guarda nada (ROLLBACK en el CATCH).
-- Si se llaman dentro de una transacción ajena (ej: el módulo de compras), usan un
-- punto de guardado (SAVE TRANSACTION): al fallar deshacen solo su parte y no
-- anulan la transacción de quien los llamó.
-- Los errores de negocio se lanzan con THROW 50000+ y el mensaje llega
-- tal cual a la pantalla.
-- ============================================================

-- ------------------------------------------------------------
-- Ingresar una pieza de vidrio o aluminio
-- @origen: COMPRA (viene de compra_detalle), RETAZO (sobrante de un corte)
--          o INICIAL (ya estaba en el almacén)
-- Devuelve: id_pieza
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_inv_ingresar_pieza
    @id_variante        INT,
    @ancho_cm           DECIMAL(8,2) = NULL,
    @alto_cm            DECIMAL(8,2) = NULL,
    @largo_cm           DECIMAL(8,2) = NULL,
    @origen             NVARCHAR(10),
    @id_compra_detalle  INT = NULL,
    @id_pieza_origen    INT = NULL,
    @id_usuario         INT = NULL,
    @observacion        NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transacción?

    DECLARE @unidad NVARCHAR(10) = (SELECT unidad_costo FROM producto_variante WHERE id_variante = @id_variante);

    IF @unidad IS NULL
        THROW 50001, 'La variante de producto no existe.', 1;
    IF @unidad = 'CAJA'
        THROW 50002, 'Tornillos y tarugos se manejan por cajas, no por piezas.', 1;
    IF @unidad = 'CM2' AND (ISNULL(@ancho_cm, 0) <= 0 OR ISNULL(@alto_cm, 0) <= 0)
        THROW 50003, 'Para vidrio ingrese ancho y alto mayores a cero.', 1;
    IF @unidad = 'CM_LINEAL' AND ISNULL(@largo_cm, 0) <= 0
        THROW 50004, 'Para aluminio ingrese el largo mayor a cero.', 1;
    IF @origen = 'COMPRA' AND @id_compra_detalle IS NULL
        THROW 50005, 'Una pieza de origen COMPRA debe indicar el detalle de compra.', 1;
    IF @origen = 'RETAZO' AND @id_pieza_origen IS NULL
        THROW 50006, 'Un retazo debe indicar la pieza de la que salió.', 1;

    -- Solo se guardan las medidas que corresponden al tipo de material
    IF @unidad = 'CM2'       SET @largo_cm = NULL;
    IF @unidad = 'CM_LINEAL' SELECT @ancho_cm = NULL, @alto_cm = NULL;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        INSERT INTO inventario_piezas (id_variante, id_pieza_origen, id_compra_detalle, ancho_cm, alto_cm, largo_cm, estado, origen)
        VALUES (@id_variante, @id_pieza_origen, @id_compra_detalle, @ancho_cm, @alto_cm, @largo_cm, 'DISPONIBLE', @origen);

        DECLARE @id_pieza INT = SCOPE_IDENTITY();

        INSERT INTO movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_compra_detalle, observacion)
        VALUES (@id_variante, @id_pieza, 'ENTRADA', @origen, 1, @id_usuario, @id_compra_detalle, @observacion);

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrió decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzó a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transacción de quien lo llamó
        THROW;                                    -- y devuelve el error a la aplicación
    END CATCH

    SELECT @id_pieza AS id_pieza;
END
GO

-- ------------------------------------------------------------
-- Cambiar el estado de una pieza
--   DISPONIBLE -> RESERVADA | VENDIDA | CONSUMIDA | DEFECTUOSA
--   RESERVADA  -> DISPONIBLE | VENDIDA | CONSUMIDA | DEFECTUOSA
--   VENDIDA, CONSUMIDA y DEFECTUOSA son finales (la pieza ya salió)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_inv_cambiar_estado_pieza
    @id_pieza     INT,
    @estado       NVARCHAR(15),
    @id_usuario   INT = NULL,
    @id_pedido    INT = NULL,
    @observacion  NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transacción?

    IF @estado NOT IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA')
        THROW 50010, 'Estado de pieza no válido.', 1;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        DECLARE @estado_actual NVARCHAR(15), @id_variante INT;
        SELECT @estado_actual = estado, @id_variante = id_variante
        FROM inventario_piezas WITH (UPDLOCK, ROWLOCK)
        WHERE id_pieza = @id_pieza;

        IF @estado_actual IS NULL
            THROW 50011, 'La pieza no existe.', 1;
        IF @estado_actual = @estado
            THROW 50012, 'La pieza ya tiene ese estado.', 1;
        IF @estado_actual IN ('VENDIDA','CONSUMIDA','DEFECTUOSA')
            THROW 50013, 'La pieza ya salió del inventario (vendida, consumida o defectuosa) y no puede cambiar de estado.', 1;

        UPDATE inventario_piezas SET estado = @estado WHERE id_pieza = @id_pieza;

        DECLARE @tipo NVARCHAR(10) =
            CASE WHEN @estado IN ('RESERVADA','DISPONIBLE') THEN 'ESTADO' ELSE 'SALIDA' END;
        DECLARE @motivo NVARCHAR(15) =
            CASE @estado WHEN 'RESERVADA'  THEN 'RESERVA'
                         WHEN 'DISPONIBLE' THEN 'LIBERACION'
                         WHEN 'VENDIDA'    THEN 'VENTA'
                         WHEN 'CONSUMIDA'  THEN 'CORTE'
                         WHEN 'DEFECTUOSA' THEN 'DEFECTO' END;

        INSERT INTO movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES (@id_variante, @id_pieza, @tipo, @motivo, 1, @id_usuario, @id_pedido,
                ISNULL(@observacion, @estado_actual + ' -> ' + @estado));

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrió decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzó a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transacción de quien lo llamó
        THROW;                                    -- y devuelve el error a la aplicación
    END CATCH
END
GO

-- ------------------------------------------------------------
-- Cortar una pieza
-- Se indica la medida que se corta; el sistema:
--   1. Marca la pieza original como CONSUMIDA
--   2. Calcula y registra los retazos que sobran (si son útiles)
--      Vidrio (ancho W x alto H, corte a x b):
--        retazo 1 = franja lateral   (W - a) x H
--        retazo 2 = franja inferior   a x (H - b)
--      Aluminio (largo L, corte l): retazo = L - l
--   Si el corte no entra derecho pero sí girado 90°, se gira automáticamente.
--   Un sobrante menor a @minimo_util_cm en algún lado se considera desperdicio.
-- Devuelve: los retazos generados (puede ser 0, 1 o 2 filas)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_inv_cortar_pieza
    @id_pieza        INT,
    @ancho_corte     DECIMAL(8,2) = NULL,
    @alto_corte      DECIMAL(8,2) = NULL,
    @largo_corte     DECIMAL(8,2) = NULL,
    @minimo_util_cm  DECIMAL(8,2) = 10,
    @id_usuario      INT = NULL,
    @id_pedido       INT = NULL,
    @observacion     NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transacción?

    DECLARE @retazos TABLE (n INT IDENTITY(1,1), id_pieza INT, ancho_cm DECIMAL(8,2), alto_cm DECIMAL(8,2), largo_cm DECIMAL(8,2));

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        DECLARE @estado NVARCHAR(15), @id_variante INT, @W DECIMAL(8,2), @H DECIMAL(8,2), @L DECIMAL(8,2);
        SELECT @estado = estado, @id_variante = id_variante, @W = ancho_cm, @H = alto_cm, @L = largo_cm
        FROM inventario_piezas WITH (UPDLOCK, ROWLOCK)
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
                    DECLARE @tmp DECIMAL(8,2) = @ancho_corte;
                    SET @ancho_corte = @alto_corte;
                    SET @alto_corte = @tmp;
                END
                ELSE
                    THROW 50023, 'El corte es más grande que la pieza.', 1;
            END

            -- franja lateral: (W - a) x H
            IF @W - @ancho_corte >= @minimo_util_cm AND @H >= @minimo_util_cm
                INSERT INTO @retazos (ancho_cm, alto_cm) VALUES (@W - @ancho_corte, @H);
            -- franja inferior: a x (H - b)
            IF @H - @alto_corte >= @minimo_util_cm AND @ancho_corte >= @minimo_util_cm
                INSERT INTO @retazos (ancho_cm, alto_cm) VALUES (@ancho_corte, @H - @alto_corte);
        END
        ELSE           -- aluminio
        BEGIN
            IF ISNULL(@largo_corte, 0) <= 0
                THROW 50024, 'Ingrese el largo del corte.', 1;
            IF @largo_corte > @L
                THROW 50025, 'El corte es más largo que la pieza.', 1;

            IF @L - @largo_corte >= @minimo_util_cm
                INSERT INTO @retazos (largo_cm) VALUES (@L - @largo_corte);
        END

        -- 1. la pieza original sale del inventario
        UPDATE inventario_piezas SET estado = 'CONSUMIDA' WHERE id_pieza = @id_pieza;

        INSERT INTO movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES (@id_variante, @id_pieza, 'SALIDA', 'CORTE', 1, @id_usuario, @id_pedido,
                ISNULL(@observacion, 'Corte de ' +
                    CASE WHEN @L IS NULL
                         THEN CONVERT(NVARCHAR(12), @ancho_corte) + ' x ' + CONVERT(NVARCHAR(12), @alto_corte) + ' cm'
                         ELSE CONVERT(NVARCHAR(12), @largo_corte) + ' cm' END));

        -- 2. cada retazo útil entra como pieza nueva
        DECLARE @n INT = 1, @total INT = (SELECT COUNT(*) FROM @retazos), @nuevo INT;
        WHILE @n <= @total
        BEGIN
            INSERT INTO inventario_piezas (id_variante, id_pieza_origen, ancho_cm, alto_cm, largo_cm, estado, origen)
            SELECT @id_variante, @id_pieza, ancho_cm, alto_cm, largo_cm, 'DISPONIBLE', 'RETAZO'
            FROM @retazos WHERE n = @n;
            SET @nuevo = SCOPE_IDENTITY();

            INSERT INTO movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
            VALUES (@id_variante, @nuevo, 'ENTRADA', 'RETAZO', 1, @id_usuario, @id_pedido,
                    'Retazo de la pieza #' + CONVERT(NVARCHAR(12), @id_pieza));

            UPDATE @retazos SET id_pieza = @nuevo WHERE n = @n;
            SET @n += 1;
        END

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrió decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzó a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transacción de quien lo llamó
        THROW;                                    -- y devuelve el error a la aplicación
    END CATCH

    SELECT id_pieza, ancho_cm, alto_cm, largo_cm FROM @retazos;
END
GO

-- ------------------------------------------------------------
-- Mover cajas (tornillos / tarugos)
--   ENTRADA: suma @cantidad          (motivo COMPRA, INICIAL o AJUSTE)
--   SALIDA:  resta @cantidad         (motivo VENTA, DEFECTO o AJUSTE) - valida stock
--   AJUSTE:  el stock pasa a ser exactamente @cantidad (conteo físico)
-- Si la variante todavía no tiene fila en inventario_cajas, se crea.
-- Devuelve: stock_resultante
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_inv_mover_cajas
    @id_variante        INT,
    @tipo               NVARCHAR(10),
    @cantidad           INT,
    @motivo             NVARCHAR(15),
    @id_usuario         INT = NULL,
    @id_pedido          INT = NULL,
    @id_compra_detalle  INT = NULL,
    @observacion        NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transacción?

    DECLARE @unidad NVARCHAR(10) = (SELECT unidad_costo FROM producto_variante WHERE id_variante = @id_variante);

    IF @unidad IS NULL
        THROW 50030, 'La variante de producto no existe.', 1;
    IF @unidad <> 'CAJA'
        THROW 50031, 'Vidrio y aluminio se manejan por piezas, no por cajas.', 1;
    IF @tipo NOT IN ('ENTRADA','SALIDA','AJUSTE')
        THROW 50032, 'Tipo de movimiento no válido.', 1;
    IF @cantidad < 0 OR (@cantidad = 0 AND @tipo <> 'AJUSTE')
        THROW 50033, 'La cantidad debe ser mayor a cero.', 1;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        IF NOT EXISTS (SELECT 1 FROM inventario_cajas WITH (UPDLOCK, HOLDLOCK) WHERE id_variante = @id_variante)
            INSERT INTO inventario_cajas (id_variante, stock_cajas) VALUES (@id_variante, 0);

        DECLARE @actual INT = (SELECT stock_cajas FROM inventario_cajas WITH (UPDLOCK) WHERE id_variante = @id_variante);
        DECLARE @nuevo INT =
            CASE @tipo WHEN 'ENTRADA' THEN @actual + @cantidad
                       WHEN 'SALIDA'  THEN @actual - @cantidad
                       ELSE @cantidad END;

        IF @nuevo < 0
        BEGIN
            DECLARE @msg NVARCHAR(200) = 'Stock insuficiente: hay ' + CONVERT(NVARCHAR(12), @actual)
                                       + ' cajas y se quieren sacar ' + CONVERT(NVARCHAR(12), @cantidad) + '.';
            THROW 50034, @msg, 1;
        END

        UPDATE inventario_cajas SET stock_cajas = @nuevo WHERE id_variante = @id_variante;

        INSERT INTO movimientos_inventario (id_variante, tipo, motivo, cantidad, stock_resultante, id_usuario, id_pedido, id_compra_detalle, observacion)
        VALUES (@id_variante, @tipo, @motivo,
                CASE WHEN @tipo = 'AJUSTE' THEN @nuevo - @actual ELSE @cantidad END,
                @nuevo, @id_usuario, @id_pedido, @id_compra_detalle,
                ISNULL(@observacion, CASE WHEN @tipo = 'AJUSTE'
                                          THEN 'Ajuste de ' + CONVERT(NVARCHAR(12), @actual) + ' a ' + CONVERT(NVARCHAR(12), @nuevo) + ' cajas'
                                          END));

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrió decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzó a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transacción de quien lo llamó
        THROW;                                    -- y devuelve el error a la aplicación
    END CATCH

    SELECT @nuevo AS stock_resultante;
END
GO

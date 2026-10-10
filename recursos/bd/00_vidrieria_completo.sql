-- ============================================================
-- Script UNICO y consolidado: Sistema para vidrieria
-- Motor: Microsoft SQL Server (T-SQL)
--
-- Generado a partir de los archivos:
--   esquema_vidrieria_sqlserver.sql  (esquema base)
--   02_modulo_inventario.sql         (modulo de inventario)
--   03_datos_prueba_inventario.sql   (datos de prueba)
--   04_modulo_clientes.sql           (modulo de clientes)
--   05_ficha_comercial_clientes.sql  (ampliacion de clientes)
--
-- Orden de ejecucion:
--   1) Base de datos y TABLAS (los cambios de los modulos ya estan
--      integrados directamente en cada CREATE TABLE)
--   2) VISTAS y PROCEDIMIENTOS / FUNCIONES
--   3) INSERT de datos
--
-- Es idempotente: se puede volver a ejecutar sin generar errores.
-- ============================================================

IF DB_ID(N'vidrieria') IS NULL
BEGIN
    CREATE DATABASE vidrieria;
END
GO

USE vidrieria;
GO

-- ============================================================
-- 1) TABLAS
-- ============================================================

-- ------------------------------------------------------------
-- Usuarios y preferencias
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.usuarios', N'U') IS NULL
CREATE TABLE dbo.usuarios (
    id_usuario          INT IDENTITY(1,1) PRIMARY KEY,
    nombre              NVARCHAR(150) NOT NULL UNIQUE,
    correo              NVARCHAR(150) NOT NULL UNIQUE,
    contrasena          NVARCHAR(255) NOT NULL,
    pregunta_seguridad  NVARCHAR(255) NOT NULL,
    respuesta           NVARCHAR(255) NOT NULL,  -- hashear igual que la contrasena, no guardar en texto plano
    rol                 NVARCHAR(20) NOT NULL DEFAULT 'VENDEDOR'  -- VENDEDOR = Trabajador, ADMINISTRADOR = Jefe
        CHECK (rol IN ('VENDEDOR','ADMINISTRADOR')),
    activo              BIT NOT NULL DEFAULT 1,
    fecha_creacion      DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID(N'dbo.usuario_preferencias', N'U') IS NULL
CREATE TABLE dbo.usuario_preferencias (
    id_usuario  INT PRIMARY KEY,
    tema        NVARCHAR(10) NOT NULL DEFAULT 'CLARO'
        CHECK (tema IN ('CLARO','OSCURO')),
    fuente      NVARCHAR(50) NOT NULL DEFAULT 'Arial',
    CONSTRAINT fk_pref_usuario FOREIGN KEY (id_usuario)
        REFERENCES dbo.usuarios(id_usuario) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.tipos_mensaje', N'U') IS NULL
CREATE TABLE dbo.tipos_mensaje (
    id_mensaje   INT IDENTITY(1,1) PRIMARY KEY,
    codigo       NVARCHAR(50)  NOT NULL UNIQUE,
    titulo       NVARCHAR(255) NOT NULL,
    texto        NVARCHAR(255)
);
GO

IF OBJECT_ID(N'dbo.usuario_mensajes_descartados', N'U') IS NULL
CREATE TABLE dbo.usuario_mensajes_descartados (
    id_usuario      INT NOT NULL,
    id_mensaje      INT NOT NULL,
    fecha_descarte  DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT pk_usuario_mensaje PRIMARY KEY (id_usuario, id_mensaje),
    CONSTRAINT fk_desc_usuario FOREIGN KEY (id_usuario)
        REFERENCES dbo.usuarios(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_desc_mensaje FOREIGN KEY (id_mensaje)
        REFERENCES dbo.tipos_mensaje(id_mensaje) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Clientes
-- (la columna version_cliente viene del modulo de clientes)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.clientes', N'U') IS NULL
CREATE TABLE dbo.clientes (
    id_cliente        INT IDENTITY(1,1) PRIMARY KEY,
    nombre            NVARCHAR(150) NOT NULL,
    documento         NVARCHAR(20),
    telefono          NVARCHAR(20),
    direccion         NVARCHAR(255),
    version_cliente   ROWVERSION NOT NULL
);
GO

-- ------------------------------------------------------------
-- Catalogo de productos (familia / variante / caracteristicas)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.productos', N'U') IS NULL
CREATE TABLE dbo.productos (
    id_producto  INT IDENTITY(1,1) PRIMARY KEY,
    categoria    NVARCHAR(20) NOT NULL,  -- VIDRIO, ALUMINIO, TORNILLO, TARUGO
    tipo         NVARCHAR(50) NOT NULL,  -- transparente, catedral, espejo, u13, u21, triangulo...
    color        NVARCHAR(30),           -- principalmente aluminio: negro, normal, madera...
    nombre       NVARCHAR(150) NOT NULL,
    descripcion  NVARCHAR(255),
    imagen_url   NVARCHAR(255),
    activo       BIT NOT NULL DEFAULT 1
);
GO

IF OBJECT_ID(N'dbo.caracteristicas', N'U') IS NULL
CREATE TABLE dbo.caracteristicas (
    id_caracteristica  INT IDENTITY(1,1) PRIMARY KEY,
    espesor_cm         DECIMAL(6,2),  -- vidrio
    medida_cm          DECIMAL(6,2)   -- tornillo / tarugo
);
GO

-- stock_minimo y la validacion de unidad_costo vienen del modulo de inventario
IF OBJECT_ID(N'dbo.producto_variante', N'U') IS NULL
CREATE TABLE dbo.producto_variante (
    id_variante        INT IDENTITY(1,1) PRIMARY KEY,
    id_producto        INT NOT NULL,
    id_caracteristica  INT NOT NULL,
    unidad_costo       NVARCHAR(10) NOT NULL,  -- CM2, CM_LINEAL, CAJA
    precio_unitario    DECIMAL(10,2) NOT NULL,
    stock_minimo       INT NOT NULL CONSTRAINT df_variante_stock_minimo DEFAULT 0,
    CONSTRAINT uq_variante UNIQUE (id_producto, id_caracteristica),
    CONSTRAINT ck_variante_unidad_costo CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA')),
    CONSTRAINT fk_var_producto FOREIGN KEY (id_producto)
        REFERENCES dbo.productos(id_producto) ON DELETE NO ACTION,
    CONSTRAINT fk_var_caract FOREIGN KEY (id_caracteristica)
        REFERENCES dbo.caracteristicas(id_caracteristica) ON DELETE NO ACTION
);
GO

-- ------------------------------------------------------------
-- Pedidos (se crean antes de compras porque compras los referencia)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.pedidos', N'U') IS NULL
CREATE TABLE dbo.pedidos (
    id_pedido   INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente  INT NOT NULL,
    id_usuario  INT NOT NULL,
    fecha       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    estado      NVARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (estado IN ('PENDIENTE','PAGADO','PARCIAL','CANCELADO')),
    total       DECIMAL(10,2) NOT NULL DEFAULT 0,
    CONSTRAINT fk_pedido_cliente FOREIGN KEY (id_cliente)
        REFERENCES dbo.clientes(id_cliente) ON DELETE NO ACTION,
    CONSTRAINT fk_pedido_usuario FOREIGN KEY (id_usuario)
        REFERENCES dbo.usuarios(id_usuario) ON DELETE NO ACTION
);
GO

-- ------------------------------------------------------------
-- Compras (entrada de material)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.compras', N'U') IS NULL
CREATE TABLE dbo.compras (
    id_compra        INT IDENTITY(1,1) PRIMARY KEY,
    proveedor        NVARCHAR(150) NOT NULL,
    fecha            DATE NOT NULL,
    total            DECIMAL(10,2) NOT NULL DEFAULT 0,
    motivo           NVARCHAR(20) NOT NULL DEFAULT 'REPOSICION'
        CHECK (motivo IN ('REPOSICION','REEMPLAZO_DEFECTO')),
    id_pedido_origen INT NULL,  -- si motivo = REEMPLAZO_DEFECTO: que pedido genero la necesidad de comprar
    CONSTRAINT fk_compra_pedido_origen FOREIGN KEY (id_pedido_origen)
        REFERENCES dbo.pedidos(id_pedido) ON DELETE NO ACTION
);
GO

IF OBJECT_ID(N'dbo.compra_detalle', N'U') IS NULL
CREATE TABLE dbo.compra_detalle (
    id_compra_detalle  INT IDENTITY(1,1) PRIMARY KEY,
    id_compra          INT NOT NULL,
    id_variante        INT NOT NULL,
    cantidad           DECIMAL(10,2) NOT NULL,
    costo_unitario     DECIMAL(10,2) NOT NULL,  -- snapshot del costo al momento de comprar
    subtotal           DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_cdet_compra FOREIGN KEY (id_compra)
        REFERENCES dbo.compras(id_compra) ON DELETE CASCADE,
    CONSTRAINT fk_cdet_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE NO ACTION
);
GO

-- ------------------------------------------------------------
-- Inventario: piezas (vidrio/aluminio) y cajas (tornillo/tarugo)
-- Los CHECK de origen y medidas vienen del modulo de inventario
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.inventario_piezas', N'U') IS NULL
CREATE TABLE dbo.inventario_piezas (
    id_pieza           INT IDENTITY(1,1) PRIMARY KEY,
    id_variante        INT NOT NULL,
    id_pieza_origen    INT NULL,               -- referencia a la pieza de la que salio (retazo)
    id_compra_detalle  INT NULL,               -- de que compra provino (si origen = COMPRA)
    ancho_cm           DECIMAL(8,2),           -- vidrio
    alto_cm            DECIMAL(8,2),           -- vidrio
    largo_cm           DECIMAL(8,2),           -- aluminio
    estado             NVARCHAR(15) NOT NULL DEFAULT 'DISPONIBLE'
        CONSTRAINT ck_pieza_estado CHECK (estado IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA')),
    origen             NVARCHAR(10) NOT NULL
        CONSTRAINT ck_pieza_origen CHECK (origen IN ('COMPRA','RETAZO','INICIAL')),
    fecha_ingreso      DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT ck_pieza_medidas CHECK (
        (ancho_cm > 0 AND alto_cm > 0 AND largo_cm IS NULL)
     OR (largo_cm > 0 AND ancho_cm IS NULL AND alto_cm IS NULL)
    ),
    CONSTRAINT fk_pieza_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_origen FOREIGN KEY (id_pieza_origen)
        REFERENCES dbo.inventario_piezas(id_pieza) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES dbo.compra_detalle(id_compra_detalle) ON DELETE SET NULL
);
GO

IF OBJECT_ID(N'dbo.inventario_cajas', N'U') IS NULL
CREATE TABLE dbo.inventario_cajas (
    id_variante   INT PRIMARY KEY,
    stock_cajas   INT NOT NULL DEFAULT 0,
    CONSTRAINT ck_cajas_stock_no_negativo CHECK (stock_cajas >= 0),
    CONSTRAINT fk_cajas_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Detalle de pedidos
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.pedido_detalle', N'U') IS NULL
CREATE TABLE dbo.pedido_detalle (
    id_detalle                INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido                 INT NOT NULL,
    id_variante               INT NOT NULL,
    id_pieza                  INT NULL,        -- vidrio/aluminio: pieza especifica usada
    cantidad_cajas            INT NULL,        -- tornillo/tarugo
    ancho_cm                  DECIMAL(8,2),    -- medida real cortada
    alto_cm                   DECIMAL(8,2),
    largo_cm                  DECIMAL(8,2),
    precio_unitario_aplicado  DECIMAL(10,2) NOT NULL,
    subtotal                  DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_det_pedido FOREIGN KEY (id_pedido)
        REFERENCES dbo.pedidos(id_pedido) ON DELETE CASCADE,
    CONSTRAINT fk_det_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_det_pieza FOREIGN KEY (id_pieza)
        REFERENCES dbo.inventario_piezas(id_pieza) ON DELETE SET NULL
);
GO

-- ------------------------------------------------------------
-- Kardex: historial de TODO lo que entra, sale o cambia en el inventario
-- (tabla del modulo de inventario)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.movimientos_inventario', N'U') IS NULL
CREATE TABLE dbo.movimientos_inventario (
    id_movimiento      INT IDENTITY(1,1) PRIMARY KEY,
    fecha              DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    id_variante        INT NOT NULL,
    id_pieza           INT NULL,               -- vidrio/aluminio: pieza afectada
    tipo               NVARCHAR(10) NOT NULL   -- ENTRADA suma, SALIDA resta, AJUSTE corrige, ESTADO solo cambia estado
        CHECK (tipo IN ('ENTRADA','SALIDA','AJUSTE','ESTADO')),
    motivo             NVARCHAR(15) NOT NULL
        CHECK (motivo IN ('COMPRA','INICIAL','RETAZO','CORTE','VENTA','DEFECTO','RESERVA','LIBERACION','AJUSTE')),
    cantidad           DECIMAL(10,2) NOT NULL, -- cajas movidas, o 1 si es una pieza
    stock_resultante   INT NULL,               -- cajas: stock despues del movimiento
    id_usuario         INT NULL,
    id_pedido          INT NULL,
    id_compra_detalle  INT NULL,
    observacion        NVARCHAR(255) NULL,
    CONSTRAINT fk_mov_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_mov_pieza FOREIGN KEY (id_pieza)
        REFERENCES dbo.inventario_piezas(id_pieza) ON DELETE NO ACTION,
    CONSTRAINT fk_mov_usuario FOREIGN KEY (id_usuario)
        REFERENCES dbo.usuarios(id_usuario) ON DELETE SET NULL,
    CONSTRAINT fk_mov_pedido FOREIGN KEY (id_pedido)
        REFERENCES dbo.pedidos(id_pedido) ON DELETE SET NULL,
    CONSTRAINT fk_mov_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES dbo.compra_detalle(id_compra_detalle) ON DELETE SET NULL
);
GO

-- ------------------------------------------------------------
-- Pagos, metodos de pago y comprobantes
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.metodos_pago', N'U') IS NULL
CREATE TABLE dbo.metodos_pago (
    id_metodo_pago  INT IDENTITY(1,1) PRIMARY KEY,
    nombre          NVARCHAR(50) NOT NULL UNIQUE  -- efectivo, tarjeta, yape, transferencia...
);
GO

IF OBJECT_ID(N'dbo.pagos', N'U') IS NULL
CREATE TABLE dbo.pagos (
    id_pago       INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido     INT NOT NULL,
    fecha         DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    monto_total   DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_pago_pedido FOREIGN KEY (id_pedido)
        REFERENCES dbo.pedidos(id_pedido) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.detalle_pago', N'U') IS NULL
CREATE TABLE dbo.detalle_pago (
    id_detalle_pago  INT IDENTITY(1,1) PRIMARY KEY,
    id_pago          INT NOT NULL,
    id_metodo_pago   INT NOT NULL,
    monto            DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_dp_pago FOREIGN KEY (id_pago)
        REFERENCES dbo.pagos(id_pago) ON DELETE CASCADE,
    CONSTRAINT fk_dp_metodo FOREIGN KEY (id_metodo_pago)
        REFERENCES dbo.metodos_pago(id_metodo_pago) ON DELETE NO ACTION
);
GO

IF OBJECT_ID(N'dbo.comprobante_pago', N'U') IS NULL
CREATE TABLE dbo.comprobante_pago (
    id_comprobante    INT IDENTITY(1,1) PRIMARY KEY,
    id_pago           INT NOT NULL UNIQUE,
    tipo_comprobante  NVARCHAR(10) NOT NULL
        CHECK (tipo_comprobante IN ('BOLETA','FACTURA')),
    serie             NVARCHAR(10) NOT NULL,
    numero            NVARCHAR(20) NOT NULL,
    fecha_emision     DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT fk_comp_pago FOREIGN KEY (id_pago)
        REFERENCES dbo.pagos(id_pago) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Indices de apoyo para los reportes mas frecuentes
-- ------------------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_pedidos_fecha' AND object_id = OBJECT_ID(N'dbo.pedidos'))
    CREATE INDEX idx_pedidos_fecha ON dbo.pedidos(fecha);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_pedidos_cliente' AND object_id = OBJECT_ID(N'dbo.pedidos'))
    CREATE INDEX idx_pedidos_cliente ON dbo.pedidos(id_cliente);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_compras_fecha' AND object_id = OBJECT_ID(N'dbo.compras'))
    CREATE INDEX idx_compras_fecha ON dbo.compras(fecha);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_compras_motivo' AND object_id = OBJECT_ID(N'dbo.compras'))
    CREATE INDEX idx_compras_motivo ON dbo.compras(motivo);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_piezas_variante_estado' AND object_id = OBJECT_ID(N'dbo.inventario_piezas'))
    CREATE INDEX idx_piezas_variante_estado ON dbo.inventario_piezas(id_variante, estado);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_piezas_medidas' AND object_id = OBJECT_ID(N'dbo.inventario_piezas'))
    CREATE INDEX idx_piezas_medidas ON dbo.inventario_piezas(ancho_cm, alto_cm);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_mov_fecha' AND object_id = OBJECT_ID(N'dbo.movimientos_inventario'))
    CREATE INDEX idx_mov_fecha ON dbo.movimientos_inventario(fecha);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_mov_variante' AND object_id = OBJECT_ID(N'dbo.movimientos_inventario'))
    CREATE INDEX idx_mov_variante ON dbo.movimientos_inventario(id_variante, fecha);
GO

-- ============================================================
-- 2) VISTAS
-- ============================================================

-- Variante con un nombre legible: "Vidrio catedral (4 mm)", "Aluminio u13 negro", etc.
CREATE OR ALTER VIEW dbo.vw_inv_variantes AS
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
FROM dbo.producto_variante pv
INNER JOIN dbo.productos p       ON p.id_producto = pv.id_producto
INNER JOIN dbo.caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
WHERE p.activo = 1;
GO

-- Piezas de vidrio/aluminio con el nombre del producto
CREATE OR ALTER VIEW dbo.vw_inv_piezas AS
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
FROM dbo.inventario_piezas ip
INNER JOIN dbo.vw_inv_variantes v ON v.id_variante = ip.id_variante;
GO

-- Resumen por variante: cuanto hay y si esta por debajo del minimo
CREATE OR ALTER VIEW dbo.vw_inv_resumen AS
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
FROM dbo.vw_inv_variantes v
LEFT JOIN (
    SELECT id_variante,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN 1 ELSE 0 END) AS piezas_disponibles,
           SUM(CASE WHEN estado = 'RESERVADA'  THEN 1 ELSE 0 END) AS piezas_reservadas,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN ancho_cm * alto_cm ELSE 0 END) AS area_disponible_cm2,
           SUM(CASE WHEN estado = 'DISPONIBLE' THEN largo_cm ELSE 0 END) AS largo_disponible_cm
    FROM dbo.inventario_piezas
    GROUP BY id_variante
) pz ON pz.id_variante = v.id_variante
LEFT JOIN dbo.inventario_cajas ic ON ic.id_variante = v.id_variante;
GO

-- Kardex legible
CREATE OR ALTER VIEW dbo.vw_inv_movimientos AS
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
FROM dbo.movimientos_inventario m
LEFT JOIN dbo.vw_inv_variantes v ON v.id_variante = m.id_variante
LEFT JOIN dbo.usuarios u         ON u.id_usuario = m.id_usuario;
GO

-- ============================================================
-- 2) PROCEDIMIENTOS ALMACENADOS / FUNCIONES
-- Cada procedimiento hace su trabajo dentro de una TRANSACCION: o se guarda todo
-- (el cambio + su registro en el kardex) o no se guarda nada (ROLLBACK en el CATCH).
-- Si se llaman dentro de una transaccion ajena (ej: el modulo de compras), usan un
-- punto de guardado (SAVE TRANSACTION): al fallar deshacen solo su parte y no
-- anulan la transaccion de quien los llamo.
-- Los errores de negocio se lanzan con THROW 50000+ y el mensaje llega
-- tal cual a la pantalla.
-- ============================================================

-- ------------------------------------------------------------
-- Ingresar una pieza de vidrio o aluminio
-- @origen: COMPRA (viene de compra_detalle), RETAZO (sobrante de un corte)
--          o INICIAL (ya estaba en el almacen)
-- Devuelve: id_pieza
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_inv_ingresar_pieza
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
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transaccion?

    DECLARE @unidad NVARCHAR(10) = (SELECT unidad_costo FROM dbo.producto_variante WHERE id_variante = @id_variante);

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
        THROW 50006, 'Un retazo debe indicar la pieza de la que salio.', 1;

    -- Solo se guardan las medidas que corresponden al tipo de material
    IF @unidad = 'CM2'       SET @largo_cm = NULL;
    IF @unidad = 'CM_LINEAL' SELECT @ancho_cm = NULL, @alto_cm = NULL;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        INSERT INTO dbo.inventario_piezas (id_variante, id_pieza_origen, id_compra_detalle, ancho_cm, alto_cm, largo_cm, estado, origen)
        VALUES (@id_variante, @id_pieza_origen, @id_compra_detalle, @ancho_cm, @alto_cm, @largo_cm, 'DISPONIBLE', @origen);

        DECLARE @id_pieza INT = SCOPE_IDENTITY();

        INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_compra_detalle, observacion)
        VALUES (@id_variante, @id_pieza, 'ENTRADA', @origen, 1, @id_usuario, @id_compra_detalle, @observacion);

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrio decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzo a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transaccion de quien lo llamo
        THROW;                             -- y devuelve el error a la aplicacion
    END CATCH

    SELECT @id_pieza AS id_pieza;
END
GO

-- ------------------------------------------------------------
-- Cambiar el estado de una pieza
--   DISPONIBLE -> RESERVADA | VENDIDA | CONSUMIDA | DEFECTUOSA
--   RESERVADA  -> DISPONIBLE | VENDIDA | CONSUMIDA | DEFECTUOSA
--   VENDIDA, CONSUMIDA y DEFECTUOSA son finales (la pieza ya salio)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_inv_cambiar_estado_pieza
    @id_pieza     INT,
    @estado       NVARCHAR(15),
    @id_usuario   INT = NULL,
    @id_pedido    INT = NULL,
    @observacion  NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transaccion?

    IF @estado NOT IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA')
        THROW 50010, 'Estado de pieza no valido.', 1;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        DECLARE @estado_actual NVARCHAR(15), @id_variante INT;
        SELECT @estado_actual = estado, @id_variante = id_variante
        FROM dbo.inventario_piezas WITH (UPDLOCK, ROWLOCK)
        WHERE id_pieza = @id_pieza;

        IF @estado_actual IS NULL
            THROW 50011, 'La pieza no existe.', 1;
        IF @estado_actual = @estado
            THROW 50012, 'La pieza ya tiene ese estado.', 1;
        IF @estado_actual IN ('VENDIDA','CONSUMIDA','DEFECTUOSA')
            THROW 50013, 'La pieza ya salio del inventario (vendida, consumida o defectuosa) y no puede cambiar de estado.', 1;

        UPDATE dbo.inventario_piezas SET estado = @estado WHERE id_pieza = @id_pieza;

        DECLARE @tipo NVARCHAR(10) =
            CASE WHEN @estado IN ('RESERVADA','DISPONIBLE') THEN 'ESTADO' ELSE 'SALIDA' END;
        DECLARE @motivo NVARCHAR(15) =
            CASE @estado WHEN 'RESERVADA'  THEN 'RESERVA'
                         WHEN 'DISPONIBLE' THEN 'LIBERACION'
                         WHEN 'VENDIDA'    THEN 'VENTA'
                         WHEN 'CONSUMIDA'  THEN 'CORTE'
                         WHEN 'DEFECTUOSA' THEN 'DEFECTO' END;

        INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES (@id_variante, @id_pieza, @tipo, @motivo, 1, @id_usuario, @id_pedido,
                ISNULL(@observacion, @estado_actual + ' -> ' + @estado));

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrio decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzo a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transaccion de quien lo llamo
        THROW;                             -- y devuelve el error a la aplicacion
    END CATCH
END
GO

-- ------------------------------------------------------------
-- Cortar una pieza
-- Se indica la medida que se corta; el sistema:
--   1. Marca la pieza original como CONSUMIDA
--   2. Calcula y registra los retazos que sobran (si son utiles)
--      Vidrio (ancho W x alto H, corte a x b):
--        retazo 1 = franja lateral   (W - a) x H
--        retazo 2 = franja inferior   a x (H - b)
--      Aluminio (largo L, corte l): retazo = L - l
--   Si el corte no entra derecho pero si girado 90 grados, se gira automaticamente.
--   Un sobrante menor a @minimo_util_cm en algun lado se considera desperdicio.
-- Devuelve: los retazos generados (puede ser 0, 1 o 2 filas)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_inv_cortar_pieza
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
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transaccion?

    DECLARE @retazos TABLE (n INT IDENTITY(1,1), id_pieza INT, ancho_cm DECIMAL(8,2), alto_cm DECIMAL(8,2), largo_cm DECIMAL(8,2));

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        DECLARE @estado NVARCHAR(15), @id_variante INT, @W DECIMAL(8,2), @H DECIMAL(8,2), @L DECIMAL(8,2);
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
                    DECLARE @tmp DECIMAL(8,2) = @ancho_corte;
                    SET @ancho_corte = @alto_corte;
                    SET @alto_corte = @tmp;
                END
                ELSE
                    THROW 50023, 'El corte es mas grande que la pieza.', 1;
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
                THROW 50025, 'El corte es mas largo que la pieza.', 1;

            IF @L - @largo_corte >= @minimo_util_cm
                INSERT INTO @retazos (largo_cm) VALUES (@L - @largo_corte);
        END

        -- 1. la pieza original sale del inventario
        UPDATE dbo.inventario_piezas SET estado = 'CONSUMIDA' WHERE id_pieza = @id_pieza;

        INSERT INTO dbo.movimientos_inventario (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES (@id_variante, @id_pieza, 'SALIDA', 'CORTE', 1, @id_usuario, @id_pedido,
                ISNULL(@observacion, 'Corte de ' +
                    CASE WHEN @L IS NULL
                         THEN CONVERT(NVARCHAR(12), @ancho_corte) + ' x ' + CONVERT(NVARCHAR(12), @alto_corte) + ' cm'
                         ELSE CONVERT(NVARCHAR(12), @largo_corte) + ' cm' END));

        -- 2. cada retazo util entra como pieza nueva
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

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrio decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzo a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transaccion de quien lo llamo
        THROW;                             -- y devuelve el error a la aplicacion
    END CATCH

    SELECT id_pieza, ancho_cm, alto_cm, largo_cm FROM @retazos;
END
GO

-- ------------------------------------------------------------
-- Mover cajas (tornillos / tarugos)
--   ENTRADA: suma @cantidad          (motivo COMPRA, INICIAL o AJUSTE)
--   SALIDA:  resta @cantidad         (motivo VENTA, DEFECTO o AJUSTE) - valida stock
--   AJUSTE:  el stock pasa a ser exactamente @cantidad (conteo fisico)
-- Si la variante todavia no tiene fila en inventario_cajas, se crea.
-- Devuelve: stock_resultante
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_inv_mover_cajas
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
    DECLARE @tran_externa BIT = CASE WHEN @@TRANCOUNT > 0 THEN 1 ELSE 0 END;  -- ¿me llamaron dentro de otra transaccion?

    DECLARE @unidad NVARCHAR(10) = (SELECT unidad_costo FROM dbo.producto_variante WHERE id_variante = @id_variante);

    IF @unidad IS NULL
        THROW 50030, 'La variante de producto no existe.', 1;
    IF @unidad <> 'CAJA'
        THROW 50031, 'Vidrio y aluminio se manejan por piezas, no por cajas.', 1;
    IF @tipo NOT IN ('ENTRADA','SALIDA','AJUSTE')
        THROW 50032, 'Tipo de movimiento no valido.', 1;
    IF @cantidad < 0 OR (@cantidad = 0 AND @tipo <> 'AJUSTE')
        THROW 50033, 'La cantidad debe ser mayor a cero.', 1;

    BEGIN TRY
    IF @tran_externa = 0 BEGIN TRANSACTION; ELSE SAVE TRANSACTION sp_inv;  -- propia, o punto de guardado

        IF NOT EXISTS (SELECT 1 FROM dbo.inventario_cajas WITH (UPDLOCK, HOLDLOCK) WHERE id_variante = @id_variante)
            INSERT INTO dbo.inventario_cajas (id_variante, stock_cajas) VALUES (@id_variante, 0);

        DECLARE @actual INT = (SELECT stock_cajas FROM dbo.inventario_cajas WITH (UPDLOCK) WHERE id_variante = @id_variante);
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

        UPDATE dbo.inventario_cajas SET stock_cajas = @nuevo WHERE id_variante = @id_variante;

        INSERT INTO dbo.movimientos_inventario (id_variante, tipo, motivo, cantidad, stock_resultante, id_usuario, id_pedido, id_compra_detalle, observacion)
        VALUES (@id_variante, @tipo, @motivo,
                CASE WHEN @tipo = 'AJUSTE' THEN @nuevo - @actual ELSE @cantidad END,
                @nuevo, @id_usuario, @id_pedido, @id_compra_detalle,
                ISNULL(@observacion, CASE WHEN @tipo = 'AJUSTE'
                                          THEN 'Ajuste de ' + CONVERT(NVARCHAR(12), @actual) + ' a ' + CONVERT(NVARCHAR(12), @nuevo) + ' cajas'
                                          END));

    IF @tran_externa = 0 COMMIT TRANSACTION;  -- si es de otro, el que la abrio decide el COMMIT
    END TRY
    BEGIN CATCH
        IF XACT_STATE() = -1 OR (XACT_STATE() = 1 AND @tran_externa = 0)
            ROLLBACK TRANSACTION;          -- deshace todo lo que se alcanzo a hacer
        ELSE IF XACT_STATE() = 1
            ROLLBACK TRANSACTION sp_inv;   -- deshace SOLO lo de este procedimiento, no la transaccion de quien lo llamo
        THROW;                             -- y devuelve el error a la aplicacion
    END CATCH

    SELECT @nuevo AS stock_resultante;
END
GO

-- ============================================================
-- Procedimientos del modulo de CLIENTES
-- (incluye las versiones mas recientes de la ficha comercial)
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.sp_cli_listar
    @texto NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @texto = NULLIF(LTRIM(RTRIM(@texto)), N'');
    DECLARE @patron NVARCHAR(604);
    SET @patron = N'%' + REPLACE(REPLACE(REPLACE(REPLACE(
        @texto, N'\', N'\\'), N'%', N'\%'), N'_', N'\_'), N'[', N'\[') + N'%';
    SELECT id_cliente, nombre, documento, telefono, direccion, version_cliente
    FROM dbo.clientes
    WHERE @texto IS NULL
       OR nombre LIKE @patron ESCAPE N'\'
       OR documento LIKE @patron ESCAPE N'\'
       OR telefono LIKE @patron ESCAPE N'\'
    ORDER BY nombre, id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_obtener
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0
        THROW 50101, 'Seleccione un cliente valido.', 1;
    SELECT id_cliente, nombre, documento, telefono, direccion, version_cliente
    FROM dbo.clientes WHERE id_cliente = @id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_guardar
    @id_cliente INT = 0,
    @nombre NVARCHAR(MAX),
    @documento NVARCHAR(MAX) = NULL,
    @telefono NVARCHAR(MAX) = NULL,
    @direccion NVARCHAR(MAX) = NULL,
    @version_original VARBINARY(8) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @nombre = NULLIF(LTRIM(RTRIM(@nombre)), N'');
    SET @documento = NULLIF(LTRIM(RTRIM(@documento)), N'');
    SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');
    SET @direccion = NULLIF(LTRIM(RTRIM(@direccion)), N'');
    IF @id_cliente IS NULL OR @id_cliente < 0
        THROW 50101, 'El ID del cliente no es valido.', 1;
    IF @nombre IS NULL
        THROW 50102, 'Ingrese el nombre del cliente.', 1;
    IF DATALENGTH(@nombre) > 300
        THROW 50103, 'El nombre admite hasta 150 caracteres.', 1;
    IF DATALENGTH(@documento) > 40
        THROW 50104, 'El documento admite hasta 20 caracteres.', 1;
    IF DATALENGTH(@telefono) > 40
        THROW 50105, 'El telefono admite hasta 20 caracteres.', 1;
    IF DATALENGTH(@direccion) > 510
        THROW 50106, 'La direccion admite hasta 255 caracteres.', 1;
    IF @telefono IS NOT NULL AND (
        @telefono COLLATE Latin1_General_100_BIN2 LIKE N'%[^0-9+() -]%'
        OR @telefono COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[0-9]%')
        THROW 50110, 'El telefono debe contener numeros; puede incluir espacios, +, parentesis y guiones.', 1;
    IF @id_cliente > 0 AND (@version_original IS NULL OR DATALENGTH(@version_original) <> 8)
        THROW 50109, 'Recargue el cliente antes de guardar sus cambios.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;
        IF @id_cliente > 0
        BEGIN
            DECLARE @version_actual BINARY(8);
            SELECT @version_actual = version_cliente
            FROM dbo.clientes WITH (UPDLOCK, HOLDLOCK)
            WHERE id_cliente = @id_cliente;
            IF @version_actual IS NULL
                THROW 50107, 'El cliente ya no existe. Actualice el listado.', 1;
            IF @version_actual <> @version_original
                THROW 50109, 'Otro trabajador modifico este cliente. Presione Recargar y revise los datos antes de guardar.', 1;
        END;

        IF @id_cliente = 0
        BEGIN
            INSERT INTO dbo.clientes (nombre, documento, telefono, direccion)
            VALUES (@nombre, @documento, @telefono, @direccion);
            SET @id_cliente = CONVERT(INT, SCOPE_IDENTITY());
        END
        ELSE
            UPDATE dbo.clientes
            SET nombre = @nombre, documento = @documento,
                telefono = @telefono, direccion = @direccion
            WHERE id_cliente = @id_cliente;
        COMMIT TRANSACTION;
        SELECT @id_cliente AS id_cliente;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_tiene_pedidos
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0
        THROW 50101, 'Seleccione un cliente valido.', 1;
    SELECT CONVERT(BIT, CASE WHEN EXISTS (
        SELECT 1 FROM dbo.pedidos WHERE id_cliente = @id_cliente)
        THEN 1 ELSE 0 END) AS tiene_pedidos;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_eliminar
    @id_cliente INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0
        THROW 50101, 'Seleccione un cliente valido.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (
            SELECT 1 FROM dbo.clientes WITH (XLOCK, HOLDLOCK)
            WHERE id_cliente = @id_cliente)
            THROW 50107, 'El cliente ya no existe. Actualice el listado.', 1;
        IF EXISTS (
            SELECT 1 FROM dbo.pedidos WITH (UPDLOCK, HOLDLOCK)
            WHERE id_cliente = @id_cliente)
            THROW 50108, 'El cliente tiene pedidos y no se puede eliminar. Su historial debe conservarse.', 1;
        DELETE FROM dbo.clientes WHERE id_cliente = @id_cliente;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_documento_coincidente
    @documento NVARCHAR(20),
    @id_excluir INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET @documento = NULLIF(LTRIM(RTRIM(@documento)), N'');
    IF @id_excluir IS NULL OR @id_excluir < 0
        THROW 50101, 'El ID del cliente no es valido.', 1;
    SELECT id_cliente, nombre
    FROM dbo.clientes
    WHERE @documento IS NOT NULL AND LTRIM(RTRIM(documento)) = @documento
      AND id_cliente <> @id_excluir
    ORDER BY nombre, id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_historial
    @id_cliente INT,
    @estado NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0
        THROW 50101, 'Seleccione un cliente valido.', 1;
    SET @estado = NULLIF(LTRIM(RTRIM(@estado)), N'');
    IF @estado IS NOT NULL AND @estado NOT IN ('PENDIENTE','PARCIAL','PAGADO','CANCELADO')
        THROW 50111, 'El estado del pedido no es valido.', 1;

    -- Agrupar cabeceras de pagos ANTES de unirlas: ni detalles ni metodos
    -- multiplican el total del pedido o el dinero registrado.
    SELECT p.id_pedido, p.fecha, p.estado, u.nombre AS vendedor, p.total,
           COALESCE(a.pagado, 0) AS pagado,
           CASE WHEN p.estado = 'CANCELADO' OR p.total <= COALESCE(a.pagado, 0)
                THEN CONVERT(DECIMAL(38,2), 0)
                ELSE p.total - COALESCE(a.pagado, 0) END AS saldo,
           CASE WHEN COALESCE(a.pagado, 0) > p.total
                THEN COALESCE(a.pagado, 0) - p.total
                ELSE CONVERT(DECIMAL(38,2), 0) END AS excedente
    FROM dbo.pedidos p
    INNER JOIN dbo.usuarios u ON u.id_usuario = p.id_usuario
    LEFT JOIN (
        SELECT id_pedido, SUM(CONVERT(DECIMAL(38,2), monto_total)) AS pagado
        FROM dbo.pagos GROUP BY id_pedido
    ) a ON a.id_pedido = p.id_pedido
    WHERE p.id_cliente = @id_cliente AND (@estado IS NULL OR p.estado = @estado)
    ORDER BY p.fecha DESC, p.id_pedido DESC;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_pagos_pedido
    @id_cliente INT,
    @id_pedido INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0 OR @id_pedido IS NULL OR @id_pedido <= 0
        THROW 50101, 'Seleccione un cliente y un pedido validos.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.pedidos WHERE id_pedido = @id_pedido AND id_cliente = @id_cliente)
        THROW 50112, 'El pedido no pertenece al cliente seleccionado o ya no existe.', 1;
    SELECT p.id_pago, p.fecha, p.monto_total AS total_pago,
           COALESCE(m.nombre, N'Sin desglose') AS metodo,
           COALESCE(d.monto, p.monto_total) AS monto_metodo,
           CASE WHEN c.id_comprobante IS NULL THEN N'Sin comprobante'
                ELSE c.tipo_comprobante + N' ' + c.serie + N'-' + c.numero END AS comprobante
    FROM dbo.pagos p
    LEFT JOIN dbo.detalle_pago d ON d.id_pago = p.id_pago
    LEFT JOIN dbo.metodos_pago m ON m.id_metodo_pago = d.id_metodo_pago
    LEFT JOIN dbo.comprobante_pago c ON c.id_pago = p.id_pago
    WHERE p.id_pedido = @id_pedido
    ORDER BY p.fecha DESC, p.id_pago DESC, d.id_detalle_pago;
END;
GO

-- ============================================================
-- 3) INSERT DE DATOS
-- ============================================================

-- Usuario administrador y mensaje por defecto (esquema base)
IF NOT EXISTS (SELECT 1 FROM dbo.usuarios WHERE nombre = N'admin')
BEGIN
    INSERT INTO dbo.usuarios (nombre, correo, contrasena, pregunta_seguridad, respuesta, rol)
    VALUES ('admin', 'admin@vidrieria.com', '123456', '¿Nombre de tu mascota?', 'firulais', 'ADMINISTRADOR');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tipos_mensaje WHERE codigo = N'CERRARSESION')
BEGIN
    INSERT INTO dbo.tipos_mensaje (codigo, titulo, texto)
    VALUES ('CERRARSESION', 'Cerrar sesión', '¿Seguro que deseas cerrar sesión?');
END
GO

-- ------------------------------------------------------------
-- Datos de PRUEBA para el modulo de inventario
-- Solo inserta si la tabla productos esta vacia (no duplica datos)
-- ------------------------------------------------------------
IF EXISTS (SELECT 1 FROM dbo.productos)
BEGIN
    PRINT 'Ya hay productos registrados: no se insertan datos de prueba.';
    RETURN;
END

-- Caracteristicas (espesor del vidrio / medida del tornillo o tarugo)
DECLARE @c_vid4 INT, @c_vid6 INT, @c_alu INT, @c_tor1 INT, @c_tar1 INT;
INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.40, NULL); SET @c_vid4 = SCOPE_IDENTITY();
INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.60, NULL); SET @c_vid6 = SCOPE_IDENTITY();
INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (NULL, NULL); SET @c_alu  = SCOPE_IDENTITY();
INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 2.54); SET @c_tor1 = SCOPE_IDENTITY();
INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 0.80); SET @c_tar1 = SCOPE_IDENTITY();

-- Productos
DECLARE @p_trans INT, @p_cated INT, @p_u13 INT, @p_tor INT, @p_tar INT;
INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO',   'transparente', NULL,    'Vidrio transparente'); SET @p_trans = SCOPE_IDENTITY();
INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO',   'catedral',     NULL,    'Vidrio catedral');     SET @p_cated = SCOPE_IDENTITY();
INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('ALUMINIO', 'u13',          'negro', 'Aluminio U13');        SET @p_u13   = SCOPE_IDENTITY();
INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('TORNILLO', 'autorroscante', NULL,  'Tornillo autorroscante'); SET @p_tor = SCOPE_IDENTITY();
INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('TARUGO',   'plastico',     NULL,    'Tarugo plástico');     SET @p_tar   = SCOPE_IDENTITY();

-- Variantes (precio por cm2, por cm lineal o por caja) + stock minimo
DECLARE @v_trans4 INT, @v_trans6 INT, @v_cated4 INT, @v_u13 INT, @v_tor INT, @v_tar INT;
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_trans, @c_vid4, 'CM2',       0.01,  3); SET @v_trans4 = SCOPE_IDENTITY();
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_trans, @c_vid6, 'CM2',       0.02,  2); SET @v_trans6 = SCOPE_IDENTITY();
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_cated, @c_vid4, 'CM2',       0.015, 2); SET @v_cated4 = SCOPE_IDENTITY();
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_u13,   @c_alu,  'CM_LINEAL', 0.10,  5); SET @v_u13    = SCOPE_IDENTITY();
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_tor,   @c_tor1, 'CAJA',      25.00, 4); SET @v_tor    = SCOPE_IDENTITY();
INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_tar,   @c_tar1, 'CAJA',      12.50, 4); SET @v_tar    = SCOPE_IDENTITY();

-- Inventario inicial (se registra por los procedimientos para que quede en el kardex)
DECLARE @admin INT = (SELECT TOP 1 id_usuario FROM dbo.usuarios WHERE nombre = 'admin');

EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 180, @alto_cm = 120, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 180, @alto_cm = 120, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 90,  @alto_cm = 60,  @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_trans6, @ancho_cm = 200, @alto_cm = 150, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_cated4, @ancho_cm = 150, @alto_cm = 100, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 600, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 600, @origen = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 350, @origen = 'INICIAL', @id_usuario = @admin;

EXEC dbo.sp_inv_mover_cajas @id_variante = @v_tor, @tipo = 'ENTRADA', @cantidad = 10, @motivo = 'INICIAL', @id_usuario = @admin;
EXEC dbo.sp_inv_mover_cajas @id_variante = @v_tar, @tipo = 'ENTRADA', @cantidad = 3,  @motivo = 'INICIAL', @id_usuario = @admin;  -- queda bajo el minimo (4)

PRINT 'Datos de prueba de inventario insertados.';
GO

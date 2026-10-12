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
--   + roles y permisos (tipo_permiso, rol_permiso, permiso)
--   + catalogo real de vidrios por pie2 y combos de registrar pedido
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
-- Roles y permisos
--   tipo_permiso : lista de permisos del sistema (cada uno habilita opciones del menú)
--   rol_permiso  : permisos que recibe por defecto cada rol (plantilla)
--   permiso      : permisos que tiene realmente cada usuario
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.tipo_permiso', N'U') IS NULL
CREATE TABLE dbo.tipo_permiso (
    id_tipo      INT IDENTITY(1,1) PRIMARY KEY,
    codigo       NVARCHAR(30)  NOT NULL UNIQUE,   -- lo usa el código del menú (USUARIOS, CLIENTES...)
    nombre       NVARCHAR(100) NOT NULL,
    descripcion  NVARCHAR(255) NULL
);
GO

IF OBJECT_ID(N'dbo.rol_permiso', N'U') IS NULL
CREATE TABLE dbo.rol_permiso (
    rol      NVARCHAR(20) NOT NULL
        CONSTRAINT ck_rol_permiso_rol CHECK (rol IN ('VENDEDOR','ADMINISTRADOR')),
    id_tipo  INT NOT NULL,
    CONSTRAINT pk_rol_permiso PRIMARY KEY (rol, id_tipo),
    CONSTRAINT fk_rolper_tipo FOREIGN KEY (id_tipo)
        REFERENCES dbo.tipo_permiso(id_tipo) ON DELETE CASCADE
);
GO

IF OBJECT_ID(N'dbo.permiso', N'U') IS NULL
CREATE TABLE dbo.permiso (
    id_usuario  INT NOT NULL,
    id_tipo     INT NOT NULL,
    CONSTRAINT pk_permiso PRIMARY KEY (id_usuario, id_tipo),
    CONSTRAINT fk_permiso_usuario FOREIGN KEY (id_usuario)
        REFERENCES dbo.usuarios(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_permiso_tipo FOREIGN KEY (id_tipo)
        REFERENCES dbo.tipo_permiso(id_tipo) ON DELETE CASCADE
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
    unidad_costo       NVARCHAR(10) NOT NULL,  -- CM2, CM_LINEAL, CAJA, PIE2
    precio_unitario    DECIMAL(10,2) NOT NULL,
    stock_minimo       INT NOT NULL CONSTRAINT df_variante_stock_minimo DEFAULT 0,
    CONSTRAINT uq_variante UNIQUE (id_producto, id_caracteristica),
    CONSTRAINT ck_variante_unidad_costo CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA','PIE2')),
    CONSTRAINT fk_var_producto FOREIGN KEY (id_producto)
        REFERENCES dbo.productos(id_producto) ON DELETE NO ACTION,
    CONSTRAINT fk_var_caract FOREIGN KEY (id_caracteristica)
        REFERENCES dbo.caracteristicas(id_caracteristica) ON DELETE NO ACTION
);
GO

-- Si la tabla ya existia de una corrida anterior (sin PIE2), se actualiza el CHECK
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'ck_variante_unidad_costo')
   AND NOT EXISTS (
       SELECT 1 FROM sys.check_constraints
       WHERE name = 'ck_variante_unidad_costo' AND definition LIKE '%PIE2%'
   )
BEGIN
    ALTER TABLE dbo.producto_variante DROP CONSTRAINT ck_variante_unidad_costo;
    ALTER TABLE dbo.producto_variante ADD CONSTRAINT ck_variante_unidad_costo
        CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA','PIE2'));
END
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
-- Items del pedido: agrupa los materiales de un mismo objeto
-- (una ventana, una mampara, un vidrio suelto, una caja de tornillos)
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.pedido_item', N'U') IS NULL
CREATE TABLE dbo.pedido_item (
    id_item      INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido    INT NOT NULL,
    tipo         NVARCHAR(15) NOT NULL
        CONSTRAINT ck_item_tipo CHECK (tipo IN ('CAJA','VIDRIO_SUELTO','VENTANA','MAMPARA', 'ALUMINIO')),
    descripcion  NVARCHAR(255) NULL,
    cantidad     INT NOT NULL DEFAULT 1
        CONSTRAINT ck_item_cantidad CHECK (cantidad > 0),
    estado       NVARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CONSTRAINT ck_item_estado CHECK (estado IN ('PENDIENTE','EN_PROCESO','COMPLETADO','CANCELADO')),
    subtotal     DECIMAL(10,2) NOT NULL DEFAULT 0,
    precio_materiales DECIMAL(10,2) NULL,   -- suma de los pedido_detalle (vidrio + aluminio + tornillos)
    costo_extra       DECIMAL(10,2) NOT NULL DEFAULT 0,  -- silicona+transporte, editable, precargado desde tarifas_ventana
    mano_obra         DECIMAL(10,2) NOT NULL DEFAULT 0,  -- 100% manual, sin sugerencia
    total             DECIMAL(10,2) NULL,
    CONSTRAINT fk_item_pedido FOREIGN KEY (id_pedido)
        REFERENCES dbo.pedidos(id_pedido) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Detalle de pedidos
-- ------------------------------------------------------------
IF COL_LENGTH('dbo.pedido_item','precio_materiales') IS NULL
    ALTER TABLE dbo.pedido_item ADD precio_materiales DECIMAL(10,2) NULL;
IF COL_LENGTH('dbo.pedido_item','costo_extra') IS NULL
    ALTER TABLE dbo.pedido_item ADD costo_extra DECIMAL(10,2) NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.pedido_item','mano_obra') IS NULL
    ALTER TABLE dbo.pedido_item ADD mano_obra DECIMAL(10,2) NOT NULL DEFAULT 0;
IF COL_LENGTH('dbo.pedido_item','total') IS NULL
    ALTER TABLE dbo.pedido_item ADD total DECIMAL(10,2) NULL;
GO

IF OBJECT_ID(N'dbo.pedido_detalle', N'U') IS NULL
CREATE TABLE dbo.pedido_detalle (
    id_detalle                INT IDENTITY(1,1) PRIMARY KEY,
    id_item                   INT NOT NULL,
    id_variante               INT NOT NULL,
    id_pieza                  INT NULL,        -- pieza usada o resultante del corte; NULL si accion = COMPRA_NUEVA y aun no llega
    id_compra_detalle         INT NULL,        -- si accion = COMPRA_NUEVA: que compra termino resolviendo esta linea
    accion                    NVARCHAR(15) NOT NULL
        CONSTRAINT ck_det_accion CHECK (accion IN ('USA_EXISTENTE','CORTA_PIEZA','COMPRA_NUEVA')),
    estado                    NVARCHAR(15) NOT NULL DEFAULT 'ACTIVO'
        CONSTRAINT ck_det_estado CHECK (estado IN ('ACTIVO','REEMPLAZADO','CANCELADO')),
    id_detalle_origen         INT NULL,        -- si esta linea reemplaza a otra (cambio de material, accidente)
    cantidad_cajas            INT NULL,        -- tornillo/tarugo
    ancho_cm                  DECIMAL(8,2),    -- medida real cortada o necesaria
    alto_cm                   DECIMAL(8,2),
    largo_cm                  DECIMAL(8,2),
    precio_unitario_aplicado  DECIMAL(10,2) NOT NULL,
    subtotal                  DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_det_item FOREIGN KEY (id_item)
        REFERENCES dbo.pedido_item(id_item) ON DELETE CASCADE,
    CONSTRAINT fk_det_variante FOREIGN KEY (id_variante)
        REFERENCES dbo.producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_det_pieza FOREIGN KEY (id_pieza)
        REFERENCES dbo.inventario_piezas(id_pieza) ON DELETE SET NULL,
    CONSTRAINT fk_det_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES dbo.compra_detalle(id_compra_detalle) ON DELETE SET NULL,
    CONSTRAINT fk_det_origen FOREIGN KEY (id_detalle_origen)
        REFERENCES dbo.pedido_detalle(id_detalle) ON DELETE NO ACTION
);
GO

-- Actualizar bases anteriores que aun no tienen detalles registrados.
-- Si hay detalles antiguos, se conservan y se solicita una migracion explicita.
IF COL_LENGTH('dbo.pedido_detalle','id_item') IS NULL
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.pedido_detalle)
        THROW 50200, 'Hay detalles de pedidos con el esquema anterior. Migrelos antes de actualizar; no se borraron datos.', 1;
    ALTER TABLE dbo.pedido_detalle ADD id_item INT NOT NULL;
END;
IF COL_LENGTH('dbo.pedido_detalle','id_pedido') IS NOT NULL
BEGIN
    EXEC(N'IF EXISTS (SELECT 1 FROM dbo.pedido_detalle WHERE id_pedido IS NOT NULL)
        THROW 50200, ''Hay detalles de pedidos con el esquema anterior. Migrelos antes de actualizar; no se borraron datos.'', 1;');
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'fk_det_pedido')
        ALTER TABLE dbo.pedido_detalle DROP CONSTRAINT fk_det_pedido;
    EXEC(N'ALTER TABLE dbo.pedido_detalle ALTER COLUMN id_pedido INT NULL;');
END;
IF COL_LENGTH('dbo.pedido_detalle','id_compra_detalle') IS NULL
    ALTER TABLE dbo.pedido_detalle ADD id_compra_detalle INT NULL;
IF COL_LENGTH('dbo.pedido_detalle','accion') IS NULL
    ALTER TABLE dbo.pedido_detalle ADD accion NVARCHAR(15) NOT NULL DEFAULT 'COMPRA_NUEVA';
IF COL_LENGTH('dbo.pedido_detalle','estado') IS NULL
    ALTER TABLE dbo.pedido_detalle ADD estado NVARCHAR(15) NOT NULL DEFAULT 'ACTIVO';
IF COL_LENGTH('dbo.pedido_detalle','id_detalle_origen') IS NULL
    ALTER TABLE dbo.pedido_detalle ADD id_detalle_origen INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'fk_det_item')
    ALTER TABLE dbo.pedido_detalle ADD CONSTRAINT fk_det_item FOREIGN KEY (id_item) REFERENCES dbo.pedido_item(id_item) ON DELETE CASCADE;
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'fk_det_compra_detalle')
    ALTER TABLE dbo.pedido_detalle ADD CONSTRAINT fk_det_compra_detalle FOREIGN KEY (id_compra_detalle) REFERENCES dbo.compra_detalle(id_compra_detalle) ON DELETE SET NULL;
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'fk_det_origen')
    ALTER TABLE dbo.pedido_detalle ADD CONSTRAINT fk_det_origen FOREIGN KEY (id_detalle_origen) REFERENCES dbo.pedido_detalle(id_detalle) ON DELETE NO ACTION;
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'ck_det_accion')
    ALTER TABLE dbo.pedido_detalle ADD CONSTRAINT ck_det_accion CHECK (accion IN ('USA_EXISTENTE','CORTA_PIEZA','COMPRA_NUEVA'));
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.pedido_detalle') AND name = 'ck_det_estado')
    ALTER TABLE dbo.pedido_detalle ADD CONSTRAINT ck_det_estado CHECK (estado IN ('ACTIVO','REEMPLAZADO','CANCELADO'));
GO

IF OBJECT_ID(N'dbo.pedido_detalle_medida', N'U') IS NULL
CREATE TABLE dbo.pedido_detalle_medida (
    id_medida   INT IDENTITY(1,1) PRIMARY KEY,
    id_detalle  INT NOT NULL,
    etiqueta    NVARCHAR(30) NOT NULL,   -- ANCHO_SUP, ANCHO_INF, ALTO_IZQ, ALTO_DER, etc.
    valor_cm    DECIMAL(8,2) NOT NULL,
    CONSTRAINT fk_medida_detalle FOREIGN KEY (id_detalle)
        REFERENCES dbo.pedido_detalle(id_detalle) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Tarifas sugeridas de costo extra de una ventana (aluminio/silicona) por
-- rango de perimetro. Solo sirve para PRECARGAR el campo "Extras" en el
-- formulario; el monto final que se cobra queda editable y se guarda en
-- pedido_item.costo_extra, no se vuelve a leer de aqui.
-- ------------------------------------------------------------
IF OBJECT_ID(N'dbo.tarifas_ventana', N'U') IS NULL
CREATE TABLE dbo.tarifas_ventana (
    id_tarifa         INT IDENTITY(1,1) PRIMARY KEY,
    perimetro_min_cm  DECIMAL(10,2) NOT NULL,
    perimetro_max_cm  DECIMAL(10,2) NOT NULL,
    costo_aluminio    DECIMAL(10,2) NOT NULL,
    costo_silicona    DECIMAL(10,2) NOT NULL,
    CONSTRAINT ck_tarifa_rango CHECK (perimetro_max_cm > perimetro_min_cm)
);
GO

INSERT INTO tarifas_ventana (perimetro_min_cm, perimetro_max_cm, costo_aluminio, costo_silicona)
VALUES (0, 300, 15, 5),
       (300.01, 480, 30, 10),
       (480.01, 700, 45, 15),
       (700.01, 99999, 60, 20);
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
-- TIPOS DE TABLA (TVP) para registrar un pedido completo de una sola vez
-- Nota: SQL Server no permite "CREATE OR ALTER TYPE". Si necesitas cambiar
-- las columnas de alguno de estos tipos, primero hay que hacer
-- DROP TYPE dbo.TVP_... (y recrear sp_ped_registrar, porque lo usa).
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoItem' AND is_table_type = 1)
CREATE TYPE dbo.TVP_PedidoItem AS TABLE (
    id_temp            INT NOT NULL PRIMARY KEY,
    tipo                NVARCHAR(20) NOT NULL,   -- VENTANA / MAMPARA / VIDRIO_SUELTO / CAJA
    etiqueta            NVARCHAR(50) NULL,
    ancho_cm            DECIMAL(10,2) NULL,
    alto_cm             DECIMAL(10,2) NULL,
    precio_materiales   DECIMAL(10,2) NULL,
    costo_extra         DECIMAL(10,2) NOT NULL,
    mano_obra           DECIMAL(10,2) NOT NULL,
    total               DECIMAL(10,2) NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoDetalle' AND is_table_type = 1)
CREATE TYPE dbo.TVP_PedidoDetalle AS TABLE (
    id_temp         INT NOT NULL PRIMARY KEY,
    id_item_temp    INT NOT NULL,
    id_variante     INT NOT NULL,
    accion          NVARCHAR(20) NOT NULL,   -- USA_EXISTENTE / CORTA_PIEZA / COMPRA_NUEVA
    id_pieza        INT NULL,                -- pieza a usar o a cortar; NULL si es COMPRA_NUEVA
    ancho_cm        DECIMAL(10,2) NULL,
    alto_cm         DECIMAL(10,2) NULL,
    largo_cm        DECIMAL(10,2) NULL,
    cantidad_cajas  INT NULL,
    subtotal        DECIMAL(10,2) NOT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoMedida' AND is_table_type = 1)
CREATE TYPE dbo.TVP_PedidoMedida AS TABLE (
    id_detalle_temp INT NOT NULL,
    etiqueta        NVARCHAR(20) NOT NULL,   -- ANCHO_SUP, ANCHO_INF, ALTO_IZQ, ALTO_DER
    valor_cm        DECIMAL(10,2) NOT NULL
);
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
    IF @unidad IN ('CM2','PIE2') AND (ISNULL(@ancho_cm, 0) <= 0 OR ISNULL(@alto_cm, 0) <= 0)
        THROW 50003, 'Para vidrio ingrese ancho y alto mayores a cero.', 1;
    IF @unidad = 'CM_LINEAL' AND ISNULL(@largo_cm, 0) <= 0
        THROW 50004, 'Para aluminio ingrese el largo mayor a cero.', 1;
    IF @origen = 'COMPRA' AND @id_compra_detalle IS NULL
        THROW 50005, 'Una pieza de origen COMPRA debe indicar el detalle de compra.', 1;
    IF @origen = 'RETAZO' AND @id_pieza_origen IS NULL
        THROW 50006, 'Un retazo debe indicar la pieza de la que salio.', 1;

    -- Solo se guardan las medidas que corresponden al tipo de material
    IF @unidad IN ('CM2','PIE2') SET @largo_cm = NULL;   -- vidrio (por cm2 o por pie2)
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

-- ------------------------------------------------------------
-- Registrar un pedido completo (uno o mas items: ventanas, mamparas,
-- vidrios sueltos, cajas) en una sola transaccion: o se guarda todo
-- (pedido + items + detalles + medidas + movimientos de inventario)
-- o no se guarda nada.
-- Cada fila de @Detalles resuelve su propia pieza segun @accion:
--   USA_EXISTENTE  -> reserva/vende la pieza indicada en @id_pieza
--   CORTA_PIEZA    -> corta @id_pieza y genera retazo(s) automaticamente
--   COMPRA_NUEVA   -> no toca inventario; el item queda PENDIENTE hasta
--                     que se registre la compra y se enlace despues
-- Devuelve: @id_pedido
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_registrar
    @id_cliente INT,
    @id_usuario INT,
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
        VALUES (@id_cliente, @id_usuario, SYSDATETIME(), 'PENDIENTE', 0);
        SET @id_pedido = CONVERT(INT, SCOPE_IDENTITY());

        -- mapeo id_temp (item) -> id real generado
        DECLARE @MapItems TABLE (id_temp INT PRIMARY KEY, id_item INT);

        DECLARE @id_temp INT, @tipo NVARCHAR(20), @etiqueta NVARCHAR(50),
                @anchoI DECIMAL(10,2), @altoI DECIMAL(10,2),
                @precio_mat DECIMAL(10,2), @extra DECIMAL(10,2), @mano DECIMAL(10,2), @totalI DECIMAL(10,2);

        DECLARE cur_items CURSOR LOCAL FAST_FORWARD FOR
            SELECT id_temp, tipo, etiqueta, ancho_cm, alto_cm, precio_materiales, costo_extra, mano_obra, total
            FROM @Items;
        OPEN cur_items;
        FETCH NEXT FROM cur_items INTO @id_temp, @tipo, @etiqueta, @anchoI, @altoI, @precio_mat, @extra, @mano, @totalI;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO dbo.pedido_item (id_pedido, tipo, descripcion, estado,
                                          precio_materiales, costo_extra, mano_obra, total)
            VALUES (@id_pedido, @tipo, @etiqueta, 'PENDIENTE',
                    @precio_mat, @extra, @mano, @totalI);

            INSERT INTO @MapItems VALUES (@id_temp, CONVERT(INT, SCOPE_IDENTITY()));

            FETCH NEXT FROM cur_items INTO @id_temp, @tipo, @etiqueta, @anchoI, @altoI, @precio_mat, @extra, @mano, @totalI;
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

-- ------------------------------------------------------------
-- Combos de frmRegistrarPedido: aluminio (color -> tipo, en cascada)
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_combo_colores_aluminio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT color
    FROM dbo.productos
    WHERE categoria = 'ALUMINIO' AND activo = 1 AND color IS NOT NULL
    ORDER BY color;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ped_combo_tipos_aluminio
    @color NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT tipo
    FROM dbo.productos
    WHERE categoria = 'ALUMINIO' AND color = @color AND activo = 1
    ORDER BY tipo;
END
GO

-- ------------------------------------------------------------
-- Combos de frmRegistrarPedido: vidrio.
-- El vidrio se identifica por la combinacion tipo + color + espesor:
-- tipo  (COLOR, ESPEJO, FIGURA...) y color (INCOLORO, AZUL...)
-- salen de productos.tipo y productos.color.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_combo_colores_vidrio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT tipo AS color_vidrio
    FROM dbo.productos
    WHERE categoria = 'VIDRIO' AND activo = 1
    ORDER BY tipo;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ped_combo_espesores_vidrio
    @tipo  NVARCHAR(50),
    @color NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT c.espesor_cm
    FROM dbo.producto_variante pv
    INNER JOIN dbo.productos p       ON p.id_producto = pv.id_producto
    INNER JOIN dbo.caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE p.categoria = 'VIDRIO' AND p.tipo = @tipo
      AND ISNULL(p.color, '') = ISNULL(@color, '') AND p.activo = 1
    ORDER BY c.espesor_cm;
END
GO

-- ------------------------------------------------------------
-- Espesores de un producto concreto (se elige el producto con
-- frmListadoProductos y de ahi salen sus espesores disponibles).
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_espesores_por_producto
    @id_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT DISTINCT c.espesor_cm
    FROM dbo.producto_variante pv
    INNER JOIN dbo.caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE pv.id_producto = @id_producto AND c.espesor_cm IS NOT NULL
    ORDER BY c.espesor_cm;
END
GO

-- ------------------------------------------------------------
-- Resolver la variante exacta (id_variante + precio) de un producto
-- segun el espesor elegido. Es lo que va dentro del TVP_PedidoDetalle.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_resolver_variante_producto
    @id_producto INT,
    @espesor_cm  DECIMAL(6,2)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM dbo.producto_variante pv
    INNER JOIN dbo.caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE pv.id_producto = @id_producto AND c.espesor_cm = @espesor_cm;
END
GO

-- ------------------------------------------------------------
-- Resolver la variante exacta (id_variante + precio) una vez elegidos
-- todos los combos. Esto es lo que va dentro del TVP_PedidoDetalle.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_resolver_variante_vidrio
    @tipo       NVARCHAR(50),
    @color      NVARCHAR(30),
    @espesor_cm DECIMAL(6,2)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM dbo.producto_variante pv
    INNER JOIN dbo.productos p       ON p.id_producto = pv.id_producto
    INNER JOIN dbo.caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE p.categoria = 'VIDRIO' AND p.tipo = @tipo
      AND ISNULL(p.color, '') = ISNULL(@color, '')
      AND c.espesor_cm = @espesor_cm AND p.activo = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ped_resolver_variante_aluminio
    @color NVARCHAR(30),
    @tipo  NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM dbo.producto_variante pv
    INNER JOIN dbo.productos p ON p.id_producto = pv.id_producto
    WHERE p.categoria = 'ALUMINIO' AND p.color = @color AND p.tipo = @tipo AND p.activo = 1;
END
GO

-- ------------------------------------------------------------
-- Sugerencia de "Extras" (aluminio + silicona) segun el perimetro
-- de la ventana. Solo precarga el campo; el usuario lo puede editar.
-- ------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_ped_tarifa_sugerida
    @perimetro_cm DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 costo_aluminio, costo_silicona
    FROM dbo.tarifas_ventana
    WHERE @perimetro_cm >= perimetro_min_cm AND @perimetro_cm <= perimetro_max_cm;
END
GO

-- ============================================================
-- Procedimientos del modulo de CLIENTES
-- (incluye las versiones mas recientes de la ficha comercial)
-- ============================================================

-- INICIO ACTUALIZACION CLIENTES
IF COL_LENGTH('dbo.clientes','version_cliente') IS NULL
    ALTER TABLE dbo.clientes ADD version_cliente ROWVERSION NOT NULL;
IF COL_LENGTH('dbo.clientes','tipo_persona') IS NULL
    ALTER TABLE dbo.clientes ADD tipo_persona NVARCHAR(10) NULL;
IF COL_LENGTH('dbo.clientes','tipo_documento') IS NULL
    ALTER TABLE dbo.clientes ADD tipo_documento NVARCHAR(15) NULL;
GO

-- Los registros anteriores quedan sin clasificar y mantienen sus datos.
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.clientes') AND name = 'CK_clientes_identificacion')
    ALTER TABLE dbo.clientes WITH CHECK ADD CONSTRAINT CK_clientes_identificacion CHECK (
        (tipo_persona IS NULL AND tipo_documento IS NULL)
        OR (tipo_persona IS NOT NULL AND tipo_documento IS NOT NULL AND (
            (tipo_persona = N'NATURAL' AND tipo_documento IN (N'DNI',N'RUC',N'SIN DOCUMENTO'))
            OR (tipo_persona = N'JURIDICA' AND tipo_documento = N'RUC'))));
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.clientes') AND name = 'CK_clientes_documento')
    ALTER TABLE dbo.clientes WITH CHECK ADD CONSTRAINT CK_clientes_documento CHECK (
        tipo_documento IS NULL
        OR (tipo_documento = N'SIN DOCUMENTO' AND documento IS NULL)
        OR (tipo_documento IN (N'DNI',N'RUC') AND documento IS NOT NULL
            AND documento COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9]%'
            AND DATALENGTH(documento) = CASE WHEN tipo_documento = N'DNI' THEN 16 ELSE 22 END
            AND (tipo_documento <> N'RUC'
                OR (tipo_persona = N'JURIDICA' AND LEFT(documento,2) = N'20')
                OR (tipo_persona = N'NATURAL' AND LEFT(documento,2) IN (N'10',N'15')))));
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.clientes') AND name = 'UX_clientes_documento')
    CREATE UNIQUE INDEX UX_clientes_documento ON dbo.clientes(tipo_documento,documento)
    WHERE tipo_documento IS NOT NULL AND documento IS NOT NULL;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_listar
    @texto NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @texto = NULLIF(LTRIM(RTRIM(@texto)), N'');
    DECLARE @patron NVARCHAR(604);
    SET @patron = N'%' + REPLACE(REPLACE(REPLACE(REPLACE(
        @texto, N'\', N'\\'), N'%', N'\%'), N'_', N'\_'), N'[', N'\[') + N'%';
    SELECT id_cliente, nombre, documento, telefono, direccion, version_cliente,
           CASE tipo_persona WHEN N'NATURAL' THEN N'Natural' WHEN N'JURIDICA' THEN N'Jurídica' ELSE N'Por clasificar' END AS tipo_persona,
           COALESCE(tipo_documento,N'Por clasificar') AS tipo_documento
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
    SELECT id_cliente, nombre, documento, telefono, direccion, version_cliente,
           tipo_persona, tipo_documento
    FROM dbo.clientes WHERE id_cliente = @id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_guardar
    @id_cliente INT = 0,
    @nombre NVARCHAR(MAX),
    @documento NVARCHAR(MAX) = NULL,
    @telefono NVARCHAR(MAX) = NULL,
    @direccion NVARCHAR(MAX) = NULL,
    @version_original VARBINARY(8) = NULL,
    @tipo_persona NVARCHAR(MAX) = NULL,
    @tipo_documento NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET @nombre = NULLIF(LTRIM(RTRIM(@nombre)), N'');
    SET @documento = NULLIF(LTRIM(RTRIM(@documento)), N'');
    SET @telefono = NULLIF(LTRIM(RTRIM(@telefono)), N'');
    SET @direccion = NULLIF(LTRIM(RTRIM(@direccion)), N'');
    SET @tipo_persona = UPPER(NULLIF(LTRIM(RTRIM(@tipo_persona)), N''));
    SET @tipo_documento = UPPER(NULLIF(LTRIM(RTRIM(@tipo_documento)), N''));
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
        OR CHARINDEX(N'+', SUBSTRING(@telefono,2,20)) > 0
        OR LEN(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@telefono,N' ',N''),N'+',N''),N'(',N''),N')',N''),N'-',N'')) NOT BETWEEN 7 AND 15)
        THROW 50110, 'El telefono debe contener entre 7 y 15 digitos. El + solo se permite al inicio.', 1;
    IF @telefono IS NOT NULL AND (
        LEN(@telefono)-LEN(REPLACE(@telefono,N'(',N'')) <> LEN(@telefono)-LEN(REPLACE(@telefono,N')',N''))
        OR LEN(@telefono)-LEN(REPLACE(@telefono,N'(',N'')) > 1
        OR (CHARINDEX(N'(',@telefono) > 0 AND CHARINDEX(N'(',@telefono) > CHARINDEX(N')',@telefono)))
        THROW 50110, 'Revise los parentesis del telefono; se permite un par alrededor del codigo de area.', 1;
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
            IF @tipo_persona IS NULL AND @tipo_documento IS NULL
                SELECT @tipo_persona = tipo_persona, @tipo_documento = tipo_documento
                FROM dbo.clientes WHERE id_cliente = @id_cliente;
        END;

        IF NOT ((@tipo_persona IS NULL AND @tipo_documento IS NULL)
            OR (@tipo_persona IS NOT NULL AND @tipo_documento IS NOT NULL AND (
                (@tipo_persona = N'NATURAL' AND @tipo_documento IN (N'DNI',N'RUC',N'SIN DOCUMENTO'))
                OR (@tipo_persona = N'JURIDICA' AND @tipo_documento = N'RUC'))))
            THROW 50113, 'Seleccione un tipo de persona y documento compatibles. La persona juridica requiere RUC.', 1;
        IF @tipo_documento = N'SIN DOCUMENTO' AND @documento IS NOT NULL
            THROW 50114, 'Deje el numero vacio si selecciona Sin documento.', 1;
        IF @tipo_documento IN (N'DNI',N'RUC') AND (
            @documento IS NULL OR @documento COLLATE Latin1_General_100_BIN2 LIKE N'%[^0-9]%'
            OR DATALENGTH(@documento) <> CASE WHEN @tipo_documento = N'DNI' THEN 16 ELSE 22 END)
            THROW 50114, 'Ingrese 8 digitos para DNI u 11 digitos para RUC, sin espacios ni letras.', 1;
        IF @tipo_documento = N'RUC' AND (
            (@tipo_persona = N'JURIDICA' AND LEFT(@documento,2) <> N'20')
            OR (@tipo_persona = N'NATURAL' AND LEFT(@documento,2) NOT IN (N'10',N'15')))
            THROW 50114, 'El RUC debe empezar por 20 para persona juridica, o por 10 o 15 para natural.', 1;

        IF @documento IS NOT NULL AND EXISTS (
            SELECT 1 FROM dbo.clientes WITH (UPDLOCK,HOLDLOCK)
            WHERE LTRIM(RTRIM(documento)) = @documento AND id_cliente <> @id_cliente
              AND (@tipo_documento IS NULL OR tipo_documento IS NULL OR tipo_documento = @tipo_documento))
            THROW 50115, 'El documento ya pertenece a otro cliente. Busque el registro existente.', 1;

        IF @id_cliente = 0
        BEGIN
            INSERT INTO dbo.clientes (nombre, documento, telefono, direccion, tipo_persona, tipo_documento)
            VALUES (@nombre, @documento, @telefono, @direccion, @tipo_persona, @tipo_documento);
            SET @id_cliente = CONVERT(INT, SCOPE_IDENTITY());
        END
        ELSE
            UPDATE dbo.clientes
            SET nombre = @nombre, documento = @documento,
                telefono = @telefono, direccion = @direccion,
                tipo_persona = @tipo_persona, tipo_documento = @tipo_documento
            WHERE id_cliente = @id_cliente;
        COMMIT TRANSACTION;
        SELECT @id_cliente AS id_cliente;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        IF ERROR_NUMBER() IN (2601,2627)
            THROW 50115, 'El documento ya pertenece a otro cliente. Busque el registro existente.', 1;
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
    @id_excluir INT = 0,
    @tipo_documento NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @documento = NULLIF(LTRIM(RTRIM(@documento)), N'');
    SET @tipo_documento = UPPER(NULLIF(LTRIM(RTRIM(@tipo_documento)), N''));
    IF @id_excluir IS NULL OR @id_excluir < 0
        THROW 50101, 'El ID del cliente no es valido.', 1;
    SELECT id_cliente, nombre
    FROM dbo.clientes
    WHERE @documento IS NOT NULL AND LTRIM(RTRIM(documento)) = @documento
      AND id_cliente <> @id_excluir
      AND (@tipo_documento IS NULL OR tipo_documento IS NULL OR tipo_documento = @tipo_documento)
    ORDER BY nombre, id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_historial
    @id_cliente INT,
    @estado NVARCHAR(15) = NULL,
    @desde DATE = NULL,
    @hasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_cliente IS NULL OR @id_cliente <= 0
        THROW 50101, 'Seleccione un cliente valido.', 1;
    SET @estado = NULLIF(LTRIM(RTRIM(@estado)), N'');
    IF @estado IS NOT NULL AND @estado NOT IN ('PENDIENTE','PARCIAL','PAGADO','CANCELADO')
        THROW 50111, 'El estado del pedido no es valido.', 1;
    IF @desde > @hasta
        THROW 50116, 'La fecha Desde no puede ser posterior a Hasta.', 1;

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
      AND (@desde IS NULL OR CONVERT(DATE,p.fecha) >= @desde)
      AND (@hasta IS NULL OR CONVERT(DATE,p.fecha) <= @hasta)
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
-- FIN ACTUALIZACION CLIENTES
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
-- Roles y permisos: datos
-- (va ANTES de los datos de prueba de inventario, porque ese bloque
--  termina con RETURN y lo que esté después no se ejecutaría)
-- ------------------------------------------------------------

-- Lista de permisos (solo agrega los que falten)
INSERT INTO dbo.tipo_permiso (codigo, nombre, descripcion)
SELECT v.codigo, v.nombre, v.descripcion
FROM (VALUES
    (N'USUARIOS',   N'Gestión de usuarios',  N'Registrar, modificar, eliminar y dar de baja usuarios'),
    (N'PERMISOS',   N'Asignar permisos',     N'Asignar o quitar permisos a los usuarios'),
    (N'CLIENTES',   N'Gestión de clientes',  N'Registrar, modificar y ver el historial de clientes'),
    (N'PRODUCTOS',  N'Gestión de productos', N'Registrar, modificar y dar de baja productos'),
    (N'INVENTARIO', N'Inventario',           N'Ver inventario, piezas, cajas y kardex'),
    (N'COMPRAS',    N'Compras',              N'Registrar y ver compras'),
    (N'PEDIDOS',    N'Pedidos y ventas',     N'Cotizar, registrar pedidos y ver ventas'),
    (N'PAGOS',      N'Pagos',                N'Registrar pagos y comprobantes'),
    (N'REPORTES',   N'Reportes',             N'Ver los reportes del negocio')
) AS v(codigo, nombre, descripcion)
WHERE NOT EXISTS (SELECT 1 FROM dbo.tipo_permiso t WHERE t.codigo = v.codigo);
GO

-- Permisos por defecto de cada rol
INSERT INTO dbo.rol_permiso (rol, id_tipo)
SELECT r.rol, t.id_tipo
FROM (VALUES
    (N'ADMINISTRADOR', N'USUARIOS'),  (N'ADMINISTRADOR', N'PERMISOS'),
    (N'ADMINISTRADOR', N'CLIENTES'),  (N'ADMINISTRADOR', N'PRODUCTOS'),
    (N'ADMINISTRADOR', N'INVENTARIO'),(N'ADMINISTRADOR', N'COMPRAS'),
    (N'ADMINISTRADOR', N'PEDIDOS'),   (N'ADMINISTRADOR', N'PAGOS'),
    (N'ADMINISTRADOR', N'REPORTES'),
    (N'VENDEDOR', N'CLIENTES'),   (N'VENDEDOR', N'INVENTARIO'),
    (N'VENDEDOR', N'COMPRAS'),    (N'VENDEDOR', N'PEDIDOS'),
    (N'VENDEDOR', N'PAGOS')
) AS r(rol, codigo)
INNER JOIN dbo.tipo_permiso t ON t.codigo = r.codigo
WHERE NOT EXISTS (SELECT 1 FROM dbo.rol_permiso x WHERE x.rol = r.rol AND x.id_tipo = t.id_tipo);
GO

-- Usuarios que todavía no tienen NINGÚN permiso reciben los de su rol
-- (si a alguien ya se le ajustaron sus permisos, no se tocan)
INSERT INTO dbo.permiso (id_usuario, id_tipo)
SELECT u.id_usuario, rp.id_tipo
FROM dbo.usuarios u
INNER JOIN dbo.rol_permiso rp ON rp.rol = u.rol
WHERE NOT EXISTS (SELECT 1 FROM dbo.permiso p WHERE p.id_usuario = u.id_usuario);
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

-- ============================================================
-- Catalogo real de vidrios (color, espejo, figura)
-- Precio por pie2: alto_cm * ancho_cm / 900 * precio_unitario
-- Idempotente: cada producto se inserta solo si no existe.
-- ============================================================

DECLARE @e3 INT, @e4 INT, @e55 INT, @e8 INT, @e10 INT;

SELECT @e3  = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm = 0.30 AND medida_cm IS NULL;
IF @e3 IS NULL BEGIN INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.30, NULL); SET @e3 = SCOPE_IDENTITY(); END

SELECT @e4  = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm = 0.40 AND medida_cm IS NULL;
IF @e4 IS NULL BEGIN INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.40, NULL); SET @e4 = SCOPE_IDENTITY(); END

SELECT @e55 = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm = 0.55 AND medida_cm IS NULL;
IF @e55 IS NULL BEGIN INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.55, NULL); SET @e55 = SCOPE_IDENTITY(); END

SELECT @e8  = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm = 0.80 AND medida_cm IS NULL;
IF @e8 IS NULL BEGIN INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (0.80, NULL); SET @e8 = SCOPE_IDENTITY(); END

SELECT @e10 = id_caracteristica FROM dbo.caracteristicas WHERE espesor_cm = 1.00 AND medida_cm IS NULL;
IF @e10 IS NULL BEGIN INSERT INTO dbo.caracteristicas (espesor_cm, medida_cm) VALUES (1.00, NULL); SET @e10 = SCOPE_IDENTITY(); END

-- ===================== COLOR =====================

DECLARE @p_incoloro INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='INCOLORO')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','COLOR','INCOLORO','Vidrio incoloro');
    SET @p_incoloro = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_incoloro, @e3,  'PIE2', 3.50, 0),
        (@p_incoloro, @e4,  'PIE2', 4.00, 0),
        (@p_incoloro, @e55, 'PIE2', 4.80, 0),
        (@p_incoloro, @e8,  'PIE2', 6.80, 0),
        (@p_incoloro, @e10, 'PIE2', 8.00, 0);
END

DECLARE @p_azul INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='AZUL')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','COLOR','AZUL','Vidrio color azul');
    SET @p_azul = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_azul, @e3,  'PIE2', 5.50, 0),
        (@p_azul, @e4,  'PIE2', 6.00, 0),
        (@p_azul, @e55, 'PIE2', 6.80, 0),
        (@p_azul, @e8,  'PIE2', 8.80, 0),
        (@p_azul, @e10, 'PIE2', 10.00, 0);
END

DECLARE @p_marron INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='MARRON')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','COLOR','MARRON','Vidrio color marrón');
    SET @p_marron = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_marron, @e3,  'PIE2', 5.00, 0),
        (@p_marron, @e4,  'PIE2', 5.50, 0),
        (@p_marron, @e55, 'PIE2', 6.30, 0),
        (@p_marron, @e8,  'PIE2', 8.30, 0),
        (@p_marron, @e10, 'PIE2', 9.50, 0);
END

-- ===================== ESPEJO =====================
-- solido: hasta 5.5mm. azul y gris: desde 5.5mm (no hay 3mm ni 4mm).

DECLARE @p_esp_solido INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='SOLIDO')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','ESPEJO','SOLIDO','Espejo sólido');
    SET @p_esp_solido = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_esp_solido, @e3,  'PIE2', 6.50, 0),
        (@p_esp_solido, @e4,  'PIE2', 7.00, 0),
        (@p_esp_solido, @e55, 'PIE2', 7.80, 0);
END

DECLARE @p_esp_azul INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='AZUL')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','ESPEJO','AZUL','Espejo azul');
    SET @p_esp_azul = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_esp_azul, @e55, 'PIE2', 10.30, 0),
        (@p_esp_azul, @e8,  'PIE2', 12.30, 0),
        (@p_esp_azul, @e10, 'PIE2', 13.50, 0);
END

DECLARE @p_esp_gris INT;
IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='GRIS')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','ESPEJO','GRIS','Espejo gris');
    SET @p_esp_gris = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_esp_gris, @e55, 'PIE2', 13.30, 0),
        (@p_esp_gris, @e8,  'PIE2', 15.30, 0),
        (@p_esp_gris, @e10, 'PIE2', 16.50, 0);
END

-- ===================== FIGURA =====================
-- mismo precio que incoloro, solo hasta 5.5mm (no hay 8mm a mas)

DECLARE @p_catedral INT, @p_cruz INT, @p_flor INT;

IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='CATEDRAL')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','FIGURA','CATEDRAL','Vidrio figura catedral');
    SET @p_catedral = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_catedral, @e3,  'PIE2', 3.50, 0),
        (@p_catedral, @e4,  'PIE2', 4.00, 0),
        (@p_catedral, @e55, 'PIE2', 4.80, 0);
END

IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='CRUZ')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','FIGURA','CRUZ','Vidrio figura cruz');
    SET @p_cruz = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_cruz, @e3,  'PIE2', 3.50, 0),
        (@p_cruz, @e4,  'PIE2', 4.00, 0),
        (@p_cruz, @e55, 'PIE2', 4.80, 0);
END

IF NOT EXISTS (SELECT 1 FROM dbo.productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='FLOR')
BEGIN
    INSERT INTO dbo.productos (categoria, tipo, color, nombre) VALUES ('VIDRIO','FIGURA','FLOR','Vidrio figura flor');
    SET @p_flor = SCOPE_IDENTITY();
    INSERT INTO dbo.producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
        (@p_flor, @e3,  'PIE2', 3.50, 0),
        (@p_flor, @e4,  'PIE2', 4.00, 0),
        (@p_flor, @e55, 'PIE2', 4.80, 0);
END

PRINT 'Catalogo de vidrios (color/espejo/figura) insertado.';
GO

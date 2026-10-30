-- ============================================================
-- Script UNICO y consolidado: Sistema para vidrieria
-- Motor: PostgreSQL 11+ (PL/pgSQL)
--
-- Version equivalente de 00_vidrieria_completo.sql (SQL Server).
-- Mantiene el mismo orden funcional:
--   esquema base, modulo de inventario, datos de prueba,
--   modulo y ficha comercial de clientes, roles y permisos,
--   catalogo real de vidrios/aluminio, modelos de ventana,
--   modulo de proveedores y catalogo simulado de proveedores.
--
-- Orden de ejecucion:
--   1) Funcion de hora, TABLAS y migraciones
--   2) VISTAS y FUNCIONES (procedimientos)
--   3) INSERT de datos y catalogos
--   4) Modulo de proveedores y catalogo simulado
--
-- Es idempotente: se puede volver a ejecutar sin generar errores.
--
-- IMPORTANTE:
--   * PostgreSQL no permite CREATE DATABASE dentro de una transaccion.
--     Cree la base antes (por ejemplo: CREATE DATABASE vidrieria;) y
--     ejecute este script conectado a ella.
--   * Todo el script corre en UNA transaccion: si algo falla, no se
--     aplica ningun cambio.
--   * Las funciones NO abren ni cierran transacciones: la transaccion
--     la controla quien las llama. Ante un error lanzan una excepcion.
--   * Los TVP de SQL Server se representan como parametros JSONB.
-- ============================================================

BEGIN;

-- ------------------------------------------------------------
-- Hora de Peru (UTC-5) para todas las fechas del sistema.
-- now() devuelve la hora del servidor; se convierte a la zona de Lima
-- para no depender de la configuracion horaria del hosting.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION fn_hora_peru() RETURNS timestamp AS $$
    SELECT (now() AT TIME ZONE 'America/Lima')::timestamp;
$$ LANGUAGE sql STABLE;

-- ============================================================
-- 1) TABLAS
-- ============================================================

-- ------------------------------------------------------------
-- Usuarios y preferencias
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario          serial PRIMARY KEY,
    nombre              varchar(150) NOT NULL UNIQUE,
    correo              varchar(150) NOT NULL UNIQUE,
    contrasena          varchar(255) NOT NULL,
    pregunta_seguridad  varchar(255) NOT NULL,
    respuesta           varchar(255) NOT NULL,
    rol                 varchar(20) NOT NULL DEFAULT 'VENDEDOR'
        CHECK (rol IN ('VENDEDOR','ADMINISTRADOR')),
    activo              boolean NOT NULL DEFAULT true,
    fecha_creacion      timestamp NOT NULL DEFAULT fn_hora_peru()
);

CREATE TABLE IF NOT EXISTS usuario_preferencias (
    id_usuario  integer PRIMARY KEY,
    tema        varchar(10) NOT NULL DEFAULT 'CLARO'
        CHECK (tema IN ('CLARO','OSCURO','AZULCLARO','AZULOSCURO')),
    fuente      varchar(50) NOT NULL DEFAULT 'Arial',
    CONSTRAINT fk_pref_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS tipos_mensaje (
    id_mensaje   serial PRIMARY KEY,
    codigo       varchar(50)  NOT NULL UNIQUE,
    titulo       varchar(255) NOT NULL,
    texto        varchar(255)
);

CREATE TABLE IF NOT EXISTS usuario_mensajes_descartados (
    id_usuario      integer NOT NULL,
    id_mensaje      integer NOT NULL,
    fecha_descarte  timestamp NOT NULL DEFAULT fn_hora_peru(),
    CONSTRAINT pk_usuario_mensaje PRIMARY KEY (id_usuario, id_mensaje),
    CONSTRAINT fk_desc_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_desc_mensaje FOREIGN KEY (id_mensaje)
        REFERENCES tipos_mensaje(id_mensaje) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Roles y permisos
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tipo_permiso (
    id_tipo      serial PRIMARY KEY,
    codigo       varchar(30)  NOT NULL UNIQUE,
    nombre       varchar(100) NOT NULL,
    descripcion  varchar(255)
);

CREATE TABLE IF NOT EXISTS rol_permiso (
    rol      varchar(20) NOT NULL
        CONSTRAINT ck_rol_permiso_rol CHECK (rol IN ('VENDEDOR','ADMINISTRADOR')),
    id_tipo  integer NOT NULL,
    CONSTRAINT pk_rol_permiso PRIMARY KEY (rol, id_tipo),
    CONSTRAINT fk_rolper_tipo FOREIGN KEY (id_tipo)
        REFERENCES tipo_permiso(id_tipo) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS permiso (
    id_usuario  integer NOT NULL,
    id_tipo     integer NOT NULL,
    CONSTRAINT pk_permiso PRIMARY KEY (id_usuario, id_tipo),
    CONSTRAINT fk_permiso_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_permiso_tipo FOREIGN KEY (id_tipo)
        REFERENCES tipo_permiso(id_tipo) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Clientes
-- (version_cliente reemplaza al ROWVERSION de SQL Server)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS clientes (
    id_cliente        serial PRIMARY KEY,
    nombre            varchar(150) NOT NULL,
    documento         varchar(20),
    telefono          varchar(20),
    direccion         varchar(255),
    version_cliente   bigint NOT NULL DEFAULT 1
);

-- ------------------------------------------------------------
-- Catalogo de productos (familia / variante / caracteristicas)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS productos (
    id_producto  serial PRIMARY KEY,
    categoria    varchar(20) NOT NULL,
    tipo         varchar(50) NOT NULL,
    color        varchar(30),
    nombre       varchar(150) NOT NULL,
    descripcion  varchar(255),
    imagen_url   varchar(255),
    activo       boolean NOT NULL DEFAULT true
);

CREATE TABLE IF NOT EXISTS caracteristicas (
    id_caracteristica  serial PRIMARY KEY,
    espesor_cm         numeric(6,2),
    medida_cm          numeric(6,2)
);

CREATE TABLE IF NOT EXISTS producto_variante (
    id_variante        serial PRIMARY KEY,
    id_producto        integer NOT NULL,
    id_caracteristica  integer NOT NULL,
    unidad_costo       varchar(10) NOT NULL,
    precio_unitario    numeric(10,3) NOT NULL,
    stock_minimo       integer NOT NULL CONSTRAINT df_variante_stock_minimo DEFAULT 0,
    CONSTRAINT uq_variante UNIQUE (id_producto, id_caracteristica),
    CONSTRAINT ck_variante_unidad_costo CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA','PIE2','UNIDAD')),
    CONSTRAINT fk_var_producto FOREIGN KEY (id_producto)
        REFERENCES productos(id_producto) ON DELETE NO ACTION,
    CONSTRAINT fk_var_caract FOREIGN KEY (id_caracteristica)
        REFERENCES caracteristicas(id_caracteristica) ON DELETE NO ACTION
);

-- ------------------------------------------------------------
-- Pedidos (se crean antes de compras porque compras los referencia)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pedidos (
    id_pedido   serial PRIMARY KEY,
    id_cliente  integer NOT NULL,
    id_usuario  integer NOT NULL,
    fecha       timestamp NOT NULL DEFAULT fn_hora_peru(),
    estado      varchar(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (estado IN ('PENDIENTE','PAGADO','PARCIAL','CANCELADO')),
    total       numeric(10,2) NOT NULL DEFAULT 0,
    fecha_termino timestamp,
    CONSTRAINT fk_pedido_cliente FOREIGN KEY (id_cliente)
        REFERENCES clientes(id_cliente) ON DELETE NO ACTION,
    CONSTRAINT fk_pedido_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE NO ACTION
);

-- ------------------------------------------------------------
-- Compras (entrada de material)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS compras (
    id_compra        serial PRIMARY KEY,
    proveedor        varchar(150) NOT NULL,
    fecha            date NOT NULL,
    total            numeric(12,3) NOT NULL DEFAULT 0,
    motivo           varchar(20) NOT NULL DEFAULT 'REPOSICION'
        CHECK (motivo IN ('REPOSICION','REEMPLAZO_DEFECTO')),
    id_pedido_origen integer,
    CONSTRAINT fk_compra_pedido_origen FOREIGN KEY (id_pedido_origen)
        REFERENCES pedidos(id_pedido) ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS compra_detalle (
    id_compra_detalle  serial PRIMARY KEY,
    id_compra          integer NOT NULL,
    id_variante        integer NOT NULL,
    cantidad           numeric(12,3) NOT NULL,
    costo_unitario     numeric(12,3) NOT NULL,
    subtotal           numeric(12,3) NOT NULL,
    piezas             integer NOT NULL CONSTRAINT df_cdet_piezas DEFAULT 1,
    ancho_cm           numeric(9,3),
    alto_cm            numeric(9,3),
    largo_cm           numeric(9,3),
    CONSTRAINT fk_cdet_compra FOREIGN KEY (id_compra)
        REFERENCES compras(id_compra) ON DELETE CASCADE,
    CONSTRAINT fk_cdet_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION
);

-- ------------------------------------------------------------
-- Inventario: piezas (vidrio/aluminio) y cajas (tornillo/tarugo)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS inventario_piezas (
    id_pieza           serial PRIMARY KEY,
    id_variante        integer NOT NULL,
    id_pieza_origen    integer,
    id_compra_detalle  integer,
    ancho_cm           numeric(9,3),
    alto_cm            numeric(9,3),
    largo_cm           numeric(9,3),
    estado             varchar(15) NOT NULL DEFAULT 'DISPONIBLE'
        CONSTRAINT ck_pieza_estado CHECK (estado IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA')),
    origen             varchar(10) NOT NULL
        CONSTRAINT ck_pieza_origen CHECK (origen IN ('COMPRA','RETAZO','INICIAL')),
    fecha_ingreso      timestamp NOT NULL DEFAULT fn_hora_peru(),
    CONSTRAINT ck_pieza_medidas CHECK (
        (ancho_cm > 0 AND alto_cm > 0 AND largo_cm IS NULL)
     OR (largo_cm > 0 AND ancho_cm IS NULL AND alto_cm IS NULL)
    ),
    CONSTRAINT fk_pieza_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_origen FOREIGN KEY (id_pieza_origen)
        REFERENCES inventario_piezas(id_pieza) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES compra_detalle(id_compra_detalle) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS inventario_cajas (
    id_variante   integer PRIMARY KEY,
    stock_cajas   integer NOT NULL DEFAULT 0,
    CONSTRAINT ck_cajas_stock_no_negativo CHECK (stock_cajas >= 0),
    CONSTRAINT fk_cajas_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Items del pedido
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pedido_item (
    id_item      serial PRIMARY KEY,
    id_pedido    integer NOT NULL,
    tipo         varchar(15) NOT NULL
        CONSTRAINT ck_item_tipo CHECK (tipo IN ('CAJA','VIDRIO_SUELTO','VENTANA','MAMPARA','ALUMINIO')),
    descripcion  varchar(255),
    cantidad     integer NOT NULL DEFAULT 1
        CONSTRAINT ck_item_cantidad CHECK (cantidad > 0),
    estado       varchar(15) NOT NULL DEFAULT 'PENDIENTE'
        CONSTRAINT ck_item_estado CHECK (estado IN ('PENDIENTE','EN_PROCESO','COMPLETADO','CANCELADO')),
    subtotal     numeric(10,2) NOT NULL DEFAULT 0,
    precio_materiales numeric(10,2),
    costo_extra       numeric(10,2) NOT NULL DEFAULT 0,
    mano_obra         numeric(10,2) NOT NULL DEFAULT 0,
    total             numeric(10,2),
    id_modelo         integer,
    CONSTRAINT fk_item_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedidos(id_pedido) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Detalle de pedidos
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS pedido_detalle (
    id_detalle                serial PRIMARY KEY,
    id_item                   integer NOT NULL,
    id_variante               integer NOT NULL,
    id_pieza                  integer,
    id_compra_detalle         integer,
    accion                    varchar(15) NOT NULL
        CONSTRAINT ck_det_accion CHECK (accion IN ('USA_EXISTENTE','CORTA_PIEZA','COMPRA_NUEVA')),
    estado                    varchar(15) NOT NULL DEFAULT 'ACTIVO'
        CONSTRAINT ck_det_estado CHECK (estado IN ('ACTIVO','REEMPLAZADO','CANCELADO')),
    id_detalle_origen         integer,
    cantidad_cajas            integer,
    ancho_cm                  numeric(8,2),
    alto_cm                   numeric(8,2),
    largo_cm                  numeric(8,2),
    precio_unitario_aplicado  numeric(10,2) NOT NULL,
    subtotal                  numeric(10,2) NOT NULL,
    CONSTRAINT fk_det_item FOREIGN KEY (id_item)
        REFERENCES pedido_item(id_item) ON DELETE CASCADE,
    CONSTRAINT fk_det_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_det_pieza FOREIGN KEY (id_pieza)
        REFERENCES inventario_piezas(id_pieza) ON DELETE SET NULL,
    CONSTRAINT fk_det_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES compra_detalle(id_compra_detalle) ON DELETE SET NULL,
    CONSTRAINT fk_det_origen FOREIGN KEY (id_detalle_origen)
        REFERENCES pedido_detalle(id_detalle) ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS pedido_detalle_medida (
    id_medida   serial PRIMARY KEY,
    id_detalle  integer NOT NULL,
    etiqueta    varchar(30) NOT NULL,
    valor_cm    numeric(8,2) NOT NULL,
    CONSTRAINT fk_medida_detalle FOREIGN KEY (id_detalle)
        REFERENCES pedido_detalle(id_detalle) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Tarifas sugeridas de costo extra de una ventana
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tarifas_ventana (
    id_tarifa         serial PRIMARY KEY,
    perimetro_min_cm  numeric(10,2) NOT NULL,
    perimetro_max_cm  numeric(10,2) NOT NULL,
    costo_aluminio    numeric(10,2) NOT NULL,
    costo_silicona    numeric(10,2) NOT NULL,
    CONSTRAINT ck_tarifa_rango CHECK (perimetro_max_cm > perimetro_min_cm)
);

INSERT INTO tarifas_ventana (perimetro_min_cm, perimetro_max_cm, costo_aluminio, costo_silicona)
SELECT v.perimetro_min_cm, v.perimetro_max_cm, v.costo_aluminio, v.costo_silicona
FROM (VALUES (0::numeric, 300::numeric, 15::numeric, 5::numeric),
             (300.01, 480, 30, 10),
             (480.01, 700, 45, 15),
             (700.01, 99999, 60, 20)) AS v(perimetro_min_cm, perimetro_max_cm, costo_aluminio, costo_silicona)
WHERE NOT EXISTS (SELECT 1 FROM tarifas_ventana);

-- ------------------------------------------------------------
-- Modelos de ventana (data-driven)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS modelos_ventana (
    id_modelo   serial PRIMARY KEY,
    nombre      varchar(100) NOT NULL,
    descripcion varchar(255),
    orden       integer NOT NULL DEFAULT 0,
    activo      boolean NOT NULL DEFAULT true,
    CONSTRAINT uq_mv_nombre UNIQUE (nombre)
);

CREATE TABLE IF NOT EXISTS modelo_ventana_variable (
    id_variable serial PRIMARY KEY,
    id_modelo   integer NOT NULL,
    orden       integer NOT NULL DEFAULT 0,
    nombre      varchar(40) NOT NULL,
    formula     varchar(300) NOT NULL,
    CONSTRAINT fk_mv_var_modelo FOREIGN KEY (id_modelo)
        REFERENCES modelos_ventana(id_modelo) ON DELETE CASCADE,
    CONSTRAINT uq_mv_var UNIQUE (id_modelo, nombre)
);

CREATE TABLE IF NOT EXISTS modelo_ventana_componente (
    id_componente serial PRIMARY KEY,
    id_modelo     integer NOT NULL,
    orden         integer NOT NULL DEFAULT 0,
    categoria     varchar(20) NOT NULL,
    tipo          varchar(50),
    rol_color     varchar(20) NOT NULL DEFAULT 'MATERIAL',
    tipo_medida   varchar(10) NOT NULL,
    expr_largo    varchar(300),
    expr_cantidad varchar(300),
    expr_ancho    varchar(300),
    expr_alto     varchar(300),
    condicion     varchar(300),
    CONSTRAINT fk_mv_comp_modelo FOREIGN KEY (id_modelo)
        REFERENCES modelos_ventana(id_modelo) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS modelo_ventana_validacion (
    id_validacion serial PRIMARY KEY,
    id_modelo     integer NOT NULL,
    condicion     varchar(300) NOT NULL,
    mensaje       varchar(255) NOT NULL,
    CONSTRAINT fk_mv_val_modelo FOREIGN KEY (id_modelo)
        REFERENCES modelos_ventana(id_modelo) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Kardex
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS movimientos_inventario (
    id_movimiento      serial PRIMARY KEY,
    fecha              timestamp NOT NULL DEFAULT fn_hora_peru(),
    id_variante        integer NOT NULL,
    id_pieza           integer,
    tipo               varchar(10) NOT NULL
        CHECK (tipo IN ('ENTRADA','SALIDA','AJUSTE','ESTADO')),
    motivo             varchar(15) NOT NULL
        CHECK (motivo IN ('COMPRA','INICIAL','RETAZO','CORTE','VENTA','DEFECTO','RESERVA','LIBERACION','AJUSTE')),
    cantidad           numeric(10,2) NOT NULL,
    stock_resultante   integer,
    id_usuario         integer,
    id_pedido          integer,
    id_compra_detalle  integer,
    observacion        varchar(255),
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

-- ------------------------------------------------------------
-- Pagos, metodos de pago y comprobantes
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS metodos_pago (
    id_metodo_pago  serial PRIMARY KEY,
    nombre          varchar(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS pagos (
    id_pago       serial PRIMARY KEY,
    id_pedido     integer NOT NULL,
    fecha         timestamp NOT NULL DEFAULT fn_hora_peru(),
    monto_total   numeric(10,2) NOT NULL,
    CONSTRAINT fk_pago_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedidos(id_pedido) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS detalle_pago (
    id_detalle_pago  serial PRIMARY KEY,
    id_pago          integer NOT NULL,
    id_metodo_pago   integer NOT NULL,
    monto            numeric(10,2) NOT NULL,
    CONSTRAINT fk_dp_pago FOREIGN KEY (id_pago)
        REFERENCES pagos(id_pago) ON DELETE CASCADE,
    CONSTRAINT fk_dp_metodo FOREIGN KEY (id_metodo_pago)
        REFERENCES metodos_pago(id_metodo_pago) ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS comprobante_pago (
    id_comprobante    serial PRIMARY KEY,
    id_pago           integer NOT NULL UNIQUE,
    tipo_comprobante  varchar(10) NOT NULL
        CHECK (tipo_comprobante IN ('BOLETA','FACTURA')),
    serie             varchar(10) NOT NULL,
    numero            varchar(20) NOT NULL,
    fecha_emision     timestamp NOT NULL DEFAULT fn_hora_peru(),
    CONSTRAINT fk_comp_pago FOREIGN KEY (id_pago)
        REFERENCES pagos(id_pago) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Proveedores (modulo de proveedores)
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS proveedores (
    id_proveedor       serial PRIMARY KEY,
    nombre             varchar(150) NOT NULL,
    documento          varchar(20),
    telefono           varchar(20),
    direccion          varchar(255),
    version_proveedor  bigint NOT NULL DEFAULT 1,
    tipo_persona       varchar(10),
    tipo_documento     varchar(15)
);

-- ------------------------------------------------------------
-- Migraciones para bases ya creadas
-- ------------------------------------------------------------
ALTER TABLE pedidos            ADD COLUMN IF NOT EXISTS fecha_termino timestamp;
ALTER TABLE pedido_item        ADD COLUMN IF NOT EXISTS precio_materiales numeric(10,2);
ALTER TABLE pedido_item        ADD COLUMN IF NOT EXISTS costo_extra numeric(10,2) NOT NULL DEFAULT 0;
ALTER TABLE pedido_item        ADD COLUMN IF NOT EXISTS mano_obra numeric(10,2) NOT NULL DEFAULT 0;
ALTER TABLE pedido_item        ADD COLUMN IF NOT EXISTS total numeric(10,2);
ALTER TABLE pedido_item        ADD COLUMN IF NOT EXISTS id_modelo integer;
ALTER TABLE pedido_detalle     ADD COLUMN IF NOT EXISTS id_item integer;
ALTER TABLE pedido_detalle     ADD COLUMN IF NOT EXISTS id_compra_detalle integer;
ALTER TABLE pedido_detalle     ADD COLUMN IF NOT EXISTS accion varchar(15) NOT NULL DEFAULT 'COMPRA_NUEVA';
ALTER TABLE pedido_detalle     ADD COLUMN IF NOT EXISTS estado varchar(15) NOT NULL DEFAULT 'ACTIVO';
ALTER TABLE pedido_detalle     ADD COLUMN IF NOT EXISTS id_detalle_origen integer;
ALTER TABLE compra_detalle     ADD COLUMN IF NOT EXISTS piezas integer NOT NULL DEFAULT 1;
ALTER TABLE compra_detalle     ADD COLUMN IF NOT EXISTS ancho_cm numeric(9,3);
ALTER TABLE compra_detalle     ADD COLUMN IF NOT EXISTS alto_cm numeric(9,3);
ALTER TABLE compra_detalle     ADD COLUMN IF NOT EXISTS largo_cm numeric(9,3);
ALTER TABLE clientes           ADD COLUMN IF NOT EXISTS version_cliente bigint NOT NULL DEFAULT 1;
ALTER TABLE clientes           ADD COLUMN IF NOT EXISTS tipo_persona varchar(10);
ALTER TABLE clientes           ADD COLUMN IF NOT EXISTS tipo_documento varchar(15);
ALTER TABLE proveedores        ADD COLUMN IF NOT EXISTS version_proveedor bigint NOT NULL DEFAULT 1;
ALTER TABLE proveedores        ADD COLUMN IF NOT EXISTS tipo_persona varchar(10);
ALTER TABLE proveedores        ADD COLUMN IF NOT EXISTS tipo_documento varchar(15);
ALTER TABLE compras            ADD COLUMN IF NOT EXISTS id_proveedor integer;

-- Precision de 3 decimales (costo de compra y medidas de pieza)
-- Las vistas se recrean mas abajo; se eliminan para permitir el ALTER TYPE.
DROP VIEW IF EXISTS vw_inv_movimientos;
DROP VIEW IF EXISTS vw_inv_resumen;
DROP VIEW IF EXISTS vw_inv_piezas;
DROP VIEW IF EXISTS vw_inv_variantes;

ALTER TABLE producto_variante  ALTER COLUMN precio_unitario TYPE numeric(10,3);
ALTER TABLE compras            ALTER COLUMN total TYPE numeric(12,3);
ALTER TABLE compra_detalle     ALTER COLUMN cantidad TYPE numeric(12,3);
ALTER TABLE compra_detalle     ALTER COLUMN costo_unitario TYPE numeric(12,3);
ALTER TABLE compra_detalle     ALTER COLUMN subtotal TYPE numeric(12,3);
ALTER TABLE inventario_piezas  ALTER COLUMN ancho_cm TYPE numeric(9,3);
ALTER TABLE inventario_piezas  ALTER COLUMN alto_cm TYPE numeric(9,3);
ALTER TABLE inventario_piezas  ALTER COLUMN largo_cm TYPE numeric(9,3);

-- CHECK de unidad_costo (acepta PIE2 y UNIDAD)
DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM pg_constraint
               WHERE conname = 'ck_variante_unidad_costo'
                 AND conrelid = 'producto_variante'::regclass) THEN
        ALTER TABLE producto_variante DROP CONSTRAINT ck_variante_unidad_costo;
    END IF;
    ALTER TABLE producto_variante ADD CONSTRAINT ck_variante_unidad_costo
        CHECK (unidad_costo IN ('CM2','CM_LINEAL','CAJA','PIE2','UNIDAD'));
END $$;

-- CHECK de tema (acepta los temas nuevos)
DO $$
DECLARE
    r record;
BEGIN
    FOR r IN SELECT conname FROM pg_constraint
             WHERE conrelid = 'usuario_preferencias'::regclass
               AND contype = 'c'
               AND pg_get_constraintdef(oid) LIKE '%tema%'
    LOOP
        EXECUTE format('ALTER TABLE usuario_preferencias DROP CONSTRAINT %I', r.conname);
    END LOOP;
    ALTER TABLE usuario_preferencias ADD CONSTRAINT ck_tema_v2
        CHECK (tema IN ('CLARO', 'OSCURO', 'AZULCLARO', 'AZULOSCURO'));
END $$;

-- FKs y CHECK del detalle de pedidos para bases antiguas
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_det_item'
                   AND conrelid = 'pedido_detalle'::regclass) THEN
        ALTER TABLE pedido_detalle ADD CONSTRAINT fk_det_item
            FOREIGN KEY (id_item) REFERENCES pedido_item(id_item) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_det_compra_detalle'
                   AND conrelid = 'pedido_detalle'::regclass) THEN
        ALTER TABLE pedido_detalle ADD CONSTRAINT fk_det_compra_detalle
            FOREIGN KEY (id_compra_detalle) REFERENCES compra_detalle(id_compra_detalle) ON DELETE SET NULL;
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'fk_det_origen'
                   AND conrelid = 'pedido_detalle'::regclass) THEN
        ALTER TABLE pedido_detalle ADD CONSTRAINT fk_det_origen
            FOREIGN KEY (id_detalle_origen) REFERENCES pedido_detalle(id_detalle) ON DELETE NO ACTION;
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_det_accion'
                   AND conrelid = 'pedido_detalle'::regclass) THEN
        ALTER TABLE pedido_detalle ADD CONSTRAINT ck_det_accion
            CHECK (accion IN ('USA_EXISTENTE','CORTA_PIEZA','COMPRA_NUEVA'));
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_det_estado'
                   AND conrelid = 'pedido_detalle'::regclass) THEN
        ALTER TABLE pedido_detalle ADD CONSTRAINT ck_det_estado
            CHECK (estado IN ('ACTIVO','REEMPLAZADO','CANCELADO'));
    END IF;
END $$;

-- Identificacion de clientes y proveedores
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_clientes_identificacion'
                   AND conrelid = 'clientes'::regclass) THEN
        ALTER TABLE clientes ADD CONSTRAINT ck_clientes_identificacion CHECK (
            (tipo_persona IS NULL AND tipo_documento IS NULL)
            OR (tipo_persona IS NOT NULL AND tipo_documento IS NOT NULL AND (
                (tipo_persona = 'NATURAL' AND tipo_documento IN ('DNI','RUC','SIN DOCUMENTO'))
                OR (tipo_persona = 'JURIDICA' AND tipo_documento = 'RUC'))));
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_clientes_documento'
                   AND conrelid = 'clientes'::regclass) THEN
        ALTER TABLE clientes ADD CONSTRAINT ck_clientes_documento CHECK (
            tipo_documento IS NULL
            OR (tipo_documento = 'SIN DOCUMENTO' AND documento IS NULL)
            OR (tipo_documento IN ('DNI','RUC') AND documento IS NOT NULL
                AND documento ~ '^[0-9]+$'
                AND length(documento) = CASE WHEN tipo_documento = 'DNI' THEN 8 ELSE 11 END
                AND (tipo_documento <> 'RUC'
                    OR (tipo_persona = 'JURIDICA' AND left(documento,2) = '20')
                    OR (tipo_persona = 'NATURAL' AND left(documento,2) IN ('10','15')))));
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_proveedores_identificacion'
                   AND conrelid = 'proveedores'::regclass) THEN
        ALTER TABLE proveedores ADD CONSTRAINT ck_proveedores_identificacion CHECK (
            (tipo_persona IS NULL AND tipo_documento IS NULL)
            OR (tipo_persona IS NOT NULL AND tipo_documento IS NOT NULL AND (
                (tipo_persona = 'NATURAL' AND tipo_documento IN ('DNI','RUC','SIN DOCUMENTO'))
                OR (tipo_persona = 'JURIDICA' AND tipo_documento = 'RUC'))));
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'ck_proveedores_documento'
                   AND conrelid = 'proveedores'::regclass) THEN
        ALTER TABLE proveedores ADD CONSTRAINT ck_proveedores_documento CHECK (
            tipo_documento IS NULL
            OR (tipo_documento = 'SIN DOCUMENTO' AND documento IS NULL)
            OR (tipo_documento IN ('DNI','RUC') AND documento IS NOT NULL
                AND documento ~ '^[0-9]+$'
                AND length(documento) = CASE WHEN tipo_documento = 'DNI' THEN 8 ELSE 11 END
                AND (tipo_documento <> 'RUC'
                    OR (tipo_persona = 'JURIDICA' AND left(documento,2) = '20')
                    OR (tipo_persona = 'NATURAL' AND left(documento,2) IN ('10','15')))));
    END IF;
END $$;

CREATE UNIQUE INDEX IF NOT EXISTS ux_clientes_documento ON clientes(tipo_documento, documento)
    WHERE tipo_documento IS NOT NULL AND documento IS NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS ux_proveedores_documento ON proveedores(tipo_documento, documento)
    WHERE tipo_documento IS NOT NULL AND documento IS NOT NULL;

-- ------------------------------------------------------------
-- Indices de apoyo para los reportes mas frecuentes
-- ------------------------------------------------------------
CREATE INDEX IF NOT EXISTS idx_pedidos_fecha           ON pedidos(fecha);
CREATE INDEX IF NOT EXISTS idx_pedidos_cliente         ON pedidos(id_cliente);
CREATE INDEX IF NOT EXISTS idx_compras_fecha           ON compras(fecha);
CREATE INDEX IF NOT EXISTS idx_compras_motivo          ON compras(motivo);
CREATE INDEX IF NOT EXISTS idx_compras_proveedor       ON compras(id_proveedor);
CREATE INDEX IF NOT EXISTS idx_piezas_variante_estado  ON inventario_piezas(id_variante, estado);
CREATE INDEX IF NOT EXISTS idx_piezas_medidas          ON inventario_piezas(ancho_cm, alto_cm);
CREATE INDEX IF NOT EXISTS idx_mov_fecha               ON movimientos_inventario(fecha);
CREATE INDEX IF NOT EXISTS idx_mov_variante            ON movimientos_inventario(id_variante, fecha);

-- Reclasificacion de los productos 2 y 3 como accesorios
UPDATE productos SET categoria = 'ACCESORIO', tipo = 'TARUGO',   color = 'PLASTICO'      WHERE id_producto = 2;
UPDATE productos SET categoria = 'ACCESORIO', tipo = 'TORNILLO', color = 'AUTORROSCANTE' WHERE id_producto = 3;

-- ============================================================
-- 2) VISTAS
-- ============================================================

CREATE OR REPLACE VIEW vw_inv_variantes AS
SELECT pv.id_variante,
       p.id_producto,
       p.categoria,
       p.tipo,
       p.color,
       c.espesor_cm,
       c.medida_cm,
       p.nombre
         || COALESCE(' ' || NULLIF(p.color, ''), '')
         || COALESCE(' - espesor ' || trim(to_char(c.espesor_cm, 'FM9990.00')) || ' cm', '')
         || COALESCE(' - medida ' || trim(to_char(c.medida_cm, 'FM9990.00')) || ' cm', '') AS producto,
       pv.unidad_costo,
       pv.precio_unitario,
       pv.stock_minimo
FROM producto_variante pv
INNER JOIN productos p       ON p.id_producto = pv.id_producto
INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
WHERE p.activo;

CREATE OR REPLACE VIEW vw_inv_piezas AS
SELECT ip.id_pieza,
       ip.id_variante,
       v.categoria,
       v.producto,
       ip.ancho_cm,
       ip.alto_cm,
       ip.largo_cm,
       (ip.ancho_cm * ip.alto_cm)::numeric(12,2) AS area_cm2,
       ip.estado,
       ip.origen,
       ip.id_pieza_origen,
       ip.id_compra_detalle,
       ip.fecha_ingreso
FROM inventario_piezas ip
INNER JOIN vw_inv_variantes v ON v.id_variante = ip.id_variante;

CREATE OR REPLACE VIEW vw_inv_resumen AS
SELECT v.id_variante,
       v.categoria,
       v.producto,
       v.unidad_costo,
       v.precio_unitario,
       COALESCE(pz.piezas_disponibles, 0) AS piezas_disponibles,
       COALESCE(pz.piezas_reservadas, 0)  AS piezas_reservadas,
       COALESCE(pz.area_disponible_cm2, 0) AS area_disponible_cm2,
       COALESCE(pz.largo_disponible_cm, 0) AS largo_disponible_cm,
       COALESCE(ic.stock_cajas, 0)         AS stock_cajas,
       CASE WHEN v.unidad_costo IN ('CAJA','UNIDAD') THEN COALESCE(ic.stock_cajas, 0)
            ELSE COALESCE(pz.piezas_disponibles, 0) END AS existencias,
       v.stock_minimo,
       (CASE WHEN (CASE WHEN v.unidad_costo IN ('CAJA','UNIDAD') THEN COALESCE(ic.stock_cajas, 0)
                              ELSE COALESCE(pz.piezas_disponibles, 0) END) <= v.stock_minimo
                  AND v.stock_minimo > 0
             THEN true ELSE false END) AS bajo_minimo
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

CREATE OR REPLACE VIEW vw_inv_movimientos AS
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

-- ============================================================
-- 3) FUNCIONES (equivalen a los procedimientos almacenados)
-- No abren ni cierran transacciones: la controla quien las llama.
-- Los errores de negocio se lanzan con RAISE EXCEPTION.
-- ============================================================

-- ------------------------------------------------------------
-- Ingresar una pieza de vidrio o aluminio. Devuelve id_pieza.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_inv_ingresar_pieza(
    p_id_variante       integer,
    p_origen            varchar,
    p_ancho_cm          numeric DEFAULT NULL,
    p_alto_cm           numeric DEFAULT NULL,
    p_largo_cm          numeric DEFAULT NULL,
    p_id_compra_detalle integer DEFAULT NULL,
    p_id_pieza_origen   integer DEFAULT NULL,
    p_id_usuario        integer DEFAULT NULL,
    p_observacion       varchar DEFAULT NULL,
    p_fecha             timestamp DEFAULT NULL
) RETURNS integer AS $$
DECLARE
    v_unidad   varchar(10);
    v_id_pieza integer;
BEGIN
    SELECT unidad_costo INTO v_unidad FROM producto_variante WHERE id_variante = p_id_variante;

    IF v_unidad IS NULL THEN
        RAISE EXCEPTION 'La variante de producto no existe.';
    END IF;
    IF v_unidad IN ('CAJA','UNIDAD') THEN
        RAISE EXCEPTION 'Tornillos, tarugos y accesorios por unidad se manejan por cantidad, no por piezas.';
    END IF;
    IF v_unidad IN ('CM2','PIE2') AND (COALESCE(p_ancho_cm, 0) <= 0 OR COALESCE(p_alto_cm, 0) <= 0) THEN
        RAISE EXCEPTION 'Para vidrio ingrese ancho y alto mayores a cero.';
    END IF;
    IF v_unidad = 'CM_LINEAL' AND COALESCE(p_largo_cm, 0) <= 0 THEN
        RAISE EXCEPTION 'Para aluminio ingrese el largo mayor a cero.';
    END IF;
    IF p_origen = 'COMPRA' AND p_id_compra_detalle IS NULL THEN
        RAISE EXCEPTION 'Una pieza de origen COMPRA debe indicar el detalle de compra.';
    END IF;
    IF p_origen = 'RETAZO' AND p_id_pieza_origen IS NULL THEN
        RAISE EXCEPTION 'Un retazo debe indicar la pieza de la que salio.';
    END IF;

    IF v_unidad IN ('CM2','PIE2') THEN
        p_largo_cm := NULL;
    END IF;
    IF v_unidad = 'CM_LINEAL' THEN
        p_ancho_cm := NULL;
        p_alto_cm  := NULL;
    END IF;

    INSERT INTO inventario_piezas
        (id_variante, id_pieza_origen, id_compra_detalle, ancho_cm, alto_cm, largo_cm,
         estado, origen, fecha_ingreso)
    VALUES
        (p_id_variante, p_id_pieza_origen, p_id_compra_detalle, p_ancho_cm, p_alto_cm, p_largo_cm,
         'DISPONIBLE', p_origen, COALESCE(p_fecha, fn_hora_peru()))
    RETURNING id_pieza INTO v_id_pieza;

    INSERT INTO movimientos_inventario
        (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_compra_detalle,
         observacion, fecha)
    VALUES
        (p_id_variante, v_id_pieza, 'ENTRADA', p_origen, 1, p_id_usuario, p_id_compra_detalle,
         p_observacion, COALESCE(p_fecha, fn_hora_peru()));

    RETURN v_id_pieza;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Cambiar el estado de una pieza
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_inv_cambiar_estado_pieza(
    p_id_pieza    integer,
    p_estado      varchar,
    p_id_usuario  integer DEFAULT NULL,
    p_id_pedido   integer DEFAULT NULL,
    p_observacion varchar DEFAULT NULL
) RETURNS void AS $$
DECLARE
    v_estado_actual varchar(15);
    v_id_variante   integer;
    v_tipo          varchar(10);
    v_motivo        varchar(15);
BEGIN
    IF p_estado NOT IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA') THEN
        RAISE EXCEPTION 'Estado de pieza no valido.';
    END IF;

    SELECT estado, id_variante INTO v_estado_actual, v_id_variante
    FROM inventario_piezas
    WHERE id_pieza = p_id_pieza
    FOR UPDATE;

    IF v_estado_actual IS NULL THEN
        RAISE EXCEPTION 'La pieza no existe.';
    END IF;
    IF v_estado_actual = p_estado THEN
        RAISE EXCEPTION 'La pieza ya tiene ese estado.';
    END IF;
    IF v_estado_actual IN ('VENDIDA','CONSUMIDA','DEFECTUOSA') AND p_estado <> 'DEFECTUOSA' THEN
        RAISE EXCEPTION 'La pieza ya salio del inventario (vendida, consumida o defectuosa) y no puede cambiar de estado.';
    END IF;

    UPDATE inventario_piezas SET estado = p_estado WHERE id_pieza = p_id_pieza;

    v_tipo := CASE WHEN p_estado IN ('RESERVADA','DISPONIBLE') THEN 'ESTADO' ELSE 'SALIDA' END;
    v_motivo := CASE p_estado WHEN 'RESERVADA'  THEN 'RESERVA'
                              WHEN 'DISPONIBLE' THEN 'LIBERACION'
                              WHEN 'VENDIDA'    THEN 'VENTA'
                              WHEN 'CONSUMIDA'  THEN 'CORTE'
                              WHEN 'DEFECTUOSA' THEN 'DEFECTO' END;

    INSERT INTO movimientos_inventario
        (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
    VALUES
        (v_id_variante, p_id_pieza, v_tipo, v_motivo, 1, p_id_usuario, p_id_pedido,
         COALESCE(p_observacion, v_estado_actual || ' -> ' || p_estado));
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Cortar una pieza y generar sus retazos utiles.
-- Devuelve las filas de retazos generados (puede ser 0, 1 o 2).
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_inv_cortar_pieza(
    p_id_pieza       integer,
    p_ancho_corte    numeric DEFAULT NULL,
    p_alto_corte     numeric DEFAULT NULL,
    p_largo_corte    numeric DEFAULT NULL,
    p_minimo_util_cm numeric DEFAULT 10,
    p_id_usuario     integer DEFAULT NULL,
    p_id_pedido      integer DEFAULT NULL,
    p_observacion    varchar DEFAULT NULL
) RETURNS TABLE(id_pieza integer, ancho_cm numeric, alto_cm numeric, largo_cm numeric) AS $$
DECLARE
    v_estado      varchar(15);
    v_id_variante integer;
    v_w           numeric(9,3);
    v_h           numeric(9,3);
    v_l           numeric(9,3);
    v_tmp         numeric(9,3);
    v_nuevo       integer;
    r             record;
BEGIN
    DROP TABLE IF EXISTS _retazos_corte;
    CREATE TEMP TABLE _retazos_corte (
        n          serial,
        id_pieza_r integer,
        ancho_r    numeric(9,3),
        alto_r     numeric(9,3),
        largo_r    numeric(9,3)
    ) ON COMMIT DROP;

    SELECT ip.estado, ip.id_variante, ip.ancho_cm, ip.alto_cm, ip.largo_cm
      INTO v_estado, v_id_variante, v_w, v_h, v_l
    FROM inventario_piezas ip
    WHERE ip.id_pieza = p_id_pieza
    FOR UPDATE;

    IF v_estado IS NULL THEN
        RAISE EXCEPTION 'La pieza no existe.';
    END IF;
    IF v_estado NOT IN ('DISPONIBLE','RESERVADA') THEN
        RAISE EXCEPTION 'Solo se pueden cortar piezas DISPONIBLES o RESERVADAS.';
    END IF;

    IF v_l IS NULL THEN  -- vidrio
        IF COALESCE(p_ancho_corte, 0) <= 0 OR COALESCE(p_alto_corte, 0) <= 0 THEN
            RAISE EXCEPTION 'Ingrese ancho y alto del corte.';
        END IF;

        IF NOT (p_ancho_corte <= v_w AND p_alto_corte <= v_h) THEN
            IF p_alto_corte <= v_w AND p_ancho_corte <= v_h THEN
                v_tmp := p_ancho_corte;
                p_ancho_corte := p_alto_corte;
                p_alto_corte := v_tmp;
            ELSE
                RAISE EXCEPTION 'El corte es mas grande que la pieza.';
            END IF;
        END IF;

        IF v_w - p_ancho_corte >= p_minimo_util_cm AND v_h >= p_minimo_util_cm THEN
            INSERT INTO _retazos_corte (ancho_r, alto_r) VALUES (v_w - p_ancho_corte, v_h);
        END IF;
        IF v_h - p_alto_corte >= p_minimo_util_cm AND p_ancho_corte >= p_minimo_util_cm THEN
            INSERT INTO _retazos_corte (ancho_r, alto_r) VALUES (p_ancho_corte, v_h - p_alto_corte);
        END IF;
    ELSE                 -- aluminio
        IF COALESCE(p_largo_corte, 0) <= 0 THEN
            RAISE EXCEPTION 'Ingrese el largo del corte.';
        END IF;
        IF p_largo_corte > v_l THEN
            RAISE EXCEPTION 'El corte es mas largo que la pieza.';
        END IF;
        IF v_l - p_largo_corte >= p_minimo_util_cm THEN
            INSERT INTO _retazos_corte (largo_r) VALUES (v_l - p_largo_corte);
        END IF;
    END IF;

    UPDATE inventario_piezas ip SET estado = 'CONSUMIDA' WHERE ip.id_pieza = p_id_pieza;

    INSERT INTO movimientos_inventario
        (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
    VALUES
        (v_id_variante, p_id_pieza, 'SALIDA', 'CORTE', 1, p_id_usuario, p_id_pedido,
         COALESCE(p_observacion,
             'Corte de ' || CASE WHEN v_l IS NULL
                 THEN p_ancho_corte::text || ' x ' || p_alto_corte::text || ' cm'
                 ELSE p_largo_corte::text || ' cm' END));

    FOR r IN SELECT * FROM _retazos_corte ORDER BY n LOOP
        INSERT INTO inventario_piezas AS ip
            (id_variante, id_pieza_origen, ancho_cm, alto_cm, largo_cm, estado, origen)
        VALUES
            (v_id_variante, p_id_pieza, r.ancho_r, r.alto_r, r.largo_r, 'DISPONIBLE', 'RETAZO')
        RETURNING ip.id_pieza INTO v_nuevo;

        INSERT INTO movimientos_inventario
            (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
        VALUES
            (v_id_variante, v_nuevo, 'ENTRADA', 'RETAZO', 1, p_id_usuario, p_id_pedido,
             'Retazo de la pieza #' || p_id_pieza::text);

        UPDATE _retazos_corte SET id_pieza_r = v_nuevo WHERE n = r.n;
    END LOOP;

    RETURN QUERY SELECT t.id_pieza_r, t.ancho_r, t.alto_r, t.largo_r
                 FROM _retazos_corte t ORDER BY t.n;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Mover cajas o unidades. Devuelve el stock resultante.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_inv_mover_cajas(
    p_id_variante       integer,
    p_tipo              varchar,
    p_cantidad          integer,
    p_motivo            varchar,
    p_id_usuario        integer DEFAULT NULL,
    p_id_pedido         integer DEFAULT NULL,
    p_id_compra_detalle integer DEFAULT NULL,
    p_observacion       varchar DEFAULT NULL
) RETURNS integer AS $$
DECLARE
    v_unidad        varchar(10);
    v_nombre_unidad varchar(10);
    v_actual        integer;
    v_nuevo         integer;
BEGIN
    SELECT unidad_costo INTO v_unidad FROM producto_variante WHERE id_variante = p_id_variante;

    IF v_unidad IS NULL THEN
        RAISE EXCEPTION 'La variante de producto no existe.';
    END IF;
    IF v_unidad NOT IN ('CAJA','UNIDAD') THEN
        RAISE EXCEPTION 'Vidrio y aluminio se manejan por piezas, no por cantidad.';
    END IF;
    v_nombre_unidad := CASE WHEN v_unidad = 'UNIDAD' THEN 'unidades' ELSE 'cajas' END;
    IF p_tipo NOT IN ('ENTRADA','SALIDA','AJUSTE') THEN
        RAISE EXCEPTION 'Tipo de movimiento no valido.';
    END IF;
    IF p_cantidad < 0 OR (p_cantidad = 0 AND p_tipo <> 'AJUSTE') THEN
        RAISE EXCEPTION 'La cantidad debe ser mayor a cero.';
    END IF;

    INSERT INTO inventario_cajas (id_variante, stock_cajas)
    VALUES (p_id_variante, 0)
    ON CONFLICT (id_variante) DO NOTHING;

    SELECT stock_cajas INTO v_actual
    FROM inventario_cajas WHERE id_variante = p_id_variante
    FOR UPDATE;

    v_nuevo := CASE p_tipo WHEN 'ENTRADA' THEN v_actual + p_cantidad
                           WHEN 'SALIDA'  THEN v_actual - p_cantidad
                           ELSE p_cantidad END;

    IF v_nuevo < 0 THEN
        RAISE EXCEPTION 'Stock insuficiente: hay % % y se quieren sacar %.', v_actual, v_nombre_unidad, p_cantidad;
    END IF;

    UPDATE inventario_cajas SET stock_cajas = v_nuevo WHERE id_variante = p_id_variante;

    INSERT INTO movimientos_inventario
        (id_variante, tipo, motivo, cantidad, stock_resultante, id_usuario, id_pedido,
         id_compra_detalle, observacion)
    VALUES
        (p_id_variante, p_tipo, p_motivo,
         CASE WHEN p_tipo = 'AJUSTE' THEN v_nuevo - v_actual ELSE p_cantidad END,
         v_nuevo, p_id_usuario, p_id_pedido, p_id_compra_detalle,
         COALESCE(p_observacion, CASE WHEN p_tipo = 'AJUSTE'
             THEN 'Ajuste de ' || v_actual::text || ' a ' || v_nuevo::text || ' ' || v_nombre_unidad
             END));

    RETURN v_nuevo;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Registrar un pedido completo.
-- p_items, p_detalles y p_medidas son arreglos JSONB (equivalen a los TVP).
-- Devuelve id_pedido.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_registrar(
    p_id_cliente integer,
    p_id_usuario integer,
    p_fecha      timestamp,
    p_items      jsonb,
    p_detalles   jsonb,
    p_medidas    jsonb
) RETURNS integer AS $$
DECLARE
    v_id_pedido integer;
    r           record;
BEGIN
    IF jsonb_array_length(COALESCE(p_items, '[]'::jsonb)) = 0 THEN
        RAISE EXCEPTION 'El pedido debe tener al menos un item (ventana, mampara, vidrio suelto o caja).';
    END IF;

    INSERT INTO pedidos (id_cliente, id_usuario, fecha, estado, total)
    VALUES (p_id_cliente, p_id_usuario, COALESCE(p_fecha, fn_hora_peru()), 'PENDIENTE', 0)
    RETURNING id_pedido INTO v_id_pedido;

    -- Items: se reservan los IDs de una sola vez y se mapea id_temp -> id real
    DROP TABLE IF EXISTS _items_src;
    CREATE TEMP TABLE _items_src ON COMMIT DROP AS
    SELECT x.*
    FROM jsonb_to_recordset(COALESCE(p_items, '[]'::jsonb)) AS x(
        id_temp          integer,
        tipo             varchar,
        etiqueta         varchar,
        ancho_cm         numeric,
        alto_cm          numeric,
        precio_materiales numeric,
        costo_extra      numeric,
        mano_obra        numeric,
        total            numeric,
        id_modelo        integer
    );

    DROP TABLE IF EXISTS _map_items;
    CREATE TEMP TABLE _map_items (id_temp integer PRIMARY KEY, id_item integer) ON COMMIT DROP;

    INSERT INTO _map_items (id_temp, id_item)
    SELECT s.id_temp, nextval(pg_get_serial_sequence('pedido_item', 'id_item'))
    FROM _items_src s;

    INSERT INTO pedido_item
        (id_item, id_pedido, tipo, descripcion, estado,
         precio_materiales, costo_extra, mano_obra, total, id_modelo)
    SELECT m.id_item, v_id_pedido, s.tipo, s.etiqueta, 'PENDIENTE',
           s.precio_materiales, COALESCE(s.costo_extra, 0), COALESCE(s.mano_obra, 0),
           s.total, s.id_modelo
    FROM _items_src s
    INNER JOIN _map_items m ON m.id_temp = s.id_temp;

    -- Detalles de origen JSONB
    DROP TABLE IF EXISTS _det_src;
    CREATE TEMP TABLE _det_src ON COMMIT DROP AS
    SELECT x.*
    FROM jsonb_to_recordset(COALESCE(p_detalles, '[]'::jsonb)) AS x(
        id_temp         integer,
        id_item_temp    integer,
        id_variante     integer,
        accion          varchar,
        id_pieza        integer,
        ancho_cm        numeric,
        alto_cm         numeric,
        largo_cm        numeric,
        cantidad_cajas  integer,
        subtotal        numeric
    );

    -- Inventario: solo las lineas que usan o cortan una pieza
    FOR r IN
        SELECT * FROM _det_src
        WHERE accion IN ('CORTA_PIEZA','USA_EXISTENTE')
        ORDER BY id_temp
    LOOP
        IF r.accion = 'CORTA_PIEZA' THEN
            PERFORM sp_inv_cortar_pieza(
                r.id_pieza, r.ancho_cm, r.alto_cm, r.largo_cm,
                10, p_id_usuario, v_id_pedido);
        ELSE
            PERFORM sp_inv_cambiar_estado_pieza(
                r.id_pieza, 'VENDIDA', p_id_usuario, v_id_pedido);
        END IF;
    END LOOP;

    -- Detalles: IDs reservados y mapeo id_temp -> id real
    DROP TABLE IF EXISTS _map_det;
    CREATE TEMP TABLE _map_det (id_temp integer PRIMARY KEY, id_detalle integer) ON COMMIT DROP;

    INSERT INTO _map_det (id_temp, id_detalle)
    SELECT d.id_temp, nextval(pg_get_serial_sequence('pedido_detalle', 'id_detalle'))
    FROM _det_src d;

    INSERT INTO pedido_detalle
        (id_detalle, id_item, id_variante, accion, id_pieza, estado,
         cantidad_cajas, ancho_cm, alto_cm, largo_cm,
         precio_unitario_aplicado, subtotal)
    SELECT m.id_detalle, mi.id_item, d.id_variante, d.accion, d.id_pieza, 'ACTIVO',
           d.cantidad_cajas, d.ancho_cm, d.alto_cm, d.largo_cm,
           pv.precio_unitario, d.subtotal
    FROM _det_src d
    INNER JOIN _map_items mi ON mi.id_temp = d.id_item_temp
    INNER JOIN _map_det m   ON m.id_temp = d.id_temp
    LEFT JOIN producto_variante pv ON pv.id_variante = d.id_variante;

    -- Medidas crudas (para el proveedor)
    INSERT INTO pedido_detalle_medida (id_detalle, etiqueta, valor_cm)
    SELECT m.id_detalle, x.etiqueta, x.valor_cm
    FROM jsonb_to_recordset(COALESCE(p_medidas, '[]'::jsonb)) AS x(
        id_detalle_temp integer,
        etiqueta        varchar,
        valor_cm        numeric
    )
    INNER JOIN _map_det m ON m.id_temp = x.id_detalle_temp;

    UPDATE pedidos
    SET total = (SELECT COALESCE(SUM(total), 0) FROM pedido_item WHERE id_pedido = v_id_pedido)
    WHERE id_pedido = v_id_pedido;

    RETURN v_id_pedido;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Marcar como rota una linea del pedido y crear su reemplazo.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_item_roto(
    p_id_detalle integer,
    p_id_usuario integer DEFAULT NULL
) RETURNS TABLE(id_detalle integer, accion varchar, id_pieza integer) AS $$
DECLARE
    v_id_item       integer;
    v_id_pedido     integer;
    v_id_variante   integer;
    v_id_pieza      integer;
    v_estado        varchar(15);
    v_ancho         numeric(10,2);
    v_alto          numeric(10,2);
    v_largo         numeric(10,2);
    v_cajas         integer;
    v_precio        numeric(10,2);
    v_subtotal      numeric(10,2);
    v_id_nuevo      integer;
    v_accion_nueva  varchar(20) := 'COMPRA_NUEVA';
    v_pieza_nueva   integer;
    v_pieza_final   integer;
    v_p_ancho       numeric(10,2);
    v_p_alto        numeric(10,2);
    v_p_largo       numeric(10,2);
BEGIN
    SELECT pd.id_item, pd.id_variante, pd.id_pieza, pd.estado, pd.ancho_cm, pd.alto_cm, pd.largo_cm,
           pd.cantidad_cajas, pd.precio_unitario_aplicado, pd.subtotal
      INTO v_id_item, v_id_variante, v_id_pieza, v_estado, v_ancho, v_alto, v_largo,
           v_cajas, v_precio, v_subtotal
    FROM pedido_detalle pd
    WHERE pd.id_detalle = p_id_detalle;

    IF v_id_item IS NULL THEN
        RAISE EXCEPTION 'El detalle indicado no existe.';
    END IF;
    IF v_estado <> 'ACTIVO' THEN
        RAISE EXCEPTION 'Solo se puede marcar como roto un detalle activo.';
    END IF;
    IF v_id_pieza IS NULL THEN
        RAISE EXCEPTION 'Solo se puede marcar como roto un material que ya esta registrado en el inventario.';
    END IF;

    SELECT id_pedido INTO v_id_pedido FROM pedido_item WHERE id_item = v_id_item;

    IF v_id_pieza IS NOT NULL THEN
        PERFORM sp_inv_cambiar_estado_pieza(
            v_id_pieza, 'DEFECTUOSA', p_id_usuario, v_id_pedido,
            'Pieza rota durante el pedido');
    END IF;

    UPDATE pedido_detalle pd SET estado = 'REEMPLAZADO' WHERE pd.id_detalle = p_id_detalle;

    IF v_cajas IS NULL AND (v_ancho IS NOT NULL OR v_largo IS NOT NULL) THEN
        SELECT ip.id_pieza INTO v_pieza_nueva
        FROM inventario_piezas ip
        WHERE ip.id_variante = v_id_variante AND ip.estado = 'DISPONIBLE'
          AND ( (v_largo IS NOT NULL AND ip.largo_cm >= v_largo)
             OR (v_ancho IS NOT NULL AND v_alto IS NOT NULL AND
                 ( (ip.ancho_cm >= v_ancho AND ip.alto_cm >= v_alto)
                OR (ip.ancho_cm >= v_alto  AND ip.alto_cm >= v_ancho) )) )
        ORDER BY CASE WHEN ip.ancho_cm IS NULL THEN ip.largo_cm ELSE ip.ancho_cm * ip.alto_cm END,
                 ip.id_pieza
        LIMIT 1;

        IF v_pieza_nueva IS NOT NULL THEN
            SELECT ip.ancho_cm, ip.alto_cm, ip.largo_cm
              INTO v_p_ancho, v_p_alto, v_p_largo
            FROM inventario_piezas ip WHERE ip.id_pieza = v_pieza_nueva;

            IF v_largo IS NOT NULL THEN
                v_accion_nueva := CASE WHEN v_p_largo BETWEEN v_largo - 0.01 AND v_largo + 0.01
                                       THEN 'USA_EXISTENTE' ELSE 'CORTA_PIEZA' END;
            ELSE
                v_accion_nueva := CASE
                    WHEN (abs(v_p_ancho - v_ancho) <= 0.01 AND abs(v_p_alto - v_alto) <= 0.01)
                      OR (abs(v_p_ancho - v_alto)  <= 0.01 AND abs(v_p_alto - v_ancho) <= 0.01)
                    THEN 'USA_EXISTENTE' ELSE 'CORTA_PIEZA' END;
            END IF;

            v_pieza_final := v_pieza_nueva;

            IF v_accion_nueva = 'CORTA_PIEZA' THEN
                PERFORM sp_inv_cortar_pieza(
                    v_pieza_nueva, v_ancho, v_alto, v_largo,
                    10, p_id_usuario, v_id_pedido);
            ELSE
                PERFORM sp_inv_cambiar_estado_pieza(
                    v_pieza_nueva, 'VENDIDA', p_id_usuario, v_id_pedido);
            END IF;
        END IF;
    END IF;

    INSERT INTO pedido_detalle AS pd
        (id_item, id_variante, accion, id_pieza, estado, id_detalle_origen,
         cantidad_cajas, ancho_cm, alto_cm, largo_cm,
         precio_unitario_aplicado, subtotal)
    VALUES
        (v_id_item, v_id_variante, v_accion_nueva, v_pieza_final, 'ACTIVO', p_id_detalle,
         v_cajas, v_ancho, v_alto, v_largo, v_precio, v_subtotal)
    RETURNING pd.id_detalle INTO v_id_nuevo;

    UPDATE pedido_item SET estado = 'PENDIENTE' WHERE id_item = v_id_item;

    RETURN QUERY SELECT v_id_nuevo, v_accion_nueva, v_pieza_final;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Cancelar un item completo del pedido.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_cancelar_item(
    p_id_item    integer,
    p_id_usuario integer DEFAULT NULL
) RETURNS void AS $$
DECLARE
    v_id_pedido integer;
    v_estado    varchar(15);
    r           record;
BEGIN
    SELECT id_pedido, estado INTO v_id_pedido, v_estado
    FROM pedido_item WHERE id_item = p_id_item;

    IF v_id_pedido IS NULL THEN
        RAISE EXCEPTION 'El item indicado no existe.';
    END IF;
    IF v_estado = 'CANCELADO' THEN
        RAISE EXCEPTION 'El item ya esta cancelado.';
    END IF;

    FOR r IN
        SELECT pd.id_pieza
        FROM pedido_detalle pd
        INNER JOIN inventario_piezas ip ON ip.id_pieza = pd.id_pieza
        WHERE pd.id_item = p_id_item AND pd.estado = 'ACTIVO' AND ip.estado = 'RESERVADA'
    LOOP
        PERFORM sp_inv_cambiar_estado_pieza(
            r.id_pieza, 'DISPONIBLE', p_id_usuario, v_id_pedido, 'Pedido cancelado');
    END LOOP;

    UPDATE pedido_detalle SET estado = 'CANCELADO'
    WHERE id_item = p_id_item AND estado = 'ACTIVO';

    UPDATE pedido_item SET estado = 'CANCELADO' WHERE id_item = p_id_item;

    UPDATE pedidos
    SET total = (SELECT COALESCE(SUM(total), 0) FROM pedido_item
                 WHERE id_pedido = v_id_pedido AND estado <> 'CANCELADO')
    WHERE id_pedido = v_id_pedido;

    IF NOT EXISTS (SELECT 1 FROM pedido_item
                   WHERE id_pedido = v_id_pedido AND estado <> 'CANCELADO') THEN
        UPDATE pedidos SET estado = 'CANCELADO' WHERE id_pedido = v_id_pedido;
    END IF;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Cancelar todos los items activos de un pedido.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_cancelar_pedido(
    p_id_pedido  integer,
    p_id_usuario integer DEFAULT NULL
) RETURNS void AS $$
DECLARE
    r record;
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pedidos WHERE id_pedido = p_id_pedido) THEN
        RAISE EXCEPTION 'El pedido indicado no existe.';
    END IF;

    FOR r IN SELECT id_item FROM pedido_item
             WHERE id_pedido = p_id_pedido AND estado <> 'CANCELADO'
             ORDER BY id_item
    LOOP
        PERFORM sp_ped_cancelar_item(r.id_item, p_id_usuario);
    END LOOP;

    IF NOT EXISTS (SELECT 1 FROM pedido_item
                   WHERE id_pedido = p_id_pedido AND estado <> 'CANCELADO') THEN
        UPDATE pedidos SET estado = 'CANCELADO' WHERE id_pedido = p_id_pedido;
    END IF;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Recalcular el estado de los items de un pedido.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_recalcular_estados(
    p_id_pedido integer
) RETURNS void AS $$
BEGIN
    UPDATE pedido_item pi
    SET estado = 'EN_PROCESO'
    WHERE pi.id_pedido = p_id_pedido
      AND pi.estado = 'PENDIENTE'
      AND EXISTS (SELECT 1 FROM pedido_detalle pd
                  WHERE pd.id_item = pi.id_item AND pd.estado = 'ACTIVO')
      AND NOT EXISTS (SELECT 1 FROM pedido_detalle pd
                      WHERE pd.id_item = pi.id_item AND pd.estado = 'ACTIVO'
                        AND pd.id_pieza IS NULL AND pd.id_compra_detalle IS NULL);
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Terminar un item del pedido (solo si esta EN_PROCESO).
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_terminar_item(
    p_id_item    integer,
    p_id_usuario integer DEFAULT NULL,
    p_fecha      timestamp DEFAULT NULL
) RETURNS void AS $$
DECLARE
    v_id_pedido integer;
    v_estado    varchar(15);
BEGIN
    SELECT id_pedido, estado INTO v_id_pedido, v_estado
    FROM pedido_item WHERE id_item = p_id_item;

    IF v_id_pedido IS NULL THEN
        RAISE EXCEPTION 'El item indicado no existe.';
    END IF;
    IF v_estado = 'CANCELADO' THEN
        RAISE EXCEPTION 'El item esta cancelado.';
    END IF;
    IF v_estado = 'COMPLETADO' THEN
        RAISE EXCEPTION 'El item ya esta completado.';
    END IF;

    PERFORM sp_ped_recalcular_estados(v_id_pedido);

    SELECT estado INTO v_estado FROM pedido_item WHERE id_item = p_id_item;
    IF v_estado <> 'EN_PROCESO' THEN
        RAISE EXCEPTION 'No se puede completar el item: aun faltan materiales en inventario.';
    END IF;

    UPDATE pedido_item SET estado = 'COMPLETADO' WHERE id_item = p_id_item;

    IF NOT EXISTS (SELECT 1 FROM pedido_item
                   WHERE id_pedido = v_id_pedido AND estado IN ('PENDIENTE','EN_PROCESO')) THEN
        UPDATE pedidos SET fecha_termino = COALESCE(p_fecha, fn_hora_peru())
        WHERE id_pedido = v_id_pedido;
    END IF;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Marcar todo el pedido como terminado.
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_terminar(
    p_id_pedido  integer,
    p_id_usuario integer DEFAULT NULL,
    p_fecha      timestamp DEFAULT NULL
) RETURNS void AS $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pedidos WHERE id_pedido = p_id_pedido) THEN
        RAISE EXCEPTION 'El pedido indicado no existe.';
    END IF;

    PERFORM sp_ped_recalcular_estados(p_id_pedido);

    IF EXISTS (SELECT 1 FROM pedido_item
               WHERE id_pedido = p_id_pedido AND estado = 'PENDIENTE') THEN
        RAISE EXCEPTION 'No se puede terminar el pedido: hay items cuyos materiales aun no estan en inventario.';
    END IF;

    UPDATE pedido_item SET estado = 'COMPLETADO'
    WHERE id_pedido = p_id_pedido AND estado = 'EN_PROCESO';

    UPDATE pedidos SET fecha_termino = COALESCE(p_fecha, fn_hora_peru())
    WHERE id_pedido = p_id_pedido;
END $$ LANGUAGE plpgsql;

-- ------------------------------------------------------------
-- Combos y resolucion de variantes de frmRegistrarPedido
-- ------------------------------------------------------------
CREATE OR REPLACE FUNCTION sp_ped_combo_colores_aluminio()
RETURNS TABLE(color varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT p.color FROM productos p
    WHERE p.categoria = 'ALUMINIO' AND p.activo AND p.color IS NOT NULL
    ORDER BY p.color;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_combo_tipos_aluminio(p_color varchar)
RETURNS TABLE(tipo varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT p.tipo FROM productos p
    WHERE p.categoria = 'ALUMINIO' AND p.color = p_color AND p.activo
    ORDER BY p.tipo;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_combo_colores_vidrio()
RETURNS TABLE(color_vidrio varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT p.tipo FROM productos p
    WHERE p.categoria = 'VIDRIO' AND p.activo
    ORDER BY p.tipo;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_combo_espesores_vidrio(p_tipo varchar, p_color varchar)
RETURNS TABLE(espesor_cm numeric) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT c.espesor_cm
    FROM producto_variante pv
    INNER JOIN productos p       ON p.id_producto = pv.id_producto
    INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE p.categoria = 'VIDRIO' AND p.tipo = p_tipo
      AND COALESCE(p.color, '') = COALESCE(p_color, '') AND p.activo
    ORDER BY c.espesor_cm;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_espesores_por_producto(p_id_producto integer)
RETURNS TABLE(espesor_cm numeric) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT c.espesor_cm
    FROM producto_variante pv
    INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE pv.id_producto = p_id_producto AND c.espesor_cm IS NOT NULL
    ORDER BY c.espesor_cm;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_resolver_variante_producto(
    p_id_producto integer,
    p_espesor_cm  numeric
) RETURNS TABLE(id_variante integer, precio_unitario numeric, unidad_costo varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM producto_variante pv
    INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE pv.id_producto = p_id_producto AND c.espesor_cm = p_espesor_cm
    LIMIT 1;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_resolver_variante_vidrio(
    p_tipo       varchar,
    p_color      varchar,
    p_espesor_cm numeric
) RETURNS TABLE(id_variante integer, precio_unitario numeric, unidad_costo varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM producto_variante pv
    INNER JOIN productos p       ON p.id_producto = pv.id_producto
    INNER JOIN caracteristicas c ON c.id_caracteristica = pv.id_caracteristica
    WHERE p.categoria = 'VIDRIO' AND p.tipo = p_tipo
      AND COALESCE(p.color, '') = COALESCE(p_color, '')
      AND c.espesor_cm = p_espesor_cm AND p.activo
    LIMIT 1;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_resolver_variante_aluminio(
    p_color varchar,
    p_tipo  varchar
) RETURNS TABLE(id_variante integer, precio_unitario numeric, unidad_costo varchar) AS $$
BEGIN
    RETURN QUERY
    SELECT pv.id_variante, pv.precio_unitario, pv.unidad_costo
    FROM producto_variante pv
    INNER JOIN productos p ON p.id_producto = pv.id_producto
    WHERE p.categoria = 'ALUMINIO' AND p.color = p_color AND p.tipo = p_tipo AND p.activo
    LIMIT 1;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_ped_tarifa_sugerida(p_perimetro_cm numeric)
RETURNS TABLE(costo_aluminio numeric, costo_silicona numeric) AS $$
BEGIN
    RETURN QUERY
    SELECT t.costo_aluminio, t.costo_silicona
    FROM tarifas_ventana t
    WHERE p_perimetro_cm >= t.perimetro_min_cm AND p_perimetro_cm <= t.perimetro_max_cm
    LIMIT 1;
END $$ LANGUAGE plpgsql;

-- ============================================================
-- Funciones del modulo de CLIENTES
-- ============================================================

CREATE OR REPLACE FUNCTION sp_cli_listar(p_texto varchar DEFAULT NULL)
RETURNS TABLE(id_cliente integer, nombre varchar, documento varchar, telefono varchar,
              direccion varchar, version_cliente bigint,
              tipo_persona varchar, tipo_documento varchar) AS $$
DECLARE
    v_texto  varchar;
    v_patron varchar;
BEGIN
    v_texto := NULLIF(btrim(p_texto), '');
    v_patron := '%' || replace(replace(replace(v_texto, '\', '\\'), '%', '\%'), '_', '\_') || '%';

    RETURN QUERY
    SELECT c.id_cliente, c.nombre, c.documento, c.telefono, c.direccion, c.version_cliente,
           (CASE c.tipo_persona WHEN 'NATURAL' THEN 'Natural'
                                WHEN 'JURIDICA' THEN 'Jurídica'
                                ELSE 'Por clasificar' END)::varchar AS tipo_persona,
           COALESCE(c.tipo_documento, 'Por clasificar')::varchar AS tipo_documento
    FROM clientes c
    WHERE v_texto IS NULL
       OR c.nombre    ILIKE v_patron ESCAPE '\'
       OR c.documento ILIKE v_patron ESCAPE '\'
       OR c.telefono  ILIKE v_patron ESCAPE '\'
    ORDER BY c.nombre, c.id_cliente;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_obtener(p_id_cliente integer)
RETURNS TABLE(id_cliente integer, nombre varchar, documento varchar, telefono varchar,
              direccion varchar, version_cliente bigint,
              tipo_persona varchar, tipo_documento varchar) AS $$
BEGIN
    IF p_id_cliente IS NULL OR p_id_cliente <= 0 THEN
        RAISE EXCEPTION 'Seleccione un cliente valido.';
    END IF;

    RETURN QUERY
    SELECT c.id_cliente, c.nombre, c.documento, c.telefono, c.direccion, c.version_cliente,
           c.tipo_persona, c.tipo_documento
    FROM clientes c WHERE c.id_cliente = p_id_cliente;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_guardar(
    p_id_cliente       integer DEFAULT 0,
    p_nombre           varchar DEFAULT NULL,
    p_documento        varchar DEFAULT NULL,
    p_telefono         varchar DEFAULT NULL,
    p_direccion        varchar DEFAULT NULL,
    p_version_original bigint DEFAULT NULL,
    p_tipo_persona     varchar DEFAULT NULL,
    p_tipo_documento   varchar DEFAULT NULL
) RETURNS integer AS $$
DECLARE
    v_version_actual bigint;
BEGIN
    p_nombre         := NULLIF(btrim(p_nombre), '');
    p_documento      := NULLIF(btrim(p_documento), '');
    p_telefono       := NULLIF(btrim(p_telefono), '');
    p_direccion      := NULLIF(btrim(p_direccion), '');
    p_tipo_persona   := upper(NULLIF(btrim(p_tipo_persona), ''));
    p_tipo_documento := upper(NULLIF(btrim(p_tipo_documento), ''));

    IF p_id_cliente IS NULL OR p_id_cliente < 0 THEN
        RAISE EXCEPTION 'El ID del cliente no es valido.';
    END IF;
    IF p_nombre IS NULL THEN
        RAISE EXCEPTION 'Ingrese el nombre del cliente.';
    END IF;
    IF char_length(p_nombre) > 150 THEN
        RAISE EXCEPTION 'El nombre admite hasta 150 caracteres.';
    END IF;
    IF char_length(p_documento) > 20 THEN
        RAISE EXCEPTION 'El documento admite hasta 20 caracteres.';
    END IF;
    IF char_length(p_telefono) > 20 THEN
        RAISE EXCEPTION 'El telefono admite hasta 20 caracteres.';
    END IF;
    IF char_length(p_direccion) > 255 THEN
        RAISE EXCEPTION 'La direccion admite hasta 255 caracteres.';
    END IF;
    IF p_telefono IS NOT NULL AND (
        p_telefono !~ '^[0-9+() -]+$'
        OR position('+' in substring(p_telefono from 2)) > 0
        OR length(regexp_replace(p_telefono, '[^0-9]', '', 'g')) NOT BETWEEN 7 AND 15)
    THEN
        RAISE EXCEPTION 'El telefono debe contener entre 7 y 15 digitos. El + solo se permite al inicio.';
    END IF;
    IF p_telefono IS NOT NULL AND (
        length(p_telefono) - length(replace(p_telefono, '(', '')) <>
        length(p_telefono) - length(replace(p_telefono, ')', ''))
        OR length(p_telefono) - length(replace(p_telefono, '(', '')) > 1
        OR (position('(' in p_telefono) > 0
            AND position('(' in p_telefono) > position(')' in p_telefono)))
    THEN
        RAISE EXCEPTION 'Revise los parentesis del telefono; se permite un par alrededor del codigo de area.';
    END IF;
    IF p_id_cliente > 0 AND p_version_original IS NULL THEN
        RAISE EXCEPTION 'Recargue el cliente antes de guardar sus cambios.';
    END IF;

    BEGIN
        IF p_id_cliente > 0 THEN
            SELECT c.version_cliente INTO v_version_actual
            FROM clientes c WHERE c.id_cliente = p_id_cliente
            FOR UPDATE;

            IF v_version_actual IS NULL THEN
                RAISE EXCEPTION 'El cliente ya no existe. Actualice el listado.';
            END IF;
            IF v_version_actual <> p_version_original THEN
                RAISE EXCEPTION 'Otro trabajador modifico este cliente. Presione Recargar y revise los datos antes de guardar.';
            END IF;
            IF p_tipo_persona IS NULL AND p_tipo_documento IS NULL THEN
                SELECT c.tipo_persona, c.tipo_documento INTO p_tipo_persona, p_tipo_documento
                FROM clientes c WHERE c.id_cliente = p_id_cliente;
            END IF;
        END IF;

        IF NOT ((p_tipo_persona IS NULL AND p_tipo_documento IS NULL)
            OR (p_tipo_persona IS NOT NULL AND p_tipo_documento IS NOT NULL AND (
                (p_tipo_persona = 'NATURAL' AND p_tipo_documento IN ('DNI','RUC','SIN DOCUMENTO'))
                OR (p_tipo_persona = 'JURIDICA' AND p_tipo_documento = 'RUC'))))
        THEN
            RAISE EXCEPTION 'Seleccione un tipo de persona y documento compatibles. La persona juridica requiere RUC.';
        END IF;
        IF p_tipo_documento = 'SIN DOCUMENTO' AND p_documento IS NOT NULL THEN
            RAISE EXCEPTION 'Deje el numero vacio si selecciona Sin documento.';
        END IF;
        IF p_tipo_documento IN ('DNI','RUC') AND (
            p_documento IS NULL OR p_documento !~ '^[0-9]+$'
            OR length(p_documento) <> CASE WHEN p_tipo_documento = 'DNI' THEN 8 ELSE 11 END)
        THEN
            RAISE EXCEPTION 'Ingrese 8 digitos para DNI u 11 digitos para RUC, sin espacios ni letras.';
        END IF;
        IF p_tipo_documento = 'RUC' AND (
            (p_tipo_persona = 'JURIDICA' AND left(p_documento,2) <> '20')
            OR (p_tipo_persona = 'NATURAL' AND left(p_documento,2) NOT IN ('10','15')))
        THEN
            RAISE EXCEPTION 'El RUC debe empezar por 20 para persona juridica, o por 10 o 15 para natural.';
        END IF;

        IF p_documento IS NOT NULL AND EXISTS (
            SELECT 1 FROM clientes c
            WHERE btrim(c.documento) = p_documento AND c.id_cliente <> p_id_cliente
              AND (p_tipo_documento IS NULL OR c.tipo_documento IS NULL
                   OR c.tipo_documento = p_tipo_documento))
        THEN
            RAISE EXCEPTION 'El documento ya pertenece a otro cliente. Busque el registro existente.';
        END IF;

        IF p_id_cliente = 0 THEN
            INSERT INTO clientes (nombre, documento, telefono, direccion, tipo_persona, tipo_documento)
            VALUES (p_nombre, p_documento, p_telefono, p_direccion, p_tipo_persona, p_tipo_documento)
            RETURNING id_cliente INTO p_id_cliente;
        ELSE
            UPDATE clientes
            SET nombre = p_nombre, documento = p_documento,
                telefono = p_telefono, direccion = p_direccion,
                tipo_persona = p_tipo_persona, tipo_documento = p_tipo_documento,
                version_cliente = version_cliente + 1
            WHERE id_cliente = p_id_cliente;
        END IF;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION 'El documento ya pertenece a otro cliente. Busque el registro existente.';
    END;

    RETURN p_id_cliente;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_tiene_pedidos(p_id_cliente integer)
RETURNS TABLE(tiene_pedidos boolean) AS $$
BEGIN
    IF p_id_cliente IS NULL OR p_id_cliente <= 0 THEN
        RAISE EXCEPTION 'Seleccione un cliente valido.';
    END IF;

    RETURN QUERY
    SELECT EXISTS (SELECT 1 FROM pedidos p WHERE p.id_cliente = p_id_cliente);
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_eliminar(p_id_cliente integer)
RETURNS void AS $$
BEGIN
    IF p_id_cliente IS NULL OR p_id_cliente <= 0 THEN
        RAISE EXCEPTION 'Seleccione un cliente valido.';
    END IF;

    PERFORM 1 FROM clientes c WHERE c.id_cliente = p_id_cliente FOR UPDATE;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'El cliente ya no existe. Actualice el listado.';
    END IF;
    IF EXISTS (SELECT 1 FROM pedidos p WHERE p.id_cliente = p_id_cliente) THEN
        RAISE EXCEPTION 'El cliente tiene pedidos y no se puede eliminar. Su historial debe conservarse.';
    END IF;

    DELETE FROM clientes WHERE id_cliente = p_id_cliente;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_documento_coincidente(
    p_documento      varchar,
    p_id_excluir     integer DEFAULT 0,
    p_tipo_documento varchar DEFAULT NULL
) RETURNS TABLE(id_cliente integer, nombre varchar) AS $$
BEGIN
    p_documento := NULLIF(btrim(p_documento), '');
    p_tipo_documento := upper(NULLIF(btrim(p_tipo_documento), ''));

    IF p_id_excluir IS NULL OR p_id_excluir < 0 THEN
        RAISE EXCEPTION 'El ID del cliente no es valido.';
    END IF;

    RETURN QUERY
    SELECT c.id_cliente, c.nombre
    FROM clientes c
    WHERE p_documento IS NOT NULL AND btrim(c.documento) = p_documento
      AND c.id_cliente <> p_id_excluir
      AND (p_tipo_documento IS NULL OR c.tipo_documento IS NULL
           OR c.tipo_documento = p_tipo_documento)
    ORDER BY c.nombre, c.id_cliente;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_historial(
    p_id_cliente integer,
    p_estado     varchar DEFAULT NULL,
    p_desde      date DEFAULT NULL,
    p_hasta      date DEFAULT NULL
) RETURNS TABLE(id_pedido integer, fecha timestamp, estado varchar, vendedor varchar,
                total numeric, pagado numeric, saldo numeric, excedente numeric) AS $$
BEGIN
    IF p_id_cliente IS NULL OR p_id_cliente <= 0 THEN
        RAISE EXCEPTION 'Seleccione un cliente valido.';
    END IF;
    p_estado := NULLIF(btrim(p_estado), '');
    IF p_estado IS NOT NULL AND p_estado NOT IN ('PENDIENTE','PARCIAL','PAGADO','CANCELADO') THEN
        RAISE EXCEPTION 'El estado del pedido no es valido.';
    END IF;
    IF p_desde > p_hasta THEN
        RAISE EXCEPTION 'La fecha Desde no puede ser posterior a Hasta.';
    END IF;

    RETURN QUERY
    SELECT p.id_pedido, p.fecha, p.estado, u.nombre AS vendedor, p.total,
           COALESCE(a.pagado, 0) AS pagado,
           CASE WHEN p.estado = 'CANCELADO' OR p.total <= COALESCE(a.pagado, 0)
                THEN 0::numeric
                ELSE p.total - COALESCE(a.pagado, 0) END AS saldo,
           CASE WHEN COALESCE(a.pagado, 0) > p.total
                THEN COALESCE(a.pagado, 0) - p.total
                ELSE 0::numeric END AS excedente
    FROM pedidos p
    INNER JOIN usuarios u ON u.id_usuario = p.id_usuario
    LEFT JOIN (
        SELECT pg.id_pedido, SUM(pg.monto_total::numeric) AS pagado
        FROM pagos pg GROUP BY pg.id_pedido
    ) a ON a.id_pedido = p.id_pedido
    WHERE p.id_cliente = p_id_cliente AND (p_estado IS NULL OR p.estado = p_estado)
      AND (p_desde IS NULL OR p.fecha::date >= p_desde)
      AND (p_hasta IS NULL OR p.fecha::date <= p_hasta)
    ORDER BY p.fecha DESC, p.id_pedido DESC;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_cli_pagos_pedido(
    p_id_cliente integer,
    p_id_pedido  integer
) RETURNS TABLE(id_pago integer, fecha timestamp, total_pago numeric,
                metodo varchar, monto_metodo numeric, comprobante varchar) AS $$
BEGIN
    IF p_id_cliente IS NULL OR p_id_cliente <= 0 OR p_id_pedido IS NULL OR p_id_pedido <= 0 THEN
        RAISE EXCEPTION 'Seleccione un cliente y un pedido validos.';
    END IF;
    IF NOT EXISTS (SELECT 1 FROM pedidos p
                   WHERE p.id_pedido = p_id_pedido AND p.id_cliente = p_id_cliente) THEN
        RAISE EXCEPTION 'El pedido no pertenece al cliente seleccionado o ya no existe.';
    END IF;

    RETURN QUERY
    SELECT p.id_pago, p.fecha, p.monto_total AS total_pago,
           COALESCE(m.nombre, 'Sin desglose') AS metodo,
           COALESCE(d.monto, p.monto_total) AS monto_metodo,
           CASE WHEN c.id_comprobante IS NULL THEN 'Sin comprobante'
                ELSE c.tipo_comprobante || ' ' || c.serie || '-' || c.numero END AS comprobante
    FROM pagos p
    LEFT JOIN detalle_pago d    ON d.id_pago = p.id_pago
    LEFT JOIN metodos_pago m    ON m.id_metodo_pago = d.id_metodo_pago
    LEFT JOIN comprobante_pago c ON c.id_pago = p.id_pago
    WHERE p.id_pedido = p_id_pedido
    ORDER BY p.fecha DESC, p.id_pago DESC, d.id_detalle_pago;
END $$ LANGUAGE plpgsql;

-- ============================================================
-- Funciones del modulo de PROVEEDORES
-- ============================================================

CREATE OR REPLACE FUNCTION sp_prov_listar(p_texto varchar DEFAULT NULL)
RETURNS TABLE(id_proveedor integer, nombre varchar, documento varchar, telefono varchar,
              direccion varchar, version_proveedor bigint,
              tipo_persona varchar, tipo_documento varchar) AS $$
DECLARE
    v_texto  varchar;
    v_patron varchar;
BEGIN
    v_texto := NULLIF(btrim(p_texto), '');
    v_patron := '%' || replace(replace(replace(v_texto, '\', '\\'), '%', '\%'), '_', '\_') || '%';

    RETURN QUERY
    SELECT p.id_proveedor, p.nombre, p.documento, p.telefono, p.direccion, p.version_proveedor,
           (CASE p.tipo_persona WHEN 'NATURAL' THEN 'Natural'
                                WHEN 'JURIDICA' THEN 'Jurídica'
                                ELSE 'Por clasificar' END)::varchar AS tipo_persona,
           COALESCE(p.tipo_documento, 'Por clasificar')::varchar AS tipo_documento
    FROM proveedores p
    WHERE v_texto IS NULL
       OR p.nombre    ILIKE v_patron ESCAPE '\'
       OR p.documento ILIKE v_patron ESCAPE '\'
       OR p.telefono  ILIKE v_patron ESCAPE '\'
    ORDER BY p.nombre, p.id_proveedor;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_prov_obtener(p_id_proveedor integer)
RETURNS TABLE(id_proveedor integer, nombre varchar, documento varchar, telefono varchar,
              direccion varchar, version_proveedor bigint,
              tipo_persona varchar, tipo_documento varchar) AS $$
BEGIN
    IF p_id_proveedor IS NULL OR p_id_proveedor <= 0 THEN
        RAISE EXCEPTION 'Seleccione un proveedor valido.';
    END IF;

    RETURN QUERY
    SELECT p.id_proveedor, p.nombre, p.documento, p.telefono, p.direccion, p.version_proveedor,
           p.tipo_persona, p.tipo_documento
    FROM proveedores p WHERE p.id_proveedor = p_id_proveedor;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_prov_guardar(
    p_id_proveedor     integer DEFAULT 0,
    p_nombre           varchar DEFAULT NULL,
    p_documento        varchar DEFAULT NULL,
    p_telefono         varchar DEFAULT NULL,
    p_direccion        varchar DEFAULT NULL,
    p_version_original bigint DEFAULT NULL,
    p_tipo_persona     varchar DEFAULT NULL,
    p_tipo_documento   varchar DEFAULT NULL
) RETURNS integer AS $$
DECLARE
    v_version_actual bigint;
BEGIN
    p_nombre         := NULLIF(btrim(p_nombre), '');
    p_documento      := NULLIF(btrim(p_documento), '');
    p_telefono       := NULLIF(btrim(p_telefono), '');
    p_direccion      := NULLIF(btrim(p_direccion), '');
    p_tipo_persona   := upper(NULLIF(btrim(p_tipo_persona), ''));
    p_tipo_documento := upper(NULLIF(btrim(p_tipo_documento), ''));

    IF p_id_proveedor IS NULL OR p_id_proveedor < 0 THEN
        RAISE EXCEPTION 'El ID del proveedor no es valido.';
    END IF;
    IF p_nombre IS NULL THEN
        RAISE EXCEPTION 'Ingrese el nombre del proveedor.';
    END IF;
    IF char_length(p_nombre) > 150 THEN
        RAISE EXCEPTION 'El nombre admite hasta 150 caracteres.';
    END IF;
    IF char_length(p_documento) > 20 THEN
        RAISE EXCEPTION 'El documento admite hasta 20 caracteres.';
    END IF;
    IF char_length(p_telefono) > 20 THEN
        RAISE EXCEPTION 'El telefono admite hasta 20 caracteres.';
    END IF;
    IF char_length(p_direccion) > 255 THEN
        RAISE EXCEPTION 'La direccion admite hasta 255 caracteres.';
    END IF;
    IF p_telefono IS NOT NULL AND (
        p_telefono !~ '^[0-9+() -]+$'
        OR position('+' in substring(p_telefono from 2)) > 0
        OR length(regexp_replace(p_telefono, '[^0-9]', '', 'g')) NOT BETWEEN 7 AND 15)
    THEN
        RAISE EXCEPTION 'El telefono debe contener entre 7 y 15 digitos. El + solo se permite al inicio.';
    END IF;
    IF p_telefono IS NOT NULL AND (
        length(p_telefono) - length(replace(p_telefono, '(', '')) <>
        length(p_telefono) - length(replace(p_telefono, ')', ''))
        OR length(p_telefono) - length(replace(p_telefono, '(', '')) > 1
        OR (position('(' in p_telefono) > 0
            AND position('(' in p_telefono) > position(')' in p_telefono)))
    THEN
        RAISE EXCEPTION 'Revise los parentesis del telefono; se permite un par alrededor del codigo de area.';
    END IF;
    IF p_id_proveedor > 0 AND p_version_original IS NULL THEN
        RAISE EXCEPTION 'Recargue el proveedor antes de guardar sus cambios.';
    END IF;

    BEGIN
        IF p_id_proveedor > 0 THEN
            SELECT p.version_proveedor INTO v_version_actual
            FROM proveedores p WHERE p.id_proveedor = p_id_proveedor
            FOR UPDATE;

            IF v_version_actual IS NULL THEN
                RAISE EXCEPTION 'El proveedor ya no existe. Actualice el listado.';
            END IF;
            IF v_version_actual <> p_version_original THEN
                RAISE EXCEPTION 'Otro trabajador modifico este proveedor. Presione Recargar y revise los datos antes de guardar.';
            END IF;
            IF p_tipo_persona IS NULL AND p_tipo_documento IS NULL THEN
                SELECT p.tipo_persona, p.tipo_documento INTO p_tipo_persona, p_tipo_documento
                FROM proveedores p WHERE p.id_proveedor = p_id_proveedor;
            END IF;
        END IF;

        IF NOT ((p_tipo_persona IS NULL AND p_tipo_documento IS NULL)
            OR (p_tipo_persona IS NOT NULL AND p_tipo_documento IS NOT NULL AND (
                (p_tipo_persona = 'NATURAL' AND p_tipo_documento IN ('DNI','RUC','SIN DOCUMENTO'))
                OR (p_tipo_persona = 'JURIDICA' AND p_tipo_documento = 'RUC'))))
        THEN
            RAISE EXCEPTION 'Seleccione un tipo de persona y documento compatibles. La persona juridica requiere RUC.';
        END IF;
        IF p_tipo_documento = 'SIN DOCUMENTO' AND p_documento IS NOT NULL THEN
            RAISE EXCEPTION 'Deje el numero vacio si selecciona Sin documento.';
        END IF;
        IF p_tipo_documento IN ('DNI','RUC') AND (
            p_documento IS NULL OR p_documento !~ '^[0-9]+$'
            OR length(p_documento) <> CASE WHEN p_tipo_documento = 'DNI' THEN 8 ELSE 11 END)
        THEN
            RAISE EXCEPTION 'Ingrese 8 digitos para DNI u 11 digitos para RUC, sin espacios ni letras.';
        END IF;
        IF p_tipo_documento = 'RUC' AND (
            (p_tipo_persona = 'JURIDICA' AND left(p_documento,2) <> '20')
            OR (p_tipo_persona = 'NATURAL' AND left(p_documento,2) NOT IN ('10','15')))
        THEN
            RAISE EXCEPTION 'El RUC debe empezar por 20 para persona juridica, o por 10 o 15 para natural.';
        END IF;

        IF p_documento IS NOT NULL AND EXISTS (
            SELECT 1 FROM proveedores p
            WHERE btrim(p.documento) = p_documento AND p.id_proveedor <> p_id_proveedor
              AND (p_tipo_documento IS NULL OR p.tipo_documento IS NULL
                   OR p.tipo_documento = p_tipo_documento))
        THEN
            RAISE EXCEPTION 'El documento ya pertenece a otro proveedor. Busque el registro existente.';
        END IF;

        IF p_id_proveedor = 0 THEN
            INSERT INTO proveedores (nombre, documento, telefono, direccion, tipo_persona, tipo_documento)
            VALUES (p_nombre, p_documento, p_telefono, p_direccion, p_tipo_persona, p_tipo_documento)
            RETURNING id_proveedor INTO p_id_proveedor;
        ELSE
            UPDATE proveedores
            SET nombre = p_nombre, documento = p_documento,
                telefono = p_telefono, direccion = p_direccion,
                tipo_persona = p_tipo_persona, tipo_documento = p_tipo_documento,
                version_proveedor = version_proveedor + 1
            WHERE id_proveedor = p_id_proveedor;
        END IF;
    EXCEPTION WHEN unique_violation THEN
        RAISE EXCEPTION 'El documento ya pertenece a otro proveedor. Busque el registro existente.';
    END;

    RETURN p_id_proveedor;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_prov_tiene_compras(p_id_proveedor integer)
RETURNS TABLE(tiene_compras boolean) AS $$
BEGIN
    IF p_id_proveedor IS NULL OR p_id_proveedor <= 0 THEN
        RAISE EXCEPTION 'Seleccione un proveedor valido.';
    END IF;

    RETURN QUERY
    SELECT EXISTS (SELECT 1 FROM compras c WHERE c.id_proveedor = p_id_proveedor);
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_prov_eliminar(p_id_proveedor integer)
RETURNS void AS $$
BEGIN
    IF p_id_proveedor IS NULL OR p_id_proveedor <= 0 THEN
        RAISE EXCEPTION 'Seleccione un proveedor valido.';
    END IF;

    PERFORM 1 FROM proveedores p WHERE p.id_proveedor = p_id_proveedor FOR UPDATE;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'El proveedor ya no existe. Actualice el listado.';
    END IF;
    IF EXISTS (SELECT 1 FROM compras c WHERE c.id_proveedor = p_id_proveedor) THEN
        RAISE EXCEPTION 'El proveedor tiene compras y no se puede eliminar. Su historial debe conservarse.';
    END IF;

    DELETE FROM proveedores WHERE id_proveedor = p_id_proveedor;
END $$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION sp_prov_documento_coincidente(
    p_documento      varchar,
    p_id_excluir     integer DEFAULT 0,
    p_tipo_documento varchar DEFAULT NULL
) RETURNS TABLE(id_proveedor integer, nombre varchar) AS $$
BEGIN
    p_documento := NULLIF(btrim(p_documento), '');
    p_tipo_documento := upper(NULLIF(btrim(p_tipo_documento), ''));

    IF p_id_excluir IS NULL OR p_id_excluir < 0 THEN
        RAISE EXCEPTION 'El ID del proveedor no es valido.';
    END IF;

    RETURN QUERY
    SELECT p.id_proveedor, p.nombre
    FROM proveedores p
    WHERE p_documento IS NOT NULL AND btrim(p.documento) = p_documento
      AND p.id_proveedor <> p_id_excluir
      AND (p_tipo_documento IS NULL OR p.tipo_documento IS NULL
           OR p.tipo_documento = p_tipo_documento)
    ORDER BY p.nombre, p.id_proveedor;
END $$ LANGUAGE plpgsql;

-- ============================================================
-- 4) INSERT DE DATOS
-- ============================================================

-- Usuario administrador (esquema base)
INSERT INTO usuarios (nombre, correo, contrasena, pregunta_seguridad, respuesta, rol)
SELECT 'admin', 'admin@vidrieria.com', '123456', '¿Nombre de tu mascota?', 'firulais', 'ADMINISTRADOR'
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre = 'admin');

INSERT INTO tipos_mensaje (codigo, titulo, texto)
SELECT v.codigo, v.titulo, v.texto
FROM (VALUES
    ('CERRARSESION',   'Cerrar sesión',      '¿Seguro que deseas cerrar sesión?'),
    ('CANCELARPEDIDO', 'Cancelar pedido',    '¿Deseas cancelar el pedido seleccionado?'),
    ('ITEMROTO',       'Un item se rompió',  '¿Estás seguro de que quieres colocar como roto el item?')
) AS v(codigo, titulo, texto)
WHERE NOT EXISTS (SELECT 1 FROM tipos_mensaje t WHERE t.codigo = v.codigo);

INSERT INTO metodos_pago (nombre)
SELECT v.nombre
FROM (VALUES ('EFECTIVO'), ('YAPE'), ('TARJETA'), ('PLIN')) AS v(nombre)
WHERE NOT EXISTS (SELECT 1 FROM metodos_pago m WHERE m.nombre = v.nombre);

-- Lista de permisos (solo agrega los que falten)
INSERT INTO tipo_permiso (codigo, nombre, descripcion)
SELECT v.codigo, v.nombre, v.descripcion
FROM (VALUES
    ('USUARIOS',   'Gestión de usuarios',  'Registrar, modificar, eliminar y dar de baja usuarios'),
    ('PERMISOS',   'Asignar permisos',     'Asignar o quitar permisos a los usuarios'),
    ('CLIENTES',   'Gestión de clientes',  'Registrar, modificar y ver el historial de clientes'),
    ('PRODUCTOS',  'Gestión de productos', 'Registrar, modificar y dar de baja productos'),
    ('INVENTARIO', 'Inventario',           'Ver inventario, piezas, cajas y kardex'),
    ('COMPRAS',    'Compras',              'Registrar y ver compras'),
    ('PEDIDOS',    'Pedidos y ventas',     'Cotizar, registrar pedidos y ver ventas'),
    ('PAGOS',      'Pagos',                'Registrar pagos y comprobantes'),
    ('REPORTES',   'Reportes',             'Ver los reportes del negocio')
) AS v(codigo, nombre, descripcion)
WHERE NOT EXISTS (SELECT 1 FROM tipo_permiso t WHERE t.codigo = v.codigo);

-- Permisos por defecto de cada rol
INSERT INTO rol_permiso (rol, id_tipo)
SELECT r.rol, t.id_tipo
FROM (VALUES
    ('ADMINISTRADOR', 'USUARIOS'),  ('ADMINISTRADOR', 'PERMISOS'),
    ('ADMINISTRADOR', 'CLIENTES'),  ('ADMINISTRADOR', 'PRODUCTOS'),
    ('ADMINISTRADOR', 'INVENTARIO'),('ADMINISTRADOR', 'COMPRAS'),
    ('ADMINISTRADOR', 'PEDIDOS'),   ('ADMINISTRADOR', 'PAGOS'),
    ('ADMINISTRADOR', 'REPORTES'),
    ('VENDEDOR', 'CLIENTES'),   ('VENDEDOR', 'INVENTARIO'),
    ('VENDEDOR', 'COMPRAS'),    ('VENDEDOR', 'PEDIDOS'),
    ('VENDEDOR', 'PAGOS')
) AS r(rol, codigo)
INNER JOIN tipo_permiso t ON t.codigo = r.codigo
WHERE NOT EXISTS (SELECT 1 FROM rol_permiso x
                  WHERE x.rol = r.rol AND x.id_tipo = t.id_tipo);

-- Usuarios que todavia no tienen NINGUN permiso reciben los de su rol
INSERT INTO permiso (id_usuario, id_tipo)
SELECT u.id_usuario, rp.id_tipo
FROM usuarios u
INNER JOIN rol_permiso rp ON rp.rol = u.rol
WHERE NOT EXISTS (SELECT 1 FROM permiso p WHERE p.id_usuario = u.id_usuario);

-- ------------------------------------------------------------
-- Datos de PRUEBA para el modulo de inventario
-- Solo inserta si la tabla productos esta vacia (no duplica datos)
-- ------------------------------------------------------------
DO $$
DECLARE
    v_c_alu  integer;
    v_c_tor1 integer;
    v_c_tar1 integer;
    v_p_u13  integer;
    v_p_tor  integer;
    v_p_tar  integer;
    v_v_u13  integer;
    v_v_tor  integer;
    v_v_tar  integer;
    v_admin  integer;
BEGIN
    IF EXISTS (SELECT 1 FROM productos) THEN
        RAISE NOTICE 'Ya hay productos registrados: no se insertan datos de prueba.';
    ELSE
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, NULL)
        RETURNING id_caracteristica INTO v_c_alu;
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 2.54)
        RETURNING id_caracteristica INTO v_c_tor1;
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 0.80)
        RETURNING id_caracteristica INTO v_c_tar1;

        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('ALUMINIO', 'U13', 'NEGRO', 'Aluminio U13')
        RETURNING id_producto INTO v_p_u13;
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('ACCESORIO', 'TARUGO', 'PLASTICO', 'Tarugo Plástico')
        RETURNING id_producto INTO v_p_tar;
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('ACCESORIO', 'TORNILLO', 'AUTORROSCANTE', 'Tornillo Autorroscante')
        RETURNING id_producto INTO v_p_tor;

        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
        VALUES (v_p_u13, v_c_alu, 'CM_LINEAL', 0.10, 5)
        RETURNING id_variante INTO v_v_u13;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
        VALUES (v_p_tor, v_c_tor1, 'CAJA', 25.00, 4)
        RETURNING id_variante INTO v_v_tor;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
        VALUES (v_p_tar, v_c_tar1, 'CAJA', 12.50, 4)
        RETURNING id_variante INTO v_v_tar;

        SELECT id_usuario INTO v_admin FROM usuarios WHERE nombre = 'admin' LIMIT 1;

        PERFORM sp_inv_ingresar_pieza(p_id_variante := v_v_u13, p_largo_cm := 600,
                                      p_origen := 'INICIAL', p_id_usuario := v_admin);
        PERFORM sp_inv_ingresar_pieza(p_id_variante := v_v_u13, p_largo_cm := 600,
                                      p_origen := 'INICIAL', p_id_usuario := v_admin);
        PERFORM sp_inv_ingresar_pieza(p_id_variante := v_v_u13, p_largo_cm := 350,
                                      p_origen := 'INICIAL', p_id_usuario := v_admin);

        PERFORM sp_inv_mover_cajas(p_id_variante := v_v_tor, p_tipo := 'ENTRADA',
                                   p_cantidad := 10, p_motivo := 'INICIAL', p_id_usuario := v_admin);
        PERFORM sp_inv_mover_cajas(p_id_variante := v_v_tar, p_tipo := 'ENTRADA',
                                   p_cantidad := 3, p_motivo := 'INICIAL', p_id_usuario := v_admin);

        RAISE NOTICE 'Datos de prueba de inventario insertados.';
    END IF;
END $$;

-- ============================================================
-- Catalogo real de vidrios (color, espejo, figura)
-- Precio por pie2: alto_cm * ancho_cm / 900 * precio_unitario
-- Idempotente: cada producto se inserta solo si no existe.
-- ============================================================
DO $$
DECLARE
    v_e3  integer;
    v_e4  integer;
    v_e55 integer;
    v_e8  integer;
    v_e10 integer;
    v_p   integer;
BEGIN
    SELECT id_caracteristica INTO v_e3 FROM caracteristicas
    WHERE espesor_cm = 0.30 AND medida_cm IS NULL LIMIT 1;
    IF v_e3 IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.30, NULL)
        RETURNING id_caracteristica INTO v_e3;
    END IF;

    SELECT id_caracteristica INTO v_e4 FROM caracteristicas
    WHERE espesor_cm = 0.40 AND medida_cm IS NULL LIMIT 1;
    IF v_e4 IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.40, NULL)
        RETURNING id_caracteristica INTO v_e4;
    END IF;

    SELECT id_caracteristica INTO v_e55 FROM caracteristicas
    WHERE espesor_cm = 0.55 AND medida_cm IS NULL LIMIT 1;
    IF v_e55 IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.55, NULL)
        RETURNING id_caracteristica INTO v_e55;
    END IF;

    SELECT id_caracteristica INTO v_e8 FROM caracteristicas
    WHERE espesor_cm = 0.80 AND medida_cm IS NULL LIMIT 1;
    IF v_e8 IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.80, NULL)
        RETURNING id_caracteristica INTO v_e8;
    END IF;

    SELECT id_caracteristica INTO v_e10 FROM caracteristicas
    WHERE espesor_cm = 1.00 AND medida_cm IS NULL LIMIT 1;
    IF v_e10 IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (1.00, NULL)
        RETURNING id_caracteristica INTO v_e10;
    END IF;

    -- ===================== COLOR =====================
    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='INCOLORO') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','COLOR','INCOLORO','Vidrio Incoloro')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 3.50, 0),
            (v_p, v_e4,  'PIE2', 4.00, 0),
            (v_p, v_e55, 'PIE2', 4.80, 0),
            (v_p, v_e8,  'PIE2', 6.80, 0),
            (v_p, v_e10, 'PIE2', 8.00, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='AZUL') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','COLOR','AZUL','Vidrio Color Azul')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 5.50, 0),
            (v_p, v_e4,  'PIE2', 6.00, 0),
            (v_p, v_e55, 'PIE2', 6.80, 0),
            (v_p, v_e8,  'PIE2', 8.80, 0),
            (v_p, v_e10, 'PIE2', 10.00, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='COLOR' AND color='MARRON') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','COLOR','MARRON','Vidrio Color Marrón')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 5.00, 0),
            (v_p, v_e4,  'PIE2', 5.50, 0),
            (v_p, v_e55, 'PIE2', 6.30, 0),
            (v_p, v_e8,  'PIE2', 8.30, 0),
            (v_p, v_e10, 'PIE2', 9.50, 0);
    END IF;

    -- ===================== ESPEJO =====================
    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='SOLIDO') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','ESPEJO','SOLIDO','Espejo Sólido')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 6.50, 0),
            (v_p, v_e4,  'PIE2', 7.00, 0),
            (v_p, v_e55, 'PIE2', 7.80, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='AZUL') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','ESPEJO','AZUL','Espejo Azul')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e55, 'PIE2', 10.30, 0),
            (v_p, v_e8,  'PIE2', 12.30, 0),
            (v_p, v_e10, 'PIE2', 13.50, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='ESPEJO' AND color='GRIS') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','ESPEJO','GRIS','Espejo Gris')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e55, 'PIE2', 13.30, 0),
            (v_p, v_e8,  'PIE2', 15.30, 0),
            (v_p, v_e10, 'PIE2', 16.50, 0);
    END IF;

    -- ===================== FIGURA =====================
    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='CATEDRAL') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','FIGURA','CATEDRAL','Vidrio Figura Catedral')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 3.50, 0),
            (v_p, v_e4,  'PIE2', 4.00, 0),
            (v_p, v_e55, 'PIE2', 4.80, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='CRUZ') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','FIGURA','CRUZ','Vidrio Figura Cruz')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 3.50, 0),
            (v_p, v_e4,  'PIE2', 4.00, 0),
            (v_p, v_e55, 'PIE2', 4.80, 0);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE categoria='VIDRIO' AND tipo='FIGURA' AND color='FLOR') THEN
        INSERT INTO productos (categoria, tipo, color, nombre)
        VALUES ('VIDRIO','FIGURA','FLOR','Vidrio Figura Flor')
        RETURNING id_producto INTO v_p;
        INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES
            (v_p, v_e3,  'PIE2', 3.50, 0),
            (v_p, v_e4,  'PIE2', 4.00, 0),
            (v_p, v_e55, 'PIE2', 4.80, 0);
    END IF;

    RAISE NOTICE 'Catalogo de vidrios (color/espejo/figura) insertado.';
END $$;

-- ============================================================
-- Catalogo real de aluminio y accesorios
-- Aluminio: precio por cm lineal (CM_LINEAL).
-- Accesorios: por unidad (UNIDAD) o por cm (CM_LINEAL, felpa).
-- Idempotente: si el producto ya existe se actualiza su precio/nombre.
-- ============================================================
DO $$
DECLARE
    v_c_sinmedida integer;
    v_idprod      integer;
    v_idvar       integer;
    r             record;
BEGIN
    SELECT id_caracteristica INTO v_c_sinmedida
    FROM caracteristicas
    WHERE espesor_cm IS NULL AND medida_cm IS NULL
    ORDER BY id_caracteristica
    LIMIT 1;
    IF v_c_sinmedida IS NULL THEN
        INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, NULL)
        RETURNING id_caracteristica INTO v_c_sinmedida;
    END IF;

    -- Quita el "Aluminio U13" de prueba (0.10) si no esta referenciado
    IF NOT EXISTS (
        SELECT 1 FROM pedido_detalle pd
            INNER JOIN producto_variante pv ON pv.id_variante = pd.id_variante
            INNER JOIN productos p          ON p.id_producto = pv.id_producto
        WHERE p.categoria = 'ALUMINIO' AND p.tipo = 'U13'
        UNION ALL
        SELECT 1 FROM compra_detalle cd
            INNER JOIN producto_variante pv ON pv.id_variante = cd.id_variante
            INNER JOIN productos p          ON p.id_producto = pv.id_producto
        WHERE p.categoria = 'ALUMINIO' AND p.tipo = 'U13'
    ) THEN
        DELETE FROM movimientos_inventario m
        USING producto_variante pv, productos p
        WHERE m.id_variante = pv.id_variante AND pv.id_producto = p.id_producto
          AND p.categoria = 'ALUMINIO' AND p.tipo = 'U13';

        DELETE FROM inventario_piezas ip
        USING producto_variante pv, productos p
        WHERE ip.id_variante = pv.id_variante AND pv.id_producto = p.id_producto
          AND p.categoria = 'ALUMINIO' AND p.tipo = 'U13';

        DELETE FROM producto_variante pv
        USING productos p
        WHERE pv.id_producto = p.id_producto
          AND p.categoria = 'ALUMINIO' AND p.tipo = 'U13';

        DELETE FROM productos WHERE categoria = 'ALUMINIO' AND tipo = 'U13';
    END IF;

    FOR r IN
        SELECT v.categoria, v.tipo, v.color, v.nombre, v.unidad, v.precio
        FROM (VALUES
            ('ALUMINIO',  'ANGULO GUIA',     'NEGRO',        'Aluminio Angulo Guia',     'CM_LINEAL', 0.15::numeric),
            ('ALUMINIO',  'ANGULO GUIA',     'ESTANDAR',     'Aluminio Angulo Guia',     'CM_LINEAL', 0.09),
            ('ALUMINIO',  'ANGULO GUIA',     'MADERA',       'Aluminio Angulo Guia',     'CM_LINEAL', 0.29),
            ('ALUMINIO',  'U13',             'NEGRO',        'Aluminio U13',             'CM_LINEAL', 0.06),
            ('ALUMINIO',  'U13',             'ESTANDAR',     'Aluminio U13',             'CM_LINEAL', 0.20),
            ('ALUMINIO',  'U13',             'MADERA',       'Aluminio U13',             'CM_LINEAL', 0.12),
            ('ALUMINIO',  'H8220',           'NEGRO',        'Aluminio H8220',           'CM_LINEAL', 0.25),
            ('ALUMINIO',  'H8220',           'ESTANDAR',     'Aluminio H8220',           'CM_LINEAL', 0.16),
            ('ALUMINIO',  'H8220',           'MADERA',       'Aluminio H8220',           'CM_LINEAL', 0.32),
            ('ALUMINIO',  'TUBO MULTIPLE',   'NEGRO',        'Aluminio Tubo Multiple',   'CM_LINEAL', 0.14),
            ('ALUMINIO',  'TUBO MULTIPLE',   'ESTANDAR',     'Aluminio Tubo Multiple',   'CM_LINEAL', 0.07),
            ('ALUMINIO',  'TUBO MULTIPLE',   'MADERA',       'Aluminio Tubo Multiple',   'CM_LINEAL', 0.22),
            ('ALUMINIO',  'PORTA FELPA',     'ESTANDAR',     'Aluminio Porta Felpa',     'CM_LINEAL', 0.014),
            ('ALUMINIO',  'PORTA FELPA',     'NEGRO',        'Aluminio Porta Felpa',     'CM_LINEAL', 0.021),
            ('ALUMINIO',  'PORTA FELPA',     'MADERA',       'Aluminio Porta Felpa',     'CM_LINEAL', 0.04),
            ('ALUMINIO',  'J PORTAFELPA',    'ESTANDAR',     'Aluminio J Portafelpa',    'CM_LINEAL', 0.03),
            ('ALUMINIO',  'J PORTAFELPA',    'NEGRO',        'Aluminio J Portafelpa',    'CM_LINEAL', 0.05),
            ('ALUMINIO',  'J PORTAFELPA',    'MADERA',       'Aluminio J Portafelpa',    'CM_LINEAL', 0.08),
            ('ALUMINIO',  'TUBO MULTIPLE SIMPLE', 'ESTANDAR','Aluminio Tubo Multiple Simple','CM_LINEAL', 0.04),
            ('ALUMINIO',  'TUBO MULTIPLE SIMPLE', 'NEGRO',   'Aluminio Tubo Multiple Simple','CM_LINEAL', 0.07),
            ('ALUMINIO',  'TUBO MULTIPLE SIMPLE', 'MADERA',  'Aluminio Tubo Multiple Simple','CM_LINEAL', 0.12),
            ('ACCESORIO', 'RUEDA 8220',      NULL,           'Accesorio Rueda 8220',     'UNIDAD',    2.00),
            ('ACCESORIO', 'FELPA 10',        'ESTANDAR',     'Accesorio Felpa 10',       'CM_LINEAL', 0.002),
            ('ACCESORIO', 'FELPA 10',        'NEGRO',        'Accesorio Felpa 10',       'CM_LINEAL', 0.005),
            ('ACCESORIO', 'FELPA 15',        'ESTANDAR',     'Accesorio Felpa 15',       'CM_LINEAL', 0.003),
            ('ACCESORIO', 'FELPA 15',        'NEGRO',        'Accesorio Felpa 15',       'CM_LINEAL', 0.006),
            ('ACCESORIO', 'FELPA 20',        'ESTANDAR',     'Accesorio Felpa 20',       'CM_LINEAL', 0.004),
            ('ACCESORIO', 'FELPA 20',        'NEGRO',        'Accesorio Felpa 20',       'CM_LINEAL', 0.007),
            ('ACCESORIO', 'SEGURO PISTILLO', 'NEGRO',        'Accesorio Seguro Pistillo','UNIDAD',    5.00),
            ('ACCESORIO', 'SEGURO PISTILLO', 'ESTANDAR',     'Accesorio Seguro Pistillo','UNIDAD',    3.00),
            ('ACCESORIO', 'SEGURO PISTILLO', 'MADERA',       'Accesorio Seguro Pistillo','UNIDAD',    6.00),
            ('ACCESORIO', 'SILICONA',        'NEGRA',        'Accesorio Silicona',       'UNIDAD',    12.00),
            ('ACCESORIO', 'SILICONA',        'TRANSPARENTE', 'Accesorio Silicona',       'UNIDAD',    10.00),
            ('ACCESORIO', 'SILICONA',        'BLANCA',       'Accesorio Silicona',       'UNIDAD',    11.00)
        ) AS v(categoria, tipo, color, nombre, unidad, precio)
    LOOP
        v_idprod := NULL;
        SELECT p.id_producto INTO v_idprod
        FROM productos p
        WHERE p.categoria = r.categoria AND p.tipo = r.tipo
          AND COALESCE(p.color, '') = COALESCE(r.color, '')
        LIMIT 1;

        IF v_idprod IS NULL THEN
            INSERT INTO productos (categoria, tipo, color, nombre)
            VALUES (r.categoria, r.tipo, r.color, r.nombre)
            RETURNING id_producto INTO v_idprod;

            INSERT INTO producto_variante
                (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
            VALUES (v_idprod, v_c_sinmedida, r.unidad, r.precio, 0);
        ELSE
            UPDATE productos SET nombre = r.nombre, color = r.color WHERE id_producto = v_idprod;

            v_idvar := NULL;
            SELECT pv.id_variante INTO v_idvar
            FROM producto_variante pv WHERE pv.id_producto = v_idprod LIMIT 1;

            IF v_idvar IS NULL THEN
                INSERT INTO producto_variante
                    (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo)
                VALUES (v_idprod, v_c_sinmedida, r.unidad, r.precio, 0);
            ELSE
                UPDATE producto_variante
                SET unidad_costo = r.unidad, precio_unitario = r.precio
                WHERE id_variante = v_idvar;
            END IF;
        END IF;
    END LOOP;

    RAISE NOTICE 'Catalogo de aluminio y accesorios insertado.';
END $$;

-- ============================================================
-- Modelos de ventana: catalogo base
-- ============================================================
DO $$
DECLARE
    v_m1 integer;
    v_m2 integer;
    v_m3 integer;
    v_m4 integer;
    v_m5 integer;
BEGIN
    INSERT INTO modelos_ventana (nombre, descripcion, orden)
    SELECT v.nombre, v.descripcion, v.orden
    FROM (VALUES
        ('Ventana 2 fijas + 1 corrediza con alero',  '2 hojas fijas y 1 corrediza, con alero', 1),
        ('Ventana 2 fijas + 1 corrediza sin alero',  '2 hojas fijas y 1 corrediza, sin alero', 2),
        ('Ventana 1 fija + 1 corrediza con alero',   '1 hoja fija y 1 corrediza, con alero', 3),
        ('Ventana 1 fija + 1 corrediza sin alero',   '1 hoja fija y 1 corrediza, sin alero', 4),
        ('Ventana 2 fijas + 2 corredizas con alero', '2 hojas fijas y 2 corredizas, con alero', 5)
    ) AS v(nombre, descripcion, orden)
    WHERE NOT EXISTS (SELECT 1 FROM modelos_ventana m WHERE m.nombre = v.nombre);

    SELECT id_modelo INTO v_m1 FROM modelos_ventana WHERE nombre = 'Ventana 2 fijas + 1 corrediza con alero';
    SELECT id_modelo INTO v_m2 FROM modelos_ventana WHERE nombre = 'Ventana 2 fijas + 1 corrediza sin alero';
    SELECT id_modelo INTO v_m3 FROM modelos_ventana WHERE nombre = 'Ventana 1 fija + 1 corrediza con alero';
    SELECT id_modelo INTO v_m4 FROM modelos_ventana WHERE nombre = 'Ventana 1 fija + 1 corrediza sin alero';
    SELECT id_modelo INTO v_m5 FROM modelos_ventana WHERE nombre = 'Ventana 2 fijas + 2 corredizas con alero';

    -- Modelo 1: 2 fijas + 1 corrediza CON alero
    IF v_m1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM modelo_ventana_variable WHERE id_modelo = v_m1) THEN
        INSERT INTO modelo_ventana_variable (id_modelo, orden, nombre, formula) VALUES
        (v_m1, 10, 'anclaje',        'max_alto - 3 - alero'),
        (v_m1, 20, 'alto_fijo',      'max_alto - 3 - 1.5 - alero'),
        (v_m1, 30, 'ancho_fijo_arr', 'IIF(tres_ancho=1, ancho_centro, ancho_abajo)/3'),
        (v_m1, 40, 'ancho_fijo_aba', 'ancho_abajo/3'),
        (v_m1, 50, 'max_fijo',       'IIF(ancho_fijo_arr>ancho_fijo_aba, ancho_fijo_arr, ancho_fijo_aba)'),
        (v_m1, 60, 'corredizo_ancho','ancho_abajo - 2*max_fijo'),
        (v_m1, 70, 'corredizo_alto', 'anclaje - 2.5'),
        (v_m1, 80, 'largo_tubo',     'IIF(tres_ancho=1, IIF(ancho_arriba>ancho_centro, ancho_arriba, ancho_centro), ancho_abajo)'),
        (v_m1, 90, 'u13_alero',      'IIF(tres_ancho=1, ancho_arriba, ancho_abajo)'),
        (v_m1, 100,'dos_vidrios',    'IIF(ancho_abajo>120 OR ancho_arriba>120, 1, 0)'),
        (v_m1, 110,'ancho_vid_aler', 'IIF(dos_vidrios=1, ancho_arriba/2, ancho_arriba)'),
        (v_m1, 120,'alto_aler_1',    'IIF(tres_alto=1, IIF(alto_izq>alto_centro, alto_izq, alto_centro), alto_izq) - 1 - anclaje'),
        (v_m1, 130,'alto_aler_2',    'IIF(tres_alto=1, IIF(alto_centro>alto_der, alto_centro, alto_der), alto_izq) - 1 - anclaje'),
        (v_m1, 140,'alto_aler',      'IIF(tres_alto=1, max_alto, alto_izq) - 1 - anclaje');

        INSERT INTO modelo_ventana_componente (id_modelo, orden, categoria, tipo, rol_color, tipo_medida, expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion) VALUES
        (v_m1, 10, 'ALUMINIO','ANGULO GUIA','MATERIAL','LARGO','ancho_abajo',NULL,NULL,NULL,NULL),
        (v_m1, 20, 'ALUMINIO','TUBO MULTIPLE','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m1, 30, 'ACCESORIO','FELPA 10','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m1, 40, 'ACCESORIO','FELPA 20','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m1, 50, 'ALUMINIO','U13','MATERIAL','LARGO','u13_alero',NULL,NULL,NULL,NULL),
        (v_m1, 60, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo_arr','alto_fijo',NULL),
        (v_m1, 70, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo_aba','alto_fijo',NULL),
        (v_m1, 80, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo_arr',NULL,NULL,NULL,NULL),
        (v_m1, 90, 'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m1, 100,'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo_aba',NULL,NULL,NULL,NULL),
        (v_m1, 110,'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m1, 120,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m1, 130,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m1, 140,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m1, 150,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m1, 160,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m1, 170,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m1, 180,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m1, 190,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m1, 200,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m1, 210,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m1, 220,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_1','dos_vidrios=1'),
        (v_m1, 230,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_2','dos_vidrios=1'),
        (v_m1, 240,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m1, 250,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m1, 260,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m1, 270,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler','dos_vidrios=0'),
        (v_m1, 280,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m1, 290,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m1, 300,'ACCESORIO','SILICONA','SILICONA','CANTIDAD',NULL,'2',NULL,NULL,NULL);

        INSERT INTO modelo_ventana_validacion (id_modelo, condicion, mensaje) VALUES
        (v_m1, 'max_alto < 90', 'Este modelo lleva alero: para alturas menores a 90 cm use un modelo sin alero.'),
        (v_m1, 'tres_alto=1 AND (max_alto-min_alto)>5', 'Con 3 alturas distintas, la diferencia maxima permitida es 5 cm.');
    END IF;

    -- Modelo 2: 2 fijas + 1 corrediza SIN alero
    IF v_m2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM modelo_ventana_variable WHERE id_modelo = v_m2) THEN
        INSERT INTO modelo_ventana_variable (id_modelo, orden, nombre, formula) VALUES
        (v_m2, 10, 'anclaje',        'max_alto'),
        (v_m2, 20, 'alto_fijo',      'max_alto - 0.5'),
        (v_m2, 30, 'ancho_fijo_arr', 'IIF(tres_ancho=1, ancho_centro, ancho_abajo)/3'),
        (v_m2, 40, 'ancho_fijo_aba', 'ancho_abajo/3'),
        (v_m2, 50, 'max_fijo',       'IIF(ancho_fijo_arr>ancho_fijo_aba, ancho_fijo_arr, ancho_fijo_aba)'),
        (v_m2, 60, 'corredizo_ancho','ancho_abajo - 2*max_fijo'),
        (v_m2, 70, 'corredizo_alto', 'anclaje - 1.5'),
        (v_m2, 80, 'largo_tubo',     'IIF(tres_ancho=1, IIF(ancho_arriba>ancho_centro, ancho_arriba, ancho_centro), ancho_abajo)');

        INSERT INTO modelo_ventana_componente (id_modelo, orden, categoria, tipo, rol_color, tipo_medida, expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion) VALUES
        (v_m2, 10, 'ALUMINIO','ANGULO GUIA','MATERIAL','LARGO','ancho_abajo',NULL,NULL,NULL,NULL),
        (v_m2, 20, 'ALUMINIO','TUBO MULTIPLE SIMPLE','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m2, 30, 'ACCESORIO','FELPA 10','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m2, 40, 'ACCESORIO','FELPA 20','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m2, 50, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo_arr','alto_fijo',NULL),
        (v_m2, 60, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo_aba','alto_fijo',NULL),
        (v_m2, 70, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo_arr',NULL,NULL,NULL,NULL),
        (v_m2, 80, 'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m2, 90, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo_aba',NULL,NULL,NULL,NULL),
        (v_m2, 100,'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m2, 110,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m2, 120,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m2, 130,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m2, 140,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m2, 150,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m2, 160,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m2, 170,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m2, 180,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m2, 190,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m2, 200,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL);

        INSERT INTO modelo_ventana_validacion (id_modelo, condicion, mensaje) VALUES
        (v_m2, 'max_alto >= 90', 'Para 90 cm o mas de alto se debe usar un modelo con alero.'),
        (v_m2, 'tres_alto=1 AND (max_alto-min_alto)>5', 'Con 3 alturas distintas, la diferencia maxima permitida es 5 cm.');
    END IF;

    -- Modelo 3: 1 fija + 1 corrediza CON alero
    IF v_m3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM modelo_ventana_variable WHERE id_modelo = v_m3) THEN
        INSERT INTO modelo_ventana_variable (id_modelo, orden, nombre, formula) VALUES
        (v_m3, 10, 'anclaje',        'max_alto - 3 - alero'),
        (v_m3, 20, 'alto_fijo',      'max_alto - 3 - 1.5 - alero'),
        (v_m3, 30, 'ancho_fijo',     'ancho_abajo/2'),
        (v_m3, 40, 'corredizo_ancho','ancho_abajo/2'),
        (v_m3, 50, 'corredizo_alto', 'anclaje - 2.5'),
        (v_m3, 60, 'largo_tubo',     'IIF(tres_ancho=1, IIF(ancho_arriba>ancho_centro, ancho_arriba, ancho_centro), ancho_abajo)'),
        (v_m3, 70, 'u13_alero',      'IIF(tres_ancho=1, ancho_arriba, ancho_abajo)'),
        (v_m3, 80, 'dos_vidrios',    'IIF(ancho_abajo>120 OR ancho_arriba>120, 1, 0)'),
        (v_m3, 90, 'ancho_vid_aler', 'IIF(dos_vidrios=1, ancho_arriba/2, ancho_arriba)'),
        (v_m3, 100,'alto_aler_1',    'IIF(tres_alto=1, IIF(alto_izq>alto_centro, alto_izq, alto_centro), alto_izq) - 1 - anclaje'),
        (v_m3, 110,'alto_aler_2',    'IIF(tres_alto=1, IIF(alto_centro>alto_der, alto_centro, alto_der), alto_izq) - 1 - anclaje'),
        (v_m3, 120,'alto_aler',      'IIF(tres_alto=1, max_alto, alto_izq) - 1 - anclaje');

        INSERT INTO modelo_ventana_componente (id_modelo, orden, categoria, tipo, rol_color, tipo_medida, expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion) VALUES
        (v_m3, 10, 'ALUMINIO','ANGULO GUIA','MATERIAL','LARGO','ancho_abajo',NULL,NULL,NULL,NULL),
        (v_m3, 20, 'ALUMINIO','TUBO MULTIPLE','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m3, 30, 'ACCESORIO','FELPA 10','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m3, 40, 'ACCESORIO','FELPA 20','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m3, 50, 'ALUMINIO','U13','MATERIAL','LARGO','u13_alero',NULL,NULL,NULL,NULL),
        (v_m3, 60, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo','alto_fijo',NULL),
        (v_m3, 70, 'ALUMINIO','J PORTAFELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m3, 80, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo',NULL,NULL,NULL,NULL),
        (v_m3, 90, 'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m3, 100,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m3, 110,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m3, 120,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m3, 130,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m3, 140,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m3, 150,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m3, 160,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m3, 170,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m3, 180,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m3, 190,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m3, 200,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_1','dos_vidrios=1'),
        (v_m3, 210,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_2','dos_vidrios=1'),
        (v_m3, 220,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m3, 230,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m3, 240,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m3, 250,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler','dos_vidrios=0'),
        (v_m3, 260,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m3, 270,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m3, 280,'ACCESORIO','SILICONA','SILICONA','CANTIDAD',NULL,'2',NULL,NULL,NULL);

        INSERT INTO modelo_ventana_validacion (id_modelo, condicion, mensaje) VALUES
        (v_m3, 'max_alto < 90', 'Este modelo lleva alero: para alturas menores a 90 cm use un modelo sin alero.'),
        (v_m3, 'tres_alto=1 AND (max_alto-min_alto)>5', 'Con 3 alturas distintas, la diferencia maxima permitida es 5 cm.');
    END IF;

    -- Modelo 4: 1 fija + 1 corrediza SIN alero
    IF v_m4 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM modelo_ventana_variable WHERE id_modelo = v_m4) THEN
        INSERT INTO modelo_ventana_variable (id_modelo, orden, nombre, formula) VALUES
        (v_m4, 10, 'anclaje',        'max_alto'),
        (v_m4, 20, 'alto_fijo',      'max_alto - 0.5'),
        (v_m4, 30, 'ancho_fijo',     'ancho_abajo/2'),
        (v_m4, 40, 'corredizo_ancho','ancho_abajo/2'),
        (v_m4, 50, 'corredizo_alto', 'anclaje - 1.5'),
        (v_m4, 60, 'largo_tubo',     'IIF(tres_ancho=1, IIF(ancho_arriba>ancho_centro, ancho_arriba, ancho_centro), ancho_abajo)');

        INSERT INTO modelo_ventana_componente (id_modelo, orden, categoria, tipo, rol_color, tipo_medida, expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion) VALUES
        (v_m4, 10, 'ALUMINIO','ANGULO GUIA','MATERIAL','LARGO','ancho_abajo',NULL,NULL,NULL,NULL),
        (v_m4, 20, 'ALUMINIO','TUBO MULTIPLE SIMPLE','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m4, 30, 'ACCESORIO','FELPA 10','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m4, 40, 'ACCESORIO','FELPA 20','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m4, 50, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo','alto_fijo',NULL),
        (v_m4, 60, 'ALUMINIO','J PORTAFELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m4, 70, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo',NULL,NULL,NULL,NULL),
        (v_m4, 80, 'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m4, 90, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m4, 100,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m4, 110,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m4, 120,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m4, 130,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m4, 140,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m4, 150,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m4, 160,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m4, 170,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m4, 180,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL);

        INSERT INTO modelo_ventana_validacion (id_modelo, condicion, mensaje) VALUES
        (v_m4, 'max_alto >= 90', 'Para 90 cm o mas de alto se debe usar un modelo con alero.'),
        (v_m4, 'tres_alto=1 AND (max_alto-min_alto)>5', 'Con 3 alturas distintas, la diferencia maxima permitida es 5 cm.');
    END IF;

    -- Modelo 5: 2 fijas + 2 corredizas CON alero
    IF v_m5 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM modelo_ventana_variable WHERE id_modelo = v_m5) THEN
        INSERT INTO modelo_ventana_variable (id_modelo, orden, nombre, formula) VALUES
        (v_m5, 10, 'anclaje',        'max_alto - 3 - alero'),
        (v_m5, 20, 'alto_fijo',      'max_alto - 3 - 1.5 - alero'),
        (v_m5, 30, 'ancho_fijo',     'ancho_abajo/4'),
        (v_m5, 40, 'corredizo_ancho','(ancho_abajo - 2*ancho_fijo)/2'),
        (v_m5, 50, 'corredizo_alto', 'anclaje - 2.5'),
        (v_m5, 60, 'largo_tubo',     'IIF(tres_ancho=1, IIF(ancho_arriba>ancho_centro, ancho_arriba, ancho_centro), ancho_abajo)'),
        (v_m5, 70, 'u13_alero',      'IIF(tres_ancho=1, ancho_arriba, ancho_abajo)'),
        (v_m5, 80, 'dos_vidrios',    'IIF(ancho_abajo>120 OR ancho_arriba>120, 1, 0)'),
        (v_m5, 90, 'ancho_vid_aler', 'IIF(dos_vidrios=1, ancho_arriba/2, ancho_arriba)'),
        (v_m5, 100,'alto_aler_1',    'IIF(tres_alto=1, IIF(alto_izq>alto_centro, alto_izq, alto_centro), alto_izq) - 1 - anclaje'),
        (v_m5, 110,'alto_aler_2',    'IIF(tres_alto=1, IIF(alto_centro>alto_der, alto_centro, alto_der), alto_izq) - 1 - anclaje'),
        (v_m5, 120,'alto_aler',      'IIF(tres_alto=1, max_alto, alto_izq) - 1 - anclaje');

        INSERT INTO modelo_ventana_componente (id_modelo, orden, categoria, tipo, rol_color, tipo_medida, expr_largo, expr_cantidad, expr_ancho, expr_alto, condicion) VALUES
        (v_m5, 10, 'ALUMINIO','ANGULO GUIA','MATERIAL','LARGO','ancho_abajo',NULL,NULL,NULL,NULL),
        (v_m5, 20, 'ALUMINIO','TUBO MULTIPLE','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m5, 30, 'ACCESORIO','FELPA 10','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m5, 40, 'ACCESORIO','FELPA 20','MATERIAL','LARGO','largo_tubo',NULL,NULL,NULL,NULL),
        (v_m5, 50, 'ALUMINIO','U13','MATERIAL','LARGO','u13_alero',NULL,NULL,NULL,NULL),
        (v_m5, 60, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo','alto_fijo',NULL),
        (v_m5, 70, 'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_fijo','alto_fijo',NULL),
        (v_m5, 80, 'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo',NULL,NULL,NULL,NULL),
        (v_m5, 90, 'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 100,'ALUMINIO','U13','MATERIAL','LARGO','ancho_fijo',NULL,NULL,NULL,NULL),
        (v_m5, 110,'ALUMINIO','U13','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 120,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m5, 130,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m5, 140,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m5, 150,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m5, 160,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 170,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 180,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 190,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 200,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 210,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 220,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'corredizo_ancho','corredizo_alto',NULL),
        (v_m5, 230,'ALUMINIO','H8220','MATERIAL','LARGO','corredizo_ancho+1',NULL,NULL,NULL,NULL),
        (v_m5, 240,'ACCESORIO','RUEDA 8220','NINGUNO','CANTIDAD',NULL,'2',NULL,NULL,NULL),
        (v_m5, 250,'ACCESORIO','SEGURO PISTILLO','VENTANA','CANTIDAD',NULL,'1',NULL,NULL,NULL),
        (v_m5, 260,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 270,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 280,'ACCESORIO','FELPA 15','MATERIAL','LARGO','anclaje',NULL,NULL,NULL,NULL),
        (v_m5, 290,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 300,'ALUMINIO','PORTA FELPA','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 310,'ACCESORIO','FELPA 20','MATERIAL','LARGO','anclaje-1',NULL,NULL,NULL,NULL),
        (v_m5, 320,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_1','dos_vidrios=1'),
        (v_m5, 330,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler_2','dos_vidrios=1'),
        (v_m5, 340,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m5, 350,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m5, 360,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=1'),
        (v_m5, 370,'VIDRIO',NULL,'MATERIAL','VIDRIO',NULL,NULL,'ancho_vid_aler','alto_aler','dos_vidrios=0'),
        (v_m5, 380,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m5, 390,'ALUMINIO','U13','MATERIAL','LARGO','max_alto-2-anclaje',NULL,NULL,NULL,'dos_vidrios=0'),
        (v_m5, 400,'ACCESORIO','SILICONA','SILICONA','CANTIDAD',NULL,'2',NULL,NULL,NULL);

        INSERT INTO modelo_ventana_validacion (id_modelo, condicion, mensaje) VALUES
        (v_m5, 'max_alto < 90', 'Este modelo lleva alero: para alturas menores a 90 cm use un modelo sin alero.'),
        (v_m5, 'tres_alto=1 AND (max_alto-min_alto)>5', 'Con 3 alturas distintas, la diferencia maxima permitida es 5 cm.');
    END IF;
END $$;

-- Bases ya creadas: el alero estaba fijo en 35 dentro de las formulas
UPDATE modelo_ventana_variable
SET formula = replace(formula, '35', 'alero')
WHERE nombre IN ('anclaje', 'alto_fijo') AND formula LIKE '%35%';

-- ============================================================
-- [REPARACION] 1) Enlace compra -> pedido para datos EXISTENTES.
--    Idempotente: solo toca lineas COMPRA_NUEVA sin pieza y piezas
--    DISPONIBLES de origen COMPRA. Empareja variante + medidas
--    normalizando NULL como 0.
-- ============================================================
DROP TABLE IF EXISTS _enlazadas;
CREATE TEMP TABLE _enlazadas (id_pieza integer, id_item integer);

WITH pendientes AS (
    SELECT pd.id_detalle, pd.id_item, pd.id_variante, pd.ancho_cm, pd.alto_cm, pd.largo_cm,
           row_number() OVER (PARTITION BY pd.id_variante, COALESCE(pd.ancho_cm,0),
                              COALESCE(pd.alto_cm,0), COALESCE(pd.largo_cm,0)
                              ORDER BY pd.id_detalle) AS rn
    FROM pedido_detalle pd
    WHERE pd.estado='ACTIVO' AND pd.accion='COMPRA_NUEVA' AND pd.id_pieza IS NULL
      AND EXISTS (SELECT 1 FROM pedido_item pi
                  WHERE pi.id_item=pd.id_item AND pi.estado<>'CANCELADO')
), nuevas AS (
    SELECT ip.id_pieza, ip.id_variante, ip.ancho_cm, ip.alto_cm, ip.largo_cm, ip.id_compra_detalle,
           row_number() OVER (PARTITION BY ip.id_variante, COALESCE(ip.ancho_cm,0),
                              COALESCE(ip.alto_cm,0), COALESCE(ip.largo_cm,0)
                              ORDER BY ip.id_pieza) AS rn
    FROM inventario_piezas ip
    WHERE ip.origen='COMPRA' AND ip.estado='DISPONIBLE'
), upd AS (
    UPDATE pedido_detalle pd
    SET id_pieza = n.id_pieza, accion = 'USA_EXISTENTE', id_compra_detalle = n.id_compra_detalle
    FROM pendientes p
    INNER JOIN nuevas n
        ON n.id_variante = p.id_variante AND n.rn = p.rn
       AND COALESCE(n.ancho_cm,0) = COALESCE(p.ancho_cm,0)
       AND COALESCE(n.alto_cm,0)  = COALESCE(p.alto_cm,0)
       AND COALESCE(n.largo_cm,0) = COALESCE(p.largo_cm,0)
    WHERE pd.id_detalle = p.id_detalle
    RETURNING pd.id_pieza, pd.id_item
)
INSERT INTO _enlazadas (id_pieza, id_item)
SELECT id_pieza, id_item FROM upd;

UPDATE inventario_piezas ip
SET estado = 'VENDIDA'
FROM _enlazadas e
WHERE e.id_pieza = ip.id_pieza;

INSERT INTO movimientos_inventario
    (id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion)
SELECT ip.id_variante, e.id_pieza, 'SALIDA', 'VENTA', 1, NULL, pi.id_pedido,
       'Enlace automatico compra-pedido (reparacion)'
FROM _enlazadas e
INNER JOIN inventario_piezas ip ON ip.id_pieza = e.id_pieza
INNER JOIN pedido_item pi       ON pi.id_item = e.id_item;

DO $$
DECLARE
    r record;
BEGIN
    FOR r IN
        SELECT DISTINCT pi.id_pedido
        FROM _enlazadas e
        INNER JOIN pedido_item pi ON pi.id_item = e.id_item
    LOOP
        PERFORM sp_ped_recalcular_estados(r.id_pedido);
    END LOOP;
END $$;

-- ============================================================
-- [REPARACION] 2) Fechas de ingreso con anio fuera de rango.
-- ============================================================
UPDATE inventario_piezas ip
SET fecha_ingreso = COALESCE(c.fecha::timestamp, fn_hora_peru())
FROM compra_detalle cd
INNER JOIN compras c ON c.id_compra = cd.id_compra
WHERE cd.id_compra_detalle = ip.id_compra_detalle
  AND (ip.fecha_ingreso > COALESCE(c.fecha::timestamp, fn_hora_peru()) + interval '2 year'
    OR ip.fecha_ingreso < COALESCE(c.fecha::timestamp, fn_hora_peru()) - interval '5 year');

UPDATE inventario_piezas ip
SET fecha_ingreso = fn_hora_peru()
WHERE NOT EXISTS (SELECT 1 FROM compra_detalle cd
                  WHERE cd.id_compra_detalle = ip.id_compra_detalle)
  AND (ip.fecha_ingreso > fn_hora_peru() + interval '2 year'
    OR ip.fecha_ingreso < fn_hora_peru() - interval '5 year');

UPDATE movimientos_inventario m
SET fecha = COALESCE(c.fecha::timestamp, fn_hora_peru())
FROM compra_detalle cd
INNER JOIN compras c ON c.id_compra = cd.id_compra
WHERE cd.id_compra_detalle = m.id_compra_detalle
  AND (m.fecha > COALESCE(c.fecha::timestamp, fn_hora_peru()) + interval '2 year'
    OR m.fecha < COALESCE(c.fecha::timestamp, fn_hora_peru()) - interval '5 year');

-- ============================================================
-- MODULO PROVEEDORES: migracion de compras historicas
-- ============================================================
INSERT INTO proveedores (nombre)
SELECT DISTINCT COALESCE(NULLIF(btrim(c.proveedor), ''), 'Proveedor histórico sin nombre')
FROM compras c
WHERE c.id_proveedor IS NULL
  AND NOT EXISTS (
      SELECT 1 FROM proveedores p
      WHERE p.nombre = COALESCE(NULLIF(btrim(c.proveedor), ''), 'Proveedor histórico sin nombre'));

UPDATE compras c
SET id_proveedor = (
    SELECT min(p.id_proveedor) FROM proveedores p
    WHERE p.nombre = COALESCE(NULLIF(btrim(c.proveedor), ''), 'Proveedor histórico sin nombre'))
WHERE c.id_proveedor IS NULL;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint
                   WHERE conname = 'fk_compra_proveedor'
                     AND conrelid = 'compras'::regclass) THEN
        ALTER TABLE compras ADD CONSTRAINT fk_compra_proveedor
            FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor);
    END IF;
END $$;

-- ============================================================
-- CATALOGO SIMULADO DE PROVEEDORES
-- Nombres comerciales FICTICIOS. Reemplaza los nombres
-- provisionales conservando sus IDs. Reejecutable.
-- ============================================================
DO $$
DECLARE
    r         record;
    v_id      integer;
    v_version bigint;
BEGIN
    FOR r IN
        SELECT v.anterior, v.nombre, v.direccion
        FROM (VALUES
            ('doce',  'Vidrios y Cristales Horizonte',              'Lima, zona industrial. Almacen de vidrios y espejos.'),
            ('los',   'Distribuidora de Aluminios Costa Central',   'Lima, zona comercial. Almacen de perfiles de aluminio.'),
            ('trece', 'Suministros para Vidrieria Las Palmeras',    'Lima, zona comercial. Distribucion de accesorios y fijaciones.')
        ) AS v(anterior, nombre, direccion)
    LOOP
        IF NOT EXISTS (SELECT 1 FROM proveedores p WHERE p.nombre = r.nombre) THEN
            v_id := NULL;
            v_version := NULL;

            SELECT p.id_proveedor, p.version_proveedor
              INTO v_id, v_version
            FROM proveedores p
            WHERE p.nombre = r.anterior AND p.documento IS NULL
            LIMIT 1;

            IF v_id IS NULL THEN
                PERFORM sp_prov_guardar(
                    p_nombre := r.nombre,
                    p_direccion := r.direccion,
                    p_tipo_persona := 'NATURAL',
                    p_tipo_documento := 'SIN DOCUMENTO');
            ELSE
                PERFORM sp_prov_guardar(
                    p_id_proveedor := v_id,
                    p_nombre := r.nombre,
                    p_direccion := r.direccion,
                    p_version_original := v_version,
                    p_tipo_persona := 'NATURAL',
                    p_tipo_documento := 'SIN DOCUMENTO');
            END IF;
        END IF;
    END LOOP;
END $$;

COMMIT;

-- ============================================================
-- Script unico ejecutado: esquema, modulos, catalogos y
-- proveedores listos (PostgreSQL).
-- ============================================================

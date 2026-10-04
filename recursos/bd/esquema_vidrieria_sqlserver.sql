-- ============================================================
-- Esquema de base de datos: Sistema para vidriería
-- Motor: Microsoft SQL Server (T-SQL)
-- ============================================================

IF DB_ID(N'vidrieria') IS NULL
BEGIN
    CREATE DATABASE vidrieria;
END
GO

USE vidrieria;
GO

-- ------------------------------------------------------------
-- Usuarios y preferencias
-- ------------------------------------------------------------

CREATE TABLE usuarios (
    id_usuario          INT IDENTITY(1,1) PRIMARY KEY,
    nombre              NVARCHAR(150) NOT NULL UNIQUE,
    correo              NVARCHAR(150) NOT NULL UNIQUE,
    contrasena          NVARCHAR(255) NOT NULL,
    pregunta_seguridad  NVARCHAR(255) NOT NULL,
    respuesta           NVARCHAR(255) NOT NULL,  -- hashear igual que la contraseña, no guardar en texto plano
    rol                 NVARCHAR(20) NOT NULL DEFAULT 'VENDEDOR'  -- VENDEDOR = Trabajador, ADMINISTRADOR = Jefe
        CHECK (rol IN ('VENDEDOR','ADMINISTRADOR')),
    activo              BIT NOT NULL DEFAULT 1,
    fecha_creacion      DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);

CREATE TABLE usuario_preferencias (
    id_usuario  INT PRIMARY KEY,
    tema        NVARCHAR(10) NOT NULL DEFAULT 'CLARO'
        CHECK (tema IN ('CLARO','OSCURO')),
    fuente      NVARCHAR(50) NOT NULL DEFAULT 'Arial',
    CONSTRAINT fk_pref_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE CASCADE
);

CREATE TABLE tipos_mensaje (
    id_mensaje   INT IDENTITY(1,1) PRIMARY KEY,
    codigo       NVARCHAR(50)  NOT NULL UNIQUE,
    titulo       NVARCHAR(255) NOT NULL,
    texto  NVARCHAR(255)
);

CREATE TABLE usuario_mensajes_descartados (
    id_usuario      INT NOT NULL,
    id_mensaje      INT NOT NULL,
    fecha_descarte  DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT pk_usuario_mensaje PRIMARY KEY (id_usuario, id_mensaje),
    CONSTRAINT fk_desc_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE CASCADE,
    CONSTRAINT fk_desc_mensaje FOREIGN KEY (id_mensaje)
        REFERENCES tipos_mensaje(id_mensaje) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Clientes
-- ------------------------------------------------------------

CREATE TABLE clientes (
    id_cliente  INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(150) NOT NULL,
    documento   NVARCHAR(20),
    telefono    NVARCHAR(20),
    direccion   NVARCHAR(255)
);

-- ------------------------------------------------------------
-- Catálogo de productos (familia / variante / características)
-- ------------------------------------------------------------

CREATE TABLE productos (
    id_producto  INT IDENTITY(1,1) PRIMARY KEY,
    categoria    NVARCHAR(20) NOT NULL,  -- VIDRIO, ALUMINIO, TORNILLO, TARUGO
    tipo         NVARCHAR(50) NOT NULL,   -- transparente, catedral, espejo, u13, u21, triangulo...
    color        NVARCHAR(30),            -- principalmente aluminio: negro, normal, madera...
    nombre       NVARCHAR(150) NOT NULL,
    descripcion  NVARCHAR(255),
    imagen_url   NVARCHAR(255),
    activo       BIT NOT NULL DEFAULT 1
);

CREATE TABLE caracteristicas (
    id_caracteristica  INT IDENTITY(1,1) PRIMARY KEY,
    espesor_cm         DECIMAL(6,2),  -- vidrio
    medida_cm          DECIMAL(6,2)   -- tornillo / tarugo
);

CREATE TABLE producto_variante (
    id_variante        INT IDENTITY(1,1) PRIMARY KEY,
    id_producto        INT NOT NULL,
    id_caracteristica  INT NOT NULL,
    unidad_costo       NVARCHAR(10) NOT NULL,  -- CM2, CM_LINEAL, CAJA
    precio_unitario    DECIMAL(10,2) NOT NULL,
    CONSTRAINT uq_variante UNIQUE (id_producto, id_caracteristica),
    CONSTRAINT fk_var_producto FOREIGN KEY (id_producto)
        REFERENCES productos(id_producto) ON DELETE NO ACTION,
    CONSTRAINT fk_var_caract FOREIGN KEY (id_caracteristica)
        REFERENCES caracteristicas(id_caracteristica) ON DELETE NO ACTION
);

-- ------------------------------------------------------------
-- Compras (entrada de material)
-- ------------------------------------------------------------

CREATE TABLE compras (
    id_compra        INT IDENTITY(1,1) PRIMARY KEY,
    proveedor        NVARCHAR(150) NOT NULL,
    fecha            DATE NOT NULL,
    total            DECIMAL(10,2) NOT NULL DEFAULT 0,
    motivo           NVARCHAR(20) NOT NULL DEFAULT 'REPOSICION'
        CHECK (motivo IN ('REPOSICION','REEMPLAZO_DEFECTO')),
    id_pedido_origen INT NULL   -- si motivo = REEMPLAZO_DEFECTO: qué pedido generó la necesidad de comprar
);

CREATE TABLE compra_detalle (
    id_compra_detalle  INT IDENTITY(1,1) PRIMARY KEY,
    id_compra          INT NOT NULL,
    id_variante        INT NOT NULL,
    cantidad           DECIMAL(10,2) NOT NULL,
    costo_unitario     DECIMAL(10,2) NOT NULL,  -- snapshot del costo al momento de comprar
    subtotal           DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_cdet_compra FOREIGN KEY (id_compra)
        REFERENCES compras(id_compra) ON DELETE CASCADE,
    CONSTRAINT fk_cdet_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION
);

-- ------------------------------------------------------------
-- Inventario: piezas (vidrio/aluminio) y cajas (tornillo/tarugo)
-- ------------------------------------------------------------

CREATE TABLE inventario_piezas (
    id_pieza           INT IDENTITY(1,1) PRIMARY KEY,
    id_variante        INT NOT NULL,
    id_pieza_origen    INT NULL,               -- referencia a la pieza de la que salió (retazo)
    id_compra_detalle  INT NULL,               -- de qué compra provino (si origen = COMPRA)
    ancho_cm           DECIMAL(8,2),           -- vidrio
    alto_cm            DECIMAL(8,2),           -- vidrio
    largo_cm           DECIMAL(8,2),           -- aluminio
    estado             NVARCHAR(15) NOT NULL DEFAULT 'DISPONIBLE'
        CHECK (estado IN ('DISPONIBLE','RESERVADA','VENDIDA','CONSUMIDA','DEFECTUOSA')),
    origen             NVARCHAR(10) NOT NULL
        CHECK (origen IN ('COMPRA','RETAZO')),
    fecha_ingreso      DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT fk_pieza_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_origen FOREIGN KEY (id_pieza_origen)
        REFERENCES inventario_piezas(id_pieza) ON DELETE NO ACTION,
    CONSTRAINT fk_pieza_compra_detalle FOREIGN KEY (id_compra_detalle)
        REFERENCES compra_detalle(id_compra_detalle) ON DELETE SET NULL
);

CREATE TABLE inventario_cajas (
    id_variante   INT PRIMARY KEY,
    stock_cajas   INT NOT NULL DEFAULT 0,
    CONSTRAINT fk_cajas_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE CASCADE
);

-- ------------------------------------------------------------
-- Pedidos y su detalle
-- ------------------------------------------------------------

CREATE TABLE pedidos (
    id_pedido   INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente  INT NOT NULL,
    id_usuario  INT NOT NULL,
    fecha       DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    estado      NVARCHAR(15) NOT NULL DEFAULT 'PENDIENTE'
        CHECK (estado IN ('PENDIENTE','PAGADO','PARCIAL','CANCELADO')),
    total       DECIMAL(10,2) NOT NULL DEFAULT 0,
    CONSTRAINT fk_pedido_cliente FOREIGN KEY (id_cliente)
        REFERENCES clientes(id_cliente) ON DELETE NO ACTION,
    CONSTRAINT fk_pedido_usuario FOREIGN KEY (id_usuario)
        REFERENCES usuarios(id_usuario) ON DELETE NO ACTION
);

CREATE TABLE pedido_detalle (
    id_detalle                INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido                 INT NOT NULL,
    id_variante               INT NOT NULL,
    id_pieza                  INT NULL,        -- vidrio/aluminio: pieza específica usada
    cantidad_cajas            INT NULL,        -- tornillo/tarugo
    ancho_cm                  DECIMAL(8,2),    -- medida real cortada
    alto_cm                   DECIMAL(8,2),
    largo_cm                  DECIMAL(8,2),
    precio_unitario_aplicado  DECIMAL(10,2) NOT NULL,
    subtotal                  DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_det_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedidos(id_pedido) ON DELETE CASCADE,
    CONSTRAINT fk_det_variante FOREIGN KEY (id_variante)
        REFERENCES producto_variante(id_variante) ON DELETE NO ACTION,
    CONSTRAINT fk_det_pieza FOREIGN KEY (id_pieza)
        REFERENCES inventario_piezas(id_pieza) ON DELETE SET NULL
);

-- ------------------------------------------------------------
-- Pagos, métodos de pago y comprobantes
-- ------------------------------------------------------------

CREATE TABLE metodos_pago (
    id_metodo_pago  INT IDENTITY(1,1) PRIMARY KEY,
    nombre          NVARCHAR(50) NOT NULL UNIQUE  -- efectivo, tarjeta, yape, transferencia...
);

CREATE TABLE pagos (
    id_pago       INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido     INT NOT NULL,
    fecha         DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    monto_total   DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_pago_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedidos(id_pedido) ON DELETE CASCADE
);

CREATE TABLE detalle_pago (
    id_detalle_pago  INT IDENTITY(1,1) PRIMARY KEY,
    id_pago          INT NOT NULL,
    id_metodo_pago   INT NOT NULL,
    monto            DECIMAL(10,2) NOT NULL,
    CONSTRAINT fk_dp_pago FOREIGN KEY (id_pago)
        REFERENCES pagos(id_pago) ON DELETE CASCADE,
    CONSTRAINT fk_dp_metodo FOREIGN KEY (id_metodo_pago)
        REFERENCES metodos_pago(id_metodo_pago) ON DELETE NO ACTION
);

CREATE TABLE comprobante_pago (
    id_comprobante    INT IDENTITY(1,1) PRIMARY KEY,
    id_pago           INT NOT NULL UNIQUE,
    tipo_comprobante  NVARCHAR(10) NOT NULL
        CHECK (tipo_comprobante IN ('BOLETA','FACTURA')),
    serie             NVARCHAR(10) NOT NULL,
    numero            NVARCHAR(20) NOT NULL,
    fecha_emision     DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT fk_comp_pago FOREIGN KEY (id_pago)
        REFERENCES pagos(id_pago) ON DELETE CASCADE
);
GO

-- ------------------------------------------------------------
-- Índices de apoyo para los reportes más frecuentes
-- ------------------------------------------------------------

CREATE INDEX idx_pedidos_fecha ON pedidos(fecha);
CREATE INDEX idx_pedidos_cliente ON pedidos(id_cliente);
CREATE INDEX idx_compras_fecha ON compras(fecha);
CREATE INDEX idx_compras_motivo ON compras(motivo);
CREATE INDEX idx_piezas_variante_estado ON inventario_piezas(id_variante, estado);
CREATE INDEX idx_piezas_medidas ON inventario_piezas(ancho_cm, alto_cm);
GO

-- FK diferida: compras.id_pedido_origen -> pedidos, se agrega al final porque
-- "pedidos" se crea después de "compras" en este script.
ALTER TABLE compras
    ADD CONSTRAINT fk_compra_pedido_origen FOREIGN KEY (id_pedido_origen)
        REFERENCES pedidos(id_pedido) ON DELETE NO ACTION;
GO


INSERT INTO usuarios (nombre, correo, contrasena_hash, pregunta_seguridad, respuesta_hash, rol)
VALUES
('admin',  'admin@vidrieria.com',  '123456', '¿Nombre de tu mascota?',       'firulais', 'ADMINISTRADOR');
GO

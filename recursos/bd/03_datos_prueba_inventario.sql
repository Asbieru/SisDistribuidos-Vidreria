-- ============================================================
-- Datos de PRUEBA para el módulo de inventario
-- Ejecutar DESPUÉS de 02_modulo_inventario.sql
-- Solo inserta si la tabla productos está vacía (no duplica datos)
-- ============================================================

USE vidrieria;
GO

IF EXISTS (SELECT 1 FROM productos)
BEGIN
    PRINT 'Ya hay productos registrados: no se insertan datos de prueba.';
    RETURN;
END

-- Características (espesor del vidrio / medida del tornillo o tarugo)
DECLARE @c_vid4 INT, @c_vid6 INT, @c_alu INT, @c_tor1 INT, @c_tar1 INT;
INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.40, NULL); SET @c_vid4 = SCOPE_IDENTITY();
INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (0.60, NULL); SET @c_vid6 = SCOPE_IDENTITY();
INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, NULL); SET @c_alu  = SCOPE_IDENTITY();
INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 2.54); SET @c_tor1 = SCOPE_IDENTITY();
INSERT INTO caracteristicas (espesor_cm, medida_cm) VALUES (NULL, 0.80); SET @c_tar1 = SCOPE_IDENTITY();

-- Productos
DECLARE @p_trans INT, @p_cated INT, @p_u13 INT, @p_tor INT, @p_tar INT;
INSERT INTO productos (categoria, tipo, color, nombre) VALUES ('VIDRIO',   'transparente', NULL,    'Vidrio transparente'); SET @p_trans = SCOPE_IDENTITY();
INSERT INTO productos (categoria, tipo, color, nombre) VALUES ('VIDRIO',   'catedral',     NULL,    'Vidrio catedral');     SET @p_cated = SCOPE_IDENTITY();
INSERT INTO productos (categoria, tipo, color, nombre) VALUES ('ALUMINIO', 'u13',          'negro', 'Aluminio U13');        SET @p_u13   = SCOPE_IDENTITY();
INSERT INTO productos (categoria, tipo, color, nombre) VALUES ('TORNILLO', 'autorroscante', NULL,  'Tornillo autorroscante'); SET @p_tor = SCOPE_IDENTITY();
INSERT INTO productos (categoria, tipo, color, nombre) VALUES ('TARUGO',   'plastico',     NULL,    'Tarugo plástico');     SET @p_tar   = SCOPE_IDENTITY();

-- Variantes (precio por cm2, por cm lineal o por caja) + stock mínimo
DECLARE @v_trans4 INT, @v_trans6 INT, @v_cated4 INT, @v_u13 INT, @v_tor INT, @v_tar INT;
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_trans, @c_vid4, 'CM2',       0.01,  3); SET @v_trans4 = SCOPE_IDENTITY();
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_trans, @c_vid6, 'CM2',       0.02,  2); SET @v_trans6 = SCOPE_IDENTITY();
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_cated, @c_vid4, 'CM2',       0.015, 2); SET @v_cated4 = SCOPE_IDENTITY();
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_u13,   @c_alu,  'CM_LINEAL', 0.10,  5); SET @v_u13    = SCOPE_IDENTITY();
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_tor,   @c_tor1, 'CAJA',      25.00, 4); SET @v_tor    = SCOPE_IDENTITY();
INSERT INTO producto_variante (id_producto, id_caracteristica, unidad_costo, precio_unitario, stock_minimo) VALUES (@p_tar,   @c_tar1, 'CAJA',      12.50, 4); SET @v_tar    = SCOPE_IDENTITY();

-- Inventario inicial (se registra por los procedimientos para que quede en el kardex)
DECLARE @admin INT = (SELECT TOP 1 id_usuario FROM usuarios WHERE nombre = 'admin');

EXEC sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 180, @alto_cm = 120, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 180, @alto_cm = 120, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_trans4, @ancho_cm = 90,  @alto_cm = 60,  @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_trans6, @ancho_cm = 200, @alto_cm = 150, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_cated4, @ancho_cm = 150, @alto_cm = 100, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 600, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 600, @origen = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_ingresar_pieza @id_variante = @v_u13,    @largo_cm = 350, @origen = 'INICIAL', @id_usuario = @admin;

EXEC sp_inv_mover_cajas @id_variante = @v_tor, @tipo = 'ENTRADA', @cantidad = 10, @motivo = 'INICIAL', @id_usuario = @admin;
EXEC sp_inv_mover_cajas @id_variante = @v_tar, @tipo = 'ENTRADA', @cantidad = 3,  @motivo = 'INICIAL', @id_usuario = @admin;  -- queda bajo el mínimo (4)

PRINT 'Datos de prueba de inventario insertados.';
GO

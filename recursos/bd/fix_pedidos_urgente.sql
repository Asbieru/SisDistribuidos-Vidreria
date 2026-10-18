-- ============================================================
-- fix_pedidos_urgente.sql
-- Recrea TVPs + SPs de pedidos con el ESQUEMA CORRECTO del 00
-- ============================================================
IF OBJECT_ID(N'dbo.sp_ped_registrar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ped_registrar;
IF OBJECT_ID(N'dbo.sp_ped_terminar_item', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ped_terminar_item;
IF OBJECT_ID(N'dbo.sp_ped_recalcular_estados', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ped_recalcular_estados;
IF OBJECT_ID(N'dbo.sp_ped_terminar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_ped_terminar;

IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoItem' AND is_table_type = 1) DROP TYPE dbo.TVP_PedidoItem;
IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoDetalle' AND is_table_type = 1) DROP TYPE dbo.TVP_PedidoDetalle;
IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoMedida' AND is_table_type = 1) DROP TYPE dbo.TVP_PedidoMedida;
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoItem' AND is_table_type = 1)
CREATE TYPE dbo.TVP_PedidoItem AS TABLE (
    id_temp            INT NOT NULL PRIMARY KEY,
    tipo                NVARCHAR(20) NOT NULL,
    etiqueta            NVARCHAR(50) NULL,
    ancho_cm            DECIMAL(10,2) NULL,
    alto_cm             DECIMAL(10,2) NULL,
    precio_materiales   DECIMAL(10,2) NULL,
    costo_extra         DECIMAL(10,2) NOT NULL,
    mano_obra           DECIMAL(10,2) NOT NULL,
    total               DECIMAL(10,2) NULL,
    id_modelo           INT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'TVP_PedidoDetalle' AND is_table_type = 1)
CREATE TYPE dbo.TVP_PedidoDetalle AS TABLE (
    id_temp         INT NOT NULL PRIMARY KEY,
    id_item_temp    INT NOT NULL,
    id_variante     INT NOT NULL,
    accion          NVARCHAR(20) NOT NULL,
    id_pieza        INT NULL,
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
    etiqueta        NVARCHAR(20) NOT NULL,
    valor_cm        DECIMAL(10,2) NOT NULL
);
GO
CREATE PROCEDURE dbo.sp_ped_recalcular_estados
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

CREATE PROCEDURE dbo.sp_ped_registrar
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
CREATE PROCEDURE dbo.sp_ped_terminar_item
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
CREATE PROCEDURE dbo.sp_ped_terminar
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
PRINT 'OK';
GO

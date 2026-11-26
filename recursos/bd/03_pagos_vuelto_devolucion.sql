/* ============================================================
   03_pagos_vuelto_devolucion.sql

   Movimientos de caja en dbo.pagos:
     PAGO        dinero que ingresa (monto_total > 0)
     VUELTO      dinero que se devuelve como cambio (monto_total < 0)
     DEVOLUCION  dinero devuelto al cliente (monto_total < 0)

   El vuelto queda enlazado con el pago que lo origino mediante
   dbo.pagos.id_pago_padre, para que al editar o eliminar el pago
   se pueda ajustar o borrar automaticamente su vuelto.

   El script es idempotente: se puede ejecutar varias veces.
   ============================================================ */
SET NOCOUNT ON;
GO

/* ------------------------------------------------------------
   1) Columna tipo
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.pagos', 'tipo') IS NULL
BEGIN
    ALTER TABLE dbo.pagos ADD tipo NVARCHAR(12) NOT NULL
        CONSTRAINT df_pagos_tipo DEFAULT N'PAGO';
END
GO

IF NOT EXISTS (SELECT 1
               FROM sys.check_constraints
               WHERE name = 'ck_pagos_tipo'
                 AND parent_object_id = OBJECT_ID('dbo.pagos'))
BEGIN
    ALTER TABLE dbo.pagos ADD CONSTRAINT ck_pagos_tipo
        CHECK (tipo IN (N'PAGO', N'VUELTO', N'DEVOLUCION'));
END
GO

/* ------------------------------------------------------------
   2) Referencia del vuelto hacia su pago padre
   ------------------------------------------------------------ */
IF COL_LENGTH('dbo.pagos', 'id_pago_padre') IS NULL
BEGIN
    ALTER TABLE dbo.pagos ADD id_pago_padre INT NULL;
END
GO

IF NOT EXISTS (SELECT 1
               FROM sys.foreign_keys
               WHERE name = 'fk_pago_padre'
                 AND parent_object_id = OBJECT_ID('dbo.pagos'))
BEGIN
    ALTER TABLE dbo.pagos ADD CONSTRAINT fk_pago_padre
        FOREIGN KEY (id_pago_padre) REFERENCES dbo.pagos(id_pago);
END
GO

IF NOT EXISTS (SELECT 1
               FROM sys.indexes
               WHERE name = 'ix_pagos_padre'
                 AND object_id = OBJECT_ID('dbo.pagos'))
BEGIN
    CREATE INDEX ix_pagos_padre ON dbo.pagos(id_pago_padre);
END
GO

/* ------------------------------------------------------------
   3) Datos existentes

   Hasta ahora el vuelto se guardaba como una cabecera negativa
   sin relacion explicita. Se marca como VUELTO y se enlaza con
   el pago positivo inmediatamente anterior del mismo pedido.
   ------------------------------------------------------------ */
UPDATE dbo.pagos
SET tipo = N'VUELTO'
WHERE monto_total < 0
  AND tipo = N'PAGO';
GO

UPDATE v
SET id_pago_padre = (
        SELECT MAX(p.id_pago)
        FROM dbo.pagos p
        WHERE p.id_pedido = v.id_pedido
          AND p.id_pago < v.id_pago
          AND p.tipo = N'PAGO'
          AND p.monto_total > 0)
FROM dbo.pagos v
WHERE v.tipo = N'VUELTO'
  AND v.id_pago_padre IS NULL;
GO

/* ------------------------------------------------------------
   4) Etiqueta los movimientos en el historial de pagos del cliente
   ------------------------------------------------------------ */
IF OBJECT_ID(N'dbo.sp_cli_pagos_pedido', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_cli_pagos_pedido;
GO
CREATE PROCEDURE dbo.sp_cli_pagos_pedido
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
           CASE p.tipo
                WHEN N'VUELTO' THEN COALESCE(m.nombre, N'Sin desglose') + N' / vuelto'
                WHEN N'DEVOLUCION' THEN COALESCE(m.nombre, N'Sin desglose') + N' / devolución'
                ELSE COALESCE(m.nombre, N'Sin desglose') END AS metodo,
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

/* ------------------------------------------------------------
   5) Verificacion
   ------------------------------------------------------------ */
SELECT
    SUM(CASE WHEN tipo = N'PAGO' THEN 1 ELSE 0 END)       AS pagos,
    SUM(CASE WHEN tipo = N'VUELTO' THEN 1 ELSE 0 END)     AS vueltos,
    SUM(CASE WHEN tipo = N'DEVOLUCION' THEN 1 ELSE 0 END) AS devoluciones,
    SUM(CASE WHEN id_pago_padre IS NULL THEN 0 ELSE 1 END) AS con_padre
FROM dbo.pagos;
GO

-- Ampliacion incremental de Clientes. Ejecutar despues de 04_modulo_clientes.sql.
-- Conserva los registros, las relaciones y las reglas de documento del equipo.
USE vidrieria;
GO

IF COL_LENGTH(N'dbo.clientes', N'version_cliente') IS NULL
    ALTER TABLE dbo.clientes ADD version_cliente ROWVERSION NOT NULL;
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

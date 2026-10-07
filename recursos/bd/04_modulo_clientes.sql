-- Mantenimiento y seleccion de Clientes.
-- Ejecutar en la misma instancia que usa la aplicacion.
-- Requiere el esquema base; no modifica tablas ni datos existentes.
-- SQL Server 2016 SP1 o superior, como 02_modulo_inventario.sql.
USE vidrieria;
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
    SELECT id_cliente, nombre, documento, telefono, direccion
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
    SELECT id_cliente, nombre, documento, telefono, direccion
    FROM dbo.clientes WHERE id_cliente = @id_cliente;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_cli_guardar
    @id_cliente INT = 0,
    @nombre NVARCHAR(MAX),
    @documento NVARCHAR(MAX) = NULL,
    @telefono NVARCHAR(MAX) = NULL,
    @direccion NVARCHAR(MAX) = NULL
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

    -- No se impone documento unico: el esquema actual no define esa regla.
    BEGIN TRY
        BEGIN TRANSACTION;
        IF @id_cliente > 0 AND NOT EXISTS (
            SELECT 1 FROM dbo.clientes WITH (UPDLOCK, HOLDLOCK)
            WHERE id_cliente = @id_cliente)
            THROW 50107, 'El cliente ya no existe. Actualice el listado.', 1;

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

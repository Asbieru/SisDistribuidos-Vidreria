-- Proveedores: ejecutar en la misma BD de la aplicación, antes de usar el módulo.
-- No borra compras ni cambia sus nombres históricos. Reejecutable.
SET XACT_ABORT ON;
GO
IF OBJECT_ID(N'dbo.proveedores',N'U') IS NULL
CREATE TABLE dbo.proveedores (
    id_proveedor INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(150) NOT NULL,
    documento NVARCHAR(20) NULL,
    telefono NVARCHAR(20) NULL,
    direccion NVARCHAR(255) NULL,
    version_proveedor ROWVERSION NOT NULL,
    tipo_persona NVARCHAR(10) NULL,
    tipo_documento NVARCHAR(15) NULL
);
GO
IF COL_LENGTH('dbo.compras','id_proveedor') IS NULL
    ALTER TABLE dbo.compras ADD id_proveedor INT NULL;
GO
BEGIN TRY
    BEGIN TRANSACTION;
    -- Se agrupan únicamente nombres idénticos (tras quitar espacios externos).
    -- Variantes ortográficas quedan separadas para no fusionar proveedores distintos.
    INSERT INTO dbo.proveedores(nombre)
    SELECT DISTINCT COALESCE(NULLIF(LTRIM(RTRIM(c.proveedor)),N''),N'Proveedor histórico sin nombre') COLLATE Latin1_General_100_BIN2
    FROM dbo.compras c
    WHERE c.id_proveedor IS NULL AND NOT EXISTS (
        SELECT 1 FROM dbo.proveedores p WITH (UPDLOCK,HOLDLOCK)
        WHERE p.nombre COLLATE Latin1_General_100_BIN2 =
            COALESCE(NULLIF(LTRIM(RTRIM(c.proveedor)),N''),N'Proveedor histórico sin nombre') COLLATE Latin1_General_100_BIN2);
    UPDATE c SET id_proveedor = p.id_proveedor
    FROM dbo.compras c
    CROSS APPLY (SELECT MIN(id_proveedor) AS id_proveedor FROM dbo.proveedores p
        WHERE p.nombre COLLATE Latin1_General_100_BIN2 =
            COALESCE(NULLIF(LTRIM(RTRIM(c.proveedor)),N''),N'Proveedor histórico sin nombre') COLLATE Latin1_General_100_BIN2) p
    WHERE c.id_proveedor IS NULL;
    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'fk_compra_proveedor' AND parent_object_id=OBJECT_ID(N'dbo.compras'))
        ALTER TABLE dbo.compras WITH CHECK ADD CONSTRAINT fk_compra_proveedor
            FOREIGN KEY(id_proveedor) REFERENCES dbo.proveedores(id_proveedor);
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_compras_proveedor' AND object_id=OBJECT_ID(N'dbo.compras'))
        CREATE INDEX IX_compras_proveedor ON dbo.compras(id_proveedor);
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
-- INICIO ACTUALIZACION PROVEEDORES
IF COL_LENGTH('dbo.proveedores','version_proveedor') IS NULL
    ALTER TABLE dbo.proveedores ADD version_proveedor ROWVERSION NOT NULL;
IF COL_LENGTH('dbo.proveedores','tipo_persona') IS NULL
    ALTER TABLE dbo.proveedores ADD tipo_persona NVARCHAR(10) NULL;
IF COL_LENGTH('dbo.proveedores','tipo_documento') IS NULL
    ALTER TABLE dbo.proveedores ADD tipo_documento NVARCHAR(15) NULL;
GO

-- Los registros anteriores quedan sin clasificar y mantienen sus datos.
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.proveedores') AND name = 'CK_proveedores_identificacion')
    ALTER TABLE dbo.proveedores WITH CHECK ADD CONSTRAINT CK_proveedores_identificacion CHECK (
        (tipo_persona IS NULL AND tipo_documento IS NULL)
        OR (tipo_persona IS NOT NULL AND tipo_documento IS NOT NULL AND (
            (tipo_persona = N'NATURAL' AND tipo_documento IN (N'DNI',N'RUC',N'SIN DOCUMENTO'))
            OR (tipo_persona = N'JURIDICA' AND tipo_documento = N'RUC'))));
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.proveedores') AND name = 'CK_proveedores_documento')
    ALTER TABLE dbo.proveedores WITH CHECK ADD CONSTRAINT CK_proveedores_documento CHECK (
        tipo_documento IS NULL
        OR (tipo_documento = N'SIN DOCUMENTO' AND documento IS NULL)
        OR (tipo_documento IN (N'DNI',N'RUC') AND documento IS NOT NULL
            AND documento COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9]%'
            AND DATALENGTH(documento) = CASE WHEN tipo_documento = N'DNI' THEN 16 ELSE 22 END
            AND (tipo_documento <> N'RUC'
                OR (tipo_persona = N'JURIDICA' AND LEFT(documento,2) = N'20')
                OR (tipo_persona = N'NATURAL' AND LEFT(documento,2) IN (N'10',N'15')))));
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.proveedores') AND name = 'UX_proveedores_documento')
    CREATE UNIQUE INDEX UX_proveedores_documento ON dbo.proveedores(tipo_documento,documento)
    WHERE tipo_documento IS NOT NULL AND documento IS NOT NULL;
GO
IF OBJECT_ID(N'dbo.sp_prov_listar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_listar;
GO


CREATE PROCEDURE dbo.sp_prov_listar
    @texto NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @texto = NULLIF(LTRIM(RTRIM(@texto)), N'');
    DECLARE @patron NVARCHAR(604);
    SET @patron = N'%' + REPLACE(REPLACE(REPLACE(REPLACE(
        @texto, N'\', N'\\'), N'%', N'\%'), N'_', N'\_'), N'[', N'\[') + N'%';
    SELECT id_proveedor, nombre, documento, telefono, direccion, version_proveedor,
           CASE tipo_persona WHEN N'NATURAL' THEN N'Natural' WHEN N'JURIDICA' THEN N'Jurídica' ELSE N'Por clasificar' END AS tipo_persona,
           COALESCE(tipo_documento,N'Por clasificar') AS tipo_documento
    FROM dbo.proveedores
    WHERE @texto IS NULL
       OR nombre LIKE @patron ESCAPE N'\'
       OR documento LIKE @patron ESCAPE N'\'
       OR telefono LIKE @patron ESCAPE N'\'
    ORDER BY nombre, id_proveedor;
END;
GO
IF OBJECT_ID(N'dbo.sp_prov_obtener', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_obtener;
GO


CREATE PROCEDURE dbo.sp_prov_obtener
    @id_proveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_proveedor IS NULL OR @id_proveedor <= 0
        THROW 50101, 'Seleccione un proveedor valido.', 1;
    SELECT id_proveedor, nombre, documento, telefono, direccion, version_proveedor,
           tipo_persona, tipo_documento
    FROM dbo.proveedores WHERE id_proveedor = @id_proveedor;
END;
GO
IF OBJECT_ID(N'dbo.sp_prov_guardar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_guardar;
GO


CREATE PROCEDURE dbo.sp_prov_guardar
    @id_proveedor INT = 0,
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
    IF @id_proveedor IS NULL OR @id_proveedor < 0
        THROW 50101, 'El ID del proveedor no es valido.', 1;
    IF @nombre IS NULL
        THROW 50102, 'Ingrese el nombre del proveedor.', 1;
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
    IF @id_proveedor > 0 AND (@version_original IS NULL OR DATALENGTH(@version_original) <> 8)
        THROW 50109, 'Recargue el proveedor antes de guardar sus cambios.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;
        IF @id_proveedor > 0
        BEGIN
            DECLARE @version_actual BINARY(8);
            SELECT @version_actual = version_proveedor
            FROM dbo.proveedores WITH (UPDLOCK, HOLDLOCK)
            WHERE id_proveedor = @id_proveedor;
            IF @version_actual IS NULL
                THROW 50107, 'El proveedor ya no existe. Actualice el listado.', 1;
            IF @version_actual <> @version_original
                THROW 50109, 'Otro trabajador modifico este proveedor. Presione Recargar y revise los datos antes de guardar.', 1;
            IF @tipo_persona IS NULL AND @tipo_documento IS NULL
                SELECT @tipo_persona = tipo_persona, @tipo_documento = tipo_documento
                FROM dbo.proveedores WHERE id_proveedor = @id_proveedor;
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
            SELECT 1 FROM dbo.proveedores WITH (UPDLOCK,HOLDLOCK)
            WHERE LTRIM(RTRIM(documento)) = @documento AND id_proveedor <> @id_proveedor
              AND (@tipo_documento IS NULL OR tipo_documento IS NULL OR tipo_documento = @tipo_documento))
            THROW 50115, 'El documento ya pertenece a otro proveedor. Busque el registro existente.', 1;

        IF @id_proveedor = 0
        BEGIN
            INSERT INTO dbo.proveedores (nombre, documento, telefono, direccion, tipo_persona, tipo_documento)
            VALUES (@nombre, @documento, @telefono, @direccion, @tipo_persona, @tipo_documento);
            SET @id_proveedor = CONVERT(INT, SCOPE_IDENTITY());
        END
        ELSE
            UPDATE dbo.proveedores
            SET nombre = @nombre, documento = @documento,
                telefono = @telefono, direccion = @direccion,
                tipo_persona = @tipo_persona, tipo_documento = @tipo_documento
            WHERE id_proveedor = @id_proveedor;
        COMMIT TRANSACTION;
        SELECT @id_proveedor AS id_proveedor;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        IF ERROR_NUMBER() IN (2601,2627)
            THROW 50115, 'El documento ya pertenece a otro proveedor. Busque el registro existente.', 1;
        THROW;
    END CATCH;
END;
GO
IF OBJECT_ID(N'dbo.sp_prov_tiene_compras', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_tiene_compras;
GO


CREATE PROCEDURE dbo.sp_prov_tiene_compras
    @id_proveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    IF @id_proveedor IS NULL OR @id_proveedor <= 0
        THROW 50101, 'Seleccione un proveedor valido.', 1;
    SELECT CONVERT(BIT, CASE WHEN EXISTS (
        SELECT 1 FROM dbo.compras WHERE id_proveedor = @id_proveedor)
        THEN 1 ELSE 0 END) AS tiene_compras;
END;
GO
IF OBJECT_ID(N'dbo.sp_prov_eliminar', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_eliminar;
GO


CREATE PROCEDURE dbo.sp_prov_eliminar
    @id_proveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF @id_proveedor IS NULL OR @id_proveedor <= 0
        THROW 50101, 'Seleccione un proveedor valido.', 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (
            SELECT 1 FROM dbo.proveedores WITH (XLOCK, HOLDLOCK)
            WHERE id_proveedor = @id_proveedor)
            THROW 50107, 'El proveedor ya no existe. Actualice el listado.', 1;
        IF EXISTS (
            SELECT 1 FROM dbo.compras WITH (UPDLOCK, HOLDLOCK)
            WHERE id_proveedor = @id_proveedor)
            THROW 50108, 'El proveedor tiene compras y no se puede eliminar. Su historial debe conservarse.', 1;
        DELETE FROM dbo.proveedores WHERE id_proveedor = @id_proveedor;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
IF OBJECT_ID(N'dbo.sp_prov_documento_coincidente', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_prov_documento_coincidente;
GO


CREATE PROCEDURE dbo.sp_prov_documento_coincidente
    @documento NVARCHAR(20),
    @id_excluir INT = 0,
    @tipo_documento NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @documento = NULLIF(LTRIM(RTRIM(@documento)), N'');
    SET @tipo_documento = UPPER(NULLIF(LTRIM(RTRIM(@tipo_documento)), N''));
    IF @id_excluir IS NULL OR @id_excluir < 0
        THROW 50101, 'El ID del proveedor no es valido.', 1;
    SELECT id_proveedor, nombre
    FROM dbo.proveedores
    WHERE @documento IS NOT NULL AND LTRIM(RTRIM(documento)) = @documento
      AND id_proveedor <> @id_excluir
      AND (@tipo_documento IS NULL OR tipo_documento IS NULL OR tipo_documento = @tipo_documento)
    ORDER BY nombre, id_proveedor;
END;
GO

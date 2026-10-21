-- Catálogo de demostración con nombres comerciales FICTICIOS.
-- No representa empresas verificadas ni incluye RUC/DNI/teléfonos inventados.
-- Solo reemplaza los nombres provisionales indicados, conservando sus IDs.
-- El nombre histórico almacenado en cada compra no se modifica.
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @catalogo TABLE(anterior NVARCHAR(150), nombre NVARCHAR(150), direccion NVARCHAR(255));
    INSERT INTO @catalogo VALUES
        (N'doce', N'Vidrios y Cristales Horizonte', N'Lima, zona industrial. Almacén de vidrios y espejos.'),
        (N'los', N'Distribuidora de Aluminios Costa Central', N'Lima, zona comercial. Almacén de perfiles de aluminio.'),
        (N'trece', N'Suministros para Vidriería Las Palmeras', N'Lima, zona comercial. Distribución de accesorios y fijaciones.');
    DECLARE @anterior NVARCHAR(150), @nombre NVARCHAR(150), @direccion NVARCHAR(255), @id INT, @version BINARY(8);
    DECLARE catalogo CURSOR LOCAL FAST_FORWARD FOR SELECT anterior,nombre,direccion FROM @catalogo;
    OPEN catalogo;
    FETCH NEXT FROM catalogo INTO @anterior,@nombre,@direccion;
    WHILE @@FETCH_STATUS=0
    BEGIN
        -- Si ya se aplicó el catálogo, no se vuelve a insertar ni se sobrescriben cambios posteriores.
        IF NOT EXISTS(SELECT 1 FROM dbo.proveedores WITH (UPDLOCK,HOLDLOCK) WHERE nombre=@nombre)
        BEGIN
            SET @id=NULL;
            SET @version=NULL;
            SELECT @id=id_proveedor,@version=version_proveedor
            FROM dbo.proveedores WITH (UPDLOCK,HOLDLOCK)
            WHERE nombre COLLATE Latin1_General_100_BIN2=@anterior COLLATE Latin1_General_100_BIN2
              AND documento IS NULL;
            IF @id IS NULL
                EXEC dbo.sp_prov_guardar @nombre=@nombre,@direccion=@direccion,
                    @tipo_persona=N'NATURAL',@tipo_documento=N'SIN DOCUMENTO';
            ELSE
                EXEC dbo.sp_prov_guardar @id_proveedor=@id,@nombre=@nombre,@direccion=@direccion,
                    @version_original=@version,@tipo_persona=N'NATURAL',@tipo_documento=N'SIN DOCUMENTO';
        END;
        FETCH NEXT FROM catalogo INTO @anterior,@nombre,@direccion;
    END;
    CLOSE catalogo;
    DEALLOCATE catalogo;
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE()<>0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

param([switch]$Aplicar)
$ErrorActionPreference='Stop'
$raiz=Split-Path $PSScriptRoot -Parent
Add-Type -AssemblyName System.Data
[void][Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaDatos.dll'))
$configuracion=New-Object capaDatos.clsConectaBD
$conexion=[Data.SqlClient.SqlConnection]::new($configuracion.miConexion.ConnectionString)
try {
    $conexion.Open()
    $cmd=$conexion.CreateCommand()
    $cmd.CommandTimeout=60
    $cmd.CommandText=@'
SET XACT_ABORT ON;
BEGIN TRANSACTION;
SELECT id_compra,id_proveedor,proveedor,total INTO #compras_catalogo FROM dbo.compras;
'@
    [void]$cmd.ExecuteNonQuery()
    $cmd.CommandText=@'
BEGIN TRY
    DECLARE @catalogo TABLE(anterior NVARCHAR(150), nombre NVARCHAR(150), direccion NVARCHAR(255));
    INSERT INTO @catalogo VALUES
        (N'doce', N'Vidrios y Cristales Horizonte', N'Lima, zona industrial. Almacen de vidrios y espejos.'),
        (N'los', N'Distribuidora de Aluminios Costa Central', N'Lima, zona comercial. Almacen de perfiles de aluminio.'),
        (N'trece', N'Suministros para Vidrieria Las Palmeras', N'Lima, zona comercial. Distribucion de accesorios y fijaciones.');
    DECLARE @anterior NVARCHAR(150), @nombre NVARCHAR(150), @direccion NVARCHAR(255), @id INT, @version BINARY(8);
    DECLARE catalogo CURSOR LOCAL FAST_FORWARD FOR SELECT anterior,nombre,direccion FROM @catalogo;
    OPEN catalogo;
    FETCH NEXT FROM catalogo INTO @anterior,@nombre,@direccion;
    WHILE @@FETCH_STATUS=0
    BEGIN
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
END TRY
BEGIN CATCH
    THROW;
END CATCH;
'@
    [void]$cmd.ExecuteNonQuery()
    $cmd.CommandText=@'
IF EXISTS(SELECT id_compra,id_proveedor,proveedor,total FROM #compras_catalogo
    EXCEPT SELECT id_compra,id_proveedor,proveedor,total FROM dbo.compras)
    THROW 50240, 'Se modificó el historial de compras al cargar el catálogo.',1;
SELECT id_proveedor,nombre,tipo_documento FROM dbo.proveedores ORDER BY nombre;
'@
    $datos=[Data.DataTable]::new()
    $adaptador=[Data.SqlClient.SqlDataAdapter]::new($cmd)
    [void]$adaptador.Fill($datos)
    $cmd.CommandText=if($Aplicar){'COMMIT TRANSACTION;'}else{'ROLLBACK TRANSACTION;'}
    [void]$cmd.ExecuteNonQuery()
    Write-Output $(if($Aplicar){'Catálogo simulado aplicado; IDs e historial conservados.'}else{'Catálogo validado y revertido; use -Aplicar para confirmar.'})
    $datos | Select-Object id_proveedor,nombre,tipo_documento | Format-Table -AutoSize
} catch {
    if($conexion.State -eq 'Open') {
        $cmd.CommandText='IF @@TRANCOUNT>0 ROLLBACK TRANSACTION;'
        try { [void]$cmd.ExecuteNonQuery() } catch { }
    }
    throw
} finally { $conexion.Dispose() }

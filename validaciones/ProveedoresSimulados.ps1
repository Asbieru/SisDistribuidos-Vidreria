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
    $cmd.CommandText=[IO.File]::ReadAllText((Join-Path $raiz 'recursos/bd/02_proveedores_simulados.sql'))
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

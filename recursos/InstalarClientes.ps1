#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]$Servidor = '(localdb)\MSSQLLocalDB',
    [string]$BaseDatos = 'vidrieria'
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data
$conexionClientes = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
$conexionClientes['Data Source'] = $Servidor
$conexionClientes['Initial Catalog'] = $BaseDatos
$conexionClientes['Integrated Security'] = $true
$conexionClientes['Connect Timeout'] = 15
$cnClientes = New-Object System.Data.SqlClient.SqlConnection $conexionClientes.ConnectionString
try {
    $cnClientes.Open()
    $comandoClientes = $cnClientes.CreateCommand()
    $comandoClientes.CommandText = "SELECT COUNT(*) FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo') AND name IN ('clientes','pedidos','usuarios','pagos','detalle_pago','metodos_pago','comprobante_pago');"
    if ([int]$comandoClientes.ExecuteScalar() -ne 7) {
        throw 'La base no contiene el esquema comercial requerido. Instale primero el esquema base del equipo.'
    }
    $nombreSqlClientes = '[' + $BaseDatos.Replace(']', ']]') + ']'
    foreach ($archivoClientes in @('04_modulo_clientes.sql', '05_ficha_comercial_clientes.sql')) {
        $rutaSqlClientes = Join-Path $PSScriptRoot ('bd/' + $archivoClientes)
        $sqlClientes = [IO.File]::ReadAllText($rutaSqlClientes)
        $sqlClientes = [regex]::Replace($sqlClientes, '(?im)^\s*USE\s+vidrieria\s*;\s*$', {
            param($coincidenciaCatalogoClientes)
            'USE ' + $nombreSqlClientes + ';'
        })
        foreach ($loteClientes in [regex]::Split($sqlClientes, '(?im)^\s*GO\s*$')) {
            if ([string]::IsNullOrWhiteSpace($loteClientes)) { continue }
            $comandoClientes.CommandText = $loteClientes
            $comandoClientes.CommandTimeout = 60
            [void]$comandoClientes.ExecuteNonQuery()
        }
        Write-Output ('Aplicado: ' + $archivoClientes)
    }
    Write-Output ('Clientes instalado en ' + $Servidor + ' / ' + $BaseDatos + '. Los datos existentes se conservaron.')
}
finally {
    if ($null -ne $comandoClientes) { $comandoClientes.Dispose() }
    $cnClientes.Dispose()
}

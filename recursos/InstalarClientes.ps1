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
    $rutaSqlClientes = Join-Path $PSScriptRoot 'bd/00_vidrieria_completo.sql'
    $sqlClientes = [IO.File]::ReadAllText($rutaSqlClientes)
    $procedimientosClientes = [regex]::Matches($sqlClientes,
        '(?ims)^CREATE OR ALTER PROCEDURE\s+dbo\.(sp_cli_\w+)\b.*?(?=^\s*GO\s*$)')
    $esperadosClientes = @('sp_cli_listar','sp_cli_obtener','sp_cli_guardar',
        'sp_cli_tiene_pedidos','sp_cli_eliminar','sp_cli_documento_coincidente',
        'sp_cli_historial','sp_cli_pagos_pedido')
    $encontradosClientes = @($procedimientosClientes | ForEach-Object { $_.Groups[1].Value })
    if ($procedimientosClientes.Count -ne 8 -or
        @(Compare-Object $esperadosClientes $encontradosClientes).Count -ne 0) {
        throw 'El script consolidado no contiene los ocho procedimientos esperados de Clientes.'
    }
    $actualizacionClientes = [regex]::Match($sqlClientes,
        '(?s)-- INICIO ACTUALIZACION CLIENTES\s*(.*?)-- FIN ACTUALIZACION CLIENTES')
    if (-not $actualizacionClientes.Success) { throw 'No se encontró la actualización de Clientes.' }
    $lotesClientes = [regex]::Split($actualizacionClientes.Groups[1].Value, '(?im)^\s*GO\s*$')
    $comandoClientes.CommandTimeout = 60
    $transaccionClientes = $cnClientes.BeginTransaction()
    $comandoClientes.Transaction = $transaccionClientes
    try {
        foreach ($loteClientes in $lotesClientes) {
            if ([string]::IsNullOrWhiteSpace($loteClientes)) { continue }
            $comandoClientes.CommandText = $loteClientes
            [void]$comandoClientes.ExecuteNonQuery()
        }
        $transaccionClientes.Commit()
    }
    catch {
        $transaccionClientes.Rollback()
        throw
    }
    finally {
        $transaccionClientes.Dispose()
    }
    Write-Output ('Clientes instalado en ' + $Servidor + ' / ' + $BaseDatos + '. Los datos existentes se conservaron.')
}
finally {
    if ($null -ne $comandoClientes) { $comandoClientes.Dispose() }
    $cnClientes.Dispose()
}

#Requires -Version 5.1
[CmdletBinding()]
param(
    [string]$Servidor = '(localdb)\MSSQLLocalDB',
    [string]$MSBuild = '',
    [switch]$ConservarResultados
)

$ErrorActionPreference = 'Stop'
if ($PSVersionTable.PSEdition -eq 'Core' -or [Threading.Thread]::CurrentThread.ApartmentState -ne 'STA') {
    throw 'Use Windows PowerShell: powershell.exe -STA -NoProfile -ExecutionPolicy Bypass -File pruebas/Clientes/VerificarClientes.ps1'
}
Add-Type -AssemblyName System.Data,System.Windows.Forms,System.Drawing
$repositorioClientes = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$tokenPruebaClientes = [Guid]::NewGuid().ToString('N')
$basePruebaClientes = 'vidrieria_clientes_pruebas_' + $tokenPruebaClientes
$carpetaPruebasClientes = [IO.Path]::GetFullPath((Join-Path $env:LOCALAPPDATA 'SisDistribuidos-Vidreria/Pruebas'))
$trabajoPruebaClientes = Join-Path $carpetaPruebasClientes $tokenPruebaClientes
$baseCreadaClientes = $false
$resolverEnsambladosClientes = $null
$comprobacionesClientes = New-Object 'Collections.Generic.List[string]'

function ConexionPruebaClientes([string]$catalogo) {
    $opciones = New-Object Data.SqlClient.SqlConnectionStringBuilder
    $opciones['Data Source'] = $Servidor
    $opciones['Initial Catalog'] = $catalogo
    $opciones['Integrated Security'] = $true
    return $opciones.ConnectionString
}

function SqlPruebaClientes([string]$sql, [string]$catalogo = $basePruebaClientes, [switch]$Tabla) {
    $cn = New-Object Data.SqlClient.SqlConnection (ConexionPruebaClientes $catalogo)
    try {
        $cn.Open()
        $cmd = $cn.CreateCommand()
        $cmd.CommandText = $sql
        $cmd.CommandTimeout = 60
        if ($Tabla) {
            $dt = New-Object Data.DataTable
            $da = New-Object Data.SqlClient.SqlDataAdapter $cmd
            try { [void]$da.Fill($dt); return ,$dt } finally { $da.Dispose() }
        }
        return $cmd.ExecuteScalar()
    }
    finally { $cn.Dispose() }
}

function ComprobarClientes([bool]$condicion, [string]$nombre) {
    if (-not $condicion) { throw ('Fallo: ' + $nombre) }
    $comprobacionesClientes.Add($nombre)
}

function EsperarErrorClientes([scriptblock]$accion, [string]$fragmento, [string]$nombre) {
    $rechazado = $false
    try { & $accion | Out-Null }
    catch {
        if ($_.Exception.ToString() -notlike ('*' + $fragmento + '*')) { throw }
        $rechazado = $true
    }
    ComprobarClientes $rechazado $nombre
}

function CampoClientes($formulario, [string]$nombre) {
    $flags = [Reflection.BindingFlags]'Instance,NonPublic,Public'
    $propiedad = $formulario.GetType().GetProperty($nombre, $flags)
    if ($null -ne $propiedad) { return $propiedad.GetValue($formulario) }
    $campo = $formulario.GetType().GetField($nombre, $flags)
    if ($null -eq $campo) { throw ('No existe el control o campo: ' + $nombre) }
    return $campo.GetValue($formulario)
}

function ProbarRegistroSimultaneoClientes {
    $cadena = (ConexionPruebaClientes $basePruebaClientes) + ';Asynchronous Processing=True'
    $conexiones = @((New-Object Data.SqlClient.SqlConnection $cadena), (New-Object Data.SqlClient.SqlConnection $cadena))
    $comandos = @()
    $pendientes = @()
    $rechazados = 0
    try {
        for ($i = 0; $i -lt 2; $i++) {
            $conexiones[$i].Open()
            $cmd = $conexiones[$i].CreateCommand()
            $cmd.CommandTimeout = 15
            $cmd.CommandText = "BEGIN TRANSACTION; EXEC dbo.sp_cli_guardar @nombre=N'Simultáneo $i',@tipo_persona=N'NATURAL',@tipo_documento=N'DNI',@documento=N'87654321'; WAITFOR DELAY '00:00:02'; COMMIT TRANSACTION;"
            $comandos += $cmd
            $pendientes += $cmd.BeginExecuteNonQuery()
        }
        for ($i = 0; $i -lt 2; $i++) {
            try { [void]$comandos[$i].EndExecuteNonQuery($pendientes[$i]) }
            catch {
                if ($_.Exception.ToString() -notlike '*documento ya pertenece*') { throw }
                $rechazados++
            }
        }
        ComprobarClientes ($rechazados -eq 1) 'Dos altas simultáneas: una se rechaza por duplicado'
        ComprobarClientes ((SqlPruebaClientes "SELECT COUNT(*) FROM clientes WHERE documento=N'87654321';") -eq 1) 'Dos altas simultáneas conservan un único cliente'
        SqlPruebaClientes "DELETE FROM clientes WHERE documento=N'87654321';" | Out-Null
    }
    finally {
        foreach ($cmd in $comandos) { $cmd.Dispose() }
        foreach ($cn in $conexiones) { $cn.Dispose() }
    }
}

function MetodoClientes($objeto, [string]$nombre, [object[]]$argumentos = @()) {
    $objeto.GetType().GetMethod($nombre, [Reflection.BindingFlags]'Instance,NonPublic,Public').Invoke($objeto, $argumentos) | Out-Null
}

function MostrarPruebaClientes($formulario) {
    $formulario.ShowInTaskbar = $false
    $formulario.StartPosition = [Windows.Forms.FormStartPosition]::Manual
    $formulario.Location = New-Object Drawing.Point(-30000, -30000)
    $formulario.Show()
    [Windows.Forms.Application]::DoEvents()
}

function CapturarPruebaClientes($formulario, [string]$nombre) {
    $imagen = New-Object Drawing.Bitmap $formulario.Width,$formulario.Height
    try {
        $formulario.DrawToBitmap($imagen, (New-Object Drawing.Rectangle(0,0,$imagen.Width,$imagen.Height)))
        $imagen.Save((Join-Path $trabajoPruebaClientes ($nombre + '.png')))
    }
    finally { $imagen.Dispose() }
}

function ElegirClientePrueba($formulario, [string]$evento, [int]$idElegido) {
    # Un temporizador del propio proceso de prueba elige en el diálogo real.
    # No se usa teclado ni se operan ventanas de la aplicación del usuario.
    $temporizador = New-Object Windows.Forms.Timer
    $temporizador.Interval = 100
    $estado = @{ Ticks = 0; Seleccionado = $false; Fallo = $null }
    $tick = {
        $estado.Ticks++
        $dialogos = @([Windows.Forms.Application]::OpenForms | Where-Object {
            $_.GetType().FullName -eq 'capaPresentacion.frmListadoClientes' -and $_.modoSeleccion
        })
        try {
            if ($dialogos.Count -gt 0) {
                $temporizador.Stop()
                $dialogos[0].cargarLista($idElegido)
                MetodoClientes $dialogos[0] 'btnSeleccionar_Click' @($null,[EventArgs]::Empty)
                $estado.Seleccionado = $true
            }
            elseif ($estado.Ticks -ge 100) {
                $temporizador.Stop()
                $estado.Fallo = 'No se abrió el selector esperado.'
                foreach ($abierto in @([Windows.Forms.Application]::OpenForms)) {
                    if ($abierto.Modal) { $abierto.DialogResult = [Windows.Forms.DialogResult]::Cancel }
                }
            }
        }
        catch {
            $estado.Fallo = $_.Exception.Message
            foreach ($dialogo in $dialogos) { $dialogo.DialogResult = [Windows.Forms.DialogResult]::Cancel }
        }
    }.GetNewClosure()
    $temporizador.add_Tick($tick)
    try {
        $temporizador.Start()
        MetodoClientes $formulario $evento @($null,[EventArgs]::Empty)
        if ($null -ne $estado.Fallo) { throw $estado.Fallo }
        ComprobarClientes $estado.Seleccionado ('El enlace ' + $evento + ' utiliza el selector real')
    }
    finally { $temporizador.Stop(); $temporizador.Dispose() }
}

try {
    New-Item -ItemType Directory -Path $trabajoPruebaClientes -Force | Out-Null
    if ([string]::IsNullOrWhiteSpace($MSBuild)) {
        $encontrado = Get-Command MSBuild.exe -ErrorAction SilentlyContinue
        if ($null -ne $encontrado) { $MSBuild = $encontrado.Source }
        else {
            $vswhereClientes = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
            if (Test-Path -LiteralPath $vswhereClientes) {
                $MSBuild = & $vswhereClientes -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
            }
        }
        if ([string]::IsNullOrWhiteSpace($MSBuild)) {
            $MSBuild = Join-Path $env:ProgramFiles 'Microsoft SQL Server Management Studio 22/Release/MSBuild/Current/Bin/MSBuild.exe'
        }
    }
    if (-not (Test-Path -LiteralPath $MSBuild)) { throw 'No se encontró MSBuild. Indique la ruta con -MSBuild.' }

    SqlPruebaClientes ('CREATE DATABASE [' + $basePruebaClientes + '];') 'master' | Out-Null
    $baseCreadaClientes = $true
    $esquemaClientes = [IO.File]::ReadAllText((Join-Path $repositorioClientes 'recursos/bd/00_vidrieria_completo.sql'))
    $esquemaClientes = $esquemaClientes.Replace("N'vidrieria'", ("N'" + $basePruebaClientes + "'"))
    $esquemaClientes = $esquemaClientes.Replace('CREATE DATABASE vidrieria;', ('CREATE DATABASE [' + $basePruebaClientes + '];'))
    $esquemaClientes = $esquemaClientes.Replace('USE vidrieria;', ('USE [' + $basePruebaClientes + '];'))
    foreach ($lote in [regex]::Split($esquemaClientes, '(?im)^\s*GO\s*$')) {
        if (-not [string]::IsNullOrWhiteSpace($lote)) { SqlPruebaClientes $lote | Out-Null }
    }
    $instaladorClientes = Join-Path $repositorioClientes 'recursos/InstalarClientes.ps1'
    SqlPruebaClientes 'ALTER TABLE dbo.clientes DROP CONSTRAINT CK_clientes_identificacion, CK_clientes_documento; DROP INDEX UX_clientes_documento ON dbo.clientes; ALTER TABLE dbo.clientes DROP COLUMN version_cliente, tipo_persona, tipo_documento;' | Out-Null
    SqlPruebaClientes "INSERT INTO clientes(nombre,documento) VALUES(N'Cliente previo',N'PREVIO'),(N'Duplicado previo A',N'00111111'),(N'Duplicado previo B',N'00111111');" | Out-Null
    & $instaladorClientes -Servidor $Servidor -BaseDatos $basePruebaClientes | Out-Null
    ComprobarClientes ((SqlPruebaClientes "SELECT COL_LENGTH('dbo.clientes','version_cliente');") -eq 8) 'Actualización agrega versión a clientes existentes'
    ComprobarClientes ((SqlPruebaClientes 'SELECT COUNT(*) FROM clientes WHERE tipo_persona IS NULL AND tipo_documento IS NULL;') -eq 3) 'Migración conserva registros anteriores sin adivinar su clasificación'
    ComprobarClientes ((SqlPruebaClientes "SELECT COUNT(*) FROM clientes WHERE documento=N'00111111';") -eq 2) 'Migración no borra duplicados anteriores'
    SqlPruebaClientes "DELETE FROM clientes WHERE documento=N'00111111';" | Out-Null
    $versionPreviaClientes = SqlPruebaClientes 'SELECT CONVERT(VARCHAR(18),version_cliente,1) FROM clientes;'
    & $instaladorClientes -Servidor $Servidor -BaseDatos $basePruebaClientes | Out-Null
    ComprobarClientes ((SqlPruebaClientes 'SELECT COUNT(*) FROM clientes;') -eq 1) 'Instalación repetida conserva clientes'
    ComprobarClientes ((SqlPruebaClientes 'SELECT CONVERT(VARCHAR(18),version_cliente,1) FROM clientes;') -eq $versionPreviaClientes) 'Instalación repetida conserva versiones'
    ComprobarClientes ((SqlPruebaClientes "SELECT COUNT(*) FROM sys.procedures WHERE name LIKE 'sp_cli_%';") -eq 8) 'Ocho procedimientos instalados'

    # Copiar y compilar el código actual. La conexión se cambia SOLO en esta
    # copia temporal para que las clases reales nunca escriban en vidrieria.
    $copiaClientes = Join-Path $trabajoPruebaClientes 'solucion'
    foreach ($capa in @('capaDatos','capaNegocio','capaPresentacion')) {
        & robocopy (Join-Path $repositorioClientes $capa) (Join-Path $copiaClientes $capa) /E /XD bin obj .vs /NFL /NDL /NJH /NJS | Out-Null
        if ($LASTEXITCODE -ge 8) { throw ('No se pudo copiar ' + $capa) }
    }
    $conexionCopiaClientes = Join-Path $copiaClientes 'capaDatos/clsConectaBD.vb'
    $fuenteConexionClientes = [IO.File]::ReadAllText($conexionCopiaClientes)
    $conexionOriginalClientes = 'data source=(localdb)\MSSQLLocalDB;Initial catalog=vidrieria;integrated security=SSPI;language=spanish'
    if (-not $fuenteConexionClientes.Contains($conexionOriginalClientes)) { throw 'La conexión del proyecto cambió. Revise el aislamiento de esta prueba.' }
    $cadenaPruebaClientes = (ConexionPruebaClientes $basePruebaClientes).Replace('"','""')
    $fuenteConexionClientes = $fuenteConexionClientes.Replace($conexionOriginalClientes, $cadenaPruebaClientes)
    [IO.File]::WriteAllText($conexionCopiaClientes, $fuenteConexionClientes, (New-Object Text.UTF8Encoding($true)))
    $salidaCompilacionClientes = & $MSBuild (Join-Path $copiaClientes 'capaPresentacion/capaPresentacion.vbproj') /t:Rebuild /p:Configuration=Debug /p:UseSharedCompilation=false /nodeReuse:false /v:minimal /nologo 2>&1
    if ($LASTEXITCODE -ne 0) { throw ($salidaCompilacionClientes -join [Environment]::NewLine) }
    ComprobarClientes $true 'Las tres capas compilan en la copia aislada'
    $resolverEnsambladosClientes = [ResolveEventHandler] {
        param($emisor, $peticion)
        return [AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.FullName -eq $peticion.Name } | Select-Object -First 1
    }
    [AppDomain]::CurrentDomain.add_AssemblyResolve($resolverEnsambladosClientes)
    foreach ($ensamblado in @('capaDatos/capaDatos.dll','capaNegocio/capaNegocio.dll','capaPresentacion/capaPresentacion.exe')) {
        $partes = $ensamblado.Split('/')
        $rutaEnsamblado = Join-Path $copiaClientes ($partes[0] + '/bin/Debug/' + $partes[1])
        # Cargar bytes evita bloquear DLL y permite limpiar al terminar.
        [Reflection.Assembly]::Load([byte[]][IO.File]::ReadAllBytes($rutaEnsamblado)) | Out-Null
    }

    $cliente = New-Object capaNegocio.Cliente
    $id = $cliente.Insertar("  María O'Connor  ", 'CLI-001', '+51 (999) 555-111', 'Jr. Las Flores 120')
    ComprobarClientes ($id -gt 0) 'Alta devuelve ID'
    $dt = $cliente.Obtener($id)
    ComprobarClientes ($dt.Rows[0]['nombre'] -eq "María O'Connor") 'Unicode, comillas y normalización'
    ComprobarClientes ($cliente.Buscar("O'Connor").Rows.Count -eq 1) 'Búsqueda con comillas'
    ComprobarClientes ($cliente.Buscar('+51').Rows.Count -eq 1) 'Búsqueda por teléfono'
    ComprobarClientes ($dt.Rows[0]['version_cliente'].Length -eq 8) 'Obtener devuelve versión de edición'
    $versionA = [byte[]]$dt.Rows[0]['version_cliente']
    $otroTrabajador = New-Object capaNegocio.Cliente
    $versionB = [byte[]]$otroTrabajador.Obtener($id).Rows[0]['version_cliente']
    $cliente.Actualizar($id, 'María Editada', 'CLI-001', '+51 999555111', '', $versionA)
    EsperarErrorClientes { $otroTrabajador.Actualizar($id,'Cambio antiguo','CLI-001',$null,$null,$versionB) } 'Otro trabajador' 'Dos editores: se rechaza la versión antigua'
    ComprobarClientes ($cliente.Obtener($id).Rows[0]['nombre'] -eq 'María Editada') 'El conflicto conserva el primer cambio'
    $versionNueva = [byte[]]$cliente.Obtener($id).Rows[0]['version_cliente']
    $cliente.Actualizar($id,'María Actualizada','CLI-001',$null,$null,$versionNueva)
    ComprobarClientes ($cliente.Obtener($id).Rows[0]['nombre'] -eq 'María Actualizada') 'Recargar versión permite editar'
    $cliente.Actualizar($id,'María Final','CLI-001',' ', '')
    $dt = $cliente.Obtener($id)
    ComprobarClientes ($dt.Rows[0]['nombre'] -eq 'María Final') 'Firma original sigue disponible'
    ComprobarClientes (($dt.Rows[0]['telefono'] -is [DBNull]) -and ($dt.Rows[0]['direccion'] -is [DBNull])) 'Opcionales vacíos como NULL'
    EsperarErrorClientes { $cliente.Insertar('  ',$null,$null,$null) } 'nombre' 'Nombre obligatorio'
    EsperarErrorClientes { $cliente.Insertar(('x'*151),$null,$null,$null) } '150' 'Límite de nombre'
    EsperarErrorClientes { $cliente.Insertar('Prueba',('x'*21),$null,$null) } '20' 'Límite de documento'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,('1'*21),$null) } '20' 'Límite de teléfono'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,$null,('x'*256)) } '255' 'Límite de dirección'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'abc',$null) } 'teléfono' 'Teléfono rechaza letras'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'+()-',$null) } 'teléfono' 'Teléfono exige entre 7 y 15 dígitos'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'123',$null) } 'teléfono' 'Teléfono rechaza números incompletos'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,('1'*16),$null) } 'teléfono' 'Teléfono rechaza exceso de dígitos'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'999+555111',$null) } 'teléfono' 'Signo + solo al inicio'
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'(01 4443333',$null) } 'paréntesis' 'Paréntesis incompletos rechazados'
    EsperarErrorClientes { $cliente.Obtener(0) } 'válido' 'ID inválido'
    EsperarErrorClientes { $cliente.Buscar(('x'*151)) } '150' 'Límite de búsqueda'
    EsperarErrorClientes { $cliente.Actualizar(999999,'Ausente',$null,$null,$null) } 'no existe' 'Edición de cliente inexistente'
    EsperarErrorClientes { $cliente.Eliminar(999999) } 'no existe' 'Eliminación de cliente inexistente'
    $literal = $cliente.Insertar('Literal %_[\',$null,$null,$null)
    ComprobarClientes ($cliente.Buscar('%_[\').Rows.Count -eq 1) 'Comodines buscados como texto literal'
    ComprobarClientes ($cliente.Buscar("' OR 1=1--").Rows.Count -eq 0) 'Entrada SQL tratada como dato'
    ComprobarClientes ($cliente.DocumentoCoincidente('CLI-001',$id).Rows.Count -eq 0) 'Documento excluye al cliente editado'
    EsperarErrorClientes { $cliente.Insertar('Documento repetido','CLI-001',$null,$null) } 'documento ya' 'No permite registrar documento repetido'
    ComprobarClientes ($cliente.DocumentoCoincidente(' CLI-001 ').Rows.Count -eq 1) 'Detección de documento con espacios'
    $repetido = $cliente.Insertar('Otro cliente','CLI-002',$null,$null)
    ComprobarClientes ($cliente.DocumentoCoincidente($null).Rows.Count -eq 0) 'Documento vacío no genera advertencia'
    ComprobarClientes ($cliente.Historial($repetido).Rows.Count -eq 0) 'Cliente sin pedidos tiene historial vacío'

    $natural = $cliente.Insertar('Ana Pérez','00123456','999555111',$null,'NATURAL','DNI')
    $naturalConRuc = $cliente.Insertar('Ana con negocio','10001234567','01 4443333',$null,'NATURAL','RUC')
    $empresa = $cliente.Insertar('Vidrios del Norte S.A.C.','20123456789','+51 (01) 4443333',$null,'JURIDICA','RUC')
    $sinDocumento = $cliente.Insertar('Contacto inicial',$null,$null,$null,'NATURAL','SIN DOCUMENTO')
    ComprobarClientes ($cliente.Obtener($natural).Rows[0]['documento'] -eq '00123456') 'DNI conserva ceros iniciales'
    ComprobarClientes ($cliente.Obtener($naturalConRuc).Rows[0]['tipo_persona'] -eq 'NATURAL') 'Una persona natural puede tener RUC'
    ComprobarClientes ($cliente.Obtener($empresa).Rows[0]['tipo_documento'] -eq 'RUC') 'Empresa conserva clasificación y RUC'
    ComprobarClientes ($cliente.Obtener($sinDocumento).Rows[0]['documento'] -is [DBNull]) 'Sin documento guarda número NULL'
    EsperarErrorClientes { $cliente.Insertar('Ana duplicada','00123456',$null,$null,'NATURAL','DNI') } 'documento ya' 'DNI duplicado bloqueado'
    EsperarErrorClientes { $cliente.Insertar('DNI corto','1234567',$null,$null,'NATURAL','DNI') } '8 dígitos' 'DNI exige ocho dígitos'
    EsperarErrorClientes { $cliente.Insertar('DNI largo','123456789',$null,$null,'NATURAL','DNI') } '8 dígitos' 'DNI rechaza nueve dígitos'
    EsperarErrorClientes { $cliente.Insertar('DNI letras','1234567A',$null,$null,'NATURAL','DNI') } '8 dígitos' 'DNI rechaza letras'
    EsperarErrorClientes { $cliente.Insertar('DNI unicode','１２３４５６７８',$null,$null,'NATURAL','DNI') } '8 dígitos' 'DNI exige dígitos ASCII'
    EsperarErrorClientes { $cliente.Insertar('RUC corto','2012345678',$null,$null,'JURIDICA','RUC') } '11 dígitos' 'RUC exige once dígitos'
    EsperarErrorClientes { $cliente.Insertar('Empresa DNI','12345678',$null,$null,'JURIDICA','DNI') } 'requiere RUC' 'Jurídica no admite DNI'
    EsperarErrorClientes { $cliente.Insertar('Empresa RUC natural','10123456789',$null,$null,'JURIDICA','RUC') } 'empezar' 'Prefijo RUC coherente con persona'
    EsperarErrorClientes { $cliente.Insertar('Sin documento con número','12345678',$null,$null,'NATURAL','SIN DOCUMENTO') } 'vacío' 'Sin documento exige número vacío'
    EsperarErrorClientes { $cliente.Insertar('Tipo incorrecto','12345678',$null,$null,'OTRO','DNI') } 'tipo de persona' 'Tipo de persona desconocido rechazado'
    EsperarErrorClientes { $cliente.Insertar('Documento desconocido','12345678',$null,$null,'NATURAL','OTRO') } 'tipo de documento' 'Tipo de documento desconocido rechazado'
    $versionNatural = [byte[]]$cliente.Obtener($natural).Rows[0]['version_cliente']
    $cliente.Actualizar($natural,'Ana Actualizada','00123456',$null,$null,$versionNatural,'NATURAL','DNI')
    ComprobarClientes ($cliente.DocumentoCoincidente('00123456',$natural,'DNI').Rows.Count -eq 0) 'Editar no se confunde con duplicado propio'
    $cliente.Actualizar($natural,'Ana Compatible','00123456',$null,$null)
    ComprobarClientes ($cliente.Obtener($natural).Rows[0]['tipo_documento'] -eq 'DNI') 'Firma anterior conserva clasificación existente'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'Duplicado SQL',@tipo_persona=N'NATURAL',@tipo_documento=N'DNI',@documento=N'00123456';" } 'documento ya' 'SQL también bloquea duplicados'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'DNI SQL',@tipo_persona=N'NATURAL',@tipo_documento=N'DNI',@documento=N'123';" } '8 digitos' 'SQL también valida formato DNI'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'Tipo SQL',@tipo_persona=N'JURIDICA',@tipo_documento=N'DNI',@documento=N'12345678';" } 'requiere RUC' 'SQL también valida compatibilidad de tipos'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'RUC SQL',@tipo_persona=N'JURIDICA',@tipo_documento=N'RUC',@documento=N'10123456789';" } 'empezar' 'SQL también valida prefijo RUC'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'Teléfono SQL',@telefono=N'123';" } '7 y 15' 'SQL también valida longitud de teléfono'
    foreach ($idTemporal in @($natural,$naturalConRuc,$empresa,$sinDocumento)) { $cliente.Eliminar($idTemporal) }
    ProbarRegistroSimultaneoClientes

    $pedido1 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PARCIAL',100); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido2 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'CANCELADO',200); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido3 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PAGADO',50); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido4 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PENDIENTE',75); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    SqlPruebaClientes "UPDATE pedidos SET fecha='2026-01-10T00:00:00' WHERE id_pedido=$pedido1; UPDATE pedidos SET fecha='2026-01-10T23:59:59' WHERE id_pedido=$pedido2; UPDATE pedidos SET fecha='2026-01-11T00:00:00' WHERE id_pedido=$pedido3; UPDATE pedidos SET fecha='2026-01-09T23:59:59' WHERE id_pedido=$pedido4;" | Out-Null
    SqlPruebaClientes "INSERT INTO metodos_pago(nombre) VALUES(N'Efectivo'),(N'Yape');" | Out-Null
    $pago1 = [int](SqlPruebaClientes "INSERT INTO pagos(id_pedido,monto_total) VALUES($pedido1,40); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    SqlPruebaClientes "INSERT INTO detalle_pago(id_pago,id_metodo_pago,monto) VALUES($pago1,1,15),($pago1,2,25); INSERT INTO comprobante_pago(id_pago,tipo_comprobante,serie,numero) VALUES($pago1,'BOLETA','B001','123'); INSERT INTO pagos(id_pedido,monto_total) VALUES($pedido1,10),($pedido2,30),($pedido3,60);" | Out-Null
    $historial = $cliente.Historial($id)
    $parcial = @($historial.Rows | Where-Object { $_['id_pedido'] -eq $pedido1 })[0]
    $cancelado = @($historial.Rows | Where-Object { $_['id_pedido'] -eq $pedido2 })[0]
    $sobrepagado = @($historial.Rows | Where-Object { $_['id_pedido'] -eq $pedido3 })[0]
    $pendiente = @($historial.Rows | Where-Object { $_['id_pedido'] -eq $pedido4 })[0]
    ComprobarClientes ($historial.Rows.Count -eq 4) 'Historial reúne solo pedidos del cliente'
    ComprobarClientes ($parcial['pagado'] -eq 50 -and $parcial['saldo'] -eq 50) 'Pago repartido no multiplica importes'
    ComprobarClientes ($cancelado['pagado'] -eq 30 -and $cancelado['saldo'] -eq 0) 'Cancelación conserva pagos y elimina saldo'
    ComprobarClientes ($sobrepagado['saldo'] -eq 0 -and $sobrepagado['excedente'] -eq 10) 'Sobrepago se muestra sin saldo negativo'
    ComprobarClientes ($pendiente['pagado'] -eq 0 -and $pendiente['saldo'] -eq 75) 'Pedido sin pagos conserva saldo'
    ComprobarClientes ($cliente.Historial($id,'parcial').Rows.Count -eq 1) 'Filtro por estado'
    ComprobarClientes ($cliente.Historial($id,$null,[datetime]'2026-01-10',[datetime]'2026-01-10').Rows.Count -eq 2) 'Filtro incluye todo el día final'
    ComprobarClientes ($cliente.Historial($id,'PARCIAL',[datetime]'2026-01-10',[datetime]'2026-01-10').Rows.Count -eq 1) 'Estado y fechas se combinan'
    ComprobarClientes ($cliente.Historial($id,$null,[datetime]'2026-01-11',$null).Rows.Count -eq 1) 'Solo fecha inicial funciona'
    ComprobarClientes ($cliente.Historial($id,$null,$null,[datetime]'2026-01-09').Rows.Count -eq 1) 'Solo fecha final funciona'
    ComprobarClientes ($cliente.Historial($id,$null,[datetime]'2025-01-01',[datetime]'2025-12-31').Rows.Count -eq 0) 'Rango sin pedidos devuelve vacío'
    EsperarErrorClientes { $cliente.Historial($id,$null,[datetime]'2026-01-11',[datetime]'2026-01-10') } 'Desde' 'Negocio rechaza fechas invertidas'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_historial @id_cliente=$id,@desde='20260111',@hasta='20260110';" } 'Desde' 'SQL rechaza fechas invertidas'
    EsperarErrorClientes { $cliente.Historial($id,'DESCONOCIDO') } 'válido' 'Estado inválido rechazado'
    $pagos = $cliente.PagosDePedido($id,$pedido1)
    ComprobarClientes ($pagos.Rows.Count -eq 3) 'Desglose incluye métodos y pago sin detalle'
    ComprobarClientes (@($pagos.Rows | Where-Object { $_['comprobante'] -eq 'BOLETA B001-123' }).Count -eq 2) 'Comprobante acompaña métodos del mismo pago'
    EsperarErrorClientes { $cliente.PagosDePedido($repetido,$pedido1) } 'no pertenece' 'Pagos no permiten consultar otro cliente'
    EsperarErrorClientes { $cliente.Eliminar($id) } 'tiene pedidos' 'Eliminación protege historial'
    ComprobarClientes ($cliente.Obtener($id).Rows.Count -eq 1) 'Cliente con pedidos permanece'
    $cliente.Eliminar($literal)
    ComprobarClientes ($cliente.Obtener($literal).Rows.Count -eq 0) 'Eliminar cliente sin pedidos'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @nombre=N'Prueba',@telefono=N'abc';" } 'telefono' 'SQL también valida teléfono'
    EsperarErrorClientes { SqlPruebaClientes "EXEC dbo.sp_cli_guardar @id_cliente=$id,@nombre=N'Prueba';" } 'Recargue' 'SQL exige versión al editar'

    foreach ($tema in @('CLARO','OSCURO')) {
        $nuevo = New-Object capaPresentacion.frmRegistrarClientes
        $nuevo.tema = $tema
        try {
            MostrarPruebaClientes $nuevo
            ComprobarClientes ((CampoClientes $nuevo 'cmbTipoPersona').SelectedIndex -eq 0 -and (CampoClientes $nuevo 'cmbTipoDocumento').SelectedItem -eq 'DNI') ('Alta inicia como persona natural con DNI (' + $tema + ')')
            (CampoClientes $nuevo 'txtNombre').Text = 'Ana Pérez'
            (CampoClientes $nuevo 'txtDocumento').Text = '00123456'
            (CampoClientes $nuevo 'txtTelefono').Text = '999555111'
            CapturarPruebaClientes $nuevo ('nuevo_' + $tema)
        } finally { $nuevo.Dispose() }
        $registro = New-Object capaPresentacion.frmRegistrarClientes
        $registro.tema = $tema; $registro.idCliente = $id
        try {
            MostrarPruebaClientes $registro
            ComprobarClientes ((CampoClientes $registro 'btnRecargar').Visible) ('Recargar disponible en edición (' + $tema + ')')
            ComprobarClientes ((CampoClientes $registro 'versionOriginal').Length -eq 8) ('Formulario conserva versión leída (' + $tema + ')')
            ComprobarClientes ((CampoClientes $registro 'cmbTipoPersona').SelectedIndex -eq -1) ('Registro anterior exige clasificar sin inferencias (' + $tema + ')')
            $panelRegistro = CampoClientes $registro 'Panel2'
            ComprobarClientes (@($panelRegistro.Controls | Where-Object { $_.Visible -and ($_.Right -gt $panelRegistro.ClientSize.Width -or $_.Bottom -gt $panelRegistro.ClientSize.Height) }).Count -eq 0) ('Registro sin controles recortados (' + $tema + ')')
            ComprobarClientes (-not ($registro.GetType().GetMethod('hayCambios',[Reflection.BindingFlags]'Instance,NonPublic').Invoke($registro,@()))) ('Abrir registro no produce cambios pendientes (' + $tema + ')')
            (CampoClientes $registro 'txtNombre').Text += ' modificado'
            ComprobarClientes ($registro.GetType().GetMethod('hayCambios',[Reflection.BindingFlags]'Instance,NonPublic').Invoke($registro,@())) ('Detecta modificaciones sin guardar (' + $tema + ')')
            (CampoClientes $registro 'cmbTipoPersona').SelectedIndex = 1
            ComprobarClientes ((CampoClientes $registro 'cmbTipoDocumento').SelectedItem -eq 'RUC') ('Jurídica selecciona RUC (' + $tema + ')')
            ComprobarClientes ((CampoClientes $registro 'lblNombre').Text -eq 'Razón social *') ('Jurídica muestra razón social (' + $tema + ')')
            ComprobarClientes (-not (CampoClientes $registro 'cmbTipoDocumento').Enabled) ('Jurídica fija tipo RUC (' + $tema + ')')
            (CampoClientes $registro 'cmbTipoPersona').SelectedIndex = 0
            ComprobarClientes ((CampoClientes $registro 'cmbTipoDocumento').Enabled) ('Natural permite cambiar tipo de documento (' + $tema + ')')
            (CampoClientes $registro 'cmbTipoPersona').SelectedIndex = 1
            CapturarPruebaClientes $registro ('registro_' + $tema)
        } finally { $registro.Dispose() }
        $listado = New-Object capaPresentacion.frmListadoClientes
        $listado.tema = $tema
        try {
            MostrarPruebaClientes $listado
            ComprobarClientes ((CampoClientes $listado 'dgvClientes').Rows.Count -eq 3) ('Listado muestra clientes (' + $tema + ')')
            (CampoClientes $listado 'txtBuscar').Text = 'Sin resultados para este texto'
            $listado.cargarLista()
            ComprobarClientes (-not (CampoClientes $listado 'btnHistorial').Enabled) ('Sin selección no abre historial (' + $tema + ')')
            (CampoClientes $listado 'txtBuscar').Clear(); $listado.cargarLista($id)
            CapturarPruebaClientes $listado ('listado_' + $tema)
        } finally { $listado.Dispose() }
        $ficha = New-Object capaPresentacion.frmHistorialClientes
        $ficha.tema = $tema; $ficha.idCliente = $id
        try {
            MostrarPruebaClientes $ficha
            ComprobarClientes ((CampoClientes $ficha 'dgvPedidos').Rows.Count -eq 4) ('Ficha muestra pedidos (' + $tema + ')')
            ComprobarClientes ((CampoClientes $ficha 'lblResumen').Text -like '*125*') ('Resumen excluye cancelados (' + $tema + ')')
            (CampoClientes $ficha 'chkFechas').Checked = $true
            (CampoClientes $ficha 'dtpDesde').Value = [datetime]'2026-01-10'
            (CampoClientes $ficha 'dtpHasta').Value = [datetime]'2026-01-10'
            $ficha.cargarHistorial()
            ComprobarClientes ((CampoClientes $ficha 'dgvPedidos').Rows.Count -eq 2) ('Ficha aplica rango de fechas (' + $tema + ')')
            ComprobarClientes ((CampoClientes $ficha 'lblResumen').Text -like '*50*') ('Resumen corresponde al rango (' + $tema + ')')
            ComprobarClientes ((CampoClientes $ficha 'lblFiltro').Text -like '*10/01/2026*') ('Indica fechas realmente consultadas (' + $tema + ')')
            MetodoClientes $ficha 'btnVerTodos_Click' @($null,[EventArgs]::Empty)
            ComprobarClientes ((CampoClientes $ficha 'dgvPedidos').Rows.Count -eq 4 -and -not (CampoClientes $ficha 'chkFechas').Checked) ('Ver todo restaura historial completo (' + $tema + ')')
            (CampoClientes $ficha 'cmbEstado').SelectedItem = 'PARCIAL'
            [Windows.Forms.Application]::DoEvents()
            ComprobarClientes ((CampoClientes $ficha 'dgvPagos').Rows.Count -eq 3) ('Seleccionar pedido muestra pagos (' + $tema + ')')
            $panel = CampoClientes $ficha 'Panel2'
            ComprobarClientes (@($panel.Controls | Where-Object { $_.Visible -and ($_.Right -gt $panel.ClientSize.Width -or $_.Bottom -gt $panel.ClientSize.Height) }).Count -eq 0) ('Ficha sin controles recortados (' + $tema + ')')
            CapturarPruebaClientes $ficha ('historial_' + $tema)
        } finally { $ficha.Dispose() }
    }
    $selector = New-Object capaPresentacion.frmListadoClientes
    $selector.modoSeleccion = $true
    try {
        MostrarPruebaClientes $selector
        $selector.cargarLista($id)
        MetodoClientes $selector 'btnSeleccionar_Click' @($null,[EventArgs]::Empty)
        ComprobarClientes ($selector.idClienteSeleccionado -eq $id -and $selector.nombreClienteSeleccionado -eq 'María Final') 'Selector devuelve cliente vigente'
        ComprobarClientes ($selector.DialogResult -eq [Windows.Forms.DialogResult]::OK) 'Selector confirma selección'
    } finally { $selector.Dispose() }
    $pedido = New-Object capaPresentacion.frmRegistrarPedido
    try {
        MostrarPruebaClientes $pedido
        ComprobarClientes ($pedido.idClienteSeleccionado -eq 0) 'Pedido inicia sin cliente seleccionado'
        ElegirClientePrueba $pedido 'btnBuscarCliente_Click' $id
        ComprobarClientes ($pedido.idClienteSeleccionado -eq $id) 'Pedido recibe ID del diálogo'
        ComprobarClientes ((CampoClientes $pedido 'txtNombreCliente').Text -eq 'María Final') 'Pedido conserva cliente seleccionado'
        ComprobarClientes ($pedido.nombreClienteSeleccionado -eq 'María Final') 'Pedido recibe el nombre del cliente'
        ComprobarClientes (@($pedido.Controls | Where-Object { $_.Visible -and ($_.Right -gt $pedido.ClientSize.Width -or $_.Bottom -gt $pedido.ClientSize.Height) }).Count -eq 0) 'Pedido sin controles recortados'
        CapturarPruebaClientes $pedido 'pedido'
    } finally { $pedido.Dispose() }
    $pedidoOscuro = New-Object capaPresentacion.frmRegistrarPedido
    $pedidoOscuro.tema = 'OSCURO'
    try {
        MostrarPruebaClientes $pedidoOscuro
        ElegirClientePrueba $pedidoOscuro 'btnBuscarCliente_Click' $id
        ComprobarClientes ($pedidoOscuro.idClienteSeleccionado -eq $id) 'Pedido oscuro recibe cliente seleccionado'
        $etiquetaCliente = (CampoClientes $pedidoOscuro 'txtNombreCliente').Parent.Controls |
            Where-Object { $_ -is [Windows.Forms.Label] -and $_.Text -like '*Cliente*' } | Select-Object -First 1
        ComprobarClientes ($null -ne $etiquetaCliente -and $etiquetaCliente.ForeColor.ToArgb() -ne $etiquetaCliente.Parent.BackColor.ToArgb()) 'Pedido oscuro conserva etiqueta de cliente legible'
        CapturarPruebaClientes $pedidoOscuro 'pedido_OSCURO'
    } finally { $pedidoOscuro.Dispose() }
    $fichaCambio = New-Object capaPresentacion.frmHistorialClientes
    $fichaCambio.idCliente = $id
    try {
        MostrarPruebaClientes $fichaCambio
        ElegirClientePrueba $fichaCambio 'btnCambiarCliente_Click' $repetido
        ComprobarClientes ($fichaCambio.idCliente -eq $repetido) 'Ficha cambia de cliente usando el selector'
        ComprobarClientes ((CampoClientes $fichaCambio 'dgvPedidos').Rows.Count -eq 0) 'Cambiar a cliente sin pedidos limpia grilla'
        ComprobarClientes ((CampoClientes $fichaCambio 'dgvPagos').DataSource -eq $null) 'Cambiar de cliente limpia pagos anteriores'
    } finally { $fichaCambio.Dispose() }

    [IO.File]::WriteAllLines((Join-Path $trabajoPruebaClientes 'resultados.txt'),$comprobacionesClientes,(New-Object Text.UTF8Encoding($true)))
    Write-Output ('Correcto: ' + $comprobacionesClientes.Count + ' comprobaciones. La base de la aplicación no se modificó.')
    if ($ConservarResultados) { Write-Output ('Resultados y capturas: ' + $trabajoPruebaClientes) }
}
catch {
    Write-Output ('Comprobaciones correctas antes del fallo: ' + $comprobacionesClientes.Count)
    Write-Output $_.ScriptStackTrace
    throw
}
finally {
    if ($null -ne $resolverEnsambladosClientes) {
        [AppDomain]::CurrentDomain.remove_AssemblyResolve($resolverEnsambladosClientes)
    }
    if ($baseCreadaClientes) {
        if ($basePruebaClientes -notmatch '^vidrieria_clientes_pruebas_[a-f0-9]{32}$') { throw 'Nombre de base de prueba inesperado.' }
        [Data.SqlClient.SqlConnection]::ClearAllPools()
        SqlPruebaClientes ('ALTER DATABASE [' + $basePruebaClientes + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [' + $basePruebaClientes + '];') 'master' | Out-Null
    }
    if (-not $ConservarResultados -and (Test-Path -LiteralPath $trabajoPruebaClientes)) {
        $rutaFinalClientes = [IO.Path]::GetFullPath($trabajoPruebaClientes)
        if (-not $rutaFinalClientes.StartsWith($carpetaPruebasClientes + [IO.Path]::DirectorySeparatorChar,[StringComparison]::OrdinalIgnoreCase) -or
            [IO.Path]::GetFileName($rutaFinalClientes) -ne $tokenPruebaClientes) { throw 'Ruta de limpieza inesperada.' }
        Remove-Item -LiteralPath $rutaFinalClientes -Recurse -Force
    }
}

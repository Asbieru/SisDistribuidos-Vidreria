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
    $esquemaClientes = [IO.File]::ReadAllText((Join-Path $repositorioClientes 'recursos/bd/esquema_vidrieria_sqlserver.sql'))
    $esquemaClientes = $esquemaClientes.Replace("N'vidrieria'", ("N'" + $basePruebaClientes + "'"))
    $esquemaClientes = $esquemaClientes.Replace('CREATE DATABASE vidrieria;', ('CREATE DATABASE [' + $basePruebaClientes + '];'))
    $esquemaClientes = $esquemaClientes.Replace('USE vidrieria;', ('USE [' + $basePruebaClientes + '];'))
    foreach ($lote in [regex]::Split($esquemaClientes, '(?im)^\s*GO\s*$')) {
        if (-not [string]::IsNullOrWhiteSpace($lote)) { SqlPruebaClientes $lote | Out-Null }
    }
    $instaladorClientes = Join-Path $repositorioClientes 'recursos/InstalarClientes.ps1'
    & $instaladorClientes -Servidor $Servidor -BaseDatos $basePruebaClientes | Out-Null
    SqlPruebaClientes "INSERT INTO clientes(nombre,documento) VALUES(N'Cliente previo',N'PREVIO');" | Out-Null
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
    $salidaCompilacionClientes = & $MSBuild (Join-Path $copiaClientes 'capaPresentacion/capaPresentacion.vbproj') /t:Rebuild /p:Configuration=Debug /v:minimal /nologo 2>&1
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
    $cliente.Actualizar($id, 'María Editada', 'CLI-001', '+51 999', '', $versionA)
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
    EsperarErrorClientes { $cliente.Insertar('Prueba',$null,'+()-',$null) } 'teléfono' 'Teléfono exige al menos un número'
    EsperarErrorClientes { $cliente.Obtener(0) } 'válido' 'ID inválido'
    EsperarErrorClientes { $cliente.Buscar(('x'*151)) } '150' 'Límite de búsqueda'
    EsperarErrorClientes { $cliente.Actualizar(999999,'Ausente',$null,$null,$null) } 'no existe' 'Edición de cliente inexistente'
    EsperarErrorClientes { $cliente.Eliminar(999999) } 'no existe' 'Eliminación de cliente inexistente'
    $literal = $cliente.Insertar('Literal %_[\',$null,$null,$null)
    ComprobarClientes ($cliente.Buscar('%_[\').Rows.Count -eq 1) 'Comodines buscados como texto literal'
    ComprobarClientes ($cliente.Buscar("' OR 1=1--").Rows.Count -eq 0) 'Entrada SQL tratada como dato'
    ComprobarClientes ($cliente.DocumentoCoincidente('CLI-001',$id).Rows.Count -eq 0) 'Documento excluye al cliente editado'
    $repetido = $cliente.Insertar('Documento repetido','CLI-001',$null,$null)
    ComprobarClientes ($cliente.DocumentoCoincidente(' CLI-001 ',$id).Rows.Count -eq 1) 'Detección de documento repetido'
    ComprobarClientes ($cliente.DocumentoCoincidente($null).Rows.Count -eq 0) 'Documento vacío no genera advertencia'
    ComprobarClientes ($cliente.Historial($repetido).Rows.Count -eq 0) 'Cliente sin pedidos tiene historial vacío'

    $pedido1 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PARCIAL',100); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido2 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'CANCELADO',200); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido3 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PAGADO',50); SELECT CONVERT(INT,SCOPE_IDENTITY());")
    $pedido4 = [int](SqlPruebaClientes "INSERT INTO pedidos(id_cliente,id_usuario,estado,total) VALUES($id,1,'PENDIENTE',75); SELECT CONVERT(INT,SCOPE_IDENTITY());")
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
        $registro = New-Object capaPresentacion.frmRegistrarClientes
        $registro.tema = $tema; $registro.idCliente = $id
        try {
            MostrarPruebaClientes $registro
            ComprobarClientes ((CampoClientes $registro 'btnRecargar').Visible) ('Recargar disponible en edición (' + $tema + ')')
            ComprobarClientes ((CampoClientes $registro 'versionOriginal').Length -eq 8) ('Formulario conserva versión leída (' + $tema + ')')
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
    $cotizacion = New-Object capaPresentacion.frmCotizarVentana
    try {
        MostrarPruebaClientes $cotizacion
        ComprobarClientes (-not (CampoClientes $cotizacion 'btnHistorialCliente').Enabled) 'Cotización sin cliente deshabilita historial'
        ElegirClientePrueba $cotizacion 'btnBuscarCliente_Click' $id
        ComprobarClientes ($cotizacion.idClienteSeleccionado -eq $id) 'Cotización recibe ID del diálogo'
        ComprobarClientes ((CampoClientes $cotizacion 'txtCliente').Text -eq 'María Final') 'Cotización conserva cliente seleccionado'
        ComprobarClientes ((CampoClientes $cotizacion 'btnHistorialCliente').Enabled) 'Cotización habilita consulta de historial'
        ComprobarClientes (@($cotizacion.Controls | Where-Object { $_.Visible -and ($_.Right -gt $cotizacion.ClientSize.Width -or $_.Bottom -gt $cotizacion.ClientSize.Height) }).Count -eq 0) 'Cotización sin controles recortados'
        CapturarPruebaClientes $cotizacion 'cotizacion'
    } finally { $cotizacion.Dispose() }
    $cotizacionOscura = New-Object capaPresentacion.frmCotizarVentana
    $cotizacionOscura.tema = 'OSCURO'
    try {
        MostrarPruebaClientes $cotizacionOscura
        ComprobarClientes ((CampoClientes $cotizacionOscura 'Label1').ForeColor.ToArgb() -ne (CampoClientes $cotizacionOscura 'Panel1').BackColor.ToArgb()) 'Cotización oscura conserva texto legible en características'
        ComprobarClientes ((CampoClientes $cotizacionOscura 'Label8').ForeColor.ToArgb() -ne (CampoClientes $cotizacionOscura 'Panel2').BackColor.ToArgb()) 'Cotización oscura conserva texto legible en medidas'
        CapturarPruebaClientes $cotizacionOscura 'cotizacion_OSCURO'
    } finally { $cotizacionOscura.Dispose() }
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

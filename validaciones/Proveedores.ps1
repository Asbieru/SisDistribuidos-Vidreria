param([switch]$Integracion)
$ErrorActionPreference='Stop'
$raiz=Split-Path $PSScriptRoot -Parent
Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Transactions
[void][Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaDatos.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaNegocio.dll'))
$proveedor=New-Object capaNegocio.Proveedor
function Comprobar($valor,[string]$mensaje) { if(-not $valor){throw $mensaje} }
function Rechazar([scriptblock]$accion,[string]$patron) {
    $rechazado=$false
    try { & $accion | Out-Null } catch {
        if($_.Exception.Message -notmatch $patron){throw}
        $rechazado=$true
    }
    Comprobar $rechazado ('No se rechazó el caso esperado: '+$patron)
}
foreach($caso in @(
    @{p='NATURAL';t='DNI';d='00001234';f=''},
    @{p='NATURAL';t='RUC';d='10123456789';f='+51 (1) 234-5678'},
    @{p='NATURAL';t='RUC';d='15123456789';f='987654321'},
    @{p='JURIDICA';t='RUC';d='20123456789';f=''},
    @{p='NATURAL';t='SIN DOCUMENTO';d='';f=''}
)) {
    # Se comprueba formato, no identidad ni inscripción fiscal.
    $proveedor.ValidarDatos('Nombre válido',$caso.d,$caso.f,'Referencia de ubicación',$caso.p,$caso.t)
}
Rechazar {$proveedor.ValidarDatos('','','','','NATURAL','SIN DOCUMENTO')} 'nombre'
Rechazar {$proveedor.ValidarDatos(('x'*151),'','','','NATURAL','SIN DOCUMENTO')} '150'
Rechazar {$proveedor.ValidarDatos('Nombre','','',('x'*256),'NATURAL','SIN DOCUMENTO')} '255'
foreach($dni in @('1234567','123456789','1234567A','１２３４５６７８')) {
    Rechazar {$proveedor.ValidarDatos('Nombre',$dni,'','','NATURAL','DNI')} 'DNI'
}
Rechazar {$proveedor.ValidarDatos('Nombre','20123456789','','','NATURAL','RUC')} 'RUC'
Rechazar {$proveedor.ValidarDatos('Nombre','10123456789','','','JURIDICA','RUC')} 'RUC'
Rechazar {$proveedor.ValidarDatos('Nombre','12345678','','','JURIDICA','DNI')} 'RUC'
Rechazar {$proveedor.ValidarDatos('Nombre','123','','','NATURAL','SIN DOCUMENTO')} 'vacío'
foreach($telefono in @('123','1234567890123456','987ABC321','98+7654321','(987654321','987)654(321','((987))654321')) {
    Rechazar {$proveedor.ValidarDatos('Nombre','',$telefono,'','NATURAL','SIN DOCUMENTO')} 'teléfono|paréntesis'
}
Write-Output 'Proveedores: validaciones de persona, documento, teléfono, nombre y dirección correctas.'
if(-not $Integracion){return}

$configuracion=New-Object capaDatos.clsConectaBD
$cadena=$configuracion.miConexion.ConnectionString
function Consultar([string]$sql,[hashtable]$parametros=@{}) {
    $cn=[Data.SqlClient.SqlConnection]::new($cadena)
    try {
        $cn.Open();$cmd=$cn.CreateCommand();$cmd.CommandText=$sql
        foreach($k in $parametros.Keys){[void]$cmd.Parameters.AddWithValue('@'+$k,$parametros[$k])}
        $datos=[Data.DataTable]::new();$adaptador=[Data.SqlClient.SqlDataAdapter]::new($cmd)
        try{[void]$adaptador.Fill($datos)}finally{$adaptador.Dispose();$cmd.Dispose()}
        return ,$datos
    }finally{$cn.Dispose()}
}
$opciones=[Transactions.TransactionOptions]::new()
$opciones.IsolationLevel=[Transactions.IsolationLevel]::Serializable
$opciones.Timeout=[TimeSpan]::FromSeconds(90)
$marca='__QA_PROVEEDORES_'+[guid]::NewGuid().ToString('N')
$alcance=[Transactions.TransactionScope]::new([Transactions.TransactionScopeOption]::RequiresNew,$opciones)
try {
    $nombre=$marca+" O'Brien [%_]"
    do{$documento=(Get-Random -Minimum 1 -Maximum 99999999).ToString('00000000')}
    while($proveedor.DocumentoCoincidente($documento).Rows.Count -ne 0)
    $id=$proveedor.Insertar($nombre,$documento,'987654321','Referencia inicial','NATURAL','DNI')
    $ficha=$proveedor.Obtener($id)
    Comprobar ($ficha.Rows.Count -eq 1 -and $ficha.Rows[0]['documento'] -eq $documento) 'Falló el registro o se perdieron ceros del documento.'
    foreach($termino in @($marca,"O'Brien",'%','_','[')) {
        $encontrados=$proveedor.Buscar($termino)
        Comprobar ($encontrados.Select('id_proveedor='+$id).Count -eq 1) ('Falló la búsqueda literal: '+$termino)
    }
    Comprobar ($proveedor.Buscar($marca+'sin_resultados').Rows.Count -eq 0) 'La búsqueda sin coincidencias devolvió proveedores.'
    Comprobar ($proveedor.DocumentoCoincidente($documento,0,'DNI').Rows.Count -eq 1) 'No se detectó el documento existente.'
    Comprobar ($proveedor.DocumentoCoincidente($documento,$id,'DNI').Rows.Count -eq 0) 'La edición detecta su propio documento como duplicado.'
    $version=[byte[]]$ficha.Rows[0]['version_proveedor']
    $proveedor.Actualizar($id,$nombre,$documento,'999888777','Referencia actualizada',$version,'NATURAL','DNI')
    $actual=$proveedor.Obtener($id)
    Comprobar ($actual.Rows[0]['telefono'] -eq '999888777' -and $actual.Rows[0]['direccion'] -eq 'Referencia actualizada') 'No se guardó la modificación.'
    Comprobar ([Convert]::ToBase64String($version) -ne [Convert]::ToBase64String([byte[]]$actual.Rows[0]['version_proveedor'])) 'No cambió la versión del registro.'

    # Inicialización y eventos no destructivos de los formularios, sin mostrarlos.
    Add-Type -AssemblyName System.Windows.Forms
    $app=[Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaPresentacion.exe'))
    $flags=[Reflection.BindingFlags]'Instance,NonPublic'
    $tipoLista=$app.GetType('capaPresentacion.frmListadoProveedores',$true)
    $lista=[Activator]::CreateInstance($tipoLista)
    try {
        $lista.modoSeleccion=$true
        $lista.BindingContext=[Windows.Forms.BindingContext]::new()
        $grid=$lista.Controls.Find('dgvProveedores',$true)[0]
        $grid.BindingContext=$lista.BindingContext
        $null=$tipoLista.GetMethod('frmListadoProveedores_Load',$flags).Invoke($lista,@($null,[EventArgs]::Empty))
        $lista.cargarLista($id)
        $null=$tipoLista.GetMethod('btnSeleccionar_Click',$flags).Invoke($lista,@($null,[EventArgs]::Empty))
        Comprobar ($lista.idProveedorSeleccionado -eq $id -and $lista.nombreProveedorSeleccionado -eq $nombre) 'La selección no devolvió el proveedor correcto.'
        $buscar=$lista.Controls.Find('txtBuscar',$true)[0]
        $buscar.Text=$marca+'sin_resultados'
        $lista.cargarLista()
        Comprobar (-not $lista.Controls.Find('btnModificar',$true)[0].Enabled -and
            -not $lista.Controls.Find('btnEliminar',$true)[0].Enabled -and
            -not $lista.Controls.Find('btnSeleccionar',$true)[0].Enabled) 'Hay acciones habilitadas en un listado vacío.'
    }finally{$lista.Dispose()}
    $tipoRegistro=$app.GetType('capaPresentacion.frmRegistrarProveedores',$true)
    $registro=[Activator]::CreateInstance($tipoRegistro)
    try {
        $registro.idProveedor=$id
        $null=$tipoRegistro.GetMethod('frmRegistrarProveedores_Load',$flags).Invoke($registro,@($null,[EventArgs]::Empty))
        Comprobar ($registro.Controls.Find('txtNombre',$true)[0].Text -eq $nombre) 'La ficha no cargó el proveedor seleccionado.'
        Comprobar (-not $tipoRegistro.GetMethod('hayCambios',$flags).Invoke($registro,@())) 'La ficha recién cargada aparece modificada.'
        $registro.Controls.Find('txtNombre',$true)[0].Text=$nombre+' editado'
        Comprobar ($tipoRegistro.GetMethod('hayCambios',$flags).Invoke($registro,@())) 'No se detectaron cambios pendientes.'
        $persona=$registro.Controls.Find('cmbTipoPersona',$true)[0]
        $persona.SelectedIndex=1
        $tipoDocumento=$registro.Controls.Find('cmbTipoDocumento',$true)[0]
        Comprobar ($tipoDocumento.SelectedItem -eq 'RUC' -and -not $tipoDocumento.Enabled) 'Persona jurídica no exige RUC en la pantalla.'
    }finally{$registro.Dispose()}
    Comprobar (-not $proveedor.TieneCompras($id)) 'Un proveedor nuevo aparece con compras.'
    $proveedor.Eliminar($id)
    Comprobar ($proveedor.Obtener($id).Rows.Count -eq 0) 'No se eliminó el proveedor sin compras.'
    Write-Output 'Alta, edición, búsqueda literal, documento duplicado, selección, ficha, cambios pendientes y eliminación: correctos.'
}finally{$alcance.Dispose()}

foreach($escenario in @('duplicado','version','historial')) {
    $alcance=[Transactions.TransactionScope]::new([Transactions.TransactionScopeOption]::RequiresNew,$opciones)
    try {
        $p=New-Object capaNegocio.Proveedor
        if($escenario -eq 'duplicado') {
            do{$doc=(Get-Random -Minimum 1 -Maximum 99999999).ToString('00000000')}
            while($p.DocumentoCoincidente($doc).Rows.Count -ne 0)
            $id=$p.Insertar($marca,$doc,'','','NATURAL','DNI')
            Rechazar {$p.Insertar($marca+' duplicado',$doc,'','','NATURAL','DNI')} 'documento.*otro proveedor'
        }elseif($escenario -eq 'version') {
            $id=$p.Insertar($marca,'','','','NATURAL','SIN DOCUMENTO')
            $f=$p.Obtener($id);$v=[byte[]]$f.Rows[0]['version_proveedor']
            $p.Actualizar($id,$marca,'','987654321','',$v,'NATURAL','SIN DOCUMENTO')
            Rechazar {$p.Actualizar($id,$marca,'','999888777','',$v,'NATURAL','SIN DOCUMENTO')} 'Otro trabajador'
        }else {
            $id=$p.Insertar($marca,'','','','NATURAL','SIN DOCUMENTO')
            $null=Consultar "INSERT INTO dbo.compras(id_proveedor,proveedor,fecha,total,motivo) VALUES(@id,@nombre,CONVERT(DATE,GETDATE()),0,'REPOSICION');" @{id=$id;nombre=$marca}
            Comprobar ($p.TieneCompras($id)) 'No se reconoció el historial de compras.'
            Rechazar {$p.Eliminar($id)} 'tiene compras'
        }
    }finally{$alcance.Dispose()}
}
$residuos=Consultar "SELECT (SELECT COUNT(*) FROM dbo.proveedores WHERE nombre LIKE @marca) + (SELECT COUNT(*) FROM dbo.compras WHERE proveedor LIKE @marca) AS n;" @{marca=$marca+'%'}
Comprobar ($residuos.Rows[0]['n'] -eq 0) 'Quedaron registros de validación.'

# El esquema y los procedimientos desplegados deben coincidir con la migración.
$sqlMigracion=[IO.File]::ReadAllText((Join-Path $raiz 'recursos/bd/01_proveedores.sql'))
$procedimientos=[regex]::Matches($sqlMigracion,'(?ms)^CREATE PROCEDURE (dbo\.sp_prov_\w+)\b.*?(?=^\s*GO\s*$)')
Comprobar ($procedimientos.Count -eq 6) 'La migración no contiene los seis procedimientos esperados.'
foreach($sp in $procedimientos) {
    $servidor=Consultar 'SELECT OBJECT_DEFINITION(OBJECT_ID(@nombre)) AS definicion;' @{nombre=$sp.Groups[1].Value}
    $esperado=[regex]::Replace($sp.Value.Trim(),'\r\n',"`n")
    $actual=[regex]::Replace(([string]$servidor.Rows[0]['definicion']).Trim(),'\r\n',"`n")
    Comprobar ($actual -eq $esperado) ('El procedimiento desplegado difiere del archivo: '+$sp.Groups[1].Value)
}
Write-Output 'Duplicados, ediciones simultáneas, protección de compras, reversión y seis procedimientos desplegados: correctos.'

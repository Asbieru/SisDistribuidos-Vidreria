param([switch]$Integracion)
$ErrorActionPreference = 'Stop'
$raiz = Split-Path $PSScriptRoot -Parent
Add-Type -AssemblyName System.Data
Add-Type -AssemblyName System.Transactions
[void][Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaDatos.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaNegocio.dll'))
function Comprobar($condicion, [string]$mensaje) { if (-not $condicion) { throw $mensaje } }
function Rechazar([scriptblock]$accion, [string]$mensaje, [string]$motivo='') {
    $rechazado = $false
    try { & $accion | Out-Null } catch {
        if ($motivo -and $_.Exception.Message -notmatch $motivo) { throw }
        $rechazado = $true
    }
    Comprobar $rechazado $mensaje
}
$casos = @(
    @{u='UNIDAD'; n=3; c=6.8; a=$null; b=$null; l=$null; q=3; s=20.4},
    @{u='CAJA'; n=4; c=12.125; a=$null; b=$null; l=$null; q=4; s=48.5},
    @{u='CM2'; n=2; c=0.003; a=100; b=200; l=$null; q=40000; s=120},
    @{u='PIE2'; n=4; c=3.5; a=1; b=7; l=$null; q=0.031; s=0.109},
    @{u='PIE2'; n=3; c=6.8; a=1; b=1; l=$null; q=0.003; s=0.023},
    @{u='CM_LINEAL'; n=3; c=0.125; a=$null; b=$null; l=250.5; q=751.5; s=93.938}
)
foreach ($caso in $casos) {
    $r = [capaNegocio.Compra]::CalcularImportes($caso.u,$caso.n,$caso.c,$caso.a,$caso.b,$caso.l)
    Comprobar ($r.Cantidad -eq [decimal]$caso.q -and $r.Subtotal -eq [decimal]$caso.s) ('Fallo de cálculo: '+$caso.u)
}
foreach ($n in @(-1,0,1.5,1000001)) {
    Rechazar { [capaNegocio.Compra]::CalcularImportes('UNIDAD',$n,1,$null,$null,$null) } 'Se aceptó un conteo inválido.'
}
foreach ($c in @(-1,0,0.0001,100000000)) {
    Rechazar { [capaNegocio.Compra]::CalcularImportes('UNIDAD',1,$c,$null,$null,$null) } 'Se aceptó un costo inválido.'
}
foreach ($medida in @($null,0,-1,1000000,1.0001)) {
    Rechazar { [capaNegocio.Compra]::CalcularImportes('CM2',1,1,$medida,1,$null) } 'Se aceptó una medida inválida.'
}
Rechazar { [capaNegocio.Compra]::CalcularImportes('OTRA',1,1,$null,$null,$null) } 'Se aceptó una unidad desconocida.'
Rechazar { [capaNegocio.Compra]::CalcularImportes('PIE2',1,1,0.001,0.001,$null) } 'Se aceptó una cantidad inferior a la precisión de almacenamiento.'
Rechazar { [capaNegocio.Compra]::CalcularImportes('CM2',1,1,999999.999,999999.999,$null) } 'Se aceptó un importe fuera de rango.'
Write-Output 'Cálculos y límites: correctos (5 unidades, redondeos y entradas inválidas).'
if (-not $Integracion) { return }

$configuracion = New-Object capaDatos.clsConectaBD
$cadena = $configuracion.miConexion.ConnectionString
function Consultar([string]$sql, [hashtable]$parametros=@{}) {
    $cn = [System.Data.SqlClient.SqlConnection]::new($cadena)
    try {
        $cn.Open()
        $cmd = $cn.CreateCommand()
        $cmd.CommandText = $sql
        foreach ($k in $parametros.Keys) { [void]$cmd.Parameters.AddWithValue('@'+$k,$parametros[$k]) }
        $tabla = [Data.DataTable]::new()
        $adaptador = [System.Data.SqlClient.SqlDataAdapter]::new($cmd)
        try { [void]$adaptador.Fill($tabla) } finally { $adaptador.Dispose(); $cmd.Dispose() }
        return ,$tabla
    } finally { $cn.Dispose() }
}
function CrearDetalle {
    $tabla = [Data.DataTable]::new()
    foreach ($columna in @('id_variante','piezas')) { [void]$tabla.Columns.Add($columna,[int]) }
    foreach ($columna in @('cantidad','costo_unitario','subtotal','ancho_cm','alto_cm','largo_cm')) { [void]$tabla.Columns.Add($columna,[decimal]) }
    [void]$tabla.Columns.Add('unidad',[string])
    return ,$tabla
}
function AgregarLinea($tabla, $id, $unidad, $n, $costo, $ancho=$null, $alto=$null, $largo=$null) {
    $r = [capaNegocio.Compra]::CalcularImportes($unidad,$n,$costo,$ancho,$alto,$largo)
    $fila = $tabla.NewRow()
    $fila['id_variante']=$id; $fila['unidad']=$unidad; $fila['piezas']=$n
    $fila['cantidad']=$r.Cantidad; $fila['costo_unitario']=$costo; $fila['subtotal']=$r.Subtotal
    if ($null -ne $ancho) { $fila['ancho_cm']=$ancho }
    if ($null -ne $alto) { $fila['alto_cm']=$alto }
    if ($null -ne $largo) { $fila['largo_cm']=$largo }
    $tabla.Rows.Add($fila)
}
$marca='__QA_COMPRAS_'+[guid]::NewGuid().ToString('N')
$opciones = [Transactions.TransactionOptions]::new()
$opciones.IsolationLevel = [Transactions.IsolationLevel]::Serializable
$opciones.Timeout = [TimeSpan]::FromSeconds(90)
$alcance = [Transactions.TransactionScope]::new([Transactions.TransactionScopeOption]::RequiresNew,$opciones)
try {
    $prov = New-Object capaNegocio.Proveedor
    $idProveedor = $prov.Insertar($marca,'','','','NATURAL','SIN DOCUMENTO')
    $ids=@{}
    foreach ($u in @('PIE2','CM2','CM_LINEAL','CAJA','UNIDAD')) {
        $categoria=if($u -in @('PIE2','CM2')){'VIDRIO'}elseif($u -eq 'CM_LINEAL'){'ALUMINIO'}else{'ACCESORIO'}
        $fixture=Consultar @'
INSERT INTO dbo.productos(categoria,tipo,nombre,activo) VALUES(@categoria,N'VALIDACION',@nombre,1);
DECLARE @p INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.caracteristicas(espesor_cm) VALUES(0.6);
DECLARE @c INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.producto_variante(id_producto,id_caracteristica,unidad_costo,precio_unitario)
    VALUES(@p,@c,@unidad,1);
SELECT CONVERT(INT,SCOPE_IDENTITY()) AS id;
'@ @{categoria=$categoria;nombre=$marca;unidad=$u}
        $ids[$u]=[int]$fixture.Rows[0]['id']
    }
    # Ejecuta selección y Agregar del formulario sin mostrar ventanas ni guardar.
    Add-Type -AssemblyName System.Windows.Forms
    $ensambladoUi=[Reflection.Assembly]::LoadFrom((Join-Path $raiz 'capaPresentacion/bin/Debug/capaPresentacion.exe'))
    $tipoUi=$ensambladoUi.GetType('capaPresentacion.frmRegistrarCompras',$true)
    $formUi=[Activator]::CreateInstance($tipoUi)
    $flags=[Reflection.BindingFlags]'Instance,NonPublic'
    try {
        $variantesUi=[Data.DataTable]::new()
        [void]$variantesUi.Columns.Add('id_variante',[int])
        [void]$variantesUi.Columns.Add('descripcion',[string])
        [void]$variantesUi.Columns.Add('precio_unitario',[decimal])
        [void]$variantesUi.Columns.Add('unidad_costo',[string])
        [void]$variantesUi.Rows.Add($ids['UNIDAD'],$marca,1,'UNIDAD')
        $detalleUi=[Data.DataTable]::new()
        foreach($c in @('id_variante','producto','cantidad','piezas','costo_unitario','subtotal','unidad','ancho_cm','alto_cm','largo_cm','medida')) {
            $tipoCol=if($c -in @('producto','unidad','medida')){[string]}elseif($c -in @('id_variante','piezas')){[int]}else{[decimal]}
            [void]$detalleUi.Columns.Add($c,$tipoCol)
        }
        $tipoUi.GetField('variantes',$flags).SetValue($formUi,$variantesUi)
        $tipoUi.GetField('detalle',$flags).SetValue($formUi,$detalleUi)
        $seleccionUi=$tipoUi.GetMethod('seleccionarVariante',$flags).Invoke($formUi,@($ids['UNIDAD']))
        Comprobar $seleccionUi 'Falló la selección del producto en compras.'
        $nudUi=$formUi.Controls.Find('nudCantidad',$true)[0]
        $nudUi.Value=2
        $null=$tipoUi.GetMethod('btnAgregarDetalle_Click',$flags).Invoke($formUi,@($null,[EventArgs]::Empty))
        Comprobar ($detalleUi.Rows.Count -eq 1 -and $detalleUi.Rows[0]['subtotal'] -eq 2) 'Agregar no calculó correctamente el detalle en pantalla.'
        $stockSinGuardar=Consultar 'SELECT COUNT(*) AS n FROM dbo.inventario_cajas WHERE id_variante=@id;' @{id=$ids['UNIDAD']}
        Comprobar ($stockSinGuardar.Rows[0]['n'] -eq 0) 'Seleccionar o Agregar modificó inventario antes de guardar.'
    } finally { $formUi.Dispose() }
    Write-Output 'Selección y Agregar: cálculo correcto y ningún cambio de inventario antes de Guardar.'
    $det=CrearDetalle
    AgregarLinea $det $ids['PIE2'] 'PIE2' 3 6.8 1 1
    AgregarLinea $det $ids['CM2'] 'CM2' 2 0.003 100 200
    AgregarLinea $det $ids['CM_LINEAL'] 'CM_LINEAL' 3 0.125 $null $null 250.5
    AgregarLinea $det $ids['CAJA'] 'CAJA' 4 12.125
    AgregarLinea $det $ids['UNIDAD'] 'UNIDAD' 3 6.8
    $compra = New-Object capaNegocio.Compra
    $idCompra=$compra.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$det,0,$null,$idProveedor)
    $guardado=Consultar @'
SELECT c.total, (SELECT SUM(subtotal) FROM dbo.compra_detalle WHERE id_compra=c.id_compra) AS suma,
 (SELECT COUNT(*) FROM dbo.inventario_piezas ip JOIN dbo.compra_detalle cd ON cd.id_compra_detalle=ip.id_compra_detalle WHERE cd.id_compra=c.id_compra) AS piezas,
 (SELECT COUNT(*) FROM dbo.movimientos_inventario mi JOIN dbo.compra_detalle cd ON cd.id_compra_detalle=mi.id_compra_detalle WHERE cd.id_compra=c.id_compra AND mi.tipo='ENTRADA') AS entradas
FROM dbo.compras c WHERE c.id_compra=@id;
'@ @{id=$idCompra}
    Comprobar ($guardado.Rows[0]['total'] -eq 282.861 -and $guardado.Rows[0]['suma'] -eq 282.861) 'El total no coincide con los cálculos.'
    Comprobar ($guardado.Rows[0]['piezas'] -eq 8 -and $guardado.Rows[0]['entradas'] -eq 10) 'No se registraron todas las entradas de inventario.'
    foreach ($u in @('CAJA','UNIDAD')) {
        $stock=Consultar 'SELECT stock_cajas FROM dbo.inventario_cajas WHERE id_variante=@id;' @{id=$ids[$u]}
        $esperado=if($u -eq 'CAJA'){4}else{3}
        Comprobar ($stock.Rows[0]['stock_cajas'] -eq $esperado) ('Stock incorrecto: '+$u)
    }
    $historial=$compra.ObtenerDetalleParaEditar($idCompra)
    Comprobar ($historial.Rows.Count -eq 5) 'No se recuperó todo el detalle.'
    Comprobar (($historial.Select("unidad='CAJA'"))[0]['piezas'] -eq 4) 'No se conservó el conteo de cajas.'
    $alterado=$det.Copy()
    $alterado.Rows[0]['cantidad']=99
    Rechazar { $compra.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$alterado,0,$null,$idProveedor) } 'Se aceptó una cantidad manipulada.' 'cantidad facturada'
    $alterado=$det.Copy()
    $alterado.Rows[0]['piezas']=0
    Rechazar { $compra.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$alterado,0,$null,$idProveedor) } 'Se aceptaron cero piezas.' 'debe ser entera'
    $alterado=$det.Copy()
    $alterado.Rows[0]['unidad']='UNIDAD'
    Rechazar { $compra.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$alterado,0,$null,$idProveedor) } 'Se aceptó una unidad manipulada.' 'unidad del producto'
    $alterado=$det.Copy()
    $alterado.ImportRow($alterado.Select("unidad='CAJA'")[0])
    Rechazar { $compra.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$alterado,0,$null,$idProveedor) } 'Se aceptaron cajas repetidas.' 'variante repetida'
    $antes=Consultar 'SELECT COUNT(*) AS n FROM dbo.compras WHERE id_proveedor=@p;' @{p=$idProveedor}
    Comprobar ($antes.Rows[0]['n'] -eq 1) 'Una validación fallida dejó una compra parcial.'

    # Un pedido activo y otro cancelado esperan la misma medida.
    # Solo el activo debe recibir una pieza de la compra siguiente.
    $pendientes=Consultar @'
DECLARE @u INT=(SELECT TOP(1) id_usuario FROM dbo.usuarios ORDER BY id_usuario);
INSERT INTO dbo.clientes(nombre) VALUES(@nombre);
DECLARE @c INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedidos(id_cliente,id_usuario,estado,total) VALUES(@c,@u,'PENDIENTE',10);
DECLARE @pa INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedido_item(id_pedido,tipo,subtotal,total) VALUES(@pa,'VIDRIO_SUELTO',10,10);
DECLARE @ia INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedido_detalle(id_item,id_variante,accion,ancho_cm,alto_cm,precio_unitario_aplicado,subtotal)
 VALUES(@ia,@variante,'COMPRA_NUEVA',30,30,10,10);
DECLARE @da INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedidos(id_cliente,id_usuario,estado,total) VALUES(@c,@u,'CANCELADO',10);
DECLARE @pc INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedido_item(id_pedido,tipo,subtotal,total) VALUES(@pc,'VIDRIO_SUELTO',10,10);
DECLARE @ic INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.pedido_detalle(id_item,id_variante,accion,ancho_cm,alto_cm,precio_unitario_aplicado,subtotal)
 VALUES(@ic,@variante,'COMPRA_NUEVA',30,30,10,10);
SELECT @da AS activo, CONVERT(INT,SCOPE_IDENTITY()) AS cancelado;
'@ @{nombre=$marca;variante=$ids['PIE2']}
    $detallePedido=CrearDetalle
    AgregarLinea $detallePedido $ids['PIE2'] 'PIE2' 2 3.5 30 30
    $compraPedido=New-Object capaNegocio.Compra
    $idCompraPedido=$compraPedido.GuardarCompra($marca,[datetime]::Today,'REPOSICION',$null,$detallePedido,0,$null,$idProveedor)
    $enlace=Consultar @'
SELECT
 (SELECT COUNT(*) FROM dbo.pedido_detalle WHERE id_detalle=@activo AND id_pieza IS NOT NULL AND accion='USA_EXISTENTE') AS activo,
 (SELECT COUNT(*) FROM dbo.pedido_detalle WHERE id_detalle=@cancelado AND id_pieza IS NULL AND accion='COMPRA_NUEVA') AS cancelado,
 (SELECT COUNT(*) FROM dbo.inventario_piezas ip JOIN dbo.compra_detalle cd ON cd.id_compra_detalle=ip.id_compra_detalle WHERE cd.id_compra=@compra AND ip.estado='DISPONIBLE') AS disponibles,
 (SELECT COUNT(*) FROM dbo.movimientos_inventario mi JOIN dbo.inventario_piezas ip ON ip.id_pieza=mi.id_pieza JOIN dbo.compra_detalle cd ON cd.id_compra_detalle=ip.id_compra_detalle WHERE cd.id_compra=@compra AND mi.tipo='SALIDA') AS salidas;
'@ @{activo=$pendientes.Rows[0]['activo'];cancelado=$pendientes.Rows[0]['cancelado'];compra=$idCompraPedido}
    Comprobar ($enlace.Rows[0]['activo'] -eq 1 -and $enlace.Rows[0]['cancelado'] -eq 1 -and
        $enlace.Rows[0]['disponibles'] -eq 1 -and $enlace.Rows[0]['salidas'] -eq 1) 'Falló la asignación a pedidos activos o la exclusión de cancelados.'
    Write-Output 'Guardado compilado: importes, cantidades, piezas, cajas, unidades y movimientos correctos.'
    Write-Output 'Validaciones de guardado y enlace con pedidos: correctos; los pedidos cancelados no consumen piezas.'
} finally {
    # Nunca Complete(): la transacción ambiente revierte todos los datos creados.
    $alcance.Dispose()
}
$restos=Consultar 'SELECT COUNT(*) AS n FROM dbo.proveedores WHERE nombre=@nombre;' @{nombre=$marca}
Comprobar ($restos.Rows[0]['n'] -eq 0) 'La prueba dejó datos residuales.'
Write-Output 'Reversión verificada: no quedaron proveedores ni compras de validación.'

foreach ($escenario in @('editar','eliminar','fallo_sql')) {
    $marcaProteccion='__QA_COMPRAS_'+[guid]::NewGuid().ToString('N')
    $transaccionProteccion=[Transactions.TransactionScope]::new([Transactions.TransactionScopeOption]::RequiresNew,$opciones)
    try {
        $proveedorProteccion=New-Object capaNegocio.Proveedor
        $idProteccion=$proveedorProteccion.Insertar($marcaProteccion,'','','','NATURAL','SIN DOCUMENTO')
        $fixture=Consultar @'
INSERT INTO dbo.productos(categoria,tipo,nombre,activo) VALUES('ACCESORIO','VALIDACION',@nombre,1);
DECLARE @p INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.caracteristicas(medida_cm) VALUES(1);
DECLARE @c INT=CONVERT(INT,SCOPE_IDENTITY());
INSERT INTO dbo.producto_variante(id_producto,id_caracteristica,unidad_costo,precio_unitario)
 VALUES(@p,@c,'UNIDAD',1);
SELECT CONVERT(INT,SCOPE_IDENTITY()) AS id;
'@ @{nombre=$marcaProteccion}
        $detalleProteccion=CrearDetalle
        AgregarLinea $detalleProteccion ([int]$fixture.Rows[0]['id']) 'UNIDAD' 2 1
        $compraProteccion=New-Object capaNegocio.Compra
        if ($escenario -eq 'fallo_sql') {
            $null=Consultar 'INSERT INTO dbo.inventario_cajas(id_variante,stock_cajas) VALUES(@id,2147483647);' @{id=[int]$fixture.Rows[0]['id']}
            Rechazar { $compraProteccion.GuardarCompra($marcaProteccion,[datetime]::Today,'REPOSICION',$null,$detalleProteccion,0,$null,$idProteccion) } 'No se rechazó el desbordamiento de stock.' 'desbordamiento|overflow|8115'
        } else {
            $idCompraProteccion=$compraProteccion.GuardarCompra($marcaProteccion,[datetime]::Today,'REPOSICION',$null,$detalleProteccion,0,$null,$idProteccion)
            if ($escenario -eq 'editar') {
                Rechazar { $compraProteccion.GuardarCompra($marcaProteccion,[datetime]::Today,'REPOSICION',$null,$detalleProteccion,$idCompraProteccion,$null,$idProteccion) } 'Se permitió modificar una compra que ya ingresó stock.' 'inventario asociados'
            } else {
                Rechazar { $compraProteccion.EliminarCompra($idCompraProteccion) } 'Se permitió eliminar una compra que ya ingresó stock.' 'inventario asociado'
            }
        }
    } finally { $transaccionProteccion.Dispose() }
    $restos=Consultar 'SELECT COUNT(*) AS n FROM dbo.proveedores WHERE nombre=@nombre;' @{nombre=$marcaProteccion}
    Comprobar ($restos.Rows[0]['n'] -eq 0) ('No se revirtió el escenario: '+$escenario)
}
Write-Output 'Protecciones de edición/eliminación y reversión ante fallo SQL: correctas.'

$auditoria=Consultar @'
SELECT
 (SELECT COUNT(*) FROM dbo.compras WHERE id_proveedor IS NULL) AS compras_sin_proveedor,
 (SELECT COUNT(*) FROM dbo.compra_detalle cd JOIN dbo.producto_variante pv ON pv.id_variante=cd.id_variante
   WHERE pv.unidad_costo IN ('CAJA','UNIDAD') AND NOT EXISTS(
    SELECT 1 FROM dbo.movimientos_inventario mi WHERE mi.id_compra_detalle=cd.id_compra_detalle AND mi.tipo='ENTRADA')) AS detalles_historicos_sin_entrada,
 (SELECT COUNT(*) FROM dbo.proveedores WHERE nombre LIKE '__QA_COMPRAS_%') AS residuos;
'@
Comprobar ($auditoria.Rows[0]['residuos'] -eq 0) 'Se encontraron registros residuales de validación.'
Write-Output ('Auditoría histórica: compras sin proveedor = '+$auditoria.Rows[0]['compras_sin_proveedor']+
    '; detalles de cajas/unidades sin entrada asociada = '+$auditoria.Rows[0]['detalles_historicos_sin_entrada']+'. No se modificó el historial.')

# Validación de compras y proveedores

Estas comprobaciones se limitan a compras y a la integración de proveedores. No cambian el archivo de conexión, las fórmulas de cotización ni el registro de pedidos.

## Ejecutar

Compilar primero la solución en Debug. Desde la raíz del repositorio:

```powershell
# Solo cálculos; no necesita conectar con la base.
powershell.exe -NoProfile -STA -File validaciones/Compras.ps1

# Formulario y guardado compilado contra la base configurada.
powershell.exe -NoProfile -STA -File validaciones/Compras.ps1 -Integracion

# Validaciones de datos de proveedores, sin conectar:
powershell.exe -NoProfile -STA -File validaciones/Proveedores.ps1

# Mantenimiento, formularios, integración y coincidencia de los procedimientos SQL:
powershell.exe -NoProfile -STA -File validaciones/Proveedores.ps1 -Integracion
```

La integración crea proveedores, productos, compras y pedidos temporales dentro de transacciones ambiente que nunca se confirman. Comprueba la reversión al terminar. Necesita permisos para insertar datos; no debe interrumpirse deliberadamente durante las pruebas. Si la conexión se pierde, SQL revierte las transacciones abiertas.

## Casos cubiertos

- Cálculos para CM2, PIE2, CM_LINEAL, CAJA y UNIDAD.
- Cantidades enteras desde 1; costos y medidas positivos; precisión y límites de almacenamiento.
- Redondeo final de subtotales, conservando el área exacta durante el cálculo.
- Seleccionar un producto y agregarlo al detalle no modifica inventario.
- Guardar ingresa cada pieza física y suma stock de cajas/unidades, con movimientos vinculados al detalle.
- Importes persistidos, suma de detalles y total de cabecera coincidentes.
- Rechazo de cantidades o unidades manipuladas y accesorios duplicados.
- Enlace con pedidos activos; exclusión de pedidos cancelados y conservación de piezas sobrantes.
- Bloqueo de modificación/eliminación de compras con movimientos de inventario.
- Fallo SQL provocado después de insertar el detalle: reversión completa y propagación del error, sin informar un guardado exitoso.
- Auditoría de compras sin proveedor y de cajas/unidades sin entradas asociadas. No corrige automáticamente registros históricos.
- Proveedores: formato de persona/documento y teléfono, límites de campos, alta,
  modificación, búsquedas literales, selección, ficha, detección de cambios,
  documentos duplicados, concurrencia, eliminación protegida y coincidencia de los
  seis procedimientos desplegados con la migración versionada. Las pruebas no
  verifican la identidad real ni el registro fiscal de DNI/RUC.

PIE2 conserva la equivalencia **comercial** de 900 cm² utilizada por el proyecto. No representa la conversión geométrica exacta del pie cuadrado internacional. Cambiar esa regla requiere confirmación del negocio y revisión coordinada de cotización/pedidos.

## Catálogo simulado de proveedores

`recursos/bd/02_proveedores_simulados.sql` reemplaza exclusivamente los nombres provisionales `doce`, `los` y `trece`, si no tienen documento registrado. Conserva los IDs y no reescribe los nombres históricos de las compras. Si los nombres comerciales ya existen, no los duplica ni sobrescribe sus modificaciones posteriores.

Los tres nombres comerciales y sus referencias de ubicación son **ficticios**. No se inventan documentos fiscales ni teléfonos: se registran sin documento. No representan empresas verificadas.

```powershell
# Validación con reversión:
powershell.exe -NoProfile -File validaciones/ProveedoresSimulados.ps1

# Aplicación explícita del catálogo:
powershell.exe -NoProfile -File validaciones/ProveedoresSimulados.ps1 -Aplicar
```

La comprobación automatizada de los controles no sustituye una revisión visual con F5 para verificar tamaños, desplazamiento de columnas y comportamiento al usar el formulario normalmente.

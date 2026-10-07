# Pruebas de Clientes

Desde la carpeta raíz del repositorio:

```powershell
powershell.exe -STA -NoProfile -ExecutionPolicy Bypass -File pruebas/Clientes/VerificarClientes.ps1
```

Requiere Windows PowerShell 5.1, SQL Server/LocalDB con permiso para crear
bases temporales, MSBuild y las referencias de .NET Framework 4.7.2 del
proyecto. No requiere un paquete de pruebas ni bibliotecas nuevas.

Opciones: `-Servidor`, `-MSBuild` para indicar el compilador, y
`-ConservarResultados` para conservar resultados y capturas fuera del
repositorio. El script muestra su ubicación bajo AppData/Local.

## Aislamiento

Cada ejecución crea una base `vidrieria_clientes_pruebas_<GUID>` y una copia
temporal del código actual. Cambia la conexión únicamente en esa copia y
compila las tres capas. Nunca modifica `clsConectaBD` en el repositorio ni
inserta datos en la base `vidrieria` de la aplicación.

Al finalizar, incluso ante un fallo, elimina solamente la base temporal con
el nombre generado. También elimina la carpeta temporal, salvo cuando se
indica `-ConservarResultados`. Las rutas y el nombre se verifican antes de
limpiar. Los formularios de prueba se abren fuera de la pantalla en el mismo
proceso de prueba y se cierran al terminar.

## Cobertura

- Instalación repetida y conservación de datos y versiones.
- Compilación de las tres capas con los archivos actuales.
- Operaciones de clientes, campos opcionales, límites y teléfonos.
- Unicode, comillas, búsquedas literales y entradas SQL como datos.
- Documento coincidente excluyendo al cliente editado.
- Dos editores con la misma versión: el segundo cambio se rechaza.
- Eliminación protegida para clientes con pedidos.
- Pedidos sin pagos, pagos repartidos, cancelaciones y sobrepagos.
- Consulta de pagos limitada al cliente del pedido.
- Formularios en ambos temas y visualización de pedidos y pagos.
- Selector modal utilizado desde Cotización y desde la ficha comercial.
- Cambio de cliente que elimina de la pantalla los pagos del anterior.

Las pruebas de formulario comprueban carga, selección y enlaces reales.
Los mensajes de confirmación de documentos y recarga se comprueban también
manualmente siguiendo `recursos/README_clientes.md`.

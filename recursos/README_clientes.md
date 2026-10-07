# Clientes y ficha comercial

Este aporte completa el mantenimiento de Clientes y añade una ficha de
consulta de pedidos y pagos. Reutiliza `clientes`, `pedidos`, `pagos`,
`clsMantenimiento`, `Temas` y `UtilFormularios`. No cambia las utilidades
compartidas ni los módulos de inventario, compras y usuarios.

## Instalación en cada equipo

La base debe contener el esquema original del equipo. No volver a ejecutar
el esquema base sobre una base ya instalada. Desde la carpeta del repositorio,
en Windows PowerShell:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File recursos/InstalarClientes.ps1
```

El instalador verifica las tablas necesarias y aplica, en orden,
`bd/04_modulo_clientes.sql` y `bd/05_ficha_comercial_clientes.sql`.
También se pueden ejecutar ambos archivos en SQL Server Management Studio,
siempre en ese orden y en la instancia que utiliza la aplicación.

El script 05 agrega `clientes.version_cliente` de tipo `ROWVERSION`, actualiza
los procedimientos del mantenimiento y añade consultas de documentos,
historial y pagos. Hay ocho procedimientos `sp_cli_*` en total. La instalación
es repetible y conserva los registros y sus versiones; no modifica importes,
estados, documentos ni relaciones existentes.

Para otra instancia o catálogo, indicar `-Servidor` y `-BaseDatos` al instalador.
Esto selecciona dónde instalar SQL; no cambia la conexión de la aplicación.
Hacer pull trae los archivos, pero no ejecuta la instalación de la base.

`frmHistorialClientes` ya está registrado en `capaPresentacion.vbproj`.
No requiere bibliotecas adicionales. Compilar y ejecutar `capaPresentacion`
como proyecto de inicio; abrir **Mantenimiento > Clientes**.

### Conexión local del proyecto

La conexión compartida utiliza `(localdb)\MSSQLLocalDB` con autenticación
Windows y base `vidrieria`. En SQL Server Management Studio se puede
seleccionar ese mismo servidor para consultar los datos que usa la aplicación.
Si se restaura una copia desde otra instancia, ambas bases son independientes:
los cambios posteriores no se sincronizan automáticamente.

## Funciones y reglas

- Registrar y editar nombre, documento, teléfono y dirección.
- Nombre obligatorio, máximo 150 caracteres.
- Documento y teléfono opcionales, máximo 20; dirección opcional, máximo 255.
- Guardar campos opcionales vacíos como `NULL`.
- Buscar por nombre, documento o teléfono mediante Buscar o Enter.
- Eliminar solo clientes sin pedidos. La base vuelve a comprobarlo
  dentro de una transacción para conservar el historial comercial.
- Usar la paleta clara u oscura recibida desde el menú.
- Teléfono opcional: números, espacios, `+`, paréntesis y guiones; debe
  contener al menos un número. No se impone longitud de DNI, RUC o teléfono
  nacional porque no hay una política de tipos de documento en el esquema.
- Advertir si otro cliente usa el mismo documento, excluyendo al cliente
  que se está editando. El usuario puede revisar y cancelar, o confirmar.
  No se agrega una restricción única ni se corrigen datos anteriores.
- Detectar ediciones antiguas mediante la versión leída al abrir el registro.
  Si otro trabajador guardó cambios, el segundo guardado se rechaza sin
  sobrescribirlo. **Recargar** pide confirmación antes de reemplazar los
  datos de la pantalla; después se pueden revisar y volver a editar.

El esquema existente no exige documento único ni define tipos de documento.
Este módulo conserva esas reglas y las firmas de los métodos originales.

La firma antigua de `Cliente.Actualizar` permanece para llamadas puntuales y
consulta la versión actual antes de actualizar. Los formularios de edición
deben usar la sobrecarga con `versionOriginal As Byte()`, tal como hace
`frmRegistrarClientes`, para detectar cambios desde que se abrió la pantalla.

## Ficha comercial

Seleccionar un cliente en el listado y pulsar **Ver historial**.
`frmHistorialClientes` muestra pedidos por estado, vendedor, importe total,
pagos registrados, saldo y excedente. Seleccionar un pedido carga sus pagos,
métodos y comprobante. **Elegir cliente** reutiliza el selector; **Actualizar**
vuelve a consultar los datos actuales sin modificar registros comerciales.

Reglas de cálculo:

- Pagado = suma de las cabeceras `pagos.monto_total` de cada pedido.
- Saldo = máximo entre total menos pagado y cero, salvo pedidos cancelados,
  cuyo saldo se muestra como cero.
- Excedente = máximo entre pagado menos total y cero.
- El resumen corresponde al filtro visible y excluye pedidos cancelados.
- El desglose tiene una fila por método. El total de una cabecera puede
  repetirse en sus filas; no se suman esas repeticiones para calcular saldo.
- Pedidos cancelados conservan sus pagos visibles para consulta. No se
  interpretan como devoluciones ni se registra ningún reembolso.

Ejemplo: pedido de S/. 100, pago de S/. 40 repartido entre efectivo y Yape,
y otro pago de S/. 10: pagado S/. 50 y saldo S/. 50, independientemente
del número de filas del desglose. Si el cliente no tiene pedidos, la ficha
lo indica y muestra un resumen en cero.

## Enlace a Cotización y futuros pedidos

La pantalla existente `frmCotizarVentana` ahora tiene **Seleccionar cliente**
y **Ver historial**. El primer botón abre el listado en modo selección y
conserva el ID y nombre elegidos. Cancelar conserva la selección anterior.
El segundo abre la ficha del cliente. El menú transmite el tema vigente.

Esta integración no completa el cálculo de cotización ni el registro del
pedido: esa lógica seguía vacía en el proyecto original. Los controles y
las responsabilidades existentes se conservan.

El responsable del formulario comercial puede abrir el mismo listado:

```vbnet
Using formulario As New frmListadoClientes With {
    .modoSeleccion = True,
    .tema = "CLARO"
}
    If formulario.ShowDialog(Me) = DialogResult.OK Then
        Dim idCliente As Integer = formulario.idClienteSeleccionado
        Dim nombreCliente As String = formulario.nombreClienteSeleccionado
    End If
End Using
```

`DialogResult.OK` entrega el ID y nombre del cliente. Cancelar no selecciona
ninguno. El selector no crea pedidos, cobra pagos ni modifica inventario;
la pantalla comercial conserva el ID y lo utiliza en su propio flujo.

## Cómo probar el aporte

1. Registrar, buscar y editar un cliente desde Mantenimiento > Clientes.
2. Registrar otro con el mismo documento: comprobar la advertencia y sus
   opciones. Probar un teléfono con letras para comprobar la validación.
3. Abrir dos ejecuciones de la aplicación y editar el mismo cliente en
   ambas. Guardar en la primera; intentar guardar en la segunda. Comprobar
   el rechazo, recargar y revisar los datos antes de guardar de nuevo.
4. Pulsar Ver historial. Si no hay pedidos, comprobar el estado vacío.
   La ficha no crea pedidos de demostración en la base de trabajo.
5. Abrir Cotización desde la barra del menú. Seleccionar un cliente,
   consultar su historial y cancelar otra selección sin perder la anterior.
6. Repetir las pantallas con los temas claro y oscuro.

Para verificar pedidos, pagos, cancelaciones y conflictos sin cargar datos
de prueba en `vidrieria`, ejecutar las pruebas versionadas:

```powershell
powershell.exe -STA -NoProfile -ExecutionPolicy Bypass -File pruebas/Clientes/VerificarClientes.ps1
```

Consultar `../pruebas/Clientes/README.md` para requisitos y aislamiento.

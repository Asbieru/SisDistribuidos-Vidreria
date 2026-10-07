# Mantenimiento de Clientes

Este aporte completa `Cliente.vb`, `frmRegistrarClientes` y
`frmListadoClientes`, y conecta la opción Clientes del menú existente.
Reutiliza la tabla `clientes`, `clsMantenimiento`, `Temas` y `UtilFormularios`.

## Instalación

Ejecutar `bd/04_modulo_clientes.sql` en la base `vidrieria` de la misma
instancia configurada en `clsConectaBD`. Requiere el esquema base existente.
El script puede repetirse: crea o actualiza únicamente los procedimientos
`sp_cli_listar`, `sp_cli_obtener`, `sp_cli_guardar`, `sp_cli_tiene_pedidos`
y `sp_cli_eliminar`. Al instalarlo conserva las tablas y los datos.

Los formularios ya estaban incluidos en el proyecto; no hay referencias
ni recursos nuevos que registrar. Compilar la solución y abrir
Mantenimiento > Clientes.

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

El esquema existente no exige documento único ni define tipos de documento.
Este módulo conserva esas reglas y las firmas de los métodos originales.

## Selección para Pedidos o Cotización

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

## Comprobación manual

Registrar un cliente; editarlo; buscarlo por cada campo; comprobar límites
y campos opcionales; eliminar un cliente sin pedidos; intentar eliminar
uno con pedidos; comprobar doble clic y selección modal; repetir con los
temas claro y oscuro.

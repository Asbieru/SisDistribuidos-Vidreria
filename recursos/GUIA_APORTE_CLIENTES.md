# Guía breve para explicar mi aporte

## Qué hice

Mi módulo es **Mantenimiento de Clientes y Ficha Comercial**. Ya existían
la tabla y una clase básica de clientes; los formularios estaban vacíos.
Completé el registro, listado, búsqueda, edición y eliminación protegida.
Añadí el historial de pedidos y pagos y la selección de cliente en Cotización.

## Cómo interactúa con el proyecto

**Formulario → Cliente.vb → clsMantenimiento → SQL Server → formulario.**

- Presentación: muestra los campos, botones, mensajes y resultados.
- Negocio: valida los datos y solicita las operaciones del cliente.
- Datos: reutiliza la conexión y las utilidades existentes del equipo.
- SQL: guarda clientes y consulta sus pedidos, vendedor, pagos y comprobantes.

La ficha consulta el flujo comercial existente. Inventario conserva su propio
módulo. Cotización recibe el ID y nombre del cliente; su cálculo y el registro
del pedido siguen pendientes.

## Demostración corta

1. Iniciar sesión y abrir **Mantenimiento > Clientes**.
2. Registrar, buscar y modificar un cliente.
3. Pulsar **Ver historial**; seleccionar un pedido para ver sus pagos.
   Si no hay pedidos, se muestra el estado vacío.
4. Abrir Cotización y pulsar **Seleccionar cliente**.

También puedo demostrar la advertencia de documento repetido y que una
edición antigua no sobrescribe lo guardado por otro trabajador.

## Valor del aporte

Permite atender clientes desde la aplicación, consultar su historial y
conservar sus pedidos. Mantiene las tres capas, la paleta y las utilidades
del equipo. Los compañeros instalan los scripts 04 y 05 en ese orden,
o ejecutan `recursos/InstalarClientes.ps1`.

## Ideas futuras

1. Completar el cálculo y guardado de cotizaciones/pedidos.
2. Integrar pedidos, pagos e inventario con operaciones consistentes.
3. Acordar tipos de documento y reforzar el acceso de usuarios.

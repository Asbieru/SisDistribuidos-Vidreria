-- ============================================================
-- Correccion UNICA de fechas guardadas con la hora del servidor
-- de hosting (UTC) en lugar de la hora de Peru (UTC-5).
--
-- Sintoma: un pedido registrado a las 15:51 en Lima aparece con
--         22/11/2026 20:51 (exactamente +5 horas).
--
-- Uso:
--   1) Ejecutar tal cual: @aplicar = 0 muestra cuantas filas se
--      corregirian y no modifica nada.
--   2) Cambiar @aplicar = 1 y ejecutar de nuevo para aplicar.
--
-- Solo toca fechas que estan en el FUTURO respecto a la hora de
-- Peru actual y dentro de las proximas 6 horas, que es el rango
-- donde cae el desfase UTC->Peru.
-- ============================================================

USE vidrieria;
GO

DECLARE @aplicar BIT = 0;              -- <- cambiar a 1 para aplicar
DECLARE @ahora DATETIME2 = dbo.fn_hora_peru();
DECLARE @hasta DATETIME2 = DATEADD(HOUR, 6, @ahora);

SELECT 'pedidos.fecha' AS columna, COUNT(*) AS filas_a_corregir
FROM dbo.pedidos WHERE fecha > @ahora AND fecha < @hasta
UNION ALL
SELECT 'pedidos.fecha_termino', COUNT(*)
FROM dbo.pedidos WHERE fecha_termino > @ahora AND fecha_termino < @hasta
UNION ALL
SELECT 'inventario_piezas.fecha_ingreso', COUNT(*)
FROM dbo.inventario_piezas WHERE fecha_ingreso > @ahora AND fecha_ingreso < @hasta
UNION ALL
SELECT 'movimientos_inventario.fecha', COUNT(*)
FROM dbo.movimientos_inventario WHERE fecha > @ahora AND fecha < @hasta;

IF @aplicar = 1
BEGIN
    UPDATE dbo.pedidos
       SET fecha = DATEADD(HOUR, -5, fecha)
     WHERE fecha > @ahora AND fecha < @hasta;

    UPDATE dbo.pedidos
       SET fecha_termino = DATEADD(HOUR, -5, fecha_termino)
     WHERE fecha_termino > @ahora AND fecha_termino < @hasta;

    UPDATE dbo.inventario_piezas
       SET fecha_ingreso = DATEADD(HOUR, -5, fecha_ingreso)
     WHERE fecha_ingreso > @ahora AND fecha_ingreso < @hasta;

    UPDATE dbo.movimientos_inventario
       SET fecha = DATEADD(HOUR, -5, fecha)
     WHERE fecha > @ahora AND fecha < @hasta;
END
GO

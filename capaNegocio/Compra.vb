Imports capaDatos
Imports System.Data
Imports System.Collections.Generic
Imports System.Text

Public Class Compra
    Private objMan As New clsMantenimiento

    Public Function Listar() As DataTable
        Return objMan.listarComando(
            "select * from compras order by fecha desc, id_compra desc")
    End Function

    Public Function ListarPorFecha(desde As Date, hasta As Date) As DataTable
        Return objMan.consultarConParametros(
            "select * from compras where fecha between @desde and @hasta " &
            "order by fecha desc, id_compra desc",
            New Dictionary(Of String, Object) From {
                {"desde", desde.Date},
                {"hasta", hasta.Date}
            })
    End Function

    Public Function Insertar(proveedor As String, fecha As Date,
                             total As Decimal, motivo As String,
                             idPedidoOrigen As Integer?) As Integer

        Dim parametros As New Dictionary(Of String, Object) From {
            {"proveedor", proveedor},
            {"fecha", fecha.Date},
            {"total", total},
            {"motivo", motivo},
            {"pedido", If(idPedidoOrigen.HasValue,
                          CType(idPedidoOrigen.GetValueOrDefault(), Object),
                          DBNull.Value)}
        }

        Dim dt As DataTable = objMan.consultarConParametros(
            "insert into compras " &
            "(proveedor, fecha, total, motivo, id_pedido_origen) " &
            "values (@proveedor, @fecha, @total, @motivo, @pedido); " &
            "select convert(int, scope_identity()) as id",
            parametros)

        Return CInt(dt.Rows(0)("id"))
    End Function

    Public Function ListarPorCompra(idCompra As Integer) As DataTable
        Return objMan.consultarConParametros(
            "select * from compra_detalle where id_compra=@id",
            New Dictionary(Of String, Object) From {{"id", idCompra}})
    End Function

    Public Function Insertar(idCompra As Integer, idVariante As Integer,
                             cantidad As Decimal,
                             costoUnitario As Decimal) As Integer

        Dim subtotal As Decimal = Decimal.Round(
            cantidad * costoUnitario, 2, MidpointRounding.AwayFromZero)

        Dim dt As DataTable = objMan.consultarConParametros(
            "insert into compra_detalle " &
            "(id_compra, id_variante, cantidad, costo_unitario, subtotal) " &
            "values (@compra, @variante, @cantidad, @costo, @subtotal); " &
            "select convert(int, scope_identity()) as id",
            New Dictionary(Of String, Object) From {
                {"compra", idCompra},
                {"variante", idVariante},
                {"cantidad", cantidad},
                {"costo", costoUnitario},
                {"subtotal", subtotal}
            })

        Return CInt(dt.Rows(0)("id"))
    End Function

    ' Guarda cabecera y detalle en una sola transacción.
    Public Function GuardarCompra(proveedor As String, fecha As Date,
                              motivo As String,
                              idPedidoOrigen As Integer?,
                              detalle As DataTable,
                              Optional idCompra As Integer = 0) As Integer

        If String.IsNullOrWhiteSpace(proveedor) Then
            Throw New Exception("Ingrese el proveedor.")
        End If

        If proveedor.Trim().Length > 150 Then
            Throw New Exception("El proveedor admite hasta 150 caracteres.")
        End If

        If detalle Is Nothing OrElse detalle.Rows.Count = 0 Then
            Throw New Exception("Agregue al menos un producto.")
        End If

        ' Cada detalle utiliza cuatro parámetros.
        If detalle.Rows.Count > 500 Then
            Throw New Exception("La compra admite hasta 500 detalles.")
        End If

        If motivo <> "REPOSICION" AndAlso
           motivo <> "REEMPLAZO_DEFECTO" Then
            Throw New Exception("Seleccione un motivo válido.")
        End If

        If motivo = "REEMPLAZO_DEFECTO" Then
            If Not idPedidoOrigen.HasValue OrElse
               idPedidoOrigen.Value <= 0 Then
                Throw New Exception("Indique el pedido de origen.")
            End If
        Else
            idPedidoOrigen = Nothing
        End If

        Dim parametros As New Dictionary(Of String, Object) From {
            {"proveedor", proveedor.Trim()},
            {"fecha", fecha.Date},
            {"motivo", motivo},
            {"pedido", If(idPedidoOrigen.HasValue,
                          CType(idPedidoOrigen.GetValueOrDefault(), Object),
                          DBNull.Value)}
        }

        Dim sql As New StringBuilder()
        sql.AppendLine("SET XACT_ABORT ON;")
        sql.AppendLine("BEGIN TRY")
        sql.AppendLine("BEGIN TRANSACTION;")

        If motivo = "REEMPLAZO_DEFECTO" Then
            sql.AppendLine(
                "IF NOT EXISTS (SELECT 1 FROM pedidos WHERE id_pedido=@pedido)")
            sql.AppendLine(
                "THROW 50001, 'El pedido de origen no existe.', 1;")
        End If

        If idCompra < 0 Then
            Throw New Exception("El ID de compra no es válido.")
        End If

        parametros.Add("idEditar", idCompra)

        If idCompra = 0 Then
            sql.AppendLine(
        "INSERT INTO compras " &
        "(proveedor, fecha, total, motivo, id_pedido_origen) " &
        "VALUES (@proveedor, @fecha, @total, @motivo, @pedido);")

            sql.AppendLine(
        "DECLARE @idCompra INT = CONVERT(INT, SCOPE_IDENTITY());")
        Else
            sql.AppendLine("DECLARE @idCompra INT = @idEditar;")

            sql.AppendLine(
        "IF NOT EXISTS (SELECT 1 FROM compras WITH (UPDLOCK, HOLDLOCK) " &
        "WHERE id_compra=@idCompra)")
            sql.AppendLine(
        "THROW 50002, 'La compra ya no existe.', 1;")

            ' Bloquear sus detalles mientras se revisan y reemplazan.
            sql.AppendLine("DECLARE @cantidadDetalles INT;")
            sql.AppendLine(
        "SELECT @cantidadDetalles=COUNT(*) " &
        "FROM compra_detalle WITH (UPDLOCK, HOLDLOCK) " &
        "WHERE id_compra=@idCompra;")

            sql.AppendLine(
        "IF EXISTS (" &
        "SELECT 1 FROM inventario_piezas ip " &
        "INNER JOIN compra_detalle cd " &
        "ON cd.id_compra_detalle=ip.id_compra_detalle " &
        "WHERE cd.id_compra=@idCompra) " &
        "OR EXISTS (" &
        "SELECT 1 FROM movimientos_inventario mi " &
        "INNER JOIN compra_detalle cd " &
        "ON cd.id_compra_detalle=mi.id_compra_detalle " &
        "WHERE cd.id_compra=@idCompra)")

            sql.AppendLine(
        "THROW 50003, " &
        "'Esta compra tiene ingresos de inventario asociados y no puede modificarse desde este formulario.', 1;")

            sql.AppendLine(
        "UPDATE compras SET proveedor=@proveedor, fecha=@fecha, " &
        "total=@total, motivo=@motivo, id_pedido_origen=@pedido " &
        "WHERE id_compra=@idCompra;")

            sql.AppendLine(
        "DELETE FROM compra_detalle WHERE id_compra=@idCompra;")
        End If

        Dim total As Decimal = 0
        Dim indice As Integer = 0
        Dim variantes As New HashSet(Of Integer)()

        For Each fila As DataRow In detalle.Rows
            Dim idVariante As Integer = CInt(fila("id_variante"))
            Dim cantidad As Decimal = CDec(fila("cantidad"))
            Dim costo As Decimal = CDec(fila("costo_unitario"))

            If idVariante <= 0 Then
                Throw New Exception("Seleccione una variante válida.")
            End If

            If Not variantes.Add(idVariante) Then
                Throw New Exception("Hay una variante repetida en el detalle.")
            End If

            If cantidad <= 0 OrElse costo <= 0 Then
                Throw New Exception(
                    "La cantidad y el costo deben ser mayores a cero.")
            End If

            If cantidad > 99999999.99D OrElse costo > 99999999.99D Then
                Throw New Exception(
                    "La cantidad o el costo supera el límite permitido.")
            End If

            If cantidad <> Decimal.Round(cantidad, 2) OrElse
               costo <> Decimal.Round(costo, 2) Then
                Throw New Exception(
                    "La cantidad y el costo admiten hasta dos decimales.")
            End If

            Dim subtotal As Decimal = Decimal.Round(
                cantidad * costo, 2, MidpointRounding.AwayFromZero)

            If subtotal > 99999999.99D Then
                Throw New Exception(
                    "Un subtotal supera el límite de la base de datos.")
            End If

            total += subtotal
            Dim sufijo As String = indice.ToString()

            parametros.Add("variante" & sufijo, idVariante)
            parametros.Add("cantidad" & sufijo, cantidad)
            parametros.Add("costo" & sufijo, costo)
            parametros.Add("subtotal" & sufijo, subtotal)

            sql.AppendLine(
                "INSERT INTO compra_detalle " &
                "(id_compra, id_variante, cantidad, costo_unitario, subtotal) " &
                "VALUES (@idCompra, @variante" & sufijo &
                ", @cantidad" & sufijo & ", @costo" & sufijo &
                ", @subtotal" & sufijo & ");")

            indice += 1
        Next

        If total > 99999999.99D Then
            Throw New Exception(
                "El total supera el límite de la base de datos.")
        End If

        parametros.Add("total", total)

        sql.AppendLine("COMMIT TRANSACTION;")
        sql.AppendLine("SELECT @idCompra AS id_compra;")
        sql.AppendLine("END TRY")
        sql.AppendLine("BEGIN CATCH")
        sql.AppendLine("IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;")
        sql.AppendLine("THROW;")
        sql.AppendLine("END CATCH;")

        Dim resultado As DataTable =
            objMan.consultarConParametros(sql.ToString(), parametros)

        Return CInt(resultado.Rows(0)("id_compra"))
    End Function

    Public Function ObtenerCompra(idCompra As Integer) As DataTable
        Return objMan.consultarConParametros(
            "SELECT * FROM compras WHERE id_compra=@id",
            New Dictionary(Of String, Object) From {{"id", idCompra}})
    End Function

    Public Function ObtenerDetalleParaEditar(idCompra As Integer) As DataTable
        Return objMan.consultarConParametros(
            "SELECT cd.id_variante, " &
            "p.nombre + ' | Variante ' + CONVERT(nvarchar(20), pv.id_variante) " &
            "+ ' | ' + pv.unidad_costo AS producto, " &
            "cd.cantidad, cd.costo_unitario, cd.subtotal " &
            "FROM compra_detalle cd " &
            "INNER JOIN producto_variante pv ON pv.id_variante=cd.id_variante " &
            "INNER JOIN productos p ON p.id_producto=pv.id_producto " &
            "WHERE cd.id_compra=@id ORDER BY cd.id_compra_detalle",
            New Dictionary(Of String, Object) From {{"id", idCompra}})
    End Function

    Public Sub EliminarCompra(idCompra As Integer)
        If idCompra <= 0 Then
            Throw New Exception("Seleccione una compra válida.")
        End If

        Dim sql As String =
            "SET XACT_ABORT ON; " &
            "BEGIN TRY " &
            "BEGIN TRANSACTION; " &
            "IF NOT EXISTS (" &
            "SELECT 1 FROM compras WITH (UPDLOCK, HOLDLOCK) " &
            "WHERE id_compra=@id) " &
            "THROW 50001, 'La compra ya no existe.', 1; " &
            "DECLARE @cantidadDetalles INT; " &
            "SELECT @cantidadDetalles=COUNT(*) " &
            "FROM compra_detalle WITH (UPDLOCK, HOLDLOCK) " &
            "WHERE id_compra=@id; " &
            "IF EXISTS (" &
            "SELECT 1 FROM inventario_piezas ip " &
            "INNER JOIN compra_detalle cd " &
            "ON cd.id_compra_detalle=ip.id_compra_detalle " &
            "WHERE cd.id_compra=@id) " &
            "OR EXISTS (" &
            "SELECT 1 FROM movimientos_inventario mi " &
            "INNER JOIN compra_detalle cd " &
            "ON cd.id_compra_detalle=mi.id_compra_detalle " &
            "WHERE cd.id_compra=@id) " &
            "THROW 50002, " &
            "'La compra tiene inventario asociado y no puede eliminarse.', 1; " &
            "DELETE FROM compra_detalle WHERE id_compra=@id; " &
            "DELETE FROM compras WHERE id_compra=@id; " &
            "COMMIT TRANSACTION; " &
            "END TRY " &
            "BEGIN CATCH " &
            "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION; " &
            "THROW; " &
            "END CATCH;"

        objMan.ejecutarConParametros(
            sql,
            New Dictionary(Of String, Object) From {{"id", idCompra}})
    End Sub
End Class
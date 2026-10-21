Imports capaDatos
Imports System.Data
Imports System.Collections.Generic
Imports System.Text
Imports System.Data.SqlClient

Public Class Compra
    Private objMan As New clsMantenimiento

    Private Function EjecutarCompra(sql As String, parametros As Dictionary(Of String, Object)) As DataTable
        Dim conexion As New clsConectaBD
        Try
            conexion.abrirconexion()
            Using comando As New SqlCommand(sql, conexion.miConexion)
                For Each parametro In parametros
                    comando.Parameters.AddWithValue("@" & parametro.Key, If(parametro.Value, DBNull.Value))
                Next
                ' Los procedimientos de inventario devuelven sus propios resultados.
                ' Consumirlos todos permite detectar errores y obtener el ID tras COMMIT.
                Using adaptador As New SqlDataAdapter(comando)
                    Dim resultados As New DataSet()
                    adaptador.Fill(resultados)
                    If resultados.Tables.Count = 0 Then Throw New Exception("No se confirmó el guardado de la compra.")
                    Dim confirmacion As DataTable = resultados.Tables(resultados.Tables.Count - 1)
                    If Not confirmacion.Columns.Contains("id_compra") OrElse confirmacion.Rows.Count <> 1 Then
                        Throw New Exception("No se confirmó el guardado de la compra.")
                    End If
                    Return confirmacion
                End Using
            End Using
        Catch ex As SqlException
            Throw New Exception(ex.Message, ex)
        Finally
            conexion.cerrarconexion()
        End Try
    End Function

    Public Class ImportesDetalle
        Public Property Cantidad As Decimal
        Public Property Subtotal As Decimal
    End Class

    ' La misma regla se usa en pantalla y al guardar. PIE2 conserva la
    ' equivalencia comercial de 30 x 30 cm empleada por el proyecto.
    Public Shared Function CalcularImportes(unidad As String, piezas As Decimal, costo As Decimal,
                                           Optional ancho As Decimal? = Nothing,
                                           Optional alto As Decimal? = Nothing,
                                           Optional largo As Decimal? = Nothing) As ImportesDetalle
        unidad = If(unidad, "").Trim().ToUpperInvariant()
        If piezas < 1D OrElse piezas <> Decimal.Truncate(piezas) OrElse piezas > 1000000D Then
            Throw New ArgumentException("La cantidad de piezas, cajas o unidades debe ser entera, desde 1 hasta 1000000.")
        End If
        If costo <= 0D OrElse costo > 99999999.999D OrElse costo <> Decimal.Round(costo, 3) Then
            Throw New ArgumentException("El costo debe ser mayor a cero, con hasta tres decimales y dentro del límite permitido.")
        End If
        Dim cantidadExacta As Decimal
        Select Case unidad
            Case "CM2", "PIE2"
                ValidarMedida(ancho, "ancho")
                ValidarMedida(alto, "alto")
                cantidadExacta = ancho.Value * alto.Value * piezas
                If unidad = "PIE2" Then cantidadExacta /= 900D
            Case "CM_LINEAL"
                ValidarMedida(largo, "largo")
                cantidadExacta = largo.Value * piezas
            Case "CAJA", "UNIDAD"
                cantidadExacta = piezas
            Case Else
                Throw New ArgumentException("La unidad de costo del producto no es válida.")
        End Select
        Dim cantidad As Decimal = Decimal.Round(cantidadExacta, 3, MidpointRounding.AwayFromZero)
        ' Redondear cada resultado al final; no cobrar usando el área ya redondeada.
        Dim subtotal As Decimal = Decimal.Round(cantidadExacta * costo, 3, MidpointRounding.AwayFromZero)
        If cantidad <= 0D OrElse subtotal <= 0D Then
            Throw New ArgumentException("La medida o el importe es demasiado pequeño para registrarse con tres decimales.")
        End If
        If cantidad > 99999999.999D OrElse subtotal > 99999999.999D Then
            Throw New ArgumentException("La cantidad o el subtotal supera el límite permitido.")
        End If
        Return New ImportesDetalle With {.Cantidad = cantidad, .Subtotal = subtotal}
    End Function

    Private Shared Sub ValidarMedida(valor As Decimal?, nombre As String)
        If Not valor.HasValue OrElse valor.Value <= 0D OrElse valor.Value > 999999.999D OrElse
            valor.Value <> Decimal.Round(valor.Value, 3) Then
            Throw New ArgumentException("Ingrese " & nombre & " mayor a cero, con hasta tres decimales y menor a 1000000 cm.")
        End If
    End Sub

    Private Shared Function MedidaDeFila(fila As DataRow, columna As String) As Decimal?
        If Not fila.Table.Columns.Contains(columna) OrElse IsDBNull(fila(columna)) Then Return Nothing
        Return CDec(fila(columna))
    End Function

    ' Compatibilidad con llamadas anteriores: solo resuelve nombres inequívocos.
    ' Las pantallas nuevas siempre envían el ID seleccionado.
    Private Shared Function ResolverProveedorSql() As String
        Return "IF @idProveedor IS NULL BEGIN " &
            "IF (SELECT COUNT(*) FROM dbo.proveedores WHERE nombre=@proveedor) <> 1 " &
            "THROW 50201, 'Seleccione un proveedor registrado.', 1; " &
            "SELECT @idProveedor=id_proveedor FROM dbo.proveedores WHERE nombre=@proveedor; END; " &
            "IF NOT EXISTS (SELECT 1 FROM dbo.proveedores WITH (HOLDLOCK) WHERE id_proveedor=@idProveedor) " &
            "THROW 50201, 'El proveedor ya no existe. Seleccione otro.', 1; " &
            "SELECT @proveedor=nombre FROM dbo.proveedores WHERE id_proveedor=@idProveedor; "
    End Function

    Public Function Listar() As DataTable
        Return objMan.listarComando(
            "select id_compra, proveedor, fecha, total, motivo, id_pedido_origen from compras order by fecha desc, id_compra desc")
    End Function

    Public Function ListarPorFecha(desde As Date, hasta As Date) As DataTable
        Return objMan.consultarConParametros(
            "select id_compra, proveedor, fecha, total, motivo, id_pedido_origen from compras where fecha between @desde and @hasta " &
            "order by fecha desc, id_compra desc",
            New Dictionary(Of String, Object) From {
                {"desde", desde.Date},
                {"hasta", hasta.Date}
            })
    End Function

    Public Function Insertar(proveedor As String, fecha As Date,
                             total As Decimal, motivo As String,
                             idPedidoOrigen As Integer?, Optional idProveedor As Integer? = Nothing) As Integer

        Dim parametros As New Dictionary(Of String, Object) From {
            {"proveedor", proveedor},
            {"idProveedor", If(idProveedor.HasValue, CType(idProveedor.Value, Object), DBNull.Value)},
            {"fecha", fecha.Date},
            {"total", total},
            {"motivo", motivo},
            {"pedido", If(idPedidoOrigen.HasValue,
                          CType(idPedidoOrigen.GetValueOrDefault(), Object),
                          DBNull.Value)}
        }

        Dim dt As DataTable = objMan.consultarConParametros(
            ResolverProveedorSql() &
            "insert into compras " &
            "(proveedor, id_proveedor, fecha, total, motivo, id_pedido_origen) " &
            "values (@proveedor, @idProveedor, @fecha, @total, @motivo, @pedido); " &
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
                              Optional idCompra As Integer = 0,
                              Optional idUsuario As Integer? = Nothing,
                              Optional idProveedor As Integer? = Nothing) As Integer

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

        Dim unidades As New Dictionary(Of Integer, String)()
        For Each variante As DataRow In objMan.listarComando(
            "SELECT pv.id_variante, pv.unidad_costo FROM dbo.producto_variante pv " &
            "INNER JOIN dbo.productos p ON p.id_producto=pv.id_producto WHERE p.activo=1").Rows
            unidades.Add(CInt(variante("id_variante")), variante("unidad_costo").ToString().Trim().ToUpperInvariant())
        Next

        Dim parametros As New Dictionary(Of String, Object) From {
            {"proveedor", proveedor.Trim()},
            {"idProveedor", If(idProveedor.HasValue, CType(idProveedor.Value, Object), DBNull.Value)},
            {"fecha", fecha.Date},
            {"motivo", motivo},
            {"pedido", If(idPedidoOrigen.HasValue,
                          CType(idPedidoOrigen.GetValueOrDefault(), Object),
                          DBNull.Value)},
            {"idUsuario", If(idUsuario.HasValue,
                             CType(idUsuario.Value, Object),
                             DBNull.Value)}
        }

        Dim sql As New StringBuilder()
        sql.AppendLine("SET XACT_ABORT ON;")
        sql.AppendLine("BEGIN TRY")
        sql.AppendLine("BEGIN TRANSACTION;")
        sql.AppendLine(ResolverProveedorSql())
        sql.AppendLine("IF @idUsuario IS NOT NULL AND NOT EXISTS " &
            "(SELECT 1 FROM dbo.usuarios WITH (HOLDLOCK) WHERE id_usuario=@idUsuario AND activo=1) " &
            "THROW 50231, 'El usuario de la compra no existe o está deshabilitado.', 1;")

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
        "(proveedor, id_proveedor, fecha, total, motivo, id_pedido_origen) " &
        "VALUES (@proveedor, @idProveedor, @fecha, @total, @motivo, @pedido);")

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
        "UPDATE compras SET proveedor=@proveedor, id_proveedor=@idProveedor, fecha=@fecha, " &
        "total=@total, motivo=@motivo, id_pedido_origen=@pedido " &
        "WHERE id_compra=@idCompra;")

            sql.AppendLine(
        "DELETE FROM compra_detalle WHERE id_compra=@idCompra;")
        End If

        ' Consumir los resultados internos para devolver únicamente el ID confirmado.
        ' Un SELECT anticipado puede ocultar errores posteriores al DataAdapter.

        Dim total As Decimal = 0
        Dim indice As Integer = 0
        Dim variantes As New HashSet(Of Integer)()

        For Each fila As DataRow In detalle.Rows
            Dim idVariante As Integer = CInt(fila("id_variante"))
            Dim cantidad As Decimal = CDec(fila("cantidad"))
            Dim costo As Decimal = CDec(fila("costo_unitario"))

            If Not unidades.ContainsKey(idVariante) Then
                Throw New ArgumentException("El producto no existe o está dado de baja. Vuelva a seleccionarlo.")
            End If
            Dim unidad As String = unidades(idVariante)
            If detalle.Columns.Contains("unidad") AndAlso fila("unidad").ToString().Trim().ToUpperInvariant() <> unidad Then
                Throw New ArgumentException("La unidad del producto cambió. Vuelva a seleccionar el producto.")
            End If
            Dim esPieza As Boolean = unidad = "CM2" OrElse unidad = "PIE2" OrElse unidad = "CM_LINEAL"
            Dim ancho As Decimal? = MedidaDeFila(fila, "ancho_cm")
            Dim alto As Decimal? = MedidaDeFila(fila, "alto_cm")
            Dim largo As Decimal? = MedidaDeFila(fila, "largo_cm")
            If unidad = "CM2" OrElse unidad = "PIE2" Then largo = Nothing
            If unidad = "CM_LINEAL" Then
                ancho = Nothing
                alto = Nothing
            End If
            If Not esPieza Then
                ancho = Nothing
                alto = Nothing
                largo = Nothing
            End If
            Dim conteo As Decimal = If(esPieza, 1D, cantidad)
            If esPieza AndAlso detalle.Columns.Contains("piezas") AndAlso Not IsDBNull(fila("piezas")) Then
                conteo = CDec(fila("piezas"))
            End If
            Dim importes As ImportesDetalle = CalcularImportes(unidad, conteo, costo, ancho, alto, largo)
            If cantidad <> importes.Cantidad Then
                Throw New ArgumentException("La cantidad facturada no coincide con las piezas y medidas. Actualice el producto.")
            End If
            Dim piezas As Integer = CInt(conteo)
            ' Solo los accesorios (caja/unidad) no admiten repetirse; las piezas si
            If Not esPieza AndAlso Not variantes.Add(idVariante) Then
                Throw New Exception("Hay una variante repetida en el detalle.")
            End If

            If cantidad <= 0 OrElse costo <= 0 Then
                Throw New Exception(
                    "La cantidad y el costo deben ser mayores a cero.")
            End If

            If cantidad > 99999999.999D OrElse costo > 99999999.999D Then
                Throw New Exception(
                    "La cantidad o el costo supera el límite permitido.")
            End If

            If cantidad <> Decimal.Round(cantidad, 3) OrElse
               costo <> Decimal.Round(costo, 3) Then
                Throw New Exception(
                    "La cantidad y el costo admiten hasta tres decimales.")
            End If

            Dim subtotal As Decimal = importes.Subtotal

            If subtotal > 99999999.999D Then
                Throw New Exception(
                    "Un subtotal supera el límite de la base de datos.")
            End If

            total += subtotal
            Dim sufijo As String = indice.ToString()

            parametros.Add("variante" & sufijo, idVariante)
            parametros.Add("unidad" & sufijo, unidad)
            sql.AppendLine("IF NOT EXISTS (SELECT 1 FROM dbo.producto_variante pv WITH (HOLDLOCK) " &
                "INNER JOIN dbo.productos p WITH (HOLDLOCK) ON p.id_producto=pv.id_producto " &
                "WHERE pv.id_variante=@variante" & sufijo & " AND pv.unidad_costo=@unidad" & sufijo & " AND p.activo=1) " &
                "THROW 50230, 'El producto fue modificado o dado de baja. Vuelva a seleccionarlo.', 1;")
            parametros.Add("cantidad" & sufijo, cantidad)
            parametros.Add("costo" & sufijo, costo)
            parametros.Add("subtotal" & sufijo, subtotal)

            sql.AppendLine(
                "INSERT INTO compra_detalle " &
                "(id_compra, id_variante, cantidad, costo_unitario, subtotal" &
                ", piezas, ancho_cm, alto_cm, largo_cm" & ") " &
                "VALUES (@idCompra, @variante" & sufijo &
                ", @cantidad" & sufijo & ", @costo" & sufijo &
                ", @subtotal" & sufijo &
                ", @piezas" & sufijo & ", @ancho" & sufijo & ", @alto" & sufijo & ", @largo" & sufijo & ");")

            parametros.Add("piezas" & sufijo, piezas)
            parametros.Add("ancho" & sufijo, If(ancho.HasValue, CType(ancho.Value, Object), DBNull.Value))
            parametros.Add("alto" & sufijo, If(alto.HasValue, CType(alto.Value, Object), DBNull.Value))
            parametros.Add("largo" & sufijo, If(largo.HasValue, CType(largo.Value, Object), DBNull.Value))

            sql.AppendLine(
                "DECLARE @cd" & sufijo & " INT = CONVERT(INT, SCOPE_IDENTITY());")
            If esPieza Then
                ' Cada pieza fisica es una fila de inventario con la misma medida
                sql.AppendLine(
                    "DECLARE @i" & sufijo & " INT = 1;")
                sql.AppendLine(
                    "WHILE @i" & sufijo & " <= @piezas" & sufijo & " BEGIN")
                sql.AppendLine(
                    "    EXEC dbo.sp_inv_ingresar_pieza @id_variante=@variante" & sufijo &
                    ", @ancho_cm=@ancho" & sufijo & ", @alto_cm=@alto" & sufijo &
                    ", @largo_cm=@largo" & sufijo & ", @origen=N'COMPRA', " &
                    "@id_compra_detalle=@cd" & sufijo & ", @id_usuario=@idUsuario, " &
                    "@fecha=@fecha;")
                sql.AppendLine(
                    "    SET @i" & sufijo & " = @i" & sufijo & " + 1;")
                sql.AppendLine("END;")
            Else
                sql.AppendLine("EXEC dbo.sp_inv_mover_cajas @id_variante=@variante" & sufijo &
                    ", @tipo=N'ENTRADA', @cantidad=@piezas" & sufijo & ", @motivo=N'COMPRA', " &
                    "@id_compra_detalle=@cd" & sufijo & ", @id_usuario=@idUsuario;")
            End If

            indice += 1
        Next

        If total > 99999999.999D Then
            Throw New Exception(
                "El total supera el límite de la base de datos.")
        End If

        ' Si hay lineas de pedido que esperaban este material (COMPRA_NUEVA),
        ' enlazarlas con las piezas recien compradas del mismo variante y medida.
        sql.AppendLine("DECLARE @Enlazadas TABLE (id_pieza INT PRIMARY KEY, id_item INT);")
        sql.AppendLine(";WITH pendientes AS (")
        sql.AppendLine(
            "    SELECT pd.id_detalle, pd.id_item, pd.id_variante, " &
            "pd.ancho_cm, pd.alto_cm, pd.largo_cm, " &
            "    ROW_NUMBER() OVER (PARTITION BY pd.id_variante, " &
            "ISNULL(pd.ancho_cm,0), ISNULL(pd.alto_cm,0), ISNULL(pd.largo_cm,0) " &
            "ORDER BY pd.id_detalle) AS rn " &
            "    FROM dbo.pedido_detalle pd WITH (UPDLOCK,HOLDLOCK) " &
            "    WHERE pd.estado='ACTIVO' AND pd.accion='COMPRA_NUEVA' AND pd.id_pieza IS NULL " &
            "AND EXISTS (SELECT 1 FROM dbo.pedido_item pi " &
            "INNER JOIN dbo.pedidos pe ON pe.id_pedido=pi.id_pedido " &
            "WHERE pi.id_item=pd.id_item AND pi.estado<>'CANCELADO' AND pe.estado<>'CANCELADO')")
        sql.AppendLine("), nuevas AS (")
        sql.AppendLine(
            "    SELECT ip.id_pieza, ip.id_variante, ip.ancho_cm, ip.alto_cm, " &
            "ip.largo_cm, ip.id_compra_detalle, " &
            "    ROW_NUMBER() OVER (PARTITION BY ip.id_variante, " &
            "ISNULL(ip.ancho_cm,0), ISNULL(ip.alto_cm,0), ISNULL(ip.largo_cm,0) " &
            "ORDER BY ip.id_pieza) AS rn " &
            "    FROM dbo.inventario_piezas ip " &
            "    INNER JOIN dbo.compra_detalle cd " &
            "    ON cd.id_compra_detalle = ip.id_compra_detalle " &
            "    WHERE cd.id_compra=@idCompra AND ip.origen='COMPRA' " &
            "AND ip.estado='DISPONIBLE'")
        sql.AppendLine(")")
        sql.AppendLine(
            "UPDATE pd SET pd.id_pieza=n.id_pieza, pd.accion='USA_EXISTENTE', " &
            "pd.id_compra_detalle=n.id_compra_detalle " &
            "OUTPUT inserted.id_pieza, inserted.id_item INTO @Enlazadas " &
            "FROM dbo.pedido_detalle pd " &
            "INNER JOIN pendientes p ON p.id_detalle=pd.id_detalle " &
            "INNER JOIN nuevas n ON n.id_variante=p.id_variante AND n.rn=p.rn " &
            "AND ISNULL(n.ancho_cm,0)=ISNULL(p.ancho_cm,0) " &
            "AND ISNULL(n.alto_cm,0)=ISNULL(p.alto_cm,0) " &
            "AND ISNULL(n.largo_cm,0)=ISNULL(p.largo_cm,0);")
        sql.AppendLine(
            "UPDATE ip SET ip.estado='VENDIDA' FROM dbo.inventario_piezas ip " &
            "INNER JOIN @Enlazadas e ON e.id_pieza=ip.id_pieza;")
        sql.AppendLine(
            "INSERT INTO dbo.movimientos_inventario " &
            "(id_variante, id_pieza, tipo, motivo, cantidad, id_usuario, id_pedido, observacion) " &
            "SELECT ip.id_variante, e.id_pieza, 'SALIDA', 'VENTA', 1, @idUsuario, pi.id_pedido, " &
            "'Enlace automatico compra-pedido' " &
            "FROM @Enlazadas e " &
            "INNER JOIN dbo.inventario_piezas ip ON ip.id_pieza=e.id_pieza " &
            "INNER JOIN dbo.pedido_item pi ON pi.id_item=e.id_item;")
        ' Recalcular el estado de los pedidos que recibieron material
        sql.AppendLine(
            "IF OBJECT_ID(N'dbo.sp_ped_recalcular_estados', N'P') IS NULL " &
            "THROW 50090, " &
            "'Falta migrar la BD: ejecute 00_vidrieria_completo.sql " &
            "(falta sp_ped_recalcular_estados).', 1;")
        sql.AppendLine(
            "DECLARE @pEd INT; " &
            "DECLARE curPed CURSOR LOCAL FAST_FORWARD FOR " &
            "SELECT DISTINCT pi.id_pedido FROM @Enlazadas e " &
            "INNER JOIN dbo.pedido_item pi ON pi.id_item=e.id_item; " &
            "OPEN curPed; FETCH NEXT FROM curPed INTO @pEd; " &
            "WHILE @@FETCH_STATUS = 0 BEGIN " &
            "EXEC dbo.sp_ped_recalcular_estados @id_pedido=@pEd; " &
            "FETCH NEXT FROM curPed INTO @pEd; END " &
            "CLOSE curPed; DEALLOCATE curPed;")

        parametros.Add("total", total)

        sql.AppendLine("COMMIT TRANSACTION;")
        sql.AppendLine("SELECT @idCompra AS id_compra;")
        sql.AppendLine("END TRY")
        sql.AppendLine("BEGIN CATCH")
        sql.AppendLine("IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;")
        sql.AppendLine("THROW;")
        sql.AppendLine("END CATCH;")

        If parametros.Count > 2100 Then
            Throw New ArgumentException("La compra supera el límite de detalles permitidos. Divídala en varias compras.")
        End If
        Dim resultado As DataTable = EjecutarCompra(sql.ToString(), parametros)

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
            "cd.cantidad, cd.costo_unitario, cd.subtotal, " &
            "pv.unidad_costo AS unidad, " &
            "ISNULL(cd.piezas, 1) AS piezas, " &
            "cd.ancho_cm, cd.alto_cm, cd.largo_cm " &
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

<%@ Page Title="Reporte de Ingresos y Egresos" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfReporteIngresosMes.aspx.vb" Inherits="wfReporteIngresosMes" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Reporte de Ingresos y Egresos</h1>
    <p class="subtitulo-pagina">
        Entradas por cobros (incluye pagos parciales) y salidas por compras, vueltos y
        devoluciones del mes seleccionado.
    </p>

    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />

    <asp:Panel ID="pnlReporte" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Mes:</td>
                <td class="dato"><asp:DropDownList ID="ddlMes" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Año:</td>
                <td class="dato"><asp:DropDownList ID="ddlAnio" runat="server" /></td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnGenerar" runat="server" Text="Generar reporte"
                CssClass="boton boton-primario" OnClick="btnGenerar_Click" />
        </div>

        <h2 class="panel-titulo"><asp:Label ID="lblPeriodo" runat="server" /></h2>

        <p class="grafico-leyenda">
            <span class="leyenda-color leyenda-venta"></span> Entradas
            <span class="leyenda-color leyenda-compra"></span> Salidas
        </p>

        <div class="ingresos-tarjetas">
            <div class="ingresos-tarjeta ingresos-tarjeta-venta">
                <span class="ingresos-tarjeta-titulo">Entradas del mes</span>
                <span class="ingresos-tarjeta-valor monto-venta"><asp:Label ID="lblEntradas" runat="server" /></span>
                <span class="ingresos-tarjeta-detalle"><asp:Label ID="lblDetalleEntradas" runat="server" /></span>
            </div>

            <div class="ingresos-tarjeta ingresos-tarjeta-compra">
                <span class="ingresos-tarjeta-titulo">Salidas del mes</span>
                <span class="ingresos-tarjeta-valor monto-compra"><asp:Label ID="lblSalidas" runat="server" /></span>
                <span class="ingresos-tarjeta-detalle"><asp:Label ID="lblDetalleSalidas" runat="server" /></span>
            </div>

            <div class="ingresos-tarjeta ingresos-tarjeta-utilidad">
                <span class="ingresos-tarjeta-titulo">Saldo neto</span>
                <span class="ingresos-tarjeta-valor"><asp:Label ID="lblSaldo" runat="server" /></span>
                <span class="ingresos-tarjeta-detalle">Entradas - salidas</span>
            </div>

            <div class="ingresos-tarjeta">
                <span class="ingresos-tarjeta-titulo">Flujo sobre entradas</span>
                <span class="ingresos-tarjeta-valor"><asp:Label ID="lblFlujo" runat="server" /></span>
                <span class="ingresos-tarjeta-detalle">Saldo / entradas</span>
            </div>
        </div>

        <h3 class="panel-subtitulo">Entradas vs. salidas</h3>
        <div class="grafico-ingresos" role="img"
            aria-label="Gráfico de entradas y salidas de dinero del mes seleccionado">
            <div class="grafico-fila">
                <span class="grafico-etiqueta">Entradas</span>
                <div class="grafico-pista">
                    <div id="barraVentas" runat="server" class="grafico-barra grafico-barra-venta"
                        style="width: 0%;"></div>
                </div>
                <span class="grafico-monto monto-venta"><asp:Label ID="lblGraficoEntradas" runat="server" /></span>
            </div>

            <div class="grafico-fila">
                <span class="grafico-etiqueta">Salidas</span>
                <div class="grafico-pista">
                    <div id="barraCompras" runat="server" class="grafico-barra grafico-barra-compra"
                        style="width: 0%;"></div>
                </div>
                <span class="grafico-monto monto-compra"><asp:Label ID="lblGraficoSalidas" runat="server" /></span>
            </div>
        </div>

        <h3 class="panel-subtitulo">Resumen del mes</h3>
        <div class="tabla-scroll">
            <table class="tabla-datos tabla-ingresos">
                <thead>
                    <tr>
                        <th>Concepto</th>
                        <th class="texto-derecha">Valor</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Cobros registrados</td>
                        <td class="texto-derecha"><asp:Label ID="lblTablaCobros" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Pedidos cobrados</td>
                        <td class="texto-derecha"><asp:Label ID="lblTablaPedidos" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Entradas (cobrado)</td>
                        <td class="texto-derecha monto-venta"><asp:Label ID="lblTablaEntradas" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Compras registradas</td>
                        <td class="texto-derecha"><asp:Label ID="lblTablaCantidadCompras" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Salidas por compras</td>
                        <td class="texto-derecha monto-compra"><asp:Label ID="lblTablaCompras" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Vueltos y devoluciones</td>
                        <td class="texto-derecha monto-compra"><asp:Label ID="lblTablaCaja" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Salidas totales</td>
                        <td class="texto-derecha monto-compra"><asp:Label ID="lblTablaSalidas" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Saldo neto</td>
                        <td class="texto-derecha"><asp:Label ID="lblTablaSaldo" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Flujo sobre entradas</td>
                        <td class="texto-derecha"><asp:Label ID="lblTablaFlujo" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
        </div>

        <h3 class="panel-subtitulo">Desglose por método de pago</h3>
        <div class="tabla-scroll">
            <asp:GridView ID="grvMetodos" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos tabla-ingresos" GridLines="None"
                EmptyDataText="No hay movimientos en el mes seleccionado.">
                <Columns>
                    <asp:BoundField DataField="metodo" HeaderText="Método" />
                    <asp:BoundField DataField="entradas" HeaderText="Entradas (S/)"
                        DataFormatString="{0:N2}"
                        HeaderStyle-CssClass="texto-derecha"
                        ItemStyle-CssClass="texto-derecha monto-venta" />
                    <asp:BoundField DataField="salidas" HeaderText="Salidas (S/)"
                        DataFormatString="{0:N2}"
                        HeaderStyle-CssClass="texto-derecha"
                        ItemStyle-CssClass="texto-derecha monto-compra" />
                    <asp:BoundField DataField="neto" HeaderText="Neto (S/)"
                        DataFormatString="{0:N2}"
                        HeaderStyle-CssClass="texto-derecha"
                        ItemStyle-CssClass="texto-derecha" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

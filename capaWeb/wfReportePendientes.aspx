<%@ Page Title="Reporte de Clientes con Pagos Faltantes" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfReportePendientes.aspx.vb" Inherits="wfReportePendientes" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Clientes con Pagos Faltantes</h1>
    <p class="subtitulo-pagina">Clientes con pedidos pendientes o parciales y saldo por cobrar.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlReporte" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Ordenar por:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlOrden" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlOrden_SelectedIndexChanged">
                        <asp:ListItem Value="FECHA_DESC">Fecha (más reciente)</asp:ListItem>
                        <asp:ListItem Value="FECHA_ASC">Fecha (más antigua)</asp:ListItem>
                        <asp:ListItem Value="CLIENTE_ASC">Cliente (A-Z)</asp:ListItem>
                        <asp:ListItem Value="CLIENTE_DESC">Cliente (Z-A)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvClientes" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay clientes con saldo pendiente."
                OnRowCommand="grvClientes_RowCommand"
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvClientes_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="id_cliente" HeaderText="ID" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="documento" HeaderText="Documento" NullDisplayText="-" />
                    <asp:BoundField DataField="telefono" HeaderText="Teléfono" NullDisplayText="-" />
                    <asp:BoundField DataField="cantidad_pedidos" HeaderText="Pedidos" />
                    <asp:BoundField DataField="total" HeaderText="Facturado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="pagado" HeaderText="Pagado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="saldo_faltante" HeaderText="Falta (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="fecha_mas_antigua" HeaderText="Pedido más antiguo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Detalle">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetalle" runat="server" Text="Ver pedidos"
                                CssClass="boton boton-mini boton-secundario" CommandName="VerDetalle"
                                CommandArgument='<%# Eval("id_cliente") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="paginacion" />
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlDetalle" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo"><asp:Label ID="lblDetalle" runat="server" /></h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Ordenar detalle por:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlOrdenDetalle" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlOrdenDetalle_SelectedIndexChanged">
                        <asp:ListItem Value="FECHA_DESC">Fecha (más reciente)</asp:ListItem>
                        <asp:ListItem Value="FECHA_ASC">Fecha (más antigua)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="tabla-scroll">
            <asp:GridView ID="grvPedidos" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="Este cliente no tiene pedidos con saldo."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvPedidos_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="estado_trabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="total" HeaderText="Importe (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="pagado" HeaderText="Pagado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="saldo" HeaderText="Falta (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="estado_pago" HeaderText="Estado de pago" />
                </Columns>
                <PagerStyle CssClass="paginacion" />
            </asp:GridView>
        </div>
        <div class="acciones">
            <asp:Button ID="btnCerrarDetalle" runat="server" Text="Cerrar detalle"
                CssClass="boton boton-secundario" OnClick="btnCerrarDetalle_Click" />
        </div>
    </asp:Panel>
</asp:Content>

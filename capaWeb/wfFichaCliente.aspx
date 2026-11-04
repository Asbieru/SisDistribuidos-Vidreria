<%@ Page Title="Ficha comercial" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfFichaCliente.aspx.vb" Inherits="wfFichaCliente" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Ficha comercial</h1>
    <p class="subtitulo-pagina"><asp:Literal ID="litCliente" runat="server" Mode="Encode" /></p>
    <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/wfRegistrarClientes.aspx"
        CssClass="boton boton-secundario" Text="Volver a Clientes" />
    <p role="status"><asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" /></p>

    <asp:Panel ID="pnlFicha" runat="server" CssClass="panel" Visible="False">
        <h2>Pedidos</h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta"><asp:Label runat="server" AssociatedControlID="ddlEstado" Text="Estado:" /></td>
                <td class="dato">
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="">Todos</asp:ListItem>
                        <asp:ListItem Value="PENDIENTE">Pendiente</asp:ListItem>
                        <asp:ListItem Value="PARCIAL">Parcial</asp:ListItem>
                        <asp:ListItem Value="PAGADO">Pagado</asp:ListItem>
                        <asp:ListItem Value="CANCELADO">Cancelado</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr><td class="etiqueta"><asp:Label runat="server" AssociatedControlID="txtDesde" Text="Desde:" /></td>
                <td class="dato"><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td></tr>
            <tr><td class="etiqueta"><asp:Label runat="server" AssociatedControlID="txtHasta" Text="Hasta:" /></td>
                <td class="dato"><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td></tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="boton boton-primario" OnClick="btnFiltrar_Click" />
            <asp:Button ID="btnVerTodos" runat="server" Text="Ver todos" CssClass="boton boton-secundario" OnClick="btnVerTodos_Click" />
        </div>
        <p><asp:Literal ID="litResumen" runat="server" Mode="Encode" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvPedidos" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay pedidos para los filtros seleccionados."
                OnRowCommand="grvPedidos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="vendedor" HeaderText="Vendedor" />
                    <asp:BoundField DataField="total" HeaderText="Total (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="pagado" HeaderText="Pagado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="saldo" HeaderText="Saldo (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="excedente" HeaderText="Excedente (S/)" DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPagos" runat="server" CommandName="VerPagos"
                                CommandArgument='<%# Eval("id_pedido") %>' Text="Ver pagos"
                                CssClass="boton boton-mini boton-secundario" CausesValidation="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <h2>Pagos del pedido</h2>
        <p role="status"><asp:Literal ID="litPagos" runat="server" Mode="Encode" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvPagos" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="Este pedido no tiene pagos registrados.">
                <Columns>
                    <asp:BoundField DataField="id_pago" HeaderText="Pago" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="total_pago" HeaderText="Total del pago (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="metodo" HeaderText="M&#233;todo" />
                    <asp:BoundField DataField="monto_metodo" HeaderText="Importe del m&#233;todo (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="comprobante" HeaderText="Comprobante" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

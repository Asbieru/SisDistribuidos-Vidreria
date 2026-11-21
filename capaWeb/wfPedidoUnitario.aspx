<%@ Page Title="Pedido" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfPedidoUnitario.aspx.vb" Inherits="wfPedidoUnitario" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblMensaje" runat="server"></asp:Label>

    <asp:Panel ID="pnlPedido" runat="server" CssClass="panel" Visible="False">
        <h1 class="titulo-pagina"><asp:Label ID="lblTitulo" runat="server" /></h1>
        <p class="subtitulo-pagina"><asp:Label ID="lblSubTitulo" runat="server" /></p>
        <p><asp:Label ID="lblEstado" runat="server" /></p>

        <h3 class="panel-subtitulo">Ítems del pedido</h3>
        <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton ID="lnkItem" runat="server"
                    CssClass='<%# If(Convert.ToInt32(Eval("id_item")) = IdItemSeleccionado, "boton boton-mini boton-primario pestana-item pestana-item-activa", "boton boton-mini boton-secundario pestana-item") %>'
                    CommandName="SeleccionarItem"
                    CommandArgument='<%# Eval("id_item") %>'
                    Text='<%# Eval("descripcion_mostrar") %>'
                    CausesValidation="False" />
            </ItemTemplate>
        </asp:Repeater>
        <div class="acciones paginacion-items">
            <asp:LinkButton ID="lnkItemsAnterior" runat="server" CssClass="boton boton-mini boton-secundario"
                Text="Anterior" OnClick="lnkItemsAnterior_Click" CausesValidation="False" />
            <asp:Label ID="lblItemsPagina" runat="server" CssClass="texto-ayuda" />
            <asp:LinkButton ID="lnkItemsSiguiente" runat="server" CssClass="boton boton-mini boton-secundario"
                Text="Siguiente" OnClick="lnkItemsSiguiente_Click" CausesValidation="False" />
        </div>

        <asp:Panel ID="pnlItem" runat="server" CssClass="detalle-item" Visible="False">
            <h3 class="panel-subtitulo"><asp:Label ID="lblItemTitulo" runat="server" /></h3>
            <table class="tabla-formulario" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="etiqueta">Total del ítem :</td>
                    <td class="dato"><asp:Label ID="lblItemPrecio" runat="server" /></td>
                </tr>
            </table>

            <div class="tabla-scroll">
                <asp:GridView ID="grvDetalles" runat="server" AutoGenerateColumns="False"
                    CssClass="tabla-datos" GridLines="None" DataKeyNames="id_detalle"
                    AllowPaging="True" PageSize="10" PagerStyle-CssClass="paginacion"
                    OnPageIndexChanging="grvDetalles_PageIndexChanging"
                    EmptyDataText="Este ítem no tiene materiales activos."
                    OnRowCommand="grvDetalles_RowCommand" OnRowDataBound="grvDetalles_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre del Item">
                            <ItemTemplate><%# Eval("nombre") %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="pieza" HeaderText="Pieza exacta en stock" />
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <asp:Label ID="lblEstadoDetalle" runat="server" Text='<%# Eval("estado") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acción">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAccion" runat="server"
                                    CssClass="boton boton-mini boton-secundario"
                                    CausesValidation="False" />
                                <asp:LinkButton ID="lnkUsarStock" runat="server"
                                    CssClass="boton boton-mini boton-primario"
                                    CausesValidation="False" />
                                <asp:HyperLink ID="lnkComprar" runat="server"
                                    CssClass="boton boton-mini boton-primario"
                                    Target="_blank" Text="Comprar Ahora" Visible="False" />
                                <asp:LinkButton ID="lnkRoto" runat="server"
                                    CssClass="boton boton-mini boton-peligro"
                                    Text="Item roto" CausesValidation="False"
                                    OnClientClick="return confirm('¿Marcar este material como roto y generar su reemplazo?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div class="acciones">
                <asp:Button ID="btnCancelarItem" runat="server" CssClass="boton boton-peligro"
                    Text="Cancelar ítem" OnClick="btnCancelarItem_Click" CausesValidation="False"
                    OnClientClick="return confirm('¿Cancelar este ítem y liberar sus piezas?');" />
                <asp:Button ID="btnTerminarItem" runat="server" CssClass="boton boton-primario"
                    Text="Terminar ítem" OnClick="btnTerminarItem_Click" CausesValidation="False"
                    OnClientClick="return confirm('¿Registrar la culminación de este ítem?');" />
            </div>
        </asp:Panel>

        <div class="acciones">
            <asp:Button ID="btnTerminarPedido" runat="server" CssClass="boton boton-primario"
                Text="Terminar pedido" OnClick="btnTerminarPedido_Click" CausesValidation="False"
                OnClientClick="return confirm('¿Marcar todo el pedido como terminado (entregado/instalado)? El pago es independiente.');" />
            <asp:HyperLink ID="lnkPagar" runat="server" CssClass="boton boton-secundario">Registrar Pago</asp:HyperLink>
            <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/wfListadoPedidos.aspx"
                CssClass="boton boton-secundario">Volver al listado</asp:HyperLink>
        </div>
    </asp:Panel>
</asp:Content>

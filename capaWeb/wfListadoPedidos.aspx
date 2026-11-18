<%@ Page Title="Listado de Pedidos" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfListadoPedidos.aspx.vb" Inherits="wfListadoPedidos" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Listado de Pedidos</h1>
    <p class="subtitulo-pagina">Busque un pedido, revise su detalle o cancélelo.</p>
    <asp:Label ID="lblMensaje" runat="server" />

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
            <table class="tabla-formulario">
                <tr>
                    <td class="etiqueta">Búsqueda :</td>
                    <td class="dato">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="campo" />
                    </td>
                </tr>
                <tr>
                    <td class="etiqueta">Categoría :</td>
                    <td class="dato">
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="campo">
                            <asp:ListItem Text="Cliente" Value="cliente" />
                            <asp:ListItem Text="Usuario" Value="usuario" Selected="True" />
                            <asp:ListItem Text="Fecha de Registro" Value="fecha_registro" />
                        </asp:DropDownList>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario"
                            OnClick="btnBuscar_Click" CausesValidation="False" />
                        <asp:Button ID="btnVerTodos" runat="server" Text="Mostrar Todos" CssClass="boton boton-secundario"
                            OnClick="btnVerTodos_Click" CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <div class="acciones">
            <asp:HyperLink ID="lnkNuevo" runat="server" NavigateUrl="~/wfRegistrarPedido.aspx"
                CssClass="boton boton-primario">Nuevo +</asp:HyperLink>
        </div>

        <div class="tabla-scroll">
            <asp:GridView ID="grvPedidos" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_pedido"
                OnRowCommand="grvPedidos_RowCommand" EmptyDataText="No se encontraron pedidos."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvPedidos_PageIndexChanging">
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="5"
                    FirstPageText="« Primera" LastPageText="Última »" Position="Bottom" />
                <PagerStyle CssClass="paginacion" />
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="ID" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Estado">
                        <ItemTemplate>
                            <span class='<%# ClaseEstado(Eval("estado")) %>'><%# Eval("estado") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkVer" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="VerPedido"
                                CommandArgument='<%# Eval("id_pedido") %>'
                                Text="Ver Pedido" CausesValidation="False"
                                Visible='<%# Eval("estado").ToString() <> "CANCELADO" %>' />
                            <asp:LinkButton ID="lnkCancelar" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="CancelarPedido"
                                CommandArgument='<%# Eval("id_pedido") %>'
                                Text="Cancelar" CausesValidation="False"
                                Visible='<%# Eval("estado").ToString() <> "CANCELADO" %>'
                                OnClientClick="return confirm('¿Está seguro de cancelar este pedido? Se liberarán las piezas reservadas.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
    </asp:Panel>

    <%-- Detalle del pedido elegido con "Ver Pedido" (equivale a frmPedidoUnitario de escritorio) --%>
    <asp:Panel ID="pnlDetalle" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo">Pedido N° <asp:Label ID="lblIdPedido" runat="server" /></h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Cliente :</td>
                <td class="dato"><asp:Label ID="lblCliente" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Vendedor :</td>
                <td class="dato"><asp:Label ID="lblUsuario" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Fecha :</td>
                <td class="dato"><asp:Label ID="lblFecha" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Estado :</td>
                <td class="dato"><asp:Label ID="lblEstado" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Total :</td>
                <td class="dato"><asp:Label ID="lblTotal" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Pagado :</td>
                <td class="dato"><asp:Label ID="lblPagado" runat="server" /></td>
            </tr>
        </table>

        <h3 class="panel-subtitulo">Items del pedido</h3>
        <div class="tabla-scroll">
            <asp:GridView ID="grvItems" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" EmptyDataText="El pedido no tiene items activos.">
                <Columns>
                    <asp:BoundField DataField="id_item" HeaderText="Item" />
                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="ancho_cm" HeaderText="Ancho (cm)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="alto_cm" HeaderText="Alto (cm)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="total" HeaderText="Total (S/.)" DataFormatString="{0:0.00}" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="acciones">
            <asp:HyperLink ID="lnkPagar" runat="server" CssClass="boton boton-primario">Registrar Pago</asp:HyperLink>
            <asp:Button ID="btnCerrarDetalle" runat="server" Text="Cerrar" CssClass="boton boton-secundario"
                OnClick="btnCerrarDetalle_Click" CausesValidation="False" />
        </div>
    </asp:Panel>
</asp:Content>

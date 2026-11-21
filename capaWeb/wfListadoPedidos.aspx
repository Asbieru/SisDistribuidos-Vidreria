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
                                Visible='<%# Eval("estado").ToString() <> "CANCELADO" AndAlso Eval("estado").ToString() <> "PAGADO" %>'
                                OnClientClick="return confirm('¿Está seguro de cancelar este pedido? Se liberarán las piezas reservadas.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
    </asp:Panel>
</asp:Content>

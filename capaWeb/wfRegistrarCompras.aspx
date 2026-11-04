<%@ Page Title="Registrar Compra" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarCompras.aspx.vb" Inherits="wfRegistrarCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Compra</h1>
    <p class="subtitulo-pagina">Registre la compra de productos al proveedor y su ingreso al inventario.</p>

    <asp:Panel ID="pnlCompra" runat="server" CssClass="panel">
        <h2 class="panel-titulo"><asp:Label ID="lblModo" runat="server" Text="NUEVA COMPRA"></asp:Label></h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Proveedor * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlProveedor" runat="server" CssClass="campo campo-largo">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Fecha * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="campo campo-corto"
                        TextMode="Date"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Motivo * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlMotivo" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlMotivo_SelectedIndexChanged">
                        <asp:ListItem Text="REPOSICION" Value="REPOSICION"></asp:ListItem>
                        <asp:ListItem Text="REEMPLAZO_DEFECTO" Value="REEMPLAZO_DEFECTO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Pedido de origen :</td>
                <td class="dato">
                    <asp:TextBox ID="txtPedidoOrigen" runat="server" CssClass="campo campo-corto"
                        MaxLength="10" Enabled="False"></asp:TextBox>
                    <span class="texto-ayuda">Solo para REEMPLAZO_DEFECTO.</span>
                </td>
            </tr>
        </table>

        <h3 class="panel-subtitulo">Agregar producto</h3>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Producto / variante * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlVariante" runat="server" CssClass="campo campo-largo"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlVariante_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Unidad :</td>
                <td class="dato">
                    <asp:Label ID="lblUnidad" runat="server" Text="(seleccione un producto)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="etiqueta"><asp:Label ID="lblCantidad" runat="server" Text="Piezas * :"></asp:Label></td>
                <td class="dato">
                    <asp:TextBox ID="txtPiezas" runat="server" CssClass="campo campo-corto"
                        MaxLength="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Ancho (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAncho" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta"><asp:Label ID="lblAlto" runat="server" Text="Alto (cm) :"></asp:Label></td>
                <td class="dato">
                    <asp:TextBox ID="txtAlto" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta"><asp:Label ID="lblCosto" runat="server" Text="Costo unitario * :"></asp:Label></td>
                <td class="dato">
                    <asp:TextBox ID="txtCosto" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnAgregarDetalle" runat="server" CssClass="boton boton-primario"
                Text="Agregar producto" OnClick="btnAgregarDetalle_Click" CausesValidation="False" />
            <asp:Button ID="btnCancelarEdicion" runat="server" CssClass="boton boton-secundario"
                Text="Cancelar edición" OnClick="btnCancelarEdicion_Click"
                CausesValidation="False" Visible="False" />
        </div>
        <asp:Label ID="lblMensajeDetalle" runat="server"></asp:Label>

        <h3 class="panel-subtitulo">Detalle de la compra</h3>
        <div class="tabla-scroll">
            <asp:GridView ID="grvDetalle" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None"
                OnRowCommand="grvDetalle_RowCommand"
                EmptyDataText="Agregue al menos un producto a la compra.">
                <Columns>
                    <asp:BoundField DataField="producto" HeaderText="Producto / variante" />
                    <asp:BoundField DataField="unidad" HeaderText="Unidad" />
                    <asp:BoundField DataField="medida" HeaderText="Medida" />
                    <asp:BoundField DataField="piezas" HeaderText="Conteo" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad facturada"
                        DataFormatString="{0:N3}" />
                    <asp:BoundField DataField="costo_unitario" HeaderText="Costo unitario"
                        DataFormatString="{0:N3}" />
                    <asp:BoundField DataField="subtotal" HeaderText="Subtotal"
                        DataFormatString="{0:N3}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditarDetalle" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarDetalle"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkQuitarDetalle" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="QuitarDetalle"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Quitar" CausesValidation="False"
                                OnClientClick="return confirm('¿Quitar este producto del detalle?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <p class="total-compra"><asp:Label ID="lblTotal" runat="server" Text="TOTAL: S/ 0.000"></asp:Label></p>

        <div class="acciones">
            <asp:Button ID="btnGuardar" runat="server" CssClass="boton boton-primario"
                Text="Guardar compra" OnClick="btnGuardar_Click" CausesValidation="False" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar compra" OnClick="btnLimpiar_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Compras recientes</h2>

        <div class="acciones" style="margin-top: 0; padding-top: 0; border-top: none;">
            <asp:Button ID="btnRecargar" runat="server" CssClass="boton boton-secundario"
                Text="Recargar listado" OnClick="btnRecargar_Click" CausesValidation="False" />
        </div>

        <div class="tabla-scroll">
            <asp:GridView ID="grvCompras" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_compra"
                OnRowCommand="grvCompras_RowCommand" EmptyDataText="Todavía no hay compras registradas.">
                <Columns>
                    <asp:BoundField DataField="id_compra" HeaderText="ID" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="total" HeaderText="Total"
                        DataFormatString="{0:N3}" />
                    <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                    <asp:BoundField DataField="id_pedido_origen" HeaderText="Pedido" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditarCompra" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarCompra"
                                CommandArgument='<%# Eval("id_compra") %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkEliminarCompra" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="EliminarCompra"
                                CommandArgument='<%# Eval("id_compra") %>'
                                Text="Eliminar" CausesValidation="False"
                                OnClientClick="return confirm('¿Eliminar esta compra? Solo es posible si no tiene inventario asociado.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMensajeLista" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

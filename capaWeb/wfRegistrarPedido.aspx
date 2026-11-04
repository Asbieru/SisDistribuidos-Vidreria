<%@ Page Title="Registrar Pedido" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarPedido.aspx.vb" Inherits="wfRegistrarPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Pedido</h1>
    <p class="subtitulo-pagina">Seleccione el cliente y agregue los vidrios o aluminios que forman el pedido.</p>

    <asp:Panel ID="pnlPedido" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Datos del pedido</h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Cliente * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlCliente" runat="server" CssClass="campo campo-largo">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Buscar cliente :</td>
                <td class="dato">
                    <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="campo campo-corto"
                        MaxLength="150"></asp:TextBox>
                    <asp:Button ID="btnBuscarCliente" runat="server" CssClass="boton boton-primario"
                        Text="Buscar" OnClick="btnBuscarCliente_Click" CausesValidation="False" />
                    <asp:Button ID="btnVerTodosClientes" runat="server" CssClass="boton boton-secundario"
                        Text="Ver todos" OnClick="btnVerTodosClientes_Click" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Usuario :</td>
                <td class="dato">
                    <asp:Label ID="lblUsuario" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <h3 class="panel-subtitulo">Agregar ítem</h3>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Tipo * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlTipoItem" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlTipoItem_SelectedIndexChanged">
                        <asp:ListItem Text="Vidrio suelto" Value="VIDRIO_SUELTO"></asp:ListItem>
                        <asp:ListItem Text="Aluminio" Value="ALUMINIO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
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
                <td class="etiqueta">Etiqueta :</td>
                <td class="dato">
                    <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="campo campo-corto"
                        MaxLength="50"></asp:TextBox>
                    <span class="texto-ayuda">Opcional. Ej.: Ventana sala, vidrio baño.</span>
                </td>
            </tr>
            <tr id="filaAncho" runat="server">
                <td class="etiqueta">Ancho (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAncho" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAlto" runat="server">
                <td class="etiqueta">Alto (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAlto" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaLargo" runat="server">
                <td class="etiqueta">Largo (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtLargo" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Precio materiales * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtPrecioMateriales" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Extras :</td>
                <td class="dato">
                    <asp:TextBox ID="txtExtra" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Mano de obra :</td>
                <td class="dato">
                    <asp:TextBox ID="txtManoObra" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnAgregarItem" runat="server" CssClass="boton boton-primario"
                Text="Agregar ítem" OnClick="btnAgregarItem_Click" CausesValidation="False" />
            <asp:Button ID="btnCancelarItemEdicion" runat="server" CssClass="boton boton-secundario"
                Text="Cancelar edición" OnClick="btnCancelarItemEdicion_Click"
                CausesValidation="False" Visible="False" />
        </div>
        <asp:Label ID="lblMensajeItem" runat="server"></asp:Label>

        <h3 class="panel-subtitulo">Ítems del pedido</h3>

        <div class="tabla-scroll">
            <asp:GridView ID="grvItems" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None"
                OnRowCommand="grvItems_RowCommand"
                EmptyDataText="Agregue al menos un ítem al pedido.">
                <Columns>
                    <asp:BoundField DataField="etiqueta" HeaderText="Etiqueta" />
                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="producto" HeaderText="Producto / variante" />
                    <asp:BoundField DataField="unidad" HeaderText="Unidad" />
                    <asp:BoundField DataField="medida" HeaderText="Medida" />
                    <asp:BoundField DataField="precio_materiales" HeaderText="Materiales"
                        DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="costo_extra" HeaderText="Extras"
                        DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="mano_obra" HeaderText="Mano de obra"
                        DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="total" HeaderText="Total"
                        DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditarItem" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarItem"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkQuitarItem" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="QuitarItem"
                                CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Quitar" CausesValidation="False"
                                OnClientClick="return confirm('¿Quitar este ítem del pedido?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <p class="total-compra"><asp:Label ID="lblTotalPedido" runat="server" Text="TOTAL: S/ 0.00"></asp:Label></p>

        <div class="acciones">
            <asp:Button ID="btnGuardarPedido" runat="server" CssClass="boton boton-primario"
                Text="Guardar pedido" OnClick="btnGuardarPedido_Click" CausesValidation="False" />
            <asp:Button ID="btnLimpiarPedido" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar pedido" OnClick="btnLimpiarPedido_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

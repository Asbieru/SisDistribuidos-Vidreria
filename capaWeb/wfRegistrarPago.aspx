<%@ Page Title="Registrar Pago" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarPago.aspx.vb" Inherits="wfRegistrarPago" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Pago</h1>
    <p class="subtitulo-pagina">Seleccione un pedido y registre sus pagos parciales o el pago total.</p>

    <asp:Panel ID="pnlBusqueda" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Seleccionar pedido</h2>
        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Buscar pedido :</td>
                <td class="dato">
                    <asp:TextBox ID="txtBuscarPedido" runat="server" CssClass="campo campo-corto"
                        MaxLength="150"></asp:TextBox>
                    <asp:Button ID="btnBuscarPedido" runat="server" CssClass="boton boton-primario"
                        Text="Buscar" OnClick="btnBuscarPedido_Click" CausesValidation="False" />
                    <asp:Button ID="btnVerTodos" runat="server" CssClass="boton boton-secundario"
                        Text="Ver todos" OnClick="btnVerTodos_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>

        <div class="tabla-scroll">
            <asp:GridView ID="grvPedidos" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_pedido"
                OnRowCommand="grvPedidos_RowCommand" EmptyDataText="No se encontraron pedidos.">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Acción">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSeleccionarPedido" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="SeleccionarPedido"
                                CommandArgument='<%# Eval("id_pedido") %>'
                                Text="Seleccionar" CausesValidation="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMensajeBusqueda" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlProceso" runat="server" Visible="False">
        <div class="panel">
            <h2 class="panel-titulo">Pago del pedido <asp:Label ID="lblPedidoId" runat="server"></asp:Label></h2>
            <p class="texto-intro"><asp:Label ID="lblPedidoInfo" runat="server"></asp:Label></p>

            <table class="tabla-formulario" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="etiqueta">Estado :</td>
                    <td class="dato"><asp:Label ID="lblEstado" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="etiqueta">Total pagado :</td>
                    <td class="dato"><asp:Label ID="lblPagado" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="etiqueta">Método de pago :</td>
                    <td class="dato">
                        <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="campo campo-corto"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="etiqueta">Cantidad (S/.) :</td>
                    <td class="dato">
                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="campo campo-corto"
                            MaxLength="20"></asp:TextBox>
                        <asp:CheckBox ID="chkVuelto" runat="server" Text="Vuelto" Visible="False" />
                    </td>
                </tr>
            </table>

            <div class="acciones">
                <asp:Button ID="btnGuardarPago" runat="server" CssClass="boton boton-primario"
                    Text="Guardar pago" OnClick="btnGuardarPago_Click" />
                <asp:Button ID="btnCancelarEdicion" runat="server" CssClass="boton boton-secundario"
                    Text="Cancelar edición" OnClick="btnCancelarEdicion_Click"
                    CausesValidation="False" Visible="False" />
                <asp:Button ID="btnCambiarPedido" runat="server" CssClass="boton boton-secundario"
                    Text="Cambiar pedido" OnClick="btnCambiarPedido_Click" CausesValidation="False" />
            </div>
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </div>

        <div class="panel">
            <h2 class="panel-titulo">Pagos registrados</h2>
            <div class="tabla-scroll">
                <asp:GridView ID="grvPagos" runat="server" AutoGenerateColumns="False"
                    CssClass="tabla-datos" GridLines="None" DataKeyNames="id_detalle_pago"
                    OnRowCommand="grvPagos_RowCommand" EmptyDataText="Este pedido todavía no tiene pagos.">
                    <Columns>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                        <asp:BoundField DataField="metodo" HeaderText="Método" />
                        <asp:BoundField DataField="monto" HeaderText="Monto"
                            DataFormatString="{0:N2}" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditarPago" runat="server"
                                    CssClass="boton boton-mini boton-secundario"
                                    CommandName="EditarPago"
                                    CommandArgument='<%# Eval("id_detalle_pago") %>'
                                    Text="Editar" CausesValidation="False" />
                                <asp:LinkButton ID="lnkEliminarPago" runat="server"
                                    CssClass="boton boton-mini boton-peligro"
                                    CommandName="EliminarPago"
                                    CommandArgument='<%# Eval("id_detalle_pago") %>'
                                    Text="Eliminar" CausesValidation="False"
                                    OnClientClick="return confirm('¿Eliminar este pago del pedido?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

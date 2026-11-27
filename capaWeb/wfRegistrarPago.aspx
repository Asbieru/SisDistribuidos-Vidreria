<%@ Page Title="Registrar Pago" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarPago.aspx.vb" Inherits="wfRegistrarPago" MaintainScrollPositionOnPostBack="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Pago</h1>
    <p class="subtitulo-pagina">
        Seleccione un pedido y registre sus pagos parciales o el pago total. Si la cantidad
        supera el saldo, el vuelto se calcula automáticamente y se registra con el método
        que usted elija.
    </p>

    <asp:Panel ID="pnlBusqueda" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Seleccionar pedido</h2>
        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Buscar pedido :</td>
                <td class="dato">
                    <asp:TextBox ID="txtBuscarPedido" runat="server" CssClass="campo campo-corto"
                        MaxLength="150"></asp:TextBox>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="Todos los estados" />
                        <asp:ListItem Value="PENDIENTE" Text="Pendiente" />
                        <asp:ListItem Value="PARCIAL" Text="Parcial" />
                        <asp:ListItem Value="PAGADO" Text="Pagado" />
                        <asp:ListItem Value="CANCELADO" Text="Cancelado" />
                    </asp:DropDownList>
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
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvPedidos_PageIndexChanging"
                OnRowCommand="grvPedidos_RowCommand" EmptyDataText="No se encontraron pedidos."
                PagerStyle-CssClass="paginacion">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha"
                        DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
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
            <asp:Button ID="btnCambiarPedido" runat="server" CssClass="boton boton-secundario"
                Text="&lt;-" ToolTip="Cambiar pedido" aria-label="Cambiar pedido"
                OnClick="btnCambiarPedido_Click" CausesValidation="False" />
            <h2 class="panel-titulo">Pago del pedido <asp:Label ID="lblPedidoId" runat="server"></asp:Label></h2>
            <p class="texto-intro"><asp:Label ID="lblPedidoInfo" runat="server"></asp:Label></p>

            <table class="tabla-formulario" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="etiqueta">Estado :</td>
                    <td class="dato"><asp:Label ID="lblEstado" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="etiqueta">Total pagado :</td>
                    <td class="dato">
                        <asp:Label ID="lblPagado" runat="server"></asp:Label>
                        <span class="texto-ayuda">Dinero neto cobrado (pagos menos vueltos y devoluciones).</span>
                    </td>
                </tr>
                <tr>
                    <td class="etiqueta">Devoluciones registradas :</td>
                    <td class="dato"><asp:Label ID="lblDevuelto" runat="server"></asp:Label></td>
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
                            MaxLength="20" AutoPostBack="True" OnTextChanged="txtCantidad_TextChanged"></asp:TextBox>
                        <asp:Label ID="lblVuelto" runat="server" CssClass="texto-ayuda"></asp:Label>
                    </td>
                </tr>
                <tr id="filaMetodoVuelto" runat="server" Style="display: none;">
                    <td class="etiqueta">Método del vuelto :</td>
                    <td class="dato">
                        <asp:DropDownList ID="ddlMetodoVuelto" runat="server" CssClass="campo campo-corto"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlMetodoVuelto_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="texto-ayuda">Elija con qué método se entregará el vuelto. Si no lo elige, el pago no se registra.</span>
                    </td>
                </tr>
            </table>

            <asp:Label ID="lblAvisoPagado" runat="server" CssClass="mensaje-info" Visible="False"
                Text="El pedido ya está pagado en su totalidad. Puede editar o eliminar los pagos registrados, pero no registrar nuevos montos." />

            <asp:Label ID="lblAvisoCancelado" runat="server" CssClass="mensaje-error" Visible="False"
                Text="Pedido cancelado: no se puede ingresar dinero ni modificar sus pagos." />

            <div class="acciones">
                <asp:Button ID="btnGuardarPago" runat="server" CssClass="boton boton-primario"
                    Text="Guardar pago" OnClick="btnGuardarPago_Click" />
                <asp:Button ID="btnCancelarEdicion" runat="server" CssClass="boton boton-secundario"
                    Text="Cancelar edición" OnClick="btnCancelarEdicion_Click"
                    CausesValidation="False" Visible="False" />
            </div>
            <asp:Label ID="lblMensaje" runat="server"></asp:Label>
        </div>

        <div class="panel">
            <h2 class="panel-titulo">Pagos registrados</h2>
            <div class="tabla-scroll">
                <asp:GridView ID="grvPagos" runat="server" AutoGenerateColumns="False"
                    CssClass="tabla-datos" GridLines="None" DataKeyNames="id_detalle_pago"
                    OnRowCommand="grvPagos_RowCommand" OnRowDataBound="grvPagos_RowDataBound"
                    EmptyDataText="Este pedido todavía no tiene pagos.">
                    <Columns>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha"
                            DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField DataField="metodo" HeaderText="Método" />
                        <asp:BoundField DataField="monto_mostrar" HeaderText="Monto" />
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
                                <asp:Label ID="lblBloqueado" runat="server" CssClass="texto-ayuda" Visible="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

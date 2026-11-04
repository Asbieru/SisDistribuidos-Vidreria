<%@ Page Title="Listado de Compras" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfListadoCompras.aspx.vb" Inherits="wfListadoCompras" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Listado de Compras</h1>
    <p class="subtitulo-pagina">Consulte las compras y sus productos.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Proveedor:</td>
                <td class="dato">
                    <asp:TextBox ID="txtProveedor" runat="server" MaxLength="150" />
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario" OnClick="btnBuscar_Click" />
                </td>
            </tr>
            <tr><td class="etiqueta">Desde:</td><td><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td></tr>
            <tr><td class="etiqueta">Hasta:</td><td><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td></tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnVerTodas" runat="server" Text="Ver todas" CssClass="boton boton-secundario" OnClick="btnVerTodas_Click" />
            <asp:HyperLink ID="lnkRegistro" runat="server" NavigateUrl="~/wfRegistrarCompras.aspx" CssClass="boton boton-secundario">Registrar Compra</asp:HyperLink>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvCompras" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No se encontraron compras." OnRowCommand="grvCompras_RowCommand">
                <Columns>
            <asp:BoundField DataField="id_compra" HeaderText="ID" />
            <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="total" HeaderText="Total (S/)" DataFormatString="{0:N3}" />
            <asp:BoundField DataField="motivo" HeaderText="Motivo" />
            <asp:BoundField DataField="id_pedido_origen" HeaderText="Pedido de origen" NullDisplayText="No aplica" />
                    <asp:TemplateField HeaderText="Detalle">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetalle" runat="server" Text="Ver detalle"
                                CssClass="boton boton-mini boton-secundario" CommandName="VerDetalle"
                                CommandArgument='<%# Eval("id_compra") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlDetalle" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo"><asp:Label ID="lblDetalle" runat="server" /></h2>
        <div class="tabla-scroll">
            <asp:GridView ID="grvDetalle" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="Esta compra no tiene productos registrados.">
                <Columns>
            <asp:BoundField DataField="producto" HeaderText="Producto / variante" />
            <asp:BoundField DataField="unidad" HeaderText="Unidad" />
            <asp:BoundField DataField="medida" HeaderText="Medida" />
            <asp:BoundField DataField="piezas" HeaderText="Conteo" />
            <asp:BoundField DataField="cantidad" HeaderText="Cantidad facturada" DataFormatString="{0:N3}" />
            <asp:BoundField DataField="costo_unitario" HeaderText="Costo unitario" DataFormatString="{0:N3}" />
            <asp:BoundField DataField="subtotal" HeaderText="Subtotal" DataFormatString="{0:N3}" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="acciones">
            <asp:Button ID="btnCerrarDetalle" runat="server" Text="Cerrar detalle" CssClass="boton boton-secundario" OnClick="btnCerrarDetalle_Click" />
        </div>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Cajas y unidades" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfMovimientoCajas.aspx.vb" Inherits="wfMovimientoCajas" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Cajas y unidades</h1>
    <p class="subtitulo-pagina">Tornillos y tarugos (por caja) y accesorios (por unidad).</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <div class="acciones">
            <asp:HyperLink ID="lnkInventario" runat="server" NavigateUrl="~/wfListadoInventario.aspx" CssClass="boton boton-secundario">Volver al inventario</asp:HyperLink>
        </div>
        <div class="tabla-scroll">
            <asp:GridView ID="grvCajas" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay productos por caja o unidad."
                DataKeyNames="id_variante,producto,unidad_costo,stock_cajas" OnRowCommand="grvCajas_RowCommand">
                <Columns>
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="unidad_costo" HeaderText="Unidad" />
                    <asp:BoundField DataField="stock_cajas" HeaderText="Stock" />
                    <asp:BoundField DataField="stock_minimo" HeaderText="Stock mínimo" />
                    <asp:CheckBoxField DataField="bajo_minimo" HeaderText="Bajo mínimo" />
                    <asp:BoundField DataField="precio_unitario" HeaderText="Precio (S/)" DataFormatString="{0:N2}" />
                    <asp:ButtonField ButtonType="Link" CommandName="Mover" Text="Registrar movimiento"
                        ControlStyle-CssClass="boton boton-mini boton-secundario" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlMovimiento" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo"><asp:Label ID="lblProducto" runat="server" /></h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Tipo:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlTipo" runat="server">
                        <asp:ListItem Value="COMPRA">Entrada (compra)</asp:ListItem>
                        <asp:ListItem Value="VENTA">Salida (venta)</asp:ListItem>
                        <asp:ListItem Value="DEFECTO">Salida por defecto</asp:ListItem>
                        <asp:ListItem Value="AJUSTE">Ajuste a conteo físico</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Cantidad:</td>
                <td class="dato"><asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Observación:</td>
                <td class="dato"><asp:TextBox ID="txtObservacion" runat="server" MaxLength="200" /></td>
            </tr>
        </table>
        <p>En "Ajuste a conteo físico" la cantidad es el stock real contado: el stock pasa a ser exactamente ese número.</p>
        <div class="acciones">
            <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="boton boton-primario" OnClick="btnRegistrar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton boton-secundario" OnClick="btnCancelar_Click" />
        </div>
    </asp:Panel>
</asp:Content>

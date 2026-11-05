<%@ Page Title="Inventario" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfListadoInventario.aspx.vb" Inherits="wfListadoInventario" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Inventario</h1>
    <p class="subtitulo-pagina">Existencias por producto y stock mínimo.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Categoría:</td>
                <td class="dato"><asp:DropDownList ID="ddlCategoria" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta"></td>
                <td class="dato"><asp:CheckBox ID="chkBajoMinimo" runat="server" Text="Solo los que están en o bajo el stock mínimo" /></td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario" OnClick="btnBuscar_Click" />
            <asp:HyperLink ID="lnkPiezas" runat="server" NavigateUrl="~/wfVerPiezas.aspx" CssClass="boton boton-secundario">Ver piezas</asp:HyperLink>
            <asp:HyperLink ID="lnkCajas" runat="server" NavigateUrl="~/wfMovimientoCajas.aspx" CssClass="boton boton-secundario">Cajas y unidades</asp:HyperLink>
            <asp:HyperLink ID="lnkKardex" runat="server" NavigateUrl="~/wfVerKardex.aspx" CssClass="boton boton-secundario">Kardex</asp:HyperLink>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvInventario" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No se encontraron productos."
                DataKeyNames="id_variante,producto,stock_minimo" OnRowCommand="grvInventario_RowCommand">
                <Columns>
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="unidad_costo" HeaderText="Unidad" />
                    <asp:BoundField DataField="existencias" HeaderText="Existencias" />
                    <asp:BoundField DataField="stock_minimo" HeaderText="Stock mínimo" />
                    <asp:CheckBoxField DataField="bajo_minimo" HeaderText="Bajo mínimo" />
                    <asp:BoundField DataField="piezas_reservadas" HeaderText="Piezas reservadas" />
                    <asp:BoundField DataField="precio_unitario" HeaderText="Precio (S/)" DataFormatString="{0:N2}" />
                    <asp:ButtonField ButtonType="Link" CommandName="EditarMinimo" Text="Stock mínimo"
                        ControlStyle-CssClass="boton boton-mini boton-secundario" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlMinimo" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo">Stock mínimo: <asp:Label ID="lblProducto" runat="server" /></h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Nuevo stock mínimo:</td>
                <td class="dato"><asp:TextBox ID="txtMinimo" runat="server" TextMode="Number" /></td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnGuardarMinimo" runat="server" Text="Guardar" CssClass="boton boton-primario" OnClick="btnGuardarMinimo_Click" />
            <asp:Button ID="btnCancelarMinimo" runat="server" Text="Cancelar" CssClass="boton boton-secundario" OnClick="btnCancelarMinimo_Click" />
        </div>
    </asp:Panel>
</asp:Content>

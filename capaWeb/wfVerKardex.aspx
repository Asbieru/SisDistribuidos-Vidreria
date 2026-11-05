<%@ Page Title="Kardex" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfVerKardex.aspx.vb" Inherits="wfVerKardex" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Kardex</h1>
    <p class="subtitulo-pagina">Historial de entradas, salidas y ajustes del inventario.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlKardex" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Producto:</td>
                <td class="dato"><asp:DropDownList ID="ddlVariante" runat="server" /></td>
            </tr>
            <tr><td class="etiqueta">Desde:</td><td><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td></tr>
            <tr><td class="etiqueta">Hasta:</td><td><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td></tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario" OnClick="btnBuscar_Click" />
            <asp:HyperLink ID="lnkInventario" runat="server" NavigateUrl="~/wfListadoInventario.aspx" CssClass="boton boton-secundario">Volver al inventario</asp:HyperLink>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvKardex" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay movimientos en ese rango.">
                <Columns>
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="id_pieza" HeaderText="Pieza" />
                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="motivo" HeaderText="Motivo" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="stock_resultante" HeaderText="Stock resultante" />
                    <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="observacion" HeaderText="Observación" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Reporte de Compras por Tiempo" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfReporteComprasTiempo.aspx.vb" Inherits="wfReporteComprasTiempo" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Reporte de Compras por Tiempo</h1>
    <p class="subtitulo-pagina">Evolución mensual de las compras registradas.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlReporte" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Desde:</td>
                <td class="dato"><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Hasta:</td>
                <td class="dato"><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Ordenar por:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlOrden" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlOrden_SelectedIndexChanged">
                        <asp:ListItem Value="FECHA_DESC">Fecha (más reciente)</asp:ListItem>
                        <asp:ListItem Value="FECHA_ASC">Fecha (más antigua)</asp:ListItem>
                        <asp:ListItem Value="PROVEEDOR_ASC">Proveedor (A-Z)</asp:ListItem>
                        <asp:ListItem Value="PROVEEDOR_DESC">Proveedor (Z-A)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnGenerar" runat="server" Text="Generar reporte"
                CssClass="boton boton-primario" OnClick="btnGenerar_Click" />
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvCompras" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay compras en el período seleccionado."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvCompras_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="periodo" HeaderText="Mes" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:BoundField DataField="cantidad_compras" HeaderText="Compras" />
                    <asp:BoundField DataField="total_compras" HeaderText="Total (S/)" DataFormatString="{0:N3}" />
                    <asp:BoundField DataField="promedio_compra" HeaderText="Promedio (S/)" DataFormatString="{0:N3}" />
                </Columns>
                <PagerStyle CssClass="paginacion" />
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

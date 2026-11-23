<%@ Page Title="Reporte de Pedidos del Mes" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfReportePedidosMes.aspx.vb" Inherits="wfReportePedidosMes" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Reporte de Pedidos del Mes</h1>
    <p class="subtitulo-pagina">Pedidos registrados en el mes seleccionado y su situación de cobro.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlReporte" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Mes:</td>
                <td class="dato"><asp:DropDownList ID="ddlMes" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Año:</td>
                <td class="dato"><asp:DropDownList ID="ddlAnio" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Estado de pago:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="TODOS">Todos</asp:ListItem>
                        <asp:ListItem Value="PENDIENTE">Pendiente</asp:ListItem>
                        <asp:ListItem Value="PARCIAL">Parcial</asp:ListItem>
                        <asp:ListItem Value="PAGADO">Pagado</asp:ListItem>
                        <asp:ListItem Value="EXCEDENTE">Excedente</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Ordenar por:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlOrden" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlOrden_SelectedIndexChanged">
                        <asp:ListItem Value="FECHA_DESC">Fecha (más reciente)</asp:ListItem>
                        <asp:ListItem Value="FECHA_ASC">Fecha (más antigua)</asp:ListItem>
                        <asp:ListItem Value="CLIENTE_ASC">Cliente (A-Z)</asp:ListItem>
                        <asp:ListItem Value="CLIENTE_DESC">Cliente (Z-A)</asp:ListItem>
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
            <asp:GridView ID="grvPedidos" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay pedidos en el mes seleccionado."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvPedidos_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="vendedor" HeaderText="Vendedor" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="estado_trabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="total" HeaderText="Importe (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="pagado" HeaderText="Cobrado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="saldo" HeaderText="Saldo (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="estado_pago" HeaderText="Estado de pago" />
                </Columns>
                <PagerStyle CssClass="paginacion" />
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

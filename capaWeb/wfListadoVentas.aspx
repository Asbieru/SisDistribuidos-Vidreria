<%@ Page Title="Listado de Ventas" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfListadoVentas.aspx.vb" Inherits="wfListadoVentas" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Listado de Ventas</h1>
    <p class="subtitulo-pagina">Consulte los pedidos, cobros y saldos.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Cliente:</td>
                <td class="dato">
                    <asp:TextBox ID="txtCliente" runat="server" MaxLength="150" />
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario" OnClick="btnBuscar_Click" />
                </td>
            </tr>
            <tr><td class="etiqueta">Desde:</td><td class="dato"><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td></tr>
            <tr><td class="etiqueta">Hasta:</td><td class="dato"><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td></tr>
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
        </table>
        <div class="acciones">
            <asp:Button ID="btnVerTodas" runat="server" Text="Ver todas" CssClass="boton boton-secundario" OnClick="btnVerTodas_Click" />
        </div>
        <p><asp:Label ID="lblResultados" runat="server" /></p>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvVentas" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No se encontraron ventas.">
                <Columns>
                    <asp:BoundField DataField="id_pedido" HeaderText="Pedido" />
                    <asp:BoundField DataField="cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="vendedor" HeaderText="Vendedor" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="estado_trabajo" HeaderText="Trabajo" />
                    <asp:BoundField DataField="total" HeaderText="Importe (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="pagado" HeaderText="Cobrado (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="saldo" HeaderText="Saldo (S/)" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="estado_pago" HeaderText="Estado de pago" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

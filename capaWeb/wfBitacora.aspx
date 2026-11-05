<%@ Page Title="Bitácora" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfBitacora.aspx.vb" Inherits="wfBitacora" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Reporte - Bitácora</h1>
    <p class="subtitulo-pagina">Accesos e incidencias del inicio de sesión.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlBitacora" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Acción:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlAccion" runat="server" />
                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="boton boton-primario" OnClick="btnFiltrar_Click" />
                </td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnRefrescar" runat="server" Text="Refrescar" CssClass="boton boton-secundario" OnClick="btnRefrescar_Click" />
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvBitacora" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay registros en la bitácora.">
                <Columns>
                    <asp:BoundField DataField="idRegistro" HeaderText="N°" />
                    <asp:BoundField DataField="fechaReg" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="horaReg" HeaderText="Hora" />
                    <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="rol" HeaderText="Rol" />
                    <asp:BoundField DataField="accion" HeaderText="Acción" />
                    <asp:BoundField DataField="numIntentos" HeaderText="Intentos fallidos" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

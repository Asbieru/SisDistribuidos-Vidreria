<%@ Page Title="Piezas" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfVerPiezas.aspx.vb" Inherits="wfVerPiezas" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Piezas de vidrio y aluminio</h1>
    <p class="subtitulo-pagina">Consulte las piezas, cámbieles el estado o córtelas.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Producto:</td>
                <td class="dato"><asp:DropDownList ID="ddlVariante" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Estado:</td>
                <td class="dato"><asp:DropDownList ID="ddlEstado" runat="server" /></td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton boton-primario" OnClick="btnBuscar_Click" />
            <asp:HyperLink ID="lnkInventario" runat="server" NavigateUrl="~/wfListadoInventario.aspx" CssClass="boton boton-secundario">Volver al inventario</asp:HyperLink>
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvPiezas" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No se encontraron piezas."
                DataKeyNames="id_pieza,producto,estado" OnRowCommand="grvPiezas_RowCommand">
                <Columns>
                    <asp:BoundField DataField="id_pieza" HeaderText="N° pieza" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="ancho_cm" HeaderText="Ancho (cm)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="alto_cm" HeaderText="Alto (cm)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="largo_cm" HeaderText="Largo (cm)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="origen" HeaderText="Origen" />
                    <asp:BoundField DataField="fecha_ingreso" HeaderText="Ingreso" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:ButtonField ButtonType="Link" CommandName="Editar" Text="Editar"
                        ControlStyle-CssClass="boton boton-mini boton-secundario" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlEditar" runat="server" CssClass="panel" Visible="False">
        <h2 class="panel-titulo"><asp:Label ID="lblPieza" runat="server" /></h2>
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Nuevo estado:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlNuevoEstado" runat="server" />
                    <asp:Button ID="btnCambiarEstado" runat="server" Text="Cambiar estado" CssClass="boton boton-primario" OnClick="btnCambiarEstado_Click" />
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Observación:</td>
                <td class="dato"><asp:TextBox ID="txtObservacion" runat="server" MaxLength="200" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Cortar (cm):</td>
                <td class="dato">
                    Ancho <asp:TextBox ID="txtAncho" runat="server" Width="70" />
                    Alto <asp:TextBox ID="txtAlto" runat="server" Width="70" />
                    Largo <asp:TextBox ID="txtLargo" runat="server" Width="70" />
                    <asp:Button ID="btnCortar" runat="server" Text="Cortar" CssClass="boton boton-primario" OnClick="btnCortar_Click"
                        OnClientClick="return confirm('¿Cortar la pieza? La pieza original quedará CONSUMIDA.');" />
                </td>
            </tr>
        </table>
        <p>Vidrio: ingrese ancho y alto. Aluminio: ingrese solo el largo. Los sobrantes útiles quedan como retazos disponibles.</p>
        <div class="acciones">
            <asp:Button ID="btnCerrarEdicion" runat="server" Text="Cerrar" CssClass="boton boton-secundario" OnClick="btnCerrarEdicion_Click" />
        </div>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Registrar Pieza" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarPieza.aspx.vb" Inherits="wfRegistrarPieza" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Pieza</h1>
    <p class="subtitulo-pagina">Ingrese una pieza de vidrio o aluminio al inventario inicial.</p>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Nueva pieza</h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Producto * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlVariante" runat="server" CssClass="campo campo-largo"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlVariante_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Ancho (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAncho" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Alto (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAlto" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Largo (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtLargo" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Observación :</td>
                <td class="dato">
                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="campo campo-largo"
                        TextMode="MultiLine" Rows="2" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Ayuda :</td>
                <td class="dato">
                    <asp:Label ID="lblAyuda" runat="server" CssClass="texto-ayuda"></asp:Label>
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnGuardar" runat="server" CssClass="boton boton-primario"
                Text="Guardar pieza" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar" OnClick="btnLimpiar_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Últimas piezas registradas</h2>

        <div class="acciones" style="margin-top: 0; padding-top: 0; border-top: none;">
            <asp:Button ID="btnRecargar" runat="server" CssClass="boton boton-secundario"
                Text="Recargar listado" OnClick="btnRecargar_Click" CausesValidation="False" />
        </div>

        <div class="tabla-scroll">
            <asp:GridView ID="grvPiezas" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None"
                EmptyDataText="Todavía no hay piezas registradas.">
                <Columns>
                    <asp:BoundField DataField="id_pieza" HeaderText="Pieza" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="ancho_cm" HeaderText="Ancho" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="alto_cm" HeaderText="Alto" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="largo_cm" HeaderText="Largo" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="area_cm2" HeaderText="Área cm²" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="origen" HeaderText="Origen" />
                    <asp:BoundField DataField="fecha_ingreso" HeaderText="Ingreso"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Permisos" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfPermisos.aspx.vb" Inherits="wfPermisos" %>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Mantenimiento - Permisos de usuario</h1>
    <p class="subtitulo-pagina">Elija un usuario y pase los permisos de una lista a otra. Los cambios se aplican al presionar GUARDAR.</p>
    <asp:Label ID="lblMensaje" runat="server" />

    <asp:Panel ID="pnlPermisos" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Usuario:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlUsuarios" runat="server" AutoPostBack="true" CssClass="campo campo-largo" />
                </td>
            </tr>
            <tr>
                <td class="etiqueta"></td>
                <td class="dato"><asp:Label ID="lblRol" runat="server" CssClass="texto-ayuda" /></td>
            </tr>
        </table>

        <table class="tabla-formulario">
            <tr>
                <td class="dato">
                    <strong>Permisos disponibles:</strong><br />
                    <asp:ListBox ID="lstDisponibles" runat="server" SelectionMode="Multiple" Rows="10" Width="260px" />
                </td>
                <td class="dato texto-centro">
                    <asp:Button ID="btnAgregar" runat="server" Text="AGREGAR &gt;&gt;" CssClass="boton boton-secundario" /><br /><br />
                    <asp:Button ID="btnQuitar" runat="server" Text="&lt;&lt; QUITAR" CssClass="boton boton-secundario" />
                </td>
                <td class="dato">
                    <strong>Permisos asignados:</strong><br />
                    <asp:ListBox ID="lstAsignados" runat="server" SelectionMode="Multiple" Rows="10" Width="260px" />
                </td>
            </tr>
        </table>

        <p><asp:Label ID="lblCantidad" runat="server" /></p>
        <p class="texto-ayuda">Use Ctrl + clic para seleccionar varios permisos a la vez.</p>

        <div class="acciones">
            <asp:Button ID="btnPermisosRol" runat="server" Text="PERMISOS DEL ROL" CssClass="boton boton-secundario"
                OnClientClick="return confirm('¿Cargar los permisos por defecto del rol? Se reemplazarán los de la lista (se guardan al presionar GUARDAR).');" />
            <asp:Button ID="btnGuardar" runat="server" Text="GUARDAR" CssClass="boton boton-primario" />
            <a class="boton boton-secundario" href="wfInicio.aspx">CERRAR</a>
        </div>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Proveedores" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarProveedores.aspx.vb" Inherits="wfRegistrarProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Proveedores</h1>
    <p class="subtitulo-pagina">Registre un proveedor nuevo o modifique los datos de uno existente.</p>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2 class="panel-titulo"><asp:Label ID="lblModo" runat="server" Text="NUEVO PROVEEDOR"></asp:Label></h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">ID :</td>
                <td class="dato">
                    <asp:TextBox ID="txtId" runat="server" CssClass="campo campo-corto"
                        ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Tipo de persona * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlTipoPersona" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlIdentificacion_SelectedIndexChanged">
                        <asp:ListItem Text="Natural" Value="NATURAL"></asp:ListItem>
                        <asp:ListItem Text="Jurídica" Value="JURIDICA"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Tipo de documento * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlIdentificacion_SelectedIndexChanged">
                        <asp:ListItem Text="DNI" Value="DNI"></asp:ListItem>
                        <asp:ListItem Text="RUC" Value="RUC"></asp:ListItem>
                        <asp:ListItem Text="Sin documento" Value="SIN DOCUMENTO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta"><asp:Label ID="lblNombreCampo" runat="server" Text="Nombres y apellidos * :"></asp:Label></td>
                <td class="dato">
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="campo campo-largo"
                        MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Número de documento :</td>
                <td class="dato">
                    <asp:TextBox ID="txtDocumento" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Teléfono :</td>
                <td class="dato">
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="campo campo-corto"
                        MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Dirección :</td>
                <td class="dato">
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="campo campo-largo"
                        MaxLength="255"></asp:TextBox>
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
                Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnNuevo" runat="server" CssClass="boton boton-secundario"
                Text="Nuevo proveedor" OnClick="btnNuevo_Click" CausesValidation="False" />
            <asp:Button ID="btnRecargar" runat="server" CssClass="boton boton-secundario"
                Text="Recargar" OnClick="btnRecargar_Click" CausesValidation="False" Visible="False" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar" OnClick="btnNuevo_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Proveedores registrados</h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Buscar :</td>
                <td class="dato">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="campo campo-corto"
                        MaxLength="150"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" CssClass="boton boton-primario"
                        Text="Buscar" OnClick="btnBuscar_Click" CausesValidation="False" />
                    <asp:Button ID="btnVerTodos" runat="server" CssClass="boton boton-secundario"
                        Text="Ver todos" OnClick="btnVerTodos_Click" CausesValidation="False" />
                </td>
            </tr>
        </table>

        <div class="tabla-scroll">
            <asp:GridView ID="grvProveedores" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_proveedor"
                OnRowCommand="grvProveedores_RowCommand" EmptyDataText="No hay proveedores registrados.">
                <Columns>
                    <asp:BoundField DataField="id_proveedor" HeaderText="ID" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre / Razón social" />
                    <asp:BoundField DataField="documento" HeaderText="Documento" />
                    <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="direccion" HeaderText="Dirección" />
                    <asp:BoundField DataField="tipo_persona" HeaderText="Persona" />
                    <asp:BoundField DataField="tipo_documento" HeaderText="Documento" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditar" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarProveedor"
                                CommandArgument='<%# Eval("id_proveedor") %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkEliminar" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="EliminarProveedor"
                                CommandArgument='<%# Eval("id_proveedor") %>'
                                Text="Eliminar" CausesValidation="False"
                                OnClientClick="return confirm('¿Eliminar este proveedor?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMensajeLista" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

<%@ Page Title="Usuarios" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarUsuarios.aspx.vb" Inherits="wfRegistrarUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Usuarios</h1>
    <p class="subtitulo-pagina">Registre un usuario nuevo o modifique sus datos y permisos.</p>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2 class="panel-titulo"><asp:Label ID="lblModo" runat="server" Text="NUEVO USUARIO"></asp:Label></h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">ID :</td>
                <td class="dato">
                    <asp:TextBox ID="txtId" runat="server" CssClass="campo campo-corto"
                        ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Usuario * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="campo campo-corto"
                        MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Correo * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="campo campo-largo"
                        MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Contraseña * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="campo campo-corto"
                        MaxLength="255"></asp:TextBox>
                    <span class="texto-ayuda">Mínimo 4 caracteres.</span>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Pregunta de seguridad * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtPregunta" runat="server" CssClass="campo campo-largo"
                        MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Respuesta * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtRespuesta" runat="server" CssClass="campo campo-largo"
                        MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Rol * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="campo campo-corto">
                        <asp:ListItem Text="VENDEDOR" Value="VENDEDOR"></asp:ListItem>
                        <asp:ListItem Text="ADMINISTRADOR" Value="ADMINISTRADOR"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="filaActivo" runat="server" visible="false">
                <td class="etiqueta">Estado :</td>
                <td class="dato">
                    <asp:CheckBox ID="chkActivo" runat="server" Text="Usuario activo" />
                </td>
            </tr>
            <tr id="filaPermisos" runat="server" visible="false">
                <td class="etiqueta">Permisos :</td>
                <td class="dato">
                    <asp:CheckBox ID="chkReasignarPermisos" runat="server"
                        Text="Reemplazar sus permisos por los del rol seleccionado" />
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnGuardar" runat="server" CssClass="boton boton-primario"
                Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnNuevo" runat="server" CssClass="boton boton-secundario"
                Text="Nuevo usuario" OnClick="btnNuevo_Click" CausesValidation="False" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar" OnClick="btnNuevo_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Usuarios registrados</h2>

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
            <asp:GridView ID="grvUsuarios" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_usuario"
                OnRowCommand="grvUsuarios_RowCommand" EmptyDataText="No hay usuarios registrados.">
                <Columns>
                    <asp:BoundField DataField="id_usuario" HeaderText="ID" />
                    <asp:BoundField DataField="nombre" HeaderText="Usuario" />
                    <asp:BoundField DataField="correo" HeaderText="Correo" />
                    <asp:BoundField DataField="rol" HeaderText="Rol" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="fecha_creacion" HeaderText="Creado"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditar" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarUsuario"
                                CommandArgument='<%# Eval("id_usuario") %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkEstado" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="CambiarEstadoUsuario"
                                CommandArgument='<%# Eval("id_usuario") %>'
                                Text='<%# If(Eval("estado").ToString() = "ACTIVO", "Inactivar", "Activar") %>'
                                CausesValidation="False" />
                            <asp:LinkButton ID="lnkEliminar" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="EliminarUsuario"
                                CommandArgument='<%# Eval("id_usuario") %>'
                                Text="Eliminar" CausesValidation="False"
                                OnClientClick="return confirm('¿Eliminar este usuario?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMensajeLista" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

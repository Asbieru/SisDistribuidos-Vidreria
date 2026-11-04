<%@ Page Title="Productos" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarProductos.aspx.vb" Inherits="wfRegistrarProductos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Productos</h1>
    <p class="subtitulo-pagina">Registre un producto nuevo o modifique los datos de uno existente.</p>

    <asp:Panel ID="pnlFormulario" runat="server" CssClass="panel">
        <h2 class="panel-titulo"><asp:Label ID="lblModo" runat="server" Text="NUEVO PRODUCTO"></asp:Label></h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">ID :</td>
                <td class="dato">
                    <asp:TextBox ID="txtId" runat="server" CssClass="campo campo-corto"
                        ReadOnly="True" Enabled="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Categoría * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="campo campo-corto">
                        <asp:ListItem Text="VIDRIO" Value="VIDRIO"></asp:ListItem>
                        <asp:ListItem Text="ALUMINIO" Value="ALUMINIO"></asp:ListItem>
                        <asp:ListItem Text="ACCESORIO" Value="ACCESORIO"></asp:ListItem>
                        <asp:ListItem Text="TORNILLO" Value="TORNILLO"></asp:ListItem>
                        <asp:ListItem Text="TARUGO" Value="TARUGO"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Tipo * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtTipo" runat="server" CssClass="campo campo-corto"
                        MaxLength="50"></asp:TextBox>
                    <span class="texto-ayuda">Ej.: transparente, catedral, u13, autorroscante.</span>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Color :</td>
                <td class="dato">
                    <asp:TextBox ID="txtColor" runat="server" CssClass="campo campo-corto"
                        MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Nombre * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="campo campo-largo"
                        MaxLength="150"></asp:TextBox>
                    <asp:Button ID="btnSugerirNombre" runat="server" CssClass="boton boton-mini boton-secundario"
                        Text="Sugerir nombre" OnClick="btnSugerirNombre_Click" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Descripción :</td>
                <td class="dato">
                    <asp:TextBox ID="txtDescripcion" runat="server" CssClass="campo campo-largo"
                        TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaActivo" runat="server" visible="false">
                <td class="etiqueta">Estado :</td>
                <td class="dato">
                    <asp:CheckBox ID="chkActivo" runat="server" Text="Producto activo" />
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnGuardar" runat="server" CssClass="boton boton-primario"
                Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnNuevo" runat="server" CssClass="boton boton-secundario"
                Text="Nuevo producto" OnClick="btnNuevo_Click" CausesValidation="False" />
            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar" OnClick="btnNuevo_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlListado" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Productos registrados</h2>

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
            <asp:GridView ID="grvProductos" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None" DataKeyNames="id_producto"
                OnRowCommand="grvProductos_RowCommand" EmptyDataText="No hay productos registrados.">
                <Columns>
                    <asp:BoundField DataField="id_producto" HeaderText="ID" />
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="color" HeaderText="Color" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="variantes" HeaderText="Variantes" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditar" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarProducto"
                                CommandArgument='<%# Eval("id_producto") %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkEstado" runat="server"
                                CssClass="boton boton-mini boton-secundario"
                                CommandName="CambiarEstadoProducto"
                                CommandArgument='<%# Eval("id_producto") %>'
                                Text='<%# If(Convert.ToBoolean(Eval("activo")), "Inactivar", "Activar") %>'
                                CausesValidation="False" />
                            <asp:LinkButton ID="lnkEliminar" runat="server"
                                CssClass="boton boton-mini boton-peligro"
                                CommandName="EliminarProducto"
                                CommandArgument='<%# Eval("id_producto") %>'
                                Text="Eliminar" CausesValidation="False"
                                OnClientClick="return confirm('¿Eliminar este producto? Solo es posible si no tiene variantes.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Label ID="lblMensajeLista" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

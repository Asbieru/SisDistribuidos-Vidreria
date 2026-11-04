<%@ Page Title="Inicio" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfInicio.aspx.vb" Inherits="wfInicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Bienvenidos al Sistema SISCORP</h1>
    <p class="texto-intro">
        Esta aplicación está diseñada para gestionar las operaciones comerciales de la vidriería:
        control de acceso, mantenimientos, compras, pedidos, pagos e inventario.
    </p>

    <asp:Panel ID="pnlAviso" runat="server" Visible="False" CssClass="panel">
        <asp:Label ID="lblAviso" runat="server" CssClass="mensaje-ok"></asp:Label>
    </asp:Panel>

    <table class="tabla-datos" cellpadding="0" cellspacing="0">
        <tr>
            <th style="width: 220px;">Módulo</th>
            <th>Descripción</th>
            <th style="width: 130px;">Acceso</th>
        </tr>
        <tr>
            <td><strong>Registrar pedido</strong></td>
            <td>Registrar un pedido con vidrios o aluminios para un cliente.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarPedido.aspx">Abrir</a>
            </td>
        </tr>
        <tr>
            <td><strong>Registrar pago</strong></td>
            <td>Registrar pagos parciales o el pago total de un pedido.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarPago.aspx">Abrir</a>
            </td>
        </tr>
        <tr>
            <td><strong>Registrar compra</strong></td>
            <td>Registrar compras a proveedores e ingresar productos al inventario.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarCompras.aspx">Abrir</a>
            </td>
        </tr>
        <tr>
            <td><strong>Registrar pieza</strong></td>
            <td>Ingresar una pieza de vidrio o aluminio al inventario inicial.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarPieza.aspx">Abrir</a>
            </td>
        </tr>
        <tr>
            <td><strong>Clientes y proveedores</strong></td>
            <td>Administrar los datos de clientes y proveedores.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarClientes.aspx">Clientes</a>
                <a class="boton boton-secundario"
                    href="wfRegistrarProveedores.aspx">Proveedores</a>
            </td>
        </tr>
        <tr>
            <td><strong>Productos y usuarios</strong></td>
            <td>Administrar productos, usuarios y sus permisos.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfRegistrarProductos.aspx">Productos</a>
                <a class="boton boton-secundario"
                    href="wfRegistrarUsuarios.aspx">Usuarios</a>
            </td>
        </tr>
        <tr>
            <td><strong>Configuración</strong></td>
            <td>Cambiar la contraseña, el tema visual y cerrar la sesión.</td>
            <td>
                <a class="boton boton-secundario"
                    href="wfCambiarContrasena.aspx">Abrir</a>
            </td>
        </tr>
    </table>

    <div class="panel">
        <h2 class="panel-titulo">Resumen de la sesión</h2>
        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Usuario conectado :</td>
                <td class="dato"><asp:Label ID="lblUsuarioInicio" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="etiqueta">Rol :</td>
                <td class="dato"><asp:Label ID="lblRolInicio" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="etiqueta">Fecha y hora :</td>
                <td class="dato"><asp:Label ID="lblFecha" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>

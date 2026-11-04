<%@ Page Title="Módulo" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfModulo.aspx.vb" Inherits="wfModulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">
        <asp:Label ID="lblModulo" runat="server"></asp:Label>
    </h1>
    <p class="subtitulo-pagina">Módulo del sistema SISCORP</p>

    <div class="panel">
        <h2 class="panel-titulo">Módulo en construcción</h2>
        <p class="texto-intro">
            Esta opción todavía no tiene su formulario web. El módulo se encuentra
            disponible en la aplicación de escritorio y será migrado en una próxima
            iteración, manteniendo el mismo diseño y las mismas operaciones.
        </p>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Módulo solicitado :</td>
                <td class="dato"><asp:Label ID="lblDetalle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="etiqueta">Estado :</td>
                <td class="dato"><span class="mensaje-info">Pendiente de migración</span></td>
            </tr>
            <tr>
                <td class="etiqueta">Usuario conectado :</td>
                <td class="dato"><asp:Label ID="lblUsuarioModulo" runat="server"></asp:Label></td>
            </tr>
        </table>

        <div class="acciones">
            <a class="boton boton-primario" href="wfInicio.aspx">Volver al inicio</a>
        </div>
    </div>
</asp:Content>

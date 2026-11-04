<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfLogin.aspx.vb" Inherits="wfLogin" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión - SISCORP</title>
    <link rel="icon" href="recursos/favicon.ico" type="image/x-icon" />
    <link href="recursos/estilos/sistema.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /* Fondo negro propio del acceso, por si la hoja de estilos externa
           no se carga o la imagen de fondo no esta disponible. */
        html,
        body,
        body.pagina-acceso {
            background: #000000 !important;
            color: #ffffff;
        }
    </style>
</head>
<body class="tema-oscuro pagina-acceso">
    <form id="form1" runat="server">
        <div class="acceso-contenedor">
            <table class="tabla-acceso" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="acceso-logo">
                        <img src="recursos/logo.png" alt="Logo SISCORP" />
                    </td>
                </tr>
                <tr>
                    <td class="acceso-titulo">INICIO DE SESIÓN</td>
                </tr>
                <tr>
                    <td class="acceso-subtitulo">Ingrese su usuario y contraseña para continuar</td>
                </tr>
                <tr>
                    <td class="acceso-panel">
                        <table class="tabla-campos-acceso" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="acceso-etiqueta">Usuario :</td>
                                <td>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="campo"
                                        MaxLength="100" autocomplete="username"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="acceso-etiqueta">Contraseña :</td>
                                <td>
                                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="campo"
                                        TextMode="Password" MaxLength="100" autocomplete="current-password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="acceso-check">
                                    <asp:CheckBox ID="chbCambiar" runat="server" AutoPostBack="True"
                                        OnCheckedChanged="chbCambiar_CheckedChanged"
                                        Text="Cambiar Contraseña" />
                                </td>
                            </tr>
                        </table>

                        <asp:Panel ID="pnlCambio" runat="server" Visible="False">
                            <table class="tabla-campos-acceso" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="acceso-etiqueta">Pregunta :</td>
                                    <td>
                                        <asp:TextBox ID="txtPregunta" runat="server" CssClass="campo"
                                            ReadOnly="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="acceso-etiqueta">Respuesta :</td>
                                    <td>
                                        <asp:TextBox ID="txtRespuesta" runat="server" CssClass="campo"
                                            MaxLength="200"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" class="acceso-acciones">
                                        <asp:Button ID="btnCambiar" runat="server" CssClass="boton boton-primario"
                                            Text="Cambiar Contraseña" OnClick="btnCambiar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <div class="acceso-acciones">
                            <asp:Button ID="btnIngresar" runat="server" CssClass="boton boton-primario"
                                Text="Ingresar" OnClick="btnIngresar_Click" />
                            <asp:Button ID="btnSalir" runat="server" CssClass="boton boton-secundario"
                                Text="Salir" OnClick="btnSalir_Click" CausesValidation="False" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="acceso-mensaje">
                        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="acceso-pie">© 2026 Todos los derechos reservados USAT</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

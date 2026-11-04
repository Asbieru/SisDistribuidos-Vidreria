<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfCambiarContrasena.aspx.vb" Inherits="wfCambiarContrasena" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Cambiar contraseña - SISCORP</title>
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
                    <td class="acceso-titulo">CAMBIAR CONTRASEÑA</td>
                </tr>
                <tr>
                    <td class="acceso-subtitulo">Ingrese y confirme su nueva contraseña</td>
                </tr>
                <tr>
                    <td class="acceso-panel">
                        <table class="tabla-campos-acceso" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="acceso-etiqueta">Usuario :</td>
                                <td>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="campo"
                                        ReadOnly="True" MaxLength="100" autocomplete="username"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="acceso-etiqueta">Nueva contraseña :</td>
                                <td>
                                    <asp:TextBox ID="txtContrasena" runat="server" CssClass="campo"
                                        TextMode="Password" MaxLength="100" autocomplete="new-password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="acceso-etiqueta">Confirmar contraseña :</td>
                                <td>
                                    <asp:TextBox ID="txtConfirmar" runat="server" CssClass="campo"
                                        TextMode="Password" MaxLength="100" autocomplete="new-password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="acceso-check">
                                    <asp:CheckBox ID="chkMostrar" runat="server" AutoPostBack="True"
                                        OnCheckedChanged="chkMostrar_CheckedChanged"
                                        Text="Mostrar contraseña" />
                                </td>
                            </tr>
                        </table>

                        <div class="acceso-acciones">
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton boton-primario"
                                Text="Guardar" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton boton-secundario"
                                Text="Cancelar" OnClick="btnCancelar_Click" CausesValidation="False" />
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

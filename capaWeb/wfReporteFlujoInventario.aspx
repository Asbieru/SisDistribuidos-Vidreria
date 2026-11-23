<%@ Page Title="Reporte de Flujo de Inventario" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfReporteFlujoInventario.aspx.vb" Inherits="wfReporteFlujoInventario" %>

<asp:Content ID="Head" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        (function () {
            function normalizar(texto) {
                return (texto || '').replace(/^\s+|\s+$/g, '').toLowerCase();
            }

            function prepararCombo(selectId, inputId, listaId) {
                var combo = document.getElementById(selectId);
                var caja = document.getElementById(inputId);
                var lista = document.getElementById(listaId);
                if (!combo || !caja || !lista) return;

                function llenarLista() {
                    lista.innerHTML = '';
                    for (var i = 0; i < combo.options.length; i++) {
                        if (combo.options[i].value === '0' || combo.options[i].value === '') continue;
                        var opcion = document.createElement('option');
                        opcion.value = combo.options[i].text;
                        lista.appendChild(opcion);
                    }
                }

                function sincronizar() {
                    var texto = normalizar(caja.value);
                    var i;
                    if (texto === '') {
                        combo.selectedIndex = 0;
                        return;
                    }

                    for (i = 0; i < combo.options.length; i++) {
                        if (normalizar(combo.options[i].text) === texto) {
                            combo.selectedIndex = i;
                            return;
                        }
                    }

                    for (i = 0; i < combo.options.length; i++) {
                        if (normalizar(combo.options[i].text).indexOf(texto) >= 0) {
                            combo.selectedIndex = i;
                            return;
                        }
                    }

                    combo.selectedIndex = 0;
                }

                llenarLista();
                caja.addEventListener('change', sincronizar);
                caja.addEventListener('blur', sincronizar);
                combo.addEventListener('change', function () {
                    var seleccion = combo.options[combo.selectedIndex];
                    if (seleccion && seleccion.value !== '0') caja.value = seleccion.text;
                });
            }

            function iniciar() {
                prepararCombo('<%= ddlProducto.ClientID %>', '<%= txtProducto.ClientID %>', 'dlProductos');
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', iniciar);
            } else {
                iniciar();
            }
        })();
    </script>
</asp:Content>

<asp:Content ID="Contenido" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="titulo-pagina">Reporte de Flujo de Inventario</h1>
    <p class="subtitulo-pagina">Entradas, salidas y ajustes agrupados por producto y mes.</p>
    <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje-error" />
    <asp:Panel ID="pnlReporte" runat="server" CssClass="panel">
        <table class="tabla-formulario">
            <tr>
                <td class="etiqueta">Desde:</td>
                <td class="dato"><asp:TextBox ID="txtDesde" runat="server" TextMode="Date" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Hasta:</td>
                <td class="dato"><asp:TextBox ID="txtHasta" runat="server" TextMode="Date" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Categoría:</td>
                <td class="dato"><asp:DropDownList ID="ddlCategoria" runat="server" /></td>
            </tr>
            <tr>
                <td class="etiqueta">Producto:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="combo-servidor" Style="display: none;" />
                    <asp:TextBox ID="txtProducto" runat="server" CssClass="campo campo-largo" MaxLength="200" />
                    <datalist id="dlProductos"></datalist>
                    <span class="texto-ayuda">Escriba para filtrar el producto.</span>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Ordenar por:</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlOrden" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlOrden_SelectedIndexChanged">
                        <asp:ListItem Value="FECHA_DESC">Fecha (más reciente)</asp:ListItem>
                        <asp:ListItem Value="FECHA_ASC">Fecha (más antigua)</asp:ListItem>
                        <asp:ListItem Value="PRODUCTO_ASC">Producto (A-Z)</asp:ListItem>
                        <asp:ListItem Value="PRODUCTO_DESC">Producto (Z-A)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div class="acciones">
            <asp:Button ID="btnGenerar" runat="server" Text="Generar reporte"
                CssClass="boton boton-primario" OnClick="btnGenerar_Click" />
        </div>
        <p><asp:Label ID="lblResumen" runat="server" /></p>
        <div class="tabla-scroll">
            <asp:GridView ID="grvFlujo" runat="server" AutoGenerateColumns="False" CssClass="tabla-datos"
                GridLines="None" EmptyDataText="No hay movimientos en el período seleccionado."
                AllowPaging="True" PageSize="10" OnPageIndexChanging="grvFlujo_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="periodo" HeaderText="Mes" />
                    <asp:BoundField DataField="categoria" HeaderText="Categoría" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="unidad_costo" HeaderText="Unidad" />
                    <asp:BoundField DataField="entradas" HeaderText="Entradas" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="salidas" HeaderText="Salidas" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="ajustes" HeaderText="Ajustes" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="cambios_estado" HeaderText="Cambios de estado" />
                    <asp:BoundField DataField="neto" HeaderText="Neto" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="movimientos" HeaderText="Movimientos" />
                </Columns>
                <PagerStyle CssClass="paginacion" />
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>

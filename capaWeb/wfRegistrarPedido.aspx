<%@ Page Title="Registrar Pedido" Language="VB" MasterPageFile="~/plantilla.master" AutoEventWireup="false" CodeFile="wfRegistrarPedido.aspx.vb" Inherits="wfRegistrarPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        (function () {
            function normalizar(texto) {
                return (texto || '').replace(/^\s+|\s+$/g, '').toLowerCase();
            }

            function numero(texto) {
                var n = parseFloat((texto || '').replace(',', '.'));
                return isNaN(n) ? null : n;
            }

            function mostrar(id, visible) {
                var elemento = document.getElementById(id);
                if (elemento) elemento.style.display = visible ? '' : 'none';
            }

            function marcado(id) {
                var elemento = document.getElementById(id);
                return elemento ? elemento.checked : false;
            }

            function prepararCombo(selectId, inputId, listaId) {
                var combo = document.getElementById(selectId);
                var caja = document.getElementById(inputId);
                var lista = document.getElementById(listaId);
                if (!combo || !caja || !lista) return;

                function llenarLista() {
                    lista.innerHTML = '';
                    for (var i = 0; i < combo.options.length; i++) {
                        if (combo.options[i].value === '') continue;
                        var opcion = document.createElement('option');
                        opcion.value = combo.options[i].text;
                        lista.appendChild(opcion);
                    }
                }

                function sincronizar() {
                    var texto = normalizar(caja.value);
                    var i;
                    if (texto === '') return;

                    // Coincidencia exacta: dispara el postback para los combos en cascada.
                    // Tambien acepta decimales con coma ("0,30" == "0.3").
                    var n = numero(caja.value);
                    for (i = 0; i < combo.options.length; i++) {
                        var opcion = normalizar(combo.options[i].text);
                        var nOpcion = numero(combo.options[i].text);
                        if (opcion === texto || (n !== null && nOpcion !== null && n === nOpcion)) {
                            if (i !== combo.selectedIndex) {
                                combo.selectedIndex = i;
                                if (typeof combo.dispatchEvent === 'function') {
                                    combo.dispatchEvent(new Event('change'));
                                }
                            }
                            return;
                        }
                    }

                    // Filtro parcial: solo mueve la selección, NUNCA reescribe
                    // lo que el usuario escribió (antes restauraba el placeholder).
                    for (i = 0; i < combo.options.length; i++) {
                        if (normalizar(combo.options[i].text).indexOf(texto) >= 0) {
                            combo.selectedIndex = i;
                            return;
                        }
                    }
                }

                llenarLista();
                caja.addEventListener('change', sincronizar);
                caja.addEventListener('blur', sincronizar);
                combo.addEventListener('change', function () {
                    var seleccion = combo.options[combo.selectedIndex];
                    if (seleccion && seleccion.value !== '') caja.value = seleccion.text;
                    llenarLista();
                });
            }

            function aplicarVisibilidad() {
                var tipo = normalizar(document.getElementById('<%= hfTipo.ClientID %>').value);
                var sinAlero = tipo.indexOf('sin alero') >= 0;

                mostrar('<%= filaAncho.ClientID %>', tipo === 'vidrio');
                mostrar('<%= filaAncho2.ClientID %>', tipo === 'vidrio' && marcado('<%= chbAncho2.ClientID %>'));
                mostrar('<%= filaAlto.ClientID %>', tipo === 'vidrio');
                mostrar('<%= filaAlto2.ClientID %>', tipo === 'vidrio' && marcado('<%= chbAlto2.ClientID %>'));
                mostrar('<%= filaLargo.ClientID %>', tipo === 'aluminio');
                // hfTipo vale "VENTANA-<modelo>", asi que se compara por prefijo:
                // el espesor del vidrio debe verse en vidrio suelto y en cualquier ventana.
                mostrar('<%= filaEspesor.ClientID %>', tipo.indexOf('aluminio') < 0);
                mostrar('<%= filaColor.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio');
                mostrar('<%= filaVentanaAncho.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio');
                mostrar('<%= filaAnchoCentro.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio' && marcado('<%= chbTresAncho.ClientID %>'));
                mostrar('<%= filaAnchoAbajo.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio' && marcado('<%= chbTresAncho.ClientID %>'));
                mostrar('<%= filaVentanaAlto.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio');
                mostrar('<%= filaAltoCentro.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio' && marcado('<%= chbTresAlto.ClientID %>'));
                mostrar('<%= filaAltoIzq.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio' && marcado('<%= chbTresAlto.ClientID %>'));
                mostrar('<%= filaAlero.ClientID %>', tipo !== 'vidrio' && tipo !== 'aluminio' && !sinAlero);
            }

            function iniciar() {
                prepararCombo('<%= ddlCliente.ClientID %>', '<%= txtCliente.ClientID %>', 'dlClientes');
                prepararCombo('<%= ddlProducto.ClientID %>', '<%= txtProducto.ClientID %>', 'dlProductos');
                prepararCombo('<%= ddlEspesor.ClientID %>', '<%= txtEspesor.ClientID %>', 'dlEspesores');
                prepararCombo('<%= ddlColorAluminio.ClientID %>', '<%= txtColorAluminio.ClientID %>', 'dlColores');

                var checkboxes = [
                    '<%= chbAncho2.ClientID %>', '<%= chbAlto2.ClientID %>',
                    '<%= chbTresAncho.ClientID %>', '<%= chbTresAlto.ClientID %>'];
                for (var i = 0; i < checkboxes.length; i++) {
                    var elemento = document.getElementById(checkboxes[i]);
                    if (elemento) elemento.addEventListener('change', aplicarVisibilidad);
                }
                aplicarVisibilidad();
            }

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', iniciar);
            } else {
                iniciar();
            }
        })();
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="titulo-pagina">Registrar Pedido</h1>
    <p class="subtitulo-pagina">Seleccione el cliente y agregue vidrios, aluminios o ventanas según los modelos configurados.</p>

    <asp:HiddenField ID="hfTipo" runat="server" Value="VIDRIO" />

    <asp:Panel ID="pnlPedido" runat="server" CssClass="panel">
        <h2 class="panel-titulo">Datos del pedido</h2>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Cliente * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlCliente" runat="server" CssClass="combo-servidor" Style="display: none;"></asp:DropDownList>
                    <asp:TextBox ID="txtCliente" runat="server" CssClass="campo campo-largo" MaxLength="150"></asp:TextBox>
                    <datalist id="dlClientes"></datalist>
                    <span class="texto-ayuda">Escriba para filtrar la lista.</span>
                </td>
            </tr>
        </table>

        <h3 class="panel-subtitulo">Agregar ítem</h3>

        <table class="tabla-formulario" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Tipo de pedido * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlTipoItem" runat="server" CssClass="campo campo-corto"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlTipoItem_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Producto / variante * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="combo-servidor" Style="display: none;"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="txtProducto" runat="server" CssClass="campo campo-largo" MaxLength="200"></asp:TextBox>
                    <datalist id="dlProductos"></datalist>
                    <span class="texto-ayuda">Escriba para filtrar la lista.</span>
                </td>
            </tr>
            <tr id="filaEspesor" runat="server">
                <td class="etiqueta">Espesor del vidrio (cm) * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlEspesor" runat="server" CssClass="combo-servidor" Style="display: none;"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlEspesor_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="txtEspesor" runat="server" CssClass="campo campo-corto" MaxLength="20"></asp:TextBox>
                    <datalist id="dlEspesores"></datalist>
                    <span class="texto-ayuda">Se carga según el producto elegido y se usa para calcular el precio del vidrio, también en ventanas.</span>
                </td>
            </tr>
            <tr id="filaColor" runat="server">
                <td class="etiqueta">Color de aluminio * :</td>
                <td class="dato">
                    <asp:DropDownList ID="ddlColorAluminio" runat="server" CssClass="combo-servidor" Style="display: none;"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlColorAluminio_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="txtColorAluminio" runat="server" CssClass="campo campo-corto" MaxLength="40"></asp:TextBox>
                    <datalist id="dlColores"></datalist>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Etiqueta :</td>
                <td class="dato">
                    <asp:TextBox ID="txtEtiqueta" runat="server" CssClass="campo campo-corto" MaxLength="50"></asp:TextBox>
                    <span class="texto-ayuda">Opcional. Ej.: Ventana sala, vidrio baño.</span>
                </td>
            </tr>

            <tr id="filaAncho" runat="server">
                <td class="etiqueta">Ancho (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAncho" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                    <asp:CheckBox ID="chbAncho2" runat="server" Text="Usar segundo ancho" AutoPostBack="True"
                        OnCheckedChanged="Medida_CheckedChanged" CssClass="opcion-medida" />
                </td>
            </tr>
            <tr id="filaAncho2" runat="server" Style="display: none;">
                <td class="etiqueta">Ancho 2 (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAncho2" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAlto" runat="server">
                <td class="etiqueta">Alto (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAlto" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                    <asp:CheckBox ID="chbAlto2" runat="server" Text="Usar segundo alto" AutoPostBack="True"
                        OnCheckedChanged="Medida_CheckedChanged" CssClass="opcion-medida" />
                </td>
            </tr>
            <tr id="filaAlto2" runat="server" Style="display: none;">
                <td class="etiqueta">Alto 2 (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAlto2" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaLargo" runat="server" Style="display: none;">
                <td class="etiqueta">Largo (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtLargo" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>

            <tr id="filaVentanaAncho" runat="server" Style="display: none;">
                <td class="etiqueta">Medidas de ancho * :</td>
                <td class="dato">
                    <asp:CheckBox ID="chbTresAncho" runat="server" Text="Tres medidas de ancho (arriba / centro / abajo)"
                        AutoPostBack="True" OnCheckedChanged="Medida_CheckedChanged" />
                </td>
            </tr>
            <tr id="filaAnchoArriba" runat="server" Style="display: none;">
                <td class="etiqueta">Ancho arriba (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtArriba" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAnchoCentro" runat="server" Style="display: none;">
                <td class="etiqueta">Ancho centro (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtCentroAncho" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAnchoAbajo" runat="server" Style="display: none;">
                <td class="etiqueta">Ancho abajo (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAbajo" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaVentanaAlto" runat="server" Style="display: none;">
                <td class="etiqueta">Medidas de alto * :</td>
                <td class="dato">
                    <asp:CheckBox ID="chbTresAlto" runat="server" Text="Tres medidas de alto (izquierda / centro / derecha)"
                        AutoPostBack="True" OnCheckedChanged="Medida_CheckedChanged" />
                </td>
            </tr>
            <tr id="filaAltoDer" runat="server" Style="display: none;">
                <td class="etiqueta">Alto derecha (cm) * :</td>
                <td class="dato">
                    <asp:TextBox ID="txtDerecha" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAltoCentro" runat="server" Style="display: none;">
                <td class="etiqueta">Alto centro (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtCentroAlto" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAltoIzq" runat="server" Style="display: none;">
                <td class="etiqueta">Alto izquierda (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtIzquierda" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr id="filaAlero" runat="server" Style="display: none;">
                <td class="etiqueta">Alero (cm) :</td>
                <td class="dato">
                    <asp:TextBox ID="txtAlero" runat="server" CssClass="campo campo-corto" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Medida_TextChanged"></asp:TextBox>
                    <span class="texto-ayuda">Se sugiere 35 cm desde 90 cm de alto.</span>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Unidad de cálculo :</td>
                <td class="dato">
                    <asp:Label ID="lblUnidad" runat="server" CssClass="texto-ayuda" Text="(seleccione un producto)"></asp:Label>
                </td>
            </tr>
        </table>

        <h3 class="panel-subtitulo seccion-importes">Importes del ítem</h3>
        <table class="tabla-formulario tabla-importes" cellpadding="0" cellspacing="0">
            <tr>
                <td class="etiqueta">Precio materiales (S/.) :</td>
                <td class="dato">
                    <span class="prefijo-importe">S/.</span>
                    <asp:TextBox ID="txtPrecioMateriales" runat="server" CssClass="campo campo-corto importe" ReadOnly="True"></asp:TextBox>
                    <span class="texto-ayuda">Calculado automáticamente.</span>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Extras (S/.) :</td>
                <td class="dato">
                    <span class="prefijo-importe">S/.</span>
                    <asp:TextBox ID="txtExtra" runat="server" CssClass="campo campo-corto importe" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Importe_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Mano de obra (S/.) :</td>
                <td class="dato">
                    <span class="prefijo-importe">S/.</span>
                    <asp:TextBox ID="txtManoObra" runat="server" CssClass="campo campo-corto importe" MaxLength="20"
                        AutoPostBack="True" OnTextChanged="Importe_TextChanged"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="etiqueta">Total del ítem (S/.) :</td>
                <td class="dato">
                    <span class="prefijo-importe">S/.</span>
                    <asp:TextBox ID="txtTotalItem" runat="server" CssClass="campo campo-corto importe importe-total" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </table>

        <div class="acciones">
            <asp:Button ID="btnAgregarItem" runat="server" CssClass="boton boton-primario"
                Text="Agregar ítem" OnClick="btnAgregarItem_Click" CausesValidation="False" />
            <asp:Button ID="btnCancelarItemEdicion" runat="server" CssClass="boton boton-secundario"
                Text="Cancelar edición" OnClick="btnCancelarItemEdicion_Click" CausesValidation="False" Visible="False" />
        </div>
        <asp:Label ID="lblMensajeItem" runat="server"></asp:Label>

        <h3 class="panel-subtitulo">Ítems del pedido</h3>

        <div class="tabla-scroll">
            <asp:GridView ID="grvItems" runat="server" AutoGenerateColumns="False"
                CssClass="tabla-datos" GridLines="None"
                OnRowCommand="grvItems_RowCommand"
                EmptyDataText="Agregue al menos un ítem al pedido.">
                <Columns>
                    <asp:BoundField DataField="etiqueta" HeaderText="Etiqueta" />
                    <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="medida" HeaderText="Medida" />
                    <asp:BoundField DataField="precio_materiales" HeaderText="Materiales" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="costo_extra" HeaderText="Extras" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="mano_obra" HeaderText="Mano de obra" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:N2}" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditarItem" runat="server" CssClass="boton boton-mini boton-secundario"
                                CommandName="EditarItem" CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Editar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkDuplicarItem" runat="server" CssClass="boton boton-mini boton-secundario"
                                CommandName="DuplicarItem" CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Duplicar" CausesValidation="False" />
                            <asp:LinkButton ID="lnkQuitarItem" runat="server" CssClass="boton boton-mini boton-peligro"
                                CommandName="QuitarItem" CommandArgument='<%# Container.DataItemIndex %>'
                                Text="Quitar" CausesValidation="False"
                                OnClientClick="return confirm('¿Quitar este ítem del pedido?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <p class="total-compra"><asp:Label ID="lblTotalPedido" runat="server" Text="TOTAL: S/ 0"></asp:Label></p>

        <div class="acciones">
            <asp:Button ID="btnGuardarPedido" runat="server" CssClass="boton boton-primario"
                Text="Guardar pedido" OnClick="btnGuardarPedido_Click" CausesValidation="False" />
            <asp:Button ID="btnLimpiarPedido" runat="server" CssClass="boton boton-secundario"
                Text="Limpiar pedido" OnClick="btnLimpiarPedido_Click" CausesValidation="False" />
        </div>
        <asp:Label ID="lblMensaje" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>

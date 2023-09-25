<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges003-001-FacturasComerciales.aspx.vb" Inherits=".Ges003_001_FacturasComerciales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Factura" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

    <style>

        .cl_Secciones {
            opacity: .6;
            color: #757575;      
            font-size: 24px;
            font-weight: bold;
        }
        
        .cl_Tarjeta {      
            font-size: 24px;
            font-weight: bold;   
            color: #432776;               
            display: flex;        
	        justify-content: center;
            align-items: center;   
            
        }

        .cl_Num__Tarjeta {
            background-color: #432776;            
            color: #fff;
            display: flex;        
            border-radius: 50%;           
	        justify-content: center;            
            align-items: center;
            width: 60px;
            height: 60px;
        }

        .cl_Num__Tarjeta {
            font-size: 2.4em;
            font-weight: bold;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
   
    <div class="d-flex">

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="Factura Comercial" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Cargar CFDI" />
                    <GWC:ButtonItem Text="Cargar Paises"/>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>

                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6" ID="dbcNumFacturaCOVE" Label="Número de factura/Folio fiscal" LabelDetail="COVE" OnClick="dbcNumFacturaCOVE_Click"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 px-0 mt-2 py-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 mt-5" ID="icFechaFactura" Rules="require" Type="Text" Format="Calendar" Name="icFechaFactura" Label="Fecha de factura" />
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 mt-5" ID="icFechaCOVE" Type="Text" Format="Calendar" Name="icFechaCOVE" Label="Fecha de COVE" />
                        </asp:Panel>
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5" ID="fbcCliente" Label="Cliente" HasDetails="true" Rules="required" RequiredSelect="true" OnTextChanged="fbcCliente_TextChanged" OnClick="fbcCliente_Click"/>
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5" ID="fbcIncoterm" Label="Incoterm" HasDetails="true" Rules="required" RequiredSelect="true" KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" Dimension="Vt022TerminosFacturacionA14"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4">
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-8 mb-5 p-0" ID="fbcPais" Label="País" RequiredSelect="true" Template="/FrontEnd/Modulos/TraficoAA/FacturasComerciales/Ges003-001-FacturasComerciales.aspx" OnTextChanged ="fbcPais_TextChanged" OnClick="fbcPais_Click"/>
                            <GWC:SwitchControl runat="server" ID="swcTipoOperacion" CssClass="col-xs-12 col-md-4 mb-5 p-0 d-flex justify-content-center" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" OnCheckedChanged="swcTipoOperacion_CheckedChanged" />
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4 mb-2 p-0">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-8 mt-2 mb-5" ID="icPesoTotal" Type="Text" Format="Real" Name="icPesoTotal" Label="Peso Total (Kg)" />
                            <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-2 mb-5 p-0 d-flex justify-content-center" ID="swcEnajenacion" Label="Enajenación" OnText="Sí" OffText="No" />
                            <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-2 mb-5 p-0 d-flex justify-content-center" ID="swcSubdivision" Label="Subdivisión" OnText="Sí" OffText="No" />
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-1 mb-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorFactura" Type="Text" Name="icValorFactura" Label="Valor factura" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scMonedaFactura" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                            </GWC:SelectControl>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mt-1 mb-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorMercancia" Type="Text" Name="icValorMercancia" Label="Valor mercancía" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scMonedaMercancia" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                            </GWC:SelectControl>
                        </asp:Panel>
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-1 mb-5" ID="fbcOrdenCompra" Label="Orden de compra" HasDetails="false" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-1 mb-5" ID="icSerieFolioFactura" Type="Text" Name="icSerieFolioFactura" Label="Serie/folio de la factura" />

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscProveedor" Label="Proveedor" Detail="Datos del proveedor">
                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                            <div class="col-xs-12 col-md-6">
                                <GWC:FindboxControl runat="server" CssClass="mb-5" ID="fbcProveedor" Label="Proveedor" HasDetails="true" Rules="required" RequiredSelect="true" OnClick="fbcProveedor_Click" OnTextChanged="fbcProveedor_TextChanged"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12 mt-5 mb-5 p-0" ID="scVinculacion" Label="Vinculación" OnClick="scVinculacion_Click">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12 mt-2 mb-5 p-0" ID="scMetodoValoracion" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11">
                                </GWC:SelectControl>
                            </div>
                            <div class="col-xs-12 col-md-6 mt-4">
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12 mt-2 mb-5 p-0" ID="scDomiciliosProveedor" Label="Domicilio fiscal" OnSelectedIndexChanged="scDomiciliosProveedor_SelectedIndexChanged">
                                </GWC:SelectControl>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mt-2 mb-5 p-0">
                                    <GWC:SwitchControl runat="server" ID="swcFungeCertificado" CssClass="col-xs-12 col-md-4 mt-2 mb-5 p-0 d-flex justify-content-center" Label="Funge como certificado" OnText="Sí" OffText="No" Checked="false" OnCheckedChanged="swcFungeCertificado_CheckedChanged"/>
                                    <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-8 mt-4 mb-5 p-0" ID="fbcProveedorCertificado" Label="Proveedor Certifica Origen" HasDetails="false" OnClick="fbcProveedor_Click" OnTextChanged="fbcProveedor_TextChanged"/>
                                </asp:Panel>
                            </div>
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscDestinatario" Label="Destinatario" Detail="Datos del destinatario" Visible="false">
                    <ListControls>

                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="fbcDestinatario" Label="Destinatario" HasDetails="true" RequiredSelect="true" OnClick="fbcProveedor_Click" OnTextChanged="fbcProveedor_TextChanged"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-4 mb-5 solid-textarea" ID="icDomicilioDestinario" Type="TextArea" Name="icDomicilioDestinario" Label="Domicilio fiscal" />

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPartidas" Label="Partidas">
                    <ListControls>

                        <GWC:PillboxControl runat="server" ID="pbPartidas" KeyField="indice" CssClass="col-xs-12" OnCheckedChange="pbPartidas_CheckedChange" OnClick="pbPartidas_Click">
                            <ListControls> 
                                
                                <asp:Panel runat="server" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                    <asp:Panel runat="server" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                        <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-1"></asp:Label>
                                        <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta col-xs-12 col-md-1" Text="0"></asp:Label>
                                    </asp:Panel>     
                                </asp:Panel>                           
                                <asp:Panel runat="server" CssClass="d-flex align-items-center">
                                    <asp:Label runat="server" ID="lbFactura" Text="Partida - Factura" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                    <div class="col-xs-12 col-md-6 mt-3 p-0">
                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" ID="fbcProducto" Label="Número de parte/alias" HasDetails="true" RequiredSelect="true" />
                                        <GWC:InputControl runat="server" ID="icDescripcionPartida" CssClass="col-xs-12 col-md-12 mt-5 mb-5 solid-textarea" Type="TextArea" Name="icDescripcionPartida" Label="Descripción" />
                                        <GWC:SwitchControl runat="server" ID="swcAplicaCOVE" CssClass="col-xs-12 col-md-12 mt-2 mb-5 d-flex justify-content-end" Label="Aplicar a COVE" OnText="Sí" OffText="No" OnCheckedChanged="swcAplicaCOVE_CheckedChanged"/>
                                        <GWC:InputControl runat="server" ID="icDescripcionCOVE" CssClass="col-xs-12 col-md-12 mt-3 mb-5 solid-textarea" Type="TextArea" Name="icDescripcionCOVE" Label="Descripción COVE" />
                                    </div>
                                    <div class="col-xs-12 col-md-6 mt-3 p-0">
                                        <GWC:InputControl runat="server" ID="icCantidadComercial" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Real" Name="icCantidadComercial" Label="Cantidad comercial" />
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scUnidadMedidaComercial" SearchBarEnabled="true" LocalSearch="false" Label="Unidad de medida comercial" OnClick="scUnidadMedidaComercial_Click" OnTextChanged="scUnidadMedidaComercial_TextChanged"/>
                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                            <GWC:InputControl runat="server" ID="icValorfacturaPartida" CssClass="col-xs-8 col-md-8 p-0 input-border-right" Type="Text" Name="icValorfacturaPartida" Label="Valor factura" />
                                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scMonedaFacturaPartida" SearchBarEnabled="true" LocalSearch="false" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                            </GWC:SelectControl>
                                        </asp:Panel>
                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                            <GWC:InputControl runat="server" ID="icValorMercanciaPartida" CssClass="col-xs-8 col-md-8 p-0 input-border-right" Type="Text" Name="icValorMercanciaPartida" Label="Valor mercancía" />
                                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scMonedaMercanciaPartida" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                            </GWC:SelectControl>
                                        </asp:Panel>
                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                                            <GWC:InputControl runat="server" ID="icPrecioUnitario" CssClass="col-xs-8 col-md-8 p-0 input-border-right" Type="Text" Name="icPrecioUnitario" Label="Precio unitario" />
                                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                            </GWC:SelectControl>
                                        </asp:Panel>
                                        <GWC:InputControl runat="server" ID="icPesoNeto" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Real" Name="icPesoNeto" Label="Peso Neto (Kg)" />
                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mt-1 mb-5" ID="fbcOrdenCompraPartida" Label="Orden de compra" HasDetails="false" />
                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mt-4 mb-5" ID="fbcPaisPartida" Label="País de origen" RequiredSelect="true" OnTextChanged="fbcPaisPartida_TextChanged" OnClick="fbcPaisPartida_Click"/>
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12 mt-4 mb-5" ID="scMetodoValoracionPartida" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11"/>
                                    </div>
                                </asp:Panel>                                               
                                <asp:Panel runat="server">
                                    <asp:Label runat="server" ID="lbClasificacion" Text="Partida - Clasificación" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                    <div class="col-xs-12 col-md-6 mt-3 p-0">
                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" ID="fbcFraccionArancelaria" Label="Fracción arancelaria" HasDetails="true" RequiredSelect="true" />
                                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" ID="fbcFraccionNico" Label="Nico" HasDetails="true" RequiredSelect="true" />
                                    </div>
                                    <div class="col-xs-12 col-md-6 mt-3 p-0">
                                        <GWC:InputControl runat="server" ID="icCantidadTarifa" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Real" Name="icCantidadTarifa" Label="Cantidad tarifa" />
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scUnidadMedidaTarifa" SearchBarEnabled="true" LocalSearch="false" Label="Unidad de medida tarifa" OnClick="scUnidadMedidaTarifa_Click" OnTextChanged="scUnidadMedidaTarifa_TextChanged">
                                        </GWC:SelectControl>
                                    </div>        
                                    <asp:Panel runat="server" class="col-xs-12 mt-3 p-0">
                                        <asp:Label runat="server" ID="lbMercancia" Text="Partida - Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                    </asp:Panel>     
                                    <div class="col-xs-12 mt-3 p-0">
                                        <GWC:InputControl runat="server" ID="icLote" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icLote" Label="Lote" />
                                        <GWC:InputControl runat="server" ID="icNumeroSerie" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icNumeroSerie" Label="Número de serie" />
                                        <GWC:InputControl runat="server" ID="icMarca" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icMarca" Label="Marca" />
                                        <GWC:InputControl runat="server" ID="icModelo" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icModelo" Label="Modelo" />
                                        <GWC:InputControl runat="server" ID="icSubmodelo" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icSubmodelo" Label="Submodelo" />
                                        <GWC:InputControl runat="server" ID="icKilometraje" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="icKilometraje" Label="Kilometraje" />
                                    </div> 
                                </asp:Panel>

                            </ListControls>
                        </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscIncrementables" Label="Incrementables">
                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-xs-12 mb-5">
                            <asp:Panel runat="server" CssClass="row fieldset justify-content-center" fieldset-legend="Valor incrementables">
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                                    <GWC:InputControl runat="server" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" ID="icFletes" Label="Fletes" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scMonedaFletes" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-3">
                                    <GWC:InputControl runat="server" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" ID="icSeguros" Label="Seguros" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scMonedaSeguros" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                                    <GWC:InputControl runat="server" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" ID="icEmbalajes" Label="Embalajes" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scMonedaEmbalajes" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-3">
                                    <GWC:InputControl runat="server" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" ID="icOtrosIncrementables" Label="Otros incrementables" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scMonedaOtrosIncrementables" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                                    <GWC:InputControl runat="server" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" ID="icDescuentos" Label="Descuentos" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scMonedaDescuentos" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnTextChanged="CargaMoneda_TextChanged" OnClick="SeleccionarMoneda_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscSubdivision" Label="Subdivisión" Visible="true">
                    <ListControls>

                        <GWC:CatalogControl ID="catSubdivision" runat="server" KeyField="Catalogo_Subdivision">
                            <Columns>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scNumeroParte" Label="Número de parte/alias" KeyField="" DisplayField="" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-12" ID="scDescripcion" Label="Descripción" KeyField="" DisplayField="" />
                                <GWC:SwitchControl runat="server" ID="swcParcialidades" CssClass="col-xs-12 col-md-12" Label="Parcialidades" OnText="Sí" OffText="No" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icParcialidades" Format="Numeric" Label="Dividir en" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCantidadOriginal" Format="Real" Label="Cantidad original" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icCantidadParcial" Format="Real" Label="Cantidad parcial" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12" Type="Text" ID="icReferencia" Label="Referencia" />
                            </Columns>
                        </GWC:CatalogControl>

                    </ListControls>
                </GWC:FieldsetControl>

            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
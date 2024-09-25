<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges003-001-FacturasComerciales.aspx.vb" Inherits=".Ges003_001_FacturasComerciales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar factura" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>

    <link rel="stylesheet" type="text/css" href="Estilos.css" />

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
   
    <div class="d-flex">

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="<span style='color:#321761'>Factura comercial</span><span style='color:#782360;'>&nbsp;importación</span>" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>
                        
                         <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIA"  style="padding-left: 20px" Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_IA col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 640 512'><path fill='#ffffff' d='M320 0c17.7 0 32 14.3 32 32V96H472c39.8 0 72 32.2 72 72V440c0 39.8-32.2 72-72 72H168c-39.8 0-72-32.2-72-72V168c0-39.8 32.2-72 72-72H288V32c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H208zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H304zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H400zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224H64V416H48c-26.5 0-48-21.5-48-48V272c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48v96c0 26.5-21.5 48-48 48H576V224h16z'/></svg>"></asp:Label>         
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManual" style="padding-left: 20px"  Visible="false">
                            <asp:Label runat="server" class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_blue  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManualNuevo" style="padding-left: 20px"  Visible="true">
                            <asp:Label runat="server"  class="cl_Num__Tarjeta_principal cl_Num__Tarjeta_gray  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                        </asp:Panel>

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-10 col-md-6 col-lg-5" ID="dbcNumFacturaCOVE" Label="Serie/Folio de factura" LabelDetail="Acuse de valor" OnClick="dbcNumFacturaCOVE_Click" Rules="required|maxlegth[120]|Unique"/>
                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-5 p-0">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="icFechaFactura" Rules="require" Type="Text" Format="Calendar" Name="icFechaFactura" Label="Fecha de factura" />
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="icFechaCOVE" Type="Text" Format="Calendar" Name="icFechaCOVE" Label="Fecha de acuse de valor" />
                         </asp:Panel>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-3" ID="icSerieFolioFactura" Type="Text" Name="icSerieFolioFactura" Label="Número de factura/Folio fiscal"/>
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-3" ID="fbcCliente" Label="Cliente" HasDetails="true" Rules="required" RequiredSelect="true" OnTextChanged="fbcCliente_TextChanged" OnClick="fbcCliente_Click" />
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-3" ID="fbcPais" Label="País" RequiredSelect="true" OnTextChanged="fbcPaisPartida_TextChanged" OnClick="fbcPaisPartida_Click"/>
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-8 col-lg-4 mt-3" ID="fbcIncoterm" Label="Incoterm" HasDetails="true" Rules="required"  RequiredSelect="true" KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" Dimension="Vt022TerminosFacturacionA14"/>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 mt-3 align-content-center" ID="swcEnajenacion" Label="Enajenación" OnText="Sí" OffText="No" />
                        <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 mt-3 align-content-center" ID="swcSubdivision" Label="Subdivisión" OnText="Sí" OffText="No" />
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3">
                            <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorFactura" Type="Text" Name="icValorFactura" Label="Valor factura" Format="Real"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaFactura" SearchBarEnabled="true" LocalSearch="True" Label="Moneda" OnClick="scMonedaFactura_Click">
                            </GWC:SelectControl>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3">
                          <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorMercancia" Type="Text" Name="icValorMercancia" Label="Valor mercancía" Format="Real"/>
                          <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaMercancia" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnClick="scMonedaMercancia_Click">
                          </GWC:SelectControl>
                        </asp:Panel>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3" ID="icPesoTotal" Type="Text" Format="Real" Name="icPesoTotal" Label="Peso total (Kg)" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-3" ID="icBultos" Type="Text" Label="Bultos" Format="Real"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 mb-5" ID="icOrdenCompra" Type="Text"  Label="Orden de compra" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3 mb-5" ID="icReferenciaCliente" Type="Text"  Label="Referencia cliente" />
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscProveedor" Label="Proveedor" Detail="Proveedor" CssClass="mt-5 p-0 mb-5">
                    <ListControls>
                        <GWC:FindboxControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12" ID="fbcProveedor" Label="Razón social" HasDetails="true" Rules="required" RequiredSelect="true" OnClick="fbcProveedor_Click" OnTextChanged="fbcProveedor_TextChanged"/>
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12" ID="scDomiciliosProveedor" Label="Domicilio fiscal" OnTextChanged="scDomiciliosProveedor_OnTextChanged" OnSelectedIndexChanged="scDomiciliosProveedor_SelectedIndexChanged" OnClick="scDomiciliosProveedor_Click" SearchBarEnabled="true" LocalSearch="true"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 mt-1" ID="scVinculacion" Label="Vinculación" OnClick="scVinculacion_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-lg-6 col-md-6 col-xs-12 mt-1" ID="scMetodoValoracion" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11">
                        </GWC:SelectControl>
                        <GWC:SwitchControl runat="server" ID="swcFungeCertificado" CssClass="col-lg-1 col-md-2 col-xs-3 mt-3 mb-5 justify-content-center" Label="Funge certificado" OnText="Sí" OffText="No" OnCheckedChanged="swcFungeCertificado_CheckedChanged"/>
                        <GWC:FindboxControl runat="server" CssClass="col-lg-5 col-md-10 col-xs-9 mt-3 mb-5" ID="fbcProveedorCertificado"  Label="Proveedor certifica origen" HasDetails="false"  OnTextChanged="fbcProveedorCertificado_TextChanged"/>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscPartidas" Label="Items" CssClass="mt-5">
                    <ListControls>
                        <GWC:PillboxControl runat="server" ID="pbPartidas" KeyField="indice" CssClass="col-xs-12"  OnClick="pbPartidas_Click">
                            <ListControls> 
                                <asp:Panel runat="server" CssClass="col-md-1 col-lg-1 col-xs-2 d-flex align-items-center flex-column margin-bottom">
                                    <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta" Text="0"></asp:Label>
                                </asp:Panel>
                               <GWC:FindboxControl runat="server" CssClass="col-xs-10 col-md-5 mt-5" ID="fbcProducto" Label="Número de parte/alias" HasDetails="true" RequiredSelect="true" OnTextChanged="fbcProducto_TextChanged" OnClick="fbcProducto_Click" />
                               <GWC:InputControl runat="server" ID="icCantidadComercial" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" Type="Text" Format="Real" Name="icCantidadComercial" Label="Cantidad comercial" />
                               <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="scUnidadMedidaComercial" SearchBarEnabled="true" LocalSearch="true" Label="Unidad de medida comercial" OnClick="scUnidadMedidaComercial_Click" OnTextChanged="scUnidadMedidaComercial_TextChanged"/>
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                     <GWC:InputControl runat="server" ID="icValorfacturaPartida" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text" Name="icValorfacturaPartida" Label="Valor factura" Format="Real"/>
                                     <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaFacturaPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFacturaPartida_Click">
                                     </GWC:SelectControl>
                               </asp:Panel>
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                   <GWC:InputControl runat="server" ID="icValorMercanciaPartida" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text" Name="icValorMercanciaPartida" Label="Valor mercancía" Format="Real" />
                                   <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaMercanciaPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaMercanciaPartida_Click">
                                   </GWC:SelectControl>
                               </asp:Panel>

                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4">
                                   <GWC:InputControl runat="server" ID="icPrecioUnitario" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text" Name="icPrecioUnitario" Label="Precio unitario" Format="Real"/>
                                   <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="True" Label="Moneda" OnClick="scMonedaPrecioUnitarioPartida_Click">
                                   </GWC:SelectControl>
                               </asp:Panel>

                              <GWC:InputControl runat="server" ID="icPesoNeto" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-4" Type="Text" Format="Real" Name="icPesoNeto" Label="Peso Neto (Kg)" Rules="real"/>
                              <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-3 mb-5" ID="fbcPaisPartida" Label="País de origen" RequiredSelect="true" OnTextChanged="fbcPaisPartida_TextChanged" OnClick="fbcPaisPartida_Click"/>
                              <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="scMetodoValoracionPartida" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11"/>
                              <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="fbcOrdenCompraPartida" Label="Orden de compra" HasDetails="false" />

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4">
                                <GWC:InputControl runat="server" ID="icDescripcionPartida" CssClass="solid-textarea mt-5" Type="TextArea" Label="Descripción mercancía" />
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 d-flex align-content-end justify-content-end">
                                    <GWC:SwitchControl runat="server" ID="swcAplicaCOVE"  CssClass="pt-3" Label="Aplica acuse valor" OnText="Sí" OffText="No"  OnCheckedChanged="swcAplicaCOVE_CheckedChanged"/>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4">
                            <GWC:InputControl runat="server" ID="icDescripcionCOVE" CssClass="mt-5 solid-textarea" Type="TextArea" Label="Descripción acuse de valor" />
                            </asp:Panel>
                                 
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-4">
                                <asp:Label runat="server" ID="lbClasificacion" Text="Clasificación" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="icFraccionArancelaria" Label="Fracción arancelaria" Name="icFraccionArancelaria" Enabled="false"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="icFraccionNico" Label="Nico" Name="icFraccionNico" Enabled="false" ToolTip=""/>
                                <GWC:InputControl runat="server" ID="icCantidadTarifa" CssClass="col-xs-12 col-md-4 col-lg-3 mb-5 mt-4" Type="Text" Format="Real" Name="icCantidadTarifa" Label="Cantidad tarifa" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="scUnidadMedidaTarifa" SearchBarEnabled="true" LocalSearch="True" Label="Unidad de medida tarifa" OnClick="scUnidadMedidaTarifa_Click" OnTextChanged="scUnidadMedidaTarifa_TextChanged">
                                </GWC:SelectControl>
                            </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-4">
                                <asp:Label runat="server" ID="lbMercancia" Text="Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                <GWC:InputControl runat="server" ID="icLote" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icLote" Label="Lote" />
                                <GWC:InputControl runat="server" ID="icNumeroSerie" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icNumeroSerie" Label="Número de serie" />
                                <GWC:InputControl runat="server" ID="icMarca" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icMarca" Label="Marca"/>
                                <GWC:InputControl runat="server" ID="icModelo" CssClass="col-xs-12 col-md-3 mb-4 mt-4" Type="Text" Name="icModelo" Label="Modelo" />
                                <GWC:InputControl runat="server" ID="icSubmodelo" CssClass="col-xs-12 col-md-3" Type="Text" Name="icSubmodelo" Label="Submodelo" />
                                <GWC:InputControl runat="server" ID="icKilometraje" CssClass="col-xs-12 col-md-3" Type="Text" Name="icKilometraje" Label="Kilometraje" Format="Real" />
                            </asp:Panel>
                </ListControls>
            </GWC:PillboxControl>
        </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscIncrementables" Label="Incrementables" CssClass="mt-5 mb-5">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-7 p-0 input-border-right" Type="Text" ID="icFletes" Label="Fletes" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-5 p-0 pl-1" ID="scMonedaFletes" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnClick="scMonedaFletes_Click">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icSeguros" Label="Seguros" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaSeguros" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnClick="scMonedaSeguros_Click">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icEmbalajes" Label="Embalajes" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaEmbalajes" SearchBarEnabled="true" LocalSearch="false" Label="Moneda"  OnClick="scMonedaEmbalajes_Click">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-3">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-7 p-0 input-border-right" Type="Text" ID="icOtrosIncrementables" Label="Otros incrementables" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-5 p-0 pl-1" ID="scMonedaOtrosIncrementables" SearchBarEnabled="true" LocalSearch="false" Label="Moneda" OnClick="scMonedaOtrosIncrementables_Click">
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-2">
                            <GWC:InputControl runat="server" CssClass="col-xs-6 col-md-6 p-0 input-border-right" Type="Text" ID="icDescuentos" Label="Descuentos" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-6 col-md-6 p-0 pl-1" ID="scMonedaDescuentos" SearchBarEnabled="true" LocalSearch="true" Label="Moneda"  OnClick="scMonedaDescuentos_Click">
                            </GWC:SelectControl>
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
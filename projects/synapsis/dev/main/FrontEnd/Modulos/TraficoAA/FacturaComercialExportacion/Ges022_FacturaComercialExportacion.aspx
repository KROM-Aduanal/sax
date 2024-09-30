<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_FacturaComercialExportacion.aspx.vb" Inherits=".Ges022_FacturaComercialExportacion" %>

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

    <link rel="stylesheet" type="text/css" href="Estilos.css" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="<span style='color:#321761'>Factura comercial</span><span style='color:#782360;'>&nbsp;exportación</span>">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Captura manual" />
                </DropdownButtons>
            </Buttonbar>
            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>
                            <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaIA"  style="padding-left: 20px" Visible="false">
                                    <asp:Label runat="server" class="cl_Num__Tarjeta cl_Num__Tarjeta_IA col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 640 512'><path fill='#ffffff' d='M320 0c17.7 0 32 14.3 32 32V96H472c39.8 0 72 32.2 72 72V440c0 39.8-32.2 72-72 72H168c-39.8 0-72-32.2-72-72V168c0-39.8 32.2-72 72-72H288V32c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H208zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H304zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16h32c8.8 0 16-7.2 16-16s-7.2-16-16-16H400zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224H64V416H48c-26.5 0-48-21.5-48-48V272c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48v96c0 26.5-21.5 48-48 48H576V224h16z'/></svg>"></asp:Label>         
                           </asp:Panel>
                           <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManual" style="padding-left: 20px"  Visible="false">
                                <asp:Label runat="server"  class="cl_Num__Tarjeta cl_Num__Tarjeta_blue  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                            </asp:Panel>

                           <asp:Panel runat="server" CssClass="col-xs-2 col-md-6 col-lg-1 align-content-center" ID="lbModoCapturaManualNuevo" style="padding-left: 20px"  Visible="True">
                                <asp:Label runat="server"  class="cl_Num__Tarjeta cl_Num__Tarjeta_gray  col-md-1 align-content-center" Text="<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 512 512'><path fill='#ffffff' d='M224 0c17.7 0 32 14.3 32 32V240H192V32c0-17.7 14.3-32 32-32zm96 160c17.7 0 32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V192c0-17.7 14.3-32 32-32zm64 64c0-17.7 14.3-32 32-32s32 14.3 32 32v64c0 17.7-14.3 32-32 32s-32-14.3-32-32V224zM93.3 51.2L175.9 240H106.1L34.7 76.8C27.6 60.6 35 41.8 51.2 34.7s35.1 .3 42.1 16.5zm27 221.3l-.2-.5h69.9H216c22.1 0 40 17.9 40 40s-17.9 40-40 40H160c-8.8 0-16 7.2-16 16s7.2 16 16 16h56c39.8 0 72-32.2 72-72l0-.6c9.4 5.4 20.3 8.6 32 8.6c13.2 0 25.4-4 35.6-10.8c8.7 24.9 32.5 42.8 60.4 42.8c11.7 0 22.6-3.1 32-8.6V352c0 88.4-71.6 160-160 160H226.3c-42.4 0-83.1-16.9-113.1-46.9l-11.6-11.6C77.5 429.5 64 396.9 64 363V336c0-32.7 24.6-59.7 56.3-63.5z'/></svg>"></asp:Label>
                            </asp:Panel>

                            <GWC:DualityBarControl runat="server" CssClass="col-xs-10 col-md-6 col-lg-5" ID="dbcNumFacturaAcuseValor" Label="Serie / Folio de factura" LabelDetail="Acuse de valor" Rules="maxlegth[40]"/>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-5 p-0">
                                 <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="icFechaFactura"  Type="Text" Format="Calendar" Label="Fecha factura" />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5 icFechaAcuseValor" ID="icFechaAcuseValor" Type="Text"  Format="Calendar" Label="Fecha acuse valor"/>
                            </asp:Panel>
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="icNumeroFacturaUUID" Type="Text"  Label="Número de factura / Folio fiscal (UUID)" Rules="required|Unique"/>
                            
                            <%--  CLIENTE--%>
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-5" ID="fbcCliente" Label="Cliente"  OnTextChanged="fbcCliente_TextChanged" OnClick="fbcCliente_Click"/>
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-4" ID="fbcPais" Label="País"  OnTextChanged="fbcPaisPartida_TextChanged" OnClick="fbcPaisPartida_Click"/>
                            <GWC:FindboxControl runat="server" CssClass="col-xs-9 col-md-5 col-lg-5 mt-4" ID="fbcIncoterm" Label="Incoterm" Rules="required" KeyField="i_Cve_TerminoFacturacion" DisplayField="t_ValorPresentacion" Dimension="Vt022TerminosFacturacionA14"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-1 col-lg-1 mt-5" ID="swcEnajenacion" Label="Enajenación" OnText="Sí" OffText="No" />
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5">
                               <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorFactura" Type="Text"  Label="Valor factura" Format="Real"/>
                               <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaFactura" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click">
                                </GWC:SelectControl>
                             </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5">
                                <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icValorMercancia" Type="Text" Label="Valor mercancía" Format="Real"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0 pl-1" ID="scMonedaMercancia" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click"></GWC:SelectControl>
                         </asp:Panel>
                          <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5" ID="icPesoTotal" Type="Text"  Label="Peso total (Kg)" Format="Real"/>
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5" ID="icBultos" Type="Text" Label="Bultos" Format="Real"/>
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5" ID="icOrdenCompra" Type="Text"  Label="Orden de compra" Rules="maxlegth[40]"/>
                         <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5 mb-5" ID="icReferenciaCliente" Type="Text"  Label="Referencia cliente" Rules="maxlegth[40]"/>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscCompradorReceptor" CssClass="mt-5 mb-5" Label="Comprador - receptor" Detail="Comprador - receptor">
                   <ListControls>
                         <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-5" ID="fbcCompradorReceptor" Label="Razón social" OnClick="fbcCompradorReceptor_Click" OnTextChanged="fbcCompradorReceptor_TextChanged" OnSelectedIndexChanged="fbcCompradorReceptor_SelectedIndexChanged"/>
                         <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mt-5" ID="scDomicilioCompradorReceptor" Label="Domicilio" OnClick="scDomicilioCompradorReceptor_Click" OnSelectedIndexChanged="scDomicilioCompradorReceptor_SelectedIndexChanged" OnTextChanged="scDomicilioCompradorReceptor_TextChanged"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mt-5 mb-5" ID="scVinculacion" Label="Vinculación" OnClick="scVinculacion_Click"></GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-8 col-md-5 col-lg-5 mt-5 mb-5" ID="scMetodoValoracion" Label="Método de valoración" Onclick="scMetodoValoracion_Click" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11" ></GWC:SelectControl>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-4 col-md-1 col-lg-1 d-flex justify-content-center px-5 mt-5 mb-5" ID="swcEsDestinatario" Label="Destinatario" OnText="Sí" OffText="No" OnCheckedChanged="swcEsDestinatario_CheckedChanged"/>
                   </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" ID="fscCompradorDestinatario" Label="Comprador - destinatario" CssClass="mt-5 mb-5" Detail="Comprador - destinatario">
                         <ListControls>
                                 <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-5 mb-5" ID="fbcCompradorDestinatario" Label="Razón social" OnClick="fbcCompradorDestinatario_Click" OnTextChanged="fbcCompradorDestinatario_TextChanged" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mt-5 mb-5" ID="scDomicilioCompradorDestinatario"  Label="Domicilio" OnClick="scDomicilioCompradorDestinatario_Click" OnTextChanged="scDomicilioCompradorDestinatario_TextChanged"></GWC:SelectControl>
                       </ListControls>
                </GWC:FieldsetControl>
                 <%--         ITEMS--%>
                <GWC:FieldsetControl runat="server" ID="fscItems" Label="Items" CssClass="mt-5">
                    <ListControls>
                        <GWC:PillboxControl runat="server" ID="pbPartidasItems"  KeyField="indice" CssClass="col-lg-12 col-md-12 col-xs-12 mt-5" OnClick="pbPartidasItems_Click">
                            <ListControls> 
                                <asp:Panel runat="server" CssClass="col-md-1 col-lg-1 col-xs-2 d-flex align-items-center flex-column margin-bottom">
                                    <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta_partida" Text="0"></asp:Label>
                                </asp:Panel>
                                <GWC:FindboxControl runat="server" CssClass="col-xs-10 col-md-5 mt-5" ID="fbcProducto" Label="Número de parte/alias"  OnClick="fbcProducto_Click" OnTextChanged="fbcProducto_TextChanged" HasDetails="true" RequiredSelect="true"/>
                                <GWC:InputControl runat="server" ID="icCantidadComercial" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" Type="Text"  Label="Cantidad comercial" Format="Real"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mt-5" ID="scUnidadMedidaComercial" SearchBarEnabled="true" LocalSearch="false" Label="Unidad de medida comercial" OnClick="scUnidadMedidaComercial_Click" OnTextChanged="scUnidadMedidaComercial_TextChanged"/>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                    <GWC:InputControl runat="server" ID="icValorDolaresPartida"  CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text"  Label="Valor dólares" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaValorDolaresPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click"></GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                    <GWC:InputControl runat="server" ID="icValorUnitarioPartida" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text"  Label="Valor unitario aduana" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaValorUnitarioPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click"> </GWC:SelectControl>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3">
                                    <GWC:InputControl runat="server" ID="icPrecioUnitario" CssClass="col-xs-8 col-md-8 col-lg-8 p-0 input-border-right" Type="Text" Label="Precio unitario" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 col-lg-4 p-0 pl-1" ID="scMonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="true" Label="Moneda" OnClick="scMonedaFactura_Click"/>
                                </asp:Panel>
                                <GWC:InputControl runat="server" ID="icPesoNeto" CssClass="col-xs-12 col-md-6 col-lg-3 mb-3 mt-3" Type="Text" Label="Peso Neto (Kg)" Format="Real"/>
                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-12 col-lg-6 mt-3 mb-5" ID="fbcPaisPartida" Label="País destino"  RequiredSelect="true" OnTextChanged="fbcPaisDestino_TextChanged" OnClick="fbcPaisDestino_Click"/>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="scMetodoValoracionPartida" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11"  />
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-3 mt-3 mb-5" ID="icOrdenCompraPartida" Label="Orden de compra" Rules="maxlegth[40]"/>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4">
                                    <GWC:InputControl runat="server" ID="icDescripcionPartida" CssClass="solid-textarea mt-5" Type="TextArea" Label="Descripción mercancía" />
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 d-flex align-content-end justify-content-end">
                                        <GWC:SwitchControl runat="server" ID="swcAplicaCOVE"  CssClass="pt-3" Label="Aplica acuse valor" OnText="Sí" OffText="No"  OnCheckedChanged="swcAplicaCOVE_CheckedChanged"/>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mt-4">
                                  <GWC:InputControl runat="server" ID="icDescripcionCOVE" CssClass="mt-5 solid-textarea" Type="TextArea" Label="Descripción acuse de valor" />
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">
                                    <asp:Label runat="server" ID="lbClasificacion" Text="Clasificación" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="icFraccionArancelaria" Label="Fracción arancelaria" Enabled="false"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="icFraccionNico"  Label="Nico" Enabled="false"/>
                                    <GWC:InputControl runat="server" ID="icCantidadTarifa" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" Type="Text"  Label="Cantidad tarifa" Format="Real"/>
                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 col-lg-3 mb-5 mt-4" ID="scUnidadMedidaTarifa"  SearchBarEnabled="true" LocalSearch="True" Label="Unidad de medida tarifa" OnClick="scUnidadMedidaTarifa_Click" OnTextChanged="scUnidadMedidaTarifa_TextChanged"/>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-5">
                                    <asp:Label runat="server" ID="lbMercancia" Text="Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                    <GWC:InputControl runat="server" ID="icLote" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4" Type="Text"  Label="Lote" Rules="maxlegth[40]"/>
                                    <GWC:InputControl runat="server" ID="icNumeroSerie" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4" Type="Text"  Label="Número de serie" Rules="maxlegth[40]"/>
                                    <GWC:InputControl runat="server" ID="icMarca" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4" Type="Text"  Label="Marca" Rules="maxlegth[40]"/>
                                    <GWC:InputControl runat="server" ID="icModelo" CssClass="col-xs-12 col-md-3 col-lg-3 mt-4" Type="Text" Label="Modelo" Rules="maxlegth[40]"/>
                                    <GWC:InputControl runat="server" ID="icSubmodelo" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3" Type="Text"  Label="Submodelo" Rules="maxlegth[40]"/>
                                    <GWC:InputControl runat="server" ID="icKilometraje" CssClass="col-xs-12 col-md-3 col-lg-3 mt-3" Type="Text" Label="Kilometraje" Format="Real" Rules="maxlegth[40]"/>
                                </asp:Panel>

                     <%--           <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5">
                                        <asp:Label runat="server" ID="Label1" Text="Regla" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                    </asp:Panel>--%>

               <%--     <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                                               <div class="row justify-content-center">
                                                 <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center" Text="A22"></asp:Label>
                                                 <asp:Label runat="server" class="icon_cube_purple_active   d-flex col-md-1 align-content-center" Text="f(x)"></asp:Label>
                                                <GWC:FindboxControl runat="server" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" ID="FindboxControl1" Label="Buscar habitación" HasDetails="true" RequiredSelect="true" />
                                                <asp:Label runat='server' class='icon_cube_active  d-flex col-md-1 mt-4 align-content-center' Text='<svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 812"><path fill="#ffffff" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>'></asp:Label>
                                                <asp:Label runat='server' class='icon_cube_green_active  d-flex col-md-1 mt-4 align-content-center' Text='<svg xmlns="http://www.w3.org/2000/svg" height="24" width="28" viewBox="0 0 448 512"><path fill="#ffffff" d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z"/></svg>'></asp:Label>
                                                <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0  ml-2 switch-cube" ID="SwitchControl2"  OnText="Online" OffText="Offline" />
                                            </div>
                               </asp:Panel>--%>

                 <%--         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                                          <div class="row justify-content-center">
                                            <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center" Text="A22"></asp:Label>
                                            <asp:Label runat="server" class="icon_cube_rose_active   d-flex col-md-1 align-content-center" Text="(x)"></asp:Label>
                                            <GWC:InputControl runat="server" ID="InputControl1" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" Type="Text" Name="ic" Label="Ingrese habitación" />
                                           <asp:Label runat='server' class='icon_cube_active  d-flex col-md-1 mt-4 align-content-center' Text='<svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 812"><path fill="#ffffff" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>'></asp:Label>
                                           <asp:Label runat='server' class='icon_cube_green_active  d-flex col-md-1 mt-4 align-content-center' Text='<svg xmlns="http://www.w3.org/2000/svg" height="24" width="28" viewBox="0 0 448 512"><path fill="#ffffff" d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z"/></svg>'></asp:Label>
                                           <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0  ml-2 switch-cube" ID="SwitchControl1"  OnText="Online" OffText="Offline" />
                                       </div>
                          </asp:Panel>--%>

                 <%--     <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                                               <div class="row justify-content-center">
                                                 <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center mt-4" Text="A22"></asp:Label>
                                                 <asp:Label runat="server" class="icon_cube_purple_active   d-flex col-md-1 align-content-center mt-4" Text="f(x)"></asp:Label>
                                                <GWC:FindboxControl runat="server" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" ID="FindboxControl2" Label="Buscar habitación" HasDetails="true" RequiredSelect="true" />
                                                 <asp:Label runat='server' class='icon_cube  d-flex col-md-1 align-content-center mt-4' Text='<svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 812"><path fill="#ffffff" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>'></asp:Label>
                                                  <asp:Label runat='server' class='icon_cube_green  d-flex col-md-1 align-content-center mt-4' Text='   <svg xmlns="http://www.w3.org/2000/svg" height="24" width="28" viewBox="0 0 448 512"><path fill="#ffffff" d="M438.6 105.4c12.5 12.5 12.5 32.8 0 45.3l-256 256c-12.5 12.5-32.8 12.5-45.3 0l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0L160 338.7 393.4 105.4c12.5-12.5 32.8-12.5 45.3 0z"/></svg>'></asp:Label>
                                                <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0  switch-cube" ID="SwitchControl3"  OnText="Online" OffText="Offline"  active="true" />
                                            </div>
                               </asp:Panel>--%>


   <%--              <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                                            <div class="row justify-content-center">
                                              <asp:Label runat="server" class="icon_cube_purple d-flex col-md-1 align-content-center mt-4" Text="A22"></asp:Label>
                                              <asp:Label runat="server" class="icon_cube_purple_active  d-flex col-md-1 align-content-center mt-4" Text="f(x)"></asp:Label>
                                             <GWC:FindboxControl runat="server" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" ID="FindboxControl3" Label="Buscar habitación" HasDetails="true" RequiredSelect="true" />
                                                 <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" style="margin-top:10px"><path fill="#424242" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>
                                                <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 512 512" style="margin-top:10px"><path fill="#009688" d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>
                                             <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0  ml-2 switch-cube" ID="SwitchControl4"  OnText="Online" OffText="Offline" active="true" />
                                         </div>
                            </asp:Panel>--%>


      <%--          <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                                    <div class="row justify-content-center">
                                      <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center mt-4" Text="A22"></asp:Label>
                                      <asp:Label runat="server" class="icon_cube_purple_active   d-flex col-md-1 align-content-center mt-4" Text="f(x)"></asp:Label>
                                     <GWC:FindboxControl runat="server" CssClass="col-xs-5 col-md-8 col-lg-9 mt-4" ID="FindboxControl4" Label="Buscar habitación" HasDetails="true" RequiredSelect="true" />
                                         <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" style="margin-top:10px"><path fill="#c6c6c6" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>
                                        <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 512 512" style="margin-top:10px"><path fill="#c6c6c6" d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>
                                     <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0 ml-2  switch-cube" ID="SwitchControl5"  OnText="Online" OffText="Offline" />
                                 </div>
                    </asp:Panel>--%>

<%--           <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                       <div class="row justify-content-center">
                         <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center mt-4" Text="A22"></asp:Label>
                        <asp:Label runat="server" class="icon_cube_rose_active   d-flex col-md-1 align-content-center mt-4" Text="(x)"></asp:Label>
                           <GWC:InputControl runat="server" ID="InputControl2" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" Type="Text" Name="ic" Label="Ingrese habitación" />
                            <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" style="margin-top:10px"><path fill="#c6c6c6" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>
                           <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 512 512" style="margin-top:10px"><path fill="#c6c6c6" d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>
                        <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0 ml-2  switch-cube" ID="SwitchControl6"  OnText="Online" OffText="Offline" />
                    </div>
       </asp:Panel>--%>

    <%--  <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mb-5 mt-4">                          
                 <div class="row justify-content-center">
                   <asp:Label runat="server" class="icon_cube_purple   d-flex col-md-1 align-content-center mt-4" Text="A22"></asp:Label>
                  <asp:Label runat="server" class="icon_cube_rose_active   d-flex col-md-1 align-content-center mt-4" Text="(x)"></asp:Label>
                     <GWC:InputControl runat="server" ID="InputControl3" CssClass="col-xs-5 col-md-6 col-lg-9 mt-4" Type="Text" Name="ic" Label="Ingrese habitación" />
                      <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 448 512" style="margin-top:10px"><path fill="#424242" d="M144 144v48H304V144c0-44.2-35.8-80-80-80s-80 35.8-80 80zM80 192V144C80 64.5 144.5 0 224 0s144 64.5 144 144v48h16c35.3 0 64 28.7 64 64V448c0 35.3-28.7 64-64 64H64c-35.3 0-64-28.7-64-64V256c0-35.3 28.7-64 64-64H80z"/></svg>
                     <svg xmlns="http://www.w3.org/2000/svg" height="30" width="30" viewBox="0 0 512 512" style="margin-top:10px"><path fill="#009688" d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>
                  <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-md-2 col-lg-1 p-0 ml-2  switch-cube" ID="SwitchControl7"  OnText="Online" OffText="Offline" />
              </div>
 </asp:Panel>--%>


                            </ListControls>
                        </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
</asp:Content>

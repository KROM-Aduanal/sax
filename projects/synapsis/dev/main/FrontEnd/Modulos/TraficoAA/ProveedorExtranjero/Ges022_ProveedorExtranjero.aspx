<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_ProveedorExtranjero.aspx.vb" Inherits=".Ges022_ProveedorExtranjero" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Proveedor" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>
   
     <link rel="stylesheet" type="text/css" href="estilos.css" />

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

<GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false"  Label="<span style='color:#321761'>Proveedores</span><span style='color:#782360;'>&nbsp;extranjeros</span>" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Domicilios" />
                    <GWC:ButtonItem Text="Limpiar domicilio" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos Generales" Priority="true" CssClass="">
                    <ListControls> 

                    <GWC:CardControl runat="server" ID="aviso" CssClass="container wc-card-danger mb-5" Visible="False"  >
                         <ListControls>
                             <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel2" >
                                        <asp:Label runat="server" ID="lbTitleAviso" Text="<svg xmlns='http://www.w3.org/2000/svg'  height='24' width='24' viewBox='0 0 512 512'><path d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm177.6 62.1C192.8 334.5 218.8 352 256 352s63.2-17.5 78.4-33.9c9-9.7 24.2-10.4 33.9-1.4s10.4 24.2 1.4 33.9c-22 23.8-60 49.4-113.6 49.4s-91.7-25.5-113.6-49.4c-9-9.7-8.4-24.9 1.4-33.9s24.9-8.4 33.9 1.4zM144.4 208a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm192-32a32 32 0 1 1 0 64 32 32 0 1 1 0-64z'/></svg> &nbsp; Este &nbsp;<span style='font-weight:bold !important'>proveedor</span>&nbsp; ya ha sido &nbsp;<span style='font-weight:bold !important; color:#432776 !important'> registrado </span>.&nbsp;" Visible="True" CssClass="mb-5 title_Card" style="font-weight: normal !important; font-size:18px !important; color:#5b5b5b !important" ></asp:Label>
                              </asp:Panel>
                         </ListControls>
                     </GWC:CardControl>
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-12 col-md-1 col-lg-1 mt-3" Type="Text" ID="icClave" Rules="onlynumber" Label="Clave" />
                            <GWC:InputControl runat="server" Type="Hide" ID="cveEmpresa" Label="CveRazonsocial" />
                            <GWC:InputControl runat="server" Type="Hide" ID="idEmpresa" Label="idEmpresa" />
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-5 mt-3" ID="fcRazonSocial" KeyField="_id" DisplayField="razonsocial" Label="Razón social" Rules="required|maxlegth[120]" OnClick="fcRazonSocial_Click" OnTextChanged="fcRazonSocial_TextChanged"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-2 d-flex justify-content-center  mt-3" ID="swcTipoUso" Label="" OnText="Importación" OffText="Exportación" Checked="True"/>
                    </ListControls>
                </GWC:FieldsetControl>
    
                <GWC:FieldsetControl runat="server" ID="fsDetalles" Label="Domicilios" Detail="Domicilios" CssClass="m-0 p-0"> 
                    <ListControls>
                          <GWC:CardControl runat="server" ID="ConfigurarDomicilios" CssClass="container p-0 mb-5" Visible="False">
                              <ListControls> 
                                   <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel1" >
                                        <asp:Label runat="server" ID="lbTitle" Text="<svg xmlns='http://www.w3.org/2000/svg'  height='24' width='24' viewBox='0 0 512 512'><path d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm177.6 62.1C192.8 334.5 218.8 352 256 352s63.2-17.5 78.4-33.9c9-9.7 24.2-10.4 33.9-1.4s10.4 24.2 1.4 33.9c-22 23.8-60 49.4-113.6 49.4s-91.7-25.5-113.6-49.4c-9-9.7-8.4-24.9 1.4-33.9s24.9-8.4 33.9 1.4zM144.4 208a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm192-32a32 32 0 1 1 0 64 32 32 0 1 1 0-64z'/></svg>&nbsp; Seleccione un &nbsp;<span style='font-weight:bold !important'> país</span>&nbsp;" Visible="True" CssClass="mb-3 title_Card" style="font-weight: normal !important; font-size:18px !important; color:#5b5b5b !important" ></asp:Label>
                                         <GWC:FindboxControl runat="server" CssClass="col-xs-11 col-11 col-md-11 col-lg-11 mt-3 mb-5" ID="fcPaises" KeyField="_id" DisplayField="País" Label="Buscar" OnTextChanged="fcPaises_TextChanged" OnClick="fcPaises_Click"/>
                                           <asp:Label runat="server" ID="lbtitleDomicilios" Text="<svg xmlns='http://www.w3.org/2000/svg' height='24' width='24' viewBox='0 0 512 512'><path d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm177.6 62.1C192.8 334.5 218.8 352 256 352s63.2-17.5 78.4-33.9c9-9.7 24.2-10.4 33.9-1.4s10.4 24.2 1.4 33.9c-22 23.8-60 49.4-113.6 49.4s-91.7-25.5-113.6-49.4c-9-9.7-8.4-24.9 1.4-33.9s24.9-8.4 33.9 1.4zM144.4 208a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm165.8 21.7c-7.6 8.1-20.2 8.5-28.3 .9s-8.5-20.2-.9-28.3c14.5-15.5 35.2-22.3 54.6-22.3s40.1 6.8 54.6 22.3c7.6 8.1 7.1 20.7-.9 28.3s-20.7 7.1-28.3-.9c-5.5-5.8-14.8-9.7-25.4-9.7s-19.9 3.8-25.4 9.7z'/></svg> &nbsp; Seleccione un &nbsp;<span style='font-weight:bold !important'>domicilio</span>" CssClass="w-100 mb-5 title_Card mt-5" style="font-weight: normal !important; font-size:18px !important;" Visible="false" ></asp:Label>
                                           <GWC:SelectControl runat="server" CssClass="col-xs-11 col-11 col-md-11 col-lg-11 mt-5 mb-5" ID="scDomiciliosRegistrados" LocalSearch="False" Label="Disponibles" Visible="false" OnClick="scDomiciliosRegistrados_Click" OnTextChanged="scDomiciliosRegistrados_TextChanged"></GWC:SelectControl>
                                            <GWC:ButtonControl runat="server" CssClass="col-xs-1 col-1 col-md-1 col-lg-1  mt-3 mb-5 d-flex justify-content-end" ID="btnTipoDomicilio" Label="Aplicar" OnClick="btnTipoDomicilio_Click"/>
                                   </asp:Panel>
                              </ListControls>
                        </GWC:CardControl>

                         <GWC:PillboxControl runat="server" ID="pbDetalleProveedorInternacional" KeyField="indice" CssClass="col-xs-12 m-0 p-0" OnCheckedChange="pbDetalleProveedor_CheckedChange" OnClick="pbDetalleProveedor_Click">
                             
                             <ListControls>
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12">
                                         <asp:Label runat="server" ID="lbMercancia" Text="Domicilio" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-12 col-xs-12 col-md-12 col-lg-1 d-flex p-0" > 
                                        <div class="row">
                                            <div class="col-lg-12 ml-5">
                                                <div class="d-flex row align-items-center flex-column p-0 ml-5 mb-5">
                                                    <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta d-block col-lg-4 col-md-4 col-4" Text="0"></asp:Label>
                                                </div>
                                            </div>
                                         </div>
                                    </asp:Panel>
                                   
                                    <GWC:InputControl runat="server" CssClass="col-xs-9 col-9 col-md-10 col-lg-5" Type="Text" ID="icTaxid" Label="Taxid" Rules="required"/>
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-3 col-3 col-md-2 col-lg-1" ID="swcDestinatario" Label="Destinatario" OnText="Si" OffText="No"/>
                                    <asp:Panel runat="server" CssClass="mt-lg-5 p-0" ID="sectionDomicilio" >
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mb-3" Type="Text" ID="icCalle" Label="Calle" Rules="required"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icNumeroExterior" Label="Número exterior" Rules="required"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icNumeroInterior" Label="Número interior"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icCodigoPostal" Label="Código postal" Format="Numeric" Rules="maxlegth[10]|onlynumber"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icColonia" Label="Colonia"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icLocalidad" Label="Localidad"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icCiudad" Label="Ciudad"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-3" Type="Text" ID="icMunicipio" Label="Municipio" OnTextChanged="icMunicipio_TextChanged"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-3" Type="Text" ID="icEntidadFederativa" Label="Entidad federativa" OnTextChanged="icEntidadFederativa_TextChanged"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-6 mt-3" Type="Text" ID="icPais" Label="País" Rules="required"/>
                                    <GWC:InputControl runat="server" Type="Hide" ID="icIdDomicilio" Label="IdDomicilio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="scDomicilio" Label="DomicilioFiscal" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveTaxid" Label="CVETAXID" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icSecDomicilio" Label="SecDomicilio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icNumeroExtInt" Label="NumExtInt" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveMunicipio" Label="CveMunicipio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveEntidadFederativa" Label="CveEntidadFederativa" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icIdPais" Label="IdPais"/>
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCvePais" Label="CvePais" />
                           
                                 </asp:Panel>
                             </ListControls> 
                            </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fsVinculaciones" Label="Vinculaciones" Detail="Vinculaciones con clientes" cssClass="mt-5">
                    <ListControls>                                                     
                        <GWC:CatalogControl runat="server" KeyField="indice" ID="ccVinculaciones" CssClass="w-100" Collapsed="true">
                            <Columns>               
                                <GWC:SelectControl runat="server" ID="scClienteVinculacion" OnClick="scClienteVinculacion_Click" OnTextChanged="scClienteVinculacion_TextChanged" LocalSearch="false" Label="Cliente">
                                </GWC:SelectControl>
                                <GWC:SelectControl  runat="server" ID="scTaxIdVinculacion" Label="Taxid proveedor" OnClick="scTaxIdVinculacion_Click"> </GWC:SelectControl>
                                 <GWC:SelectControl runat="server" ID="scVinculacion" Label="Vinculación" SearchBarEnabled="False" OnClick="scVinculacion_Click" OnTextChanged="scVinculacion_TextChanged" >
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" Type="Text" ID="icPorcentajeVinculacion" Label="Porcentaje" Format="Real" />
                            </Columns>
                        </GWC:CatalogControl>     
                    </ListControls>
                </GWC:FieldsetControl>

                 <GWC:FieldsetControl runat="server" ID="fsConfiguracionAdicional" Label="Configuración" Detail="Configuración adicional">
                    <ListControls>                        
                        <GWC:CatalogControl runat="server" ID="ccConfiguracionAdicional" KeyField="indice" CssClass="w-100" Collapsed="true">
                            <Columns>
                                <GWC:SelectControl runat="server" ID="scTaxIdConfiguracion" Label="Taxid proveedor" OnClick="scTaxIdConfiguracion_Click">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" ID="scClienteConfiguracion" OnClick="scClienteConfiguracion_Click" OnTextChanged="scClienteConfiguracion_TextChanged" LocalSearch="false" Label="Cliente">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" ID="scMetodoValoracion" Label="Método de valoración"  KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" ID="scIncoterm" Label="INCOTERM" KeyField ="i_Cve_TerminoFacturacion" DisplayField ="t_ValorPresentacion" Dimension ="Vt022TerminosFacturacionA14">
                                </GWC:SelectControl>
                            </Columns>
                        </GWC:CatalogControl>    
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fsHistorialDomicilios" Label="Domicilios" Detail="Historial de domicilios físcales"  Collapsed="true">
                    <ListControls>                        
                        <GWC:CatalogControl runat="server" ID="ccDomiciliosFiscales" KeyField="indice" CssClass="w-100" Collapsed="true" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="icTaxIDRFC" Label="TAXID"/>
                                <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icDomicilio" Label="Domicilio físcal"/>
                                <GWC:SwitchControl runat="server" ID="swcArchivarDomicilio" Label="Archivado" OnText="Sí" OffText="No"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server"></asp:Content>
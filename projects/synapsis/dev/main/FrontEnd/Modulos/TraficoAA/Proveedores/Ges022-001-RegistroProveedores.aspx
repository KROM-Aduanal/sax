<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-RegistroProveedores.aspx.vb" Inherits=".Ges022_001_RegistroProveedores" %>

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

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false"  Label="<span style='color:#321761'>Proveedores</span><span style='color:#782360;'>&nbsp;nacionales</span>" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Domicilios registrados" />
                    <GWC:ButtonItem Text="Vaciar domicilio" />
                    <GWC:ButtonItem Text="Clonar proveedor" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos Generales" Priority="true" CssClass="">
                    <ListControls> 

                     <GWC:CardControl runat="server" ID="aviso" CssClass="container wc-card-danger mb-5 mt-0" Visible="False"  >
                          <ListControls>
                              <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel2" >
                                         <asp:Label runat="server" ID="lbTitleAviso" Text="<svg xmlns='http://www.w3.org/2000/svg'  height='24' width='24' viewBox='0 0 512 512'><path d='M464 256A208 208 0 1 0 48 256a208 208 0 1 0 416 0zM0 256a256 256 0 1 1 512 0A256 256 0 1 1 0 256zm177.6 62.1C192.8 334.5 218.8 352 256 352s63.2-17.5 78.4-33.9c9-9.7 24.2-10.4 33.9-1.4s10.4 24.2 1.4 33.9c-22 23.8-60 49.4-113.6 49.4s-91.7-25.5-113.6-49.4c-9-9.7-8.4-24.9 1.4-33.9s24.9-8.4 33.9 1.4zM144.4 208a32 32 0 1 1 64 0 32 32 0 1 1 -64 0zm192-32a32 32 0 1 1 0 64 32 32 0 1 1 0-64z'/></svg> &nbsp; Este &nbsp;<span style='font-weight:bold !important'>proveedor</span>&nbsp; ya ha sido &nbsp;<span style='font-weight:bold !important; color:#432776 !important'> registrado </span>.&nbsp;" Visible="True" CssClass="mb-5 title_Card" style="font-weight: normal !important; font-size:18px !important; color:#5b5b5b !important" ></asp:Label>
                               </asp:Panel>
                          </ListControls>
                      </GWC:CardControl>

                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-12 col-md-1 col-lg-1 mt-3" Type="Text" ID="icClave" Rules="onlynumber" Label="Clave" />
                            <GWC:InputControl runat="server" Type="Hide" ID="cveEmpresa" Label="CveRazonsocial" />
                            <GWC:InputControl runat="server" Type="Hide" ID="idEmpresa" Label="idEmpresa" />
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-5 mt-3" ID="fcRazonSocial" KeyField="_id" DisplayField="razonsocial" Label="Razón social" Rules="required|maxlegth[250]|Unique" OnClick="fcRazonSocial_Click" OnTextChanged="fcRazonSocial_TextChanged"/> 
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-2 d-flex justify-content-center mt-3" ID="swcTipoUso" Label="" OnText="Importación" OffText="Exportación" Checked="False" OnCheckedChanged="swcTipoUso_CheckedChanged"/>
                    </ListControls>
                </GWC:FieldsetControl>
    
                <GWC:FieldsetControl runat="server" ID="fsDetalles" Label="Domicilios" Detail="Domicilios" CssClass="m-0 p-0"> 
                    <ListControls>
                          <GWC:CardControl runat="server" ID="ConfigurarDomicilios" CssClass="container m-0 p-0 mb-5" Visible="False">
                              <ListControls> 
                                   <asp:Panel runat="server" CssClass="m-0 mt-0 p-0" ID="Panel1" >
                                        <asp:Label runat="server" ID="lbTitle" Text="Seleccione domicilio" Visible="True" CssClass="w-100 mb-3 title_Card"></asp:Label> 
                                        <GWC:SelectControl runat="server" CssClass="col-xs-10 col-10 col-md-10 col-lg-10 mt-3 mb-5" ID="scDomiciliosRegistrados" LocalSearch="True" Label="Registrados" OnClick="scDomiciliosRegistrados_Click" OnSelectedIndexChanged="scDomiciliosRegistrados_SelectedIndexChanged" OnTextChanged="scDomiciliosRegistrados_TextChanged"></GWC:SelectControl>
                                        <GWC:ButtonControl runat="server" CssClass="col-xs-2 col-2 col-md-2 col-lg-2 mt-3 mb-3 d-flex justify-content-center" ID="btnTipoDomicilio" Label="Aplicar" OnClick="btnTipoDomicilio_Click"/>
                                   </asp:Panel>
                              </ListControls>
                        </GWC:CardControl>

              <%--      <GWC:CardControl runat="server" ID="CardControlRFC" CssClass="container wc-card-danger" Visible="False" style="border-color:#79145c !important; margin-bottom:100px !important"  >
                        <ListControls>
                            <asp:Panel runat="server" CssClass="m-0 mt-0 p-0 col-lg-10 col-md-10 col-12" ID="Panel3" >
                                 <asp:Label runat="server" ID="Label4" Text="" Visible="True" CssClass="w-100 mb-1 title_card_purple">
                                     <svg xmlns="http://www.w3.org/2000/svg" style="margin-bottom:25px; margin-right:5px" height="32" width="32" viewBox="0 0 512 512"><path fill="#757575" d="M256 32c14.2 0 27.3 7.5 34.5 19.8l216 368c7.3 12.4 7.3 27.7 .2 40.1S486.3 480 472 480L40 480c-14.3 0-27.6-7.7-34.7-20.1s-7-27.8 .2-40.1l216-368C228.7 39.5 241.8 32 256 32zm0 128c-13.3 0-24 10.7-24 24l0 112c0 13.3 10.7 24 24 24s24-10.7 24-24l0-112c0-13.3-10.7-24-24-24zm32 224a32 32 0 1 0 -64 0 32 32 0 1 0 64 0z"/></svg>
                                     Reemplazar el <span class="texto-resaltado">RFC</span> afectará a sus domicilios. <span class="texto-resaltado">¿Desea continuar?</span>
                                 </asp:Label>
                             </asp:Panel>
                                <asp:Panel runat="server" CssClass="m-0 mt-0 p-0 col-lg-2 col-md-2 col-12" ID="Panel4" >
                                    <asp:Button runat="server" CssClass="btn  mr-2 btn-active" Text="Aceptar" style="border-radius:18px; background-color:#7e61b0; border:none; padding:8px 24px; color:#ffffff; opacity:0.8;  box-shadow: 0px 1px 4px rgba(0, 0, 0, 0.2);"  id="btnCambiarRfcDomicilios" Label="Aceptar" onClick="btnCambiarRfcDomicilios_Click" />
                                    <asp:Button runat="server" CssClass="btn btn-active" Text="Cancelar" style="border-radius:18px; background-color:#616161; border:none; padding:8px 24px; color:#ffffff; opacity:0.4;  box-shadow: 0px 1px 4px rgba(0, 0, 0, 0.2);" id="btnNoCambiarRfcDomicilios" Label="Cancelar" onClick="btnNoCambiarRfcDomicilios_Click" />
                               </asp:Panel>
                        </ListControls>
                    </GWC:CardControl>--%>

                         <GWC:PillboxControl runat="server" ID="pbDetalleProveedor" KeyField="indice" CssClass="col-xs-12 m-0 p-0" OnCheckedChange="pbDetalleProveedor_CheckedChange" OnClick="pbDetalleProveedor_Click">
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
                                    <GWC:InputControl runat="server"  CssClass="col-xs-10 col-md-6 col-lg-5 mt-lg-5 p-5 justify-content-between" Type="Text" ID="icRFC" Label="RFC" Rules="required"/>
                                    <GWC:InputControl runat="server"  CssClass="col-xs-12 col-md-12 col-lg-3 mt-lg-3 p-5 justify-content-between" Type="Text" ID="icCURP" Label="CURP" Visible="False"/>
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-1 p-5 d-flex justify-content-center" ID="swcTipoPersona" Label="Persona moral" OnText="Si" OffText="No" OnCheckedChanged="swcTipoPersona_CheckedChanged" Checked="true"/>
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-6 col-lg-1  p-5 d-flex justify-content-center" ID="swcDestinatario" Label="Destinatario" OnText="Si" OffText="No" OnCheckedChanged="swcDestinatario_CheckedChanged" Checked="true"/>
                                    <asp:Panel runat="server" CssClass="mt-5 p-0" ID="sectionDomicilio" >
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-3" Type="Text" ID="icCalle" Label="Calle" Rules="required"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icNumeroExterior" Label="Número exterior"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icNumeroInterior" Label="Número interior"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icCodigoPostal" Label="Código postal" Format="Numeric" Rules="maxlegth[10]|onlynumber"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icColonia" Label="Colonia"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icLocalidad" Label="Localidad"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-3" Type="Text" ID="icCiudad" Label="Ciudad"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-3" Type="Text" ID="icMunicipio" Label="Municipio" OnTextChanged="icMunicipio_TextChanged"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-3" Type="Text" ID="icEntidadFederativa" Label="Entidad federativa" OnTextChanged="icEntidadFederativa_TextChanged"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-3" Type="Text" ID="icPais" Label="País" Rules="required"/>
                                    <GWC:InputControl runat="server" Type="Hide" ID="icIdDomicilio" Label="IdDomicilio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icSecDomicilio" Label="SecDomicilio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveRfc" Label="IdRFC" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveCurp" Label="IdCURP" />    
                                    <GWC:InputControl runat="server" Type="Hide" ID="scDomicilio" Label="DomicilioFiscal" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCvePais" Label="CvePais" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icIdPais" Label="IdPais"/>
                                    <GWC:InputControl runat="server" Type="Hide" ID="icNumeroExtInt" Label="NumExtInt" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveMunicipio" Label="CveMunicipio" />
                                    <GWC:InputControl runat="server" Type="Hide" ID="icCveEntidadFederativa" Label="CveEntidadFederativa" />  
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
                                <GWC:SelectControl  runat="server" ID="scTaxIdVinculacion" Label="RFC proveedor" OnClick="scTaxIdVinculacion_Click"> </GWC:SelectControl>
                                 <GWC:SelectControl runat="server" ID="scVinculacion" Label="Vinculación" SearchBarEnabled="False" OnClick="scVinculacion_Click" OnTextChanged="scVinculacion_TextChanged" >
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" Type="Text" ID="icPorcentajeVinculacion" Label="Porcentaje" Format="Real"/>
                            </Columns>
                        </GWC:CatalogControl>     
                    </ListControls>
                </GWC:FieldsetControl>

                 <GWC:FieldsetControl runat="server" ID="fsConfiguracionAdicional" Label="Configuración" Detail="Configuración adicional">
                    <ListControls>                        
                        <GWC:CatalogControl runat="server" ID="ccConfiguracionAdicional" KeyField="indice" CssClass="w-100" Collapsed="true">
                            <Columns>
                                <GWC:SelectControl runat="server" ID="scTaxIdConfiguracion" Label="RFC proveedor" OnClick="scTaxIdConfiguracion_Click">
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
                                <GWC:InputControl runat="server" Type="Text" ID="icTaxIDRFC" Label="RFC proveedor"/>
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
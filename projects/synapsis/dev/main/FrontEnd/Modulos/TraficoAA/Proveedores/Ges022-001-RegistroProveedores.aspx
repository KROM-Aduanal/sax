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

     <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Proveedores operativos (Exportación)" OnCheckedChanged="MarcarPagina">
            
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>

            <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos Generales" Priority="true" Enabled="False">
                    <ListControls>   
                        <GWC:InputControl runat="server" ID="bacalao" OnTextChanged="bacalao_TextChanged"/>
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-12 col-md-2 col-lg-1 mb-5 pt-3 mr-5" Enabled="False" Type="Text" ID="icClave" Rules="onlynumber" Label="Clave" />
                            <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="scProveedor" Label="SecuenciaProveedor" />
                            <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="cveEmpresa" Label="CveRazonsocial" />
                            <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="idEmpresa" Label="idEmpresa" />
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-12 col-md-10 col-lg-5 mb-5 pt-3" ID="fcRazonSocial" KeyField="_id" DisplayField="razonsocial" Label="Razón social" Rules="required|maxlegth[120]" OnClick="fcRazonSocial_Click" OnTextChanged="fcRazonSocial_TextChanged"/>                        
                            <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-12 col-md-9 col-lg-3 mb-5 pt-3" ID="fbcPais" Label="País" RequiredSelect="true" OnTextChanged="fbcPais_TextChanged" OnClick="fbcPais_Click" Visible="False"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-3 col-lg-2 mb-5 pt-3 d-flex justify-content-center" Enabled="False" ID="swcTipoUso" Label="Tipo de uso" OnText="Importación" OffText="Exportación" Checked="False" OnCheckedChanged="swcTipoUso_CheckedChanged"/>
                    </ListControls>
                </GWC:FieldsetControl>

<%--                COMPONENTE NUEVO--%>
               <GWC:FieldsetControl runat="server" ID="fsConfiguracionDomicilio" Label="Configurar" Detail="Domicilio" CssClass="" Visible="False"> 

                   <ListControls>

                        <asp:Panel runat="server" CssClass="col-2 col-xs-12 col-md-3 col-lg-3 mt-5 mb-5">    
                                <GWC:GroupControl runat="server" Label="Seleccione" Type="Radio" Columns="X2" OnCheckedChanged="rdSeleccionarDomicilio_CheckedChanged" ID="rdSeleccionarDomicilio">
                                    <ListItems>
                                            <GWC:Item Text="Registrado" />
                                            <GWC:Item Text="Nuevo" />
                                    </ListItems>
                                </GWC:GroupControl>
                        </asp:Panel>

                       <GWC:SelectControl runat="server"  CssClass="col-xs-10 col-md-7 col-lg-7 mb-5 mt-5" ID="scDomicilios" Label="Domicilios fiscales" OnSelectedIndexChanged="scDomicilios_SelectedIndexChanged" OnClick="scDomicilios_Click"/>


                      <asp:Panel runat="server" CssClass="col-2 col-xs-2 col-md-2 col-lg-2 mt-5 mb-5">
                          <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-12 col-md-12 col-lg-12" ID="swcEditarDomicilio" Label="Editar" OnText="Sí" OffText="No" OnCheckedChanged="swcEditarDomicilio_CheckedChanged"/>          
                      </asp:Panel>

                   </ListControls>

               </GWC:FieldsetControl>

             <%--   FIN DE COMPONENTE NUEVO--%>

                <GWC:FieldsetControl runat="server" ID="fsDetalles" Label="Domicilios" Detail="Detalles"> 

                    <ListControls> 
                         <GWC:PillboxControl runat="server" ID="pbDetalleProveedor" KeyField="indice" CssClass="col-xs-12 mb-0" OnCheckedChange="pbDetalleProveedor_CheckedChange" OnClick="pbDetalleProveedor_Click">
                             <ListControls>
   
                                    <asp:Panel runat="server" CssClass="col-1 col-xs-12 col-md-1 col-lg-1 d-flex align-items-center flex-column">
                                        <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-12"></asp:Label>
                                        <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta col-xs-12 col-md-12" Text="0"></asp:Label>
                                    </asp:Panel>

                                    <GWC:InputControl runat="server"  Visible="false" CssClass="col-xs-12 col-md-5 mt-5 p-5 justify-content-between" Type="Text" ID="icTaxId" Label="Tax number/ID físcal"/>      
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-5 mt-5 p-5 justify-content-between" Type="Text" ID="icRFC" Label="RFC" />
                                    <GWC:InputControl runat="server"  Visible="False" CssClass="col-xs-12 col-md-3 mt-5 p-5 justify-content-between" Type="Text" ID="icCURP" Label="CURP" />
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-2 mt-5 p-5 d-flex justify-content-center" ID="swcTipoPersona" Label="Tipo de persona" OnText="Física" OffText="Moral" OnCheckedChanged="swcTipoPersona_CheckedChanged"/>
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-6 col-lg-1 mt-5 p-5 d-flex justify-content-center" ID="swcDestinatario" Label="Destinatario" OnText="Sí" OffText="No" OnCheckedChanged="swcDestinatario_CheckedChanged"/>

                                 <asp:Panel runat="server" CssClass="" ID="sectionDomicilio" >
                                    <asp:Label runat="server" ID="lbDomicilio" Text="Domicilio" Visible="True" CssClass="w-100 cl_Domicilios p-5" ></asp:Label>                                                             
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icCalle" Label="Calle" Rules="required"/>                                                            
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icNumeroExterior" Label="Número exterior"/>                                                                                         
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icNumeroInterior" Label="Número interior"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icCodigoPostal" Label="Código postal" Rules="maxlegth[10]|onlynumber"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icColonia" Label="Colonia"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icLocalidad" Label="Localidad"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icCiudad" Label="Ciudad"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5" Type="Text" ID="icMunicipio" Label="Municipio" OnTextChanged="icMunicipio_TextChanged"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5" Type="Text" ID="icEntidadFederativa" Label="Entidad federativa" OnTextChanged="icEntidadFederativa_TextChanged"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-5" Type="Text" ID="icPais" Label="País"/>

                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icIdDomicilio" Label="IdDomicilio" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icSecDomicilio" Label="SecDomicilio" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCveRfc" Label="IdRFC" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCveCurp" Label="IdCURP" />    
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCveTaxId" Label="IdTaxId" />

                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="scDomicilio" Label="DomicilioFiscal" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCvePais" Label="CvePais" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icIdPais" Label="IdPais"/>
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icNumeroExtInt" Label="NumExtInt" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCveMunicipio" Label="CveMunicipio" />
                                    <GWC:InputControl runat="server" CssClass="m-0 p-0" Type="Hide" ID="icCveEntidadFederativa" Label="CveEntidadFederativa" />
                                           
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
                                <GWC:SelectControl runat="server" ID="scTaxIdVinculacion" Label="TaxID/RFC" OnClick="scTaxIdVinculacion_Click">
                                </GWC:SelectControl>
                                 <GWC:SelectControl runat="server" ID="scVinculacion" Label="Vinculación" SearchBarEnabled="False" OnClick="scVinculacion_Click">
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" Type="Text" ID="icPorcentajeVinculacion" Label="Porcentaje"/>
                            </Columns>
                        </GWC:CatalogControl>     
                    </ListControls>
                </GWC:FieldsetControl>

                 <GWC:FieldsetControl runat="server" ID="fsConfiguracionAdicional" Label="Configuración" Detail="Configuración adicional">
                    <ListControls>                        
                        <GWC:CatalogControl runat="server" ID="ccConfiguracionAdicional" KeyField="indice" CssClass="w-100" Collapsed="true">
                            <Columns>
                                <GWC:SelectControl runat="server" ID="scTaxIdConfiguracion" Label="TaxID/RFC" OnClick="scTaxIdConfiguracion_Click">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" ID="scClienteConfiguracion" OnClick="scClienteConfiguracion_Click" OnTextChanged="scClienteConfiguracion_TextChanged" LocalSearch="false" Label="Cliente">
                                </GWC:SelectControl>
                                <GWC:SelectControl runat="server" ID="scMetodoValoracion" Label="Método de valoración" KeyField ="i_Cve_MetodoValoracion" DisplayField ="t_DescripcionCorta" Dimension ="Vt022MetodosValoracionA11">
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
                                <GWC:InputControl runat="server" Type="Text" ID="icTaxIDRFC" Label="TaxID/RFC"/>
                                <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icDomicilio" Label="Domicilio físcal"/>
                                <GWC:SwitchControl runat="server" ID="swcArchivarDomicilio" Label="Archivado" OnText="Sí" OffText="No"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

            </Fieldsets>

        </GWC:FormControl>
     </div>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">

</asp:Content>

<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MaintainScrollPositionOnPostBack="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-TarifaArancelaria.aspx.vb" Inherits=".Ges022_001_TarifaArancelaria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Fración" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>

 <% End If %>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <style>
        .wc-group:before {
            background-color: #3b0b43 !important;
            color: #fff !important;
            width: 100%;
            text-align: center;
            top: 0 !important;
            left: 0 !important;
            border-radius: 4px 4px 0 0;
            padding: 10px 18px !important;
        }
        .wc-group.group-boders {
          border: 1px solid #3b0b43;
        }
        .wc-group {
          padding-top: 50px;
          margin-top: 42px;
        }
        .wc-group label {
          line-height: 36px;
        }
        .important-controls{
            padding: 8px 0;
            border-radius: 7px;
            border: 1px solid #3b0b43;
            border-top-width: 16px;
            padding-top: 32px;
            margin: 0 15px;
            margin-bottom: 16px;
        }
        .tbl-t tr td:first-child{
            width:240px;
        }
     
        .tbl-t tr th{
            white-space: nowrap;
        }
        .tbl-t tr td:nth-child(3),.tbl-t tr td:nth-child(6),.tbl-t tr td:nth-child(7){
            text-align: center;
        }
        .bigsize textarea {
            font-size: 120%;
        }
        .wc-dualitybar input[type="text"] {
            font-size: 120%;
        }
        .wc-dualitybar label:nth-child(2) > div > div:nth-child(2) a {
            display:none !important;
        }
    </style>
    
    <div class="d-flex">
        
        <GWC:FormControl runat="server" Label="Tarifa Arancelaria" ID="__SYSTEM_MODULE_FORM" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                </DropdownButtons>
            </Buttonbar>      
            <Fieldsets>
                <GWC:FieldsetControl runat="server" Label="Datos generales" ID="fscDatosGenerales">
                    <ListControls>  
                       
                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="dbcFraccionArancelaria" Label="Fracción arancelaria" LabelDetail="NICO"/>                       
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 nopadding">
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-3 mb-5 d-flex justify-content-center" ID="swcTipoOperacion" label="Tipo de Operación" OnText="Importación" OffText="Exportación" Checked="true" OnCheckedChanged="sw_TipoOperacion_CheckedChanged"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-3 mb-5 d-flex justify-content-center" ID="swcTipoMaterialPeligroso" label="Tipo de material" OnText="Peligroso" OffText="Peligroso"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-3 mb-5 d-flex justify-content-center" ID="swcTipoMaterialVulnerable" label="Material vulnerable" OnText="Si" OffText="No"/>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-3 mb-5 d-flex justify-content-center" ID="swcTipoMaterialSensible" label="Material sensible" OnText="Si" OffText="No"/>
                        </asp:Panel>          
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 nopadding">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 mb-5 solid-textarea bigsize" Type="Textarea" ID="icFraccion" Label="Descripción Fracción"/>
                            <GWC:InputControl runat="server" CssClass="col-xs-12 mb-5 solid-textarea bigsize" Type="Textarea" ID="icNico" Label="Descripción NICO"/>
                            <asp:Panel runat="server" CssClass="col-xs-12">
                                 <asp:Panel runat="server" CssClass="row no-gutters important-controls">
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="icValorImpuestoGeneral" Label="IGI / IGE"/>                               
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2" Type="Text" ID="icValorIVA" Label="IVA"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="icFranjaRegionFonteriza" Label="Franja región fronteriza"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4" Type="Text" ID="icUnidadMedida" Label="Unidad de medida"/>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:Panel>   
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 pointer-events-none">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaPublicacion" Label="Publicación"/> 
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Calendar" ID="icFechaEntradaVigor" Label="Entrada en vigor"/>                         
                            <GWC:GroupControl runat="server" CssClass="col-xs-12 col-md-12 mb-5 nopadding" Bordered="true" Columns="X3" type="Checkbox" Enabled="true" ID="gcRegulacionesRequeridas" Label="Requisitos">
                                <ListItems>
                                    <GWC:Item Text="IEPS"/>
                                    <GWC:Item Text="Normas"/>
                                    <GWC:Item Text="AP"/>
                                    <GWC:Item Text="Anexos"/>
                                    <GWC:Item Text="Cuotas"/>
                                    <GWC:Item Text="Embargos"/>
                                    <GWC:Item Text="Precios"/>
                                    <GWC:Item Text="Cupos"/>
                                    <GWC:Item Text="Permisos"/>
                                    <GWC:Item Text="Padrón"/>
                                </ListItems>
                            </GWC:GroupControl>                            
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>                      
                <GWC:FieldsetControl runat="server" ID="fscPreferencias" Label="Tratados" Detail="Preferencias arancelarias">
                    <ListControls>
                        
                        <asp:Panel runat="server" CssClass="row no-gutters">
                            <div class="col-xs-12">
                                <p>- Sin contenido</p>
                            </div>
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" ID="fscTratados"  Label="Tratados comerciales">  
                    <ListControls>
                        <GWC:TabbarControl runat="server" ID="tbcTratados" CssClass="col-xs-6">
                            <Tabs>       
                                <GWC:TabItem Text="AP"/>
                                <GWC:TabItem Text="T-MEC"/>
                                <GWC:TabItem Text="TLCUEM"/>
                                <GWC:TabItem Text="TIPAT"/>
                                <GWC:TabItem Text="AELC"/>
                                <GWC:TabItem Text="Otros"/>
                            </Tabs>
                            <TabsSections>
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccAP" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100 tbl-t">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesAp" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveAp" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelAp" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionAp" Format="Calendar" Label="Fecha Publicación"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorAp" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccTMEC" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesTmec" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveTmec" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelTmec" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionTmec" Format="Calendar" Label="Fecha Publicación"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorTmec" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl>        
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccTLCUEM" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesTlcuem" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveTlcuem" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelTlcuem" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionTlcuem" Format="Calendar" Label="Fecha Publicación"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorTlcuem" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl> 
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccTIPAT" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesTipat" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveTipat" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelTipat" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionTipat" Format="Calendar" Label="Fecha Publicación"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorTipat" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl> 
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccAELC" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesAelc" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveAelc" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelAelc" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionAelc" Format="Calendar" Label="Fecha Publicación"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorAelc" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl> 
                                <GWC:FieldsetControl runat="server">
                                    <ListControls>
                                        <GWC:CatalogControl runat="server" ID="ccOtros" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100">
                                            <Columns>
                                                <GWC:InputControl runat="server" ID="icPaisesOtros" Label="Paises"/>
                                                <GWC:InputControl runat="server" ID="icClaveOtros" Label="Clave"/>
                                                <GWC:InputControl runat="server" ID="icArancelOtros" Label="Arancel"/>
                                                <GWC:InputControl runat="server" ID="icPreferenciaOtros" Label="Preferencia"/>
                                                <GWC:InputControl runat="server" ID="icPublicacionOtros" Format="Calendar" Label="Fecha Publicacion"/>
                                                <GWC:InputControl runat="server" ID="icEntradaVigorOtros" Format="Calendar" Label="Fecha en Vigor"/>
                                            </Columns>
                                        </GWC:CatalogControl>
                                    </ListControls>
                                </GWC:FieldsetControl> 

                            </TabsSections>
                        </GWC:TabbarControl>
                    </ListControls>
                </GWC:FieldsetControl>
               
                
                <GWC:FieldsetControl runat="server" ID="fscAcuerdoIntegracion" Label="Acuerdo de integración">
                    <ListControls>
                        <GWC:FieldsetControl runat="server" Label="ALADI" Priority="false">
                            <ListControls>
                                <GWC:CatalogControl runat="server" ID="ccAladi" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100 tbl-t">
                                    <Columns>
                                        <GWC:InputControl runat="server" ID="icPaisAladi" Label="Paises"/>
                                        <GWC:InputControl runat="server" ID="icSectorAladi" Label="Sector"/>
                                        <GWC:InputControl runat="server" ID="icArancelAladi" Label="Arancel"/>
                                        <GWC:InputControl runat="server" ID="icPreferenciaAladi" Label="Preferencia"/>
                                        <GWC:InputControl runat="server" ID="icObservacionAladi" Label="Observación"/>
                                        <GWC:InputControl runat="server" ID="icPublicacionAladi" Format="Calendar" Label="Fecha Publicación"/>
                                        <GWC:InputControl runat="server" ID="icFechaVigorAladi" Format="Calendar" Label="Fecha en Vigor"/>
                                    </Columns>
                                </GWC:CatalogControl>
                            </ListControls>
                        </GWC:FieldsetControl>
                    </ListControls>
                </GWC:FieldsetControl>
               
                <GWC:FieldsetControl runat="server" ID="fscProgramasFomento" Label="Programas de fomento">
                    <ListControls>
                        <GWC:FieldsetControl runat="server" Label="PROSEC" Priority="false">
                            <ListControls>
                                <GWC:CatalogControl runat="server" ID="ccProsec" KeyField="indice" Collapsed="true" UserInteraction="false" CssClass="w-100 tbl-t">
                                    <Columns>
                                        <GWC:InputControl runat="server" ID="icSectorProsec" Label="Sector"/>
                                        <GWC:InputControl runat="server" ID="icArancelProsec" Label="Arancel"/>
                                        <GWC:InputControl runat="server" ID="icNotaProsec" Label="Nota"/>
                                        <GWC:InputControl runat="server" ID="icPublicacionProsec" Format="Calendar" Label="Fecha Publicación"/>
                                        <GWC:InputControl runat="server" ID="icFechaVigorProsec" Format="Calendar" Label="Fecha en Vigor"/>
                                    </Columns>
                                </GWC:CatalogControl>
                            </ListControls>
                        </GWC:FieldsetControl>
                    </ListControls>
                </GWC:FieldsetControl>

                 
                <GWC:FieldsetControl runat="server" ID="fscArancelCupo" Label="Arancel cupo">
                    <ListControls>
                        <GWC:PillboxControl runat="server" ID="ccArancelCupo" KeyField="indice" CssClass="w-100">
                            <ListControls>
                                <GWC:InputControl runat="server" Label="Pais" ID="icPaisArancelCupo" CssClass="col-xs-4"/>
                                <GWC:InputControl runat="server" Label="Fecha publicacion" Format="Calendar"  ID="icPublicacionArancelCupo" CssClass="col-xs-4"/>
                                <GWC:InputControl runat="server" Label="Entrada en vigor" Format="Calendar"  ID="icFechaVigorArancelCupo" CssClass="col-xs-4"/>
                                <GWC:InputControl runat="server" Label="Total" ID="icTotalArancelCupo" CssClass="col-xs-4"/>
                                <GWC:InputControl runat="server" Label="Medida" ID="icMedidaArancelCupo" CssClass="col-xs-4"/>
                                <asp:Panel runat="server" CssClass="col-xs-6 p-0">
                                    <GWC:InputControl runat="server" Label="Arancel" ID="icArancelArancelCupo" CssClass="col-xs-12 mt-4"/>
                                    <GWC:InputControl runat="server" Label="Arancel fuera" ID="icArancelFueraArancelCupo" CssClass="col-xs-12 mt-4"/>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="col-xs-6 p-0">
                                    <GWC:InputControl runat="server" Label="Nota" Type="TextArea" ID="icNotaArancelCupo" CssClass="col-xs-12"/>
                                </asp:Panel>
                            </ListControls>
                        </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>
                

                <GWC:FieldsetControl runat="server" ID="fscRequisitos" Label="Requisitos">
                    <ListControls>
                        <GWC:FieldsetControl runat="server" ID="fscCupo" Label="Cupos / país" Visible="false" Priority="false">
                            <ListControls>
                                
                                <%--<asp:panel runat="server" CssClass="fieldset col-xs-12">
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Bold="True" Text="--" Detail="Pais:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="--" Detail="Total Cupo:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="0" Detail="Arancel:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="0" Detail="Arancel Fuera:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Text="28/12/2020" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="28/12/2020" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="--" Detail="Medida:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="Certificado de cupo por la Secretaría de Economía." Detail="Nota:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>                         
                                </asp:panel> --%>
                                <asp:Panel runat="server" CssClass="row no-gutters">
                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>
                                </asp:Panel>

                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscImpuestoEspecial" Label="IEPS" Detail="Impuesto especial sobre producción y servicios / IEPS" Visible="false" Priority="false">
                            <ListControls>
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If IEPS IsNot Nothing %>
                                <% For Each row As IepsItem In IEPS %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="" Detail="Categoría:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Bold="True" Text="" Detail="Tipo:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="" Detail="Tasa:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="" Detail="Cuota:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="" Detail="Medida:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.Nota%>" Detail="Observación:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div> 

                                <% Next  %>
                                <% Else %>

                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>

                                <% End If %>
                                </asp:panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscCuotasCompensatorias" Label="CC" Detail="Cuotas compensatorias / Empresas" Visible="false" Priority="false">
                            <ListControls>
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If Cuotas IsNot Nothing %>
                                <% For Each row As CuotaItem In Cuotas %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="<%=row.NombreEmpresa%>" Detail="Empresa:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Bold="True" Text="<%=row.NombrePais%>" Detail="Pais:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.Tasa%>" Detail="Cuota:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.TipoCuota%>" Detail="Tipo:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="<%=row.FechaPublicacion%>" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.FechaInicioVigencia%>" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.Nota%>" Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div> 

                                <% Next  %>
                                <% Else %>

                                <div class="col-xs-12">
                                    <p>- Sin contenido</p>
                                </div>

                                <% End If %>
                                </asp:panel>     
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscPreciosEstimados" Label="PE" Detail="Precios estimados / Mercancía" Visible="false" Priority="false">
                            <ListControls>                       
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If Precios IsNot Nothing %>
                                <% For Each row As PrecioItem In Precios %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="<%=row.PrecioEstimado%>" Detail="Precio:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.DescripcionUM%>" Detail="Unidad:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="<%=row.FechaPublicacion%>" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.FechaInicioVigencia%>" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.DetalleProducto%>" Detail="Descripción:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div> 

                                <% Next  %>
                                <% Else %>

                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>

                                <% End If %>
                                </asp:panel>   
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscPermisos" Label="Permisos" Priority="false">
                            <ListControls>
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If Permisos IsNot Nothing %>
                                <% For Each row As PermisoItem In Permisos %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="<%=row.ClavePermiso%>" Detail="Clave:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.Descripcion%>" Detail="Permiso:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="<%=row.FechaPublicacion%>" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.FechaInicioVigencia%>" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.Particularidad%>" Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div>

                                <% Next  %>
                                <% Else %>

                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>

                                <% End If %>
                                </asp:panel>

                                <%--<GWC:FieldsetControl runat="server" ID="_fsSE" Visible="false" Priority="false" Label="CP" Detail="Secretaría de Economía / SE">                           
                                </GWC:FieldsetControl>
                                <GWC:FieldsetControl runat="server" ID="_fsSADER" Visible="false" Priority="false" Label="CP" Detail="Secretaría de Agricultura y Desarrollo Rural / SADER">
                                </GWC:FieldsetControl>
                                <GWC:FieldsetControl runat="server" ID="_fsSSA" Priority="false" Label="CP" Detail="Secretaría de Salud / SSA"/>                
                                <GWC:FieldsetControl runat="server" ID="_fsSEMARNAT" Visible="false" Priority="false" Label="CP" Detail="Secretaría de Medio Ambiente y Recursos Naturales / SEMARNAT">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsCICOPLAFEST" Visible="false" Priority="false" Label="CP" Detail="Comisión Intersecretarial para el Control del Proceso y Uso de Plaguicidas y Sustancias Tóxicas / CICOPLAFEST">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsSEDENA" Visible="false" Priority="false" Label="CP" Detail="Secretaría de la Defensa Nacional  / SEDENA">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsSENER" Visible="false" Priority="false" Label="CP" Detail="Secretaría de Energía / SENER">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsINAH" Visible="false" Priority="false" Label="CP" Detail="Instituto Nacional de Antropología e Historia / INAH">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsINBAL" Visible="false" Priority="false" Label="CP" Detail="Instituto Nacional de Bellas Artes / INBAL">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsCMC" Visible="false" Priority="false" Label="CP" Detail="Consejo Mexicano del Café o los consejos estatales / CMC">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsCRT" Visible="false" Priority="false" Label="CP" Detail="Consejo Regulador Del Tequila / CRT">
                                </GWC:FieldsetControl>                  
                                <GWC:FieldsetControl runat="server" ID="_fsCRE" Visible="false" Priority="false" Label="CP" Detail="Comisión Reguladora de Energía / CRE">
                                </GWC:FieldsetControl>--%>
                            </ListControls> 
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscNormas" Label="Normas" Detail="Normas oficiales mexicanas" Visible="false" Priority="false">
                            <ListControls>                       
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If Normas IsNot Nothing %>
                                <% For Each row As NormaItem In Normas %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="<%=row.NOM%>" Detail="NOM:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="<%=row.Descripcion%>" Detail="Descripción:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="<%=row.FechaPublicacion%>" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.FechaInicioVigencia%>" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="" Detail="Dato omitido o inexacto:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="" Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div> 

                                <% Next  %>
                                <% Else %>

                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>

                                <% End If %>
                                </asp:panel>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscAnexos" Label="Anexos" Visible="false" Priority="false">
                            <ListControls>                       
                        
                                <%--<asp:panel runat="server" CssClass="fieldset col-xs-12">
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Bold="True" Text="17" Detail="Número:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Bold="True" Text="Transito interno por territorio nacional." Detail="Nombre:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="Mercancías por las que no procederá el tránsito internacional por territorio nacional." Detail="Descripción:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Text="28/12/2020" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="28/12/2020" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="IX. Tratándose de las siguientes mercancías: k) Juguetes: Juguetes y modelos, con motor, excepto lo comprendido en las  fracciones arancelarias 95030002, 95030003, 95030004, 95030005,  95030006, 95030007, 95030009,95030010, 95030011, 95030012, 95030014, 95030015 y 95030018." Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>                         
                                </asp:panel>--%>
                                <asp:Panel runat="server" CssClass="row no-gutters">
                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>
                                </asp:Panel>

                            </ListControls>
                        </GWC:FieldsetControl>                
                        <GWC:FieldsetControl runat="server" ID="fscEmbargos" Label="Embargos" Visible="false" Priority="false">
                            <ListControls>                       
                        
                                <%--<asp:panel runat="server" CssClass="fieldset col-xs-12">
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Bold="True" Text="Corea" Detail="Pais:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="Prohibida" Detail="Aplicación:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="--" Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Text="28/12/2020" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="28/12/2020" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="Láseres, excepto los diodos láser." Detail="Mercancía:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>                         
                                </asp:panel>--%>
                                <asp:Panel runat="server" CssClass="row no-gutters">
                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>
                                </asp:Panel>

                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscCuposMinimos" Label="Cupos mínimos" Visible="false" Priority="false">
                            <ListControls>
                        
                                <%--<asp:panel runat="server" CssClass="fieldset col-xs-12">
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Bold="True" Text="Cuba" Detail="Pais:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="Dolares" Detail="Unidad:" CssClass="col-xs-12"></gwc-label>
                                        <gwc-label Text="20,000" Detail="Cupo:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>
                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                        <gwc-label Text="28/12/2020" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="28/12/2020" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                        <gwc-label Text="Rellenos." Detail="Descripción:" CssClass="col-xs-12"></gwc-label>
                                    </asp:Panel>                         
                                </asp:panel>--%>
                                <asp:Panel runat="server" CssClass="row no-gutters">
                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>
                                </asp:Panel>
                               
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" ID="fscPadronSctorial" Label="Padrón sectorial" Visible="false" Priority="false">
                            <ListControls>
                                <asp:panel runat="server" CssClass="row no-gutters">
                                <% If Padrones IsNot Nothing %>
                                <% For Each row As PadronItem In Padrones %>
                            
                                    <div class="fieldset col-xs-12">
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Bold="True" Text="<%=row.Sector%>" Detail="Sector:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="" Detail="Anexo:" CssClass="col-xs-12"></gwc-label>
                                            <gwc-label Text="" Detail="Acotación:" CssClass="col-xs-12"></gwc-label>
                                        </div>
                                        <div class="col-xs-12 col-md-6">
                                            <gwc-label Text="" Detail="Publicación:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="" Detail="Entrada en vigor:" CssClass="col-xs-12 col-md-6"></gwc-label>
                                            <gwc-label Text="<%=row.Notas%>" Detail="Descripción:" CssClass="col-xs-12"></gwc-label>
                                        </div>                         
                                    </div> 

                                <% Next  %>
                                <% Else %>

                                    <div class="col-xs-12">
                                        <p>- Sin contenido</p>
                                    </div>

                                <% End If %>
                                </asp:panel>
                            </ListControls>
                        </GWC:FieldsetControl>                
                        <GWC:FieldsetControl runat="server" ID="fscDescripciones" Label="Descripciones" Priority="false">
                            <ListControls>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="Text" ID="icSeccion" Label="Sección"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="TextArea" ID="icCapitulo" Label="Capítulo"/>     
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="TextArea" ID="icPartida" Label="Partida"/>
                                </asp:Panel>                            
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-6">                                
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 mb-5" Type="TextArea" ID="icSubpartida" Label="Subpartida"/> 
                                </asp:Panel>   
                            </ListControls>
                        </GWC:FieldsetControl>
                    </ListControls>
                </GWC:FieldsetControl>
                    
            </Fieldsets>
        </GWC:FormControl>    
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

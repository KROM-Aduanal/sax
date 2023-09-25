<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-RegistroApendices.aspx.vb" Inherits=".Ges022_001_RegistroApendices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

<GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
       
        <GWC:FormControl HasAutoSave="false" ID="__SYSTEM_MODULE_FORM" runat="server" Label="Alta de Apendices" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera"/>
            <Fieldsets>
                <GWC:FieldsetControl ID="A1" runat="server" Detail="Aduana sección" Label="Apendice 01">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo1" runat="server" KeyField="i_Cve_AduanaSeccion" CssClass="w-100" Dimension="Vt022AduanaSeccionA01"  UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Aduana" Label="Aduana" Rules="required|min_length[2]|onlynumber"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Seccion" Label="Sección"/>  
                                <GWC:InputControl runat="server" Type="Text" ID="t_AduanaSeccionDenominacion" Label="Denominación"/>                                       
                            </Columns>
                        </GWC:CatalogControl>

                    </ListControls>
                </GWC:FieldsetControl>
               
                <GWC:FieldsetControl ID="A2" runat="server" Detail="Claves de pedimento" Label="Apendice 02">                         
                    <ListControls> 
                        <GWC:CatalogControl ID="catalogo2"  runat="server" KeyField="i_Cve_ClavePedimento" CssClass="w-100" Dimension="Vt022ClavesPedimentoA02" Collapsed="false"  UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Pedimento" Label="Clave de Pedimento" Rules="required|min_length[2]|onlynumber"/>   
                                <GWC:InputControl runat="server" Type="Text" Format="Calendar" ID="f_FechaRegistro" Label="Fecha Registro"/>  
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A3" runat="server" Detail="Medios de transporte" Label="Apendice 03">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo3" runat="server" KeyField="i_Cve_MedioTransporte" CssClass="w-100" Dimension="Vt022MediosTransporteA03" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_MedioTransporte" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_MedioTransporte" Label="Medio de Transporte"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>
                        
                <GWC:FieldsetControl ID="A4" runat="server" Detail="Claves de paises" Label="Apendice 04">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo4" runat="server" KeyField="i_Cve_Pais" CssClass="w-100" Dimension="Vt022PaisesA04" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Pais" Label="Pais"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_ClaveSAAIFIII" Label="Clave SAAI FIII"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_ClaveSAAIM3" Label="Clave SAAI M3"/>                                      
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A5" runat="server" Detail="Claves de monedas" Label="Apendice 05">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo5" runat="server" KeyField="i_Cve_Moneda" CssClass="w-100" Dimension="Vt022MonedasA05" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="InputControl1" Label="Pais"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Moneda" Label="Clave Moneda"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_NombreMoneda" Label="Nombre Moneda"/>                                      
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A6" runat="server" Detail="Recintos fiscalizados" Label="Apendice 06">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo6" runat="server" KeyField="i_Cve_RecintoFiscalizado" CssClass="w-100" Dimension="Vt022RecintosFiscalizadosA06" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Aduana" Label="Aduana"/>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveRecintoFiscalizado" Label="Clave "/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_RecintoFiscalizado" Label="Recinto Fiscalizado"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A7" runat="server" Detail="Unidades de medida" Label="Apendice 07">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo7" runat="server" KeyField="i_Cve_UnidadMedida" CssClass="w-100" Dimension="Vt022UnidadesMedidaA07" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveUnidadMedida" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionUnidadMedida" Label="Unidad de Medida"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A8" runat="server" Detail="Identificadores" Label="Apendice 08">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo8" runat="server" KeyField="i_Cve_Identificador" CssClass="w-100" Dimension="Vt022IdentificadoresA08" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Identificador" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Identificador" Label="Identificador"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Nivel" Label="Nivel"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A9" runat="server" Detail="Regulaciones y restricciones no arancelarias" Label="Apendice 09">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo9" runat="server" KeyField="i_Cve_RegulacionRestriccionNoArancelaria" CssClass="w-100" Dimension="Vt022RegulacionesRestriccionesNoArancelariasA09" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_RegulacionRestriccionNoArancelaria" Label="Unknow"/>
                                <GWC:InputControl runat="server" Type="Text" Format="Calendar" ID="InputControl2" Label="Fecha de Registro"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A10" runat="server" Detail="Tipo de contenedores y vehículos de autotransporte" Label="Apendice 10">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo10" runat="server" KeyField="i_Cve_TipoContenedorVehiculoTransporte" CssClass="w-100" Dimension="Vt022TiposContenedoresVehiculosTransporteA10" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveTipoContenedorVehiculoTransporte" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionTipoContenedorVehiculoTransporte" Label="Unidad de Medida"/>
                                <GWC:InputControl runat="server" Type="Text" ID="i_EsContenedor" Label="Contenedor"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A11" runat="server" Detail="Claves de métodos de valoración" Label="Apendice 11">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo11" runat="server" KeyField="i_Cve_MetodoValoracion" CssClass="w-100" Dimension="Vt022MetodosValoracionA11" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveMetodoValoracion" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionCorta" Label="Descripción"/>
                                <GWC:InputControl runat="server" Type="Text" ID="InputControl3" Label="Contenedor"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A12" runat="server" Detail="Contribuciones, cuotas compensatorias, gravámenes y derechos" Label="Apendice 12" >
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo12" runat="server" KeyField="i_Cve_Contribucion" CssClass="w-100" Dimension="Vt022ContribucionesA12" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveContribucion" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Contribucion" Label="Contribución"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Abreviacion" Label="Abreviación"/>
                                <GWC:InputControl runat="server" Type="Text" ID="InputControl4" Label="Nivel"/>                  
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A13" runat="server" Detail="Formas de pago" Label="Apendice 13">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo13" runat="server" KeyField="i_Cve_FormaPago" CssClass="w-100" Dimension="Vt022FormasPagoA13" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveFormaPago" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionFormaPago" Label="Descripción"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A14" runat="server" Detail="Términos de facturación" Label="Apendice 14">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo14" runat="server" KeyField="i_Cve_TerminoFacturacion" CssClass="w-100" Dimension="Vt022TerminosFacturacionA14" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveTerminoFacturacion" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_TerminoFacturacion" Label="Clave Término de Facturación"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_TerminoFacturacion" Label="Término de Facturación"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A15" runat="server" Detail="Destinos de mercancía" Label="Apendice 15">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo15" runat="server" KeyField="i_Cve_DestinoMercancia" CssClass="w-100" Dimension="Vt022DestinosMercanciasA15" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveDestinoMercancia" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionDestinoMercancia" Label="Descripción"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A16" runat="server" Detail="Regímenes" Label="Apendice 16">
                    <ListControls>
                        <GWC:CatalogControl id="catalogo16" runat="server" KeyField="i_Cve_Regimen" CssClass="w-100" Dimension="Vt022RegimenesA16" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Regimen" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionRegimen" Label="Descripción"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A18" runat="server" Detail="Tipos de tasas" Label="Apendice 18">
                    <ListControls>
                        <GWC:CatalogControl ID="catalogo18" runat="server" KeyField="i_Cve_TipoTasa" CssClass="w-100" Dimension="Vt022TiposTasasA18" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="i_ClaveTipoTasa" Label="Clave"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_DescripcionTipoTasa" Label="Descripción"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl ID="A21" runat="server" Detail="Recinto fiscalizado estratégicos" Label="Apendice 21">
                    <ListControls>
                        <GWC:CatalogControl runat="server" ID="catalogo21" KeyField="i_Cve_RecintoFiscalizadoEstrategico" CssClass="w-100" Dimension="Vt022RecintosFiscalizadosEstrategicosA21" Collapsed="false" UserInteraction="false">
                            <Columns>
                                <GWC:InputControl runat="server" Type="Text" ID="InputControl5" Label="Aduana"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_RFE" Label="Clave RFE"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_AdministradorRFE" Label="Administrador RFE"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_Cve_Operador" Label="Clave Operador"/>
                                <GWC:InputControl runat="server" Type="Text" ID="t_OperadorRFE" Label="Operador"/>
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

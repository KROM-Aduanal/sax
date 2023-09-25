<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-RegistroProveedores.aspx.vb" Inherits=".Ges022_001_RegistroProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Proveedor" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>
        
    <style>

        .cl_Domicilios {
            opacity: .6;
            color: #757575;
            display: inline-block;
            padding: 0 14px;
            border-radius: 0 16px 16px 0px;
        }

        .cl_Domicilios {
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

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

<GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">

     <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Proveedores operativos" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar" />
                    <GWC:ButtonItem Text="Imprimir" />
                    <GWC:ButtonItem Text="Mandar por Correo" />
                </DropdownButtons>
            </Buttonbar>
            <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos Generales" Priority="true">
                    <ListControls>                        
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-1 mb-5" Enabled="False" Type="Text" ID="icClave" Rules="onlynumber" Label="Clave" />
                        <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-5 mb-5" ID="fcRazonSocial" KeyField="_id" DisplayField="razonsocial" Label="Nombre, denominación o razón social" Rules="required|maxlegth[120]" OnClick="fcRazonSocial_Click" OnTextChanged="fcRazonSocial_TextChanged"/>                        
                        <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 text-left" ID="swcTipoUso" Label="Tipo de uso" OnText="Exportación" OffText="Importación" Checked="True" OnCheckedChanged="swcTipoUso_CheckedChanged"/>                                           
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fsDetalles" Label="Detalle" Detail="Detalle proveedor">                           
                    <ListControls>
                        
                         <GWC:PillboxControl runat="server" ID="pbDetalleProveedor" KeyField="indice" CssClass="col-xs-12" OnCheckedChange="pbDetalleProveedor_CheckedChange" OnClick="pbDetalleProveedor_Click">
                                <ListControls>

                                    <asp:Panel runat="server" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                        <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-1"></asp:Label>
                                        <asp:Label runat="server" ID="lbNumero" class="cl_Num__Tarjeta col-xs-12 col-md-1" Text="0"></asp:Label>
                                    </asp:Panel>

                                    <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-4 mb-5 align-items-center" Type="Text" ID="icTaxId" Label="Tax number/ID físcal" />
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 align-items-center" Type="Text" ID="icRFC" Label="RFC" />
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 align-items-center" Type="Text" ID="icCURP" Label="CURP" />
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-1 mb-5 text-Left" ID="swcDestinatario" Label="Destinatario" OnText="Sí" OffText="No" />             
                                                      
                                    <asp:Label runat="server" ID="lbDomicilio" Text="Domicilio" Visible="True" CssClass="w-100 p-5 cl_Domicilios" ></asp:Label>

                                    <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-3 mb-5 text-left" ID="swcUtilizarDatos" Label="Utilizar datos" OnText="Registrados" OffText="Nuevos" OnCheckedChanged="swcUtilizarDatos_CheckedChanged"/>
                                    <GWC:SwitchControl runat="server" CssClass="col-xs-6 col-md-3 mb-5 text-left" ID="swcEditarDomicilio" Label="Editar domicilio" OnText="Sí" OffText="No" OnCheckedChanged="swcEditarDomicilio_CheckedChanged"/>                                                                        
                                    <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scDomicilios" Label="Domicilio físcal" OnSelectedIndexChanged="scDomicilios_SelectedIndexChanged" OnClick="scDomicilios_Click"/>

                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="icCalle" Label="Calle" Rules="required"/>                                                            
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icNumeroExterior" Label="Número exterior"/>                                                                                         
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icNumeroInterior" Label="Número interior"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="icColonia" Label="Colonia"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="icCodigoPostal" Label="Código postal" Rules="maxlegth[10]|onlynumber"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icLocalidad" Label="Localidad"/>
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icCiudad" Label="Ciudad"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" ID="icMunicipio" Label="Municipio"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-2 mb-5" Type="Text" ID="icEntidadFederativa" Label="Entidad federativa"/>   
                                    <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" ID="icPais" Label="País"/>
                                </ListControls> 
                            </GWC:PillboxControl>             
                    
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fsVinculaciones" Label="Vinculaciones" Detail="Vinculaciones con clientes">
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

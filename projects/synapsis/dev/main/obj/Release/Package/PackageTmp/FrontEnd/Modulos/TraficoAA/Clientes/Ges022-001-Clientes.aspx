<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-Clientes.aspx.vb" Inherits=".Ges022_001_Clientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Cliente" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>

 <% End If %>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

<GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
     <div class="d-flex">
           
           <GWC:FormControl HasAutoSave="false" ID="__SYSTEM_MODULE_FORM" runat="server" Label="Catálogo de Clientes" OnCheckedChanged="MarcarPagina">
                    <Buttonbar runat="server" OnClick="EventosBotonera">
                        <DropdownButtons>
                            <GWC:ButtonItem Text="Factor de cambio"/>
                             <GWC:ButtonItem Text="Limpiar"/>
                             <GWC:ButtonItem Text="Descargar"/>
                            <GWC:ButtonItem Text="Imprimir"/>
                            <GWC:ButtonItem Text="Mandar por Correo"/>
                        </DropdownButtons>
                    </Buttonbar>           
                    <Fieldsets>

                        <GWC:FieldsetControl ID="_fsdatosgenerales" runat="server" Label="Datos Generales">
                            
                             <ListControls>                               
                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="i_Cve_Empresa" KeyField="_id" DisplayField="razonsocial" Label="Razón social|IMP/EXP" Rules="required" OnTextChanged="i_Cve_Empresa_TextChanged" OnClick="i_Cve_Empresa_Click"/>
                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 d-flex align-items-end" ID="s_tipoPersona" label="Tipo Persona" OnText="M" OffText="F" Checked="true"/>
                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 d-flex align-items-end" ID="s_Habilitado" label="Habilitado" OnText="Sí" OffText="No" OnCheckedChanged="s_Habilitado_CheckedChanged"/>
                                <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-2 mb-5 d-flex align-items-end" ID="s_Extranjero" label="Extranjero" OnText="Sí" OffText="No" OnCheckedChanged="CambioTipoEmpresa"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_RFC" Label="RFC" Rules="required|maxlegth[16]"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_CURP" Label="Curp" Rules="required|maxlegth[18]" ToolTip="Hola mundo" ToolTipExpireTime="4" ToolTipModality="Classic" ToolTipStatus="OkInfo"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_TaxID" Label="Tax Number" Visible="false" />

                            </ListControls>
                           
                        </GWC:FieldsetControl>
                        
                        <GWC:FieldsetControl runat="server" Label="Domicilio" Priority="false" ID="fsDomicilio">
                             <ListControls>
                                 <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 d-flex align-items-end">
                                     <GWC:SwitchControl runat="server" Visible="true" ID="s_SeleccionarDomicilio" label="Utilizar datos" OnText="Registrados" OffText="Nuevos" Checked="true" OnCheckedChanged="s_SeleccionarDomicilio_CheckedChanged"/>
                                     <GWC:SwitchControl runat="server" Visible="false" CssClass="ml-5" ID="s_EditarDomicilio" label="Editar domicilio" OnText="Si" OffText="No" Checked="false" OnCheckedChanged="s_EditarDomicilio_CheckedChanged"/>
                                 </asp:Panel>
                                
                                <GWC:SelectControl ID="s_Domicilios" CssClass="col-xs-12 col-md-6 mb-5" runat="server" Label="Domicilio Fiscal"/>

                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_Calle" Label="Calle" Rules="required"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_NumeroExt" Label="Número exterior" Rules="required|onlynumber"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_NumeroInt" Label="Número interior"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_Colonia" Label="Colonia"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_CP" Label="Código postal" Rules="required|maxlegth[7]|onlynumber"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_Estado" Label="Estado"   Rules="required"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_Ciudad" Label="Ciudad"   Rules="required"/>
                                <GWC:InputControl runat="server" Visible="false" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" ID="t_Pais" Label="País"  Rules="required"/>
                             </ListControls>
                        </GWC:FieldsetControl>


                    <%--<GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_Calle" ID="InputControl1" Label="Calle" Rules="required"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_NumeroExt" ID="InputControl2" Label="Número exterior" Rules="required|onlynumber"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_NumeroInt" ID="InputControl3" Label="Número interior"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_Colonia" ID="InputControl4" Label="Colonia"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_CP" ID="InputControl5" Label="Código postal" Rules="required|maxlegth[7]|onlynumber"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_Estado" ID="InputControl6" Label="Estado"   Rules="required"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_Ciudad" ID="InputControl7" Label="Ciudad"   Rules="required"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" Type="Text"  Name="t_Pais" ID="InputControl8" Label="País"  Rules="required"/>--%>


                        <GWC:FieldsetControl iD="_fsaduanaspordefecto" runat="server" Label="Aduanas por defecto">
                            <ListControls>
                                
                                <GWC:CatalogControl CssClass="w-100" ID="_cataduanasdefecto" runat="server" Collapsed="true" KeyField="indice">
                                    <Columns>
                                        
                                        <GWC:SelectControl ID="sc_claveAduanaSeccion" SearchBarEnabled="false" runat="server" Label="Modalidad|Aduana" OnClick="sc_claveAduanaSeccion_Click"/>
                                        
                                        <GWC:SelectControl ID="sc_patenteAduanal" runat="server" SearchBarEnabled="false" Label="Patente" OnClick="sc_patenteAduanal_Click" />

                                        <GWC:SelectControl  ID="sc_tipoOperacion" runat="server" SearchBarEnabled="false" Label="Tipo de operación" >
                                            <Options>
                                                <GWC:SelectOption Text="Importación" Value="1" />
                                                <GWC:SelectOption Text="Exportación" Value="2" />
                                            </Options>
                                        </GWC:SelectControl>
                                        
                                      </Columns>
                                </GWC:CatalogControl>
                            </ListControls>

                        </GWC:FieldsetControl>

                        <GWC:FieldsetControl ID="_fscontactos" runat="server" Label="Contactos del Importador/Exportador">

                            <ListControls>
                                <GWC:CatalogControl CssClass="w-100"  ID="ccContactos" runat="server" Collapsed="true" KeyField="indice">
                                    <Columns>
                                        
                                        <GWC:InputControl ID="icNombreContacto" runat="server" Type="Text" Label="Nombre"/>
                                        <GWC:InputControl ID="icInfoContacto" runat="server" Type="Text" Label="Información adicional"/>
                                        <GWC:InputControl ID="icTelefono1" runat="server" Type="Text" Format="Phone" Label="Teléfono 1" />
                                        <GWC:InputControl ID="icMovil" runat="server" Type="Text" Format="Phone" Label="Móvil" />
                                        <GWC:InputControl ID="icCorreoElectronico" runat="server" Type="Text" Label="e-Mail" />

                                    </Columns>
                                </GWC:CatalogControl>

                            </ListControls>

                        </GWC:FieldsetControl>
                        

                        <GWC:FieldsetControl ID="_fssesllos" runat="server" Label="Sellos Digitales">

                            <ListControls>
                                <GWC:FileControl runat="server" CssClass="col-xs-12 col-md-6" ID="icRutaCertificado" Label="Ruta archivo certificado" OnChooseFile="icRutaCertificado_ChooseFile" ShowButtonsTitle="false"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="icContraseniaCertificado" Label="Contraseña" Format="Security" />
                                <GWC:FileControl runat="server" CssClass="col-xs-12 col-md-6" ID="icRutaLlave" Label="Ruta archivo llave"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" ID="icCveWebServices" Label="Clave web services" Format="Security"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6" Type="Text" Format="Calendar" ID="icFechaVigencia" Label="Vigencia"/>
                            </ListControls>

                        </GWC:FieldsetControl>
                        
                        <GWC:FieldsetControl ID="_fsencargo" runat="server" Label="Encargo Conferido Ortorgado">

                            <ListControls>
                                <GWC:CatalogControl CssClass="w-100"  ID="ccEncargosConferidos" runat="server" KeyField="indice" Collapsed="true">
                                    <Columns>
                                        
                                        <GWC:SelectControl ID="scPatenteAduanaEncargo" runat="server" SearchBarEnabled="false" Label="Patente" OnClick="scPatenteAduanaEncargo_Click" />
                                                                                                                        
                                        <GWC:InputControl runat="server" ID="icFechaInicialEncargo"  Type="Text" Format="Calendar" Label="Inicio" />

                                        <GWC:InputControl runat="server" ID="icFechaFinalEncargo" Type="Text" Format="Calendar"  Label="Final" />

                                        <GWC:SwitchControl runat="server" ID="scEncargoActivo" Label="Activo" OnText="Sí" OffText="No"/>   

                                    </Columns>

                                </GWC:CatalogControl>

                            </ListControls>

                        </GWC:FieldsetControl>
                        
                          <GWC:FieldsetControl ID="_fspago" runat="server" Label="Pago electrónico del cliente">

                            <ListControls>
                                <GWC:CatalogControl CssClass="w-100"  ID="ccPagoElectronico" runat="server" KeyField="indice" Collapsed="true">
                                    <Columns>
                                        <GWC:SelectControl ID="scTipoOperacionPago" runat="server" Label="Tipo operación">
                                            <Options>
                                                <GWC:SelectOption Text="Importación" Value="1" />
                                                <GWC:SelectOption Text="Exportación" Value="2" />
                                                <GWC:SelectOption Text="Ambas" Value="3" />
                                            </Options>
                                        </GWC:SelectControl>
                                        
                                        <GWC:SelectControl ID="scPatentePago" runat="server" Label="Patente | Aduana" OnClick="scPatentePago_Click"/>

                                        <GWC:SelectControl ID="scClaveDocumentoPago" runat="server" Dimension="Vt022ClavesPedimentoA02" Label="Clave documento">
                                            <Options>
                                                <GWC:SelectOption Text="A1" Value="1" />
                                                <GWC:SelectOption Text="F1" Value="2" />
                                                <GWC:SelectOption Text="IN" Value="3" />
                                            </Options>
                                        </GWC:SelectControl>
                                        <GWC:SelectControl ID="scBancoPago" runat="server" Dimension="BancosSATTokenDimencion" Label="Clave Banco" >
                                            <Options>
                                                <GWC:SelectOption Text="Santander de México S.A de C.V." Value="1" />
                                                <GWC:SelectOption Text="Banco Nacional de México S.A de C.V." Value="2" />
                                                <GWC:SelectOption Text="BanXico S.A" Value="3" />
                                            </Options>
                                        </GWC:SelectControl>
                                        <GWC:InputControl runat="server" ID="icCuentaPago" Type="Text" Label="No. de cuenta" Format="Card" />
                                        <GWC:InputControl runat="server" ID="icRangoCuentaPago" Type="Text" Label="Rango de pago" />
                                        <GWC:SwitchControl runat="server" ID="scEstatusPago" Label="Activo" OnText="Sí" OffText="No"/>   

                                    </Columns>
                                </GWC:CatalogControl>
                            </ListControls>
                        </GWC:FieldsetControl>

                        <GWC:FieldsetControl ID="_fsexpediente" runat="server" Label="Expediente legal del cliente" Enabled ="false">

                            <ListControls>
                                <GWC:CatalogControl CssClass="w-100" ID="ccExpedienteLegal" runat="server" KeyField="i_Cve_ExpedienteLegal" Collapsed="true"  >
                                    <Columns>
                                        <GWC:SelectControl ID="scPlantilla" runat="server" Label="Plantilla" >
                                            <Options>
                                                <GWC:SelectOption Text="197 - Permiso Sanitario" Value="1" />
                                                <GWC:SelectOption Text="185 - SAGARPA" Value="2" />
                                                <GWC:SelectOption Text="178 - FITOS" Value="3" />
                                            </Options>
                                        </GWC:SelectControl>
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

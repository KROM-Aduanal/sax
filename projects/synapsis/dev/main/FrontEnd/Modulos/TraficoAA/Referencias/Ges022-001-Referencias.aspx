<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-Referencias.aspx.vb" Inherits=".Ges022_001_Referencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Referencia" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"  />

    <% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <style>
        .wc-pillbox .wc-buttonbar{
            display: none;
        }
        .bg_Asistencias input{
            background-color: #f7f2f9;
            color:black;
        }
    </style>
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="Referencias" OnCheckedChanged="MarcarPagina" >
            <Buttonbar runat="server" OnClick="EventosBotonera" >
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                    <GWC:ButtonItem Text="Guías múltiples" ID="btGuiasM"/>
                </DropdownButtons>
            </Buttonbar>   

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>

                        <GWC:CardControl runat="server" ID="ccDocumento" Visible="false" CssClass="col-xs-12 col-md-6 mb-5">
                            <listcontrols>
                                <asp:Label runat="server" CssClass="col-xs-12 col-md-12 margin-bottom font-weight-bold" Text="Arrastra un documento para iniciar" style="text-align:left; color:#432776"/>
                                <GWC:FileControl runat="server" CssClass="col-xs-12 col-md-9" ID="fcDocumento" OnChooseFile="icRutaDocumento_ChooseFile" Dragable="True" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3" ID="scTipoDocumentos" Label="Tipo de documento">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="BL"/>
                                        <GWC:SelectOption Value="2" Text="Factura comercial"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:ButtonControl runat="server" ID="btGuardarDocumento" Label="Agregar" OnClick="btGuardarDocumento_OnClick"/>
                            </listcontrols>
                        </GWC:CardControl> 
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0 align-content-center align-middle text_align-center">
                            <div class="col-xs-12 col-md-6 mt-3 p-0" style="margin-bottom:5px;">
                                 <GWC:CardControl runat="server" ID="ccDespacho" Visible="false" CssClass="col-xs-12 col-md-6 mb-5">
                                     <listcontrols>
                                         <asp:Label runat="server" CssClass="col-xs-12 col-md-12 margin-bottom font-weight-bold" Text="¿Deseas ver tu aviso consolidado con las remesas?" style="text-align:left; color:#432776"/>                                 
                                         <GWC:ButtonControl runat="server" ID="btonIr" Label="Ir" OnClick="btIr_OnClick"/>
                                     </listcontrols>
                                 </GWC:CardControl> 
                            </div>
                        </asp:Panel>
                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="dbcReferencia" Label="Referencia Aduanal" LabelDetail="Pedimento Aduanal" OnClick="dbcReferencia_Click"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5 p-0 d-flex align-items-center jc-center" > 
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-5 p-0" ID="swcTipoOperacion" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true" Rules="required" OnCheckedChanged="swcTipoOperacion_CheckedChanged"></GWC:SwitchControl>                       
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-3 p-0" ID="swcMaterialPeligroso" Label="Material peligroso" OnText="Si" OffText="No"></GWC:SwitchControl>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-4 p-0" ID="swcRectificacion" Label="¿Tiene rectificación?" OnText="Si" OffText="No"  Visible="false"></GWC:SwitchControl>
                        </asp:Panel>
                        
                        <GWC:SelectControl runat="server" CssClass="bg_Asistencias col-xs-12 col-md-3 mb-5" ID="scTipoDocumento" SearchBarEnabled="true" Label="Tipo de pedimento" OnClick ="scTipoDocumento_Click" OnSelectedIndexChanged="scTipoDocumento_SelectedIndexChanged" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="bg_Asistencias col-xs-12 col-md-3 mb-5" ID="scClaveDocumento" Label="Clave pedimento" KeyField ="t_Cve_Pedimento" DisplayField ="t_ClaveDescripcion" Dimension ="Vt022ClavesPedimentoA02" Rules="required" OnClick="scClaveDocumento_Click" OnSelectedIndexChanged="scClaveDocumento_SelectedIndexChanged">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="bg_Asistencias col-xs-12 col-md-3 mb-5" ID="scRegimen" Label="Régimen" KeyField="t_Cve_Regimen" DisplayField="t_DescripcionCorta" Dimension="Vt022RegimenesA16" Enabled="false" Rules="required" OnClick="scRegimen_Click" ToolTip="Sugerencia del sistema, validar por favor" ToolTipModality="Ondemand" ToolTipStatus="OkInfo">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="bg_Asistencias col-xs-12 col-md-3" ID="scTipoDespacho" Label="¿Cómo vas a despachar?" OnClick="scTipoDespacho_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md- mb-5" ID="scPrefijo" Label="Prefijo" OnSelectedIndexChanged="scPrefijo_SelectedIndexChanged" Visible ="false">
                        </GWC:SelectControl>                        
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scPatente" Label="Modalidad | Aduana | Patente" SearchBarEnabled="true" OnClick="scModalidadAduanaPatente_Click"/>                            
                        <GWC:InputControl runat ="server" CssClass ="col-xs-12 col-md-3 mb-5" ID="icPedimentoOriginal" Label ="Pedimento original" Type ="Text" Visible="false"/>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scTipoCarga" Label="Tipo de carga/lote" OnClick="scTipoCarga_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scEjecutivoCuenta" Label="Ejecutivo de cuenta" KeyField ="i_Cve_EjecutivosMisEmpresas" DisplayField ="t_NombreCompleto" Dimension ="EjecutivosMiEmpresa">
                        </GWC:SelectControl>                                                                      
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="icDescripcionCompleta" Label="Descripción mercancia">
                        </GWC:InputControl>
                    </ListControls>
                </GWC:FieldsetControl>                

                <GWC:FieldsetControl runat="server" ID="fscCliente" Label="Cliente">                    

                    <ListControls>
                                      
                        <GWC:FindboxControl runat="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="fbcCliente" Label ="Cliente" KeyField ="_id" DisplayField ="CA_RAZON_SOCIAL" OnTextChanged ="fbcCliente_TextChanged" OnClick ="fbcCliente_Click" Rules="required"/>
                        <GWC:InputControl runat ="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="icRFC" Label ="RFC" Type ="Text" Enabled="false"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icRFCFacturacion" Label="RFC Facturación" Type="Text" Rules="maxlegth[13]" Enabled="false"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icBancoPago" Label="Banco asignado para pago" Type="Text"/> 
                                            
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscTrackingExpo" Label="Tracking" Visible="false">
                    <ListControls runat="server">
                        <asp:Panel runat="server" CssClass="w-100">
                            <ul class="timeline2" >
                                <li class="li complete" margin-right="0px">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Apertura </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Proforma </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Cierre </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Pago </h4>
                                </div>
                                </li>
                                <li class="li">
                                <div class="timestamp">
                                    <span class="date">-</span>
                                </div>
                                <div class="status">
                                    <h4> Despacho </h4>
                                </div>
                                </li>
                            </ul>    
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscTrackingImpo" Label="Tracking" Visible="false">
                    <ListControls runat="server" >
                        <asp:Panel runat="server" CssClass="w-100">
                            <ul class="timeline2" style="align-items:center">
                                <li class="li complete" margin-right="0px">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Apertura </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Entrada </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Proforma </h4>
                                </div>
                                </li>
                                <li class="li complete">
                                <div class="timestamp">
                                    <span class="date">18 Feb</span>
                                </div>
                                <div class="status">
                                    <h4> Pago </h4>
                                </div>
                                </li>
                                <li class="li">
                                <div class="timestamp">
                                    <span class="date">-</span>
                                </div>
                                <div class="status">
                                    <h4> Despacho </h4>
                                </div>
                                </li>
                            </ul>    
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscDocumentos" Label="Documentos" Visible="false">
                    <ListControls>  
                        <asp:Panel runat="server" CssClass="w-100">
                            <div style="width:60%">
                                <GWC:FileControl runat="server" CssClass="col-xs-12 col-md-9" ID="fcDocumentos" OnChooseFile="icRutaDocumentos_ChooseFile" Dragable="True" />
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3" ID="scTipoDocumentosFijo" Label="Tipo de documento">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="BL"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:ButtonControl runat="server" ID="ButtonControl1" Label="Agregar" OnClick="btGuardarDocumentos_OnClick"/>
                            </div>
                        </asp:Panel>

                        <GWC:CatalogControl ID="ccDocumentos" runat="server" KeyField="indice" CssClass="w-100 mt-5 mb-5 p-0" CanAdd="true" CanClone="false">

                            <Columns>

                                <GWC:SelectControl runat="server" ID="icArchivo" Label="Nombre" Enabled="false"/>

                                <GWC:SelectControl runat="server" ID="icTipoArchivo" Label="Tipo" Enabled="false"/>

                            </Columns>

                        </GWC:CatalogControl>

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fscGuia" Label="Guía">
                    <ListControls>     
                        
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scRecintoFiscal" Label="Recinto fiscal">
                            <Options>
                                <GWC:SelectOption Value="1" Text="ICAVE"/>
                                <GWC:SelectOption Value="2" Text="CICE"/>
                                <GWC:SelectOption Value="3" Text="TNG"/>
                            </Options>
                        </GWC:SelectControl>
                        
                         <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-4" ID="scGuias" OnText="Múltiple" OffText="Simple" Checked="false" OnCheckedChanged="swcGuiaMultiple_CheckedChanged"/>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0" ID="pnGuia">               
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="icNumeroGuia" Label="Número de guía">
                                </GWC:InputControl>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mb-5">
                                    <GWC:InputControl runat="server" ID="scTransportista" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" Name="scTransportista" Label="Transportista" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scPais" Label="País" KeyField="i_Cve_Pais" DisplayField="t_Pais" Dimension="Vt022PaisesA04">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icTipoCargaGuia" Label="Tipo de carga"/>
                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-3 mb-5">
                                    <GWC:InputControl runat="server" ID="scPesoBruto" CssClass="col-xs-7 col-md-7 p-0 input-border-right" Type="Text" Name="scPesoBruto" Label="Peso Bruto" />
                                    <GWC:SelectControl runat="server" CssClass="col-xs-5 col-md-5 p-0" ID="scUnidadMedida" SearchBarEnabled="true" LocalSearch="false" Label="Unidad de medida"  OnClick="SeleccionarUnidadMedida_Click">
                                    </GWC:SelectControl>
                                </asp:Panel>
                                <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-3 mb-5" ID="scTipoGuia" Label="Tipo de guía">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="Master"/>
                                        <GWC:SelectOption Value="2" Text="La otra"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-4 mb-5" ID="icConsignatario" Label ="Consignatario" Type ="Text"/>
                                <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-2 mb-5" ID="icFechaSalidaOrigen" Label ="Salida de origen" Type ="Text" Format="Calendar"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-12 solid-textarea" Type="TextArea" ID="icDescripcionMercancia" Label="Descripción de la mercancia">
                                </GWC:InputControl>
                        </asp:Panel>

                        <GWC:CatalogControl ID="ccGuias" runat="server" KeyField="Guias_Multi" CssClass="w-100 mt-5 mb-5 p-0" Visible="false">

                            <Columns>

                                <GWC:InputControl runat="server" ID="icNumeroGuiaMulti" Label="Guía"/>

                                <GWC:InputControl runat="server" ID="scTransportistaMulti" Type="Text" Name="scTransportistaMulti" Label="Transportista" />
                                    
                                <GWC:SelectControl runat="server" ID="scPaisMulti" Label="País"  KeyField="i_Cve_Pais" DisplayField="t_Pais" Dimension="Vt022PaisesA04">
                                </GWC:SelectControl>

                                <GWC:SelectControl runat="server" ID="scTipoGuiaMulti" Label="Tipo guía" Enabled="false">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="Master"/>
                                        <GWC:SelectOption Value="2" Text="La otra"/>
                                    </Options>
                                </GWC:SelectControl>

                                <GWC:InputControl runat="server" ID="icTipoCargaGuiaMulti" Label="Tipo de carga" Enabled="false"/>

                                <GWC:InputControl runat="server" ID="scPesoBrutoMulti" Type="Text" Name="scPesoBrutoMulti" Label="Peso Bruto" />
                                    
                                <GWC:SelectControl runat="server" ID="scUnidadMedidaMulti" SearchBarEnabled="true" LocalSearch="false" Label="Unidad de medida" OnClick="SeleccionarUnidadMedidaMulti_Click">
                                </GWC:SelectControl>
                                
                                <GWC:InputControl runat="server" ID="icFechaSalidaOrigenMulti" Label ="Salida de origen" Type ="Text" Format="Calendar"/>

                            </Columns>

                        </GWC:CatalogControl>
                        
                    </ListControls>
                </GWC:FieldsetControl>



                <GWC:FieldsetControl runat="server" ID="fscFechas" Label="Fechas" Visible="false">
                    <ListControls>                        
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaEta" Label ="ETA (Estimada de arribo)" Type ="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaRevalidacion" Label ="Revalidación" Type ="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaPrevio" Label ="Previo" Type ="Text" Format="Calendar"/>

                        
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaPresentacion" Label ="Presentación" Type ="Text" Format="Calendar" Visible="false"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaSalida" Label ="Salida" Type ="Text" Format="Calendar" Visible="false"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaEtd" Label ="ETD" Type ="Text" Format="Calendar" Visible="false"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaCierreFisico" Label ="Cierre físico" Type ="Text" Format="Calendar" Visible="false"/>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

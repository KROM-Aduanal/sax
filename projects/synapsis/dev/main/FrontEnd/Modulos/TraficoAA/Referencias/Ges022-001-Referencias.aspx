<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-Referencias.aspx.vb" Inherits=".Ges022_001_Referencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Referencia" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

    <% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa" />

    <% End If %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="true" Label="Referencias" OnCheckedChanged="MarcarPagina" >
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                </DropdownButtons>
            </Buttonbar>   

            <Fieldsets>
                <GWC:FieldsetControl runat="server" ID="Generales" Label="Generales">
                    <ListControls>
                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="dbcReferencia" Label="Referencia Aduanal" LabelDetail="Pedimento Aduanal" OnClick="dbcReferencia_Click" />
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icOriginal" Label="Original" Type="Text" Visible="false" />
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-4 mb-5 " ID="swcTipoOperacion" Label="Tipo de operación" OnText="IMP" OffText="EXP" Checked="true" Rules="required" OnCheckedChanged="swcTipoOperacion_CheckedChanged"></GWC:SwitchControl>                        
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-4 mb-5 " ID="swcMaterialPeligroso" Label="Material peligroso" OnText="Si" OffText="No" Checked="false"></GWC:SwitchControl>
                            <GWC:SwitchControl runat="server" CssClass="col-xs-12 col-md-4 mb-5 " ID="swcRectificacion" Label="Rectificación" OnText="Si" OffText="No" Checked="false" OnCheckedChanged="swcRectificacion_CheckedChanged"></GWC:SwitchControl>
                        </asp:Panel>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scTipoReferencia" Label="Tipo de referencia" OnSelectedIndexChanged ="scTipoReferencia_SelectedIndexChanged">
                            <Options>
                                <GWC:SelectOption Text="Operativa" Value="1" />
                                <GWC:SelectOption Text="Corresponsalía" Value="2" />
                                <GWC:SelectOption Text="Corresponsalía terceros" Value="2" />
                            </Options>
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPrefijo" Label="Prefijo" OnSelectedIndexChanged="scPrefijo_SelectedIndexChanged" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scTipoCarga" Label="Tipo de carga/lote" OnClick="scTipoCarga_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scPatente" Label="Patente" SearchBarEnabled="false" OnClick="scPatente_Click">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scRegimen" Label="Régimen" KeyField="t_Cve_Regimen" DisplayField="t_DescripcionCorta" Dimension="Vt022RegimenesA16" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scTipoDocumento" SearchBarEnabled="true" Label="Tipo de documento" OnClick ="scTipoDocumento_Click" OnSelectedIndexChanged="scTipoDocumento_SelectedIndexChanged" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scClaveDocumento" Label="Clave de documento" KeyField ="i_Cve_ClavePedimento" DisplayField ="t_Cve_Pedimento" Dimension ="Vt022ClavesPedimentoA02" Rules="required">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scAduanaEntradaSalida" Label="Aduana de entrada/salida" KeyField ="i_Cve_AduanaSeccion" DisplayField ="t_AduanaSeccionDenominacion" Dimension ="Vt022AduanaSeccionA01">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scAduanaDespacho" Label="Aduana de despacho" SearchBarEnabled ="false" Onclick="scAduanaDespacho_Click">
                        </GWC:SelectControl>
                         <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scDestinoMercancia" Label="Destino de la mercancia" KeyField ="i_Cve_DestinoMercancia" DisplayField ="t_DescripcionDestinoMercancia" Dimension ="Vt022DestinosMercanciasA15">
                        </GWC:SelectControl>
                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="scEjecutivoCuenta" Label="Ejecutivo de cuenta" KeyField ="i_Cve_EjecutivosMisEmpresas" DisplayField ="t_NombreCompleto" Dimension ="EjecutivosMiEmpresa">
                        </GWC:SelectControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Cliente" Label="Cliente">
                    <ListControls>
                        <GWC:FindboxControl runat="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="fbcCliente" Label ="Cliente" KeyField ="_id" DisplayField ="CA_RAZON_SOCIAL" OnTextChanged ="fbcCliente_TextChanged" OnClick ="fbcCliente_Click" Rules="required"/>
                        <GWC:InputControl runat ="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="icRFC" Label ="RFC" Type ="Text"/>
                        <GWC:InputControl runat ="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="icCURP" Label ="CURP" Type ="Text"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icRFCFacturacion" Label="RFC Facturación" Type="Text" Rules="maxlegth[13]"/>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="icBancoPago" Label="Banco asignado para pago" Type="Text"/> 
                        <GWC:CatalogControl runat ="server" CssClass ="w-100" ID="ccCDetallesliente" KeyField ="indice">
                            <Columns>
                                <GWC:SelectControl runat="server" ID="scTipoDato" Label="Tipo">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="PO"/>
                                        <GWC:SelectOption Value="2" Text="Legajo"/>
                                        <GWC:SelectOption Value="3" Text="Factura"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" ID="icNumero" Label="Número" Type="Text"/>
                            </Columns>
                        </GWC:CatalogControl>                      
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="Seguimiento" Label="Seguimiento" Enabled="false" >
                    <ListControls>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                            <GWC:SwitchControl runat ="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="swcEntrada" Label ="Entrada" OnText ="Si" OffText ="No" />
                            <GWC:SwitchControl runat ="server" CssClass ="col-xs-12 col-md-6 mb-5" ID="scPagoAnticipado" Label ="Pago anticipado" OnText ="Si" OffText ="No" />
                        </asp:Panel>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaETA" Label ="ETA(Estimada de arribo)" Type ="Text" Format="Calendar" />
                        <GWC:InputControl runat="server" ID="icFechaETD" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha ETD" Type="Text" Format="Calendar" Visible="false"/>
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaATA" Label ="ATA" Type ="Text" Format="Calendar" />
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaRevalidacion" Label ="Fecha de Revalidacion" Type ="Text" Format="Calendar" />
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaRegistro" Label ="Fecha de Registro" Type ="Text" Format="Calendar" />
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaPagoPedimento" Label ="Fecha de Pago de pedimento" Type ="Text" Format="Calendar" />
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaDespacho" Label ="Fecha de Despacho" Type ="Text" Format="Calendar" />
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="DatosAdicionales" Label="Datos Adicionales">
                    <ListControls>                        
                        <GWC:InputControl runat="server" cssclass="col-xs-12 col-md-6 mb-5" ID="icFechaRecepcionDocumentos" Label ="Fecha de recepcion de documentos" Type ="Text" Format="Calendar"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 mb-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-8 col-md-8 p-0 input-border-right" ID="icFechaPrevio" Label ="Fecha de Previo" Type ="Text" Format="Calendar"/>
                            <GWC:SelectControl runat="server" CssClass="col-xs-4 col-md-4 p-0" ID="scTipoPrevio" Label="Tipo de previo">
                                <Options>
                                    <GWC:SelectOption Text="En origen" Value="1" />
                                    <GWC:SelectOption Text="En puerto destino" Value="2" />
                                    <GWC:SelectOption Text="Corresponsalía terceros" Value="2" />
                                </Options>
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

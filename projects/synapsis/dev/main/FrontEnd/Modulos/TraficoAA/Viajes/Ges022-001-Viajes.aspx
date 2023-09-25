<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-Viajes.aspx.vb" Inherits=".Ges022_001_Viajes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">
<% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Viaje" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>

<% End If %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Viajes" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                </DropdownButtons>
            </Buttonbar>   
            <Fieldsets>
                <GWC:FieldsetControl runat="server" Label="Datos Generales" ID="fs_DatosGenerales">
                    <ListControls>
                        <GWC:SelectControl runat="server" ID="sl_TipoTransporte" CssClass="col-xs-12 col-md-4 mb-5" Label="Tipo de transporte">
                            <Options>
                                <GWC:SelectOption Value="1" Text="Marítimo"/>
                                <GWC:SelectOption Value="2" Text="Aéreo"/>
                                <GWC:SelectOption Value="3" Text="Terrestre"/>
                            </Options>
                        </GWC:SelectControl>
                        <GWC:SwitchControl runat="server" ID="sw_Operacion" CssClass="col-xs-12 col-md-2 mb-5 d-flex justify-content-center" Label="Operación" OffText="Importación" OnText="Exportación" OnCheckedChanged="sw_Operacion_CheckedChanged"/>
                        <GWC:FindboxControl runat="server" ID="fbx_NaveBuqueOtros" CssClass="col-xs-12 col-md-6 mb-5" Label="Nave/Buque/Otros" Type="Text" OnTextChanged="fbx_NaveBuqueOtros_TextChanged"/>
                        <GWC:FindboxControl runat="server" ID="fbx_NavieraAereolineaOtros" CssClass="col-xs-12 col-md-6 mb-5" Label="Naviera/Aereolinea/Otros" Type="Text" OnTextChanged="fbx_NavieraAereolineaOtros_TextChanged"/>
                        <GWC:FindboxControl runat="server" ID="fbx_ReexpedidoraForwarding" CssClass="col-xs-12 col-md-6 mb-5" Label="Reexpedidora/Forwarding" Type="Text" OnTextChanged="fbx_ReexpedidoraForwarding_TextChanged"/>
                        <GWC:InputControl runat="server" ID="txt_FolioCapitania" CssClass="col-xs-12 col-md-6 mb-5" Label="Folio de capitanía" Type="Text" Format="Numeric"/>
                        <GWC:InputControl runat="server" ID="txt_NumeroViaje" CssClass="col-xs-12 col-md-6 mb-5" Label="Número de viaje" Type="Text"/>
                    </ListControls>
                </GWC:FieldsetControl>
                
                <GWC:FieldsetControl runat="server" Label="Datos operativos" ID="fs_DatosOperativos">
                    <ListControls>
                        <GWC:FindboxControl runat="server" ID="fbx_PuertoExtrangero" CssClass="col-xs-12 col-md-6 mb-5" Label="Puerto extranjero" Type="Text" OnTextChanged="fbx_PuertoExtrangero_TextChanged"/>
                        <GWC:InputControl runat="server" ID="txt_FechaSalidaOrigen" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha de salida origen" Type="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" ID="txt_FechaETA" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha ETA" Type="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" ID="txt_FechaETD" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha ETD" Type="Text" Format="Calendar" Visible="false"/>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" Label="Datos adicionales" ID="fs_DatosAdicionales">
                    <ListControls>
                        <GWC:InputControl runat="server" ID="txt_FechaFondeo" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha de fondeo" Type="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" ID="txt_FechaAtraque" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha de atraque" Type="Text" Format="Calendar"/>
                        <GWC:InputControl runat="server" ID="txt_FechaCierreDocumentos" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha cierre documentos" Type="Text" Format="Calendar" Visible="false"/>
                        <GWC:InputControl runat="server" ID="txt_FechaPresentacion" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha de presentación" Type="Text" Format="Calendar" Visible="false"/>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" Label="Referencias" ID="fs_Referencias">
                    <ListControls>
                        <GWC:ListboxControl runat="server" ID="lbx_Refencias" CssClass="w-100" OnTextChanged="lbx_Refencias_TextChanged"/>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_PlantillaSynapsis.aspx.vb" Inherits=".Ges022_001_Plantilla" %>
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
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Persona" OnCheckedChanged="MarcarPagina">
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
                        <GWC:InputControl runat="server" ID="txt_name" CssClass="col-xs-12 col-md-6 mb-5" Label="Nombre(s)" Type="Text"/>
                        <GWC:InputControl runat="server" ID="txt_lastname" CssClass="col-xs-12 col-md-6 mb-5" Label="Apellido(s)" Type="Text"/>
                        <GWC:SelectControl runat="server" ID="sl_gender" CssClass="col-xs-12 col-md-6 mb-5" Label="Género">
                            <Options>
                                <GWC:SelectOption Value="1" Text="Hombre"/>
                                <GWC:SelectOption Value="2" Text="Mujer"/>
                                <GWC:SelectOption Value="3" Text="Indefinido"/>
                            </Options>
                        </GWC:SelectControl>
                        <GWC:InputControl runat="server" ID="txt_birthday" CssClass="col-xs-12 col-md-6 mb-5" Label="Fecha de Nacimiento" Type="Text" Format="Calendar"/>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" Label="Datos Adicionales" ID="fs_DatosAdicionales">
                    <ListControls>
                        <GWC:FieldsetControl runat="server" Priority="false" Label="Datos Academicos">
                            <ListControls>
                                <GWC:CatalogControl runat="server" ID="cat_schools" CssClass="w-100" KeyField="indice">
                                    <Columns>
                                        <GWC:InputControl runat="server" ID="txt_school" Label="Nombre Escuela"/>
                                        <GWC:SelectControl runat="server" ID="sl_type" Label="Grado">
                                            <Options>
                                                <GWC:SelectOption Value="1" Text="Kinder"/>
                                                <GWC:SelectOption Value="2" Text="Primaria"/>
                                                <GWC:SelectOption Value="3" Text="Secundaria"/>
                                                <GWC:SelectOption Value="4" Text="Bachillerato"/>
                                                <GWC:SelectOption Value="5" Text="Universidad"/>
                                                <GWC:SelectOption Value="6" Text="Maestria"/>
                                                <GWC:SelectOption Value="7" Text="Doctorado"/>
                                            </Options>
                                        </GWC:SelectControl>
                                        <GWC:InputControl runat="server" ID="txt_st_date" Label="Fecha de Inicio" Type="Text" Format="Calendar"/>
                                        <GWC:InputControl runat="server" ID="txt_ed_date" Label="Fecha de Culminación" Type="Text" Format="Calendar"/>
                                    </Columns>
                                </GWC:CatalogControl>
                            </ListControls>
                        </GWC:FieldsetControl>
                        <GWC:FieldsetControl runat="server" Priority="false" Label="Datos de Contácto">
                            <ListControls>
                                <GWC:InputControl runat="server" ID="txt_email" CssClass="col-xs-12 col-md-6 mb-5" Label="Correo Electrónico" Type="Text"/>
                                <GWC:InputControl runat="server" ID="txt_phone" CssClass="col-xs-12 col-md-6 mb-5" Label="Teléfono" Type="Text" Format="Phone"/>
                            </ListControls>
                        </GWC:FieldsetControl>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

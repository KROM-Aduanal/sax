<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-Revalidacion.aspx.vb" Inherits=".Ges022_001_Revalidacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">
<% If IsPopup = False Then %>

    <GWC:FindbarControl Label="Buscar Revalidación" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>

<% End If %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>

    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

<% End If %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Revalidación"  OnCheckedChanged="MarcarPagina">
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
                        <GWC:FindboxControl runat="server" ID="fbx_Referencia" CssClass="col-xs-12 col-md-6 mb-5" Label="Referencia" OnTextChanged="fbx_Referencia_TextChanged" OnClick="fbx_Referencia_Click"/>
                        <%-- lectura --%>
                        <GWC:InputControl runat="server" ID="txt_Cliente" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Label="Cliente"/>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" Label="Datos revalidación" ID="fs_DatosRevalidacion">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="w-100">
                            <GWC:InputControl runat="server" ID="txt_GuiaMaster" CssClass="col-xs-12 col-md-6 mb-5" Label="No. guia master"/>

                            <GWC:SwitchControl runat="server" ID="sw_Revalidado" CssClass="col-xs-12 col-md-2 mb-5 d-flex justify-content-center" Label="Revalidado" OffText="No" OnText="Si" OnCheckedChanged="sw_Revalidado_CheckedChanged"/>
                            <GWC:InputControl runat="server" ID="txt_FechaRevalidacion" CssClass="col-xs-12 col-md-4 mb-5" Type="Text" Format="Calendar" Label="Fecha revalidación" Visible="false"/>

                        </asp:Panel>
                        
                        <GWC:SelectControl runat="server" ID="sl_TipoCarga" CssClass="col-xs-12 col-md-6 mb-5" Label="Tipo de carga" OnSelectedIndexChanged="sl_TipoCarga_SelectedIndexChanged">
                            <Options>
                                <GWC:SelectOption Value="1" Text="Carga suelta"/>
                                <GWC:SelectOption Value="2" Text="Contenerizada"/>
                                <GWC:SelectOption Value="3" Text="Lote"/>
                                <GWC:SelectOption Value="4" Text="Protegida"/>
                            </Options>
                        </GWC:SelectControl>
                       <GWC:FileControl runat="server" Label="BL revalidado" ID="fl_BLRevalidado" CssClass="col-xs-12 col-md-6 mb-5"/> 
                        <GWC:CatalogControl runat="server" CssClass="w-100" ID="cat_CargaSuelta" KeyField="indice" Visible="false">
                            <Columns>
                                <%--<GWC:SelectControl runat="server" ID="sl_ClaseCarga-" Label="Clase"></GWC:SelectControl>--%>
                                <%--<GWC:InputControl runat="server" ID="sl_ClaseCarga" Label="Clase" Type="Text"/> --%>
                                <GWC:SelectControl runat="server" ID="sltest" Label="Embalaje">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="Cuñetes"/>
                                        <GWC:SelectOption Value="2" Text="Bultos"/>
                                        <GWC:SelectOption Value="3" Text="Cajas"/>
                                        <GWC:SelectOption Value="4" Text="Huacales"/>
                                        <GWC:SelectOption Value="5" Text="Paleta"/>
                                        <GWC:SelectOption Value="6" Text="Rollos"/>
                                        <GWC:SelectOption Value="7" Text="Units"/>
                                        <GWC:SelectOption Value="8" Text="Lote"/>
                                        <GWC:SelectOption Value="9" Text="Cartones"/>
                                        <GWC:SelectOption Value="10" Text="Sacos"/>
                                        <GWC:SelectOption Value="11" Text="Unidades"/>
                                        <GWC:SelectOption Value="12" Text="Paquetes"/>
                                        <GWC:SelectOption Value="13" Text="Tambores"/>
                                        <GWC:SelectOption Value="14" Text="Otros"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" ID="txt_CantidadCarga" Label="Cantidad" Type="Text" Format="Numeric"/>
                                <GWC:InputControl runat="server" ID="txt_PesoCarga" Label="Peso" Type="Text" Format="Real"/>
                            </Columns>
                        </GWC:CatalogControl>

                        <GWC:CatalogControl runat="server" CssClass="w-100" ID="cat_Contenedores" KeyField="indice" Visible="false">
                            <Columns>
                                <GWC:InputControl runat="server" ID="txt_NumeroContenedor" Label="Número del contenedor" Type="Text"/>
                                <GWC:SelectControl runat="server" ID="sl_TamanoContenedor" Label="Tamaño del contenedor">
                                    <Options>
                                        <GWC:SelectOption Value="1" Text="Tanque 20 pies"/>
                                        <GWC:SelectOption Value="2" Text="Plataforma de 48 pies"/>
                                        <GWC:SelectOption Value="3" Text="Plataforma de 45 pies"/>
                                        <GWC:SelectOption Value="4" Text="Plataforma de 28 pies"/>
                                        <GWC:SelectOption Value="5" Text="Flat 40 pies"/>
                                        <GWC:SelectOption Value="6" Text="Flat 20 pies"/>
                                        <GWC:SelectOption Value="7" Text="Contenedor estandar de cubo alto 40 pies"/>
                                        <GWC:SelectOption Value="8" Text="Contenedor tapa dura 20 pies"/>
                                        <GWC:SelectOption Value="9" Text="Contenedor tapa dura 40 pies"/>
                                    </Options>
                                </GWC:SelectControl>
                                <GWC:InputControl runat="server" ID="txt_PesoContenedor" Label="Peso" Type="Text" Format="Real"/>
                            </Columns>
                        </GWC:CatalogControl>
                        <asp:Panel runat="server">
                            <br /><br /><br /><br /><br /><br />  
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

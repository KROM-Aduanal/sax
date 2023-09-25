<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="BusquedaGeneral.aspx.vb" Inherits=".BusquedaGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="search-header-bar">
        <div class="container">
            <div class="row align-items-end">
                <div class="col d-flex align-items-center pr-4">
                    <div class="col-auto p-4">
                        <img src="/FrontEnd/Librerias/Krom/imgs/find.png" class="header-image"/>
                    </div>
                    <div class="col p-4">
                        <div class="row no-gutters">
                            <h2 class="header-title">
                                Operaciones Aduanales
                                <small>1 a 20 de 8,024 operaciones | <a href="">Guardar búsqueda</a></small>
                            </h2>
                        </div>
                        <div class="row align-items-center no-gutters">
                            <div class="col-auto text-white pr-4">
                                <b>Buscar</b>
                            </div>
                            <div class="col">
                               <GWC:FindbarControl ID="fbar" runat="server" Label="e.g. RKU21-0005896, container MCU3452345"></GWC:FindbarControl>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-auto pl-4">
                   <div class="row align-items-center no-gutters">
                        <div class="col text-white trasluce pr-4">
                            Ver despachables
                        </div>
                        <div class="col-auto">
                           <GWC:SwitchControl ID="sw" CssClass="w-100" runat="server" OnText="SI" OffText="NO"/>
                        </div>
                    </div>
                    <div class="row no-gutters">
                        <div class="col-auto text-white pr-4" style="margin-top:6px;">
                            <b>Ordenar</b>
                        </div>
                        <div class="col bordered-select">
                           <GWC:SelectControl ID="slt" CssClass="w-100" runat="server" Label="Pedimento"></GWC:SelectControl>
                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container p-4">
        <div class="row">
            <div class="col-auto pr-4">
                
                <GWC:FilterbarControl runat="server" Label="Refina tu búsqueda">
                    <Filters>
                        <GWC:FilterItem runat="server" Text="Producto/Parte">
                            <Items>
                                <GWC:FilterOption Text="MÁQUINA DE VENOCLISIS" Badge="6412"/>
                                <GWC:FilterOption Text="JERINGAS DESECLABLES R4" Badge="512"/>
                                <GWC:FilterOption Text="MÁQUINA MEZCLADORA" Badge="961"/>
                                <GWC:FilterOption Text="BENDAS ESTÁNDAR" Badge="56"/>
                            </Items>
                         </GWC:FilterItem>
                        <GWC:FilterItem runat="server" Id="pruebarara" Text="Valor aduana" Dimension="Vt022MediosTransporteA03" KeyField="i_Cve_MedioTransporte" DisplayField="t_MedioTransporte"/>
                        <GWC:FilterItem runat="server" Text="Aduana mexicana"/>
                        <GWC:FilterItem runat="server" Text="Pais de origen"/>
                        <GWC:FilterItem runat="server" Text="Fecha de pago"/>
                        <GWC:FilterItem runat="server" Text="Fecha de despacho"/>
                        <GWC:FilterItem runat="server" Text="Proveedor"/>
                        <GWC:FilterItem runat="server" Text="Buque"/>
                        <GWC:FilterItem runat="server" Text="Número de GUIA/BL"/>
                    </Filters>
                </GWC:FilterbarControl>
                
            </div>
            <div class="col pl-2">
                
                <GWC:CollectionViewControl 
                    runat="server" 
                    Template="http://10.66.1.15:8085/FrontEnd/Modulos/TraficoAA/BusquedaGeneral/TemplateArticulo.aspx" 
                    Dimension="Vt022MediosTransporteA03" 
                    PageItems="50"
                    ID="transportes"/>

            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="footer" runat="server">
</asp:Content>


<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-AcuseValor.aspx.vb" Inherits=".Ges022_001_AcuseValor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">
<% If IsPopup = False Then %>
    <GWC:FindbarControl Label="Buscar Acuse de Valor" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral"/>
<% End If %>

    <style>

        .cl_Secciones {
            opacity: .6;
            color: #757575;      
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

        .sc__Subdivision {
            display: block !important;
            border:1px solid red !important;
            
            
        }

        .customsizetextarea {
           height: 2.4em;
        }
        .ALGODON{

        }

        @media(min-width:400px){
             .ALGODON{
                                  justify-content: flex-start;
                                             
              }

        }
        @media(min-width: 576px){
             .ALGODON{
                                  justify-content: flex-start;
                                            
              }

        }


        @media(min-width:760px){
              .ALGODON{
                  justify-content: flex-start;
                            
             }

        }

        @media(min-width:992px){
              .ALGODON{
                  justify-content: flex-start;
                             
                margin-left:20px;                     
 
              }

        }

                @media(min-width:1200px){
              .ALGODON{
                  justify-content: flex-start;
                             
                margin-left:20px;                     
              }

        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>
    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>
<% End If %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">
    <div class="d-flex">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Acuse de Valor" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Adendar"/>
                    <GWC:ButtonItem Text="Limpiar"/>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                </DropdownButtons>
            </Buttonbar>   
            <Fieldsets>
               <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>

                        <GWC:DualityBarControl runat="server" CssClass="col-xs-12 col-md-6" ID="dbc_NumFacturaAcuseValor" Label="Folio del Documento" LabelDetail="Acuse de Valor" OnClick="dbc_NumFacturaAcuseValor_Click"/>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 px-0 mt-2 py-5">
                            <GWC:SwitchControl runat="server" ID="swc_TipoOperacion" CssClass="col-xs-6 col-md-3 mb-5 p-0  d-flex ALGODON" Label="Tipo de operación" OnText="Importación" OffText="Exportación" Checked="true"  />
                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-5 mt-2 mb-5 p-0 d-flex justify-content-end" ID="sc_TipoDocumento" Label="Tipo de documento"  SearchBarEnabled="false" LocalSearch="false" Rules="required"  >
                                  <Options >
                                         <GWC:SelectOption Value="1" Text="Factura"/>
                                         <GWC:SelectOption Value="3" Text="Carta Factura"/>
                                  </Options>
                            </GWC:SelectControl>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                            <GWC:SelectControl runat="server" ID="sc_TipoMoneda" CssClass="col-xs-12 col-md-3 mt-2 mb-5"  Label="Tipo de Moneda"  SearchBarEnabled="true" LocalSearch="false" Rules="required"  OnTextChanged="sc_TipoMoneda_TextChanged" OnClick="sc_TipoMoneda_Click" OnSelectedIndexChanged="sc_TipoMoneda_SelectedIndesxChanged">

                            </GWC:SelectControl> 
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-2 mb-5" ID="icFechaExpedicion" Rules="require" Type="Text" Format="Calendar" Name="icFechaExpedicion" Label="Expedición Documento" />
                            <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 nopadding">
                                <GWC:SwitchControl runat="server" ID="swc_Subdivision" CssClass="col-xs-6 col-md-3  d-flex justify-content-end" Label="Subdivisión" OnText="Sí" OffText="No" Checked="false" />
                                <GWC:SwitchControl runat="server" ID="swc_RelacionFactura" CssClass="col-xs-6 col-md-5  d-flex justify-content-end " Label="Relación de Facturas" OnText="Sí" OffText="No" Checked="false" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mt-4">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5 solid-textarea p-0 justify-content-start" ID="ic_Observaciones" Type="TextArea" Format="SinDefinir" Name="ic_Observaciones" Label="Observaciones" />
                            <GWC:SwitchControl runat="server" ID="swc_CertificadoOrigen" CssClass="col-xs-12 col-md-2 mt-5 mb-5 p-0 d-flex  justify-content-center  " Label="Certificado Origen" OnText="Sí" OffText="No" Checked="false" OnCheckedChanged="swc_CertificadoOrigen_CheckedChanged" />
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5 mb-5 p-0 d-flex justify-content-end" ID="ic_ExpotadorAutorizado" Type="Text" Format="SinDefinir" Name="icExpotadorAutorizado" Label="Exportador Autorizado" Visible="False" />
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mt-4">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-3 mt-5 mb-5 p-0 d-flex justify-content-end" ID="ic_edocument" Type="Text" Format="SinDefinir" Name="ic_edocument" Label="Adenda" Visible="False" />
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" Label="Proveedor" ID="fs_Proveedor">
                    <ListControls>
                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                         
                                <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5" Priority="false" Label="Razón Social" ID="fbc_Proveedor" OnClick="fbc_Proveedor_Click" OnTextChanged="fbc_Proveedor_TextChanged"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-8 solid-textarea customsizetextarea" ID="ic_DireccionProveedor" Type="TextArea" Format="SinDefinir" Name="ic_DireccionProveedor" Label="Dirección Fiscal" />
                             
                            
                        </asp:Panel>
                        <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5" ID="ic_IDFiscalProveedor" Type="Text" Format="SinDefinir" Name="ic_IDFiscalProveedor" Label="IDFiscal/TaxNumber/RFC/CURP" />
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" Label="Destinatario" ID="fs_Destinatario">
                    <ListControls>
                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                         
                                <GWC:FindboxControl runat="server" ID="fbc_Destinatario" CssClass="col-xs-12 col-md-6 mt-2 mb-5" Priority="false" Label="Razón Social" OnClick="fbc_Proveedor_Click" OnTextChanged="fbc_Destinatario_TextChanged"/>
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-9 solid-textarea" ID="ic_DireccionDestinatario" Type="TextArea" Format="SinDefinir" Name="ic_DireccionDestinatario" Label="Dirección Fiscal" />
                             
                                <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5" ID="ic_IDFiscalDestinatario" Type="Text" Format="SinDefinir" Name="ic_IDFiscalDestinatario" Label="IDFiscal/TaxNumber/RFC/CURP" />
                            
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl  runat="server" Label="Partidas" ID="fs_partidas">
                      <ListControls>
                        <GWC:PillboxControl runat="server"  ID="pb_PartidasAcuseValor" KeyField="indice" CssClass="col-xs-12" OnCheckedChange="pb_PartidasAcuseValor_CheckedChange" OnClick="pb_PartidasAcuseValor_Click" >
                           <ListControls>
                               <asp:Panel runat="server" CssClass="col-md-1 d-flex align-items-center flex-column margin-bottom">
                                        <asp:Label runat="server" ID="lbTarjeta" Text="No." class="cl_Tarjeta col-xs-12 col-md-1"></asp:Label>
                                        <asp:Label runat="server" ID="lbNumeroAcuseValor" class="cl_Num__Tarjeta col-xs-12 col-md-1" Text="0"></asp:Label>
                               </asp:Panel>                           
                               <asp:Panel runat="server" CssClass="d-flex align-items-center">
                                        <asp:Label runat="server" ID="lbFactura" Text="Partida - Factura" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                               </asp:Panel>
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">
                                               <div class="col-xs-12 col-md-6 mt-3 p-0">
                                                    <GWC:InputControl runat="server" ID="ic_DescripcionAcuseValor" CssClass="col-xs-12 col-md-12 mt-3 mb-5 solid-textarea" Type="TextArea" Name="ic_DescripcionAcuseValor" Label="Descripción A.V." />
                                               </div>
                                               <div class="col-xs-12 col-md-6 mt-3 p-0">
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">
                                                          <GWC:InputControl runat="server" ID="ic_CantidadAcuseValor" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Format="Real" Name="ic_CantidadAcuseValor" Label="Cantidad" OnTextChanged="ic_CantidadAcuseValor_TextChanged" />
                                                          <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5" ID="sc_UnidadAcuseValor" Name="sc_UnidadAcuseValor" SearchBarEnabled="true" LocalSearch="false" Label="Unidad A.V." OnTextChanged="sc_UnidadAcuseValor_TextChanged" OnClick="sc_UnidadAcuseValor_Click">

                                                          </GWC:SelectControl> 
                                                    </asp:Panel>
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">
                                                            <GWC:InputControl runat="server" ID="ic_PrecioUnitarioAcuseValor" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="ic_PrecioUnitarioAcuseValor" Label="Precio unitario"  />
                                                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 " ID="sc_MonedaPrecioUnitarioPartida"  Name="sc_MonedaPrecioUnitarioPartida" SearchBarEnabled="true" LocalSearch="false" Label="Tipo de Moneda" OnTextChanged="sc_TipoMoneda_TextChanged">

                                                            </GWC:SelectControl>                                             
                                                    </asp:Panel>  
                                                    <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 mb-5">
                                                              <GWC:InputControl runat="server" ID="ic_ValorFacturaPartida" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="ic_ValorFacturaPartida" Label="Total" />
                                                              <GWC:InputControl runat="server" ID="ic_ValorDolaresPartida" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="ic_ValorDolaresPartida" Label="Total Dólares" />
                                                    </asp:Panel>
                                               </div>
                                         </asp:Panel>
                               </asp:Panel>                                               
                               <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
                                    <asp:Panel runat="server" class="col-xs-12 mt-3 p-0">
                                        <asp:Label runat="server" ID="lbMercancia" Text="Partida - Detalle mercancía" Visible="True" CssClass="w-100 cl_Secciones"></asp:Label>
                                    </asp:Panel>     
                                    <div class="col-xs-12 mt-3 p-0">
                                        <GWC:InputControl runat="server" ID="ic_MarcaAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="ic_MarcaAcuseValor" Label="Marca" />
                                        <GWC:InputControl runat="server" ID="ic_ModeloAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="ic_ModeloAcuseValor" Label="Modelo" />
                                        <GWC:InputControl runat="server" ID="ic_SubmodeloAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="ic_SubmodeloAcuseValor" Label="Submodelo" />
                                        <GWC:InputControl runat="server" ID="ic_NumeroSerieAcuseValor" CssClass="col-xs-12 col-md-3 mb-5" Type="Text" Name="ic_NumeroSerieAcuseValor" Label="Número de serie" />
                                    </div> 
                                </asp:Panel>
                           </ListControls>
                         </GWC:PillboxControl>
                    </ListControls>
                </GWC:FieldsetControl>

                <GWC:FieldsetControl runat="server" ID="fs_Configuracion" Label="Configuración">
                    <ListControls>

                                <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 p-0">
 
                                    <div class="col-xs-12 mt-3 p-0">
                                        <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-6 mb-5 " ID="sc_SelloCliente" SearchBarEnabled="true" LocalSearch="false" Label="Sello" />
                                        <GWC:InputControl runat="server" ID="ic_RFCSConsulta" CssClass="col-xs-12 col-md-6 mb-5 solid-textarea" Type="TextArea" Name="ic_RFCSConsulta" Label="RFC'S Consulta" />
                                        <GWC:InputControl runat="server" ID="ic_PatenteAduanal" CssClass="col-xs-12 col-md-6 mb-5" Type="Text" Name="ic_PatenteAduanal" Label="Patente" />
                                    </div> 
                                </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

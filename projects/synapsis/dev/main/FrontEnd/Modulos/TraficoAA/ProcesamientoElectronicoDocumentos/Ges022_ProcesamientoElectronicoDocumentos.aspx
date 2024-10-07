<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022_ProcesamientoElectronicoDocumentos.aspx.vb" Inherits=".Ges022_ProcesamientoElectronicoDocumentos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

    <% If IsPopup = False Then %>

      <GWC:FindbarControl Label="Buscar documento" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="BusquedaGeneral" />

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
            color: #fff;
            display: flex;        
            border-radius: 50%;           
            justify-content: center;            
            align-items: center;
            width: 50px;
            height: 50px;
            box-shadow: -1px 3px 10px 4px rgba(0, 0, 0, 0.2);
        }

        .estado_completado{
           background-color: #79145c;
        }

        .estado_completado2{

             background-color: #009688;
        }

         .estado_detalles{
           background-color: rgb(67,39,118);
         }

        .estado_borrador{
            background-color: #757575;
            opacity: 0.6;
        }

        .wc-file-dragable {
	        width: 100%;
	        position: relative;
	        background-color: #f9f9f9;
	     /*   border: 3px dotted #cecdcd !important;*/
	        transition:.5s all;
            border-radius: 8px 8px;
        }
        .btn-procesamiento{
            color:#ffffff !important; 
            border-radius: 22px 22px; 
            opacity:0.9;
            box-shadow: 0px 1px 2px 1px rgba(0, 0, 0, 0.2);
        }
        .btn-ia{
            background-color:#009688; 
        }

        .btn-xml{
             background-color:#673ab7; 
        }

        .btn-procesamiento:hover {
            opacity: 1;
        }

        
    </style>
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="ContentCompanyList" runat="server">
    <% If IsPopup = False Then %>

        <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

    <% End If %>
</asp:Content>

<asp:Content runat="server" ID="Content4" ContentPlaceHolderID="contentBody">
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" Label="<span style='color:#321761'>Procesamiento electrónico de</span><span style='color:#782360;'>&nbsp;documentos</span>" HasAutoSave="false">

                <Buttonbar runat="server" OnClick="EventosBotonera">
                    <DropdownButtons>
                        <GWC:ButtonItem Text="Descargar" />
                        <GWC:ButtonItem Text="Imprimir" />
                        <GWC:ButtonItem Text="Mandar por Correo" />
                    </DropdownButtons>
                </Buttonbar>

             <Fieldsets>
                <GWC:FieldsetControl ID="fsDatosGenerales" runat="server" Label="Datos generales" Priority="true" CssClass="">
                    <ListControls> 
                             <asp:Panel runat="server" CssClass="col-12 col-md-1 col-lg-1 d-flex align-items-center flex-column margin-bottom"  ID="lbEstadoIntegracionCompleto" Visible="true" >
                                 <asp:Label runat="server" ID="lbEstadoCompletado" ToolTip="Completado" class="cl_Num__Tarjeta estado_completado col-xs-12 col-md-12 col-lg-12" Text='<svg xmlns="http://www.w3.org/2000/svg" height="52" width="60" viewBox="0 0 640 512"><path fill="#fafcff" d="M320 0c17.7 0 32 14.3 32 32l0 64 120 0c39.8 0 72 32.2 72 72l0 272c0 39.8-32.2 72-72 72l-304 0c-39.8 0-72-32.2-72-72l0-272c0-39.8 32.2-72 72-72l120 0 0-64c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224l16 0 0 192-16 0c-26.5 0-48-21.5-48-48l0-96c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-16 0 0-192 16 0z"/></svg>'></asp:Label>
                             </asp:Panel>

                           <asp:Panel runat="server" CssClass="col-12 col-md-1 col-lg-1 d-flex align-items-center flex-column margin-bottom" ID="lbEstadoIntegracionCompleto2" Visible="False">
                               <asp:Label runat="server" ID="lbEstadoCompletado2" ToolTip="Completado" class="cl_Num__Tarjeta estado_completado2 col-xs-12 col-md-12 col-lg-12" Text='<svg xmlns="http://www.w3.org/2000/svg" height="52" width="60" viewBox="0 0 640 512"><path fill="#fafcff" d="M320 0c17.7 0 32 14.3 32 32l0 64 120 0c39.8 0 72 32.2 72 72l0 272c0 39.8-32.2 72-72 72l-304 0c-39.8 0-72-32.2-72-72l0-272c0-39.8 32.2-72 72-72l120 0 0-64c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224l16 0 0 192-16 0c-26.5 0-48-21.5-48-48l0-96c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-16 0 0-192 16 0z"/></svg>'></asp:Label>
                           </asp:Panel>

                              <asp:Panel runat="server" CssClass="col-12 col-md-1 col-lg-1 d-flex align-items-center flex-column margin-bottom" ID="lbEstadoIntegracionConDetalles" Visible="False">
                                  <asp:Label runat="server" ID="lbEstadoDetalles" ToolTip="No completado" class="cl_Num__Tarjeta estado_detalles col-xs-12 col-md-12 col-lg-12" Text='<svg xmlns="http://www.w3.org/2000/svg" height="52" width="60" viewBox="0 0 640 512"><path fill="#fafcff" d="M320 0c17.7 0 32 14.3 32 32l0 64 120 0c39.8 0 72 32.2 72 72l0 272c0 39.8-32.2 72-72 72l-304 0c-39.8 0-72-32.2-72-72l0-272c0-39.8 32.2-72 72-72l120 0 0-64c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224l16 0 0 192-16 0c-26.5 0-48-21.5-48-48l0-96c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-16 0 0-192 16 0z"/></svg>'></asp:Label>
                              </asp:Panel>

                            <asp:Panel runat="server" CssClass="col-12 col-md-1 col-lg-1 d-flex align-items-center flex-column margin-bottom" ID="lbEstadoIntegracionBorrador" Visible="False">
                                <asp:Label runat="server" ID="lbEstadoBorrador" ToolTip="Borrador" class="cl_Num__Tarjeta estado_borrador col-xs-12 col-md-12 col-lg-12" Text='<svg xmlns="http://www.w3.org/2000/svg" height="52" width="60" viewBox="0 0 640 512"><path fill="#fafcff" d="M320 0c17.7 0 32 14.3 32 32l0 64 120 0c39.8 0 72 32.2 72 72l0 272c0 39.8-32.2 72-72 72l-304 0c-39.8 0-72-32.2-72-72l0-272c0-39.8 32.2-72 72-72l120 0 0-64c0-17.7 14.3-32 32-32zM208 384c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zm96 0c-8.8 0-16 7.2-16 16s7.2 16 16 16l32 0c8.8 0 16-7.2 16-16s-7.2-16-16-16l-32 0zM264 256a40 40 0 1 0 -80 0 40 40 0 1 0 80 0zm152 40a40 40 0 1 0 0-80 40 40 0 1 0 0 80zM48 224l16 0 0 192-16 0c-26.5 0-48-21.5-48-48l0-96c0-26.5 21.5-48 48-48zm544 0c26.5 0 48 21.5 48 48l0 96c0 26.5-21.5 48-48 48l-16 0 0-192 16 0z"/></svg>'></asp:Label>
                            </asp:Panel>

                            <GWC:FindboxControl runat="server" CssClass="col-12 col-md-12 col-lg-5" ID="fcpreReferencia" KeyField="_id" DisplayField="prereferencia" Label="Pre-referencia" Rules="required" OnClick="fcpreReferencia_Click" OnTextChanged="fcpreReferencia_TextChanged"/>
                            <GWC:InputControl runat="server" CssClass="col-12 col-md-12 col-lg-6" ID="icRazonSocialCliente" Type="Text" Name="icRazonSocialCliente" Label="Cliente" Enabled="false"/>
                            
                    </ListControls>
                </GWC:FieldsetControl>
    
                <GWC:FieldsetControl runat="server" ID="fsDetallesDocumentos" Label="Integrar documentos" Detail="Integrar documentos" CssClass="m-0 p-0" > 
                    <ListControls>
                        <asp:Panel runat="server" CssClass="row" >
                            <GWC:SelectControl runat="server" CssClass="col-12 col-md-12 col-lg-3" ID="scTipoDocumentos"  Label="Tipo documento"  OnClick="scTipoDocumentos_Click" OnSelectedIndexChanged="scTipoDocumentos_SelectedIndexChanged" ></GWC:SelectControl>
                            <GWC:FileControl runat="server" CssClass="col-12 col-md-12 col-lg-12" ID="fcDocumento" Dragable="True"  OnChooseFile="fcDocumento_ChooseFile"/>
                            <asp:Panel runat="server" CssClass="col-lg-10 col-md-4 col-4"></asp:Panel>
                            <asp:Panel runat="server" CssClass="col-lg-2 d-flex justify-content-end" ID="lbButtonIA" Visible="false">
                                <asp:Button  runat="server" Label="Procesar" CssClass="btn btn-procesamiento button btn-ia" OnClick="btnIAProcesar_Click" Text='Procesar c/IA'/>
                            </asp:Panel>
                            <asp:Panel runat="server" CssClass="col-lg-2 d-flex justify-content-end" ID="lbButtonXML" Visible="false">
                                
                            </asp:Panel>

                        </asp:Panel>

                         <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 col-lg-12 mt-3 mb-5">
                             <asp:Label runat="server" ID="lbDetalleDocumentosProcesados" Text="Detalle documentos procesados" Visible="True" CssClass="w-100 cl_Secciones mt-5"></asp:Label>
                         </asp:Panel>

                         <GWC:CatalogControl runat="server" ID="ccDocumentosProcesados" KeyField="indice" CssClass="w-100" Collapsed="true" UserInteraction="true" CanAdd="false" CanClone="false" CanDelete="false" >
                            <Columns>
                                <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icNombreArchivo" Label="Archivo"/>
                                <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icTipo" Label="Tipo"/>
                                <GWC:InputControl CssClass="text-align-center" runat="server" Type="Text" ID="icEstado" Label="Estado"/>
                            </Columns>
                        </GWC:CatalogControl>
                    </ListControls>
                </GWC:FieldsetControl>
            </Fieldsets>
        </GWC:FormControl>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server"></asp:Content>


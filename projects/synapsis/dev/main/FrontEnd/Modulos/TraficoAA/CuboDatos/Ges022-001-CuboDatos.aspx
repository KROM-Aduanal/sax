<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-CuboDatos.aspx.vb" Inherits=".Ges022_001_CuboDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>
    <GWC:FindbarControl Label="Buscar Gajo" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="CargarGajo" OnTextChanged="BuscarGajo"/>
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
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Cubo de Datos" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera">
                <DropdownButtons>
                    <GWC:ButtonItem Text="Probar Fórmula"/>
                    <GWC:ButtonItem Text="Limpiar"/>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                </DropdownButtons>
            </Buttonbar>   
            <Fieldsets>
               <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Generales">
                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-4 mt-2 mb-5 solid-textarea p-0 justify-content-start" ID="ic_RoomName" Type="Text" Format="SinDefinir" Name="ic_RoomName" Label="Nombre del Gajo" />
                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4 mt-2 mb-5 p-0 d-flex justify-content-end" ID="sc_TipoRegla" Name="sc_TipoRegla" Label="Tipo de regla"  SearchBarEnabled="false" LocalSearch="false" Rules="required"  >
                                  <Options >
                                         <GWC:SelectOption Value="1" Text="Fórmula"/>
                                         <GWC:SelectOption Value="2" Text="Operando"/>
                                         <GWC:SelectOption Value="3" Text="Archivo CSV"/>
                                         <GWC:SelectOption Value="4" Text="Archivo JSON"/>
                                  </Options>
                            </GWC:SelectControl>
                            <GWC:SelectControl runat="server" CssClass="col-xs-12 col-md-4 mt-2 mb-5 p-0 d-flex justify-content-end" ID="sc_DestinoCubo" Name="sc_DestinoCubo" Label="Cubo destino"  SearchBarEnabled="false" LocalSearch="false" Rules="required"  >
                                  <Options >
                                         <GWC:SelectOption Value="1" Text="A22"/>
                                         <GWC:SelectOption Value="2" Text="VOCE"/>
                                         <GWC:SelectOption Value="3" Text="UCC"/>
                                         <GWC:SelectOption Value="4" Text="UCAA"/>
                                         <GWC:SelectOption Value="5" Text="UAA"/>
                                         <GWC:SelectOption Value="6" Text="CDI"/>
                                  </Options>
                            </GWC:SelectControl>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                            <GWC:InputControl runat="server" CssClass="col-xs-12 col-md-6 mt-2 mb-5 solid-textarea p-0 justify-content-start" ID="ic_RoomRules" Type="TextArea" Format="SinDefinir" Name="ic_RoomRules" Label="Regla del Gajo" />
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">
                             <GWC:CatalogControl runat="server" KeyField="indice" ID="cc_ValoresOperandos" CssClass="w-100" Collapsed="true">
                                <Columns>
                                   <GWC:InputControl runat="server" ID="operandName_" Label="Operando"/>
                                   <GWC:InputControl runat="server" ID="operandValue_" Label="Valor"/>
                                </Columns>
                            </GWC:CatalogControl>
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>


            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false" MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-CuboDatos.aspx.vb" Inherits=".Ges022_001_CuboDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>
    <GWC:FindbarControl Label="Buscar Recámara" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="CargarGajo" OnTextChanged="BuscarGajo" />
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
                
       .CUBODESTINO {
            left:1%;
            top:2px;
         }
        
        .CUBODESTINO input {
            background-color:#050745;            
            color: #fff;
            display: flex;        
            border-radius: 0%;   
            border-color:white;
            box-shadow:none;
	        justify-content: center;            
            align-items: center;
            font-size:small;
            font-weight:bold;
            width: 70%;
            height: 34px;
        }

        .CUBODESTINO input:hover {
            background-color:#050745;            
            color: #fff;
            display: flex;        
            border-radius: 0%;   
            border-color:white;
            box-shadow:none;
	        justify-content: center;            
            align-items: center;
            font-size:small;
            font-weight:bold;
            width: 70%;
            height: 34px;
        }


          .bc_funcionesexel {
            border-radius: 20%; 
            background-color:#3e1093;
            left:2%;
            height: stretch;   
            width: contain;
        }

           .bc_funcionesexel input {
            background-color: #3e1093;            
	        align-items: center; 
            
            height: 30px;   
            width: 40px;            
        }

               .LDIAS {
            left:1%;
            bottom:5px;
         }
        
        .LDIAS input {
            background-color: white;            
            color: #757575;
            display: flex;        
            border-radius: 0%;   
            border-color:white;
            box-shadow:none;
	        justify-content: center;            
            align-items: center;
            font-size:x-large;
            font-weight:bold;
            width: 70%;
            height: 70%;
        }



        .swcOnlines {

            left:38%;
            bottom:8px;
            width: 48%;
            height: 40%;

        }


        .icruless textarea {
            height:250px ;
        }

        .icruless{
            bottom:25px;
        }

        .bc_checaformula input {
             
            background-color: #340687;      
            border-radius: 5%; 

        }

        .bc_checaformula input:hover {
             
            background-color: #340687;      
            border-radius: 5%; 

        }

          .bc_checaformula  {
          background-color: #fff;    
                  bottom:25px;
            color:#757575
       
        }

        .LMensajes input {
            background-color: white;            
            color: #757575;
            display: flex;        
            border-radius: 0%;   
            border-color:white;
            box-shadow:none;
	        justify-content: left;            
                 
            font-size:large;
            font-weight:bold;
            font-family:Calibri;
            width: 70%;
            height: 110%;
        }


        .swcMensajeDefault {

            left:64%;
            bottom:85px;
            
            width: 48%;
            height: 40%;
        }

        .tc_MensajesAlert {

            left:44%;
            bottom:280px;
            width: 20%;

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
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM" HasAutoSave="false" Label="Reglas de Validación General" OnCheckedChanged="MarcarPagina">
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
               <GWC:FieldsetControl runat="server" ID="fscGenerales" Label="Reglas" >
                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12" ID="p_Principal">

                             <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 px-0 mt-2 py-2">
                        
                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2">
                                        
                                                   <GWC:ButtonControl runat="server" Label="A22" ID="bc_SourceCube" CssClass="CUBODESTINO col-xs-2 col-md-1"/>

                                                    <GWC:ButtonControl runat="server" icon="funcionexcel1.png"  ID="bc_Function" CssClass="bc_funcionesexel col-xs-1 col-md-1 mt-2 px-0" OnClick="CambioContenido"  />

                                                    <GWC:ButtonControl runat="server" icon="variable1.png"  ID="bc_variable" CssClass="bc_funcionesexel col-xs-1 col-md-1 mt-2 px-0" Visible="false" OnClick="CambioContenido"  />

                                                    <GWC:FindboxControl runat="server" CssClass="col-xs-12 col-md-8 mt-2 mb-1 px-5" Priority="false" Label="Nombre de la regla" ID="fbc_RoomName" />
                                                                                     
                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0" style="display: flex;">
                           
                                                   <GWC:ImageControl runat="server" Source="/FrontEnd/Librerias/Krom/Imgs/avatarkrom.png" CssClass="col-xs-1 col-md-1 mb-1.8 "  ID="img_User" Name="img_User"  />
  
                                                   <GWC:ButtonControl runat="server" Label="0d" Font-Names="" ID="bc_CurrentUser" CssClass="LDIAS col-xs-2 col-md-2 mb-5"/>

                                                   <GWC:SwitchControl runat="server" ID="swc_Online" CssClass="swcOnlines col-xs-1 col-md-1 mb-5" OnText="Online" OffText="Offline" Checked="false"  />

                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0">
                                  
                                            <GWC:InputControl runat="server" CssClass="icruless col-xs-12 col-md-10 solid-textarea"  ID="ic_RoomRules" Type="TextArea" Format="SinDefinir" Name="ic_RoomRules" Label="Regla" />

                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass=" col-xs-12 col-md-8 px-0 mt-2 py-5" Visible="false" ID="panel_ValoresOperandos">
                 
                                            <GWC:CatalogControl runat="server" KeyField="indice" ID="cc_ValoresOperandos" CssClass="w-100" Collapsed="true">

                                                 <Columns>

                                                       <GWC:InputControl runat="server" ID="operandName_" Label="Operando"/>
                                                                    
                                                       <GWC:InputControl runat="server" ID="operandValue_" Label="Valor"/>
                                                                                
                                                 </Columns>
                                                                               
                                            </GWC:CatalogControl>
                                                           
                                        </asp:Panel>

                                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0">
                           
                                            <GWC:ButtonControl runat="server" Label="Verificar"  Icon="check.png" ID="bc_VerificarFormula" CssClass="bc_checaformula col-xs-2 col-md-2 ml-3 mr-4" OnClick="VerificarFormula"/>

                                            <GWC:ButtonControl runat="server" Label="Limpiar"  Icon="eraser.png" ID="bc_LimpiarFormula" CssClass="bc_checaformula col-xs-2 col-md-2 ml-5"/>

                                        </asp:Panel>
                  
                                 </asp:Panel>

                                 <asp:Panel runat="server" CssClass="col-xs-12 col-md-6 px-0 ">

                                           <GWC:ButtonControl runat="server" Label="Mensajes" Font-Names="" ID="bc_Mensajes" CssClass="LMensajes col-xs-12 col-md-8 px-0"/>

                                           <GWC:TabbarControl runat="server" ID="tc_Mensajes" CssClass="tc_MensajesAlert">
                                               
                                                <Tabs >
  
                                                     <GWC:TabItem Text="Alert"/>
                         
                                                     <GWC:TabItem Text="Warning"/>
                                                        
                                                     <GWC:TabItem Text="Info"/>

                                                </Tabs>
                                                                           
                                                <TabsSections>
                                                                                       
                                                   <GWC:FieldsetControl runat="server" ID="fc_Alert"  CssClass="mb-5" >
                                                                                   
                                                       <ListControls>

                                                            <GWC:InputControl runat="server" CssClass="icruless col-xs-12 col-md-10 solid-textarea mt-5"  ID="ic_Alert" Type="TextArea" Format="SinDefinir" Name="ic_Alert" />
                                                                  
                                                       </ListControls>
                                                                       
                                                   </GWC:FieldsetControl>
                                                                  
                                                   <GWC:FieldsetControl runat="server" ID="fc_Warning">
                                                                                   
                                                       <ListControls>

                                                            <GWC:InputControl runat="server" CssClass="icruless col-xs-12 col-md-10 solid-textarea mt-5"  ID="ic_Warning" Type="TextArea" Format="SinDefinir" Name="ic_Warning" />
                                                                  
                                                       </ListControls>
                                                                       
                                                   </GWC:FieldsetControl>

                                                   <GWC:FieldsetControl runat="server" ID="fc_Info">
                                                                                   
                                                       <ListControls>

                                                            <GWC:InputControl runat="server" CssClass="icruless col-xs-12 col-md-10 solid-textarea mt-5"  ID="ic_Info" Type="TextArea" Format="SinDefinir" Name="ic_Info" />
                                                                  
                                                       </ListControls>
                                                                       
                                                   </GWC:FieldsetControl>

                                                </TabsSections>
                                                                                           
                                          </GWC:TabbarControl>

                                          <GWC:SwitchControl runat="server" ID="swc_MensajeDefault" CssClass="swcMensajeDefault col-xs-6 col-md-12 py-0" OnText="Default" OffText="Default" Checked="false"  />


                                 </asp:Panel>

                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0">
                                  
                                 <GWC:InputControl runat="server" CssClass="icruless col-xs-12 col-md-10 solid-textarea"  ID="ic_DescripcionRules" Type="TextArea" Format="SinDefinir" Name="ic_DescripcionRules" Label="Descripción" />

                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>


            </Fieldsets>
        </GWC:FormControl>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">
</asp:Content>

<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false"  MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-CuboDatos.aspx.vb" Inherits=".Ges022_001_CuboDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

<% If IsPopup = False Then %>
    <GWC:FindbarControl Label="Buscar Gajo" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="CargarGajo" OnTextChanged="BuscarGajo"/>
<% End If %> 

<style>
    /*
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

    }*/

       
</style>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">
<% If IsPopup = False Then %>
    <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>
<% End If %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">


    <div class="d-flex" id="IDPRINCIPALPRINCIPAL" >
        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM"   HasAutoSave="false" Label="Reglas del Cubo" OnCheckedChanged="MarcarPagina">
            <Buttonbar runat="server" OnClick="EventosBotonera" OnLoad="ColocaAutorizar" >
                <DropdownButtons >
                    <GWC:ButtonItem Text="Solicitar Autorización"/>
                    <GWC:ButtonItem Text="Limpiar"/>
                    <GWC:ButtonItem Text="Descargar"/>
                    <GWC:ButtonItem Text="Imprimir"/>
                    <GWC:ButtonItem Text="Mandar por Correo"/>
                    <GWC:ButtonItem Text="Autorizar" Visible="false" ID="bi_SudoAutorizar"/>
                    <GWC:ButtonItem Text="Desechar" Visible="false" ID="bi_SudoDesechar"/>
                </DropdownButtons>
            </Buttonbar>   
            <Fieldsets >
                 <GWC:FieldsetControl runat="server" Label="Formulas" ID="fscformulas" CssClass="fscformulascabulidad">
                    <ListControls>
                        <asp:Panel runat="server" CssClass="col-md-12 col-xs-6" ID="p_formulillas">
                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Regla" CssClass="fieldset-subtitle"></asp:Label>
                            <asp:Panel runat="server" CssClass="wc-cubo-formulas" ID="p_FormulaActual">

                                <div>

                                      <GWC:ButtonControl CssClass="cubo-btn" runat="server" Label="A22" ID="bc_SourceCube"/>
                                      <GWC:ButtonControl CssClass="cubo-btn-formula" runat="server" ID="bc_Function"  OnClick="CambioContenido"/>
                                      <GWC:ButtonControl CssClass="cubo-btn-variable" runat="server" ID="bc_Var" Visible="False" OnClick="CambioContenido"/>
                                      <GWC:InputControl runat="server" CssClass="cubo-input-search" Label="Escriba aquí"  ID="ic_RoomName" />

                                </div>
                                <div >
                                    
                                    <gwc-userdata title="<%=_userName%>" Image="<%=_userImage%>" Date="<%=_accionDate%>" ></gwc-userdata>
                                   
                                    <GWC:ButtonControl runat="server" Label="Por Autorizar" ID="bc_PorAutorizar" CssClass="bcporautorizar" Visible="false"/>
                                    <GWC:ButtonControl runat="server" Label="Verificado" ID="bc_Verificado" CssClass="swcverificado" Visible="false"/>
                                    <GWC:SwitchControl runat="server" OffText="Offline" OnText="Online" ID="swc_Online"/>
                                </div>
                                <div>
                                    <div is="wc-feditor" contenteditable="true"></div>
                                    <asp:TextBox ID="tb_Formula" runat="server" CssClass="feditor-formula-actual"></asp:TextBox>
                                </div>
                                <div>
                                    <GWC:ButtonControl runat="server" Label="Limpiar"  CssClass="iralimpiar" OnClick="LimpiarFormulaCubo" ID="bc_LimpiarFormula"/>
                                    <GWC:ButtonControl runat="server" Label="Elaborar prueba" ID="bc_ElaborarPrueba" CssClass="iraformulacabulidad"  OnClick="IrVerificarFormula" Enabled="false"/>
                                </div>

                            </asp:Panel>


                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-md-6 col-xs-6" ID="p_actualizacionformula" Visible="False">
                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Regla" CssClass="fieldset-subtitle"></asp:Label>

                            <asp:Panel runat="server" CssClass="wc-cubo-formulas" ID="p_FormulaNueva">

                                <div>

                                      <GWC:ButtonControl CssClass="cubo-btn-formula" runat="server" ID="bc_FunctionChange"  OnClick="NewChangeContent"/>
                                      <GWC:ButtonControl CssClass="cubo-btn-variable" runat="server" ID="bc_VarChange" Visible="False" OnClick="NewChangeContent"/>
                                      <GWC:InputControl runat="server" CssClass="cubo-input-search" Label="Escriba aquí"  ID="ic_RoomNameNew" />

                                </div>
                                <div>

                                      <GWC:InputControl runat="server" CssClass="cubo-input-search2 mt-3" Label="Razón por la que se modifica"  ID="ic_changeReason" />

                                </div>
                                <div>
                                    <div is="wc-feditor" contenteditable="true"></div>
                                    <asp:TextBox ID="tb_FormulaNueva" runat="server" CssClass="feditor-formula-nueva"></asp:TextBox>
                                </div>

                            </asp:Panel>


                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-6 wc-cubo-messages">
                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Mensajes" CssClass="fieldset-subtitle"></asp:Label>
                            <GWC:TabbarControl runat="server">
                                <Tabs>
                                    <GWC:TabItem Text="Alertas"/>
                                    <GWC:TabItem Text="Advertencias"/>
                                    <GWC:TabItem Text="Información"/>
                                </Tabs>
                                <TabsSections>
                                    <GWC:FieldsetControl runat="server">
                                        <ListControls>
                                            <GWC:InputControl runat="server" CssClass="w-100" Type="TextArea"/>
                                            <asp:Panel runat="server">
                                                <GWC:SwitchControl runat="server" OffText="Por defecto" OnText="Personalizado"/>
                                            </asp:Panel>
                                        </ListControls>
                                    </GWC:FieldsetControl>
                                    <GWC:FieldsetControl runat="server">
                                        <ListControls>
                                            <GWC:InputControl runat="server" CssClass="w-100" Type="TextArea"/>
                                            <asp:Panel runat="server">
                                                <GWC:SwitchControl runat="server" OffText="Por defecto" OnText="Personalizado"/>
                                            </asp:Panel>
                                        </ListControls>
                                    </GWC:FieldsetControl>
                                    <GWC:FieldsetControl runat="server">
                                        <ListControls>
                                            <GWC:InputControl runat="server" CssClass="w-100" Type="TextArea"/>
                                            <asp:Panel runat="server">
                                                <GWC:SwitchControl runat="server" OffText="Por defecto" OnText="Personalizado"/>
                                            </asp:Panel>
                                        </ListControls>
                                    </GWC:FieldsetControl>
                                </TabsSections>
                            </GWC:TabbarControl>
                            
                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>
                <GWC:FieldsetControl runat="server" Label="Información" ID="fscinformacion" >
                    <ListControls>
                   
                        <asp:Panel runat="server" CssClass="col-xs-6 wc-cubo-description">
                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Descripción" CssClass="fieldset-subtitle" ></asp:Label>
                            <GWC:InputControl runat="server" CssClass="w-100" Type="TextArea" ID="ic_DescripcionRules"/>

                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>
                        <asp:Panel runat="server" CssClass="col-xs-6 wc-cubo-comments">
                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Histórico" CssClass="fieldset-subtitle"></asp:Label>
                            <ul>
                                <gwc-comment title="<%=_userName%>" date="<%=_accionDate%>" image="<%=_userImage%>" Text="<%=_accionText%>"></gwc-comment>
                               
                                <gwc-comment title="<%=_userName2%>" date="<%=_accionDate2%>" image="<%=_userImage%>" Text="<%=_accionText2%>"></gwc-comment>
                                
                                <gwc-comment title="<%=_userName3%>" date="<%=_accionDate3%>" image="<%=_userImage%>" Text="<%=_accionText3%>"></gwc-comment>                              
                            </ul>

                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>
                    </ListControls>
                </GWC:FieldsetControl>

                        
               <GWC:FieldsetControl runat="server"  Label="Prueba de Fórmulas" ID="fscProbarFormulas" CssClass="formulariocabulidad">
                    <ListControls>

 
                        <asp:Panel runat="server" CssClass="col-xs-12 col-md-12 px-0 mt-2 py-5">

                             
                            
                             <GWC:ButtonControl runat="server" Label="Ejecutar Prueba" ID="bc_ProbarFormula" OnClick="VerificarFormula"  CssClass="probarformulacabulidad" Enabled="true"/>
                            
                            

                             <GWC:CatalogControl runat="server" KeyField="indice" ID="cc_ValoresOperandos" CssClass="w-100 catalogopendacabulidad" Collapsed="true">
                                 
                                <Columns>
                                   <GWC:InputControl runat="server" ID="operandName_" Label="Operando" CssClass="catalogopendacabulidad"/>
                                   <GWC:InputControl runat="server" ID="operandValue_" Label="Valor"/>
                                </Columns>
                            </GWC:CatalogControl>
                        </asp:Panel>

                    </ListControls>
                </GWC:FieldsetControl>


            </Fieldsets>
        </GWC:FormControl>
    </div>

 <script>

     localStorage.setItem('contenidoFEditor', '');

     localStorage.setItem('checarInputs', 'NO');

     document.addEventListener('click', function (event) {

         var bc_EleaborarPrueba_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPrueba_bc_ElaborarPrueba');

         if (bc_EleaborarPrueba_.disabled)

             bc_EleaborarPrueba_.style.backgroundColor = '#cecdcd';

         else

             bc_EleaborarPrueba_.style.backgroundColor = 'var(--tintColor) ';

         if (event.target.parentNode.classList.contains('iraformulacabulidad')) {

             var feditor_ = document.querySelector('.feditor-formula-actual');

             if (feditor_.disabled) 

                 feditor_ = document.querySelector('.feditor-formula-nueva');

             if (feditor_.value == "") {

                 localStorage.setItem('contenidoFEditor', '');

             }
             else {

                 if (feditor_.disabled) {

                   //  const bc_ircabulidad_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPrueba_bc_ElaborarPrueba');

                     bc_EleaborarPrueba_.style.backgroundColor = '#cecdcd';

                     bc_EleaborarPrueba_.disabled = true;

                 } else {




                     //const expresion = feditor_.value;

                     //const operandos_ = expresion.split(/[-+*/()]/).filter(operando_ => !operando_.includes("*") &&
                     //                                                                 !operando_.includes("-") &&
                     //                                                                 !operando_.includes("+") &&
                     //                                                                 !operando_.includes("/") &&
                     //                                                                 !operando_.includes("(") &&
                     //                                                                 !operando_.includes(")") &&
                     //                                                                 !operando_.includes("[") &&
                     //                                                                 !operando_.includes("]") &&
                     //                                                                 !operando_.includes(" ") &&
                     //                                                                 !operando_.includes(String.fromCharCode(160))
                     //); // Busca todos los números en la expresión

                     //var tieneElementos_ = false;

                     //console.log(operandos_)

                     //for (var pos_ = 0; pos_ < operandos_.length;pos_++) {

                     //    console.log(operandos_[pos_]);

                     //    if (checanumero(operandos_[pos_])) {
                     //        console.log(operandos_.length + ':' + operandos_[pos_]);
                     //        tieneElementos_ = true;
                     //        break;
                     //    }
                     //}

                     localStorage.setItem('checarInputs', 'SI');


                     //algo_[0].addEventListener("input", function (event) {
                     //    console.log("El valor ha cambiado:", event.target.value);
                     //});


                 }

             }
         } else if (event.target.parentNode.classList.contains('probarformulacabulidad')) {

             var feditor_ = document.querySelector('.feditor-formula-actual');

             if (feditor_.disabled) 
                 feditor_ = document.querySelector('.feditor-formula-nueva');

             if (feditor_.value == "") {

                 localStorage.setItem('contenidoFEditor', '');

             }
             else {

                 const content_ = document.querySelector('.content-wrapper-page');

                 const fieldset_ = document.querySelector('.fieldset-subtitle');


                 if (content_) {
                     content_.scroll({
                         top: fieldset_.offsetTop - 22.2,
                         behavior: 'smooth'
                     });
                 }
             }
         } 
     });

     document.addEventListener('input', function (event) {

         //myDiv.dataset.nthChild;

         var algo_ = document.querySelectorAll("[is=wc-input]");
         //var algo_ = document.getElementsByName('ctl00$contentBody$__SYSTEM_MODULE_FORM$fscProbarFormulas$cc_ValoresOperandos$operandName_$operandName_');
       //  console.log("Nodos:" + algo_.length);

         var icSystem_ = document.getElementById('IDPRINCIPALPRINCIPAL');

         var icRoomName_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_ic_RoomName_ic_RoomName');

         var icChangeReason_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_ic_changeReason_ic_changeReason');

         icRoomName_.style.width = '800px';

         if (icChangeReason_) {
             icChangeReason_.style.width = '400px';
            // icChangeReason_.style.borderwidth = '800px';
         }
     
         //alert(algo_.style.width);
         const componentes_ = document.querySelectorAll("[is=wc-feditor]");
         const componente_ =componentes_[0];
         const componenteNuevo_ = componentes_[1];

         if (event.target === componente_ || event.target === componenteNuevo_) {

             var feditor_ = document.querySelector(".feditor-formula-actual");

             if (feditor_.disabled)

                 feditor_ = document.querySelector('.feditor-formula-nueva');

             var bc_ircabulidad_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPrueba_bc_ElaborarPrueba');

             //console.log(bc_ircabulidad_);

             if (feditor_.value === "") {

                 localStorage.setItem('contenidoFEditor', '');

                 bc_ircabulidad_.style.backgroundColor = '#cecdcd';

                 bc_ircabulidad_.disabled = true;
             }
             else {

                 if (feditor_.disabled) {

                     var bc_limpiar_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_LimpiarFormula_bc_LimpiarFormula');

                     bc_limpiar_.style.backgroundColor = '#cecdcd';

                     bc_limpiar_.disabled = true;

                     bc_ircabulidad_.style.backgroundColor = '#cecdcd';

                     bc_ircabulidad_.disabled = true;

                 } else {

                     bc_ircabulidad_.style.backgroundColor = 'var(--tintColor)';

                     bc_ircabulidad_.disabled = false;
                 }
                 //bc_ircabulidad_.style.backgroundImage = 'url("/FrontEnd/Librerias/Krom/imgs/matrazc.png")';
                 //bc_ircabulidad_.style.backgroundPosition = 'left center';
                 //bc_ircabulidad_.style.backgroundSize = '17%';


             }
         }

         var nodos_ = document.querySelectorAll("[is=wc-input]");


         if (nodos_.length > 3) {

             if (localStorage.getItem('checarInputs') === 'SI') {
                 localStorage.setItem('checarInputs', 'NO');

                 const content_ = document.querySelector('.content-wrapper-page');

                 const fieldset_ = document.querySelector('.formulariocabulidad');


                if (content_) {

                    content_.scroll({
                        top: fieldset_.offsetTop - 22.2,
                        behavior: 'smooth'
                    });
                }

             }

         }
         
     });


     function checanumero(numero_) {

         numero_ = numero_.trim();

         var tieneLetra_ = false;

         var cuentaPunto_ = 0;

         for (const digito_ of numero_) {

             if (!/^[0-9]$/.test(digito_)) {

                 if (digito_ === ".") {

                     cuentaPunto_++;

                     if (cuentaPunto_ > 1) {

                         tieneLetra_ = true;

                         break;
                     }
                 } else {
                     
                     // Si no es un número ni un punto, rompemos el ciclo
                     tieneLetra_ = true;
                     break;
                 }
             }
         }
         return tieneLetra_;
     }

     function getNthChild(element) {
         let count = 1;
         const parent = element.parentElement;
         for (const child of parent.children) {
             if (child === element) {
                 return count;
             }
             console.log(child);
             count++;
         }
         return -1; // Elemento no encontrado
     }


 </script>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">

</asp:Content>


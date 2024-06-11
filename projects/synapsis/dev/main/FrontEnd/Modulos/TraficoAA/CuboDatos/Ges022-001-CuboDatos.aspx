<%@ Page Title="" Language="vb" Async="true" AutoEventWireup="false"  MasterPageFile="~/FrontEnd/Modulos/Home.Master" CodeBehind="Ges022-001-CuboDatos.aspx.vb" Inherits=".Ges022_001_CuboDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"/>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentFindbar" runat="server">

         <% If IsPopup = False Then %>

               <GWC:FindbarControl Label="Buscar recámara" ID="__SYSTEM_CONTEXT_FINDER" runat="server" OnClick="CargarGajo" OnTextChanged="BuscarGajo"/>

         <% End If %> 


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentCompanyList" runat="server">

          <% If IsPopup = False Then %>

                <GWC:SelectControl CssClass="col-auto company-list-select" runat="server" SearchBarEnabled="false" ID="__SYSTEM_ENVIRONMENT" OnSelectedIndexChanged="CambiarEmpresa"/>

          <% End If %>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="contentBody" runat="server">

    <div class="d-flex" id="IDPRINCIPALPRINCIPAL" >

        <GWC:FormControl runat="server" ID="__SYSTEM_MODULE_FORM"   HasAutoSave="false" Label="Reglas del Cubo" OnCheckedChanged="MarcarPagina">

            <Buttonbar runat="server" OnClick="EventosBotonera" OnLoad="ColocaAutorizar" CssClass="botoneracabulidad" ID="bb_botonera" >

                <DropdownButtons >

                    <GWC:ButtonItem Text="Solicitar Autorización" Visible="false" ID="bi_SolicitarAutorizacion"/>

                    <GWC:ButtonItem Text="Limpiar"/>

                    <GWC:ButtonItem Text="Descargar"/>

                    <GWC:ButtonItem Text="Imprimir"/>

                    <GWC:ButtonItem Text="Mandar por Correo"/>

                    <GWC:ButtonItem Text="Autorizar" Visible="false" ID="bi_SudoAutorizar"/>

                    <GWC:ButtonItem Text="Desechar" Visible="false" ID="bi_SudoDesechar"/>

                    <GWC:ButtonItem Text="Comparar" Visible="false" ID="bi_Comparar"  />

                    <GWC:ButtonItem Text="Quitar Comparación" Visible="false" ID="bi_QuitarComparar"/>

                    <GWC:ButtonItem Text="Leer CSV"  ID="bi_LeerCSV" Visible="true" />

                     <GWC:ButtonItem Text="Probar Ruta de Validación"  ID="bi_ProbarRuta" Visible="true" />


                </DropdownButtons>

            </Buttonbar>   

            <Fieldsets >

                 <GWC:FieldsetControl runat="server" Label="Formulas" ID="fscformulas" CssClass="fscformulascabulidad">

                    <ListControls>

                        <asp:Panel runat="server" CssClass="col-md-12 col-xs-6" ID="p_formulillas">

                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Regla" CssClass="fieldset-subtitle" ID="l_RulesOld"></asp:Label>

                            <asp:Panel runat="server" CssClass="wc-cubo-formulas" ID="p_FormulaActual">

                                <div>

                                          <GWC:ButtonControl CssClass="cubo-btn" runat="server" Label="A22" ID="bc_SourceCube"  Visible="true" OnClick="ShowBranchNames"/>

                                          <GWC:ButtonControl CssClass="cubo-btn-formula" runat="server" ID="bc_Function"  OnClick="CambioContenido" Visible="true" />

                                          <GWC:ButtonControl CssClass="cubo-btn-variable" runat="server" ID="bc_Var" Visible="false" OnClick="CambioContenido"/>

                                          <GWC:FindboxControl runat="server" ID="fbc_RoomName" CssClass="cubo-fbc-search" KeyField="_id" DisplayField="campo" RequiredSelect="true" Label="Escriba quí el nombre de la habitación" Visible="true" OnTextChanged="fbc_RoomName_TextChanged"/>

                                          <GWC:InputControl runat="server" CssClass="cubo-input-search" Label="Escriba aquí"  ID="ic_RoomName" Visible="false" />
                                           
                                          <GWC:ButtonControl runat="server" Label="" ID="bc_PorAutorizar" CssClass="bcporautorizar" Visible="true" Enabled="false" ForeColor="DarkGray"/>

                                          <GWC:ButtonControl runat="server" Label="" ID="bc_Verificado" CssClass="swcverificado" Visible="true" Enabled="false" ForeColor="DarkGray"/>

                                          <GWC:SwitchControl runat="server"  CssClass="swc-online" OffText="Offline" OnText="Online" ID="swc_Online" Visible="true"  OnLoad="RevisaVerificado"  />

                                </div>

                                <div >

                                            <GWC:SelectControl runat="server" ID="sc_BranchNames"  SearchBarEnabled="false" LocalSearch="false" Rules="required" visible="false"  OnSelectedIndexChanged="ChangeCubeSource" >

                                                     <Options >

                                                               <GWC:SelectOption Value="1" Text="A22"/>

                                                               <GWC:SelectOption Value="2" Text="VOCE"/>

                                                               <GWC:SelectOption Value="3" Text="UCAA"/>

                                                               <GWC:SelectOption Value="4" Text="UAA"/>

                                                               <GWC:SelectOption Value="5" Text="UCC"/>

                                                               <GWC:SelectOption Value="6" Text="CDI"/>

                                                               <GWC:SelectOption Value="7" Text="CANCELAR"/>

                                                     </Options>

                                           </GWC:SelectControl> 

                                           <asp:Panel runat="server"  ID="p_userdata" Visible="false"   >

                                                   <gwc-userdata title="<%=_userName%>" Image="<%=_userImage%>" Date="<%=_accionDate%>" ></gwc-userdata>

                                           </asp:Panel>
                                   


                                </div>

                                <div>

                                    <div is="wc-feditor" contenteditable="true"></div>

                                    <asp:TextBox ID="tb_Formula" runat="server" CssClass="feditor-formula-actual" OnLoad="RevisaVerificado"></asp:TextBox>

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
                            <asp:Label runat="server" Text="Regla" CssClass="fieldset-subtitle" ID="l_RulesNew"></asp:Label>

                            <asp:Panel runat="server" CssClass="wc-cubo-formulas" ID="p_FormulaNueva">

                                <div>
                                      
                                      <GWC:ButtonControl CssClass="cubo-btn" runat="server" Label="A22" ID="bc_SourceCubeChange"/>

                                      <GWC:ButtonControl CssClass="cubo-btn-formula" runat="server" ID="bc_FunctionChange"  OnClick="NewChangeContent"/>

                                      <GWC:ButtonControl CssClass="cubo-btn-variable" runat="server" ID="bc_VarChange" Visible="False" OnClick="NewChangeContent"/>

                                      <GWC:InputControl runat="server" CssClass="cubo-input-search" Label="Escriba aquí"  ID="ic_RoomNameNew"/>
                                                                        
                                    <GWC:ButtonControl runat="server" Label="" ID="bc_PorAutorizarNueva" CssClass="bcporautorizar" Visible="true" Enabled="false" ForeColor="DarkGray"/>

                                    <GWC:ButtonControl runat="server" Label="" ID="bc_VerificadoNueva" CssClass="swcverificado" Visible="true" Enabled="false" ForeColor="DarkGray"/>

                                    <GWC:SwitchControl runat="server"  CssClass="swc-online" OffText="Offline" OnText="Online" ID="swc_OnlineNueva" OnLoad="RevisaVerificado"/>


                                </div>

                                <div>

                                    <asp:Panel runat="server"  ID="p_userchange" Visible="true"  >

                                         <gwc-userdata ID="algo_" title="<%=_userName%>" Image="<%=_userImage%>" Date="<%=_accionDate%>" ></gwc-userdata>

                                    </asp:Panel>

                                   

                                </div>

                                <div>

                                    <div is="wc-feditor" contenteditable="true"></div>

                                    <asp:TextBox ID="tb_FormulaNueva" runat="server" CssClass="feditor-formula-nueva"></asp:TextBox>
                                         <GWC:InputControl runat="server" CssClass="cubo-input-search2" Label="Razón por la que se modifica"  ID="ic_changeReason" Type="TextArea" />
                                </div>

                                <div>

                                    <GWC:ButtonControl runat="server" Label="Limpiar"  CssClass="iralimpiar" OnClick="LimpiarFormulaCubo" ID="bc_LimpiarFormulaEditar"/>

                                    <GWC:ButtonControl runat="server" Label="Elaborar prueba" ID="bc_ElaborarPruebaEditar" CssClass="iraformulacabulidad"  OnClick="IrVerificarFormula" Enabled="false"/>

                                </div>


                            </asp:Panel>


                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>
 
                        <asp:Panel runat="server" CssClass="col-md-12 col-xs-6 wc-cubo-messages">

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
                   
                        <asp:Panel runat="server" CssClass="col-md-12 col-xs-6 wc-cubo-description" ID="p_descriptions">

                            <%-- DISEÑO COMPONENTE --%>
                            <asp:Label runat="server" Text="Descripción" CssClass="fieldset-subtitle" ></asp:Label>

                            <GWC:InputControl runat="server" CssClass="w-100" Type="TextArea" ID="ic_DescripcionRules"/>

                            <%-- FIN DISEÑO COMPONENTE --%>
                        </asp:Panel>

                        <asp:Panel runat="server" CssClass="col-xs-6 wc-cubo-comments" Visible="false" ID="p_historico">

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

                        
               <GWC:FieldsetControl runat="server"  Label="Prueba de Fórmulas" ID="fscProbarFormulas" CssClass="formulariocabulidad" Visible="false">
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

     localStorage.setItem('ValorNuevo', '');



     document.addEventListener('click', function (event) {


         var bc_EleaborarPrueba_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPrueba_bc_ElaborarPrueba');

         if (!bc_EleaborarPrueba_) {
             bc_EleaborarPrueba_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPruebaEditar_bc_ElaborarPruebaEditar');
         }

         if (bc_EleaborarPrueba_.disabled)

             bc_EleaborarPrueba_.style.backgroundColor = '#cecdcd';

         else

             bc_EleaborarPrueba_.style.backgroundColor = 'var(--tintColor) ';


         if (event.target.parentNode.classList.contains('iraformulacabulidad')) {

             var feditor_ = document.querySelector('.feditor-formula-actual');

             if (feditor_) {

                 if (feditor_.disabled)

                     feditor_ = document.querySelector('.feditor-formula-nueva');
             } else
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





                     localStorage.setItem('checarInputs', 'SI');


                     //algo_[0].addEventListener("input", function (event) {
                     //    console.log("El valor ha cambiado:", event.target.value);
                     //});


                 }

             }
         } else if (event.target.parentNode.classList.contains('probarformulacabulidad')) {

             var feditor_ = document.querySelector('.feditor-formula-actual');

             if (feditor_) {

                 if (feditor_.disabled)
                     feditor_ = document.querySelector('.feditor-formula-nueva');
             } else
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

         //var bc_verificadog_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_Verificado_bc_Verificado');

         //if (bc_verificadog_)
         //    console.log(bc_verificadog_);

         var algo_ = document.querySelectorAll("[is=wc-input]");
         //var algo_ = document.getElementsByName('ctl00$contentBody$__SYSTEM_MODULE_FORM$fscProbarFormulas$cc_ValoresOperandos$operandName_$operandName_');
         //  console.log("Nodos:" + algo_.length);

         //var icSystem_ = document.getElementById('IDPRINCIPALPRINCIPAL');

         //var icRoomName_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_ic_RoomName_ic_RoomName');

         //var icChangeReason_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_ic_changeReason_ic_changeReason');

         ////if (icRoomName_)
         ////    icRoomName_.style.width = '400px';

         ////console.log(icRoomName_);

         //if (icChangeReason_) {
         //    icChangeReason_.style.width = '400px';
         //    // icChangeReason_.style.borderwidth = '800px';
         //}

         //alert(algo_.style.width);
         const componentes_ = document.querySelectorAll("[is=wc-feditor]");

         var componente_
         var componenteNuevo_

         if (componentes_.length === 1) {
             componente_ = componentes_[0];
             componenteNuevo_ = null;
         } else {

             componente_ = componentes_[0];
             componenteNuevo_ = componentes_[1];

         }

         if (event.target === componente_ || event.target === componenteNuevo_) {

             var feditor_ = document.querySelector(".feditor-formula-actual");



             if (feditor_) {
                 if (feditor_.disabled)

                     feditor_ = document.querySelector('.feditor-formula-nueva');
             }
             else {

                 feditor_ = document.querySelector('.feditor-formula-nueva');

             }


             var bc_ircabulidad_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPrueba_bc_ElaborarPrueba');

             if (!bc_ircabulidad_)
                 bc_ircabulidad_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_ElaborarPruebaEditar_bc_ElaborarPruebaEditar');


             //console.log(bc_ircabulidad_);

             if (feditor_.value === "") {

                 localStorage.setItem('contenidoFEditor', '');

                 bc_ircabulidad_.style.backgroundColor = '#cecdcd';

                 bc_ircabulidad_.disabled = true;

             }
             else {

                 if (feditor_.disabled) {

                     var bc_limpiar_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_LimpiarFormula_bc_LimpiarFormula');

                     if (!bc_limpiar_)
                         bc_limpiar_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_LimpiarFormulaEditar_bc_LimpiarFormulaEditar');

                     bc_limpiar_.style.backgroundColor = '#cecdcd';

                     bc_limpiar_.disabled = true;


                     bc_ircabulidad_.style.backgroundColor = '#cecdcd';

                     bc_ircabulidad_.disabled = true;

                     var bc_verificado_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_Verificado_bc_Verificado');

                     if (bc_verificado_)
                         bc_verificado_.disabled= true;


                 } else {

                     var valorOld = localStorage.getItem('ValorNuevo');
                     var valorNew = feditor_.value;

                     if (valorOld !== valorNew) {

                         var bc_verificado_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_Verificado_bc_Verificado');

                         if (!bc_verificado_)
                             bc_verificado_ = document.getElementById('contentBody___SYSTEM_MODULE_FORM_fscformulas_bc_VerificadoNueva_bc_VerificadoNueva');

                         if (bc_verificado_) {
                             bc_verificado_.disabled = true;
                         }
                     }


                     localStorage.setItem('ValorNuevo', valorNew);

                     bc_ircabulidad_.style.backgroundColor = 'var(--tintColor)';

                     bc_ircabulidad_.disabled = false;

                 }
                 //bc_ircabulidad_.style.backgroundImage = 'url("/FrontEnd/Librerias/Krom/imgs/matrazc.png")';
                 //bc_ircabulidad_.style.backgroundPosition = 'left center';
                 //bc_ircabulidad_.style.backgroundSize = '17%';


             }
         }

         var nodos_ = document.querySelectorAll("[is=wc-input]");

         var tamanio_ = 3

         var feditor_ = document.querySelector(".feditor-formula-actual");

         if (feditor_) {
             if (feditor_.disabled)
                 tamanio_ = 4;
         } else
             tamanio_ = 4;

         const element3 = document.querySelector('.botoneracabulidad');

        // element3.style.borderBottomColor = '#cecdcd';

        //this.closest('.row').setAttribute('style', 'border-color:#cecdcd !important;');

       //  const element4 = document.querySelector('.swc-online');

         console.log(element3);
/*         console.log(element3.className);*/

       //  element3.className ='cubo-input-search w-40';

        // console.log(element4);

        // element3.className = 'wc-input __component form-group position-relative w-70 mb-3 '

         //element4.style.position = 'relative'; // Establecer la posición absoluta
         //element4.style.left = '900px';

        // console.log(element3.className);

        

         //console.log("El tamaño de la cabulidad es:" + tamanio_);
         //console.log(nodos_);

         if (nodos_.length > tamanio_) {

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



 </script>

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="footer" runat="server">

</asp:Content>


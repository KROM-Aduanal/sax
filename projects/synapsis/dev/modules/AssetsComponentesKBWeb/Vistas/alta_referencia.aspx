<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Home.Master" CodeBehind="alta_referencia.aspx.vb" Inherits=".alta_referencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">

        <section class="content">
            <!--
            FORM EXAMPLE -->
            <div class="box box-primary">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i><p class="box-title" style="font-size: 20px;">Alta Referencia</p>
                </div>

                <div class="box-body">

                    <form class="d-none" method="post" id="form" action="/CapaPresentacion/alta_referencia.aspx/GuardarDatos" findbar-action="/CapaPresentacion/alta_referencia.aspx/ObtenerAltasReferencias" findbar-text-field="t_NumeroPedimento" is="kbw-form">
                        <fieldset>
                          <legend>General</legend>
                          <div class="row no-gutters">
                              <input type="hidden" name="i_Cve_Referencia"/>
                            <div class="col-auto p-1">
                                <input type="radio" name="i_TipoOperacion" placeholder="Importación" checked="checked" is="kbw-radio" value="1"/>
                            </div>
                            <div class="col-auto p-1">
                                <input type="radio" name="i_TipoOperacion" placeholder="Exportación" is="kbw-radio" value="2"/>
                            </div>
                          </div>
                          <div class="row no-gutters">  
                            <div class="col-xs-12 col-md-4 p-1">
                                <input type="text" name="d_FechaRegistro" placeholder="Fecha de Registro" is="kbw-input-date" rules="required"/>
                            </div>
                          </div>
                          <div class="row no-gutters"> 
                            <div class="col-xs-12 col-md-4 p-1">
                                <select name="i_Cve_MisEmpresas" is="kbw-select" rules="required">
                                  <option value="">Elija un Prefijo</option>
                                  <% If Not listaEmpresas Is Nothing Then
                                          For Each row As Dictionary(Of String, Object) In listaEmpresas%>
                                            <option value="<%=row("Clave")%>"><%=row("Prefijo/Nombre Corto (3 dig)")%></option>                            
                                       <% Next
                                        End If %>
                                </select> 
                            </div>
                          </div>
                          <div class="row no-gutters"> 
                            <div class="col-xs-12 col-md-4 p-1">
                                <select name="i_Cve_Ejecutivo" is="kbw-select" rules="required">
                                  <option value="">Elija un Ejecutivo</option>
                                     <% If Not listaEjecutivos Is Nothing Then
                                          For Each row As Dictionary(Of String, Object) In listaEjecutivos %>
                                            <option value="<%=row("Clave")%>"><%=row("Nombre Completo")%></option>                            
                                       <% Next
                                        End If %>
                                </select> 
                            </div>  
                          </div>
                        </fieldset>
                        <fieldset>
                          <legend>Cliente</legend>
                          <div class="row no-gutters align-items-center">
                            <div class="col-xs-11 col-md-4 p-1">
                                <select name="i_Cve_Cliente" is="kbw-select" to-append-template="/CapaPresentacion/Vistas/modal_client.aspx" rules="required">
                                  <option value="">Elija un Ciente</option>
                                  <% If Not listaClientes Is Nothing Then
                                          For Each row As Dictionary(Of String, Object) In listaClientes%>
                                            <option value="<%=row("Clave")%>"><%=row("Nombre de cuenta contable")%></option>                            
                                       <% Next
                                        End If %>
                                </select> 
                            </div> 
                          </div>                 
                          <dl class="row no-gutters">
                            <dt class="col-xs-2">RFC</dt>
                            <dd class="col-xs-10" id="rfc"></dd>
                            <dt class="col-xs-2">Número</dt>
                            <dd class="col-xs-10" id="num"></dd>
                            <dt class="col-xs-2" >Direccion</dt>
                            <dd class="col-xs-10" id="address"></dd>
                            <dt class="col-xs-2">Ciudad</dt>
                            <dd class="col-xs-10" id="city"></dd>
                            <dt class="col-xs-2">Estado</dt>
                            <dd class="col-xs-10" id="state"></dd>
                            <dt class="col-xs-2">Pais</dt>
                            <dd class="col-xs-10" id="country"></dd>
                          </dl>
                        </fieldset>
                        <fieldset>
                          <legend>Pedimento</legend>
                          <div class="row no-gutters">
                            <div class="col-xs-12 col-md-4 p-1">
                                <input type="text" name="t_NumeroPedimento" placeholder="Pedimento" readonly="readonly" is="kbw-input" value=""/>
                            </div>
                          </div>
                          <div class="row no-gutters"> 
                            <div class="col-xs-12 col-md-4 p-1">
                                <select name="i_TipoCarga" is="kbw-select" rules="required">
                                  <option value="">Elija un Tipo de Carga</option>
                                  <option value="1">Granel</option>
                                  <option value="2">Lote</option>
                                  <option value="3">Carga Suelta</option>
                                  <option value="4">Contenerizada</option>
                                </select> 
                            </div> 
                          </div>
                          <div class="row no-gutters">  
                            <div class="col-xs-12 col-md-4 p-1">
                                <select name="i_TipoModalidad" is="kbw-select" rules="required">
                                  <option value="">Elija un Tipo de Modalidad</option>
                                  <option value="1">Maritima</option>
                                  <option value="2">Aerea</option>
                                  <option value="3">Terrestre</option>
                                </select> 
                            </div>  
                          </div>  
                        </fieldset>
                      </form>
                </div>
            </div>
        </section>      

        <script>
            const input = document.querySelector('[name="t_NumeroPedimento"]');

            input.addEventListener('change',(e)=>{
               
                if(!input.value) {
                    
                    input.value = '3945-2100801-4-20' +Math.floor(Math.random() * 10)+ ''+ Math.floor(Math.random() * 10);
                
                }
            
            })

            input.dispatchEvent(new Event('change'));

            document.addEventListener('click',function(e){
                if(e.target && e.target.id== 'probando'){
                    e.preventDefault();
                    form.pushView = '/CapaPresentacion/Vistas/modal_ejecutive.aspx';
                }
            });

        </script>
         
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
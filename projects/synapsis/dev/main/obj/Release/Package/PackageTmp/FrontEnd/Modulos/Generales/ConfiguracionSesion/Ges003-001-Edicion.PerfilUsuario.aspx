<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="/FrontEnd/Modulos/Home.Master" CodeBehind="Ges003-001-Edicion.PerfilUsuario.aspx.vb" Inherits=".PerfilUsuario" %>

<%@ Import Namespace="System.Collections.Generic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">

    <section class="content">
        <div class="row">

            <%--<div class="col-md-3">
                <div class="box box-primary">
                    <div class="box-body box-profile">
                        <img class="profile-user-img img-responsive img-circle" src="Componentes/dist/img/PedroBautista3.jpg" alt="User profile picture" />
                        <h3 class="profile-username text-center">Pedro Bautista</h3>

                        <p class="text-muted text-center">Krom Aduanal</p>

                        <a href="#" class="btn btn-primary btn-block"><b>Cambiar Imagen</b></a>
                    </div>
                </div>
            </div>--%>

            <div class="col-md-3">
                <%--<form id="Form1" enctype="multipart/form-data" action="helpers/CargarDocumentos.aspx/CambiarImagen" method="post" class="frm-parent">--%>
                <form id="frm-cambiarImagen-usuario" enctype="multipart/form-data" action="helpers/CargarDocumentos.aspx/CargarArchivos" method="post" class="frm-parent">
                    <div class="box box-primary wrapper_form">
                        <div class="box-body box-profile">
                            <img class="profile-user-img img-responsive img-circle" src="<%= ImagenUsuario%>" alt="User profile picture" />
                                
                            <h3 class="profile-username text-center"><%=NombreUsuario%> <%=ApellidoPaternoUsuario%></h3>
                            <p class="text-muted text-center"><%=EmpresaUsuario%></p>
                            
                        </div>

                        <div class="box-footer">
                            <%--<div class="btn btn-primary btn-block" id="btn-cambiarImagen-usuario"><b>Cambiar imagen</b></div>--%>
                            <button type="submit" class="btn btn-primary btn-block" id="btn-cambiarImagen-usuario"><b>Cambiar imagen</b></button>
                            <input type="file" name="uploadedfile" style="display: none;" />
                        </div>
                    </div>
                    
                </form>
            </div>

            <div class="col-md-9">

                <form class="frm-parent" id="frm-actualizar-usuario" action="Ges003-001-Edicion.PerfilUsuario.aspx/ActualizarUsuario" method="post">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <h3 class="box-title">Datos de perfil</h3>
                        </div>

                        <div class="box-body">

                            <div class="form-group col-md-4"> 
                                <label for="nombre_usuario">Nombre</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                    <input type="text" name="nombre_usuario" class="form-control" value="<%=NombreUsuario%>" rules="trim|required|string" />
                                </div>
                            </div>
                            
                            <div class="form-group col-md-4"> 
                                <label for="paterno_usuario">Apellido paterno</label>
                                <div class="input-group" style="width: 100%;">
                                    <%--<span class="input-group-addon"><i class="fa fa-user"></i></span>--%>
                                    <input type="text" name="paterno_usuario" class="form-control" value="<%=ApellidoPaternoUsuario%>" rules="trim|required|string" style="border-radius: 4px;" />
                                </div>
                            </div>

                            <div class="form-group col-md-4"> 
                                <label for="materno_usuario">Apellido materno</label>
                                <div class="input-group" style="width: 100%;">
                                    <%--<span class="input-group-addon"><i class="fa fa-user"></i></span>--%>
                                    <input type="text" name="materno_usuario" class="form-control" value="<%=ApellidoMaternoUsuario%>" rules="trim|string" style="border-radius: 4px;" />
                                </div>
                            </div>

                            <div class="form-group col-md-6"> 
                                <label>Correo</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                    <input type="text" class="form-control" value="<%=CorreoUsuario%>" disabled />
                                </div>
                            </div>

                            <div class="form-group col-md-6"> 
                                <label>Empresa</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-building-o"></i></span>
                                    <input type="text" class="form-control" value="<%=EmpresaUsuario%>" disabled />
                                </div>
                            </div>

                            <div class="form-group col-md-6"> 
                                <label for="telefono_usuario">Telefono</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                                    <input type="text" name="telefono_usuario" class="form-control" value="<%=TelefonoUsuario%>" rules="trim|numeric|exact_length[10]" />
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <label for="nacimiento_usuario">Fecha de nacimiento:</label>

                                <div class="input-group date">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <input type="text" name="nacimiento_usuario" class="form-control pull-right datepicker" value="<%=CumpleanosUsuario%>" rules="is_adult" readonly="readonly" />
                                </div>
                            </div>    

                            <div class="form-group"> 
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" checked="checked" disabled="disabled"/> <a href="#" data-toggle="modal" data-target="#myModal">Condiciones y términos de uso de la información</a>
                                    </label>
                                </div>
                            </div>

                            <%--<button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal">Open Modal</button>--%>

                            <!-- Modal -->
                            <div id="myModal" class="modal fade" role="dialog">
                                <div class="modal-dialog">

                                <!-- Modal content-->
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            <h4 class="modal-title"><b>AVISO DE PRIVACIDAD</b></h4>
                                        </div>
                                        <div class="modal-body">
                                            <p style="text-align: right; font-size: 12px;">Fecha de la última revisión: 04/Ago/2016</p>
                                            <p><b>GRUPO KROM ADUANAL</b> el cual se encuentra conformado por las siguientes personales morales <b>Grupo Reyes Kuri, S.C., Servicios Aduanales del Pacífico, S.C., Comercio Exterior del Golfo, S.C. y Despachos Aéreos Integrados, S.C.</b>, señalan como domicilio para todo lo relacionado con el presente documento al ubicado en <b>Emiliano Zapata, número exterior 80, esq. Xicoténcatl, Col. Ricardo Flores Magón, C.P. 91900, de la Ciudad de Veracruz, Estado de Veracruz</b>; con fundamento en el párrafo II del artículo 16 de la Constitución Política de los Estados Unidos Mexicanos y el artículo 15 de la Ley Federal de Protección de Datos Personales en Posesión de Particulares (LFPDPPP) y su reglamento, hace de su conocimiento lo siguiente.</p>
                                            <p>El <b>RESPONSABLE</b> será aquella persona moral de <b>GRUPO KROM ADUANAL Y KROM LOGISTICA</b> que recabe sus datos personales.</p>
                                            <p>Mediante el presente <b>AVISO DE PRIVACIDAD</b> se hace de su conocimiento que la información (datos personales) que se recabará del <b>TITULAR</b> (como lo es el nombre, domicilio, Registro Federal de Contribuyentes, teléfonos y cualquier otra información complementaria) mediante el presente portal de internet (en adelante el <b>PORTAL</b>), así como la finalidad del tratamiento que le dará el <b>RESPONSABLE</b>.</p>
                                            <p>El solo hecho de que el <b>TITULAR</b> proporcione sus datos personales vía el <b>PORTAL</b>, implica el otorgamiento del consentimiento para el tratamiento de sus datos personales en términos de lo establecido en el presente <b>AVISO DE PRIVACIDAD</b>. Asimismo, el TITULAR acepta y consiente el tratamiento que se le dará a los datos personales que proporcione al <b>RESPONSABLE</b> y/o <b>ENCARGADO</b> con el solo ingreso al <b>PORTAL</b> y al momento de proporcionar por sí mismos sus datos personales.</p>

                                            <h5><b>FINALIDADES DEL TRATAMIENTO DE SUS DATOS</b></h5>
                                            <p>Los datos personales que proporcione el <b>TITULAR</b> a través del <b>PORTAL</b> serán utilizados por el <b>RESPONSABLE</b> para identificarlo plenamente como cliente o proveedor, utilizando dichos datos personales para contactarlo vía electrónica, telefónica o personal, realizar cotizaciones, presentar estadísticas, intercambiar información de mercado, dar seguimiento de solicitudes, presentación de propuestas de solicitud u otorgamiento de bienes y servicios, elaboración de todo tipo de convenio o contratos acorde con los actos que se vayan a realizar, envío de publicidad y promociones, emitir, recibir y enviar comprobantes fiscales, realizar encuestas de servicio y calidad, y en general elaborar y obtener cualquier otro documento o actividad que propicie, consolide, fortalezca y mantenga una relación comercial entre el <b>TITULAR</b> y el <b>RESPONSABLE</b>.</p>
                                            <p>El <b>TITULAR</b> podrá especificar el medio por el cual desea recibir información acorde con la relación que en su momento pueda tener con el <b>RESPONSABLE</b>; en caso de no contar con ésta especificación por parte del TITULAR, el <b>RESPONSABLE</b> establecerá el canal que considere pertinente para enviarle información conducente al efecto pudiendo ser por correo electrónico o físico a la dirección que al efecto proporcioné el <b>TITULAR</b>.</p>

                                            <h5><b>OPCIONES Y MEDIOS PARA LIMITAR EL USO O DIVULGACIÓN</b></h5>
                                            <p>Los datos personales que proporcione el <b>TITULAR</b>, serán almacenados en nuestra base de datos (archivos electrónicos y/o físicos), la cual cuenta con las medidas, candados y controles de seguridad informáticos, físicos y técnicos, que han demostrado ser eficaces y suficientes para salvaguardar cualquier intromisión no autorizada, daños, pérdidas, alteraciones, destrucción o uso indebido de sus datos personales, los cuales son utilizados por el <b>RESPONSABLE</b> para la protección de su propia información.</p>

                                            <h5><b>DERECHOS ARCO</b></h5>
                                            <p>El <b>TITULAR</b> puede en cualquier momento ejercer sus derechos de acceso, rectificación, cancelación u oposición (<b>derechos ARCO</b>) respecto a sus datos personales; pudiendo realizarlo mediante un escrito debidamente firmado por el <b>TITULAR</b> de los datos dirigido a la C. Laura Patricia Cruz Castro, Departamento de Recursos Humanos, ubicado en el domicilio proporcionado al inicio de este documento o bien a través de un correo electrónico a la siguiente dirección: laura.cruz@kromaduanal.com.</p>

                                            <h5><b>TRANSFERENCIA DE DATOS</b></h5>
                                            <p>Los datos personales del <b>TITULAR</b> podrán ser transferidos a empresas subsidiarias, filiales, afiliadas y controladoras del <b>RESPONSABLE</b> así como también con aquellas que tengan relaciones comerciales, para los fines ya citados en el presente.</p>

                                            <h5><b>MODIFICACIONES AL AVISO DE PRIVACIDAD</b></h5>
                                            <p>Cualquier modificación a este <b>AVISO DE PRIVACIDAD</b> le será informada por la misma vía en la que el presente le es notificado y/o al correo electrónico que nos proporcione para tal efecto.</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            
                        </div>

                        <div class="box-footer">
                            <button type="submit" id="btn-actualizar-info-usuario" class="btn btn-primary pull-right">Actualizar</button>
                        </div>
                    </div>
                </form>

                <form class="frm-parent" id="frm-cambiar-contrasena" action="Ges003-001-Edicion.PerfilUsuario.aspx/CambiarContrasena" method="post">

                    <div class="box box-success collapsed-box undo-shadow">
                        <div class="box-header with-border">
                            <%--<label>Cambiar contraseña</label>--%>
                            <h3 class="box-title">Cambiar contraseña</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus kb-btn-ba"></i></button>
                            </div>
                        </div>

                        <div class="box-body">
                            <div class="form-group"> 
                                <label for="contraseña_actual">Contraseña actual</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-unlock-alt"></i></span>
                                    <input type="password" name="contraseña_actual" class="form-control" rules="required" />
                                </div>
                            </div>

                            <div class="form-group"> 
                                <label for="nueva_contraseña">Nueva contraseña</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa  fa-lock"></i></span>
                                    <input type="password" target="change-type" name="nueva_contraseña" class="form-control" rules="required|medium_password" />
                                    <span class="input-group-addon-right change-input-password"><i class="ion ion-eye-disabled"></i></span>
                                </div>
                            </div>

                            <div class="form-group"> 
                                <label for="repetir_contraseña">Confirma tu nueva contraseña </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa  fa-lock"></i></span>
                                    <input type="password" name="confirmar_contraseña" class="form-control" rules="required|matches[nueva_contraseña]" />
                                </div>
                            </div>
                        </div>
                                
                        <div class="box-footer">
                            <button type="submit" id="btn-cambiar-contraseña" class="btn btn-primary pull-left">Cambiar</button>
                        </div>
                    </div>
                    
                </form>

            </div>

        </div>
    </section>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
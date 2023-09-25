<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LogIn.aspx.vb" Inherits=".LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" class="h-100">

<head>

    <meta charset="utf-8" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <title>Synapsis</title>

    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    
    <!-- Fonts and icons -->
    <link rel="apple-touch-icon" sizes="76x76" href="/FrontEnd/Recursos/imgs/ico_Synapsis.png" />

    <link rel="icon" type="image/png" href="/FrontEnd/Recursos/imgs/ico_Synapsis.png" />

    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" />
    
    <!-- CSS Files -->
    <link href="/FrontEnd/Librerias/BootstrapV4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="/FrontEnd/Librerias/BootstrapV4/css/now-ui-kit.css" rel="stylesheet" />
    
    <link href="/FrontEnd/Librerias/Krom/css/Login.css" rel="stylesheet" />
 
</head>
    
<body class="w-100 h-100">   
    
    <div class="row no-gutters h-100 sinapsis-login">
        <div class="d-flex flex-column align-items-center">
            <form id="form1" action="" method="post" runat="server">

            <div class="row no-gutters sinapsis-form">
        
                <div class="col-12 col-md-6 position-ralative">
                    <video class="video-min-bg" src="/FrontEnd/Recursos/videos/VideoWeb_1280x720.mp4" autoplay="true" loop="true" muted=""></video>
                    <div></div>
                    <h2 class="sinapsis-title"><b>Synap</b>sis<small>¡Bienvenidos al futuro!</small></h2>
                    <img src="/FrontEnd/Recursos/Imgs/aduana.png"/>
                </div>

                <div class="col-12 col-md-6">

                    <h3 class="mt-4 mb-4 text-center">¡Iniciemos sesión!</h3>

                    <div class="row no-gutters">
                        <div class="col-12">
                            <label class="sinapsis-input-top">
                                <span>Correo</span>
                                <input class="user-input" type="text" name="user" id="user" placeholder="Correo electrónico"/>
                                <a href="" class="clear">x</a>
                            </label>
                        </div>
                        <div class="col-12">
                            <label class="sinapsis-input-bottom">
                                <span>Contraseña</span>
                                <input class="pass-input" type="password" name="password" id="password" placeholder="Contraseña"/>
                                <a href="" class="clear">x</a>
                            </label>
                        </div>
                    </div>
                    
                    <div class="row no-gutters justify-content-center align-items-center">
                        <div class="col-12">
                            <label style="margin:0"><input type="checkbox" name="recordarSesion"/> Recordar mi datos en este equipo</label>
                        </div>
                        <div class="col-12 mt-4">
                            <button class="btn btn-block submit-input" disabled>¡Comenzar ya!</button>
                        </div>
                        <div class="col-12 mt-1 mb-4">
                            <a href="/FrontEnd/Modulos/Generales/LogIn/RecuperarContrasena.aspx" class="btn btn-link btn-block">¿Olvidaste tu contraseña?</a> 
                        </div>
                    </div>

                </div>

            </div>

        </form>

            <div class="col nav-social align-items-center">
                <a target="_blank" href="http://www.krom.com.mx">
                    <i class="fa fa-globe"></i>
                </a>
                <a target="_blank" href="https://www.facebook.com/kromaduanalylogistica/">
                    <i class="fa fa-facebook-f"></i>
                </a>
                <a target="_blank" href="https://www.linkedin.com/authwall?trk=gf&trkInfo=AQGmcO-zRKV0lgAAAWcTXAUoNbE6AwPZBLw2hq_DN7w1cqT-J5YUiddxDzf9oll0Ysq7uOt8iEcPTUHw1OWiAEg1-0kXDUgxGEmm0Iry0bxZY8PjtryuulcYHxV3ZZ0PTRimLEg=&originalReferer=http://www.kromaduanal.com/&sessionRedirect=https%3A%2F%2Fwww.linkedin.com%2Fcompany%2Fkrom-aduanal-y-log%25C3%25ADstica%3Ftrk%3Dnav_account_sub_nav_company_admin">
                    <i class="fa fa-linkedin"></i>
                </a>
                <%--<small><script> document.write(new Date().getFullYear())</script>, T.I. Corporativo.</small>--%>
            </div>
        </div>
    </div>

</body>

<!-- Core JS Files -->
<script src="/FrontEnd/Librerias/JQuery/jquery-3.3.1.js" type="text/javascript"></script>
<script src="/FrontEnd/Librerias/Tether/tether.min.js" type="text/javascript"></script>
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap.min.js" type="text/javascript"></script>

<!-- Plugin for Switches, full documentation here: http://www.jque.re/plugins/version3/bootstrap.switch/ -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap-switch.js"></script>

<!-- Plugin for the Sliders, full documentation here: http://refreshless.com/nouislider/ -->
<script src="/FrontEnd/Librerias/NouiSlider/nouislider.min.js" type="text/javascript"></script>

<!-- Plugin for the DatePicker, full documentation here: https://github.com/uxsolutions/bootstrap-datepicker -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap-datepicker.js" type="text/javascript"></script>

<!-- Control Center for Now Ui Kit: parallax effects, scripts for the example pages etc -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/now-ui-kit.js" type="text/javascript"></script>

<!-- Krom components -->
<script src="/FrontEnd/Librerias/Krom/js/KromComponentes.js"></script>

<!-- Krom plugins -->
<script src="/FrontEnd/Librerias/Krom/js/KROM-Plugins.js"></script>

<script>

    var mensaje_ = "<%= Session("fallaLogin_")%>"

    if (mensaje_ != "") {

        $.KromMessage("danger", mensaje_)

        <% Session("fallaLogin_") = ""%>

    }

    var cambioContrasena_ = localStorage.getItem("CambioContrasena")

    if (cambioContrasena_ != null && cambioContrasena_ != '') {

        $.KromMessage("success", cambioContrasena_)

        localStorage.removeItem("CambioContrasena")

    }

    //Con esta variable se obtienen los mensaje entre paginas
    var flashdata_ = '<%= Session("flashdata")%>'

    if (flashdata_ != null && flashdata_ != '') {

        $.KromMessage("info", flashdata_)

        <% Session.Contents.Remove("flashdata")%>

    }

    document.querySelectorAll('.clear').forEach((a) => {

        a.addEventListener('click', (e) => {

            e.preventDefault();

            e.target.closest('label').querySelector('input').value = "";

            submitPerm();

        });

    });

    document.querySelector('.user-input').addEventListener('keyup', e => submitPerm());

    document.querySelector('.pass-input').addEventListener('keyup', e => submitPerm());

    function submitPerm() {
       if (document.querySelector('.user-input').value && document.querySelector('.pass-input').value) {
            document.querySelector('.submit-input').removeAttribute('disabled');
        } else {
            document.querySelector('.submit-input').setAttribute('disabled','disabled');
        }
    }

    window.onload = e => submitPerm();

</script>

</html>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RecuperarContrasena.aspx.vb" Inherits=".RecuperarContrasena" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>

    <meta charset="utf-8" />

    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <title>Synapsis</title>

    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    
    <!-- Fonts and icons -->
    <link rel="apple-touch-icon" sizes="76x76" href="/FrontEnd/Recursos/imgs/ico_Synapsis.png" />

    <link rel="icon" type="image/png" href="/FrontEnd/Recursos/imgs/ico_KromAduanal.png" />

    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700,200" rel="stylesheet" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" />
    
    <!-- CSS Files -->
    <link href="/FrontEnd/Librerias/BootstrapV4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="/FrontEnd/Librerias/BootstrapV4/css/now-ui-kit.css" rel="stylesheet" />

    <link rel="stylesheet" href="/FrontEnd/Librerias/Krom/css/krom.css"/>

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>

</head>

<body class="login-page">
    
    <div id="loading">
    
            <img style="width:150px;height:150px;opacity:0.4;" src="/FrontEnd/Recursos/imgs/krom_logo_loading.gif" />
    
    </div>

    <nav class="navbar navbar-toggleable-md bg-primary fixed-top navbar-transparent" color-on-scroll="500">

        <div class="container">

            <div class="collapse navbar-collapse justify-content-end" id="navigation" data-nav-image="/FrontEnd/Recursos/imgs/blurred-image-1.jpg">

                <ul class="navbar-nav">
                    
                    <li class="nav-item">

                        <a class="nav-link" href="http://www.krom.com.mx">Sitio oficial</a>

                    </li>
                    
                    <li class="nav-item">

                        <a class="nav-link" rel="tooltip" title="Like us on Facebook" data-placement="bottom" href="https://www.facebook.com/kromaduanalylogistica/" target="_blank">

                            <i class="fa fa-facebook-square"></i>

                            <p class="hidden-lg-up">Facebook</p>

                        </a>

                    </li>

                    <li class="nav-item">

                        <a class="nav-link" rel="tooltip" title="Follow us on LinkedIn" data-placement="bottom" href="https://www.linkedin.com/authwall?trk=gf&trkInfo=AQGmcO-zRKV0lgAAAWcTXAUoNbE6AwPZBLw2hq_DN7w1cqT-J5YUiddxDzf9oll0Ysq7uOt8iEcPTUHw1OWiAEg1-0kXDUgxGEmm0Iry0bxZY8PjtryuulcYHxV3ZZ0PTRimLEg=&originalReferer=http://www.kromaduanal.com/&sessionRedirect=https%3A%2F%2Fwww.linkedin.com%2Fcompany%2Fkrom-aduanal-y-log%25C3%25ADstica%3Ftrk%3Dnav_account_sub_nav_company_admin" target="_blank">

                            <i class="fa fa-linkedin-square"></i>

                            <p class="hidden-lg-up">LinkedIn</p>

                        </a>

                    </li>

                </ul>

            </div>

        </div>

    </nav>
    
    <div class="page-header" filter-color="orange">

        <div class="page-header-image" >

            <video  height="100%" id="login-video" src="/FrontEnd/Recursos/videos/VideoWeb_1280x720.mp4" autoplay="true" loop="true" muted></video>

        </div>

        <div class="container">

            <div class="col-md-4 content-center">
                
                <div class="card card-login card-plain">

                    <form id="RecuperarContrasena" class="frm-parent" action="RecuperarContrasena.aspx\EnviarContrasena" method="post" runat="server">

                        <div class="header header-primary text-center">

                            <div class="logo-container">

                                <img src="/FrontEnd/Recursos/imgs/logo_krom_bn.png"/>

                            </div>

                        </div>
                        
                        <div class="content">
                            
                            <p style="font-size: 12px;">Introduce tu usuario para que te enviemos una nueva contraseña</p>
                            
                            <div class="input-group form-group-no-border input-lg">

                                <span class="input-group-addon">

                                    <i class="now-ui-icons users_circle-08"></i>

                                </span>

                                <input type="text" class="form-control" name="user" id="user" placeholder="email@dominio.com" rules="trim|required|valid_email"/>

                            </div>

                            <div class="g-recaptcha" data-callback="recaptchaCallback" data-sitekey="6Lc2rocUAAAAAFwwKH9gucAgLk8nzO3vT1ABMt8I"></div>

                        </div>

                        <div class="footer text-center">

                            <input type="submit" id="recuperar-contrasena" class="btn btn-primary btn-round btn-lg btn-block" value="¡ Recuperar !" disabled/>

                        </div>

                    </form>

                </div>

            </div>

        </div>

        <footer class="footer">

            <div class="container">

                <div class="copyright">

                    &copy;

                    <script>

                        document.write(new Date().getFullYear())

                    </script>, T.I. Coporativo

                </div>

            </div>

        </footer>

    </div>

</body>

<!--   Core JS Files   -->
<script src="/FrontEnd/Librerias/JQuery/jquery-3.3.1.js" type="text/javascript"></script>

<script src="/FrontEnd/Librerias/Tether/tether.min.js" type="text/javascript"></script>

<script src="/FrontEnd/Librerias/BootstrapV4/js/bootstrap.min.js" type="text/javascript"></script>

<!-- Control Center for Now Ui Kit: parallax effects, scripts for the example pages etc -->
<script src="/FrontEnd/Librerias/BootstrapV4/js/now-ui-kit.js" type="text/javascript"></script>

<!-- Krom components -->
<script src="/FrontEnd/Librerias/Krom/js/KromComponentes.js"></script>

<!-- Krom plugins -->
<script src="/FrontEnd/Librerias/Krom/js/KROM-Plugins.js"></script>

<script>

    function recaptchaCallback() {

        $("#recuperar-contrasena").removeAttr('disabled')

    };

    $('#recuperar-contrasena').click(function (event) {

        event.preventDefault()

        $.KromForm({ 'idButton': this.id }, function (response) {
            if (response.code == 200) {
                $.KromMessage('success', response.message)
                setTimeout(function myFunction() {
                    window.location.replace("/FrontEnd/Modulos/LogIn/LogIn.aspx");
                }, 3000);
            } else {
                $.KromMessage('danger', response.message)
            }
        })

    })
</script>

</html>
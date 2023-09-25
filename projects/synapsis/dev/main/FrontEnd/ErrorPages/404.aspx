<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="404.aspx.vb" Inherits=".PageNotFound" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>Error</title>
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0, shrink-to-fit=no' name='viewport' />
    <link rel="stylesheet" href="/FrontEnd/Librerias/BootstrapV3/dist/css/bootstrap.min.css" />
    <style>
        .bg-body {
            background: #722c62; /* Old browsers */
            background: -moz-linear-gradient(45deg, #722c62 0%, #730059 45%, #360056 100%); 
            background: -webkit-linear-gradient(45deg, #722c62 0%,#730059 45%,#360056 100%); 
            background: linear-gradient(45deg, #722c62 0%,#730059 45%,#360056 100%); 
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#722c62', endColorstr='#360056',GradientType=1 ); 
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
        }
        .bg-body img {
            opacity: .2;
            position: absolute;
            right: 0;
            bottom: 0;
            pointer-events: none;
        }
        .box-warnig {
            background-color: rgba(255,255,255,.1);
            border-radius: 7px;
            /*box-shadow: 0px 2px 8px rgba(0,0,0,0.25);*/
            padding:16px 144px;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%,-50%);
        }
        h1 {
            font-size: 144px;
            color: #fff;
            font-weight: bold;
        }
        h3 {
            font-size: 36px;
            color: #fff;
        }
        p {
            color: #fff;
        }
    </style>
</head>
    <body>
        <div class="bg-body">
            <image src="/FrontEnd/Recursos/Imgs/bg404.png"/>
            <div class="box-warnig">
                <h1>404</h1>
                <h3>Página no encontrada</h3>
                <p>Lo sentimos pero la página solicitada no fue encontrada</p>
                <a href="/FrontEnd/Modulos/Generales/LogIn/LogIn.aspx">Volver</a>
            </div>
         </div>
    </body>
</html>
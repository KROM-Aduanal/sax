const _paginaLogIn = "/FrontEnd/Modulos/Generales/LogIn/LogIn.aspx"

$(".btnCerrarSesion").click(function (event) {
    event.preventDefault()
    $.ajax({
        type: "POST"
        , url: _paginaLogIn + "/CerrarSesion"
        , contentType: 'application/json; charset=utf-8'
        , dataType: 'JSON'
        , beforeSend: function () {
            $("#loading").css("display", "inherit");
        }
        , error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
        }
        , success: function (response) {
            window.location.href = _paginaLogIn + "?sta=2"
        }
        , complete: function () {
            $("#loading").css("display", "none");
        }
    });
})

$('.toggle-menu').click(function (event_) {

    $('.binnacle-aside').toggleClass('binnacle-aside-hidden', function (event_) {

        //$('.arrow-colapse').toggleClass('colapse');

        let hasClass_ = $(this).hasClass('binnacle-aside-hidden');

        if (hasClass_) {

            $('.content-wrapper').css({ 'margin-right': '0' });

            //$('.content-wrapper').css({ 'transform': 'translate(0px, 0)' });
            
            //$('.arrow-colapse').css({ 'left': '-30px', 'transform': 'scaleX(-1)' });

            $('.toggle-menu').removeClass('toggle-menu-next').addClass('toggle-menu-prev');

        } else {

            $('.content-wrapper').css({ 'margin-right': '270px' });

            //$('.content-wrapper').css({ 'transform': 'translate(-230px, 0)' });

            //$('.arrow-colapse').css({ 'left': '0', 'transform': 'scaleX(1)' });

            $('.toggle-menu').removeClass('toggle-menu-prev').addClass('toggle-menu-next');

        }

        /*setTimeout(() => {
            $(".content-wrapper-page").niceScroll();
            $(".content-wrapper-page").getNiceScroll().resize();
        }, 300);*/
        

    });

});

/*$('.arrow-colapse-menu').click(function (event_) {
    
    $('body').toggleClass('sidebar-open', function (event_) {

        let hasClass_ = $(this).hasClass('sidebar-open');

        if (hasClass_) {

            $('.arrow-colapse-menu').css({ 'left': '23rem', 'transform': 'scaleX(-1)' });

        } else {

            $('.arrow-colapse-menu').css({ 'left': '0', 'transform': 'scaleX(1)' });

        }

    });

});*/

$('.finder-bar__logo').click(function (event_) {

    $('body').toggleClass('sidebar-open', function (event_) { });

});



$('[name="right-menu"]').change(function () {

    const id = $(this).val();
    
    $('.black-item-data').addClass('d-none');

    $('[menu-section="' + id + '"]').removeClass('d-none');

});

/***********************************/
/*REDIRECCIONA AL PERFIL DE USUARIO*/
//$(".perfil-usuario").click(function (event) {
//    event.preventDefault();
//    window.location.href = "/FrontEnd/Modulos/ConfiguracionSesion/Ges003-001-Edicion.PerfilUsuario.aspx";
//});
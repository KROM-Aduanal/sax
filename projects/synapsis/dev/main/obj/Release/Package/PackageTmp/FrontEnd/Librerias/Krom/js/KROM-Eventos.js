(function () {

    //#Region "Atributes"

    //#End Region

    //#Region "Llamados"

    KromComponentes()

    //#End Region

    //#Region "Eventos"
    

    /***********************************/
    /*EVENTO PARA SUBIR CFDIs JUPI*/
    $('#btn-cargar').click(function (event) {
        event.preventDefault();

        //Prepara los elementos de form
        var parent_ = $(this).closest('form.frm-parent');
        var uploadFile_ = $('input[type="file"]', parent_);
        var submit_ = $('button[type="submit"]', parent_);
        var textarea_ = $('button[type="textarea"]', parent_);

        var formData = new FormData();

        var files_ = uploadFile_[0].files
        $.each(files_, function (index, element) {
            formData.append("file-" + index, element);
        });
        //ñ que no se vaya directo por el id
        formData.append("valida-extencion", $('#valida-extencion').val());
        formData.append("extenciones", $('#extenciones').val());
        formData.append("tamano-maximo", $('#tamano-maximo').val());
        formData.append("tipo", $('#tipo').val());

        //Sirve para inspeccionar el FormData
        //for (var pair of formData.entries()) {
        //    console.log(pair[0]+ ', ' + pair[1]); 
        //}

        $.ajax({
            type: 'POST'
            , url: '../../../../../../../../../../../../CapaPresentacion/' + parent_.attr('action')
            , data: formData
            , processData: false
            , contentType: false
            , cache: false
            , beforeSend: function () {
                $("#loading").css("display", "inherit");
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
            }
            , success: function (response) {
                $.KromMessage('info', response.message)
                $('textarea#ta-cargados').val(response.message)
                setTimeout(function myFunction() {
                    $('textarea#ta-archivos').val("")//location.reload()
                    $('input#it-subir').val(null)
                }, 3000);
            }
            , complete: function () {
                $("#loading").css("display", "none");
            }
        });

    });


    /***********************************/
    /*EVENTOS PARA EL PERFIL DE USUARIO*/
    //CAMBIA EL TYPO DE TEXBOX
    $('.change-input-password').click(function (event) {
        var input_ = $(this).parent().find('input[target="change-type"]')
        var son_ = $('i:first', this)

        if (input_.attr('type') == 'password') {
            input_.attr('type', 'text')
            son_.removeClass('ion-eye-disabled')
            son_.addClass('ion-eye')
        } else {
            input_.attr('type', 'password')
            son_.removeClass('ion-eye')
            son_.addClass('ion-eye-disabled')
        }

        //NOTA: No se hace agrega al plugin para usarlo de ejemplo de como extender la libreria de jquery
    })

    //EVENTO PARA CAMBIAR LA CONTRASEÑA
    $('#btn-cambiar-contraseña').click(function (event) {

        event.preventDefault()
        $.KromForm({ 'idButton': 'btn-cambiar-contraseña' }, function (response) {
            if (response.code == 200) {
                localStorage.setItem("CambioContrasena", response.message);
                $("#btnCerrarSesion").trigger("click")
            } else {
                $.KromMessage('danger', response.message)
            }
        })

    })

    //EVENTO PARA CAMBIAR LA INFORMACIÓN DEL USUARIO
    $('#btn-actualizar-info-usuario').click(function (event) {

        event.preventDefault()
        $.KromForm({ 'idButton': this.id, 'upperCase': true }, function (response) {
            if (response.code == 200) {
                $.KromMessage('success', response.message)
                setTimeout(function myFunction() {
                    location.reload()
                }, 3000);
            } else {
                $.KromMessage('danger', response.message)
            }
        })

    })

    //EVENTO PARA CAMBIAR LA IMAGEN DE PERFIL
    $('#btn-cambiarImagen-usuario').click(function (event) {
        event.preventDefault();

        //Prepara los elementos de form
        var parent_ = $(this).closest('form.frm-parent');
        var uploadFile_ = $('input[type="file"]', parent_);
        var submit_ = $('button[type="submit"]', parent_);

        uploadFile_.trigger('click');
        uploadFile_.change(function () {

            //var formData = new FormData(parent_[0]);

            var formData = new FormData();

            var files_ = uploadFile_[0].files
            $.each(files_, function (index, element) {
                formData.append("file-" + index, element);
            });

            formData.append("valida-extencion", true);
            formData.append("extenciones", 'png,jpg,jpeg');
            formData.append("tamano-maximo", 5120);
            formData.append("tipo", 'imagenusuario');

            //Sirve para inspeccionar el FormData
            //for (var pair of formData.entries()) {
            //    console.log(pair[0]+ ', ' + pair[1]); 
            //}

            $.ajax({
                type: 'POST'
                , url: '../../../../../../../../../../../../CapaPresentacion/' + parent_.attr('action')
                , data: formData
                , processData: false
                , contentType: false
                , cache: false
                , type: 'POST'
                , beforeSend: function () {
                    $("#loading").css("display", "inherit");
                }
                , error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                }
                , success: function (response) {
                    if (response.code == 200) {
                        $.KromMessage('success', response.message)
                        setTimeout(function myFunction() {
                            location.reload()
                        }, 3000);
                    } else {
                        uploadFile_.val('')
                        $.KromMessage("info", response.message)
                    }
                }
                , complete: function () {
                    $("#loading").css("display", "none");
                }
            });

        });

    });

    /******************************/
    /*EVENTO PARA EL SCROLL BUTTON*/
    var scrollBotton_ = $('#ScrollBackBotton_wrapper');
    var scrollArrow_ = $('#ScrollBackArrow');

    $(window).scroll(function () {
        if ($(window).scrollTop() > 300) {
            scrollBotton_.addClass('show')
            //scrollBotton_.animate({ 'opacity': 1 }, 200)
            scrollArrow_.animate({ 'top': '50%' }, 500)
        } else {
            scrollBotton_.removeClass('show');
            //scrollBotton_.animate({ 'opacity': 0 }, 200)
        }
    });

    scrollBotton_.on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, '300');
        //scrollBotton_.css('opacity', 0)
    });

    ///*FLOATING BUTTOMS*/
    //$('#zoom-btn-copy').click(function(event) {
    //    $('.buttons-copy').trigger( "click" );
    //});

    //$('#zoom-btn-pdf').click(function(event) {
    //    $('.buttons-pdf').trigger( "click" );
    //});

    //$('#zoom-btn-print').click(function(event) {
    //    $('.buttons-print').trigger( "click" );
    //});

    //$('#zoom-btn-download').click(function(event) {
    //    self.location = "../../../../../../../../../../CapaPresentacion/helpers/DescargarDocumentos.aspx?mod=repcom";
    //});


    /***********************/
    /*EVENTOS DE DASHBOARDS*/
    $('#consultaOperacionesVerdes').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", " \"AND t_Semaforo = 'Verde'\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    $('#consultaOperacionesRojas').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", " \"AND t_Semaforo = 'Rojo'\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    $('#consultaOperacionesDespachadas').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", " \"AND NOT f_FechaDespacho IS NULL\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    $('#consultaOperacionesVivasPagadas').click(function (e) {
        e.preventDefault();
        //localStorage.setItem("filtroDashBoard", " \"AND NOT sc_f_FechaPago IS NULL AND f_FechaDespacho IS NULL AND sc_f_FechaPago >= DATEADD(MONTH, -6 , DATEADD(MM, DATEDIFF(MM,0,GETDATE()), 0))\"");
        //window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";

        localStorage.setItem("filtroDashBoard", " \"AND b_FechaDespacho = 'No'\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    $('#consultaOperacionesVivasSinPagar').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", "");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_OperacionesVivas.aspx";
    })

    $('#consultaOperacionesImpo').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", " \"AND i_TipoOperacion = 1\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    $('#consultaOperacionesExpo').click(function (e) {
        e.preventDefault();
        localStorage.setItem("filtroDashBoard", " \"AND i_TipoOperacion = 2\"");
        window.location.href = "/FrontEnd/Modulos/ConsultasOperaciones/Cat003-001-Consultas_Operaciones.aspx";
    })

    //#End Region

})()


function Package() {
    
    const _kromComponents = new KromComponentes();

    var _package = this;
    
    var _baseURL = window.location.protocol + window.location.host + '/';
    
    var _nombreUsuario = "";
    
    this.kromDataTable = function (settings) {

        var url = (typeof settings.url === "undefined" || settings.url.indexOf("/") < 0) ? "" : settings.url;

        var conditions = (typeof settings.conditions === "undefined") ? "{clausulasLibres_: \"\"}" : settings.conditions;

        var label = (typeof settings.label === "undefined") ? "" : settings.label;

        var columns = (typeof settings.columns === "undefined") ? "" : (($.isArray(settings.columns)) ? settings.columns : "");

        var callback = (typeof settings.callback === "function") ? settings.callback : null;

        var entries = (typeof $('#tbl_referencias').attr('entries') === 'undefined') ? 100 : $('#tbl_referencias').attr('entries');

        var elements_ = (typeof settings.elements === "undefined") ? 'lBfrtip' : settings.elements;

        var activateMessage_ = (typeof settings.message === "undefined") ? false : ((settings.message.activate == true)? true : false);

        if (url != "" && label != "") {

            $.ajax({
                type: "POST",
                url: url,
                data: conditions,
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                }
                , beforeSend: function () {
                    $("#loading").css("display", "inherit");
                }
                , success: function (response) {

                    var source = response.d;

                    var comlumnData_ = []
                    var count_ = 0
                    $.each(source, function (row_, data_) {
                        $.each(data_, function (key_, value_) {
                            if (key_ != '__type') {
                                var data_ = {}
                                comlumnData_.push({ 'targets': count_, 'data': key_ })
                                count_ += 1
                            }
                        })
                        return false;
                    });
                    
                    if (typeof settings.render != "undefined") {
                        
                        $.each(settings.render, function (key, value) {
                            var target_ = value.targets
                            var data_ = value.data
                            var render_ = value.render

                            if ((typeof target_ != "undefined" && typeof target_ == "number")
                                && (typeof data_ != "undefined" && typeof data_ == "string")
                                && (typeof render_ == "function")) {
                                
                                var object_ = { "targets": target_, "data": data_, "render": render_ }
                                comlumnData_.splice(target_, 1, object_)
                            }
                        })
                    }
                    
                    if (typeof settings.extraContent != "undefined") {

                        var total = comlumnData_.length - 1
                        var Counter_ = settings.extraContent.length - 1
                        var extraContent_ = ''
                        $.each(settings.extraContent, function (key, value) {
                            total += 1
                            comlumnData_.push({
                                targets: total
                                , data: null
                                , defaultContent: value
                            })
                        })

                    }

                    var buttons_ = { "buttons": settings.buttons }

                    _package.loadScript('/FrontEnd/Librerias/DataTables/js/buttons-print.js', function () { });

                    tabla = $(label).DataTable({
                        destroy: true
                        , ordering: false
                        , iDisplayLength: entries
                        //, dom: 'lBfrtip'
                        , dom: elements_
                        , initComplete: function () {
                            var buttons = $(".dt-buttons")
                            var search = $(label + "_filter")
                            buttons.insertAfter(search)
                            buttons.css("width", "100%")
                            $('.tdExtras').remove()
                            $('.dt-buttons').hide()
                        }
                        , buttons: [
                            settings.buttons
                        ]
                        , data: source
                        , columnDefs: comlumnData_
                    });
                    
                    /**/
                    _package.ShowDocuments(tabla)

                    //Deprecated
                    $('select[name="tbl_referencias_length"]').on('change', function () {
                        var entries = $(this).val();
                        $('#tbl_referencias').attr('entries', entries)
						
                    })

                    if (activateMessage_) {
                        var message_ = settings.message.message
                        var tipo_ = "info"
                        if (message_ == 'aCookie') {

                            _package.loadScript('/FrontEnd/Librerias/JQuery/jquery.cookie.js', function () {
                            
                                var registrosEncontrados_ = $.cookie('cookieTotalRegistros')
                                var nombreUsuario_ = $.cookie('nombreUsuario')
                            
                                if (nombreUsuario_ == "undefined" || nombreUsuario_ == "") {
                                    nombreUsuario_ = "Undefined"
                                } else {
                                    nombreUsuario_ = nombreUsuario_.split(' ')[0]
                                    nombreUsuario_ = nombreUsuario_.substring(0, 1).toUpperCase() + nombreUsuario_.substring(1).toLowerCase()
                                }

                                var message_ = ""
                                
                                if (typeof registrosEncontrados_ !== 'undefined') {

                                    if (registrosEncontrados_ == 0) {

                                        message_ = "¡Vaya!, " + nombreUsuario_ + " no encontré coincidencias para la búsqueda, intenta nuevamente cambiando tus filtros"
                                        tipo_ = "danger"

                                    } else if (registrosEncontrados_ > 20) {

                                        var adjetivo_ = ' disponibles en extranet'

                                        tipo_ = "info"
                                        if (registrosEncontrados_ < 10000) {
                                            adjetivo_ = ' existentes'
                                            tipo_ = "success"
                                        }

                                        message_ = "" + nombreUsuario_ + ", aquí " + source.length + " registros de " + +registrosEncontrados_ + adjetivo_

                                    } else if (registrosEncontrados_ < 20) {

                                        message_ = "¡Hey " + nombreUsuario_ + "!, únicamente encontré " + source.length + " registros para esta búsqueda"
                                        tipo_ = "success"

                                    } else if (registrosEncontrados_ = 20) {

                                        message_ = "¡Hey " + nombreUsuario_ + "!, aquí tienes una muestra de " + source.length + " registros, para ver más, pulsa click en buscar ;)"
                                        tipo_ = "info"
                                    }
                                }

                                _package.message(tipo_, message_)

                            });
                        }
                        //_package.message(tipo_, message_)
                    }

                }
                , complete: function () {
                    $("section.content").css("cssText", "display: inherit !important;");
                    $("#loading").css("display", "none");
                }
            });

        } else {
            //Codigo para mostrar errores
        }

    }

    this.message = function (type, text, delay) {

        var text = (text) ? text : "Undefined"
        var delay_ = (delay) ? delay : 5000
        
        var container = $("<div>", { css: { "width": "0px", "height": "60px", "border-radius": "3px", "padding": "5px", "font-size": "13px", "position": "fixed", "top": "55px", "left": "calc(100% - 330px)", "overflow": "hidden", "opacity": 0, "z-index": 9999 } })
        var wrapper = $("<div>", { css: { "width": "285px", "height": "48px", "position": "relative" } })
        var text = $("<p>", { text: text, css: { "position": "absolute", "top": "50%", "transform": "translateY(-50%)" } })

        switch(type) {
            case "success":
                container.css({
                    "background-color": '#00a65a'
                    , "border-left": '4px solid #00733e'
                    , "color": "#ffffff"
                });
                break;

            case "warning":
                container.css({
                    "background-color": '#f39c12'
                    , "border-left": '4px solid #c87f0a'
                    , "color": "#ffffff"
                });
                break;

            case "info":
                container.css({
                    "background-color": '#00c0ef'
                    , "border-left": '4px solid #0097bc'
                    , "color": "#ffffff"
                });
                break;

            case "danger":
                container.css({
                    "background-color": '#dd4b39'
                    , "border-left": '4px solid #c23321'
                    , "color": "#ffffff"
                });
                break;

            default:
                container.css({
                    "background-color": '#ffffff',
                    "border-left": '4px solid #FF9800'
                });
        }

        text.appendTo(wrapper)
        wrapper.appendTo(container)
        container.appendTo("body")

        container
			.animate({
			    opacity: 1
			    //, padding: "5px"
				, width: "300px"}
				, 500)
			.delay(delay_)
			.animate({
			    //padding: "0px"
			    width: "0px"
				, opacity: 0}
				, 500
				, function () {
				    container.remove()
				});
            
    }

    this.KromClausulasLibres = function (data, clausulas_) {
        
        var clausulasLibres_ = ''
        var cadenaClausulas_ = ''
        var objetoClausulas_ = {}

        $.each(data, function (index, dato) {
            
            switch(dato.tipo) {
                case "String":
          
                    var type = $("#" + dato.label).attr('type')
                    
                    if (typeof type != 'undefined') {
                        
                        var label_ = $("#" + dato.label).val().trim();
                        
                        if (label_ != "") {
                            clausulasLibres_ += " AND " + dato.clausula + " LIKE '%" + label_ + "%'"
                        }
                    } else {
                        var label_ = $("#" + dato.label).val();
                        
                        var clausula_ = ''
                        if (label_.length != 0 && label_[0] != "Todos") {
                            $.each(label_, function (index, val) {
                                if (label_.length - 1 == index) {
                                    clausula_ += "'" + val + "'";
                                } else {
                                    clausula_ += "'" + val + "', ";
                                }
                            });
                            clausulasLibres_ += ' AND ' + dato.clausula + ' IN (' + clausula_ + ')'
                        }
                    }
                    break;

                case "Int":
                    var label_ = $("#" + dato.label).val();
                    var clausula_ = ''
                    var type = $("#" + dato.label).attr('type')
                    
                    if (typeof type != 'undefined') {
                        if (label_ != "") {
                            clausulasLibres_ += ' AND ' + dato.clausula + " LIKE '%" + label_ + "%'"
                        }
                    } else {
                        if (label_.length != 0 && label_[0] != "Todos") {
                            $.each(label_, function (index, val) {
                                if (label_.length - 1 == index) {
                                    clausula_ += val;
                                } else {
                                    clausula_ += val + ", ";
                                }
                            });
                            clausulasLibres_ += ' AND ' + dato.clausula + ' IN (' + clausula_ + ')'
                        }
                    }
                    break;

                case "Select":
                    var label_ = $("#" + dato.label).val();
                    var clausula_ = ''
                    if (label_.length != 0 && label_[0] != "Todos") {
                        $.each(label_, function (index, val) {
                            if (label_.length - 1 == index) {
                                clausula_ += "'" + val + "'";
                            } else {
                                clausula_ += "'" + val + "', ";
                            }
                        });
                        clausulasLibres_ += ' AND ' + dato.clausula + ' IN (' + clausula_ + ')'
                    }
                    break;

                case "Date":
                    var startDate_ = $("#" + dato.label + " span").attr("start-date");
                    var endDate_ = $("#" + dato.label + " span").attr("end-date");
                    if (typeof startDate_ != "undefined" && endDate_ != "undefined") {
                        //clausulasLibres_ += " AND CAST(" + dato.clausula + " AS DATE) BETWEEN '" + startDate_ + "' AND '" + endDate_ + "'"
                        clausulasLibres_ += " AND " + dato.clausula + "  >= '" + startDate_ + "' AND " + dato.clausula + " <= '" + endDate_ + "'"
                    }
                    break;

                case "StaticDate":
                    var value_ = $("#" + dato.label).val();
                    var newDate_ = ''
                    var date_ = new Date()
                    if (value_ != '') {
                        switch (value_) {
                            case "7days":
                                date_.setDate(date_.getDate() - 6)
                                break;

                            case "30days":
                                date_.setDate(date_.getDate() - 29)
                                break;

                            case "60days":
                                date_.setDate(date_.getDate() - 59)
                                break;

                            case "lastYear":
                                date_.setDate(date_.getDate() - 364)
                                break;

                            default:
                                return false

                        }
                    } else {
                        
						date_.setDate(date_.getDate() - 6)
                        
                    }
                    var year_ = date_.getFullYear()
                    var month_ = ((date_.getMonth() + 1).toString().length == 2) ? (date_.getMonth() + 1) : '0' + (date_.getMonth() + 1);
                    var day_ = (date_.getDate().toString().length == 2) ? date_.getDate() : '0' + date_.getDate();
                    newDate_ = [year_, month_, day_].join('-')
                    clausulasLibres_ += " AND " + dato.clausula + " >= '" + newDate_ + "'"
                    break;

                default:
                    clausulasLibres_ += '';
            }

        });

        cadenaClausulas_ = "{clausulasLibres_: \"" + clausulasLibres_ + "\"}"
        objetoClausulas_ = JSON.parse("{\"clausulasLibres_\": \"" + clausulasLibres_ + "\"}")

        if (typeof clausulas_ == "function") {
            clausulas_(cadenaClausulas_, objetoClausulas_)
        }

    }

    this.KromForm = function (config, method_) {
        
        var idButton = (typeof config.idButton != 'undefined') ? '#' + config.idButton : false;

        var upperCase_ = (typeof config.upperCase != 'undefined') ? ((config.upperCase == true) ? true : false) : false;
        
        if (idButton) {

            var form = $(idButton).closest('form.frm-parent')

            if (typeof form != 'undefined' && form.length > 0) {

                var url = form[0].action
                if (typeof url != 'undefined') {

                    var tagsRules = {}

                    $('[name]', form).each(function (index, element) {

                        var hadRules = $(element).attr('rules')

                        if (hadRules != '') {

                            tagsRules[element.name] = $(element).attr('rules')

                        }

                    })

                    var elementsForm = []
                    
                    $('[returnabled]', form).each(function (index, element) {

                        elementsForm.push(element)

                    })

                    var error_ = _package.validationsForm(tagsRules, form)
                    
                    $('.input-group, .form-group', form).each(function (index, value) {

                        $('.show-errors', this).first().css('display', 'table')

                    })

                    //Validación en las tabs
                    if (error_.length != 0) {

                        if(config. validateTabs != 'undefined') {

                            if(config. validateTabs) {
    
                                let tabsIds_ = []
    
                                $('.show-errors', form).each(function (index_, value_) {
    
                                    let tabParentID_ = $(this).closest('.wizard-tab-content-box').attr('id')
    
                                    if (!tabsIds_.includes(tabParentID_)) {
    
                                        tabsIds_.push(tabParentID_)
    
                                    }
    
                                })
    
                                $.each(tabsIds_, function (index_, value_) { 
                                
                                    let error_ = $('<span>', {'class': 'show-erros-tabs'})
    
                                    let tab_ = $('[href="#' + value_ + '"]').closest('.nav-item')
    
                                    error_.appendTo(tab_)
                                    
                                });

                                $.KromMessage('info', 'Revisa los datos del formulario, hubo algun error :/')
    
                            }
    
                        }

                    }

                    if (error_.length == 0) {

                        var fields = {}

                        $('[name]', form).each(function (index, element) {

                            if (upperCase_) {
                                fields[element.name] = element.value.toUpperCase()
                            } else {
                                fields[element.name] = element.value
                            }

                        })

                        var data = JSON.stringify({ 'data_': fields })
                        
                        $.ajax({
                            type: "POST"
                            , url: url
                            , data: data
                            , contentType: 'application/json; charset=utf-8'
                            , dataType: 'JSON'
                            , error: function (xhr, ajaxOptions, thrownError) {
                                console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                            }
                            , beforeSend: function () {
                                $("#loading").css("display", "inherit");
                            }
                            , success: function (response) {
                                var response = JSON.parse(response.d)

                                if (elementsForm.length > 0)
                                    response['elementsForm'] = elementsForm

                                if (typeof method_ === 'function')
                                    method_(response)
                            }
                            , complete: function () {
                                $("#loading").css("display", "none");
                            }
                        })

                    }

                }

            } else {
                console.log('El ID del boton del formualrio es incorrecto o el formulario padre es indefinido')
            }

        } else {
            console.log('El ID del boton del formulario es indefinido')
        }

    }

    this.validationsForm = function (tags_, form_) {

        var errors_ = []
        var className_ = 'show-errors'
        var validations_ = {
            required: function (name_, attr) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo es requerido'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })
                
                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val().trim() == "") {
                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('required')
                }

            }
            , min_length: function (name_, attr) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo requiere un mínimo de ' + attr + ' caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val().trim() != "") {

                    if (tag_.val().length <= attr) {

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('min_length')
                    }

                }
            }
            , max_length: function (name_, attr) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo requiere un máximo de ' + attr + ' caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val().trim() != "") {

                    if (tag_.val().length >= attr) {

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('max_length')
                    }

                }
            }
            , exact_length: function (name_, attr) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo requiere exactamente ' + attr + ' caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val().trim() != "") {

                    if (tag_.val().length != attr) {

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('exact_length')
                    }

                }
            }
            , trim: function (name_) {
                var newValue_ = $('[name="' + name_ + '"]', form_).val().trim()
                $('[name="' + name_ + '"]', form_).attr('value', newValue_)
            }
            , matches: function (name_, attr) {
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')

                var partner_ = $('[name="' + attr + '"]', form_)
                var field_ = $('[name="' + name_ + '"]', form_)
                var label_ = $('[name="' + attr + '"]', form_).closest('.form-group').find('label').html()

                var text_ = 'Este campo debe ser igual a "' + label_ + '"'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (partner_.val() != field_.val()) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('matches')
                }
            }
            , low_password: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'La contraseña requiere por lo menos una mayúscula, una minúscula y tener al menos 6 caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                //var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$/;
                var regex = /^(([a-z])+([A-Z])+|(([A-Z])+[a-z])+)+.*\S{6,}$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('low_password')
                }
            }
            , medium_password: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'La contraseña debe tener 2 mayúsculas, 2 minúsculas, 2 números y tener al menos 8 caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^(?=(?:.*\d){2})(?=(?:.*[A-Z]){2})(?=(?:.*[a-z]){2})\S{8,}$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('medium_password')
                }
            }
            , high_password: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'La contraseña debe tener 2 mayúsculas, 2 minúsculas, 2 números, 2 símbolos y tener al menos 10 caracteres'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                //var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$/;
                var regex = /^(?=(?:.*\d){2})(?=(?:.*[A-Z]){2})(?=(?:.*[a-z]){2})(?=(?:.*[$@$!%*?&]){2})\S{10,}$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('high_password')
                }
            }
            , string: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo solo acepta letras'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^[A-Za-z\sáÁéÉíÍóÓúÚñÑ]+$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.attr('rules').indexOf('required') >= 0 && tag_.val() != '') {
                    if (!regex.test(tag_.val())) {

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('string')
                    }
                }

            }
            , numeric: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo solo acepta números'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^[0-9]+$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val().trim() != "") {
                
                    if (!regex.test(tag_.val())) {

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('numeric')
                    }

                }
            }
            , alphanumeric: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo solo acepta letras y numeros'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^[A-Za-záÁéÉíÍóÓúÚñÑ0-9]+$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('alphanumeric')
                }
            }
            , alphanumeric_space: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Este campo solo acepta letras y numeros'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^[A-Za-záÁéÉíÍóÓúÚñÑ0-9 ]+$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('alphanumeric_space')
                }
            }
            , valid_email: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'El correo electrónico no es valido'
                var error_ = $('<span>', { 'class': className_, 'text': text_ })

                var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (!regex.test(tag_.val())) {

                    $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                        $(error_, parent_).remove()
                    })

                    parent_.append(error_)
                    errors_.push('valid_email')
                }
            }
            , is_adult: function (name_) {
                var tag_ = $('[name="' + name_ + '"]', form_)
                var parent_ = $('[name="' + name_ + '"]', form_).closest('.input-group')
                var text_ = 'Debes tener por lo menos 18 años.'

                var date_ = new Date();
                var currentYear_ = date_.getFullYear();

                var anio_ = tag_.val().split('/')[2]
                var age_ = currentYear_ - anio_

                if(parent_.length == 0) {
                    
                    parent_ = $('[name="' + name_ + '"]', form_).closest('.form-group')

                }

                if (tag_.val() != "") { 

                    if (anio_.length != 4) {

                        text_ = 'Debes colocar una fecha correcta'
                        var error_ = $('<span>', { 'class': className_, 'text': text_ })

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('min_length')

                    } else if (age_ <= 18) {

                        var error_ = $('<span>', { 'class': className_, 'text': text_ })
                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('min_length')

                    } else if (age_ >= 80) {
                        // text_ = 'Wow! Tienes más de 100 años!'
                        text_ = 'En KROM sabemos que siempre haz dado lo mejor, pero pensamos que ya es tiempo de descanzar despues de todos estos años'
                        var error_ = $('<span>', { 'class': className_, 'text': text_ })

                        $('[name="' + name_ + '"]', form_).on('click focus mouseenter', function (event) {
                            $(error_, parent_).remove()
                        })

                        parent_.append(error_)
                        errors_.push('min_length')
                    }

                }
            }
        }

        $.each(tags_, function (indexTags_, valueTags_) {

            if (typeof valueTags_ != 'undefined') {
                
                var rules_ = valueTags_.split('|')
                $.each(rules_, function (indexRules_, valueRules_) {

                    var hadAttr = valueRules_.split('[')
                    if (typeof hadAttr[1] != 'undefined') {
                        var attr = hadAttr[1].split(']')[0]
                        eval('validations_.' + hadAttr[0] + '("' + indexTags_ + '", "' + attr + '")')
                    } else {
                        eval('validations_.' + hadAttr[0] + '("' + indexTags_ + '")')
                    }

                })

            }

        })

        return errors_

    }

    this.getDate = function (format_, period_, type_) {
        
        var newDate_ = ''
        
        var date_ = new Date()

        if (typeof period_ != 'undefined' && typeof type_ != 'undefined') {
            switch (type_) {
                case 'day':
                    date_.setDate(date_.getDate() + period_)
                    break;

                case 'month':
                    
                    break;

                case 'year':
                    
                    break;

                default:
                    date_.setDate(date_.getDate() + period_)

            }
        }

        var year_ = date_.getFullYear()
        var month_ = ((date_.getMonth() + 1).toString().length == 2) ? (date_.getMonth() + 1) : '0' + (date_.getMonth() + 1);
        var day_ = (date_.getDate().toString().length == 2) ? date_.getDate() : '0' + date_.getDate();

        if (typeof format_ != 'undefined') {
            if (typeof format_ === 'string') {
                if (format_.indexOf('-') > 0) {
                    var splitFormat_ = format_.split('-')
                    if (splitFormat_.length == 3) {
                        newDate_ = [eval(splitFormat_[0] + "_"), eval(splitFormat_[1] + "_"), eval(splitFormat_[2] + "_")].join('-')
                    }
                } else {
                    newDate_ = 'La fecha debe separarse con un guion (-)'
                }
            } else {
                newDate_ = 'El formato de fecha debe ser un string "year-month-day"'
            }
        } else {
            newDate_ = [year_, month_, day_].join('-')
        }
        
        return newDate_
    }

    this.loadScript = function (url, callback) {
        jQuery.ajax({
            url: url
            , dataType: 'script'
            , error: function (xhr, ajaxOptions, thrownError) { location.reload() }
            , success: callback
            , async: true
        });
    }

    this.NombreUsuario = (response_ = 'undefined') => {
        
        _package.loadScript('/FrontEnd/Librerias/JQuery/jquery.cookie.js', () => {
            
            var nombreUsuario_ = $.cookie('nombreUsuario');
                            
            if (nombreUsuario_ == "undefined" || nombreUsuario_ == "" || nombreUsuario_ == undefined) {
            
                nombreUsuario_ = "Undefined";
                
            } else {
            
                nombreUsuario_ = nombreUsuario_.split(' ')[0];
            
                nombreUsuario_ = nombreUsuario_.substring(0, 1).toUpperCase() + nombreUsuario_.substring(1).toLowerCase();
            
            }
            
            _nombreUsuario = nombreUsuario_;

            if (response_ !== 'undefined')

                if (response_ !== 'function')

                    response_(_nombreUsuario)

        });
        
    }

    this.ShowDocuments = function (table_) {

        $('.mostrar-documentos').click(function (event) {
            var tr = $(this).closest('tr');
            var row = table_.row(tr);
            const boton_ = $(this)
            
            if (boton_.children().hasClass("fa-plus-circle")) {
                $.ajax({
                    type: 'POST'
                    , url: 'Cat003-001-Consultas_Operaciones.aspx/ConsultarDocumentos'
                    , data: '{"id_": "' + row.data().ID + '"}'
                    , contentType: 'application/json; charset=utf-8'
                    , dataType: 'JSON'
                    , error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
                    }
                    , beforeSend: function () {
                        $("#loading").css("display", "inherit");
                    }
                    , success: function (response) {

                        if (response.d != "") {
                            const documentos_ = JSON.parse(response.d);
                            const svg_ = new KromComponentes();

                            function format(d) {
                                
                                let trJoin_ = [];

                                $.each(documentos_.listaDocumentos_, (index, documento) => {
                                    index ++;
                                    if ((index + 2) % 3 == 0) {
                                        trJoin_.push(`<tr>`);
                                    }
                                    
                                    let icono = svg_.ObtenerSVG(documento.extension, 'undefined', 35) != "" ? svg_.ObtenerSVG(documento.extension, 'undefined', 35) : '<img src="/FrontEnd/Recursos/imgs/pdf.png" target="icon" style="width: 35px;" title="">';
                                    
                                    let ext_, doc_, archivo_;
                                    if (documento.extension == '.zip') {
                                        ext_ = 'zip'
                                        doc_ = 'mdz';
                                        archivo_ = documento.referencia;
                                    } else {
                                        ext_ = 'pdf'
                                        doc_ = 'mdo';
                                        archivo_ = documento.nombreArchivo;
                                    }

                                    trJoin_.push(`
                                        <td style="cursor: pointer;">
                                            <a class="" href="/FrontEnd/NameSpaces/DescargarDocumentos.aspx?mod=opeadmin&doc=${doc_}&bus=${archivo_}&ext=${ext_}&des=${documento.descarga}">
                                                ${icono}
                                            </a>
                                        </td>
                                        <td>${documento.nombreDocumento}</td>`)

                                    if (index % 3 == 0) {
                                        trJoin_.push(`</tr>`)
                                    }
                                });

                                return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
                                            trJoin_.join("") +
                                        '</table>';
                            }

                            if (!row.child.isShown()) {
                                row.child(format(row.data())).show();
                                tr.addClass('shown');
                                boton_.children().removeClass('fa-plus-circle');
                                boton_.children().addClass('fa-minus-circle'); 
                            }

                    } else {
                            _package.message('info', _nombreUsuario + " los documentos se encuentran en procesamiento, en breve podrás descargarlos.");
                        }

                    }
                    , complete: function () {
                        $("#loading").css("display", "none");
                    }
                });
            } else {
                row.child.hide();
                tr.removeClass('shown');
                boton_.children().removeClass('fa-minus-circle');
                boton_.children().addClass('fa-plus-circle');
            }
        });

    }
    
    //APARTIR DE AQUI SE EMPIEZA A USAR ECMASCRIPT 6
    this.KromFormWizard = (config_, objectRequet_) => {
        
        if(typeof(config_) != 'undefined' && typeof(config_) == 'object') {
            
            const tabWizard_ = new TabWizard()

            const formTabsContainerID_ = (typeof(config_.formTabsContainerID) != 'undefined') ? config_.formTabsContainerID : false;

            const tabs_ = (typeof(config_.tabs) != 'undefined') ? config_.tabs : undefined;
            
            const contentTabsBox_ = (typeof(config_.tabsContentContainer) != 'undefined') ? $('#' + config_.tabsContentContainer) : undefined; 

            const tabType_ = (typeof(config_.tabType) != 'undefined') ? config_.tabType : tabWizard_.TabType.Selection; 

            const trackingAtt_ = (typeof(config_.trackingAttribute) != 'undefined') ? config_.trackingAttribute : false;

            const executeForm_ = (typeof(config_.executeForm) != 'undefined') ? true : false;

            if(formTabsContainerID_) {

                let totalTabs_ = 0
                
                $.each(totalTabs_, () => {
                    
                    totalTabs_ += 1

                })

                if (tabs_ != 'undefined' && totalTabs_ >= 0) {

                    let tabWizardUL_ = $('<ul>', {'class': 'wizard-tabs-box'})

                    let href_, icon_, text_, tabWizardLI_, tabWizardA_, tabWizardEM_
                        , tabWizardSpan_, trackingAttTap_, trackingAttContent_ = undefined

                    let tabArray_ = []

                    $.each(tabs_, (index_, tab_) => {
                        
                        tabNumber_ = index_ + 1

                        href_ = (typeof(tab_.HREF) != 'undefined') ? '#' + tab_.HREF : '#';

                        icon_ = (typeof(tab_.Icon) != 'undefined') ? tab_.Icon : tabNumber_;

                        text_ = (typeof(tab_.Text) != 'undefined') ? tab_.Text : 'Esto es un texto';

                        tabWizardLI_ = $('<li>', {'class': 'nav-item'})

                        tabWizardA_ = $('<a>', {'href': href_, 'class' : 'nav-link'})

                        tabWizardEM_ = $('<em>').append(icon_)

                        tabWizardSpan_ = $('<span>').append(text_)

                        tabWizardEM_.appendTo(tabWizardA_)

                        tabWizardSpan_.appendTo(tabWizardA_)

                        tabWizardA_.appendTo(tabWizardLI_)

                        tabWizardLI_.appendTo(tabWizardUL_)

                        if (trackingAtt_) {

                            trackingAttTap_ = trackingAtt_ + 'Tab'

                            trackingAttContent_ = trackingAtt_ + 'Content'

                            tabWizardLI_.attr(trackingAttTap_, '')

                            $(href_).attr(trackingAttContent_, '')

                        }

                        tabArray_.push(href_)

                    })

                    tabWizardUL_.appendTo($('#' + formTabsContainerID_))

                    contentTabsBox_.appendTo($('#' + formTabsContainerID_))
                    
                    $('a[href="' + tabs_[0].HREF + '"]').parent().addClass('active')

                    $(tabs_[0].HREF).css('display', 'block')

                    //****************************/
                    //KromFormWizard plugin events

                    const tabTypeTabWizard_ = tabWizard_.TabType

                    $(".nav-item a", $('.wizard-tabs-box')).each(function (index_) {
            
                        $(this).on("click", function (e_) {

                            e_.preventDefault();

                        })
                                
                    })

                    switch (tabType_) {
                        
                        case tabTypeTabWizard_.Selection:
                        
                            $('a[href="' + tabArray_[0] + '"]').closest('.nav-item').addClass('active');

                            $(tabArray_[0]).css('display', 'block')

                            $(".nav-item a", $('.wizard-tabs-box')).each(function (index_) {
            
                                $(this).on("click", function (e_) {
                                    
                                    e_.preventDefault();
        
                                    let tabContentBox_ = $(this).attr('href')

                                    if(trackingAtt_){

                                        $('[' + trackingAttTap_ + ']').removeClass('active')

                                        $('[' + trackingAttContent_ + ']').css('display', 'none')

                                    } else {

                                        $('.nav-item').removeClass('active')

                                        $('.wizard-tab-content-box').css('display', 'none')

                                    }

                                    $(this).closest('.nav-item').addClass('active')

                                    $(tabContentBox_).css('display', 'block')
                                    
                                })
                                
                            })

                            break;

                        case tabTypeTabWizard_.Step:
                        
                            $('a[href="' + tabArray_[0] + '"]').closest('.nav-item').addClass('active');

                            $(tabArray_[0]).css('display', 'block')

                            $($('.wizard-tab-content-previous-button', contentTabsBox_), $('#' + formTabsContainerID_)).each(function(index_){

                                let previousStep_ = tabArray_[index_]

                                let tab_ = $('a[href="' + previousStep_ + '"]').closest('.nav-item')
                                
                                $(this).attr('PreviousStep', previousStep_)
                                
                                $(this).on("click", function (e_) {
                                    
                                    e_.preventDefault();
                                    
                                    if(trackingAtt_){

                                        $('[' + trackingAttTap_ + ']').removeClass('active')

                                    } else {

                                        $('.nav-item').removeClass('active')

                                    }

                                    tab_.addClass('active');

                                    $(this).closest('.wizard-tab-content-box').css('display', 'none');
                                    
                                    $(previousStep_).css('display', 'block')

                                    $('.show-erros-tabs', tab_).each(function () {
                                        
                                      $(this).remove()
                                        
                                    })
                                    
                                })
                                
                            })

                            $($('.wizard-tab-content-next-button', contentTabsBox_), $('#' + formTabsContainerID_)).each(function(index_){

                                let previousStep_ = tabArray_[index_]

                                let nextStep_ = tabArray_[index_ + 1]

                                let tab_ = $('a[href="' + nextStep_ + '"]').closest('.nav-item')

                                $(this).attr('NextStep', nextStep_)

                                $(this).on("click", function (e_) {
                                    
                                    e_.preventDefault();
        
                                    if(trackingAtt_){

                                        $('[' + trackingAttTap_ + ']').removeClass('active')

                                    } else {

                                        $('.nav-item').removeClass('active')

                                    }

                                    $('a[href="' + previousStep_ + '"]').closest('.nav-item').addClass('done');

                                    tab_.addClass('active');

                                    $(this).closest('.wizard-tab-content-box').css('display', 'none');
                                    
                                    $(nextStep_).css('display', 'block')

                                    $('.show-erros-tabs', tab_).each(function () {
                                        
                                        $(this).remove()
                                        
                                    })
                                    
                                })

                            })

                            break;
                    
                        default:

                            break;

                    }

                    if (executeForm_) {

                        // let sendButtonID_ = config_.executeForm[0].sendButtonID

                        // let form_ = $('a[href="' + tabArray_[0] + '"]').closest('.frm-parent')

                        // form_.submit(function (e) { 
                            
                        //     let tagsRules_ = {}

                        //     $('[name]', form_).each(function (index_, element_) {

                        //         let hadRules_ = $(element_).attr('rules')
                                
                        //         if (hadRules_ != '') {
                                
                        //             tagsRules_[element_.name] = $(element_).attr('rules')
                                
                        //         }

                        //     })

                        //     let error_ = _package.validationsForm(tagsRules_, form_)
                        
                        //     $('.form-group', form_).each(function (index_, value_) {
                                
                        //         $('.show-errors', this).first().css('display', 'table')
                                
                        //     })

                        //     if (error_.length != 0) {

                        //         e.preventDefault()

                        //         let tabsIds_ = []

                        //         $('.show-errors', form_).each(function (index_, value_) {

                        //             let tabParentID_ = $(this).closest('.wizard-tab-content-box').attr('id')

                        //             if (!tabsIds_.includes(tabParentID_)) {

                        //                 tabsIds_.push(tabParentID_)

                        //             }

                        //         })

                        //         $.each(tabsIds_, function (index_, value_) { 
                                   
                        //             let error_ = $('<span>', {'class': 'show-erros-tabs'})

                        //             let tab_ = $('[href="#' + value_ + '"]').closest('.nav-item')

                        //             error_.appendTo(tab_)
                                    
                        //         });

                        //     }
                            
                        // })

                    }

                } else {

                    $.KromMessage('warning', 'Falta declarar las tabs')

                }

            } else {
                
                $.KromMessage('warning', 'El contenedor no esta definido')

            }
        
        } else {
        
            $.KromMessage('warning', 'Se han declarado mal los parametros')
        
        }

    }

    this.KromModal = (config_, method_) => {
        
        let title_ = (typeof(config_.title) != 'undefined') ? config_.title : '';

        let url_ = (typeof(config_.url) != 'undefined') ? config_.url : false;

        let size_ = (typeof(config_.size) != 'undefined') ? config_.size : 'medium';

        let hideFooter_ = (typeof(config_.hideFooter) != 'undefined') ? config_.hideFooter : false;

        let closeModalClass_ = (typeof(config_.closeCloak) != 'undefined' 
                                && (config_.closeCloak == true || config_.closeCloak == false) ) ? config_.closeCloak : false;

        let heightBody_ = (typeof(config_.heightBody) != 'undefined') ? config_.heightBody : '';

        if(url_) {

            const measures_ = {
                small: 400
                , medium: 600
                , big: 800
            }

            const sizeButtomClose_ = 16

            const closeModal_ = (closeModalClass_) ? 'close-kromModal' : '';

            let cloak_ = $('<div>', {'class': 'cloak ' + closeModal_ + ' d-flex ai-center jc-center'})

            let modal_ = $('<div>', {'class': 'krom-modal-content content-hidden', 'style': 'width: ' + measures_[size_] + 'px; background-color: #ffffff; border-radius: 4px; padding: 10px;'})

            let modalHeader_ = $('<div>', {'class': 'krom-modal-header d-flex jc-spaceBetween with-border-important'})

            let modalTitle_ = $('<h4>', {'style': 'width: 90%; margin-left: 25px;'}).html('<b>' + title_ + '</b>')

            let boxModalClose_ = $('<span>', {'class': '', 'style': ''}).html(_kromComponents.ObtenerSVG('close', '#b0bec5', sizeButtomClose_, 'cur-pointer'))

            let modalClose_ = $('<div>', {'class': 'd-flex ai-center jc-center', 'style': 'width: 10%;'})

            let modalBody_ = $('<div>', {'class': 'krom-modal-body kromModal-body-collapsed'})

            let modalFooter_ = $('<div>', {'class': 'krom-modal-footer'})

            let buttonSave_ = $('<button>', {'class': 'btn btn-success pull-right'}).html('Guardar')

            let buttonCancel_ = $('<button>', {'class': 'close-kromModal btn btn-default pull-right mr-5'}).html('Cancelar')

            buttonSave_.appendTo(modalFooter_)

            buttonCancel_.appendTo(modalFooter_)

            modalTitle_.appendTo(modalHeader_)

            boxModalClose_.appendTo(modalClose_)
            
            modalClose_.appendTo(modalHeader_)

            modalHeader_.appendTo(modal_)

            modalBody_.appendTo(modal_)

            if(!hideFooter_)
                modalFooter_.appendTo(modal_)

            modal_.appendTo(cloak_)

            $('body').prepend(cloak_)

            modalBody_.load(url_, function(){

                modal_.addClass('ghost-content-down')

                if(heightBody_ == '')
                    modalBody_.addClass('kromModal-body')
                else
                    modalBody_.animate({height: '200px'}, 200);

                _package.LoadInitialEvents()

                if(typeof(config_.loadData) !== 'undefined') {

                    if(typeof(config_.loadData) === 'function') {

                        // AQUI VA ALGO QUE HAGA RETORNAR LOS VALORES DE UNA CONSULTA
                        // O HACER ALGO QUE HAGA QUE SE CARGUEN AUTOMATICAMENTE
                        
                        let formElements_ = {}

                        $('[returnable]').each(function (index_, element_) {
                            
                            formElements_[element_.id] = element_
                            
                        });

                        config_.loadData(formElements_)

                    }

                }

                if(typeof(config_.save) !== 'undefined') {

                    if(typeof(config_.save) === 'function') {

                        buttonSave_.click(function (e) { 

                            e.preventDefault();

                            let formElements_ = {}

                            $('[returnable]').each(function (index_, element_) {
                                
                                formElements_[element_.id] = element_
                                
                            });

                            config_.save(formElements_, cloak_)
                            
                        });

                    }

                }

            });

            /* *** - *** Modal svets *** - *** */

            $('.close-kromModal').each(function (index_, element_) {

                $(this).click(function (e_) { 

                    e_.preventDefault();

                    let target_ = $(e_.target)
                    
                    if(target_.is(element_)) {

                        $('[removeScript]').remove();

                        $(cloak_).remove();

                    } 
                    
                });

            });

            $(boxModalClose_).click(function (e_) { 
                
                e_.preventDefault();

                $(cloak_).remove();
                
            });

        } else {

            $.KromMessage('warning', 'La uri no esta definida')

        }

    }

    this.KromDataForm = ({url_ = '', fields_ = {}, response_ = 'undefined', loadEffect_ = false}) => {

        let data_ = ''
        
        if (Object.keys(fields_).length > 0) {

            data_ = JSON.stringify({ 'data_': fields_ })

        }
        
        if (url_ != '') {

            $.ajax({
                
                type: "POST"
                
                , url: url_
                
                , data: data_
                
                , contentType: 'application/json; charset=utf-8'
                
                , dataType: 'JSON'
                
                , error: function (xhr_, ajaxOptions_, thrownError_) {
                
                    console.log(xhr_.status + " \n" + xhr_.responseText, "\n" + thrownError_);
                
                }
                
                , beforeSend: function () {
                
                    if (loadEffect_)

                        $("#loading").css("display", "inherit");

                }

                , success: function (res_) {

                    var res_ = JSON.parse(res_.d)

                    if (typeof(response_) !== 'undefined')

                        if (typeof(response_) === 'function')
                
                        response_(res_)
                }
                
                , complete: function () {
                    
                    if (loadEffect_)

                        $("#loading").css("display", "none");
                
                }

            })

        } else {

            $.KromMessage('warning', 'La uri no esta definida')

        }

    }

    this.LoadInitialEvents = () => {

        $('.date-picker').datepicker({
            format: "dd/mm/yyyy"
            , clearBtn: true
            , language: "es"
            , autoclose: true
            , todayHighlight: true
            
        })
    
        // DATE PICKER EVENTS
        $('.daterange-btn').each(function (index, el) {

            var daterange = $(this).attr('id');
    
            $('#' + daterange).attr('name', daterange)
    
            $('#' + daterange).daterangepicker(
                {
                    ranges: {
                        "Limpiar": [moment().subtract(2, 'days'), moment()],
                        'Hoy': [moment(), moment()],
                        'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Últimos 7 días': [moment().subtract(6, 'days'), moment()],
                        'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
                        'Este mes': [moment().startOf('month'), moment().endOf('month')],
                        'Ultimo mes': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    }
                    , autoUpdateInput: false
                    , "locale": {
                        "applyLabel": "Aplicar"
                        , "cancelLabel": "Cancelar"
                        , "format": "DD/MM/YYYY"
                        , "customRangeLabel": "Rango"
                        , "daysOfWeek": [
                            "Do",
                            "Lu",
                            "Ma",
                            "Mi",
                            "Ju",
                            "Vi",
                            "Sa"
                        ]
                        , "monthNames": [
                            "Enero",
                            "Febrero",
                            "Marzo",
                            "Abril",
                            "Mayo",
                            "Junio",
                            "Julio",
                            "Agosto",
                            "Septiembre",
                            "Octubre",
                            "Noviembre",
                            "Diciembre"
                        ]
                    }
                    //, startDate: moment().subtract(29, 'days')
                    //, endDate: moment()
                }
                , function (start, end, chosen_date) {
                    var emptyDate_ = start._pf.empty;
                    var rangeLabel_ = $(this)[0].chosenLabel
                    
                    if (emptyDate_ != true && rangeLabel_ != "Limpiar") {
                        $('#' + daterange + ' span').html(start.format('D MMMM YYYY') + ' - ' + end.format('DD MMMM YYYY'))
                        $('#' + daterange + ' span').attr("start-date", start.format('YYYY-MM-DD'))
                        $('#' + daterange + ' span').attr("end-date", end.format('YYYY-MM-DD'))
                    } else {
                        var dateField_ = $('#' + daterange).parent().parent().find('label').html()
                        $('#' + daterange + '.daterange-btn span').html('<i class="fa fa-calendar"></i>' + dateField_)
    
                        $('#' + daterange + '.daterange-btn span').removeAttr("start-date")
                        $('#' + daterange + '.daterange-btn span').removeAttr("end-date")
                    }
                }
                
            )
        });

    }

    /**
     * LLAMADO DE FUNCIONES
    **/
    _package.NombreUsuario();

}

var _package = new Package();

$.fn.extend({
    kromAjax: ({type_ = 'POST'
                , url_ = undefined
                , data_ = undefined
                , contentType_ = 'application/json; charset=utf-8'
                , dataType_ = 'JSON'
                , success = (params) => {}
    }) => {

        $.ajax({
            type: type_
            , url: url_
            , data: data_
            , contentType: contentType_
            , dataType: dataType_
            , error: function (xhr, ajaxOptions, thrownError) {
    
                console.log(xhr.status + " \n" + xhr.responseText, "\n" + thrownError);
    
            }
            , beforeSend: function () {
    
                $("#loading").css("display", "inherit");
    
            }
            , success: function (response_) {
                    
                if (typeof(success) !== 'undefined')
    
                    if (typeof(success) === 'function')
                    
                        success(response_.d)
                    
            }
            , complete: function () {
    
                $("#loading").css("display", "none");
    
            }
        })

    }
})

$.extend({

    kromDataTable: function (data) {
        
        _package.kromDataTable(data)
        
    }
        
    , KromMessage: function (type, text, delay) {
        
        _package.message(type, text, delay)
        
    }
        
    , KromClausulasLibres: function (data, clausulas_) {
        
        _package.KromClausulasLibres(data, clausulas_)
        
    }
        
    , KromForm: function (config, method_) {
        
        _package.KromForm(config, method_)
        
    }
        
    , getDate: function (format_) {
        
        _package.getDate(format_)
        
    }
        
    , KromFormWizard: function(config_, objectRequet_) {
        
        
        _package.KromFormWizard(config_, objectRequet_)
        
    }
        
    , KromModal: function(config_) {

        _package.KromModal(config_)

    }

    , KromDataForm: function ({url_ = '', fields_ = {}, response_ = 'undefined', loadEffect_ = false}) {
            
        _package.KromDataForm({url_: url_, fields_: fields_, response_: response_, loadEffect_: loadEffect_})

    }

});
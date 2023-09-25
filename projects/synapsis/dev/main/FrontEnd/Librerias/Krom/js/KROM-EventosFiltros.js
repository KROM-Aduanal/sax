(function () {
    
    //Initialize Select2 Elements
    $('.select2').select2()

    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })

    //Datemask2 mm/dd/yyyy
    $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })

    //Money Euro
    $('[data-mask]').inputmask()

    //Date range picker
    $('#reservation').daterangepicker()

    //Date range picker with time picker
    $('#reservationtime').daterangepicker({ timePicker: true, timePickerIncrement: 30, format: 'MM/DD/YYYY h:mm A' })

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    })

    //Red color scheme for iCheck
    $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    })

    //Flat red color scheme for iCheck
    $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    })

    //Colorpicker
    $('.my-colorpicker1').colorpicker()

    //color picker with addon
    $('.my-colorpicker2').colorpicker()

    //Timepicker
    $('.timepicker').timepicker({
        showInputs: false
    })

    //$('.daterange-btn').daterangepicker(
    //    {
    //        ranges: {
    //            //'None': ['', ''],
    //            'Today': [moment(), moment()],
    //            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    //            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
    //            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
    //            'This Month': [moment().startOf('month'), moment().endOf('month')],
    //            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    //        },
    //        startDate: moment().subtract(29, 'days'),
    //        endDate: moment()
    //    },
    //    function (start, end) {
    //        var emptyDate_ = start._pf.empty;
    //        if (!emptyDate_) {
    //            $('.daterange-btn span').html(start.format('D MMMM YYYY') + ' - ' + end.format('D MMMM YYYY'))
    //            $('.daterange-btn span').attr("start-date", start.format('YYYY-MM-D'))
    //            $('.daterange-btn span').attr("end-date", end.format('YYYY-MM-D'))
    //        }
    //    }
    //)

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
  
    //Date picker
    $('.datepicker').datepicker({
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "es",
        autoclose: true
    })

})()

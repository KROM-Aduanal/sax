class GeneradorGraficas{

    constructor(){

        this._galeriaColores = []

        this._galeriaColoresKrom = ["#40a060", "#20a0c0", "#60c0a0", "#60a0c0", "#40c080", "#0080c0", "#00a0a0", "#0080a0", "#00c0a0", "#00a0c0", "#40c0a0", "#40c0c0", "#40a080", "#60c0c0", "#60c080"]

        this._galeriaColoresGesto = ['#009688', '#00BCD4', '#26A69A', '#26C6DA', '#00897B', '#00ACC1', '#4DB6AC', '#4DD0E1', '#00796B', '#0097A7', '#80CBC4', '#80DEEA', '#00695C', '#00838F', '#006064']

        switch (localStorage.getItem('skin')) {

            case 'skin-blue':

                this._galeriaColores = this._galeriaColoresKrom

                break;

            case 'skin-black':

                this._galeriaColores = this._galeriaColoresGesto

                break;

            default:

                this._galeriaColores = this._galeriaColoresKrom
        }

    }

    /*Creación de gráficas de pastel*/
    CrearGraficaDona(div_,
                     labelColor_,
                     labelValor_, 
                     valorData_, 
                     response_,
                     arrayColoresPropia_){

        var source_ = response_.d

        var posicion_ = 0

        var arrayData_ = []

        var arrayColores_ = []

        if (arrayColoresPropia_ === undefined){

              var galeriaColores_ = this._galeriaColores

        }

        else{

              var galeriaColores_ = arrayColoresPropia_
        
        }

        $.each(source_, function (index, valor) {

              arrayData_.push({ label: valor[labelValor_], value: valor[valorData_] })

              arrayColores_.push(galeriaColores_[posicion_])

              posicion_ = posicion_ + 1

              if (posicion_ == galeriaColores_.length) {

                      posicion_ = 0

              }

        });

        $('#'+div_).empty()

       Morris.Donut({

                  element: div_,

                  resize: true,

                  colors: arrayColores_,

                  labelColor: labelColor_,

                  data: arrayData_,

                  hideHover: 'auto'

       });

    }
    /*Creación de gráficas de pastel*/

    /*Creación de gráficas de Area, Linea, Bar Y barra doble*/
    CrearGraficaAreaLineBarStacked(div_,
                                   labelColor_,
                                   labelValor_, 
                                   valoresData_,
                                   valoresEtiquetas,
                                   response_, 
                                   tipoGrafica_,
                                   arrayColoresPropia_){

        var source_ = response_.d

        var posicion_ = 0

        var arrayData_ = []

        var arrayColores_ = []
            
        if (arrayColoresPropia_ === undefined){

             var galeriaColores_ = this._galeriaColores

        }

        else{

             var galeriaColores_ = arrayColoresPropia_

        }

        $.each(source_, function (index, valor) {

            switch (valoresData_.length) {

                case 1:

                    arrayData_.push({ label: valor[labelValor_], 
                                      value1: valor[valoresData_[0]]})

                    break

                case 2:

                    arrayData_.push({ label: valor[labelValor_], 
                                      value1: valor[valoresData_[0]], 
                                      value2: valor[valoresData_[1]]})

                    break

                case 3:

                    arrayData_.push({ label: valor[labelValor_], 
                                      value1: valor[valoresData_[0]], 
                                      value2: valor[valoresData_[1]],
                                      value3: valor[valoresData_[2]]})

                    break

                case 4:

                    arrayData_.push({ label: valor[labelValor_], 
                                      value1: valor[valoresData_[0]], 
                                      value2: valor[valoresData_[1]],
                                      value3: valor[valoresData_[2]],
                                      value4: valor[valoresData_[3]]})

                    break

                default:
                    
                    arrayData_.push({ label: valor[labelValor_], value1: valor[valoresData_[0]]})

                    break

            }

        });

        var config_ = {

                      data: arrayData_,

                      labelColor: labelColor_,

                      xkey: 'label',

                      fillOpacity: 0.6,

                      hideHover: 'auto',

                      behaveLikeLine: true,

                      resize: true,

                      pointFillColors:['#ffffff'],

                      pointStrokeColors: ['black'],

                      parseTime: false/*,
                      
                      yLabelFormat: function (y) { return y.toString()/1000 + 'km'; }*/
            
            
            
            /*,

                      hoverCallback: function (index, options, content) {
                          var row = options.data[index];
                          //assumes you have already calculated the total of your own dataset
                          return (value/1000)+'K';
                      }
            */
            
            
            
            
            /*,



                      options: {
                          scales: {
                              yAxes: [
                                {
                                    ticks: {
                                        callback: function(valor, index, valores) {
                                            return Number(valor).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                                        }
                                    },
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Cantidad'
                                    }
                                }
                              ],
                              xAxes: [
                                {
                                    scaleLabel: {
                                        display: true,
                                        labelString: 'Días de la semana'
                                    }
                                }
                              ]
                          }
                      }*/

        };
        
        switch (valoresData_.length) {

            case 1:

                config_.ykeys = ['value1']

                config_.labels = [valoresEtiquetas[0]]

                config_.lineColors = [this._galeriaColores[0]]

                break

            case 2:

                config_.ykeys = ['value1', 'value2']

                config_.labels = [valoresEtiquetas[0], valoresEtiquetas[1]]

                config_.lineColors = [this._galeriaColores[0], this._galeriaColores[1]]

                break

            case 3:

                config_.ykeys = ['value1', 'value2', 'value3']

                config_.labels = [valoresEtiquetas[0], valoresEtiquetas[1], valoresEtiquetas[2]]

                config_.lineColors = [this._galeriaColores[0], this._galeriaColores[1], this._galeriaColores[2]]

                break

            case 4:

                config_.ykeys = ['value1', 'value2', 'value3', 'value4']

                config_.labels = [valoresEtiquetas[0], valoresEtiquetas[1], valoresEtiquetas[2], valoresEtiquetas[3]]

                config_.lineColors = [this._galeriaColores[0], this._galeriaColores[1], this._galeriaColores[2], this._galeriaColores[3]]

                break

            default:
                    
                config_.ykeys = ['value1']

                config_.labels = [valoresEtiquetas[0]]

                config_.lineColors = [this._galeriaColores[0]]

                break

        }

        $('#'+div_).empty()

        config_.element = div_

        switch (tipoGrafica_) {

            case 'Area':

                Morris.Area(config_)

                break

            case 'Line':

                Morris.Line(config_)

                break

            case 'Bar':

                Morris.Bar(config_)

                break

            case 'Stacked':

                config_.stacked = true

                Morris.Bar(config_)

                break

            default:
                   
                Morris.Bar(config_)

                break

        }

    }
    /*Creación de gráficas de Area, Linea, Bar Y barra doble*/

}
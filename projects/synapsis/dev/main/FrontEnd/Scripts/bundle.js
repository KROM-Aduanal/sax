import {jquery} from '/FrontEnd/Librerias/JQuery/jquery.3.2.1.min.js'

//import {tetherLibrery} from '/FrontEnd/Liberias/Tether/tether.min.js'

import {bootstrap} from '/FrontEnd/Librerias/Bootstrap/bootstrap.min.js'
import {bootstrapSwitch} from '/FrontEnd/Librerias/Bootstrap/bootstrap-switch.js'
import {nouiSlider} from '/FrontEnd/Librerias/NouiSlider/nouislider.min.js'
import {bootstrapDatePicker} from '/FrontEnd/Librerias/Bootstrap/bootstrap-datepicker.js'

//import {uiKit} from '/FrontEnd/Liberias/Bootstrap/now-ui-kit.js'

import {kromPlugins} from '/FrontEnd/Librerias/Krom/KROM-Plugins.js'
import {KromComponentes} from '/FrontEnd/Librerias/Krom/KromComponentes.js'

export function bundle () {
    jquery()
    //tetherLibrery()
    bootstrap()
    bootstrapSwitch()
    nouiSlider()
    bootstrapDatePicker()
    //uiKit()
    kromPlugins()
    KromComponentes()
}
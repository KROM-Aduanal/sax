import {jquery} from '/FrontEnd/Liberias/JQuery/jquery.3.2.1.min.js'

//import {tetherLibrery} from '/FrontEnd/Liberias/Tether/tether.min.js'

import {bootstrap} from '/FrontEnd/Liberias/Bootstrap/bootstrap.min.js'
import {bootstrapSwitch} from '/FrontEnd/Liberias/Bootstrap/bootstrap-switch.js'
import {nouiSlider} from '/FrontEnd/Liberias/NouiSlider/nouislider.min.js'
import {bootstrapDatePicker} from '/FrontEnd/Liberias/Bootstrap/bootstrap-datepicker.js'

//import {uiKit} from '/FrontEnd/Liberias/Bootstrap/now-ui-kit.js'

import {kromPlugins} from '/FrontEnd/Liberias/Krom/KROM-Plugins.js'
import {KromComponentes} from '/FrontEnd/Liberias/Krom/KromComponentes.js'

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
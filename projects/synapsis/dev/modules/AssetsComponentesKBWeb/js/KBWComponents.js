//Extends Default Prototypes

import  * as Extends  from './components/kbw-prototypes.js';

//Import Components

import { KBWInput } from './components/kbw-input.js';
import { KBWInputDate } from './components/kbw-input-date.js';
import { KBWTextArea } from './components/kbw-textarea.js';
import { KBWSwitch } from './components/kbw-switch.js';
import { KBWSelect } from './components/kbw-select.js';
import { KBWCheckbox } from './components/kbw-checkbox.js';
import { KBWRadio } from './components/kbw-radio.js';
import { KBWFindbar } from './components/kbw-findbar.js';
import { KBWForm } from './components/kbw-form.js';
import { KBWTable } from './components/kbw-table.js';
import { KBWButton } from './components/kbw-button.js';
import { DisplayAlert, DisplayToast, DisplayMessage, DisplayActionSheet } from './components/kbw-dialogs.js';

//Defined Components

customElements.define('kbw-input', KBWInput, { extends: 'input' });

customElements.define('kbw-input-date', KBWInputDate, { extends: 'input' });

customElements.define('kbw-textarea', KBWTextArea, { extends: 'textarea' });

customElements.define('kbw-switch', KBWSwitch, { extends: 'input' });

customElements.define('kbw-select', KBWSelect, { extends: 'select' });

customElements.define('kbw-checkbox', KBWCheckbox, { extends: 'input' });

customElements.define('kbw-radio', KBWRadio, { extends: 'input' });

customElements.define('kbw-findbar', KBWFindbar, { extends: 'input' });

customElements.define('kbw-form', KBWForm, { extends: 'form' });

customElements.define('kbw-table', KBWTable, { extends: 'table' });

customElements.define('kbw-button', KBWButton, { extends: 'button' });

//Initialize Utilities

window.DisplayAlert = DisplayAlert;

window.DisplayToast = DisplayToast;

window.DisplayMessage = DisplayMessage;

window.DisplayActionSheet = DisplayActionSheet;

//Initialize External Libraries

$("form").niceScroll();
$("body").niceScroll();
$('.select2').select2();
$('.datepicker').datepicker({autoclose: true});


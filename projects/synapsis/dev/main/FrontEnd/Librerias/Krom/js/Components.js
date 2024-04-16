
//Import Components
import { WCToolTip } from './components/webcomponent-tooltip.js';

import { WCForm } from './components/webcomponent-form.js';

import { WCCatalog } from './components/webcomponent-catalog.js';

import { WCFile } from './components/webcomponent-file.js';

import { WCFindbar } from './components/webcomponent-findbar.js';

import { WCFindbox } from './components/webcomponent-findbox.js';

import { WCSelect } from './components/webcomponent-select.js';

import { WCTable } from './components/webcomponent-table.js';

import { WCCollectionView } from './components/webcomponent-collectionview.js';

import { WCImage } from './components/webcomponent-image.js';

import { WCInput } from './components/webcomponent-input.js';

import { WCListbox } from './components/webcomponent-listbox.js';

import { WCPillbox } from './components/webcomponent-pillbox.js';

import { WCLabel } from './components/webcomponent-label.js';

import { WCComment } from './components/webcomponent-comment.js';

import { WCUserData } from './components/webcomponent-userdata.js';

import { WCFEditor } from './components/webcomponent-feditor.js';

//Defined Components

customElements.define('wc-tooltip', WCToolTip, { extends: 'input' });

customElements.define('wc-form', WCForm, { extends: 'form' });

customElements.define('wc-catalog', WCCatalog, { extends: 'table' });

customElements.define('wc-file', WCFile, { extends: 'input' });

customElements.define('wc-findbar', WCFindbar, { extends: 'input' });

customElements.define('wc-findbox', WCFindbox, { extends: 'input' });

customElements.define('wc-select', WCSelect, { extends: 'select' });

customElements.define('wc-table', WCTable, { extends: 'table' });

customElements.define('wc-collectionview', WCCollectionView, { extends: 'ul' });

customElements.define('wc-image', WCImage, { extends: 'div' });

customElements.define('wc-input', WCInput, { extends: 'input' });

customElements.define('wc-listbox', WCListbox, { extends: 'input' });

customElements.define('wc-pillbox', WCPillbox, { extends: 'div' });

customElements.define('gwc-label', WCLabel);

customElements.define('gwc-userdata', WCUserData);

customElements.define('gwc-comment', WCComment);

customElements.define('wc-feditor', WCFEditor, { extends: 'div' });


//Initialize External Libraries

//$(".content-wrapper-page").niceScroll();

$(document).on('click', 'legend label', function (e) {

    const legend = $(this).closest('legend');

    const input = legend.next('input');

    input.click();

});

//DOBLE SUBMIT CONTROLLER
$('button, input:submit','#form1').click(e => e.target.disabled = true);

//DATEPICKER
$(document).on('focusin', '.datepicker', function () {
    $(this).datepicker({
        autoclose: true, format: 'yyyy-mm-dd',
    });
}).on('changeDate','.datepicker', function (e) {
    this.dispatchEvent(new Event('change'));
});

//TIMEPICKER
$(document).on('focusin', '.timepicker', function () {
	$(this).timepicker({ showInputs: false });
}).on('change', '.timepicker', function (e) {
    this.dispatchEvent(new Event('change'));
});

//FORMATS
$(document).on('focusin', '[data-mask]', function () {
    $(this).inputmask();
});

//NUMERIC FORMAT
$(document).on('propertychange input', '.numeric', function () {
    var input = $(this);
    input.val(input.val().replace(/[^\d]+/g, ''));
});

//REAL FORMAT
$(document).on('keypress', '.real', function (evt) {

    var input = $(this).get(0);
    
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        //Check if the text already contains the . character
        if (input.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31 &&
            (charCode < 48 || charCode > 57))
            return false;
    }
    return true;

});

//MONEY FORMAT
$(document).on('keyup', '.currency', e => formatCurrency($(e.target)));

$(document).on('blut', '.currency', e => formatCurrency($(e.target), 'blur'));

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        input_val = "$" + left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
        input_val = "$" + input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}


//OPEN BLOCKED INPUT MODAL
$('body').on('click', '.lock-input', function (e) {

    if ($(this).get(0).hasAttribute('pbref')) {

        e.preventDefault();

        DisplayPrompt('Desbloquear', 'Ingrese el c&oacute;digo de seguridad para desbloquear este control', null, null, (value_) => {

            if (value_) {

                $(this).get(0).setAttribute('onclick', $(this).attr('pbref').replace("__argument", value_));

                $(this).get(0).removeAttribute('pbref');

                setTimeout(() => $(this).click(), 0);

            }

        });

    }

});

/*$('body').on('focus', '.__value', function (e) {

    const len = e.target.value.length;

    setTimeout(function () { e.target.setSelectionRange(len, len); }, 0);

});*/

$(document).click((event) => {
    if (!$(event.target).closest('.__reminder').length) {
        $('.__reminder').remove();
    }
});



$('body').on('click', '.__down', function (e) {

    const component = e.target.closest('.wc-collectionview') || false;

    if (component) {

        if (!e.target.hasAttribute('in-process')) {

            e.preventDefault();

            e.target.setAttribute('in-process', 'true');

            const element = e.target.closest('[fieldid]');

            const jsonString = component.querySelector('.__data');

            const indice = element.parentNode.querySelector('.__item').getAttribute('itemid');

            try {

                const data = JSON.parse(jsonString.value);

                let result = data.filter(a => a[component.getAttribute('keyfield')] == indice);

                result[0][element.getAttribute('fieldid')]['Dropdowm'] = new Boolean(true);
                //!result[0][element.getAttribute('fieldid')]['Dropdowm'];

                jsonString.value = JSON.stringify(data);

                e.target.click();

            } catch (e) { console.log(e) }

        }

    }

});

$('body').on('change', '.wc-collectionview input[id], .wc-collectionview textarea[id]', (e) => {

    const component = e.target.closest('.wc-collectionview') || false;

    if (component) {

        OnCollectionViewControlChanged(e.target);

    }

});

window.onSelectChange = (e) => {

    const component = e.closest('.wc-collectionview') || false;

    if (component) {

        OnCollectionViewControlChanged(e);

    }
    
};

function OnCollectionViewControlChanged(e) {

    const element = e.closest('[fieldid]')

    if (element.classList.contains('wc-catalog') == false) {

        const component = e.closest('.wc-collectionview');

        const jsonString = component.querySelector('.__data');

        const indice = element.parentNode.querySelector('.__item').getAttribute('itemid');

        try {

            const data = JSON.parse(jsonString.value);

            let result = data.filter(a => a[component.getAttribute('keyfield')] == indice);

            if (e.type.toLowerCase() == 'select-one') {

                result[0][element.getAttribute('fieldid')]['Value'] = e.value;

                result[0][element.getAttribute('fieldid')]['Text'] = e.options[e.selectedIndex].text;

                result[0][element.getAttribute('fieldid')]['Dropdowm'] = !result[0][element.getAttribute('fieldid')]['Dropdowm'];

            } else {

                result[0][element.getAttribute('fieldid')] = e.value;

            }

            jsonString.value = JSON.stringify(data);

        } catch (e) { console.log(e) }

    }

}

window.onCatalogChange = (e) => OnCatalogControlChange(e); 

function OnCatalogControlChange(e) {
    
    const collection = e.closest('.wc-collectionview') || false;

    if (collection) {

        const fieldid = e.parentNode.getAttribute('fieldid');

        const dataCatalogo = e.jsonString.value;

        const data = collection.querySelectorAll('.__data');

        const jsonString = data[data.length - 1];

        const indice = e.parentNode.parentNode.querySelector('.__item').getAttribute('itemid');

        try {

            const data = JSON.parse(jsonString.value);

            let result = data.filter(a => a[collection.getAttribute('keyfield')] == indice);

            result[0][fieldid] = dataCatalogo;

            jsonString.value = JSON.stringify(data);

        } catch (e) { console.log(e) }

    }

}

/*
//generate a random ID, doesn't really matter how    
if (!sessionStorage.tab) {
    var max = 99999999;
    var min = 10000000;
    sessionStorage.tab = Math.floor(Math.random() * (max - min + 1) + min);
}

//set tab_id cookie before leaving page / beforeunload
window.addEventListener('load', function () {
    //document.cookie = 'tab_id=' + sessionStorage.tab;
    $('.TabIdentifier').val(sessionStorage.tab);
});
*/


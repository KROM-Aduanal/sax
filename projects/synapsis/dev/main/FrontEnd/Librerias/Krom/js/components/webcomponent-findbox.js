export class WCFindbox extends HTMLInputElement {

    set selectedIndex(value) {

        const item = this.listItems[value-1];

        if (item) {

            this.keyField.value = item.id;

            this.valueField.value = item.innerText;

        }
       
    }

    constructor() {

        super();

    }

    connectedCallback() {

        this.typingTimer            = null;

        this.doneTypingInterval     = 1000;

        this.component              = this.closest('.__component');

        this.clearButton            = this.component.querySelector('.__close');

        this.keyField               = this.component.querySelector('.__key');

        this.valueField             = this.component.querySelector('.__value');

        this.listItems              = this.component.querySelectorAll('a.link-unstyled') || [];

        this.dropdown               = this.component.querySelector('.__dropdown')

        const contextForm           = this.component.querySelector('.__add');

        const contextList           = this.component.querySelector('.__find');

        if (this.valueField.value) {

            this.clearButton.classList.remove('d-none');

        }
        
        this.valueField.addEventListener('keyup', e => this.controlTyping(e));

        this.listItems.addEventListener("click", (e) => this.controlItemSelected(e));

        if (contextForm) contextForm.addEventListener("click", e => this.appendFormContext(e));

        if (contextList) contextList.addEventListener("click", e => this.appendListContext(e));

        if (sessionStorage.getItem(this.valueField.getAttribute("name"))) {

            const len = this.valueField.value.length;
           
            this.valueField.focus();

            this.valueField.setSelectionRange(len, len);

            sessionStorage.removeItem(this.valueField.getAttribute("name"));

        }
        
        if (this.getAttribute('disabled')) this.closest('.row').setAttribute('style', 'border-color:#cecdcd !important;');

    }

    controlTyping(e) {

        if (e.keyCode == 8 && this.keyField.value) {

            this.keyField.value = '';

            this.valueField.value = '';

            this.clearButton.classList.add('d-none');

        } else {

            const input = e.target;

            const value = input.value;

            clearTimeout(this.typingTimer);

            if (value) {

                this.clearButton.classList.remove('d-none');

                this.typingTimer = setTimeout(() => {
                    
                    sessionStorage.setItem(this.valueField.getAttribute("name"), '__focus');

                    input.blur();

                }, this.doneTypingInterval);

            }

        }

    }

    controlItemSelected(e) {
        
        e.preventDefault();

        this.keyField.value = e.target.id;

        this.valueField.value = e.target.innerText;

        if ("createEvent" in document) {

            var evt = document.createEvent("HTMLEvents");

            evt.initEvent("change", false, true);

            this.keyField.dispatchEvent(evt);

        } else {

            this.keyField.fireEvent("onchange");
          
        }
        
    }

    appendFormContext(e) {

        e.preventDefault();

        const formcontrol = document.querySelector('#form1');
        
        formcontrol.context.push(this.getAttribute('template'));

    }

    appendListContext(e) {

        e.preventDefault();

        const formcontrol = document.querySelector('#form1');

        this.getCatalogDetailData(data => formcontrol.context.detail(data, this));

    }

    getCatalogDetailData(func) {

        //const method_ = this.getAttribute('id') + '_PreparaCatalogo';
        const method_ = 'PreparaCatalogo';

        PageMethods[method_](null, (data) => {

            if (typeof func === 'function') {

                func(data);

            }

        });

    }

}

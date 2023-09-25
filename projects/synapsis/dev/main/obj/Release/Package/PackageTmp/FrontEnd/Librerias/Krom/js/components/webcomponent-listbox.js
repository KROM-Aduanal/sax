export class WCListbox extends HTMLInputElement {

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

        this.keyField               = this.component.querySelector('.__key');

        this.valueField             = this.component.querySelector('.__value');

        this.listItems              = this.component.querySelectorAll('ul.__content li') || [];

        this.listItemsDelete        = this.component.querySelectorAll('ul.__items a') || [];

        this.dropdown               = this.component.querySelector('.__dropdown')

        this.valueField.addEventListener('keyup', e => this.controlTyping(e));

        this.listItems.addEventListener("click", (e) => this.controlItemSelected(e));

        this.listItemsDelete.addEventListener("click", (e) => this.controlItemDeleted(e));

        
    }

    controlTyping(e) {

        const input = e.target;

        const value = input.value;

        clearTimeout(this.typingTimer);

        if (value) {

            this.typingTimer = setTimeout(() => {

                input.blur();

            }, this.doneTypingInterval);

        }

    }

    controlItemSelected(e) {

        e.preventDefault();

        e.stopPropagation();

        var jsonData_ = []

        try {

            jsonData_ = JSON.parse(this.keyField.value) || [];

        } catch (e) { }

        var indexPath = 0

        /*const items = this.component.querySelectorAll('.__items li') || false;

        if (items.length > 0) {
           
            const last = items[items.length-1];
            
            if (last) {

                indexPath = last.getAttribute('index-path');

            }
            
        }*/

        // new Number(indexPath) + 1
        jsonData_.push({ Indice: new Number(indexPath), Value: e.target.id, Text: e.target.innerText, Delete: new Boolean(false)});

        this.keyField.value = JSON.stringify(jsonData_);

        this.valueField.value = e.target.innerText;

        if ("createEvent" in document) {

            var evt = document.createEvent("HTMLEvents");

            evt.initEvent("change", false, true);

            this.keyField.dispatchEvent(evt);

        } else {

            this.keyField.fireEvent("onchange");
          
        }
        
    }

    controlItemDeleted(e) {

        e.preventDefault();

        e.stopPropagation();

        try {

            var jsonData_ = JSON.parse(this.keyField.value);

            var nodes = Array.from(this.component.querySelectorAll('.__items li'));
            
            let selected = -1;
            if (selected !== nodes.indexOf(e.target.closest('li'))) {
                selected = nodes.indexOf(e.target.closest('li'));
            } 

            jsonData_[selected].Delete = new Boolean(true);
            
            this.keyField.value = JSON.stringify(jsonData_);

            if ("createEvent" in document) {

                var evt = document.createEvent("HTMLEvents");

                evt.initEvent("change", false, true);

                this.keyField.dispatchEvent(evt);

            } else {

                this.keyField.fireEvent("onchange");

            }

        } catch (e) { console.log(e); }

    }

}

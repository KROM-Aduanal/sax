export class WCFindbar extends HTMLInputElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this.typingTimer = null;

        this.doneTypingInterval = 1000;

        this.minimumCharacters = Number(this.getAttribute('minimum-characters')) || 1;

        this.pathname = window.location.pathname.split("/").pop().replace(".aspx", "");

        this.component = this.closest('.__component');

        this.clearButton = this.component.querySelector('.__close');

        this.keyField = this.component.querySelector('.__key');

        this.valueField = this.component.querySelector('.__value');

        this.listItems = this.component.querySelectorAll('.__content a') || [];
        
        this.dropdown = this.component.querySelector('.__dropdown')

        if (this.valueField.value) this.clearButton.classList.remove('d-none');

        this.valueField.addEventListener('keyup', e => this.controlTyping(e));

        this.valueField.addEventListener('focus', e => this.controlFocus(e));

        this.valueField.addEventListener('click', e => e.stopPropagation());

        this.listItems.addEventListener("click", (e) => this.controlItemSelected(e));

        const filters_ = this.component.querySelector('.__filters') || false;

        if (filters_) {

            filters_.addEventListener('mouseover', (e) => { this.component.querySelector('.__filters').classList.remove('contracted') });

            filters_.addEventListener('mouseout', (e) => { this.component.querySelector('.__filters').classList.add('contracted') });

        }

        if (sessionStorage.getItem(this.valueField.getAttribute("name"))) {

            const len = this.valueField.value.length;

            this.valueField.focus();

            this.valueField.setSelectionRange(len, len);

            sessionStorage.removeItem(this.valueField.getAttribute("name"));

        }
        
    }


    controlTyping(e) {

        const input = e.target;

        const value = input.value;

        clearTimeout(this.typingTimer);

        if (value && value.length >= this.minimumCharacters) {

            this.clearButton.classList.remove('d-none');

            this.typingTimer = setTimeout(() => {
                
                this.prepareReminder(value);

                sessionStorage.setItem(this.valueField.getAttribute("name"), '__focus');

                input.blur();

            }, this.doneTypingInterval);

        } 

    }

    controlItemSelected(e) {

        e.preventDefault();

        this.keyField.value = e.target.id;
        
        this.valueField.value = e.target.innerText;
        
        if ("createEvent" in document) {

            var evt = document.createEvent("HTMLEvents");

            evt.initEvent("change", false, true);
            //evt.initEvent("change", true, false);
            
            this.keyField.dispatchEvent(evt);

        } else {
            
            this.keyField.fireEvent("onchange");

        }
        
    }

    controlFocus(e) {
       
        if (!this.closest('.__component').querySelector('.__dropdown')) {

            try {

                const lastWords = JSON.parse(localStorage.getItem(this.pathname)) || [];

                if (lastWords.length > 0) {

                    const ul = document.createElement('ul');

                    ul.setAttribute("class", "row no-gutters __dropdown __reminder");

                    for (let i = 0; i < lastWords.length; i++) {

                        const li = document.createElement('li');

                        const a = document.createElement('a');

                        a.classList.add('col-auto');

                        const atxt = document.createElement('a');

                        atxt.classList.add('col');

                        atxt.append(document.createTextNode(lastWords[i]));

                        li.appendChild(atxt);

                        li.appendChild(a);

                        ul.appendChild(li);

                        atxt.addEventListener('click', (e) => {

                            e.preventDefault();

                            e.stopPropagation();

                            this.valueField.value = e.target.innerText;

                            if ("createEvent" in document) {

                                var evt = document.createEvent("HTMLEvents");

                                evt.initEvent("change", false, true);

                                this.valueField.dispatchEvent(evt);

                            } else {

                                this.valueField.fireEvent("onchange");

                            }

                        });

                        a.addEventListener('click', (e) => {
                           
                            e.preventDefault();

                            e.stopPropagation();
                           
                            try {

                                var lastWords = JSON.parse(localStorage.getItem(this.pathname));

                                const index = lastWords.indexOf(e.target.previousSibling.innerText);

                                if (index >= 0) {

                                    lastWords.splice(index, 1);
                                    
                                    localStorage.setItem(this.pathname, JSON.stringify(lastWords));

                                    if (lastWords.length == 0) {

                                        this.component.querySelector('.__reminder').remove();

                                    }

                                }

                            } catch (e) { }

                            e.target.closest('li').remove();

                        });
                       
                    }

                    this.closest('.__component').append(ul);

                }

            } catch (e) { }

        }

    }
   
    prepareReminder(word) {
        
        try {

            var lastWords = JSON.parse(localStorage.getItem(this.pathname));

            if (!lastWords.includes(word)) {

                lastWords.unshift(word);

                lastWords = lastWords.slice(0, 10);

            }
            
            localStorage.setItem(this.pathname, JSON.stringify(lastWords));

        } catch(e) {
            
            localStorage.setItem(this.pathname, JSON.stringify([word]));

        }

    }

}


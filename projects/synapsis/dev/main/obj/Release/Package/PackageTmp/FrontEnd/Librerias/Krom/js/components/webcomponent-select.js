
export class WCSelect extends HTMLSelectElement {

    constructor() {

        super();

    }

   connectedCallback() {

        this.typingTimer = null;

        this.doneTypingInterval = 1000;

        this.component = this.closest('.__component');

        this.searchInput = this.component.querySelector('.__search') || false;

        this.textInput = this.component.querySelector('input.col');

        const contextForm = this.component.querySelector('.__add');

        const contextList = this.component.querySelector('.__find');

        if (this.searchInput) this.searchInput.addEventListener('keyup', e => this.controlTyping(e));

        if (contextForm) contextForm.addEventListener("click", e => this.appendFormContext(e));

        if (contextList) contextList.addEventListener("click", e => this.appendListContext(e));

        this.fillControl();

        if (this.selectedIndex >= 0)
        this.textInput.value = this.options[this.selectedIndex].text;
       
        try {

            const data = JSON.parse(this.getAttribute('tooltip-sttings'));

            switch (Number(data.mode)) {

                case 1:

                    this.settingClassicToolTip(data);

                    break;
                case 2:

                    this.settingOndemanToolTip(data);

                    break;
                case 3:

                    this.settingInteractiveToolTip(data);

                    break;

            }

        } catch (e) { }

    }

    getToolTipElement(data) {

        const bgcolors = { 1: '#d4edda', 2: '#cce5ff', 3: '#fff3cd', 4: '#f8d7da' };

        const txtcolors = { 1: '#155724', 2: '#004085', 3: '#856404', 4: '#721c24' };

        let tooltip = document.createElement('span');

        tooltip.setAttribute('class', 'tooltip-element');

        if (data.mode > 1) {

            let close = document.createElement('a');

            close.innerText = 'x';

            close.addEventListener('click', (e) => {

                e.preventDefault();

                e.target.style.pointerEvents = 'none';

                tooltip.style.opacity = 0;

                setTimeout(() => tooltip.remove(), 800);

            });

            tooltip.appendChild(close);

        }

        //mejora para colocar el tooltip dinamicamente

        tooltip.style = '--tintColor:' + bgcolors[Number(data.status)] + '; --textColor:' + txtcolors[Number(data.status)];

        let text = document.createTextNode(data.message);

        tooltip.appendChild(text);

        return tooltip;

    }

    settingClassicToolTip(data) {

        this.textInput.addEventListener('mouseenter', () => {

            const oldtooltip = this.component.querySelector('.tooltip-element') || false;

            if (oldtooltip == false) {

                let tooltip = this.getToolTipElement(data);

                this.component.appendChild(tooltip);

                tooltip.style.opacity = 1;

            }

        });

        this.textInput.addEventListener('mouseleave', () => {

            let tooltip = this.component.querySelector('.tooltip-element') || false;

            if (tooltip != false) {

                tooltip.style.opacity = 0;

                setTimeout(() => tooltip.remove(), 800);

            }

        });


    }

    settingOndemanToolTip(data) {
        
        if (data.visible == 'True') {

            let tooltip = this.getToolTipElement(data);

            this.component.appendChild(tooltip);

            tooltip.style.opacity = 1;

            const expiretime = Number(data.expiretime) * 1000;

            if (expiretime > 0) {

                setTimeout(() => {

                    tooltip.style.opacity = 0;

                    setTimeout(() => tooltip.remove(), 800);

                }, expiretime);

            }
        }

    }

    settingInteractiveToolTip(data) {



    }

    fillControl() {

        const items = this.component.querySelector('.__items') || false;

        if (items) {

            items.innerHTML = '';

            const tmp = document.createElement('template');

            tmp.innerHTML = `
			<li>
				<label class="d-block">
					<input value="" name="${this.id}"/>
				</label>
			</li>
		    `;

            for (let i = 0; i < this.options.length; i++) {

                if (this.options[i].value) {

                    let li = tmp.content.cloneNode(true);

                    let label = li.querySelector('label');

                    let input = li.querySelector('input');

                    input.type = this.hasAttribute('multiple') ? 'checkbox' : 'radio';

                    input.value = this.options[i].value;

                    label.appendChild(document.createTextNode(this.options[i].text));

                    input.addEventListener('change', e => this.itemSelected(e.target, i));

                    items.appendChild(li);

                }

            }
            
            if (this.options.length == 0) {
                
                const li = document.createElement('li');

                li.appendChild(document.createTextNode('Sin elementos disponibles'));

                li.classList.add('text-center');

                li.classList.add('p-4');

                items.appendChild(li);

            }

        } 
    }

    itemSelected(item, i) {


        this.selectedIndex = i;

        /**/
        //CUSTOM EVENT
        /**/
        window.onSelectChange(this);

        if ("createEvent" in document) {

            var evt = document.createEvent("HTMLEvents");

            evt.initEvent("change", false, true);

            this.dispatchEvent(evt);
            
        } else {
           
            this.fireEvent("onchange");
            

        }

    }

    controlTyping(e) {
        
        const input = e.target;

        const value = input.value;

        if (!input.hasAttribute('server-mode')) {

            const ul = this.component.querySelector('.__items');

            const items = ul.querySelectorAll('li');

            items.forEach((li) => {

                const string = li.innerText;

                if (string.toLowerCase().indexOf(value.toLowerCase()) == -1) {

                    li.style.display = 'none';

                } else {

                    li.style.display = '';

                }

            });

        } else {

            clearTimeout(this.typingTimer);

            if (value) {

                this.typingTimer = setTimeout(() => {

                    input.blur();
                    
                }, this.doneTypingInterval);

            }

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

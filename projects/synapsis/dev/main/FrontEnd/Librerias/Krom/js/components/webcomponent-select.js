
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

                    input.addEventListener('change', e => this.itemSelected(e.target, i, e));

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

    itemSelected(item, i, evt) {

        this.selectedIndex = i;

        /**/
        //CUSTOM EVENT
        /**/
        window.onSelectChange(this, evt);

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

export class WCPillbox extends HTMLDivElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this._component = this.closest('.__component');

        //this._jsonString = this._component.querySelectorAll('.__data');

        this._jsonString = [].slice.call(this._component.querySelectorAll('.__data')).pop();
       
        const controls_ = this._component.querySelectorAll('input[id], textarea[id], select[id]') || [];

        controls_.addEventListener('change', e => this.prepareJsonData(e));

        this.prepareJsonData(null);

        this.repairCounters();

    }

    prepareJsonData(e) {
       
        try {

            const json_ = JSON.parse(this._jsonString.value);
            
            const filteredJson_ = json_.filter((e) => { return e.borrado == false && e.archivado == false });

            const controls_ = this._component.querySelectorAll('input[id], textarea[id], select[id]') || [];
            
            const page_ = Number(this._jsonString.getAttribute('page'));
            
            controls_.forEach((control_) => {
                
                const container_ = control_.closest('.__component');

                const component_ = container_.parentElement;

                const controlId_ = component_.classList.item(component_.classList.length - 1);

                if (controlId_) {

                    if (container_.classList.contains('wc-switch')) {

                        filteredJson_[page_][controlId_] = control_.checked;

                    } else if (container_.classList.contains('wc-select')) {

                        if (control_.options.length > 0) {

                            const text = (control_.options.length) ? control_.options[control_.selectedIndex].text : '';

                            filteredJson_[page_][controlId_] = { Value: control_.value, Text: text };

                        } else {

                            filteredJson_[page_][controlId_] = { Value: "", Text: "" };

                        }

                    } else if (container_.classList.contains('wc-findbox')) {

                        filteredJson_[page_][controlId_] = { Value: control_.value, Text: container_.querySelector('.__value').value };

                    } else if (container_.classList.contains('wc-catalog')) {

                        const subcontrolId_ = container_.classList.item(container_.classList.length - 1);
                        
                        try {

                            const subjson_ = JSON.parse(container_.querySelector('.__data').value);

                            filteredJson_[page_][subcontrolId_] = subjson_;

                        } catch (e) {

                            filteredJson_[page_][subcontrolId_] = null;

                        }

                    } else {

                        filteredJson_[page_][controlId_] = control_.value;

                    }

                }

            });
            
            json_.forEach((item) => {

                const fitereditem = filteredJson_.filter((el) => Number(el.identidad) == Number(item.identidad));

                if (fitereditem.length) {

                    for (const [key, value] of Object.entries(fitereditem[0])) {

                        item[key] = value;

                    }

                }

            });

            this._jsonString.value = JSON.stringify(json_);
            
        } catch (e) {
            //console.log(e);
        }

    }

    repairCounters() {

        const current = this._component.querySelector('.wc-buttonbar').nextElementSibling;

        const total = this._component.querySelector('.wc-buttonbar').nextElementSibling.nextElementSibling;

        const title = this._component.querySelector('.wc-buttonbar').querySelector('.__preview').nextElementSibling;

        title.innerText = current.value + ' de ' + total.value;

    }

}

export class WCCatalog extends HTMLTableElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this.component = this.closest('.__component');

        this.jsonString = this.component.querySelector('.__data');

        if (this.component.querySelector('input[id="id_"]')) this.keyField = this.component.querySelector('input[id="id_"]').closest('td').id;

        const addBtn = this.component.querySelector('.__add') || false;

        const cloneBtn = this.component.querySelector('.__clone') || false;

        const deleteBtn = this.component.querySelector('.__delete') || false;

        const checkBtns = this.querySelector('.__checks') || false;

        const findText = this.component.querySelector('.__find') || false;

        if (checkBtns) checkBtns.addEventListener('click', e => this.checkRow(e));

        if (addBtn) addBtn.addEventListener('click', e => this.appendRow(e));

        if (cloneBtn) cloneBtn.addEventListener('click', e => this.cloneRow(e));

        if (deleteBtn) deleteBtn.addEventListener('click', e => this.deleteRow(e));

        if (findText) findText.addEventListener('keyup', e => this.findElements(e));

        this.setControlsEvents(this);

    }

    setControlsEvents(region_) {

        const inputs = region_.querySelectorAll('tr.__row input[id], tr.__row textarea[id], tr.__row select[id]') || [];

        inputs.addEventListener('change', (e) => {

            if (!e.target.closest('.__row').length) {

                if (!e.target.hasAttribute('in-process')) {

                    e.preventDefault();

                    e.target.setAttribute('in-process', 'true');

                    this.prepareJsonData();

                    this.prepareJsonDrop(e.target);

                    /**/
                    //CUSTOM EVENT
                    /**/
                    window.onCatalogChange(this);

                    if ("createEvent" in document) {

                        var evt = document.createEvent("HTMLEvents");

                        evt.initEvent("change", false, true);

                        e.target.dispatchEvent(evt);

                    } else {

                        e.target.fireEvent("onchange");

                    }

                }

            } else {

                e.preventDefault();

            }

        });


        const tableRows_ = region_.querySelectorAll('.__row .__down');

        tableRows_.addEventListener('click', (e) => {

            if (!e.target.hasAttribute('in-process')) {

                e.preventDefault();

                e.target.setAttribute('in-process', 'true');

                this.prepareJsonDrop(e.target);

                e.target.click();

            }

        });

        const gestures = region_.querySelectorAll('.__gesture');

        gestures.addEventListener('click', (e) => {

            if (!e.target.hasAttribute('in-process')) {

                e.preventDefault();

                e.target.setAttribute('in-process', 'true');

                try {

                    const dataRows_ = JSON.parse(this.jsonString.value);

                    dataRows_.SelectedItem = null;

                    this.jsonString.value = JSON.stringify(dataRows_);

                } catch (e) { }

                e.target.click();

            }

        });

        const quitBtn = region_.querySelectorAll('.__quit') || false;

        quitBtn.addEventListener('click', (e) => this.quitRow(e));

    }

    //DATA PROCESS

    prepareJsonData() {

        try {

            const json = JSON.parse(this.jsonString.value);

            json.Items = [];

            this.querySelectorAll('tbody tr.__row').forEach((tr) => {

                var row_ = {};

                tr.querySelectorAll('td').forEach((td) => {

                    const control_ = td.querySelector('input[id], textarea[id], select[id]') || false;

                    if (control_) {

                        if (control_.type.toLowerCase() == 'checkbox') {

                            if (control_.id == 'id_') {

                                row_[td.id] = new Number(control_.value);

                            } else {

                                row_[td.id] = new Boolean(control_.checked);

                            }

                        } else if (control_.type.toLowerCase() == 'select-one') {

                            row_[td.id] = {
                                Value: control_.value,
                                Text: control_.options[control_.selectedIndex].text
                            };

                        } else {

                            row_[td.id] = control_.value;

                        }

                    }


                });

                json.Items.push(row_);

            });

            this.jsonString.value = JSON.stringify(json);

        } catch (e) { }
    }

    prepareJsonDrop(e) {

        const column_ = e.closest('td');

        const row_ = e.closest('tr');

        const rowIndex = row_.rowIndex - 2;

        try {

            const dataRows_ = JSON.parse(this.jsonString.value);

            if (!dataRows_.SelectedItem) {

                dataRows_.SelectedItem = {
                    Id: column_.id,
                    Drop: true,
                    RowId: rowIndex
                };

            } else {

                if (dataRows_.SelectedItem.Id == column_.id) {

                    dataRows_.SelectedItem.Drop = !dataRows_.SelectedItem.Drop;

                    dataRows_.SelectedItem.RowId = rowIndex;

                    dataRows_.LastRowInteraction = rowIndex;

                } else {

                    dataRows_.SelectedItem.Id = column_.id;

                    dataRows_.SelectedItem.Drop = true;

                    dataRows_.SelectedItem.RowId = rowIndex;

                }

            }

            this.jsonString.value = JSON.stringify(dataRows_);

        } catch (e) { }

    }

    //CATALOG DEFAULT EVENTS

    //Add Row
    appendRow(e) {

        e.preventDefault();

        var json_ = {};

        try {

            json_ = JSON.parse(this.jsonString.value);

        } catch (e) {

            json_ = {
                Items: [],
                DropItems: null,
                SelectedItem: null,
                LastRowInteraction: -1
            };

        }

        const dataRow_ = {};

        this.querySelectorAll('.__template td').forEach((td) => {

            if (td.id) {

                const control_ = td.querySelector('input[id], textarea[id], select[id]');

                if (control_.type.toLowerCase() == 'select-one') {

                    dataRow_[td.id] = {
                        Value: '',
                        Text: ''
                    };

                } else if (control_.type.toLowerCase() == 'checkbox') {

                    dataRow_[td.id] = new Boolean(false);

                } else {

                    dataRow_[td.id] = '';

                }

                dataRow_[this.keyField] = new Number(0);

            }

        });

        json_.Items.push(dataRow_);

        this.jsonString.value = JSON.stringify(json_);

        this.ServerNotify();

    }

    //Clone Rows
    cloneRow(e) {

        e.preventDefault();

        try {

            const json_ = JSON.parse(this.jsonString.value);

            const dataRows_ = json_.Items;

            const tableRows_ = this.querySelectorAll('tr.__row td:first-child input:not(.__checks):checked');

            tableRows_.forEach((e) => {

                const rowIndex = e.closest('tr').rowIndex - 2;

                const dataRow_ = {};

                for (const key_ in dataRows_[rowIndex]) {

                    dataRow_[key_] = dataRows_[rowIndex][key_];

                }

                dataRow_[this.keyField] = 0;

                dataRows_.push(dataRow_);

            });

            this.jsonString.value = JSON.stringify(json_);

            this.ServerNotify();

        } catch (e) { }

    }

    //Delete Rows
    deleteRow(e) {

        e.preventDefault();

        const tableRows_ = this.querySelectorAll('tr.__row td:first-child input:not(.__checks):checked');

        const removeItems_ = [];

        tableRows_.forEach((e) => {

            if (Number(e.value) != 0) {

                removeItems_.push(Number(e.value));

            } else {

                e.closest('tr.__row').remove();

                this.prepareJsonData();

            }

        });

        if (removeItems_.length > 0) {

            try {

                const json_ = JSON.parse(this.jsonString.value);

                json_.DropItems = removeItems_;

                this.jsonString.value = JSON.stringify(json_);

                this.ServerNotify();

            } catch (e) { }

        }

    }

    //Remove Row
    quitRow(e) {

        e.preventDefault();

        try {

            const json_ = JSON.parse(this.jsonString.value);

            const tableRow_ = e.target.closest('tr.__row').querySelector('td:first-child input:not(.__checks)');

            if (Number(tableRow_.value) != 0) {

                if (!json_.DropItems) json_.DropItems = [];

                json_.DropItems.push(Number(tableRow_.value));

                this.jsonString.value = JSON.stringify(json_);

                this.ServerNotify();

            } else {

                e.target.closest('tr.__row').remove();

                this.prepareJsonData();

            }

        } catch (e) { }

    }

    //Checked Rows
    checkRow(e) {

        const checks_ = this.querySelectorAll('tr.__row td:first-child input:not(.__checks)');

        checks_.forEach((c) => c.checked = e.target.checked);

    }

    //Find Rows
    findElements(e) {

        const input_ = e.target;

        const value_ = input_.value;

        const tableRows_ = this.querySelectorAll('tr.__row');

        if (value_.length >= 1) {

            for (let i = 0; i < tableRows_.length; i++) {

                let visible_ = false;

                const elements_ = tableRows_[i].querySelectorAll('input, select, textarea, p');

                for (let j = 0; j < elements_.length; j++) {

                    const element_ = elements_[j];

                    if (element_.nodeName.toLowerCase() == 'p') {

                        if (element_.innerText.toLowerCase().indexOf(value_.toLowerCase()) >= 0) {

                            visible_ = true;

                        }

                    } else {

                        if (element_.value.toLowerCase().indexOf(value_.toLowerCase()) >= 0) {

                            visible_ = true;

                        }

                    }

                }

                tableRows_[i].style.display = visible_ ? '' : 'none';

            }

        } else {

            for (let i = 0; i < tableRows_.length; i++) {

                tableRows_[i].style.display = '';

            }

        }

    }

    //SERVER METHODS

    ServerNotify() {

        //const event = new CustomEvent("build", { detail: elem.dataset.time });

        const event = new Event("build");

        this.dispatchEvent(event);

        this.jsonString.onchange();


    }



}


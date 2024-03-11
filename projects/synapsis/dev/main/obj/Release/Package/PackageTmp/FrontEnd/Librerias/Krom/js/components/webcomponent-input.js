export class WCInput extends HTMLInputElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this.component = this.closest('.__component');

    }


}

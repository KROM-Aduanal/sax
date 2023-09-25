export class WCLabel extends HTMLElement {

    static get observedAttributes() { 
        return ['Bold', 'TintColor', 'Text', 'Detail', 'CssClass', 'Icon'];
    }

    attributeChangedCallback(name, oldValue, newValue) {

        this.render();

    }

    constructor() {

        super();


    }

    connectedCallback() {

        if (!this.rendered) {

            this.render();

            this.rendered = true;

        }

    }

    render() {

        const tintColor = this.getAttribute('TintColor') ? this.getAttribute('TintColor') : '#6c4fd3';

        var icon = "";

        if (this.getAttribute('Icon')) {

            icon = `style='
                        background-image: url(/FrontEnd/Librerias/Krom/imgs/${this.getAttribute('Icon')});
                        background-position:left;
                        background-repeat:no-repeat;
                        background-size:24px;
                        padding-left:28px;'`;

        }

        this.innerHTML = `
            <label class="d-flex ${this.getAttribute("CssClass")}">
                <p class="col-auto ${ Boolean(this.getAttribute('Bold')) ? 'font-weight-bold' : ''}" style="color:${tintColor};">${this.getAttribute('Detail')}&nbsp;&nbsp;</p>
                <p class="col" ${icon}>${this.getAttribute('Text')}</p>
            </label>
        `;
    }

}
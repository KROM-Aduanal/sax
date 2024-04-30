export class WCUserData extends HTMLElement {

    static get observedAttributes() { 
        return ['Image', 'Title', 'Date', 'CssClass', 'TintColor'];
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

        this.innerHTML = `
            <div class="wc-userdata ${this.getAttribute("CssClass")}" style="--tintColor:${tintColor};">
                <image src="${this.getAttribute("Image")}">
                <span>
                    ${this.getAttribute("Title")}
                    <small>${this.getAttribute("Date")}</small>
                </span>
            </div>
        `;
    }

}
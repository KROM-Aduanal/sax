export class WCComment extends HTMLElement {

    static get observedAttributes() { 
        return ['Image','Title','Date','Text','TintColor', 'CssClass'];
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
            <li class="wc-comment ${this.getAttribute("CssClass")}" style="--tintColor:${tintColor};">
                <div class="wc-userdata">
                    <image src="${this.getAttribute("Image")}">
                    <span>
                        ${this.getAttribute("Title")}
                        <small>${this.getAttribute("Date")}</small>
                    </span>
                </div>
                <div>
                    ${this.getAttribute("Text")}
                </div>
            </li>
        `;
    }

}
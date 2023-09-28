export class WCToolTip extends HTMLInputElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this.component = this.closest('.__component');

        try {

            const data = JSON.parse(this.value);
            
            switch (Number(data.mode)) {

                case 1:

                    this.settingClassicToolTip(data);

                    break;
                case 2:

                    this.settingOndemanToolTip(data);

                    break;
                case 3:

                    //this.settingInteractiveToolTip(data);

                    break;

            }

        } catch (e) { }

    }

    
    getToolTipElement(data) {

        const bgcolors = { 1: '#d4edda', 2: '#cce5ff', 3: '#fff3cd', 4: '#f8d7da' };

        const txtcolors = { 1: '#155724', 2: '#004085', 3: '#856404', 4: '#721c24' };

        let tooltip = document.createElement('i');

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
       
        this.component.addEventListener('mouseenter', () => {

            const oldtooltip = this.component.querySelector('.tooltip-element') || false;

            if (oldtooltip == false) {

                let tooltip = this.getToolTipElement(data);

                this.component.appendChild(tooltip);

                tooltip.style.opacity = 1;

            }

        });

        this.component.addEventListener('mouseleave', () => {

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

            setTimeout(() => {

                data.visible = "False";

                this.component.querySelector('.__tooltip').value = JSON.stringify(data);

            }, 1000);

        }

    }


}

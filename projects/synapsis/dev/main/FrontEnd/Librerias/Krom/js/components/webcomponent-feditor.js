export class WCFEditor extends HTMLDivElement {

	constructor() {

        super();

  	}

  	connectedCallback() {
  		
            this._component = this;

            this._input = this.nextElementSibling;

            this._component.addEventListener('input', function () {

                let content_ = this.innerText;

                const operadoresRegex_ = /([-+*/,:<>])/g;

                content_ = content_.replace(operadoresRegex_, ' $1 ');

                const variablesRegex_ = /(?:[^']|^)([A-Z_]+[0-9]*)(?=[^']*|$)/g;

                content_ = content_.replace(variablesRegex_, '<span class="variable">$&</span>');

                const keywords_ = ['SUMAR', 'RESTAR', 'MULTIPLICAR', 'DIVIDIR', 'RED', 'TRUNC', 'SI', 'RANGO'];

                keywords_.forEach(keyword_ => {

                    const keywordRegex_ = new RegExp(`\\b${keyword_}\\b`, 'g');

                    content_ = content_.replace(keywordRegex_, '<span class="keyword">' + keyword_ + '</span>');

                });

                const parentesisRegex_ = /[\(\)]/g;

                content_ = content_.replace(parentesisRegex_, '<span class="parentesis">$&</span>');

                this.innerHTML = content_;

                this._input.value = this.innerText;

                setEndOfContenteditable(this);
                //setCursorAtLastPosition(this)

            });


            /*function setCursorAtLastPosition(contentEditableElement) {
                var selection, range;

                if (window.getSelection) {
                    selection = window.getSelection();
                    if (selection.rangeCount > 0) {
                        range = selection.getRangeAt(0).cloneRange();
                        range.setStart(selection.focusNode, selection.focusOffset);
                        range.collapse(true);
                        selection.removeAllRanges();
                        selection.addRange(range);
                    }
                } else if (document.selection) {
                    range = document.selection.createRange();
                    range.moveStart('character', -1);
                    range.moveStart('character', 1);
                    range.collapse(false);
                    range.select();
                }
            }*/


            function setEndOfContenteditable(contentEditableElement) {

                var range, selection;

                if (document.createRange)
                {
                    range = document.createRange();

                    range.selectNodeContents(contentEditableElement);

                    range.collapse(false);

                    selection = window.getSelection();

                    selection.removeAllRanges();

                    selection.addRange(range);
                }
                else if (document.selection)
                {
                    range = document.body.createTextRange();

                    range.moveToElementText(contentEditableElement);

                    range.collapse(false);

                    range.select();
                }

            }

            if (this._input.value) {

                this._component.innerHTML = this._input.value;

                var event = document.createEvent('Event');

                event.initEvent('input', true, false);

                this._component.dispatchEvent(event);

            }

    }

    
	
}

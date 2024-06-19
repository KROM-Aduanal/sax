export class WCFEditor extends HTMLDivElement {

	constructor() {

        super();

  	}

  	connectedCallback() {
  		
            this._component = this;

            this._input = this.nextElementSibling;


            this._component.addEventListener('keypress', function (event) {

                if(this._input.disabled)
                    event.preventDefault(); // Prevent character from being inserted
            });

            this._component.addEventListener('keydown', function (event) {
                if (event.key === 'Backspace' || event.key === 'Delete') {
                    if (this._input.disabled)
                        event.preventDefault();
                }
            });

            this._component.addEventListener('input', function () {



                let content_ = this.innerText + '}';

                var contentFinal_ = "";

                var words_ = [];

                var word_ = "";

                var wordFinal_ = '';

                var quote_ = false;

                const expresionRegularNumeros_ = /^[0-9]+(\.[0-9]+)?$/;

                const variablesRegex_ = /(?:[^']|^)([A-Z_]+[0-9]*)(?=[^']*|$)/g;

                const keywords_ = ['AHORA', 'ASIGNAR','BUSCARV', 'COINCIDIR', 'CONCATENAR', 'DENTRO', 'DICCIONARIO', 'ENLISTAR', 'ESBLANCO',
                                   'ESPACIOS', 'EXTRAE', 'HOY', 'LARGO' , 'NO', 'O', 'RANGO', 'RED', 'REDONDEAR',
                                   'ROOM','SECUENCIA', 'SETROOM', 'SI', 'SUMAR', 'SUMAR.SI', 'SUSTITUIR', 'TRUNC', 'TRUNCAR', 'Y'];


                
                const characters_ = content_.replace(/\[13\]/g, '\n').split('');

                characters_.forEach(character_ => {

                    if (character_ === '+' || character_ === '-' || character_ === '*' || character_ === '/' || character_ === ',' || character_ === '-' || character_ === '(' || character_ === ')' || character_ === '^' || character_ === '=' || character_ === '>' || character_ === '<' || character_ === '}') {
                        if (word_.length > 0) {

                            if (word_.indexOf(String.fromCharCode(39)) !== -1 || quote_ || word_.indexOf(String.fromCharCode(34)) !== -1) {

                                for (let i = 0; i < word_.length; i++) {
                                    if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n') {
                                        contentFinal_ += '<span class="constanstring"><br></span>';
                                    }
                                    else {
                                        contentFinal_ += '<span class="constanstring">' + word_[i] + '</span>';
                                        //                                            wordFinal_ += word_[i];
                                    }
                                }
                            } else
                                if (keywords_.indexOf(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', '')) !== -1) {

                                    for (let i = 0; i < word_.length; i++) {
                                        if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n') {
                                            contentFinal_ += '<span class="keyword"><br></span>';
                                        }
                                        else
                                            contentFinal_ += '<span class="keyword">' + word_[i] + '</span>';

                                    }


                                } else if (expresionRegularNumeros_.test(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', ''))) {
                                    for (let i = 0; i < word_.length; i++) {
                                        if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n') {
                                            contentFinal_ += '<span class="constante"><br></span>';
                                        }
                                        else
                                            contentFinal_ += '<span class="constante">' + word_[i] + '</span>';
                                    }


                                } else if (variablesRegex_.test(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', ''))) {
                                    for (let i = 0; i < word_.length; i++) {
                                        if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n') {
                                            contentFinal_ += '<span class="variable"><br></span>';
                                        }
                                        else
                                            contentFinal_ += '<span class="variable">' + word_[i] + '</span>';
                                    }

                                } else {
                                    for (let i = 0; i < word_.length; i++) {
                                        if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n') {

                                            contentFinal_ += '<span class="variable"><br></span>';
                                        }
                                        else
                                            contentFinal_ += '<span class="variable">' + word_[i] + '</span>';

                                    }
                                }

                            if (character_ === "(" || character_ === ")") {

                                contentFinal_ += '<span class="parentesis">' + character_ + '</span>';

                            } else if (character_ !== '}') {
                                contentFinal_ += '<span class="operador">' + character_ + '</span>';

                            }


                        } else if (character_ !== '}') {

                            contentFinal_ += '<span class="operador">' + character_ + '</span>';

                        }

                        word_ = '';

                    }
                    else {
                        if (character_ === String.fromCharCode(39) || character_ === String.fromCharCode(34)) {

                            quote_ = !quote_;
                        }
                        word_ += character_;

                    }
                    if (character_.charCodeAt(0) === 13 || character_.charCodeAt(0) === 10 || character_ === '\n' || character_ === '\r' || character_ === '\r\n') {
                        wordFinal_ += '[13]';
                    }
                    else
                        if (character_!=="}")
                            wordFinal_ += character_;

                    });


                    var beforeSpan_ = '';

                if (localStorage.getItem('contenidoFEditor') !== null) {
                    beforeSpan_ = localStorage.getItem('contenidoFEditor');
                    } 

                    this.innerHTML = contentFinal_;

                    const afterSpan_ = contentFinal_;

                    this._input.value = wordFinal_; // this.innerText;

                    setCursorAtEndAfterChange(this, beforeSpan_, afterSpan_);

            });


            function setCursorAtEndAfterChange(contentEditableElement, beforeSpan_, afterSpan_) {
                var range, selection;

                if (document.createRange) {

                    const before_ = beforeSpan_.length > 0 ? beforeSpan_.split('</span>') : [];
                    const after_ = afterSpan_.split('</span>');
                    var tamanio_ = 0;
                    var changeforce_ = false;

                    if (before_.length <= after_.length)

                        tamanio_ = before_.length;

                    else

                        tamanio_ = after_.length;

                    var cuenta_ = 0;

                    var changeindex_ = 0;

                   
                    while (changeindex_ < tamanio_) {

                        var spanIndex_ = before_[changeindex_].search('>');

                        var characterbefore_ = before_[changeindex_].substring(spanIndex_ + 1);

                        spanIndex_ = after_[changeindex_].search('>');

                        var characterafter_ = after_[changeindex_].substring(spanIndex_ + 1);

                        if (characterbefore_ !== characterafter_) {

                            cuenta_++;

                            if (before_.length < after_.length) {

                                var spanIndex_ = before_[changeindex_].search('>');

                                var characterbefore_ = before_[changeindex_].substring(spanIndex_ + 1);

                                spanIndex_ = after_[changeindex_+1].search('>');

                                var characterafter_ = after_[changeindex_+1].substring(spanIndex_ + 1);

                                if (characterbefore_ !== characterafter_) {
                                    var different_ = true;

                                    if ((characterbefore_.charCodeAt(0) === 32 && characterafter_.charCodeAt(0) === 160) || (characterbefore_.charCodeAt(0) === 160 && characterafter_.charCodeAt(0) === 32))
                                        different_ = false;

                                    if (different_)
                                        cuenta_++;
                                    else
                                        cuenta_ = 7;

                                }
                                else
                                    cuenta_ = 7;
                                changeindex_ = tamanio_ + 1000;

                            }
                            else
                                if (before_.length > after_.length) {

                                    var spanIndex_ = before_[changeindex_+1].search('>');

                                    var characterbefore_ = before_[changeindex_+1].substring(spanIndex_ + 1);

                                    spanIndex_ = after_[changeindex_].search('>');

                                    var characterafter_ = after_[changeindex_].substring(spanIndex_ + 1);

                                    if (characterbefore_ !== characterafter_) {

                                        var different_ = true;

                                        if ((characterbefore_.charCodeAt(0) === 32 && characterafter_.charCodeAt(0) === 160) || (characterbefore_.charCodeAt(0) === 160 && characterafter_.charCodeAt(0) === 32))
                                            different_ = false;

                                        if (different_)
                                            cuenta_++;
                                        else
                                            cuenta_ = 7;

                                    }
                                    else
                                        cuenta_ = 7;
                                    changeindex_ = tamanio_ + 1000;

                                } else {
                                    cuenta_ = 7;
                                    changeindex_ = tamanio_ + 1000;
                                }
                        } else changeindex_++;
                    }

                    if (cuenta_===2)
                            changeforce_ = true;

                 
                    selection = window.getSelection();

                    console.log("Cuenta:" + cuenta_);
                    console.log(before_);
                    console.log(after_);
                    //console.log(cambios__);

                    if (selection.rangeCount > 0) {


                        var lastRange = selection.getRangeAt(selection.rangeCount - 1);

                        var indice_ = 0;

                        var found_ = 0;

                        if (!changeforce_) {

                            if (before_.length <= after_.length)

                                tamanio_ = before_.length;

                            else

                                tamanio_ = after_.length;



                          //  console.log("Tamaño=" + tamanio_)

                            while (found_ < tamanio_) {

                                var spanIndex_ = before_[found_].search('>');

                                var characterbefore_ = before_[found_].substring(spanIndex_ + 1);

                                spanIndex_ = after_[found_].search('>');

                                var characterafter_ = after_[found_].substring(spanIndex_ + 1);


                                if (characterbefore_ !== characterafter_) {

                                    if (before_.length < after_.length)

                                        indice_ = found_ +1;

                                    else

                                        indice_ = found_;

                                    found_ = tamanio_ + 1000;

                                } else

                                    found_++;
                            }
                        }

                        console.log(indice_);

                        if (found_ === 0 || found_===tamanio_)
                            indice_ = after_.length-1;

                        localStorage.setItem('contenidoFEditor', afterSpan_);

                        lastRange.setStart(contentEditableElement, indice_);

                         selection.removeAllRanges();

                        selection.addRange(lastRange);
                        
                     
                        
                    } else {

                        // If there is no selection, place the cursor at the end
                        range.collapse(false);
                        selection.removeAllRanges();
                        selection.addRange(range);
                    }
                } else if (document.selection) {
                    range = document.createRange();
                    range.selectNodeContents(contentEditableElement);
                    range = document.body.createTextRange();
                    range.moveToElementText(contentEditableElement);

                    // Check if there is a selection
                    if (range.getBookmark().length > 0) {
                        // Get the last modified range
                        var lastRange = document.selection.createRange();

                        // Collapse the last range to the end
                        lastRange.collapse(false);

                        // Select the last range
                        range.setEndPoint('EndToEnd', lastRange);
                        range.select();
                    } else {
                        // If there is no selection, place the cursor at the end
                        range.collapse(false);
                        range.select();
                    }
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

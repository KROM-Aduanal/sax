export class WCFEditor extends HTMLDivElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this._component = this;

        this._input = this.nextElementSibling;



        this._component.addEventListener('input', function () {

            let content_ = this.innerText + '}';

            var contentFinal_ = "";

            var words_ = [];

            var word_ = "";

            var quote_ = false;

            const expresionRegularNumeros_ = /^[0-9]+(\.[0-9]+)?$/;

            const variablesRegex_ = /(?:[^']|^)([A-Z_]+[0-9]*)(?=[^']*|$)/g;

            const keywords_ = ['ASIGNAR', 'EN', 'ESBLANCO', 'ESPACIOS', 'EXISTE', 'EXTRAE', 'LARGO', 'O', 'RANGO', 'RED', 'REDONDEAR',
                'ROOM', 'SETROOM', 'SI', 'SUMAR', 'SUMAR.SI', 'TRUNC', 'TRUNCAR', 'Y'];


            const characters_ = content_.split('');

            characters_.forEach(character_ => {

                if (character_ === '+' || character_ === '-' || character_ === '*' || character_ === '/' || character_ === ',' || character_ === '-' || character_ === '(' || character_ === ')' || character_ === '^' || character_ === '=' || character_ === '}') {
                    if (word_.length > 0) {
                        console.log("La palabra es:" + word_);

                        if (word_.indexOf(String.fromCharCode(39)) !== -1 || quote_ || word_.indexOf(String.fromCharCode(34)) !== -1) {

                            for (let i = 0; i < word_.length; i++) {
                                if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n')
                                    contentFinal_ += '<span class="constanstring"><br></span>';
                                else
                                    contentFinal_ += '<span class="constanstring">' + word_[i] + '</span>';
                            }
                        } else
                            if (keywords_.indexOf(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', '')) !== -1) {

                                for (let i = 0; i < word_.length; i++) {
                                    if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n')
                                        contentFinal_ += '<span class="keyword"><br></span>';
                                    else
                                        contentFinal_ += '<span class="keyword">' + word_[i] + '</span>';

                                }
                                // console.log("Entro a keyword " + character_);


                            } else if (expresionRegularNumeros_.test(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', ''))) {
                                for (let i = 0; i < word_.length; i++) {
                                    if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n')
                                        contentFinal_ += '<span class="constante"><br></span>';
                                    else
                                        contentFinal_ += '<span class="constante">' + word_[i] + '</span>';
                                }
                                // console.log("Entro a constante " + character_);


                            } else if (variablesRegex_.test(word_.trim().replace(' ', '').replace(String.fromCharCode(160), '').replace(String.fromCharCode(13), '').replace('\n', '').replace('\r\n', '').replace('\r', '').replace('\t', ''))) {
                                for (let i = 0; i < word_.length; i++) {
                                    if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n')
                                        contentFinal_ += '<span class="variable"><br></span>';
                                    else
                                        contentFinal_ += '<span class="variable">' + word_[i] + '</span>';
                                }
                                //  console.log("Entro variable" + character_);

                            } else {
                                for (let i = 0; i < word_.length; i++) {
                                    if (word_.charCodeAt(i) === 13 || word_.charCodeAt(i) === 10 || word_[i] === '\n' || word_[i] === '\r' || word_[i] === '\r\n')
                                        contentFinal_ += '<span class="variable"><br></span>';
                                    else
                                        contentFinal_ += '<span class="variable">' + word_[i] + '</span>';
                                    // console.log("Entro a lo que sea" + character_);
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
                        // console.log("la cabulidad es:"+character_);
                        quote_ = !quote_;
                    }
                    word_ += character_;

                }

            });


            var beforeSpan_ = '';

            if (localStorage.getItem('cabulidadFeditor') !== null) {
                beforeSpan_ = localStorage.getItem('cabulidadFeditor');
                console.log("Si entra a recibir lo local");
            } else {
                // Handle the case where userName is not found in localStorage
                console.log('userName not found in localStorage');
            }
            //beforeSpan_ = this.innerHTML;

            //let storecadenilla_ = cadenilla_;

            //let beforeSpan_ = storecadenilla_.split("</span>");


            //console.log(this._input.innerHTML);

            //  const before_ = this._input.value;


            this.innerHTML = contentFinal_;

            const afterSpan_ = contentFinal_;

            this._input.value = this.innerText;

            //     const after_ = this._input.value;


            //console.log(contentFinal_);

            // console.log(this.innerHTML);
            console.log("Lo que hay en el vector before:");
            console.log(beforeSpan_);
            console.log("Lo que hay en el vector after:");
            console.log(afterSpan_);



            setCursorAtEndAfterChange(this, beforeSpan_, afterSpan_);

        });


        //function setEndOfContenteditable(contentEditableElement) {

        //    var range, selection;

        //    if (document.createRange)
        //    {
        //        range = document.createRange();

        //        range.selectNodeContents(contentEditableElement);

        //        range.collapse(false);

        //        selection = window.getSelection();

        //        selection.removeAllRanges();

        //        selection.addRange(range);
        //    }
        //    else if (document.selection)
        //    {
        //        range = document.body.createTextRange();

        //        range.moveToElementText(contentEditableElement);

        //        range.collapse(false);

        //        range.select();
        //    }




        //}

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
                    if (before_[changeindex_] !== after_[changeindex_]) {
                        cuenta_++;
                        if (before_.length < after_.length) {

                            if (before_[changeindex_] !== after_[changeindex_ + 1])
                                cuenta_++;
                            else
                                cuenta_ = 7;
                            changeindex_ = tamanio_ + 1000;

                        }
                        else
                            if (before_.length > after_.length) {

                                if (before_[changeindex_ + 1] !== after_[changeindex_])
                                    cuenta_++;
                                else
                                    cuenta_ = 7;
                                changeindex_ = tamanio_ + 1000;

                            } else {
                                cuenta_ = 7;
                                changeindex_ = tamanio_ + 1000;
                            }
                    } else changeindex_++;
                }

                if (cuenta_ === 2)
                    changeforce_ = true;


                selection = window.getSelection();

                if (selection.rangeCount > 0) {


                    var lastRange = selection.getRangeAt(selection.rangeCount - 1);

                    var indice_ = 0;

                    if (!changeforce_) {

                        if (before_.length <= after_.length)

                            tamanio_ = before_.length;

                        else

                            tamanio_ = after_.length;

                        var found_ = 0;

                        console.log("Tamaño=" + tamanio_)

                        while (found_ < tamanio_) {

                            if (before_[found_] !== after_[found_]) {

                                if (before_.length < after_.length)

                                    indice_ = found_ + 1;

                                else

                                    indice_ = found_;

                                found_ = tamanio_ + 1000;

                            } else

                                found_++;
                        }
                    }

                    if (indice_ === 0)
                        indice_ = after_.length - 1;

                    localStorage.setItem('cabulidadFeditor', afterSpan_);

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
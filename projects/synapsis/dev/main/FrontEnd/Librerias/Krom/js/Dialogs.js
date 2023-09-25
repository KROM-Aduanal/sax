function DisplayPrompt() {

    const title_ = arguments[0] || '';
    const message_ = arguments[1] || '';
    const accept_ = arguments[2] || 'Enviar';
    const cancel_ = arguments[3] || 'Cancelar';
    const func_ = arguments[4] || null;

    const template_ = document.createElement('template');

    template_.innerHTML = `
              <div class="modal show">
                <div class="modal-dialog modal-sm modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">${title_}</h4>
                            <button type="button" class="close">
                                <span aria-hidden="true">x</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p>${message_}</p>
                            <input type="text" class="form-control" placeholder="Escriba aqu&iacute;"/>
                        </div>
                        <div class="modal-footer justify-content-end">
                            <button type="button" class="btn btn-default" data-dismiss="modal">${cancel_}</button>
                            <button type="button" class="btn btn-primary">${accept_}</button>
                        </div>
                    </div>
                </div>
              </div>
	            `;

    document.body.appendChild(template_.content.cloneNode(true));

    const modal_ = document.querySelector('.modal');

    const btnaccept_ = modal_.querySelector('.btn-primary');

    const btncancel_ = modal_.querySelector('.btn-default');

    const btnclose_ = modal_.querySelector('.close');

    const input_ = modal_.querySelector('.form-control');

    btnaccept_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

        if (typeof func_ === 'function') {

            func_(input_.value);

        }

    });

    btncancel_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

    });

    btnclose_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

    });

}

function DisplayAlert() {

    const title_ = arguments[0] || 'Confirmación';
    const message_ = arguments[1] || '';
    const accept_ = arguments[2] || 'Entendido';
    const cancel_ = arguments[3] || '';
    const argument_ = arguments[4] || null;
    const hasCancelButton = (!cancel_) ? 'd-none' : '';
    
    const template_ = document.createElement('template');

    template_.innerHTML = `
            <div class="modal show">
            <div class="modal-dialog modal-sm modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">${title_}</h4>
                        <button type="button" class="close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>${message_}</p>
                    </div>
                    <div class="modal-footer justify-content-between">
                        <button type="button" class="btn btn-default ${hasCancelButton}" data-dismiss="modal">${cancel_}</button>
                        <button type="button" class="btn btn-primary">${accept_}</button>
                    </div>
                </div>
            </div>
            </div>
	        `;

    document.body.appendChild(template_.content.cloneNode(true));

    const modal_ = document.querySelector('.modal');

    const btnaccept_ = modal_.querySelector('.btn-primary');

    const btncancel_ = modal_.querySelector('.btn-default');

    const btnclose_ = modal_.querySelector('.close');

    btnaccept_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

        if (typeof argument_ === 'function') {

            argument_(true);

        } else {

            PageMethods["ProcessDialogConfirmation"]({ arg: argument_, accept: true }, (data) => {

                try {

                    const res = JSON.parse(data);

                    if (res.code == 200) {

                        __serverObserver();

                    }

                } catch (e) {}
                
            });
            

        }

    });

    btncancel_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

        if (typeof argument_ === 'function') {

            argument_(false);

        } else {

            PageMethods["ProcessDialogConfirmation"]({ arg: argument_, accept: false }, (data) => {

                try {

                    const res = JSON.parse(data);

                    if (res.code == 200) {

                        __serverObserver();

                    }

                } catch (e) { }

            });

        }

    });

    btnclose_.addEventListener('click', () => {

        document.querySelector('.modal').remove();

    });

}

function DisplayMessage() {

    const message_ = arguments[0] || false;

    const code_ = arguments[1] || 1;

    const template_ = document.createElement('template');

    var tintColor_;

    var title_;

    if (code_ == 1) { tintColor_ = '#1ca477'; title_ = 'Excelente'; }

    if (code_ == 2) { tintColor_ = '#a41c22'; title_ = 'Error'; }

    if (code_ == 3) { tintColor_ = '#1c8ba4'; title_ = 'Información'; }

    template_.innerHTML = `
       <div class="wc-toast" style="--tintColor:${tintColor_}">
		  <i></i>
          <p>
            <small>${title_}</small>
            ${message_}
          </button>
       </div>
    `;

    document.querySelectorAll('.wc-toast').forEach(e => e.remove());

    document.body.append(template_.content.cloneNode(true));

    const toast = document.querySelector('.wc-toast');

    setTimeout(e => {
        toast.style.bottom = '4%';
        toast.querySelector('i').classList.add('wc-toast-animate');
        setTimeout(e => {
            toast.style.bottom = '-100%';
            setTimeout(e => toast.remove(), 1000);
        }, 5000);
    }, 0);

}


function __serverObserver() {

    const serverObserver = document.querySelector('.__serverObserver');

    serverObserver.click();

    //__doPostBack(serverObserver.id);

}

function openPDF(pdfstring, title) {

    var win = window.open();

    win.document.title = title;

    var iframe = "<iframe src='" + pdfstring + "' frameborder='0' style='border:0; top:0px; left:0px; bottom:0px; right:0px; width:100%; height:100%;position:absolute;' allowfullscreen></iframe>";

    win.document.write(iframe);

}
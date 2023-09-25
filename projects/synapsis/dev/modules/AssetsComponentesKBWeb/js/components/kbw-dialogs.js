export function DisplayActionSheet() {

    return new Promise((done_, fail_) => {
		  
        const title_ 	 = arguments[0] || false;
		const message_ 	 = arguments[1] || false; 
        const buttons_   = Array.from(arguments).slice(2,arguments.length);

	    const template_  = document.createElement('template');

        template_.innerHTML = `
          <div class="modal fade">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">${title_}</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>${message_}</p>
                    </div>
                    <div class="modal-footer flex-column">
                        ${getButtons(buttons_)}
                    </div>
                </div>
            </div>
          </div>
	        `;
	    	    
        function getButtons(buttons_) {
        
            const domElements = [];

            buttons_.forEach((button_, index) => {
                
                domElements.push(`<button id="${index}" type="button" class="btn btn-light btn-block">${button_}</button>`)  ;

            });

            return domElements.join('');
            
        }

        document.body.appendChild(template_.content.cloneNode(true));
	    
	    const modal_ 	= document.querySelector('.modal');
	    
	    const actionButtons_ 	= modal_.querySelectorAll('.btn-light');

	    actionButtons_.forEach((actionButton_) => {
	    
	        actionButton_.addEventListener('click', (e) => {
	    	
                $(modal_).modal('hide');

                done_(e.target.id);

            });

	    });

        $(modal_).modal();
    
   });

}

export function DisplayAlert() {

    return new Promise((done_, fail_) => {
		  
        const title_ 	 = arguments[0] || false;
		const message_ 	 = arguments[1] || false;
		const accept_ 	 = arguments[2] || 'Entendido';
		const reject_ 	 = arguments[3] || false;
		const alignment_ = arguments.length > 3 ? 'justify-content-between' : 'justify-content-end';
		const display_ 	 = arguments.length <= 3 ? 'd-none' : '';

	    const template_  = document.createElement('template');

        template_.innerHTML = `
          <div class="modal fade">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">${title_}</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>${message_}</p>
                    </div>
                    <div class="modal-footer ${alignment_}">
                        <button type="button" class="btn btn-default ${display_}" data-dismiss="modal">${reject_}</button>
                        <button type="button" class="btn btn-primary">${accept_}</button>
                    </div>
                </div>
            </div>
          </div>
	        `;
	    	    
        document.body.appendChild(template_.content.cloneNode(true));
	    
	    const modal_ 	= document.querySelector('.modal');
	    
	    const button_ 	= modal_.querySelector('.btn-primary');

        button_.addEventListener('click', () => {
	    	
            $(modal_).modal('hide');

            done_();

        });

        $(modal_).modal();
    
   });

}


export function DisplayToast() {

	const message_ 	 = arguments[0] || false;
	
    const template_  = document.createElement('template');

    template_.innerHTML = `
        <div class="kbw-toast">
            <i class="far fa-bell"></i>
            <div>
                <small>${document.title}</small>
                <p>${message_}</p>
            </div>
        </div>
    `;
    	    
    document.querySelector('body').appendChild(template_.content.cloneNode(true));

    const alert_ = document.querySelector('.kbw-toast');
    
    setTimeout(() => {  

        alert_.style.transition = '1s all';

        alert_.style.top = '5%';

        alert_.querySelector('i').classList.add('kbw-toast-animate');

        setTimeout(() => {  
                        
            alert_.style.top = '-100%';
        
            setTimeout(() => {
        
                alert_.remove();

            },500);

        },6000);
    
    },500);  

}

export function DisplayMessage() {

	const message_ 	 = arguments[0] || false;
	
    const template_  = document.createElement('template');

    template_.innerHTML = `
       <div class="alert alert-success alert-dismissible fadeanim show" role="alert">
		  ${message_}
          <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
       </div>
    `; 
    
    const messageContainer = document.querySelector('.alert-messages')

    messageContainer.innerHTML = ''

    messageContainer.appendChild(template_.content.cloneNode(true));
      
    const alert_ = document.querySelector('.alert');

    $(alert_).alert();

}

$('body').on('hidden.bs.modal','.modal', function (e) {
    
    e.target.remove();

});



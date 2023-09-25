export class KBWSelect extends HTMLSelectElement {
  
    // Setters & Getters 

    static get observedAttributes() {
    
        return ['to-append-template'];
  
    }
   
    attributeChangedCallback(name_, oldValue_, newValue_) {
    
        switch(name_) {
            case 'to-append-template':
                this.template = newValue_;
                break;
        }

    }

    set hidden(value_) {
    
        this.parentNode.style.display = value_ ? 'none' : '';

    }

    set error(value_) {
  
       const lastErrror_ = this.parentNode.querySelector('small');

       if(lastErrror_) {

          lastErrror_.remove();

       }
        
       const error_     = document.createElement('small');

       error_.innerText = value_;

       this.parentNode.appendChild(error_);
  
       this.parentNode.classList.add('has-error');

   }

    set dataSource(source_) {

        this.innerHTML = '';

        source_.forEach((row_) => {

            const option_        = document.createElement('option');

            option_.value        = row_.key;

            option_.innerHTML    = row_.value;

            this.append(option_);

        });

    };

    set appendOption(row_) {

        const option_        = document.createElement('option');

        option_.value        = row_.key;

        option_.innerHTML    = row_.value;

        this.append(option_);

    }

  
    // Constructor

    constructor() {
        super();  

        //Prepare Default Properties and Attributes

        this.initialize();

        //Draw
      
        this.draw();

        //Prepare Events

        this.triggers();

    }

    //Initialize

    initialize() {

        //Properties

        //Attributes

        this.id = this.name;
         
        this.classList.add('form-control');

        this.classList.add('select2');

        if(this.getAttribute('multile')) {
            
            this.setAttribute('data-placeholder', 'Select a State');

        }

    }

    //Draw

    draw() {

        const template_           = document.createElement('template');

        template_.innerHTML = `
          <div class="form-group position-relative ${this.getAttribute('to-append-template')? 'd-flex flex-row-reverse align-items-center' : ''}">
              ${this.getAttribute('to-append-template')? '<a class="btn btn-primary" href=""><i class="fas fa-plus"></i></a>' : ''}
          </div>
        `;
      
        const parentNode_        = this.parentNode;

        const templateContent_   = template_.content.cloneNode(true);
      
        const elementContainer_  = templateContent_.querySelector('.form-group');

        parentNode_.insertBefore(templateContent_, this);

        elementContainer_.appendChild(this);

        //Identified Componentes

        if(this.getAttribute('to-append-template')) {

            this._selectAppendButton                     = this.parentNode.querySelector('a');
      
            this._selectAppendButton.style.borderRadius  = '0 .2rem 0.25rem 0';

            this._selectAppendButton.style.marginTop     = '-1rem';

        }
    }
    
    //Events

    triggers() {
    
        if(this._selectAppendButton) {
            
            const form = this.closest('form');

            var identifier;

            this._selectAppendButton.addEventListener('click', (e_) => {

                e_.preventDefault();

                identifier = this.template.split('/').pop().replace('.aspx','');

                form.pushView = this.template;

            });

            form.addEventListener('formDetailSubmit', (object_) => {
                
                if(object_.detail.id == identifier) {
                    
                    this.saveFormData(object_.detail, (data_) => {
                            
                        object_.detail.close();
          
                        this.appendOption = {key:data_.key, value: data_.value};

                        this.value = data_.key;

                        this.dispatchEvent(new Event("change"));
                        
                    });

                }

          });
        
        }

    }

    // Methods

    saveFormData(form_, func_) {
    
        const button_ = form_.content.querySelector('button');

        button_.spinner.start();

        fetch(form_.content.action,{
            body: JSON.stringify({Data_: Object.fromEntries(new FormData(form_.content))}),
            method: 'post',
            headers: {
                'content-type': 'application/json; charset=utf-8',
            }
        })
        .then(response => response.json())
        .then((json) => {

            button_.spinner.stop();
                  
            if(json.d.code == 200) {
                    
                form_.content.clear();
                  
                DisplayToast(json.d.message);

                if(typeof func_ == 'function') {
         
                    func_(json.d.response);

                }
                  
            } else {

                if ('errors' in json.d) {

                    Object.entries(json.d.errors).forEach((dataError_) => {
                            
                        const currentInput_ = form_.content.querySelector('[name="'+dataError_[0]+'"]');

                        const lastErrror_ = currentInput_.parentNode.querySelector('small');

                        if(lastErrror_) {

                            lastErrror_.remove();

                        }
        
                        const error_     = document.createElement('small');

                        error_.innerText = dataError_[1];

                        currentInput_.parentNode.appendChild(error_);

                        currentInput_.style.borderColor = '#f6477a';

                 });
                        
                } else {
                            
                        DisplayAlert('Algo ha ocurrido', json.d.message);

                }

            } 
                  
    
        });
    
    }

};


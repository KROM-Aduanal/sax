export class KBWFindbar extends HTMLInputElement {
  
  // Setters & Getters 

  set hidden(value_) {
    
       this.parentNode.style.display = value_ ? 'none' : '';

  }

  get selectedItem() {

      return this._jsonResponse[this._selectedIndex];

  }

  static get observedAttributes() {
    
      return ['action', 'text-field'];
  
  }

  attributeChangedCallback(name_, oldValue_, newValue_) {
    
      switch(name_) {
        case 'action':
          this._findbarAction = newValue_;
        break;
        case 'text-field':
            this._dropdownItemTextField = newValue_;
        break;
      }

  }

  // Constructor

  constructor() {
      super();  

      if(this.type == 'text') {

        //Prepare Default Properties and Attributes

        this.initialize();

        //Draw

        this.draw();

        //Prepare Events

        this.triggers();

      }
      
  }

  //Initialize

  initialize() {

      //Properties

      this._typingTimer        = null;                
    
      this._doneTypingInterval = 1000;

      this._jsonResponse       = [];

      this._selectedIndex      = 0;

      //Attributes

      this.id                  = this.name;

      this.autocomplete        = 'off';

      this.classList.add('form-control');

  }

  //Draw

  draw() {

      const template_     = document.createElement('template');

      template_.innerHTML = `
        <div class="kbw-findbar position-relative d-flex flex-row-reverse align-items-center">
          <a class="btn btn-primary" href="">
            <i class="fa fa-search"></i>
          </a>
          <div class="dropdown-menu"></div>
        </div>
      `;

      const parentNode_        = this.parentNode;

      const templateContent_   = template_.content.cloneNode(true);
      
      const elementContainer_  = templateContent_.querySelector('.kbw-findbar');

      parentNode_.insertBefore(templateContent_, this);

      elementContainer_.appendChild(this);

      //Identified Componentes

      this._findbarDropdown = this.parentNode.querySelector('div');

      this._findbarButton   = this.parentNode.querySelector('a');
      
      this._findbarIcon     = this.parentNode.querySelector('i');

  }

  //Events

  triggers() {

      this.addEventListener('keyup', this.clientTyping);

      this._findbarButton.addEventListener('click', (e) => e.preventDefault());

  }

  // Methods

  spinner() {
    return {
      start: () => {

        this._findbarIcon.classList.remove('fa-search');

        this._findbarIcon.classList.add('fa-spinner');

        this._findbarIcon.classList.add('fa-spin');

      },
      stop: () => {

        this._findbarIcon.classList.remove('fa-spinner');

        this._findbarIcon.classList.remove('fa-spin');

        this._findbarIcon.classList.add('fa-search');

      }
    }
  }

  dropdownActions() {
      return {
        close : () => {

          this._findbarDropdown.innerHTML      = '';

          this._findbarDropdown.style.display  = 'none';

        },
        open : () => {

          this._findbarDropdown.innerHTML    = '';

          this._jsonResponse.forEach((arg, i) => {

            const dropdownItem_ = document.createElement('a');

            dropdownItem_.setAttribute('id', i);

            dropdownItem_.setAttribute('href', '');

            dropdownItem_.classList.add('dropdown-item');

            dropdownItem_.innerText = arg[this._dropdownItemTextField];

            this._findbarDropdown.appendChild(dropdownItem_);

            dropdownItem_.addEventListener('click', (e) => {

              e.preventDefault();
              
              this._selectedIndex      = Number(e.target.id);

              this.value      = this._jsonResponse[this._selectedIndex][this._dropdownItemTextField];

              this.dispatchEvent(new Event('dropdownItemSelected'));

              this.dropdownActions().selected();

            });

          });

          this._findbarDropdown.style.display  = 'block';

        },
        selected : () => {

            this.dropdownActions().close();

            this.disabled   = true;

            this._findbarIcon.classList.remove('fa-search');

            this._findbarIcon.classList.add('fa-times');

            this._findbarButton.addEventListener('click', (e) => {

              e.preventDefault();

              this._findbarButton.removeEventListener('click', null);

              this.disabled   = false;

              this.value      = '';

              this._findbarIcon.classList.remove('fa-times');

              this._findbarIcon.classList.add('fa-search');

              this.focus();

              this.dispatchEvent(new Event('dropdownItemDeselected'));

            });

        }
      }
  }
  
  clientTyping() {

      clearTimeout(this._typingTimer);

      if(this.value) {

        this._typingTimer = setTimeout(() => {

          this.doneTyping(this.value);

        }, this._doneTypingInterval);

      } else {

        this.dropdownActions().close();

      }

  }

  doneTyping(value_) {

      this.spinner().start();

      setTimeout(() => {

          fetch(this._findbarAction,{
             method: 'post',
             headers: {
                 'content-type': 'application/json; charset=utf-8',
             }
         })
        .then(response => response.json())
        .then((json_) => {

            this.spinner().stop();
      
            this._jsonResponse = json_.d;

            this.dropdownActions().open();

        });

      },1000);

  }

};


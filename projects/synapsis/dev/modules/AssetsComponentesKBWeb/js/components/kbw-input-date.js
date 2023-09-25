export class KBWInputDate extends HTMLInputElement {
  
  // Setters & Getters 

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

  // Constructor

  constructor() {
      super();  

      if(this.type == 'text') {
	  
          //Prepare Default Properties and Attributes

          this.initialize();

          //Draw

          this.draw();

          //Prepare Events

      }
      
  }

  //Initialize

  initialize() {

      //Properties
      
      //Attributes
        
      this.classList.add('form-control');

      this.classList.add('datepicker');

      this.setAttribute('data-inputmask-alias', 'datetime');

      this.setAttribute('data-inputmask-inputformat', 'dd/mm/yyyy');

      this.setAttribute('im-insert', 'false');

      this.id            = this.name;

      this.autocomplete  = 'off';

  }

  //Draw

  draw() {

      const template_           = document.createElement('template');
      //input-group for icon
      template_.innerHTML = `
        <div class="form-group position-relative">
          <label class="form-control-flotating" for="${this.id}">${this.placeholder}</label>
        </div>
      `;
      
      const parentNode_        = this.parentNode;

      const templateContent_   = template_.content.cloneNode(true);
      
      const elementContainer_  = templateContent_.querySelector('.form-group');

      parentNode_.insertBefore(templateContent_, this);

      elementContainer_.appendChild(this);

      elementContainer_.appendChild(elementContainer_.querySelector('.form-control-flotating'));

  }

  // Methods

};


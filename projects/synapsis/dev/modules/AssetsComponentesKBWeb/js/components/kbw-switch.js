export class KBWSwitch extends HTMLInputElement {
  
  // Setters & Getters 

  set hidden(value_) {
    
      this.closest('.form-group').style.display = value_ ? 'none' : '';

  }

  // Constructor

  constructor() {
      super();  

      if(this.type == 'checkbox') {
	  
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
        
      this.classList.add('custom-control-input');

      this.id  = this.name + '_' + parseInt(Math.random()*1000000000, 10);

  }

  //Draw

  draw() {

      const template_          = document.createElement('template');

      template_.innerHTML = `
        <div class="form-group">
          <div class="custom-control custom-switch">
            <label for="${this.id}" class="custom-control-label">${this.placeholder}</label>
          </div>
        </div>
      `;
      
      const parentNode_        = this.parentNode;

      const templateContent_   = template_.content.cloneNode(true);
      
      const elementContainer_  = templateContent_.querySelector('.custom-switch');

      parentNode_.insertBefore(templateContent_, this);

      elementContainer_.appendChild(this);

      elementContainer_.appendChild(elementContainer_.querySelector('.custom-control-label'));
  }

  // Methods

};


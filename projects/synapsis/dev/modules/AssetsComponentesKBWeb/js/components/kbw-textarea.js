export class KBWTextArea extends HTMLTextAreaElement {
  
  // Setters & Getters 

  set hidden(value_) {
    
      this.parentNode.style.display = value_ ? 'none' : '';

  }

  // Constructor

  constructor() {
      super();  

      //Prepare Default Properties and Attributes

      this.initialize();

      //Draw

      this.draw();

      //Prepare Events

  }

  //Initialize

  initialize() {

      //Properties
      
      //Attributes
      
      this.id            = this.name;

      this.classList.add('form-control');

      this.autocomplete  = 'off';

  }

  //Draw

  draw() {

      const template_           = document.createElement('template');

      template_.innerHTML = `
        <div class="form-group">
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


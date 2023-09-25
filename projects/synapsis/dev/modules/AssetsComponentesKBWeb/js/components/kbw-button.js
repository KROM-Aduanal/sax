export class KBWButton extends HTMLButtonElement {
  
    // Setters & Getters 

    set hidden(value_) {
    
        this.style.display = value_ ? 'none' : '';
  
    }

    set text(value_) {

        this._buttonLabel.innerHTML = value_;

    }

    get spinner() {

        return {
            start : () => {

                this.disabled = true;

                this._buttonSpinner.classList.remove('d-none');

            },
            stop: () => {
                
                this.disabled = false;

                this._buttonSpinner.classList.add('d-none');

            }
        }    

    }

    // Constructor

    constructor() {
        super();  

        //Prepare Default Properties and Attributes

        //Draw

        this.draw();
        
        //Prepare Events

    }

    //Initialize
    
    //Draw

    draw() {

        //Properties
      
        //Attributes
        
        this.innerHTML = `
          <span class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
          <span class="btn-text">${this.innerText}</span>
        `;

        //Identified Componentes

        this._buttonSpinner = this.querySelector('.spinner-border');

        this._buttonLabel   = this.querySelector('.btn-text');
       
    }

    // Methods

};
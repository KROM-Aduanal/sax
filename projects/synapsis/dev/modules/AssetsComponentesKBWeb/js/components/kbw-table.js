export class KBWTable extends HTMLTableElement {
  
  // Setters & Getters 

  get tableRows() {
  
      return {
          add : (rowData_) => {
              
              const tHead_ = this.tHead.querySelectorAll('th');

              const dataBinding_ = Array.from(tHead_).map(o => o.getAttribute('bind'));

              const row_ = this.querySelector('tbody').insertRow();

              dataBinding_.forEach((bind_, i_) => {

                    const cell_    = row_.insertCell();

                    cell_.setAttribute('label', tHead_[i_].innerText);

                    const fields_  =  bind_.split('.');

                    var value_     = false;

                    fields_.forEach((field) => {

                        value_ = value_? value_[field] : rowData_[field];

                    });

                    cell_.innerText = value_;

               });

               row_.animate([
                        { opacity: '0' },
                        { opacity: '1' }
               ], {
                   duration: 1000
               }); 

               // Add Row Actions
               if(this.actions.length) {
            
                   this.setActions(row_)
            
               }
              
          },
          update : (indexRow_, rowData_) => {
          
              const tHead_ = this.tHead.querySelectorAll('th');

              const dataBinding_ = Array.from(tHead_).map(o => o.getAttribute('bind'));

              const row_ = this.rows[indexRow_ + 1];

              Object.entries(row_.childNodes).forEach((column_, i_) => {
                  
                  const fields_  =  dataBinding_[i_].split('.');

                  var value_     = false;

                  fields_.forEach((field) => {

                      value_ = value_? value_[field] : rowData_[field];

                  });

                  column_[1].innerText = value_;  
              
              });

              row_.animate([
                    { color: '#14937b' },
                    { color: '' }
              ], {
                  duration: 2000
              });

          },
          delete : (indexRow_) => {
          
              const row_ = this.rows[indexRow_];
              
              row_.animate([
                        { opacity: '1' },
                        { opacity: '0' }
                   ], {
                        duration: 1000
                   });

              setTimeout(() => row_.remove(), 1000);
          
          }
      };

  }

  set dataSource(source_) {
    
    const tHead_ = this.tHead.querySelectorAll('th');

    const dataBinding_ = Array.from(tHead_).map(o => o.getAttribute('bind'))
   
    source_.forEach((item_) => {

        const row_ = this.querySelector('tbody').insertRow();

        dataBinding_.forEach((bind_, i_) => {

            const cell_    = row_.insertCell();

            cell_.setAttribute('label', tHead_[i_].innerText);

            const fields_  =  bind_.split('.');

            var value_     = false;

            fields_.forEach((field) => {

                value_ = value_? value_[field] : item_[field];

            });

            cell_.innerText = value_;

        });

        // Add Row Actions
        if(this.actions.length) {
            
            this.setActions(row_)
            
        }

    });

    if(this.parentNode.clientWidth == this.parentNode.scrollWidth) {

      this.parentNode.parentNode.style.setProperty('--gradient-blur', 0);

    }

  };


  setActions(row_) {
  
    const cell  = row_.insertCell();

    this.actions.forEach((action) => {

        const button = action.cloneNode(true);

        cell.appendChild(button);

        button.addEventListener('click', (e) => {

            e.preventDefault();

            const row = e.target.closest('tr');
                    
            if(e.target.hasAttribute('delete-element')) {
                        
                DisplayAlert("Eliminar Registro","¿Esta seguro(a) que desea realizar esta acción?","Aceptar","Cancelar").then(() => {
                        

                    fetch(e.target.getAttribute('action'),{
                            body: JSON.stringify({id_: e.target.id}),
                            method: 'post',
                            headers: {
                                'content-type': 'application/json; charset=utf-8',
                            }
                    })
                    .then(response => response.json())
                    .then((json) => {

                               
                        if(json.d.code == 200) {
                    
                            this.tableRows.delete(row.rowIndex);

                            DisplayMessage(json.d.message)
                  
                        } 
                  

                    });


                });

            }

        });

    });

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

      this.actions = [];

      //Attributes

  }

  //Draw

  draw() {

      const template_  = document.createElement('template');

      template_.innerHTML = `
        <div class="kbw-table">
          <div class="table-responsive">
          </div>
        </div>
      `;

      const parentNode_        = this.parentNode;

      const templateContent_   = template_.content.cloneNode(true);
      
      const elementContainer_  = templateContent_.querySelector('.table-responsive');

      parentNode_.insertBefore(templateContent_, this);

      elementContainer_.appendChild(this);
      
      let actionsTemplate = this.querySelector('template');

      if(actionsTemplate) {
      
          actionsTemplate = actionsTemplate.content.cloneNode(true);
          
          const actions = actionsTemplate.querySelectorAll('button');
          
          Object.entries(actions).forEach((action, i) => {
          
              this.actions.push(action[1]);

          });

      }

  }

  //Events

  triggers() {

      this.parentNode.addEventListener('scroll', (e) => {
      
        const scroll_ = ((100 - Math.min(e.target.scrollLeft, 100)) / 100).toFixed(1);

        e.target.parentNode.style.setProperty('--gradient-blur', scroll_);

      });    

  }

  // Methods

};



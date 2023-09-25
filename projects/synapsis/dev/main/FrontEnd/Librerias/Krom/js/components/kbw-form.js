export class KBWForm extends HTMLFormElement {
  
  // Setters & Getters 

  set isEditing(value_) {
      
      this.form_in_editing = value_;

  };

  get isEditing(){
    
      return this.form_in_editing;

  };

  set pushView(url_) {

      fetch(url_).then((response_) => {

          response_.text().then((text_) => {
              
              const identifier_ = url_.split('/').pop().replace('.aspx','');

              this._formActionButton.hidden  = true;

              this._formFindbar.hidden = true;    

              this._formBackButton.classList.remove('d-none');

              const view_ = document.createElement('div');

              view_.setAttribute('class', 'kbw-subform border-right');

              view_.setAttribute('id', 'push_'+identifier_);
              
              view_.innerHTML = text_;

              this._formChildContainer.appendChild(view_);

              setTimeout(() => { view_.style.left = '0%'; },250);
        
              this.pushedChildView.push(document.getElementById('push_'+identifier_));  
    
              const form_ = view_.querySelector('form');

              form_.addEventListener('submit', (e) => {

                  e.preventDefault();

                  this.formSubmit(form_, () => {

                      const formElements_  = e.target.elements;

                      var formObject_        = {
                          id: identifier_,
                          content: form_,
                          close: () => {
                              this.popView();
                          }
                      };

                      this.dispatchEvent(new CustomEvent('formDetailSubmit', { detail: formObject_ })); 

                });

             });


             this.clearInputsFormErrors(form_);
               

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
      
        this.pushedChildView = [];

                 //Attributes
     
        const contentHeight  = document.querySelector('.main-header').offsetHeight + document.querySelector('.main-footer').offsetHeight + 90;

        const windowHeight   = window.innerHeight;

        this._sectionHeight  = windowHeight - contentHeight;

        this.style.maxHeight = this._sectionHeight + 'px';

             }

                 //Draw

  draw() {

      this._formSections  = this.querySelectorAll('legend');

      const template_     = document.createElement('template');

      template_.innerHTML = `
        <div class="kbw-form position-relative">
            <ul class="nav justify-content-between align-items-center border-bottom p-2 mb-2 bg-light d-none">
              <li class="nav-item btn-back d-none">
                <a class="btn" href=""><i class="fas fa-chevron-left"></i> Regresar</a>
              </li>  
              <li class="nav-item">
                <input class="kbw-findbar" is="kbw-findbar" placeholder="Buscar Registro" action="${this.getAttribute('findbar-action')}" text-field="${this.getAttribute('findbar-text-field')}" class="form-control">
              </li>
              <li class="nav-item flex-group text-right">
                <button class="btn btn-primary btn-save" is="kbw-button"><i class="far fa-save" style="font-size: 22px;"></i></button>
              </li>
            </ul>
            <div class="row no-gutters position-relative overflow-hidden kbw-form-pushed">
              <div class="col-xs-12 col-lg-auto align-items-stretch d-flex" style="transition: 0.5s all;">
                <div class="kbw-form__menu-line"><span class="kbw-form__menu-dot"></span></div>
                <ul class="kbw-form-menu nav flex-lg-column p-2 w-100">
                  ${this.setFormMenu()}
                </ul>
              </div>
              <div class="col-xs-12 col-lg p-2 position-relative overflow-hidden"></div>
            </div>
        </div>
      `;
               
      const parentNode_        = this.parentNode;

      const templateContent_   = template_.content.cloneNode(true);
      
      const elementContainer_  = templateContent_.querySelector('.col-lg');

      parentNode_.insertBefore(templateContent_, this);

      elementContainer_.appendChild(this);

      //Identified Componentes

      this._formActionButton    = this.closest('.kbw-form').querySelector('.btn-save');

      this._formFindbar         = this.closest('.kbw-form').querySelector('.kbw-findbar');

      this._formBackButton      = this.closest('.kbw-form').querySelector('.btn-back');

      this._formNavItems        = this.closest('.kbw-form').querySelectorAll('a.nav-link');

      this._formMenuDot         = this.closest('.kbw-form').querySelector('.kbw-form__menu-dot');

      this._formChildContainer  = this.closest('.kbw-form').querySelector('.kbw-form-pushed')

      this.classList.remove('d-none');

  }

  //Events

  triggers() {

      this._formActionButton.addEventListener('click', () => {

          this.formSubmit(this, () => {
   
              this._formActionButton.spinner.start();

              let formData_ = new FormData(this);

              formData_.delete(this.querySelector('[isKey]').getAttribute('name'));

              fetch(this.action,{
                  body: JSON.stringify({
                      datos_: {
                          updateOnKeyValue: this.querySelector('[isKey]').value
                          , attributes: Object.fromEntries(formData_)
                      }
                  }),
                 method: 'post',
                 headers: {
                    'content-type': 'application/json; charset=utf-8',
                 }
              })
              .then(response => response.json())
              .then((json) => {

                  this._formActionButton.spinner.stop();
                  
                    if(json.d.code == 200) {
                    
                        this.clearForm();
                        
                        DisplayMessage(json.d.message);
                  
                    } else {

                        if ('errors' in json.d) {

                            Object.entries(json.d.errors).forEach((dataError_) => {
                            
                                    const currentInput_ = this.querySelector('[name="'+dataError_[0]+'"]');

                                    const lastErrror_ = currentInput_.parentNode.querySelector('small');

                                    if(lastErrror_) {

                                      lastErrror_.remove();

                                    }
        
                                    const error_     = document.createElement('small');

                                    error_.innerText = dataError_[1];

                                    currentInput_.parentNode.appendChild(error_);

                                    currentInput_.parentNode.classList.add('has-error');

                            });
                        
                        } else {
                            
                            DisplayAlert('Algo ha ocurrido', json.d.message);

                        }

                    } 
                  

                });

            });
      
     });

    this._formFindbar.addEventListener('dropdownItemSelected', (e_) => {
        
        //this._formActionButton.text  = 'Editar';
        this._formActionButton.innerHTML = '<i class="fas fa-pencil-alt" style="font-size: 22px;"></i>';

        this.prepareUpdate(e_);

        this.isEditing = true

    });

    this._formFindbar.addEventListener('dropdownItemDeselected', (e_) => {
        
         //this._formActionButton.text  = 'Guardar';
        this._formActionButton.innerHTML = '<i class="far fa-save" style="font-size: 22px;"></i>';

         this.isEditing = false

         this.clearForm()

        });

      this._formBackButton.addEventListener('click', (e_) => { 

          e_.preventDefault();

          this.popView(); 

        });

      this._formNavItems.forEach((a_) => {

        a_.addEventListener('click', (e_) => {

          e_.preventDefault();

          const sectionId_ = Number(e_.target.getAttribute('id')) - 1;
            
          this.scroll({
              top: this._formSections[sectionId_].closest('fieldset').offsetTop,
            behavior: 'smooth'  
        });

          this._formNavItems.forEach(el_ => el_.classList.remove('active'));

            e_.target.classList.add('active');

            let positioTop_ = parseInt(e_.target.parentElement.offsetTop) + parseInt(14);

            this._formMenuDot.style.top = positioTop_ + 'px';

        });

      });


      function isElementInViewport (el, self) {

         
          if (typeof jQuery === "function" && el instanceof jQuery) {
              el = el[0];
        }

          var rect = el.getBoundingClientRect();
          
          return (
              rect.top >= 100 &&
              rect.left >= 0 &&
              rect.bottom <= (window.innerHeight - 100 || self.clientHeight - 100) && 
              rect.right <= (window.innerWidth || self.clientWidth) 
          );
          
        }

      function onVisibilityChange(el, self, callback) {
       
          var old_visible = false;

          var visible = isElementInViewport(el, self);

          if (visible != old_visible) {

              old_visible = visible;

              if (typeof callback == 'function') {

                  callback();
    
                }
            }
      }

            this.addEventListener("scroll", (e) => {
          
                this._formSections.forEach((legend, i) => {

                    onVisibilityChange(legend, this, () => {
                  
                        this._formNavItems.forEach(el => el.classList.remove('active'));

                 this._formNavItems[i].classList.add('active');

              });
            
          });

    });

    window.addEventListener('resize', (e) => {
       
        const contentHeight  = document.querySelector('.main-header').offsetHeight + document.querySelector('.main-footer').offsetHeight + 90;

        const windowHeight   = window.innerHeight;

        this._sectionHeight  = windowHeight - contentHeight;

        this.style.maxHeight = this._sectionHeight + 'px';

        this._formSections.forEach((legend_, i_) => {

            //legend_.parentNode.style.minHeight = this._sectionHeight + 'px';

            legend_.closest('fieldset').style.minHeight = this._sectionHeight + 'px';
        
        });


    });

    this.clearInputsFormErrors(this);

  }

  // Methods

  popView() {
      
      const view_ = this.pushedChildView.pop()
      
      view_.style.left = '-100%';
      
      setTimeout(() => {

        if(this.pushedChildView.length == 0) {

            this._formActionButton.hidden  = false;

            this._formFindbar.hidden = false;

            this._formBackButton.classList.add('d-none');

        }

        setTimeout(() => {
        
            view_.remove();
        
        },500);

      },500);
      
  }

  setFormMenu() {

      const formNav_    = [];

      if(this._formSections.length) { 

          this._formSections.forEach((legend_, i_) => {

          //legend_.parentNode.style.minHeight = this._sectionHeight + 'px';
        legend_.closest('fieldset').style.minHeight = this._sectionHeight + 'px';

          const navItem_ = document.createElement('li');

          const navLink_  = document.createElement('a');

          navItem_.classList.add('nav-item');

          navItem_.classList.add('menu-legend');

          navLink_.setAttribute('href', '');

          navLink_.setAttribute('id', (i_ + 1));

          navLink_.classList.add('nav-link');

          navLink_.classList.add('position-relative');

          if(this._formSections.length >= 3) {

              navLink_.classList.add('nav-link-responsive');

          }

          //navLink_.innerText = legend_.innerHTML;
            navLink_.innerText = legend_.innerText;

          if(i_ == 0)  navLink_.classList.add('active');

          navItem_.appendChild(navLink_);

          formNav_.push(navItem_.outerHTML);
         
        });
      
      }

      return formNav_.join('');
    
  }

    formSubmit(form_, func_) {

        if(this.formValid(form_)) {

            func_();

        }

    }


   formValid(form_) {

    var errors_    = [];

    const validator = {
        required: (currentInput_) => {
          
          var text_             = currentInput_.value;
          
          var errorMessage_     = 'Este campo es requerido';

          if(text_.trim() == "") {

              setMessage(currentInput_, errorMessage_);

          }
        },
        valid_email: (currentInput_) => {

          var text_             = currentInput_.value;
        
          var errorMessage_     = 'Debe contener una dirección de correo valida';

          if(text_.trim() != "") {

            if(!(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(text_))) {
              
              setMessage(currentInput_, errorMessage_);
              
            }

          }
        },
        match: (currentInput_, extra_) => {

          var text_             = currentInput_.value;

          var matchInput_       = document.querySelector('[name="'+extra_+'"]');

          var matchText_        = matchInput_.value;

          var errorMessage_     = 'El campo {field_a} y {field_b} no coinciden';

          errorMessage_         = errorMessage_.replace("{field_a}",currentInput_.placeholder).replace("{field_b}",matchInput_.placeholder);

          if(text_.trim() != "") {

            if(text_ != matchText_){

              setMessage(currentInput_, errorMessage_);

            }

          }
        },
        onlytext: function(currentInput_) {
            
          var text_             = currentInput_.value;

          var errorMessage_     = 'Solo se permiten letras';

          if(text_.trim() != "") {

            if(!(/^[A-Za-záÁéÉíÍóÓúÚñÑ ]+$/i.test(text_))) {

              setMessage(currentInput_, errorMessage_);

            }
          }
        },
        onlynumber: function(currentInput_) {
          
          var text_             = currentInput_.value;
          
          var errorMessage_     = 'Solo se permiten caracteres numéricos';

          if(text_.trim() != "") {

            if(!(/^[0-9. ]+$/i.test(text_))) {

              setMessage(currentInput_, errorMessage_);

            }
          }
        },
        onlyinteger: function(currentInput_) {

          var text_             = currentInput_.value;

          var errorMessage_     = 'Solo se permiten números enteros';

          if(text_.trim() != "") {

            if(!(/^[0-9 ]+$/i.test(text_))) {

              setMessage(currentInput_, errorMessage_);

            }
          }
        },
        max_length: function(currentInput_, extra){
          
          var text_             = currentInput_.value;
          
          var errorMessage_     = 'Debe tener como máximo {n} caracteres';
          
          errorMessage_         = errorMessage_.replace("{n}",extra);

          if(text_.trim() != "") {
            
            if(text_.length > extra){

              setMessage(currentInput_, errorMessage_);

            }

          }
        },
        min_length: function(currentInput_, extra){
           
          var text_             = currentInput_.value;

          var errorMessage_     = 'Debe tener como mínimo {n} caracteres';

          errorMessage_         = errorMessage_.replace("{n}",extra);
          
          if(text_.trim() != "") {

            if(text_.length < extra){

              setMessage(currentInput_, errorMessage_);

            }

          }
        },
        alphanumeric: function(currentInput_) {
          
          var text_             = currentInput_.value;

          var errorMessage_     = 'Solo se permiten letras y números';

          if(text_.trim() != "") {

            if(!(/^[A-Za-záÁéÉíÍóÓúÚñÑ0-9. ]+$/i.test(text_))) {

              setMessage(currentInput_, errorMessage_);

            }
          }
        },
        valid_url: function(currentInput_) {
          
          var text_      = currentInput_.value;
          
          var errorMessage_   = 'Debe contener una dirección url valida';

          if(text_.trim() != "") {

            if(!(/(https?:\/\/(?:www\.|(?!www))[^\s\.]+\.[^\s]{2,}|www\.[^\s]+\.[^\s]{2,})/.test(text_))) {

              setMessage(currentInput_, errorMessage_);

            }
          }
        },
        filetype: function(currentInput_, extra_) {
          
          var files_    = currentInput_.files;
          
          var fileError = [];

          var types_    = Array.from(files_).map((file) => file);

          for(var i in types_) {

            const currentType_ = types_[i].type;

            if(currentType_ != "") {

              if(currentType_ != extra_) {

                var errorMessage_   = 'El archivo {file} debe ser de tipo {type}';
                
                errorMessage_       = errorMessage_.replace("{file}",types_[i].name);
                
                errorMessage_       = errorMessage_.replace("{type}",extra);

                fileError.push(errorMessage_);

              }

            } else {

              var ext_ = ((types_[i].name).split('.')).pop();

              if(ext_ != extra_) {

                  var errorMessage_   = 'El archivo {file} debe ser de tipo {type}';
                
                  errorMessage_       = errorMessage_.replace("{file}",types_[i].name);
                
                  errorMessage_       = errorMessage_.replace("{type}",extra);

                  fileError.push(errorMessage_);

              }
            }
          }

          if(fileError.length > 0) {

            setMessage(currentInput_, fileError);

          }

        },
        filesize: function(currentInput_, extra_) {
          
          var files_ = currentInput_.files;
          
          var types_ = Array.from(files_).map((file) => file);

          var fileError = [];

          for(var i in types_) {

            if((types_[i].size/1024) > Number(extra_)) {

                var errorMessage_   = 'El archivo {file} no debe pesar mas de {size} kb';

                errorMessage_       = errorMessage_.replace("{file}",types_[i].name);

                errorMessage_       = errorMessage_.replace("{size}",extra);

                fileError.push(errorMessage_);

            }
          }

          if(fileError.length > 0) {

            setMessage(currentInput_, fileError);

          }
        }
    };

    function setMessage(currentInput_, errorMessage_) {

        currentInput_.error = errorMessage_
        
        errors_.push({
          input:    currentInput_.name,
          message:  errorMessage_
        });

    }

      
    function getExtra(str_) {
      
      var extra_    = str.substring(str_.indexOf("[") + 1, str_.indexOf("]"));

      var func_      = str.replace("[" + extra_ + "]", "");

      return [func_, extra_];

    };

    const formElements_  = form_.elements;
      
    for(let i = 0; i < formElements_.length; i++) {

        if(formElements_[i].type != 'fieldset') { 

            var rules_       = formElements_[i].getAttribute('rules') != null ? (formElements_[i].getAttribute('rules')).split("|") : false;

            for(let r = 0; r < rules_.length; r++) {
          
                  var rul_ = (/\[([A-Za-z0-9\/]+)\]/.test(rules_[r])) ? getExtra(rules_[r])[0] : rules_[r];
                  
                  var fn_  = validator[rul_] != false ? validator[rul_] : false;
                  
                  if(typeof fn_ == 'function') {

                      fn_(formElements_[i], ((/\[([A-Za-z0-9\/]+)\]/.test(rules_[r])) ? getExtra(rules_[r])[1] : false));
                 
                    }

            }
        }
    }

    return errors_.length > 0 ? false : true;

  }

  prepareUpdate(e_) {

      const row_ = e_.target.selectedItem;
     
      const formElements_  = form.elements;
    
      for (let i=0, item; item = formElements_[i]; i++) {
          
          const bind_ = item.getAttribute('name'); 

          if(bind_) {

              const fields_ = bind_.split('.');

              var value_     = false;

              fields_.forEach((field_) => {

                  value_ = value_? value_[field_] || '' : row_[field_] || '';

            });

            if(item.type.toLowerCase() == 'radio' || item.type.toLowerCase() == 'checkbox') {
                
                item.checked = item.value == value_ ? true : false;

            } else if(item.type.toLowerCase() == 'select-one'){

                item.value = value_;
                
                item.dispatchEvent(new Event("change"));

            } else {

                item.value = value_;

            }
              
              

         }
      
     }  

  }

  clearInputsFormErrors(form_) {
        
        const formElements_  = form_.elements;
      
        for (let i = 0; i < formElements_.length; i++) {

            const input_ = formElements_[i];

            if(input_.type != 'fieldset') {
            
                if(input_.classList.contains('select2'))
                {
                    $(input_).on('select2:open', (e) => {
                        this.clearErrors(e);
                    });
                }
                else
                {
                    input_.addEventListener('focus', (e) => this.clearErrors(e));
                }

            }
        }

  }

  clearErrors(e_) {
              
      const error_ = e_.target.parentNode.querySelector('small');

      if(error_) {

        error_.remove();

        e_.target.parentNode.classList.remove('has-error');

        }

    }

    clearForm(){
        
        if(!this.isEditing)
        {
            this.clear();
        }

    }

};



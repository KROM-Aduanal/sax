export default function() { 

    HTMLElement.prototype.closest = function(selector){
    
        let el = this;

        let matchesFn;

        let parent;

        // find vendor prefix
        ['matches','webkitMatchesSelector','mozMatchesSelector','msMatchesSelector','oMatchesSelector'].some(function(fn) {
        
            if (typeof document.body[fn] == 'function') {

                matchesFn = fn;
        
                return true;
            }

            return false;
    
        });

        // traverse parents
        while (el) {
        
            parent = el.parentElement;
        
            if (parent && parent[matchesFn](selector)) {
        
                return parent;
            }

            el = parent;
    
        }

        return null;

    };

};


/*HTMLElement.prototype.tooltip = function(message){
  
  this.parentNode.style.position = 'relative';

  const tooltip = document.createElement('div');

  tooltip.classList.add('kbw-tooltip');

  tooltip.innerText = message;

  this.parentNode.appendChild(tooltip);

  this.parentNode.querySelector('.kbw-tooltip').addEventListener('click', e => e.target.remove() );

};*/


HTMLFormElement.prototype.serialized = function(){

    // Setup our serialized data
    var serialized = [];

    // Loop through each field in the form
    for (var i = 0; i < this.elements.length; i++) {

        var field = form.elements[i];

        // Don't serialize fields without a name, submits, buttons, file and reset inputs, and disabled fields
        if (!field.name || field.disabled || field.type === 'file' || field.type === 'reset' || field.type === 'submit' || field.type === 'button') continue;

        // If a multi-select, get all selections
        if (field.type === 'select-multiple') {
            for (var n = 0; n < field.options.length; n++) {
                if (!field.options[n].selected) continue;
                serialized.push(encodeURIComponent(field.name) + "=" + encodeURIComponent(field.options[n].value));
            }
        }

            // Convert field data to a query string
        else if ((field.type !== 'checkbox' && field.type !== 'radio') || field.checked) {
            serialized.push(encodeURIComponent(field.name) + "=" + encodeURIComponent(field.value));
        }
    }

    return serialized.join('&');

}

HTMLFormElement.prototype.clear = function(){

    this.reset();
         
    const selectElements_ = this.querySelectorAll('select, input');
        
    if(selectElements_.length) {
             
        selectElements_.forEach((el_) => el_.dispatchEvent(new Event("change")));
             
    }

}
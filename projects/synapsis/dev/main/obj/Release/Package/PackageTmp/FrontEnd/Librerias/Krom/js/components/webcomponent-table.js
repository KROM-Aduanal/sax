export class WCTable extends HTMLTableElement {

	set dataSource(value) {

		if (value) {

			this.setTHead(Object.keys(value[0]));

			this.setTBody(value);

        }

	}

	constructor() {

      	super();
		
  	}

  	connectedCallback() {
  		
  		this.querySelector('input').addEventListener('keyup', e => this.controlTyping(e));

  	}

	
  	setTHead(thead) {

  		const tr = this.querySelector('thead').insertRow();

  		thead.forEach((col) => {

	    	const td    = tr.insertCell();

	        td.innerText = col;

	    });

  	}

  	setTBody(tbody) {

  	    this.querySelector('tbody').innerHTML = '';

  		tbody.forEach((row) => {

	        const tr = this.querySelector('tbody').insertRow();

	        Object.values(row).forEach((col) => {

	            const td    = tr.insertCell();

	            td.innerText = col;

	        });

	        tr.addEventListener('click', e => this.selectedRow(e));

	    });

  	}

  	selectedRow(e) {
  		
  		const indexPath = e.target.parentNode.rowIndex;

		this.dispatchEvent(new CustomEvent('selected', { detail: indexPath }));

  	}

  	controlTyping(e) {

  		const input = e.target;

  		const value = input.value;

  		const rows = this.querySelectorAll('table tbody tr');

  		if(value.length >= 1){

			for(let i = 0; i < rows.length; i++) {

			    let visible = false;

			    const cols = rows[i].querySelectorAll('td');

			    for(let j = 0; j < cols.length; j++ ) {

			        const col = cols[j];
			            
			        if(col.innerText.toLowerCase().indexOf(value.toLowerCase()) >= 0) {

			            visible = true

			        }

			    }

			    rows[i].style.display = visible? '' : 'none';

			}

		} else {

			for(let i = 0; i < rows.length; i++) {

				rows[i].style.display = '';

			}

		}

  	}

}







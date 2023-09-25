export class WCCatalog extends HTMLTableElement {

	//Atributos

	setAttributes() {

		this.classList.add('table');

		//this.classList.add('table-striped');

	}

	//Propiedad

	//Constructor

	constructor() {

		super();

		this.kromComponents = new KromComponentes();

		this.setAttributes();

		this.drawComponent();

		this.setDataSource();

		this.setEvents();

	}

	//Metodos

	drawComponent() {

		const template_ = document.createElement('template');

		template_.innerHTML = `
			<div class="wc-catalog">
				<div class="wc-catalog__actions-wrapper">
					<a href="" class="add-item d-inherit">${this.kromComponents.ObtenerSVG('plus-square-regular', '#607D8B', 24, 'cur-pointer')}</a>
					<a href="" class="duplicate-item d-inherit">${this.kromComponents.ObtenerSVG('copy-regular', '#607D8B', 24, 'cur-pointer')}</a>
					<a href="" class="delete-item d-inherit">${this.kromComponents.ObtenerSVG('trash-alt-regular', '#607D8B', 24, 'cur-pointer')}</a>
				</div>
				<div class="table-responsive pt-3 wc-catalog__body-wrapper">
				</div>
			</div>
		`;

		const parentNode_ = this.parentNode;

		const templateContent_ = template_.content.cloneNode(true);

		const elementContainer_ = templateContent_.querySelector('.table-responsive');

		parentNode_.insertBefore(templateContent_, this);

		elementContainer_.appendChild(this);

		//

		var cell_ = this.querySelector('tHead tr').insertCell(0);

		cell_.innerHTML = '<input type="checkbox"/>';

		cell_ = this.querySelector('tHead tr').insertCell();


		const tHead_ = this.tHead.querySelectorAll('th');

		this.fieldsDisplay = Array.from(tHead_).map(o => o.getAttribute('display'));

		this.fieldsType = Array.from(tHead_).map(o => o.getAttribute('type'));

		this.catalogDuplicateButton = elementContainer_.parentNode.querySelector('.duplicate-item');

		this.catalogDeleteButton = elementContainer_.parentNode.querySelector('.delete-item');

		this.catalogAddButton = elementContainer_.parentNode.querySelector('.add-item');

	}

	setDataSource() {

		try {

			const source_ = JSON.parse(this.getAttribute('source'));

			source_.forEach((data_) => {

				this.addTableRow(data_);

			});

		} catch (e) {



		}

	}

	addTableRow(data_) {

		const row_ = this.querySelector('tbody').insertRow();

		const tHead_ = this.tHead.querySelectorAll('th');

		var cell_ = row_.insertCell();

		cell_.innerHTML = '<input type="checkbox"/>';

		this.fieldsDisplay.forEach((field_, i_) => {

			const cell_ = row_.insertCell();

			switch (this.fieldsType[i_]) {

				case 'text':

					cell_.innerHTML = `<input is="kbw-input" type="text" placeholder="${tHead_[i_].innerText}" value="${data_ ? data_[field_] : ''}"/>`;

					break;

				case 'date':

					cell_.innerHTML = `<input is="kbw-input-date" type="text" placeholder="${tHead_[i_].innerText}" value="${data_ ? data_[field_] : ''}"/>`;

					break;

				case 'list':

					cell_.innerHTML = `
            			<select is="wc-select">
            				${(function () {

							const options = [];

							try {

								const items = JSON.parse(tHead_[i_].getAttribute('source'));

								items.forEach((item) => {

									options.push(`
            								<option value="${item[0]}">${item[1]}</option>
            							`);

								});

							} catch (e) {

								console.log(e);

							}

							return options.join('');

						})()}
            			</select>
            			`;

					break;

			}

		});

		var cell_ = row_.insertCell();

		//cell_.innerHTML = '<a href="" class="wc-catalog-delete-action">delete</a>';
		cell_.innerHTML = `<a href="" class="wc-catalog-delete-action">${this.kromComponents.ObtenerSVG('times-circle', '#607D8B', 24, 'cur-pointer')}</a>`;
	}


	//Eventos

	setEvents() {

		this.catalogAddButton.addEventListener('click', (e) => {

			e.preventDefault();

			this.addTableRow();

		});


		this.catalogDeleteButton.addEventListener('click', (e) => {

			e.preventDefault();

			this.querySelectorAll('tbody tr td:nth-child(1) input:checked').forEach((input) => {

				input.parentNode.parentNode.remove();

			});

			this.querySelector('thead tr td input').checked = false;

		});


		this.catalogDuplicateButton.addEventListener('click', (e) => {

			e.preventDefault();

			this.querySelectorAll('tbody tr td:nth-child(1) input:checked').forEach((input) => {

				input.checked = false;

				const row = input.closest('tr').cloneNode(true);

				input.parentNode.parentNode.after(row);

			});

			this.querySelector('thead tr td input').checked = false;

		});


		this.querySelector('thead tr td input').addEventListener('click', (e) => {

			if (e.target.checked) {

				this.querySelectorAll('tbody tr td:nth-child(1) input').forEach((input) => {

					input.checked = true;

				});

			} else {

				this.querySelectorAll('tbody tr td:nth-child(1) input').forEach((input) => {

					input.checked = false;

				});

			}

		});


		this.addEventListener('click', (e) => {

			if (e.target && e.target.classList.contains('wc-catalog-delete-action')) {

				e.preventDefault();

				e.target.parentNode.parentNode.remove();

			}

		});

	}


}
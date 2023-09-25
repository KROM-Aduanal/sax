export class WCCollectionView extends HTMLUListElement {


    constructor() {

        super();
        
    }

    connectedCallback() {
        
        //let currentPage = 1;
        //const limit = 10;
        //let total = 0;

        /*window.addEventListener('scroll', () => {

            const {
                scrollTop,
                scrollHeight,
                clientHeight
            } = document.documentElement;

            if (scrollTop + clientHeight >= scrollHeight - 5 &&
                this.hasMoreRows(currentPage, limit, total)) {
                currentPage++;
                this.loadRows(currentPage, limit);
            }

        }, {
            passive: true
        });*/

        //this.component = this.closest('.__component');

        //this.loadRows(currentPage, limit);
        
       
    }

    /*hasMoreRows(page, limit, total) {

        const startIndex = (page - 1) * limit + 1;

        return total === 0 || startIndex < total;

    }

    loadRows(page, limit) {

        //showLoader();

        setTimeout(async () => {
            try {

                if (this.hasMoreQuotes(page, limit, total)) {

                    const response = await getQuotes(page, limit);

                    showQuotes(response.data);

                    total = response.total;
                }
            } catch (error) {

                console.log(error.message);

            } finally {

                //hideLoader();
            }

        }, 500);

    }*/

    
}

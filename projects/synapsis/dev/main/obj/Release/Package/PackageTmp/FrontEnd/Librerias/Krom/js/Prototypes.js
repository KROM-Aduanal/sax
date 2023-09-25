NodeList.prototype.addEventListener = function (event, func) {

    this.forEach((node) => {

        if (typeof func === 'function') {

            node.addEventListener(event, (e) => {

                  func(e);

            });

        }

    });

}



export class WCImage extends HTMLDivElement {

    constructor() {

        super();

    }

    connectedCallback() {

        this._indexPath = 0;

        this._component = this;

        this._image = this._component.querySelector('img');

        this._bar = this._component.querySelector('div > div');

        this.preloadResources();

    }

    preloadResources() {

        var imgs_ = [];

        var loadImgs = 0;

        if (Number(this._component.getAttribute('img-sources')) > 0) {

            this._component.querySelectorAll('span').forEach((span_) => {

                imgs_.push(span_.getAttribute('img-url'));

            });

        } else {

            imgs_.push(this._component.getAttribute('img-url'));

        }

        imgs_.forEach((img_) => {

            var newImg_ = new Image;

            newImg_.onload = (e) => {

                loadImgs++;

                if (loadImgs == imgs_.length) {

                    this._component.classList.remove('preload');

                    if (loadImgs == 1) {

                        this._image.src = e.target.src;

                    } else {

                        this._bar.classList.add('run');

                        this.setHistory(imgs_);

                    }

                }

            }

            newImg_.src = img_;

        });

    }

    setHistory(imgs_) {

        this._image.src = imgs_[this._indexPath];

        this._indexPath = (this._indexPath < (imgs_.length - 1)) ? this._indexPath + 1 : 0;

        setTimeout(_ => this.setHistory(imgs_), 4000);

    }

}
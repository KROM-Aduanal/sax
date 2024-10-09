export class WCFile extends HTMLInputElement {

	constructor() {

        super();

        this.component = this.closest('.__component');

        this.dataField = this.component.querySelector('.__data');

        this.contentItems = this.component.querySelector('.__items');

        this.contentDetails = this.component.querySelector('.__details');

        this.fileInput = this.component.querySelector('input[type="file"]');

        //this.fileInput.setAttribute('accept', "application/pdf,image/jpeg,text/xml");

        this.uploadButton = this.component.querySelector('.__upload');

        this.downloadButton = this.component.querySelector('.__download');

        this.deleteButton = this.component.querySelector('.__delete');

        this.textInput = this.component.querySelector('.__text');

        this.valueInput = this.component.querySelector('.__value');
        
  	}

  	connectedCallback() {
            
  		if(this.type == 'file') {

            this.addEventListener('change', (event) => {

                const file = event.target.files[0];
                  
                this.readFile(file, () => {

                    this.placeholder = file.name;

                    const formdata = new FormData();

                    formdata.append("file", file);

                    formdata.append("data", this.dataField.value);

                    this.uploadButton.classList.add('in-process');

                    this.uploadFile(formdata, (res) => {

                        this.textInput.value = res.response.fileName;

                        this.valueInput.value = res.response.fileId;

                        this.downloadButton.classList.remove('disabled');

                        this.deleteButton.classList.remove('disabled');

                        this.uploadButton.classList.remove('in-process');

                    });

                });

            });

            this.component.addEventListener('drop', (event) => {

                event.preventDefault();

                this.component.classList.remove('dragable-over');

                this.files = event.dataTransfer.files;

                if (this.validateFilesType()) {

                    this.contentDetails.style.setProperty('--total', '"' + this.savedfiles + ' de ' + (this.files.length + this.savedfiles) + '"');

                    this.processDragableFile(this.files.length, 1);

                } else {

                    DisplayAlert('Lo sentimos!', 'Uno o mas documentos no estan permitidos; solo se admiten los tipos <b> ' + this.getAttribute('accept') + '</b>');

                }

            });
            
            this.component.addEventListener('dragover', (event) => {

                event.preventDefault();

                this.component.classList.add('dragable-over');

            });

            this.component.addEventListener('dragleave', () => {

                this.component.classList.remove('dragable-over');

            });
               
            try {

                const json = JSON.parse(this.dataField.value);
               
                switch (json.type) {

                    case 'upload':

                        json.type = 'nothing';

                        this.dataField.value = JSON.stringify(json);

                        this.fileInput.click();

                        break;
                    case 'dragable':

                        json.type = 'nothing';

                        this.dataField.value = JSON.stringify(json);

                        break;
                    case 'download':

                        json.type = 'nothing';

                        this.dataField.value = JSON.stringify(json);

                        this.downloadFile();

                        break;
                    case 'delete':

                        json.type = 'nothing';

                        this.dataField.value = JSON.stringify(json);

                        this.deleteFile();

                        break;
                       
                }

            } catch (e) { }

            try {

                const jsonfiles = JSON.parse(this.valueInput.value);

                this.setDefaultSavedFiles(jsonfiles);

            } catch (e) { }

            this.savedfiles = this.getSavedFiles().length

  		}
            
    }

    validateFilesType() {

        for (let i = 0; i < this.files.length; i++) {

            const typeimage_ = this.getAttribute('accept'); //+ ",image/jpeg"

            const types = typeimage_.split(',');

            if (!types.includes(this.files[i].type)) {

                return false;

            }

        }

        return true;

    }

    getSavedFiles() {

        try {

            return JSON.parse(this.valueInput.value);

        } catch (e) {

            return [];

        }

    }

    setDefaultSavedFiles(jsonfiles) {
       
        if (jsonfiles.length) {
            
            this.contentDetails.style.setProperty('--total', '"' + jsonfiles.length + ' de ' + jsonfiles.length + '"');
            
            jsonfiles.forEach((file) =>  {
                
                const item = document.createElement('span');

                const item_button = document.createElement('a');

                item_button.setAttribute('id', file.fileId);

                item_button.addEventListener('click', e => this.deleteDragableFile(e));

                item.append(document.createTextNode(file.fileName));

                item.append(item_button);

                this.contentItems.append(item);

            });

        } 

    }

    processDragableFile(num_items, index) {

        if (num_items > 0) {

            let file = this.files[index - 1];

            this.readFile(file, () => {

                const formdata = new FormData();

                formdata.append("file", file);

                formdata.append("data", this.dataField.value);

                this.uploadFile(formdata, (res) => {

                    this.contentDetails.style.setProperty('--total', '"' + (index + this.savedfiles) + ' de ' + (this.files.length + this.savedfiles) + '"');

                    const item = document.createElement('span');

                    const item_button = document.createElement('a');

                    item_button.setAttribute('id', res.response.fileId);

                    item_button.addEventListener('click', e => this.deleteDragableFile(e));

                    item.append(document.createTextNode(file.name));

                    item.append(item_button);

                    this.contentItems.append(item);

                    try {

                        let json = JSON.parse(this.valueInput.value);

                        json.push(res.response);

                        this.valueInput.value = JSON.stringify(json);

                    }
                    catch (e) {

                        this.valueInput.value = JSON.stringify([res.response]);

                    }

                    index++;

                    num_items--;

                    this.processDragableFile(num_items, index);

                });

            });

        } else {

            this.savedfiles = this.getSavedFiles().length;

        }

    }

    downloadFile() {

        try {

            const value = JSON.parse(this.dataField.value);
            
            const formdata = new FormData();

            formdata.append("data", JSON.stringify(value.data));

            this.downloadButton.classList.add('in-process');

            $.ajax({
                url: '/FileUploadHandler.download',
                type: 'POST',
                data: formdata,
                cache: false,
                contentType: false,
                processData: false,
                success: (res) => {

                    if (res.code == 200) {

                        var bytes = this.Base64ToBytes(res.response);

                        var blob = new Blob([bytes], { type: "application/octetstream" });

                        var url = window.URL || window.webkitURL;

                        var objectUrl = url.createObjectURL(blob);

                        var a = $("<a />")
                                .attr("download", value.data.fileName)
                                .attr("href", objectUrl);

                        $("body").append(a);

                        a[0].click();

                        a.remove();

                    } 

                    this.downloadButton.classList.remove('in-process');

                },
                error: (a) => {

                },
                failure: (a) => {

                }
            });

        } catch (e) { }
        
    }

    deleteDragableFile(e) {

        e.preventDefault();

        DisplayAlert('Eliminar archivo', '&#xBF;Esta seguro(a) que desea continuar con esta acci&oacute;n&#x3F;', 'Si', 'No', (accept) => {

            if (accept == true) {

                const fileId = e.target.id;

                try {

                    var json = JSON.parse(this.valueInput.value);

                    const formdata = new FormData();

                    formdata.append("data", JSON.stringify({ fileId: fileId }));

                    $.ajax({
                        url: '/FileUploadHandler.delete',
                        type: 'POST',
                        data: formdata,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: (res) => {

                            if (res.code == 200) {

                                json = json.filter(function (obj) {

                                    return obj.fileId !== fileId;

                                });

                                e.target.parentNode.remove();

                                this.valueInput.value = JSON.stringify(json);

                                this.savedfiles = this.getSavedFiles().length

                                this.contentDetails.style.setProperty('--total', '"' + this.savedfiles + ' de ' + this.savedfiles + '"');

                                DisplayMessage(res.message);

                            } else {

                                DisplayMessage(res.message, 2);

                            }
                            
                        },
                        error: (a) => {

                        },
                        failure: (a) => {

                        }
                    });

                }
                catch (e) { }

            }

        });

    }

    deleteFile() {

        try {

            const value = JSON.parse(this.dataField.value);

            DisplayAlert('Eliminar archivo', '&#xBF;Esta seguro(a) que desea continuar con esta acci&oacute;n&#x3F;', 'Si', 'No', (accept) => {

                if (accept == true) {

                    const formdata = new FormData();

                    formdata.append("data", JSON.stringify(value.data));

                    this.deleteButton.classList.add('in-process');

                    $.ajax({
                        url: '/FileUploadHandler.delete',
                        type: 'POST',
                        data: formdata,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: (res) => {

                            if (res.code == 200) {

                                this.textInput.value = this.textInput.getAttribute("placeholder");

                                this.valueInput.value = "";

                                this.downloadButton.classList.add('disabled');

                                this.deleteButton.classList.add('disabled');

                                this.savedfiles = this.getSavedFiles().length

                                this.contentDetails.style.setProperty('--total', '"' + this.savedfiles + ' de ' + this.savedfiles + '"');

                                DisplayMessage(res.message);

                            } else {

                                DisplayMessage(res.message, 2);

                            }
                            this.deleteButton.classList.remove('in-process');

                        },
                        error: (a) => {

                        },
                        failure: (a) => {

                        }
                    });

                }

            });

        } catch (e) { }

    }

    readFile(file, callback) {
       
	    var reader = new FileReader();

        reader.onload = () => {

            if (typeof callback == 'function') {

                callback();

            }


        };

        this.setAttribute('disabled', true);

        reader.readAsDataURL(file); 

    }

    uploadFile(formdata, callback) {

        $.ajax({
            url: '/FileUploadHandler.upload',
            type: 'POST',
            data: formdata,
            cache: false,
            contentType: false,
            processData: false,
            success: (res) => {

                if (res.code == 200) {

                    this.component.setAttribute('style', '--percent:100%;');

                    if (typeof callback == 'function') {

                        callback(res);

                    }

                    this.removeAttribute('disabled');

                    DisplayMessage(res.message);

                } else {

                    DisplayMessage(res.message, 2);

                }

            },
            error: (a) => {

                console.log("error");

                console.log(a);

            },
            failure: (a) => {

                console.log("failure");

                console.log(a);
            },
            xhr: () => {

                var fileXhr = $.ajaxSettings.xhr();

                console.log("xhr");

                console.log(fileXhr);

                if (fileXhr.upload) {

                    fileXhr.upload.addEventListener("progress", (e) => {

                        if (e.lengthComputable) {

                            var percentage = Math.ceil(((e.loaded / e.total) * 100)) - 25;
                            
                            this.component.setAttribute('style', '--percent:' + percentage + '%;');
                            
                        }

                    }, false);


                }
                return fileXhr;
            }
        });

    }

    Base64ToBytes(base64) {

        var s = window.atob(base64);

        var bytes = new Uint8Array(s.length);

        for (var i = 0; i < s.length; i++) {

            bytes[i] = s.charCodeAt(i);

        }

        return bytes;

    }
	
}

/*<configuration>
    <system.web>
        <compilation debug="true" targetFramework="4.5.2" />
        <httpRuntime executionTimeout="120" maxRequestLength="2147483647" />
        <pages controlRenderingCompatibilityVersion="4.0" />
    </system.web>
    <system.webServer>
        <security>
            <requestFiltering>
                <requestLimits maxAllowedContentLength="2147483648" />
            </requestFiltering>
        </security>
    </system.webServer>
</configuration>*/

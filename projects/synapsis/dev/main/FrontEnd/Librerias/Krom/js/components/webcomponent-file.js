export class WCFile extends HTMLInputElement {

	constructor() {

        super();

        this.component = this.closest('.__component');

        this.dataField = this.component.querySelector('.__data');

        this.fileInput = this.component.querySelector('input[type="file"]');

        this.uploadButton = this.component.querySelector('.__upload');

        this.downloadButton = this.component.querySelector('.__download');

        this.deleteButton = this.component.querySelector('.__delete');

        this.textInput = this.component.querySelector('.__text');

        this.valueInput = this.component.querySelector('.__value');

  	}

  	connectedCallback() {
  		
  		if(this.type == 'file') {

  			this.addEventListener('change', e => this.readFile(e));

            try {

                const json = JSON.parse(this.dataField.value);
               
                switch (json.type) {

                    case 'upload':

                        json.type = 'nothing';

                        this.dataField.value = JSON.stringify(json);

                        this.fileInput.click();

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


    deleteFile() {

        try {

            const value = JSON.parse(this.dataField.value);

            const formdata = new FormData();

            formdata.append("data", JSON.stringify(value.data));

            DisplayAlert('Eliminar archivo', '&#xBF;Esta seguro(a) que desea continuar con esta acci&oacute;n&#x3F;', 'Si', 'No', (accept) => {

                if (accept == true) {

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

  	readFile(e) {
  		
  		var input  = e.target;

	    var reader = new FileReader();

        reader.onload = () => {
 
            this.placeholder = input.files[0].name;

            const formdata = new FormData();

            formdata.append("file", input.files[0]);

            formdata.append("data", this.dataField.value);

            this.uploadButton.classList.add('in-process');
                
            $.ajax({
                url: '/FileUploadHandler.upload',
                type: 'POST',
                data: formdata,
                cache: false,
                contentType: false,
                processData: false,
                success:  (res) => {
                    
                    input.removeAttribute('disabled');

                    if (res.code == 200) {
                        
                        this.textInput.value = res.response.fileName;

                        this.valueInput.value = res.response.fileId;

                        this.downloadButton.classList.remove('disabled');

                        this.deleteButton.classList.remove('disabled');

                    } 

                    this.uploadButton.classList.remove('in-process');

                },
                error: (a) => {

                },
                failure: (a) => {
                    
                },
                xhr: () => {

                    var fileXhr = $.ajaxSettings.xhr();

                    if (fileXhr.upload) {
                       
                        fileXhr.upload.addEventListener("progress", (e) => {

                            if (e.lengthComputable) {

                                var percentage = Math.ceil(((e.loaded / e.total) * 100));
                                
                                /*this.component.setAttribute('style', '--percent:"Procesando ' + percentage + '%";');

                                if (percentage == 100) {

                                    this.component.setAttribute('style', '--percent:"Subiendo...";');

                                }*/

                            }

                        }, false);
                    }
                    return fileXhr;
                }
            });


        };

        input.setAttribute('disabled', true);

	    reader.readAsDataURL(input.files[0]);

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

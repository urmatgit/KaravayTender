// upload

var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
var maxFilesSize = 52428800; // 50MB -> 50 * 1024 * 1024
var uploadfilename = document.querySelector("#uploadfilename");
var uploadbutton = document.querySelector("button[name='uploadbutton']");
var uploadfileinputs = document.getElementById("Files");
var uploadfileinput = document.getElementById("file");
uploadbutton.addEventListener('click', (e) => {
    uploadfileinput.click();
})
function clienUploadfilename(withtablse=true) {
    uploadfilename.innerHTML = "";
    uploadFiles = {};
    uploadfileinputs.files = null;
    if (withtablse) {
        var files_table = document.querySelector("#files_panel");

        files_table.innerHTML = "";
    }
    $("#Files").rules("add", "required");
}
var getFileName = (disposition) => {
    const utf8FilenameRegex = /filename\*=UTF-8''([\w%\-\.]+)(?:; ?|$)/;
    const asciiFilenameRegex = /filename=(["']?)(.*?[^\\])\1(?:; ?|$)/;
    let fileName = '';
    if (utf8FilenameRegex.test(disposition)) {
        fileName = decodeURIComponent(utf8FilenameRegex.exec(disposition)[1]);
    } else {
        const matches = asciiFilenameRegex.exec(disposition);
        if (matches != null && matches[2]) {
            fileName = matches[2];
        }
    }
    return fileName;
}
var onDownloadFile = (index) => {

    const url = index;
    const filename = getFileName(index);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();
    toastr["info"](`${window.translations.Download_Success}`);
}

var onRemoveFile = (index, file) => {
    bootbox.confirm({
        message: `${window.translations.DeleteFileDialog}`,  //"@_localizer["Are you sure delete a row?"]",
        buttons: {
            confirm: {
                label: `${window.translations.Yes}`, //'@_localizer["Yes"]',
                className: 'btn-success'
            },
            cancel: {
                label: `${window.translations.No}`, //'@_localizer["No"]',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                
                axios.get(pagelink + '?handler=DeleteFile&id=' + currentEditRow.Id + '&name=' + file)
                    .then(res => {
                        if (typeof currentEditRow != 'undefined') {
                            getFiles(pagelink + '?handler=FilesList&id=' + currentEditRow.Id);

                        } else {
                            $(`#files_panel tr[id=${index - 1}]`).remove();
                        }
                        toastr["info"](`${window.translations.DeleteSuccess}`);// '@_localizer["Delete Success"]');
                    })
                    .catch((error) => {
                        if (error.response.data.Errors) {
                            const errors = error.response.data.Errors;
                            errors.forEach(item => {
                                toastr["error"](item);
                            });
                        } else {
                            toastr["error"](`${window.translations.DeleteFail},${error.response.data}`);
                        }
                    });
            }
        }
    })

    
    
}


function getFiles(path) {
    //axios.get('/Contragents/Index?handler=FilesList&id=' + id)
    axios.get(path)
        .then(res => {
            console.log(res);
            //ContragentOnRegistrationCount

            var files_table = document.querySelector("#files_panel");
            files_table.innerHTML = "";
            let index = 0;
            res.data.forEach(file => {
                let row = files_table.insertRow();
                row.id = index;
                let rowvalue = row.insertCell(0);
                rowvalue.innerHTML = ++index;
                let colFile = row.insertCell(1);
                colFile.innerHTML = file.replace(/^.*[\\\/]/, '');
                let colAction = row.insertCell(2);
                colAction.innerHTML = `<button type="button" name="download_files" title="Загрузить файл" class="btn btn-sm btn-outline-primary" onclick="onDownloadFile('${file.replaceAll('\\', '/')}')" >
                            <i class="${window.translations.IconPrefix} fa-download" ></i>
							</button> ` ;
                if (typeof _canDeleteFile != 'undefined' && _canDeleteFile) {
                
                    colAction.innerHTML+=`
                    <button type="button" name="remove_files" title="Удалить файл" class="btn btn-sm btn-outline-primary" onclick="onRemoveFile(${index}, '${colFile.innerHTML}')" >
                        <i class="${window.translations.IconPrefix} fa-trash-alt" ></i>
                    </button>`;
                }
               
            });
            if (res.data && res.data.length>0)
            //    $('#Files').removeAttr('required');
                $("#Files").rules("remove", "required");
        })
        .catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`Получение количество контрагентов (OnRegistraion),${error.response.data}`);
            }
        });
}

var uploadFiles = {};
uploadfileinput.onchange = (e) => {
    //const files = uploadfileinput.taget.files;
    console.log(e);

    console.log(e.target.files);

    Array.from(e.target.files).forEach(file => {
        console.log(file.name);
        let isBigFile = "";
        let UnderLineText = "";
        if (IsBigFile(file)) {
            isBigFile = window.translations.FileSizeToBig;
            UnderLineText = 'style="text-decoration: line-through"';
        }
        if (file.name in uploadFiles) {

        }
        else {
            fileName = file.name;//uploadfileinput.value.split('\\').pop();
            uploadfilename.innerHTML += `
              <div class="alert ${isBigFile == "" ? "alert-info" : "alert-danger"}   alert-dismissible fadeup show mb-2 mt-2" role="alert" id="${fileName}">
                     <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                         <span aria-hidden="true"><i class="${window.translations.IconPrefix} fa-times"></i></span>
                      </button>
                      <div class="d-flex align-items-center">
							<div class="alert-icon">
									<i class="${window.translations.IconPrefix} fa-upload fs-xl"></i>
							</div>
                            
							<div class="flex-1">
	    						 <span ${UnderLineText}>${fileName}  (${(file.size / 1024 / 1024).toFixed(3)} Mb)</span> ${isBigFile}
							</div>
					</div>
             </div>
        `
        }
        if (isBigFile === "")
            uploadFiles[file.name] = file;

    });
    UpdateFiles();
    $('#uploadfilename .alert').on('closed.bs.alert', (e) => {
        console.log(e);
        delete uploadFiles[e.currentTarget.id];
        UpdateFiles();
        uploadfileinput.value = null;

    })
};
function IsBigFile(file) {

    if (file.size > maxFileSize) {
        return true;
    }
    return false;
}
UpdateFiles = function () {
    const dT = new DataTransfer();

    $.map(uploadFiles, function (value, key) {
        dT.items.add(value);
    });
    uploadfileinputs.files = dT.files;
    console.log(uploadfileinputs.files);
}
function AppendFilesToFormData(rormData) {
    let uploadfileinputs = document.getElementById("Files");
    if (uploadfileinputs) {
        for (var i = 0; i != uploadfileinputs.files.length; i++) {
            rormData.append("files[]", uploadfileinputs.files[i]);
        }
    }
}

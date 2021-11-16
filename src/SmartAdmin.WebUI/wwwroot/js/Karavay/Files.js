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
function clienUploadfilename() {
    uploadfilename.innerHTML = "";
    uploadFiles = {};
    uploadfileinputs.files = null;
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
                let rowvalue = row.insertCell(0);
                rowvalue.innerHTML = ++index;
                let colFile = row.insertCell(1);
                colFile.innerHTML = file.replace(/^.*[\\\/]/, '');
                let colAction = row.insertCell(2);
                colAction.innerHTML = `<button type="button" name="download_files" class="btn btn-sm btn-outline-primary" onclick="onDownloadFile('${file.replaceAll('\\', '/')}')" >
                            <i class="${window.translations.IconPrefix} fa-download" ></i>
							</button>`;
            });

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

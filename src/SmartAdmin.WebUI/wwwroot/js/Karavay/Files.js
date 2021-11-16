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

var onDownloadFile = (index) => {

    const url = index;
    const filename = getFileName(index);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();
    toastr["info"](`${translations.Download_Success}`);
}


function getConragentFiles(path) {
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

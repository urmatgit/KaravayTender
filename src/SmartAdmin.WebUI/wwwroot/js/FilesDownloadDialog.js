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
function onDownloadFileSpec(value) {
    const url = value;
    const filename = getFileName(value);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();
    toastr["info"](`Файл загружен`);
}
function onDownloadQualityDoc(value) {
    var data = {
        name: value

    }
    var headers = {
        "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
    }
    axios.post('/QualityDocs/Index?handler=Download',
        data,
        {
            headers: headers

        }
    )
        .then(response => {
            if (response.data)
                onDownloadFileSpec(response.data);
            else
                toastr["error"]('Файл не найден!');
        })
}
function ShowFileDownloadrDialog(files, id, onOk) {
     
    let rows = "";
    let filesArr = files.split(',')
    if (id) {
        $.each(filesArr, function (index, value) {
            rows += `<tr><td><button type="button" name="download_files" title="Загрузить файл" class="btn btn-sm btn-outline-primary" onclick="onDownloadFileSpec('Files/Specifications/${id}/${value.replaceAll('\\', '/')}')" >
                            <i class="${window.translations.IconPrefix} fa-download" ></i>
							</button> </td>
                    <td>  ${value}  </td>
                 </tr>`;
        });
    } else {
        $.each(filesArr, function (index, value) {
            rows += `<tr><td><button type="button" name="download_files" title="Загрузить файл" class="btn btn-sm btn-outline-primary" onclick="onDownloadQualityDoc('${value}')" >
                            <i class="${window.translations.IconPrefix} fa-download" ></i>
							</button> </td>
                    <td>  ${value}  </td>
                 </tr>`;
        });
    }

    bootbox.dialog({
        title: "Скачать файлы",
        //size: 'large',
        message: `<div class="row"> 
           <div id="panel-1" class="card  border" style="width: -webkit-fill-available;">
    <div class="card-header py-2">
							<div class="card-title">
								Файлы
							</div>
						</div>
    
        <div class="card-body" >

            <table class="table table-sm m-0" >
                <thead class="bg-primary-500">
                    <tr>
                        <th>#</th>
                        <th> Файл</th>
                        <th> </th>

                    </tr>
                </thead>
                <tbody >
                     ${rows}
                </tbody>
            </table>

        </div>
    
</div>
             </div>`,
        buttons: {
            
            success: {
                label: '<i class="fa fa-check"></i> Закрыть',
                className: "btn-success",
                callback: function () {
                    return true;

                     
                    
                    if (onOk)
                        onOk(selectedParticipant, DateRequest, message)
                    //var name = $('#name').val();
                    //var answer = $("input[name='awesomeness']:checked").val()
                    //console.log("Hello " + name + ". You've chosen " + answer + "")
                }
            }
        }
    }
    );
};


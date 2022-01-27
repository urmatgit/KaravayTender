function ShowFileDownloadrDialog(files,id, onOk) {
     
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


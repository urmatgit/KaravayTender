
// translations
// pagelink
//_canEdit
//_catDelete
$('#searchbutton').click(function () {
    reloadData();
});
$('#addbutton').click(function () {
    popupmodal(null);
});
$('#deletebutton').click(function () {
    onDeleteChecked();
});
$('#exportbutton').click(function () {
    onExport();
});
$('#importbutton').click(function () {
    showImportModal();
});
$('#gettemplatebutton').click(function () {
    onGetTemplate();
});
$('#edit_form :submit').click(function (e) {
    const form = document.querySelector('#edit_form');
    if ($(form).valid() === false) {
        form.classList.add('was-validated');
    } else {
        let request = $('#edit_form').serialize();
        if (typeof UpdateFiles == 'function')
            UpdateFiles();
        var form_data = new FormData($('#edit_form')[0]);
        //if (AppendFilesToFormData) {
        //    AppendFilesToFormData(request);
        //}
        axios.post(`${pagelink}`, form_data).then(res => {
            toastr["info"](`${translations.SaveSuccess} `);
            $('#edit_modal').modal('toggle');
            reloadData();
        }).catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${translations.SaveFail},${error.response.data}`);
            }
        });
    }
    event.preventDefault();
    event.stopPropagation();
})
tblColumns = [];
function createColumns() {
    var InitColumns=
     [
        { field: 'ck', checkbox: true },
        {
            field: '_action',
            title: `${window.translations.Command} `,
            width: 100,
            align: 'center',
            formatter: function (value, row, index) {
                return `<div class="btn-group" role="group">
								  <button id="commandbtngroup" type="button" ${(_canEdit ? "" : "disabled")}  class="btn btn-outline-primary btn-sm dropdown-toggle waves-effect waves-themed" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									<i class="${window.translations.IconPrefix} fa-edit"></i>
								 </button>
								 <div class="dropdown-menu dropdown-menu-animated" aria-labelledby="commandbtngroup">
								   <button class="dropdown-item" onclick="onEdit(${index})" ${(_canEdit ? "" : "disabled")}><i class="fal fa-edit mr-1"></i> ${translations.Edit}</button>
								   <button class="dropdown-item" onclick="onDelete('${row.Id}')" ${(_canDelete ? "" : "disabled")} ><i class="fal fa-trash-alt mr-1"></i> ${translations.Delete}</button>
								 </div>
							  </div>`;
            }
        }
         



        ];
    if (tblColumns.length > 0) {
        return InitColumns.concat(tblColumns);
    }
    return InitColumns;
};
var $dg = {};
var initdatagrid = () => {
    $dg = $('#main_dg').datagrid({
        height: (window.innerHeight - 320),
        method: 'GET',
        rownumbers: false,
        singleSelect: true,
        selectOnCheck: false,
        checkOnSelect: false,
        pagination: true,
        clientPaging: false,
        remoteFilter: true,
        sortName: 'Id',
        sortOrder: 'desc',
        pageSize: 15,
        pageList: [10, 15, 30, 50, 100, 1000],
        onBeforeLoad: function () {
            $('#deletebutton').prop('disabled', true);
        },
        onCheckAll: function (rows) {
            const checked = $(this).datagrid('getChecked').length > 0;
            $('#deletebutton').prop('disabled', !checked);
        },
        onUncheckAll: function () {
            $('#deletebutton').prop('disabled', true);
        },
        onCheck: function () {
            $('#deletebutton').prop('disabled', false);
        },
        onUncheck: function () {
            const checked = $(this).datagrid('getChecked').length > 0;
            $('#deletebutton').prop('disabled', !checked);
        },
        columns: [createColumns()]
    })
        .datagrid('enableFilter', {})
        .datagrid('load', `${pagelink}?handler=Data`);

	}

	var reloadData = () => {
        $dg.datagrid('load', `${pagelink}?handler=Data`);
}

//$(() => {
//    initdatagrid();
//})
var popupmodal = (nomenclature) => {
    $('#edit_modal').modal('toggle');
    $('#edit_modal .modal-title').html(`${translations.AddCaption}`);
    $('#edit_form').clearForm();
    $('#edit_form')[0].reset();
    if (typeof clienUploadfilename == 'function')
        clienUploadfilename();
    if (nomenclature) {
        $('#edit_modal .modal-title').html(`${translations.EditCaption}`);
        if (typeof jsonToFormCallBack !== 'undefined')
            $('#edit_form').jsonToForm(nomenclature, jsonToFormCallBack);
        else
            $('#edit_form').jsonToForm(nomenclature);
    } else {
        $('#edit_form #Input_Id').val(0)

    }
}

var onEdit = (index) => {
    var nomenclature = $dg.datagrid('getRows')[index];
    if (typeof getFiles == 'function')
        getFiles(pagelink + '?handler=FilesList&id=' + nomenclature.Id);
    popupmodal(nomenclature);
}
var onDelete = (id) => {
    bootbox.confirm({
        message: `${translations.DeleteRowDialog}`,  //"@_localizer["Are you sure delete a row?"]",
        buttons: {
            confirm: {
                label: `${ translations.Yes }`, //'@_localizer["Yes"]',
                className: 'btn-success'
            },
            cancel: {
                label: `${translations.No}`, //'@_localizer["No"]',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                axios.get(`${pagelink}?handler=Delete&id=` + id).then(res => {
                    toastr["info"](`${translations.DeleteSuccess}`);// '@_localizer["Delete Success"]');
                    reloadData();
                })
                    .catch((error) => {
                        if (error.response.data.Errors) {
                            const errors = error.response.data.Errors;
                            errors.forEach(item => {
                                toastr["error"](item);
                            });
                        } else {
                            toastr["error"](`${translations.DeleteFail},${error.response.data}`);
                        }
                    });
            }
        }
    })
}
var onDeleteChecked = () => {
    var checkedId = $dg.datagrid('getChecked').map(x => x.Id);
    if (checkedId.length > 0) {
        bootbox.confirm({
            message: `${translations.DeleteRowsDialog}`,// "@_localizer["Are you sure delete checked rows?"]",
            buttons: {
                confirm: {
                    label: `${translations.Yes}`,
                    className: 'btn-success'
                },
                cancel: {
                    label: `${translations.No}`,
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    var paras = new URLSearchParams(checkedId.map(s => ['id', s]));
                    console.log(paras.toString())
                    axios.get(`${pagelink}?handler=DeleteChecked&` + paras.toString()).then(res => {
                        toastr["info"](`${translations.Delete} ${checkedId.length} ${translations.Success}"]`);
                        reloadData();
                    })
                        .catch((error) => {
                            if (error.response.data.Errors) {
                                const errors = error.response.data.Errors;
                                errors.forEach(item => {
                                    toastr["error"](item);
                                });
                            } else {
                                toastr["error"](`${translations.DeleteFail},${error.response.data}`);
                            }
                        });
                }
            }
        });

    }
}
var onExport = () => {
    var options = $dg.datagrid('options');
    var data = {
        filterRules: JSON.stringify(options.filterRules),
        sort: options.sortName,
        order: options.sortOrder,
    }
    console.log(options, data)
    var headers = {
        "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
    }
    axios.post(`${pagelink}?handler=Export`,
        data,
        {
            headers: headers,
            responseType: "blob"
        })
        .then(response => {
            const filename = getFileName(response.headers['content-disposition']);
            const url = window.URL.createObjectURL(new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8' }));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
        }).catch(error => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${translations.ExportFail},${error.response.data}`);
            }
        })


}



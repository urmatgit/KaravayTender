
// translations
// pagelink
//_canEdit
//_catDelete

$('#searchbutton').click(function () {
    reloadData();
});
$('#addbutton').click(function () {
    openEditpanel(null);
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
$('#startStage').click(function (e) {
    //
    bootbox.prompt({
        //title: `На портале ОАО "КАРАВАЙ" по работе с поставщиками появилась возможность \n предоставить ценовое предложение по лоту № ${currentEditRow.Number}.
        //          Просим предоставить Ваши предложения пройдя по ссылке ___` ,
        title: 'Срок предоставления до',
        inputType: 'date',
        buttons: {
            confirm: {
                label: `${translations.Ok}`, //'@_localizer["Yes"]',
                className: 'btn-success'
            },
            cancel: {
                label: `${translations.Cancel}`, //'@_localizer["No"]',
                className: 'btn-danger'
            }
        },
        callback: function (result) {

            console.log(result);

            SubmitForm("?handler=Run&deadline=" + result, function (res) {
                SetReadOnlyForm();
                LoadComState(currentEditRow.Id);
            });
            
        }
    
    });

    

});

function SetReadOnlyForm() {
    const form = document.querySelector('#edit_form_panel');
    for (var i = 0, fLen = form.length; i < fLen; i++) {
        form.elements[i].readOnly = true;
    }
    $('.custom-select').prop('disabled', true);
    //$('.custom-control-input[type=checkbox]').prop('disabled', true);
    SetEnableToRoleButton(false);
    
    
    $('#save').prop('disabled', true);
    $('#startStage').prop('disabled', true);
}
function SubmitForm(addParam,callback) {
    const form = document.querySelector('#edit_form_panel');
    if ($(form).valid() === false) {
        form.classList.add('was-validated');
    } else {
        let request = $('#edit_form_panel').serialize();
        if (typeof UpdateFiles == 'function')
            UpdateFiles();
        var form_data = new FormData($('#edit_form_panel')[0]);
        //if (AppendFilesToFormData) {
        //    AppendFilesToFormData(request);
        //}
        axios.post(`${pagelink}${addParam ? addParam: ''}`, request).then(res => {
            toastr["info"](`${translations.SaveSuccess} `);

            //$('#table-page-content').show();
            //$('#edit_panel').hide();
            reloadData();
            if (callback) {
                callback(res.data);
            }
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
}
$('#save').click(function (e) {
    SubmitForm("", function (res) {
        SetEnableToRoleButton(true);
        openEditpanel(res.Data);
    });
});
function SetEnableToRoleButton(enable) {
    $(".editable[role='button']").each(function (val) {
        if (!enable)
            $(this).hide();// prop('disabled', true).addClass('ui-disabled');
        else
            $(this).show();// prop('disabled', false).removeClass('ui-disabled');
    })
}
function openEditpanel(row) {

    currentEditRow = row;
    $('#table-page-content').hide();
    $('#edit_panel').show();
    $("#edit_panel").trigger("ShowEdit");
    $('#edit_panel .panel-title').html(`${window.translations.AddCaption}`);
    
    $('#edit_form_panel').clearForm();
    $('#edit_form_panel')[0].reset();
    
    if (row) {
        $('#edit_panel .panel-title').html(`${window.translations.EditCaption}`);
        
            $('#edit_form_panel').jsonToForm(row, {

                IsDeliveryInPrice: function (value) {
                    if (value == true) {
                        $('#edit_form_panel [name*="IsDeliveryInPrice"]').prop('checked', true);
                    } else {
                        $('#edit_form_panel [name*="IsDeliveryInPrice"]').prop('checked', false);
                    }
                },
                IsBankDays: function (value) {
                    if (value == true) {
                        $('#edit_form_panel [name*="IsBankDays"]').prop('checked', true);
                    } else {
                        $('#edit_form_panel [name*="IsBankDays"]').prop('checked', false);
                    }
                },
                TermBegin: function (value) {
                    var dateFormat = "YYYY-MM-DD";
                    
                    var date = moment(value).format(dateFormat);
                    $('#edit_form_panel [name*="TermBegin"]').val(date);
                },
                TermEnd: function (value) {
                    var dateFormat = "YYYY-MM-DD";

                    var date = moment(value).format(dateFormat);
                    $('#Input_TermEnd').val(date);
                }
                //Status: function (value) {
                //    if (value != 0) {
                        
                //    } else {
                //        SetEnableToRoleButton(true);
                //    }
                //}

            });

        if (row.Status != 0) {
            SetReadOnlyForm();
        }
     
    }
    else {
        $('#edit_form_panel #Input_Id').val(0)
        
        if (typeof OnNewRow == 'function')
            OnNewRow();
        SetEnableToRoleButton(false);
    }
}
tblFilters = [{}];

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
var checkRowEvent = new CustomEvent("rowCheck", {
    detail: {
        check: true
    },
    bubbles: true,
    cancelable: false
});


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
            checkRowEvent.detail.check = true;
            this.dispatchEvent(checkRowEvent);
        },
        onCheckAll: function (rows) {
            const checked = $(this).datagrid('getChecked').length > 0;
            $('#deletebutton').prop('disabled', !checked);
            checkRowEvent.detail.check = !checked;
            this.dispatchEvent(checkRowEvent);
           
        },
        onUncheckAll: function () {
            $('#deletebutton').prop('disabled', true);
            checkRowEvent.detail.check = true;
            this.dispatchEvent(checkRowEvent);
        },
        onCheck: function () {
            $('#deletebutton').prop('disabled', false);
            checkRowEvent.detail.check = false;
            this.dispatchEvent(checkRowEvent);
            
        },
        onUncheck: function () {
            const checked = $(this).datagrid('getChecked').length > 0;
            $('#deletebutton').prop('disabled', !checked);
            checkRowEvent.detail.check = !checked;
            this.dispatchEvent(checkRowEvent);
        },
        columns: [createColumns()]
    })
        .datagrid('enableFilter', this.tblFilters)
        .datagrid('load', `${pagelink}?handler=Data`);

	}

	var reloadData = () => {
        $dg.datagrid('load', `${pagelink}?handler=Data`);
}

//$(() => {
//    initdatagrid();
//})
$('#edit_modal').on('shown.bs.modal', function () {
    //var IsAddClick = $('#edit_form #Input_Id').val();
    //if (!IsAddClick || IsAddClick == 0) return;
    //if (typeof clienUploadfilename == 'function')
    //    clienUploadfilename();
    

})
var currentEditRow = null;
var popupmodal = (nomenclature) => {
    $('#edit_modal').modal('toggle');
    $('#edit_modal .modal-title').html(`${translations.AddCaption}`);
    $('#edit_form').clearForm();
    $('#edit_form')[0].reset();
    currentEditRow = nomenclature;
    if (nomenclature) {
        $('#edit_modal .modal-title').html(`${translations.EditCaption}`);
        if (typeof jsonToFormCallBack !== 'undefined')
            $('#edit_form').jsonToForm(nomenclature, jsonToFormCallBack);
        else
            $('#edit_form').jsonToForm(nomenclature);
    } else {
        $('#edit_form #Input_Id').val(0)
        if (typeof clienUploadfilename == 'function')
            clienUploadfilename();
        if (typeof OnNewRow == 'function')
            OnNewRow();
    }
}

var onEdit = (index) => {
    var nomenclature = $dg.datagrid('getRows')[index];
    openEditpanel(nomenclature);
    //if (typeof getFiles == 'function')
    //    getFiles(pagelink + '?handler=FilesList&id=' + nomenclature.Id);
    //popupmodal(nomenclature);
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

$(document).ready(function () {

   // $("#edit_form").data("validator").settings.ignore = "";


});



// translations
// pagelink
//_canEdit
//_catDelete

$('#comstage_updatebutton').click(function () {
    LoadComState(currentEditRow.Id);
});
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
$('#btnCancelStage').click(function (e) {
    bootbox.confirm({
        title: "Вы уверены, отменить проведения КП?",
        message: "Всем участникам с статусом 'Ожидание' и 'Подтверждение' будут присвоена статус 'Отмена'",
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Отмена',
                className: 'btn-danger'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Подтвердить',
                className: 'btn-success'
            }
        },
        callback: function (result) {
            if (result) {
                CancelComOffer();
            }

        }
    });
})
function CancelComOffer() {
    SubmitForm("?handler=CancelComOffer", function (res) {
        
        openEditpanel(res.Data, true);

        //LoadComState(currentEditRow.Id);
    });
}
$('#btnSelectWinnerStage').click(function (e) {
    let particicpants = clsparticipant.dg.datagrid('getRows');
    console.log(particicpants);
    ShowSelectWinnerDialog(particicpants, function (id, date, message) {
        
        var headers = {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        }

        axios.post(`${pagelink}?handler=SelectWinner`, { "ComOfferId": currentEditRow.Id, DeadlineDate: date, ContragentId: id, Message: message }, {
            headers: headers
        }).then(res => {
            toastr["info"](`${translations.SaveSuccess} `);
            openEditpanel(res.data.Data, true);
            reloadData();
        })
    });

    
});

$('#btnSendStage').click(function (e) {
    let stageid = $('#StageId').val();
    if (!stageid || stageid == "0") {
        bootbox.alert("Отправить запрос, можно только на 'Последнее предложение'");
        return;
    }
    var rows = dgcomstage.datagrid('getRows');
    console.log(rows);
    
    let ContrPrice = new Array();
    $('input.editable:checkbox').each(function () {
        let id = $(this).attr('id').split("_");
        const value = $(this).prop("checked");
        tmpCP = {
            "ContrId": parseInt(id[1]),
              "ComPositionId": parseInt(id[0]),
              "RequestPrice": value
            }
        console.log($(this).attr('id'));
        ContrPrice.push(tmpCP);
        //var sThisVal = $(this).val();
    });


    DeadLinePrompt(function (result) {


        let tmpOjb = {
            "ComOfferId": currentEditRow.Id, 
            "StageId": parseInt(stageid),
            "ContrPrices": ContrPrice,

        }
        console.log(tmpOjb);


        var headers = {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        }

        axios.post(`${pagelink}?handler=SendPrice`, { stageComRequest: tmpOjb, Deadline: result }, {
            headers: headers
        }).then(res => {
            toastr["info"](`${translations.SaveSuccess} `);
            openEditpanel(res.data.Data, true);
            reloadData();
        })
        //    SetReadOnlyForm();
        //    openEditpanel(currentEditRow, true);
        //    //LoadComState(currentEditRow.Id);
    });
});

$('#btnEndStage').click(function (e) {
    let stageid = $('#StageId').val();
    if (!stageid || stageid == "0") {
        bootbox.alert("завершить этап, можно только на 'Последней предложение'");
        return;
    }

    
        bootbox.confirm({
            title: "Вы уверенны завершить этап?",
            message: (checkEmptyPrices() ?  "Имеются не заполненные цены, поставщики с не заполненными ценами будут исключены из торги":" "),
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Отмена',
                    className: 'btn-danger'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Подтвердить',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {
                    endStage();
                }

            }
        });
    
    //check empty prices
   

})
function endStage() {

    let stageid = $('#StageId').val();
    SubmitForm("?handler=EndStage&stageid=" + stageid, function (res) {
        SetReadOnlyForm();
        openEditpanel(res.Data, true);

        //LoadComState(currentEditRow.Id);
    });
}
function checkEmptyPrices() {
    
    let hasEmpty = false;
    console.log($('span.stageprice').length);
    $('span.stageprice').each(function () {
        console.log($(this).html());
        if (!$.trim($(this).html()))
            hasEmpty = true;
        //var sThisVal = $(this).val();
    });
    return hasEmpty;



}
$('#btnChangeDeadline').click(function (e) {
    //
    bootbox.prompt({
        //title: `На портале ОАО "КАРАВАЙ" по работе с поставщиками появилась возможность \n предоставить ценовое предложение по лоту № ${currentEditRow.Number}.
        //          Просим предоставить Ваши предложения пройдя по ссылке ___` ,
        title: 'Изменить срок ответа до',
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

            if (result) {

                let stageid = $('#StageId').val();
                SubmitForm("?handler=Deadline&deadline=" + result + "&stageid=" + stageid, function (res) {
                    SetReadOnlyForm();
                    openEditpanel(currentEditRow, true);
                    //LoadComState(currentEditRow.Id);
                } );
            }

        }

    });



});

function DeadLinePrompt(callback) {
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

            if (callback && result)
                callback(result);

        }

    });
}
$('#btnStartStage').click(function (e) {

    const form = document.querySelector('#edit_form_panel');
    if ($(form).valid() === false) {

        form.classList.add('was-validated');

    } else {
        DeadLinePrompt(function (result) {
            if (result) {
                console.log(result);
                var dialogWait = bootbox.dialog({
                    message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> Пожалуйста подождите, запускаем первый этап...</p>',
                    centerVertical: true,
                    closeButton: true
                });
                dialogWait.init(function () {
                    setTimeout(function () {
                        dialogWait.modal('hide');
                    }, 2000);
                });
                SubmitForm("?handler=Run&deadline=" + result, function (res) {

                    dialogWait.modal('hide');
                    SetReadOnlyForm();
                    openEditpanel(res.Data, true);
                    //LoadComState(currentEditRow.Id);
                }, function (error) {
                    dialogWait.modal('hide');
                });
                dialogWait.modal('hide');
            }
        });
    }
    ////
    //bootbox.prompt({
    //    //title: `На портале ОАО "КАРАВАЙ" по работе с поставщиками появилась возможность \n предоставить ценовое предложение по лоту № ${currentEditRow.Number}.
    //    //          Просим предоставить Ваши предложения пройдя по ссылке ___` ,
    //    title: 'Срок предоставления до',
    //    inputType: 'date',
    //    buttons: {
    //        confirm: {
    //            label: `${translations.Ok}`, //'@_localizer["Yes"]',
    //            className: 'btn-success'
    //        },
    //        cancel: {
    //            label: `${translations.Cancel}`, //'@_localizer["No"]',
    //            className: 'btn-danger'
    //        }
    //    },
    //    callback: function (result) {

    //        console.log(result);
    //        var dialog = bootbox.dialog({
    //            message: '<p class="text-center mb-0"><i class="fa fa-spin fa-cog"></i> Пожалуйста подождите, запускаем первый этап...</p>',
    //            centerVertical: true,
    //            closeButton: false
    //        });

    //        SubmitForm("?handler=Run&deadline=" + result, function (res) {

    //            dialog.modal('hide');
    //            SetReadOnlyForm();
    //            openEditpanel(res.Data,true);
    //            //LoadComState(currentEditRow.Id);
    //        }, function (error) {
    //            dialog.modal('hide');
    //        });
            
    //    }
    
    //});

    

});

function SetReadOnlyForm() {
    const form = document.querySelector('#edit_form_panel');
    for (var i = 0, fLen = form.length; i < fLen; i++) {
        form.elements[i].readOnly = true;
    }
    $('#edit_form_panel .custom-select').prop('disabled', true);
    $('.custom-control-input[type=checkbox]').prop('disabled', true);
    SetEnableToRoleButton(false);
    
    
    //$('#save').prop('disabled', true);
    //$('#btnStartStage').prop('disabled', true);
}
function SubmitForm(addParam,callback,onerror) {
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
        axios.post(`${pagelink}${addParam ? addParam : ''}`, request).then(res => {
            toastr["info"](`${translations.SaveSuccess} `);

            //$('#table-page-content').show();
            //$('#edit_panel').hide();
            reloadData();
            if (callback) {
                callback(res.data);
            }
        }).catch((error) => {
            if (onerror)
                onerror(error)
        });
    }
    event.preventDefault();
    event.stopPropagation();
}
$('#save').click(function (e) {
    SubmitForm("", function (res) {
        
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
function openEditpanel(row,stage) {

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

        if (row.Status > 0) {
            
            $('#ComStageTab').show();
            $('#ComStage').show();
            SetReadOnlyForm();
            //SetEnableToRoleButton(true);
            $('a[href="#ComStage"]').click();

            
        } else {
            SetEditable();
            SetEnableToRoleButton(true);
            
            $('#ComStage').hide();
            $('#ComStageTab').hide();
            $('a[href="#ComPosition"]').click();
        }
        showHideButtons(row.Status);
     
    }
    else {
        //ADd
        $('#edit_form_panel #Input_Id').val(0)
        axios.get(`${pagelink}?handler=NextNumber`).then(res => {
            
            $('#edit_form_panel #Input_Number').val(res.data);
        })
        //let total = $dg.datagrid('getData').total;
        //$('#edit_form_panel #Input_Number').val(total+1)
        
        SetEditable();
        if (typeof OnNewRow == 'function')
            OnNewRow();
        SetEnableToRoleButton(false);
        $('#ComStage').hide();
        $('#ComStageTab').hide();
        $('a[href="#ComPosition"]').click();
        showHideButtons(-1);
    }
    

}
function showHideButtons(Status) {
    $('#save').hide();
    $('#btnStartStage').hide();
    $('#btnSendStage').hide();
    $('#btnEndStage').hide();
    $('#btnChangeDeadline').hide();
    $('#btnCancelStage').hide();
    $('#btnSelectWinnerStage').hide();
    $('#btnReturnToEvaluation').hide();
    
    switch (Status) {
        case -1:
            $('#save').show();
            break;
        case 0://Подготовка
            $('#save').show();
            $('#btnStartStage').show();
            
            
            break;
        case 1: //Ожидание КП
            if (currentStage === undefined) 
                currentStage = false;
            
            let now = new Date();
            //let deadlineDate = new Date(currentStage.DeadlineDate);
            //moment(value) > moment().utc()
            if (CheckPriceConfirmed() || (currentStage && moment(currentStage.DeadlineDate) <= moment().utc()))
            //if (CheckPriceConfirmed() || (currentStage && Date.parse(moment(now).format("DD.MM.YYYY")) > Date.parse(moment(new Date(currentStage.DeadlineDate)).format("DD.MM.YYYY"))))
                $('#btnEndStage').show();
            $('#btnChangeDeadline').show();
            break;
        case 2:   //Оценка КП
            //$('input.editable:checkbox').removeAttr('disabled');
            let StageType = $('input[name="GetStageType"]:checked').val();

            $('input.editable:checkbox').each(function () {

                let id = $(this)[0].id;
                if (StageType == 1) {
                    $(`#${id}`).removeAttr("disabled");
                }
                else
                    $(`#${id}`).attr("disabled");
                //$(this).prop('disabled', false);
                console.log($(`#${id}`));
                //var sThisVal = $(this).val();
            });

            $('#btnSendStage').show();
            $('#btnCancelStage').show();
            $('#btnSelectWinnerStage').show();
            
            

            break;
        case 3: //"Определение победителя"
            let now1 = new Date();
            if (currentStage === undefined)
            currentStage = false;
            if (currentStage && Date.parse(now1) > Date.parse(currentStage.DeadlineDate))
                $('#btnReturnToEvaluation').show();
            break;



    }
}
function SetEditable() {
    const form = document.querySelector('#edit_form_panel');
    for (var i = 0, fLen = form.length; i < fLen; i++) {
        if (!form.elements[i].classList.contains("readonly"))
            form.elements[i].readOnly = false;
    }
    $('.custom-select').prop('disabled', false);
    $('.custom-control-input[type=checkbox]').prop('disabled', false);
}
tblFilters = [{}];

tblColumns = [];
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
        height: (window.innerHeight - 250),
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
        onClickCell: function (index, field, value) {
            
                console.log(value);
                onEdit(index);
            
        },
        
        columns: [createColumnsComOffer()]
        ,
        rowStyler: function (index, row) {
            let style = 'cursor: pointer;';
            if (row["Status"] == 4 && row["WinnerName"]) {//"Победитель определён"
                style += 'background-color:lightgreen;color:blue;';
            }
             
            return style;
        }
    
    })
    
        .datagrid('enableFilter', this.tblFilters)
        .datagrid('load', `${pagelink}?handler=Data`);

	}

function reloadData()  {
       let comOfferFilterFor = $("#filterForComOffer").val();
    $dg.datagrid('load', `${pagelink}?handler=Data&comOfferFilterFor=` + comOfferFilterFor);
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
var onCopy = (index) => {
    var nomenclature = $dg.datagrid('getRows')[index];
    
    axios.post(`${pagelink}?handler=Copy&id=` + nomenclature.Id).then(res => {
        toastr["info"](`${translations.SaveSuccess} `);

        //$('#table-page-content').show();
        //$('#edit_panel').hide();
        reloadData();
        openEditpanel(res.data.Data);
    })
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


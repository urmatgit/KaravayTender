
// translations
// this._pageLink
//_canEdit
//_catDelete
class TableComParticipant extends clsBaseTable {


    CreateActionColumn(value, row, index) {
        return `<div class="btn-group" role="group">
								                  <button id="commandbtngroup1" type="button" ${(_canEdit ? "" : "disabled")}  class="btn btn-outline-primary btn-sm dropdown-toggle waves-effect waves-themed" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
								        	        <i class="${window.translations.IconPrefix} fa-edit"></i>
								               </button>
								 <div class="dropdown-menu dropdown-menu-animated" aria-labelledby="commandbtngroup1">
								   
								   <a role="button" class="dropdown-item editable" onclick="clsparticipant.onDelete('${row.ContragentId}','${row.ComOfferId}')" ${(_canDelete ? "" : "disabled")} ><i class="fal fa-trash-alt mr-1"></i> ${translations.Delete}</a>
								 </div>
							  </div>`}





    onDelete(contrgentId, comofferid) {
        var self = this;
        bootbox.confirm({
            message: `${translations.DeleteRowDialog}`,  //"@_localizer["Are you sure delete a row?"]",
            buttons: {
                confirm: {
                    label: `${translations.Yes}`, //'@_localizer["Yes"]',
                    className: 'btn-success'
                },
                cancel: {
                    label: `${translations.No}`, //'@_localizer["No"]',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    axios.get(`${self._pageLink}?handler=Delete&contragentId=` + contrgentId + "&comofferid=" + comofferid).then(res => {
                        toastr["info"](`${translations.DeleteSuccess}`);// '@_localizer["Delete Success"]');
                        self.reloadData();
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
    onDeleteChecked = () => {
        var self = this;
        var checkedId = this.dg.datagrid('getChecked').map(x => x.ContragentId);
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
                        axios.get(`${self._pageLink}?handler=DeleteChecked&` + paras.toString() + '&comofferid=' + currentEditRow.Id).then(res => {
                            toastr["info"](`${translations.Delete} ${checkedId.length} ${translations.Success}"]`);
                            self.reloadData();
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









}
function CheckPriceConfirmed() {
    let rows = clsparticipant.dg.datagrid('getRows');
    let result = true;
    for (let i = 0; i < rows.length; i++) {
        //[Display(Name = "Цена предоставлена")]
        //PriceConfirmed = 2,
        //"Отказ поставщика" =3
        if (rows[0].Status != 3 && rows[0].Status == 1) {
            result = false;
            break;
        }
    }
    return result;
}
let clsparticipant = null;
var $dgContr = {};

var initdatagridContr = () => {
    $dgContr = $('#contragent_dg').datagrid({
        height: (window.innerHeight / 2),
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
        onLoadError: function (XMLHttpRequest, textStatus, errorThrown) {
            toastr["error"](`${translations.LoadFail},${errorThrown}`);
        },
        onBeforeLoad: function () {

        },
        onCheckAll: function (rows) {
            const checked = $(this).datagrid('getChecked').length > 0;

        },
        onUncheckAll: function () {

        },
        onCheck: function () {

        },
        onUncheck: function () {
            const checked = $(this).datagrid('getChecked').length > 0;

        },
        columns: [[

            { field: 'ck', checkbox: true },


            { field: 'StatusStr', title: translations.Status, sortable: true, width: 140 },
            { field: 'Name', title: translations.Name, sortable: true, width: 100 },
            { field: 'FullName', title: translations.FullName, sortable: true, width: 180 },
            { field: 'DirectionName', title: translations.Direction, sortable: true, width: 180 },
            {
                field: 'IsServiceStr', title: translations.Service, align: 'center', sortable: true, width: 80, formatter: checkboxformatter


            },
            { field: 'INN', title: translations.INN, sortable: true, width: 120 },
            { field: 'KPP', title: translations.KPP, sortable: true, width: 120 },
            { field: 'ManagerName', title: translations.Manager, sortable: true, width: 140 },
            { field: 'Phone', title: translations.Phone, sortable: true, width: 180 },
            { field: 'ContactPerson', title: translations.ContactPerson, sortable: true, width: 180 },
            { field: 'ContactPhone', title: translations.ContactPhone, sortable: true, width: 180 },
            { field: 'Email', title: 'Email', sortable: true, width: 130 }

        ]]

    })
        .datagrid('enableFilter', [

            {
                field: 'StatusStr',
                type: 'combobox',
                options: {
                    panelHeight: 'auto',
                    data: ContragentStatuses
                    ,

                    onChange: function (newValue, oldValue) {

                        $dgContr.datagrid('addFilterRule', {
                            field: 'Status',
                            op: 'equal',
                            value: newValue

                        });

                        console.log(newValue);

                    }
                }
            },

            {
                field: 'IsServiceStr',
                type: 'combobox',
                options: {
                    panelHeight: 'auto',
                    valueField: 'id',
                    textField: 'text',
                    data: YesNotList,
                    onChange: function (newValue, oldValue) {
                        console.log(newValue);
                        $dgContr.datagrid('addFilterRule', {
                            field: 'IsService',
                            op: 'equal',
                            value: newValue
                        });

                    }

                }

            }
        ]);
    //.datagrid('load', '@Url.Page("/Contragents/Index")?handler=DataActive');

}
var reloadDataContragent = () => {
    $dgContr.datagrid('load', '/Contragents/Index?handler=DataActive');

}
$('#participant_modal').on('shown.bs.modal', function () {
    $dgContr.datagrid('resize');
});
function InitParticipantTable() {
    clsparticipant = new TableComParticipant("participant", "clsparticipant", '/ComParticipants/Index');

    clsparticipant.tblColumns = [
        { field: 'ContragentName', title: translations.Participant, sortable: true, width: 130 },
        { field: 'StatusStr', title: translations.Status, sortable: true, width: 150 },


        { field: 'StepFailure', title: translations.StepFailure, sortable: true, width: 150 },
        { field: 'Description', title: "Комментарий", sortable: true, width: 250 }


    ];
    clsparticipant.tblFilters = [

        {
            field: 'StatusStr',
            type: 'combobox',
            options: {
                panelHeight: 'auto',
                data: ParticipantStatus,
    onChange: function (newValue, oldValue) {
        console.log('Sttus change ' + newValue);
        clsparticipant.dg.datagrid('addFilterRule', {
            field: 'Status',
            op: 'equal',
            value: newValue
        });
    }
}

                }
            ]
clsparticipant.OnNewRow = () => {
    reloadDataContragent();
}
    clsparticipant._defaultSortName = "ContragentId";
    clsparticipant._addCaption = translations.AddParticipants;

clsparticipant.Init();
clsparticipant.OnSubmitClick = (par) => {

    var checkedId = $dgContr.datagrid('getChecked').map(x => x.Id);
    if (checkedId <= 0) {
        bootbox.alert({
            message: translations.NoParticipantsSel,
            backdrop: true
        });
        return;
    }
    //
    $('#InputContrPar_ComOfferId').val(currentEditRow.Id);
    $('#InputContrPar_ComOfferId').trigger('change');
    //
    // checkedId = [1, 2, 4];
    $('#InputContrPar_ContragentIds').val(checkedId);
    $('#InputContrPar_ContragentIds').trigger('change');

    //alert($('#InputContrPar_ContragentIds').val());
    let request = $('#participant_edit_form').serialize();
    var form_data = new FormData();
    //form_data.append('InputContrPar.ContragentIds', JSON.stringify(checkedId));
    //form_data.append('InputContrPar.ComOfferId', 4);
    axios.post('/ComParticipants/Index?handler=AddMass', request)
        .then(res => {
            $(`#${par._name}_modal`).modal('toggle');
            toastr["info"](`${translations.AddSuccess} `);
            clsparticipant.reloadData();
        })
        .catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${translations.AddFail},${error.response.data}`);
            }
        });

};
//Контрагент таблица
initdatagridContr();
//clsparticipant.reloadData('/ComParticipants/Index?handler=Data');
}


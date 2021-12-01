
// translations
// this._pageLink
//_canEdit
//_catDelete
class clsBaseTable {

    _name;
    _pageLink;
    dg = {};
    tblFilters = [{}];

    tblColumns = [];

    jsonToFormCallBack;
    OnNewRow;
    //Update grid param;
    reloadParam;
    constructor(name, link) {
        this._name = name;
        this._pageLink = link;


    }
    Init() {
        var self = this;
        //  $(`#${this._name}_edit_form`).data("validator").settings.ignore = "";
        $(`#${this._name}_searchbutton`).click(function () {
            alert(`#${self._name}_searchbutton`);

            //this.reloadData();

        });
        $(`#${this._name}_addbutton`).click(function () {
            self.popupmodal(null);

        });
        $(`#${this._name}_deletebutton`).click(function () {
            self.onDeleteChecked();
        });
        $(`#${this._name}_exportbutton`).click(function () {
            self.onExport();
        });
        $(`#${this._name}_importbutton`).click(function () {
            self.showImportModal();
        });
        $(`#${this._name}_gettemplatebutton`).click(function () {
            self.onGetTemplate();
        });

        $(`#${this._name}_edit_form :submit`).click(function (e) {

            self.OnSubmitClick(self);

            event.preventDefault();
            event.stopPropagation();
        });
        this.initdatagrid1();
        this.dg.datagrid('resize');
    }
    OnSubmitClick(cls) {
        const form = document.querySelector(`#${cls._name}_edit_form`);
        if ($(form).valid() === false) {
            form.classList.add('was-validated');
        } else {
            $(form).removeClass("was-validated");
            let request = $(`#${cls._name}_edit_form`).serialize();
            if (typeof UpdateFiles == 'function')
                UpdateFiles();
            let form_data = new FormData($(`#${cls._name}_edit_form`)[0]);
            //if (AppendFilesToFormData) {
            //    AppendFilesToFormData(request);
            //}
            axios.post(`${cls._pageLink}`, form_data).then(res => {
                toastr["info"](`${translations.SaveSuccess} `);
                $(`#${cls._name}_modal`).modal('toggle');
                if (typeof clienUploadfilename == 'function')
                    clienUploadfilename();
                cls.reloadData();
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
    }
    ResizeGrid() {
        this.dg.datagrid('resize');
    }




    createColumns() {
        var InitColumns =
            [
                { field: 'ck', checkbox: true },
                {
                    field: '_action',
                    title: `${window.translations.Command} `,
                    width: 100,
                    align: 'center',
                    formatter: function (value, row, index) {
                        return `<div class="btn-group" role="group">
								  <button id="commandbtngroup1" type="button" ${(_canEdit ? "" : "disabled")}  class="btn btn-outline-primary btn-sm dropdown-toggle waves-effect waves-themed" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
									<i class="${window.translations.IconPrefix} fa-edit"></i>
								 </button>
								 <div class="dropdown-menu dropdown-menu-animated" aria-labelledby="commandbtngroup1">
								   <a role="button" class="dropdown-item" onclick="clscomposition.onEdit(${index})" ${(_canEdit ? "" : "disabled")}><i class="fal fa-edit mr-1"></i> ${translations.Edit}</a>
								   <a role="button" class="dropdown-item" onclick="clscomposition.onDelete('${row.Id}')" ${(_canDelete ? "" : "disabled")} ><i class="fal fa-trash-alt mr-1"></i> ${translations.Delete}</a>
								 </div>
							  </div>`;
                    }
                }




            ];
        if (this.tblColumns.length > 0) {
            return InitColumns.concat(this.tblColumns);
        }
        return InitColumns;
    };


    initdatagrid1() {
        this.dg = $(`#${this._name}_dg`).datagrid({
            height: (window.innerHeight / 3),
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
                $(`#${this._name}_deletebutton`).prop('disabled', true);


            },
            onCheckAll: function (rows) {
                const checked = $(this).datagrid('getChecked').length > 0;
                $(`#${this._name}_deletebutton`).prop('disabled', !checked);


            },
            onUncheckAll: function () {
                $(`#${this._name}_deletebutton`).prop('disabled', true);

            },
            onCheck: function () {
                $(`#${this._name}_deletebutton`).prop('disabled', false);


            },
            onUncheck: function () {
                const checked = $(this).datagrid('getChecked').length > 0;
                $(`#${this._name}_deletebutton`).prop('disabled', !checked);

            },
            columns: [this.createColumns()]
        })
            .datagrid('enableFilter', this.tblFilters);
        //.datagrid('load', `${this._pageLink}?handler=Data`);

    }

    reloadData(addParam) {


        if (addParam) {
            this.dg.datagrid('load', addParam);
            this.reloadParam = addParam;
        }
        else {
            if (this.reloadParam)
                this.dg.datagrid('load', this.reloadParam);
            else 
            this.dg.datagrid('load', `${this._pageLink}?handler=Data`);
        }
            
    }

    //$(() => {
    //    initdatagrid();
    //})
    //$(`#${this._name}_modal`).on('shown.bs.modal', function () {
    //    //var IsAddClick = $('#edit_form #Input_Id').val();
    //    //if (!IsAddClick || IsAddClick == 0) return;
    //    //if (typeof clienUploadfilename == 'function')
    //    //    clienUploadfilename();


    //})
    currentEditRow = null;
    popupmodal(nomenclature) {
        $(`#${this._name}_modal`).modal('toggle');
        $(`#${this._name}_modal .modal-title`).html(`${translations.AddCaption}`);
        $(`#${this._name}_edit_form`).clearForm();
        $(`#${this._name}_edit_form`)[0].reset();
        this.currentEditRow = nomenclature;
        if (nomenclature) {
            $(`#${this._name}_modal .modal-title`).html(`${translations.EditCaption}`);
            if (typeof this.jsonToFormCallBack !== 'undefined')
                $(`#${this._name}_edit_form`).jsonToForm(nomenclature, this.jsonToFormCallBack);
            else
                $(`#${this._name}_edit_form`).jsonToForm(nomenclature);
        } else {
            $(`#${this._name}_edit_form #InputPos_Id`).val(0)
            if (typeof clienUploadfilename == 'function')
                clienUploadfilename();
            if (typeof this.OnNewRow == 'function')
                this.OnNewRow();
        }
    }

    onEdit(index) {

        var nomenclature = this.dg.datagrid('getRows')[index];
        if (typeof getFiles == 'function')
            getFiles(this._pageLink + '?handler=FilesList&id=' + nomenclature.Id);
        this.popupmodal(nomenclature);
    }
    onDelete(id) {
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
                    axios.get(`${self._pageLink}?handler=Delete&id=` + id).then(res => {
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
        var checkedId = this.dg.datagrid('getChecked').map(x => x.Id);
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
                        axios.get(`${self._pageLink}?handler=DeleteChecked&` + paras.toString()).then(res => {
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
    onExport = () => {
        var options = this.dg.datagrid('options');
        var data = {
            filterRules: JSON.stringify(options.filterRules),
            sort: options.sortName,
            order: options.sortOrder,
        }
        console.log(options, data)
        var headers = {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        }
        axios.post(`${this._pageLink}?handler=Export`,
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








}

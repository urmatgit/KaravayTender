const tblStageColumns = [[
    { field: 'NomName', title: 'Позиция', sortable: true, width: 130, rowspan: 2, align: 'center' },
    { field: 'Stage', title: 'Этап', sortable: true, width: 100, rowspan: 2 },
    {
        field: 'StageDeadline', title: 'Срок ответа до', sortable: true, width: 100, rowspan: 2, formatter: dateformatter
    },
    { field: "ContName1", title: "name1", colspan: 2, styler: function (value, row,index) { return { class: 'clHead' }; } },
    { field: "ContName2", title: "name2", colspan: 2, styler: function (value, row, index) { return { class: 'clHead' }; }},
    { field: "ContName3", title: "name3", colspan: 2, styler: function (value, row, index) { return { class: 'clHead' }; } },
    { field: "ContName4", title: "name4", colspan: 2 },
    { field: "ContName5", title: "name5", colspan: 2 },
    { field: "ContName6", title: "name6", colspan: 2 },
    { field: "ContName7", title: "name7", colspan: 2 },
    { field: "ContName8", title: "name8", colspan: 2 },
    { field: "ContName9", title: "name9", colspan: 2 },
    { field: "ContName10", title: "name10", colspan: 2 },
    { field: "ContName11", title: "name11", colspan: 2 },
    { field: "ContName12", title: "name12", colspan: 2 },
    { field: "ContName13", title: "name13", colspan: 2 },
    { field: "ContName14", title: "name14", colspan: 2 },
    { field: "ContName15", title: "name15", colspan: 2 },
    { field: "ContName16", title: "name16", colspan: 2 },
    { field: "ContName17", title: "name17", colspan: 2 },
    { field: "ContName18", title: "name18", colspan: 2 },
    { field: "ContName19", title: "name19", colspan: 2 },
    { field: "ContName20", title: "name20", colspan: 2 },
], [

    {
        field: 'ContrPrice1', title: 'Цена', width: 100,  formatter: function (value, row) { return formatPriceStage(value, row, 1); }
    },
    {
        field: 'RequestPrice1', title: 'Запрос цены', width: 100,  formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 1); }
    },
    
    { field: 'ContrPrice2', title: 'Цена', width: 100,   formatter: function (value, row) { return formatPriceStage(value,row,2); }},
    { field: 'RequestPrice2', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 2); } },
    
    { field: 'ContrPrice3', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,3); } },
    { field: 'RequestPrice3', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 3); } },
    
    { field: 'ContrPrice4', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,4); } },
    { field: 'RequestPrice4', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 4); } },
    
    { field: 'ContrPrice5', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,5); }},
    { field: 'RequestPrice5', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 5); } },
    
    { field: 'ContrPrice6', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,6); } },
    { field: 'RequestPrice6', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 6); } },
    
    { field: 'ContrPrice7', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,7); } },
    { field: 'RequestPrice7', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 7); } },
    
    { field: 'ContrPrice8', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,8); }},
    { field: 'RequestPrice8', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 8); } },
    
    { field: 'ContrPrice9', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,9); }},
    { field: 'RequestPrice9', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 9); } },
    
    { field: 'ContrPrice10', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,10); }},
    { field: 'RequestPrice10', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 10); } },
    
    { field: 'ContrPrice11', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,11); }},
    { field: 'RequestPrice11', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 11); } },
    
    { field: 'ContrPrice12', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,12); }},
    { field: 'RequestPrice12', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 12); } },
    
    { field: 'ContrPrice13', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,13); } },
    { field: 'RequestPrice13', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 13); } },
    
    { field: 'ContrPrice14', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,14); } },
    { field: 'RequestPrice14', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 14); } },
    
    { field: 'ContrPrice15', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,15); } },
    { field: 'RequestPrice15', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 15); } },
    
    { field: 'ContrPrice16', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,16); } },
    { field: 'RequestPrice16', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 16); } },
    
    { field: 'ContrPrice17', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,17); }},
    { field: 'RequestPrice17', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 17); } },
    
    { field: 'ContrPrice18', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,18); }},
    { field: 'RequestPrice18', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 18); } },

    { field: 'ContrPrice19', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,19); }},
    { field: 'RequestPrice19', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 19); } },
    
    { field: 'ContrPrice20', title: 'Цена', width: 100, formatter: function (value, row) { return formatPriceStage(value,row,20); } },
    { field: 'RequestPrice20', title: 'Запрос цены', width: 100, formatter: function (value, row, index) { return checkboxformatterEdit(value, row, index, 20); } },
    ]
    

];
var dgcomstage = {};
initdatagridComStage = (cdata, dataComStages) => {
    dgcomstage = $(`#comstage_dg`).datagrid({
        height: (window.innerHeight / 2 -50),
        method: 'GET',
        rownumbers: true,
        singleSelect: true,
        selectOnCheck: false,
        checkOnSelect: false,
        pagination: true,
        clientPaging: true,
        remoteFilter: false,
        remoteSort: false,
        sortName: "Stage",
        sortOrder: 'asc',
        pageSize: 15,
        pageList: [10, 15, 30, 50, 100, 1000],
        columns: cdata,
        data: dataComStages,
        onLoadSuccess: function (data) {
            var panel = $(this).datagrid("getPanel");
            var myheaderCol = panel.find("div.datagrid-header td");
            // here is to add the css style
            //myheaderCol.css("border-", "1px solid #000");
            $('table.datagrid-htable').find('.datagrid-cell').css("text-align", 'center');
            //$(".editable[type='checkbox']").bind('click', function (e) {
            //    $(this).prop('checked', false);
            //});
            showHideButtons(currentEditRow.Status);
            //comstage_dg_datagrid-cell-c5-ContName1
            
            let contrNames = document.querySelectorAll('[class*=ContName]')
            if (contrNames && contrNames.length > 0) {
                contrNames.forEach(function (name) {
                    if (!$(name).hasClass('ColTopHeader'))
                        $(name).addClass("ColTopHeader");
                })
            }
        },
        onCellEdit: function (index, field, value) {
            //if (field == 'RequestPrice1') {
            //    var ck = $(this).datagrid('getEditor', { index: index, field: field });
            //    $(ck.target).bind('change', function (e) {
            //        this.value = false;
            //    })
            //}
        }

    });
}
function checkboxformatterEdit(value, row, index,colIndex) {

    // if (istrue(value)) {

    //     const checked = `<div >
    //                   <input type="checkbox" class="editable"  checked="checked"  id=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]}>
                       
    //               </div>`;
    //    return checked;
    //} else {
    //    var unchecked = `<div >
    //                   <input type="checkbox" class="editable" id=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]} >
    //               </div>`;

    //    return unchecked;
    //}
    let editable = "editable";
    if (row["ContrStatus" + colIndex] != 1 && row["ContrStatus" + colIndex] != 2)
        editable = "";
    if (istrue(value)) {

        const checked = `<div class="custom-control custom-checkbox">
                       <input type="checkbox" class="custom-control-input ${editable}" name="defaultCheckedDisabledEditable${index}" checked="checked"  id=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]}  disabled>
                       <label class="custom-control-label" for=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]}></label>
                   </div>`;
        return checked;
    } else {
        var unchecked = `<div class="custom-control custom-checkbox">
                       <input type="checkbox" class="custom-control-input ${editable}" name="defaultCheckedDisabledEditable${index}"  id=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]} disabled>
                       <label class="custom-control-label" for=${row["ComPositionId" + colIndex]}_${row["ContrId" + colIndex]}></label>
                   </div>`;

        return unchecked;
    }


}
var currentStage = false;
function LoadComState(comofferid) {
    
    let StageType = $('input[name="GetStageType"]:checked').val();
    axios.get(`/ComStages/Index?handler=Data&stage=` + StageType + "&comofferid=" + comofferid)
        .then(res => {
            let tblHeader = [[]];
            tblHeader[0] = tblStageColumns[0].slice();
            tblHeader[1] = tblStageColumns[1].slice();
            
            if (res && res.data.Header) {
                
                

                res.data.Header.forEach(function (val, index) {
                    tblHeader[0][3 + index].title = val;
                })
                tblHeader[0].splice(3 + res.data.Header.length)
                tblHeader[1].splice(res.data.Header.length * 2)
                initdatagridComStage(tblHeader, res.data.Body);
                $(`#comstage_dg`).datagrid('resize');
            } else {
                tblHeader[0].splice(2 )
                tblHeader[1].splice(0)
                initdatagridComStage(tblHeader,[[]]);
            }
            $('#StageNumber').html(res.data.CurrentStage);
            $('#StageId').val(res.data.CurrentStageId);
            currentStage = res.data;
            if (StageType === '1') {
                $(`#comstage_dg`).datagrid('hideColumn', 'Stage');
            } else {
                $(`#comstage_dg`).datagrid('showColumn', 'Stage');
                $('#StageId').val(0);
            }
            showHideButtons(currentEditRow.Status)
            
        })
        .catch((error) => {
            console.log(error);
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${translations.LoadFail},${error.response.data}`);
            }
        });
}

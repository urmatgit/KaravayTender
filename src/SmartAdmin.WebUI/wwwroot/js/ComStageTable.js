const tblStageColumns = [[
    { field: 'NomName', title: 'Позиция', sortable: true, width: 130, rowspan: 2, align: 'center'},
    { field: 'Stage', title: 'Этап', sortable: true, width: 100, rowspan: 2 },
    { field: "ContName1", title: "name1", colspan: 2,},
    { field: "ContName2", title: "name2", colspan: 2, },
    { field: "ContName3", title: "name3", colspan: 2 },
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
    
    { field: 'ContrPrice1', title: 'Прайс1',   width: 100 },
    {
        field: 'ContrStatus1', title: 'Статус', width: 100, formatter: checkboxformatterEdit
    },
    
    { field: 'ContrPrice2', title: 'Прайс',   width: 100 },
    { field: 'ContrStatus2', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice3', title: 'Прайс',   width: 100 },
    { field: 'ContrStatus3', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice4', title: 'Прайс',   width: 100 },
    { field: 'ContrStatus4', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice5', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus5', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice6', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus6', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice7', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus7', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice8', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus8', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice9', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus9', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice10', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus10', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice11', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus11', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice12', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus12', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice13', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus13', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice14', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus14', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice15', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus15', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice16', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus16', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice17', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus17', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice18', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus18', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },

    { field: 'ContrPrice19', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus19', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    
    { field: 'ContrPrice20', title: 'Прайс1',   width: 100 },
    { field: 'ContrStatus20', title: 'Статус',   width: 100, formatter: checkboxformatterEdit },
    ]
    

];
var dgcomstage = {};
initdatagridComStage = (cdata, dataComStages) => {
    dgcomstage = $(`#comstage_dg`).datagrid({
        height: (window.innerHeight / 3),
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
        },
        onCellEdit: function (index, field, value) {
            //if (field == 'ContrStatus1') {
            //    var ck = $(this).datagrid('getEditor', { index: index, field: field });
            //    $(ck.target).bind('change', function (e) {
            //        this.value = false;
            //    })
            //}
        }

    }).datagrid('enableCellEditing').datagrid('gotoCell', {
        index: 0,
        field: 'ContrStatus1'
    });;
}
function checkboxformatterEdit(value, row, index) {
    if (istrue(value)) {

        const checked = `<div class="custom-control custom-checkbox">
                       <input type="checkbox" class="custom-control-input editable" name="defaultCheckedDisabledEditable${index}" checked="checked" >
                       <label class="custom-control-label" for="defaultCheckedDisabledEditable${index}"></label>
                   </div>`;
        return checked;
    } else {
        var unchecked = `<div class="custom-control custom-checkbox">
                       <input type="checkbox" class="custom-control-input editable" name="defaultCheckedDisabledEditable${index}"  >
                       <label class="custom-control-label" for="defaultCheckedDisabledEditable${index}"></label>
                   </div>`;

        return unchecked;
    }


}
function LoadComState(comofferid) {
    
    let StageType = $('input[name="GetStageType"]:checked').val();
    axios.get(`/ComStages/Index?handler=Data&stage=` + StageType + "&comofferid=" + comofferid)
        .then(res => {
            let tblHeader = [[]];
            tblHeader[0] = tblStageColumns[0].slice();
            tblHeader[1] = tblStageColumns[1].slice();
            if (res && res.data.Header) {
                
                

                res.data.Header.forEach(function (val, index) {
                    tblHeader[0][2 + index].title = val;
                })
                tblHeader[0].splice(2 + res.data.Header.length)
                tblHeader[1].splice(res.data.Header.length * 2)
                initdatagridComStage(tblHeader, res.data.Body);
                $(`#comstage_dg`).datagrid('resize');
            } else {
                tblHeader[0].splice(2 )
                tblHeader[1].splice(0)
                initdatagridComStage(tblHeader,[[]]);
            }
            
            
            
            
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

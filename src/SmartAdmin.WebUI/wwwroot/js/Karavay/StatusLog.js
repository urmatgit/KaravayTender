 
var $dgStatusLog = {};
var initdatagridSL = () => {
    $dgStatusLog = $('#statuslog_dg').datagrid({
        height: (window.innerHeight /3),
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
            toastr["error"](`${window.translations.loadfail},${errorThrown}`);
        },

        columns: [[
            //   { field: 'ck', checkbox: true },
            //   {
            //       field: '_action',
            //       title: '@_localizer["Command"]',
            //       width: 80,
            //       align: 'center',
            //       formatter: function (value, row, index) {
            //           return `<div class="btn-group" role="group">
            // <button id="commandbtngroup" type="button" @(_canEdit.Succeeded? "":"disabled")  class="btn btn-outline-primary btn-sm dropdown-toggle waves-effect waves-themed" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            //<i class="@(Settings.Theme.IconPrefix) fa-edit"></i>
            //</button>
            //<div class="dropdown-menu dropdown-menu-animated" aria-labelledby="commandbtngroup">
            //  <button class="dropdown-item" onclick="onEdit(${index})" @(_canEdit.Succeeded? "":"disabled")><i class="fal fa-edit mr-1"></i> @_localizer["Edit"]</button>
            //  <button class="dropdown-item" onclick="onDelete('${row.Id}')" @(_canDelete.Succeeded? "":"disabled") ><i class="fal fa-trash-alt mr-1"></i> @_localizer["Delete"]</button>
            //  <button class="dropdown-item" onclick="onShowStatusLog('${row.Id}')" @(_canAcceditation.Succeeded? "":"disabled") ><i class="fal fa-trash-alt mr-1"></i> @_localizer["Status log"]</button>
            //</div>
            //</div>`;
            //       }
            //   },
            { field: 'DateTime', title: window.translations.Status, sortable: true, width: 140, formatter: datetimeformatter },
            { field: 'StatusStr', title: window.translations.Status, sortable: true, width: 140 },
            { field: 'ContragentName', title: window.translations.Name, sortable: true, width: 100 },

            { field: 'UserName', title: window.translations.Manager, sortable: true, width: 140 }




        ]]
    })
        .datagrid('enableFilter', [
            {
                field: 'StatusStr',
                type: 'combobox',
                options: {
                    panelHeight: 'auto',
                    valueField: 'id',
                    textField: 'text',
                    data: Statuses,
      //              data: [
      //                  {
      //                      value: null,
      //                      text: 'Все'
      //                  },
						////@foreach(var val in Enum.GetValues(typeof (CleanArchitecture.Razor.Domain.Enums.ContragentStatus)))
      ////                  {
      ////                  <text>
      ////                      {
      ////                          value: '@val',
      ////                          text: '@Html.Raw((((CleanArchitecture.Razor.Domain.Enums.ContragentStatus)val).ToDescriptionString()))'
						////	},
      ////                  </text>
      ////                  }
						//],
                    onChange: function (newValue, oldValue) {

                        $dgStatusLog.datagrid('addFilterRule', {
                            field: 'Status',
                            op: 'equal',
                            value: newValue
                        });

                        console.log(newValue);

                    }       
					}
				}

        ])
        .datagrid('load', '/Contragents/Index?handler=StatusLogs&ContragentId=' + currentEditRow.id);

	}

function ShowSelectWinnerDialog(selectData, onOk) {
    let selecttor = '<select id="winnerSelect" class="custom-select" required>';
    selecttor += `<option value="">пусто</option>`;
    selectData.forEach(function (item, index) {
        if (item.Status==2)
            selecttor += `<option value="${item.ContragentId}">${item.ContragentName}</option>`;
    });
    
    selecttor += '</select>';

   

    bootbox.dialog({
        title: "Запрос на подтверждение победителя",
        //size: 'large',
        message: '<div class="row">  ' +
            '<div class="col-md-12"> ' +
            '<form class="form-horizontal" id="selectWinner" class="needs-validation" novalidate="novalidate"> ' +
            
                    '<div class="form-group row"> ' +
                        '<div class="col-md-5 mat my-auto align_right">'+
                            '<label class="form-label" for="winnerSelect">Выберите победителя</label> ' +
                        '</div>' +
                    '<div class="col-md-7">' +
            selecttor +
            '<span class="invalid-feedback" asp-validation-for="winnerSelect"></span>'+
            '</div>' +
            '</div>' +
            '<div class="form-group row"> ' +
            '<div class="col-md-5 mat my-auto align_right">' +
            '<label class="form-label" for="winnerDate">Срок ответа до</label> ' +
            '</div>' +
            '<div class="col-md-7">' +
            '<input class="form-control" id="winnerDate" type="date" required>' +
            '<span class="invalid-feedback" asp-validation-for="winnerDate"></span>' +
            '</div>' +
            '</div>' +
            '<div class="form-group row"> ' +
            '<label class="form-label" for="winnerDate">Сообщение</label> ' +
            '<textarea id="message" class="form-control"></textarea>'+
            '</div>' +
            '</form> </div>  </div>',
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Отмена',
                className: 'btn-danger'
            },
            success: {
                label: '<i class="fa fa-check"></i> Подтвердить',
                className: "btn-success",
                callback: function () {
                    let selectedParticipant = $('#winnerSelect').val();
                    let DateRequest = $('#winnerDate').val();
                    let message = $('#message').val();
                    const form = document.querySelector('#selectWinner');
                    if ($(form).valid() === false) {
                        form.classList.add('was-validated');
                        return false;
                    }

                     
                    
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


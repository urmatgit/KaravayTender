

function getConragentStatusCount() {
    axios.get('/Contragents/Index?handler=OnRegistraionCount')
        .then(res => {
            console.log(res);
            //ContragentOnRegistrationCount
            $('*[id*=ContragentOnRegistrationCount]:visible').each(function () {
                if (res.data.Data <= 0) {
                    $(this).hide();
                }else 
                $(this).text(res.data.Data);
            });
        })
        .catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`Получение количество контрагентов (OnRegistraion),${error.response.data}`);
            }
        });
}


function getConragentUserById(id, callback) {
    axios.get('/Contragents/Index?handler=ContragentUser&id=' + id)
        .then(res => {
            console.log(res);
            if (callback)
                callback(res);
        })
        .catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${windows.translations.GetCategoriesFail},${error.response.data}`);
            }
        });
}
function getCategoriesByDirectionId(id, contragentid) {
    axios.get('/Contragents/Register?handler=Categories&directionid=' + id )
        .then(res => {
            console.log(res);

            $("#categories").html(res.data)

        })
        .catch((error) => {
            if (error.response.data.Errors) {
                const errors = error.response.data.Errors;
                errors.forEach(item => {
                    toastr["error"](item);
                });
            } else {
                toastr["error"](`${windows.translations.GetCategoriesFail},${error.response.data}`);
            }
        });
}
(function ($) {
    var selectedCategories = new Array();
    //$("input[name='category.Name']").change(function () {
    //    var categoryIds = document.getElementById("CategoryIds");
    //    var result = new Object();
    //    var resJson = "";
    //    $("input[name='category.Name']").each(function () {
    //        console.log(`${this.id}:${this.checked}`);
    //        // result += `${checkbox.id}:${checkbox.checked},`
    //        result.Id = this.id;
    //        result.IsCheck = this.checked
    //        resJson += `${JSON.stringify(result)},`;
    //    });
    //    categoryIds.value = `[${resJson.slice(0, -1)}]`;
    //})
    
    
getCheckedCategories = () => {
    var categories = document.getElementsByName("category.Name");
    var categoryIds = document.getElementById("CategoryIds");
    var result = new Object();
    var resJson = "";
    for (var checkbox of categories) {
        console.log(`${checkbox.id}:${checkbox.checked}`);
        // result += `${checkbox.id}:${checkbox.checked},`
        result.Id = checkbox.id;
        result.IsCheck = checkbox.checked
        resJson += `${JSON.stringify(result)},`;

    }
    categoryIds.value = `[${resJson.slice(0, -1)}]`;
    // console.log(`[${resJson.slice(0, -1)}]`);
}

//axios.get('@Url.Page("/Customers/Index")?handler=Delete&id=' + id)
$("#Input_DirectionId").change(function () {


    var id = $(this).val();
    console.log(id);
    getCategoriesByDirectionId(id);
    //$.ajax({
    //    type: "Get",
    //    url: ,  //remember change the controller to your owns.
    //    success: function (data) {
    //        $("#categories").html("");
    //        $("#categories").html(data);
    //    },
    //    error: function (response) {
    //        console.log(response.responseText);
    //    }
    //});
});
}(jQuery));

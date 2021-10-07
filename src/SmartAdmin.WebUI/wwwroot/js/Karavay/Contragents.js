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
// upload
var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
var maxFilesSize = 52428800; // 50MB -> 50 * 1024 * 1024
var uploadbutton = document.querySelector("button[name='uploadbutton']");
var uploadfileinputs = document.getElementById("Files");
var uploadfileinput = document.getElementById("file");
uploadbutton.addEventListener('click', (e) => {
    uploadfileinput.click();
})
let uploadFiles = {};
uploadfileinput.onchange = (e) => {
    //const files = uploadfileinput.taget.files;
    console.log(e);

    console.log(e.target.files);

    Array.from(e.target.files).forEach(file => {
        console.log(file.name);
        let isBigFile = "";
        let UnderLineText = "";
        if (IsBigFile(file)) {
            isBigFile = window.translations.FileSizeToBig;
            UnderLineText = 'style="text-decoration: line-through"';
        }
        if (file.name in uploadFiles) {

        }
        else {
            fileName = file.name;//uploadfileinput.value.split('\\').pop();
            uploadfilename.innerHTML += `
              <div class="alert ${isBigFile == "" ? "alert-info" : "alert-danger"}   alert-dismissible fadeup show mb-2 mt-2" role="alert" id="${fileName}">
                     <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                         <span aria-hidden="true"><i class="${window.translations.IconPrefix} fa-times"></i></span>
                      </button>
                      <div class="d-flex align-items-center">
							<div class="alert-icon">
									<i class="${window.translations.IconPrefix} fa-upload fs-xl"></i>
							</div>
							<div class="flex-1">
	    						 <span ${UnderLineText}>${fileName}  (${(file.size / 1024 / 1024).toFixed(3)} Mb)</span> ${isBigFile}
							</div>
					</div>
             </div>
        `
        }
        if (isBigFile === "")
            uploadFiles[file.name] = file;

    });
    UpdateFiles();
    $('#uploadfilename .alert').on('closed.bs.alert', (e) => {
        console.log(e);
        delete uploadFiles[e.currentTarget.id];
        UpdateFiles();
        uploadfileinput.value = null;

    })
};
function IsBigFile(file) {

    if (file.size > maxFileSize) {
        return true;
    }
    return false;
}
UpdateFiles = function () {
    const dT = new DataTransfer();

    $.map(uploadFiles, function (value, key) {
        dT.items.add(value);
    });
    uploadfileinputs.files = dT.files;
    console.log(uploadfileinputs.files);
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

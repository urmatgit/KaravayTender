var DateFilterOptions = {
    formatter: function (date) {
        return dateformatter(date);
    },
    parser: function (s) {
        if (!s) return new Date();
    }
};
function datetimeformatter(value, row, index) {
    if (typeof value === "undefined") {
        return null;
    }
    else if (moment(value).isValid() && !moment(value).isSame(moment('/Date(-62135596800000)/'))) {
        return moment(value).format('DD-MM-YYYY HH:mm:ss');
    } else {
        return null;
    }
}

function formatPrice(value) {
    var formatter = new Intl.NumberFormat('ru-Ru', {
        style: 'currency',
        currency: 'RUB',

        // These options are needed to round to whole numbers if that's what you want.
        //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
        //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
    });

    var number = Number(value);
    if (isNaN(number) || number==0) {
        return '';
    } else {
        return formatter.format(number);
        
    }
}
function formatPriceStage(value, row, colIndex) {
    var formatter = new Intl.NumberFormat('ru-Ru', {
        style: 'currency',
        currency: 'RUB',

        // These options are needed to round to whole numbers if that's what you want.
        //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
        //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
    });
    let showIconX = false;
    if (row["ShowX" + colIndex])
        showIconX = true;
    var number = Number(value);
    var priceColor = row["ContrPriceColor" + colIndex] ? row["ContrPriceColor" + colIndex] : "black";
    if (isNaN(number) || number == 0) {
        return (showIconX ? '<i class="fas fa-times-circle" style="color:red;margin-right:4px""></i>' : '') + '<span class="stageprice" ></span>';
    }
    else if (row.GoodPrice == null || row.GoodPrice.Price == null) {
        return (showIconX ? '<i class="fas fa-times-circle" style="color:red;margin-right:4px""></i>' : '') + `<span class="stageprice" style="color:${priceColor}" >` + formatter.format(number) + '</span>';
    } else {
        if (value == row.GoodPrice.Price && row["ContrId" + colIndex] == row.GoodPrice.ContragentId && !showIconX)
            return (showIconX ? '<i class="fas fa-times-circle" style="color:red;margin-right:4px"></i>' : '') + `<span class="stageprice" style="color:${row.GoodPrice.ColorRGB};">` + formatter.format(number) + '</span>'; //number.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');;
        else
            return (showIconX ? '<i class="fas fa-times-circle" style="color:red;margin-right:4px"></i>' : '') + `<span class="stageprice" style="color:${priceColor}">` + formatter.format(number) + '</span>';
    }
}


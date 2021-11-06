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

var base = {
    registerControl: function () {
        //base.refreshInformation();
    },
    smalldateFormatJsonDMY: function (datetime) {
        if (datetime == '' || datetime == undefined || datetime == null) {
            return '';
        } else {
            var newdate = new Date(parseInt(datetime.substr(6)));
            var month = newdate.getMonth() + 1;
            var day = newdate.getDate();
            var year = newdate.getFullYear();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            return day + "/" + month + "/" + year;
        }
    },
    bigdateFormatJsonDMY: function (datetime) {
        if (datetime == '' || datetime == undefined || datetime == null) {
            return '';
        } else {
            var newdate = new Date(parseInt(datetime.substr(6)));
            var month = newdate.getMonth() + 1;
            var day = newdate.getDate();
            var year = newdate.getFullYear();
            var hour = newdate.getHours();
            var min = newdate.getMinutes();
            var sec = newdate.getSeconds();
            if (month < 10)
                month = "0" + month;
            if (day < 10)
                day = "0" + day;
            if (hour < 10)
                hour = "0" + hour;
            if (min < 10)
                min = "0" + min;
            if (sec < 10)
                sec = "0" + sec;
            return day + "/" + month + "/" + year + " " + hour + ":" + min + ":" + sec;
        }
    },
    notification: function (type, message) {
        var strAlert = '<div class="alert-classic"><div class="alert alert-' + type + ' d-flex align-items-center" role="alert">' + (type == 'danger' ? '<i class="iconly-Danger icli"></i>' : '<i class="iconly-Tick-Square icli" ></i>') + '<div>' + message + '</div></div></div>';
        return strAlert;
    },
    success: function (message) {
        toastr({
            type: "success",
            message: message,
            timer: 2000
        });
    },
    error: function (message) {
        toastr({
            type: "error",
            message: message,
            timer: 2000
        });
    },
    loadButton: function (text) {
        return '<span class="d-flex align-items-center"><span class="spinner-border flex-shrink-0" role="status"><span class="visually-hidden">' + text + '...</span></span><span class="flex-grow-1 ms-2">' + text + '...</span></span>';
    },
    copy: function (content) {
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val(content).select();
        document.execCommand("copy");
        $temp.remove();
    },
    alert: function (type, message) {
        return '<p class="text-' + type + ' p-0">' + message + '</p>';
    },
    notificationIsRead: function (id) {
        $.ajax({
            url: '/pa/updateisread',
            type: 'post',
            data: {
                id: id
            },
            success: function () {

            }
        });
    }
};
$(document).ready(function () {
    base.registerControl();
});
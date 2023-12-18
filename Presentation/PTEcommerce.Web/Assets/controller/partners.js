var partner = {
    init: function () {
        if ($('#txtAccount').length > 0) {
            partner.getReport();
        }
        $('#start, #end').change(function () {
            partner.getReport();
        });
        if ($('#txtHistory').length > 0) {
            partner.getHistoryRef();
        }
        if ($('#tblAccountReferal').length > 0) {
            partner.getAccountRef();
        }
        if ($('#tblAccountTransfer').length > 0) {
            partner.getCustomerTransferHistory();
        }
    },
    getReport: function () {
        $.ajax({
            url: '/partners/reportpartner',
            type: 'get',
            data: {
                startdate: $('#start').val(),
                enddate: $('#end').val()
            },
            success: function (res) {
                $('#txtAccount').html(res.account);
                $('#txtPrices').html(res.price);
            }
        });
    },
    getAccountRef: function () {
        $.ajax({
            url: '/partners/getlistreferal',
            type: 'get',
            success: function (res) {
                $('#tblAccountReferal').html(res);
                $('.totalCustomer').html(accounting.formatMoney($('#totalRow').val(), "", 0, ".", ",", "%v%s"));
                $('#totalPriceAll').html(accounting.formatMoney($('#totalPrice').val(), "", 2, ".", ",", "%v%s"));
                $('.totalLoinhuan').html(accounting.formatMoney($('#totalFinalPrice').val() - $('#totalPrice').val(), "", 2, ".", ",", "%v%s"));
            }
        });
    },
    getHistoryRef: function () {
        $.ajax({
            url: '/partners/historydetail',
            type: 'get',
            success: function (res) {
                $('#txtHistory').html(res);
                $('#totalRefDisplay').html(accounting.formatMoney($('#totalPakage').val() - $('#totalPrice').val(), " USD", 2, ".", ",", "%v%s"));
                $('#totalQuantityPakage').html(accounting.formatMoney($('#totalRow').val(), "", 0, ".", ",", "%v%s"));
                $('#totalPricePakage').html(accounting.formatMoney($('#totalPakage').val(), "", 0, ".", ",", "%v%s"));
            }
        });
    },
    getCustomerTransferHistory: function () {
        $.ajax({
            url: '/partners/customertransferhistory',
            type: 'get',
            data: {
                startdate: $('#start').val(),
                enddate: $('#end').val()
            },
            success: function (res) {
                $('#tblAccountTransfer').html(res);
                $('#totalRefDisplay').html(accounting.formatMoney($('#totalPakage').val() - $('#totalPrice').val(), " USD", 2, ".", ",", "%v%s"));
                $('#totalQuantityPakage').html(accounting.formatMoney($('#totalRow').val(), "", 0, ".", ",", "%v%s"));
                $('#totalQuantity').html(accounting.formatMoney($('#totalRow').val(), "", 0, ".", ",", "%v%s"));
                $('#totalPricePakage').html(accounting.formatMoney($('#totalPakage').val(), "", 0, ".", ",", "%v%s"));
            }
        });
    }
};
$(document).ready(function () {
    partner.init();
});
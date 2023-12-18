var pakage = {
    registerControl: function () {
        pakage.getlistnetwork();
        $('#btnSubmiPakage').click(function () {
            pakage.submitOrder();
        });
    },
    getlistnetwork: function () {
        var searchUrl = "/AdministratorManager/Pakages/ListAllPagging";
        $('#tblPakageList').bootstrapTable('destroy');
        $('#tblPakageList').bootstrapTable({
            method: 'get',
            url: searchUrl,
            queryParams: function (p) {
                return {
                    limit: p.limit,
                    offset: p.offset
                };
            },
            striped: true,
            sidePagination: 'server',
            pagination: true,
            paginationVAlign: 'bottom',
            limit: 20,
            pageSize: 20,
            pageList: [20, 50, 100, 200, 500],
            search: false,
            //showColumns: true,
            showRefresh: false,
            minimumCountColumns: 2,
            columns: [
                {
                    field: 'Id',
                    title: '',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var html = '<button type="button" class="btn btn-primary btn-sm btnShowConfirmOrder" data-id="' + value + '" data-name="' + row.Name + '" data-minrate="' + row.MinRate + '" data-maxrate="' + row.MaxRate + '" data-ratenow="' + row.RateNow + '" data-price="' + row.Prices + '" data-type="' + row.Type + '" data-bonus="' + row.Bonus + '"><i class="bx bx-pencil"></i></button>';
                        return html;
                    }
                },
                {
                    field: 'Type',
                    title: 'Loại gói',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return 'Gói ngày';
                        }
                        else {
                            return 'Gói tháng';
                        }
                    }
                },
                {
                    field: 'Name',
                    title: 'Tên gói',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'Prices',
                    title: 'Giá tiền',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'MinRate',
                    title: 'Lãi tối thiểu',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " %", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'MaxRate',
                    title: 'Lãi tối đa',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " %", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'RateNow',
                    title: 'Lãi hiện tại',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " %", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'Bonus',
                    title: 'Thưởng thêm',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 2, ".", ",", "%v%s");
                    }
                }
            ],
            onLoadSuccess: function (data) {
                if (data.total > 0 && data.rows.length === 0) {
                    $('#tblPakageList').bootstrapTable('refresh', { silent: true });
                }
                $('.btnShowConfirmOrder').click(function () {
                    var name = $(this).data('name');
                    var minrate = $(this).data('minrate');
                    var maxrate = $(this).data('maxrate');
                    var ratenow = $(this).data('ratenow');
                    var type = $(this).data('type');
                    var price = $(this).data('price');
                    var bonus = $(this).data('bonus');
                    var id = $(this).data('id');
                    $('#ipOrderId').val(id);
                    $('.pakageName').html(name);
                    $('#ipName').val(name);
                    $('#ipRateMin').val(minrate);
                    $('#ipRateMax').val(maxrate);
                    $('#ipRateNow').val(ratenow);
                    $('#sltType').val(type);
                    $('#ipBonus').val(bonus);
                    $('#ipPrice').val(price);
                    $('#myModalConfirmPakage').modal('show');
                });
            }
        });
    },
    submitOrder: function () {
        $.ajax({
            url: '/AdministratorManager/pakages/Update',
            type: 'post',
            data: {
                Id: $('#ipOrderId').val(),
                Name: $('#ipName').val(),
                MinRate: $('#ipRateMin').val(),
                MaxRate: $('#ipRateMax').val(),
                RateNow: $('#ipRateNow').val(),
                Type: $('#sltType').val(),
                Bonus: $('#ipBonus').val(),
                Prices: $('#ipPrice').val()
            },
            success: function (res) {
                if (res.status) {
                    $('#myModalConfirmpakage').modal('hide');
                    base.toastrAlert(res.message, 'success');
                    $('#tblPakageList').bootstrapTable('refresh', { silent: true });
                }
                else {
                    base.toastrAlert(res.message, 'error');
                }
            }
        })
    }
};
$(document).ready(function () {
    pakage.registerControl();
});
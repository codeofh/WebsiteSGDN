var orderpakage = {
    registerControl: function () {
        orderpakage.getlistnetwork();
        $('#sltAccountSearch').select2();
        $('#btnSearch').click(function () {
            orderpakage.getlistnetwork();
        });
    },
    getlistnetwork: function () {
        var searchUrl = "/AdministratorManager/orderpakages/ListAllPagging";
        $('#tblHistoryOrderPakages').bootstrapTable('destroy');
        $('#tblHistoryOrderPakages').bootstrapTable({
            method: 'get',
            url: searchUrl,
            queryParams: function (p) {
                return {
                    limit: p.limit,
                    offset: p.offset,
                    status: $('#sltStatusSearch').val(),
                    idAccount: $('#sltAccountSearch').val(),
                    startDate: $('#startDate').val(),
                    endDate: $('#endDate').val()
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
                    field: 'TransactionId',
                    title: 'Mã GD',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'FullName',
                    title: 'Tài khoản',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return (value != null ? value : 'Chưa điền') + '<br/>(' + row.Email + ')';
                    }
                },
                {
                    field: 'PakageName',
                    title: 'Gói',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var html = '';
                        html += '<span class="text-primary"><b>' + value + '</b></span><br/>';
                        html += '<span class="text-danger"><b>' + accounting.formatMoney(row.Prices, " $", 0, ".", ",", "%v%s") + '</b></span>';
                        return html;
                    }
                },
                {
                    field: 'CreatedDate',
                    title: 'Ngày đầu tư',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return base.bigdateFormatJsonDMY(value);
                    }
                },
                {
                    field: 'CompleteDate',
                    title: 'Ngày thanh toán',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return base.bigdateFormatJsonDMY(value);
                    }
                },
                {
                    field: 'RateNow',
                    title: 'Lãi xuất',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, "%", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'FinalPrice',
                    title: 'Lợi nhuận',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 2, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'Status',
                    title: 'Trạng thái',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return '<span class="badge badge-pill badge-soft-primary font-size-11">Đang xử lý</span>';
                        }
                        else {
                            return '<span class="badge badge-pill badge-soft-success font-size-11">Đã duyệt</span>';
                        }
                    }
                }
            ],
            onLoadSuccess: function (data) {
                if (data.total > 0 && data.rows.length === 0) {
                    $('#tblHistoryOrderPakages').bootstrapTable('refresh', { silent: true });
                }
            }
        });
    }
};
$(document).ready(function () {
    orderpakage.registerControl();
});
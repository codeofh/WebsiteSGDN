var bankRecharge = {
    registerControl: function () {
        bankRecharge.getlistnetwork();
        $('#idAccount').select2();
        $('#btnSearch').click(function () {
            bankRecharge.getlistnetwork();
        });
        $('#btnSubmitRecharge').click(function () {
            bankRecharge.submitOrder();
        });
    },
    getlistnetwork: function () {
        var searchUrl = "/AdministratorManager/BankRecharge/ListAllPagging";
        $('#tblHistoryRecharge').bootstrapTable('destroy');
        $('#tblHistoryRecharge').bootstrapTable({
            method: 'get',
            url: searchUrl,
            queryParams: function (p) {
                return {
                    limit: p.limit,
                    offset: p.offset,
                    status: $('#statusBR').val(),
                    idAccount: $('#idAccount').val(),
                    startDate: $('#startDate').val(),
                    endDate: $('#endDate').val(),
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
                        var role = $('#txtRoleId').val();
                        if (row.Status == 1 && role == 1) {
                            var html = '<button type="button" class="btn btn-primary btn-sm btnShowConfirmOrder" data-id="' + value + '" data-gd="' + row.TransactionId + '" data-amount="' + row.Amount + '" data-status="' + row.Status + '"><i class="bx bx-check"></i></button>';
                            return html;
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    field: 'UserConfirm',
                    title: 'Người duyệt',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value == null ? '...' : value;
                    }
                },
                {
                    field: 'TransactionId',
                    title: 'Mã G.D',
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
                    field: 'Amount',
                    title: 'Số tiền',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 0, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'CreatedDate',
                    title: 'Ngày tạo',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return base.bigdateFormatJsonDMY(value);
                    }
                },
                {
                    field: 'ApproveDate',
                    title: 'Ngày duyệt',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return base.bigdateFormatJsonDMY(value);
                    }
                },
                {
                    field: 'Status',
                    title: 'Trạng thái',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        switch (value) {
                            case 1:
                                return '<span class="badge badge-pill badge-soft-primary font-size-11">Đang xử lý</span>';
                            case 2:
                                return '<span class="badge badge-pill badge-soft-success font-size-11">Đã duyệt</span>';
                            case 4:
                                return '<span class="badge badge-pill badge-soft-danger font-size-11">Đã hủy</span>';
                        }
                    }
                }
            ],
            onLoadSuccess: function (data) {
                if (data.total > 0 && data.rows.length === 0) {
                    $('#tblHistoryRecharge').bootstrapTable('refresh', { silent: true });
                }
                $('.btnShowConfirmOrder').click(function () {
                    var amount = $(this).data('amount');
                    var status = $(this).data('status');
                    var id = $(this).data('id');
                    var gd = $(this).data('gd');
                    $('#ipOrderId').val(id);
                    $('#orderId').html(gd);
                    $('#ipBankAmount').val(amount);
                    $('#sltStatus').val(status);
                    $('#myModalConfirmbankRecharge').modal('show');
                });
            }
        });
    },
    submitOrder: function () {
        if ($('#sltStatus').val() == 1) {
            base.toastrAlert('Vui lòng chọn trạng thái duyệt nạp tiền', 'error');
        }
        else if ($('#ipNote').val() == '') {
            base.toastrAlert('Vui lòng nhập ghi chú nạp tiền', 'error');
        }
        else {
            $.ajax({
                url: '/AdministratorManager/bankRecharge/SubmitOrder',
                type: 'post',
                data: {
                    id: $('#ipOrderId').val(),
                    status: $('#sltStatus').val(),
                    note: $('#ipNote').val()
                },
                success: function (res) {
                    if (res) {
                        $('#myModalConfirmbankRecharge').modal('hide');
                        base.toastrAlert(res.message, 'success');
                        $('#tblHistoryRecharge').bootstrapTable('refresh', { silent: true });
                    }
                    else {
                        base.toastrAlert(res.message, 'error');
                    }
                }
            })
        }
    }
};
$(document).ready(function () {
    bankRecharge.registerControl();
});
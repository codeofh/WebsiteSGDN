var account = {
    registerControl: function () {
        account.getlistaccount();
        $('#btnSearch').click(function () {
            account.getlistaccount();
        });
        $('#btnSubmitWithdrawal').click(function () {
            account.setamountforuser();
        });
        $('#btnSubmitChangePassword').click(function () {
            account.updatePassword();
        });
    },
    getlistaccount: function () {
        var searchUrl = "/AdministratorManager/AccountCustomer/ListAllPaging";
        $('#tblAccount').bootstrapTable('destroy');
        $('#tblAccount').bootstrapTable({
            method: 'get',
            url: searchUrl,
            queryParams: function (p) {
                return {
                    limit: p.limit,
                    offset: p.offset,
                    searchString: $('#ipSearchString').val(),
                    month: $('#txtMonth').val(),
                    year: $('#txtYear').val(),
                    from: $('#txtFromAmount').val(),
                    to: $('#txtToAmount').val()
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
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            columns: [
                {
                    field: 'Id',
                    title: '',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var html = '';
                        var role = $('#txtRoleId').val();
                        if (role == 1) {
                            html += '<button type="button" class="btn btn-primary btn-sm btnShowAmountAccount" data-name="' + row.Email + '" data-id="' + value + '" >Cộng/trừ tiền</button> ';
                        }
                        html += '<button type="button" class="btn btn-success btn-sm btnShowChangePassword" data-name="' + row.Email + '" data-id="' + value + '" >Đổi mk</button> ';
                        return html;
                    }
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
                    field: 'FullNameRef',
                    title: 'Đại lý',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (row.EmailRef == null) {
                            return '';
                        }
                        else {
                            return (value != null ? value : 'Chưa điền') + '<br/>(' + row.EmailRef + ')';
                        }
                    }
                },
                {
                    field: 'AmountAvaiable',
                    title: 'Số tiền',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 0, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'TotalBenefit',
                    title: 'Tổng lợi nhuận',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, index, row) {
                        return accounting.formatMoney(value, " $", 0, ".", ",", "%v%s");
                    }
                },
                {
                    field: 'AmountRef',
                    title: 'Tổng hoa hồng',
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
                    field: 'VerifyEmail',
                    title: 'Xác thực email',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value)
                            return '<span class="badge badge-pill badge-soft-success font-size-11">Đã xác thực</span>';
                        else
                            return '<span class="badge badge-pill badge-soft-danger font-size-11">Chưa xác thực</span>';
                    }
                },
                {
                    field: 'VerifyPhone',
                    title: 'Xác thực sdt',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value)
                            return '<span class="badge badge-pill badge-soft-success font-size-11">Đã xác thực</span>';
                        else
                            return '<span class="badge badge-pill badge-soft-danger font-size-11">Chưa xác thực</span>';
                    }
                },
                {
                    field: 'VerifyKYC',
                    title: 'KYC',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value)
                            return '<span class="badge badge-pill badge-soft-success font-size-11 btnKYC" data-id="' + row.Id + '">Đã xác thực</span>';
                        else
                            return '<span class="badge badge-pill badge-soft-danger font-size-11 btnKYC" data-id="' + row.Id + '">Chưa xác thực</span>';
                    }
                },
                {
                    field: 'FullName',
                    title: 'Thông tin KYC',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return '<button type="button" class="btn btn-primary btnShowKYC" data-front="' + row.KYCFront + '" data-back="' + row.KYCBack + '" data-name="' + row.FullName + '" data-bod="' + row.BirthOfDate + '" data-address="' + row.Address + '" data-gender="' + row.Gender + '">Xem chi tiết</button>';
                    }
                }
            ],
            onLoadSuccess: function (data) {
                if (data.total > 0 && data.rows.length === 0) {
                    $('#tblAccount').bootstrapTable('refresh', { silent: true });
                }
                $('.btnShowAmountAccount').click(function () {
                    var id = $(this).data('id');
                    var name = $(this).data('name');
                    $('#ipAccount').val(id);
                    $('#UsernameAdd').html(name);
                    $('#myModalConfirmAddAmount').modal('show');
                });
                $('.btnShowChangePassword').click(function () {
                    var id = $(this).data('id');
                    var name = $(this).data('name');
                    $('#ipAccountPassword').val(id);
                    $('#UsernamePass').html(name);
                    $('#myModalConfirmChangePassword').modal('show');
                });
                $('.btnKYC').click(function () {
                    var id = $(this).data('id');
                    account.updateKYC(id);
                });
                $('.btnShowKYC').click(function () {
                    var name = $(this).data('name');
                    var front = $(this).data('front');
                    var back = $(this).data('back');
                    var bod = $(this).data('bod');
                    var address = $(this).data('address');
                    var gender = $(this).data('gender');
                    $('#ipFullNameKYC').val(name);
                    $('#ipGenderKYC').val(gender == '1' ? 'Nam' : 'Nữ');
                    $('#ipBirthOfDateKYC').val(base.bigdateFormatJsonDMY(bod));
                    $('#ipAddressKYC').val(address);
                    $('#kycImageFront').attr('src', front);
                    $('#kycImageBack').attr('src', back);
                    $('#myModalShowKYC').modal('show');
                });
            }
        });
    },
    setamountforuser: function () {
        $.ajax({
            url: '/AdministratorManager/AccountCustomer/SetAmount',
            type: 'POST',
            data: {
                idAccount: $('#ipAccount').val(),
                amount: $('#ipAmount').val(),
                note: $('#ipNote').val(),
                type: $('#sltType').val()
            },
            success: function (res) {
                if (res.status) {
                    base.toastrAlert('Tác vụ thành công', 'success');
                    $('#ipAmount').val('');
                    $('#ipNote').val('');
                    $('#myModalConfirmAddAmount').modal('hide');
                    $('#tblAccount').bootstrapTable('refresh', { silent: true });
                }
                else {
                    base.toastrAlert(res.msg, 'error');
                }
            }
        });
    },
    updatePassword: function () {
        $.ajax({
            url: '/AdministratorManager/AccountCustomer/UpdatePassword',
            type: 'POST',
            data: {
                idAccount: $('#ipAccount').val(),
                password: $('#ipPasswordChange').val(),
            },
            success: function (res) {
                if (res.status) {
                    base.toastrAlert(res.message, 'success');
                    $('#tblAccount').bootstrapTable('refresh', { silent: true });
                }
                else {
                    base.toastrAlert(res.message, 'error');
                }
            }
        });
    },
    updateKYC: function (id) {
        $.ajax({
            url: '/AdministratorManager/AccountCustomer/UpdateKYC',
            type: 'POST',
            data: {
                idAccount: id
            },
            success: function (res) {
                if (res.status) {
                    base.toastrAlert(res.message, 'success');
                    $('#tblAccount').bootstrapTable('refresh', { silent: true });
                }
                else {
                    base.toastrAlert(res.message, 'error');
                }
            }
        });
    },
};
$(document).ready(function () {
    account.registerControl();
});
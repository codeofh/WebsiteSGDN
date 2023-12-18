var pa = {
    init: function () {
        $('#txtRechargeAmount').keyup(function () {
            if ($(this).val() == '' || $(this).val() == undefined) {
                $('#txtShowAmountRecharge').text('0');
            }
            else {
                $('#txtShowAmountRecharge').text(accounting.formatMoney($(this).val() / $('#txtConvertUSD').val(), "", 2, ".", ",", "%v%s"))
            }
        });
        $('#txtWithdrawalAmount').keyup(function () {
            if ($(this).val() == '' || $(this).val() == undefined) {
                $('#txtShowAmountWithdrawal').text('0');
            }
            else {
                $('#txtShowAmountWithdrawal').text(accounting.formatMoney($(this).val() * $('#txtConvertUSD').val(), "", 2, ".", ",", "%v%s"))
            }
        });
        $('#btnRechargeAmount').click(function () {
            pa.createDeposit();
        });
        $('#btnWithdrawalAmount').click(function () {
            pa.createWithdrawal();
        });
        if ($('#history-customer').length > 0) {
            pa.history();
        }
        $("#sltDayHistory").change(function () {
            pa.history();
        });
        $('#btnSubmitChangePassword').click(function () {
            pa.changePassword();
        });
        $('#btnConfirmBuyPakage').click(function () {
            pa.createInvest();
        });
        $('.btnShowConfirmBuy').click(function () {
            var name = $(this).data('name');
            var id = $(this).data('id');
            pa.confirmInvest(name, id);
        });
        $('#btnReceiveOTPChangePassword').click(function () {
            pa.sendOTP();
        });
        $('#btnShowCreateBankCustomer').click(function () {
            $('#txtIdBankCustomer').val(0);
            $('#txtBankAccount').val('');
            $('#txtBankNumer').val('');
            $('#sltIdBank').val(0);
            $('#notificationCreateOrUpdateBankCustomer').empty();
            $('#modalShowConfirmCreateOrUpdateBank').modal('show');
        });
        $('#btnConfirmCreateOrUpdateBank').click(function () {
            pa.createOrUpdateBankCustomer();
        });
        $('.btnDeleteBankCustomer').click(function () {
            var account = $(this).data('account');
            var number = $(this).data('number');
            var id = $(this).data('id');
            $('#txtBankAccountNumber').html(account + '/' + number);
            $('#txtSubmitIdDelete').val(id);
            $('#modalShowConfirmDelete').modal('show');
        });
        $('#btnConfirmDeleteBankCustomer').click(function () {
            pa.deleteBankCustomer();
        });
        $('.btnShowUpdateBankCustomer').click(function () {
            var id = $(this).data('id');
            var bankid = $(this).data('bankid');
            var account = $(this).data('account');
            var number = $(this).data('number');
            $('#txtIdBankCustomer').val(id);
            $('#txtBankAccount').val(account);
            $('#txtBankNumer').val(number);
            $('#sltIdBank').val(bankid);
            $('#notificationCreateOrUpdateBankCustomer').empty();
            $('#modalShowConfirmCreateOrUpdateBank').modal('show');
        });
        $('#bankIdCustomerWithdrawal').change(function () {
            var account = $(this).find(':selected').data('account');
            var number = $(this).find(':selected').data('number');
            $('#txtBankAccount').val(account);
            $('#txtBankNumber').val(number);
        });
    },
    createDeposit: function () {
        $.ajax({
            url: '/pa/createdeposit',
            type: 'post',
            data: {
                price: $('#txtRechargeAmount').val()
            },
            beforeSend: function () {
                $('#btnRechargeAmount').prop('disabled', true);
                $('#btnRechargeAmount').html(base.loadButton('Xác nhận'));
            },
            success: function (res) {
                $('#btnRechargeAmount').prop('disabled', false);
                $('#btnRechargeAmount').html('Xác nhận');
                if (res.status) {
                    $('#notification').html(base.alert('success', res.message));
                }
                else {
                    $('#notification').html(base.alert('danger', res.message));
                }
            }
        });
    },
    createWithdrawal: function () {
        $.ajax({
            url: '/pa/createwithdrawal',
            type: 'post',
            data: {
                price: $('#txtWithdrawalAmount').val(),
                bankId: $('#bankIdCustomerWithdrawal').val()
            },
            beforeSend: function () {
                $('#btnWithdrawalAmount').prop('disabled', true);
                $('#btnWithdrawalAmount').html(base.loadButton('Xác nhận'));
            },
            success: function (res) {
                $('#btnWithdrawalAmount').prop('disabled', false);
                $('#btnWithdrawalAmount').html('Xác nhận');
                if (res.status) {
                    $('#notification').html(base.alert('success', res.message));
                }
                else {
                    $('#notification').html(base.alert('danger', res.message));
                }
            }
        });
    },
    createInvest: function () {
        $.ajax({
            url: '/pa/createinvest',
            type: 'post',
            data: {
                pakageId: $('#txtSubmitIdBuy').val()
            },
            success: function (res) {
                $('#modalShowConfirmBuy').modal('hide');
                if (res.status) {
                    $('#modalShowError').modal('show');
                    $('#txtShowError').html(res.message);
                }
                else {
                    if (res.code == 1) {
                        $('#modalShowNotEnough').modal('show');
                    }
                    else {
                        $('#modalShowError').modal('show');
                        $('#txtShowError').html(res.message);
                    }
                }
            }
        });
    },
    sendOTP: function () {
        $.ajax({
            url: '/customer/sendotpchangepassword',
            type: 'get',
            beforeSend: function () {
                $('#btnReceiveOTPChangePassword').prop('disabled', true);
                $('#btnReceiveOTPChangePassword').html(base.loadButton('Đang gửi mã'));
            },
            success: function (res) {
                $('#btnReceiveOTPChangePassword').prop('disabled', false);
                $('#btnReceiveOTPChangePassword').html('Nhận mã xác nhận');
                if (res.status) {
                    $('#notificationChangePassword').html(base.alert('success', res.message));
                }
                else {
                    $('#notificationChangePassword').html(base.alert('danger', res.message));
                }
            }
        })
    },
    confirmInvest: function (name, id) {
        $('#btnPakageName').html(name);
        $('#txtSubmitIdBuy').val(id);
        $('#modalShowConfirmBuy').modal('show');
    },
    history: function () {
        $.ajax({
            url: '/pa/historydetail',
            type: 'get',
            data: {
                day: $("#sltDayHistory").val()
            },
            success: function (res) {
                $('#history-customer').html(res);
            }
        })
    },
    changePassword: function () {
        $.ajax({
            url: '/customer/changepassword',
            type: 'POST',
            data: {
                password: $('#txtPassword').val(),
                repassword: $('#txtRepassword').val(),
                otp: $('#txtOTP').val()
            },
            beforeSend: function () {
                $('#btnSubmitChangePassword').prop('disabled', true);
            },
            success: function (res) {
                $('#btnSubmitChangePassword').prop('disabled', false);
                if (res.status) {
                    $('#notificationChangePassword').html(base.alert('success', res.message));
                    window.location.href = '/accounts/sign-in';
                }
                else {
                    $('#notificationChangePassword').html(base.alert('danger', res.message));
                }
            }
        });
    },
    createOrUpdateBankCustomer: function () {
        $.ajax({
            url: '/pa/createoreditbank',
            type: 'POST',
            data: {
                id: $('#txtIdBankCustomer').val(),
                bankId: $('#sltIdBank').val(),
                bankAccount: $('#txtBankAccount').val(),
                bankNumber: $('#txtBankNumer').val()
            },
            beforeSend: function () {
                $('#btnConfirmCreateOrUpdateBank').prop('disabled', true);
            },
            success: function (res) {
                $('#btnConfirmCreateOrUpdateBank').prop('disabled', false);
                if (res.status) {
                    $('#notificationCreateOrUpdateBankCustomer').html(base.alert('success', res.message));
                    $('#modalShowConfirmCreateOrUpdateBank').modal('hide');
                    window.location.reload();
                }
                else {
                    $('#notificationCreateOrUpdateBankCustomer').html(base.alert('danger', res.message));
                }
            }
        });
    },
    deleteBankCustomer: function () {
        $.ajax({
            url: '/pa/deletebank',
            type: 'post',
            data: {
                id: $('#txtSubmitIdDelete').val()
            },
            beforeSend: function () {
                $('#btnConfirmDeleteBankCustomer').prop('disabled', true);
            },
            success: function (res) {
                $('#btnConfirmDeleteBankCustomer').prop('disabled', false);
                $('#modalShowConfirmDelete').modal('hide');
                window.location.reload();
            }
        })
    }
};
$(document).ready(function () {
    pa.init();
});
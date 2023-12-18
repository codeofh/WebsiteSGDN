var is_busy = false;
var offset = 0;
var stopped = false;
var customer = {
    registerControl: function () {
        $('#btnUpdateInformation').click(function () {
            customer.updateInformation();
        });
        $('#btnSendOTPEmail').click(function () {
            customer.sendOtpEmail();
        });
        $('#btnVerifyOtpPhone').click(function () {
            customer.verifyPhone();
        });
        $('#btnVerifyOtpEmail').click(function () {
            customer.verifyEmail();
        });
        $('#btnSubmitImageKYC').click(function () {
            customer.uploadKYC();
        });
    },
    sendOtpEmail: function () {
        $.ajax({
            url: '/Customer/SendEmailOTP',
            type: 'post',
            beforeSend: function () {
                $('#btnSendOTPEmail').prop('disabled', true);
                $('#btnSendOTPEmail').html(base.loadButton('Gửi mã cho tôi'));
            },
            success: function (res) {
                $('#btnSendOTPEmail').prop('disabled', false);
                $('#btnSendOTPEmail').html('Gửi mã cho tôi');
                if (res.status) {
                    $('#notificationEmail').html(base.alert('success', res.message));
                }
                else {
                    $('#notificationEmail').html(base.alert('danger', res.message));
                }
            }
        })
    },
    verifyEmail: function () {
        $.ajax({
            url: '/Customer/VerifyAccount',
            type: 'post',
            data: {
                type: 0,
                otp: $('#txtOtpEmail').val()
            },
            beforeSend: function () {
                $('#btnSendOTPEmail').prop('disabled', true);
                $('#btnSendOTPEmail').html(base.loadButton('Gửi mã cho tôi'));
            },
            success: function (res) {
                $('#btnSendOTPEmail').prop('disabled', false);
                $('#btnSendOTPEmail').html('Gửi mã cho tôi');
                if (res.status) {
                    $('#notificationEmail').html(base.alert('success', res.message));
                    window.location.reload();
                }
                else {
                    $('#notificationEmail').html(base.alert('danger', res.message));
                }
            }
        })
    },
    verifyPhone: function () {
        $.ajax({
            url: '/Customer/VerifyAccount',
            type: 'post',
            data: {
                type: 1,
                otp: $('#txtPhone').val()
            },
            beforeSend: function () {
                $('#btnVerifyOtpPhone').prop('disabled', true);
                $('#btnVerifyOtpPhone').html(base.loadButton('Tiếp tục'));
            },
            success: function (res) {
                $('#btnVerifyOtpPhone').prop('disabled', false);
                $('#btnVerifyOtpPhone').html('Tiếp tục');
                if (res.status) {
                    $('#notificationPhone').html(base.alert('success', res.message));
                    window.location.reload();
                }
                else {
                    $('#notificationPhone').html(base.alert('danger', res.message));
                }
            }
        })
    },
    updateInformation: function () {
        $.ajax({
            url: '/customer/update',
            type: 'post',
            data: {
                FullName: $('#txtFullName').val(),
                Gender: $('input[name="txtGender"]:checked').val(),
                BirthOfDate: $('#txtBirthOfDate').val(),
                Address: $('#txtAddress').val()
            },
            beforeSend: function () {
                $('#btnUpdateInformation').prop('disabled', true);
            },
            success: function (res) {
                $('#btnUpdateInformation').prop('disabled', false);
                if (res.status) {
                    $('.firstStep').addClass('d-none');
                    $('.lastStep').removeClass('d-none');
                }
                else {
                    $('#notificationInformation').html(base.alert('danger', res.message));
                }
            }
        });
    },
    uploadKYC: function () {
        var formData = new FormData();
        formData.append('front', $('#kycFontImage')[0].files[0]);
        formData.append('back', $('#kycBackImage')[0].files[0]);
        $.ajax({
            type: 'post',
            url: '/customer/kycimage',
            data: formData,
            success: function (res) {
                if (res.status) {
                    $('#notificationImageKYC').html(base.alert('success', res.message));
                }
                else {
                    $('#notificationImageKYC').html(base.alert('danger', res.message));
                }
            },
            processData: false,
            contentType: false,
            error: function () {
                alert("Không thể tải ảnh lên!");
            }
        });
    }
};
$(document).ready(function () {
    customer.registerControl();
});
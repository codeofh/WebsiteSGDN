var login = {
    registerControl: function () {
        $('#btnLogin').click(function () {
            login.login();
        });
        $('#btnRegister').click(function () {
            login.register();
        });
        $('#btnResetPassword').click(function () {
            login.resetpassword();
        });
    },
    login: function () {
        $.ajax({
            url: '/accounts/sign-in',
            type: 'POST',
            data: {
                userName: $('#username').val(),
                password: $('#password').val()
            },
            beforeSend: function () {
                $('#btnLogin').prop('disabled', true);
                $('#btnLogin').html(base.loadButton('Tiếp tục'));
            },
            success: function (res) {
                $('#btnLogin').prop('disabled', false);
                $('#btnLogin').html('Tiếp tục');
                if (res.status) {
                    window.location.href = '/pa/invest';
                }
                else {
                    $('#notification').html(base.alert('danger', res.msg));
                }
            }
        });
    },
    register: function () {
        $.ajax({
            url: '/accounts/sign-up',
            type: 'POST',
            data: {
                Email: $('#email').val(),
                Password: $('#password').val(),
                Ref: $('#ref').val(),
            },
            beforeSend: function () {
                $('#btnRegister').prop('disabled', true);
                $('#btnRegister').html(base.loadButton('Tiếp tục'));
            },
            success: function (res) {
                $('#btnRegister').prop('disabled', false);
                $('#btnRegister').html('Tiếp tục');
                if (res.status) {
                    window.location.href = '/pa/invest';
                }
                else {
                    $('#notification').html(base.alert('danger', res.msg));
                }
            }
        });
    },
    resetpassword: function () {
        $.ajax({
            url: '/accounts/reset-password',
            type: 'POST',
            data: {
                Email: $('#username').val()
            },
            beforeSend: function () {
                $('#btnResetPassword').prop('disabled', true);
                $('#btnResetPassword').html(base.loadButton('Tiếp tục'));
            },
            success: function (res) {
                $('#btnResetPassword').prop('disabled', false);
                $('#btnResetPassword').html('Tiếp tục');
                if (res.status) {
                    $('#notification').html(base.alert('success', res.message));
                }
                else {
                    $('#notification').html(base.alert('danger', res.message));
                }
            }
        });
    }
};
$(document).on('keypress', function (e) {
    if (e.which == 13) {
        if ($('#btnLogin').length > 0) {
            login.login();
        }
        if ($('#btnRegister').length > 0) {
            login.register();
        }
    }
});
$(document).ready(function () {
    login.registerControl();
});
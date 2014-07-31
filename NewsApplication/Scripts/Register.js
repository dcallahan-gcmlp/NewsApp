



$(document).ready(function () {

    $('.register-form').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },

            Password: {
                required: true,
                //equalTo: "#passwordVerify"
            }
        }
    });
})
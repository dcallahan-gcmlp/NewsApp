

$(document).ready(function () {
    

    $('.login-form').validate({
        rules: {
            Email: {
                required: true,
                email: true
            },

            Password: {
                required: true,    
            }
        }
    });
})
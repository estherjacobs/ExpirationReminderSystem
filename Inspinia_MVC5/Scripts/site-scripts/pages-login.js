$(function () {
    
    $(document).ready(function () {
      
        $("#login-form").validate({
            rules: {
                email: {
                    required: true,
                    } 
                },
                password: {
                    required: true,
                    email: true,

                },
            messages: {
                email: {
                    required: "Enter your name",
                },
                email: {
                    required: "Enter your email",
                    email: "Email is not valid",
                },
            }
        })

        $("#forgotpassword-form").validate({
            rules: {
                email: {
                    required: true,
                    email: true,
                    remote: {
                        url: '/Pages/CheckEmail',
                        type: 'get',
                        data: {
                            password: function () {
                                return $('#email').val();
                            }
                        }
                    }
                }
            },

            messages: {
                email: {
                    required: "Enter your email address",
                    email: "Enter a valid email address",
                    remote: "Sorry! This email does not exist"
                }
            }
        });


    })

    
});
$(function () {

    $(document).ready(function () {

        $("#register-form").validate({
            rules: {
                name: {
                    required: true,
                    minlength: 3
                },
                email: {
                    required: true,
                    email: true,
                    remote: {
                        url: '/Pages/CheckIfEmailExist',
                        type: 'post',
                        dataType: 'json',
                        data: {
                            password: function () {
                                return $('#email').val();
                            }
                        }
                    }
                },
                phone: {
                    required: true,
                    phoneUS: true
                },
                password: {
                    required: true,
                    minlength: 6
                },
                cpassword: {
                    required: true,
                    minlength: 6,
                    equalTo: "#password"
                }
               
            },
            messages: {
                name: {
                    required: "Please enter your name",
                    minlength: "The name must have a minimum of 3 characters"
                },
                email: {
                    required: "Please enter your email",
                    range: "Email is not valid",
                    remote: "An account with this email already exists"
                },
                phone: {
                    required: "Please enter your mobile phone number",
                    phoneUS: "Please enter a valid US number"
                },
                password: {
                    required: "Please create a password",
                    minlength: "Password must have a minimum of 6 characters"
                },
                cpassword: {
                    required: "Please confirm the password",
                    minlength: "Password must have a minimum of 6 characters",
                    equalTo: "Does not match"
                }
            }
        })
    })
});
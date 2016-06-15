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
                password: {
                    required: true,
                    minlength: 6
                },
                cpassword: {
                    equalTo: "#password"
                },
                phone: {
                    required: true,
                    phoneUS: true
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
                password: {
                    required: "Please create a password",
                    minlength: "Password must have a minimum of 6 characters"
                },
                cpassword: {
                    equalTo: "Does not match"
                },
                phone: {
                    required: "Please enter your mobile phone number",
                    phoneUS: "Please enter a valid US number"
                }
            }
        })
    })
});
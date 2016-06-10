$(function () {
    $(document).ready(function () {
        //jQuery.validator.setDefaults({
        //    debug: true,
        //    success: "valid"
        //});
        $("#update-account-form").validate({
            rules: {
                name: {
                    required: true,
                    minlength: 3
                },
                email: {
                    required: true,
                    email: true
                },
                phone: {
                    required: true,
                    phoneUS: true
                }
            },
            messages: {
                name: {
                    required: "Please enter your firstname",
                    minlength: "Your username must consist of at least 3 characters"
                },
                email: {
                    required: "Please enter",
                    email: "Please enter a valid email address"
                },
                phone: {
                    required: "Please enter",
                    phoneUS: "Please enter a valid phone number"
                }
            }
        });
    });
})
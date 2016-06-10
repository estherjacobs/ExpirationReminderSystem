$(function () {
    $(document).ready(function () {
        $("#reset-auth-form").validate({
            rules: {
                password: {
                    required: true,
                },
          
                confirmPassword: {
                    required: true,
                    equalTo: "#password"
                },
            },
            messages: {
                password: {
                    required: "Enter a new password",
                },
                confirmPassword: {
                    required: "Confirm your new password",
                    equalTo: "Does not match"
                },
            }
        })
    })
})
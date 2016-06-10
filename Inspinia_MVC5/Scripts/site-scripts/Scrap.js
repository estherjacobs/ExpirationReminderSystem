$(function () {
    //$(document).ready(function () {
    jQuery.validator.setDefaults({
        debug: true,
        success: "valid"
    });
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
                phoneNumber: true
            }
        },
        messages: {
            name: {
                required: "Please enter your firstname",
                minlength: "Your username must consist of at least 2 characters"
            },
            email: {
                required: "Please enter a email address",
                email: "Please enter a valid email address"
            },
            phone: {
                required: "Please enter a phone number",
                phoneNumber: "Please enter a valid phone number"
            }
        }
    });
});

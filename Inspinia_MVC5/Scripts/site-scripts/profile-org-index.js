$(function () {

    $(document).ready(function () {

        $("#update-profile-org-form").validate({
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
                },
                address: {
                    required: true
                },
                city: {
                    required: true
                },
                state: {
                    required: true
                },
                zip: {
                    required: true,
                    minlength: 5
                },
                year: {
                    required: true,
                }
            },
            messages: {
                name: {
                    required: "Please enter the name",
                    minlength: "The name must have a minimum of 3 characters"
                },
                email: {
                    required: "Please enter your email",
                    email: "This email is invalid"
                },
                
                phone: {
                    required: "Please enter a phone number",
                    phoneUS: "Please enter a valid US number"
                },
                address: {
                    required: "Please enter a address"
                },
                city: {
                    required: "Please enter a address"
                },
                state: {
                    required: "Please enter a address"
                },
                zip: {
                    required: "Please enter a address",
                    minlength: "Please enter a valid zip code"
                },
                year: {
                    required: "Please enter a address"
                },
            }
        })
    })
});
$(function () {
    $(document).ready(function () {

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
                    required: "Please enter your name",
                    minlength: "Your name must be a minimum of 3 characters"
                },
                email: {
                    required: "Please enter a email address",
                    email: "Please enter a valid email address"
                },
                phone: {
                    required: "Please enter a mobile phone number",
                    phoneUS: "Please enter a valid US number"
                }
            }
        });

        $("#update-password-form").validate({
            rules: {
                oldPassword: {
                    required: true,
                    minlength: 6,
                    remote: {
                        url: '/Profile/CheckPassword',
                        type: 'post',
                        data: {
                            password: function () {
                                return $('#oldPassword').val();
                            }
                        }
                    }
                },
                newPassword: {
                    required: true,
                    minlength: 6
                },
                confirmNewPassword: {
                    required: true,
                    minlength: 6,
                    equalTo: "#newPassword"
                }
            },

            messages: {
                oldPassword: {
                    required: "Please enter current password",
                    remote: "incorrect password"
                },
                newPassword: {
                    required: "Please enter a new password",
                    minlength: "Must be a minimum of 6 characters"
                },
                confirmNewPassword: {
                    required: "Please confirm the new password",
                    equalTo: "Does not match",
                }
            }
        });

        $('#update-notification-form').on('change', function () {
            var radioValue = ($('input[type=radio]:checked', '#update-notification-form').val());
        });

        $("#update-notification-form").validate({
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
                    required: "Please enter a name",
                    minlength: "Name must be a minimum of 3 characters"
                },
                email: {
                    required: "Please enter a email address",
                    email: "Please enter a valid email address"
                },
                phone: {
                    required: "Please enter a mobile phone number",
                    phoneUS: "Please enter a valid US number"
                }
            }
        });
    });

   
});
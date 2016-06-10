$(function () {
    $("#search").on('keyup', function () {
        var text = $(this).val();
        $("table tr:gt(0)").each(function () {
            var tr = $(this);
            var name = tr.find('td').text();
            if (name.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                tr.show();
            } else {
                tr.hide();
            }
        });
    });


    $("#addOrganization").on('click', function () {
        $("#new-organization").modal();
    });

    //VALIDATE ADD ORGANIZATION FORM

    //$(".form-new-organization").validate({
    //    rules: {
    //        orgID: {
    //            required: true,
    //            remote: {
    //                url: '/Pages/CheckIfOrgExist',
    //                type: 'post',
    //                dataType: 'json',
    //                data: {
    //                    password: function () {
    //                        return $('#orgID').val();
    //                    }
    //                }
    //            }
    //        },
    //        accessCode: {
    //            required: true,
    //            remote: {
    //                url: '/Pages/CheckIfAccessExist',
    //                type: 'post',
    //                dataType: 'json',
    //                data: {
    //                    password: function () {
    //                        return $('#orgID').val();
    //                    }
    //                }
    //            }
    //        }
    //    },
    //    messages: {
    //        orgID: {
    //            required: "Enter a organization id",
    //            remote: "Organization ID is invalid"
    //        },
    //        accessCode: {
    //            required: "Enter a access code",
    //            remote: "Access Code is invalid"
    //        }
    //    }
    //});
});




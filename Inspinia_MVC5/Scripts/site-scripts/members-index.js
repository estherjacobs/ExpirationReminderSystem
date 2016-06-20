$(function () {
    $(".addMember").on('click', function () {
        var itemid = $(this).data('companyid');
        function populateModal(itemid) {
            $("#companyid").val(itemid);
        }
        $("#addMemberModal").modal();
    });


    $('.viewItemImage').data('bgcolor', '#23c6c8').hover(function () {
        var $this = $(this);
        var newBgc = $this.data('bgcolor');
        $this.data('bgcolor', $this.css('color')).css('color', newBgc);
    });

    $("#itemTable").on('click', '.viewItemImage', function () {
        $("#edit-image-1").attr('src', "")
        $("#edit-image-2").attr('src', "")
        var itemid = $(this).data('itemid');

        $.get('/members/ViewItemImage1', { itemid: itemid }, function (path) {
            if (path != null) {
                var filePath = '/Images/' + path
                $("#edit-image-1").attr('src', filePath)
            }
            if (path == null) {
               
            }
        });

        $.get("/members/ViewItemImage2", { itemid: itemid }, function (path) {
            if (path != null) {
                var filePath = '/Images/' + path
                $("#edit-image-2").attr('src', filePath)
            }
        });

        $("#viewImageModal").modal();
    });

    $("#itemChosenTable").on('click', '.viewItemImage', function () {

        var itemid = $(this).data('itemid');

        $.get('/members/ViewItemImage1', { itemid: itemid }, function (path) {
            if (path != null) {
                var filePath = '/Images/' + path
                $("#edit-image-1").attr('src', filePath)
            }

        });

        $.get("/members/ViewItemImage2", { itemid: itemid }, function (path) {
            if (path != null) {
                var filePath = '/Images/' + path
                $("#edit-image-2").attr('src', filePath)
            }
        });

        $("#viewImageModal").modal();
    });

    $(".top-search").on('keyup', function () {
        var text = $(this).val();
        $("table tr").each(function () {
            var tr = $(this);
            var name = tr.find('td').text();
            if (name.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                tr.show();
            } else {
                tr.hide();
            }
        });
    });

    $(".disableUser").prop('disabled', true);
    //$(".stooltip").hide();
    $(".viewMember").on('click', function () {
        var userid = $(this).data('id');
        var orgid = $("#orgId").val();
        $("#courseTable").empty()
        $("#itemTable").empty()
        $("#itemChosenTable").empty()
        $("#coreCourseTable").empty()
        //$(this).closest('tr').remove();
        var orgid = $("#orgId").val();
        $.get('/members/getUserDetails', { userid: userid, orgid: orgid }, function (User) {
            $("#member-first").text(User.FullName),
            $("#member-name").val(User.FullName),
            $("#member-email").text(User.Email),
            $("#specific-userid").val(User.Id),
            $("#member-number").text(User.PhoneNumber),
            User.Courses.forEach(addCourseToTable),
            User.RequiredItems.forEach(addItemToTable),
            User.ExtraItems.forEach(addExtraItemToTable),
            User.CoreCourses.forEach(addCCourseToTable)

            if (User.Permission == 1) {

                $("#member-permission").text("User")

                $(".disableUser").prop('disabled', false);
            };

            if (User.Permission == 2) {

                $("#member-permission").text("Admin")

                $.get('/members/getmypermission', {
                    orgid: orgid
                }, function (MyPer) {
                    if (MyPer == 2) {
                        $(".disableUser").prop('disabled', true);
                    }
                    if (MyPer == 3) {
                        $(".disableUser").prop('disabled', false);
                        //$(".disableUser").attr({
                        //    "data-toggle": "tooltip",
                        //    "data-placement": "bottom",
                        //    "title": "You can't disable a super administrator"
                        //});
                
                    }
                });
            };

            if (User.Permission == 3) {
                $("#member-permission").text("Super Administrator")
                $.get('/members/getmypermission', {orgid: orgid }, function (MyPer) {
                    if (MyPer == 3) {
                        $(".disableUser").prop('disabled', true);
                    }
                });
            };

            $.get('/members/getdatejoined', { userid: userid, orgid: orgid }, function (date) {
                if (date != null) {
                    $("#member-date").text(date)
                }
            });
           
            //$(".disableUser").prop('disabled', false);
        });
    });
    function addCourseToTable(Course) {
        var row = new EJS({ url: '/content/templates/course-row.html' })
            .render(Course);
        $("#courseTable").append($(row));
    }
    function addItemToTable(RequiredItem) {
        var ExpirationItemId = RequiredItem.Id;
        //var request = $.ajax({
        //    url: '/members/checkifimage',
        //    async: false,
        //    type: 'POST',
        //    dataType: 'JSON',
        //    success: function (data) {
        //        if (data) {
        //            var row = new EJS({ url: '/content/templates/item-row.html' })
        //                 .render(RequiredItem);
        //            $("#itemTable").append($(row));
        //        } else {
        //            var row = new EJS({ url: '/content/templates/item-row-x.html' })
        //                 .render(RequiredItem);
        //            $("#itemTable").append($(row));
        //        }
        //    }
        //})


        var row = new EJS({ url: '/content/templates/item-row.html' })
            .render(RequiredItem);
        $("#itemTable").append($(row));
    }
    function addExtraItemToTable(ExtraItem) {
        var row = new EJS({ url: '/content/templates/item-row.html' })
            .render(ExtraItem);
        $("#itemChosenTable").append($(row));
    }
    function addCCourseToTable(CoreCourse) {
        var row = new EJS({ url: '/content/templates/core-course-row.html' })
            .render(CoreCourse);
        $("#coreCourseTable").append($(row));
    }
    $(document).ready(function () {
        $(".disableUser").click(function () {
            var userid = $("#specific-userid").val();
            var name = $("#member-name").val();
            var orgid = $("#orgId").val();

            swal({
                title: "Are you sure?",
                text: "You would like to disable this member from your organization",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, disable him!",
                cancelButtonText: "No, cancel please!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
              function (isConfirm) {
                  if (isConfirm) {
                      $.post('/members/disableUser', { userid: userid, orgid: orgid }, function () {
                      });
                      swal("Disabled!", name + " has been disabled.", "success");
                  } else {
                      swal("Cancelled", name + " is safe", "error");
                  }
              });
        });
    });
});

//VALIDATE ADD MEMBER FORM
$(".form-add-member").validate({
    rules: {
        email: {
            required: true,
            remote: {
                url: '/members/CheckIfEmailExist',
                type: 'get',
                dataType: 'json',
                data: {
                    password: function () {
                        return $('#email').val();
                    }
                }
            }
        },
        permission: {
            required: true,
            range: [1, 2],
            remote: {
                url: '/members/CheckIfEmailExist',
                type: 'get',
                dataType: 'json',
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
            required: "Enter an email address.",
            remote: "An account with this email already exists."
        },
        permission: {
            required: "Select a permission status.",
            range: "Select a permission status."
        }
    }
});
$('.viewMember').click(function () {
    var trid = $(this).closest('tr').attr('id');
});


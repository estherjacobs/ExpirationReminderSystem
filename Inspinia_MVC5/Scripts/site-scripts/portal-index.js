$(function () {

    $(document).ready(function () {

        //$.get("/portal/getemtcredits", (function (data) {
        //    var second = 24 - data;
        //    $("#sparkline6").sparkline([data, second], {
        //        type: 'pie',
        //        height: '200',
        //        sliceColors: ['#1ab394', '#F5F5F5']
        //    })
        //}));

        //ADD NEW EXPIRATION ITEM

        $(".additem").on('click', function () {
            $("#addModal").modal();
        });

        $(".form-add-item").validate({
            rules: {
                name: {
                    required: true,
                    minlength: 3
                },
                category: {
                    required: true,
                    range: [1, 10]
                },
                reminder: {
                    required: true,
                    range: [1, 30]
                },
                issuedate: {
                    required: {
                        date: true,
                        depends: function () {
                            return $('#category').val() == "2" || $('#category').val() == "3" || $('#category').val() == "1";
                        }
                    }
                },
                expiredate: {
                    date: true,
                    required: true
                },
                "image[0]": {
                    required: {
                        depends: function () {
                            return $('#category').val() == "2" || $('#category').val() == "3" || $('#category').val() == "1";
                        }
                    }
                },
                "image[1]": {
                    required: {
                        depends: function () {
                            return $('#category').val() == "2" || $('#category').val() == "3";
                        }
                    }
                },
                notes: {
                    required: false,
                    maxlength: 200
                }
            },
            messages: {
                name: {
                    required: "You can't leave this empty",
                    minlength: "Must be a minimum of 3 characters"
                },
                category: {
                    required: "You can't leave this empty",
                    range: "You can't leave this empty"
                },
                reminder: {
                    required: "You can't leave this empty",
                    range: "Enter a number between 1 and 50"
                },
                issuedate: {
                    required: "You can't leave this empty",
                    date: "Invalid date"
                },
                expiredate: {
                    required: "You can't leave this empty",
                    date: "Invalid date"
                },
                firstImage: {
                    required: "You must upload a image here"
                },
                secondImage: {
                    required: "You must upload a image here"
                },
                notes: {
                    maxlength: "This field is too long"
                }
            }
        })

        //SHOW AND REMOVE STARS BASED ON CHOOSEN CATEGORY

        $('.form-add-item').on('change', function () {
            categorySelected = $('#category :selected').val();

            if (categorySelected == "2" || categorySelected == "3") {
               
                $(".star1").show();
                $(".star2").show();
                $(".star3").show();
                $(".frontCard").show();
                $(".backCard").show();
            }
            else if (categorySelected == "1") {
                
                $(".star1").show();
                $(".star2").show();
                $(".frontCard").show();
            }
            else {
                $(".star1").hide();
                $(".star2").hide();
                $(".star3").hide();
                $(".backCard").hide();
            }
        });


        $("#firstImage").change(function () {
            readURLAdd(this);
        });
        function readURLAdd(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#add-image-1').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        };

        $("#secondImage").change(function () {
            readURLAdd2(this);
        });
        function readURLAdd2(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#image_upload_preview2').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        //ADD REMINDER

        $('.addReminder').data('bgcolor', '#23c6c8').hover(function () {
            var $this = $(this);
            var newBgc = $this.data('bgcolor');
            $this.data('bgcolor', $this.css('color')).css('color', newBgc);
        });

        $('.addReminder').on('click', function () {
            var itemid = $(this).data('itemid');
            populateModal(itemid);
            function populateModal(itemid) {
                $("#expirationItemId").val(itemid);
            }
            $('#addReminderModal').modal();
        });

        //VALIDATE REMINDER FORM
        $("#add-reminder-form").validate({
            rules: {
                addReminder: {
                    required: true,
                    range: [1, 30]
                },

                messages: {
                    addReminder: {
                        required: "Enter a reminder",
                        range: "Enter a number in between 1 and 50"

                    }
                }
            }
        });

        //EDIT EXPIRATION ITEM FILL AND CALL MODAL

        $(".edit-expirationitem").on('click', function () {
            var itemid = $(this).data('itemid');
            var name = $(this).data('name');
            var cat = $(this).data('category');
            var issuedate = $(this).data('issuedate');
            var expiredate = $(this).data('expiredate');
            var notes = $(this).data('notes');
            if (cat == "2" || cat == "3") {
                $(".editImageGroup").show();
                $(".backGroup").show();
                $.get("/portal/EditImageOne", { itemid: itemid }, function (path) {
                    var filePath = '/Images/' + path
                    $("#edit-image-1").attr('src', filePath)
                });
                $.get("/portal/EditImageTwo", { itemid: itemid }, function (path) {
                    var filePath = '/Images/' + path
                    $("#edit-image-2").attr('src', filePath)
                });

            }
            if (cat == "1") {
                $(".editImageGroup").show();
                $.get("/portal/EditImageOne", { itemid: itemid }, function (path) {
                    var filePath = '/Images/' + path
                    $("#edit-image-1").attr('src', filePath)
                    $(".backGroup").hide();
                });
            }
            if (cat == "5" || cat == "4" || cat == "6" || cat == "7") {

                $.get("/portal/CheckIfImage", { itemid: itemid }, function (exist) {
                    if (exist) {
                        $.get("/portal/EditImageOne", { itemid: itemid }, function (path) {
                            var filePath = '/Images/' + path
                            $("#edit-image-1").attr('src', filePath)
                            $(".editImageGroup").show();
                            $(".backGroup").hide();
                        });
                    } else {
                        $(".editImageGroup").hide();
                        $(".backGroup").show();
                    }
                });
            }

            populateModal(itemid, name, cat, issuedate, expiredate, notes);
            function populateModal(itemid, name, cat, issuedate, expiredate, notes) {
                $("#EIitemid").val(itemid);
                $("#EIname").val(name);
                $("#EIcategory").val(cat);
                $("#EIissuedate").val(issuedate);
                $("#EIexpiredate").val(expiredate);
                $("#EInotes").val(notes);
            }
            $("#editModal").modal();
        });

        //EDIT EXPIRATION ITEM FILE UPLOAD HACK
        $(".frontCardEdit").on('click', function () {
            $("#editImage1Click").trigger('click');
        });

        $(".backCardEdit").on('click', function () {
            $("#editImage2Click").trigger('click');
        });

        //UPLOAD FILE PREVIEW
        $("#editImage1Click").change(function () {
            readURL1(this);
        });
        function readURL1(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#edit-image-1').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        $("#editImage2Click").change(function () {
            readURL2(this);
        });
        function readURL2(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#edit-image-2').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        //VALIDATE EDIT EXPIRATION ITEM FORM
        $("#update-item").validate({
            rules: {
                EIname: {
                    required: true,
                    minlength: 3
                },
                EIcategory: {
                    range: [1, 10]
                },
                EIexpiredate: {
                    required: true
                }
            },
            messages: {
                EIname: {
                    required: "Please enter a name",
                    minlength: "Name must be a minimum of 3 characters"
                },

                EIexpiredate: {
                    required: "Please choose a issue date"
                },
                EIcategory: {
                    range: "You must choose a category"
                }
            }
        });
        //DELETE EXPIRATION ITEM
        $("#expirationItemTable").on('click', '.delete-eitem', function () {
            var itemid = $(this).data('itemid');
            var row = $(this).parent();
            $.post('/portal/DeleteEItem', { itemid: itemid }, function () {
                row.remove();
            });
        })

        //CALL NEW NON-EXPIRATION MODAL

        $(".addNonExpirationitem").on('click', function () {
            $("#addNonExpirationModal").modal();
        });
        //VALIDATE NON-EXPIRATION MODAL
        $("#edit-neitem-form").validate({
            rules: {
                ENEName: {
                    required: true,
                    minlength: 3
                },
                ENEDate: {
                    required: true
                }

            },
            messages: {
                ENEName: {
                    required: "Please enter a name",
                    minlength: "Name must be a minimum of 3 characters"
                },
                ENEDate: {
                    required: "Please enter a date"
                }
            }
        });

        //EDIT NON-EXPIRATION ITEM FILL AND CALL

        $(".edit-neitem-modal").on('click', function () {
            var itemid = $(this).data('itemid');
            var notes = $(this).data('notes');
            var date = $(this).data('date');
            var name = $(this).data('name');
            populateModal(date, notes, itemid, name);
            function populateModal(date, notes, itemid, name) {
                $("#ENENotes").val(notes);
                $("#ENEDate").val(date);
                $("#ENEName").val(name);
                $("#ENEitemid").val(itemid);
            }

            $("#editNEItemModal").modal();

        });

        //DELETE NON-EXPIRATION ITEM

        $("#NETable").on('click', '.deleteNEItem', function () {

            var itemid = $(this).data('itemid');
            var row = $(this).parent();
            $.post('/portal/DeleteNEItem', { itemid: itemid }, function () {
                row.remove();
            });
        });
        //CALL ADD COURSE MODAL


        $(".addCourseitem").on('click', function () {
            $("#addCourseModal").modal();
        });
        $(".addCoreCourseitem").on('click', function () {
            $("#addCoreCourseModal").modal();
        });

        //ADD COURSE VALIDATE

        $("#add-course-form").validate({
            rules: {
                courseName: {
                    required: true,
                    minlength: 3
                },
                courseCredits: {
                    range: [1, 24],
                    required: {
                        depends: function () {
                            return $('#category').val() == "0";
                        }
                    }
                },
                courseDate: {
                    required: true,
                },
                courseNotes: {
                    required: false,
                }
            },
            messages: {
                courseName: {
                    required: "Please enter a course name",
                    minlength: "Your name must be a minimum of 3 characters"
                },
                courseCredits: {
                    required: "Please enter number of credits",
                    range: "Number must be within the range of 1 and 6"
                },
                courseDate: {
                    required: "Please enter a course date"
                }
            }
        });



        //Add core course validate

        $("#add-core-course-form").validate({
            rules: {
                courseName: {
                    required: true,
                    minlength: 3
                },
                coreCategory: {
                    required: true,
                    range: [1, 4]
                },
                courseDate: {
                    required: true
                },
                courseNotes: {
                    required: false
                }
            },
            messages: {
                courseName: {
                    required: "Please enter a course name",
                    minlength: "Your name must be a minimum of 3 characters"
                },
                coreCategory: {
                    required: "Please choose a course category",
                    range: "Please choose a course category"
                },
                courseDate: {
                    required: "Please enter a course date"
                }
            }
        });

        //CALL EDIT COURSE MODAL

        //$(".edit-course").on('click', function () {
        //    $("#edit-course-modal").modal();
        //});
        //$(".edit-core-course").on('click', function () {
        //    $("#edit-core-course-modal").modal();
        //});

        $(".edit-course").on('click', function () {
            var itemid = $(this).data('itemid');
            var notes = $(this).data('notes');
            var date = $(this).data('date');
            var credits = $(this).data('credits');
            var name = $(this).data('name');
            populateModal(date, notes, itemid, name, credits);
            function populateModal(date, notes, itemid, name, credits) {
                $("#notes").val(notes);
                $("#credits").val(credits);
                $("#date").val(date);
                $("#name").val(name);
                $("#itemid").val(itemid);
            }

            $("#edit-course-modal").modal();

        });
        $(".edit-core-course").on('click', function () {
            var itemid = $(this).data('itemid');
            var notes = $(this).data('notes');
            var date = $(this).data('date');
            var category = $(this).data('coursecat');
            var name = $(this).data('name');
            populateModalCC(date, notes, itemid, name, category);
            function populateModalCC(date, notes, itemid, name, category) {
                $("#ccnotes").val(notes);
                $("#cccategory").val(category);
                $("#ccdate").val(date);
                $("#ccname").val(name);
                $("#ccitemid").val(itemid);
            }

            $("#edit-core-course-modal").modal();

        });


        $("#edit-course-form").validate({
            rules: {
                name: {
                    required: true,
                    minlength: 3
                },
                credits: {
                    range: [1, 24],
                    required: {
                        depends: function () {
                            return $('#category').val() == "0";
                        }
                    }
                },
                date: {
                    required: true,
                },
                notes: {
                    required: false,
                }
            },
            messages: {
                name: {
                    required: "Please enter a course name",
                    minlength: "Your name must be a minimum of 3 characters"
                },
                credits: {
                    required: "Please enter number of credits",
                    range: "Number must be within the range of 1 and 6"
                },
                date: {
                    required: "Please enter a course date"
                }
            }
        });

        $("#edit-core-course-form").validate({
            rules: {
                name: {
                    required: true,
                    minlength: 3
                },
                category: {
                    required: true,
                    range: [1, 4]
                },
                date: {
                    required: true
                },
                notes: {
                    required: false
                }
            },
            messages: {
                name: {
                    required: "Please enter a course name",
                    minlength: "Your name must be a minimum of 3 characters"
                },
                category: {
                    required: "Please choose a course category",
                    range: "Please choose a course category"
                },
                date: {
                    required: "Please enter a course date"
                }
            }
        });

        //DELETE COURSE

        $("#courseTable").on('click', '.deleteCourse', function () {
            var courseid = $(this).data('itemid');
            var row = $(this).parent();
            $.post('/portal/DeleteCourse', { courseid: courseid }, function () {
                row.remove();
            });
        });

        $("#courseTable").on('click', '.deleteCoreCourse', function () {
            var courseid = $(this).data('itemid');
            var row = $(this).parent();
            $.post('/portal/DeleteCoreCourse', { courseid: courseid }, function () {
                row.remove();
            });
        })



        $("#dropdown").change(function () {
            var val = $(this).val();
            switch (val) {

            }
        });

        //GENERAL DATE PICKER

        $('#data_1 .input-group.date').datepicker({
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true
        });

        $('body').addClass('light-navbar');

        //Pie chart with items in system

        var doughnutData = [
               {
                   value: 300,
                   color: "#a3e1d4",
                   highlight: "#1ab394",
                   label: "Expiration Items"
               },
               {
                   value: 50,
                   color: "#d88d9d",
                   highlight: "#ED5565",
                   label: "Non Expiration Items"
               },
               {
                   value: 100,
                   color: "#FCC990",
                   highlight: "#f8ac59",
                   label: "Courses"
               }
        ];

        var doughnutOptions = {
            segmentShowStroke: true,
            segmentStrokeColor: "#fff",
            segmentStrokeWidth: 2,
            percentageInnerCutout: 45, // This is 0 for Pie charts
            animationSteps: 100,
            animationEasing: "easeOutBounce",
            animateRotate: true,
            animateScale: false,
            responsive: true,
        };

        //var ctx = document.getElementById("doughnutChart").getContext("2d");
        //var myNewChart = new Chart(ctx).Doughnut(doughnutData, doughnutOptions);

        //FILL PIE CHART WITH COURSES COMPLETED FOR THIS SEMESTER

        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
        });

        //Required Items Modal
        $(".requiredItems").on('click', function () {
            $("#RequiredItemsList").empty();
            var userid = $(this).data('userid');
            var orgid = $(this).data('orgid');
            $.get('/portal/GetCompletedItems', { userid: userid, orgid: orgid }, function (Items) {
                Items.RequiredItems.forEach(addCompletedItems);
            });
            $.get('/portal/GetRequiredItems', { userid: userid, orgid: orgid }, function (Items) {
                Items.RequiredItems.forEach(addNonCompletedItems);
            });
            $("#requiredItemsModal").modal();
        });

        function addCompletedItems(Item) {
            $("#RequiredItemsList").append('<li><span class="m-l-xs todo-completed">' + Item.CategoryName + '</span></li>')
        }

        function addNonCompletedItems(Item) {
            $("#RequiredItemsList").append('<li><span class="m-l-xs">' + Item.CategoryName + '</span></li>')
        }

        $(".chooseItemToShare").on('click', function () {
            $("#extraItemList").empty();
            var userid = $(this).data('userid');
            var orgid = $(this).data('orgid');
            $("#item-shared-user").val(userid),
            $("#item-shared-org").val(orgid),
            $.get('/portal/GetExtraItems', { userid: userid, orgid: orgid }, function (Items) {
                Items.Items.forEach(addExtraItems);
            });
            $("#chooseItemToShareModal").modal();
        });

        function addExtraItems(Item) {

            if (Item.Shared == true) {
                $("#extraItemList").append('<li><input type="checkbox" checked value="' + Item.ItemId + '" name="item" class="i-checks" /> <span class="m-l-xs">' + Item.Name + ' - ' + Item.CategoryName + '</span></li>')
            }
            else {
                $("#extraItemList").append('<li><input type="checkbox" value="' + Item.ItemId + '" name="item" class="i-checks" /> <span class="m-l-xs">' + Item.Name + ' - ' + Item.CategoryName + '</span></li>')

            }
        }
    });
});



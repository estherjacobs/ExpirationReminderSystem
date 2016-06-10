$(function () {

    $('.addNote').on('click', function () {
        $('.addNoteModal').modal();
    });

    $(".form-add-note").validate({
        rules: {
            subject: {
                required: true,
                maxlength: 75
            },
            category: {
                required: true,
                range: [1, 10]
            },

            notes: {
                required: true
          
            }
        },
        messages: {
            subject: {
                required: "You can't leave this empty",
                maxlength: "This field is too long"
            },
            category: {
                required: "You can't leave this empty",
                range: "You can't leave this empty"
            },
            notes: {
                required: "You can't leave this empty"
            }
        }
    })
    //$('.deleteNote').on('click', function () {
    //    $('.addNoteModal').modal();
    //});

    $(".grid-notes").on('click', '.deleteNote', function () {
        var itemid = $(this).data('itemid');
        var box = $(this).parent().parent().parent().parent();
        $.post('/notes/DeleteNote', { itemid: itemid }, function () {
            box.remove();
        });
    })
})
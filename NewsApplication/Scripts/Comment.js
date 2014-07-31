

$(document).ready(function() {
    $('.comment-form').submit(function (e) {
        e.preventDefault(); //prevents the form from synchronously acting
        
        var $form = $(e.currentTarget); //refers to the current form that we're working with
        // $ if the variable is a jquery wrapped html element
        
        var comment = {
            "Body": $form.find('[name="Body"]').val(),
            "ArticleContentId": $form.find('[name="ArticleContentId"]').val()
        };
        $.post($form.attr("action"), comment, function (resp) {
            if (resp.error) {
                alert(resp.errormsg);
                return;
            }
            $form.find('[name="Body"]').val('');
            if (comment.Body != "") {
                var $commentForm = $('<form action="/NewsApplication/Comment/Delete" class="deleteComment-form" role="form"  method="post">' +
                        '<div id="commentContainer">' +
                            '<p>' + comment.Body + '</p>' +
                            '<input type="hidden" value="' + resp.id + '" name="ArticleCommentsId"/>' +
                            '<button type="submit" class="btn btn-default">Delete</button>' +
                        '</div>' +
                    '</form>').hide().fadeIn(500);
                $form.parent().children('#articleComments').append($commentForm);
            }
        });
    });
});


$(document).ready(function () {
    $('.deleteComment-form').submit(function(e) {
        e.preventDefault();

        var $form = $(e.currentTarget);
        var comment = {
            "ArticleCommentsId": $form.find('[name="ArticleCommentsId"]').val()
        };
        $.post($form.attr("action"), comment, function(resp) {
            if (resp.error) {
                alert(resp.message);
                return;
            }
            $form.fadeOut(500, function() {
                $(this).remove();
            });
        });
    });
})
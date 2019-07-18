$(document).ready(function () {

    $(".editbtn").on("click", function (e) {
            $("#jobnameedit").attr("value", $(this).data("name"));
            $("#jobid").attr("value", $(this).data("id"));
        });
})
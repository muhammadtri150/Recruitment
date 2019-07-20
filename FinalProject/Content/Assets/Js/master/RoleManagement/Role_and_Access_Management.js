$(document).ready(function () {

    $(".btn-edit").on("click", function () {

        $("#Edit_ROLE_ID").attr("value", $(this).data("role_id"));
        $("#Edit_ROLE_NAME").attr("value", $(this).data("role_name"));
    });
});
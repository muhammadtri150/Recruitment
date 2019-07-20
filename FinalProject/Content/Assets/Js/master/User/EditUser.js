$(document).ready(function () {

    $(".btn-edit").on("click", function () {
        $("#EDIT_USER_ID").attr("value", $(this).data("user_id"));
        $("#Edit_FULL_NAME").attr("value", $(this).data("user_full_name"));
        $("#Edit_USERNAME").attr("value", $(this).data("user_name"));
        //$("#Edit_PASSWORD").attr("value", $(this).data("user_password"));
        var role_id = $(this).data("user_role_id");
        jQuery.each($(".OptionRole"), function (i, v) { if (v.value == role_id) { v.selected = true; } });
        
    });

});
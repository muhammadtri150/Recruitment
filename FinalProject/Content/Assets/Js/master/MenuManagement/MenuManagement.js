$(document).ready(function () {

    $(".btn-edit").on("click", function () {

        $("#Edit_MENU_ID").attr("value", $(this).data("id"));
        $("#Edit_TITLE_MENU").attr("value", $(this).data("title"));
        $("#Edit_LOGO_MENU").attr("value", $(this).data("logo"));

    });

});
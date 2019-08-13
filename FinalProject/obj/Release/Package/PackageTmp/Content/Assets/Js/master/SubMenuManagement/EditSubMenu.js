$(document).ready(function () {

    $(".btn-edit").on("click", function () {

        $("#Edit_SUB_MENU_ID").attr("value", $(this).data("id"));
        $("#Edit_TITLE_SUBMENU").attr("value", $(this).data("title"));
        $("#Edit_LOGO_SUBMENU").attr("value", $(this).data("logo"));
        $("#Edit_URL").attr("value", $(this).data("url"));

        var menu_id = $(this).data("menu_id");
        jQuery.each($(".EditOptMenu"), function (i, v) {

            if (v.value== menu_id) { v.selected = true; }

        });

    });

});
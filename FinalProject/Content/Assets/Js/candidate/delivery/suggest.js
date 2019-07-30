$(document).ready(function () {

    $(".btn_edit").on("click", function () {
        $("#CANDIDATE_ID").attr("value", $(this).data("candidate_id"));
        $("#SELECTION_ID").attr("value", $(this).data("selection_id"));

        var client_id = $(this).data("client_id");
        var state_id = $(this).data("state_id");

        jQuery.each($(".states"), function (i, v) {
            if (v.value == state_id) { v.selected = true; }
        });
        jQuery.each($(".clients"), function (i, v) {
            if (v.value == client_id ) { v.selected = true; }
        });

        $("#DELIVERY_ID").attr("value", $(this).data("delivery_id"));
    });

});

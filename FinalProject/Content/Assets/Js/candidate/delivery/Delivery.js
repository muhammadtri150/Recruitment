$(document).ready(function () {

    $(".btn_next").on("click", function () {

        $("#CANDIDATE_ID").attr("value", $(this).data("candidate_id"));
        $("#SuggestionDate").attr("value", $(this).data("suggestion_date"));

    });

});
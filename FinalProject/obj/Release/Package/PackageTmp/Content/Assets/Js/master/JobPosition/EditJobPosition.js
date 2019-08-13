$(document).ready(function () {
    $(".btn-edit").on("click", function () {

        $("#Edit_JOBPOSITION_ID").attr("value", $(this).data("jobposition_id"))
        $("#Edit_JOBPOSITION_NAME").attr("value", $(this).data("jobposition_name"))
        $("#Edit_JOBPOSITION_NOTE").append($(this).data("jobposition_note"))

    })
})
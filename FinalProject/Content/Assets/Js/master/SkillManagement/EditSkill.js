$(document).ready(function () {

    $(".btn-edit").on("click", function () {

            $("#Edit_SKILL_ID").attr("value",$(this).data("skill_id"))
            $("#Edit_SKILL_NAME").attr("value", $(this).data("skill_name"))

    });

});
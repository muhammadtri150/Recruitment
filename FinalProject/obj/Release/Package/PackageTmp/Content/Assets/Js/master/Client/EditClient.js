$(document).ready(function () {

    $(".btn-edit").on("click", function () {

        $("#EditId").attr("value", $(this).data("client_id"));
        $("#EditName").attr("value", $(this).data("client_name"));
        $("#EditAddress").attr("value", $(this).data("client_address"));
        $("#EditOtherAddress").attr("value", $(this).data("client_other_address"));
        $("#EditIndustries").attr("value", $(this).data("client_industries"));
        $("#EditAddress").append($(this).data("client_address"));
        $("#EditOtherAddress").append($(this).data("client_other_address"));

    });
});

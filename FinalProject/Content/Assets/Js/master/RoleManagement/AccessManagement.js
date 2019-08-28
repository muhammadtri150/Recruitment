$(document).ready(function () {
    $(".CheckAcc").on("click", function () {

        var RoleId = $(this).data("role_id");
        var MenuId = $(this).data("menu_id");

        $.ajax({
            url: "http://localhost:80/recruitment2/master/rolemanagement/accesscandidateprocess",
            type: "post",
            data: { ROLE_ID: RoleId, MENU_ID: MenuId },
            success: function (d) {
                //document.location.href = "http://localhost:64197/master/rolemanagement/accessmenu/" + RoleId;
                //$("#ModalAccessCandidate").triggerHandler("click");
            }
        });
    });

    $(".CheckAccCandidate").on("click", function () {

        var RoleId = $(this).data("role_id");
        var MenuId = $(this).data("menu_id");
        var ActionId = $(this).data("action_id");

        $.ajax({
            url: "http://localhost:80/recruitment2/master/rolemanagement/accesscandidateprocess",
            type: "post",
            data: {
                ROLE_ID: RoleId,
                SUB_MENU_CANDIDATE_ID: MenuId,
                ACTION_CANDIDATE_ID: ActionId
            },
            success: function (d) {
                //document.location.href = "http://localhost/master/rolemanagement/accessmenu/" + RoleId;
            }
        });
    });
});
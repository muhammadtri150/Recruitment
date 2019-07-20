$(document).ready(function () {
    $(".CheckAcc").on("click", function () {

        var RoleId = $(this).data("role_id");
        var MenuId = $(this).data("menu_id");

        $.ajax({
            url: "http://localhost:64197/master/rolemanagement/accessmenu/proccess",
            type: "post",
            data: { ROLE_ID: RoleId, MENU_ID: MenuId },
            success: function (d) {
                document.location.href = "http://localhost:64197/master/rolemanagement/accessmenu/"+RoleId;
            }
        });
  });
});
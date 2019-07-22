using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.DTO;
using FinalProject.Controllers;
using System.Threading;
using System.Security;
using System.Security.Principal;

namespace FinalProject.Filters
{
    public class UserAccessCandidateFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext FilterContext)
        {

            var Context = FilterContext.RequestContext.HttpContext;

            try
            {
                //get data user login
                UserDTO DataUserLogin = (UserDTO)Context.Session["UserLogin"];

                if (DataUserLogin == null) Context.Response.Redirect("~/auth/login");

                //get request url from user, and split to get sub menu and menu candidate
                string[] Url = FilterContext.HttpContext.Request.RawUrl.Split('/');
                string Menu = Url[1];
                string SubMenu = Url[2];
                string Action = "read";
                if (Url.Length == 4) Action = Url[3];

                //prepare data
                using(DBEntities db = new DBEntities())
                {
                    //prepare data menu base on url segment 2 (index 1)
                    TB_MENU DataMenu = db.TB_MENU.FirstOrDefault(m => m.TITLE_MENU == Menu);

                    //check that menu is there or not
                    if (DataMenu == null) throw new Exception();

                    //get data access menu base on menu id and lore od of user login
                    TB_ACCESS_MENU DataAccessMenu = db.TB_ACCESS_MENU.FirstOrDefault(acc =>
                                                        acc.MENU_ID == DataMenu.MENU_ID &&
                                                        acc.ROLE_ID == DataUserLogin.ROLE_ID

                                                        );

                    //check data access menu is there or not 
                    if(DataAccessMenu == null) throw new Exception();


                    //prepare dat sub menu base on url segment 3 (index 2)
                    TB_SUBMENU DataSubMenu = db.TB_SUBMENU.FirstOrDefault(sm => sm.TITLE_SUBMENU == SubMenu);

                    //check existing data sub menu
                    if (DataSubMenu == null) throw new Exception();

                    //prepare data Action Candidate base of url segment 4 (index 3)
                    TB_ACTION_CANDIDATE DataActionCandidate = db.TB_ACTION_CANDIDATE.FirstOrDefault(ac => ac.ACTION_NAME == Action);

                    //check existing data action for data candidate
                    if (DataActionCandidate == null) throw new Exception();


                    //take data from tb_user_access_menu_candidate base on role id user login, id menu, id sub menu
                    TB_USER_ACCESS_MENU_CANDIDATE Access = db.TB_USER_ACCESS_MENU_CANDIDATE.FirstOrDefault(acc =>
                                                             acc.ROLE_ID == DataUserLogin.ROLE_ID &&
                                                             acc.SUB_MENU_CANDIDATE_ID == DataSubMenu.SUB_MENU_ID &&
                                                             acc.ACTION_CANDIDATE_ID == DataActionCandidate.ID
                                                          );

                    //check existing data user access to sub menu candidate
                    if (Access == null) throw new Exception();
                }
        }
            catch (Exception)
            {
                Context.Response.Redirect("~/auth/error");
            }
            base.OnActionExecuting(FilterContext);
        }

    }
}
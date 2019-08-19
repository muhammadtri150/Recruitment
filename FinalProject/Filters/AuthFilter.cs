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
using System.Configuration;

namespace FinalProject.Filters
{
    public class AuthFilter:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var Context = filterContext.RequestContext.HttpContext;
            try
            {
                if (Context.Session["UserLogin"] == null)
                {
                    Context.Response.Redirect("~/auth/login");
                }
                else
                {
                    UserDTO UserLogin = (UserDTO)Context.Session["UserLogin"];
                    //get session and prosess match betwen menu, sub menu and user role
                    using (DBEntities db = new DBEntities())
                    {
                        //get tb_role base on role id in session user login
                        TB_ROLE UserRole = db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == UserLogin.ROLE_ID);
                        string[] url = filterContext.HttpContext.Request.RawUrl.ToString().Split('/');
                        string Title_Menu = url[2];
                        if(Title_Menu.ToLower() == "dashboard")
                        {
                            Context.Response.Redirect("~/dashboard");
                        }
                        else
                        {
                            if (Title_Menu == "" || Title_Menu == null)
                            {
                                Context.Response.Redirect("~");
                            }

                            TB_MENU Tb_Menu = db.TB_MENU.FirstOrDefault(m => m.TITLE_MENU == Title_Menu);

                            TB_ACCESS_MENU Access_Menu = db.TB_ACCESS_MENU.FirstOrDefault(am => (am.MENU_ID == Tb_Menu.MENU_ID && am.ROLE_ID == UserRole.ROLE_ID));
                            //cheking access based role user and tb_access_menu

                            if (Access_Menu == null)
                            {
                                throw new Exception();
                            }
                        }
                    };
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserLogin.USERNAME), null);
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                Context.Response.Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("null"), null);
            base.OnAuthorization(filterContext);
        }
    }
}
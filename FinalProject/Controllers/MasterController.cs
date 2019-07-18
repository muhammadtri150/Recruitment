using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.DTO;
using FinalProject.Filters;
using System.Security.Cryptography;
using System.Text;

namespace FinalProject.Controllers
{
    [AllowAnonymous]
    [AuthFilter]
    public class MasterController : Controller
    {
        // GET: Admin
        [Route("master")]
        public ActionResult Index()
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, string>()
                {
                    {"title","User"}
                };
                return View("Index");
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }

        [Route("master/adduser")]
        public ActionResult AddUser(UserDTO NewUser)
        {
            if (NewUser != null)
            {

                using (DBEntities db = new DBEntities())
                {
                    if (ModelState.IsValid)
                    {
                        //encrypt password with sha256

                        TB_USER DataNewUser = new TB_USER
                        {
                            ROLE_ID = NewUser.ROLE_ID,
                            USERNAME = NewUser.USERNAME,
                            PASSWORD = new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(NewUser.PASSWORD)).ToString(),
                            FULL_NAME = NewUser.FULL_NAME,
                            EMAIL = NewUser.EMAIL
                        };

                        db.TB_USER.Add(DataNewUser);

                        if (db.SaveChanges() < 1)
                        {
                            TempData.Add("message", "Add new user is fail");
                            TempData.Add("type", "warning");
                            return Redirect("~/auth/login");
                        }
                        else
                        {
                            TempData.Add("message", "Add new User Successfully");
                            TempData.Add("type", "success");
                            return Redirect("~/auth/login");
                        }
                    }
                }
            }
            return View("AddUser");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.DTO;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        [Route("dashboard")]
        public ActionResult Index()
        {
            try
            {
                if (Session["UserLogin"] == null)
                {
                   return Redirect("~/auth/login");
                }
                else
                {
                  ViewBag.DataView = new Dictionary<string, string>()
                  {
                     {"title","Dashboard"}
                  };
                  return View("Index");
                }
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }
    }
}
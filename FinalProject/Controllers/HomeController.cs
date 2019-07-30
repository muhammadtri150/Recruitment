using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Filters;
using FinalProject.DTO;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    [AllowAnonymous]
    [AuthFilter]
    public class HomeController : Controller
    {
       [Route("")]
       public ActionResult Index()
        {
            try
            {
                    return Redirect("~/dashboard");
                
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
    }
}
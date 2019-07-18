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

//----------------------------------------------------- sub menu job portal, can add, edite, and delete ----------------------------------------

        [Route("master/jobportal")]
        public ActionResult JobPortal()
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {
                    List<JobPortalDTO> ListJobPortal = db.JOB_PORTAL.Select(j =>
                            new JobPortalDTO
                            {
                                JOB_ID = j.JOB_ID,
                                JOBPORTAL_NAME = j.JOBPORTAL_NAME,
                                JOBPORTAL_ADDED = j.JOBPORTAL_ADDED
                            }
                        ).ToList();
                    ViewBag.DataView = new Dictionary<string, string>()
                    {
                        {"title","Job Portal" }
                    };
                    return View("JobPortal/Index",ListJobPortal);
                }
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }

        [Route("master/jobportal/add")]
        public ActionResult JobPortalAdd(JobPortalDTO JobPortalDTO)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    //--inisialisasi object tb_jobportal and generate added time
                    db.JOB_PORTAL.Add(new JOB_PORTAL
                    {
                        JOBPORTAL_NAME = JobPortalDTO.JOBPORTAL_NAME,
                        JOBPORTAL_ADDED = DateTime.Now
                    });
                    if(db.SaveChanges() > 0)
                    {
                        TempData.Add("message", "Job Portal added successfully");
                        TempData.Add("type", "success");
                    }
                    else
                    {
                        TempData.Add("message", "Job Portal failed to add");
                        TempData.Add("type", "warning");
                    }
                    return Redirect("~/master/jobportal");
                }
            }
            catch(Exception e)
            {
                return Redirect("~/auth/error");
            }
        }

        [Route("master/jobportal/edit")]
        public ActionResult JobPortalEdit(JobPortalDTO JobPortalDTO)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                   JOB_PORTAL TB_JobPortal =  db.JOB_PORTAL.FirstOrDefault(p => p.JOB_ID == JobPortalDTO.JOB_ID);
                    TB_JobPortal.JOBPORTAL_NAME = JobPortalDTO.JOBPORTAL_NAME;
                    if (db.SaveChanges() > 0)
                    {
                        TempData.Add("message", "Job Portal edited successfully");
                        TempData.Add("type", "success");
                    }
                    else
                    {
                        TempData.Add("message", "Job Portal failed to edit");
                        TempData.Add("type", "warning");
                    }
                    return Redirect("~/master/jobportal");
                }
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }



























        // --------------------------------------------------------------------Sub Menu Add User-------------------------------------------------------
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
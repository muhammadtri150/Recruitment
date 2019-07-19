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

        //######################################## sub menu job portal, can add, edite, and delete ########################################

        //---------------------------------------------Just for show sub menu page job portal 
        [Route("master/jobportal")]
        public ActionResult JobPortal()
        {
            try
            {
                using (DBEntities db = new DBEntities())
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
                    return View("JobPortal/Index", ListJobPortal);
                }
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }
        // -------------------------------------------------------- add new job portal --------------------------------------
        [Route("master/jobportal/add")]
        public ActionResult JobPortalAdd(JobPortalDTO JobPortalDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //--inisialisasi object tb_jobportal and generate added time
                        db.JOB_PORTAL.Add(new JOB_PORTAL
                        {
                            JOBPORTAL_NAME = JobPortalDTO.JOBPORTAL_NAME,
                            JOBPORTAL_ADDED = DateTime.Now
                        });
                        if (db.SaveChanges() > 0)
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
                TempData.Add("message", "Please fill the field");
                TempData.Add("type", "danger");
                return Redirect("~/master/jobportal");
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }
        //------------------------------------------ edit job portal----------------------------------------------------
        [Route("master/jobportal/edit")]
        public ActionResult JobPortalEdit(JobPortalDTO JobPortalDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        JOB_PORTAL TB_JobPortal = db.JOB_PORTAL.FirstOrDefault(p => p.JOB_ID == JobPortalDTO.JOB_ID);
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
                TempData.Add("message", HtmlHelper.ValidationSummaryMessageElement);
                TempData.Add("type", "danger");
                return Redirect("~/master/jobportal");
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }
        // ---------------------------------------------------- remove job portal -------------------------------------------------------------
        [Route("master/jobportal/delete/{id?}")]
        public ActionResult JobPortalDelete(string id = null)
        {
            try
            {

                using (DBEntities db = new DBEntities())
                {

                    if (id == null)
                    {
                        return Redirect("~/master/jobportal");
                    }
                    int JobId = Convert.ToInt32(id);
                    JOB_PORTAL Tb_JobPortal = db.JOB_PORTAL.FirstOrDefault(j => j.JOB_ID == JobId);
                    if (Tb_JobPortal == null)
                    {
                        return Redirect("~/master/jobportal");
                    }

                    else
                    {
                        db.JOB_PORTAL.Remove(Tb_JobPortal);

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Job Portal have been deleted");
                            TempData.Add("type", "success");
                        }
                        else
                        {
                            TempData.Add("message", "Job Portal failed to deleted");
                            TempData.Add("type", "danger");
                        }
                    }
                }
                return Redirect("~/master/jobportal");
            }
            catch (Exception e)
            {
                return Redirect("~/auth/error");
            }
        }


//################################################################### Sub menu Client add, delete,update ##################################################

        //---------------------------------------------------------- view of client -------------------------------------------------------------------
        [Route("master/client")]
        public ActionResult Client()
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    // prepare data clients for view
                    List<ClientDTO> ListClient = db.TB_CLIENT.Select(c => new ClientDTO {
                        ID = c.ID,
                        CLIENT_ID = c.CLIENT_ID,
                        CLIENT_NAME = c.CLIENT_NAME,
                        CLIENT_ADDRESS = c.CLIENT_ADDRESS,
                        CLIENT_OTHERADDRESS = c.CLIENT_OTHERADDRESS,
                        CLIENT_INDUSTRIES = c.CLIENT_INDUSTRIES,
                    }).ToList();
                    //set data to show in view
                    ViewBag.DataView = new Dictionary<string, string>()
                    {
                        {"title","Job Portal" }
                    };

                    return View("Client/Index", ListClient);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }


        //-------------------------------------------------------- for add client ----------------------------------------------------
        [Route("master/client/add")]
        public ActionResult ClientAdd(ClientDTO DataNewClient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        // generate id client
                        string ClientId = "CLA" + DateTime.Now.ToString("fffff");

                        //prosess to insert data 
                        db.TB_CLIENT.Add(new TB_CLIENT
                        {
                            CLIENT_ID = ClientId,
                            CLIENT_NAME = DataNewClient.CLIENT_NAME,
                            CLIENT_ADDRESS = DataNewClient.CLIENT_ADDRESS,
                            CLIENT_OTHERADDRESS = DataNewClient.CLIENT_OTHERADDRESS,
                            CLIENT_INDUSTRIES = DataNewClient.CLIENT_INDUSTRIES,
                        });
                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Client added Successfully");
                            TempData.Add("type", "success");
                        }

                        else
                        {
                            TempData.Add("message", "New Client failed to added");
                            TempData.Add("type", "danger");
                        }
                    }
                    return Redirect("~/master/client");
                }
                TempData.Add("message", "Please complete the form add");
                TempData.Add("type", "danger");
                return Redirect("~/master/client");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------ for edit client ---------------------------------------------------------
        [Route("master/client/edit")]
        public ActionResult ClientEdit(ClientDTO DataEditClient)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    
                    if (ModelState.IsValid)
                    {
                        //get data from table clirnt base on data of parameter and
                        TB_CLIENT Tb_Client = db.TB_CLIENT.FirstOrDefault(c => c.ID == DataEditClient.ID);
                        Tb_Client.CLIENT_NAME = DataEditClient.CLIENT_NAME;
                        Tb_Client.CLIENT_ADDRESS = DataEditClient.CLIENT_ADDRESS;
                        Tb_Client.CLIENT_OTHERADDRESS = DataEditClient.CLIENT_OTHERADDRESS;
                        Tb_Client.CLIENT_INDUSTRIES = DataEditClient.CLIENT_INDUSTRIES;

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Client edit Successfully");
                            TempData.Add("type", "success");
                        }

                        else
                        {
                            TempData.Add("message", "New Client failed to edit");
                            TempData.Add("type", "danger");
                        }
                        return Redirect("~/master/client");
                    }
                    TempData.Add("message","Please Complete the form edit");
                    TempData.Add("type", "danger");
                    return Redirect("~/master/client");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
    }
}

        //-------------------------------------------------------- for delete client ----------------------------------------------------
        [Route("master/client/delete/{id?}")]
        public ActionResult Clientdelete(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
            {

                //prosess to insert data 
                if (id == null)
                {
                    return Redirect("~/master/client");
                }
                //convert id to int for search client in tble

                int ClientId = Convert.ToInt16(id);
                TB_CLIENT DataClient = db.TB_CLIENT.FirstOrDefault(c => c.ID == ClientId);


                if (DataClient == null)
                {
                    return Redirect("~/master/client");
                }

                //if client is already the remove it
                else
                {
                    db.TB_CLIENT.Remove(DataClient);
                    //check prosses success or not
                    if (db.SaveChanges() > 0)
                    {
                        TempData.Add("message", "Job Portal have been deleted");
                        TempData.Add("type", "success");
                    }

                    else
                    {
                        TempData.Add("message", "Job Portal failed to deleted");
                        TempData.Add("type", "danger");
                    }

                    return Redirect("~/master/client");
                }
            }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

//################################################################### Sub Menu User Management (user) ####################################################

       //-------------------------------------------------------------- view for user management sub menu 

        [Route("master/usermanagement")]
        public ActionResult User()
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {
                    //prepare data user dto for view

                    List<UserDTO> ListUser = db.TB_USER.Select(u =>
                        new UserDTO
                        {
                            USER_ID = u.USER_ID,
                            FULL_NAME = u.FULL_NAME,
                            USERNAME = u.USERNAME,
                            PASSWORD = u.PASSWORD,
                            ROLE_ID = u.ROLE_ID,
                            ROLE_NAME = db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == u.ROLE_ID).ROLE_NAME
                        }
                    ).ToList();

                    //return view and send with list users
                    return View("User/Index", ListUser);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }










        // --------------------------------------------------------------------Sub Menu Add User-------------------------------------------------------
        [Route("master/usermanagement/add")]
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
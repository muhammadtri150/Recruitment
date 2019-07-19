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
using FinalProject.Utils;

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

                            UserLogingUtils.SaveLoggingUserActivity("add new job portal "+ JobPortalDTO.JOBPORTAL_NAME);
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

                            UserLogingUtils.SaveLoggingUserActivity("edit job portal " + JobPortalDTO.JOBPORTAL_NAME);
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
                            UserLogingUtils.SaveLoggingUserActivity("remove job portal " + Tb_JobPortal.JOBPORTAL_NAME);
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
                        {"title","Client" }
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
                            UserLogingUtils.SaveLoggingUserActivity("add new client "+ClientId);
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
                            UserLogingUtils.SaveLoggingUserActivity("edit client " + Tb_Client.CLIENT_ID);
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
                        UserLogingUtils.SaveLoggingUserActivity("delete client " + DataClient.CLIENT_ID);
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

       //-------------------------------------------------------------- view for user management sub menu -------------------------------

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
                    ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","User Management"},
                        {"ListRole",db.TB_ROLE.Select(r => new RoleDTO{ ROLE_NAME = r.ROLE_NAME, ROLE_ID = r.ROLE_ID}).ToList() }

                    };
                    return View("User/Index", ListUser);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //--------------------------------------------------------------- add new user -----------------------------------------------
        [Route("master/usermanagement/add")]
        public ActionResult UserAdd(UserDTO DataNewUser)
        {
            try
            {
                var d = DataNewUser;
                var a = "S";
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //prepare data new user
                        //password user is encrypte with algo sha256
                        TB_USER Tb_User = new TB_USER
                        {
                            FULL_NAME = DataNewUser.FULL_NAME,
                            USERNAME = DataNewUser.USERNAME,
                            PASSWORD = CryptographyUtils.Encrypt(DataNewUser.PASSWORD),
                            ROLE_ID = DataNewUser.ROLE_ID,
                            EMAIL = DataNewUser.EMAIL
                        };

                        db.TB_USER.Add(Tb_User);

                        //check prosses insert data 
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New User Successfully to added");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Add new user " + Tb_User.USERNAME);
                        }
                        else
                        {
                            TempData.Add("message", "User failed to Added");
                            TempData.Add("type", "danger");
                        } 
                    }
                    return Redirect("~/master/usermanagement");
                }
                TempData.Add("message", "Please Complete the form add User");
                TempData.Add("type", "danger");
                return Redirect("~/master/usermanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        // -------------------------------------------------------------- Edit User -------------------------------------------------------
        [Route("master/usermanagement/edit")]
        public ActionResult UserEdit(UserDTO DataEditUser)
        {
            try
            {
                ModelState.Remove("CONFIRM_PASSWORD");
                using (DBEntities db = new DBEntities())
                {

                    if (ModelState.IsValid)
                    {
                        //get data from table user base on data of parameter and change the data directly
                        TB_USER Tb_User = db.TB_USER.FirstOrDefault(u => u.USER_ID == DataEditUser.USER_ID);
                        Tb_User.FULL_NAME = DataEditUser.FULL_NAME;
                        Tb_User.USERNAME = DataEditUser.USERNAME;
                        Tb_User.PASSWORD = CryptographyUtils.Encrypt(DataEditUser.PASSWORD);
                        Tb_User.EMAIL = DataEditUser.EMAIL;
                        Tb_User.ROLE_ID = DataEditUser.ROLE_ID;

                        //check proses update
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Edit User Successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("edit user id " + Tb_User.USER_ID);
                        }

                        else
                        {
                            TempData.Add("message", "User failed to edit");
                            TempData.Add("type", "danger");
                        }
                        return Redirect("~/master/usermanagement");
                    }
                    TempData.Add("message", "Please Complete the form edit");
                    TempData.Add("type", "danger");
                    return Redirect("~/master/usermanagement");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
        //------------------------------------------------------ Delete User ----------------------------------------------------------
        [Route("master/usermanagement/delete/{id?}")]
        public ActionResult UserDelete(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {

                    //prosess to delete data 
                    if (id == null)
                    {
                        return Redirect("~/master/usermanagement");
                    }
                    //convert id to int for search client in tble

                    int UserId = Convert.ToInt16(id);
                    TB_USER DataUser = db.TB_USER.FirstOrDefault(u => u.USER_ID == UserId);


                    if (DataUser == null)
                    {
                        return Redirect("~/master/usermanagement");
                    }

                    //if client is already the remove it
                    else
                    {
                        db.TB_USER.Remove(DataUser);
                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "User have been deleted");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("delete user id " + DataUser.USER_ID);
                        }

                        else
                        {
                            TempData.Add("message", "User failed to deleted");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/usermanagement");
                    }
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

 //################################################################## Job Position Management ######################################################

        [Route("master/jobpositionmanagement")]
        public ActionResult JobPositionManagement()
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {
                    List<JobPositionDTO> ListJobPosition = db.TB_JOB_POSITION.Select(j => new JobPositionDTO
                    {
                        JOBPOSITION_ID = j.JOBPOSITION_ID,
                        JOBPOSITION_NAME = j.JOBPOSITION_NAME,
                        JOBPOSITION_NOTE = j.JOBPOSITION_NOTE
                    }).ToList();

                    //prepare data for show at view
                    ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","Job Position Management"}
                    };

                    return View("JobPositionManagement/index",ListJobPosition);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------- job position add ------------------------------------------
        [Route("master/jobpositionmanagement/add")]
        public ActionResult JobPositionManagementAdd(JobPositionDTO DataJobPosition)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if (ModelState.IsValid)
                    {
                        db.TB_JOB_POSITION.Add(new TB_JOB_POSITION
                        {
                            JOBPOSITION_NAME = DataJobPosition.JOBPOSITION_NAME,
                            JOBPOSITION_NOTE = DataJobPosition.JOBPOSITION_NOTE
                        });

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Job Position added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("delete user id " + DataJobPosition.JOBPOSITION_NAME);
                        }

                        else
                        {
                            TempData.Add("message", "New Job Position failed to added");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/jobpositionmanagement");
                    }
                    TempData.Add("message", "please complete form add job position");
                    TempData.Add("type", "danger");
                    return Redirect("~/master/jobpositionmanagement");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //--------------------------------------------------- job position edit --------------------------------------------
        [Route("master/jobpositionmanagement/edit")]
        public ActionResult JobPositionManagementEdit(JobPositionDTO DataJobPosition)
        {
            //try
            //{
                using (DBEntities db = new DBEntities())
                {
                    if (ModelState.IsValid)
                    {
                        TB_JOB_POSITION Tb_Job_Position = db.TB_JOB_POSITION.FirstOrDefault(j => j.JOBPOSITION_ID == DataJobPosition.JOBPOSITION_ID);

                        Tb_Job_Position.JOBPOSITION_NAME = DataJobPosition.JOBPOSITION_NAME;
                        Tb_Job_Position.JOBPOSITION_NOTE = DataJobPosition.JOBPOSITION_NOTE;

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Job Position edit successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("delete user id " + DataJobPosition.JOBPOSITION_NAME);
                        }

                        else
                        {
                            TempData.Add("message", "Job Position failed to edit");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/jobpositionmanagement");
                    }
                    TempData.Add("message", "please complete form edit job position");
                    TempData.Add("type", "danger");
                    return Redirect("~/master/jobpositionmanagement");
                }
            //}
            //catch (Exception)
            //{
            //    return Redirect("~/auth/error");
            //}
        }

        //--------------------------------------------------- job position Delete --------------------------------------------
        [Route("master/jobpositionmanagement/delete/{id?}")]
        public ActionResult JobPositionManagementDelete(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if (id == null)
                    {
                        return Redirect("~/master/usermanagement");
                    }
                    //convert id to int for search client in tble

                    int JobId = Convert.ToInt16(id);

                    TB_JOB_POSITION Tb_Job_Position = db.TB_JOB_POSITION.FirstOrDefault(j => j.JOBPOSITION_ID == JobId);
                    db.TB_JOB_POSITION.Remove(Tb_Job_Position);

                        //check prosses success or not
                    if (db.SaveChanges() > 0)
                    {
                        TempData.Add("message", "New Job Position delete successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("delete user id " + Tb_Job_Position.JOBPOSITION_ID);
                    }

                    else
                    {
                        TempData.Add("message", "Job Position failed to delete");
                        TempData.Add("type", "danger");
                    }

                        return Redirect("~/master/jobpositionmanagement");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
    }
}
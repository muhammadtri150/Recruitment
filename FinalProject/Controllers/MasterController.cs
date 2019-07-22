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
                ModelState.Remove("PASSWORD");
                using (DBEntities db = new DBEntities())
                {

                    if (ModelState.IsValid)
                    {
                        //get data from table user base on data of parameter and change the data directly
                        TB_USER Tb_User = db.TB_USER.FirstOrDefault(u => u.USER_ID == DataEditUser.USER_ID);
                        Tb_User.FULL_NAME = DataEditUser.FULL_NAME;
                        Tb_User.USERNAME = DataEditUser.USERNAME;
                        if (DataEditUser.PASSWORD != null) Tb_User.PASSWORD = CryptographyUtils.Encrypt(DataEditUser.PASSWORD);
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

 //################################################################ Skill Management #################################################

        [Route("master/skillmanagement")]
        public ActionResult Skill()
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {
                    //prepare data skill
                    List<SkillDTO> ListSkill = db.TB_SKILL.Select(s => new SkillDTO
                    {
                        SKILL_ID = s.SKILL_ID,
                        SKILL_NAME = s.SKILL_NAME
                    }).ToList();

                    ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","Skill Management" }
                    };

                    return View("SkillManagement/Index", ListSkill);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- add skill --------------------------------------------------
        [Route("master/skillmanagement/add")]
        public ActionResult SkillAdd(SkillDTO DataNewSkill)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //prepare data skill and insert to database directly
                        db.TB_SKILL.Add(new TB_SKILL
                        {
                            SKILL_NAME = DataNewSkill.SKILL_NAME
                        });

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Skill saddedd successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Add skill " + DataNewSkill.SKILL_NAME);
                        }

                        else
                        {
                            TempData.Add("message", "New skill failed to added");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/skillmanagement");
                    }
                }
                TempData.Add("message", "Please complete form add skill");
                TempData.Add("type", "danger");
                return Redirect("~/master/skillmanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- edit skill --------------------------------------------------
        [Route("master/skillmanagement/edit")]
        public ActionResult SkillEdit(SkillDTO DataSkill)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //prepare data skill and update

                        TB_SKILL Tb_Skill = db.TB_SKILL.FirstOrDefault(s => s.SKILL_ID == DataSkill.SKILL_ID);

                        Tb_Skill.SKILL_NAME = DataSkill.SKILL_NAME;

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Skill edit successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit skill id " + DataSkill.SKILL_NAME);
                        }

                        else
                        {
                            TempData.Add("message", "skill failed to edit");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/skillmanagement");
                    }
                }
                TempData.Add("message", "Please complete form add skill");
                TempData.Add("type", "danger");
                return Redirect("~/master/skillmanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- delete skill --------------------------------------------------
        [Route("master/skillmanagement/delete/{id?}")]
        public ActionResult SkillDelete(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if(id == null)
                    {
                        return Redirect("~/master/skillmanagement");
                    } 

                    //Prepare data table skill and then remove directly while instance data skill
                    int SkillId = Convert.ToInt16(id);
                    TB_SKILL Tb_Skill = db.TB_SKILL.FirstOrDefault(s => s.SKILL_ID == SkillId);
                    db.TB_SKILL.Remove(Tb_Skill);

                    //ceck data is already or not
                    if (Tb_Skill == null)
                    {
                        return Redirect("~/master/skillmanagement");
                    }

                    //check prosses success or not
                    if (db.SaveChanges() > 0)
                     {
                            TempData.Add("message", "Skill delete successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Delete skill id " + SkillId);
                     }

                     else
                     {
                            TempData.Add("message", "skill failed to delete");
                            TempData.Add("type", "danger");
                     }

                     return Redirect("~/master/skillmanagement");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //############################################### access menu management and role management ######################################

        [Route("master/rolemanagement")]
        public ActionResult RoleManagement()
        {
            try
            {
               using(DBEntities db = new DBEntities()){

                    //prepare daa view by access menu DTO, dto and model wil deffrent structure because for show at view
                    //get data from tb menu, tb user and tb access menu
                    List<MenuDTO> ListMenu = db.TB_MENU.Select(m => new MenuDTO{
                        MENU_ID = m.MENU_ID,
                        TITLE_MENU = m.TITLE_MENU,
                        LOGO_MENU = m.LOGO_MENU
                    }).ToList();

                    List<RoleDTO> ListRole = db.TB_ROLE.Select(r => new RoleDTO
                    {
                        ROLE_ID = r.ROLE_ID,
                        ROLE_NAME = r.ROLE_NAME
                    }).ToList();


                    //add list for view in viewbag for check box
                    ViewBag.DataView = new Dictionary<string, object>() {
                        {"title","Role Management"},
                        {"ListMenu", ListMenu}
                    };

                    return View("RoleManagement/Index", ListRole);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------------------- add role ----------------------------------------------------
        [Route("master/rolemanagement/add")]
        //--- this proccess for add role to tb_role but receive parameter accessmenudto because there data is needed from that dto ok
        public ActionResult RoleAdd(AccessMenuDTO DataNewRole)
        {
            try
            {
                ModelState.Remove("MENU_ID");

                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //prepare data new role and insert to database directly
                        db.TB_ROLE.Add(new TB_ROLE
                        {
                            ROLE_NAME = DataNewRole.ROLE_NAME
                        });

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Role added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Add new Role " + DataNewRole.ROLE_NAME);
                        }

                        else
                        {
                            TempData.Add("message", "New role failed to added");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/rolemanagement");
                    }
                }
                TempData.Add("message", "Please complete form add role");
                TempData.Add("type", "danger");
                return Redirect("~/master/rolemanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------------------- edit role ----------------------------------------------------
        [Route("master/rolemanagement/edit")]
        //--- this proccess for add role to tb_role but receive parameter accessmenudto because there data is needed from that dto ok
        public ActionResult RoleEdit(AccessMenuDTO DataRole)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //prepare data rolw and update

                        TB_ROLE Tb_Role = db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == DataRole.ROLE_ID);

                        Tb_Role.ROLE_NAME = DataRole.ROLE_NAME;

                        //check prosses success or not
                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Role edit successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit Role id " + Tb_Role.ROLE_ID);
                        }

                        else
                        {
                            TempData.Add("message", "Role failed to edit");
                            TempData.Add("type", "danger");
                        }

                        return Redirect("~/master/rolemanagement");
                    }
                }
                TempData.Add("message", "Please complete form edit role");
                TempData.Add("type", "danger");
                return Redirect("~/master/rolemanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------------------- DELETE role ----------------------------------------------------
        [Route("master/rolemanagement/delete/{id?}")]
        //--- this proccess for add role to tb_role but receive parameter accessmenudto because there data is needed from that dto ok
        public ActionResult RoleDelete(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    //convert id to string for sort data role
                    int Role_Id = Convert.ToInt16(id);

                    //if delete role, that mean data row in tb access menu base of role id deleted will be deleted to
                    //looping for remove data access menu based on id role
                    foreach (var data in db.TB_ACCESS_MENU.Where(ac => ac.ROLE_ID == Role_Id).ToList())
                    {
                        db.TB_ACCESS_MENU.Remove(data);
                    }
                    //remove role
                    db.TB_ROLE.Remove(db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == Role_Id));
                        //check prosses success or not
                    if (db.SaveChanges() > 0)
                    {
                        TempData.Add("message", "Role delete successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("Edit Role id " + Role_Id);
                    }

                    else
                    {
                        TempData.Add("message", "Role failed to delete");
                        TempData.Add("type", "danger");
                    }

                    return Redirect("~/master/rolemanagement");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //----------------------------------------------------- for view access menu -------------------------------------------
        [Route("master/rolemanagement/accessmenu/{id?}")]
        public ActionResult AccessManagement(string id = null)
        {
            try
            {
                if(id == null)
                {
                    return Redirect("~/master/rolemanagement/");
                }
                else
                {
                    using(DBEntities db = new DBEntities())
                    {
                        List<MenuDTO> List_Menu = db.TB_MENU.Select(m => new MenuDTO
                        {
                            MENU_ID = m.MENU_ID,
                            TITLE_MENU = m.TITLE_MENU,
                            LOGO_MENU = m.LOGO_MENU
                        }).ToList();

                        int RoleId = Convert.ToInt16(id);
                        ViewBag.DataView = new Dictionary<string, object>()
                        {
                            {"title", "Access Menu Management"},
                            {"RoleId",RoleId},
                            //get data sub menu where menu is candidate (menu id candidate = 7)
                            {"SubMenuCandidate", db.TB_SUBMENU.Select(sm => new SubMenuDTO{
                                                            MENU_ID = sm.MENU_ID,
                                                            SUB_MENU_ID = sm.SUB_MENU_ID,
                                                            TITLE_MENU = sm.TITLE_SUBMENU,
                                                            URL = sm.URL,
                                                            LOGO_SUBMENU = sm.LOGO_SUBMENU
                                                        }).Where(sm => sm.MENU_ID == 7).ToList()},
                            //get data state candidate 
                            {"StateCandidate",db.TB_STATE_CANDIDATE.Select(sc => new StateCandidateDTO{
                                                             ID = sc.ID,
                                                             STATE_NAME = sc.STATE_NAME
                                                         }).ToList() },
                            //get data action candidate (create,update,delete,read)
                            {"ActionCandidate",db.TB_ACTION_CANDIDATE.Select(ac => new ActionCandidateDTO{
                                                              ID = ac.ID,
                                                              ACTION_NAME = ac.ACTION_NAME
                                                          }).ToList() }
                        };

                        return View("RoleManagement/AccessMenu", List_Menu);
                    }
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------------ PROCESS ACCESS ACTION MENU CANDIDATE ------------------------------------
        [Route("master/rolemanagement/accesscandidateprocess")]
        public int AccessMenuCandidate(UserAccessMenuCandidateDTO Data)
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {
                    TB_USER_ACCESS_MENU_CANDIDATE TbAccessMenuCandidate =
                        db.TB_USER_ACCESS_MENU_CANDIDATE.FirstOrDefault(d =>
                            d.SUB_MENU_CANDIDATE_ID == Data.SUB_MENU_CANDIDATE_ID &&
                            d.ROLE_ID == Data.ROLE_ID &&
                            d.ACTION_CANDIDATE_ID == Data.ACTION_CANDIDATE_ID);

                    if(TbAccessMenuCandidate == null)
                    {
                        db.TB_USER_ACCESS_MENU_CANDIDATE.Add(new TB_USER_ACCESS_MENU_CANDIDATE {
                            SUB_MENU_CANDIDATE_ID = Data.SUB_MENU_CANDIDATE_ID,
                            ROLE_ID = Data.ROLE_ID,
                            ACTION_CANDIDATE_ID = Data.ACTION_CANDIDATE_ID
                        });
                    }
                    else
                    {
                        db.TB_USER_ACCESS_MENU_CANDIDATE.Remove(TbAccessMenuCandidate);
                    }
                    
                    return db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //------------------------------------------------------ for edit access process add or delete ------------------------------------
        [Route("master/rolemanagement/accessmenu/proccess")]
        public int AccessManagement(AccessMenuDTO DataAccess)
        {
            try
            {
                    using(DBEntities db = new DBEntities())
                    {
                        //check availablity of access menu, delete if already and add if not there ok
                        TB_ACCESS_MENU Tb_Access_Menu = db.TB_ACCESS_MENU.FirstOrDefault(ac =>
                            ac.MENU_ID == DataAccess.MENU_ID && ac.ROLE_ID == DataAccess.ROLE_ID
                        );
                        
                        if(Tb_Access_Menu != null)
                        {
                            db.TB_ACCESS_MENU.Remove(Tb_Access_Menu);
                        }
                        else
                        {
                            // if data is not already that mean insert deata access
                            db.TB_ACCESS_MENU.Add(new TB_ACCESS_MENU { ROLE_ID = DataAccess.ROLE_ID, MENU_ID = DataAccess.MENU_ID });
                        }
                    
                
                        return db.SaveChanges();
                    }
            }
            catch (Exception)
            {
                return 0;
            }
        }

 //########################################################## Sub Menu Management ##############################################################
        [Route("master/submenumanagement")]
        public ActionResult SubMenu()
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    List<SubMenuDTO> ListSubMenu = db.TB_SUBMENU.Select(sm => new SubMenuDTO {
                        MENU_ID = sm.MENU_ID,
                        TITLE_SUBMENU = sm.TITLE_SUBMENU,
                        LOGO_SUBMENU = sm.LOGO_SUBMENU,
                        URL = sm.URL,
                        SUB_MENU_ID = sm.SUB_MENU_ID,
                        TITLE_MENU = db.TB_MENU.FirstOrDefault(menu => menu.MENU_ID == sm.MENU_ID).TITLE_MENU
                    }).ToList();
                    
                    ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","Submenu Management"},
                        {"ListMenu",db.TB_MENU.Select(m => new MenuDTO{  MENU_ID = m.MENU_ID, TITLE_MENU = m.TITLE_MENU }).ToList() }
                    };

                    return View("SubMenuManagement/Index",ListSubMenu);
                }    
            }
            catch(Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //--------------------------------------------------------- Add Sub Menu -----------------------------------------------------
        [Route("master/submenumanagement/add")]
        public ActionResult SubMenuAdd(SubMenuDTO DataNewSubMenu)
        {
            try
            {
                ModelState.Remove("SUB_MENU_ID");
                ModelState.Remove("SUB_MENU_ID");
                if (ModelState.IsValid)
                {
                   using(DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data new sub menu------------------------------
                        db.TB_SUBMENU.Add(new TB_SUBMENU {
                            MENU_ID = DataNewSubMenu.MENU_ID,
                            TITLE_SUBMENU = DataNewSubMenu.TITLE_SUBMENU,
                            LOGO_SUBMENU = DataNewSubMenu.LOGO_SUBMENU,
                            URL = DataNewSubMenu.URL
                        });

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Sub menu added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Add new Sub Menu " + DataNewSubMenu.TITLE_SUBMENU + " in menu id " + DataNewSubMenu.MENU_ID);
                        }

                        else
                        {
                            TempData.Add("message", "New sub menu failed to added");
                            TempData.Add("type", "danger");
                        }
                    }
                    return Redirect("~/master/submenumanagement");
                }
                TempData.Add("message", "Please complete form add sub menu");
                TempData.Add("type", "danger");
                return Redirect("~/master/submenumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //--------------------------------------------------------- Edit Sub MENU ----------------------------------------------------
        [Route("master/submenumanagement/edit")]
        public ActionResult SubMenuEdit(SubMenuDTO DataSubMenu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data new sub menu------------------------------
                        TB_SUBMENU Tb_SubMenu = db.TB_SUBMENU.FirstOrDefault(sm => sm.SUB_MENU_ID == DataSubMenu.SUB_MENU_ID);
                        Tb_SubMenu.TITLE_SUBMENU = DataSubMenu.TITLE_SUBMENU;
                        Tb_SubMenu.LOGO_SUBMENU = DataSubMenu.LOGO_SUBMENU;
                        Tb_SubMenu.URL = DataSubMenu.URL;
                        Tb_SubMenu.MENU_ID = DataSubMenu.MENU_ID;

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "New Sub menu added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit Sub Menu " + DataSubMenu.TITLE_SUBMENU + " in menu id " + DataSubMenu.MENU_ID);
                        }

                        else
                        {
                            TempData.Add("message", "sub menu failed to edit");
                            TempData.Add("type", "danger");
                        }
                    }
                    return Redirect("~/master/submenumanagement");
                }
                TempData.Add("message", "Please complete form edit sub menu");
                TempData.Add("type", "danger");
                return Redirect("~/master/submenumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //--------------------------------------------------------- Delete Sub MENU ----------------------------------------------------
        [Route("master/submenumanagement/delete/{id?}")]
        public ActionResult SubMenuDelete(string id = null)
        {
            try
            {
                if(id == null)
                {
                    return Redirect("~/master/submenumanagement");
                }

                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data new sub menu------------------------------

                        int SubMenuId = Convert.ToInt16(id);
                        TB_SUBMENU Tb_SubMenu = db.TB_SUBMENU.FirstOrDefault(sm => sm.SUB_MENU_ID == SubMenuId);

                        if(Tb_SubMenu == null)
                        {
                            return Redirect("~/master/submenumanagement");
                        }

                        else
                        {
                            db.TB_SUBMENU.Remove(Tb_SubMenu);

                            if (db.SaveChanges() > 0)
                            {
                                TempData.Add("message", "New Sub menu DELETE successfully");
                                TempData.Add("type", "success");
                                UserLogingUtils.SaveLoggingUserActivity("Edit Sub Menu " + SubMenuId + " in menu id " + Tb_SubMenu.MENU_ID);
                            }

                            else
                            {
                                TempData.Add("message", "sub menu failed to DELETE");
                                TempData.Add("type", "danger");
                            }
                        }
                    }
                }
                return Redirect("~/master/submenumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
        //####################################################################### Menu Management #####################################
        [Route("master/menumanagement")]
        public ActionResult MenuManagement()
        {
            try
            {
                using(DBEntities db = new DBEntities())
                {

                    List<MenuDTO> ListMenu = db.TB_MENU.Select(m => new MenuDTO
                    {
                        MENU_ID = m.MENU_ID,
                        TITLE_MENU = m.TITLE_MENU,
                        LOGO_MENU = m.LOGO_MENU
                    }).ToList();

                    ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","Menu Management" }
                    };

                    return View("MenuManagement/Index", ListMenu);

                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
        

        //---------------------------------------------------- for edit menu -----------------------------------------------------
        [Route("master/menumanagement/edit")]
        public ActionResult MenuEdit(MenuDTO DataMenu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data menu------------------------------
                        TB_MENU Tb_Menu = db.TB_MENU.FirstOrDefault(m => m.MENU_ID == DataMenu.MENU_ID);
                        Tb_Menu.TITLE_MENU = DataMenu.TITLE_MENU;
                        Tb_Menu.LOGO_MENU = DataMenu.LOGO_MENU;

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Menu edit successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit Menu ID " + DataMenu.MENU_ID);
                        }

                        else
                        {
                            TempData.Add("message", "Menu failed to edit");
                            TempData.Add("type", "danger");
                        }
                    }
                    return Redirect("~/master/menumanagement");
                }
                TempData.Add("message", "Please complete form edit menu");
                TempData.Add("type", "danger");
                return Redirect("~/master/menumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------- for add menu -----------------------------------------------------
        [Route("master/menumanagement/add")]
        public ActionResult MenuAdd(MenuDTO DataNewMenu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data menu and insert to database directly ------------------------------

                        db.TB_MENU.Add(new TB_MENU { TITLE_MENU = DataNewMenu.TITLE_MENU, LOGO_MENU = DataNewMenu.LOGO_MENU });

                        if (db.SaveChanges() > 0)
                        {
                            TempData.Add("message", "Menu edit successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Add Menu " + DataNewMenu.TITLE_MENU);
                        }

                        else
                        {
                            TempData.Add("message", "Menu failed to Add");
                            TempData.Add("type", "danger");
                        }
                    }
                    return Redirect("~/master/menumanagement");
                }
                TempData.Add("message", "Please complete form edit menu");
                TempData.Add("type", "danger");
                return Redirect("~/master/menumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------- delete menu -------------------------------------------------------
        [Route("master/menumanagement/delete/{id?}")]
        public ActionResult MenuDelete(string id = null)
        {
            try
            {
                if (id == null)
                {
                    return Redirect("~/master/menumanagement");
                }

                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        //----------------------------------- prepare data new sub menu------------------------------

                        int MenuId = Convert.ToInt16(id);
                        TB_MENU Tb_Menu = db.TB_MENU.FirstOrDefault(m => m.MENU_ID == MenuId);

                        if (Tb_Menu == null)
                        {
                            return Redirect("~/master/submenumanagement");
                        }

                        else
                        {
                            db.TB_MENU.Remove(Tb_Menu);

                            if (db.SaveChanges() > 0)
                            {
                                TempData.Add("message", "New Sub menu DELETE successfully");
                                TempData.Add("type", "success");
                                UserLogingUtils.SaveLoggingUserActivity("delete Menu " + MenuId);
                            }

                            else
                            {
                                TempData.Add("message", "Menu failed to DELETE");
                                TempData.Add("type", "danger");
                            }
                        }
                    }
                }
                return Redirect("~/master/submenumanagement");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
    }
}
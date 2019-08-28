using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Controllers;
using FinalProject.DTO;
using FinalProject.Filters;
using FinalProject.Utils;
using FinalProject.Models;
using System.Configuration;

namespace FinalProject.Controllers
{
    [UserAccessCandidateFilter]
    public class CandidateController : Controller
    {
        DBEntities db = new DBEntities();
        [Route("candidate")]
        public ActionResult Index()
        {
            return Redirect("~/candidate/preselection");
        }
        //################################################ Sub Menu Candidate Preselection ################################# 

        //********************************************************** Manage Data Candidate **********************************************************

        //------------------------------------------------------- for view candidate preselection -----------------------------------------------
        [Route("candidate/praselection/read/{i?}")]
        public ActionResult CandidatePreselection(string i = null)
        {
            //try
            //{
            //    //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //not  : data in this view especialy for candidate where state_id is 1,10 or 11 (state in step preselection)

                //formula pagination
                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 1).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 1 || d.CANDIDATE_STATE == 10 || d.CANDIDATE_STATE == 11).Skip(idx).Take(perPage).ToList();

                //---------------------------- prepare data viewbag --------------------


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);
                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).Skip(idx).Take(5).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));

                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.CANDIDATE_STATE == 1 || d.CANDIDATE_STATE == 10 || d.CANDIDATE_STATE == 11)).Skip(idx).Take(5).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                            d.CANDIDATE_EMAIL.Contains(Keyword) ||
                            d.CANDIDATE_NAME.Contains(Keyword) ||
                            d.CANDIDATE_PHONE.Contains(Keyword) &&
                            (d.CANDIDATE_STATE == 1 || d.CANDIDATE_STATE == 10 || d.CANDIDATE_STATE == 11)).Skip(idx).Take(5).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "" && (StateId == 0 && Position == "all" && Keyword == ""))
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                    d.CANDIDATE_STATE == 1 || d.CANDIDATE_STATE == 10 || d.CANDIDATE_STATE == 11).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);
                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.CANDIDATE_STATE == 1 || d.CANDIDATE_STATE == 10 || d.CANDIDATE_STATE == 11)).Skip(idx).Take(dt).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Praselection"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 1)},
                    {"PageCount",PageCount}
                    };
                //return view
                return View("Preselection/Index", ListCandidate);
            //}
            //catch (Exception)
            //{
            //    return Redirect("~/auth/error");
            //}
        }

        //----------------------------------------------------------- view form add new candidate -----------------------------------------
        [Route("candidate/praselection/create/candidate")]
        public ActionResult CandidatePreselectionAdd()
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {

                    ViewBag.DataView = new Dictionary<string, object>(){
                    {"title","Praselection"}
                    };
                    return View("Preselection/AddCandidate");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- View Detail candidate ------------------------------------------------
        [Route("candidate/praselection/read/detailcandidate/{id?}")]
        public ActionResult DetailCandidate(string id = null)
        {
            try
            {
                using (DBEntities db = new DBEntities())
            {
                if (id == null) return Redirect("~/candidate/praselection");

                int candidateId = Convert.ToInt16(id);

                DetailCandidateDTO DataDetail = Manage_DetailCandidate.GetData(candidateId);

                if (DataDetail == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                    {
                        {"title","Praselection"}
                    };

                return View("Preselection/DetailCandidate", DataDetail);
            }

            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //-------------------------------------------------- PROCESS ADD NEW CANDIDATE --------------------------------------
        [Route("candidate/praselection/create/candidate/process")]
        public ActionResult CandidatePreselectionAdd(CandidateDTO DataNewCandidate, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //process add will return list object, [0] is return from db.saveCahnge() and [1] return candidate_id (CA******)
                    var ProcessAdd = Manage_CandidateDTO.AddData(DataNewCandidate, Pict, Cv);

                    if (Convert.ToInt16(ProcessAdd[0]) > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "New Candidate added successfully");
                        TempData.Add("type", "success");

                        UserLogingUtils.SaveLoggingUserActivity("add new Candidate" + Convert.ToString(ProcessAdd[1]));
                        }
                    }
                    else
                    {
                        if (TempData["message"] == null)
                        {
                            TempData.Add("message", "New Candidate failed to add");
                            TempData.Add("type", "warning");
                        }
                    }
                    return Redirect("~/candidate/praselection/read");
                }

                ViewBag.DataView = new Dictionary<string, object>(){
                    {"title","Praselection"}
                    };
                return View("Preselection/AddCandidate");
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }

        //------------------------------------------ VIEW EDIT CANDIDATE ---------------------------------------------------
        [Route("candidate/praselection/update/candidate/{id?}")]
        public ActionResult CandidateEdit(string id = null)
        {
            try
            {
                if (id == null) return Redirect("~/candidate/praselection");

                int CandidateId = Convert.ToInt16(id);
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == CandidateId);

                if (DataCandidate == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","praselection"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 2 || d.ID == 1 ).ToList() }
                };

                return View("Preselection/EditCandidate", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }



        //------------------------------------------ Process Edit Data Candidate -------------------------------------------------
        [Route("candidate/praselection/update/candidate/process")]
        public ActionResult CandidateEdit(CandidateDTO Data, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                    if (ProcessEdit > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                        }
                    }
                    else
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                        }
                    }

                    return Redirect("~/candidate/praselection/read");
                }
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID);
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","praselection"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 2 || d.ID == 1).ToList() }
                };

                return View("Preselection/EditCandidate", DataCandidate);
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }

        //************************************************* JOB EXPERIENCE OF CANDIDATE *****************************************************


        //----------------------------------------------------- process add new job experience ------------------------------
        [Route("candidate/praselection/create/jobExp")]
        public ActionResult JobExpAdd(CandidateJobExperienceDTO NewJobExp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var ProcessAdd = Manage_CandidateJobExperienceDTO.AddData(NewJobExp);

                        if (ProcessAdd > 0)
                        {
                            if (TempData.Peek("message") == null)
                            {
                                TempData.Remove("message");
                                TempData.Remove("type");
                            TempData.Add("message", "Candidate new job experience added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("add job experience Candidate " + NewJobExp.CANDIDATE_ID + " Job Experience in " + NewJobExp.COMPANY_NAME);
                            }
                        }
                        else
                        {
                            if (TempData.Peek("message") == null)
                            {
                                TempData.Remove("message");
                                TempData.Remove("type");
                            TempData.Add("message", "Candidate new job experience failed to add");
                            TempData.Add("type", "warning");
                            }
                        }
                        return Redirect("~/candidate/praselection/read/detailcandidate/" + NewJobExp.CANDIDATE_ID);
                    }
                }

                TempData.Add("message", "Candidate new job experience failed to add please complete form add");
                TempData.Add("type", "danger");
                return Redirect("~/candidate/praselection/read/detailcandidate/" + NewJobExp.CANDIDATE_ID);
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }

        //----------------------------------------------------------- view edit job exp ------------------------------------
        [Route("candidate/praselection/update/jobExp/{id?}")]
        public ActionResult JobExp(string id = null)
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Praselection"}
                };
                int JobExpId = Convert.ToInt16(id);
                CandidateJobExperienceDTO Data = Manage_CandidateJobExperienceDTO.GetData().FirstOrDefault(d => d.ID == JobExpId);
                return View("Preselection/EditJobExp", Data);
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //----------------------------------------------------------- process edit job exp ---------------------------------
        [Route("candidate/praselection/update/jobExp/process")]
        public ActionResult JobExpEdit(CandidateJobExperienceDTO NewJobExp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var ProcessEdit = Manage_CandidateJobExperienceDTO.EditData(NewJobExp);

                        if (ProcessEdit > 0)
                        {
                            if (TempData.Peek("message") == null)
                            {
                                TempData.Remove("message");
                                TempData.Remove("type");
                            TempData.Add("message", "Candidate job experience edited successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("edit job experience Candidate " + NewJobExp.CANDIDATE_ID + " Job Experience in " + NewJobExp.COMPANY_NAME);
                            }
                        }
                        else
                        {
                            if (TempData.Peek("message") == null)
                            {
                                TempData.Remove("message");
                                TempData.Remove("type");
                            TempData.Add("message", "Candidate job experience failed to edit");
                            TempData.Add("type", "warning");
                            }
                        }
                        return Redirect("~/candidate/praselection/read/detailcandidate/" + NewJobExp.CANDIDATE_ID);
                    }
                }

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Praselection"}
                };
                CandidateJobExperienceDTO Data = Manage_CandidateJobExperienceDTO.GetData().FirstOrDefault(d => d.ID == NewJobExp.ID);
                return View("Preselection/EditJobExp", Data);

            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }











        //################################################# CANDIDATE CALL ###############################################################

        //------------------------------------------------- View for candidate call -----------------------------------------------------
        [Route("candidate/call/read/{i?}")]
        public ActionResult CandidateCall(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 2(call) or 18(called) (state in step call)

                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]); ;
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 2).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18).Skip(idx).Take(perPage).ToList();

                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------

                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);
                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                            (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "" && (StateId == 0 && Position == "all" && Keyword == ""))
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);

                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================

                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Call"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 2 || d.ID == 18)},
                    {"PageCount",PageCount}
                    };

                return View("Call/Call", ListCandidate);
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }




        //---------------------------------------------------------- View next call to called ----------------------------------------
        [Route("candidate/call/update/next/{id?}")]
        public ActionResult CallNext(string id = null)
        {
            try
            {
                if (id == null) return Redirect("~/candidate/call");

                int CandidateId = Convert.ToInt16(id);
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == CandidateId);

                if (DataCandidate == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Call"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 8 ||  d.ID == 2).ToList() }
                };

                return View("Call/EditCandidateCall", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------ Process call next to called -------------------------------------------------
        [Route("candidate/call/update/next/process")]
        public ActionResult CandidateCallNext(CandidateDTO Data, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                int ProcessEdit;

                var state = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(c => c.ID == Data.ID).CANDIDATE_STATE_ID;
                if (state != Data.CANDIDATE_STATE_ID)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var SelectHis = db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_STATE == 2 && d.CANDIDATE_ID == Data.ID);
                        //process removing state call
                        db.TB_CANDIDATE_SELECTION_HISTORY.Remove(SelectHis);
                        db.SaveChanges();

                    }
                    ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                        if (ProcessEdit > 0)
                        {
                            if (TempData["message"] == null)
                            {
                                TempData.Add("message", "Candidate Update successfully");
                                TempData.Add("type", "success");
                                UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                                //check state candidate before updated
                            }

                        }
                        else
                        {
                            if (TempData["message"] == null)
                            {
                                TempData.Add("message", "Candidate failed to Update");
                                TempData.Add("type", "warning");
                            }
                        }    

                        return Redirect("~/candidate/call/read/called");
                }

                ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                Manage_CandidateSelectionHistoryDTO.EditData(new CandidateSelectionHistoryDTO
                {
                    
                    CANDIDATE_ID = Data.ID,
                    CANDIDATE_STATE = Data.CANDIDATE_STATE_ID,
                    NOTES = Data.NOTES,
                    CANDIDATE_SOURCE = Data.SOURCE,
                    CANDIDATE_INTERVIEW_DATE = Data.CANDIDATE_INTERVIEW_DATE,
                    CANDIDATE_APPLIED_POSITION = Data.POSITION,
                    CANDIDATE_SUITABLE_POSITION = Data.SUITABLE_POSITION,
                    CANDIDATE_EXPECTED_SALARY = Data.EXPECTED_sALARY
                });

                if (ProcessEdit > 0)
                {
                     if(TempData["message"] == null)
                     {
                            TempData.Add("message", "Candidate Update successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                            //check state candidat      e before updated
                     }
                }
                else
                {
                        if (TempData["message"] == null)
                        {
                            TempData.Add("message", "Candidate failed to Update");
                            TempData.Add("type", "warning");
                        }
                }

                return Redirect("~/candidate/call/read");
            }

            CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID);

            ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Call"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 8 ||  d.ID == 2).ToList() }
                };
            return View("Call/EditCandidateCall", DataCandidate);
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }

        //------------------------------------------------- View for candidate !!! CALLED !!! ---------------------------------------------
        [Route("candidate/call/read/called/{i?}")]
        public ActionResult CandidateCalled(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 2(call) or 18(called) (state in step call)

                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 18 || sh.CANDIDATE_STATE == 8).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 8 || d.CANDIDATE_STATE == 15 || d.CANDIDATE_STATE == 17).Skip(idx).Take(perPage).ToList();


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 8)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                            (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 8)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "" && (StateId == 0 && Position == "all" && Keyword == ""))
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 8 || d.CANDIDATE_STATE == 15 || d.CANDIDATE_STATE == 17).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);
                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.CANDIDATE_STATE == 2 || d.CANDIDATE_STATE == 18)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================

                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Call"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 8)},
                    {"PageCount",PageCount}
                    };

                return View("Call/Called", ListCandidate);

            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- View next called to Interview ----------------------------------------
        [Route("candidate/call/update/called/next/{id?}")]
        public ActionResult CalledNext(string id = null)
        {
            try
            {
                if (id == null) return Redirect("~/candidate/call");

                int CandidateId = Convert.ToInt16(id);
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == CandidateId);
                DataCandidate.CANDIDATE_INTERVIEW_DATE = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().FirstOrDefault(d => d.CANDIDATE_STATE == 8 && d.CANDIDATE_ID == CandidateId).CANDIDATE_INTERVIEW_DATE;
                if (DataCandidate == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Call"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 8 ||  d.ID == 19).ToList() }
                };

                return View("Call/EditCandidateCalled", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------ Process called next to interview -------------------------------------------------
        [Route("candidate/call/update/called/next/process")]
        public ActionResult CandidateCalledNext(CandidateDTO Data, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
            {
                var ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                Manage_CandidateSelectionHistoryDTO.EditData(new CandidateSelectionHistoryDTO
                {
                    CANDIDATE_ID = Data.ID,
                    CANDIDATE_STATE = Data.CANDIDATE_STATE_ID,
                    NOTES = Data.NOTES,
                    CANDIDATE_SOURCE = Data.SOURCE,
                    CANDIDATE_INTERVIEW_DATE = Data.CANDIDATE_INTERVIEW_DATE,
                    CANDIDATE_APPLIED_POSITION = Data.POSITION,
                    CANDIDATE_SUITABLE_POSITION = Data.SUITABLE_POSITION,
                    CANDIDATE_EXPECTED_SALARY = Data.EXPECTED_sALARY
                });


                if (ProcessEdit > 0)
                {
                        if(TempData["message"] == null)
                        {
                            TempData.Add("message", "Candidate Update successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                        }
                }
                else
                {
                        if(TempData["message"] == null)
                        {
                            TempData.Add("message", "Candidate failed to Update");
                            TempData.Add("type", "warning");
                        }
                }

                    return Redirect("~/candidate/call/read/called");
            }
            CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID);
            DataCandidate.CANDIDATE_INTERVIEW_DATE = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().FirstOrDefault(d => d.CANDIDATE_STATE == 8 && d.CANDIDATE_ID == Data.ID).CANDIDATE_INTERVIEW_DATE;

            ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Call"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 8 ||  d.ID == 19).ToList() }
                };

            return View("Call/EditCandidateCalled", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }






        //************************************************************ Candidate INTERVIEW ************************************************************

        //------------------------------------------------------------ candidate interview -----------------------------------------------------------

        [Route("candidate/interview/read/{i?}")]
        public ActionResult CandidateInterview(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 19(interview process)


                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 19).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 19).Skip(idx).Take(perPage).ToList();


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == "" && DataPerPage == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == "" && DataPerPage == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.ID == 19)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all" && DataPerPage == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                       (d.ID == 19)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "" && (StateId == 0 && Position == "all" && Keyword == ""))
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 19).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);

                    }

                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.ID == 19)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================

                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Interview"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 19)},
                    {"PageCount",PageCount}
                    };

                return View("Interview/Interview", ListCandidate);

            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //---------------------------------------------------------- View edit interview to interviewed ----------------------------------------
        [Route("candidate/interview/update/next/{id?}")]
        public ActionResult InterviewNext(string id = null)
        {
            try
            {
                if (id == null) return Redirect("~/candidate/call");

                int CandidateId = Convert.ToInt16(id);
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == CandidateId);

                if (DataCandidate == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Interview"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 15 || d.ID == 16 || d.ID == 17 || d.ID == 19).ToList()}
                };

                return View("Interview/EditCandidateInterview", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------ Process interview next to interviewed -------------------------------------------------
        [Route("candidate/interview/update/next/process")]
        public ActionResult CandidateInterviewNext(CandidateDTO Data, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
            {

                int ProcessEdit;

                var state = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(c => c.ID == Data.ID).CANDIDATE_STATE_ID;
                if (state != Data.CANDIDATE_STATE_ID)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        var SelectHis = db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_STATE == 19 && d.CANDIDATE_ID == Data.ID);
                        //process removing state call
                        db.TB_CANDIDATE_SELECTION_HISTORY.Remove(SelectHis);
                        db.SaveChanges();

                    }
                    ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                        if (ProcessEdit > 0)
                        {
                            if (TempData["message"] == null)
                            {
                                TempData.Add("message", "Candidate Update successfully");
                                TempData.Add("type", "success");
                                UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                                //check state candidate before updated
                            }

                        }
                        else
                        {
                            if(TempData["message"] == null)
                            {
                            TempData.Add("message", "Candidate failed to Update");
                            TempData.Add("type", "warning");
                            }
                        }

                        return Redirect("~/candidate/interview/read");
                }

                ProcessEdit = Manage_CandidateDTO.EditCandidate(Data, Pict, Cv);

                Manage_CandidateSelectionHistoryDTO.EditData(new CandidateSelectionHistoryDTO
                {
                    CANDIDATE_ID = Data.ID,
                    CANDIDATE_STATE = Data.CANDIDATE_STATE_ID,
                    NOTES = Data.NOTES,
                    CANDIDATE_SOURCE = Data.SOURCE,
                    CANDIDATE_INTERVIEW_DATE = Data.CANDIDATE_INTERVIEW_DATE,
                    CANDIDATE_APPLIED_POSITION = Data.POSITION,
                    CANDIDATE_SUITABLE_POSITION = Data.SUITABLE_POSITION,
                    CANDIDATE_EXPECTED_SALARY = Data.EXPECTED_sALARY
                });

                //process removing state call

                if (ProcessEdit > 0)
                {
                    if(TempData["message"] == null) {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                    }
                    
                    UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                    if (Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(c => c.ID == Data.ID).CANDIDATE_STATE_ID != Data.CANDIDATE_STATE_ID)
                    {
                        using (DBEntities db = new DBEntities())
                        {
                            var SelectHis = db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_STATE == 2 && d.CANDIDATE_ID == Data.ID);
                            db.TB_CANDIDATE_SELECTION_HISTORY.Remove(SelectHis);
                            db.SaveChanges();
                        }
                        return Redirect("~/candidate/interview/read/interviewed");
                    }
                }
                else
                {
                    if(TempData["message"] == null)
                    {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                    }
                }
                return Redirect("~/candidate/interview/read");
            }
            ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","interview"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 15 || d.ID == 16 || d.ID == 17 || d.ID == 19).ToList()}
                };
            CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID);
            //TempData.Add("message", "Candidate failed to Update, please complete form edit");
            //TempData.Add("type", "danger");
            return View("Interview/EditCandidateInterview", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }


        //------------------------------------------------------------ view candidate interviewed -----------------------------------------

        [Route("candidate/interview/read/interviewed/{i?}")]
        public ActionResult CandidateInterviewed(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 15(hold), 16(pass), 17(drop)

                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 17 || sh.CANDIDATE_STATE == 16 || sh.CANDIDATE_STATE == 15).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 15 || d.CANDIDATE_STATE == 16 || d.CANDIDATE_STATE == 17).Skip(idx).Take(perPage).ToList();
                //prepare vew bag


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.ID == 15 || d.ID == 17 || d.ID == 16)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                       (d.ID == 15 || d.ID == 17 || d.ID == 16)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "" && (StateId == 0 && Position == "all" && Keyword == ""))
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 15 || d.CANDIDATE_STATE == 16 || d.CANDIDATE_STATE == 17).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);
                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.ID == 15 || d.ID == 17 || d.ID == 16)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================

                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Interview"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 15 || d.ID == 16 || d.ID == 17)},
                    {"PageCount",PageCount}
                };

                return View("Interview/Interviewed", ListCandidate);

            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }


        //---------------------------------------------------------- View edit interviewed  ----------------------------------------
        [Route("candidate/interview/update/interviewed/next/{id?}")]
        public ActionResult InterviewedNext(string id = null)
        {
            try
            {
                if (id == null) return Redirect("~/candidate/call");

                int CandidateId = Convert.ToInt16(id);
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == CandidateId);

                if (DataCandidate == null) return Redirect("~/candidate/praselection");

                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Interview"},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 15 || d.ID == 16 || d.ID == 17 || d.ID == 19).ToList()}
                };

                return View("Interview/EditCandidateInterviewed", DataCandidate);
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------ Process interviewed edit -------------------------------------------------
        [Route("candidate/interview/update/interviewed/next/process")]
        public ActionResult CandidateInterviewedNext(CandidateDTO Data, HttpPostedFileBase Pict = null, HttpPostedFileBase Cv = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ProcessEdit = 0;
                    using (DBEntities db = new DBEntities())
                    {
                        db.TB_CANDIDATE.FirstOrDefault(d => d.ID == Data.ID).CANDIDATE_STATE_ID = Data.CANDIDATE_STATE_ID;
                        db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(d => d.CANDIDATE_ID == Data.ID && (d.CANDIDATE_STATE == 15 || d.CANDIDATE_STATE == 16 || d.CANDIDATE_STATE == 17)).CANDIDATE_STATE = Data.CANDIDATE_STATE_ID;
                        ProcessEdit = db.SaveChanges();
                    }

                    if (ProcessEdit > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("Edit Candidate" + Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID));
                        }
                    }
                    else
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                        }
                    }

                    return Redirect("~/candidate/interview/read/interviewed");
                }
                CandidateDTO DataCandidate = Manage_CandidateDTO.GetDataCandidate().FirstOrDefault(d => d.ID == Data.ID);
                TempData.Add("message", "Candidate failed to Update, please complete form edit");
                TempData.Add("type", "danger");

                return View("Interview/EditCandidateInterviewed", DataCandidate);
            }
            catch (Exception e)
            {
                string msg = e.Message.Replace('\n', ' ') + e.StackTrace.Replace('\n', ' ');
                return Redirect("~/auth/error?msg=" + (ConfigurationManager.AppSettings["env"].ToString().Equals("development") ? msg : " "));
            }
        }







        //**********************************************************  Delivery   *******************************************************


        //---------------------------------------------------------- view for delivery -------------------------------------------------
        [Route("candidate/delivery/read/{i?}")]
        public ActionResult CandidateDelivery(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 15(hold), 16(pass), 17(drop)


                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh =>  sh.CANDIDATE_STATE == 16).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 16).Skip(idx).Take(perPage).ToList();


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.ID == 16)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                       (d.ID == 16)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "")
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d => d.CANDIDATE_STATE == 16).Skip(idx).Take(perPage).ToList();
                        DataCount = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d => d.CANDIDATE_STATE == 16).ToList().Count;
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));

                        Session.Add("DataPage", dt);

                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.ID == 16)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================
                //ViewBag.DataView = new Dictionary<string, object>()
                //{
                //    {"title","Delivery" },
                //    {"PageCount", PageCount}
                //};

                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Delivery"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"PageCount",PageCount}
                    };
                return View("Delivery/Delivery", ListCandidate);

            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //-------------------------------------------- process delivery next -------------------------------------------------------
        [Route("candidate/delivery/create/next")]
        public ActionResult DeliveryNext(DeliveryHistoryDTO data)
        {
            try
            {
                int ProcessEdit;
                using (DBEntities db = new DBEntities())
            {
                    
                var Candidate = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == data.CANDIDATE_ID);
                Candidate.CANDIDATE_STATE_ID = 6;
                ProcessEdit = db.SaveChanges();
                //process add selection history
                //preparedata pic
                UserDTO DataPic = (UserDTO)Session["UserLogin"];
                var ProcessAddSelectionHistory = Manage_CandidateSelectionHistoryDTO.AddData(new CandidateSelectionHistoryDTO
                {
                    CANDIDATE_ID = data.CANDIDATE_ID,
                    PIC_ID = DataPic.USER_ID,
                    CANDIDATE_APPLIED_POSITION = Candidate.POSITION,
                    CANDIDATE_SUITABLE_POSITION = Candidate.SUITABLE_POSITION,
                    CANDIDATE_SOURCE = Candidate.SOURCE,
                    CANDIDATE_STATE = 6,
                    CANDIDATE_EXPECTED_SALARY = Candidate.EXPECTED_SALARY,
                    PROCESS_DATE = DateTime.Now,
                    NOTES = data.NOTE,
                    CANDIDATE_INTERVIEW_DATE = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().FirstOrDefault(d => d.CANDIDATE_ID == data.CANDIDATE_ID && d.CANDIDATE_STATE == 8).CANDIDATE_INTERVIEW_DATE
                });

                var ProcessAddDelivery = Manage_DeliveryHistoryDTO.AddData(new DeliveryHistoryDTO
                {
                    DELIVERY_ID = data.DELIVERY_ID,
                    CANDIDATE_ID = data.CANDIDATE_ID,
                    CLIENT_ID = data.CLIENT_ID,
                    LAST_PIC = DataPic.USER_ID,
                    CANDIDATE_STATE = 6,
                    PROCESS_DATE = DateTime.Now,
                    NOTE = data.NOTE,
                    CANDIDATE_POSITION = Candidate.SUITABLE_POSITION
                });
                    if (ProcessEdit > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("edit suggest state of candidate id " + data.CANDIDATE_ID);
                        }
                    }
                    else
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                        }
                    }

                    return Redirect("~/candidate/delivery/read/suggest");
            }
                
            }
            catch (Exception)
            {
               return Redirect("~/auth/error");
            }
        }

        //=========================================================== SUGGEST CANDIDATE ==========================================================

        [Route("candidate/delivery/read/suggest/{i?}")]
        public ActionResult SuggestCandidate(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 14(offering or 6(sent to client))

                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 6 || sh.CANDIDATE_STATE == 14 ).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                d.CANDIDATE_STATE == 6 || d.CANDIDATE_STATE == 14).Skip(idx).Take(perPage).ToList();


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        (d.ID == 6 || d.ID == 14)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                       (d.ID == 6 || d.ID == 14 )).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "")
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                        d.CANDIDATE_STATE == 6 || d.CANDIDATE_STATE == 14 ).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);

                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         (d.ID == 6 || d.ID == 14)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================
                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Delivery"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"PageCount",PageCount}
                    };

                return View("Delivery/Suggested", ListCandidate);
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------- candidate delivery update ---------------------------------------------------
        [Route("candidate/delivery/update/suggest")]
        public ActionResult SuggestUpdate(DeliveryHistoryDTO Data)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    int CandidateId = Convert.ToInt16(Data.CANDIDATE_ID);
                    db.TB_CANDIDATE.FirstOrDefault(ca => ca.ID == CandidateId).CANDIDATE_STATE_ID = Data.CANDIDATE_STATE;

                    var Delivery = db.TB_DELIVERY_HISTORY.FirstOrDefault(d => d.DELIVERY_ID == Data.DELIVERY_ID);
                    Delivery.CLIENT_ID = Data.CLIENT_ID;
                    Delivery.CANDIDATE_STATE = Data.CANDIDATE_STATE;

                    db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(s => s.ID == Data.SELECTION_ID).CANDIDATE_STATE = Data.CANDIDATE_STATE;

                    if (db.SaveChanges() > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("edit suggest state of candidate id " + Data.CANDIDATE_ID);
                        }
                    }
                    else
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                        }
                    }

                }
                    return Redirect("~/candidate/delivery/read/suggest");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }


        //=========================================================== SUGGESTION CANDIDATE ==========================================================
        [Route("candidate/suggestion/read/{i?}")]
        public ActionResult SuggestionCandidate(string i = null)
        {
            try
            {
                //---------------------------- prepare data candidate for show in view --------------
                //note : data candidate from class Manage_CandidateSelectionHistoryDTO method GetDataSelectionHistory
                //note : data in this view especialy for candidate where state_id is 14(offering or 6(sent to client))

                int perPage = Session["DataPage"] == null ? 5 : Convert.ToInt16(Session["DataPage"]);
                float DataCount = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == 14 || sh.CANDIDATE_STATE == 21).ToList().Count();
                int PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                int idx = (i == null ? 0 : (perPage * int.Parse(i) - perPage));
                List<CandidateSelectionHistoryDTO> ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d => d.CANDIDATE_STATE == 14 || d.CANDIDATE_STATE == 21).Skip(idx).Take(perPage).ToList();


                //============================ process searchng ============================
                if (Request["filter"] != null)
                {
                    string Position = Request["POSITION"];
                    int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                    string Keyword = Request["Keyword"];
                    string DataPerPage = Request["DataPerPage"];
                    int dt = DataPerPage == "" ? 5 : Convert.ToInt16(DataPerPage);

                    if (StateId != 0 && (Position == "all" && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Position != "all" && (StateId == 0 && Keyword == ""))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_APPLIED_POSITION == Position ||
                        d.CANDIDATE_SUITABLE_POSITION == Position &&
                        ( d.ID == 14 || d.ID == 21)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (Keyword != "" && (StateId == 0 && Position == "all"))
                    {
                        ListCandidate = ListCandidate.Where(d =>
                        d.CANDIDATE_EMAIL.Contains(Keyword) ||
                        d.CANDIDATE_NAME.Contains(Keyword) ||
                        d.CANDIDATE_PHONE.Contains(Keyword) &&
                       ( d.ID == 14 || d.ID == 21)).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                    if (DataPerPage != "")
                    {
                        perPage = dt;
                        ListCandidate = Manage_CandidateSelectionHistoryDTO.GetDataSelectionHistory().Where(d =>
                         d.CANDIDATE_STATE == 14 || d.CANDIDATE_STATE == 21).Skip(idx).Take(perPage).ToList();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                        Session.Add("DataPage", dt);

                    }
                    else
                    {
                        ListCandidate = ListCandidate.Where(d =>
                         d.CANDIDATE_APPLIED_POSITION == Position ||
                         d.CANDIDATE_SUITABLE_POSITION == Position ||
                         d.CANDIDATE_STATE == StateId ||
                         d.CANDIDATE_EMAIL.Contains(Keyword) ||
                         d.CANDIDATE_NAME.Contains(Keyword) ||
                         d.CANDIDATE_PHONE.Contains(Keyword) &&
                         ( d.ID == 14 || d.ID == 21)).Skip(idx).Take(perPage).ToList();
                        DataCount = ListCandidate.Count();
                        PageCount = Convert.ToInt16(Math.Ceiling(DataCount / perPage));
                    }
                }
                //============================ end process searchng ============================
                //prepare vew bag
                //---------------------------- prepare data viewbag --------------------
                ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Suggestion"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"PageCount",PageCount}
                    };

                return View("Suggestion/Suggestion", ListCandidate);
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------- candidate suggestion update ---------------------------------------------------
        [Route("candidate/suggestion/update")]
        public ActionResult SuggestionUpdate(DeliveryHistoryDTO Data)
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    int CandidateId = Convert.ToInt16(Data.CANDIDATE_ID);
                    db.TB_CANDIDATE.FirstOrDefault(ca => ca.ID == CandidateId).CANDIDATE_STATE_ID = Data.CANDIDATE_STATE;

                    var Delivery = db.TB_DELIVERY_HISTORY.FirstOrDefault(d => d.DELIVERY_ID == Data.DELIVERY_ID);
                    Delivery.CLIENT_ID = Data.CLIENT_ID;
                    Delivery.CANDIDATE_STATE = Data.CANDIDATE_STATE;

                    db.TB_CANDIDATE_SELECTION_HISTORY.FirstOrDefault(s => s.ID == Data.SELECTION_ID).CANDIDATE_STATE = Data.CANDIDATE_STATE;

                    if (db.SaveChanges() > 0)
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate Update successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("edit suggestion state of candidate id " + Data.CANDIDATE_ID);
                        }
                    }
                    else
                    {
                        if(TempData["message"] == null)
                        {
                        TempData.Add("message", "Candidate failed to Update");
                        TempData.Add("type", "warning");
                        }
                    }

                }
                return Redirect("~/candidate/suggestion/read");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }



































    }
}
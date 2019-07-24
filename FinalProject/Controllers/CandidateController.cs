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

namespace FinalProject.Controllers
{
    [UserAccessCandidateFilter]
    public class CandidateController : Controller
    {
        [Route("candidate")]
        public ActionResult Index()
        {
            return Redirect("~/candidate/preselection");
        }
//################################################################ Sub Menu Candidate Preselection ###################################### 
        
    //------------------------------------------------------- for view candidate preselection -------------------------------------------

        [Route("candidate/preselection")]
        public ActionResult CandidatePreselection()
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {
                    List<CandidateSelectionHistoryDTO> ListCandidate = DataCandidateUtils.GetDataSelectionHistory().Where(sh =>
                    sh.CANDIDATE_STATE == 1 || sh.CANDIDATE_STATE == 10 || sh.CANDIDATE_STATE == 11).ToList();
                //------------------------------------------------------ process searchng -----------------------------

                    if(Request["filter"] != null)
                    {
                        string Position = Request["POSITION"];
                        int StateId = Convert.ToInt16(Request["CANDIDATE_STATE"]);
                        string Keyword = Request["Keyword"];

                        if(StateId != 0 && (Position == "all" && Keyword == ""))
                        {
                            ListCandidate = ListCandidate.Where(d => d.CANDIDATE_STATE == StateId).ToList();
                        }
                        if(Position != "all"  && (StateId == 0 && Keyword == ""))
                        {
                            ListCandidate = ListCandidate.Where(d => 
                            d.CANDIDATE_APPLIED_POSITION == Position || 
                            d.CANDIDATE_SUITABLE_POSITION == Position).ToList();
                        }
                        if (Keyword != "" && (StateId == 0 && Position == "all"))
                        {
                            ListCandidate = ListCandidate.Where(d =>
                                d.CANDIDATE_EMAIL.Contains(Keyword) ||
                                d.CANDIDATE_NAME.Contains(Keyword) ||
                                d.CANDIDATE_PHONE.Contains(Keyword)).ToList();
                        }
                        else {
                            ListCandidate = ListCandidate.Where(d =>
                             d.CANDIDATE_APPLIED_POSITION == Position || 
                             d.CANDIDATE_SUITABLE_POSITION == Position || 
                             d.CANDIDATE_STATE == StateId ||
                             d.CANDIDATE_EMAIL.Contains(Keyword) ||
                             d.CANDIDATE_NAME.Contains(Keyword) ||
                             d.CANDIDATE_PHONE.Contains(Keyword)).ToList();
                        }
                    }

                //------------------------------------------------------ end process searchng -----------------------------

                    //prepare data for show in view
                    ViewBag.DataView = new Dictionary<string, object>{
                    {"title","Preselection"},
                    {"ListPosition",Manage_JobPositionDTO.GetData()},
                    {"ListState",Manage_StateCandidateDTO.GetData().Where(d => d.ID == 1 || d.ID == 10 || d.ID == 11)}
                    };

                    return View("Preselection/Index", ListCandidate);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

//*********************************************************************** add new candidate **********************************************************************

            //----------------------------------------------------------- view form add new candidate ------------------------------------------------------------------------
        [Route("candidate/preselection/create/candidate")]
        public ActionResult CandidatePreselectionAdd()
        {
            try
            {
                using (DBEntities db = new DBEntities())
                {

                    ViewBag.DataView = new Dictionary<string, object>(){
                    {"title","Preselection"}
                    };
                    return View("Preselection/AddCandidate");
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //----------------------------------------------------- PROCESS ADD NEW CANDIDATE ----------------------------------------------------------------------
        [Route("candidate/preselection/create/candidate/process")]
        public ActionResult CandidatePreselectionAdd(CandidateDTO DataNewCandidate,  HttpPostedFileBase Pict, HttpPostedFileBase Cv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Manage_CandidateDTO Manage_candidate = new Manage_CandidateDTO();

                    //process add will return list object, [0] is return from db.saveCahnge() and [1] return candidate_id (CA******)
                    var ProcessAdd = Manage_candidate.AddData(DataNewCandidate,Pict,Cv);

                    if (Convert.ToInt16(ProcessAdd[0]) > 0)
                    {
                        TempData.Add("message", "New Candidate added successfully");
                        TempData.Add("type", "success");

                        UserLogingUtils.SaveLoggingUserActivity("add new Candidate"+ Convert.ToString(ProcessAdd[1]));
                    }
                    else
                    {
                        TempData.Add("message", "New Candidate failed to add");
                        TempData.Add("type", "warning");
                    }
                    return Redirect("~/candidate/preselection");
                }

                TempData.Add("message", "New Candidate failed to add, please complete form add");
                TempData.Add("type", "danger");
            
                return Redirect("~/candidate/preselection");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //************************************************* ADD NEW JOB EXPERIENCE OF CANDIDATE *************************************************
       
        //------------------------------------------------------ View ADD NEW JOB EXPERIENCE OF CANDIDATE --------------------------------------------

        //[Route("candidate/preselection/read/jobExp")]
        //public ActionResult JobExp(CandidateJobExperienceDTO NewJobExp)
        //{
        //    try
        //    {
        //        return View("~/candidate/preselection/create/JobExp");
        //    }
        //    catch (Exception)
        //    {
        //        return Redirect("~/auth/error");
        //    }
        //}

        //------------------------------------------------------------ process add new job experience ----------------------------------------
        [Route("candidate/preselection/create/jobExp")]
        public ActionResult JobExpAdd(CandidateJobExperienceDTO NewJobExp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (DBEntities db = new DBEntities())
                    {
                        Manage_CandidateJobExperienceDTO Manage_JobEXP = new Manage_CandidateJobExperienceDTO();
                        var ProcessAdd = Manage_JobEXP.AddData(NewJobExp);

                        if (ProcessAdd > 0)
                        {
                            TempData.Add("message", "Candidate new job experience added successfully");
                            TempData.Add("type", "success");
                            UserLogingUtils.SaveLoggingUserActivity("add job experience Candidate " + NewJobExp.CANDIDATE_ID + " Job Experience in " + NewJobExp.COMPANY_NAME);
                        }
                        else
                        {
                            TempData.Add("message", "Candidate new job experience failed to add");
                            TempData.Add("type", "warning");
                        }
                        return Redirect("~/candidate/preselection/create/jobExp");
                    }
                }
                TempData.Add("message", "Candidate new job experience failed to add please complete form add");
                TempData.Add("type", "danger");
                return Redirect("~/candidate/preselection/create/jobExp");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }



        //------------------------------------------------------------ candidate call -----------------------------------------

        [Route("candidate/call")]
        public ActionResult CandidateCall()
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Candidate - Call"}
                };
                return View("Call/Index");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }


        //------------------------------------------------------------ candidate interview -----------------------------------------

        [Route("candidate/interview")]
        public ActionResult CandidateInterview()
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Candidate - Interview"}
                };
                return View("Interview/Index");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }


        //------------------------------------------------------------ candidate delivery -----------------------------------------

        [Route("candidate/delivery")]
        public ActionResult CandidateDelivery()
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Candidate - Delivery"}
                };
                return View("Delivery/Index");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //------------------------------------------------------------ candidate sales -----------------------------------------

        [Route("candidate/sales")]
        public ActionResult CandidateSales()
        {
            try
            {
                ViewBag.DataView = new Dictionary<string, object>()
                {
                    {"title","Candidate - Sales"}
                };
                return View("Sales/Index");
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }
    }
}
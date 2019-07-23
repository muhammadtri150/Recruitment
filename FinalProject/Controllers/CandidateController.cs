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
                    List<CandidateDTO> ListCandidate = new DataCandidateUtils().GetDataCandidatePerState().FindAll(d => d.CANDIDATE_STATE_ID == 7 || d.CANDIDATE_STATE_ID == 11);

                    ViewBag.DataView = new Dictionary<string, object>(){
                    {"title","Preselection"}
                    };
                    return View("Preselection/Index", ListCandidate);
                }
            }
            catch (Exception)
            {
                return Redirect("~/auth/error");
            }
        }

        //----------------------------------------------------------- add new candidate ------------------------------------------------------------------------
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

        //----------------------------------------------------- PROCESS ADD NEW CANDIDATE -------------------------------------------------------------
        [Route("candidate/preselection/create/candidate/process")]
        public ActionResult CandidatePreselectionAdd(CandidateDTO DataNewCandidate,  HttpPostedFileBase Pict, HttpPostedFileBase Cv)
        {
            //try
            //{
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
            //}
            //catch (Exception)
            //{
            //    return Redirect("~/auth/error");
            //}
        }

        //------------------------------------------------------- ADD NEW JOB EXPERIENCE OF CANDIDATE -------------------------------------------------
       
        //------------------------------------------------------ View wADD NEW JOB EXPERIENCE OF CANDIDATE --------------------------------------------

        [Route("candidate/preselection/create/jobdesc")]
        public ActionResult CandidatePreselectionAdd(CandidateJobExperienceDTO NewJobExp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Manage_CandidateJobExperienceDTO ManageJobExp = new Manage_CandidateJobExperienceDTO();

                    //process add will return integer, success if return > 0

                    if (ManageJobExp.AddData(NewJobExp) > 0)
                    {
                        TempData.Add("message", "Candidate new job experience added successfully");
                        TempData.Add("type", "success");
                        UserLogingUtils.SaveLoggingUserActivity("add Candidate " + NewJobExp.CANDIDATE_ID+" Job Experience in "+NewJobExp.COMPANY_NAME);
                    }
                    else
                    {
                        TempData.Add("message", "Candidate new job experience failed to add");
                        TempData.Add("type", "warning");
                    }
                    return Redirect("~/candidate/preselection");
                }

                TempData.Add("message", "Candidate new job experience failed to add, please complete form add");
                TempData.Add("type", "danger");

                return Redirect("~/candidate/preselection");
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
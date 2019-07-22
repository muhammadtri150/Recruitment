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
                    List<CandidateDTO> ListCandidate = new DataCandidateUtils().GetDataCandidatePerState(1);

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

    //----------------------------------------------------------- view form add new candidate -------------------------------------------
        [Route("candidate/preselection/create")]
        public ActionResult CandidatePreselectionAdd()
        {
            //try
            //{
                using (DBEntities db = new DBEntities())
                {

                    ViewBag.DataView = new Dictionary<string, object>(){
                    {"title","Preselection"}
                    };
                    return View("Preselection/AddCandidate");
                }
            //}
            //catch (Exception)
            //{
            //    return Redirect("~/auth/error");
            //}
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
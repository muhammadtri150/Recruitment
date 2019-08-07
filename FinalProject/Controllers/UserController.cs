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
        DBEntities db = new DBEntities();
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

                    List<DashboardDTO> Toppositions = new List<DashboardDTO>();//alltime result
                    List<DashboardDTO> ToppositionMonths = new List<DashboardDTO>();//month result
                    List<DashboardDTO> ToppositionYears = new List<DashboardDTO>();//year result

                    //all most

                    List<TB_CANDIDATE_SELECTION_HISTORY> candpres = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 1).ToList();
                    SearchTopJob(candpres);
                    List<DashboardDTO> toppres = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppress = toppres.Where(m => m.moststate == "Pra-Selection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    Toppositions.Add(toppress);

                    List<TB_CANDIDATE_SELECTION_HISTORY> candcall = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 8).ToList();
                    SearchTopJob(candcall);
                    List<DashboardDTO> topcalls = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO topcall = topcalls.Where(m => m.moststate == "Called").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    Toppositions.Add(topcall);

                    TempData["topposition"] = Toppositions;

                    //month most
                    List<TB_CANDIDATE_SELECTION_HISTORY> candpresmonth = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.PROCESS_DATE.Value.Year == DateTime.Now.Year).Where(m => m.PROCESS_DATE.Value.Month == DateTime.Now.Month).Where(m => m.CANDIDATE_STATE == 1).ToList();
                    SearchTopJob(candpresmonth);
                    List<DashboardDTO> toppresmonth = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppressmonth = toppresmonth.Where(m => m.moststate == "Pra-Selection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionMonths.Add(toppressmonth);

                    List<TB_CANDIDATE_SELECTION_HISTORY> candcallmonth = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.PROCESS_DATE.Value.Year == DateTime.Now.Year).Where(m => m.PROCESS_DATE.Value.Month == DateTime.Now.Month).Where(m => m.CANDIDATE_STATE == 8).ToList();
                    SearchTopJob(candcallmonth);
                    List<DashboardDTO> topcallsmonth = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO topcallmonth = topcallsmonth.Where(m => m.moststate == "Called").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionMonths.Add(topcallmonth);

                    TempData["toppositionmonth"] = ToppositionMonths;

                    //year most
                    List<TB_CANDIDATE_SELECTION_HISTORY> candpresyear = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.PROCESS_DATE.Value.Year == DateTime.Now.Year).Where(m => m.CANDIDATE_STATE == 1).ToList();
                    SearchTopJob(candpresyear);
                    List<DashboardDTO> toppresyear = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppressyear = toppresyear.Where(m => m.moststate == "Pra-Selection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionYears.Add(toppressyear);

                    List<TB_CANDIDATE_SELECTION_HISTORY> candcallyear = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.PROCESS_DATE.Value.Year == DateTime.Now.Year).Where(m => m.CANDIDATE_STATE == 8).ToList();
                    SearchTopJob(candcallyear);
                    List<DashboardDTO> topcallsyear = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO topcallyear = topcallsyear.Where(m => m.moststate == "Called").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionYears.Add(topcallyear);

                    TempData["toppositionyear"] = ToppositionYears;

                    return View("Index");
                }
            }
            catch
            {
                return Redirect("~/auth/error");
            }
        }
        

        public ActionResult GetPie()
        {
            Ratio getcount = new Ratio();
            getcount.Jobstreet = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "Jobstreet").Count();
            getcount.JobsID = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "JobsID").Count();
            getcount.JobsDB = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "JobsDB").Count();
            getcount.Joblike = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "Joblike").Count();
            getcount.TopKarir = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "Top Karir").Count();
            getcount.KarirPad = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "KarirPad").Count();
            getcount.Karir2 = db.TB_CANDIDATE_SELECTION_HISTORY.Where(x => x.CANDIDATE_SOURCE == "Karir2.com").Count();
            return Json(getcount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBar()
        {
            Total total = new Total();
            total.PreJava = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 1 || m.CANDIDATE_APPLIED_POSITION == "Java").Count();
            total.PrePHP = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 1 || m.CANDIDATE_APPLIED_POSITION == "PHP").Count();
            total.PreRuby = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 1 || m.CANDIDATE_APPLIED_POSITION == "Ruby").Count();
            total.PreVB = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 1 || m.CANDIDATE_APPLIED_POSITION == "VB.NET").Count();

            total.CallJava = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 8 || m.CANDIDATE_APPLIED_POSITION == "Java").Count();
            total.CallPHP = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 8 || m.CANDIDATE_APPLIED_POSITION == "PHP").Count();
            total.CallRuby = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 8 || m.CANDIDATE_APPLIED_POSITION == "Ruby").Count();
            total.CallVB = db.TB_CANDIDATE_SELECTION_HISTORY.Where(m => m.CANDIDATE_STATE == 8 || m.CANDIDATE_APPLIED_POSITION == "VB.NET").Count();
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        public void SearchTopJob(List<TB_CANDIDATE_SELECTION_HISTORY> candidate)
        {
            TempData["Top"] = 0;
            List<DashboardDTO> most = new List<DashboardDTO>();
            foreach (TB_CANDIDATE_SELECTION_HISTORY c in candidate)
            {
                int Totalpos = candidate.Where(m => m.CANDIDATE_STATE == c.CANDIDATE_STATE).Count();
                TB_JOB_POSITION namepos = db.TB_JOB_POSITION.Where(m => m.JOBPOSITION_NAME == c.CANDIDATE_APPLIED_POSITION).FirstOrDefault();
                TB_STATE_CANDIDATE statepos = db.TB_STATE_CANDIDATE.Where(m => m.ID == c.CANDIDATE_STATE).FirstOrDefault();
                DashboardDTO mostpos = new DashboardDTO { mostqty = Totalpos, mostposition = namepos.JOBPOSITION_NAME, moststate = statepos.STATE_NAME };

                most.Add(mostpos);
            }
            TempData["Top"] = most;
        }
    }
}
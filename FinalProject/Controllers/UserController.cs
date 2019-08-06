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

                    List<TB_CANDIDATE> candpres = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 1).ToList();
                    SearchTopJob(candpres);
                    List<DashboardDTO> toppres = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppress = toppres.Where(m => m.moststate == "preselection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    Toppositions.Add(toppress);

                    List<TB_CANDIDATE> candcall = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 8).ToList();
                    SearchTopJob(candcall);
                    List<DashboardDTO> topcalls = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO topcall = topcalls.Where(m => m.moststate == "Called").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    Toppositions.Add(topcall);

                    TempData["topposition"] = Toppositions;

                    //month most
                    List<TB_CANDIDATE> candpresmonth = db.TB_CANDIDATE.Where(m => m.SOURCING_DATE.Value.Year == DateTime.Now.Year).Where(m => m.SOURCING_DATE.Value.Month == DateTime.Now.Month).Where(m => m.CANDIDATE_STATE_ID == 1).ToList();
                    SearchTopJob(candpresmonth);
                    List<DashboardDTO> toppresmonth = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppressmonth = toppresmonth.Where(m => m.moststate == "preselection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionMonths.Add(toppressmonth);

                    List<TB_CANDIDATE> candcallmonth = db.TB_CANDIDATE.Where(m => m.SOURCING_DATE.Value.Year == DateTime.Now.Year).Where(m => m.SOURCING_DATE.Value.Month == DateTime.Now.Month).Where(m => m.CANDIDATE_STATE_ID == 8).ToList();
                    SearchTopJob(candcallmonth);
                    List<DashboardDTO> topcallsmonth = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO topcallmonth = topcallsmonth.Where(m => m.moststate == "Called").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionMonths.Add(topcallmonth);

                    TempData["toppositionmonth"] = ToppositionMonths;

                    //year most
                    List<TB_CANDIDATE> candpresyear = db.TB_CANDIDATE.Where(m => m.SOURCING_DATE.Value.Year == DateTime.Now.Year).Where(m => m.CANDIDATE_STATE_ID == 1).ToList();
                    SearchTopJob(candpresyear);
                    List<DashboardDTO> toppresyear = (List<DashboardDTO>)TempData.Peek("Top");
                    DashboardDTO toppressyear = toppresyear.Where(m => m.moststate == "preselection").OrderByDescending(n => n.mostqty).FirstOrDefault();
                    ToppositionYears.Add(toppressyear);

                    List<TB_CANDIDATE> candcallyear = db.TB_CANDIDATE.Where(m => m.SOURCING_DATE.Value.Year == DateTime.Now.Year).Where(m => m.CANDIDATE_STATE_ID == 8).ToList();
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
        public void SearchTopJob(List<TB_CANDIDATE> candidate)
        {
            TempData["Top"] = 0;
            List<DashboardDTO> most = new List<DashboardDTO>();
            foreach (TB_CANDIDATE c in candidate)
            {
                int Totalpos = candidate.Where(m => m.CANDIDATE_STATE_ID == c.CANDIDATE_STATE_ID).Count();
                TB_JOB_POSITION namepos = db.TB_JOB_POSITION.Where(m => m.JOBPOSITION_NAME == c.POSITION).FirstOrDefault();
                TB_STATE_CANDIDATE statepos = db.TB_STATE_CANDIDATE.Where(m => m.ID == c.CANDIDATE_STATE_ID).FirstOrDefault();
                DashboardDTO mostpos = new DashboardDTO { mostqty = Totalpos, mostposition = namepos.JOBPOSITION_NAME, moststate = statepos.STATE_NAME };

                most.Add(mostpos);
            }
            TempData["Top"] = most;
        }

        public ActionResult GetPie()
        {
            Ratio getcount = new Ratio();
            getcount.Jobstreet = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "Jobstreet").Count();
            getcount.JobsID = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "JobsID").Count();
            getcount.JobsDB = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "JobsDB").Count();
            getcount.Joblike = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "Joblike").Count();
            getcount.TopKarir = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "Top Karir").Count();
            getcount.KarirPad = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "KarirPad").Count();
            getcount.Karir2 = db.JOB_PORTAL.Where(x => x.JOBPORTAL_NAME == "Karir2.com").Count();
            return Json(getcount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBar()
        {
            Total total = new Total();
            total.PreJava = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 1 || m.POSITION == "Java").Count();
            total.PrePHP = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 1 || m.POSITION == "PHP").Count();
            total.PreRuby = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 1 || m.POSITION == "Ruby").Count();
            total.PreVB = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 1 || m.POSITION == "ASP").Count();

            total.CallJava = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 8 || m.POSITION == "Java").Count();
            total.CallPHP = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 8 || m.POSITION == "PHP").Count();
            total.CallRuby = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 8 || m.POSITION == "Ruby").Count();
            total.CallVB = db.TB_CANDIDATE.Where(m => m.CANDIDATE_STATE_ID == 8 || m.POSITION == "ASP").Count();
            return Json(total, JsonRequestBehavior.AllowGet);
        }

        public class Ratio
        {
            public int Jobstreet { get; set; }
            public int JobsID { get; set; }
            public int JobsDB { get; set; }
            public int Joblike { get; set; }
            public int TopKarir { get; set; }
            public int KarirPad { get; set; }
            public int Karir2 { get; set; }
        }

        public class Total
        {
            public int PreJava { get; set; }
            public int PrePHP { get; set; }
            public int PreRuby { get; set; }
            public int PreVB { get; set; }
            public int CallJava { get; set; }
            public int CallPHP { get; set; }
            public int CallRuby { get; set; }
            public int CallVB { get; set; }

        }
    }
}
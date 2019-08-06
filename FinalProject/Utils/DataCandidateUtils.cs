using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.DTO;
using FinalProject.Models;
using FinalProject.Controllers;
using FinalProject.Filters;

namespace FinalProject.Utils
{
    public class DataCandidateUtils
    {

        //pagination
        public static int Pagination(int StateId)
        {
            using(DBEntities db = new DBEntities())
            {
                float count = db.TB_CANDIDATE_SELECTION_HISTORY.Where(sh => sh.CANDIDATE_STATE == StateId).ToList().Count();
                return Convert.ToInt16(Math.Ceiling(count / 5));
            }
        }

    }
}
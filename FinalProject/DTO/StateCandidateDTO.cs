using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class StateCandidateDTO
    {
        public int ID { get; set; }
        public string STATE_NAME { get; set; }
    }

    //manage------------------------------------
    public class Manage_StateCandidateDTO {

       public static List<StateCandidateDTO> GetData() { 
             using(DBEntities db = new DBEntities())
             {
                return db.TB_STATE_CANDIDATE.Select(s => new StateCandidateDTO
                {
                    ID = s.ID,
                    STATE_NAME = s.STATE_NAME
                }).ToList();
             }
       }
    }
}
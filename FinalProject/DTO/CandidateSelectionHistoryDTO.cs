using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using FinalProject.DTO;

namespace FinalProject.DTO
{
    public class CandidateSelectionHistoryDTO
    {

        public int ID { get; set; }
        
        
        public string CANDIDATE_APPLIED_POSITION { get; set; }
        public string CANDIDATE_SUITABLE_POSITION { get; set; }
        public string CANDIDATE_SOURCE { get; set; }
        public int? CANDIDATE_STATE { get; set; }
        public string CANDIDATE_STATE_NAME { get; set; }
        public decimal? CANDIDATE_EXPECTED_SALARY { get; set; }
        public System.DateTime? PROCESS_DATE { get; set; }
        public string NOTES { get; set; }

        //data candidate
        public int? CANDIDATE_ID { get; set; }
        public string CANDIDATE_NAME { get; set; }
        public System.DateTime? CANDIDATE_SOURCING_DATE { get; set; }
        public string CANDIDATE_EMAIL { get; set; }
        public string CANDIDATE_PHONE { get; set; }

        //data PIC
        public int? PIC_ID { get; set; }
        public string PIC_FULL_NAME { get; set; }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------
    public class Manage_CandidateSelectionHistoryDTO
    {

        public int AddData(CandidateSelectionHistoryDTO data)
        {
            using(DBEntities db = new DBEntities())
            {
                
                db.TB_CANDIDATE_SELECTION_HISTORY.Add(new TB_CANDIDATE_SELECTION_HISTORY
                {
                    CANDIDATE_ID = data.CANDIDATE_ID,
                    PIC_ID = data.PIC_ID,
                    CANDIDATE_APPLIED_POSITION = data.CANDIDATE_APPLIED_POSITION,
                    CANDIDATE_SUITABLE_POSITION = data.CANDIDATE_SUITABLE_POSITION,
                    CANDIDATE_SOURCE = data.CANDIDATE_SOURCE,
                    CANDIDATE_STATE = data.CANDIDATE_STATE,
                    CANDIDATE_EXPECTED_SALARY = data.CANDIDATE_EXPECTED_SALARY,
                    PROCESS_DATE = DateTime.Now,
                    NOTES = data.NOTES
                });

                return db.SaveChanges();
            }
        }

    }
}
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

    //--------------------------------------------------------- Add data ----------------------------------------------------------------
    public class Manage_CandidateSelectionHistoryDTO
    {

        public static int AddData(CandidateSelectionHistoryDTO data)
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

        //---------------------------------------------------------- for get data ----------------------------------------------------
        public static List<CandidateSelectionHistoryDTO> GetDataSelectionHistory()
        {
            using (DBEntities db = new DBEntities())
            {
                List<CandidateSelectionHistoryDTO> Data = db.TB_CANDIDATE_SELECTION_HISTORY.
                    Select(sh => new CandidateSelectionHistoryDTO
                    {
                        ID = sh.ID,
                        CANDIDATE_ID = sh.CANDIDATE_ID,
                        CANDIDATE_APPLIED_POSITION = sh.CANDIDATE_APPLIED_POSITION,
                        CANDIDATE_SUITABLE_POSITION = sh.CANDIDATE_SUITABLE_POSITION,
                        CANDIDATE_SOURCE = sh.CANDIDATE_SOURCE,
                        CANDIDATE_EXPECTED_SALARY = sh.CANDIDATE_EXPECTED_SALARY,
                        PROCESS_DATE = sh.PROCESS_DATE,
                        NOTES = sh.NOTES,
                        CANDIDATE_STATE = sh.CANDIDATE_STATE,
                        CANDIDATE_STATE_NAME = db.TB_STATE_CANDIDATE.FirstOrDefault(s => s.ID == sh.CANDIDATE_STATE).STATE_NAME,
                        CANDIDATE_EMAIL = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == sh.CANDIDATE_ID).CANDIDATE_EMAIL,
                        CANDIDATE_SOURCING_DATE = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == sh.CANDIDATE_ID).SOURCING_DATE,
                        CANDIDATE_PHONE = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == sh.CANDIDATE_ID).CANDIDATE_PHONENUMBER,
                        CANDIDATE_NAME = db.TB_CANDIDATE.FirstOrDefault(c => c.ID == sh.CANDIDATE_ID).CANDIDATE_NAME,
                        PIC_ID = sh.PIC_ID,
                        PIC_FULL_NAME = db.TB_USER.FirstOrDefault(u => u.USER_ID == sh.PIC_ID).FULL_NAME
                    }
                ).ToList();
                return Data;
            }
        }

    }
}
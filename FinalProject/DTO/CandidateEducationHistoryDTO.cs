using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DTO
{
    public class CandidateEducationHistoryDTO
    {
        public int ID { get; set; }

        //data candidate
        public int CANDIDATE_ID { get; set; }
        public string CANDIDATE_NAME { get; set; }

        //data user (pic)
        public int USER_ID { get; set; }
        public string USERNAME { get; set; }

        public string CANDIDATE_APPLIED_POSITION { get; set; }
        public string CANDIDATE_SUITABLE_POSITION { get; set; }
        public string CANDIDATE_LAST_POSITION { get; set; }
        public string CANDIDATE_SOURCE { get; set; }

        //data candidate state
        public int CANDIDATE_STATE { get; set; }
        public string CANDIDATE_STATE_NAME { get; set; }

        public decimal CANDIDATE_EXPECTED_SALARY { get; set; }
        public System.DateTime PROCESS_DATE { get; set; }
        public string NOTES { get; set; }
    }
}
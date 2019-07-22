using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class CandidateSelectionHistoryDTO
    {
        public int ID { get; set; }
        

        public string CANDIDATE_APPLIED_POSITION { get; set; }
        public string CANDIDATE_SUITABLE_POSITION { get; set; }
        public string CANDIDATE_SOURCE { get; set; }
        public decimal? CANDIDATE_EXPECTED_SALARY { get; set; }
        public System.DateTime? PROCESS_DATE { get; set; }
        public string NOTES { get; set; }

        //data candidate
        public int? CANDIDATE_ID { get; set; }

        //data PIC
        public UserDTO PIC { get; set; }

        //data candidate
        public CandidateDTO CANDIDATE { get; set; }


        //data state
        public int? CANDIDATE_STATE { get; set; }
        public string STATE_NAME { get; set; }
    }
}
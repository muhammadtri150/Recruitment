using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using FinalProject.Controllers;
using FinalProject.Filters;
using FinalProject.Utils;

namespace FinalProject.DTO
{
    public class CandidateDTO
    {
        //this dto consist of entire data that needed for view, so the format of candidate dto is not same with tb_dto, ok

        public int ID { get; set; }
        public string CANDIDATE_ID { get; set; }
        public string CANDIDATE_NAME { get; set; }
        public int? CANDIDATE_AGE { get; set; }
        public System.DateTime? CANDIDATE_BIRTH_DATE { get; set; }
        public string CANDIDATE_PLACE_BIRTH { get; set; }
        public string CANDIDATE_ETNIC { get; set; }
        public string CANDIDATE_HOMENUMBER { get; set; }
        public string CANDIDATE_PHONENUMBER { get; set; }
        public string CANDIDATE_EMAIL { get; set; }
       
        public string CANDIDATE_CURRENT_ADDRESS { get; set; }
        public string CANDIDATE_KTP_NUMBER { get; set; }
        public string CANDDIATE_NPWP_NUMBER { get; set; }
        public string CANDIDATE_CV { get; set; }
        public string CANDIDATE_PHOTO { get; set; }

        public string SOURCE { get; set; }
        public System.DateTime? SOURCING_DATE { get; set; }


        //datagender
        public int? GENDER_ID { get; set; }
        public string GENDER_NAME { get; set; }

        //data marital status
        public int? MARITAL_STATUS_ID { get; set; }
        public string MARITAL_STATUS { get; set; }

        //data religion
        public int? RELIGION_ID { get; set; }
        public string RELIGION { get; set; }

        //data address
        public int? CANDIDATE_CITY { get; set; }
        public string CANDIDATE_CITY_NAME { get; set; }
        public int? CANDIDATE_PROVINCE { get; set; }
        public string CANDIDATE_PROVINCE_NAME { get; set; }

        //data education candidate
        public string CANDIDATE_LAST_EDUCATON { get; set; }
        public double? CANDIDATE_GPA { get; set; }
        public string CANDIDATE_MAJOR { get; set; }
        public string CANDIDATE_DEGREE { get; set; }

        //data state
        public int? CANDIDATE_STATE_ID { get; set; }
        public string CANDIDATE_STATE_NAME { get; set; }

    }
}
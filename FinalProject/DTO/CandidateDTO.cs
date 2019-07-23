using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using FinalProject.Controllers;
using FinalProject.Filters;
using FinalProject.Utils;
using System.IO;

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
        public string PARENT_ADDRESS { get; set; }
        public string RESIDENT_CARD_NUMBER { get; set; }
        public string TELEPHONE_NUMBER { get; set; }
        public string CANDIDATE_PHONENUMBER { get; set; }
        public string CANDIDATE_EMAIL { get; set; }
        public List<string> CANDIDATE_SKILL { get; set; }


        public System.DateTime? AVAILABLE_JOIN { get; set; }
        public string RECOMENDATION { get; set; }
        public decimal?  EXPECTED_sALARY { get; set; }
        public string  NOTES { get; set; }
        public string  POSITION { get; set; }



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
        public string ZIP_CODE { get; set; }


        //data education candidate
        public string CANDIDATE_LAST_EDUCATON { get; set; }
        public double? CANDIDATE_GPA { get; set; }
        public string CANDIDATE_MAJOR { get; set; }
        public string CANDIDATE_DEGREE { get; set; }
        public System.DateTime? EDUCATON_START_DATE { get; set; }
        public System.DateTime? EDUCATON_END_DATE { get; set; }

        //data state
        public int? CANDIDATE_STATE_ID { get; set; }
        public string CANDIDATE_STATE_NAME { get; set; }

    }

    public class Manage_CandidateDTO
    {
        public List<object> AddData(CandidateDTO DataNewCandidate, HttpPostedFileBase Pict, HttpPostedFileBase Cv)
        {
            using(DBEntities db = new DBEntities())
            {
                //generate id candidate
                string Candidate_ID = "CA" + DateTime.Now.ToString("fffff");

                //process file pict candidate
                string pict_ext = Pict.FileName.Split('.')[1];
                string pict_name = "Pict_"+Candidate_ID +"."+ pict_ext;
                string path_pict = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Data/DataCandidate/Pict/"), pict_name);
                Pict.SaveAs(path_pict);

                //process file Cv
                string Cv_ext = Cv.FileName.Split('.')[1];
                string Cv_name = "Cv_"+Candidate_ID +"."+ Cv_ext;
                string path_cv = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Data/DataCandidate/Cv/"), Cv_name);
                Cv.SaveAs(path_cv);

                
                //process insert data
                db.TB_CANDIDATE.Add(new TB_CANDIDATE
                {
                    CANDIDATE_ID = Candidate_ID,
                    CANDIDATE_NAME = DataNewCandidate.CANDIDATE_NAME,
                    CANDIDATE_AGE = DataNewCandidate.CANDIDATE_AGE,
                    CANDIDATE_BIRTH_DATE = DataNewCandidate.CANDIDATE_BIRTH_DATE,
                    CANDIDATE_PLACE_BIRTH = DataNewCandidate.CANDIDATE_PLACE_BIRTH,
                    MARITAL_STATUS_ID = DataNewCandidate.MARITAL_STATUS_ID,
                    GENDER_ID = DataNewCandidate.GENDER_ID,
                    RELIGION_ID = DataNewCandidate.RELIGION_ID,
                    CANDIDATE_ETNIC = DataNewCandidate.CANDIDATE_ETNIC,
                    CANDIDATE_HOMENUMBER = DataNewCandidate.CANDIDATE_HOMENUMBER,
                    CANDIDATE_PHONENUMBER = DataNewCandidate.CANDIDATE_PHONENUMBER,
                    CANDIDATE_EMAIL = DataNewCandidate.CANDIDATE_EMAIL,
                    CANDIDATE_CITY = DataNewCandidate.CANDIDATE_CITY,
                    CANDIDATE_PROVINCE = DataNewCandidate.CANDIDATE_PROVINCE,
                    CANDIDATE_CURRENT_ADDRESS = DataNewCandidate.CANDIDATE_CURRENT_ADDRESS,
                    CANDIDATE_KTP_NUMBER = DataNewCandidate.CANDIDATE_KTP_NUMBER,
                    CANDDIATE_NPWP_NUMBER = DataNewCandidate.CANDDIATE_NPWP_NUMBER,
                    CANDIDATE_CV = Cv_name,
                    CANDIDATE_PHOTO = pict_name,
                    CANDIDATE_LAST_EDUCATON = DataNewCandidate.CANDIDATE_LAST_EDUCATON,
                    CANDIDATE_GPA = DataNewCandidate.CANDIDATE_GPA,
                    CANDIDATE_MAJOR = DataNewCandidate.CANDIDATE_MAJOR,
                    CANDIDATE_DEGREE = DataNewCandidate.CANDIDATE_DEGREE,
                    CANDIDATE_STATE_ID = DataNewCandidate.CANDIDATE_STATE_ID,
                    SOURCE = DataNewCandidate.SOURCE,
                    SOURCING_DATE = DataNewCandidate.SOURCING_DATE,
                    ZIP_CODE = DataNewCandidate.ZIP_CODE,
                    PARENT_ADDRESS = DataNewCandidate.PARENT_ADDRESS,
                    RESIDENT_CARD_NUMBER = DataNewCandidate.RESIDENT_CARD_NUMBER,
                    TELEPHONE_NUMBER = DataNewCandidate.TELEPHONE_NUMBER,
                    AVAILABLE_JOIN = DataNewCandidate.AVAILABLE_JOIN,
                    RECOMENDATION = DataNewCandidate.RECOMENDATION,
                    EXPECTED_SALARY = DataNewCandidate.EXPECTED_sALARY,
                    NOTES = DataNewCandidate.NOTES,
                    POSITION = DataNewCandidate.POSITION,
                    EDUCATION_START_DATE = DataNewCandidate.EDUCATON_START_DATE,
                    EDUCATION_END_DATE = DataNewCandidate.EDUCATON_END_DATE,
                });

                int res = 0;

                if (db.SaveChanges() > 0)
                {
                    //insert skill
                    foreach (string skill in DataNewCandidate.CANDIDATE_SKILL)
                    {
                        db.TB_CANDIDATE_SKILL.Add(new TB_CANDIDATE_SKILL
                        {
                            CANDIDATE_ID = db.TB_CANDIDATE.FirstOrDefault(c => c.CANDIDATE_ID == Candidate_ID).ID,
                            SKILL = skill
                        });
                        res = db.SaveChanges();
                    }
                }
                return new List<object>() { { res }, {Candidate_ID} };
            }
        }
    }
}
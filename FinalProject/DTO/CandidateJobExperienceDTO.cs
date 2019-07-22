using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class CandidateJobExperienceDTO
    {
        public int ID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string INDUSTRIES { get; set; }
        public System.DateTime? START_DATE { get; set; }
        public System.DateTime? END_DATE { get; set; }
        public decimal? CURRENT_SALARY { get; set; }
        public string SKILL_NAME { get; set; }
        public string JOBDESC { get; set; }
        public string BENEFIT { get; set; }

        //data candidate
        public int? CANDIDATE_ID { get; set; }
        public string CANDIDATE_NAME { get; set; }
        public string CANDIDATE_POSITION { get; set; }
    }

    public class DataCandidateJobExperienceDTO
    {
        public static List<CandidateJobExperienceDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                List<CandidateJobExperienceDTO> Data = db.TB_CANDIDATE_JOB_EXPERIENCE.Select(j => new CandidateJobExperienceDTO {
                    ID = j.ID,
                    COMPANY_NAME = j.COMPANY_NAME,
                    INDUSTRIES = j.INDUSTRIES,
                    CANDIDATE_POSITION = j.CANDIDATE_POSITION,
                    START_DATE = j.START_DATE,
                    END_DATE = j.END_DATE,
                    CURRENT_SALARY = j.CURRENT_SALARY,
                    SKILL_NAME = j.SKILL_NAME,
                    JOBDESC = j.JOBDESC,
                    BENEFIT = j.BENEFIT
                }).ToList();
                return Data;
            }
        } 

        //for get benefit but in arra format, will split the benefit base on ','
        public static string[] GetBenefit(CandidateJobExperienceDTO Candidate)
        {
            return DataCandidateJobExperienceDTO.GetData().FirstOrDefault(d => d.CANDIDATE_ID == Candidate.ID).BENEFIT.Split(',');
        }
    }
}
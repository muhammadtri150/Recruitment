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
        public string PROJECT { get; set; }

        //data candidate
        public int? CANDIDATE_ID { get; set; }
        public string CANDIDATE_NAME { get; set; }
        public string CANDIDATE_POSITION { get; set; }
    }

    //--------------------------------------------------------- class for mnage data dto ------------------------------------------------------------

    public class Manage_CandidateJobExperienceDTO
    {
        // for get entre data of candidate table
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
                    BENEFIT = j.BENEFIT,
                    PROJECT = j.PROJCT
                }).ToList();
                return Data;
            }
        } 

        //for get benefit in array format, will split the benefit base on ','
        public static string[] GetBenefit(CandidateJobExperienceDTO JobExp)
        {
            return Manage_CandidateJobExperienceDTO.GetData().FirstOrDefault(d => d.ID == JobExp.ID).BENEFIT.Split(',');
        }

        //for add new job experience of candidate
        public static int AddData(CandidateJobExperienceDTO NewJobExp)
        {
            using(DBEntities db = new DBEntities())
            {
                db.TB_CANDIDATE_JOB_EXPERIENCE.Add(new TB_CANDIDATE_JOB_EXPERIENCE
                {
                    CANDIDATE_ID        = NewJobExp.CANDIDATE_ID,
                    INDUSTRIES          = NewJobExp.INDUSTRIES,
                    CANDIDATE_POSITION  = NewJobExp.CANDIDATE_POSITION,
                    START_DATE          = NewJobExp.START_DATE,
                    END_DATE            = NewJobExp.END_DATE,
                    CURRENT_SALARY      = NewJobExp.CURRENT_SALARY,
                    SKILL_NAME          = NewJobExp.SKILL_NAME,
                    JOBDESC             = NewJobExp.JOBDESC,
                    BENEFIT             = NewJobExp.BENEFIT,
                    COMPANY_NAME        = NewJobExp.COMPANY_NAME,
                    PROJCT              = NewJobExp.PROJECT
                });

                return db.SaveChanges();
            }
        }

    }
}
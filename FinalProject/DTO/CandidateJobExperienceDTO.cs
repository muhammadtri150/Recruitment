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
                    JOBDESC = j.JOBDESC,
                    BENEFIT = j.BENEFIT,
                    PROJECT = j.PROJCT
                }).ToList();
                return Data;
            }
        } 

        //for get benefit in array format, will split the benefit base on ','
        public static List<string> GetBenefit(int id)
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_BENEFIT_JOB_EXPERIENCE.Where(d => d.JOB_EXP_ID == id).Select(d => d.BENEFIT).ToList();
            }
        }

        //for get benefit in array format, will split the benefit base on ','
        public static List<string> GetSkillName(int id)
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_SKILL_JOB_EXPERIENCE.Where(d => d.ID_JOBEXP == id).Select(d => d.SKILL_NAME).ToList(); 
            }
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
                    JOBDESC             = NewJobExp.JOBDESC,
                    BENEFIT             = NewJobExp.BENEFIT,
                    COMPANY_NAME        = NewJobExp.COMPANY_NAME,
                    PROJCT              = NewJobExp.PROJECT
                });
                
                if(db.SaveChanges() > 0)
                {
                    if(NewJobExp.SKILL_NAME != null || NewJobExp.SKILL_NAME != "" || NewJobExp.BENEFIT != null || NewJobExp.BENEFIT != "")
                    {
                        TB_CANDIDATE_JOB_EXPERIENCE DataJobExp = db.TB_CANDIDATE_JOB_EXPERIENCE.FirstOrDefault(d =>
                        d.CANDIDATE_ID == NewJobExp.CANDIDATE_ID &&
                        d.COMPANY_NAME == NewJobExp.COMPANY_NAME);

                        string[] Skills = NewJobExp.SKILL_NAME.Split(',');
                        string[] Benefits = NewJobExp.BENEFIT.Split(',');

                        if (Skills.Length > 1)
                        {
                            foreach (string skill in Skills)
                            {
                                db.TB_SKILL_JOB_EXPERIENCE.Add(new TB_SKILL_JOB_EXPERIENCE
                                {
                                    ID_JOBEXP = DataJobExp.ID,
                                    SKILL_NAME = skill
                                });
                            }
                        }
                        if (Skills.Length == 0)
                        {
                            db.TB_SKILL_JOB_EXPERIENCE.Add(new TB_SKILL_JOB_EXPERIENCE
                            {
                                ID_JOBEXP = DataJobExp.ID,
                                SKILL_NAME = DataJobExp.SKILL_NAME
                            });
                        }
                        if(Benefits.Length > 1)
                        {
                            foreach (string benefit in Benefits)
                            {
                                db.TB_BENEFIT_JOB_EXPERIENCE.Add(new TB_BENEFIT_JOB_EXPERIENCE
                                {
                                    JOB_EXP_ID = DataJobExp.ID,
                                    BENEFIT = benefit
                                });
                            }
                        }
                        if(Benefits.Length == 0)
                        {
                            db.TB_BENEFIT_JOB_EXPERIENCE.Add(new TB_BENEFIT_JOB_EXPERIENCE
                            {
                                JOB_EXP_ID = DataJobExp.ID,
                                BENEFIT = DataJobExp.BENEFIT
                            });
                        }
                    }
                    if(db.SaveChanges() > 0)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            return 1;
        }

        //edit job experience
        //for add new job experience of candidate
        public static int EditData(CandidateJobExperienceDTO Data)
        {
            using (DBEntities db = new DBEntities())
            {
                db.TB_CANDIDATE_JOB_EXPERIENCE.FirstOrDefault(d => d.ID == Data.ID);



                if (db.SaveChanges() > 0)
                {
                    if (Data.SKILL_NAME != null || Data.SKILL_NAME != "" || Data.BENEFIT != null || Data.BENEFIT != "")
                    {
                        TB_CANDIDATE_JOB_EXPERIENCE DataJobExp = db.TB_CANDIDATE_JOB_EXPERIENCE.FirstOrDefault(d =>
                        d.CANDIDATE_ID == Data.CANDIDATE_ID &&
                        d.COMPANY_NAME == Data.COMPANY_NAME);

                        string[] Skills = Data.SKILL_NAME.Split(',');
                        string[] Benefits = Data.BENEFIT.Split(',');

                        foreach(var d in db.TB_BENEFIT_JOB_EXPERIENCE.Where(d => Benefits.Contains(d.BENEFIT)))
                        {
                            db.TB_BENEFIT_JOB_EXPERIENCE.Remove(d);
                        }
                        
                        foreach(var d in db.TB_SKILL_JOB_EXPERIENCE.Where(d => Skills.Contains(d.SKILL_NAME)))
                        {
                            db.TB_SKILL_JOB_EXPERIENCE.Remove(d);
                        }

                        db.SaveChanges();

                        if (Skills.Length > 1)
                        {
                            foreach (string skill in Skills)
                            {
                                db.TB_SKILL_JOB_EXPERIENCE.Add(new TB_SKILL_JOB_EXPERIENCE
                                {
                                    ID_JOBEXP = DataJobExp.ID,
                                    SKILL_NAME = skill
                                });
                            }
                        }
                        if (Skills.Length == 0)
                        {
                            db.TB_SKILL_JOB_EXPERIENCE.Add(new TB_SKILL_JOB_EXPERIENCE
                            {
                                ID_JOBEXP = DataJobExp.ID,
                                SKILL_NAME = DataJobExp.SKILL_NAME
                            });
                        }
                        if (Benefits.Length > 1)
                        {
                            foreach (string benefit in Benefits)
                            {
                                db.TB_BENEFIT_JOB_EXPERIENCE.Add(new TB_BENEFIT_JOB_EXPERIENCE
                                {
                                    JOB_EXP_ID = DataJobExp.ID,
                                    BENEFIT = benefit
                                });
                            }
                        }
                        if (Benefits.Length == 0)
                        {
                            db.TB_BENEFIT_JOB_EXPERIENCE.Add(new TB_BENEFIT_JOB_EXPERIENCE
                            {
                                JOB_EXP_ID = DataJobExp.ID,
                                BENEFIT = DataJobExp.BENEFIT
                            });
                        }
                    }
                    if (db.SaveChanges() > 0)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            return 1;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.DTO;
using FinalProject.Models;
using FinalProject.Controllers;
using FinalProject.Filters;

namespace FinalProject.Utils
{
    public class DataCandidateUtils
    {

        public List<CandidateSelectionHistoryDTO> GetDataSelectionHistory()
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
                        STATE_NAME = db.TB_STATE_CANDIDATE.FirstOrDefault(st => st.ID == sh.CANDIDATE_STATE).STATE_NAME,
                        CANDIDATE = db.TB_CANDIDATE.Select(ca => new CandidateDTO
                        {
                            ID = ca.ID,
                            CANDIDATE_ID = ca.CANDIDATE_ID,
                            CANDIDATE_NAME = ca.CANDIDATE_NAME,
                            CANDIDATE_AGE = ca.CANDIDATE_AGE,
                            CANDIDATE_BIRTH_DATE = ca.CANDIDATE_BIRTH_DATE,
                            CANDIDATE_PLACE_BIRTH = ca.CANDIDATE_PLACE_BIRTH,
                            CANDIDATE_ETNIC = ca.CANDIDATE_ETNIC,
                            CANDIDATE_HOMENUMBER = ca.CANDIDATE_HOMENUMBER,
                            CANDIDATE_PHONENUMBER = ca.CANDIDATE_PHONENUMBER,
                            CANDIDATE_EMAIL = ca.CANDIDATE_EMAIL,
                            CANDIDATE_CITY = ca.CANDIDATE_CITY,
                            CANDIDATE_CITY_NAME = db.TB_CITY.FirstOrDefault(ci => ci.CITY_ID == ca.CANDIDATE_CITY).CITY_NAME,
                            CANDIDATE_PROVINCE = ca.CANDIDATE_PROVINCE,
                            CANDIDATE_PROVINCE_NAME = db.TB_PROVINCE.FirstOrDefault(pr => pr.PROVINCE_ID == ca.CANDIDATE_PROVINCE).PROVINCE_NAME,
                            CANDIDATE_CURRENT_ADDRESS = ca.CANDIDATE_CURRENT_ADDRESS,
                            CANDIDATE_KTP_NUMBER = ca.CANDIDATE_KTP_NUMBER,
                            CANDIDATE_CV = ca.CANDIDATE_CV,
                            CANDIDATE_PHOTO = ca.CANDIDATE_PHOTO,
                            CANDIDATE_LAST_EDUCATON = ca.CANDIDATE_LAST_EDUCATON,
                            CANDDIATE_NPWP_NUMBER = ca.CANDDIATE_NPWP_NUMBER,
                            MARITAL_STATUS_ID = ca.MARITAL_STATUS_ID,
                            MARITAL_STATUS = db.TB_MARITALSTATUS.FirstOrDefault(ma => ma.MARITALSTATUS_ID == ca.MARITAL_STATUS_ID).MARITALSTATUS_NAME,
                            RELIGION_ID = ca.RELIGION_ID,
                            RELIGION = db.TB_RELIGION.FirstOrDefault(re => re.RELIGION_ID == ca.RELIGION_ID).RELIGION_NAME,
                            CANDIDATE_DEGREE = ca.CANDIDATE_DEGREE,
                            CANDIDATE_MAJOR = ca.CANDIDATE_MAJOR,
                            CANDIDATE_GPA = ca.CANDIDATE_GPA
                        }).FirstOrDefault(ca => ca.ID == sh.CANDIDATE_ID),
                        PIC = db.TB_USER.Select(usr => new UserDTO
                        {
                            USER_ID = usr.USER_ID,
                            USERNAME = usr.USERNAME,
                            FULL_NAME = usr.FULL_NAME,
                            EMAIL = usr.EMAIL,
                            ROLE_ID = usr.ROLE_ID,
                            ROLE_NAME = db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == usr.ROLE_ID).ROLE_NAME
                        }).FirstOrDefault(usr => usr.USER_ID == sh.PIC_ID),
                    }
                ).ToList();



                return Data;
            }
        }

        public List<CandidateDTO> GetDataCandidatePerState(int StateId)
        {
            using (DBEntities db = new DBEntities())
            {
                List<CandidateDTO> ListCandidateDTO = db.TB_CANDIDATE.Select(ca => new CandidateDTO
                {
                    ID = ca.ID,
                    CANDIDATE_ID = ca.CANDIDATE_ID,
                    CANDIDATE_NAME = ca.CANDIDATE_NAME,
                    CANDIDATE_AGE = ca.CANDIDATE_AGE,
                    CANDIDATE_BIRTH_DATE = ca.CANDIDATE_BIRTH_DATE,
                    CANDIDATE_PLACE_BIRTH = ca.CANDIDATE_PLACE_BIRTH,
                    CANDIDATE_ETNIC = ca.CANDIDATE_ETNIC,
                    CANDIDATE_HOMENUMBER = ca.CANDIDATE_HOMENUMBER,
                    CANDIDATE_PHONENUMBER = ca.CANDIDATE_PHONENUMBER,
                    CANDIDATE_EMAIL = ca.CANDIDATE_EMAIL,
                    CANDIDATE_CITY = ca.CANDIDATE_CITY,
                    CANDIDATE_CITY_NAME = db.TB_CITY.FirstOrDefault(ci => ci.CITY_ID == ca.CANDIDATE_CITY).CITY_NAME,
                    CANDIDATE_PROVINCE = ca.CANDIDATE_PROVINCE,
                    CANDIDATE_PROVINCE_NAME = db.TB_PROVINCE.FirstOrDefault(pr => pr.PROVINCE_ID == ca.CANDIDATE_PROVINCE).PROVINCE_NAME,
                    CANDIDATE_CURRENT_ADDRESS = ca.CANDIDATE_CURRENT_ADDRESS,
                    CANDIDATE_KTP_NUMBER = ca.CANDIDATE_KTP_NUMBER,
                    CANDIDATE_CV = ca.CANDIDATE_CV,
                    CANDIDATE_PHOTO = ca.CANDIDATE_PHOTO,
                    CANDIDATE_LAST_EDUCATON = ca.CANDIDATE_LAST_EDUCATON,
                    CANDDIATE_NPWP_NUMBER = ca.CANDDIATE_NPWP_NUMBER,
                    MARITAL_STATUS_ID = ca.MARITAL_STATUS_ID,
                    MARITAL_STATUS = db.TB_MARITALSTATUS.FirstOrDefault(ma => ma.MARITALSTATUS_ID == ca.MARITAL_STATUS_ID).MARITALSTATUS_NAME,
                    RELIGION_ID = ca.RELIGION_ID,
                    RELIGION = db.TB_RELIGION.FirstOrDefault(re => re.RELIGION_ID == ca.RELIGION_ID).RELIGION_NAME,
                    CANDIDATE_DEGREE = ca.CANDIDATE_DEGREE,
                    CANDIDATE_MAJOR = ca.CANDIDATE_MAJOR,
                    CANDIDATE_GPA = ca.CANDIDATE_GPA,
                    CANDIDATE_STATE_ID = ca.CANDIDATE_STATE_ID,
                    CANDIDATE_STATE_NAME = db.TB_STATE_CANDIDATE.FirstOrDefault(st => st.ID == ca.CANDIDATE_STATE_ID).STATE_NAME,
                    SOURCE = ca.SOURCE,
                    SOURCING_DATE = ca.SOURCING_DATE
                }).ToList().FindAll(ca => ca.CANDIDATE_STATE_ID == StateId);

                return ListCandidateDTO;
            }
        }
    }
}
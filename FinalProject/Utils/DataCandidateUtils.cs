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

        //for get data entire table selection history
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

        //for get data pic/user where change the state of candidate
        public static List<UserDTO> GetDataPIC()
        {
           using (DBEntities db = new DBEntities())
            {
                return db.TB_USER.Select(usr => new UserDTO
                {
                    USER_ID = usr.USER_ID,
                    USERNAME = usr.USERNAME,
                    EMAIL = usr.EMAIL,
                    FULL_NAME = usr.FULL_NAME,
                    ROLE_ID = usr.ROLE_ID,
                    ROLE_NAME = db.TB_ROLE.FirstOrDefault(r => r.ROLE_ID == usr.ROLE_ID).ROLE_NAME
                }).ToList();
            }
        }

        //for get entire data candidate in table candidate indb
        public static List<CandidateDTO> GetDataCandidate()
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
                    MARITAL_STATUS_ID = ca.MARITAL_STATUS_ID,
                    GENDER_ID = ca.GENDER_ID,
                    RELIGION_ID = ca.RELIGION_ID,
                    CANDIDATE_ETNIC = ca.CANDIDATE_ETNIC,
                    CANDIDATE_HOMENUMBER = ca.CANDIDATE_HOMENUMBER,
                    CANDIDATE_PHONENUMBER = ca.CANDIDATE_PHONENUMBER,
                    CANDIDATE_EMAIL = ca.CANDIDATE_EMAIL,
                    CANDIDATE_CITY = ca.CANDIDATE_CITY,
                    CANDIDATE_PROVINCE = ca.CANDIDATE_PROVINCE,
                    CANDIDATE_CURRENT_ADDRESS = ca.CANDIDATE_CURRENT_ADDRESS,
                    CANDIDATE_KTP_NUMBER = ca.CANDIDATE_KTP_NUMBER,
                    CANDDIATE_NPWP_NUMBER = ca.CANDDIATE_NPWP_NUMBER,
                    CANDIDATE_CV = ca.CANDIDATE_CV,
                    CANDIDATE_PHOTO = ca.CANDIDATE_PHOTO,
                    CANDIDATE_LAST_EDUCATON = ca.CANDIDATE_LAST_EDUCATON,
                    CANDIDATE_GPA = ca.CANDIDATE_GPA,
                    CANDIDATE_MAJOR = ca.CANDIDATE_MAJOR,
                    CANDIDATE_DEGREE = ca.CANDIDATE_DEGREE,
                    CANDIDATE_STATE_ID = 1,
                    SOURCE = ca.SOURCE,
                    SOURCING_DATE = ca.SOURCING_DATE,
                    ZIP_CODE = ca.ZIP_CODE,
                    PARENT_ADDRESS = ca.PARENT_ADDRESS,
                    RESIDENT_CARD_NUMBER = ca.RESIDENT_CARD_NUMBER,
                    TELEPHONE_NUMBER = ca.TELEPHONE_NUMBER,
                    AVAILABLE_JOIN = ca.AVAILABLE_JOIN,
                    RECOMENDATION = ca.RECOMENDATION,
                    EXPECTED_sALARY = ca.EXPECTED_SALARY,
                    NOTES = ca.NOTES,
                    POSITION = ca.POSITION,
                    EDUCATON_START_DATE = ca.EDUCATION_START_DATE,
                    EDUCATON_END_DATE = ca.EDUCATION_END_DATE,
                }).ToList();
                return ListCandidateDTO;
            }
        }
    }
}
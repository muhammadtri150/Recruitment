using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DTO
{
    public class UserAccessMenuCandidateDTO
    {
        public int ID { get; set; }
        public int ROLE_ID { get; set; }
        public int SUB_MENU_CANDIDATE_ID { get; set; }
        public int ACTION_CANDIDATE_ID { get; set; }
    }
}
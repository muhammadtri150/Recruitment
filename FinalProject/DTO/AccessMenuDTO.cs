using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class AccessMenuDTO
    {

        public int ACCESS_MENU_ID { get; set; }

        [Required]
        public int ROLE_ID { get; set; }

        [Required]
        public int MENU_ID { get; set; }

        //addition property for view
        public string MENU_TITLE { get; set; }
        public string ROLE_NAME { get; set; }
        
    }
}
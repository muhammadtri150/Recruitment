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
        public int ROLE_ID { get; set; }
        public int MENU_ID { get; set; }
        
    }
}
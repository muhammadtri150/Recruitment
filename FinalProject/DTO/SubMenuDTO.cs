using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class SubMenuDTO
    {
        [Required]
        public int MENU_ID { get; set; }
        [Required]
        public int SUB_MENU_ID { get; set; }
        [Required]
        public string TITLE_SUBMENU { get; set; }


        public string LOGO_SUBMENU { get; set; }
        public string URL { get; set; }


        //---------------- addition --------------
        public string TITLE_MENU { get; set; }

    }
}
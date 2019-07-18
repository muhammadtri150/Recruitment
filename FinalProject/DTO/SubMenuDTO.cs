using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class SubMenuDTO
    {
        public int MENU_ID { get; set; }
        public int SUB_MENU_ID { get; set; }
        public string TITLE_SUBMENU { get; set; }
        public string LOGO_SUBMENU { get; set; }
        public string URL { get; set; }

    }
}
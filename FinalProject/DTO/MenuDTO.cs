using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class MenuDTO
    {
        public int MENU_ID { get; set; }
        public string TITLE_MENU { get; set; }
        public string LOGO_MENU { get; set; }
    }
}
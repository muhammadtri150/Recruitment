using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DTO
{
    public class DashboardDTO
    {
        public int mostqty;
        public string moststate;
        public string mostposition;

       
    }

    public class Ratio
    {
        public int Jobstreet { get; set; }
        public int JobsID { get; set; }
        public int JobsDB { get; set; }
        public int Joblike { get; set; }
        public int TopKarir { get; set; }
        public int KarirPad { get; set; }
        public int Karir2 { get; set; }
    }

    public class Total
    {
        public int PreJava { get; set; }
        public int PrePHP { get; set; }
        public int PreRuby { get; set; }
        public int PreVB { get; set; }
        public int CallJava { get; set; }
        public int CallPHP { get; set; }
        public int CallRuby { get; set; }
        public int CallVB { get; set; }

    }
    
    
}
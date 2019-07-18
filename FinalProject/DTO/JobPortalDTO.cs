using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class JobPortalDTO
    {
        public int JOB_ID { get; set; }

        [Required]
        public string JOBPORTAL_NAME { get; set; }


        public Nullable<System.DateTime> JOBPORTAL_ADDED { get; set; }
    }
}
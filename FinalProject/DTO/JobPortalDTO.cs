using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class JobPortalDTO
    {
        public int JOB_ID { get; set; }

        [Required]
        public string JOBPORTAL_NAME { get; set; }


        public Nullable<System.DateTime> JOBPORTAL_ADDED { get; set; }
    }

    public class Manage_JobPortalDTO
    {
        public static List<JobPortalDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.JOB_PORTAL.Select(d => new JobPortalDTO
                {
                    JOB_ID = d.JOB_ID,
                    JOBPORTAL_NAME = d.JOBPORTAL_NAME,
                    JOBPORTAL_ADDED = d.JOBPORTAL_ADDED
                }).ToList();
            }
        }
    }
}
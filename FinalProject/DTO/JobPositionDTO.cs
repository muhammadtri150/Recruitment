using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class JobPositionDTO
    {
        public int JOBPOSITION_ID { get; set; }

        [Required]
        public string JOBPOSITION_NAME { get; set; }


        public string JOBPOSITION_NOTE { get; set; }
    }

    //manage data --------------------------------------------------------------------------
    public class Manage_JobPositionDTO{
    
        public static List<JobPositionDTO> GetData()
        {
           using(DBEntities db = new DBEntities())
            {
                return db.TB_JOB_POSITION.Select(j => new JobPositionDTO
                {
                   JOBPOSITION_ID = j.JOBPOSITION_ID,
                   JOBPOSITION_NAME = j.JOBPOSITION_NAME,
                   JOBPOSITION_NOTE = j.JOBPOSITION_NOTE
                }).ToList();
            }
        }

    }
}
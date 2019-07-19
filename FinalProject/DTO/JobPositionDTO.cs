using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class JobPositionDTO
    {
        public int JOBPOSITION_ID { get; set; }

        [Required]
        public string JOBPOSITION_NAME { get; set; }


        public string JOBPOSITION_NOTE { get; set; }
    }
}
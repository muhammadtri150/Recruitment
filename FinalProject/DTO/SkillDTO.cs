using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class SkillDTO
    {
        public int SKILL_ID { get; set; }

        [Required]
        public string SKILL_NAME { get; set; }
    }
}
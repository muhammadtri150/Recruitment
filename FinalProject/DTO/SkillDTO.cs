using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class SkillDTO
    {
        public int SKILL_ID { get; set; }

        [Required]
        public string SKILL_NAME { get; set; }
    }

    public class Manage_SkillDTO
    {
        public static List<SkillDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_SKILL.Select(d => new SkillDTO
                {
                    SKILL_ID = d.SKILL_ID,
                    SKILL_NAME = d.SKILL_NAME
                }).ToList();
            }
        }
    }
}
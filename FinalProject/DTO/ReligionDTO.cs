using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class ReligionDTO
    {
        public int RELIGION_ID { get; set; }
        public string RELIGION_NAME { get; set; }
    }

    public class Manage_ReligionDTO
    {
        public static List<ReligionDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_RELIGION.Select(d => new ReligionDTO {
                    RELIGION_ID = d.RELIGION_ID,
                    RELIGION_NAME = d.RELIGION_NAME
                }).ToList();
            } 
        }
    }
}
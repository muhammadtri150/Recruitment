using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class MaritalStatusDTO
    {
        public int MARITALSTATUS_ID { get; set; }
        public string MARITALSTATUS_NAME { get; set; }
    }

    public class Manage_MaritalStatusDTO
    {
        public static List<MaritalStatusDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_MARITALSTATUS.Select(d => new MaritalStatusDTO
                {
                    MARITALSTATUS_ID = d.MARITALSTATUS_ID,
                    MARITALSTATUS_NAME = d.MARITALSTATUS_NAME
                }).ToList();
            }
        }
    }
}
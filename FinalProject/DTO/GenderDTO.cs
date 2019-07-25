using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class GenderDTO
    {
        public int GENDER_ID { get; set; }
        public string GENDER_NAME { get; set; }
    }

    public class Manage_GenderDTO
    {
        public static List<GenderDTO> GetData()
        {
           using(DBEntities db = new DBEntities())
            {
                return db.TB_GENDER.Select(d => new GenderDTO
                {
                    GENDER_ID = d.GENDER_ID,
                    GENDER_NAME = d.GENDER_NAME
                }).ToList();
            }
        }
    }

}
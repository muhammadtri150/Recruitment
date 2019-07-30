using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class ClientDTO
    {
        [Required]
        public int ID { get; set; }
        public string CLIENT_ID { get; set; }

        [Required]
        public string CLIENT_NAME { get; set; }

        [Required]
        public string CLIENT_ADDRESS { get; set; }

        public string CLIENT_OTHERADDRESS { get; set; }

        [Required]
        public string CLIENT_INDUSTRIES { get; set; }
    }

    public class Manage_ClientDTO
    {
        public static List<ClientDTO> GetData()
        {
            using(DBEntities db = new DBEntities())
            {
                return db.TB_CLIENT.Select(d => new ClientDTO
                {
                    ID = d.ID,
                    CLIENT_ID = d.CLIENT_ID,
                    CLIENT_NAME = d.CLIENT_NAME,
                    CLIENT_ADDRESS = d.CLIENT_ADDRESS,
                    CLIENT_OTHERADDRESS = d.CLIENT_OTHERADDRESS,
                    CLIENT_INDUSTRIES = d.CLIENT_INDUSTRIES
                }).ToList();
            }
        }
    }

}
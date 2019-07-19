using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
}
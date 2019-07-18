using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.DTO
{
    public class ClientDTO
    {
        public int ID { get; set; }
        public string CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CLIENT_ADDRESS { get; set; }
        public string CLIENT_OTHERADDRESS { get; set; }
        public string CLIENT_INDUSTRIES { get; set; }
    }
}
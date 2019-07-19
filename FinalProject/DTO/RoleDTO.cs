using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FinalProject.Models;

namespace FinalProject.DTO
{
    public class RoleDTO
    {
        public int ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
    }
}
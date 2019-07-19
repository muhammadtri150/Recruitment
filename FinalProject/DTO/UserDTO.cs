using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class UserDTO
    {
        public int USER_ID { get; set; }

        [RegularExpression(".{5,30}")]
        [Required(ErrorMessage = "Username Must Be fill")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Password Must Be fill")]
        public string PASSWORD { get; set; }

        [Compare("PASSWORD")]
        [Required(ErrorMessage = "Password Must Be fill")]
        public string CONFIRM_PASSWORD { get; set; }

        public string EMAIL { get; set; }

        [Required]
        [RegularExpression(@"([a-z]|\W){5,30}",ErrorMessage = "only letters")]
        public string FULL_NAME { get; set; }

        [Required]
        public int ROLE_ID { get; set; }
        
        public string ROLE_NAME { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DTO
{
    public class UserDTO
    {
        
        [Required(ErrorMessage = "Role is required")]
        public int USER_ID { get; set; }

        [RegularExpression(".{5,30}")]
        [Required(ErrorMessage = "Username Must Be fill")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Password Must Be fill")]
        public string PASSWORD { get; set; }

        [Compare("PASSWORD")]
        [Required(ErrorMessage = "Password Must Be fill")]
        public string CONFIRM_PASSWORD { get; set; }

        [Required()]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required]
        [RegularExpression(@"/\[a-z]{1,30}/i")]
        public string FULL_NAME { get; set; }

        public int ROLE_ID { get; set; }

        [Required]
        public string ROLE_NAME { get; set; }
    }
}
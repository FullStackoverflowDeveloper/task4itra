using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppFour.Models
{
    public class AppUser : IdentityUser
    {
        public string RegistrationDate { get; set; }
        public string LatestLoginDate { get; set; }
        public bool Status { get; set; }
        public bool Check { get; set; }
    }
}

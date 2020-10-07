using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, Display(Name = "First Name")]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        [MaxLength(40)]
        public string LastName { get; set; }

        [MinimumAge(18, ErrorMessage = "You must be 18 years old.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }
    }
}

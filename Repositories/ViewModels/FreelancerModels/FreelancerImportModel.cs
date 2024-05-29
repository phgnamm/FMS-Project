using Repositories.Enums;
using Repositories.ViewModels.AccountModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.FreelancerModels
{
    public class FreelancerImportModel
    {
        [Required(ErrorMessage = "First Name is required!")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last Name is required!")]
        [Display(Name = "LastName")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required!")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid Gender")]
        public Gender Gender { get; set; } 
      
        [Required(ErrorMessage = "Freelancer code is required!")]
        [Display(Name = "FreelancerCode")]
        [RegularExpression(@"^FR\d{5}$", ErrorMessage = "Freelancer code must be in the format 'FRXXXXX' where XXXXX is 5 digits.")]
        public string? Code { get; set; }
    
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Skill is required!")]
        [Display(Name = "Skill")]
        public required string Skill { get; set; }

    }
}

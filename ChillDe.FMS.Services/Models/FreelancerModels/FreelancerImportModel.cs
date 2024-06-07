using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.ViewModels.FreelancerModels
{
    public class FreelancerImportModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(50, ErrorMessage = "FirstName must be no more than 50 characters")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(50, ErrorMessage = "LastName must be no more than 50 characters")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Invalid Gender")]
        public Gender Gender { get; set; } 
      
        [Required(ErrorMessage = "Freelancer code is required!")]
        public string? Code { get; set; }
    
        public string? Address { get; set; }
        public float Wallet { get; set; } = 0;

        [Required(ErrorMessage = "Skills are required!")]
        public List<SkillInputModel> Skills { get; set; } = new List<SkillInputModel>();
    }

    public class SkillInputModel
    {
        [Required(ErrorMessage = "SkillType is required")]
        public string SkillType { get; set; } = "";

        [Required(ErrorMessage = "SkillName is required")]
        public List<string> SkillNames { get; set; } = new List<string>();

    }
}

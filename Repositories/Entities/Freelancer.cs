using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Freelancer:BaseEntity
    {
        public string? Code { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Dob {  get; set; }
        public string? Address { get; set; }
        public float? Wallet { get; set; }
        public string? Image { get; set; } = "";
        public string? Status { get; set; }

        //Relationship
        public virtual ICollection<FreelancerSkill> FreelancerSkills { get; set; } = new List<FreelancerSkill>();
        public virtual ICollection<ProjectApply> ProjectApplies { get; set; } = new List<ProjectApply>();
    }
}

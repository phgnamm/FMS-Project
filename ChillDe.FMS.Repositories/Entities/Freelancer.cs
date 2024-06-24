using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.Entities
{
    public class Freelancer : BaseEntity
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public float Wallet { get; set; }
        public string? Image { get; set; }
        public FreelancerStatus Status { get; set; }
        public int? Warning { get; set; }
        
        // Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Relationship
        public virtual ICollection<FreelancerSkill> FreelancerSkills { get; set; } = new List<FreelancerSkill>();
        public virtual ICollection<ProjectApply> ProjectApplies { get; set; } = new List<ProjectApply>();
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
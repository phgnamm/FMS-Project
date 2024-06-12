using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Services.ViewModels.FreelancerModels
{
    public class FreelancerDetailModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double Wallet { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
        public List<SkillSet> Skills { get; set; }
    }

    public class SkillSet
    {
        public string SkillType { get; set; }
        public List<string> SkillNames { get; set; }
    }
}



using System.Text.Json.Serialization;

namespace ChillDe.FMS.Repositories.Entities
{
    public class FreelancerSkill : BaseEntity
    {
        public Guid? FreelancerId { get; set; }
        public Guid? SkillId { get; set; }

        // Relationship
        [JsonIgnore]
        public virtual Freelancer? Freelancer { get; set; }
        [JsonIgnore]
        public virtual Skill? Skill { get; set; }
    }
}
namespace ChillDe.FMS.Repositories.Entities
{
    public class FreelancerSkill : BaseEntity
    {
        public Guid? FreelancerId { get; set; }
        public Guid? SkillId { get; set; }

        // Relationship
        public virtual Freelancer? Freelancer { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}
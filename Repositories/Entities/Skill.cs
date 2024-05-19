namespace Repositories.Entities
{
    public class Skill : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }

        //Relationship
        public virtual ICollection<FreelancerSkill> FreelancerSkills { get; set; } = new List<FreelancerSkill>();
    }
}
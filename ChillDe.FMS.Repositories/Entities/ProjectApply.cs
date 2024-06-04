namespace ChillDe.FMS.Repositories.Entities
{
    public class ProjectApply : BaseEntity
    {
        // Foreign Key 
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }

        // Relationship
        public virtual Project? Project { get; set; }
        public virtual Freelancer? Freelancer { get; set; }

        public virtual ICollection<DeliverableProduct> DeliverableProducts { get; set; } =
            new List<DeliverableProduct>();
    }
}
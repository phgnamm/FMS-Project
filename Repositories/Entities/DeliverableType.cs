namespace Repositories.Entities
{
    public class DeliverableType : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Relationship
        public virtual ICollection<ProjectDeliverable> ProjectDeliverable { get; set; } =
            new List<ProjectDeliverable>();
    }
}
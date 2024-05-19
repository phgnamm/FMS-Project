namespace Repositories.Entities
{
    public class ProjectDeliverable : BaseEntity
    {
        public string? Name { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string? Status { get; set; }

        // ForeignKey Key
        public Guid? ProjectId { get; set; }
        public Guid? DeliverableTypeId { get; set; }

        // Relationship
        public Project? Project { get; set; }
        public DeliverableType? DeliverableType { get; set; }
        public ICollection<DeliverableProduct> DeliverableProducts { get; set; } = new List<DeliverableProduct>();
    }
}
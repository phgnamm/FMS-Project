using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.Entities
{
    public class DeliverableProduct : BaseEntity
    {
        public string? Name { get; set; }
        public string? URL { get; set; }
        public DeliverableProductStatus? Status { get; set; }
        public string? Feedback { get; set; }

        // Foreign Key
        public Guid? ProjectApplyId { get; set; }
        public Guid? ProjectDeliverableId { get; set; }

        // Relationship
        public ProjectApply? ProjectApply { get; set; }
        public ProjectDeliverable? ProjectDeliverable { get; set; }
    }
}
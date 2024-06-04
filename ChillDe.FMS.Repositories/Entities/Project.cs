namespace ChillDe.FMS.Repositories.Entities
{
    public class Project : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } // Day
        public float? Price { get; set; }
        public string? Status { get; set; }

        // Foreign key
        public Guid? AccountId { get; set; }
        public Guid? ProjectCategoryId { get; set; }

        // Relationship
        public virtual Account? Account { get; set; }
        public virtual ProjectCategory? ProjectCategory { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<ProjectApply> ProjectApplies { get; set; } = new List<ProjectApply>();

        public virtual ICollection<ProjectDeliverable> ProjectDeliverables { get; set; } =
            new List<ProjectDeliverable>();
    }
}
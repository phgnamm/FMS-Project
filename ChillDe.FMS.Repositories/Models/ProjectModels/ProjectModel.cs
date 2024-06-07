
using ChillDe.FMS.Repositories.Entities;

namespace ChillDe.FMS.Repositories.Models.ProjectModels
{
    public class ProjectModel : BaseEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } // Day
        public float? Price { get; set; }
        public string? Status { get; set; }

        public Guid? AccountId { get; set; }
        public string AccountName {  get; set; }
        public Guid? ProjectCategoryId { get; set; }
        public string ProjectCategoryName { get; set; }
    }
}

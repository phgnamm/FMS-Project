
using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels.ProjectDeliverableModels
{
    public class ProjectDeliverableCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "SubmissionDate is required")]
        public DateTime? SubmissionDate { get; set; }
        public string? Status { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Deliverable type is required")]
        public Guid DeliverableTypeId { get; set; }
    }
}

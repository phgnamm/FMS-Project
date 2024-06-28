using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectDeliverableModel
{
    public class ProjectDeliverableCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        // [Required(ErrorMessage = "SubmissionDate is required")]
        public DateTime? SubmissionDate { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "Deliverable type is required")]
        public Guid DeliverableTypeId { get; set; }

    }
}

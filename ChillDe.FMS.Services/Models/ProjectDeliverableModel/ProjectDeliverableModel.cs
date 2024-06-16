using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectDeliverableModel
{
    public class ProjectDeliverableModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public ProjectDeliverableStatus? Status { get; set; }
        public Guid? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public Guid? DeliverableTypeId { get; set; }
        public string? DeliverableTypeName { get; set; }
    }
}

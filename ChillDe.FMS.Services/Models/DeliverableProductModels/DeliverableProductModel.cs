using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.DeliverableProductModels
{
    public class DeliverableProductModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public DeliverableProductStatus? Status { get; set; }
        public string? Feedback { get; set; }
        public Guid? ProjectApplyId { get; set; }
        public Guid? ProjectDeliverableId { get; set; }
        public string? ProjectDeliverableName { get; set; }
    }
}

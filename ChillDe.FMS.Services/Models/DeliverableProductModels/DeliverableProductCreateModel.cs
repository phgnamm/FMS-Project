using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.DeliverableProductModels
{
    public class DeliverableProductCreateModel
    {
        [Required(ErrorMessage = "Product's name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Project's URL is required")]
        public string? URL { get; set; }
        public Guid? ProjectApplyId { get; set; }
        public Guid? ProjectDeliverableId { get; set; }
    }
}

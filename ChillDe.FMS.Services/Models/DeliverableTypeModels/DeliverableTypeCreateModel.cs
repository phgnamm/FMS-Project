using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.DeliverableTypeModels
{
    public class DeliverableTypeCreateModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(128, MinimumLength = 2, ErrorMessage = "Invalid type!")]
        [RegularExpression(@"^\.[a-zA-Z0-9]*$", ErrorMessage = "Invalid type!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectApplyModels
{
    public class ProjectApplyCreateModel
    {
        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectId { get; set; }
        [Required(ErrorMessage = "Freelancer is required")]
        public Guid FreelancerId { get; set; }
    }
}

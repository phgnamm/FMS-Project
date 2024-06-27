using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.TransactionModels
{
    public class TransactionCreateModel
    {
        [Required(ErrorMessage = "Freelancer is required")]
        public Guid FreelancerId { get; set; }
        [Required(ErrorMessage = "Project is required")]
        public Guid ProjectApplyId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.TransactionModels
{
    public class TransactionDetailModel
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public float? Price { get; set; }
        public string FreelancerFirstName { get; set; }
        public string FreelancerLastName { get; set; }
        public string? ProjectName { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set; }
     


    }
}

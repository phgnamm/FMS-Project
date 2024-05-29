using Newtonsoft.Json;
using Repositories.ViewModels.FreelancerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.ResponseModels
{
    public class FreelancerImportResponseModel
    {
        public List<FreelancerImportModel>? DuplicatedFreelancer { get; set; }
        public List<FreelancerImportModel>? AddedFreelancer { get; set; }

        public string? Message { get; set; }
        public bool Status { get; set; } = true;
    }
}

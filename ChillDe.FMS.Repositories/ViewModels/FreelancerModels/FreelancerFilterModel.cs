using ChillDe.FMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.ViewModels.FreelancerModels
{
    public class FreelancerFilterModel
    {
        public string Sort { get; set; } = "creationdate";
        public string SortDirection { get; set; } = "desc";
        public  string? SkillName { get; set; }
        public  string? SkillType { get; set; }
        public FreelancerStatus? Status { get; set; }
        public Gender? Gender { get; set; }
        public string? Search { get; set; }
    }
}

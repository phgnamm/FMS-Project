using Repositories.Entities;
using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.FreelancerModels
{
    public class FreelancerFilterModel
    {
        public string Sort { get; set; } = "creationdate";
        public string SortDirection { get; set; } = "desc";
        public required string SkillName { get; set; }
        public required string SkillType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Gender? Gender { get; set; }
        public string? Search { get; set; }
    }
}

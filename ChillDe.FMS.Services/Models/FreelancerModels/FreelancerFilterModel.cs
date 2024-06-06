using ChillDe.FMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Common;

namespace ChillDe.FMS.Repositories.ViewModels.FreelancerModels
{
    public class FreelancerFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public  string? SkillName { get; set; }
        public  string? SkillType { get; set; }
        public FreelancerStatus? Status { get; set; }
        public Gender? Gender { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.ACCOUNT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.ACCOUNT_MAX_PAGE_SIZE;
    }
}

using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectApplyModels
{
    public class ProjectApplyFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set; }
        public string? SkillName { get; set; }
        public string? SkillType { get; set; }
        public Gender? Gender { get; set; }
        public ProjectApplyStatus? Status { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;

    }
}

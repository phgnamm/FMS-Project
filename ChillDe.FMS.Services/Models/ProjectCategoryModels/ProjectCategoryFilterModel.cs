using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectCategoryModels
{
    public class ProjectCategoryFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        
        public string? Name { get; set; }
        
        public bool IsDeleted { get; set; } = false;
      
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.PROJECCT_CATEGORY_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.PROJECCT_CATEGORY_MAX_PAGE_SIZE;
    }
}

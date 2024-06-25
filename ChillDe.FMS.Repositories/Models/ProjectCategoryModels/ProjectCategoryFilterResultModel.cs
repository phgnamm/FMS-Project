
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.ProjectCategoryModels
{
    public class ProjectCategoryFilterResultModel
    {
        
        public Guid Id { get; set; }
        public string? Description { get; set; }

        public string? Name { get; set; }
        public bool? IsDeleted { get; set; } = false;
        
        public string? Search { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

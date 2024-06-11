using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.ProjectModels
{
    public class ProjectFilterModel
    {
        public string Sort { get; set; } = "creationdate";
        public string SortDirection { get; set; } = "desc";
        public string? Code { get; set; }
        public string? Name { get; set; }
        public ProjectStatus? Status { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Search { get; set; }
    }
}

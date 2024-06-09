using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.ProjectModels
{
    public class ProjectDetailModel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } // Day
        public float? Price { get; set; }
        public string? Status { get; set; }
        public Guid? AccountId { get; set; }
        public string? AccountName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}

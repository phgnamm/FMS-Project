using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectModels
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } // Day
        public float? Price { get; set; }
        public ProjectStatus? Status { get; set; }
        public ProjectVisibility? Visibility { get; set; }
        public Guid? AccountId { get; set; }
        public string? AccountEmail { get; set; }
        public string? AccountFirstName { get; set; }
        public string? AccountLastName { get; set; }
        public Guid? ProjectCategoryId { get; set; }
        public string? ProjectCategoryName { get; set; }

    }
}

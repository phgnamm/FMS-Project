﻿using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectModels
{
    public class ProjectCreateModel
    {
        [Required(ErrorMessage = "Project's code is required")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Project's name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Project's description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Project's duration is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid duration!")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Project's price is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid price!")]
        public float? Price { get; set; }

        [Required(ErrorMessage = "Visibility is required")]
        public ProjectVisibility? Visibility { get; set; }
        //public string AccountCode { get; set; }
        public Guid AccountId { get; set; }
        public Guid ProjectCategoryId { get; set; }

    }
}

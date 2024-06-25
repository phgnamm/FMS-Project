using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectCategoryModels
{
    public class ProjectCategoryUpdateModel
    {
        [RegularExpression("^(Programming & Tech||Programming & Tech||Digital Marketing||Writing & Translation||Video & Animation||Music & Audio||Business||)$",
           ErrorMessage = "Invalid Category Name. Must be: Programming & Tech - Programming & Tech - Digital Marketing - Writing & Translation - Video & Animation - Music & Audio - Business ")]
        public string? Name { get; set; }
        public string? Description { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Models.SkillModels
{
    public class SkillFilterResultModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }

        public string? Name { get; set; }
        //filter
        public string? Type { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public string? Search { get; set; }

        public DateTime CreationDate { get; set; }
    }   
}

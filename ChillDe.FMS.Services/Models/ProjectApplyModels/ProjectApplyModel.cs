using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Services.Models.ProjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectApplyModels
{
    public class ProjectApplyModel
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set; }
        public string? FreelancerFirstName { get; set; }
        public string? FreelancerLastName { get; set; }
        public string? Image {  get; set; }
        public List<SkillGroupModel> Skills { get; set; }
        public ProjectModel? Project { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public ProjectApplyStatus? Status { get; set; }


    }
}

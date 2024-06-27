using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.ProjectApplyModels
{
    public class ProjectApplyUpdateModel
    {
        public Guid Id { get; set; }
        public ProjectApplyStatus? Status { get; set; }
    }
}

using ChillDe.FMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IProjectApplyRepository : IGenericRepository<ProjectApply>
    {
        Task<ProjectApply> GetAcceptedProjectApplyByProjectId(Guid projectId);
        Task<List<ProjectApply>> GetNonAcceptedProjectApplies(Guid projectId, Guid projectApplyId);
    }
}

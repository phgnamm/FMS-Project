using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;

namespace ChillDe.FMS.Services.Interfaces
{
    public interface IProjectApplyService
    {
        //Task<ResponseDataModel<FreelancerDetailModel>> ApplyFreelancer
        //    (Guid freelancerId, Guid projectId);
        Task<ResponseModel> AddProjectApply(ProjectApplyCreateModel projectApplyModel);
        Task<ResponseModel> UpdateProjectApply(ProjectApplyUpdateModel projectApplyUpdateModel);
        Task<ResponseModel> DeleteProjectApply(Guid projectApplyId);
        Task<Pagination<ProjectApplyModel>> GetProjectAppliesByFilter
            (ProjectApplyFilterModel projectApplyFilterModel);
    }
}

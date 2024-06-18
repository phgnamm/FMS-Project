using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectDeliverableModel;
namespace Services.Interfaces
{
    public interface IProjectDeliverableService
    {
        Task<ResponseDataModel<ProjectDeliverableCreateModel>> CreateProjectDeliverable
            (ProjectDeliverableCreateModel projectDeliverableModel);
        Task<Pagination<ProjectDeliverableModel>> GetAllProjectDeliverable
            (ProjectDeliverableFilterModel projectDeliverableFilter);
        Task<ResponseModel> DeleteProjectDeliverable(Guid id);
    }
}

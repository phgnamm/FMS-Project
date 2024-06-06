
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectModels;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<ResponseDataModel<ProjectAddModel>> CreateProject(ProjectAddModel projectModel);
    }
}

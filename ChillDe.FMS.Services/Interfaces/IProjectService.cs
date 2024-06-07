
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectModels;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<ResponseDataModel<ProjectAddModel>> CreateProject(ProjectAddModel projectModel);
        Task<ResponseDataModel<ProjectModel>> GetProject(Guid id);
        Task<ResponseDataModel<ProjectModel>> UpdateProject(Guid id, ProjectAddModel updateProject);
        Task<ResponseDataModel<ProjectModel>> DeleteProject(Guid id);
        Task<Pagination<ProjectModel>> GetAllProjects(ProjectFilterModel projectFilterModel);
    }
}

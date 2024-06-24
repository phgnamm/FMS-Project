using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Services.Interfaces
{
    public interface IProjectCategoryService
    {
        Task<ResponseDataModel<List<ProjectCategoryModel>>> GetProjectCategoriesByNames(List<string> names);
        Task<Pagination<ProjectCategory>> GetProjectCategoriesByFilterAsync(ProjectCategoryFilterModel filterModel);

        Task<ResponseDataModel<ProjectCategory>> UpdateProjectCategoryAsync(Guid id, ProjectCategoryUpdateModel updateModel);
        Task<ResponseModel>BlockProjectCategoryAsync(Guid id);
        Task<ResponseModel> CreateProjectCategoyryAsync(List<ProjectCategoryCreateModel> createModel);

    }
}

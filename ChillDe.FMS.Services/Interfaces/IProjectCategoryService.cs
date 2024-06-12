using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;

namespace Services.Interfaces
{
    public interface IProjectCategoryService
    {
        Task<ResponseDataModel<List<ProjectCategoryModel>>> GetProjectCategoriesByNames(List<string> names);
    }
}

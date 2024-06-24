
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.QueryModels;
using ChillDe.FMS.Repositories.Models.SkillModels;
using Repositories.ViewModels.ProjectCategoryModels;
using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IProjectCategoryRepository : IGenericRepository<ProjectCategory>
    {
        Task<List<ProjectCategory>> GetByNames(List<string> names);
        Task<QueryResultModel<List<ProjectCategoryFilterResultModel>>> GetProCateByFilter(
            Expression<Func<ProjectCategoryFilterResultModel, bool>> filter = null,
            Func<IQueryable<ProjectCategoryFilterResultModel>, IOrderedQueryable<ProjectCategoryFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null);

        Task<ProjectCategory> GetProjectCategoryById(Guid id);
       
    }
}

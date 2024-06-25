using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.QueryModels;
using ChillDe.FMS.Repositories.Models.SkillModels;
using Repositories.ViewModels.ProjectCategoryModels;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        Task<List<Skill>> GetAllByType( string type);
        Task<QueryResultModel<List<SkillFilterResultModel>>> GetSkillByFilter(
            Expression<Func<SkillFilterResultModel, bool>> filter = null,
            Func<IQueryable<SkillFilterResultModel>, IOrderedQueryable<SkillFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null);
        Task<Skill> GetSkillById(Guid id);

    }
}

using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.QueryModels;
using ChillDe.FMS.Repositories.Models.SkillModels;
using Microsoft.EntityFrameworkCore;
using Repositories.ViewModels.ProjectCategoryModels;
using System.Linq;
using System.Linq.Expressions;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly AppDbContext _dbContext;

        public SkillRepository(AppDbContext dbContext, IClaimsService claimsService) : base(dbContext, claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Skill>> GetAllByType(string type)
        {
          return  await _dbContext.Skill.Where(s => s.Type == type).ToListAsync();
        }

        public async     Task<QueryResultModel<List<SkillFilterResultModel>>> GetSkillByFilter(
            Expression<Func<SkillFilterResultModel,
            bool>> filter = null, Func<IQueryable<SkillFilterResultModel>,
            IOrderedQueryable<SkillFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            int totalCount = 0;

            IQueryable<SkillFilterResultModel> query = _dbContext.Skill.Select(s => new SkillFilterResultModel()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Type = s.Type,
                IsDeleted = s.IsDeleted,
                CreationDate = s.CreationDate,
            });

            if (filter != null)
            {
                query = query.Where(filter);
            }
            totalCount = await query.CountAsync();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<SkillFilterResultModel>>()
            {
                TotalCount = totalCount,
                Data = query.ToList(),
            };
        
        }

        public async Task<Skill> GetSkillById(Guid id)
        {
            return await _dbContext.Skill.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}

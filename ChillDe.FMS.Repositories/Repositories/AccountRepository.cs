using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.Models.QueryModels;
using Microsoft.EntityFrameworkCore;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;

        public AccountRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountByCode(string code)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Account> GetAccountById(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<QueryResultModel<List<AccountModel>>> GetAllAsync(
            Expression<Func<AccountModel, bool>> filter = null,
            Func<IQueryable<AccountModel>, IOrderedQueryable<AccountModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            int totalCount = 0;

            IQueryable<AccountModel> query =
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                select new AccountModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Address = user.Address,
                    Image = user.Image,
                    Code = user.Code,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = _dbContext.Roles.FirstOrDefault(r => r.Id == userRole.RoleId).Name,
                    RoleId = userRole.RoleId,
                    IsDeleted = user.IsDeleted,
                    CreationDate = user.CreationDate,
                    // todo add other base entity fields
                };

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
                int validPageSize =
                    pageSize.Value > 0
                        ? pageSize.Value
                        : PaginationConstant.DEFAULT_MIN_PAGE_SIZE;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return new QueryResultModel<List<AccountModel>>()
            {
                TotalCount = totalCount,
                Data = query.ToList(),
            };
        }
    }
}
using System.Linq.Expressions;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
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

        public async Task<QueryResultModel<List<AccountFilterResultModel>>> GetAllAsync(
            Expression<Func<AccountFilterResultModel, bool>> filter = null,
            Func<IQueryable<AccountFilterResultModel>, IOrderedQueryable<AccountFilterResultModel>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            int totalCount = 0;

            IQueryable<AccountFilterResultModel> query = _dbContext.Users
                .Join(
                    _dbContext.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { user, userRole }
                )
                .Join(
                    _dbContext.Roles,
                    userRolePair => userRolePair.userRole.RoleId,
                    role => role.Id,
                    (userRolePair, role) => new AccountFilterResultModel()
                    {
                        Id = userRolePair.user.Id,
                        FirstName = userRolePair.user.FirstName,
                        LastName = userRolePair.user.LastName,
                        Gender = userRolePair.user.Gender,
                        DateOfBirth = userRolePair.user.DateOfBirth,
                        Address = userRolePair.user.Address,
                        Image = userRolePair.user.Image,
                        Code = userRolePair.user.Code,
                        Email = userRolePair.user.Email,
                        PhoneNumber = userRolePair.user.PhoneNumber,
                        Role = role.Name,
                        RoleId = role.Id,
                        IsDeleted = userRolePair.user.IsDeleted,
                        CreationDate = userRolePair.user.CreationDate,
                        // todo add other base entity fields
                    }
                );
            
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

            return new QueryResultModel<List<AccountFilterResultModel>>()
            {
                TotalCount = totalCount,
                Data = query.ToList(),
            };
        }
    }
}
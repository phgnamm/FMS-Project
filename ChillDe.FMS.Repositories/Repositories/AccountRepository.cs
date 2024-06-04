using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChillDe.FMS.Repositories.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<Account> _userManager;

        public AccountRepository(AppDbContext dbContext, UserManager<Account> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<Account> GetAccountByCode(string code)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<List<AccountModel>> GetAccountsByFilter(PaginationParameter paginationParameter,
            AccountFilterModel accountFilterModel)
        {
            var accountList = _dbContext.Users.AsQueryable();

            // Filter
            accountList = accountList.Where(x => x.IsDeleted == accountFilterModel.IsDeleted);

            if (accountFilterModel.Gender != null)
            {
                accountList = accountList.Where(x => x.Gender == accountFilterModel.Gender);
            }

            if (accountFilterModel.Role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(accountFilterModel.Role.ToString());
                accountList = accountList.Where(x => usersInRole.Contains(x));
            }

            // Search
            if (accountFilterModel.Search != null)
            {
                accountList = accountList
                    .Where(x => x.FirstName.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                                x.LastName.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                                x.Code.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                                x.Email.ToLower().Contains(accountFilterModel.Search.ToLower()));
            }

            switch (accountFilterModel.Sort.ToLower())
            {
                case "firstname":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.FirstName)
                        : accountList.OrderByDescending(x => x.FirstName);
                    break;
                case "lastname":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.LastName)
                        : accountList.OrderByDescending(x => x.LastName);
                    break;
                case "code":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.Code)
                        : accountList.OrderByDescending(x => x.Code);
                    break;
                case "dateofbirth":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.DateOfBirth)
                        : accountList.OrderByDescending(x => x.DateOfBirth);
                    break;
                default:
                    accountList = accountList.OrderByDescending(x => x.CreationDate);
                    break;
            }

            var accountModelList = await accountList
                .Select(f => new AccountModel()
                {
                    Id = f.Id,
                    FirstName = f.FirstName,
                    LastName = f.LastName,
                    Gender = f.Gender.ToString(),
                    DateOfBirth = f.DateOfBirth,
                    Address = f.Address,
                    Image = f.Image,
                    Code = f.Code,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    Role = _dbContext.UserRoles
                        .Where(ur => ur.UserId == f.Id)
                        .Join(_dbContext.Roles,
                            ur => ur.RoleId,
                            r => r.Id,
                            (ur, r) => r.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return accountModelList;
        }
    }
}
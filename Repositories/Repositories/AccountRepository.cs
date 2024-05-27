using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Common;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.ViewModels.AccountModels;

namespace Repositories.Repositories
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

        public async Task<List<Account>> GetAccountsByFilter(PaginationParameter paginationParameter,
            AccountFilterModel accountFilterModel)
        {
            var accountList = await _dbContext.Users.ToListAsync();

            // Filter
            accountList = accountList.Where(x => x.IsDeleted == accountFilterModel.IsDeleted).ToList();

            if (accountFilterModel.Gender != null)
            {
                accountList = accountList.Where(x => x.Gender == accountFilterModel.Gender).ToList();
            }

            if (accountFilterModel.Role != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(accountFilterModel.Role.ToString());
                accountList = accountList.Where(x => usersInRole.Contains(x)).ToList();
            }

            // Search
            if (accountFilterModel.Search != null)
            {
                accountList = accountList
                    .Where(x => x.FirstName.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                                x.LastName.ToLower().Contains(accountFilterModel.Search.ToLower())).ToList();
            }

            switch (accountFilterModel.Sort.ToLower())
            {
                case "firstname":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.FirstName).ToList()
                        : accountList.OrderByDescending(x => x.FirstName).ToList();
                    break;
                case "lastname":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.LastName).ToList()
                        : accountList.OrderByDescending(x => x.LastName).ToList();
                    break;
                case "dateofbirth":
                    accountList = (accountFilterModel.SortDirection.ToLower() == "asc")
                        ? accountList.OrderBy(x => x.DateOfBirth).ToList()
                        : accountList.OrderByDescending(x => x.DateOfBirth).ToList();
                    break;
                default:
                    accountList = accountList.OrderByDescending(x => x.CreationDate).ToList();
                    break;
            }

            return accountList;
        }
    }
}
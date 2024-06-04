using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByCode(string code);

        public Task<List<AccountModel>> GetAccountsByFilter(PaginationParameter paginationParameter,
            AccountFilterModel accountFilterModel);
    }
}
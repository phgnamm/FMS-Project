using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Services.Models.TransactionModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Pagination<TransactionDetailModel>> GetTransactionByFilter(TransactionFilterModel transactionFilterModel);
    }
}

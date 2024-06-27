using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChillDe.FMS.Services.Models.TransactionModels
{
    public class TransactionFilterModel: PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set; }
        public bool Status { get; set; }
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.ACCOUNT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.ACCOUNT_MAX_PAGE_SIZE;
    }
}

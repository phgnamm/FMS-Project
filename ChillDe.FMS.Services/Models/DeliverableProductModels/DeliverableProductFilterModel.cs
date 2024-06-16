using ChillDe.FMS.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Models.DeliverableProductModels
{
    public class DeliverableProductFilterModel : PaginationParameter
    {
        public string Order { get; set; } = "creation-date";
        public bool OrderByDescending { get; set; } = true;
        public Guid? ProjectDeliverableId { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}

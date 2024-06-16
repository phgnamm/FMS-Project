using ChillDe.FMS.Repositories.Common;

namespace ChillDe.FMS.Services.Models.DeliverableTypeModels
{
    public class DeliverableTypeFilterModel : PaginationParameter
    {
        public string? Search { get; set; }
        protected override int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        protected override int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
    }
}

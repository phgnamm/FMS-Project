using System.Text.Json.Serialization;

namespace ChillDe.FMS.Repositories.Common
{
    public class PaginationParameter
    {
        public int MinPageSize { get; set; } = PaginationConstant.DEFAULT_MIN_PAGE_SIZE;
        public int MaxPageSize { get; set; } = PaginationConstant.DEFAULT_MAX_PAGE_SIZE;
        public int PageIndex { get; set; } = 1;

        [JsonIgnore]
        public int PageSize
        {
            get { return MinPageSize; }
            set { MinPageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
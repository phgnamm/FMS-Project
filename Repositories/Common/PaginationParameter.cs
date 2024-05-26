using System.Text.Json.Serialization;

namespace Repositories.Common;

public class PaginationParameter
{
    const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;

    [JsonIgnore]
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
    }
}
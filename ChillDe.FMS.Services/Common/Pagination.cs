namespace ChillDe.FMS.Repositories.Common;

public class Pagination<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }

    public Pagination(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }
}
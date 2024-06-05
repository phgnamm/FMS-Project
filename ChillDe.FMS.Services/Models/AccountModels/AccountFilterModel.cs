using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.ViewModels.AccountModels;

public class AccountFilterModel : PaginationParameter
{
    public string Order { get; set; } = "creationdate";
    public bool OrderByDescending { get; set; } = true;
    public Role? Role { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Gender? Gender { get; set; }
    public string? Search { get; set; }
}
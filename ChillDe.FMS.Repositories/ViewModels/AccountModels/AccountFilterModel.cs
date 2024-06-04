using ChillDe.FMS.Repositories.Enums;

namespace ChillDe.FMS.Repositories.ViewModels.AccountModels;

public class AccountFilterModel
{
    public string Sort { get; set; } = "creationdate";
    public string SortDirection { get; set; } = "desc";
    public Role? Role { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Gender? Gender { get; set; }
    public string? Search { get; set; }
}
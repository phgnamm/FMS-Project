using ChillDe.FMS.Repositories.Common;

namespace ChillDe.FMS.Services.Models.SkillModels;

public class SkillFilterModel : PaginationParameter
{
    public string Order { get; set; } = "creation-date";
    public bool OrderByDescending { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string? Search { get; set; }
    // protected override int MinPageSize { get; set; } = PaginationConstant.SKILL_MIN_PAGE_SIZE;
    // protected override int MaxPageSize { get; set; } = PaginationConstant.SKILL_MAX_PAGE_SIZE;
}
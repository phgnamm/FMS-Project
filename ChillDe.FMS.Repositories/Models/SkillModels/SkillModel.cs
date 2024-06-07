using ChillDe.FMS.Repositories.Entities;

namespace ChillDe.FMS.Repositories.Models.SkillModels;

public class SkillModel : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
}
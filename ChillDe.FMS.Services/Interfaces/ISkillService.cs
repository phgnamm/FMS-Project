using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Services.Models.SkillModels;

namespace Services.Interfaces
{
    public interface ISkillService
    {
        Task<Pagination<SkillGroupModel>> GetAllSkillsGroupByType(SkillFilterModel skillFilterModel);
    }
}
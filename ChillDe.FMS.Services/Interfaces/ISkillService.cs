using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.SkillModels;

namespace Services.Interfaces
{
    public interface ISkillService
    {
        Task<Pagination<SkillGroupModel>> GetAllSkillsGroupByType(SkillFilterModel skillFilterModel);
        
        Task<Pagination<SkillModel>> GetAllSkill (SkillFilterModel skillFilterModel);
        Task<ResponseModel> UpdateSkill(Guid id, SkillUpdateModel skillUpdateModel);
        Task<ResponseModel> DeleteSkill(Guid id);   
        Task<ResponseDataModel<SkillModel>> GetSkill(Guid id);   
        Task <ResponseModel> CreateSkill(List<SkillCreateModel> skillCreateModel);
    }
}
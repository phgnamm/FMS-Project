using AutoMapper;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;

namespace ChillDe.FMS.Repositories.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Account
            CreateMap<AccountRegisterModel, Account>();
            CreateMap<Account, AccountModel>();
            CreateMap<AccountFilterResultModel, AccountModel>();

            // Freelancer
            CreateMap<Freelancer, FreelancerModel>();
            CreateMap<Freelancer, FreelancerImportModel>().ReverseMap();
            CreateMap<AccountModel, FreelancerModel>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Skill
            CreateMap<Skill, SkillModel>();
            CreateMap<List<Skill>, List<SkillGroupModel>>()
                .ConvertUsing((skills, skillModels, context) =>
                {
                    return skills
                        .GroupBy(skill => skill.Type)
                        .Select(group => new SkillGroupModel
                        {
                            SkillType = group.Key,
                            SkillNames = group.Select(skill => skill.Name).ToList()
                        })
                        .ToList();
                });
        }
    }
}
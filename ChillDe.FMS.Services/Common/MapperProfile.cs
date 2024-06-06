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

            // Freelancer
            CreateMap<Freelancer, FreelancerModel>();
            CreateMap<Freelancer, FreelancerImportModel>()
                 .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.FreelancerSkills
                     .GroupBy(fs => fs.Skill.Type)
                     .Select(group => new SkillInputModel
                     {
                         SkillType = group.Key,
                         SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                     }).ToList()))
                 .ReverseMap();
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
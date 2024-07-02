using AutoMapper;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Models.AccountModels;
using ChillDe.FMS.Repositories.Models.SkillModels;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;
using ChillDe.FMS.Services.Models.ProjectDeliverableModel;
using ChillDe.FMS.Services.Models.ProjectModels;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.Models.DeliverableTypeModels;
using ChillDe.FMS.Services.Models.SkillModels;
using Repositories.ViewModels.ProjectCategoryModels;
using ProjectCategoryModel = ChillDe.FMS.Services.Models.ProjectCategoryModels.ProjectCategoryModel;


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
            CreateMap<Freelancer, FreelancerDetailModel>()
                   .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                       src.FreelancerSkills
                           .GroupBy(fs => fs.Skill.Type)
                           .Select(group => new SkillSet
                           {
                               SkillType = group.Key,
                               SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                           }).ToList()
                       )).ReverseMap()
                   .ForMember(dest => dest.Code, opt => opt.Ignore()); 
            CreateMap<Freelancer, FreelancerImportModel>()
                  .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.FreelancerSkills
                    .GroupBy(fs => fs.Skill.Type)
                    .Select(group => new SkillInputModel
                    {
                        SkillType = group.Key,
                        SkillNames = group.Select(fs => fs.Skill.Name).ToList()
                    }).ToList()))
                  .ReverseMap()
                  .ForMember(dest => dest.Code, opt => opt.Ignore()); ;

            // Thêm cấu hình cho SkillModel và Skill
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
            CreateMap<SkillModel,SkillFilterResultModel>().ReverseMap();
            CreateMap<Skill, SkillCreateModel>().ReverseMap();  
            CreateMap<Skill, SkillModel>().ReverseMap();  

            //Project
            CreateMap<ProjectCreateModel, Project>();
            CreateMap<Project, ProjectCreateModel>();
            CreateMap<ProjectUpdateModel, Project>();
            CreateMap<Project, ProjectModel>().ReverseMap();

            //ProjectCategory
            CreateMap<ProjectCategoryModel, ProjectCategory>().ReverseMap();
            CreateMap<ProjectCategory,ProjectCategoryFilterResultModel>().ReverseMap();
            CreateMap<ProjectCategoryUpdateModel, ProjectCategory>();
            CreateMap<ProjectCategoryCreateModel, ProjectCategory>();

            //ProjectDeliverable
            CreateMap<ProjectDeliverableCreateModel, ProjectDeliverable>();
            CreateMap<ProjectDeliverable, ProjectDeliverableModel>();

            //DeliverableProduct
            CreateMap<DeliverableProductCreateModel, DeliverableProduct>();

            //DeliverableType
            CreateMap<DeliverableTypeCreateModel, DeliverableType>();
            CreateMap<DeliverableType, DeliverableTypeModel>();

            //ProjectApply
            CreateMap<ProjectApplyCreateModel, ProjectApply>();
        }
    }
}
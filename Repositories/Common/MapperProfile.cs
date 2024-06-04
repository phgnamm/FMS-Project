using AutoMapper;
using Repositories.Entities;
using Repositories.ViewModels.AccountModels;
using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ProjectDeliverableModels;
using Repositories.ViewModels.ProjectModels;

namespace Repositories.Common
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
			CreateMap<Freelancer, FreelancerImportModel>().ReverseMap();
            CreateMap<AccountModel, FreelancerModel>()
           .ForMember(dest => dest.Role, opt => opt.Ignore());

            //Project
            CreateMap<ProjectAddModel, Project>();

			//ProjectDeliverable
			CreateMap<ProjectDeliverableCreateModel, ProjectDeliverable>();
        }
	}
}

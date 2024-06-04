using AutoMapper;
using ChillDe.FMS.Repositories.Entities;
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
			CreateMap<Freelancer, FreelancerImportModel>().ReverseMap();
            CreateMap<AccountModel, FreelancerModel>()
           .ForMember(dest => dest.Role, opt => opt.Ignore());
        }
	}
}

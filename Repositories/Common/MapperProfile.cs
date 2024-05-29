using AutoMapper;
using Repositories.Entities;
using Repositories.ViewModels.AccountModels;
using Repositories.ViewModels.FreelancerModels;

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
			CreateMap<Freelancer, FreelancerImportModel>().ForMember(dest => dest.Code, opt => opt.Ignore()).ReverseMap();
		}
	}
}

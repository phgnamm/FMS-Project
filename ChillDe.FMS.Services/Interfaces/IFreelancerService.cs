using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.TransactionModels;
using ChillDe.FMS.Services.ViewModels.FreelancerModels;

namespace Services.Interfaces;

public interface IFreelancerService
{
    Task<FreelancerImportResponseModel> AddRangeFreelancer(List<FreelancerImportModel> freelancers);
    Task<ResponseDataModel<FreelancerDetailModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer);
    Task<ResponseDataModel<FreelancerDetailModel>> GetFreelancer(Guid id);
    Task<ResponseDataModel<List<FreelancerModel>>> DeleteFreelancer(List<Guid> freelancerIds);
    Task<Pagination<FreelancerDetailModel>> GetFreelancersByFilter(FreelancerFilterModel freelancerFilterModel);
    Task<ResponseModel> RestoreFreelancer(Guid id);
    

}
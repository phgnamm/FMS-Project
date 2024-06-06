using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;

namespace Services.Interfaces;

public interface IFreelancerService
{
    Task<FreelancerImportResponseModel> AddRangeFreelancer(List<FreelancerImportModel> freelancers);
    Task<ResponseDataModel<FreelancerModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer);
    Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id);
    Task<ResponseDataModel<List<FreelancerModel>>> DeleteFreelancer(List<Guid> freelancerIds);
    Task<Pagination<FreelancerDetailModel>> GetFreelancersByFilter(FreelancerFilterModel freelancerFilterModel);
}
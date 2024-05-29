using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ResponseModels;

namespace Services.Interfaces;

public interface IFreelancerService
{
    Task<FreelancerImportResponseModel> AddRangeFreelancer(List<FreelancerImportModel> freelancers);
    Task<ResponseDataModel<FreelancerModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer);
    Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id);
    Task<ResponseDataModel<List<FreelancerModel>>> DeleteFreelancer(List<Guid> freelancerIds);
}
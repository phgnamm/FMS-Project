using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ResponseModels;

namespace Services.Interfaces;

public interface IFreelancerService
{
    Task<FreelancerImportResponseModel> AddRangeFeelancer(List<FreelancerImportModel> freelancers);
    Task<ResponseDataModel<FreelancerModel>> UpdateFreelancerAsync(Guid id, FreelancerImportModel updateFreelancer);
    Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id);
}
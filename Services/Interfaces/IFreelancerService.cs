using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ResponseModels;

namespace Services.Interfaces;

public interface IFreelancerService
{
    Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id);
}
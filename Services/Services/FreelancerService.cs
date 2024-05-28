using AutoMapper;
using Repositories.Interfaces;
using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ResponseModels;
using Services.Interfaces;

namespace Services.Services;

public class FreelancerService : IFreelancerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FreelancerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDataModel<FreelancerModel>> GetFreelancer(Guid id)
    {
        var user = await _unitOfWork.FreelancerRepository.GetAsync(id);

        if (user == null)
        {
            return new ResponseDataModel<FreelancerModel>()
            {
                Status = false,
                Message = "User not found"
            };
        }

        var userModel = _mapper.Map<FreelancerModel>(user);
			
        return new ResponseDataModel<FreelancerModel>()
        {
            Status = true,
            Message = "Get account successfully",
            Data = userModel
        };
    }
}
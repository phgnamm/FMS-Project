using AutoMapper;
using Repositories.Entities;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}

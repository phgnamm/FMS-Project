using AutoMapper;
using Repositories.Entities;
using Repositories.Enums;
using Repositories.Interfaces;
using Repositories.ViewModels.ProjectDeliverableModels;
using Repositories.ViewModels.ProjectModels;
using Repositories.ViewModels.ResponseModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ProjectDeliverableService : IProjectDeliverableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectDeliverableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDataModel<ProjectDeliverableCreateModel>> CreateProjectDeliverable
            (ProjectDeliverableCreateModel projectDeliverableModel)
        {
            var project = _unitOfWork.ProjectRepository.GetAsync(projectDeliverableModel.ProjectId);
            if (project == null)
            {
                return new ResponseDataModel<ProjectDeliverableCreateModel>()
                {
                    Message = "Project not found",
                    Status = false
                };
            }
            DeliverableType deliverableType = await _unitOfWork.DeliverableTypeRepository.GetAsync(projectDeliverableModel.DeliverableTypeId);
            if (deliverableType == null)
            {
                return new ResponseDataModel<ProjectDeliverableCreateModel>()
                {
                    Message = "Deliverable type not found",
                    Status = false
                };
            }
            ProjectDeliverable projectDeliverable = _mapper.Map<ProjectDeliverable>(projectDeliverableModel);
            projectDeliverable.Status = ProjectStatus.Checking.ToString();
            projectDeliverable.DeliverableTypeId = deliverableType.Id;
            projectDeliverable.ProjectId = projectDeliverableModel.ProjectId;

            await _unitOfWork.ProjectDeliverableRepository.AddAsync(projectDeliverable);
            await _unitOfWork.SaveChangeAsync();
            return new ResponseDataModel<ProjectDeliverableCreateModel>()
            {
                Message = "Create project deliverable successfully!",
                Status = true
            };
        }
    }
}

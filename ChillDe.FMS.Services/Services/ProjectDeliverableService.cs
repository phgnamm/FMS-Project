using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.ProjectDeliverableModel;
using ChillDe.FMS.Services.Models.ProjectModels;
using Services.Interfaces;

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
            projectDeliverable.Status = ProjectDeliverableStatus.Checking;
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

        public async Task<Pagination<ProjectDeliverableModel>> GetAllProjectDeliverable
            (ProjectDeliverableFilterModel projectDeliverableFilter)
        {
            var projectDeliverableList = await _unitOfWork.ProjectDeliverableRepository.GetAllAsync(
            filter: x =>
                (x.IsDeleted != true) &&
                (projectDeliverableFilter.ProjectId == null || x.ProjectId == projectDeliverableFilter.ProjectId),
            orderBy: x =>
            {
                return projectDeliverableFilter.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate);
            },
            pageIndex: projectDeliverableFilter.PageIndex,
            pageSize: projectDeliverableFilter.PageSize,
            includeProperties: "Project,DeliverableType"
            );
            if (projectDeliverableList != null)
            {
                var projectDeliverableDetailList = projectDeliverableList.Data
                    .Select(p => new ProjectDeliverableModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    SubmissionDate = p.SubmissionDate,
                    Status = p.Status,
                    ProjectId = p.ProjectId,
                    ProjectName = p.Project.Name,
                    DeliverableTypeId = p.DeliverableTypeId,
                    DeliverableTypeName = p.DeliverableType.Name
                }).ToList();

                return new Pagination<ProjectDeliverableModel>(projectDeliverableDetailList,
                    projectDeliverableList.TotalCount, projectDeliverableFilter.PageIndex,
                    projectDeliverableFilter.PageSize);
            }
            return null;
        }

        public async Task<ResponseModel> DeleteProjectDeliverable(Guid id)
        {
            var projectDeliverable = await _unitOfWork.ProjectDeliverableRepository.GetAsync(id);
            if (projectDeliverable != null)
            {
                var result = _mapper.Map<ProjectDeliverableModel>(projectDeliverable);
                _unitOfWork.ProjectDeliverableRepository.SoftDelete(projectDeliverable);
                await _unitOfWork.SaveChangeAsync();
                if (result != null)
                {
                    return new ResponseModel()
                    {
                        Status = true,
                        Message = "Delete project deliverable successfully"
                    };
                }
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Delete project deliverable failed"
                };
            }
            return new ResponseModel()
            {
                Status = false,
                Message = "Project deliverable not found"
            };
        }
    }
}

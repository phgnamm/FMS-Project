using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DeliverableProductModels;

namespace ChillDe.FMS.Services.Services
{
    public class DeliverableProductService : IDeliverableProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliverableProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateDeliverableProduct
            (DeliverableProductCreateModel deliverableProductModel)
        {
            // var projectApply = await _unitOfWork.ProjectApplyRepository.GetAsync
            //     ((Guid)deliverableProductModel.ProjectApplyId);
            // if (projectApply == null)
            // {
            //     return new ResponseModel
            //     {
            //         Message = "Freelancer has not applied this project yet.",
            //         Status = false
            //     };
            // }

            var projectDeliverable = await _unitOfWork.ProjectDeliverableRepository.GetAsync
                ((Guid)deliverableProductModel.ProjectDeliverableId);
            if (projectDeliverable == null)
            {
                return new ResponseModel
                {
                    Message = "Project deliverable not found.",
                    Status = false
                };
            }


            DeliverableProduct deliverableProduct = _mapper.Map<DeliverableProduct>(deliverableProductModel);
            deliverableProduct.Status = DeliverableProductStatus.Checking;

            if (deliverableProductModel.ProjectId != null)
            {
                // var project = await _unitOfWork.ProjectRepository.GetAsync((Guid)projectApply.ProjectId);
                var project = await _unitOfWork.ProjectRepository.GetAsync(deliverableProductModel.ProjectId);
                if (project != null)
                {
                    project.Status = ProjectStatus.Checking;
                }

                _unitOfWork.ProjectRepository.Update(project);

                var projectApply = await _unitOfWork.ProjectApplyRepository
                    .GetAcceptedProjectApplyByProjectId(deliverableProductModel.ProjectId);
                deliverableProduct.ProjectApplyId = projectApply.Id;
            }

            await _unitOfWork.DeliverableProductRepository.AddAsync(deliverableProduct);

            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Message = "Submiss product successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> DeleteDeliverableProduct(Guid deliverableProductId)
        {
            var deliverableProduct = await _unitOfWork.DeliverableProductRepository
                .GetAsync(deliverableProductId);

            if (deliverableProduct == null)
            {
                return new ResponseModel
                {
                    Message = "Product not found",
                    Status = false
                };
            }

            _unitOfWork.DeliverableProductRepository.SoftDelete(deliverableProduct);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Message = "Delete product successfully",
                Status = true
            };
        }

        public async Task<ResponseModel> UpdateDeliverableProduct
            (Guid deliverableProductId, DeliverableProductStatus status, string feedback)
        {
            var delivarableProduct = await _unitOfWork.DeliverableProductRepository.GetAsync(deliverableProductId);
            if (delivarableProduct == null)
            {
                return new ResponseModel
                {
                    Message = "Product not found.",
                    Status = false
                };
            }

            delivarableProduct.Status = status;
            delivarableProduct.Feedback = feedback;

            var projectDeliverable = await _unitOfWork.ProjectDeliverableRepository
                .GetAsync((Guid)delivarableProduct.ProjectDeliverableId);
            projectDeliverable.Status = (ProjectDeliverableStatus)status;

            _unitOfWork.DeliverableProductRepository.Update(delivarableProduct);
            _unitOfWork.ProjectDeliverableRepository.Update(projectDeliverable);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseModel
            {
                Message = "Update product successfully",
                Status = true
            };
        }

        public async Task<Pagination<DeliverableProductModel>> GetAllDeliverableProduct
            (DeliverableProductFilterModel deliverableProductFilterModel)
        {
            var deliverableProductList = await _unitOfWork.DeliverableProductRepository.GetAllAsync(
                filter: x =>
                    (x.IsDeleted != true) &&
                    (deliverableProductFilterModel.ProjectDeliverableId == null ||
                     x.ProjectDeliverableId == deliverableProductFilterModel.ProjectDeliverableId) &&
                    (deliverableProductFilterModel.AccountId == null || x.ProjectApply.Project.AccountId ==
                        deliverableProductFilterModel.AccountId) &&
                    (deliverableProductFilterModel.FreelancerId == null ||
                     x.ProjectApply.FreelancerId == deliverableProductFilterModel.FreelancerId),
                orderBy: x =>
                {
                    return deliverableProductFilterModel.OrderByDescending
                        ? x.OrderByDescending(x => x.CreationDate)
                        : x.OrderBy(x => x.CreationDate);
                },
                pageIndex: deliverableProductFilterModel.PageIndex,
                pageSize: deliverableProductFilterModel.PageSize,
                includeProperties: "ProjectDeliverable, ProjectApply.Project"
            );
            if (deliverableProductList != null)
            {
                var deliverableProductDetailList = deliverableProductList.Data
                    .Select(p => new DeliverableProductModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        URL = p.URL,
                        Status = p.Status,
                        ProjectApplyId = p.ProjectApplyId,
                        ProjectDeliverableId = p.ProjectDeliverableId,
                        ProjectDeliverableName = p.ProjectDeliverable.Name,
                        Feedback = p.Feedback,
                        ProjectId = p.ProjectApply.ProjectId,
                        ProjectName = p.ProjectApply.Project.Name
                    }).ToList();

                return new Pagination<DeliverableProductModel>(deliverableProductDetailList,
                    deliverableProductList.TotalCount, deliverableProductFilterModel.PageIndex,
                    deliverableProductFilterModel.PageSize);
            }

            return null;
        }
    }
}
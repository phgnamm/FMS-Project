using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.DeliverableTypeModels;
using ChillDe.FMS.Services.Models.ProjectModels;

namespace ChillDe.FMS.Services.Services
{
    public class DeliverableTypeService : IDeliverableTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliverableTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Pagination<DeliverableTypeModel>> GetAllDeliverableType
            (DeliverableTypeFilterModel deliverableTypeFilterModel)
        {
            var deliverableTypeList = await _unitOfWork.DeliverableTypeRepository.GetAllAsync(
                filter: x =>
                    x.IsDeleted == deliverableTypeFilterModel.IsDeleted &&
                    (string.IsNullOrEmpty(deliverableTypeFilterModel.Search) ||
                     x.Name.ToLower().Contains(deliverableTypeFilterModel.Search.ToLower()) ||
                     x.Description.ToLower().Contains(deliverableTypeFilterModel.Search.ToLower())),
                orderBy: x => x.OrderByDescending(x => x.CreationDate),
            pageIndex: deliverableTypeFilterModel.PageIndex,
            pageSize: deliverableTypeFilterModel.PageSize
        );

            if (deliverableTypeList != null)
            {
                var deliverableTypeDetailList = deliverableTypeList.Data.Select(p => new DeliverableTypeModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

                return new Pagination<DeliverableTypeModel>(deliverableTypeDetailList,
                    deliverableTypeList.TotalCount, deliverableTypeFilterModel.PageIndex,
                    deliverableTypeFilterModel.PageSize);
            }
            return null;
        }

        public async Task<ResponseDataModel<DeliverableType>> CreateDeliverableType
            (DeliverableTypeCreateModel deliverableTypeCreateModel)
        {
            var existed = await _unitOfWork.DeliverableTypeRepository.GetByName(deliverableTypeCreateModel.Name);

            if (existed != null)
            {
                return new ResponseDataModel<DeliverableType>
                {
                    Message = "Deliverable type name already exists",
                    Status = false,
                };
            }
            
            var deliverableType = _mapper.Map<DeliverableType>(deliverableTypeCreateModel);
            await _unitOfWork.DeliverableTypeRepository.AddAsync(deliverableType);
            await _unitOfWork.SaveChangeAsync();

            return new ResponseDataModel<DeliverableType>
            {
                Message = "Create deliverable type successfully",
                Status = true,
                Data = deliverableType
            };
        }

        public async Task<ResponseModel> UpdateDeliverableType(Guid id, DeliverableTypeCreateModel deliverableTypeCreateModel)
        {
            var deliverableType = await _unitOfWork.DeliverableTypeRepository.GetAsync(id);

            if (deliverableType == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Deliverable type does not exist"
                };
            }

            deliverableType.Name = deliverableTypeCreateModel.Name;
            deliverableType.Description = deliverableTypeCreateModel.Description;
            
            _unitOfWork.DeliverableTypeRepository.Update(deliverableType);
            var result = await _unitOfWork.SaveChangeAsync();

            if (result == 0)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot update deliverable type"
                };
            }
            
            return new ResponseModel()
            {
                Status = true,
                Message = "Update deliverable type successfully"
            };
        }

        public async Task<ResponseModel> DeleteDeliverableType(Guid id)
        {
            var deliverableType = await _unitOfWork.DeliverableTypeRepository.GetAsync(id);

            if (deliverableType == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Deliverable type does not exist"
                };
            }

            var ongoingProjectsCount = await _unitOfWork.ProjectRepository.CountProjectByDeliverableType(id);
            
            if (ongoingProjectsCount > 0)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Deliverable type is used, cannot delete"
                };
            }
            
            _unitOfWork.DeliverableTypeRepository.SoftDelete(deliverableType);
            var result = await _unitOfWork.SaveChangeAsync();

            if (result == 0)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Cannot delete deliverable type"
                };
            }
            
            return new ResponseModel()
            {
                Status = true,
                Message = "Delete deliverable type successfully"
            };
        }

        public async Task<ResponseDataModel<DeliverableTypeModel>> GetDeliverableType(Guid id)
        {
            var deliverableType = await _unitOfWork.DeliverableTypeRepository.GetAsync(id);

            if (deliverableType == null)
            {
                return new ResponseDataModel<DeliverableTypeModel>()
                {
                    Status = false,
                    Message = "Deliverable type does not exist"
                };
            }

            var result = _mapper.Map<DeliverableTypeModel>(deliverableType);
            
            return new ResponseDataModel<DeliverableTypeModel>()
            {
                Status = false,
                Message = "Deliverable type does not exist",
                Data = result
            };
        }
    }
}

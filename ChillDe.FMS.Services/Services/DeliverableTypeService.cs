using AutoMapper;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Interfaces;
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
                (string.IsNullOrEmpty(deliverableTypeFilterModel.Search) ||
                 x.Name.ToLower().Contains(deliverableTypeFilterModel.Search.ToLower()) ||
                 x.Description.ToLower().Contains(deliverableTypeFilterModel.Search.ToLower())),

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
    }
}

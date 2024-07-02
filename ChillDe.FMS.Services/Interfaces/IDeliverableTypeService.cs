using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using ChillDe.FMS.Services.Models.DeliverableTypeModels;

namespace ChillDe.FMS.Services.Interfaces
{
    public interface IDeliverableTypeService
    {
        Task<Pagination<DeliverableTypeModel>> GetAllDeliverableType
            (DeliverableTypeFilterModel deliverableTypeFilterModel);

        Task<ResponseDataModel<DeliverableType>> CreateDeliverableType
            (DeliverableTypeCreateModel deliverableTypeCreateModel);

        Task<ResponseModel> UpdateDeliverableType
            (Guid id, DeliverableTypeCreateModel deliverableTypeCreateModel);

        Task<ResponseModel> DeleteDeliverableType(Guid id);
        
        Task<ResponseDataModel<DeliverableTypeModel>> GetDeliverableType(Guid id);
    }
}
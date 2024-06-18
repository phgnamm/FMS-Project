using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.DeliverableTypeModels;

namespace ChillDe.FMS.Services.Interfaces
{
    public interface IDeliverableTypeService
    {
        Task<Pagination<DeliverableTypeModel>> GetAllDeliverableType
            (DeliverableTypeFilterModel deliverableTypeFilterModel);
        Task<ResponseDataModel<DeliverableType>> CreateDeliverableType
            (DeliverableTypeCreateModel deliverableTypeCreateModel);
    }
}

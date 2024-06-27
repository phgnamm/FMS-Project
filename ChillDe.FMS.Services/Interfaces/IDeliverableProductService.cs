using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Services.Interfaces
{
    public interface IDeliverableProductService
    {
        Task<ResponseModel> CreateDeliverableProduct
            (DeliverableProductCreateModel deliverableProductModel);

        Task<ResponseModel> DeleteDeliverableProduct(Guid deliverableProductId);

        Task<ResponseModel> UpdateDeliverableProduct
            (Guid deliverableProductId, DeliverableProductStatus status, string feedback);

        Task<Pagination<DeliverableProductModel>> GetAllDeliverableProduct
            (DeliverableProductFilterModel deliverableProductFilterModel);
    }
}

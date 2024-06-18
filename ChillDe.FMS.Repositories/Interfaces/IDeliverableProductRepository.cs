using ChillDe.FMS.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IDeliverableProductRepository : IGenericRepository<DeliverableProduct>
    {
        Task<DeliverableProduct> GetByProjectApplyId(Guid projectApplyId);
    }
}

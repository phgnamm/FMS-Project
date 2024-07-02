using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;

namespace Repositories.Interfaces
{
    public interface IDeliverableTypeRepository : IGenericRepository<DeliverableType>
    {
        Task<DeliverableType> GetByName(string name);
    }
}

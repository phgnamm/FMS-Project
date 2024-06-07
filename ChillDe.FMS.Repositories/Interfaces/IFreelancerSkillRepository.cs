using ChillDe.FMS.Repositories.Entities;
namespace ChillDe.FMS.Repositories.Interfaces
{
    public interface IFreelancerSkillRepository : IGenericRepository<FreelancerSkill>
    {
        List<FreelancerSkill> GetFreelancerSKill(Guid id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class FreelancerSkill: BaseEntity
    {
        public Guid? FrelancerId { get; set; }
        public Guid? SkillId { get; set; }

        //Relationship
        public virtual Freelancer? Freelancer { get; set; }
        public virtual Skill? Skill { get; set; }
    }
}

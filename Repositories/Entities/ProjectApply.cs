using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ProjectApply: BaseEntity
    {
        //Foreign Key 
        public Guid? ProjectId { get; set; }
        public Guid? FrelancerId { get; set; }
        public string? Status { get; set; }

        //Relationship
        public virtual Project? ApplyProject { get; set; }
        public virtual Freelancer? Freelancer { get; set; }
        public virtual ICollection<DeliverableProduct> DeliverableProducts { get; set;} = new List<DeliverableProduct>();
    }
}

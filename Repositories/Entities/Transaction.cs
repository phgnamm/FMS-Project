using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class Transaction: BaseEntity
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

        //Foreign Key
        public Guid? ProjectId { get; set; }
        public Guid? FreelancerId { get; set;}

        //Relationship
        public virtual Project? Project { get; set; }
        public virtual Freelancer? Freelancer { get; set; }
    }
}

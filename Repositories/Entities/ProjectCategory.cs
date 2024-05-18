using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Entities
{
    public class ProjectCategory: BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        //Relationship
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}

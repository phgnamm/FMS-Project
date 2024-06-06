using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.ResponseModels
{
    public class ProjectAddResponseModel
    {
        public string? Message { get; set; }
        public bool Status { get; set; } = true;
    }
}

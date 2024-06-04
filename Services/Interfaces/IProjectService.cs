using Repositories.ViewModels.ProjectModels;
using Repositories.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<ResponseDataModel<ProjectAddModel>> CreateProject(ProjectAddModel projectModel);
    }
}

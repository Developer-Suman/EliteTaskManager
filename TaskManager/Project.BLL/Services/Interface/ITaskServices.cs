using Project.BLL.DTOs.Task;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface ITaskServices
    {
        Task<Result<NickNameDTOs>> AddNickName(NickNameDTOs nickNameDTOs);
        Task<Result<ProjectDetailsDTOs>> AddProjectDetails(ProjectDetailsDTOs projectDetailsDTOs);
    }
}

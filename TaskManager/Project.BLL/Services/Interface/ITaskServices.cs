using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Pagination;
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
        Task<Result<TaskDetailsDTOs>> AddTaskDetails(TaskDetailsDTOs taskDetailsDTOs);
        Task<Result<PagedResult<AllNickNameDTOs>>> GetAllNickName(PaginationDTOs paginationDTOs, CancellationToken cancellationToken);
        Task<Result<PagedResult<AllProjectDetailsDTOs>>> GetAllProjectDetails(PaginationDTOs paginationDTOs, CancellationToken cancellationToken);
    }
}

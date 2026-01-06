using AutoMapper;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.Task;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.Models.Task;
using Project.DLL.Models.Task.SetUp;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class TaskServices : ITaskServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public TaskServices(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<NickNameDTOs>> AddNickName(NickNameDTOs nickNameDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var nickName = new NickName(
                        newId,
                        nickNameDTOs.name,
                        nickNameDTOs.createdAt
                        );

                    await _unitOfWork.Repository<NickName>().AddAsync(nickName);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new NickNameDTOs(
                         nickName.Name,
                         nickName.CreatedAt

                     );
                    scope.Complete();

                    return Result<NickNameDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding");
                }

            }
        }

        public async Task<Result<ProjectDetailsDTOs>> AddProjectDetails(ProjectDetailsDTOs projectDetailsDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var projectDetails = new ProjectDetails(
                        newId,
                        projectDetailsDTOs.name,
                        projectDetailsDTOs.description,
                        projectDetailsDTOs.createdAt
                        );

                    await _unitOfWork.Repository<ProjectDetails>().AddAsync(projectDetails);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new ProjectDetailsDTOs(
                         projectDetails.Name,
                         projectDetails.Description,
                         projectDetails.CreatedAt

                     );
                    scope.Complete();

                    return Result<ProjectDetailsDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding");
                }

            }
        }

        public async Task<Result<TaskDetailsDTOs>> AddTaskDetails(TaskDetailsDTOs taskDetailsDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var taskDetails = new TaskDetails(
                        newId,
                        taskDetailsDTOs.nickNameId,
                        taskDetailsDTOs.title,
                        taskDetailsDTOs.taskStatus,
                        taskDetailsDTOs.projectDetailsId,
                        taskDetailsDTOs.taskPriority,
                        taskDetailsDTOs.taskReviewed,
                        taskDetailsDTOs.descriptions,
                        taskDetailsDTOs.startDate,
                        taskDetailsDTOs.dueDate,
                        taskDetailsDTOs.doingLink,
                        taskDetailsDTOs.finalLink
                        );

                    await _unitOfWork.Repository<TaskDetails>().AddAsync(taskDetails);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = _mapper.Map<TaskDetailsDTOs>(taskDetails);
                   
                    scope.Complete();

                    return Result<TaskDetailsDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding");
                }

            }
        }

        public async Task<Result<DeleteNickNameDTOs>> DeleteNickName(string NickNameId, CancellationToken cancellationToken)
        {
            try
            {
                var nickName = await _unitOfWork.Repository<NickName>().GetByIdAsync(NickNameId);
                if (nickName is null)
                {
                    return Result<DeleteNickNameDTOs>.Failure("NotFounds", "NickName cannot be Found");

                }

                _unitOfWork.Repository<NickName>().Delete(nickName);
                await _unitOfWork.SaveChangesAsync();
                return Result<DeleteNickNameDTOs>.Success(_mapper.Map<DeleteNickNameDTOs>(nickName));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<AllNickNameDTOs>>> GetAllNickName(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            try
            {
                var nickName = await _unitOfWork.Repository<NickName>().GetAllAsyncWithPagination();
                var nickNamePagedResult = await nickName.AsNoTracking().OrderByDescending(x=>x.CreatedAt).ToPagedResultAsync(paginationDTOs.pageIndex, paginationDTOs.pageSize, paginationDTOs.IsPagination);

                if (nickNamePagedResult.Data.Items is null && nickName.Any())
                {
                    return Result<PagedResult<AllNickNameDTOs>>.Failure("NotFound", "NickName are not Found");

                }

                var nickNameResult = _mapper.Map<PagedResult<AllNickNameDTOs>>(nickNamePagedResult.Data);


                return Result<PagedResult<AllNickNameDTOs>>.Success(nickNameResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching");
            }
        }

        public async Task<Result<PagedResult<AllProjectDetailsDTOs>>> GetAllProjectDetails(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            try
            {
                var projectDetails = await _unitOfWork.Repository<ProjectDetails>().GetAllAsyncWithPagination();
                var projectDetailsPagedResult = await projectDetails.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToPagedResultAsync(paginationDTOs.pageIndex, paginationDTOs.pageSize, paginationDTOs.IsPagination);

                if (projectDetailsPagedResult.Data.Items is null && projectDetails.Any())
                {
                    return Result<PagedResult<AllProjectDetailsDTOs>>.Failure("NotFound", "ProjectDetails are not Found");

                }

                var projectDetailsResult = _mapper.Map<PagedResult<AllProjectDetailsDTOs>>(projectDetailsPagedResult.Data);


                return Result<PagedResult<AllProjectDetailsDTOs>>.Success(projectDetailsResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching");
            }
        }

        public async Task<Result<PagedResult<AllTaskDetailsDTOs>>> GetAllTaskDetails(PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetails = await _unitOfWork.Repository<TaskDetails>().GetAllAsyncWithPagination();
                var taskDetailsPagedResult = await taskDetails.AsNoTracking().ToPagedResultAsync(paginationDTOs.pageIndex, paginationDTOs.pageSize, paginationDTOs.IsPagination);

                if (taskDetailsPagedResult.Data.Items is null && taskDetails.Any())
                {
                    return Result<PagedResult<AllTaskDetailsDTOs>>.Failure("NotFound", "TaskDetails are not Found");

                }

                var taskDetailsResult = _mapper.Map<PagedResult<AllTaskDetailsDTOs>>(taskDetailsPagedResult.Data);


                return Result<PagedResult<AllTaskDetailsDTOs>>.Success(taskDetailsResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching");
            }
        }

        public async Task<Result<NickNameGetByIdDTOs>> NickNameGetById(string NickNameId, CancellationToken cancellationToken)
        {
            try
            {
                var nickNameData = await _unitOfWork.Repository<NickName>().GetByIdAsync(NickNameId);
                if (nickNameData is null)
                {
                    return Result<NickNameGetByIdDTOs>.Failure("NotFound", "NickName are not Found");
                }
                return Result<NickNameGetByIdDTOs>.Success(_mapper.Map<NickNameGetByIdDTOs>(nickNameData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting NickName");
            }
        }

        public async Task<Result<ProjectDetailsGetByIdDTOs>> ProjectDetailsGetById(string ProjectDetailsId, CancellationToken cancellationToken)
        {
            try
            {
                var projectDetailsData = await _unitOfWork.Repository<ProjectDetails>().GetByIdAsync(ProjectDetailsId);
                if (projectDetailsData is null)
                {
                    return Result<ProjectDetailsGetByIdDTOs>.Failure("NotFound", "projectDetailsData are not Found");
                }
                return Result<ProjectDetailsGetByIdDTOs>.Success(_mapper.Map<ProjectDetailsGetByIdDTOs>(projectDetailsData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting NickName");
            }
        }

        public async Task<Result<TaskDetailsGetByIdDTOs>> TaskDetailsGetById(string TaskDetailsId, CancellationToken cancellationToken)
        {
            try
            {
                var taskDetailsData = await _unitOfWork.Repository<TaskDetails>().GetByIdAsync(TaskDetailsId);
                if (taskDetailsData is null)
                {
                    return Result<TaskDetailsGetByIdDTOs>.Failure("NotFound", "taskDetailsData are not Found");
                }
                return Result<TaskDetailsGetByIdDTOs>.Success(_mapper.Map<TaskDetailsGetByIdDTOs>(taskDetailsData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting TaskDetails");
            }
        }

        public async Task<Result<NickNameUpdateDTOs>> UpdateNickName(string NickNameId, NickNameUpdateDTOs nickNameUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var nickNameToBeUpdated = await _unitOfWork.Repository<NickName>().GetByIdAsync(NickNameId);
                    if (nickNameToBeUpdated is null)
                    {
                        return Result<NickNameUpdateDTOs>.Failure("NotFound", "NickName are not Found");
                    }
                    _mapper.Map(nickNameUpdateDTOs, nickNameToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new NickNameUpdateDTOs(
                        nickNameToBeUpdated.Name,
                        nickNameToBeUpdated.CreatedAt

                        );
                    scope.Complete();

                    return Result<NickNameUpdateDTOs>.Success(_mapper.Map<NickNameUpdateDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating");
                }

            }
        }

        public async Task<Result<UpdateProjectDetailsDTOs>> UpdateProjectDetails(string ProjectDetailsId, UpdateProjectDetailsDTOs updateProjectDetailsDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var projectDetailsToBeUpdated = await _unitOfWork.Repository<ProjectDetails>().GetByIdAsync(ProjectDetailsId);
                    if (projectDetailsToBeUpdated is null)
                    {
                        return Result<UpdateProjectDetailsDTOs>.Failure("NotFound", "ProjectDetails are not Found");
                    }
                    _mapper.Map(updateProjectDetailsDTOs, projectDetailsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new UpdateProjectDetailsDTOs(
                        projectDetailsToBeUpdated.Name,
                        projectDetailsToBeUpdated.Description,
                        projectDetailsToBeUpdated.CreatedAt

                        );
                    scope.Complete();

                    return Result<UpdateProjectDetailsDTOs>.Success(_mapper.Map<UpdateProjectDetailsDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating");
                }

            }
        }

        public async Task<Result<UpdateTaskDetailsDTOs>> UpdateTaskDetails(string TaskDetailsId, UpdateTaskDetailsDTOs updateTaskDetailsDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var taskDetailsToBeUpdated = await _unitOfWork.Repository<TaskDetails>().GetByIdAsync(TaskDetailsId);
                    if (taskDetailsToBeUpdated is null)
                    {
                        return Result<UpdateTaskDetailsDTOs>.Failure("NotFound", "ProjectDetails are not Found");
                    }
                    _mapper.Map(updateTaskDetailsDTOs, taskDetailsToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new UpdateTaskDetailsDTOs(
                        taskDetailsToBeUpdated.NickNameId,
                        taskDetailsToBeUpdated.Title,
                        taskDetailsToBeUpdated.TaskStatus,
                        taskDetailsToBeUpdated.ProjectDetailsId,
                        taskDetailsToBeUpdated.TaskPriority,
                        taskDetailsToBeUpdated.TaskReviewed,
                        taskDetailsToBeUpdated.Descriptions,
                        taskDetailsToBeUpdated.StartDate,
                        taskDetailsToBeUpdated.DueDate,
                        taskDetailsToBeUpdated.DoingLink,
                        taskDetailsToBeUpdated.FinalLink

                        );
                    scope.Complete();

                    return Result<UpdateTaskDetailsDTOs>.Success(_mapper.Map<UpdateTaskDetailsDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating");
                }

            }
        }

    }
}

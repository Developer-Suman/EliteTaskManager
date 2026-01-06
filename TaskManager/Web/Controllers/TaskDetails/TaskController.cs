using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.Task;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System.Text.Json;
using Web.Configs;

namespace Web.Controllers.TaskDetails
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("AllowAllOrigins")]
    public class TaskController : WebBaseController
    {
        private readonly ITaskServices _taskServices;

        public TaskController(ITaskServices taskServices, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _taskServices = taskServices;

        }
        #region DeleteNickName
        [HttpDelete("DeleteNickName{Id}")]
        public async Task<IActionResult> DeleteNickName([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var deleteNickNameResult = await _taskServices.DeleteNickName(Id, cancellationToken);

            #region switch
            return deleteNickNameResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteNickNameResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }


        #endregion

        #region UpdateTaskDetails
        [HttpPatch("UpdateTaskDetails{Id}")]
        public async Task<IActionResult> UpdateTaskDetails([FromRoute] string Id, [FromBody] UpdateTaskDetailsDTOs updateTaskDetailsDTOs)
        {
            var updateTaskDetailsResult = await _taskServices.UpdateTaskDetails(Id, updateTaskDetailsDTOs);

            #region switch
            return updateTaskDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateTaskDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateTaskDetailsResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        #endregion

        #region UpdateProjectDetails
        [HttpPatch("UpdateProjectDetails{Id}")]
        public async Task<IActionResult> UpdateProjectDetails([FromRoute] string Id, [FromBody] UpdateProjectDetailsDTOs updateProjectDetailsDTOs)
        {
            var updateProjectDetailsResult = await _taskServices.UpdateProjectDetails(Id, updateProjectDetailsDTOs);

            #region switch
            return updateProjectDetailsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateProjectDetailsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateProjectDetailsResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        #endregion

        #region NickNameUpdate
        [HttpPatch("NickNameUpdate{Id}")]
        public async Task<IActionResult> NickNameUpdate([FromRoute] string Id, [FromBody] NickNameUpdateDTOs nickNameUpdateDTOs)
        {
            var updateNickNameResult = await _taskServices.UpdateNickName(Id, nickNameUpdateDTOs);

            #region switch
            return updateNickNameResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateNickNameResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateNickNameResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        #endregion

        #region TaskDetailsGetById

        [HttpGet("TaskDetailsGetById{Id}")]
        public async Task<IActionResult> TaskDetailsGetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var taskDetailsGetByIdResultData = await _taskServices.TaskDetailsGetById(Id, cancellationToken);

            #region switch
            return taskDetailsGetByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(taskDetailsGetByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(taskDetailsGetByIdResultData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }


        #endregion

        #region ProjectDetailsGetById

        [HttpGet("ProjectDetailsGetById{Id}")]
        public async Task<IActionResult> ProjectDetailsGetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var projectDetailsGetByIdResultData = await _taskServices.ProjectDetailsGetById(Id, cancellationToken);

            #region switch
            return projectDetailsGetByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(projectDetailsGetByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(projectDetailsGetByIdResultData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }


        #endregion

        #region NickNameGetById

        [HttpGet("NickNameGetById{Id}")]
        public async Task<IActionResult> NickNameGetById([FromRoute] string Id, CancellationToken cancellationToken)
        {
            var nickNameGetByIdResultData = await _taskServices.NickNameGetById(Id, cancellationToken);

            #region switch
            return nickNameGetByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(nickNameGetByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(nickNameGetByIdResultData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }


        #endregion

        #region GetAllTaskDetails
        [HttpGet("GetAllTaskDetails")]
        public async Task<IActionResult> GetTaskDetails([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getTaskDetails = await _taskServices.GetAllTaskDetails(paginationDTOs, cancellationToken);


            #region switch
            return getTaskDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getTaskDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getTaskDetails.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        #endregion

        #region GetAllProjectDetails
        [HttpGet("GetAllProjectDetails")]
        public async Task<IActionResult> GetProjectDetails([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getProjectDetails = await _taskServices.GetAllProjectDetails(paginationDTOs, cancellationToken);


            #region switch
            return getProjectDetails switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getProjectDetails.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getProjectDetails.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        #endregion

        #region GetAllNickName
        [HttpGet("GetAllNickName")]
        public async Task<IActionResult> GetNickName([FromQuery] PaginationDTOs paginationDTOs, CancellationToken cancellationToken)
        {
            var getNickName = await _taskServices.GetAllNickName(paginationDTOs, cancellationToken);


            #region switch
            return getNickName switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getNickName.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getNickName.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        #endregion

        #region TaskDetails
        [HttpPost("AddTaskDetails")]
        public async Task<IActionResult> AddTaskDetails([FromBody] TaskDetailsDTOs taskDetailsDTOs)
        {
            var addtaskDetails = await _taskServices.AddTaskDetails(taskDetailsDTOs);
            #region switch
            return addtaskDetails switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddTaskDetails), addtaskDetails.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addtaskDetails.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }
        #endregion

        #region ProjectDetails
        [HttpPost("AddProjectDetails")]
        public async Task<IActionResult> AddProjectDetails([FromBody] ProjectDetailsDTOs projectDetailsDTOs)
        {
            var addProjectDetails = await _taskServices.AddProjectDetails(projectDetailsDTOs);
            #region switch
            return addProjectDetails switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddProjectDetails), addProjectDetails.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addProjectDetails.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }
        #endregion

        #region NickName
        [HttpPost("AddNickName")]
        public async Task<IActionResult> AddNickName([FromBody] NickNameDTOs nickNameDTOs)
        {
            var addNickName = await _taskServices.AddNickName(nickNameDTOs);
            #region switch
            return addNickName switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(AddNickName), addNickName.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(addNickName.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }
        #endregion
    }
}

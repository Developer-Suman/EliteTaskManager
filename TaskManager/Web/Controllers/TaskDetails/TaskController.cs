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
